using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_account_admin_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            load();
            data();
            live();
        }
    }

    protected void onchangeyear(object sendeer, EventArgs e)
    {
        lb_payroll.Text = "";
        payroll();
    }

    public void load() 
    {
        DataTable dt;

        //Select Date Today.
        dt = dbhelper.getdata("SELECT YEAR(GETDATE())todate");
        dl_year.Text = dt.Rows[0]["todate"].ToString();

        //Insert Current Year
        dt = dbhelper.getdata("select * from MYear where [Year] like " + dt.Rows[0]["todate"] + "");
        if (dt.Rows.Count == 0)
        {
            dt = dbhelper.getdata("SELECT YEAR(GETDATE())todate");
            dbhelper.getdata("insert into MYear values ('" + dt.Rows[0]["todate"] + "')");
        }

        //Select Display Year
        dt = dbhelper.getdata("select * from MYear order by [Year] desc");
        dl_year.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            dl_year.Items.Add(new ListItem(dr["Year"].ToString(), dr["id"].ToString()));
        }
    }

    public void data()
    {
        //Holiday
        DataTable dt = dbhelper.getdata("select top 1 datediff(DAY,[date], GETDATE()), DATENAME(MM, a.date) + RIGHT(CONVERT(VARCHAR(12), a.date, 107), 9), (select daytype from mdaytype where id=(select [type] from MDayTypeHoliday where id=a.HolidayId) ) daytype,(select name from MDayTypeHoliday where id=a.HolidayId) holname from mdaytypeday a where a.date+1 > GETDATE() and case when a.status is null then 'active' else a.status end not like '%cancel%' order by a.date");
        if (dt.Rows.Count > 0)
        {
            int x = int.Parse(dt.Rows[0][0].ToString());
            l_holiday.Text = dt.Rows[0][1].ToString();
            lb_upcoming.Text = dt.Rows[0][2].ToString();
            l_holname.Text = dt.Rows[0][3].ToString();

            if (x == 0)
                lb_upcoming.Visible = false;
            else
                lb_upcoming.Visible = true;
        }
        else
        {
            lb_upcoming.Visible = false;
            l_holiday.Text = "-";
        }

        dt = getdata.GetHeadCount();
        lb_hc.Text = dt.Rows[0]["cnt"].ToString();

        dt = dbhelper.getdata("select (select COUNT(distinct(a.l_id)) from TLeaveApplicationLine a left join Approver b on a.EmployeeId=b.emp_id where a.status like '%verification%') + " +
            "(select COUNT(*)from RequestUpdate201 where status = 'For Approval')+" +
            "(select COUNT(*) from TOverTimeLine where status like '%verification%') + " +
            "(select COUNT(*) from Tmanuallogline where status like '%verification%') + " +
            "(select COUNT(*) from TRestdaylogs where status like '%verification%') + " +
            "(select COUNT(*) from Ttravel where status like '%verification%') + " +
            "(select COUNT(*) from Tundertime where status like '%verification%') cnt");
        lb_ra.Text = dt.Rows[0]["cnt"].ToString();
        dt = dbhelper.getdata("select (select COUNT(*) from CLI_Recruitment.dbo.Jobs where date_cancelled is NULL)cnt");
        lb_po.Text = dt.Rows[0]["cnt"].ToString();
        dt = dbhelper.getdata("select (select COUNT(*)from MEmployee a left join Mpayrollgroup b on a.PayrollGroupId=b.id where b.status<>0 and b.Id<>7 and b.Id<>4 and a.sex='male')male,(select  COUNT(*)from MEmployee a left join Mpayrollgroup b on a.PayrollGroupId=b.id where b.status<>0 and b.Id<>7 and b.Id<>4 and a.sex='female')female");
        lb_gender.Text = "{value: " + dt.Rows[0]["male"].ToString() + ", label: 'Male'}, {value: " + dt.Rows[0]["female"].ToString() + ", label: 'Female'}";
        dt = dbhelper.getdata("select b.department, count(*) cnt from MEmployee a " +
        "left join MDepartment b on a.departmentid = b.id " +
        "left join Mpayrollgroup c on a.PayrollGroupId=c.id " +
        "where c.status <> 0 " +
        "group by b.department");

        foreach (DataRow row in dt.Rows)
        {
            lb_hpd.Text += "{ y: '" + row["department"].ToString() + "', a: " + row["cnt"].ToString() + " },";    
        }

        payroll();

    }

    public void payroll()
    {
        double amount= 0;
        DataTable dt = dbhelper.getdata("WITH R(N) AS( SELECT 0 UNION ALL SELECT N+1 FROM R WHERE N < 11) " +
        "SELECT DATENAME(MONTH,DATEADD(MONTH,-N,GETDATE())) AS [month], " +
        "(select case when sum(a.NetIncome)is NULL then 0 else SUM(cast(a.NetIncome as money)) end " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "left join TDTR f on b.DTRId=f.Id " +
        "where PayrollId <> 9 " +
        "and year(payrolldate) = " + dl_year.SelectedItem + " and b.status is null " +
        "and MONTH(payrolldate) = month(DATEADD(MONTH,-N,GETDATE())))  amount " +
        "FROM R order by MONTH(DATEADD(MONTH,-N,GETDATE())) ");
        foreach (DataRow row in dt.Rows)
        {
            lb_payroll.Text += "{x: '" + row["month"].ToString() + "', Php: " + row["amount"].ToString() + "},";
            amount += double.Parse(row["amount"].ToString());
        }
        lb_tamount.Text = "Php "  + Convert.ToDecimal(amount).ToString("#,###.00");
    }

    protected void live()
    {
        string query = "select distinct idnumber from tdtrperpayrolperline  " +
        "where  CONVERT(VARCHAR(10), biotime, 101) = CONVERT(VARCHAR(10), GETDATE(), 101) " +
        "and CONVERT(VARCHAR(8), biotime, 108) > '08:05:00'";
        DataTable dt = dbhelper.getdata(query);
        lbl_tlate.Text = dt.Rows.Count.ToString();

        query = "select distinct idnumber from tdtrperpayrolperline where  CONVERT(VARCHAR(10), biotime, 101) = CONVERT(VARCHAR(10), GETDATE(), 101)";
        dt = dbhelper.getdata(query);
        lbl_punchheadcount.Text = dt.Rows.Count.ToString();

        DataTable ds= dbhelper.getdata("Select * from(Select a.Id,(Select COUNT(*) from tdtrperpayrolperline b where b.empid=a.Id and CONVERT(date, b.biotime) = CONVERT(date, GETDATE())) tdtr,(Select COUNT(*) from MShiftCodeDay c where c.ShiftCodeId=a.ShiftCodeId and ((c.RestDay='True' and c.Day='Not Fix') or (c.RestDay='True' and c.Day=datename(dw,GETDATE())))) msd,(Select COUNT(*) from TChangeShiftLine d left join MShiftCodeDay e on d.ShiftCodeId=e.ShiftCodeId where d.EmployeeId=a.Id and Convert(date,d.Date)=Convert(date,GETDATE()) and ((e.RestDay='True' and e.Day='Not Fix') or (e.RestDay='True' and e.Day=datename(dw,GETDATE()))) and d.status='approved')tcs from MEmployee a  )tt where tdtr=0 and (msd>0 or tcs>0)");

        lbl_tabsent.Text = ((int.Parse(lb_hc.Text) - dt.Rows.Count) - ds.Rows.Count).ToString();


        //DataTable final_disp = new DataTable();
        //final_disp.Columns.Add(new DataColumn("empid", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("idnumber", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("empname", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("Department", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("shiftcode", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("date", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("in1", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("out1", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("in2", typeof(string)));
        //final_disp.Columns.Add(new DataColumn("out2", typeof(string)));
        ////final_disp.Columns.Add(new DataColumn("latehrs", typeof(string)));
        ////final_disp.Columns.Add(new DataColumn("othrs", typeof(string)));
        //DataRow final_dr;
        //string query = "select * from memployee";
        ////if (function.Decrypt(Request.QueryString["user_id"].ToString(), true) != "1")
        ////    query += " and id=" + function.Decrypt(Request.QueryString["user_id"].ToString(), true) + "";
        //DataTable gtempList = dbhelper.getdata(query);
        //foreach (DataRow dr in gtempList.Rows)
        //{
        //    DataTable dt = adjustdtrformat.livedtr();
        //    DataRow[] adj_data = dt.Select("emp_id='" + dr["id"] + "'");
        //    final_dr = final_disp.NewRow();
        //    final_dr["empid"] = dr["id"];
        //    final_dr["idnumber"] = dr["idnumber"];
        //    final_dr["empname"] = dr["firstname"].ToString() + ' ' + dr["middlename"].ToString() + ' ' + dr["lastname"].ToString();
        //    DataTable dtgetdept = dbhelper.getdata("select * from mdepartment where id=" + dr["departmentid"] + "");
        //    string dattee = adj_data.Count() > 0 ? adj_data[0]["date"].ToString() : "00/00/0000";
        //    string hjhj = "select * from TChangeShiftLine where employeeid=" + dr["id"] + " and left(convert(varchar,date,101),10)='" + dattee + "'";
        //    DataTable dtgetchangeshift = dbhelper.getdata(hjhj);
        //    string scid = dtgetchangeshift.Rows.Count > 0 ? dtgetchangeshift.Rows[0]["shiftcodeid"].ToString() : dr["shiftcodeid"].ToString();
        //    DataTable get_exactscid = dbhelper.getdata("select * from MShiftCode where id=" + scid + "");
        //    //string day=Convert.ToDateTime(dattee).DayOfWeek.ToString();
        //    //DataTable get_exactdayshiftday = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + scid + " and day='" + day + "' ");


        //    final_dr["Department"] = dtgetdept.Rows[0]["department"].ToString();
        //    final_dr["shiftcode"] = get_exactscid.Rows[0]["Remarks"].ToString();
        //    final_dr["date"] = adj_data.Count() > 0 ? adj_data[0]["date"].ToString() : "0";
        //    final_dr["in1"] = adj_data.Count() > 0 ? adj_data[0]["Date_Time_In"].ToString() : "0";
        //    final_dr["out1"] = adj_data.Count() > 0 ? adj_data[0]["Date_Time_Out1"].ToString() : "0";
        //    final_dr["in2"] = adj_data.Count() > 0 ? adj_data[0]["Date_Time_In2"].ToString() : "0";
        //    final_dr["out2"] = adj_data.Count() > 0 ? adj_data[0]["Date_Time_Out"].ToString() : "0";
        //    //final_dr["latehrs"] = "0";
        //    //final_dr["othrs"] = "0";
        //    final_disp.Rows.Add(final_dr);
        //}
        //grid_timesheet.DataSource = final_disp;
        //grid_timesheet.DataBind();
    }
    int cnt_abs = 0; int total_lates = 0; int total_headcnt = 0;
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TimeSpan total_late = new TimeSpan();
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
            //lbl_punchheadcount.Text = "Head Counts:" + total_headcnt + " ";
            //lbl_tlate.Text = "Total Late:" + total_lates + " ";
            //lbl_tabsent.Text = "Total Absent:" + cnt_abs + " ";
            lbl_punchheadcount.Text = total_headcnt.ToString();
            lbl_tlate.Text = total_lates.ToString();
            lbl_tabsent.Text = cnt_abs.ToString();
        }
    }
  
}