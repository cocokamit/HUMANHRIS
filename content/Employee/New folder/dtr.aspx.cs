using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Human;

public partial class content_Employee_dtr : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
           Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            pay_range();
            disp();
            loadable();
            loadsc();
        }
    }

    protected void pay_range()
    {
        DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto from reisol_range where action is null order by dfrom desc";

        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
        ddl_pay_range.Items.Add(new ListItem("Payrol Period","0"));
        foreach(DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }
    }
    protected void btn_go(object sender, EventArgs e)
    {
        hdn_total_tempot.Value = "";
        if (ddl_pay_range.SelectedValue != "0")
        {
            string[] hhh = ddl_pay_range.Text.Split('-');
            txt_from.Text = hhh[0].ToString();
            txt_to.Text = hhh[1].ToString();
        }
        disp();
    }
    protected void disp()
    {
        string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
        DataTable dt = dbhelper.getdata("select * from memployee where id="+Session["emp_id"].ToString()+"");
        DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
        
        string []gggg=ddl_pay_range.Text.Split('-');
        string from = txt_from.Text.Length > 0 ? txt_from.Text : month + "/01/" + DateTime.Now.Year;
        string to = txt_to.Text.Length > 0 ? txt_to.Text : month + "/" + day + "/" + DateTime.Now.Year;
        if (ddl_pay_range.SelectedValue == "0")
        {
            if (dtperiod.Rows.Count > 0)
            {
                from = dtperiod.Rows[0]["ffrom"].ToString();
                to = dtperiod.Rows[0]["tto"].ToString();
            }
        }
        else
        {
            from = gggg[0].ToString();
            to = gggg[1].ToString();
        }
        //if (txt_to.Text.Length == 0 && txt_to.Text.Length == 0)
        //{
            //if (dtperiod.Rows.Count > 0)
            //{
            //    from = dtperiod.Rows[0]["ffrom"].ToString();
            //    to = dtperiod.Rows[0]["tto"].ToString();
            //}
        //}

        DataTable dtr = Core.DTRF(from, to, "KIOSK_" + Session["emp_id"].ToString());

        decimal t_temp_ot = 0;
        if (hdn_total_tempot.Value.Length == 0)
        {
            foreach (DataRow dr in dtr.Rows)
            {
                t_temp_ot = t_temp_ot + decimal.Parse(dr["tempot"].ToString());
                //hdn_total_tempot.Value = (hdn_total_tempot.Value.Length == 0 ? 0 : decimal.Parse(hdn_total_tempot.Value) + decimal.Parse(dr["tempot"].ToString())).ToString();
            }
        }

        hdn_total_tempot.Value = t_temp_ot.ToString();
        grid_item.DataSource = dtr;
        grid_item.DataBind(); 
    }
    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_ot = (LinkButton)e.Row.FindControl("lnk_ot");
            LinkButton lnk_tadj = (LinkButton)e.Row.FindControl("lnk_tadj");
            LinkButton lnk_offset = (LinkButton)e.Row.FindControl("lnk_offset");
            LinkButton lnk_status = (LinkButton)e.Row.FindControl("lnk_status");
            LinkButton lnk_cws = (LinkButton)e.Row.FindControl("lnk_cws");

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
            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + scid + ffix + " ");
            DataTable dthd = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(date,a.date,101),10)=convert(date,'" + dddate + "')");
            DataTable dtotpolicy = dbhelper.getdata("select c.id otrolesid,c.policyid otpolicyid,c.considered,c.roles from memployee a left join MCompany b on a.CompanyId=b.Id left join sys_policy_roles c on b.ot_roles=c.id where a.id=" + e.Row.Cells[24].Text + "");
            DataTable dtchkot = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where ( status like'%for approval%'  or status like'%verification%' or status like'%approved%')  and LEFT(CONVERT(date,date,101),10)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");

            DataTable dtchkcws = dbhelper.getdata("select *  from temp_shiftcode where  (status like'%for approval%'  or status like'%verification%' or status like'%approved%' ) and LEFT(CONVERT(date,date_change,101),10)=convert(date,'" + dddate + "') and emp_id=" + e.Row.Cells[24].Text + "");

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

            if (e.Row.Cells[11].Text == "True" || e.Row.Cells[13].Text == "True")
            {
                e.Row.Cells[11].Text = "✓";
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[11].Text = "--";

            //if (e.Row.Cells[12].Text == "True" )
            //{
            //    e.Row.Cells[12].Text = "✓";
            //    e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
            //}
            //else
            //    e.Row.Cells[12].Text = "--";

            //if (e.Row.Cells[13].Text == "True")
            //{
            //    e.Row.Cells[13].Text = "✓";
            //    e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            //}
            //else
            //    e.Row.Cells[13].Text = "--";

            if (dtoffset.Rows.Count > 0)
            {
                e.Row.Cells[16].BackColor = System.Drawing.Color.Magenta;
                e.Row.Cells[16].Text = dtoffset.Rows[0]["offsethrs"].ToString();
            }

            DataTable chkwv = dbhelper.getdata("select * from TRestdaylogs a where a.status like '%Approved%' and  left(convert(varchar,a.date,101),10)='" + dddate + "'");





            string opa = "select b.DateStart,b.DateEnd, * from TPayroll a " +
                                "left join tdtr b on a.DTRId=b.Id " +
                                "where a.status is null and CONVERT(date,b.dateend)>= convert(date,'" + dddate + "') and b.status is null and b.PayrollGroupId=" + dtemp.Rows[0]["PayrollGroupId"].ToString() + "";
            DataTable dtopa = dbhelper.getdata(opa);
            if (dtopa.Rows.Count > 1)
            {
                lnk_tadj.Visible = false;
                lnk_ot.Visible = false;
                lnk_offset.Visible = false;
                lnk_cws.Visible = false;
            }
            else
            {

                if (dtchkmanual.Rows.Count > 0)
                {
                    lnk_tadj.Enabled = false;
                    lnk_tadj.ForeColor = System.Drawing.Color.Gray;
                }
                if (dtchkcws.Rows.Count > 0)
                {
                    lnk_cws.Enabled = false;
                    lnk_cws.ForeColor = System.Drawing.Color.Gray;
                     
                }
                if (e.Row.Cells[9].Text == "--")
                {

                    lnk_ot.Enabled = false;
                    lnk_ot.ForeColor = System.Drawing.Color.Gray;
                    //lnk_ot.Visible = false;
                    
                    
                    lnk_offset.Visible = false;
                }
                else
                {
                    string[] SPP = e.Row.Cells[9].Text.Split(' ');
                    DateTime dttt = Convert.ToDateTime(dddate).AddDays(1);

                    DateTime actout = Convert.ToDateTime(e.Row.Cells[9].Text);
                    string hihi = thersnight == "1" ? dttt.ToString("MM/dd/yyyy") : dddate;//SPP[0].ToString()
                    DateTime setupout = Convert.ToDateTime(hihi + " " + chkdtsc.Rows[0]["timeout2"].ToString());
                    DataTable dtcompdetails = dbhelper.getdata("select * from mcompany where id=" + dtemp.Rows[0]["companyid"].ToString() + "");
                    //if (dtcompdetails.Rows[0]["ot_roles"].ToString() == "2" || dtcompdetails.Rows[0]["ot_roles"].ToString() == "1")
                    //{
                        if (actout >= setupout.AddMinutes(30))
                        {
                            //if (dtemp.Rows[0]["DivisionId"].ToString() == "3")
                            //{

                                lnk_ot.Visible = true;
                            //}
                        }
                    //}
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

            }
            if (e.Row.Cells[25].Text.Contains("Holiday"))
            {
                e.Row.Cells[25].BackColor = System.Drawing.Color.YellowGreen;
            }
            if (e.Row.Cells[25].Text == "RD")
            {

                e.Row.Cells[25].BackColor = System.Drawing.Color.Yellow;
                // e.Row.ForeColor = System.Drawing.Color.Yellow;
            }
            if (lnk_status.Text == "RD")
            {
                lnk_status.Style.Add("background-color", "yellow");

                lnk_status.Style.Add("border", "1px");
                lnk_status.Style.Add("border-radius", "5px");
            }
            if (lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("RH"))
            {
                lnk_status.Style.Add("background-color", "yellowgreen");

                lnk_status.Style.Add("border", "1px");
                lnk_status.Style.Add("border-radius", "5px");
            }
            if (e.Row.Cells[26].Text == "True")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void click_adj(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string[] timein = row.Cells[6].Text.Split(' ');
            modal_ta.Style.Add("display", "block");
            string[] ddate = row.Cells[3].Text.Split('-');
            lbl_date.Text = ddate[0];
            hdn_scid.Value = row.Cells[23].Text.ToString();
            DataTable sc = dbhelper.getdata("select * from MShiftCode where id=" + hdn_scid.Value + " and fix=1");
            string ffix = "";
            if (sc.Rows.Count > 0)
                ffix = "and a.day='" + DateTime.Parse(ddate[0]).DayOfWeek + "'";

            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + hdn_scid.Value + ffix + "  ");
            nightshift.Value = chkdtsc.Rows[0]["nighshift"].ToString();
            if (chkdtsc.Rows[0]["mandatorytopunch"].ToString() == "False")
            { brek.Visible=false;}

            txt_in1_date.Text =  DateTime.Parse(ddate[0]).ToString("yyyy-MM-dd");
            txt_out1_date.Text = row.Cells[7].Text.Length>2?DateTime.Parse(row.Cells[7].Text).ToString("yyyy-MM-dd"):"--";
            txt_in2_date.Text = row.Cells[8].Text.Length > 2 ? DateTime.Parse(row.Cells[8].Text).ToString("yyyy-MM-dd") : "--";
            txt_out2_date.Text = row.Cells[9].Text.Length > 2 ? DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd") : "--";


            txt_in1_time.Text = row.Cells[6].Text.Length > 2 ? DateTime.Parse(row.Cells[6].Text).ToString("HH:mm") : "--";
            txt_out1_time.Text = row.Cells[7].Text.Length > 2 ? DateTime.Parse(row.Cells[7].Text).ToString("HH:mm") : "--";
            txt_in2_time.Text = row.Cells[8].Text.Length > 2 ? DateTime.Parse(row.Cells[8].Text).ToString("HH:mm") : "--";
            txt_out2_time.Text = row.Cells[9].Text.Length > 2 ? DateTime.Parse(row.Cells[9].Text).ToString("HH:mm") : "--";
        }
    }
    protected void save_time_adjustment(object sender, EventArgs e)
    {
        int scid=0;
        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
      
        string in1 = (txt_in1_date.Text + " " + txt_in1_time.Text).Replace(" ", "").Length > 0 ? txt_in1_date.Text + " " + txt_in1_time.Text : "0";
        string in2 = brek.Visible==false?"0":(txt_out1_date.Text + " " + txt_out1_time.Text);
        string out1 = brek.Visible == false ?"0":(txt_in2_date.Text + " " + txt_in2_time.Text);
        string out2 = (txt_out2_date.Text + " " + txt_out2_time.Text).Replace(" ", "").Length > 0 ? txt_out2_date.Text + " " + txt_out2_time.Text : "0";
        DataTable dtemp=dbhelper.getdata("select * from memployee where id="+Session["emp_id"].ToString()+"");
        DataTable changeshift=dbhelper.getdata("select * from TChangeShiftLine where EmployeeId="+Session["emp_id"].ToString()+" and CONVERT(date,date)=CONVERT(date,'"+lbl_date.Text+"')");
        scid = changeshift.Rows.Count > 0 ? int.Parse(changeshift.Rows[0]["Shiftcodeid"].ToString()) : int.Parse(dtemp.Rows[0]["Shiftcodeid"].ToString());

        DataTable sc = dbhelper.getdata("select * from MShiftCodeDay where shiftcodeid="+scid+"");
        
        DataTable daytype = dbhelper.getdata("select *, a.id,a.DayType,a.workingdays,a.RestdayDays,case when b.BranchId is null then '0' else b.BranchId end BranchId ,case when left(convert(varchar,b.Date,101),10) is null then '0' else left(convert(varchar,b.Date,101),10) end datte from MDayType a left join MDayTypeDay b on a.Id=b.DayTypeId where b.status is null order by a.id asc ");
        DataRow[] dtgetallDayType = daytype.Select("datte='" + lbl_ddate.Text + "' and BranchId='" + dtemp.Rows[0]["branchid"].ToString() + "'");

        decimal daymultiplier = 0;
        decimal nightmultiplier = 0;
        decimal otdaymultiplier = 0;
        decimal otnightmultiplier = 0;




        if (hdn_rd.Value.Contains("Restday"))
        {
            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["RestdayDays"].ToString()) : decimal.Parse(daytype.Rows[0]["RestdayDays"].ToString());
            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightrestdaydays"].ToString()) : decimal.Parse(daytype.Rows[0]["nightrestdaydays"].ToString());
            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTrestdaydays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTrestdaydays"].ToString());
            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightRestdayDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightRestdayDays"].ToString());
        }
        else
        {
            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["workingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["workingdays"].ToString());
            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["nightworkingdays"].ToString());
            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTworkingdays"].ToString());
            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightWorkingDays"].ToString());
        }
        if (chk_input(in1, in2, out1, out2))
        {
            stateclass a = new stateclass();
            a.sa = Session["emp_id"].ToString();
            a.sb = lbl_date.Text;
            a.sc = in1;//time in 1
            a.si = in2;//time out 1
            a.sh = out1;//time in 2
            a.sd = out2;//time out 2
            a.se = txt_reason.SelectedValue;
            a.sf = "For Approval";
            a.sg = txt_remarks.Text.Replace("'", "");
            a.sj = approver_id.Rows[0]["under_id"].ToString();
            
            DateTime dtout = Convert.ToDateTime(out2);
            if (sc.Rows[0]["timein1"].ToString().Contains("PM") && sc.Rows[0]["timeout2"].ToString().Contains("AM"))
                dtout = Convert.ToDateTime(lbl_date.Text + " " + sc.Rows[0]["timeout2"].ToString()).AddDays(1);
            if (Convert.ToDateTime(out2) < dtout)
                dtout = Convert.ToDateTime(out2);

            string x = bol.manual_logs(a);
            if (x == "0")
                Response.Write("<script>alert('Data is already exist!')</script>");
            else
            {
                string query = "select  (CONVERT(float, datediff(minute, '" + in1 + "','" + dtout + "' )) / 60 ) - " + sc.Rows[0]["nightbreakhours"].ToString() + "  as time_diff ";
                DataTable comhrs = dbhelper.getdata(query);
                decimal hrsss = decimal.Parse(comhrs.Rows[0]["time_diff"].ToString()) > decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) ? decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) : decimal.Parse(comhrs.Rows[0]["time_diff"].ToString());
                // if (getotout > otin)
              

                DateTime JJ = Convert.ToDateTime(lbl_date.Text + " 10:00 PM");
                decimal night = 0;
                if (dtout > JJ)
                {

                    night = decimal.Parse((getnightdif.getnight(in1.ToString(), dtout.ToString(), "dtr")).ToString());
                    night = night > 0 ? night : 0;
                }
                dbhelper.getdata("update Tmanuallogline set hourlyrate='" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "',regmultiplier='" + daymultiplier + "', nightmultiplier='"+nightmultiplier+"',hrs='" + (hrsss - night) + "',nighthrs='" + night + "' where id=" + x + "");
                function.AddNotification("Time Adjustment Approval", "am", approver_id.Rows[0]["under_userid"].ToString(), x);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
            }
        }
        
    }
    protected bool chk_input(string in11,string in22,string out11,string out22)
    {
        bool ret = true;
        string in1 =in11.Length>1?DateTime.Parse(in11).ToString("yyyy-MM-dd"):"0";
        string in2 =in22.Length>1?DateTime.Parse(in22).ToString("yyyy-MM-dd"):"0";
        string out1 =out11.Length>1?DateTime.Parse(out11).ToString("yyyy-MM-dd"):"0";
        string out2 =out22.Length>1?DateTime.Parse(out22).ToString("yyyy-MM-dd"):"0";
        if(brek.Visible==false)
        {
            if (in11.Length > 1 && out22.Length > 1)
            {
                if (DateTime.Parse(out2)< DateTime.Parse(in1))
                {
                    lbl_errmsg.Text = "Time Out 2 must not lesser than Time In 1.";
                    ret = false;
                }
                else if (DateTime.Parse(out22) < DateTime.Parse(in11))
                {
                    lbl_errmsg.Text = "Time Out 2 must not lesser than Time In 1.";
                    ret = false;
                }
                else if (DateTime.Parse(out2) > DateTime.Parse(in1).AddDays(1))
                {
                    lbl_errmsg.Text = "Invalid Input Date / Time!";
                    ret = false;
                }
                else
                    lbl_errmsg.Text = "";
            }
            else
            {
                lbl_errmsg.Text = "Invalid Input";
                ret = false;
            }
        }
        else
        {
            if (in11.Length > 1 && in22.Length > 1 && out11.Length > 1 && out22.Length > 1)
            {
                if (DateTime.Parse(in2) < DateTime.Parse(in1) || DateTime.Parse(out1) < DateTime.Parse(in1) || DateTime.Parse(out2) < DateTime.Parse(in1))
                {
                    lbl_errmsg.Text = "Time Out 1,Time in 2 and Time Out 2 must not lesser than Time In 1.";
                    ret = false;
                }
                else if (DateTime.Parse(out11) < DateTime.Parse(in11) ||  DateTime.Parse(out22) < DateTime.Parse(in22))
                {
                    lbl_errmsg.Text = "Time Out 1,Time in 2 and Time Out 2 must not lesser than Time In 1.";
                    ret = false;
                }
                else if (DateTime.Parse(in2) > DateTime.Parse(in1).AddDays(1) || DateTime.Parse(out1) > DateTime.Parse(in1).AddDays(1) || DateTime.Parse(out2) > DateTime.Parse(in1).AddDays(1))
                {
                    lbl_errmsg.Text = "Invalid Input Date / Time!";
                    ret = false;
                }
                else
                    lbl_errmsg.Text = "";
            }
            else
            {
                lbl_errmsg.Text = "Invalid Input";
                ret = false;
            }
        }  
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
            DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
            DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
            string[] pay_rangee = ddl_pay_range.Text.Split('-');
            hdn_rd.Value=row.Cells[25].Text;
            string from="";
            string to="";
            if (ddl_pay_range.SelectedValue == "0")
            {
                if (dtperiod.Rows.Count > 0)
                {
                    from = dtperiod.Rows[0]["ffrom"].ToString();
                    to = dtperiod.Rows[0]["tto"].ToString();
                }
            }
            else
            {
                from = pay_rangee[0].ToString();
                to = pay_rangee[1].ToString();
            }
            string query = "select " +
            "(select case when SUM(offsethrs) is null then 0 else SUM(offsethrs) end from toffset where empid=" + row.Cells[24].Text.ToString() + " and convert(DATE,appliedfrom)>='" + from.ToString() + "' and  convert(DATE,appliedfrom)<='" + to.ToString() + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) " +
            " + " +
            "(select case when sum(overtimehours + overtimenighthours) is null then 0 else sum(overtimehours + overtimenighthours) end from tovertimeline where employeeid=" + row.Cells[24].Text.ToString() + " and convert(date,[date])>='" + from.ToString() + "' and convert(date,[date])<='" + to.ToString() + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) t_hrs ";
            DataTable dttt = dbhelper.getdata(query);
            lbl_teh_ot.Text = (decimal.Parse(hdn_total_tempot.Value) - decimal.Parse(dttt.Rows[0]["t_hrs"].ToString())).ToString();
            string hehe = lbl_teh_ot.Text.Length > 0 ? lbl_teh_ot.Text : "0.00";
            lbl_ddate.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
            TextBox1.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
            TextBox2.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
            modal_ot.Style.Add("display", "block");
            hdn_scid.Value = row.Cells[23].Text.ToString();
            if (decimal.Parse(hehe) < 1)
            {
                Button2.Visible = false;
            }
        }
   }
    protected void save_ot_request(object sender, EventArgs e)
    {
        DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + lbl_ddate.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification' )");
        DataTable dtemp=dbhelper.getdata("select * from memployee where id="+Session["emp_id"].ToString()+"");

        DataTable daytype = dbhelper.getdata("select *, a.id,a.DayType,a.workingdays,a.RestdayDays,case when b.BranchId is null then '0' else b.BranchId end BranchId ,case when left(convert(varchar,b.Date,101),10) is null then '0' else left(convert(varchar,b.Date,101),10) end datte from MDayType a left join MDayTypeDay b on a.Id=b.DayTypeId where b.status is null order by a.id asc ");
        DataRow[] dtgetallDayType = daytype.Select("datte='" + lbl_ddate.Text + "' and BranchId='" + dtemp.Rows[0]["branchid"].ToString() + "'");

        decimal daymultiplier = 0;
        decimal nightmultiplier = 0;
        decimal otdaymultiplier = 0;
        decimal otnightmultiplier = 0;
        if (hdn_rd.Value.Contains("Restday"))
        {
            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["RestdayDays"].ToString()) : decimal.Parse(daytype.Rows[0]["RestdayDays"].ToString());
            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightrestdaydays"].ToString()) : decimal.Parse(daytype.Rows[0]["nightrestdaydays"].ToString());
            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTrestdaydays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTrestdaydays"].ToString());
            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightRestdayDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightRestdayDays"].ToString());
        }
        else
        {
            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["workingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["workingdays"].ToString());
            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["nightworkingdays"].ToString());
            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTworkingdays"].ToString());
            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightWorkingDays"].ToString());
        }
        if (tdcheck.Rows.Count == 0)
        {
            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + hdn_scid.Value + " and a.day='" + DateTime.Parse(lbl_ddate.Text).DayOfWeek + "'");
            DateTime otin = Convert.ToDateTime(lbl_ddate.Text + " " + chkdtsc.Rows[0]["timeout2"].ToString());
            DateTime getotout = Convert.ToDateTime(TextBox1.Text + " " + TextBox2.Text);
            if (chkdtsc.Rows[0]["timein1"].ToString().Contains("PM") && chkdtsc.Rows[0]["timeout2"].ToString().Contains("AM"))
            otin = Convert.ToDateTime(TextBox1.Text + " " + chkdtsc.Rows[0]["timeout2"].ToString());

            string query = "select CONVERT(float, datediff(minute, '" + otin + "','" + getotout + "' )) / 60 as time_diff ";
            DataTable dtt = new DataTable();
            dtt = dbhelper.getdata(query);
            string nighthrs = getnightdif.getnight(otin.ToString(), getotout.ToString(), "dtr");// (getotout - getshiftout).TotalHours.ToString();
            string totalreghours = (decimal.Parse(dtt.Rows[0]["time_diff"].ToString()) - decimal.Parse(nighthrs)).ToString();
            
            DataTable dtcompdetails=dbhelper.getdata("select * from mcompany where id="+dtemp.Rows[0]["companyid"].ToString()+"");
            string[] nighthours = nighthrs.Split('.');
            string[] totalreghrs = totalreghours.ToString().Split('.');
            if (dtcompdetails.Rows[0]["ot_roles"].ToString() == "2")
            {
                nighthrs = nighthours[0];
                totalreghours=totalreghrs[0];
            }

            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable dtinsertotline = new DataTable();
            string queryyy = "insert into TOverTimeLine ([OverTimeId],[EmployeeId],[Date],[OvertimeHours],[OvertimeNightHours],[OvertimeLimitHours],[Remarks],[status],[ifdone],[dtr_id],[sysdate],[time_in],[time_out],[overtimehoursapp],[overtimenighthoursapp],[approver_id],regmultiplier,nightmultiplier,hourlyrate) values(NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_otremarks.Text.Replace("'", "") + "','For Approval',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,'" + approver_id.Rows[0]["under_id"].ToString() + "','" + otdaymultiplier + "','" + otnightmultiplier + "','" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "' ) select scope_identity() id";
            dtinsertotline = dbhelper.getdata(queryyy);
                
            function.AddNotification("Overtime Approval", "ao", approver_id.Rows[0]["under_userid"].ToString(), dtinsertotline.Rows[0]["id"].ToString());
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_OT'", true);
        }
        else
            Response.Write("<script>alert('Input Data is already Exist!')</script>");
    }
    protected void add_offset(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
            DataTable dtperiod = adjustdtrformat.payrollperiod("all",dt.Rows[0]["payrollgroupid"].ToString());
            string query = "select " +
            "(select case when SUM(offsethrs) is null then 0 else SUM(offsethrs) end from toffset where empid=" + row.Cells[24].Text.ToString() + " and convert(DATE,appliedfrom)>='" + dtperiod.Rows[0]["ffrom"].ToString() + "' and  convert(DATE,appliedfrom)<='" + dtperiod.Rows[0]["tto"].ToString() + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) " + 
            " + " +
            "(select case when sum(overtimehours + overtimenighthours) is null then 0 else sum(overtimehours + overtimenighthours) end from tovertimeline where employeeid=" + row.Cells[24].Text.ToString() + " and convert(date,[date])>='" + dtperiod.Rows[0]["ffrom"].ToString() + "' and convert(date,[date])<='" + dtperiod.Rows[0]["tto"].ToString() + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) t_hrs ";
            DataTable dttt = dbhelper.getdata(query);
            lbl_teh.Text = (decimal.Parse(hdn_total_tempot.Value) - decimal.Parse(dttt.Rows[0]["t_hrs"].ToString())).ToString();
            txt_off_date.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
            modal_os.Style.Add("display", "block");
            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + row.Cells[23].Text.ToString() + " and a.day='" + DateTime.Parse(txt_off_date.Text).DayOfWeek + "'");
            DateTime getotout = Convert.ToDateTime(row.Cells[9].Text);
            //DateTime otin = Convert.ToDateTime(txt_off_date.Text + " " + chkdtsc.Rows[0]["timeout2"].ToString());

            //DataTable dtt = dbhelper.getdata("select CONVERT(float, datediff(minute, '" + otin + "','" + getotout.ToString() + "' )) / 60 as time_diff ");
            //decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString());
            decimal offhrs = 0;
            string hehe = lbl_teh.Text.Length > 0 ? lbl_teh.Text : "0.00";
            if (decimal.Parse(hehe) > 0)
            {
                if (decimal.Parse(lbl_teh.Text) >= decimal.Parse(row.Cells[21].Text))
                    offhrs = decimal.Parse(row.Cells[21].Text);
                else
                    offhrs = decimal.Parse(lbl_teh.Text);
                Button3.Visible = true;
            }
            else
                Button3.Visible = false;

            txt_off_out.Text = row.Cells[9].Text;
            txt_off_hrs.Text = string.Format("{0:n2}", offhrs);
           


        }
    }
    protected void save_off_set(object sender, EventArgs e)
    {
        if (checkoffsave())
        {
            //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
            string ret;
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("save_off_set", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@appliedfrom", SqlDbType.VarChar, 50).Value = txt_off_date.Text;
                    //cmd.Parameters.Add("@appliedto", SqlDbType.VarChar, 50).Value = ddl_doffset.SelectedValue;
                    cmd.Parameters.Add("@offsethrs", SqlDbType.VarChar, 50).Value = txt_off_hrs.Text;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@approverid", SqlDbType.Int).Value = approver_id.Rows[0]["under_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }

            function.AddNotification("Time Adjustment Approval", "offset", approver_id.Rows[0]["under_userid"].ToString(), ret);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_DTR'", true);
        }
    }
    protected bool checkoffsave()
    {
        bool err = true;
        if (txt_off_date.Text.Length == 0)
        {
            err = false;
            lbl_off_err.Text = "Invalid Date Input!";
        }
        else if (txt_off_out.Text.Length == 0)
        {
            err = false;
            lbl_off_err.Text = "Invalid Time Out Input!";
        }
        else if (txt_off_hrs.Text.Length == 0)
        {
            err = false;
            lbl_off_err.Text = "Invalid Offset Hrs!";
        }
        //else if (ddl_doffset.SelectedValue == "0")
        //{
        //    err = false;
        //    lbl_off_err.Text = "Invalid Date Offset!";
        //}
        else if (txt_off_reason.Text.Length == 0)
        {
            err = false;
            lbl_off_err.Text = "Invalid Reason!";
        }
        else
            lbl_off_err.Text = "";
        return err;
       
    }
    protected void loadable()
    {

        //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
        DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where status is null");

        foreach (DataRow dr in sc.Rows)
        {
            ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
            item.Attributes.Add("title", dr["remarks"].ToString());

            //li.Attributes["title"] = li.Text;
            ddl_shiftcode.Items.Add(item);

        }
    }
    protected void click_cws(object sender, EventArgs e)
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
            loadable();
            modal_cws.Style.Add("display", "block");






            dd.Value = row.Cells[6].Text;


            string[] aw = row.Cells[3].Text.Split('-');
            txt_date.Text = aw[0];


            string aa = "select * from TchangeshiftLine where [date]=left(convert(varchar,'" + txt_date.Text + "',101),10) and employeeId='" + Session["emp_id"].ToString() + "' and ShiftCodeId='" + row.Cells[23].Text + "'  ";
            DataTable hh = new DataTable();
            hh = dbhelper.getdata(aa);

            if (hh.Rows.Count == 0)
                key.Value = row.Cells[23].Text;
            else
                key.Value = hh.Rows[0]["id"].ToString();
            ddl_shiftcode.SelectedValue = row.Cells[23].Text;
        }
    }
    protected void click_save_changeshift(object sender, EventArgs e)
    {
        //DataTable dt = dbhelper.getdata("select * from temp_shiftcode where changeshift_id=" + key.Value + " and status not like '%Cancel%' order by date_change ");

        //if (dt.Rows.Count == 0)
        //{
        //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");


        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
        DataTable dtinsertcs = dbhelper.getdata("insert into temp_shiftcode (date_change,emp_id,changeshift_id,shiftcode_id,status,remarks,approver_id) values ('" + DateTime.Parse(txt_date.Text).ToShortDateString() + "','" + Session["emp_id"].ToString() + "'," + key.Value + "," + ddl_shiftcode.SelectedValue + ",'For Approval','" + txt_r.Text.Replace("'", "") + "','" + approver_id.Rows[0]["under_id"].ToString() + "') select scope_identity() id ");
        function.AddNotification("Change Shift Approval", "acs", approver_id.Rows[0]["under_userid"].ToString(), dtinsertcs.Rows[0]["id"].ToString());

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='KOISK_DTR'", true);
        //}
        //else
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Shift Code Already Change'); window.location='cs?key=" + Request.QueryString["key"].ToString() + "'", true);
    }


    //wv
    protected void click_save_wv(object sender, EventArgs e)
    {
        if (checker())
        {
            DataTable dt = dbhelper.getdata("select * from TRestdaylogs where EmployeeId='" + Session["emp_id"].ToString() + "' and convert(date,Date)=convert(date,'" + txt_otd.Text.Trim() + "') and (status like '%Approved%' or status like'%Verification%' or status like'%For Approval%') ");
            if (dt.Rows.Count == 0)
            {
                //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
                DataTable dtinserthdrd = dbhelper.getdata("insert into TRestdaylogs (EmployeeId,shiftcodeId,Date,reason,status,dtr_id,sysdate,approver_id,class) values (" + Session["emp_id"].ToString() + "," + ddl_sc.SelectedValue + ",'" + txt_otd.Text.Trim() + "','" + txt_lineremarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,GETDATE(),'" + approver_id.Rows[0]["under_id"].ToString() + "','" + rbl_class.SelectedValue + "') select scope_identity() id");
                function.AddNotification(rbl_class.SelectedValue + " Approval", rbl_class.SelectedValue, approver_id.Rows[0]["under_userid"].ToString(), dtinserthdrd.Rows[0]["id"].ToString());
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);
                close();
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Exist');", true);
                close();
            }

        }
    }
    protected void select(object sender, EventArgs e)
    {
        if (rbl_class.SelectedValue == "RD")
        {
            sc.Style.Add("display", "block");

        }
        else
            sc.Style.Add("display", "none");
    }
    protected bool checker()
    {
        bool oi = true;


        if (txt_otd.Text.Length == 0)
        {
            oi = false;
            lbl_date.Text = "Invalid date";
        }
        else
            lbl_date.Text = "";

        if (txt_lineremarks.Text == "")
        {
            oi = false;
            lbl_reason.Text = "*";
        }
        else
            lbl_reason.Text = "";

        if (rbl_class.SelectedValue == "RD")
        {
            if (ddl_sc.SelectedValue == "0")
            {
                oi = false;
                lbl_sc.Text = "*";
            }
            else
                lbl_sc.Text = "";
        }
        else
            lbl_sc.Text = "";

        return oi;

    }
    protected void loadsc()
    {
        string query = "select * from mshiftcode where status is null and fix=0 and ID>1";
        DataTable dt = dbhelper.getdata(query);
        ddl_sc.Items.Clear();
        ddl_sc.Items.Add(new ListItem("", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_sc.Items.Add(new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString()));
        }
    }
    protected void clickrd(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_status = (LinkButton)row.FindControl("lnk_status");
            if (lnk_status.Text.Contains("RD") || lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH"))
            {

                string[] datet = row.Cells[3].Text.Split('-');
                txt_otd.Text=datet[0].ToString();
                DataTable dtchk = dbhelper.getdata("select * from TRestdaylogs where EMPLOYEEID=" + Session["emp_id"].ToString() + " AND CONVERT(date,[Date])= CONVERT(date,'" + datet[0] + "') and status like'%Approved%'");
                if (dtchk.Rows.Count == 0)
                {
                    WV.Style.Add("display", "block");
                    if (lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH"))
                    {
                        rbl_class.SelectedValue = "HD";
                        sc.Style.Add("display", "none");
                    }
                    else if (lnk_status.Text.Contains("RD"))
                    {
                        rbl_class.SelectedValue = "RD";
                        sc.Style.Add("display", "block");
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Exist!');", true);
                    close();
                }

            }
        }

    }
    protected void close()
    {
        modal_cws.Style.Add("display", "NONE");
        modal_os.Style.Add("display", "NONE");
        modal_ot.Style.Add("display", "NONE");
        modal_ta.Style.Add("display", "NONE");
        WV.Style.Add("display", "NONE");
        hdn_total_tempot.Value = "";
        if (ddl_pay_range.SelectedValue != "0")
        {
            string[] hhh = ddl_pay_range.Text.Split('-');
            txt_from.Text = hhh[0].ToString();
            txt_to.Text = hhh[1].ToString();
            payrange.Value = ddl_pay_range.SelectedValue;
        }

        disp();
    }
}