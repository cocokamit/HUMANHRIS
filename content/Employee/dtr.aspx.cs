using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Human;
using System.Web.Services;

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
        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto,"+
        "(select COUNT(*)  from payroll_range where CONVERT(date,dfrom) <= CONVERT(date,GETDATE()) AND CONVERT(date,dtoo) >= CONVERT(date,GETDATE()) and id=a.id) cnt "+
        "from payroll_range a where action is null order by dfrom desc";
        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
       // ddl_pay_range.Items.Add(new ListItem("Payrol Period","0"));
        foreach(DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }

        DataRow[] sel = dt_range.Select("cnt > 0");
        if (sel.Length > 0)
            ddl_pay_range.SelectedValue = sel[0]["ffrom"].ToString() + '-' + sel[0]["tto"].ToString();
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
        string[] period = ddl_pay_range.Text.Split('-');

        string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
        DataTable dt = dbhelper.getdata("select * from memployee where id="+Session["emp_id"].ToString()+"");
        DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());

         /**BTK 112519
         * LEAVE **/
        string leaves = "select * from TLeaveApplicationLine where employeeid=" + Session["emp_id"].ToString() + " " +
        "and [date] between CONVERT(date,'" + period[0] + "') and CONVERT(date,'" + period[1] + "')  " +
        "and status like '%approved%' order by [date]";
        ViewState["LEAVES"] = dbhelper.getdata(leaves);


        if (ddl_pay_range.Items.Count > 0)
        {
            string[] gggg = ddl_pay_range.Text.Split('-');

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

            ///**OBT**/
            //string obt = "SELECT convert(varchar,obIn,101) tDate,* FROM OBTLine where empid = " + Session["emp_id"].ToString() + " and obIn between convert(date,'" + from + "') and convert(date,'" + to + "') and status='approved'";
            //ViewState["OBT"] = dbhelper.getdata(obt);

            string obt = "SELECT convert(varchar,tDate,101) tDate,* FROM OBTLine where empid = " + Session["emp_id"].ToString() + " and tDate between convert(date,'" + from + "') and convert(date,'" + to + "') and status='approved'";
            ViewState["OBT"] = dbhelper.getdata(obt);

            DataTable dtr = adjustdtrformat2.DTRF(from, to, "KIOSK_" + Session["emp_id"].ToString());
            ViewState["DTR_HANG"] = dtr;

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

        alert.Visible = grid_item.Rows.Count == 0 ? true : false;
        //compensatory();
    }

    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_otm = (LinkButton)e.Row.FindControl("lnk_otm");
            LinkButton lnk_ot = (LinkButton)e.Row.FindControl("lnk_ot");
            LinkButton lnk_tadj = (LinkButton)e.Row.FindControl("lnk_tadj");
            LinkButton lnk_offset = (LinkButton)e.Row.FindControl("lnk_offset");
            LinkButton lnk_status = (LinkButton)e.Row.FindControl("lnk_status");
            LinkButton lnk_offset_stat = (LinkButton)e.Row.FindControl("lnk_offset_stat");
            LinkButton lnk_cws = (LinkButton)e.Row.FindControl("lnk_cws");
            LinkButton lnk_offsethdrd = (LinkButton)e.Row.FindControl("lnk_offsethdrd");
            LinkButton lnk_offset_absent = (LinkButton)e.Row.FindControl("lnk_offset_absent");

            Label lbl_uthrs = (Label)e.Row.FindControl("lbl_uthrs");
            Label lbl_aw = (Label)e.Row.FindControl("lbl_aw");

            string[] period = ddl_pay_range.SelectedValue.Split('-');
            DataTable offset__cutoff = dbhelper.getdata("select *  from toffset where  (status like'%for approval%'  or status like'%verification%' or status like'%Approve%') and CONVERT(date,appliedfrom)>=convert(date,'" + period[0] + "') and CONVERT(date,appliedfrom)<=convert(date,'" + period[1] + "') and empid=" + e.Row.Cells[24].Text + " and appliedto is null");

            string scid = e.Row.Cells[23].Text;
            string[] datenname = e.Row.Cells[3].Text.Split('-');
            string[] getdatefromout = e.Row.Cells[9].Text.Split(' ');


            e.Row.Cells[3].Text = DateTime.Parse(datenname[0]).ToString("MM/dd/yyyy") + " " + datenname[1];
             
            string[] ddd = datenname[0].Split('/');

            string dday = ddd[1].Length > 1 ? ddd[1] : "0" + ddd[1];
            string mmonth = ddd[0].Length > 1 ? ddd[0] : "0" + ddd[0];

            string dddate = mmonth + "/" + dday + "/" + ddd[2];

            /**BTK 112519
            *CHECK DTR IRREGULARITY 
            double RenderedHrs = double.Parse(e.Row.Cells[14].Text);

            if(RenderedHrs < 8)
            {
                if(e.Row.Cells[23].Text != "1")
                    e.Row.ForeColor = System.Drawing.Color.Red;
            }
             * 
            END**/
 
            if (dddate == "10/01/2018")
            {
                string efrerfe = "";
            }

            DataTable chkotmeal = dbhelper.getdata("select * from TMealHours where CONVERT(date,[Date])=convert(date,CONVERT(datetime,'" + dddate + "')) and EmployeeId = " + e.Row.Cells[24].Text + " ");
            DataTable ifallowed = dbhelper.getdata("select DivisionId2,Fullname,case when allowAccess is null then 'False' else allowAccess end allowAccess,case when allowOTMeal is null then 'False' else allowOTMeal end allowOTMeal,case when nonepunching is null then 'False' else nonepunching end nonepunching,case when allowOT  is null then 'False' else allowOT end allowOT from MEmployee where Id = " + e.Row.Cells[24].Text + "");
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
            Label lbl_sr = (Label)e.Row.FindControl("lbl_sr");
            ff.ToolTip = sc.Rows[0]["Remarks"].ToString();
            lbl_sr.Text = sc.Rows[0]["Remarks"].ToString() == "RD" ? "" : sc.Rows[0]["Remarks"].ToString();
            
            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + scid + ffix + " ");
            DataTable dthd = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where CONVERT(date,a.date)=convert(date,'" + dddate + "')");
            DataTable dtotpolicy = dbhelper.getdata("select c.id otrolesid,c.policyid otpolicyid,c.considered,c.roles from memployee a left join MCompany b on a.CompanyId=b.Id left join sys_policy_roles c on b.ot_roles=c.id where a.id=" + e.Row.Cells[24].Text + "");
            DataTable dtchkot = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where (status like'%approved%')  and CONVERT(date,date)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");
            DataTable dtchkotpending = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where ( status like'%for approval%'  or status like'%verification%')  and CONVERT(date,date)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");

            DataTable dtchkcws = dbhelper.getdata("select *  from temp_shiftcode where  (status like'%for approval%'  or status like'%verification%' or status like'%approved%' ) and CONVERT(date,date_change)=convert(date,'" + dddate + "') and emp_id=" + e.Row.Cells[24].Text + "");
            DataTable dtchkshifcode = dbhelper.getdata("select *  from TChangeShiftLine where status like'%approved%' and LEFT(CONVERT(date,Date,101),10)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");

            DataTable dtgetstatusofworkverification = dbhelper.getdata("select * from TRestdaylogs a where (a.status like '%For Approval%'  or a.status like '%Verification%') and CONVERT(date,a.date)=convert(date,'" + dddate + "') and a.EmployeeId=" + e.Row.Cells[24].Text + " ");
            DataTable dtgetstatusofworkverificationapproved = dbhelper.getdata("select * from TRestdaylogs a where (a.status like '%Approved%') and CONVERT(date,a.date)=convert(date,'" + dddate + "') and a.EmployeeId=" + e.Row.Cells[24].Text + " ");

            DataTable dtchkmanual = dbhelper.getdata("select *  from Tmanuallogline where  (status like'%for approval%'  or status like'%verification%' or status like'%approved%' ) and CONVERT(date,date)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");
            DataTable dtchkmanualnotapproved = dbhelper.getdata("select *  from Tmanuallogline where  (status like'%for approval%'  or status like'%verification%' ) and CONVERT(date,date)=convert(date,'" + dddate + "') and EmployeeId=" + e.Row.Cells[24].Text + "");

            DataTable dtoffset = dbhelper.getdata("select *  from toffset where  (status like'%for approval%'  or status like'%verification%') and CONVERT(date,appliedfrom)=convert(date,'" + dddate + "') and empid=" + e.Row.Cells[24].Text + " and appliedto is null");
            DataTable dtoffsetapproved = dbhelper.getdata("select *  from toffset where  status like'%Approved%' and CONVERT(date,appliedfrom)=convert(date,'" + dddate + "') and empid=" + e.Row.Cells[24].Text + " and appliedto is null");
            DataTable dtemp = dbhelper.getdata("select *,case when allowoffset is null then 'False' else allowoffset end allowedofset from memployee where  id=" + e.Row.Cells[24].Text + "");
            DataTable dtcs = dbhelper.getdata("select " +
                                        "case when time_in='0' then 0 else convert(datetime,time_in)end time_in, " +
                                        "case when time_out1='0' then 0 else convert(datetime,time_out1)end time_out1,  " +
                                        "case when time_in1='0' then 0 else convert(datetime,time_in1)end time_in1, " +
                                        "case when time_out='0' then 0 else convert(datetime,time_out)end time_out " +
                //"convert(datetime,time_in)time_in,convert(datetime,time_out1)time_out1,convert(datetime,time_in1)time_in1,convert(datetime,time_out)time_out 
                                        "from tmanuallogline where employeeid='" + e.Row.Cells[24].Text + "' and (status like'%for approval%' or status like'%verification%' ) and CONVERT(date,[date])=convert(date,CONVERT(datetime,'" + dddate + "'))");
            
            DataTable dtchkoffrdhdfrom = dbhelper.getdata("select left(convert(varchar,appliedto,101),10)appto, * from toffset where CONVERT(date,appliedfrom)='" + dddate + "' and (status like'%For Approval%' or status like'%Verification%' or status like'%Approved%') and action is null and appliedto is not null and empid=" + e.Row.Cells[24].Text + "");
            DataTable dtchkoffrdhdto = dbhelper.getdata("select left(convert(varchar,appliedto,101),10)appto, * from toffset where CONVERT(date,appliedto)='" + dddate + "' and (status like'%For Approval%' or status like'%Verification%' or status like'%Approved%') and action is null and appliedto is not null and empid=" + e.Row.Cells[24].Text + "");


            decimal consideredhrs = 0;
            DateTime get1in = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeIn1"].ToString() : "0:00:00");
            DateTime get1out = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeOut1"].ToString() : "0:00:00");
            DateTime get2in = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeIn2"].ToString() : "0:00:00");
            DateTime get2out = Convert.ToDateTime(chkdtsc.Rows.Count > 0 ? chkdtsc.Rows[0]["TimeOut2"].ToString() : "0:00:00");
            if (dtcs.Rows.Count > 0)
            {
                e.Row.Cells[6].ForeColor = System.Drawing.Color.Magenta;
                e.Row.Cells[7].ForeColor = System.Drawing.Color.Magenta;
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Magenta;
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Magenta;
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
                e.Row.Cells[6].Text = "--";
            if (e.Row.Cells[9].Text.Contains("1/1/1900"))
                e.Row.Cells[9].Text = "--";

             /**BTK 112510
             * LEAVE
             * IDICATE PAID LEAVE**/
            if (e.Row.Cells[10].Text == "True" || e.Row.Cells[12].Text == "True")
            {
                DataTable LEAVES = (DataTable)ViewState["LEAVES"];
                DataRow[] leave = LEAVES.Select("date='"+dddate+"'");

                e.Row.Cells[10].Text = "✓";
                e.Row.Cells[10].ForeColor = leave[0]["WithPay"].ToString() == "True" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                e.Row.Cells[10].ToolTip =  leave[0]["WithPay"].ToString() == "True" ? "Paid Leave" : "Unpaid Leave";

                if(e.Row.Cells[10].Text == "True")
                    e.Row.Cells[10].ToolTip = "Wholeday";
                if(e.Row.Cells[12].Text == "True")
                    e.Row.Cells[10].ToolTip = "Halfday";
            }
            else
                e.Row.Cells[10].Text = "--";

            /**This is for date indicator**/
            DataTable show_otlink = dbhelper.getdata("select b.AllowOtMeal from MEmployee a left join MDivision2 b on a.DivisionId2 = b.Id where a.Id = " + Session["emp_id"].ToString() + "");
            if (show_otlink.Rows[0]["AllowOtMeal"].ToString() == "True")
            {
                lnk_otm.Visible = true;
            }
            else
                lnk_otm.Visible = false;

            DataTable dtindicator = dbhelper.getdata("select convert(varchar, getdate(), 101)+' '+DATENAME(weekday,GETDATE())indicator");
            string indicator = dtindicator.Rows[0]["indicator"].ToString();
            if (e.Row.Cells[3].Text == indicator)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }

            if (dtemp.Rows[0]["allowedofset"].ToString() == "True")
            {
                if (decimal.Parse(lbl_uthrs.Text) > 0)
                    lnk_offset.Visible = true;
                else
                    lnk_offset.Visible = false;

                if (lbl_aw.Text == "True")
                    lnk_offset_absent.Visible = true;
                else
                    lnk_offset_absent.Visible = false;
            }

            /**ABSENT
            * ADVANCE DATE HIRED**/
            if (lbl_aw.Text == "True" || e.Row.Cells[13].Text == "True")
            {

                if (e.Row.Cells[10].Text == "True" || e.Row.Cells[12].Text == "True")
                {
                    lbl_aw.Text = "--";
                    if (e.Row.Cells[13].Text == "True")
                        lbl_aw.ToolTip = "Wholeday";
                    if (e.Row.Cells[13].Text == "True")
                        lbl_aw.ToolTip = "Halfday";
                }
                else
                {
                    lbl_aw.Text = "✓";
                    e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                    if (lbl_aw.Text == "True")
                        lbl_aw.ToolTip = "Wholeday";
                    if (e.Row.Cells[13].Text == "True")
                        lbl_aw.ToolTip = "Halfday";
                }

                //cancel pre ot in 
                DataTable preotcans = dbhelper.getdata("select * from TOverTimeLine a where EmployeeId = " + e.Row.Cells[24].Text + " and convert(varchar,Date,101) = '" + dddate + "' and status like '%For Approval%' or status like '%verification%' order by Id desc");
                DataTable todate = dbhelper.getdata("select convert(varchar, getdate(), 110)todate");
                if (preotcans.Rows.Count > 0)
                {
                    DateTime otdate = Convert.ToDateTime(preotcans.Rows[0]["Date"].ToString());
                    DateTime tootdate = Convert.ToDateTime(todate.Rows[0]["todate"].ToString());

                    if (tootdate > otdate.AddDays(1))
                    {
                       // dbhelper.getdata("update TOverTimeLine set status = 'Cancel-" + dddate + "' where Id = " + preotcans.Rows[0]["Id"].ToString() + "");
                    }
                }
                //end
            }
            else
                e.Row.Cells[11].Text = "--";

            DataTable chkwv = dbhelper.getdata("select * from TRestdaylogs a where a.status like '%Approved%' and  left(convert(varchar,a.date,101),10)='" + dddate + "'");
            string opa = "select b.DateStart,b.DateEnd, * from TPayroll a " +
                                "left join tdtr b on a.DTRId=b.Id " +
                                "where a.status is null and CONVERT(date,b.dateend)>= convert(date,'" + dddate + "') and b.status is null and b.PayrollGroupId=" + dtemp.Rows[0]["PayrollGroupId"].ToString() + "";
            DataTable dtopa = dbhelper.getdata(opa);
            if (dtopa.Rows.Count > 1)
            {
                lnk_otm.Enabled = false;
                lnk_tadj.Enabled = false;
                lnk_ot.Enabled = false;
                lnk_cws.Enabled = false;
                lnk_offset.Visible = false;
                lnk_offset_absent.Visible = false;
            }
            else
            {
                //Kung naka file
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

                if (dtgetstatusofworkverification.Rows.Count > 0)
                {
                    lnk_cws.Enabled = false;
                    lnk_cws.ForeColor = System.Drawing.Color.Gray;

                    //Time Adjustment Disable if File Work Verification
                    lnk_tadj.Enabled = false;
                    lnk_tadj.ForeColor = System.Drawing.Color.Gray;
                }

                if (chkotmeal.Rows.Count == 0)
                {
                    //if (ifallowed.Rows[0]["allowOTMeal"].ToString() == "True")
                    //{
                        if (e.Row.Cells[9].Text.Length > 2)
                        {
                            if (Convert.ToDateTime(e.Row.Cells[9].Text) >= Convert.ToDateTime(e.Row.Cells[29].Text).AddHours(2))
                            {
                                TimeSpan tmtotal;
                                DateTime tout = Convert.ToDateTime(e.Row.Cells[9].Text);//official
                                DateTime tin = Convert.ToDateTime(e.Row.Cells[29].Text);//schedule
                                tmtotal = tout - tin;//01:00:00
                                decimal dec = Convert.ToDecimal(TimeSpan.Parse(tmtotal.ToString()).Hours);
                                DataTable chkobt = dbhelper.getdata("select * from obtline where status <> 'Cancelled' and empID = " + Session["emp_id"] + " and CONVERT(date,[tDate])=convert(date,CONVERT(datetime,'" + dddate + "'))");
                                if (chkobt.Rows.Count == 0)
                                {
                                    if (chkotmeal.Rows.Count == 0)
                                    {
                                        lnk_otm.Enabled = true;
                                        lnk_otm.ForeColor = System.Drawing.Color.Blue;
                                        //dbhelper.getdata("insert into TMealHours values('" + Session["emp_id"] + "','" + dddate + "','" + tin + "','" + tout + "','" + dec + "','OTMeal','Approved',NULL,GETDATE())");
                                    }
                                }
                            }
                        }
                    //}
                }

                if (dtchkot.Rows.Count == 0)
                {
                    if (e.Row.Cells[9].Text.Length > 2)
                    {
                        if (Convert.ToDateTime(e.Row.Cells[9].Text) >= Convert.ToDateTime(e.Row.Cells[29].Text).AddHours(1))
                        {
                            if (dtchkmanualnotapproved.Rows.Count == 0)
                            {
                                if (ifallowed.Rows[0]["allowOT"].ToString() == "True")
                                {
                                    lnk_ot.Enabled = true;
                                    lnk_ot.ForeColor = System.Drawing.Color.Blue;
                                }
                            }
                            else
                            {
                                lnk_ot.Enabled = false;
                                lnk_ot.ForeColor = System.Drawing.Color.Gray;
                            }

                        }//Crossdates OT
                        if (Convert.ToDateTime(e.Row.Cells[6].Text) >= Convert.ToDateTime(e.Row.Cells[29].Text).AddHours(9.5))
                        {
                            lnk_ot.Enabled = false;
                            lnk_ot.ForeColor = System.Drawing.Color.Gray;
                            lnk_otm.Enabled = false;
                            lnk_otm.ForeColor = System.Drawing.Color.Gray;
                        }

                        DataTable dtaddday = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + e.Row.Cells[29].Text + "',101))crossdates");
                        if (Convert.ToDateTime(e.Row.Cells[9].Text).AddHours(-1) >= Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString()))
                        {
                            lnk_ot.Enabled = true;
                            lnk_ot.ForeColor = System.Drawing.Color.Blue;
                            if (Convert.ToDateTime(e.Row.Cells[9].Text).AddHours(-2) >= Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString()))
                            {
                                lnk_otm.Enabled = true;
                                lnk_otm.ForeColor = System.Drawing.Color.Blue;
                            }
                        }
                    }
                }
                else
                {
                    TimeSpan final_ot;
                    string nighthrs;

                    DataTable dtsts = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where(status like'%approved%')and CONVERT(date,date)=convert(date,GETDATE()) and EmployeeId='" + e.Row.Cells[24].Text + "'");
                    if (dtsts.Rows.Count > 0)
                    {
                        DateTime final_out = Convert.ToDateTime(e.Row.Cells[9].Text);
                        DateTime final_set_out = Convert.ToDateTime(e.Row.Cells[29].Text);
                        DateTime preot_out = Convert.ToDateTime(dtsts.Rows[0]["time_out"]);

                        if (final_out < preot_out)
                        {
                            DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + final_out + "')/30*30, 0)outmins");
                            final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - final_set_out;
                            nighthrs = getnightdif.getnight(final_set_out.ToString(), final_out.ToString(), "dtr");//0
                            if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                            {
                                hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                                hdn_night_othrs.Text = nighthrs;

                                //dbhelper.getdata("update TOverTimeLine set OvertimeHours='" + hdn_reg_othrs.Text + "',OvertimeNightHours='" + hdn_night_othrs.Text + "',time_in='" + final_set_out + "',time_out='" + final_out + "',overtimehoursapp='" + hdn_reg_othrs.Text + "',overtimenighthoursapp='" + hdn_night_othrs.Text + "' where Id='" + dtsts.Rows[0]["Id"] + "'");
                            }
                        }
                    }
                    lnk_tadj.Enabled = false;
                    lnk_tadj.ForeColor = System.Drawing.Color.Gray;
                }


                if (dtoffset.Rows.Count > 0)
                {
                    e.Row.Cells[16].ForeColor = System.Drawing.Color.Magenta;
                    e.Row.Cells[16].Text = dtoffset.Rows[0]["offsethrs"].ToString();
                    lnk_offset.Visible = true;
                    e.Row.Cells[16].ToolTip = dtoffset.Rows[0]["status"].ToString();

                    lnk_offset.Visible = false;
                    lnk_offset_absent.Visible = false;
                }

                if (decimal.Parse(e.Row.Cells[20].Text) > 0)
                    e.Row.Cells[20].ForeColor = System.Drawing.Color.Red;
                if (decimal.Parse(lbl_uthrs.Text) > 0)
                    e.Row.Cells[21].ForeColor = System.Drawing.Color.Red;
            }

            //OBT 25-26
            if (e.Row.Cells[26].Text.Contains("Holiday")) 
                e.Row.Cells[26].BackColor = System.Drawing.Color.YellowGreen;
            if (e.Row.Cells[26].Text == "RD")
                e.Row.Cells[26].BackColor = System.Drawing.Color.Yellow;
           
            if (lnk_status.Text == "RD")
            {
                lnk_status.Style.Add("background-color", "yellow");
                lnk_status.Style.Add("border", "1px");
                lnk_status.Style.Add("border-radius", "5px");
            }

            if (lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("CH"))
            {
                lnk_status.Style.Add("background-color", "yellowgreen");
                lnk_status.Style.Add("border", "1px");
                lnk_status.Style.Add("border-radius", "5px");
            }

            DataTable OBT = (DataTable)ViewState["OBT"];
            DataRow[] OBTFilter = OBT.Select("tDate='" + dddate + "'");


            if (OBTFilter.Length > 0)
            {
                e.Row.Cells[25].Text = "✓";
                e.Row.Cells[25].ForeColor = System.Drawing.Color.Blue;

                if (e.Row.Cells[26].Text == "RD")
                {
                    lnk_cws.Enabled = true;
                    lnk_cws.ForeColor = System.Drawing.Color.Blue;
                }
                lnk_cws.Enabled = false;
                lnk_cws.ForeColor = System.Drawing.Color.Gray;
                lnk_otm.Enabled = false;
                lnk_otm.ForeColor = System.Drawing.Color.Gray;

                if (dtchkmanual.Rows.Count > 0)
                {
                    lnk_tadj.Enabled = false;
                    lnk_tadj.ForeColor = System.Drawing.Color.Gray;
                    lnk_tadj.ToolTip = "Already Exists";
                }
                else
                {
                    lnk_tadj.Enabled = true;
                    lnk_tadj.ForeColor = System.Drawing.Color.Blue;
                }

                DataTable dtgetobtot = dbhelper.getdata("select (select convert(varchar,obOut,101))obOut2,obOut from obtline where empID=" + Session["emp_id"].ToString() + " and status='Approved' and (select convert(varchar,tDate,101))='" + dddate + "'");
                if (ifallowed.Rows[0]["allowOT"].ToString() == "True")
                {
                    if (dtgetobtot.Rows.Count > 0)
                    {
                        if (dtgetobtot.Rows[0]["obOut2"].ToString() == dddate)
                        {
                            //OBT OT 2HOURS
                            if (Convert.ToDateTime(dtgetobtot.Rows[0]["obOut"].ToString()) >= Convert.ToDateTime(e.Row.Cells[29].Text).AddHours(1))
                            {
                                lnk_ot.Enabled = true;
                                lnk_ot.ForeColor = System.Drawing.Color.Blue;
                            }
                        }
                        else
                        {//crossdate sa ot
                            DataTable dtaddday = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + e.Row.Cells[29].Text + "',101))crossdates");

                            if (Convert.ToDateTime(dtgetobtot.Rows[0]["obOut"].ToString()) >= Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString()).AddHours(2))
                            {
                                lnk_ot.Enabled = true;
                                lnk_ot.ForeColor = System.Drawing.Color.Blue;
                            }
                        }
                    }
                }
            }
            else
                e.Row.Cells[25].Text = "--";

            //OBT 26-27
            if (e.Row.Cells[27].Text == "True")
                e.Row.ForeColor = System.Drawing.Color.Red;

            /**OFFSETTING            
             * BTK 09/23/2019 **/
            if (dddate == "09/03/2019")
            {
            }

            lnk_status.Text = dtgetstatusofworkverification.Rows.Count > 0 ? lnk_status.Text + "(Pending)" : lnk_status.Text;
            if (dtgetstatusofworkverificationapproved.Rows.Count > 0)
            {
                if (decimal.Parse(e.Row.Cells[14].Text) > 0)
                {
                    //cntCompensatory(dtgetstatusofworkverificationapproved);
                    lnk_offsethdrd.Visible = true;
                }
               // lnk_cws.Visible = false;

                /**BTK
               * 111519
               * OFFSETTING
                 * Employee allow offsetting
               * **/
                lnk_offsethdrd.Visible = dtemp.Rows[0]["allowhdoffset"].ToString() == "True" ? true : false;
            }
            else
                lnk_offsethdrd.Visible = false;

            if (dtchkoffrdhdfrom.Rows.Count > 0)
            {
                lnk_offset_stat.Visible = true;
                lnk_offsethdrd.Visible = false;
                lnk_offset_stat.Text = dtchkoffrdhdfrom.Rows[0]["appto"].ToString();
                lnk_offset_stat.ToolTip = dtchkoffrdhdfrom.Rows[0]["status"].ToString();
                // lnk_cws.Visible = false;
            }

            if (dtchkoffrdhdto.Rows.Count > 0)
            {
                string[] offsetstatus = null;
                string finaloffsetstatus = null;
                if (dtchkoffrdhdto.Rows[0]["status"].ToString().Contains('-'))
                {
                    offsetstatus = dtchkoffrdhdto.Rows[0]["status"].ToString().Split('-');
                    finaloffsetstatus = offsetstatus[0];
                }
                else
                    finaloffsetstatus = dtchkoffrdhdto.Rows[0]["status"].ToString();
                e.Row.Cells[16].Text = dtchkoffrdhdto.Rows[0]["offsethrs"].ToString();
                lnk_offset_absent.Visible = false;
                lnk_offset.Visible = false;

                e.Row.Cells[16].ToolTip = finaloffsetstatus;

                if (finaloffsetstatus != "Approved")
                    e.Row.Cells[16].ForeColor = System.Drawing.Color.Magenta;

                //lnk_cws.Visible = false;
                //lnk_tadj.Visible = false;
                //lnk_ot.Visible = false;

            }
            /*END OFFSETTING*/
          
            if (dtchkotpending.Rows.Count > 0)
            {
                string[] otstatus = null;
                string finalotstatus = null;
                if (dtchkotpending.Rows[0]["status"].ToString().Contains('-'))
                {
                    otstatus = dtchkotpending.Rows[0]["status"].ToString().Split('-');
                    finalotstatus = otstatus[0];
                }
                else
                    finalotstatus = dtchkotpending.Rows[0]["status"].ToString();

                if (decimal.Parse(dtchkotpending.Rows[0]["overtimehours"].ToString()) > 0)
                {
                    if (finalotstatus != "Approved")
                        e.Row.Cells[17].ForeColor = System.Drawing.Color.Magenta;
                }
                e.Row.Cells[17].Text = dtchkotpending.Rows[0]["overtimehours"].ToString();
                e.Row.Cells[17].ToolTip = finalotstatus;

                if (decimal.Parse(dtchkotpending.Rows[0]["overtimenighthours"].ToString()) > 0)
                {
                    if (finalotstatus != "Approved")
                        e.Row.Cells[18].ForeColor = System.Drawing.Color.Magenta;
                }
                e.Row.Cells[18].Text = dtchkotpending.Rows[0]["overtimenighthours"].ToString();
                e.Row.Cells[18].ToolTip = finalotstatus;
                //lnk_ot.Visible = false;
                lnk_ot.Enabled = false;
                lnk_ot.ForeColor = System.Drawing.Color.Gray;
            }

            if (offset__cutoff.Rows.Count > 0)
            {
                lnk_cws.Enabled = false;
                lnk_cws.ForeColor = System.Drawing.Color.Gray;
                lnk_tadj.Enabled = false;
                lnk_tadj.ForeColor = System.Drawing.Color.Gray;
                lnk_offset.Visible = false;
                lnk_offset_absent.Visible = false;
            }

            //Att logs
            string cdate = DateTime.Parse(datenname[0]).ToString("MM/dd/yyyy");
            Label shift = (Label)e.Row.Cells[2].FindControl("Label6");
            GridView att = (GridView)e.Row.Cells[28].FindControl("gv_attlog");//OBT 27-28


            string query = "SELECT a.biotime Date_Time,a.id tdtrID "  +
            "FROM tdtrperpayrolperline a " +
            "left join memployee b on a.empid=b.id " +
            "where convert(varchar,a.biotime, 101)  =  convert(datetime,'" + cdate + "') and b.id=" + e.Row.Cells[24].Text + " order by a.biotime";

            att.DataSource = dbhelper.getdata(query);
            att.DataBind();

            //OBT
            //FOR APPROVAL PANI
            //DataTable OBT = (DataTable)ViewState["OBT"];
            //DataRow[] OBTFilter = OBT.Select("tDate='" + dddate + "'");

            //if (OBTFilter.Length > 0)
            //    e.Row.Cells[25].Text = "✓"; //OBTFilter[0]["tHrs"].ToString(); ;
            //else
            //    e.Row.Cells[25].Text = "-";
    

        }
    }

    protected void cntCompensatory(DataTable param)
    {
        DataTable dt = getdata.compensatory(param.Rows[0]["id"].ToString());
        if (dt.Rows.Count == 0)
        {
            string[] period = ddl_pay_range.Text.Split('-');
            dbhelper.getdata("insert into TCompensatory values (" + param.Rows[0]["id"].ToString() + ",0,'" + period[0] + "','" + period[1] + "',1,0)");
        }
    }

    protected void compensatory()
    {
        string query = "select dateadd(month, -1 ,a.dfrom ) fdate, * from payroll_range  a where [action] is null " +
        "and (select COUNT(*) from payroll_range where GETDATE() between  a.dfrom and a.dtoo) > 0";

        DataTable filter = dbhelper.getdata(query);
        if (filter.Rows.Count > 0)
        {
            DataTable dt = getdata.compensatory(Session["emp_id"].ToString(), filter.Rows[0]["fdate"].ToString(), filter.Rows[0]["dtoo"].ToString());
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("status like '%Approved%' and offsetstatus not like '%Approved%'");
                if (dr.Length > 0)
                    gvCompensatory.DataSource = dr.CopyToDataTable();
            }
        }
        gvCompensatory.DataBind();
    }

    protected void rowboundCompensatory(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblstatus = (Label)e.Row.FindControl("lblstatus");
            if (e.Row.Cells[0].Text == "For Approval")
                lblstatus.Text = "For Approval";
        }
    }

    protected void click_adj(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string[] timein = row.Cells[6].Text.Split(' ');
            modal_ta.Style.Add("display", "block");
            string[] ddate = row.Cells[3].Text.Split(' ');
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
        /**BETA 110519
        * EMAIL NOTIFICATION
        * - TIME ADJUSTMENT
        * - REQUEST APPROVAL
        * **/
        int scid=0;
        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
      
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
            a.sf = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
            a.sg = txt_remarks.Text.Replace("'", "");
            a.sj = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
            
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
                
                /**START MAIL NOTIFICATION**/
                string approver = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
                function.Notification("timeadjust", "alert", x, approver);
                /**END**/

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
        close();
    }

    protected void add_ot(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal_ot.Style.Add("display", "block");
            TimeSpan final_ot;
            string nighthrs;
            hdn_reg_othrs.Text = "0";
            hdn_night_othrs.Text = "0";

            //OBT✓
            string obtstat = row.Cells[25].Text;
            if (obtstat == "✓")
            {
                string[] aw = row.Cells[3].Text.Split(' ');

                DataTable dtgetobtot = dbhelper.getdata("select(select convert(varchar,obOut,101))obOut2,* from obtline where empID=" + Session["emp_id"].ToString() + " and status='Approved' and (select convert(varchar,tDate,101))='" + aw[0] + "'");
                DataTable dtgetmanual = dbhelper.getdata("select(select DATEADD(mi,DATEDIFF(mi, 0,time_OUT)/30*30,0)outmins)manualout,(select DATEADD(mi,DATEDIFF(mi, 0,time_in)/30*30,0)outmins)manualin from Tmanuallogline where EmployeeId=" + Session["emp_id"].ToString() + " and status like '%Approved%' and (select convert(varchar,[Date],101))='" + aw[0] + "'");
                if (dtgetmanual.Rows.Count > 0)
                {
                    //Base OBT with Manual out
                    string[] eww = dtgetmanual.Rows[0]["manualout"].ToString().Split(' ');

                    if (aw[0] == eww[0])
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + dtgetmanual.Rows[0]["manualout"].ToString() + "')/30*30, 0)outmins");
                        DateTime obtout = Convert.ToDateTime(dtget30.Rows[0]["outmins"].ToString());
                        DateTime obtschdout = Convert.ToDateTime(row.Cells[29].Text);
                        final_ot = obtout - obtschdout;
                        nighthrs = getnightdif.getnight(obtschdout.ToString(), obtout.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                            hdn_night_othrs.Text = nighthrs;
                        }
                        lbl_ddate.Text = DateTime.Parse(dtgetmanual.Rows[0]["manualin"].ToString()).ToString("MM/dd/yyyy");
                        TextBox1.Text = DateTime.Parse(dtgetmanual.Rows[0]["manualout"].ToString()).ToString("yyyy-MM-dd");
                        TextBox2.Text = DateTime.Parse(dtgetmanual.Rows[0]["manualout"].ToString()).ToString("HH:mm");
                        hdn_setupout.Value = obtout.ToString();
                        hdn_actout.Value = obtschdout.ToString();
                    }
                }
                //base on OBT out
                else if (aw[0] == dtgetobtot.Rows[0]["obOut2"].ToString())
                {
                    DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + dtgetobtot.Rows[0]["obOut"].ToString() + "')/30*30, 0)outmins");
                    DateTime obtout = Convert.ToDateTime(dtget30.Rows[0]["outmins"].ToString());
                    DateTime obtschdout = Convert.ToDateTime(row.Cells[29].Text);
                    final_ot = obtout - obtschdout;
                    nighthrs = getnightdif.getnight(obtschdout.ToString(), obtout.ToString(), "dtr");
                    if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                    {
                        hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                        hdn_night_othrs.Text = nighthrs;
                    }
                    lbl_ddate.Text = DateTime.Parse(dtgetobtot.Rows[0]["obIn"].ToString()).ToString("MM/dd/yyyy");
                    TextBox1.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("yyyy-MM-dd");
                    TextBox2.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("HH:mm");
                    hdn_setupout.Value = obtout.ToString();
                    hdn_actout.Value = obtschdout.ToString();
                }
                else
                {   //Crossdate sa OT OBT
                    DataTable dtaddday = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + Convert.ToDateTime(row.Cells[29].Text) + "',101))crossdates");
                    DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + dtgetobtot.Rows[0]["obOut"].ToString() + "')/30*30, 0)outmins");
                    DateTime obtout = Convert.ToDateTime(dtget30.Rows[0]["outmins"].ToString());
                    DateTime obtschdout = Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString());
                    final_ot = obtout - obtschdout;
                    nighthrs = getnightdif.getnight(obtout.ToString(), obtschdout.ToString(), "dtr");
                    if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                    {
                        hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                        hdn_night_othrs.Text = nighthrs;
                    }
                    lbl_ddate.Text = DateTime.Parse(dtgetobtot.Rows[0]["obIn"].ToString()).ToString("MM/dd/yyyy");
                    TextBox1.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("yyyy-MM-dd");
                    TextBox2.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("HH:mm");
                    hdn_setupout.Value = obtout.ToString();
                    hdn_actout.Value = obtschdout.ToString();
                }
            }

            //if not on OBT
            else
            {
                //if on Crossdates OT
                DataTable dtaddday = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + row.Cells[29].Text + "',101))crossdates");
                if (Convert.ToDateTime(row.Cells[9].Text).AddHours(-1) >= Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString()))
                {
                    DataTable dtout = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + row.Cells[29].Text + "',101))crossdates");
                    DateTime crossout = Convert.ToDateTime(row.Cells[9].Text);
                    DateTime schedout = Convert.ToDateTime(dtout.Rows[0]["crossdates"].ToString());
                    if (crossout > schedout)
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + crossout + "')/30*30, 0)outmins");
                        final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - schedout;
                        nighthrs = getnightdif.getnight(schedout.ToString(), crossout.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                            hdn_night_othrs.Text = nighthrs;
                        }
                    }
                    lbl_ddate.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
                    TextBox1.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
                    TextBox2.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
                    hdn_setupout.Value = schedout.ToString();
                    hdn_actout.Value = crossout.ToString();
                }

                //if on Normal OT
                else
                {
                    DateTime final_out = Convert.ToDateTime(row.Cells[9].Text);
                    DateTime final_set_out = Convert.ToDateTime(row.Cells[29].Text);

                    if (final_out > final_set_out)
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + final_out + "')/30*30, 0)outmins");
                        final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - final_set_out;
                        nighthrs = getnightdif.getnight(final_set_out.ToString(), final_out.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            hdn_reg_othrs.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                            hdn_night_othrs.Text = nighthrs;
                        }
                    }


                    lbl_ddate.Text = DateTime.Parse(row.Cells[6].Text).ToString("MM/dd/yyyy");
                    TextBox1.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
                    TextBox2.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
                    hdn_setupout.Value = final_set_out.ToString();
                    hdn_actout.Value = final_out.ToString();
                }
            }
        }
    }

    public static string roundupanddown(double value)
    {
        double rounded = Math.Floor(value * 2) / 2;
        return string.Format("{0:0.00}", rounded);
    }
    protected void calculatedate(object sender, EventArgs e)
    {
        string[] decimals = hdn_reg_othrs.Text.Split('.');
        if (Int32.Parse(decimals[1]) >= 30)
        {
            hdn_reg_othrs.Text = decimals[0] + ".50";
        }
        else
        {
            hdn_reg_othrs.Text = decimals[0] + ".00";
        }
        DateTime final_out = Convert.ToDateTime("" + hdn_actout.Value + "");
        DateTime final_set_out = Convert.ToDateTime("" + hdn_setupout.Value + "");
        TimeSpan final_ot;
        string nighthrs;
        string total;
        if (final_out > final_set_out)
        {
            DataTable dtget30 = dbhelper.getdata("select DATEADD(mi, DATEDIFF(mi, 0, '" + final_out + "')/30*30, 0)outmins");
            final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - final_set_out;
            nighthrs = getnightdif.getnight(final_set_out.ToString(), final_out.ToString(), "dtr");
            if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
            {
                total = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                hdn_night_othrs.Text = nighthrs;
                if (decimal.Parse(hdn_reg_othrs.Text) > decimal.Parse(total))
                {
                    Response.Write("<script>alert('Above rendered hours is not allowed!')</script>");
                    hdn_reg_othrs.Text = total;
                }
            }
        }
    }
    protected void save_ot_request(object sender, EventArgs e)
    {
        /**BETA 110519
         * EMAIL NOTIFICATION
         * - OVERTIME REQUEST
         * - APPROVAL
         * **/
        DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + lbl_ddate.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status)) like '%approved%' or SUBSTRING(status,0, CHARINDEX('-',status)) like '%for verification%' )");
        DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
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
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
            string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
            string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
            
            string queryyy = "insert into TOverTimeLine ([OverTimeId],[EmployeeId],[Date],[OvertimeHours],[OvertimeNightHours],[OvertimeLimitHours],[Remarks],[status],[ifdone],[dtr_id],[sysdate],[time_in],[time_out],[overtimehoursapp],[overtimenighthoursapp],[approver_id],regmultiplier,nightmultiplier,hourlyrate) values(NULL," + Session["emp_id"].ToString() + ",'" + lbl_ddate.Text + "','" + hdn_reg_othrs.Text + "','" + hdn_night_othrs.Text + "','0','" + txt_otremarks.Text.Replace("'", "") + "','"+aa+"',NULL,NULL,getdate(),'" + hdn_setupout.Value + "','" + hdn_actout.Value + "',0,0,'" + bb + "','" + otdaymultiplier + "','" + otnightmultiplier + "','" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "' ) select scope_identity() id";
            DataTable dtinsertotline = dbhelper.getdata(queryyy);

            /**START MAIL NOTIFICATION**/
            function.Notification("ot", "alert", dtinsertotline.Rows[0]["id"].ToString(), bb);
            /**END**/

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);
            close();
        }
        else
            Response.Write("<script>alert('Input Data is already Exist!')</script>");
    }

    protected void add_offset(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            Label lbl_uthrs = (Label)row.Cells[21].FindControl("lbl_uthrs");
            Label lbl_aw = (Label)row.Cells[11].FindControl("lbl_aw");
            LinkButton lnk_offset = (LinkButton)row.Cells[21].FindControl("lnk_offset");
            DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
            DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
            decimal halfday = decimal.Parse(dt.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
            string[] payrange = ddl_pay_range.SelectedValue.Split('-');
            string[] datedate = row.Cells[3].Text.Split('-');
            if (ddl_pay_range.SelectedValue.Length > 1)
            {
                modal_os.Style.Add("display", "block");
                //string query = "select " +
                //"(select case when SUM(tobededuct_hrs) is null then 0 else SUM(tobededuct_hrs) end from toffset where appliedto is null and empid=" + row.Cells[24].Text.ToString() + " and convert(DATE,appliedfrom)>='" + payrange[0] + "' and  convert(DATE,appliedfrom)<='" + payrange[1] + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) " +
                //" + " +
                //"(select case when sum(overtimehours + overtimenighthours) is null then 0 else sum(overtimehours + overtimenighthours) end from tovertimeline where employeeid=" + row.Cells[24].Text.ToString() + " and convert(date,[date])>='" + payrange[0] + "' and convert(date,[date])<='" + payrange[1] + "' and (status  like'%Approved%' or  status  like '%For Approval%' or status like '%verification%')) t_hrs ";
                //DataTable dttt = dbhelper.getdata(query);
                string[] gggg = ddl_pay_range.Text.Split('-');
                DataTable dtr = ViewState["DTR_HANG"] as DataTable;


                decimal t_temp_ot = 0;
                decimal integerPart = 0;
                DataTable tmp_ot = new DataTable();
                tmp_ot.Columns.Add(new DataColumn("date", typeof(string)));
                tmp_ot.Columns.Add(new DataColumn("excess", typeof(string)));
                DataRow dtrdr;
                foreach (DataRow dr in dtr.Rows)
                {
                    if (dr["timeout2"].ToString().Length > 2)
                    {
                        DateTime dtout = Convert.ToDateTime(dr["timeout2"].ToString());
                        DateTime dtsetupout = Convert.ToDateTime(dr["setupfinalout"].ToString());
                        if (dtout > dtsetupout.AddMinutes(59))
                        {
                            TimeSpan finalot = dtout - dtsetupout;
                            integerPart = decimal.Parse(dr["tempot"].ToString());
                            string[] datee = dr["date"].ToString().Split('-');
                            DataTable getdt = dbhelper.getdata("select COALESCE(SUM(b.excess_hrs),0) excess_hrs from toffset a left join Toffset_date b on a.id=b.offset_id where (a.status like'%For Approval%' or a.status like'%verification%' or a.status like'%Approved%') and CONVERT(date,b.date_from)=convert(date,'" + datee[0] + "') and b.empid=" + Session["emp_id"] + " ");
                            DataTable getdtot = dbhelper.getdata("select * from tovertimeline where (status like '%For Approval%' or status like '%Approved%' or status like '%verification%') and CONVERT(date,date)= convert(date,'" + datee[0] + "') and employeeid=" + Session["emp_id"] + "");
                            string[] dateee = dr["date"].ToString().Split('-');
                            if (getdtot.Rows.Count == 0)
                            {
                                integerPart = getdt.Rows.Count > 0 ? integerPart - decimal.Parse(getdt.Rows[0]["excess_hrs"].ToString()) : integerPart;
                                if (integerPart > 0)
                                {
                                    dtrdr = tmp_ot.NewRow();
                                    dtrdr["date"] = dateee[0];
                                    dtrdr["excess"] = integerPart;
                                    tmp_ot.Rows.Add(dtrdr);
                                }
                            }

                            t_temp_ot = t_temp_ot + integerPart;
                        }
                    }
                }
                grid_temp_ot.DataSource = tmp_ot;
                grid_temp_ot.DataBind();
                txt_off_date.Text = DateTime.Parse(datedate[0]).ToString("MM/dd/yyyy");
                txt_off_out.Text = row.Cells[9].Text;
                txt_off_hrs.Text = "0";
                lbl_teh.Text = "0";

                if (row.Cells[6].Text.Length > 2 && row.Cells[9].Text.Length > 2)
                {
                    if (lbl_aw.Text == "✓")
                    {

                        hdn_max_offset.Value = halfday.ToString();
                    }
                    else
                        hdn_max_offset.Value = lbl_uthrs.Text;
                }
                else
                    hdn_max_offset.Value = dt.Rows[0]["FixNumberOfHours"].ToString();

                txt_off_hrs.Text = hdn_max_offset.Value;
            }
            else
                Response.Write("<script>alert('Incorrect Payroll Range!')</script>");
        }

    }
    protected void save_off_set(object sender, EventArgs e)
    {
        if (checkoffsave())
        {
            //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable emp = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + " ");
            decimal halfday = decimal.Parse(emp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
            string ret;
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("save_off_set", con))
                {
                    string hhuors = decimal.Parse(lbl_teh.Text) <= decimal.Parse(txt_off_hrs.Text) ? lbl_teh.Text : txt_off_hrs.Text;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@appliedfrom", SqlDbType.VarChar, 50).Value = txt_off_date.Text;
                    //cmd.Parameters.Add("@appliedto", SqlDbType.VarChar, 50).Value = ddl_doffset.SelectedValue;
                    cmd.Parameters.Add("@offsethrs", SqlDbType.VarChar, 50).Value = hhuors;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
                    cmd.Parameters.Add("@approverid", SqlDbType.Int).Value = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
                    cmd.Parameters.Add("@tobededuct_hrs", SqlDbType.VarChar, 50).Value = hhuors;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }

            if (int.Parse(ret) > 0)
            {
                foreach (GridViewRow gr in grid_temp_ot.Rows)
                {
                    string alloc_hrs = gr.Cells[1].Text;
                    CheckBox chk = (CheckBox)gr.FindControl("chk_");
                    if (chk.Checked == true)
                    {
                        if (hdn_alocated_bal.Text.Length > 0)
                        {
                            string[] arr = hdn_alocated_bal.Text.Split('-');
                            if (Convert.ToDateTime(gr.Cells[0].Text).Date == Convert.ToDateTime(arr[0]).Date)
                                alloc_hrs = (double.Parse(gr.Cells[1].Text) - double.Parse(arr[1].ToString())).ToString();
                        }
                        dbhelper.getdata("insert into Toffset_date (date,date_from,empid,offset_id,excess_hrs)values(getdate(),'" + gr.Cells[0].Text + "'," + Session["emp_id"].ToString() + "," + ret + ",'" + alloc_hrs + "') ");
                    }
                }
            }


            function.AddNotification("Time Adjustment Approval", "offset", approver_id.Rows[0]["under_id"].ToString(), ret);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);
            close();
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

        sql_income.ConnectionString = Config.connection();
        SetShiftCodes();
    }

    protected void SetShiftCodes()
    {
        DataTable sc = dbhelper.getdata("select shiftcode +' ('+ Remarks + ')' as shiftcode,id,remarks from  MShiftCode where status is null order by shiftcode ");
        ddl_shiftcode.Items.Clear();
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
            shitcode.Value = row.Cells[23].Text;


            /**BTK 120219
            * Determine current day if holiday**/
            SetShiftCodes();
            string[] date = row.Cells[3].Text.Split(' ');
            DataTable dt = dbhelper.getdata("select * from mdaytypeday where convert(varchar,[Date],101) = '" + date[0] + "'");
            if (dt.Rows.Count == 0)
                ddl_shiftcode.Items.Remove(ddl_shiftcode.Items.FindByText("PH (Public holiday)"));
        }
    }

    protected void click_save_changeshift(object sender, EventArgs e)
    {
        /**BETA 110519
         * EMAIL NOTIFICATION
         * - CHANGE SHIFT
         * - REQUEST APPROVAL
         * **/

        //DataTable dt = dbhelper.getdata("select * from temp_shiftcode where changeshift_id=" + key.Value + " and status not like '%Cancel%' order by date_change ");

        //if (dt.Rows.Count == 0)
        //{
        //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");


        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
        
        string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
        string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

        DataTable dtinsertcs = dbhelper.getdata("insert into temp_shiftcode (date_change,emp_id,changeshift_id,shiftcode_id,status,remarks,approver_id,old_shiftcodeid) values ('" + DateTime.Parse(txt_date.Text).ToShortDateString() + "','" + Session["emp_id"].ToString() + "'," + key.Value + "," + ddl_shiftcode.SelectedValue + ",'" + aa + "','" + txt_r.Text.Replace("'", "") + "','" + bb + "',"+shitcode.Value+") select scope_identity() id ");

        string x = dtinsertcs.Rows[0]["id"].ToString();
        string approver = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
        function.Notification("changeshift", "for approval", x, approver);

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
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
                string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
                string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
                
                DataTable dtinserthdrd = dbhelper.getdata("insert into TRestdaylogs (EmployeeId,shiftcodeId,Date,reason,status,dtr_id,sysdate,approver_id,class) values (" + Session["emp_id"].ToString() + "," + ddl_sc.SelectedValue + ",'" + txt_otd.Text.Trim() + "','" + txt_lineremarks.Text.Replace("'", "") + "','" + aa + "',NULL,GETDATE(),'" + bb + "','" + rbl_class.SelectedValue + "') select scope_identity() id");
                
                string x = dtinserthdrd.Rows[0]["id"].ToString();
                string approver = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
                function.Notification("workverification", "for approval", x, approver);

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
        DataTable sc = dbhelper.getdata("select shiftcode +' ('+ Remarks + ')' as shiftcode,id,remarks from  MShiftCode where status is null order by shiftcode ");

        foreach (DataRow dr in sc.Rows)
        {
            ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());

            item.Attributes.Add("title", dr["remarks"].ToString());

            //li.Attributes["title"] = li.Text;
            ddl_sc.Items.Add(item);

        }
    }
    protected void clickrd(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_status = (LinkButton)row.FindControl("lnk_status");
            if (lnk_status.Text.Contains("RD") || lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("CH"))
            {

                string[] datet = row.Cells[3].Text.Split(' ');
                txt_otd.Text=datet[0].ToString();
                DataTable dtchk = dbhelper.getdata("select * from TRestdaylogs where EMPLOYEEID=" + Session["emp_id"].ToString() + " AND CONVERT(date,[Date])= CONVERT(date,'" + datet[0] + "') and status not like'%Cancel%'");
                if (dtchk.Rows.Count == 0)
                {
                    WV.Style.Add("display", "block");
                    if (lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("CH"))
                    {
                        rbl_class.SelectedValue = "HD";
                    }
                    else if (lnk_status.Text.Contains("RD"))
                    {
                        rbl_class.SelectedValue = "RD";
                    }


                    //SHOW ATT LOGS
                    string query = "SELECT a.biotime Date_Time,a.id tdtrID " +
                    "FROM tdtrperpayrolperline a " +
                    "left join memployee b on a.empid=b.id " +
                    "where convert(varchar,a.biotime, 101)  =  convert(datetime,'" + datet[0] + "') and b.id=" + Session["emp_id"].ToString() + " order by a.biotime";

                    gv_attlog.DataSource = dbhelper.getdata(query);
                    gv_attlog.DataBind();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Exist!');", true);
                    close();
                }

            }
        }

    }

    protected void clickrdhdrdoffset(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            LinkButton lnk_status = (LinkButton)row.FindControl("lnk_status");
            if (lnk_status.Text.Contains("RD") || lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("DH"))
            {
                string[] datet = row.Cells[3].Text.Split(' ');
                txt_d_from.Text = datet[0].ToString();
                DataTable dtchk = dbhelper.getdata("select * from toffset where empid=" + Session["emp_id"].ToString() + " AND CONVERT(date,[appliedfrom])= CONVERT(date,'" + datet[0] + "') and (status like'%Approved%' or status like'%For Approval%'  or status like'%Verication%')");
                DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
                txt_noho.Text = dtemp.Rows[0]["FixNumberOfHours"].ToString();
                if (dtchk.Rows.Count == 0)
                {
                    hdrd_offset.Style.Add("display", "block");
                    if (lnk_status.Text.Contains("RH") || lnk_status.Text.Contains("SH") || lnk_status.Text.Contains("DH"))
                        rbl_rdhd.SelectedValue = "HD";
                    else if (lnk_status.Text.Contains("RD"))
                        rbl_rdhd.SelectedValue = "RD";
                }
                else
                {
                    Response.Write("<script>alert('Already Exist!')</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Exist!');", true);
                    close();
                }
            }
        }

    }

    protected void click_save_offset_hdrd(object sender, EventArgs e)
    {
        if (!offsetting())
        {
            alert.Visible = false;
            string[] range = ddl_pay_range.SelectedItem.Text.Split('-');
            DateTime start = Convert.ToDateTime(range[0]);
            DateTime end = Convert.ToDateTime(range[1]).AddDays(31);
            DateTime tto = Convert.ToDateTime(txt_d_to.Text);
            if (tto >= start && tto <= end)
            {
                DataTable dtr = Core.DTRF(txt_d_to.Text, txt_d_to.Text, "KIOSK_" + Session["emp_id"].ToString());
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");

                if (dtr.Rows.Count > 0)
                {
                    if (dtr.Rows[0]["aw"].ToString() == "True")
                    {
                        bool _continue = true;
                        if (rbl_rdhd.SelectedValue == "HD")
                        {
                            if (dtr.Rows[0]["ShiftCode"].ToString() != "OPH")
                            {
                                _continue = false;
                                lblInfo.Text = "OPH required";
                                pnlOffdanger.Visible = true;
                            }
                        }
                        else
                        {

                            string filter = "OILOIM";
                            if (!filter.Contains(dtr.Rows[0]["ShiftCode"].ToString()))
                            {
                                _continue = false;
                                lblInfo.Text = "OIL/OIM required";
                                pnlOffdanger.Visible = true;
                            }
                        }

                        if (_continue)
                        {
                            DataTable dtchkrdhd = dbhelper.getdata("select * from toffset where empid="+Session["emp_id"].ToString()+" and (CONVERT(date,appliedfrom)='" + txt_d_from.Text + "' or CONVERT(date,appliedto) ='" + txt_d_to.Text + "') and (status like'%For Approval%' or status like'%Approved%' or status like'%Verification%') and action is null and appliedto is not null");
                            if (dtchkrdhd.Rows.Count == 0)
                            {
                                string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
                                string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

                                string query = "INSERT INTO toffset(date,appliedfrom,appliedto,offsethrs,status,empid,userid,aproverid,remarks)VALUES(getdate(),'" + txt_d_from.Text + "','" + txt_d_to.Text + "','" + txt_noho.Text + "','" + aa + "'," + Session["emp_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + bb + "','" + txt_reason_offsethdrd.Text + "')";
                                dbhelper.getdata(query);

                                //SET MONITORING
                                dtchkrdhd = dbhelper.getdata("select * from toffset where (CONVERT(date,appliedfrom)='" + txt_d_from.Text + "' or CONVERT(date,appliedto) ='" + txt_d_to.Text + "') and (status like'%For Approval%' or status like'%Approved%' or status like'%Verification%') and action is null and appliedto is not null");
                                DataTable wv = dbhelper.getdata("select * from TRestdaylogs a where (a.status like '%Approved%') and CONVERT(date,a.date)=convert(date,'" + txt_d_from.Text + "') and a.EmployeeId=" + Session["emp_id"].ToString());
                                dbhelper.getdata("update TCompensatory set TOffset=" + dtchkrdhd.Rows[0]["id"].ToString() + " where TVerification=" + wv.Rows[0]["id"].ToString());

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);
                                close();
                            }
                            else
                            {
                                Response.Write("<script>alert('Not Available for Offset!')</script>");
                                close();
                            }
                        }
                    }
                    else
                    {
                        
                        lblInfo.Text =  "Invalid date";
                        pnlOffdanger.Visible = true;
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Out of range.')</script>");
                close();
            }
        }
    }

    protected bool offsetting()
    {
        Label5.Text = txt_d_to.Text.Trim().Length == 0 ? "*" : "";
        string required = Label5.Text;
        return required.Contains("*");
    }

    protected void close()
    {
        modal_otm.Style.Add("display", "NONE");
        modal_cws.Style.Add("display", "NONE");
        modal_os.Style.Add("display", "NONE");
        modal_ot.Style.Add("display", "NONE");
        modal_ta.Style.Add("display", "NONE");
        WV.Style.Add("display", "NONE");
        hdrd_offset.Style.Add("display", "NONE");
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
    protected void click_checked(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((CheckBox)sender).Parent.Parent)
        {
            DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"] + "");
            double total_tem_ot = lbl_teh.Text.Length == 0 ? 0 : double.Parse(lbl_teh.Text);
            CheckBox chk = (CheckBox)row.Cells[2].FindControl("chk_");
            switch (chk.Checked)
            {
                case true:
                    if (decimal.Parse(lbl_teh.Text) < decimal.Parse(txt_off_hrs.Text))
                    {
                        total_tem_ot = total_tem_ot + double.Parse(row.Cells[1].Text);
                        if (total_tem_ot <= double.Parse(txt_off_hrs.Text))
                            hdn_alocated_bal.Text = "";
                        else
                            hdn_alocated_bal.Text = row.Cells[0].Text + "-" + (total_tem_ot - double.Parse(txt_off_hrs.Text)).ToString();


                        lbl_teh.Text = total_tem_ot.ToString();
                        decimal halfday = decimal.Parse(dt.Rows[0]["fixnumberofhours"].ToString()) / 2;
                        if (decimal.Parse(lbl_teh.Text) >= halfday)
                        {
                            if (decimal.Parse(lbl_teh.Text) >= decimal.Parse(txt_off_hrs.Text))
                                Button3.Visible = true;
                        }
                        else
                            Button3.Visible = false;
                    }
                    else
                        chk.Checked = false;

                    if (decimal.Parse(lbl_teh.Text) > 0)
                        Button3.Visible = true;
                    else
                        Button3.Visible = false;
                    break;
                case false:

                    string[] arr = hdn_alocated_bal.Text.Split('-');
                    double ttt = hdn_alocated_bal.Text.Length > 0 ? double.Parse(arr[1]) : 0;
                    if (ttt >= double.Parse(row.Cells[1].Text))
                    {
                        ttt = ttt - double.Parse(row.Cells[1].Text);
                        hdn_alocated_bal.Text = ttt > 0 ? arr[0] + "-" + ttt.ToString() : "";
                    }
                    else
                        hdn_alocated_bal.Text = "";

                    lbl_teh.Text = (total_tem_ot - double.Parse(row.Cells[1].Text)).ToString();
                    if (decimal.Parse(lbl_teh.Text) > 0)
                        Button3.Visible = true;
                    else
                        Button3.Visible = false;

                    break;
            }
        }
    }

    [WebMethod]
    public static string deviceinformer(string emp)
    {
        string info = "";

        DataTable dt = dbhelper.getdata("Select * from tdtrperpayrol_deviceinfo where tdtrID=" + emp);
        if (dt.Rows.Count>0)
        {
            info += dt.Rows[0]["browser"].ToString() + "~";
            info += dt.Rows[0]["device"].ToString() + "~";
            info += dt.Rows[0]["latitude"].ToString() + "~";
            info += dt.Rows[0]["longitude"].ToString() + "~";
        }
        return info;
    }
    protected void click_otm(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal_otm.Style.Add("display", "block");
            TimeSpan final_ot;
            string nighthrs;
            hdn_reg_othrs.Text = "0";
            hdn_night_othrs.Text = "0";

            //OBT
            string obtstat = row.Cells[25].Text;
            if (obtstat == "✓")
            {
                string[] aw = row.Cells[3].Text.Split(' ');

                DataTable dtgetobtot = dbhelper.getdata("select(select convert(varchar,obOut,101))obOut2,* from obtline where empID=" + Session["emp_id"].ToString() + " and status='Approved' and (select convert(varchar,tDate,101))='" + aw[0] + "'");
                DataTable dtgetmanual = dbhelper.getdata("select(select DATEADD(mi,DATEDIFF(mi, 0,time_OUT)/30*30,0)outmins)manualout,(select DATEADD(mi,DATEDIFF(mi, 0,time_in)/30*30,0)outmins)manualin from Tmanuallogline where EmployeeId=" + Session["emp_id"].ToString() + " and status like '%Approved%' and (select convert(varchar,[Date],101))='" + aw[0] + "'");
                if (dtgetmanual.Rows.Count > 0)
                {
                    //Base OBT with Manual out
                    string[] eww = dtgetmanual.Rows[0]["manualout"].ToString().Split(' ');

                    if (aw[0] == eww[0])
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + dtgetmanual.Rows[0]["manualout"].ToString() + "')/30*30, 0)outmins");
                        DateTime obtout = Convert.ToDateTime(dtget30.Rows[0]["outmins"].ToString());
                        DateTime obtschdout = Convert.ToDateTime(row.Cells[29].Text);
                        final_ot = obtout - obtschdout;
                        nighthrs = getnightdif.getnight(obtschdout.ToString(), obtout.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            txt_totalotm.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                            hdn_night_othrs.Text = nighthrs;
                        }
                        txt_otmstart.Text = DateTime.Parse(row.Cells[29].Text).ToString();
                        txt_otmend.Text = DateTime.Parse(row.Cells[9].Text).ToString();
                        txt_outmealdate.Text = DateTime.Parse(dtgetmanual.Rows[0]["manualout"].ToString()).ToString("yyyy-MM-dd");
                        txt_outmealtime.Text = DateTime.Parse(dtgetmanual.Rows[0]["manualout"].ToString()).ToString("HH:mm");
                        hdn_setupout.Value = obtout.ToString();
                        hdn_actout.Value = obtschdout.ToString();
                    }
                }
                else
                {
                    DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + dtgetobtot.Rows[0]["obOut"].ToString() + "')/30*30, 0)outmins");
                    DateTime obtout = Convert.ToDateTime(dtget30.Rows[0]["outmins"].ToString());
                    DateTime obtschdout = Convert.ToDateTime(row.Cells[29].Text);
                    final_ot = obtout - obtschdout;
                    nighthrs = getnightdif.getnight(obtschdout.ToString(), obtout.ToString(), "dtr");
                    if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                    {
                        txt_totalotm.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                        hdn_night_othrs.Text = nighthrs;
                    }
                    txt_otmstart.Text = DateTime.Parse(row.Cells[29].Text).ToString();
                    txt_otmend.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString();
                    txt_outmealdate.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("yyyy-MM-dd");
                    txt_outmealtime.Text = DateTime.Parse(dtgetobtot.Rows[0]["obOut"].ToString()).ToString("HH:mm");
                    hdn_setupout.Value = obtout.ToString();
                    hdn_actout.Value = obtschdout.ToString();
                }
            }
            else
            {
                //if on Crossdates OT
                DataTable dtaddday = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + row.Cells[29].Text + "',101))crossdates");//sched out
                if (Convert.ToDateTime(row.Cells[9].Text).AddHours(-1) >= Convert.ToDateTime(dtaddday.Rows[0]["crossdates"].ToString()))
                {
                    DataTable dtout = dbhelper.getdata("select DATEADD(DAY,+1,convert(varchar,'" + row.Cells[29].Text + "',101))crossdates");
                    DateTime crossout = Convert.ToDateTime(row.Cells[9].Text);
                    DateTime schedout = Convert.ToDateTime(dtout.Rows[0]["crossdates"].ToString());
                    if (crossout > schedout)
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + crossout + "')/30*30, 0)outmins");
                        final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - schedout;
                        nighthrs = getnightdif.getnight(schedout.ToString(), crossout.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            txt_totalotm.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()));
                        }
                    }
                    txt_otmstart.Text = DateTime.Parse(row.Cells[29].Text).ToString();
                    txt_otmend.Text = DateTime.Parse(row.Cells[9].Text).ToString();
                    txt_outmealdate.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
                    txt_outmealtime.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
                }
                else
                {
                    DateTime final_out = Convert.ToDateTime(row.Cells[9].Text);
                    DateTime final_set_out = Convert.ToDateTime(row.Cells[29].Text);

                    if (final_out > final_set_out)
                    {
                        DataTable dtget30 = dbhelper.getdata("select DATEADD(mi,DATEDIFF(mi, 0, '" + final_out + "')/30*30, 0)outmins");
                        final_ot = Convert.ToDateTime(dtget30.Rows[0]["outmins"]) - final_set_out;
                        nighthrs = getnightdif.getnight(final_set_out.ToString(), final_out.ToString(), "dtr");
                        if (decimal.Parse(final_ot.TotalHours.ToString()) >= 1)
                        {
                            txt_totalotm.Text = string.Format("{0:n2}", decimal.Parse(final_ot.TotalHours.ToString()));
                        }
                    }
                    txt_otmstart.Text = DateTime.Parse(row.Cells[29].Text).ToString();
                    txt_otmend.Text = DateTime.Parse(row.Cells[9].Text).ToString();
                    txt_outmealdate.Text = DateTime.Parse(row.Cells[9].Text).ToString("yyyy-MM-dd");
                    txt_outmealtime.Text = DateTime.Parse(row.Cells[9].Text).ToString("HH:mm");
                    hdn_setupout.Value = final_set_out.ToString();
                    hdn_actout.Value = final_out.ToString();
                }
            }
        }
    }
    //for update
    protected void save_otmeal(object sender, EventArgs e)
    {
        /**BETA 110519
         * EMAIL NOTIFICATION
         * - OVERTIME REQUEST
         * - APPROVAL
         * **/
        DataTable tdcheck = dbhelper.getdata("select * from TMealHours where EmployeeId = " + Session["emp_id"].ToString() + " and left(convert(varchar,Date,101),10)='" + txt_outmealdate.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status)) like '%Approved%' or SUBSTRING(status,0, CHARINDEX('-',status)) like '%for verification%')");
        DataTable dtemp = dbhelper.getdata("select TOP(1)a.under_id, * from Approver a left join MEmployee b on a.emp_id=b.Id where b.id=" + Session["emp_id"].ToString() + " and a.herarchy <> 0");

        if (tdcheck.Rows.Count == 0)
        {
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
            string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
            string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

            string query = "insert into TMealHours values('" + Session["emp_id"] + "','" + txt_otmstart.Text + "','" + hdn_setupout.Value + "','" + hdn_actout.Value + "','" + txt_totalotm.Text + "','" + txt_otmnote.Text.Replace("'", "") + "','For Approval',NULL,GETDATE(),'" + dtemp.Rows[0]["under_id"].ToString() + "') select scope_identity() id";
            DataTable dtinsertotline = dbhelper.getdata(query);

            /**START MAIL NOTIFICATION**/
            function.Notification("mealOT", "alert", dtinsertotline.Rows[0]["id"].ToString(), bb);
            /**END**/

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);
            close();
        }
        else
            Response.Write("<script>alert('Input Data is already Exist!')</script>");
    }
}