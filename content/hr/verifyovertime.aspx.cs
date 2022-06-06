using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_verifyovertime : System.Web.UI.Page
{
    public static string  query, id;
    public DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        query = "select Id,BranchId,DepartmentId,FirstName from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
        dt = dbhelper.getdata(query);

        query = "select a.id, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot,a.OvertimeHours, " +
              "a.OvertimeNightHours,a.remarks,a.status,a.time_in,a.time_out, " +
              "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
              "from TOverTimeLine a " +
              "left join MEmployee b on a.EmployeeId=b.Id " +
              "where a.status like '%Verification%' order by a.date desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;

    }

    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
            query = "select a.id, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot,a.OvertimeHours, " +
            "a.OvertimeNightHours,a.remarks,a.status,a.time_in,a.time_out, " +
            "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
            "from TOverTimeLine a " +
            "left join MEmployee b on a.EmployeeId=b.Id " +
            "where a.status like '%Verification%' " +
            "and left(convert(varchar,a.date,101),10) between '" + txt_f.Text + "' and '" + txt_t.Text + "' " +
            "order by a.date desc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }
    protected void click_add(object sender, EventArgs e)
    {
        Div1.Visible = true;
        pAdd.Visible = true;
        Div2.Visible = false;
    }
    protected bool checker()
    {
        bool oi = true;
        return oi;
    }
    protected void addOT(object sender, EventArgs e)
    {
        if (checker())
        {
            string a = DateTime.Now.ToString("MM/dd/yyyy");
            query = "select top 1 *,CONVERT(varchar,[Date],101) as datee from TChangeShiftLine where EmployeeId='" + Session["emp_id"].ToString() + "' order by id desc ";
            DataTable wtf = new DataTable();
            wtf = dbhelper.getdata(query);
            if (wtf.Rows.Count <= 0)
            {
                query = "select a.ShiftCodeId, " +
                        "b.[Day],b.TimeOut2 " +
                        "from MEmployee a " +
                        "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                        "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
            }
            else
            {

                //string a = DateTime.Now.ToString();

                if (DateTime.Parse(wtf.Rows[0]["datee"].ToString()) < DateTime.Parse(txt_date.Text))
                {
                    query = "select top 1 a.ShiftCodeId, " +
                            "b.[Day],b.TimeOut2 " +
                            "from TChangeShiftLine a " +
                            "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                            "left join MEmployee c on a.EmployeeId=c.Id " +
                            "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                    //  if (DateTime.Parse(wtf.Rows[0]["datee"].ToString()) < DateTime.Parse(txt_date.Text))
                    query += " order by a.Date desc ";
                    //else
                    // query += " order by Date asc";
                }
                else
                {
                    if (wtf.Rows.Count < 0)
                    {

                        query = "select a.ShiftCodeId, " +
                           "b.[Day],b.TimeOut2 " +
                           "from MEmployee a " +
                           "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                           "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
                    }
                    else
                    {
                        query = "select top 1 a.ShiftCodeId, " +
                          "b.[Day],b.TimeOut2 " +
                          "from TChangeShiftLine a " +
                          "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                          "left join MEmployee c on a.EmployeeId=c.Id " +
                          "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                        query += " order by Date asc";
                    }
                }


            }
            dt = dbhelper.getdata(query);

            DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + txt_date.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification' )");
            if (tdcheck.Rows.Count == 0)
            {

                DateTime getshiftout = Convert.ToDateTime(txt_date.Text + " " + dt.Rows[0]["TimeOut2"].ToString());
                DateTime getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text);
                DateTime otin = Convert.ToDateTime(txt_date.Text + " " + txt_start.Text);


                if (getshiftout > otin)
                    otin = getshiftout;
                if (otin.ToString().Contains("PM") && getotout.ToString().Contains("AM"))
                    getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text).AddDays(1);
                if (getotout > getshiftout)
                {
                    query = "select CONVERT(float, datediff(minute, '" + otin + "','" + getotout.ToString() + "' )) / 60 as time_diff ";
                    DataTable dtt = new DataTable();
                    dtt = dbhelper.getdata(query);

                    string nighthrs = getnightdif.getnight(getshiftout.ToString(), getotout.ToString(), "dtr");// (getotout - getshiftout).TotalHours.ToString();
                    decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString()) - decimal.Parse(nighthrs);

                    dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + txt_date.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0 )");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_OT'", true);
                }
                else
                    Response.Write("<script>alert('Incorect Time Input!')</script>");
            }
            else
                Response.Write("<script>alert('Input Data is already Exist!')</script>");
        }
    }
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        //CheckBox ChkBoxHeader = (CheckBox)grid_view.HeaderRow.FindControl("chkboxSelectAll");
        //int i = 0;
        //foreach (GridViewRow row in grid_view.Rows)
        //{
        //    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkEmp");
        //    if (ChkBoxHeader.Checked == true)
        //    {
        //        ChkBoxRows.Checked = true;
        //        i++;
        //    }
        //    else
        //    {

        //        ChkBoxRows.Checked = false;
        //        if (i > 0)
        //        {
        //            i--;
        //        }
        //    }
        //}


        //if (i > 0)
        //{

        //    if (i == 1)
        //        lbl_del_notify.Text = i + " row selected".ToString();
        //    else
        //        lbl_del_notify.Text = i + " rows selected".ToString();

        //    ib_del.Style.Add("display", "block");
        //    ib_can.Style.Add("display", "block");
        //}
        //else
        //{
        //    lbl_del_notify.Text = "";
        //    ib_del.Style.Add("display", "none");
        //    ib_can.Style.Add("display", "none");
        //}
    }
    protected void delete2(object sender, EventArgs e)
    {
        string test = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            if (chkEmp.Checked == true)
            {
                test = "naa";
                dbhelper.getdata("update TOverTimeLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
            }
        }
        if (test.Trim().Length > 0)
        {
            //Credentials?user_id=" + function.Encrypt(user_id, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='al'", true);
        }
    }
    protected void delete3(object sender, EventArgs e)
    {
        string test = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            if (chkEmp.Checked == true)
            {
                test = "naa";
                //dbhelper.getdata("update TOverTimeLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
            }
        }
        if (test.Trim().Length > 0)
        {

            //Credentials?user_id=" + function.Encrypt(user_id, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='ao'", true);
        }
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
           //function.ReadNotification(row.Cells[0].Text, 0);
            view_id.Value = row.Cells[0].Text;
            hdn_empid.Value = row.Cells[10].Text;
            Div1.Visible = true;
            Div2.Visible = true;
            DataTable dt = dbhelper.getdata("select b.firstname+' '+b.lastname c_name,left(convert(varchar,a.date,101),10)date,a.time_in,a.time_out,a.OvertimeHours,a.OvertimeNightHours,a.remarks  from TOverTimeLine a " +
            " left join memployee b on a.employeeid=b.id where a.ID="+view_id.Value+"");

            lbl_name.Text = dt.Rows[0]["c_name"].ToString();
            lbl_date.Text = dt.Rows[0]["date"].ToString();
            lbl_in.Text = dt.Rows[0]["time_in"].ToString();
            lbl_out.Text = dt.Rows[0]["time_out"].ToString();
            lbl_orha.Text = dt.Rows[0]["OvertimeHours"].ToString();
            lbl_onha.Text = dt.Rows[0]["OvertimeNightHours"].ToString();
            lbl_reason.Text = dt.Rows[0]["remarks"].ToString().Length == 0 ? "-" : dt.Rows[0]["remarks"].ToString();
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("verifyot", false);
    }
    protected void click_approved(object sender, EventArgs e)
    {
        if (chek())
        {
            stateclass a = new stateclass();
            function.ReadNotification(view_id.Value, 0);
            a.sa = view_id.Value;
            a.sb = Session["emp_id"].ToString();
            a.sc = "OT";
            a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
            a.se = "ok";
            bol.system_logs(a);
            dbhelper.getdata("update TOverTimeLine set overtimehoursapp=" + lbl_orha.Text + ",overtimenighthoursapp="+lbl_onha.Text+", status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + view_id.Value + "");
            DataTable dtgetamt = dbhelper.getdata("select ((hourlyrate * nightmultiplier)*overtimenighthoursapp) + ((hourlyrate * regmultiplier)*overtimehoursapp) amt_to_be_paid,EmployeeId from (select case when regmultiplier IS null then 0 else regmultiplier end regmultiplier,case when nightmultiplier IS null then 0 else nightmultiplier end nightmultiplier,case when hourlyrate IS null then 0 else hourlyrate end hourlyrate,overtimehoursapp,overtimenighthoursapp,EmployeeId from TOverTimeLine where Id=" + view_id.Value + " )t");
            DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + hdn_empid.Value + "");
            DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(lbl_date.Text, dtemp.Rows[0]["PayrollGroupId"].ToString()));
            if (dtchkpayroll.Rows.Count == 1)
            {
                DataTable dtr = Core.DTRF(lbl_date.Text, lbl_date.Text, "KIOSK_" + hdn_empid.Value);
                decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["ot_amount"].ToString()) + decimal.Parse(dtr.Rows[0]["otn_amount"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["ot_amount"].ToString()) + decimal.Parse(dtr.Rows[0]["otn_amount"].ToString()) : 0 : 0;
                if (amttt > 0)
                {
                    DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(lbl_date.Text, hdn_empid.Value));
                    if (decimal.Parse(chkifnaa.Rows[0]["overtimeamount"].ToString()) + decimal.Parse(chkifnaa.Rows[0]["overtimenightamount"].ToString()) == 0)
                    {
                        DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(lbl_date.Text,view_id.Value, hdn_empid.Value, "Overtime", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                    }
                }
            }
            function.Notification(view_id.Value, "ot", "0");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='verifyot'", true);
        }
    }
    protected void click_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            view_id.Value = row.Cells[0].Text;
            hdn_empid.Value = row.Cells[10].Text;
            Div1.Visible = true;
            Div4.Visible = true;
            DataTable dt = dbhelper.getdata("select b.firstname+' '+b.lastname c_name,left(convert(varchar,a.date,101),10)date,a.time_in,a.time_out,a.OvertimeHours,a.OvertimeNightHours,a.remarks  from TOverTimeLine a " +
            "left join memployee b on a.employeeid=b.id where a.ID=" + view_id.Value + "");

            lbl_name1.Text = dt.Rows[0]["c_name"].ToString();
            lbl_date1.Text = dt.Rows[0]["date"].ToString();
            lbl_oti.Text = dt.Rows[0]["time_in"].ToString();
            lbl_oto.Text = dt.Rows[0]["time_out"].ToString();
        }
    }
    protected void click_can(object sender, EventArgs e)
    {
        //if (txt_reason.Text.Length > 0)
        //{
        stateclass a = new stateclass();
      //  function.ReadNotification(view_id.Value, 0);
        a.sa = view_id.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "OT";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update TOverTimeLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + view_id.Value + "");
        function.Notification("ot", "1", view_id.Value, hdn_empid.Value);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='verifyot'", true);
        //}
        //else
        //    Response.Write("<script>alert('Remarks must be requires')</script>");
    }
    protected bool chek()
    {
        bool err = true;
        if (lbl_onha.Text.Length == 0)
        {
            lbl_eronha.Text = "*";
            err = false;
        }
        else if (lbl_orha.Text.Length == 0)
        {
            lbl_erorha.Text = "*";
            err = false;
        }
        return err;
    }
    
}