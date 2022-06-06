using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class content_hr_addleaveapplication : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            pg.Value=function.Decrypt(Request.QueryString["pg"].ToString(), true);
            disp();
        }
    }
    protected void disp()
    {
        string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                      "left join MPosition b on a.PositionId=b.Id where a.PayrollGroupId=" + pg.Value + "";
        DataTable dt = dbhelper.getdata(query);
        ddl_employee.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
      
    }
    protected void click_emp(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select a.id, a.leave,a.leavetype,case when (select leavestatus from memployee where ID=" + ddl_employee.SelectedValue + ")='False' then '0' else a.yearlytotal end yearlytotal,(select leavestatus from memployee where ID=" + ddl_employee.SelectedValue + ") leavestatus, " +
                                        " case when (select leavestatus from memployee where ID=" + ddl_employee.SelectedValue + ")='False' then 0 else case when (a.yearlytotal)-(select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + ddl_employee.SelectedValue + " and withpay='True') is null then a.yearlytotal " + 
                                        "else " +
                                        "(a.yearlytotal)-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + ddl_employee.SelectedValue + " and withpay='True') is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + ddl_employee.SelectedValue + " and withpay='True')  end) " +
                                        "end end " +
                                        "leave_bal from MLeave a ");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_veirfy(object sender, EventArgs e)
    {
       DateTime datef = Convert.ToDateTime(this.txt_from.Text);
       TimeSpan nod = DateTime.Parse(txt_from.Text) - DateTime.Parse(txt_to.Text);
       string nodformat = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0}", nod.Days, nod.Hours, nod.Minutes, nod.Seconds).Replace("-", "");
       DataTable final_disp = new DataTable();
       final_disp.Columns.Add(new DataColumn("date", typeof(string)));
       final_disp.Columns.Add(new DataColumn("noh", typeof(string)));
       final_disp.Columns.Add(new DataColumn("pay", typeof(string)));
       DataRow final_dr;
       string noh = rb_range.Checked == true ? "1" : "0.5";
       DataTable dts = dbhelper.getdata("select " +
                                        "((select yearlytotal from  MLeave where id="+ddl_leave.SelectedValue+") " +
                                        "- " +
                                        "(select case when sum(NumberOfHours)is null then '0' else sum(NumberOfHours) end  from TLeaveApplicationLine where LeaveId=" + ddl_leave.SelectedValue + " and EmployeeId=" + ddl_employee.SelectedValue + " and withpay='True'))current_bal " +
                                        "from Memployee a   " +
                                        "where a.Id="+ddl_employee.SelectedValue+"");
        decimal get_t_noh = 0;
        for (int i = 0; i <= int.Parse(nodformat); i++)
        {
            string[] getval = txt_from.Text.Trim().Split('/');
            string[] f_datef = datef.AddDays(i).ToString().Replace(" 12:00:00 AM", "").Split('/');
            string gggg=dts.Rows[0]["current_bal"].ToString().Contains('-') ? "0" : dts.Rows[0]["current_bal"].ToString();
            decimal hhhh = decimal.Parse(gggg) - get_t_noh;
            string getpay = grid_view.Rows[0].Cells[5].Text=="True"?(hhhh > 0 ? "True" : "False").ToString():"False";

            string month=f_datef[0].Length>1?f_datef[0]:"0"+f_datef[0];
            string day=f_datef[1].Length>1?f_datef[1]:"0"+f_datef[1];

            if (chekingleave(ddl_employee.SelectedValue, ddl_employee.SelectedValue, month + "/" + day + "/" + f_datef[2]))
            {
                final_dr = final_disp.NewRow();
                final_dr["date"] = month + "/" + day + "/" + f_datef[2];
                final_dr["noh"] = noh;
                final_dr["pay"] = getpay;
                final_disp.Rows.Add(final_dr);
                get_t_noh = get_t_noh + decimal.Parse(noh);
            }
        }
        grid_leave.DataSource = final_disp;
        grid_leave.DataBind();
       
        
        
        //foreach (DataRow dr in final_disp.Rows)
        //{

        //    DataTable dt = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + ddl_employee.SelectedValue + "' and left(convert(varchar,Date,101),10)='" + dr["date"] + "'");
        //   // DataTable dts = dbhelper.getdata("select COUNT(*)cnt from TLeaveApplicationLine where LeaveId=" + ddl_leave.SelectedValue + " and  EmployeeId='" + ddl_employee.SelectedValue + "'");
        //    DataTable dts = dbhelper.getdata("select SUM(b.yearlytotal)-SUM(a.NumberOfHours) current_bal from TLeaveApplicationLine a " +
        //                                    "left join MLeave b on a.LeaveId=b.Id " +
        //                                    "where a.LeaveId=" + ddl_leave.SelectedValue + " and a.EmployeeId='" + ddl_employee.SelectedValue + "'");
        //    DataTable dtemp = dbhelper.getdata("select * from memployee where EmployeeId='" + ddl_employee.SelectedValue + "'");
             
        //    DataTable dtrest = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + dtemp.Rows[0]["shiftcodeid"].ToString() + " and RestDay='True'");
        //    DataTable dtholliday = dbhelper.getdata("select * from MDayTypeDay where BranchId=" + dtemp.Rows[0]["BranchId"].ToString() + "");

        //   // string pay = int.Parse(dts.Rows[0]["cnt"].ToString()) > 0 ? "True" : "False";
            
        //    DateTime datel = Convert.ToDateTime(dr["date"]);
        //    string getday = datel.DayOfWeek.ToString();
        //    if (dt.Rows.Count == 0 && getday != dtrest.Rows[0]["day"].ToString() && dr["date"].ToString() != dtrest.Rows[0]["date"].ToString())
        //    {
        //       // string noh=(rb_range.Checked==true?decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()):decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString())/2).ToString();
                
        //         dbhelper.getdata("insert into TLeaveApplicationLine values (" + ddl_employee.SelectedValue + ",'" + ddl_leave.SelectedValue + "','" + dr["date"] + "'," + noh + ",'" + pay + "','" + txt_lineremarks.Text + "','" + "Approved-" + user_id + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,getdate())");
        //    }

        //}


        //DataTable dt = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + ddl_employee.SelectedValue + "' and left(convert(varchar,Date,101),10)='" + txt_from.Text.Trim() + "'");
        //dt = dbhelper.getdata("select COUNT(*)cnt from TLeaveApplicationLine where LeaveId=" + ddl_leave.SelectedValue + " and EmployeeId=" + ddl_employee.SelectedValue + "");
        //string pay = int.Parse(dt.Rows[0]["cnt"].ToString()) > 0 ? "True" : "False";
        //if (dt.Rows.Count == 0)
        //{
        //    //dbhelper.getdata("insert into TLeaveApplicationLine values (" + ddl_employee.SelectedValue + ",'" + ddl_leave.SelectedValue + "','" + txt_ld.Text.Trim() + "'," + txt_noh.Text + ",'" + pay + "','" + txt_lineremarks.Text + "','" + "Approved-" + user_id + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,getdate())");
        //}
        //else
        //    Response.Write("<script>alert('already exist!')</script>");
    }
    protected void click_save(object sender, EventArgs e)
    {
        if (grid_view.Rows.Count > 0)
        {
            foreach (GridViewRow gr in grid_leave.Rows)
            {
                dbhelper.getdata("insert into TLeaveApplicationLine values (" + ddl_employee.SelectedValue + ",'" + ddl_leave.SelectedValue + "','" + gr.Cells[0].Text + "'," + gr.Cells[1].Text + ",'" + gr.Cells[2].Text + "','" + txt_lineremarks.Text + "','" + "Approved-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,getdate())");
            }
            Response.Redirect("addleaveapplication?user_id=" + function.Encrypt(key.Value, true) + "&pg=" + function.Encrypt(pg.Value, true) + "", false);
        }
        else
        {
            Response.Write("<script>alert('No Data Found')</script>");
        }
    }
    protected bool chekingleave( string emp_id,string leave_id,string date_leave)
    {
        bool err = true;
        DataTable dtemp = dbhelper.getdata("select * from memployee where Id='" + emp_id + "'");
        DataTable dt = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "'");
        DataTable dtrest = dbhelper.getdata("select * from MShiftCodeDay where shiftcodeid='" + dtemp.Rows[0]["shiftcodeid"].ToString() + "' and restday='True' and status is null");
        DataTable dthol = dbhelper.getdata("select * from MDayTypeDay where branchid='" + dtemp.Rows[0]["branchid"].ToString() + "' and date='" + date_leave + "' and status is null");

        string shift = Convert.ToDateTime(date_leave).DayOfWeek.ToString();
        if (shift == dtrest.Rows[0]["Day"].ToString())
        {
            err = false;
        }
        else if (dthol.Rows.Count>0)
        {
            err = false;
        }
        return err;
    }

}