using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;
using System.Data.SqlClient;
using System.Web.Services;

public partial class content_Employee_Default : System.Web.UI.Page
{
    public static DataTable dt;
    public static string today;
    public static string events;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");

        hf_id.Value = Session["user_id"].ToString();

        hf_empid.Value = Session["emp_id"].ToString();
        if (!IsPostBack)
            loadable();

        modal.Style.Add("display", "none");
    }

    protected void grid_hide(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
    }
    
    protected void loadable()
    {
        //Todays date
        string[] d = DateTime.Now.ToShortDateString().Split('/');
        string day = d[1].Length == 1 ? "0" + d[1] : d[1];
        string month =d[0].Length == 1 ? "0" + d[0] : d[0];
        int year = int.Parse(d[2]);
        today = d[2] + '-' + month + '-' + day;

        string ttoday = month + '/' + day + '/' + d[2];

        DataTable dtr = Core.DTRF(ttoday, ttoday, "KIOSK_" + Session["emp_id"].ToString());
        if (dtr.Rows.Count > 0)
        {
            if (dtr.Rows[0]["timein1"].ToString().Length > 2)
            {
                string[] hjhj = dtr.Rows[0]["timein1"].ToString().Split(' ');
                lbl_time.Text = hjhj[1];
                lbl_ampm.Text = hjhj[2];
            }
            else
            {
                lbl_time.Text = "00:00";
                lbl_ampm.Text = "AM";
            }

        }

              
        //Calendar
        events = null;
        int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        DataTable dt_employee = dbhelper.getdata("select ShiftCodeId from memployee where id=" + Session["emp_id"].ToString());
        string maw = "select left(CONVERT(VARCHAR(12), [date], 101), 10) [date], * " +
        "from [TChangeShiftLine] a " +
        "left join MShiftCode b on a.ShiftCodeId=b.Id " +
        "where  employeeid=" + Session["emp_id"].ToString();
        DataTable dt_calendar = dbhelper.getdata("select left(CONVERT(VARCHAR(12), [date], 101), 10) [date], * " +
        "from [TChangeShiftLine] a " +
        "left join MShiftCode b on a.ShiftCodeId=b.Id " +
        "where  employeeid=" + Session["emp_id"].ToString());

        DataTable dt_holiday = dbhelper.getdata("select distinct left(CONVERT(VARCHAR(12), [date], 101), 10) [date], * from MDayTypeDay a where a.BranchId=(Select BranchId from MEmployee where id=" + Session["emp_id"].ToString() + ") and case when a.status is null then 'active' else a.status end not like '%cancel%' ");
        foreach (DataRow rows in dt_holiday.Rows)
        {
            events += "{title: '" + rows["Remarks"].ToString().Replace("'", "") + "', start: '" + Convert.ToDateTime(rows["date"].ToString()).ToString("yyyy-MM-dd") + "',  backgroundColor: '#00a65a' },";
        }

        DataTable dt_leaves = dbhelper.getdata("select left(CONVERT(VARCHAR(12), [Date], 101), 10) [date], * from TLeaveApplicationLine where EmployeeId=" + Session["emp_id"].ToString() + " and [status] like '%Approved%' ");
        foreach (DataRow rows in dt_leaves.Rows)
        {
            events += "{title: 'Leave', start: '" + Convert.ToDateTime(rows["Date"].ToString()).ToString("yyyy-MM-dd") + "',backgroundColor: '#800080' },";
        }

        DataTable dt_announcement = dbhelper.getdata("select left(CONVERT(VARCHAR(12), [memo_date], 101), 10) [date], * from Tmemo a Left Join TmemoLine b on a.id=b.memo_id where b.recipient=" + Session["emp_id"].ToString() + "");
        foreach (DataRow rows in dt_announcement.Rows)
        {
            events += "{title: '" + rows["memo_subject"].ToString().Replace("'", "") + "', start: '" + Convert.ToDateTime(rows["date"].ToString()).ToString("yyyy-MM-dd") + "',end:'" + Convert.ToDateTime(rows["expirationdate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "',backgroundColor: '#f39c12' },";
        }

        DataTable dt_note = dbhelper.getdata("select left(CONVERT(VARCHAR(12), [Date], 101), 10) [date], * from MNote where emp_id=" + Session["emp_id"].ToString() + "");
        foreach (DataRow rows in dt_note.Rows)
        {
            events += "{title: '" + rows["Description"].ToString().Replace("'", "") + "', start: '" + Convert.ToDateTime(rows["Date"].ToString()).ToString("yyyy-MM-dd") + "',end:'" + Convert.ToDateTime(rows["ExpDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "',backgroundColor: '#00c0ef' },";
        }

        for (int i = 1; i <= days; i++)
        {
            string dd = i.ToString().Length == 1 ? "0" + i : i.ToString();
            
            //{
            //    DataTable shift = dbhelper.getdata("select shiftcode from MShiftCode where id=" + dt_employee.Rows[0]["ShiftCodeId"].ToString());
            //    events += "{title: '" + shift.Rows[0]["shiftcode"].ToString() + "', start: '" + year + "-" + month + "-" + dd + "'},";
            //}
            //else

            if (dt_calendar.Rows.Count > 0)
            {
                string now = month+"/"+dd+"/"+year;
                DataRow[] s = dt_calendar.Select("date='" + now + "' and status='approved'");
                if(s.Count() > 0)
                    events += "{title: '" + s[0]["shiftcode"].ToString() + "', start: '" + year + "-" + month + "-" + dd + "'},";
            }
        }

        string query = "select DATENAME(MM, getdate()) + RIGHT(CONVERT(VARCHAR(12), getdate(), 107), 9) today, PayrollGroupId from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);

        l_today.Text = dtt.Rows[0]["today"].ToString();
        
        //Approval
        query = "select (select COUNT(*) from TLeaveApplicationLine where status like '%for approval%' and employeeid  = " + Session["emp_id"].ToString() + ") TLeaveApplicationLine, " +
        "(select COUNT(*) from TOverTimeLine where status like '%for approval%' and employeeid  = " + Session["emp_id"].ToString() + ") TOverTimeLine," +
        "(select COUNT(*) from TMealHours where status like '%for approval%' and employeeid  = " + Session["emp_id"].ToString() + ") TMealHours," +
        "(select COUNT(*) from Tmanuallogline where status like '%for approval%' and employeeid  = " + Session["emp_id"].ToString() + ") Tmanuallogline," +
        "(select COUNT(*) from Ttravel where status like '%for approval%' and emp_id  = " + Session["emp_id"].ToString() + ") Ttravel," +
        "(select COUNT(*) from Tundertime where status like '%for approval%' and emp_id = " + Session["emp_id"].ToString() + ") Tundertime," +
        "(select COUNT(*) from TRestdaylogs where status like '%For Approval%' and EmployeeId  = " + Session["emp_id"].ToString() + ") TRestdaylogs," +
        "(select  COUNT(*) from temp_shiftcode  where emp_id = " + Session["emp_id"].ToString() + " and status like '%for approval%' ) Tchangeshit," +
        "(select COUNT(*) from TmemoLine where recipient = " + hf_id.Value + " and status is null) Tmemo";
        DataTable dtapproval = dbhelper.getdata(query);
        l_mla.Text = dtapproval.Rows[0]["TMealHours"].ToString();
        l_ot.Text = dtapproval.Rows[0]["TOverTimeLine"].ToString();
        l_undertime.Text = dtapproval.Rows[0]["Tundertime"].ToString();
        l_leave.Text = dtapproval.Rows[0]["TLeaveApplicationLine"].ToString();
        l_ta.Text = dtapproval.Rows[0]["Tmanuallogline"].ToString();
        l_obt.Text = dtapproval.Rows[0]["Ttravel"].ToString();
        l_wvs.Text = dtapproval.Rows[0]["TRestdaylogs"].ToString();
        l_csht.Text = dtapproval.Rows[0]["Tchangeshit"].ToString();

        int request = int.Parse(l_ot.Text) + int.Parse(l_undertime.Text) + int.Parse(l_mla.Text) + int.Parse(l_leave.Text) + int.Parse(l_ta.Text) + int.Parse(l_obt.Text) + int.Parse(l_wvs.Text) + int.Parse(l_csht.Text);
        l_request.Text = request.ToString();
       
        //Leave
        //query = "select a.id, a.leave,a.leavetype,case when (select leavestatus from memployee where ID=" + Session["emp_id"].ToString() + ")='False' then '0' else a.yearlytotal end yearlytotal,(select leavestatus from memployee where ID=" + Session["emp_id"].ToString() + ") leavestatus, " +
        //" case when (select leavestatus from memployee where ID=" + Session["emp_id"].ToString() + ")='False' then 0 else case when (a.yearlytotal)-(select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + Session["emp_id"].ToString() + " and withpay='True') is null then a.yearlytotal " +
        //"else " +
        //"(a.yearlytotal)-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + Session["emp_id"].ToString() + " and withpay='True' and status like '%Approve%') is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + Session["emp_id"].ToString() + " and withpay='True' and status like '%Approve%')  end) " +
        //"end end " +
        //"leave_bal from MLeave a where a.action is null ";

        DataTable dttsetup = dbhelper.getdata("Select * from SetUpTable");

        if (dttsetup.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(Session["emp_id"].ToString(), DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

            DataTable dpres = dbhelper.getdata(twos[0]);
            DataTable dprev = dbhelper.getdata(twos[1]);

            for (int i = 0; i < dpres.Rows.Count; i++)
            {
                double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance"].ToString());
                double diff = asd <= 0 ? 0 : asd;

                for (int j = 0; j < dprev.Rows.Count; j++)
                {
                    if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                    {
                        dprev.Rows[j]["Balance"] = (Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff).ToString();
                    }
                }
            }
            l_sl.Text = dprev.Rows.Count > 0 ? dprev.Rows[0]["Balance"].ToString() : "0.00";
            l_vlc.Text = dprev.Rows.Count > 0 ? dprev.Rows.Count >= 2 ? dprev.Rows[1]["Balance"].ToString() : "0.00" : "0.00";
        }


        //Holiday
        DataTable dt = dbhelper.getdata("select top 1 datediff(DAY,[date], GETDATE()), DATENAME(MM, a.date) + RIGHT(CONVERT(VARCHAR(12), a.date, 107), 9), (select daytype from mdaytype where id= a.daytypeid ) daytype from mdaytypeday a where a.date+1 > GETDATE() and case when a.status is null then 'active' else a.status end not like '%cancel%' order by a.date");
        if (dt.Rows.Count > 0)
        {
            int x = int.Parse(dt.Rows[0][0].ToString());
            l_holiday.Text = dt.Rows[0][1].ToString();

            if (x == 0)
            {
                lb_upcoming.Visible = false;
            }
            else
                lb_upcoming.Visible = true;
            if (dt.Rows.Count > 0)
            {
                lb_upcoming.Text = dt.Rows[0][2].ToString();
                lb_upcoming.Visible = true;
            }

        }
        else
        {
            lb_upcoming.Visible = false;
            l_holiday.Text = "None";
        }

        DataTable delnotelist = dbhelper.getdata("select * from MNote where emp_id=" + Session["emp_id"].ToString() + "");
        grid_forms.DataSource = delnotelist;
        grid_forms.DataBind();
        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;

    }

    protected void dtr_go(object sender, EventArgs e)
    {
        data();
    }

    protected void data()
    {
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("Insert into MNote Values('" + txt_datefirst.Text + "','" + txt_dateexp.Text + "','" + txt_description.Text + "'," + Session["emp_id"].ToString() + ")");

        Response.Redirect("employee-dashboard");
    }
    protected void deleteer(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = dbhelper.getdata("Delete from MNote where id=" + row.Cells[0].Text + "");
            loadable();
            modal.Style.Add("display", "block");
        }
    }
    protected void delviewclick(object sender, EventArgs e)
    {
        modal.Style.Add("display", "block");
    }
    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }

    [WebMethod]
    public static void getLeaveUpdate(string id)
    {
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            string datetime = DateTime.Now.ToString();
            string empid = id;
            string query = " Select emp_status,DateHired,case when DATEDIFF(Year, DateHired, GETDATE()) !=0 then DATEDIFF(Year, DateHired, GETDATE()) else (case when DATEDIFF(MONTH, DateHired, GETDATE())>=6 then 0.5 else 0 end) end empspan from MEmployee where Id=" + id + "";

            using (SqlCommand clm = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = clm.ExecuteReader();
                if (reader.Read())
                {
                    DataTable dttt = dbhelper.getdata("Select * from SetUpTable");

                    if (dttt.Rows.Count > 0)
                    {
                        if (dttt.Rows[0]["LeaveType"].ToString() == "4")
                        {
                            //string additionals = "";
                            //string[] lma = dttt.Rows[0]["LeaveMonthlyAdd"].ToString().Split(',');

                            //for (int i = 0; i < lma.Count() - 1; i++)
                            //{
                            //    additionals += "leaveid=(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null) or ";
                            //    i += 2;
                            //}
                            string additionals = "";
                            string[] lma = dttt.Rows[0]["LeaveMonthlyAdd"].ToString().Split(',');
                            List<string> list = new List<string>(lma);
                            list.RemoveAt(list.Count - 1);
                            DataTable dts = dbhelper.getdata("Select a.*,(Select LeaveType from MLeave where Id=a.LeaveTypeId) [lti],(Select yearlytotal from MLeave where Id=a.LeaveTypeId) [ytotal] from LeaveTypeSetUp a");

                            foreach (DataRow row in dts.Rows)
                            {
                                string[] temp = row["StatusReq"].ToString().Split(',');
                                if (temp.Contains(reader["emp_status"].ToString()))
                                {
                                    list.Add("01 January");
                                    list.Add(row["ytotal"].ToString());
                                    list.Add(row["lti"].ToString());
                                }
                            }

                            list.Add("");
                            lma = list.ToArray();

                            for (int i = 0; i < lma.Count() - 1; i++)
                            {
                                additionals += "leaveid=(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null) or ";
                                i += 2;
                            }

                            string[] ldn = dttt.Rows[0]["LeaveDivision"].ToString().Split(',');
                            string adddldn = "";
                            for (int i = 0; i < ldn.Count() - 1; i++)
                            {
                                adddldn += " DivisionId2=" + ldn[i] + " or";
                            }
                            string adddlds = "";
                            for (int i = 0; i < ldn.Count() - 1; i++)
                            {
                                adddlds += " DivisionId2!=" + ldn[i] + " and";
                            }

                            DataTable dtgetempspan = dbhelper.getdata("Select top 1 effectivedate,DATEDIFF(month, effectivedate, Convert(varchar,Year(DateAdd(year,1,GETDATE())))+'-01-01') empspan from memployeestatus a left join MEmployee b on a.empid=b.Id where statusid=2 and (" + adddlds.Substring(0, adddlds.Length - 4) + ") and DATEDIFF(month, effectivedate, Convert(varchar,Year(DateAdd(year,1,GETDATE())))+'-01-01')<12 and effectivedate <= GETDATE()  and empid=" + id + " order by effectivedate desc");
                            DataTable dtgetempspan2 = dbhelper.getdata("Select emp_status,DateHired,DATEDIFF(month, DateHired, Convert(varchar,Year(DateAdd(year,1,GETDATE())))+'-01-01') empspan from MEmployee where Id=" + id + " and (emp_status=2 or (" + adddldn.Substring(0, adddldn.Length - 3) + ")) and DATEDIFF(month, DateHired, Convert(varchar,Year(DateAdd(year,1,GETDATE())))+'-01-01')<12");

                            DateTime empspan = new DateTime();


                            if (dtgetempspan2.Rows.Count > 0)
                            {
                                empspan = Convert.ToDateTime(dtgetempspan2.Rows[0]["DateHired"].ToString());

                                DateTime now = DateTime.Now;
                                DateTime edy = new DateTime(now.Year, 12, 1);

                                for (int i = 0; i < lma.Count() - 1; i++)
                                {
                                    if (lma[i + 2] == "VL" || lma[i + 2] == "SL")
                                    {
                                        double p = (((GetLastDayOfMonth(edy) - Convert.ToDateTime(empspan)).TotalDays / 30) * (Convert.ToDouble(lma[i + 1]) / 12));

                                        double f = Math.Floor(p);
                                        if (p > (Math.Floor(p) + .5) && p < (Math.Ceiling(p)))
                                        {
                                            p = f + 1;
                                        }
                                        else if (p > (Math.Floor(p)) && p < (Math.Floor(p) + .5))
                                        {
                                            p = f + .5;
                                        }

                                        lma[i + 1] = p.ToString();
                                    }
                                    i += 2;
                                }
                            }
                            else if (dtgetempspan.Rows.Count > 0)
                            {
                                empspan = Convert.ToDateTime(dtgetempspan.Rows[0]["effectivedate"].ToString());

                                DateTime now = DateTime.Now;
                                DateTime edy = new DateTime(now.Year, 12, 1);

                                for (int i = 0; i < lma.Count() - 1; i++)
                                {
                                    if (lma[i + 2] == "VL" || lma[i + 2] == "SL")
                                    {
                                        double p = (((GetLastDayOfMonth(edy) - Convert.ToDateTime(empspan)).TotalDays / 30) * (Convert.ToDouble(lma[i + 1]) / 12));

                                        double f = Math.Floor(p);
                                        if (p > (Math.Floor(p) + .5) && p < (Math.Ceiling(p)))
                                        {
                                            p = f + 1;
                                        }
                                        else if (p > (Math.Floor(p)) && p < (Math.Floor(p) + .5))
                                        {
                                            p = f + .5;
                                        }

                                        lma[i + 1] = p.ToString();
                                    }
                                    i += 2;
                                }
                            }

                            

                            string nani = "";

                            string dfrom = Convert.ToDateTime(Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("2000-MM-dd")) > Convert.ToDateTime(DateTime.Now.ToString("2000-MM-dd")) ? DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("MM-dd") : DateTime.Now.ToString("yyyy") + "-" + Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("MM-dd");
                            string dto = Convert.ToDateTime(dfrom).AddYears(1).ToString("yyyy-MM-dd");

                            DateTime from = Convert.ToDateTime(dfrom);
                            DateTime to = Convert.ToDateTime(dto);

                            string[] diff = Enumerable.Range(0, Int32.MaxValue)
                                     .Select(e => from.AddMonths(e))
                                     .TakeWhile(e => e <= to)
                                     .Select(e => e.ToString("MMMM-yyyy")).ToArray();

                            for (int i = 0; i < lma.Count() - 1; i++)
                            {
                                for (int j = 0; j < diff.Count(); j++)
                                {
                                    if (Convert.ToDateTime(diff[j]).Month == Convert.ToDateTime(lma[i]).Month && DateTime.Now >= Convert.ToDateTime(lma[i] + " " + Convert.ToDateTime(diff[j]).ToString("yyyy")))
                                    {
                                        DataTable dtt = dbhelper.getdata("Select a.*,(Select LeaveType from Mleave where id=a.leaveid) acrl,(Select converttocash from Mleave where id=a.leaveid) ctc from leave_credits a where empid=" + id + " and action is NULL and (" + additionals.Substring(0, additionals.Length - 3) + ") and mark=2 and yyyyear=" + from.Year.ToString() + "");

                                        if (dtt.Select("acrl='" + lma[i + 2] + "'").Count() > 0)
                                        {
                                            nani += " Update leave_credits set credit=" + lma[i + 1] + " where leaveid=(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null) and mark=2 and empid=" + id + " and yyyyear=" + from.Year.ToString() + "";
                                        }
                                        else
                                        {
                                            dbhelper.getdata("insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + id + ",1,(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null)," + lma[i + 1] + ",'no','Yearly'," + from.Year.ToString() + ",2)");
                                        }
                                    }
                                }

                                i += 2;
                            }


                            if (nani != "")
                            {
                                dbhelper.getdata(nani);
                            }

                        }
                    }
                }
                con.Close();
            }
        }
    }

    public static DateTime GetLastDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }
}