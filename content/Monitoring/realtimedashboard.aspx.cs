using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Monitoring_realtimedashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        DataTable final_disp = new DataTable();
        final_disp.Columns.Add(new DataColumn("empid", typeof(string)));
        final_disp.Columns.Add(new DataColumn("idnumber", typeof(string)));
        final_disp.Columns.Add(new DataColumn("empname", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Department", typeof(string)));
        final_disp.Columns.Add(new DataColumn("shiftcode", typeof(string)));
        final_disp.Columns.Add(new DataColumn("date", typeof(string)));
        final_disp.Columns.Add(new DataColumn("in1", typeof(string)));
        final_disp.Columns.Add(new DataColumn("out1", typeof(string)));
        final_disp.Columns.Add(new DataColumn("in2", typeof(string)));
        final_disp.Columns.Add(new DataColumn("out2", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("latehrs", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("othrs", typeof(string)));
        DataRow final_dr;

       
        DataTable gtempList = dbhelper.getdata("select * from memployee where payrollgroupid<>'9'");
        foreach (DataRow dr in gtempList.Rows)
        {
            DataTable dt = adjustdtrformat.livedtr();
            DataRow[] adj_data =dt.Select("emp_id="+dr["id"]+"");
            final_dr = final_disp.NewRow();
            final_dr["empid"] = dr["id"];
            final_dr["idnumber"] = dr["idnumber"];
            final_dr["empname"] = dr["firstname"].ToString()+' '+dr["middlename"].ToString()+' '+dr["lastname"].ToString();
            DataTable dtgetdept=dbhelper.getdata("select * from mdepartment where id="+dr["departmentid"]+"");
            string dattee = adj_data.Count()>0?adj_data[0]["date"].ToString():"00/00/0000";
            string hjhj = "select * from TChangeShiftLine where employeeid=" + dr["id"] + " and left(convert(varchar,date,101),10)='" + dattee + "'";
            DataTable dtgetchangeshift = dbhelper.getdata(hjhj);
            string scid=dtgetchangeshift.Rows.Count>0?dtgetchangeshift.Rows[0]["shiftcodeid"].ToString():dr["shiftcodeid"].ToString();
            
            DataTable get_exactscid=dbhelper.getdata("select * from MShiftCode where id="+scid+"");
            //string day=Convert.ToDateTime(dattee).DayOfWeek.ToString();
           
            //DataTable get_exactdayshiftday = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + scid + " and day='" + day + "' ");


            final_dr["Department"] = dtgetdept.Rows[0]["department"].ToString();
            final_dr["shiftcode"] = get_exactscid.Rows[0]["Remarks"].ToString();
            final_dr["date"] = adj_data.Count()>0?adj_data[0]["date"].ToString():"0";
            final_dr["in1"] = adj_data.Count()>0?adj_data[0]["Date_Time_In"].ToString():"0";
            final_dr["out1"] = adj_data.Count()>0?adj_data[0]["Date_Time_Out1"].ToString():"0";
            final_dr["in2"] = adj_data.Count()>0?adj_data[0]["Date_Time_In2"].ToString():"0";
            final_dr["out2"] = adj_data.Count() > 0 ? adj_data[0]["Date_Time_Out"].ToString() : "0";
            //final_dr["latehrs"] = "0";
            //final_dr["othrs"] = "0";
            final_disp.Rows.Add(final_dr);
        }
        grid_timesheet.DataSource = final_disp;
        grid_timesheet.DataBind();
       
    }
    int cnt_abs = 0; int total_lates = 0; int total_headcnt = 0;
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TimeSpan total_late=new TimeSpan();
            if (e.Row.Cells[6].Text == "0")
                cnt_abs = cnt_abs + 1;
            if (e.Row.Cells[7].Text.Length > 1)
            {
                string date = Convert.ToDateTime(e.Row.Cells[6].Text).DayOfWeek.ToString();
                DataTable dt = dbhelper.getdata("select * from mshiftcodeday where shiftcodeid=" + getshiftcode.shiftcode(date, e.Row.Cells[1].Text) + " and day='" + date + "' ");

                DateTime shift_in1 = Convert.ToDateTime(date + " " + dt.Rows[0]["TimeIn1"].ToString());
                DateTime curr_in1 = Convert.ToDateTime(e.Row.Cells[7].Text);

                if (curr_in1 > shift_in1)
                    total_late = (curr_in1 - shift_in1);
                if (decimal.Parse(total_late.TotalHours.ToString()) > 0)
                {
                    total_lates = total_lates + 1;
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }

                total_headcnt = total_headcnt + 1;




                //string getdayshift=
            }
            lbl_punchheadcount.Text = "Head Counts:" + total_headcnt + " ";
            lbl_tlate.Text = "Total Late:" + total_lates + " ";
            lbl_tabsent.Text = "Total Absent:" + cnt_abs + " ";
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Response.Redirect("timesheetmonitoring?user_id=RWr2swgaIPQ=");
    }
}