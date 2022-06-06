using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_DTR_att : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            loadable();
           //disp();
        }
    }

    protected void loadable()
    {
        DataTable dt;
        string query = "select distinct(a.emp_id), " +
        "b.IdNumber,b.FirstName+' '+b.LastName as fullname " +
        "from approver a " +
        "left join memployee b on a.emp_id=b.id ";
        dt = dbhelper.getdata(query);

        drop_emp.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            drop_emp.Items.Add(new ListItem(dr["fullname"].ToString(), dr["emp_id"].ToString()));
        }
    }

    protected void btn_go(object sender, EventArgs e)
    {
        disp();
    }

    protected void disp()
    {
        string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();

        DataTable dt = dbhelper.getdata("select * from memployee where id=" + drop_emp.SelectedValue + "");
        DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
        string from = txt_from.Text.Length > 0 ? txt_from.Text : month + "/01/" + DateTime.Now.Year;
        string to = txt_to.Text.Length > 0 ? txt_to.Text : month + "/" + day + "/" + DateTime.Now.Year;
        if (txt_to.Text.Length == 0 && txt_to.Text.Length == 0)
        {
            if (dtperiod.Rows.Count > 0)
            {
                from = dtperiod.Rows[0]["ffrom"].ToString();
                to = dtperiod.Rows[0]["tto"].ToString();
            }
        }

        //string a = drop_emp.SelectedValue;

        DataTable dtr = Core.DTRF(from, to, "KIOSK_" + drop_emp.SelectedValue);
        if (dtr.Rows.Count > 0)
        {
            grid_item.DataSource = dtr;
            grid_item.DataBind();
        }

        pnl_alert.Visible = dtr.Rows.Count == 0 ? true : false;

    }

    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_ot = (LinkButton)e.Row.FindControl("lnk_ot");
            LinkButton lnk_tadj = (LinkButton)e.Row.FindControl("lnk_tadj");
            LinkButton lnk_offset = (LinkButton)e.Row.FindControl("lnk_offset");
            string scid = e.Row.Cells[23].Text;
            string[] datenname = e.Row.Cells[3].Text.Split('-');
            string[] getdatefromout = e.Row.Cells[9].Text.Split(' ');

            string[] ddd = datenname[0].Split('/');

            string dday = ddd[1].Length > 1 ? ddd[1] : "0" + ddd[1];
            string mmonth = ddd[0].Length > 1 ? ddd[0] : "0" + ddd[0];

            string dddate = mmonth + "/" + dday + "/" + ddd[2];
            string qquery = "select *,case when dtunder IS null then convert(datetime,LEFT(CONVERT(varchar,date_filed,101),10)+' '+[time]) else convert(datetime,dtunder)  end  timeout from Tundertime where LEFT(CONVERT(date,date_filed,101),10)=convert(date,'" + dddate + "') and emp_id=" + e.Row.Cells[24].Text + "  and class='Authorized Undertime'";
            DataTable dtut = dbhelper.getdata(qquery + " and (status like '%Approved%')");
            DataTable dtutapp = dbhelper.getdata(qquery + " and (status like'%for approval%'  or status like'%verification%')");

            DataTable chksc = dbhelper.getdata("select * from MShiftCode where id=" + scid + "  ");
            string ffix;
            if (chksc.Rows[0]["fix"].ToString() == "True")
                ffix = " and day='" + datenname[1] + "'";
            else
                ffix = "";

            DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where id='" + e.Row.Cells[23].Text + "' and status is null");
            LinkButton ff = (LinkButton)e.Row.FindControl("lnk_shift");
            ff.ToolTip = sc.Rows[0]["Remarks"].ToString();

            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + scid + ffix + " ");
            DataTable dthd = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(date,a.date,101),10)=convert(date,'" + dddate + "')");
            DataTable dtotpolicy = dbhelper.getdata("select c.id otrolesid,c.policyid otpolicyid,c.considered,c.roles from memployee a left join MCompany b on a.CompanyId=b.Id left join sys_policy_roles c on b.ot_roles=c.id where a.id=" + e.Row.Cells[24].Text + "");
            DataTable dtchkot = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where ( status like'%for approval%'  or status like'%verification%' or status like'%approved%')  and LEFT(CONVERT(date,date,101),10)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");
            DataTable dtchkmanual = dbhelper.getdata("select *  from Tmanuallogline where  (status like'%for approval%'  or status like'%verification%' or status like'%approved%' ) and LEFT(CONVERT(date,date,101),10)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");
            DataTable dtoffset = dbhelper.getdata("select *  from toffset where  (status like'%for approval%'  or status like'%verification%') and LEFT(CONVERT(date,appliedfrom,101),10)=convert(date,'" + dddate + "') and empid=" + e.Row.Cells[24].Text + "");
            DataTable dtemp = dbhelper.getdata("select *,case when allowoffset is null then 'False' else allowoffset end allowedofset from memployee where  id=" + e.Row.Cells[24].Text + "");
            DataTable dtcs = dbhelper.getdata("select " +
                                        "case when time_in='0' then 0 else convert(datetime,time_in)end time_in, " +
                                        "case when time_out1='0' then 0 else convert(datetime,time_out1)end time_out1,  " +
                                        "case when time_in1='0' then 0 else convert(datetime,time_in1)end time_in1, " +
                                        "case when time_out='0' then 0 else convert(datetime,time_out)end time_out " +
                // "convert(datetime,time_in)time_in,convert(datetime,time_out1)time_out1,convert(datetime,time_in1)time_in1,convert(datetime,time_out)time_out 
           "from tmanuallogline where employeeid='" + e.Row.Cells[24].Text + "' and (status like'%for approval%' or status like'%verification%' ) and CONVERT(date,[date])=convert(date,CONVERT(datetime,'" + dddate + "'))");
            decimal consideredhrs = 0;
            DateTime get1in = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeIn1"].ToString() : "0:00:00");
            DateTime get1out = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeOut1"].ToString() : "0:00:00");
            DateTime get2in = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeIn2"].ToString() : "0:00:00");
            DateTime get2out = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeOut2"].ToString() : "0:00:00");

            string thersnight = "0";
            DataRow[] get_per_emp;
            if (chkdtsc.Rows[0]["TimeOut1"].ToString() == "0:00:00" && chkdtsc.Rows[0]["TimeIn2"].ToString() == "0:00:00")
            {
                if (get1in.ToString().Contains("PM"))
                {
                    if (get2out.ToString().Contains("AM"))
                    {
                        get2out = get2out.AddDays(1);
                        thersnight = "1";
                    }
                }
            }
            else
            {
                if (get1in.ToString().Contains("PM"))
                {
                    if (get1out.ToString().Contains("AM"))
                        thersnight = "1";
                    if (get2in.ToString().Contains("AM"))
                        thersnight = "1";
                    if (get2out.ToString().Contains("AM"))
                        thersnight = "1";
                }
                else if (get1out.ToString().Contains("PM"))
                {
                    if (get2in.ToString().Contains("AM"))
                        thersnight = "1";
                    if (get2out.ToString().Contains("AM"))
                        thersnight = "1";
                }
                else if (get2in.ToString().Contains("PM"))
                {
                    if (get2out.ToString().Contains("AM"))
                        thersnight = "1";
                }
                else if (get1in.ToString().Contains("PM"))
                {
                    if (get2out.ToString().Contains("AM"))
                        thersnight = "1";
                }

            }
            if (dtcs.Rows.Count > 0)
            {
                e.Row.Cells[6].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[7].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[8].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[9].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[6].Text = dtcs.Rows[0]["time_in"].ToString();
                e.Row.Cells[7].Text = dtcs.Rows[0]["time_out1"].ToString();
                e.Row.Cells[8].Text = dtcs.Rows[0]["time_in1"].ToString();
                e.Row.Cells[9].Text = dtcs.Rows[0]["time_out"].ToString();
            }
            if (dtutapp.Rows.Count > 0)
            {
                e.Row.Cells[9].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[9].Text = Convert.ToDateTime(dtutapp.Rows[0]["timeout"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
            }
            DataRow[] getrd = chkdtsc.Select("restday='True'");
            if (getrd.Count() > 0)
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;
                // e.Row.ForeColor = System.Drawing.Color.Yellow;
            }
            if (e.Row.Cells[6].Text.Contains("1/1/1900"))
            {
                e.Row.Cells[6].Text = "--";
            }
            if (e.Row.Cells[7].Text.Contains("1/1/1900"))
            {
                e.Row.Cells[7].Text = "--";
            }
            if (e.Row.Cells[8].Text.Contains("1/1/1900"))
            {
                e.Row.Cells[8].Text = "--";
            }
            if (e.Row.Cells[9].Text.Contains("1/1/1900"))
            {
                e.Row.Cells[9].Text = "--";
            }


            if (getrd.Count() == 0 && (e.Row.Cells[6].Text == "--" || e.Row.Cells[7].Text == "--" || e.Row.Cells[8].Text == "--" || e.Row.Cells[9].Text == "--"))
            {
                //e.Row.BackColor = System.Drawing.Color.LightSkyBlue;
            }
            if (e.Row.Cells[10].Text == "True")
            {
                e.Row.Cells[10].Text = "✓";
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[10].Text = "--";

            if (e.Row.Cells[11].Text == "True")
            {
                e.Row.Cells[11].Text = "✓";
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[11].Text = "--";

            if (e.Row.Cells[12].Text == "True")
            {
                e.Row.Cells[12].Text = "✓";
                e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[12].Text = "--";

            if (e.Row.Cells[13].Text == "True")
            {
                e.Row.Cells[13].Text = "✓";
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[13].Text = "--";

            if (dtoffset.Rows.Count > 0)
            {
                e.Row.Cells[16].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[16].Text = dtoffset.Rows[0]["offsethrs"].ToString();
            }

            //if (dddate == "07/07/2018")
            //{
            //    string hihiss = "s";
            //}
            DataTable chkwv = dbhelper.getdata("select * from TRestdaylogs a where a.status like '%Approved%' and  left(convert(varchar,a.date,101),10)='" + dddate + "'");
            if (dtchkmanual.Rows.Count > 0)
                lnk_tadj.Visible = false;
            if (e.Row.Cells[9].Text == "--")
            {
                lnk_ot.Visible = false;
                lnk_offset.Visible = false;
            }
            else
            {
                string[] SPP = e.Row.Cells[9].Text.Split(' ');
                DateTime dttt = Convert.ToDateTime(dddate).AddDays(1);

                DateTime actout = Convert.ToDateTime(e.Row.Cells[9].Text);
                string hihi = thersnight == "1" ? dttt.ToString("MM/dd/yyyy") : dddate;//SPP[0].ToString()
                DateTime setupout = Convert.ToDateTime(hihi + " " + chkdtsc.Rows[0]["timeout2"].ToString());
                if (actout >= setupout.AddHours(1))
                    lnk_ot.Visible = true;
                //else
                //    lnk_ot.Visible = false;
            }
            if (dtemp.Rows[0]["allowedofset"].ToString() == "True")
            {
                if (decimal.Parse(e.Row.Cells[21].Text) > 0)
                {
                    if (dtut.Rows.Count > 0)
                    {
                        lnk_offset.Visible = true;
                        lnk_ot.Visible = false;
                    }
                }
            }
            if (dtchkot.Rows.Count > 0 || dtoffset.Rows.Count > 0)
            {
                lnk_ot.Visible = false;
                lnk_offset.Visible = false;
            }

            if (e.Row.Cells[4].Text.Contains("Holiday"))
            {
                e.Row.BackColor = System.Drawing.Color.YellowGreen;
            }
        }
    }

    protected void click_adj(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //string[] timein = row.Cells[6].Text.Split(' ');
            //modal_ta.Style.Add("display", "block");
            //string[] ddate = row.Cells[3].Text.Split('-');
            //lbl_date.Text = ddate[0];
            //hdn_scid.Value = row.Cells[23].Text.ToString();
            //DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + hdn_scid.Value + " and a.day='" + DateTime.Parse(ddate[0]).DayOfWeek + "'");
            //nightshift.Value = chkdtsc.Rows[0]["nighshift"].ToString();
            //if (chkdtsc.Rows[0]["mandatorytopunch"].ToString() == "False")
            //{ brek.Visible = false; }

            //txt_in1_date.Text = DateTime.Parse(ddate[0]).ToString("yyyy-MM-dd");
            //txt_out1_date.Text = row.Cells[7].Text.Length > 2 ? DateTime.Parse(row.Cells[7].Text).ToString("yyyy-MM-dd") : "--";
            //txt_in2_date.Text = row.Cells[8].Text.Length > 2 ? DateTime.Parse(row.Cells[8].Text).ToString("yyyy-MM-dd") : "--";
            //txt_out2_date.Text = row.Cells[9].Text.Length > 2 ? DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd") : "--";


            //txt_in1_time.Text = row.Cells[6].Text.Length > 2 ? DateTime.Parse(row.Cells[6].Text).ToString("HH:mm") : "--";
            //txt_out1_time.Text = row.Cells[7].Text.Length > 2 ? DateTime.Parse(row.Cells[7].Text).ToString("HH:mm") : "--";
            //txt_in2_time.Text = row.Cells[8].Text.Length > 2 ? DateTime.Parse(row.Cells[8].Text).ToString("HH:mm") : "--";
            //txt_out2_time.Text = row.Cells[9].Text.Length > 2 ? DateTime.Parse(row.Cells[9].Text).ToString("HH:mm") : "--";
        }
    }

    protected void save_time_adjustment(object sender, EventArgs e)
    {
        //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
        //DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");

        //string in1 = (txt_in1_date.Text + " " + txt_in1_time.Text).Replace(" ", "").Length > 0 ? txt_in1_date.Text + " " + txt_in1_time.Text : "0";
        //string in2 = brek.Visible == false ? "0" : (txt_out1_date.Text + " " + txt_out1_time.Text);
        //string out1 = brek.Visible == false ? "0" : (txt_in2_date.Text + " " + txt_in2_time.Text);
        //string out2 = (txt_out2_date.Text + " " + txt_out2_time.Text).Replace(" ", "").Length > 0 ? txt_out2_date.Text + " " + txt_out2_time.Text : "0";

        //if (chk_input(in1, in2, out1, out2))
        //{
        //    stateclass a = new stateclass();
        //    //if (approver_id.Rows.Count == 0)
        //    //{
        //    //    a.sa = Session["emp_id"].ToString();
        //    //    a.sb = lbl_date.Text;
        //    //    a.sc = in1;//time in 1
        //    //    a.si = in2;// time out 1
        //    //    a.sh = out1;//time in 2
        //    //    a.sd = out2; //time out 2
        //    //    a.se = txt_reason.SelectedValue;
        //    //    a.sf = "For Approval-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString();
        //    //    a.sg = txt_remarks.Text;
        //    //    a.sj = "0";
        //    //    string x = bol.manual_logs(a);
        //    //    dbhelper.getdata("update Tmanuallogline set status='" + "verification-" + DateTime.Now.ToShortDateString().ToString() + "' where id='" + x + "'");
        //    //    if (x == "0")
        //    //        Response.Write("<script>alert('Data is already exist!')</script>");
        //    //    else
        //    //    {
        //    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    if (approver_id.Rows[0]["under_id"].ToString() == "0")
        //    //    {
        //    //        a.sa = Session["emp_id"].ToString();
        //    //        a.sb = lbl_date.Text;
        //    //        a.sc = in1;//time in 1
        //    //        a.si = in2;// time out 1
        //    //        a.sh = out1;//time in 2
        //    //        a.sd = out2; //time out 2
        //    //        a.se = txt_reason.SelectedValue;
        //    //        a.sf = "For Approval-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString();
        //    //        a.sg = txt_remarks.Text;
        //    //        // if (approver_id.Rows.Count == 0)
        //    //        a.sj = "0";
        //    //        // else
        //    //        //  a.sj = approver_id.Rows[0]["under_id"].ToString();
        //    //        string x = bol.manual_logs(a);
        //    //        dbhelper.getdata("update Tmanuallogline set status='" + "verification-" + DateTime.Now.ToShortDateString().ToString() + "' where id='" + x + "'");
        //    //        if (x == "0")
        //    //            Response.Write("<script>alert('Data is already exist!')</script>");
        //    //        else
        //    //        {

        //    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
        //    //        }
        //    //}
        //    //else
        //    //{
        //    a.sa = Session["emp_id"].ToString();
        //    a.sb = lbl_date.Text;
        //    a.sc = in1;//time in 1
        //    a.si = in2;// time out 1
        //    a.sh = out1;//time in 2
        //    a.sd = out2; //time out 2
        //    a.se = txt_reason.SelectedValue;
        //    a.sf = "For Approval";
        //    a.sg = txt_remarks.Text.Replace("'", "");
        //    a.sj = approver_id.Rows[0]["under_id"].ToString();
        //    string x = bol.manual_logs(a);
        //    if (x == "0")
        //        Response.Write("<script>alert('Data is already exist!')</script>");
        //    else
        //    {

        //        function.AddNotification("Time Adjustment Approval", "am", approver_id.Rows[0]["under_userid"].ToString(), x);
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
        //    }

        //}

    }

    protected bool chk_input(string in11, string in22, string out11, string out22)
    {
        bool ret = true;
        //string in1 = in11.Length > 1 ? DateTime.Parse(in11).ToString("yyyy-MM-dd") : "0";
        //string in2 = in22.Length > 1 ? DateTime.Parse(in22).ToString("yyyy-MM-dd") : "0";
        //string out1 = out11.Length > 1 ? DateTime.Parse(out11).ToString("yyyy-MM-dd") : "0";
        //string out2 = out22.Length > 1 ? DateTime.Parse(out22).ToString("yyyy-MM-dd") : "0";
        //if (brek.Visible == false)
        //{
        //    if (in11.Length > 1 && out22.Length > 1)
        //    {
        //        if (DateTime.Parse(out2) < DateTime.Parse(in1))
        //        {
        //            lbl_errmsg.Text = "Time Out 2 must not lesser than Time In 1.";
        //            ret = false;
        //        }
        //        else if (DateTime.Parse(out22) < DateTime.Parse(in11))
        //        {
        //            lbl_errmsg.Text = "Time Out 2 must not lesser than Time In 1.";
        //            ret = false;
        //        }
        //        else if (DateTime.Parse(out2) > DateTime.Parse(in1).AddDays(1))
        //        {
        //            lbl_errmsg.Text = "Invalid Input Date / Time!";
        //            ret = false;
        //        }
        //        else
        //            lbl_errmsg.Text = "";
        //    }
        //    else
        //    {
        //        lbl_errmsg.Text = "Invalid Input";
        //        ret = false;
        //    }
        //}
        //else
        //{
        //    if (in11.Length > 1 && in22.Length > 1 && out11.Length > 1 && out22.Length > 1)
        //    {
        //        if (DateTime.Parse(in2) < DateTime.Parse(in1) || DateTime.Parse(out1) < DateTime.Parse(in1) || DateTime.Parse(out2) < DateTime.Parse(in1))
        //        {
        //            lbl_errmsg.Text = "Time Out 1,Time in 2 and Time Out 2 must not lesser than Time In 1.";
        //            ret = false;
        //        }
        //        else if (DateTime.Parse(out11) < DateTime.Parse(in11) || DateTime.Parse(out22) < DateTime.Parse(in22))
        //        {
        //            lbl_errmsg.Text = "Time Out 1,Time in 2 and Time Out 2 must not lesser than Time In 1.";
        //            ret = false;
        //        }
        //        else if (DateTime.Parse(in2) > DateTime.Parse(in1).AddDays(1) || DateTime.Parse(out1) > DateTime.Parse(in1).AddDays(1) || DateTime.Parse(out2) > DateTime.Parse(in1).AddDays(1))
        //        {
        //            lbl_errmsg.Text = "Invalid Input Date / Time!";
        //            ret = false;
        //        }
        //        else
        //            lbl_errmsg.Text = "";
        //    }
        //    else
        //    {
        //        lbl_errmsg.Text = "Invalid Input";
        //        ret = false;
        //    }
        //}
        return ret;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    protected void add_ot(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //lbl_ddate.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
            //TextBox1.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
            //TextBox2.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
            //modal_ot.Style.Add("display", "block");
            //hdn_scid.Value = row.Cells[23].Text.ToString();
        }
    }

    protected void save_ot_request(object sender, EventArgs e)
    {
       // DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + lbl_ddate.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification' )");
        //if (tdcheck.Rows.Count == 0)
        //{
            //DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + hdn_scid.Value + " and a.day='" + DateTime.Parse(lbl_ddate.Text).DayOfWeek + "'");
            //DateTime getotout = Convert.ToDateTime(TextBox1.Text + " " + TextBox2.Text);
            //DateTime otin = Convert.ToDateTime(lbl_ddate.Text + " " + chkdtsc.Rows[0]["timeout2"].ToString());
            //string query = "select CONVERT(float, datediff(minute, '" + otin.TimeOfDay + "','" + getotout.TimeOfDay + "' )) / 60 as time_diff ";
            //DataTable dtt = new DataTable();
            //dtt = dbhelper.getdata(query);
            //string nighthrs = getnightdif.getnight(otin.ToString(), getotout.ToString(), "dtr");// (getotout - getshiftout).TotalHours.ToString();
            //decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString()) - decimal.Parse(nighthrs);

            ////DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            //DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");

            //DataTable dtinsertotline = new DataTable();
            //dtinsertotline = dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','For Approval',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,'" + approver_id.Rows[0]["under_id"].ToString() + "' ) select scope_identity() id ");



            //update TOverTimeLine set status='" + "For Verification-" + DateTime.Now.ToShortDateString().ToString() + "' where id=@id set id

            //if (approver_id.Rows.Count == 0)
            //{
            //  dtinsertotline= dbhelper.getdata("declare @id int insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,0 ) set @id=scope_identity() update TOverTimeLine set status='" + "For Verification-" + DateTime.Now.ToShortDateString().ToString() + "' where id=@id set id");
            //}
            //else
            //{
            //    if (approver_id.Rows[0]["under_id"].ToString() == "0")
            //    {
            //       dtinsertotline= dbhelper.getdata("declare @id int insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,0 ) set @id=scope_identity() update TOverTimeLine set status='" + "For Verification-" + DateTime.Now.ToShortDateString().ToString() + "' where id=@id");
            //    }
            //    else
            //    {
            //       dtinsertotline= dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,'" + approver_id.Rows[0]["under_id"].ToString() + "' )");

            //    }
            //}
            //dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,'" + aw + "' )");
            //function.AddNotification("Overtime Approval", "ao", approver_id.Rows[0]["under_userid"].ToString(), dtinsertotline.Rows[0]["id"].ToString());
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_OT'", true);


        //}
        //else
        //    Response.Write("<script>alert('Input Data is already Exist!')</script>");


    }
    protected void add_offset(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //DataTable dt = dbhelper.getdata("select * from memployee where id=" + drop_emp.SelectedValue + "");
            //DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
            //txt_off_date.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
            //modal_os.Style.Add("display", "block");
            //string qq = "";
            //DataTable dtAuthorizedundertime = dbhelper.getdata("select left(convert(varchar,a.date_filed,101),10)df from Tundertime a " +
            //"where a.emp_id=" + Session["emp_id"] + " and a.class='Authorized Undertime' and a.status like '%Approved%' and a.date_filed between '" + dtperiod.Rows[0]["ffrom"].ToString() + "' and  DATEADD(DAY,1,'" + dtperiod.Rows[0]["tto"].ToString() + "') " +
            //" and " +
            //"(select COUNT(*) from toffset where empid=" + Session["emp_id"] + " and convert(date,appliedto)=convert(date,a.date_filed) and (status like '%Approved%' or status like '%For Approval%'))=0");
            //ddl_doffset.Items.Clear();
            //ddl_doffset.Items.Add(new ListItem("None", "0"));
            //foreach (DataRow dr in dtAuthorizedundertime.Rows)
            //{
            //    ddl_doffset.Items.Add(new ListItem(dr["df"].ToString(), dr["df"].ToString()));
            //}
            //DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + row.Cells[23].Text.ToString() + " and a.day='" + DateTime.Parse(txt_off_date.Text).DayOfWeek + "'");
            //DateTime getotout = Convert.ToDateTime(row.Cells[9].Text);
            //DateTime otin = Convert.ToDateTime(txt_off_date.Text + " " + chkdtsc.Rows[0]["timeout2"].ToString());

            //DataTable dtt = dbhelper.getdata("select CONVERT(float, datediff(minute, '" + otin + "','" + getotout.ToString() + "' )) / 60 as time_diff ");
            //decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString());


            //txt_off_out.Text = row.Cells[9].Text;
            //txt_off_hrs.Text = string.Format("{0:n2}", totalreghours);

        }
    }
    protected void save_off_set(object sender, EventArgs e)
    {
        if (checkoffsave())
        {
            //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            //DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
            //string ret;
            //using (SqlConnection con = new SqlConnection(dbconnection.conn))
            //{
            //    using (SqlCommand cmd = new SqlCommand("save_off_set", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@appliedfrom", SqlDbType.VarChar, 50).Value = txt_off_date.Text;
            //        cmd.Parameters.Add("@appliedto", SqlDbType.VarChar, 50).Value = ddl_doffset.SelectedValue;
            //        cmd.Parameters.Add("@offsethrs", SqlDbType.VarChar, 50).Value = txt_off_hrs.Text;
            //        cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
            //        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
            //        cmd.Parameters.Add("@approverid", SqlDbType.Int).Value = approver_id.Rows[0]["under_id"].ToString();
            //        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
            //        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
            //        con.Open();

            //        cmd.ExecuteNonQuery();
            //        ret = cmd.Parameters["@out"].Value.ToString();
            //        con.Close();
            //    }
            //}

            //function.AddNotification("Time Adjustment Approval", "offset", approver_id.Rows[0]["under_userid"].ToString(), ret);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
        }
    }

    protected bool checkoffsave()
    {
        bool err = true;

        //if (txt_off_date.Text.Length == 0)
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Date Input!";
        //}
        //else if (txt_off_out.Text.Length == 0)
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Time Out Input!";
        //}
        //else if (txt_off_hrs.Text.Length == 0)
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Offset Hrs!";
        //}
        //else if (ddl_doffset.SelectedValue == "0")
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Date Offset!";
        //}
        //else if (txt_off_reason.Text.Length == 0)
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Reason!";
        //}
        //else
        //    lbl_off_err.Text = "";

        return err;

    }
}