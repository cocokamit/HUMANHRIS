using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;

public partial class content_Admin_EmpDTR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            pay_range();

            if (System.Web.HttpContext.Current.Session["idman"] != null && System.Web.HttpContext.Current.Session["fullname"] != null)
            {
                if (System.Web.HttpContext.Current.Session["idman"].ToString() != "" && System.Web.HttpContext.Current.Session["fullname"].ToString() != "")
                {
                    hfempid.Value = System.Web.HttpContext.Current.Session["idman"].ToString();
                    tb_search.Text = System.Web.HttpContext.Current.Session["fullname"].ToString();
                    disp();
                }
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "deyum", "asd()", true);
        }
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id,a.LastName+', '+a.firstname+' '+a.MiddleName+' ' fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id left join nobel_user c on c.emp_id = a.Id where c.name like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void click_search(object sender, EventArgs e)
    {
        hdn_total_tempot.Value = "";
        if (ddl_pay_range.SelectedValue != "0")
        {
            string[] hhh = ddl_pay_range.Text.Split('-');
            txt_from.Text = hhh[0].ToString();
            txt_to.Text = hhh[1].ToString();
        }
        if (hfempid.Value != "")
        {
            Session["idman"] = hfempid.Value;
            Session["fullname"] = fullnamess.Value;

            disp();
        }
    }

    protected void pay_range()
    {
        DataTable dt = dbhelper.getdata("select * from memployee order by LastName asc");
        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto," +
         "(select COUNT(*)  from payroll_range where CONVERT(date,dfrom) <= CONVERT(date,GETDATE()) AND CONVERT(date,dtoo) >= CONVERT(date,GETDATE()) and id=a.id) cnt " +
         "from payroll_range a where action is null order by dfrom desc";
        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
        //ddl_pay_range.Items.Add(new ListItem("Select Payroll Range","0"));

        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }

        //ddl_emp_num.Items.Clear();

        //foreach (DataRow dr in dt.Rows)
        //{
        //    ddl_emp_num.Items.Add(new ListItem(dr["LastName"].ToString() + ", " + dr["Firstname"].ToString() + " - " + dr["IdNumber"].ToString(), dr["Id"].ToString()));
        //}

        //hfempid.Value = ddl_emp_num.Items[0].Value.ToString();

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

        //if (ddl_emp_num.SelectedValue != "0")
        //{
        //    hfempid.Value = ddl_emp_num.SelectedValue.ToString();
        //}

        ////Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "dtrfunction()", true);
        //disp();
    }


    protected void disp()
    {
        if (hfempid.Value != "")
        {
            string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
            string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
            DataTable dt = dbhelper.getdata("select * from memployee where id=" + hfempid.Value);
            if (dt.Rows.Count > 0)
            {
                DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());

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

                DataTable dtr = adjustdtrformat2.DTRF(from, to, "KIOSK_" + hfempid.Value);

                dtr.Columns.Add("Remarks", typeof(String));
                decimal t_temp_ot = 0;
                int integerPart = 0;
                if (hdn_total_tempot.Value.Length == 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        integerPart = (int)System.Math.Floor(decimal.Parse(dr["tempot"].ToString()));
                        t_temp_ot = t_temp_ot + integerPart;
                    }
                }

                DataTable empapplog = dbhelper.getdata("Select * from sys_applog2 order by datechange desc");

                for (int i = 0; i < dtr.Rows.Count; i++)
                {
                    string[] rowdate = dtr.Rows[i]["date"].ToString().Split('-');
                    DataRow[] row = empapplog.Select("atdate='" + rowdate[0] + " 00:00:00.000' AND empid=" + hfempid.Value);
                    if (row.Count() > 0)
                    {
                        dtr.Rows[i]["remarks"] = row[0]["remarks"].ToString();
                    }
                }
                hdn_total_tempot.Value = t_temp_ot.ToString();
                grid_item.DataSource = dtr;
                grid_item.DataBind();
                Session["SuperSecret"] = dtr;
            }
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static void disps(string hfempid, string cutoffspan, string txt_from, string txt_to)
    {

        string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
        DataTable dt = dbhelper.getdata("select * from memployee where id=" + hfempid);
        if (dt.Rows.Count > 0)
        {
            DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());

            string[] gggg = cutoffspan.Split('-');
            string from = txt_from.Length > 0 ? txt_from : month + "/01/" + DateTime.Now.Year;
            string to = txt_to.Length > 0 ? txt_to : month + "/" + day + "/" + DateTime.Now.Year;
            if (cutoffspan == "0")
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

            DataTable dtr = adjustdtrformat2.DTRF(from, to, "KIOSK_" + hfempid);

            HttpContext.Current.Session["SuperSecret"] = dtr;

        }
    }

    protected void btnSample_Click(object sender, EventArgs e)
    {
        grid_item.DataSource = Session["SuperSecret"] as DataTable;
        grid_item.DataBind();
    }

    protected void orcd(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("ddl_shiftcode") as DropDownList).Enabled = false;
            (e.Row.FindControl("txtin1date") as TextBox).Enabled = false;
            (e.Row.FindControl("txtin1time") as TextBox).Enabled = false;
            (e.Row.FindControl("txtout2date") as TextBox).Enabled = false;
            (e.Row.FindControl("txtout2time") as TextBox).Enabled = false;
            (e.Row.FindControl("ddl_absent") as DropDownList).Enabled = false;
            (e.Row.FindControl("setot") as TextBox).Enabled = false;
            (e.Row.FindControl("setotn") as TextBox).Enabled = false;
            (e.Row.FindControl("txtremarks") as TextBox).Enabled = false;
        }
    }


    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlsc = (e.Row.FindControl("ddl_shiftcode") as DropDownList);
            DataTable dt = new DataTable();
            Label txtdate = (e.Row.FindControl("txtdate") as Label);
            ddlsc.DataSource = dt = dbhelper.getdata("Select ShiftCode,Id,Remarks from MShiftCode where status is null order by ShiftCode asc");
            ddlsc.DataTextField = "ShiftCode";
            ddlsc.DataValueField = "Id";
            ddlsc.DataBind();

            for (int i = 0; i < ddlsc.Items.Count; i++)
            {
                ddlsc.Items[i].Attributes.Add("title", dt.Rows[i]["Remarks"].ToString());
            }

            string sc = e.Row.Cells[26].Text;
            if (sc != "")
            {
                ddlsc.Items.FindByValue(sc).Selected = true;
            }
            DropDownList ddabsent = (e.Row.FindControl("ddl_absent") as DropDownList);

            sc = e.Row.Cells[12].Text;
            if (sc != "")
            {
            ddabsent.Items.FindByValue(sc).Selected = true;
                }
        }
    }

    [WebMethod]
    public static string IfRD(string empid, string ff, string shiftc)
    {
        string result = "no";

        if (shiftc != "")
        {
            string shiftcodeid = shiftc;
            DataRow[] getscc = adjustdtrformat2.dtchksc.Select("id=" + shiftcodeid + "");

            if (getscc.Count() > 0)
            {
                if (getscc[0]["ShiftCode"].ToString() == "RD")
                {
                    result = "yes";
                }
            }
        }
        return result;
    }

    [WebMethod]
    public static void SaveSC(string empid, string shiftc, string datepoint, string remarks)
    {

        string datenow = DateTime.Now.ToString();
        string[] datenower = datepoint.Split('-');
        string datenowv2 = Convert.ToDateTime(datenow).ToString("MM/dd/yyyy");

        string query = "";
        //------------------Change Shift---------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (shiftc != "")
        {
            string aa = "select * from TchangeshiftLine where [date]=left(convert(varchar,'" + datenower[0] + "',101),10) and employeeId='" + empid + "' and ShiftCodeId='" + shiftc + "'  ";
            DataTable hh = new DataTable();
            hh = dbhelper.getdata(aa);

            if (hh.Rows.Count > 0)
            {
                query = "update TChangeShiftLine set ShiftCodeId='" + shiftc + "' where Id=" + hh.Rows[0]["Id"].ToString() + "";
                dbhelper.getdata(query);
            }

            if (hh.Rows.Count == 0)
            {
                query = "insert into TChangeShiftLine (ChangeShiftId,EmployeeId,Date,ShiftCodeId,Remarks,status,dtr_id) values (0," + empid + ",'" + datenower[0] + "','" + shiftc + "','" + remarks + "','approved',NULL) ";
                dbhelper.getdata(query);
            }
        }

    }
    

    [WebMethod]
    public static void SaveInfo(string empid, string shiftc, string timein1, string timeout1, string absenter, string datepoint, string os, string ot, string otn, string late, string ut, string remarks, string reghrs, string night, string finalout, string osdates, string excesses, string offsethr)
    {
        string info = "";
        string datenow = DateTime.Now.ToString();
        string tos = os == "" ? "0.00" : os;
        string tot = ot == "" ? "0.00" : ot;
        string totn = otn == "" ? "0.00" : otn;
        string tut = ut == "" ? "0.00" : ut;
        reghrs = reghrs == "" ? "0.00" : reghrs;
        night = night == "" ? "0.00" : night;
        string[] datenower = datepoint.Split('-');
        string datenowv2 = Convert.ToDateTime(datenow).ToString("MM/dd/yyyy");
        dbhelper.getdata("Insert into sys_applog2 values('" + shiftc + "','" + timein1 + "','" + timeout1 + "','" + absenter + "'," + tos + "," + tot + "," + totn + "," + late + "," + tut + ",'RWD','" + datenow + "'," + empid + ",'" + datenower[0] + "','" + remarks + "'," + reghrs + "," + night + ")");


        //-------------------Failure Time-in/Time-out---------------------------------------------------------------------------------------------------------------------------------------------------------------
        DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + empid + "");
        DataTable daytype = dbhelper.getdata("select *, a.id,a.DayType,a.workingdays,a.RestdayDays,case when b.BranchId is null then '0' else b.BranchId end BranchId ,case when left(convert(varchar,b.Date,101),10) is null then '0' else left(convert(varchar,b.Date,101),10) end datte from MDayType a left join MDayTypeDay b on a.Id=b.DayTypeId where b.status is null order by a.id asc ");
        DataRow[] dtgetallDayType = daytype.Select("datte='" + datenower[0] + "' and BranchId='" + dtemp.Rows[0]["branchid"].ToString() + "'");

        decimal daymultiplier = 0;
        decimal nightmultiplier = 0;
        decimal otdaymultiplier = 0;
        decimal otnightmultiplier = 0;

        daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["workingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["workingdays"].ToString());
        nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["nightworkingdays"].ToString());
        otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTworkingdays"].ToString());
        otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightWorkingDays"].ToString());


        string query = "";
        if (timein1 != "" && timeout1 != "")
        {
            int scid = 0;
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + empid + " ");

            string in1 = (timein1).Replace(" ", "").Length > 0 ? timein1 : "0";
            string in2 = "0";
            string out1 = "0";
            string out2 = (timeout1).Replace(" ", "").Length > 0 ? timeout1 : "0";
            DataTable changeshift = dbhelper.getdata("select * from TChangeShiftLine where EmployeeId=" + empid + " and CONVERT(date,date)=CONVERT(date,'" + datenower[0] + "')");
            scid = changeshift.Rows.Count > 0 ? int.Parse(changeshift.Rows[0]["Shiftcodeid"].ToString()) : int.Parse(dtemp.Rows[0]["Shiftcodeid"].ToString());

            DataTable sc = dbhelper.getdata("select * from MShiftCodeDay where shiftcodeid=" + scid + "");


            DateTime dtout = Convert.ToDateTime(out2);
            if (sc.Rows[0]["timein1"].ToString().Contains("PM") && sc.Rows[0]["timeout2"].ToString().Contains("AM"))
                dtout = Convert.ToDateTime(datenower[0] + " " + sc.Rows[0]["timeout2"].ToString()).AddDays(1);
            if (Convert.ToDateTime(out2) < dtout)
                dtout = Convert.ToDateTime(out2);

            DataTable dt = dbhelper.getdata("Select * from Tmanuallogline where EmployeeId=" + empid + " and LEFT(CONVERT(varchar,date,101),10)='" + datenower[0] + "' and (status like'%approved%' or status like '%for approval%' or status like '%verification%') order by [sysdate] desc");

            if (dt.Rows.Count > 0)
            {
                dbhelper.getdata("update Tmanuallogline set status='Cancel-" + datenowv2 + "', note='The Administrator altered this data.This request is no longer valid.' where Id=" + dt.Rows[0]["Id"].ToString());
            }

            query = "select  (CONVERT(float, datediff(minute, '" + in1 + "','" + dtout + "' )) / 60 ) - " + sc.Rows[0]["nightbreakhours"].ToString() + "  as time_diff ";
            DataTable comhrs = dbhelper.getdata(query);

            decimal hrsss = decimal.Parse(comhrs.Rows[0]["time_diff"].ToString()) > decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) ? decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) : decimal.Parse(comhrs.Rows[0]["time_diff"].ToString());
            // if (getotout > otin)

            DateTime JJ = Convert.ToDateTime(datenower[0] + " 10:00 PM");
            decimal nights = 0;
            if (dtout > JJ)
            {
                nights = decimal.Parse((getnightdif.getnight(in1.ToString(), dtout.ToString(), "dtr")).ToString());
                nights = nights > 0 ? nights : 0;
            }

            dbhelper.getdata("insert into Tmanuallogline (EmployeeId,[Date],time_in,time_OUT,reason,[status],sysdate,note,time_in1,time_OUT1,approver_id,hourlyrate,regmultiplier,nightmultiplier,hrs,nighthrs) values (" + empid + ",'" + datenower[0] + "','" + timein1 + "','" + timeout1 + "','" + remarks + "','Approved-" + datenow + "',GETDATE(),'" + remarks + "',NULL,NULL,0,'" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "','" + daymultiplier + "','" + nightmultiplier + "','" + (hrsss - nights) + "','" + nights + "')");

        }  //function.AddNotification("Time Adjustment Approval", "am", approver_id.Rows[0]["under_id"].ToString(), x);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully');", true);

        //-------------------Overtime Application---------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (otn != "" && ot != "")
        {
            DateTime final_out = Convert.ToDateTime(timeout1);
            DateTime final_set_out = Convert.ToDateTime(finalout);
            TimeSpan final_ot;
            decimal total_ot;
            string nighthrs;
            string hdn_reg_othrs = "0";
            string hdn_night_othrs = "0";
            if (final_out > final_set_out)
            {
                final_ot = final_out - final_set_out;
                nighthrs = getnightdif.getnight(final_set_out.ToString(), final_out.ToString(), "dtr");
                if (decimal.Parse(final_ot.TotalHours.ToString()) > 1)
                {
                    int yow = (int)System.Math.Floor(decimal.Parse(final_ot.TotalHours.ToString()) - decimal.Parse(nighthrs));
                    hdn_reg_othrs = ot;
                    hdn_night_othrs = otn;
                }
            }
            string hdn_setupout = final_set_out.ToString();
            string hdn_actout = final_out.ToString();

            DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + empid + " and left(convert(varchar,date,101),10)='" + datenower[0] + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification' )");

            if (tdcheck.Rows.Count > 0)
            {
                dbhelper.getdata("update TOverTimeLine set status='Cancel-" + datenowv2 + "', Remarks='The Administrator altered this data.This request is no longer valid.' where Id=" + tdcheck.Rows[0]["Id"].ToString());
            }

            string queryyy = "insert into TOverTimeLine ([OverTimeId],[EmployeeId],[Date],[OvertimeHours],[OvertimeNightHours],[OvertimeLimitHours],[Remarks],[status],[ifdone],[dtr_id],[sysdate],[time_in],[time_out],[overtimehoursapp],[overtimenighthoursapp],[approver_id],regmultiplier,nightmultiplier,hourlyrate) values(NULL," + empid + ",'" + datenower[0] + "','" + hdn_reg_othrs + "','" + hdn_night_othrs + "','0','" + remarks + "','Approved-" + datenow + "',NULL,NULL,getdate(),'" + hdn_setupout + "','" + hdn_actout + "'," + ot + "," + otn + ",'0','" + otdaymultiplier + "','" + otnightmultiplier + "','" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "' ) select scope_identity() id";
            dbhelper.getdata(queryyy);
        }
        //------------------Change Shift---------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (shiftc != "")
        {
            string aa = "select * from TchangeshiftLine where [date]=left(convert(varchar,'" + datenower[0] + "',101),10) and employeeId='" + empid + "' and ShiftCodeId='" + shiftc + "'  ";
            DataTable hh = new DataTable();
            hh = dbhelper.getdata(aa);

            if (hh.Rows.Count > 0)
            {
                query = "update TChangeShiftLine set ShiftCodeId='" + shiftc + "' where Id=" + hh.Rows[0]["Id"].ToString() + "";
                dbhelper.getdata(query);
            }

            if (hh.Rows.Count == 0)
            {
                query = "insert into TChangeShiftLine (ChangeShiftId,EmployeeId,Date,ShiftCodeId,Remarks,status,dtr_id) values (0," + empid + ",'" + datenower[0] + "','" + shiftc + "','" + remarks + "','approved',NULL) ";
                dbhelper.getdata(query);
            }
        }
        //---------------------------Status Class------------------------------------------------------------------------------------------------------------------------------------------------------
        //if (shiftc != "")
        //{
        //    DataTable dtt = dbhelper.getdata("select * from TRestdaylogs where EmployeeId='" + empid + "' and convert(date,Date)=convert(date,'" + datenower[0] + "') and (status like '%Approved%' or status like'%Verification%' or status like'%For Approval%') ");
        //    if (dtt.Rows.Count > 0)
        //    {
        //        dbhelper.getdata("update TRestdaylogs set status='Cancel-" + datenowv2 + "', reason='The Administrator altered this data.This request is no longer valid.' where Id=" + dtt.Rows[0]["Id"].ToString());
        //    }

        //    dbhelper.getdata("insert into TRestdaylogs (EmployeeId,shiftcodeId,Date,reason,status,dtr_id,sysdate,approver_id,class) values (" + empid + "," + shiftc + ",'" + datenower[0] + "','" + remarks + "','" + "Approved-" + datenowv2 + "',NULL,GETDATE(),'0','RD') select scope_identity() id");
        //}
        //---------------------------Offseting------------------------------------------------------------------------------------------------------------------------------------------------------

        if (os != "")
        {
            decimal halfday = decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
            string d = decimal.Parse(os) <= halfday ? halfday.ToString() : os;

            DataTable dttt = dbhelper.getdata("Select * from nobel_user where emp_id=" + empid + "");

            DataTable getoffsetexist = dbhelper.getdata("Select * from toffset where empid='" + empid + "' and convert(date,appliedfrom)=convert(date,'" + datenower[0] + "') and (status like '%Approved%' or status like'%Verification%' or status like'%For Approval%')");
            DataTable dtttt = dbhelper.getdata("insert into toffset (date,appliedfrom,offsethrs,status,empid,userid,aproverid,tobededuct_hrs,remarks) values (GETDATE(),'" + datenower[0] + "'," + os + ",'Approved-" + datenowv2 + "'," + empid + "," + dttt.Rows[0]["id"].ToString() + ",0," + d + ",'" + remarks + "') Select SCOPE_IDENTITY() as dayum");

            if (getoffsetexist.Rows.Count > 0)
            {
                DataTable dto = dbhelper.getdata("update toffset set status='Cancel-" + datenowv2 + "', remarks='The Administrator altered this data.This request is no longer valid.' where Id=" + getoffsetexist.Rows[0]["Id"].ToString() + "");
                dbhelper.getdata("update Toffset_date set offset_id=" + dtttt.Rows[0]["dayum"].ToString() + " where offset_id=" + getoffsetexist.Rows[0]["Id"].ToString());
            }

            string[] osdate = osdates.Split(',');
            string[] excess = excesses.Split(',');
            decimal osss = decimal.Parse(os);
            decimal offset = decimal.Parse(offsethr);

            for (int i = 0; i < osdate.Count(); i++)
            {
                if (offset > 0)
                {
                    if (excess[i] != "")
                    {
                        offset = offset - decimal.Parse(excess[i]);

                        dbhelper.getdata("insert into Toffset_date (date,date_from,empid,offset_id,excess_hrs)values(getdate(),'" + osdate[i] + "'," + empid + "," + dtttt.Rows[0]["dayum"].ToString() + "," + (Convert.ToDecimal(excess[i]) - Math.Abs(offset)) + ") ");
                    }
                }

            }
        }

    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static DetailsClass[] GetOffset(string empid, string ut, string aw, string datepoint, string timeout, string timein)
    {
        DataTable dt = dbhelper.getdata("select * from memployee where id=" + empid + "");
        DataTable dtperiod = adjustdtrformat.payrollperiod("all", dt.Rows[0]["payrollgroupid"].ToString());
        decimal halfday = decimal.Parse(dt.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
        string[] datedate = datepoint.Split('-');
        DataTable dtr = HttpContext.Current.Session["SuperSecret"] as DataTable;

        decimal t_temp_ot = 0;
        decimal integerPart = 0;

        List<DetailsClass> Detail = new List<DetailsClass>();

        foreach (DataRow dr in dtr.Rows)
        {
            if (dr["timeout2"].ToString().Length > 2)
            {
                DateTime dtout = Convert.ToDateTime(dr["timeout2"].ToString());
                DateTime dtsetupout = Convert.ToDateTime(dr["setupfinalout"].ToString());
                if (dtout > dtsetupout.AddMinutes(59))
                {
                    TimeSpan finalot = dtout - dtsetupout;
                    integerPart = (int)System.Math.Floor(decimal.Parse(dr["tempot"].ToString()));

                    string[] datee = dr["date"].ToString().Split('-');
                    DataTable getdt = dbhelper.getdata("select COALESCE(SUM(b.excess_hrs),0) excess_hrs from toffset a left join Toffset_date b on a.id=b.offset_id where (a.status like'%Approval%' or a.status like'%verification%' or a.status like'%Approved%') and CONVERT(date,b.date_from)=convert(date,'" + datee[0] + "') and b.empid=" + empid + " ");
                    DataTable getdtot = dbhelper.getdata("select * from tovertimeline where (status like '%For Approval%' or status like '%Approved%' or status like '%verification%') and CONVERT(date,date)= convert(date,'" + datee[0] + "') and employeeid=" + empid + "");
                    string[] dateee = dr["date"].ToString().Split('-');
                    if (getdtot.Rows.Count == 0)
                    {
                        DetailsClass DataObj = new DetailsClass();

                        integerPart = getdt.Rows.Count > 0 ? integerPart - decimal.Parse(getdt.Rows[0]["excess_hrs"].ToString()) : integerPart;
                        integerPart = (int)System.Math.Floor(integerPart);
                        if (integerPart >= 1)
                        {
                            DataObj.date = dateee[0];
                            DataObj.excess = integerPart;

                            if (timein.Length > 2 && timeout.Length > 2)
                            {
                                if (aw == "True")
                                {
                                    DataObj.oshrs = halfday.ToString();
                                }
                                else
                                    DataObj.oshrs = ut;
                            }
                            else
                            {
                                DataObj.oshrs = dt.Rows[0]["FixNumberOfHours"].ToString();
                            }

                            Detail.Add(DataObj);
                        }
                    }
                    t_temp_ot = t_temp_ot + integerPart;
                }
            }
        }


        return Detail.ToArray();
    }

    public class DetailsClass
    {
        public string date { get; set; }
        public decimal excess { get; set; }
        public string oshrs { get; set; }

    }

    [WebMethod]
    public static DetailRequest[] GetRequests(string empid, string datepoint, string shiftc)
    {
        List<DetailRequest> Detail = new List<DetailRequest>();
        string[] datenower = datepoint.Split('-');

        DataTable tdcheck = dbhelper.getdata("select a.sysdate,case when (select FirstName from MEmployee where id=a.approver_id)='' then 'Admin' else (select FirstName+' '+LastName from MEmployee where id=a.approver_id) end [Names]  from TOverTimeLine a where a.employeeid=" + empid + " and left(convert(varchar,a.date,101),10)='" + datenower[0] + "' and (SUBSTRING(a.status,0, CHARINDEX('-',a.status))='For Approval' or SUBSTRING(a.status,0, CHARINDEX('-',a.status))='for verification' )");
        foreach (DataRow row in tdcheck.Rows)
        {
            DetailRequest DataObj = new DetailRequest();
            DataObj.date = row["sysdate"].ToString();
            DataObj.approver = row["Names"].ToString();
            DataObj.type = "Overtime";
            Detail.Add(DataObj);
        }

        DataTable getoffsetexist = dbhelper.getdata("Select case when (select FirstName from MEmployee where id=a.aproverid)='' then 'Admin' else (select FirstName+' '+LastName from MEmployee where id=a.aproverid) end [Names] from toffset a where a.empid='" + empid + "' and convert(date,a.appliedfrom)=convert(date,'" + datenower[0] + "') and ( a.status like'%Verification%' or a.status like'%For Approval%')");
        foreach (DataRow row in getoffsetexist.Rows)
        {
            DetailRequest DataObj = new DetailRequest();
            DataObj.date = "--";
            DataObj.approver = row["Names"].ToString();
            DataObj.type = "Offset";
            Detail.Add(DataObj);
        }


        DataTable dtt = dbhelper.getdata("select a.sysdate, case when (select FirstName from MEmployee where id=a.approver_id)='' then 'Admin' else (select FirstName+' '+LastName from MEmployee where id=a.approver_id) end [Names] from TRestdaylogs a where a.EmployeeId='" + empid + "' and convert(date,a.Date)=convert(date,'" + datenower[0] + "') and (a.status like '%For Approval%' or a.status like'%Verification%') ");
        foreach (DataRow row in dtt.Rows)
        {
            DetailRequest DataObj = new DetailRequest();
            DataObj.date = row["sysdate"].ToString();
            DataObj.approver = row["Names"].ToString();
            DataObj.type = "Status Change";
            Detail.Add(DataObj);
        }

        //DataTable hh = dbhelper.getdata("select * from TchangeshiftLine a where a.[date]=left(convert(varchar,'" + datepoint + "',101),10) and a.employeeId='" + empid + "' and a.ShiftCodeId='" + shiftc + "' ");
        //foreach (DataRow row in hh.Rows)
        //{
        //    DetailRequest DataObj = new DetailRequest();
        //    DataObj.date = row["sysdate"].ToString();
        //    DataObj.approver = row["Names"].ToString();
        //    DataObj.type = "Change Shift";
        //}


        DataTable dt = dbhelper.getdata("Select a.sysdate, case when (select FirstName from MEmployee where id=a.approver_id)='' then 'Admin' else (select FirstName+' '+LastName from MEmployee where id=a.approver_id) end [Names] from Tmanuallogline a where a.EmployeeId=" + empid + " and LEFT(CONVERT(varchar,a.date,101),10)='" + datenower[0] + "' and (a.status like '%for approval%' or a.status like '%verification%') order by [sysdate] desc");
        foreach (DataRow row in dt.Rows)
        {
            DetailRequest DataObj = new DetailRequest();
            DataObj.date = row["sysdate"].ToString();
            DataObj.approver = row["Names"].ToString();
            DataObj.type = "Time Adjustment";
            Detail.Add(DataObj);
        }
        return Detail.ToArray();
    }

    public class DetailRequest
    {
        public string date { get; set; }
        public string type { get; set; }
        public string approver { get; set; }
    }

    [WebMethod]
    public static string GetInfo(string empid, string ff, string shiftc, string timein1, string timeout1, string absenter, string datepoint)
    {
        string info = "";

        string txtf = ff.Trim();
        decimal t_total_tempot = 0;
        string status = "RWD";
        decimal rdamount = 0;


        string[] f_datef = datepoint.Trim().Split(' ');
        string[] getdate = f_datef[0].Trim().Split('/');

        string[] dayofweek = datepoint.Trim().Split('-');
        string[] ggg = dayofweek[0].Split('/');
        string month = ggg[0].ToString().Length > 1 ? ggg[0].ToString() : "0" + ggg[0].ToString();
        string day = ggg[1].ToString().Length > 1 ? ggg[1].ToString() : "0" + ggg[1].ToString();
        string dtrdate = month + "/" + day + "/" + ggg[2];


        DataRow[] dtt = adjustdtrformat2.dttest.Select("emp_id='" + empid + "' and date='" + dtrdate + "'");
        DataRow[] getmanual = adjustdtrformat2.dtmanual.Select("employeeid='" + empid + "' and dtrdate='" + dtrdate + "'");
        DataRow[] drundertime = adjustdtrformat2.dtgtwthveri.Select("sstatus like '%Approved%' and emp_id=" + adjustdtrformat2.dtgetemp.Rows[0]["id"] + " and date_filed='" + dtrdate + "'");
                   
        DataRow[] getleave = adjustdtrformat2.dtleave.Select("employeeid='" + empid + "' and dtrdate='" + dtrdate + "'");
        DataRow[] getrdl = adjustdtrformat2.dtrdl.Select("EmployeeId='" + empid + "' and dtrdate='" + dtrdate + "'");
        DataRow[] dtgetchangeshift = adjustdtrformat2.dtchangeshift.Select("dtrdate='" + dtrdate + "' and employeeid='" + empid + "'");
        DataRow[] dtgetallDayType = adjustdtrformat2.dtdaytype.Select("datte='" + dtrdate + "' and BranchId='" + adjustdtrformat2.dtgetemp.Rows[0]["BranchId"].ToString() + "'");
        if (dtgetallDayType.Count() > 0 && int.Parse(dtgetallDayType[0]["BranchId"].ToString()) > 0)
            dtgetallDayType = adjustdtrformat2.dtdaytype.Select("datte='" + month + "/" + day + "/" + getdate[2] + "' and BranchId='" + adjustdtrformat2.dtgetemp.Rows[0]["BranchId"].ToString() + "'");
        else
            dtgetallDayType = adjustdtrformat2.dtdaytype.Select("datte='" + month + "/" + day + "/" + getdate[2] + "' and BranchId='0'");
                      
        //string shiftcodeid = dtgetchangeshift.Count() > 0 ? dtgetchangeshift[0]["shiftcodeid"].ToString() : shiftc;
        string shiftcodeid = shiftc;
        DataRow[] getscc = adjustdtrformat2.dtchksc.Select("id=" + shiftcodeid + "");

        string ffix ="";
        if (getscc.Length > 0)
        {
            if (getscc[0]["fix"].ToString() == "1")
                ffix = " and day='" + dayofweek[1] + "'";
            else
                ffix = "";
        }
        DataRow[] dtgetrestday = adjustdtrformat2.dtrestday.Select("ShiftCodeId=" + shiftcodeid +ffix+ " ");

        shiftcodeid = getrdl.Count() > 0 ? getrdl[0]["shiftcodeid"].ToString() : shiftcodeid;

        DataRow[] dtgetshiftcode = adjustdtrformat2.dtshiftcode.Select("ShiftCodeId=" + shiftcodeid + "");
        DataRow[] droffset = adjustdtrformat2.dtgtoffset.Select("status like '%Approved%' and empid=" + adjustdtrformat2.dtgetemp.Rows[0]["id"] + " and appliedfrom='" + dtrdate + "' and appliedto is null");
                      
       
        string scs = shiftcodeid;

        decimal offsethrs = droffset.Count() > 0 ? decimal.Parse(droffset[0]["offsethrs"].ToString()) : 0;
        double getgracehours = double.Parse(dtgetshiftcode[0]["LateGraceMinute"].ToString()) / 60;

        string tmin1 = timein1;
        string tmout1 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[3].ToString().Length > 0 ? dtt[0].ItemArray[3].ToString() : "--" : "--" : getmanual[0]["time_out1"].ToString().Length > 1 ? getmanual[0]["time_out1"].ToString() : "--";
        string tmin2 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[4].ToString().Length > 0 ? dtt[0].ItemArray[4].ToString() : "--" : "--" : getmanual[0]["time_in1"].ToString().Length > 1 ? getmanual[0]["time_in1"].ToString() : "--";
        string tmout2 = timeout1;
        tmout2 = drundertime.Count() > 0 ? drundertime[0]["acttimeout"].ToString() : tmout2;

        //check if Holiday
        string rd = "False";
        string hd = "False";
        if (dtgetallDayType.Count() > 0)
        {
            if (int.Parse(dtgetallDayType[0]["id"].ToString()) > 1)
            {
                if (getrdl.Count() == 0)
                {
                    if (adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "1" || adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "4" || adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "5")
                    {
                        tmin1 = "--";
                        tmout1 = "--";
                        tmin2 = "--";
                        tmout2 = "--";
                    }

                    /**BUTYOK 11162020
                     * Public Holiday**/
                    if (scs == "65")
                    {
                        tmin1 = "--";
                        tmout1 = "--";
                        tmin2 = "--";
                        tmout2 = "--";
                    }


                }
                status = dtgetallDayType[0]["daytype"].ToString().Contains("Regular") ? "RH" : dtgetallDayType[0]["daytype"].ToString().Contains("Special") ? "SH" : "DH";
                hd = "True";
            }
        }

        //check if RD
        if (dtgetrestday.Count() > 0)
        {
            if (int.Parse(dtgetrestday[0]["id"].ToString()) > 1)
            {
                if (getrdl.Count() == 0)
                {
                    tmin1 = "--";
                    tmout1 = "--";
                    tmin2 = "--";
                    tmout2 = "--";
                }
            }
            status = "RD";
            rd = "True";
        }



        tmin1 = tmin1.Length <= 2 ? "--" : Convert.ToDateTime(tmin1).ToString("MM/dd/yyyy hh:mm tt");
        tmout1 = tmout1.Length <= 2 ? "--" : Convert.ToDateTime(tmout1).ToString("MM/dd/yyyy hh:mm tt");
        tmin2 = tmin2.Length <= 2 ? "--" : Convert.ToDateTime(tmin2).ToString("MM/dd/yyyy hh:mm tt");
        tmout2 = tmout2.Length <= 2 ? "--" : Convert.ToDateTime(tmout2).ToString("MM/dd/yyyy hh:mm tt");
       
        double getgracem = double.Parse(dtgetshiftcode[0]["LateGraceMinute"].ToString());

        //biotime in and out actual
        DateTime getin = Convert.ToDateTime(tmin1.ToString().Length > 2 ? tmin1.ToString() : "0:00:00").AddMinutes(-getgracem);
        DateTime getoutt1 = Convert.ToDateTime(tmout1.ToString().Length > 2 ? tmout1.ToString() : "0:00:00");
        DateTime getinn1 = Convert.ToDateTime(tmin2.ToString().Length > 2 ? tmin2.ToString() : "0:00:00");
        DateTime getout = Convert.ToDateTime(tmout2.ToString().Length > 2 ? tmout2.ToString() : "0:00:00");

        //shift code set up
        DateTime setupgetin1 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchin"].ToString()); //total_flexIn
        DateTime setupgetout1 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchout1"].ToString());
        DateTime setupgetin2 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchin2"].ToString());
        DateTime setupgetout2 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchout"].ToString());//.AddMinutes(flexhours)
        string setupfinalout = setupgetout2.ToString();

        DataTable dtcompanydet = dbhelper.getdata("select * from mcompany where id=" + adjustdtrformat2.dtgetemp.Rows[0]["companyid"] + "");

        //cross dates
        string crossdate = null;
        if (setupgetin1.ToString().Contains("PM"))
        {
            if (setupgetout1.ToString().Contains("AM"))
            {
                setupgetout1 = setupgetout1.AddDays(1);
                crossdate = "1";
            }
            if (setupgetin2.ToString().Contains("AM"))
            {
                setupgetin2 = setupgetin2.AddDays(1);
                crossdate = "1";
            }
            if (setupgetout2.ToString().Contains("AM"))
            {
                setupgetout2 = setupgetout2.AddDays(1);
                crossdate = "1";
            }
        }
        else if (setupgetout1.ToString().Contains("PM"))
        {
            if (setupgetin2.ToString().Contains("AM"))
            {
                setupgetin2 = setupgetin2.AddDays(1);
                crossdate = "1";
            }
            if (setupgetout2.ToString().Contains("AM"))
            {
                setupgetout2 = setupgetout2.AddDays(1);
                crossdate = "1";
            }
        }
        else if (setupgetin2.ToString().Contains("PM"))
        {
            if (setupgetout2.ToString().Contains("AM"))
            {
                setupgetout2 = setupgetout2.AddDays(1);
                crossdate = "1";
            }
        }
        else if (setupgetin1.ToString().Contains("12:00:00 AM"))
        {
            /**BTK 02112020
             * TO HANDLE MIDDAY SHIFT**/
            setupgetin1 = setupgetin1.AddDays(1);
            setupgetout2 = setupgetout2.AddDays(1);
            crossdate = "1";
        }

        decimal t_leavehour = 0;
        decimal temphrs = 0;
        decimal leaveamount = 0;
        decimal leavewoamount = 0;
        string inoutduringleavehalfday = "0";
        decimal nohleave = 0;
        string leaveh = "False";
        string leavew = "False";
        string absenth = "False";
        string absentw = "False";
        string irregularity = "False";
        if (getleave.Count() > 0)
        {
            if (decimal.Parse(getleave[0]["noh"].ToString()) >= 1)
            {
                leavew = "True";
                nohleave = decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
            }
            else
            {

                leaveh = "True";
                nohleave = decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                inoutduringleavehalfday = getleave[0]["inoutduringhalfdayleave"].ToString().Length > 0 ? getleave[0]["inoutduringhalfdayleave"].ToString() : "0";

                absenth = "True";
            }

        
        }

        //cleaning punch
        decimal finalut = 0;
        decimal night = 0;
        decimal reghours = 0;
        TimeSpan stregh = new TimeSpan();
        TimeSpan gettemplate = new TimeSpan();
        TimeSpan sthalf = new TimeSpan();
        TimeSpan ndhalft = new TimeSpan();
        TimeSpan ndut = new TimeSpan();
        TimeSpan stot = new TimeSpan();
        TimeSpan ndot = new TimeSpan();
        decimal finalot = t_total_tempot;
        decimal othrs = 0;
        decimal othrsnight = 0;
        TimeSpan brkhrs = new TimeSpan();
        double breakdeduct = double.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString());
        double nightbreakdeduct = 0;
        decimal overbreak = 0;
        decimal t_total = 0;
        decimal t_total_ut = 0;
        decimal difbet1out2in = 0;
        decimal difbet1out2innight = 0;
        int yyyyy = 0;
        if (tmin1.ToString().Length > 2 && tmout2.ToString().Length > 2)
        {

            //get late
            TimeSpan templatehrs = new TimeSpan();
            TimeSpan flexbrhrs = new TimeSpan();
            DateTime fdateout1late = new DateTime();
            decimal flexmidlate = 0;
            if (leaveh == "True")
            {
                if (int.Parse(inoutduringleavehalfday) == 1)
                    setupgetin1 = setupgetin1.AddHours(double.Parse(nohleave.ToString()) + breakdeduct);
                //fdateout1late = setupgetin1;
                if (int.Parse(inoutduringleavehalfday) == 2)
                {

                    string ded = "-" + (double.Parse(nohleave.ToString()) + breakdeduct).ToString();
                    setupgetout2 = setupgetout2.AddHours(double.Parse(ded));
                    //fdateout1late = setupgetout2;
                }
                if (getin > setupgetin1)
                    sthalf = getin - setupgetin1;
                t_total = decimal.Parse(sthalf.TotalHours.ToString()) + decimal.Parse(ndhalft.TotalHours.ToString());
            }
            else
            {

                if (getin > setupgetin1)
                {
                    sthalf = getin - setupgetin1;
                }
                if (tmin2.ToString().Length > 2)
                {
                    if (dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True")
                    {
                        if (dtgetshiftcode[0]["flexbreak"].ToString() == "True")
                        {
                            if (dtgetshiftcode[0]["punchout1"].ToString() == "0:00:00" && dtgetshiftcode[0]["punchin2"].ToString() == "0:00:00")
                            {
                                flexbrhrs = (getinn1 - getoutt1);
                                flexmidlate = decimal.Parse(flexbrhrs.TotalHours.ToString()) > decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) ? decimal.Parse(flexbrhrs.TotalHours.ToString()) - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) : 0;
                            }
                        }
                        else
                        {

                            if (getinn1 > setupgetin2)
                            {
                                ndhalft = getinn1 - setupgetin2;
                                flexmidlate = decimal.Parse(ndhalft.TotalHours.ToString()) > decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) ? decimal.Parse(ndhalft.TotalHours.ToString()) - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) : 0;
                            }
                        }
                    }
                }

                t_total = decimal.Parse(sthalf.TotalHours.ToString()) + flexmidlate;
                t_total = t_total > 0 ? t_total : 0;
                overbreak = flexmidlate;

                if (scs == "2")
                {
                    string[] xxx = getin.ToString().Split(' ');
                    if (getin > Convert.ToDateTime(xxx[0] + " 12:00:00 PM"))
                    {
                        yyyyy = 1;
                        absenth = "True";
                        setupgetin1 = Convert.ToDateTime(xxx[0] + " 01:00:00 PM");
                        setupgetout2 = setupgetout2.AddHours(1);
                        setupfinalout = setupgetout2.ToString();
                        if (getin > setupgetin1)
                        {
                            sthalf = getin - setupgetin1;
                            t_total = decimal.Parse(sthalf.TotalHours.ToString());
                        }
                        else
                            t_total = 0;

                    }
                    else
                    {
                        setupgetin1 = setupgetin1.AddHours(double.Parse(t_total.ToString()));
                        setupgetout2 = setupgetout2.AddHours(double.Parse(t_total.ToString()));
                        setupfinalout = setupgetout2.ToString();
                        t_total = 0;
                    }

                }
            }

            //get hours between punch
            DateTime fdatein = new DateTime();
            DateTime fdateout = new DateTime();

            // get undertime
            DateTime fdateout1under = new DateTime();
            if (leaveh == "True")
            {
                if (int.Parse(inoutduringleavehalfday) == 1)
                    fdateout1under = setupgetout1;
                if (int.Parse(inoutduringleavehalfday) == 2)
                    fdateout1under = setupgetout2;
            }
            else
                fdateout1under = setupgetout2;

            if (getout < fdateout1under)
            {
                ndut = fdateout1under - getout;
                t_total_ut = decimal.Parse(ndut.TotalHours.ToString());
                finalut = t_total_ut - offsethrs;
                finalut = finalut > 0 ? finalut : 0;

            }
            finalut = finalut > 0 ? finalut : 0;

            //get break hours
            DateTime fdateout1 = new DateTime();
            DateTime fdatein2 = new DateTime();
            TimeSpan overb = new TimeSpan();
            if (tmout1.ToString().Length > 0 && tmin2.ToString().Length > 2)
            {
                fdateout1 = getoutt1;
                fdatein2 = getinn1;
                if (dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True" && dtgetshiftcode[0]["flexbreak"].ToString() == "False")
                {
                    if (fdateout1 >= setupgetout1)
                        fdateout1 = setupgetout1;
                    if (fdatein2 <= setupgetin2)
                        fdatein2 = setupgetin2;

                    if (fdatein2 > fdateout1)
                    {
                        //need to break down night break in shift code
                        brkhrs = fdatein2 - fdateout1;
                        nightbreakdeduct = double.Parse((getnight(fdatein2.ToString(), fdateout1.ToString(), "dtr")).ToString());
                        breakdeduct = double.Parse(brkhrs.TotalHours.ToString()) > breakdeduct ? double.Parse(brkhrs.TotalHours.ToString()) - nightbreakdeduct : breakdeduct;
                    }
                }
            }


            nightbreakdeduct = yyyyy == 1 ? 0 : nightbreakdeduct > 0 ? nightbreakdeduct : 0;
            breakdeduct = yyyyy == 1 ? 0 : breakdeduct > 0 ? breakdeduct : 0;
            overbreak = yyyyy == 1 ? 0 : overbreak > 0 ? overbreak : 0;

            decimal excesshrs = 0;
            if (getout > getin)
            {
                DateTime tmppp = new DateTime();
                if (leaveh == "True")
                {
                    switch (int.Parse(inoutduringleavehalfday))
                    {
                        case 1:
                            if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                            {
                                if (getout > getin)
                                {
                                    stregh = getout - getin;
                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                    excesshrs = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - nohleave : 0;
                                    reghours = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - excesshrs : reghours;

                                    night = decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                                    reghours = reghours - night;
                                }
                            }
                            else //not open shift
                            {
                                tmppp = setupgetout2;

                                if (getin > setupgetin1)
                                    fdatein = getin;
                                else
                                    fdatein = setupgetin1;

                                if (getout > tmppp)
                                    fdateout = tmppp;
                                else
                                    fdateout = getout;
                                if (fdateout > fdatein)
                                {
                                    stregh = fdateout - fdatein;
                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                    night = decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString());
                                    reghours = reghours - night;
                                }
                            }
                            break;
                        case 2:

                            if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                            {
                                if (getout > getin)
                                {
                                    stregh = getout - getin;
                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                    excesshrs = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - nohleave : 0;
                                    reghours = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - excesshrs : reghours;
                                    night = decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                                    reghours = reghours - night;
                                }
                            }
                            else //not open shift
                            {
                                tmppp = setupgetin1;
                                if (getin > tmppp)
                                    fdatein = getin;
                                else
                                    fdatein = tmppp;

                                if (getout > setupgetout2)
                                    fdateout = setupgetout2;
                                else
                                    fdateout = getout;
                                if (getout > tmppp)
                                {
                                    stregh = fdateout - fdatein;
                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                    night = decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString());
                                    reghours = reghours - night;
                                }
                            }
                            break;
                    }
                }
                else
                {

                    if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                    {
                        if (getout > getin)
                        {
                            stregh = getout - getin;
                            reghours = decimal.Parse(stregh.TotalHours.ToString());
                            night = (decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString())) - decimal.Parse(nightbreakdeduct.ToString());
                            excesshrs = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - (decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5")) : 0;
                            if (reghours >= decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") + decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()))
                                reghours = reghours - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString());
                            else
                                reghours = reghours - excesshrs;

                            reghours = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) ? decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) : reghours;
                            reghours = reghours - night;

                        }
                    }
                    else
                    {
                        if (getin > setupgetin1)
                            fdatein = getin;
                        else
                            fdatein = setupgetin1;

                        if (getout > setupgetout2)
                            fdateout = setupgetout2;
                        else
                            fdateout = getout;


                        if (fdateout > fdatein)
                        {
                            stregh = fdateout - fdatein;
                            night = (decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString())) - decimal.Parse(nightbreakdeduct.ToString());
                            reghours = decimal.Parse(stregh.TotalHours.ToString()) - night;
                            reghours = reghours > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - decimal.Parse(breakdeduct.ToString()) : reghours;
                        }
                    }
                }
            }

        }
        else
        {
            if (offsethrs > 0)
                absentw = "False";
            else
                absentw = "True";

            if (rd == "True")
            {
                absentw = "False";
            }

            if (hd == "True")
            {
                absentw = "False";
            }

            if (leavew == "True" || leaveh == "True")
                absentw = "False";
        }

        //data finalizing
        night = night > decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? night - decimal.Parse(breakdeduct.ToString()) : night;
        night = night > 0 ? night : 0;
        reghours = reghours > 0 ? reghours : 0;



        t_total = dtgetshiftcode[0]["openshift"].ToString() == "True" ? 0 : t_total;

        if (t_total >= decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) / 2)
        {
            t_total = t_total - (decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) / 2);
            absenth = "True";
        }

        reghours = dtcompanydet.Rows[0]["ob_roles"].ToString() == "2" ? reghours + overbreak : reghours;
        t_total = dtcompanydet.Rows[0]["ob_roles"].ToString() == "2" ? t_total - overbreak : t_total;


        if (finalut >= decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5"))
        {
            decimal halfhalf = decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
            if (finalut >= halfhalf + decimal.Parse(breakdeduct.ToString()))
                finalut = finalut - decimal.Parse(breakdeduct.ToString());
            finalut = finalut >= decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? finalut - (decimal.Parse(adjustdtrformat2.dtgetemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse("0.5")) : 0;
            absenth = "True";
        }

        night = night > 0 ? night : 0;
        reghours = reghours > 0 ? reghours : 0;

        decimal totallate = t_total;

        night = Math.Round(night, 5);
        reghours = Math.Round(reghours, 5);

      
      

        //absent
        if (absentw == "True")
        {
            reghours = 0;
            night = 0;
            finalut = 0;//undertime
            totallate = 0;//late
        }

        /**HOLIDAY**/
        if (hd == "True")
        {
            if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(adjustdtrformat2.dtgetemp.Rows[0]["datehired"].ToString()))
            {
                finalut = 0; //undertime
                totallate = 0; //late
                reghours = 0;
                night = 0;
                rd = "False";
                hd = "False";
            }
            else
            {
                finalut = 0; //undertime
                totallate = 0; //late
                if (getrdl.Count() == 0)
                {
                    if (adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "1" || adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "4" || adjustdtrformat2.dtgetemp.Rows[0]["DivisionId"].ToString() == "5")
                    {
                        reghours = 0;
                        night = 0;
                    }
                }


            }
        }

        if (rd == "True")
        {
            if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(adjustdtrformat2.dtgetemp.Rows[0]["datehired"].ToString()))
            {
                finalut = 0; //undertime
                totallate = 0; //late
                reghours = 0;
                night = 0;
                rd = "False";
                hd = "False";
            }
            else
            {
                finalut = 0; //undertime
                totallate = 0; //late
                if (getrdl.Count() == 0)
                {
                    reghours = 0;
                    night = 0;
                }
            }
        }




        totallate = rd == "True" ? 0 : totallate;
        finalut = rd == "True" ? 0 : finalut;

        if (leavew == "True")
        {
            reghours = 0;

            totallate = 0;
            finalut = 0;
            night = 0;
        }


        info += string.Format("{0:n2}", reghours) + "~";
        info += string.Format("{0:n2}", night) + "~";
        info += string.Format("{0:n2}", totallate) + "~";
        info += string.Format("{0:n2}", finalut) + "~";
        info += status + "~";

        return info;
    }


    public static DateTime lateroundoffmin_in(DateTime value)
    {
        int prevmin = value.Minute;
        DateTime finalvalue = value.AddMinutes(-prevmin);
        int finalmin = 0;
        if (prevmin > 0 && prevmin <= 30)
            finalmin = 30;
        else if (prevmin > 30)
            finalmin = 60;
        return finalvalue.AddMinutes(finalmin);
    }
    public static DateTime lateroundoffmin_out(DateTime value)
    {
        int prevmin = value.Minute;
        DateTime finalvalue = value.AddMinutes(-prevmin);

        int finalmin = 0;
        if (prevmin < 30)
            finalmin = 0;
        else if (prevmin > 30)
            finalmin = 30;
        return finalvalue.AddMinutes(finalmin);
    }
    public static string roundupanddown(double value)
    {

        double rounded = Math.Floor(value * 2) / 2;
        //string result = string.Format("{0:0.00}", rounded);
        return string.Format("{0:0.00}", rounded);
    }

    public static string utroundoff(double value)
    {
        string v = string.Format("{0:n2}", value);
        string[] splitter = v.ToString().Split('.');
        string decmal = "." + splitter[1].ToString();
        decimal round;
        if (decimal.Parse(decmal) >= decimal.Parse("0.50"))
            round = decimal.Parse(splitter[0]) + 1;
        else if (decimal.Parse(decmal) >= decimal.Parse("0.01") && decimal.Parse(decmal) < decimal.Parse("0.50"))
            round = decimal.Parse(splitter[0]) + decimal.Parse("0.50");
        else
            round = decimal.Parse(splitter[0]);
        return string.Format("{0:0.00}", round);
    }

    public static string getnight(string inn, string oout, string key)
    {
        //set up
        DateTime startnight = new DateTime();
        DateTime endnight = new DateTime();
        DateTime twlvehalftnight = new DateTime();
        DateTime startmorningnight = new DateTime();
        DateTime startDate = Convert.ToDateTime(inn);
        DateTime startDate1 = Convert.ToDateTime(inn);
        DateTime stopDate = Convert.ToDateTime(oout);
        //DateTime stopDate1 = Convert.ToDateTime(oout);
        TimeSpan nod = new TimeSpan();
        TimeSpan nod1 = new TimeSpan();
        TimeSpan tnod = new TimeSpan();

        switch (key)
        {
            case "shiftcode":
                startnight = Convert.ToDateTime("22:00:00.000");
                endnight = Convert.ToDateTime("06:00:00.000").AddDays(1);
                twlvehalftnight = Convert.ToDateTime("00:00:00.000").AddDays(1);
                startmorningnight = Convert.ToDateTime("00:00:00.000").AddDays(1);
                while ((startDate) <= stopDate)
                {
                    if (stopDate > startnight)
                    {
                        if (stopDate > endnight)
                        {
                            if (startDate > startnight)
                                nod = endnight - startDate;
                            else
                                nod = endnight - startnight;
                            break;
                        }
                        else
                        {
                            if (startDate > startnight)
                                nod = stopDate - startDate;
                            else
                                nod = stopDate - startnight;
                            break;
                        }
                    }
                    startDate = startDate.AddHours(1);
                }
                tnod = nod;

                break;
            case "dtr":
                string[] getdate = inn.ToString().Split(' ');
                startnight = Convert.ToDateTime(getdate[0] + " 22:00:00.000");
                //if (startDate.Date != stopDate.Date)
                //{
                endnight = Convert.ToDateTime(getdate[0] + " 06:00:00.000").AddDays(1);
                twlvehalftnight = Convert.ToDateTime(getdate[0] + " 00:00:00.000").AddDays(1);
                startmorningnight = Convert.ToDateTime(getdate[0] + " 00:00:00.000").AddDays(1);
                //}

                while ((startDate) <= stopDate)
                {

                    if (startDate.ToString().Contains("PM"))
                    {
                        if (startDate.TimeOfDay >= startnight.TimeOfDay)
                        {
                            if (stopDate <= endnight)
                            {
                                if (startDate1 > startnight)
                                    nod = stopDate - startDate;
                                else
                                    nod = stopDate - startnight;
                                break;
                            }
                            else
                            {
                                nod = endnight - startDate;
                                break;
                            }
                        }
                    }
                    else if (startDate.ToString().Contains("AM"))
                    {
                        if (startDate.TimeOfDay <= endnight.TimeOfDay)
                        {
                            if (stopDate.TimeOfDay <= endnight.TimeOfDay)
                            {
                                nod = stopDate.TimeOfDay - startDate.TimeOfDay;
                                break;
                            }
                            //else
                            //{
                            //    nod = endnight - startDate;
                            //    break;
                            //}
                        }
                    }

                    startDate = startDate.AddHours(1);
                }
                tnod = nod;
                break;
        }
        return tnod.TotalHours.ToString();

    }

}