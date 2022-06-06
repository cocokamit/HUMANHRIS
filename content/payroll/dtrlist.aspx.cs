using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_dtrlist : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            ds.Value = function.Decrypt(Request.QueryString["ds"].ToString(),true);
            de.Value = function.Decrypt(Request.QueryString["de"].ToString(),true);
            this.disp();

            sql_income.ConnectionString = Config.connection();

            //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GetTimeZoneOffset(); ", true);
        }
    }

    protected void disp()
    {
        DataTable dtshiftcode = dbhelper.getdata("select * from mshiftcode");
        DataTable emplist = dbhelper.getdata("select *,lastname+', '+firstname+' '+middlename e_name from memployee where id=" + Request.QueryString["a"].ToString() + "");
        ViewState["mastershift"] = dtshiftcode;
        string query = "select ROW_NUMBER() OVER(ORDER BY a.ID ASC) AS RowN, a.ID,a.shiftcodeid,left(convert(varchar,a.date,101),10)date, b.shiftcode," +
                "CONVERT(VARCHAR(10), a.timein1, 120) datein1, " +
                "(select LEFT(CONVERT(varchar,datestart,101),10)ds from tdtr where id=a.dtrid) ds, " +
                "(select LEFT(CONVERT(varchar,DateEnd,101),10)de from tdtr where id=a.dtrid) de, " +

                "case when a.timein1 ='--' then '0' else CONVERT(VARCHAR(5), CONVERT(TIME,a.timein1), 108)end timein1, " +
                "left(CONVERT(varchar,a.timeout2,101),10)dateout2, " +
                "case when a.timeOUT2 ='--' then '0' else CONVERT(VARCHAR(5), CONVERT(TIME,a.timeOUT2), 108) end timeout2, " +
                "cast(round(a.regularhours,18) as numeric(18,2))regularhours,cast(round(a.nighthours,18)as numeric(18,2))nighthours,a.totaloffsethrs,cast(round(a.overtimehours,18)as numeric(18,2))overtimehours,cast(round(a.overtimenighthours,18) as numeric(18,2))overtimenighthours,cast(round(a.tardylatehours,18)as numeric(18,2))tardylatehours,cast(round(a.tardyundertimehours,18)as numeric(18,2))tardyundertimehours, " +
                "cast(round(case when Absent='True' then fixnofhours else case when HalfdayAbsent='True' then fixnofhours*0.5 else 0 end end,18)as numeric(18,2)) absent, " +
                "cast(round(case when OnLeave='True' then fixnofhours else case when HalfLeave='True' then fixnofhours*0.5 else 0 end end,18)as numeric(18,2)) onleave, " +
                "case when daytypeid=1 then 'RWD' ELSE CASE WHEN daytypeid=3 THEN 'SH' else case when daytypeid=4 then 'LH' ELSE 'DH'  end  end end daystatus,a.remarks,a.restday, " +
                "case when DayMultiplier>1 then 'True' else 'False' end withpremium " +
                "from tdtrline a left join mshiftcode b on a.shiftcodeid=b.id where a.dtrid=" + function.Decrypt(Request.QueryString["b"].ToString(), true) + " and a.employeeid=" + Request.QueryString["a"].ToString() + "";
        DataTable dtt = dbhelper.getdata(query);
        ViewState["testing"] = dtt;
        gvDTR.DataSource = dtt;
        gvDTR.DataBind();

        DataTable dtpayrollchecking = dbhelper.getdata("select * from TPayroll where DTRId=" + function.Decrypt(Request.QueryString["b"].ToString(), true) + " and  status is null");
        hdn_dtr_id.Value = dtpayrollchecking.Rows.Count > 0 ? "1" : "0";
        hdn_ds.Value = dtt.Rows[0]["ds"].ToString();
        hdn_ds.Value = dtt.Rows[0]["de"].ToString();
        lbl_emp_name.Text = emplist.Rows[0]["e_name"].ToString();
        hdn_pg.Value = emplist.Rows[0]["payrollgroupid"].ToString();
        if (dtpayrollchecking.Rows.Count > 0)
        {
            gvDTR.Columns[15].Visible = false;
        }

    }
    protected void OnRowDataBoundgrid2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // DataTable shift = ViewState["mastershift"] as DataTable;
            LinkButton LinkButton3 = (LinkButton)e.Row.FindControl("LinkButton3");
            LinkButton LinkButton1 = (LinkButton)e.Row.FindControl("LinkButton1");
            DropDownList ddl_sc = (DropDownList)e.Row.FindControl("ddl_sc");
            Label lbl_date_in = (Label)e.Row.FindControl("lbl_date_in");
            Label lbl_date_out = (Label)e.Row.FindControl("lbl_date_out");
            TextBox lbl_offset_hrs = (TextBox)e.Row.FindControl("lbl_offset_hrs");
            Label lbl_absent = (Label)e.Row.FindControl("lbl_absent");

            TextBox txt_date_in = (TextBox)e.Row.FindControl("txt_date_in");
            TextBox txt_date_out = (TextBox)e.Row.FindControl("txt_date_out");
            TextBox txt_date_offset = (TextBox)e.Row.FindControl("txt_date_offset");
            TextBox txt_reg_ot = (TextBox)e.Row.FindControl("txt_reg_ot");
            TextBox txt_night_ot = (TextBox)e.Row.FindControl("txt_night_ot");
            TextBox txt_remarks = (TextBox)e.Row.FindControl("txt_remarks");

            Label lbl_status = (Label)e.Row.FindControl("lbl_status");
            Label lbl_shiftcodeid = (Label)e.Row.FindControl("lbl_shiftcodeid");

            Label lbl_rd = (Label)e.Row.FindControl("lbl_rd");
            Label lbl_premium = (Label)e.Row.FindControl("lbl_premium");

            Label lbl_latehrs = (Label)e.Row.FindControl("lbl_latehrs");
            Label lbl_uthrs = (Label)e.Row.FindControl("lbl_uthrs");

            CheckBox  chk_premium=(CheckBox)e.Row.FindControl("chk_premium");
            CheckBox  chk_rd=(CheckBox)e.Row.FindControl("chk_rd");
            
            string[] dateout = lbl_date_out.Text.Split('/');
            string finaldateout = lbl_date_out.Text.Contains('/') ? dateout[2].ToString() + "-" + dateout[0].ToString() + "-" + dateout[1].ToString() : "0";

            string[] datein = lbl_date_in.Text.Split('/');
            string finaldatein = lbl_date_in.Text.Contains('/') ? datein[2].ToString() + "-" + datein[0].ToString() + "-" + datein[1].ToString() : "0";

            txt_date_in.Text = finaldatein;
            txt_date_out.Text = finaldateout;

           // ddl_sc.SelectedValue = lbl_shiftcodeid.Text;

            if (hdn_row.Value.Length > 0)
            {
                if (e.Row.RowIndex == int.Parse(hdn_row.Value)-1)
                {
                    LinkButton3.Style.Add("color", "#337ab7");
                    LinkButton3.Enabled = true;
                    txt_reg_ot.Enabled = true;
                    txt_night_ot.Enabled = true;
                    txt_remarks.Enabled = true;
                    hdn_row.Value = "";
                }
            }
            if (hdn_row_after_saving.Value.Length > 0)
            {
                if (e.Row.RowIndex == int.Parse(hdn_row_after_saving.Value) - 1)
                {
                    txt_reg_ot.Enabled = false;
                    txt_night_ot.Enabled = false;
                    hdn_row_after_saving.Value = "";
                }
            }
            
            if (decimal.Parse(lbl_absent.Text) == 0)
            {
                txt_date_offset.Visible = false;
                lbl_offset_hrs.Enabled = false;
            }
            //lbl_status.Text != "RWD" ||
            if (lbl_premium.Text=="True")
                chk_premium.Checked = true;
            if (lbl_rd.Text == "True")
                chk_rd.Checked = true;
            if (decimal.Parse(lbl_absent.Text) > 0)
                lbl_absent.ForeColor = System.Drawing.Color.Red;
            if (decimal.Parse(lbl_offset_hrs.Text) > 0)
                lbl_offset_hrs.ForeColor = System.Drawing.Color.Purple;
            if (decimal.Parse(lbl_latehrs.Text) > 0)
                lbl_latehrs.ForeColor = System.Drawing.Color.Red;
            if (decimal.Parse(lbl_uthrs.Text) > 0)
                lbl_uthrs.ForeColor = System.Drawing.Color.Red;
          
        }
    }
    protected void recompute(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton LinkButton3 = (LinkButton)row.FindControl("LinkButton3");
               
            Label lbl_rown = (Label)row.FindControl("lbl_rown");
            DropDownList ddl_sc = (DropDownList)row.FindControl("ddl_sc");
            TextBox txt_date_in = (TextBox)row.FindControl("txt_date_in");
            TextBox txt_time_in = (TextBox)row.FindControl("txt_time_in");
            TextBox txt_date_out = (TextBox)row.FindControl("txt_date_out");
            TextBox txt_time_out = (TextBox)row.FindControl("txt_time_out");
            TextBox txt_reg_ot = (TextBox)row.FindControl("txt_reg_ot");
            TextBox txt_night_ot = (TextBox)row.FindControl("txt_night_ot");
            TextBox lbl_offset_hrs=(TextBox)row.FindControl("lbl_offset_hrs");
            Label lbl_absent = (Label)row.FindControl("lbl_absent");
            CheckBox chk_rd=(CheckBox)row.FindControl("chk_rd");
            CheckBox chk_premium=(CheckBox)row.FindControl("chk_premium");
            Label lbl_status = (Label)row.FindControl("lbl_status");

            string scid = ddl_sc.SelectedValue;
            string ddate = row.Cells[1].Text;
            string fff = txt_date_in.Text + " " + txt_time_in.Text;
            string ttt = txt_date_out.Text + " " + txt_time_out.Text;
            string empid = Request.QueryString["a"].ToString();
            DataTable masterdata=ViewState["testing"] as DataTable;
            string offsethrrss=decimal.Parse(lbl_offset_hrs.Text) > decimal.Parse(lbl_absent.Text)?lbl_absent.Text:lbl_offset_hrs.Text;
            string chk_rd_value=chk_rd.Checked==true?"True":"False";
            string chk_hd_value = lbl_status.Text == "RWD" ? "False" : "True";
            string chk_premium_value=chk_premium.Checked==true?"True":"False";


            DataTable dtr = adjustdtrformat.overriding(fff, ttt, scid, ddate, empid, offsethrrss, chk_rd_value, chk_premium_value, chk_hd_value);
             string query = "select " + dtr.Rows[0]["shiftcodeid"].ToString() + " shiftcodeid, " +
             "case when '" + dtr.Rows[0]["timein1"].ToString().Length + "'>2 then CONVERT(VARCHAR(10), '" + dtr.Rows[0]["timein1"].ToString() + "', 120) else '--' end datein1, " +
             "case when '" + dtr.Rows[0]["timein1"].ToString() + "' ='--' then '0' else CONVERT(VARCHAR(5), CONVERT(TIME,'" + dtr.Rows[0]["timein1"].ToString() + "'), 108)end timein1, " +
             "case when '" + dtr.Rows[0]["timeOUT2"].ToString().Length + "'>2 then CONVERT(VARCHAR(10), '" + dtr.Rows[0]["timeOUT2"].ToString() + "', 120) else '--' end dateout2, " +
             "case when '" + dtr.Rows[0]["timeOUT2"].ToString() + "' ='--' then '0' else CONVERT(VARCHAR(5), CONVERT(TIME,'" + dtr.Rows[0]["timeOUT2"].ToString() + "'), 108)end timeout2, " +
             "cast(round('" + dtr.Rows[0]["reg_hr"].ToString() + "',18) as numeric(18,2))regularhours,cast(round('" + dtr.Rows[0]["night"].ToString() + "',18)as numeric(18,2))nighthours,cast(round('" + dtr.Rows[0]["offsethrs"].ToString() + "',18)as numeric(18,2))offsethrs,cast(round('" + dtr.Rows[0]["ot"].ToString() + "',18)as numeric(18,2))overtimehours,cast(round('" + dtr.Rows[0]["otn"].ToString() + "',18) as numeric(18,2))overtimenighthours,cast(round('" + dtr.Rows[0]["late"].ToString() + "',18)as numeric(18,2))tardylatehours,cast(round('" + dtr.Rows[0]["ut"].ToString() + "',18)as numeric(18,2))tardyundertimehours, " +
             "cast(round(case when '" + dtr.Rows[0]["aw"].ToString() + "'='True' then '" + dtr.Rows[0]["fixnumberofhours"].ToString() + "' else case when '" + dtr.Rows[0]["ah"].ToString() + "'='True' then convert(float,'" + dtr.Rows[0]["fixnumberofhours"].ToString() + "')*0.5 else 0 end end,18)as numeric(18,2)) absent, " +
             "cast(round(case when '" + dtr.Rows[0]["olw"].ToString() + "'='True' then '" + dtr.Rows[0]["fixnumberofhours"].ToString() + "' else case when '" + dtr.Rows[0]["olh"].ToString() + "'='True' then convert(float,'" + dtr.Rows[0]["fixnumberofhours"].ToString() + "')*0.5 else 0 end end,18)as numeric(18,2)) onleave, " +
             " '" + dtr.Rows[0]["restday"].ToString() + "' restday, " +
             "case when cast(round(" + dtr.Rows[0]["dm"].ToString() + ",2)as numeric(18,2)) > 1 then 'True' else 'False' end withpremium," +
             "case when '" + dtr.Rows[0]["daytypeid"].ToString() + "' = 1 then 'RWD' ELSE CASE WHEN '" + dtr.Rows[0]["daytypeid"].ToString() + "'=3 THEN 'SH' else case when '" + dtr.Rows[0]["daytypeid"].ToString() + "'=4 then 'LD' ELSE 'DH'  end  end end daystatus ";
            DataTable dtt = dbhelper.getdata(query);
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["shiftcodeid"] = dtt.Rows[0]["shiftcodeid"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["datein1"] = dtt.Rows[0]["datein1"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["timein1"] = dtt.Rows[0]["timein1"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["dateout2"] = dtt.Rows[0]["dateout2"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["timeout2"] = dtt.Rows[0]["timeout2"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["regularhours"] = dtt.Rows[0]["regularhours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["nighthours"] = dtt.Rows[0]["nighthours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["totaloffsethrs"] = dtt.Rows[0]["offsethrs"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["overtimehours"] = dtt.Rows[0]["overtimehours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["overtimenighthours"] = dtt.Rows[0]["overtimenighthours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["tardylatehours"] = dtt.Rows[0]["tardylatehours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["tardyundertimehours"] = dtt.Rows[0]["tardyundertimehours"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["absent"] = dtt.Rows[0]["absent"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["onleave"] = dtt.Rows[0]["onleave"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text)-1]["daystatus"] = dtt.Rows[0]["daystatus"].ToString();

            masterdata.Rows[int.Parse(lbl_rown.Text) - 1]["restday"] = dtt.Rows[0]["restday"].ToString();
            masterdata.Rows[int.Parse(lbl_rown.Text) - 1]["withpremium"] = dtt.Rows[0]["withpremium"].ToString();
            hdn_row.Value = lbl_rown.Text;
            gvDTR.DataSource = masterdata;
            gvDTR.DataBind();
            ViewState["finalsave"] = dtr;
       
        }
    }
    protected void submit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //TextBox txt_reg_ot = (TextBox)row.FindControl("txt_reg_ot");
            //TextBox txt_night_ot = (TextBox)row.FindControl("txt_night_ot");
            //TextBox txt_remarks = (TextBox)row.FindControl("txt_remarks");
            //DataTable dtr = ViewState["finalsave"] as DataTable;
            //Label lbl_rown = (Label)row.FindControl("lbl_rown");
            //Label lbl_id = (Label)row.FindControl("lbl_id");
           
            //hdn_row_after_saving.Value = lbl_rown.Text;
            //int i = 0;
            //foreach (DataRow dr in dtr.Rows)
            //{
            //    string[] getdate = dr["date"].ToString().Split('-'); //lbl_date.Text.Trim().Split('-');
            //    string[] dateformat = getdate[0].Split('/');
            //    string mm = dateformat[0].ToString().Length > 1 ? dateformat[0] : "0" + "" + dateformat[0].ToString();
            //    string dd = dateformat[1].ToString().Length > 1 ? dateformat[1] : "0" + "" + dateformat[1].ToString();
            //    DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + dr["empid"].ToString() + "");
            //    stateclass sc = new stateclass();
            //    sc.sa = "0"; //DTRId
            //    //sc.sb = dr["empid"].ToString(); //ddl_emp.SelectedValue; //EmployeeId
            //    //sc.sc = dr["shiftcodeid"].ToString(); //ddl_shiftcode.SelectedValue; //ShiftCodeId
            //    //sc.sd = mm + "/" + dd + "/" + dateformat[2]; //getdate[0];//date
            //    //sc.se = dr["daytypeid"].ToString();//ddl_type.SelectedValue; //DayTypeId
            //    //sc.sf = dr["dm"].ToString();//lbl_dm.Text; //DayMultiplier
            //    //sc.sg = dr["restday"].ToString();//chk_rd.Checked ? "True" : "False";//RestDay
            //    //sc.sh = dr["timein1"].ToString();//lbl_timein1.Text; //TimeIn1
            //    //sc.si = dr["timeout2"].ToString();//lbl_timeout2.Text;//TimeOut2
            //    //sc.sj = dr["olw"].ToString();//chk_ol.Checked ? "True" : "False";//OnLeave
            //    //sc.sjj = dr["olh"].ToString();//chk_olH.Checked == true ? "True" : "False";//onleave halfday
            //    //sc.sk = dr["aw"].ToString();//chk_a.Checked ? "True" : "False";//Absent
            //    //sc.sii = dr["ah"].ToString();//chk_aH.Checked == true ? "True" : "False";// absent halfday
            //    //sc.sl = dr["reg_hr"].ToString().Replace(",", ""); ;//lbl_reg_workhours.Text;//RegularHours
            //    //sc.sqq = dr["offsethrs"].ToString().Replace(",", ""); ;//@totaloffsethrs
            //    //sc.sm = dr["night"].ToString().Replace(",", ""); //lbl_night.Text;//NightHours
            //    //sc.sn = txt_reg_ot.Text.Replace(",", ""); // lbl_ot.Text;//OvertimeHours
            //    //sc.so = txt_night_ot.Text.Replace(",", "");// lbl_otn.Text;//OvertimeNightHours
            //    //sc.sp = dr["totalamt"].ToString().Replace(",", "");//lbl_total.Text;//GrossTotalHours
            //    //sc.sq = dr["late"].ToString().Replace(",", "");//lbl_late.Text;//TardyLateHours
            //    //sc.sr = dr["ut"].ToString().Replace(",", "");// lbl_ut.Text;//TardyUndertimeHours
            //    //sc.ss = dr["nethours"].ToString().Replace(",", "");// lbl_nethours.Text;//NetTotalHour
            //    //sc.st = dr["hrrate"].ToString().Replace(",", "");// lbl_regrateperhour.Text;//RatePerHour
            //    //sc.su = dr["nightrate"].ToString().Replace(",", "");// lbl_nightrateperhour.Text;//RatePerNightHour
            //    //sc.sv = dr["otrate"].ToString().Replace(",", "");// lbl_otrateperhour.Text;//RatePerOvertimeHour
            //    //sc.sw = dr["otnrate"].ToString().Replace(",", "");// lbl_otnrateperhour.Text;//RatePerOvertimeNightHour
            //    //sc.sx = dr["reg_amount"].ToString().Replace(",", "");//lbl_regamount.Text;//RegularAmount
            //    //sc.sy = dr["night_amount"].ToString().Replace(",", "");//lbl_nightamount.Text;//NightAmount
            //    //sc.sz = (decimal.Parse(txt_reg_ot.Text.Replace(",", "")) * decimal.Parse(dr["otrate"].ToString().Replace(",", ""))).ToString();//dr["ot_amount"].ToString().Replace(",", "")   lbl_otamount.Text;//OvertimeAmount
            //    //sc.saa = (decimal.Parse(txt_night_ot.Text.Replace(",", "")) * decimal.Parse(dr["otnrate"].ToString().Replace(",", ""))).ToString();// dr["otn_amount"].ToString().Replace(",", "");//lbl_otnamount.Text;//OvertimeNightAmount
            //    //sc.sbb = dr["totalamt"].ToString().Replace(",", "");//lbl_totalamt.Text;//TotalAmount
            //    //sc.scc = dtemp.Rows[0]["HourlyRate"].ToString();//lbl_trph.Text;//RatePerHourTardy
            //    //sc.sdd = dr["dailyrate"].ToString().Replace(",", "");//lbl_ARPD.Text;//RatePerAbsentDay
            //    //sc.see = dr["tardyamt"].ToString().Replace(",", ""); // lbl_tardyamount.Text;//TardyAmount
            //    //sc.sff = dr["absentamt"].ToString().Replace(",", "");// lbl_absentamount.Text;//AbsentAmount
            //    //sc.sgg = dr["netamt"].ToString().Replace(",", "");// lbl_netamount.Text;//NetAmount
            //    //sc.shh = dr["leaveamt"].ToString().Replace(",", "");// lbl_leaveamount.Text;//leaveAmount
            //    //sc.skk = dr["restdayamt"].ToString().Replace(",", "");// lbl_rdamt.Text;//rdamt
            //    //sc.sll = dr["holidayamt"].ToString().Replace(",", "");// lbl_hdamt.Text;//hdamt
            //    //sc.smm = dr["lateamt"].ToString().Replace(",", "");// lbl_latea.Text; //lamount
            //    //sc.snn = dr["lbl_undera"].ToString().Replace(",", "");// lbl_undera.Text;//uamount
            //    //sc.soo = dr["timein2"].ToString(); //lbl_latea.Text; //lamount
            //    //sc.spp = dr["timeout1"].ToString(); //lbl_undera.Text;//uamount
            //    //sc.srr = "0";
            //    //sc.sss = dr["overbreak"].ToString().Replace(",", "");
            //    //sc.stt = Session["user_id"].ToString();
            //    //sc.suu = lbl_id.Text;
            //    //sc.svv = txt_remarks.Text;




            //    sc.sb = dr["empid"].ToString();//ddl_emp.SelectedValue; //EmployeeId
            //    sc.sc = dr["shiftcodeid"].ToString();//ddl_shiftcode.SelectedValue; //ShiftCodeId
            //    sc.sd = mm + "/" + dd + "/" + dateformat[2]; //getdate[0];//date
            //    sc.se = dr["daytypeid"].ToString();//ddl_type.SelectedValue; //DayTypeId
            //    sc.sf = dr["dm"].ToString();//lbl_dm.Text; //DayMultiplier
            //    sc.sg = dr["restday"].ToString();//chk_rd.Checked ? "True" : "False";//RestDay
            //    sc.sh = dr["timein1"].ToString();//lbl_timein1.Text; //TimeIn1
            //    sc.si = dr["timeout2"].ToString();//lbl_timeout2.Text;//TimeOut2
            //    sc.sj = dr["olw"].ToString();//chk_ol.Checked ? "True" : "False";//OnLeave
            //    sc.sjj = dr["olh"].ToString();//chk_olH.Checked == true ? "True" : "False";//onleave halfday
            //    sc.sk = dr["aw"].ToString();//chk_a.Checked ? "True" : "False";//Absent
            //    sc.sii = dr["ah"].ToString();//chk_aH.Checked == true ? "True" : "False";// absent halfday
            //    sc.sl = dr["reg_hr"].ToString().Replace(",", ""); ;//lbl_reg_workhours.Text;//RegularHours
            //    sc.sqq = dr["offsethrs"].ToString().Replace(",", ""); ;//@totaloffsethrs
            //    sc.sm = dr["night"].ToString().Replace(",", ""); //lbl_night.Text;//NightHours
            //    sc.sn = txt_reg_ot.Text.Replace(",", ""); // lbl_ot.Text;//OvertimeHours
            //    sc.so = txt_night_ot.Text.Replace(",", "");// lbl_otn.Text;//OvertimeNightHours
            //    sc.sp = dr["totalhrs"].ToString().Replace(",", "");//lbl_total.Text;//GrossTotalHours
            //    sc.sq = dr["late"].ToString().Replace(",", "");//lbl_late.Text;//TardyLateHours
            //    sc.sr = dr["ut"].ToString().Replace(",", "");// lbl_ut.Text;//TardyUndertimeHours
            //    sc.ss = dr["nethours"].ToString().Replace(",", "");// lbl_nethours.Text;//NetTotalHour
            //    sc.st = dr["hrrate"].ToString().Replace(",", "");// lbl_regrateperhour.Text;//RatePerHour
            //    sc.su = dr["nightrate"].ToString().Replace(",", "");// lbl_nightrateperhour.Text;//RatePerNightHour
            //    sc.sv = dr["otrate"].ToString().Replace(",", "");// lbl_otrateperhour.Text;//RatePerOvertimeHour
            //    sc.sw = dr["otnrate"].ToString().Replace(",", "");// lbl_otnrateperhour.Text;//RatePerOvertimeNightHour
            //    sc.sx = dr["reg_amount"].ToString().Replace(",", "");//lbl_regamount.Text;//RegularAmount
            //    sc.sy = dr["night_amount"].ToString().Replace(",", "");//lbl_nightamount.Text;//NightAmount
            //    sc.sz = (decimal.Parse(txt_reg_ot.Text.Replace(",", "")) * decimal.Parse(dr["otrate"].ToString().Replace(",", ""))).ToString();//dr["ot_amount"].ToString().Replace(",", "")   lbl_otamount.Text;//OvertimeAmount
            //    sc.saa = (decimal.Parse(txt_night_ot.Text.Replace(",", "")) * decimal.Parse(dr["otnrate"].ToString().Replace(",", ""))).ToString();// dr["otn_amount"].ToString().Replace(",", "");//lbl_otnamount.Text;//OvertimeNightAmount
            //    sc.sbb = dr["totalamt"].ToString().Replace(",", "");//lbl_totalamt.Text;//TotalAmount
            //    sc.scc = dr["reghourlyrate"].ToString();//lbl_trph.Text;//RatePerHourTardy
            //    sc.sdd = dr["dailyrate"].ToString().Replace(",", "");//lbl_ARPD.Text;//RatePerAbsentDay
            //    sc.see = dr["tardyamt"].ToString().Replace(",", ""); // lbl_tardyamount.Text;//TardyAmount
            //    sc.sff = dr["absentamt"].ToString().Replace(",", "");// lbl_absentamount.Text;//AbsentAmount
            //    sc.sgg = dr["netamt"].ToString().Replace(",", "");// lbl_netamount.Text;//NetAmount
            //    sc.shh = dr["leaveamt"].ToString().Replace(",", "");// lbl_leaveamount.Text;//leaveAmount
            //    sc.skk = dr["restdayamt"].ToString().Replace(",", "");// lbl_rdamt.Text;//rdamt
            //    sc.sll = dr["holidayamt"].ToString().Replace(",", "");// lbl_hdamt.Text;//hdamt
            //    sc.smm = dr["lateamt"].ToString().Replace(",", "");// lbl_latea.Text; //lamount
            //    sc.snn = dr["lbl_undera"].ToString().Replace(",", "");// lbl_undera.Text;//uamount

            //    sc.soo = dr["timein2"].ToString(); //lbl_latea.Text; //lamount
            //    sc.spp = dr["timeout1"].ToString(); //lbl_undera.Text;//uamount
            //    sc.srr = "0";
            //    sc.sss = dr["overbreak"].ToString().Replace(",", "");
            //    sc.stt = dr["surge_pay"].ToString().Replace(",", "");
            //    sc.suu = dr["dailyrate"].ToString().Replace(",", "");
            //    sc.svv = dr["monthlyrate"].ToString().Replace(",", "");
            //    sc.sww = dr["payrollrate"].ToString().Replace(",", "");
            //    sc.sxx = dr["app_trn_id"].ToString();
            //    sc.syy = Session["user_id"].ToString();
            //    sc.szz = lbl_id.Text;
            //    sc.saaa = txt_remarks.Text;
            //    string dtrperline = bol.updatedtrperline(sc);
            //    i++;
            //}
            //this.disp();
        }
    }
    protected void close(object sender, EventArgs e)
    {
       // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "window.location='payrollsum?&dtrid=" + function.Encrypt(function.Decrypt(Request.QueryString["b"].ToString(), true), true) + "&pg=" + function.Encrypt(hdn_pg.Value, true) + "&ds=" + function.Encrypt(hdn_ds.Value, true) + "&de=" + function.Encrypt(de.Value, true) + "" + "#" + Request.QueryString["a"].ToString() + "'", true);
       // ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");

        Response.Redirect("payrollsum?&dtrid=" + function.Encrypt(function.Decrypt(Request.QueryString["b"].ToString(), true), true) + "&pg=" + function.Encrypt(hdn_pg.Value, true) + "&ds=" + function.Encrypt(hdn_ds.Value, true) + "&de=" + function.Encrypt(de.Value, true) + "" + "&tar=" + Request.QueryString["a"].ToString() + "",true);
    }
  
}