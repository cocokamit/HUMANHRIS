using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_payroll_addproccesspayroll : System.Web.UI.Page
{
    public static string conn = Config.connection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            load2x();
            loadablenew();
        }
    }
    protected void verify_det(object sender, EventArgs e)
    {
        if (int.Parse(ddl_pg.SelectedValue) > 0)
        {
            loadable();
            getemp_per_payroll();
            ddl_dtr.Enabled = true;
            ddl_otherdeduction.Enabled = true;
            ddl_otherincome.Enabled = true;
            Button1.Visible = true;
        }
        else
        {
            ddl_dtr.Enabled = false;
            ddl_otherdeduction.Enabled = false;
            ddl_otherincome.Enabled = false;
            Button1.Visible = false;
            grid_item.DataSource = null;
            grid_item.DataBind();
            Response.Write("<script>alert('No Data found!')</script>");
        }
    }
    protected void load2x()
    {
        string query = "select * from MPayrollGroup where payrollgroup<>'Resigned' and status=1  order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadable()
    {
        string query = "select id,left(convert(varchar,datestart,101),10)datestart,left(convert(varchar,dateend,101),10)dateend from TDTR  where status is null and PayrollGroupId=" + ddl_pg.SelectedValue + " and payroll_id is null order by Id desc";
        DataTable dtover = dbhelper.getdata(query);
        ddl_dtr.Items.Clear();
        ddl_dtr.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_dtr.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }
        dtover = dbhelper.getdata("select * from TPayrollOtherIncome where status='True' and action is null and status1='Approved' and PayrollGroupId=" + ddl_pg.SelectedValue + " and payroll_id is null");
        ddl_otherincome.Items.Clear();
        ddl_otherincome.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_otherincome.Items.Add(new ListItem(dr["remarks"].ToString(), dr["id"].ToString()));
        }
        dtover = dbhelper.getdata("select * from TPayrollOtherDeduction where status='True' and PayrollGroupId=" + ddl_pg.SelectedValue + " and payroll_id is null and action is null");
        ddl_otherdeduction.Items.Clear();
        ddl_otherdeduction.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_otherdeduction.Items.Add(new ListItem(dr["remarks"].ToString(), dr["id"].ToString()));
        }


    }
    protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ////cast the sender as gridview
            //GridView gridView = sender as GridView;

            ////create a new gridviewrow
            //GridViewRow gridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);


            //TableCell tableCell = new TableCell();
            //tableCell.Text = "";
            //tableCell.ColumnSpan = 7;
            //tableCell.Attributes.Add("style", "background-color:#333;  ");
            //gridViewRow.Cells.Add(tableCell);

            //TableCell tableDAYS = new TableCell();
            //tableDAYS.Text = "Mandatory Deduction";
            //tableDAYS.ColumnSpan = 3;
            //tableDAYS.Attributes.Add("style", "background-color:#333; color:white;  ");
            //gridViewRow.Cells.Add(tableDAYS);

            //TableCell test1 = new TableCell();
            //test1.Text = "";
            //test1.ColumnSpan = 3;
            //test1.Attributes.Add("style", "background-color:#333; color:white;");
            //gridViewRow.Cells.Add(test1);

            //TableCell DTR_STATUS = new TableCell();
            //DTR_STATUS.Text = "Employer Contribution";
            //DTR_STATUS.ColumnSpan = 4;
            //DTR_STATUS.Attributes.Add("style", "background-color:#333; color:white;");
            //gridViewRow.Cells.Add(DTR_STATUS);


            //// add the new row to the gridview
            //gridView.Controls[0].Controls.AddAt(0, gridViewRow);
        }
    }
    protected void getemp_per_payroll()
    {
        string query = "select a.id empid, a.DivisionId, a.allowhdoffset, a.lastname+', '+a.firstname as empname,b.id payrolltypeid,b.payrolltype,c.id taxcodeid,c.taxcode,a.taxtable,a.accountid,case when (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) is null then a.emp_status  else (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) end empstatus,a.payrollrate,a.dailyrate  from MEmployee a " +
                        "left join mpayrolltype b on a.payrolltypeid=b.id " +
                        "left join mtaxcode c on a.taxcodeid=c.id " +
                        "where a.PayrollGroupId='" + ddl_pg.SelectedValue + "' order by a.lastname+', '+a.firstname asc ";
        DataTable getemp = dbhelper.getdata(query);
        DataRow mdr;
        DataTable master = (DataTable)ViewState["master"];
        int ii = 0;
        foreach (DataRow dr in getemp.Rows)
        {
            mdr = master.NewRow();
            mdr["row"] = ii;
            mdr["empid"] = dr["empid"];
            mdr["employee"] = dr["empname"];
            mdr["paytypeid"] = dr["payrolltypeid"]; //string.Format("{0:n2}", reghours);
            mdr["paytype"] = dr["payrolltype"];
            mdr["taxcodeid"] = dr["taxcodeid"];
            mdr["taxcode"] = dr["taxcode"];
            mdr["taxtable"] = dr["taxtable"];
            mdr["gl_account"] = dr["accountid"];
            mdr["empstatus"] = dr["empstatus"];
            mdr["basicpay"] = dr["payrolltypeid"].ToString() == "1" ? string.Format("{0:n2}", decimal.Parse(dr["payrollrate"].ToString())).Replace(",", "") : string.Format("{0:n2}", decimal.Parse(dr["dailyrate"].ToString())).Replace(",", "");
            
            mdr["lateamt"] = "0.00";
            mdr["undertimeamt"] = "0.00";
            mdr["absentamt"] = "0.00";
            mdr["regularpay"] = "0.00";
            mdr["nightpay"] = "0.00";
            mdr["otamt"] = "0.00";
            mdr["restdaypay"] = "0.00";
            mdr["leavew/pay"] = "0.00";
            mdr["otherincometaxable"] = "0.00";
            mdr["grossincome"] = "0.00";
            mdr["ssscontribution"] = "0.00";
            mdr["phiccontribution"] = "0.00";
            mdr["mdmfcontribution"] = "0.00";
            mdr["withholdingtax"] = "0.00";
            mdr["otherdeduction"] = "0.00";
            mdr["otherincomenontax"] = "0.00";
            mdr["netincome"] = "0.00";
            mdr["employer_ssscontribution"] = "0.00";
            mdr["employer_sssec"] = "0.00";
            mdr["employer_phiccontribution"] = "0.00";
            mdr["employer_mdmfcontribution"] = "0.00";
            mdr["NTI"] = "0.00";
            mdr["NonTaxableNightShiftDifference_prorated"] = "0.00";
            mdr["NonTaxableAllowance_prorated"] = "0.00";
            mdr["NonTaxableActingCapacityAllowance_fix"] = "0.00";
            mdr["NonTaxableNightShiftDifference_prorated_dr"] = "0.00";
            mdr["NonTaxableNightShiftDifference_prorated_hr"] = "0.00";
            mdr["NonTaxableAllowance_prorated_dr"] = "0.00";
            mdr["NonTaxableAllowance_prorated_hr"] = "0.00";
            mdr["NonTaxableNightShiftDifference_prorated_total_amt"] = "0.00";
            mdr["NonTaxableAllowance_prorated_total_amt"] = "0.00";

            mdr["latehrs"] = "0.00";
            mdr["undertimehrs"] = "0.00";
            mdr["absenthrs"] = "0.00";
            mdr["lwophrs"] = "0.00";
            mdr["surge_pay"] = "0.00";
            mdr["TotalNetSalaryAmount"] = "0.00";

            mdr["DivisionId"] =  dr["DivisionId"];

             /**BTK 04082020**/
            mdr["allowhdoffset"] = dr["allowhdoffset"];

            master.Rows.Add(mdr);
            ii++;
        }
        ViewState["master"] = master;
        grid_item.DataSource = master;
        grid_item.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");

    }
    protected void loadable_inside_grid(int rcnt)
    {
        //get emplooyee
        string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                        "left join MPosition b on a.PositionId=b.Id where a.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        DropDownList ddl_emp = (DropDownList)grid_item.Rows[rcnt].Cells[1].FindControl("ddl_emp");
        DropDownList ddl_type = (DropDownList)grid_item.Rows[rcnt].Cells[2].FindControl("ddl_type");
        DropDownList ddl_taxcode = (DropDownList)grid_item.Rows[rcnt].Cells[3].FindControl("ddl_taxcode");
        ddl_emp.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPayrollType";
        dt = dbhelper.getdata(query);
        ddl_type.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_type.Items.Add(new ListItem(dr["payrolltype"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MTaxCode";
        dt = dbhelper.getdata(query);
        ddl_taxcode.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_taxcode.Items.Add(new ListItem(dr["taxcode"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadablenew()
    {
        //MASTER DATA
        DataTable master = new DataTable();
        master.Columns.Add(new DataColumn("row", typeof(string)));
        master.Columns.Add(new DataColumn("empid", typeof(string)));
        master.Columns.Add(new DataColumn("employee", typeof(string)));
        master.Columns.Add(new DataColumn("paytypeid", typeof(string)));
        master.Columns.Add(new DataColumn("paytype", typeof(string)));
        master.Columns.Add(new DataColumn("taxcodeid", typeof(string)));
        master.Columns.Add(new DataColumn("taxcode", typeof(string)));
        master.Columns.Add(new DataColumn("taxtable", typeof(string)));
        master.Columns.Add(new DataColumn("basicpay", typeof(string)));
        

        master.Columns.Add(new DataColumn("lateamt", typeof(string)));
        master.Columns.Add(new DataColumn("undertimeamt", typeof(string)));
        master.Columns.Add(new DataColumn("absentamt", typeof(string)));
        master.Columns.Add(new DataColumn("totalleavehrs", typeof(string)));
        master.Columns.Add(new DataColumn("latehrs", typeof(string)));
        master.Columns.Add(new DataColumn("undertimehrs", typeof(string)));
        master.Columns.Add(new DataColumn("absenthrs", typeof(string)));
        master.Columns.Add(new DataColumn("lwophrs", typeof(string)));
  
  

        master.Columns.Add(new DataColumn("regularpay", typeof(string)));
        master.Columns.Add(new DataColumn("nightpay", typeof(string)));
        master.Columns.Add(new DataColumn("otamt", typeof(string)));
        master.Columns.Add(new DataColumn("restdaypay", typeof(string)));
        master.Columns.Add(new DataColumn("leavew/pay", typeof(string)));
        master.Columns.Add(new DataColumn("hollidaypay", typeof(string)));
        master.Columns.Add(new DataColumn("TotalNetSalaryAmount", typeof(string)));
        master.Columns.Add(new DataColumn("otherincometaxable", typeof(string)));
        master.Columns.Add(new DataColumn("grossincome", typeof(string)));
        master.Columns.Add(new DataColumn("ssscontribution", typeof(string)));
        master.Columns.Add(new DataColumn("phiccontribution", typeof(string)));
        master.Columns.Add(new DataColumn("mdmfcontribution", typeof(string)));
        master.Columns.Add(new DataColumn("withholdingtax", typeof(string)));
        master.Columns.Add(new DataColumn("otherdeduction", typeof(string)));
        master.Columns.Add(new DataColumn("otherincomenontax", typeof(string)));
        master.Columns.Add(new DataColumn("netincome", typeof(string)));

        master.Columns.Add(new DataColumn("employer_ssscontribution", typeof(string)));
        master.Columns.Add(new DataColumn("employer_sssec", typeof(string)));
        master.Columns.Add(new DataColumn("employer_phiccontribution", typeof(string)));
        master.Columns.Add(new DataColumn("employer_mdmfcontribution", typeof(string)));
        master.Columns.Add(new DataColumn("gl_account", typeof(string)));
        master.Columns.Add(new DataColumn("empstatus", typeof(string)));
        master.Columns.Add(new DataColumn("NTI", typeof(string)));

        master.Columns.Add(new DataColumn("dummygrosspay", typeof(string)));


        master.Columns.Add(new DataColumn("NonTaxableNightShiftDifference_prorated", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableAllowance_prorated", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableActingCapacityAllowance_fix", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableNightShiftDifference_prorated_dr", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableNightShiftDifference_prorated_hr", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableAllowance_prorated_dr", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableAllowance_prorated_hr", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableNightShiftDifference_prorated_total_amt", typeof(string)));
        master.Columns.Add(new DataColumn("NonTaxableAllowance_prorated_total_amt", typeof(string)));
        master.Columns.Add(new DataColumn("surge_pay", typeof(string)));
        master.Columns.Add(new DataColumn("MWE", typeof(string)));

        master.Columns.Add(new DataColumn("savingspercentage_amt", typeof(string)));
        master.Columns.Add(new DataColumn("hdmfpercentage_amt", typeof(string)));
        master.Columns.Add(new DataColumn("thirteenmonth", typeof(string)));

      




        master.Columns.Add(new DataColumn("DivisionId", typeof(string)));
         /**BTK 04082020**/
        master.Columns.Add(new DataColumn("allowhdoffset", typeof(string)));

        //MPF
        master.Columns.Add(new DataColumn("mpf_er", typeof(string)));
        master.Columns.Add(new DataColumn("mpf_ee", typeof(string)));

        ViewState["master"] = master;
    }
    protected void click_compute(object sender, EventArgs e)
    {

        //MPF
        decimal sss_mpf = 0;
        decimal sss_mpf_er = 0;
        decimal sss_mpf_ee = 0;

        hdn_gds.Value = "";
        if (int.Parse(ddl_dtr.SelectedValue) > 0)
        {
           

            int i = 0;
            string[] rangee = ddl_dtr.SelectedItem.Text.Split('-');
            decimal nol = 0;
            string query = "";
            DataTable dttt = (DataTable)ViewState["master"];
            query = "select cast(round(dailyrate,12)as numeric(12,2))dd, case when (select top 1 statusid from memployeestatus where empid=a.Id order by datechange desc) is null " +
            "then emp_status else (select top 1 statusid from memployeestatus where empid=a.Id order by datechange desc) end EmployeStatus , * from memployee a";

            DataTable dtemp = dbhelper.getdata(query);
            foreach (DataRow drr in dttt.Rows)
            {

                DataRow[] dtgetemp = dtemp.Select("ID=" + drr["empid"] + "");
                
                query = "select case when sum(Amount) is null then '0.00' else sum(Amount) end Amount from TPayrollOtherDeductionLine where payotherdeduction_id=" + ddl_otherdeduction.SelectedValue + " and Emp_Id=" + drr["empid"] + " ";
                DataTable dtgetpayotherdeductionline = dbhelper.getdata(query);
                if (drr["empid"].ToString() == "405")
                {
                    string jkjk = "dfdf";
                }

                
                //query = "select  " +
                //           "(select case when sum(a.taxable_amt) is null then '0.00' else sum(a.taxable_amt) end nontaxable " +
                //           "from TPayrollOtherIncomeLine a " +
                //           "left join motherincome b on a.otherincome_id=b.id " +
                //           "where a.payotherincome_id='" + ddl_otherincome.SelectedValue + "' and a.Emp_Id='" + drr["empid"] + "' and b.taxable='True') withtax, " +
                //           "(select case when sum(a.Amount) is null then '0.00' else sum(a.Amount) end Amount " +
                //           "from TPayrollOtherIncomeLine a " +
                //           "left join motherincome b on a.otherincome_id=b.id " +
                //           "where a.payotherincome_id='" + ddl_otherincome.SelectedValue + "' and a.Emp_Id='" + drr["empid"] + "' and b.taxable='False')nontax ";
               
                //DataTable dtgetpayotherincomeline = dbhelper.getdata(query);


                DataTable dtgetpayotherincomeline = dbhelper.getdata("select case when sum(a.taxable_amt) is null then '0.00' else sum(a.taxable_amt) end withtax, case when sum(a.nontaxable_amt) is null then '0.00' else sum(a.nontaxable_amt) end nontax	from TPayrollOtherIncomeLine a 	left join motherincome b on a.otherincome_id=b.id where a.payotherincome_id='" + ddl_otherincome.SelectedValue + "' and a.Emp_Id='" + drr["empid"] + "' and a.status is null");

                query = "select top 1 amt from NTI where empid=" + drr["empid"] + " order by id desc ";
                DataTable dtnti = dbhelper.getdata(query);
                query = "select top 1 amt from mealallowance where empid=" + drr["empid"] + " order by id desc ";
                DataTable dtallow = dbhelper.getdata(query);

                if (drr["empid"].ToString() == "50")
                {
                    string testttt = "";
                }


                query = "select " +
                "SUM(TotalRegularWorkingHours + TotalRegularRestdayHours + TotalRegularNightHours + TotalRestdayNightHours)reg_work_hrs, " +
                "SUM(TotalRegularWorkingAmount - hdpremium + (TotalRegularNightAmount - TotalRegularNightPremium))reg_work_amt, " +
                "SUM(TotalRegularNightPremium)regnightpremium, " +
                "SUM(hdpremium)hdregpremium, " +
                    //"SUM(TotalRegularRestdayAmount+TotalRestdayNightAmount)rd_amt, " +
                "SUM(TotalRegularRestdayAmount - rdpremium)rd_amt, " +
                "SUM(TotalRegularOvertimeAmount + TotalRestdayOvertimeAmount + TotalRegularNightOvertimeAmount + TotalRestdayNightOvertimeAmount) ot_amt,SUM(rdpremium) rdpremium,(select SUM(otmeal) from TDTRLine where DTRID = " + ddl_dtr.SelectedValue + " and EmployeeId = " + drr["empid"] + ")otmeal " +
                "from  " +
                "(select  " +
                "(select case when sum(RegularHours + totaloffsethrs) is null then '0' else sum(RegularHours + totaloffsethrs) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + drr["empid"] + "  and daytypeid=a.id and restday='False')TotalRegularWorkingHours,  " +
                "(select case when sum((RegularHours + totaloffsethrs)*rateperhour) is null then '0' else sum((RegularHours + totaloffsethrs)*rateperhour) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + drr["empid"] + "  and daytypeid=a.id and restday='False')TotalRegularWorkingAmount, " +
                "(select case when sum(((RegularHours + totaloffsethrs)*rateperhour)-((RegularHours + totaloffsethrs)*RatePerHourTardy)) is null then '0' else sum(((RegularHours+nighthours + totaloffsethrs)*rateperhour)-((RegularHours + totaloffsethrs+nighthours)*RatePerHourTardy)) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + drr["empid"] + "  and daytypeid=a.id and restday='False')hdpremium, " +

                "(select case when SUM(RegularHours) is null then '0' else  SUM(RegularHours) end  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True')TotalRegularRestdayHours, " +
                "(select case when SUM((RegularHours)*rateperhour) is null then '0' else  SUM((RegularHours)*rateperhour) end  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True')TotalRegularRestdayAmount, " +
                "(select case when sum(((RegularHours)*rateperhour)-((RegularHours)*RatePerHourTardy)) is null then '0' else sum(((RegularHours+nighthours)*rateperhour)-((RegularHours+nighthours)*RatePerHourTardy)) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + drr["empid"] + "  and daytypeid=a.id and restday='True')rdpremium, " +

                "(select case when SUM(nighthours) is null then '0' else  SUM(nighthours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightHours,  " +
                "(select case when SUM(nighthours * (ratepernighthour)) is null then '0' else  SUM(nighthours * (ratepernighthour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightAmount, " +
                "(select case when SUM(nighthours * (ratepernighthour-RatePerHour)) is null then '0' else  SUM(nighthours * (ratepernighthour-RatePerHour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightPremium, " +

                "(select case when SUM(nighthours) is null then '0' else  SUM(nighthours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightHours,  " +
                "(select case when SUM(nighthours * (ratepernighthour)) is null then '0' else  SUM(nighthours * (ratepernighthour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightAmount, " +
                "(select case when SUM(nighthours * (ratepernighthour-RatePerHour)) is null then '0' else  SUM(nighthours * (ratepernighthour-RatePerHour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + " and daytypeid=a.id and restday='True') TotalRestdayNightPremium, " +

                "(select case when SUM(overtimehours) is null then '0' else  SUM(overtimehours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularOvertimeHours, " +
                "(select case when SUM(overtimehours * rateperovertimehour) is null then '0' else  SUM(overtimehours *rateperovertimehour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularOvertimeAmount,  " +
                "(select case when SUM(overtimehours) is null then '0' else  SUM(overtimehours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + " and daytypeid=a.id and restday='True') TotalRestdayOvertimeHours, " +
                "(select case when SUM(overtimehours * rateperovertimehour) is null then '0' else  SUM(overtimehours * rateperovertimehour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayOvertimeAmount, " +

                "(select case when SUM(OvertimeNightHours) is null then '0' else  SUM(OvertimeNightHours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightOvertimeHours, " +
                "(select case when SUM(OvertimeNightHours * rateperovertimenighthour) is null then '0' else  SUM(OvertimeNightHours * rateperovertimenighthour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightOvertimeAmount, " +
                "(select case when SUM(OvertimeNightHours) is null then '0' else  SUM(OvertimeNightHours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightOvertimeHours, " +
                "(select case when SUM(OvertimeNightHours * rateperovertimenighthour) is null then '0' else  SUM(OvertimeNightHours * rateperovertimenighthour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + drr["empid"] + "  and daytypeid=a.id and restday='True')  TotalRestdayNightOvertimeAmount " +
                "from mdaytype a where a.status is null) payrollmaster ";
                DataTable dtdtrline = dbhelper.getdata(query);


                DataTable dtrdeduction = dbhelper.getdata("select " +
                                        "case when (select  SUM(case when OnLeave='True' then " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " else 0 end + case when halfleave='True' then " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " * 0.5  else 0 end)totalleavehours from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) is null then 0 else (select  SUM(case when OnLeave='True' then " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " else 0 end + case when halfleave='True' then " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " * 0.5  else 0 end)totalleavehours from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) end TotalleaveHours, " +
                                        "(select case when SUM(TardyLateHours) is null then '0' else  SUM(TardyLateHours) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) TotalTardyLateHours, " +
                                        "(select case when SUM(TardyUndertimeHours) is null then '0' else  SUM(TardyUndertimeHours) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) TotalTardyUndertimeHours, " +
                                        "(select case when SUM(tardyamount) is null then '0' else SUM(tardyamount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) TotalTardyAmount, " +
                                        "(select COUNT(*) * " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " from tdtrline where Absent='True' and DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + ") Wabsenthrs ," +
                                        "(select COUNT(*) * (" + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " * 0.5 ) from tdtrline where HalfdayAbsent='True' and DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + ") Habsenthrs ," +
                                        "(select COUNT(*) * " + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " from TLeaveApplicationLine  where  WithPay='False' and NumberOfHours = 1 and dtr_id=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + ")wLWOP ," +
                                        "(select COUNT(*) * (" + decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString()) + " * 0.5 ) from TLeaveApplicationLine  where  WithPay='False' and NumberOfHours < 1 and dtr_id=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + ") hLWOP ," +
                                        "(select case when SUM(AbsentAmount) is null then '0' else SUM(AbsentAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) TotalAbsentAmount, " +
                                        "(select case when SUM(LAmount) is null then '0' else  SUM(LAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) LAmount, " +
                                        "(select case when SUM(UAmount) is null then '0' else  SUM(UAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) UAmount, " +
                                        "(select case when sum(LeaveAmount) is null then 0 else sum(LeaveAmount) end from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + " and EmployeeId=" + drr["empid"] + ") Leavewpay,  " +
                                        "(select case when SUM(meal) is null then '0' else  SUM(meal) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + " ) t_meal_allow, " +
                                        "(select case when SUM(NetAmount) is null then '0' else  SUM(NetAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + "  ) TotalNetSalaryAmount, " +
                                        "(select case when SUM(surge_pay) is null then '0' else  SUM(surge_pay) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + drr["empid"] + "  ) surgepay ");

                //decimal otmealamt = 0;
                decimal Regamt = 0;
                decimal payrollrate = 0;
                decimal total_otpay = 0;
                decimal total_rdpay = 0;
                decimal total_hdpay = 0;
                decimal total_regpay = 0;
                decimal TotalNetSalaryAmount = 0;
                decimal lwpay = 0;
                decimal nightpay = 0;
                decimal NonTaxableNightShiftDifference_prorated = 0;
                decimal NonTaxableAllowance_prorated = 0;
                decimal NonTaxableActingCapacityAllowance_fix = 0;
                decimal NonTaxableNightShiftDifference_prorated_dr = 0;
                decimal NonTaxableNightShiftDifference_prorated_hr = 0;
                decimal NonTaxableAllowance_prorated_dr = 0;
                decimal NonTaxableAllowance_prorated_hr = 0;
                decimal NonTaxableNightShiftDifference_prorated_total_amt = 0;
                decimal NonTaxableAllowance_prorated_total_amt = 0;
                decimal surgepay = 0;

                decimal latehrs = 0;
                decimal uthrs = 0;
                decimal absenthrs = 0;
                decimal lwophrs = 0;
                decimal totalleavehrs = 0;

                decimal savingpercentage = 0;
                decimal hdmfpercentage = 0;
                decimal thirteenmonth = 0;
                decimal t_hrs = decimal.Parse(dtdtrline.Rows[0]["reg_work_hrs"].ToString()) + decimal.Parse(dtrdeduction.Rows[0]["TotalleaveHours"].ToString());

                decimal basicpay = t_hrs > 0 ? decimal.Parse(dttt.Rows[i]["basicpay"].ToString()) : 0;
                decimal lamt = decimal.Parse(dtrdeduction.Rows[0]["LAmount"].ToString());
                decimal uamt = decimal.Parse(dtrdeduction.Rows[0]["UAmount"].ToString());
                decimal absentamt = t_hrs > 0 ? decimal.Parse(dtrdeduction.Rows[0]["TotalAbsentAmount"].ToString()) : 0;

                latehrs = decimal.Parse(dtrdeduction.Rows[0]["TotalTardyLateHours"].ToString());
                uthrs = decimal.Parse(dtrdeduction.Rows[0]["TotalTardyUndertimeHours"].ToString());
                absenthrs = decimal.Parse(dtrdeduction.Rows[0]["Wabsenthrs"].ToString()) + decimal.Parse(dtrdeduction.Rows[0]["Habsenthrs"].ToString());
                lwophrs = decimal.Parse(dtrdeduction.Rows[0]["wLWOP"].ToString()) + decimal.Parse(dtrdeduction.Rows[0]["hLWOP"].ToString());
                totalleavehrs =dtrdeduction.Rows[0]["TotalleaveHours"].ToString().Length>0? decimal.Parse(dtrdeduction.Rows[0]["TotalleaveHours"].ToString()):0;
                surgepay = 0;

                TotalNetSalaryAmount = decimal.Parse(dtrdeduction.Rows[0]["TotalNetSalaryAmount"].ToString()); 

                if (drr["paytypeid"].ToString() == "1")
                {
                    total_regpay = decimal.Parse(dtdtrline.Rows[0]["reg_work_hrs"].ToString()) > 0 ? basicpay : decimal.Parse(dtdtrline.Rows[0]["reg_work_amt"].ToString());
                    total_regpay = decimal.Parse(dtdtrline.Rows[0]["reg_work_hrs"].ToString()) > 0 ? total_regpay - lamt - uamt - absentamt : total_regpay;
                }
                else
                    total_regpay = decimal.Parse(dtdtrline.Rows[0]["reg_work_amt"].ToString());


                //otmealamt = decimal.Parse(dtdtrline.Rows[0]["otmeal"].ToString());
                total_otpay = decimal.Parse(dtdtrline.Rows[0]["ot_amt"].ToString());
                nightpay = decimal.Parse(dtdtrline.Rows[0]["regnightpremium"].ToString());
                total_rdpay = dtgetemp[0]["FixNumberOfDays"].ToString() == "365" ? decimal.Parse(dtdtrline.Rows[0]["rdpremium"].ToString()) : decimal.Parse(dtdtrline.Rows[0]["rd_amt"].ToString()) + decimal.Parse(dtdtrline.Rows[0]["rdpremium"].ToString());// decimal.Parse(dtdtrline.Rows[0]["rdpremium"].ToString()); 
                total_hdpay = decimal.Parse(dtdtrline.Rows[0]["hdregpremium"].ToString());

                

                decimal gettgrosshrs = 0;
                decimal total_leave = 0;
                decimal otamt = total_otpay;
                decimal rdpay = total_rdpay;
                lwpay = decimal.Parse(dtrdeduction.Rows[0]["Leavewpay"].ToString()) > basicpay ? basicpay : decimal.Parse(dtrdeduction.Rows[0]["Leavewpay"].ToString());// dtgettdtrline.Rows.Count > 0 ? gettgrosshrs > 0 ? dtgettdtrline.Rows[0]["Leavewpay"].ToString() : "0.00" : "0.00";
                decimal hpay = total_hdpay;

                 /**BTK 02172020
                 * MANAGERIAL EXCEPTION**/
                if (drr["DivisionId"].ToString() == "1" || drr["DivisionId"].ToString() == "4" || drr["DivisionId"].ToString() == "5")
                {
                    otamt = 0;
                    rdpay = 0;
                    hpay = 0;
                    nightpay = 0;
                }

                  /**BTK 04082020
                 * IF EMPLOYEE WAS GRANT HOLIDAY OFFSET NO PREMIUM PAY**/
                if (drr["DivisionId"].ToString() == "2" || drr["DivisionId"].ToString() == "3")
                {
                    if (drr["allowhdoffset"].ToString() == "True")
                        hpay = 0;
                }

                DataTable basicotherincome = dbhelper.getdata( "select ISNULL(z.taxable_amt + z.nontaxable_amt,0)OI from TPayrollOtherIncomeLine z " +
                                            "left join motherincome y on z.OtherIncome_id=y.id " +
                                            "where (y.incometypeid=12 or y.incometypeid=13) and z.status is null " +
                                            "and z.Emp_id=" + drr["empid"] + " and z.PayOtherIncome_id=" + ddl_otherincome.SelectedValue + "");
                decimal basicOI=basicotherincome.Rows.Count>0?decimal.Parse(basicotherincome.Rows[0]["OI"].ToString()):0;

                decimal getemployeesavings = (total_regpay + basicOI) * decimal.Parse("0.05");

                
                dttt.Rows[i]["basicpay"] = string.Format("{0:n2}", basicpay).Replace(",", "");
                dttt.Rows[i]["lateamt"] = string.Format("{0:n2}", lamt).Replace(",", "");
                dttt.Rows[i]["undertimeamt"] = string.Format("{0:n2}", uamt).Replace(",", "");
                dttt.Rows[i]["absentamt"] = string.Format("{0:n2}", absentamt).Replace(",", "");
                dttt.Rows[i]["otamt"] = string.Format("{0:n2}", otamt).Replace(",", "");
                dttt.Rows[i]["restdaypay"] = string.Format("{0:n2}", rdpay).Replace(",", "");
                dttt.Rows[i]["hollidaypay"] = string.Format("{0:n2}", hpay).Replace(",", "");
                dttt.Rows[i]["leavew/pay"] = string.Format("{0:n2}", lwpay).Replace(",", "");

                if (dtgetemp[0]["payrolltypeid"].ToString() == "1")
                    payrollrate = 0;
                else
                    payrollrate = 0;

                int nod_month = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                /**IKN 04292020
                 * COMMENT [basicpay]**/
                //basicpay = 0;

                total_regpay = dtgetemp[0]["payrolltypeid"].ToString() == "1" ? total_regpay > lwpay?total_regpay - lwpay:0 : total_regpay;

                //*IKN 04292020
                //total_regpay = dtgetemp[0]["payrolltypeid"].ToString() == "1" ? decimal.Parse(dtdtrline.Rows[0]["reg_work_hrs"].ToString()) > 0 ? total_regpay - lwpay : total_regpay : total_regpay;
                total_regpay = total_regpay > 0 ? total_regpay : 0;
                decimal oit = dtgetpayotherincomeline.Rows.Count > 0 ? t_hrs > 0 ? decimal.Parse(dtgetpayotherincomeline.Rows[0]["withtax"].ToString()) : 0 : decimal.Parse("0.00");

                /**IKN 04292020
                * CHANGE COMPUTATION**/
                //decimal grossincome = oit + total_regpay + otamt + rdpay + lwpay + hpay + nightpay + surgepay;
                decimal grossincome = (basicpay + oit + otamt + rdpay + hpay + nightpay) - (lamt + uamt + absentamt);

                dttt.Rows[i]["regularpay"] = string.Format("{0:n2}", total_regpay).Replace(",","");
                dttt.Rows[i]["nightpay"] = string.Format("{0:n2}", nightpay).Replace(",", "");
                dttt.Rows[i]["otherincometaxable"] = string.Format("{0:n2}", oit).Replace(",", "");
                dttt.Rows[i]["grossincome"] = string.Format("{0:n2}", grossincome).Replace(",", "");

                decimal mmrate = dtgetemp[0]["MonthlyRate"].ToString().Length > 0? decimal.Parse(dtgetemp[0]["MonthlyRate"].ToString()) > 0 ? decimal.Parse(dtgetemp[0]["MonthlyRate"].ToString()) : 0 : 0;
                decimal ddrate = dtgetemp[0]["DailyRate"].ToString().Length > 0 ? decimal.Parse(dtgetemp[0]["DailyRate"].ToString()) > 0 ? decimal.Parse(dtgetemp[0]["DailyRate"].ToString()) : 0 : 0;
                decimal amount_mandatory_deduct = dtgetemp[0]["payrolltypeid"].ToString() == "1" ? mmrate : grossincome;

                //DataTable prevgross = dbhelper.getdata("select b.grossincome from tpayroll a left join tpayrollline b on a.id=b.payrollid left join tdtr c on a.dtrid=c.id where MONTH(c.datestart)=" + Convert.ToDateTime(rangee[0]).Month + " and DAY(c.datestart)=18 and YEAR(c.datestart)=" + Convert.ToDateTime(rangee[0]).Year + " and b.employeeid=" + drr["empid"] + " and a.status is null");
                //decimal prevgrossincome = prevgross.Rows.Count > 0 ? decimal.Parse(prevgross.Rows[0]["grossincome"].ToString()) : 0;

                //decimal ssmrate = grossincome + prevgrossincome;
                ////dtgetemp[0]["payrolltypeid"].ToString() == "1" ? mmrate : (ddrate * decimal.Parse(dtgetemp[0]["fixnumberofdays"].ToString()) / 12);

                //query = "select * from MstTableSSS where  AmountStart <= " + ssmrate + "  and AmountEnd  >= " + ssmrate + ""; //fixed monthly deduction for SSS
                //DataTable dtgetssstables = dbhelper.getdata(query);
                //query = "select * from MstTablePHIC where  AmountStart <= " + ssmrate + "  and AmountEnd  >= " + ssmrate + "";
                //DataTable dtgetphictables = dbhelper.getdata(query);
                //query = "select * from MstTableHDMF where  AmountStart <= " + ssmrate + "  and AmountEnd  >= " + ssmrate + "";
                //DataTable dtgethdmftables = dbhelper.getdata(query);

                int prevcuttoff = Convert.ToDateTime(rangee[0]).Month;
                DataTable prevgross = dbhelper.getdata("select b.grossincome from tpayroll a left join tpayrollline b on a.id=b.payrollid left join tdtr c on a.dtrid=c.id where MONTH(c.datestart)=" + prevcuttoff + " and DAY(c.datestart)=11 and YEAR(c.datestart)=" + Convert.ToDateTime(rangee[0]).Year + " and b.employeeid=" + drr["empid"] + " and a.status is null");
                decimal prevgrossincome = prevgross.Rows.Count > 0 ? decimal.Parse(prevgross.Rows[0]["grossincome"].ToString()) : 0;

                decimal ssmrate = grossincome + prevgrossincome;
                decimal sssmratebasicpay = dtgetemp[0]["payrolltypeid"].ToString() == "1" ? mmrate : (ddrate * decimal.Parse(dtgetemp[0]["fixnumberofdays"].ToString()) / 12);

                query = "select * from MstTableSSS where  AmountStart <= " + ssmrate + "  and AmountEnd  >= " + ssmrate + ""; //fixed monthly deduction for SSS
                DataTable dtgetssstables = dbhelper.getdata(query);
                query = "select * from MstTablePHIC where  AmountStart <= " + sssmratebasicpay + "  and AmountEnd  >= " + sssmratebasicpay + "";
                DataTable dtgetphictables = dbhelper.getdata(query);
                query = "select * from MstTableHDMF where  AmountStart <= " + ssmrate + "  and AmountEnd  >= " + ssmrate + "";
                DataTable dtgethdmftables = dbhelper.getdata(query);

                decimal sss = 0;
                decimal sssec = 0;
                decimal phic = 0;
                decimal mdmf = 0;
                decimal sssemp = 0;
                decimal sssecemp = 0;
                decimal phicemp = 0;
                decimal hdmfemp = 0;
                decimal NTI = 0;
                decimal fmr = 0;

                string taxtble = "2";
                if (dtgetemp[0]["TaxTable"].ToString().ToUpper() == "SEMI-MONTHLY")
                    taxtble = "2";
                else if (dtgetemp[0]["TaxTable"].ToString() == "MONTHLY")
                    taxtble = "1";


                /**BTK 02132020
                 * CONSULTANT (13) NO GOVT DEDUCTION
                 * ADD CONDITION **/

                    /**GOVT DEDUCTION ON 3TH DAY OF THE MONTH**/
                    if (Convert.ToDateTime(rangee[0]).Day == 26)
                    {
                        sss = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EmployeeContribution"].ToString()) : 0;
                        sssec = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EmployeeEC"].ToString()) : 0;
                        //sssemp = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EmployerContribution"].ToString()) : 0;
                        sssemp = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EmployerContribution"].ToString()) + decimal.Parse(dtgetssstables.Rows[0]["er_mpf"].ToString()) + decimal.Parse(dtgetssstables.Rows[0]["EmployerEC"].ToString()) : 0;
                        sssecemp = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EmployerEC"].ToString()) : 0;

                        //MPF
                        sss_mpf_er = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["ER_MPF"].ToString()) : 0;
                        sss_mpf_ee = dtgetssstables.Rows.Count > 0 ? decimal.Parse(dtgetssstables.Rows[0]["EE_MPF"].ToString()) : 0;
                    }
                    if (Convert.ToDateTime(rangee[0]).Day == 11)
                    {
                        mdmf = (decimal.Parse(dtgethdmftables.Rows.Count > 0 ? dtgethdmftables.Rows[0]["EmployeeValue"].ToString() : "0.00"));
                        hdmfemp = (decimal.Parse(dtgethdmftables.Rows.Count > 0 ? dtgethdmftables.Rows[0]["EmployerValue"].ToString() : "0.00"));
                        if (dtgetphictables.Rows.Count > 0)
                        {
                            if (dtgetphictables.Rows[0]["EmployeeContribution"].ToString().Contains("-"))
                            {
                                phic = ((Math.Round(sssmratebasicpay) * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())) / 2);
                                phicemp = ((Math.Round(sssmratebasicpay) * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())) / 2);
                            }
                            else if (dtgetphictables.Rows[0]["EmployeeContribution"].ToString() == "900.00")
                            {
                                phic = decimal.Parse(dtgetphictables.Rows[0]["EmployeeContribution"].ToString());
                                phicemp = decimal.Parse(dtgetphictables.Rows[0]["EmployerContribution"].ToString());
                            }
                            else
                            {
                                phic = decimal.Parse(dtgetphictables.Rows[0]["EmployeeContribution"].ToString()) / 2;
                                phicemp = decimal.Parse(dtgetphictables.Rows[0]["EmployerContribution"].ToString()) / 2;
                            }
                        }
                    }

                string [] dtrrange=ddl_dtr.SelectedItem.Text.Replace(" ","").Split('-');
                DateTime lastrange = Convert.ToDateTime(dtrrange[1]);
                int day =lastrange.Day;

                NonTaxableNightShiftDifference_prorated =decimal.Parse(dtgetemp[0]["NonTaxableNightShiftDifference"].ToString())>0 ? decimal.Parse(dtgetemp[0]["NonTaxableNightShiftDifference"].ToString()) / 2:0;
                NonTaxableAllowance_prorated =  decimal.Parse(dtgetemp[0]["NonTaxableAllowance"].ToString())>0? decimal.Parse(dtgetemp[0]["NonTaxableAllowance"].ToString()) / 2:0;
                NonTaxableActingCapacityAllowance_fix = decimal.Parse(dtgetemp[0]["NonTaxableActingCapacityAllowance"].ToString())>0? decimal.Parse(dtgetemp[0]["NonTaxableActingCapacityAllowance"].ToString()) / 2:0;
                NonTaxableNightShiftDifference_prorated_dr = NonTaxableNightShiftDifference_prorated * 12 / decimal.Parse(dtgetemp[0]["FixNumberOfDays"].ToString());
                NonTaxableNightShiftDifference_prorated_hr = NonTaxableNightShiftDifference_prorated_dr / decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString());
                NonTaxableAllowance_prorated_dr = NonTaxableAllowance_prorated * 12 / decimal.Parse(dtgetemp[0]["FixNumberOfDays"].ToString());
                NonTaxableAllowance_prorated_hr = NonTaxableAllowance_prorated_dr / decimal.Parse(dtgetemp[0]["FixNumberOfHours"].ToString());

                decimal t_deduct_hrs = latehrs + uthrs + absenthrs + lwophrs;
               


                NonTaxableNightShiftDifference_prorated_total_amt = NonTaxableNightShiftDifference_prorated - (t_deduct_hrs * NonTaxableNightShiftDifference_prorated_hr);
                NonTaxableAllowance_prorated_total_amt = NonTaxableAllowance_prorated - (t_deduct_hrs * NonTaxableAllowance_prorated_hr);
                decimal totalotherincomenti = dtgetpayotherincomeline.Rows.Count > 0 ? decimal.Parse(dtgetpayotherincomeline.Rows[0]["nontax"].ToString()) : decimal.Parse("0.00");
                NTI = Math.Round(NonTaxableNightShiftDifference_prorated_total_amt, 2) + Math.Round(NonTaxableAllowance_prorated_total_amt,2) ;
                decimal emp_sss = t_hrs > 0 ? grossincome > 0 ? sss : 0 : 0;
                eec.Value = sssec.ToString();
                decimal employer_sss = t_hrs > 0 ? grossincome > 0 ? sssemp : 0 : 0;
                decimal employer_sssec = t_hrs > 0 ? grossincome > 0 ? sssecemp : 0 : 0;

                decimal emp_phic = t_hrs > 0 ? grossincome > 0 ? phic : 0 : 0;
                decimal employer_phic = t_hrs > 0 ? grossincome > 0 ? phicemp : 0 : 0;

                decimal emp_mdmf = t_hrs > 0 ? grossincome > 0 ? mdmf : 0 : 0;
                decimal employer_hmdf = t_hrs > 0 ? grossincome > 0 ? hdmfemp : 0 : 0;


                DataTable dtprevpayline = dbhelper.getdata("select * from tpayrollline a left join tpayroll b on a.PayrollId=b.Id where b.status is null and EmployeeId=" + drr["empid"] + "");
                if (dtprevpayline.Rows.Count == 0)
                {
                    emp_sss = grossincome >= 1000 ? emp_sss : 0;
                    emp_phic = grossincome >= 1000 ? emp_phic : 0;
                    emp_mdmf = grossincome >= 1000 ? emp_mdmf : 0;

                    employer_sss = grossincome >= 1000 ? employer_sss : 0;
                    employer_sssec = grossincome >= 1000 ? employer_sssec : 0;
                    employer_phic = grossincome >= 1000 ? employer_phic : 0;
                    employer_hmdf = grossincome >= 1000 ? employer_hmdf : 0;

                }

                decimal emp_sss_final = emp_sss + (dtgetemp[0]["SSSAddOn"].ToString().Length > 0 ? decimal.Parse(dtgetemp[0]["SSSAddOn"].ToString()) : 0);
                decimal emp_hdmf_final = emp_mdmf + (dtgetemp[0]["hdmfAddOn"].ToString().Length > 0 ? decimal.Parse(dtgetemp[0]["hdmfAddOn"].ToString()) : 0);


                dttt.Rows[i]["ssscontribution"] = string.Format("{0:n5}", emp_sss_final).Replace(",", "");
                dttt.Rows[i]["phiccontribution"] = string.Format("{0:n5}", emp_phic).Replace(",", "");
                dttt.Rows[i]["mdmfcontribution"] = string.Format("{0:n5}", emp_hdmf_final).Replace(",", "");

                dttt.Rows[i]["employer_ssscontribution"] = string.Format("{0:n5}",employer_sss).Replace(",", "");
                dttt.Rows[i]["employer_sssec"] = string.Format("{0:n5}", employer_sssec).Replace(",", "");
                dttt.Rows[i]["employer_phiccontribution"] = string.Format("{0:n5}", employer_phic).Replace(",", "");
                dttt.Rows[i]["employer_mdmfcontribution"] = string.Format("{0:n5}", employer_hmdf).Replace(",", "");
                DataTable gettax = new DataTable();
                decimal grossminusmandatorydedduction = new decimal();
                //grossminusmandatorydedduction = grossincome - emp_sss - emp_phic - emp_mdmf;

                //MPF
                grossminusmandatorydedduction = grossincome - (emp_sss + emp_phic + emp_mdmf + sss_mpf_ee);

                string queryyminimum = "select top 1 min_amt from mminimum "; //where convert(date,effectivedate)<=convert(date,'" + Convert.ToDateTime(rangee[0]) + "') order by ID desc";
                DataTable dtqueryyminimum = dbhelper.getdata(queryyminimum);
                string amttotax = decimal.Parse(dtqueryyminimum.Rows[0]["min_amt"].ToString()) >= decimal.Parse(dtgetemp[0]["dd"].ToString()) ? oit.ToString() : grossminusmandatorydedduction.ToString();
                
               
                query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='3' and  Amount<='" + amttotax + "' order by Amount desc";
                gettax = dbhelper.getdata(query);

                decimal getpercent = 0;
                decimal amounttax = 0;
                decimal amountcolumn = 0;
                if (gettax.Rows.Count > 0)
                {
                    getpercent = decimal.Parse(gettax.Rows[0]["percentage"].ToString());
                    amounttax = decimal.Parse(gettax.Rows[0]["tax"].ToString());
                    amountcolumn = decimal.Parse(gettax.Rows[0]["amount"].ToString());
                }
                else
                {
                    getpercent = 0;
                    amounttax = 0;
                    amountcolumn = 0;
                }
                getpercent = getpercent.ToString().Length == 1 ? decimal.Parse("0.0" + getpercent) : decimal.Parse("0." + getpercent);
                decimal getwht = getpercent * (decimal.Parse(amttotax) - amountcolumn) + amounttax;
                decimal wht = t_hrs > 0 ? getwht : 0;
                decimal od = t_hrs > 0 ? dtgetpayotherdeductionline.Rows.Count > 0 ? decimal.Parse(dtgetpayotherdeductionline.Rows[0]["Amount"].ToString()) : 0 : 0;
                decimal oint = t_hrs > 0 ? totalotherincomenti : 0;
                NTI = t_hrs > 0 ? NTI : 0;
                decimal total_add_on = (dtgetemp[0]["SSSAddOn"].ToString().Length > 0 ? decimal.Parse(dtgetemp[0]["SSSAddOn"].ToString()) : 0) + (dtgetemp[0]["hdmfAddOn"].ToString().Length > 0 ? decimal.Parse(dtgetemp[0]["hdmfAddOn"].ToString()) : 0);
                decimal ni = (grossminusmandatorydedduction + oint) - (wht + od + total_add_on);
                ni = ni > 0 ? ni : 0;
                dttt.Rows[i]["withholdingtax"] = string.Format("{0:n5}", wht).Replace(",", "");
                dttt.Rows[i]["otherdeduction"] = string.Format("{0:n5}", od).Replace(",", "");
                dttt.Rows[i]["otherincomenontax"] = string.Format("{0:n5}", oint).Replace(",", "");
                dttt.Rows[i]["netincome"] = string.Format("{0:n5}", ni).Replace(",", ""); ;
                dttt.Rows[i]["NTI"] = string.Format("{0:n5}", NTI).Replace(",", ""); ;
                dttt.Rows[i]["dummygrosspay"] = string.Format("{0:n5}", grossincome);
               


                dttt.Rows[i]["NonTaxableNightShiftDifference_prorated"] = string.Format("{0:n5}", NonTaxableNightShiftDifference_prorated).Replace(",", "");
                dttt.Rows[i]["NonTaxableAllowance_prorated"] = string.Format("{0:n5}", NonTaxableAllowance_prorated).Replace(",", "");
                dttt.Rows[i]["NonTaxableActingCapacityAllowance_fix"] = string.Format("{0:n5}", NonTaxableActingCapacityAllowance_fix).Replace(",", "");
                dttt.Rows[i]["NonTaxableNightShiftDifference_prorated_dr"] = string.Format("{0:n5}", NonTaxableNightShiftDifference_prorated_dr).Replace(",", ""); ;
                dttt.Rows[i]["NonTaxableNightShiftDifference_prorated_hr"] = string.Format("{0:n5}", NonTaxableNightShiftDifference_prorated_hr).Replace(",", ""); ;
                dttt.Rows[i]["NonTaxableAllowance_prorated_dr"] = string.Format("{0:n5}", NonTaxableAllowance_prorated_dr).Replace(",", "");
                dttt.Rows[i]["NonTaxableAllowance_prorated_hr"] = string.Format("{0:n5}", NonTaxableAllowance_prorated_hr).Replace(",", "");
                dttt.Rows[i]["NonTaxableNightShiftDifference_prorated_total_amt"] = string.Format("{0:n5}", NonTaxableNightShiftDifference_prorated_total_amt).Replace(",", "");
                dttt.Rows[i]["NonTaxableAllowance_prorated_total_amt"] = string.Format("{0:n5}", NonTaxableAllowance_prorated_total_amt).Replace(",", ""); ;

                dttt.Rows[i]["latehrs"] = string.Format("{0:n5}", latehrs).Replace(",", "");
                dttt.Rows[i]["undertimehrs"] = string.Format("{0:n5}", uthrs).Replace(",", "");
                dttt.Rows[i]["absenthrs"] = string.Format("{0:n5}", absenthrs).Replace(",", "");
                dttt.Rows[i]["lwophrs"] = string.Format("{0:n5}", lwophrs).Replace(",", "");
                dttt.Rows[i]["totalleavehrs"] = string.Format("{0:n5}", totalleavehrs).Replace(",", ""); 
                dttt.Rows[i]["TotalNetSalaryAmount"] = string.Format("{0:n5}", TotalNetSalaryAmount).Replace(",", "");
                dttt.Rows[i]["surge_pay"] = string.Format("{0:n5}", surgepay).Replace(",", "");
                dttt.Rows[i]["MWE"] = dtgetemp[0]["IsMinimumWageEarner"].ToString();
                hdmfpercentage = grossincome * decimal.Parse("0.02");
                thirteenmonth = grossincome / 12;
                dttt.Rows[i]["savingspercentage_amt"] = getemployeesavings;
                dttt.Rows[i]["hdmfpercentage_amt"] = hdmfpercentage;
                dttt.Rows[i]["thirteenmonth"] = thirteenmonth;

                //MPF
                dttt.Rows[i]["mpf_er"] = sss_mpf_er.ToString().Replace(",", "").Trim();
                dttt.Rows[i]["mpf_ee"] = sss_mpf_ee.ToString().Replace(",", "").Trim();
                i++;
            }
            ViewState["master"] = dttt;
            grid_item.DataSource = dttt;
            grid_item.DataBind();
            Button4.Visible = true;
            //Button3.Visible = true;
            Button2.Visible = true;
            ddl_pg.Enabled = false;
            ddl_otherincome.Enabled = false;
            ddl_otherdeduction.Enabled = false;
            ddl_dtr.Enabled = false;
        }
        else
        {
            grid_item.DataSource = null;
            grid_item.DataBind();
            Response.Write("<script>alert('No Data Found!')</script>");
            getemp_per_payroll();
        }
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");

    }
    protected void refresh(object sender, EventArgs e)
    {
        Response.Redirect("addproccesspayroll?pg=" + function.Encrypt(ddl_pg.SelectedValue, true) + "");
    }

    protected void click_proccess_dtr(object sender, EventArgs e)
    {
        string[] range = ddl_dtr.SelectedItem.Text.Split('-');
        string[] ryear=range[0].Replace(" ","").Split('/');
        DataTable dtperiod = dbhelper.getdata("select * from mperiod where period=" + DateTime.Now.Year + "");
        DataTable dtcnt = dbhelper.getdata("select count(*) cnt from TPayroll ");
        DataTable inspayrol = dbhelper.getdata("INSERT INTO TPayroll " +
           "([PeriodId] " +
           ",[MonthId] " +
           ",[PayrollNumber] " +
           ",[PayrollDate] " +
           ",[PayrollGroupId] " +
           ",[DTRId] " +
           ",[PayrollOtherIncomeId] " +
           ",[PayrollOtherDeductionId] " +
           ",[Remarks] " +
           ",[EntryUserId] " +
           ",[EntryDateTime] " +
           ",[UpdateUserId] " +
           ",[UpdateDateTime] " +
           ",[LastWithholdingTaxId] " +
           ",[status],Payrolldate_1,pp_from,pp_to)" +
           "VALUES " +
           "(" + dtperiod.Rows[0]["id"].ToString() + " " + //get period
           ",'" + DateTime.Now.Month + "' " + //getmonth
           ",0 " +
           ",getdate() " +
           "," + ddl_pg.SelectedValue + " " +
           "," + ddl_dtr.SelectedValue + " " +
           "," + ddl_otherincome.SelectedValue + " " +
           "," + ddl_otherdeduction.SelectedValue + " " +
           ",'" + txt_remarks.Text.Replace("'", "''") + "' " +
           "," + Session["user_id"].ToString() + "" +
           ",getdate() " +
           "," + Session["user_id"].ToString() + " " +
           ",getdate() " +
           ",0 " +
           ",NULL,'" + txt_PD.Text + "','" + txt_pp_from.Text + "','" + txt_pp_to.Text + "') select scope_identity() id ");
        dbhelper.getdata("update TPayroll set PayrollNumber='" + ryear[2] + "-000" + inspayrol.Rows[0]["id"].ToString() + "' where id =" + inspayrol.Rows[0]["id"].ToString() + "");
        dbhelper.getdata("update TPayrollOtherDeduction set payroll_id=" + inspayrol.Rows[0]["id"].ToString() + " where id=" + ddl_otherdeduction.SelectedValue + "");
        dbhelper.getdata("update TPayrollOtherIncome set payroll_id=" + inspayrol.Rows[0]["id"].ToString() + " where id=" + ddl_otherincome.SelectedValue + "");
        dbhelper.getdata("update TDTR set payroll_id=" + inspayrol.Rows[0]["id"].ToString() + " where id=" + ddl_dtr.SelectedValue + "");
        dbhelper.getdata("insert into govt_deduction_schedule (date,userid,gds_desc,payrollid,payrollgroupid)values(getdate()," + Session["user_id"].ToString() + ",'" + hdn_gds.Value + "'," + inspayrol.Rows[0]["id"].ToString() + ","+ddl_pg.SelectedValue+")");

        DataTable masterdat = (DataTable)ViewState["master"];
        foreach (DataRow dr in masterdat.Rows)
        {
            DataTable getemp = dbhelper.getdata("select * from memployee where id='" + dr["empid"].ToString() + "'");
            //dttt.Rows[i]["latehrs"] = string.Format("{0:n5}", latehrs).Replace(",", "");
            //dttt.Rows[i]["undertimehrs"] = string.Format("{0:n5}", uthrs).Replace(",", "");
            //dttt.Rows[i]["absenthrs"] = string.Format("{0:n5}", absenthrs);
            //dttt.Rows[i]["lwophrs"] = string.Format("{0:n5}", lwophrs);
            //DataTable dtrdeduction = dbhelper.getdata(" select " +
            //                        "(select  SUM(case when OnLeave='True' then 8 else 0 end + case when halfleave='True' then 4 else 0 end)totalleavehours from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + " ) TotalleaveHours, " +
            //                        "(select case when SUM(TardyLateHours) is null then '0' else  SUM(TardyLateHours) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + " ) TotalTardyLateHours, " +
            //                        "(select case when SUM(TardyUndertimeHours) is null then '0' else  SUM(TardyUndertimeHours) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + " ) TotalTardyUndertimeHours, " +
            //                        "(select case when SUM(tardyamount) is null then '0' else  SUM(tardyamount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + " ) TotalTardyAmount, " +
            //                        "(select case when SUM(AbsentAmount) is null then '0' else  SUM(AbsentAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + " ) TotalAbsentAmount, " +
            //                        "(select case when SUM(NetAmount) is null then '0' else  SUM(NetAmount) end from tdtrline where DTRID=" + ddl_dtr.SelectedValue + " and employeeid=" + dr["empid"] + "  ) TotalNetSalaryAmount ");
            
            DataTable master = new DataTable();
            DataRow mdr;
            string paylineid = "0";
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("proc_payroll", con))
                {
                    string PayrollId=inspayrol.Rows[0]["id"].ToString();
                    string EmployeeId=dr["empid"].ToString();
                    string PayrollTypeId=dr["paytypeid"].ToString();
                    string TaxCodeId=dr["taxcodeid"].ToString().Length > 0 ? dr["taxcodeid"].ToString() : "0";
                    string TotalTardyLateHours=dr["latehrs"].ToString().Replace("'", "''").Replace(",", "");
                    string TotalTardyUndertimeHours= dr["undertimehrs"].ToString().Replace("'", "''").Replace(",", "");
                    string TotalSalaryAmount= dr["regularpay"].ToString().Replace(",","");
                    string TotalTardyAmount=(decimal.Parse(dr["lateamt"].ToString().Replace(",", "")) + decimal.Parse(dr["undertimeamt"].ToString().Replace(",", ""))).ToString();
                    string TotalAbsentAmount=dr["absentamt"].ToString().Replace(",", "");
                    string TotalNetSalaryAmount= dr["TotalNetSalaryAmount"].ToString().Replace(",", "");
                    string TotalOtherIncomeTaxable=dr["otherincometaxable"].ToString().Replace("'", "''").Replace(",", "");
                    string GrossIncome= dr["grossincome"].ToString().Replace("'", "''").Replace(",", "");
                    string TotalOtherIncomeNonTaxable = dr["otherincomenontax"].ToString().Replace("'", "''").Replace(",", "");
                    string GrossIncomeWithNonTaxable=dr["otherincomenontax"].ToString().Replace("'", "''").Replace(",", "");
                    string SSSContribution=dr["ssscontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string SSSECContribution=decimal.Parse(dr["grossincome"].ToString().Replace("'", "''").Replace(",", "")) >= 1000?eec.Value:"0.00";
                    string PHICContribution=dr["phiccontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string HDMFContribution=dr["mdmfcontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string Tax= dr["withholdingtax"].ToString().Replace("'", "''").Replace(",", "");
                    string TotalOtherDeduction= dr["otherdeduction"].ToString().Replace("'", "''").Replace(",", "");
                    string NetIncome= dr["netincome"].ToString().Replace("'", "''").Replace(",", "");
                    string SSSContributionEmployer=dr["employer_ssscontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string SSSECContributionEmployer=dr["employer_sssec"].ToString().Replace("'", "''").Replace(",", "");
                    string PHICContributionEmployer= dr["employer_phiccontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string HDMFContributionEmployer= dr["employer_mdmfcontribution"].ToString().Replace("'", "''").Replace(",", "");
                    string AccountId= dr["gl_account"].ToString();
                    string payrollrate = getemp.Rows[0]["payrollrate"].ToString().Length > 0 ? getemp.Rows[0]["payrollrate"].ToString().Replace(",", "") : "0";
                    string dailyrate = getemp.Rows[0]["dailyrate"].ToString().Length > 0 ? getemp.Rows[0]["dailyrate"].ToString().Replace(",", "") : "0";
                    string monthlyrate = getemp.Rows[0]["monthlyrate"].ToString().Length > 0 ? getemp.Rows[0]["monthlyrate"].ToString().Replace(",", "") : "0";
                    string taxtable=getemp.Rows[0]["TaxTable"].ToString().Length > 0 ? getemp.Rows[0]["TaxTable"].ToString() : "0";
                    string hourlyrate = getemp.Rows[0]["hourlyrate"].ToString().Length > 0 ? getemp.Rows[0]["hourlyrate"].ToString().Replace(",", "") : "0";
                    string FixNumberOfDays=getemp.Rows[0]["FixNumberOfDays"].ToString().Length > 0 ? getemp.Rows[0]["FixNumberOfDays"].ToString().Replace(",", "") : "0";
                    string FixNumberOfHours= getemp.Rows[0]["FixNumberOfHours"].ToString().Length > 0 ? getemp.Rows[0]["FixNumberOfHours"].ToString().Replace(",", "") : "0";
                    string AbsentDailyRate = getemp.Rows[0]["dailyrate"].ToString().Length > 0 ? getemp.Rows[0]["dailyrate"].ToString().Replace(",", "") : "0";
                    string NightHourlyRate = "0";
                    string OvertimeHourlyRate = "0";
                    string OvertimeNightHourlyRate = "0";
                    string TardyHourlyRate = "0";
                    string totalregularpay=dr["regularpay"].ToString().Replace("'", "''").Replace(",", "");
                    string totallateamt= dr["lateamt"].ToString().Replace("'", "''").Replace(",", "");
                    string totalundertimeamt=dr["undertimeamt"].ToString().Replace("'", "''").Replace(",", "");
                    string totalot= dr["otamt"].ToString().Replace("'", "''").Replace(",", "");
                    string totalrdpay=dr["restdaypay"].ToString().Replace("'", "''").Replace(",", "");
                    string totalleavepay=dr["leavew/pay"].ToString().Replace("'", "''").Replace(",", "");
                    string empstatus=dr["empstatus"].ToString().Replace("'", "''");
                    string NTI=dr["NTI"].ToString().Replace("'", "''").Replace(",", "");
                    string totalleavehrs=dr["totalleavehrs"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableNightShiftDifference_prorated=dr["NonTaxableNightShiftDifference_prorated"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableAllowance_prorated=dr["NonTaxableAllowance_prorated"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableActingCapacityAllowance_fix=dr["NonTaxableActingCapacityAllowance_fix"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableNightShiftDifference_prorated_dr=dr["NonTaxableNightShiftDifference_prorated_dr"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableNightShiftDifference_prorated_hr= dr["NonTaxableNightShiftDifference_prorated_hr"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableAllowance_prorated_dr=dr["NonTaxableAllowance_prorated_dr"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableAllowance_prorated_hr=dr["NonTaxableAllowance_prorated_hr"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableNightShiftDifference_prorated_total_amt = dr["NonTaxableNightShiftDifference_prorated_total_amt"].ToString().Replace("'", "''").Replace(",", "");
                    string NonTaxableAllowance_prorated_total_amt = dr["NonTaxableAllowance_prorated_total_amt"].ToString().Replace("'", "''").Replace(",", "");
                    string totalhdpay = dr["hollidaypay"].ToString().Replace("'", "''").Replace(",", "");
                    string surgepay = dr["surge_pay"].ToString().Replace("'", "''").Replace(",", "");
                    string MWE = dr["MWE"].ToString();

                    string savingspercentage_amt = dr["savingspercentage_amt"].ToString();
                    string hdmfpercentage_amt = dr["hdmfpercentage_amt"].ToString();
                    string thirteenmonth = dr["thirteenmonth"].ToString();

                    //MPF
                    string mpf_er = dr["mpf_er"].ToString();
                    string mpf_ee = dr["mpf_ee"].ToString();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PayrollId", SqlDbType.Int).Value = PayrollId;
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = EmployeeId;
                    cmd.Parameters.Add("@PayrollTypeId", SqlDbType.Int).Value = PayrollTypeId;
                    cmd.Parameters.Add("@TaxCodeId", SqlDbType.Int).Value = TaxCodeId;
                    cmd.Parameters.Add("@TotalTardyLateHours", SqlDbType.VarChar, 50).Value = TotalTardyLateHours;
                    cmd.Parameters.Add("@TotalTardyUndertimeHours", SqlDbType.VarChar, 50).Value = TotalTardyUndertimeHours;
                    cmd.Parameters.Add("@TotalSalaryAmount", SqlDbType.VarChar, 50).Value = TotalSalaryAmount;
                    cmd.Parameters.Add("@TotalTardyAmount", SqlDbType.VarChar, 50).Value = TotalTardyAmount;// dtrdeduction.Rows[0]["TotalTardyAmount"].ToString().Replace(",", "");
                    cmd.Parameters.Add("@TotalAbsentAmount", SqlDbType.VarChar, 50).Value = TotalAbsentAmount;
                    cmd.Parameters.Add("@TotalNetSalaryAmount", SqlDbType.VarChar, 50).Value = TotalNetSalaryAmount;
                    cmd.Parameters.Add("@TotalOtherIncomeTaxable", SqlDbType.VarChar, 50).Value = TotalOtherIncomeTaxable;
                    cmd.Parameters.Add("@GrossIncome", SqlDbType.VarChar, 50).Value = GrossIncome;
                    cmd.Parameters.Add("@TotalOtherIncomeNonTaxable", SqlDbType.VarChar, 50).Value = TotalOtherIncomeNonTaxable;
                    cmd.Parameters.Add("@GrossIncomeWithNonTaxable", SqlDbType.VarChar, 50).Value = GrossIncomeWithNonTaxable;
                    cmd.Parameters.Add("@SSSContribution", SqlDbType.VarChar, 50).Value = SSSContribution;
                    cmd.Parameters.Add("@SSSECContribution", SqlDbType.VarChar, 50).Value = SSSECContribution;
                    cmd.Parameters.Add("@PHICContribution", SqlDbType.VarChar, 50).Value = PHICContribution;
                    cmd.Parameters.Add("@HDMFContribution", SqlDbType.VarChar, 50).Value = HDMFContribution;
                    cmd.Parameters.Add("@Tax", SqlDbType.VarChar, 50).Value = Tax;
                    cmd.Parameters.Add("@TotalOtherDeduction", SqlDbType.VarChar, 50).Value = TotalOtherDeduction;
                    cmd.Parameters.Add("@NetIncome", SqlDbType.VarChar, 50).Value = NetIncome;
                    cmd.Parameters.Add("@SSSContributionEmployer", SqlDbType.VarChar, 50).Value = SSSContributionEmployer;
                    cmd.Parameters.Add("@SSSECContributionEmployer", SqlDbType.VarChar, 50).Value = SSSECContributionEmployer;
                    cmd.Parameters.Add("@PHICContributionEmployer", SqlDbType.VarChar, 50).Value = PHICContributionEmployer;
                    cmd.Parameters.Add("@HDMFContributionEmployer", SqlDbType.VarChar, 50).Value = HDMFContributionEmployer;
                    cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = AccountId;
                    cmd.Parameters.Add("@payrollrate", SqlDbType.VarChar, 50).Value = payrollrate;
                    cmd.Parameters.Add("@dailyrate", SqlDbType.VarChar, 50).Value = dailyrate;
                    cmd.Parameters.Add("@monthlyrate", SqlDbType.VarChar, 50).Value = monthlyrate;
                    cmd.Parameters.Add("@taxtable", SqlDbType.VarChar, 50).Value = taxtable;
                    cmd.Parameters.Add("@hourlyrate", SqlDbType.VarChar, 50).Value = hourlyrate;
                    cmd.Parameters.Add("@FixNumberOfDays", SqlDbType.VarChar, 50).Value = FixNumberOfDays;
                    cmd.Parameters.Add("@FixNumberOfHours", SqlDbType.VarChar, 50).Value = FixNumberOfHours;
                    cmd.Parameters.Add("@AbsentDailyRate", SqlDbType.VarChar, 50).Value = AbsentDailyRate;
                    cmd.Parameters.Add("@NightHourlyRate", SqlDbType.VarChar, 50).Value = NightHourlyRate;
                    cmd.Parameters.Add("@OvertimeHourlyRate", SqlDbType.VarChar, 50).Value = OvertimeHourlyRate;
                    cmd.Parameters.Add("@OvertimeNightHourlyRate", SqlDbType.VarChar, 50).Value = OvertimeNightHourlyRate;
                    cmd.Parameters.Add("@TardyHourlyRate", SqlDbType.VarChar, 50).Value = TardyHourlyRate;
                    cmd.Parameters.Add("@totalregularpay", SqlDbType.VarChar, 50).Value = totalregularpay;
                    cmd.Parameters.Add("@totallateamt", SqlDbType.VarChar, 50).Value = totallateamt;
                    cmd.Parameters.Add("@totalundertimeamt", SqlDbType.VarChar, 50).Value = totalundertimeamt;
                    cmd.Parameters.Add("@totalot", SqlDbType.VarChar, 50).Value = totalot;
                    cmd.Parameters.Add("@totalrdpay", SqlDbType.VarChar, 50).Value = totalrdpay;
                    cmd.Parameters.Add("@totalleavepay", SqlDbType.VarChar, 50).Value = totalleavepay;
                    cmd.Parameters.Add("@totalhdpay", SqlDbType.VarChar, 50).Value = totalhdpay;
                    cmd.Parameters.Add("@empstatus", SqlDbType.Int).Value = empstatus;
                    cmd.Parameters.Add("@NTI", SqlDbType.VarChar, 50).Value = NTI;
                    cmd.Parameters.Add("@totalleavehrs", SqlDbType.VarChar, 50).Value = totalleavehrs; //dtrdeduction.Rows[0]["totalleavehours"].ToString().Length==0?"0":dtrdeduction.Rows[0]["totalleavehours"].ToString().Replace(",", ""); 


                    cmd.Parameters.Add("@NonTaxableNightShiftDifference_prorated", SqlDbType.VarChar, 50).Value = NonTaxableNightShiftDifference_prorated;
                    cmd.Parameters.Add("@NonTaxableAllowance_prorated", SqlDbType.VarChar, 50).Value = NonTaxableAllowance_prorated;
                    cmd.Parameters.Add("@NonTaxableActingCapacityAllowance_fix", SqlDbType.VarChar, 50).Value = NonTaxableActingCapacityAllowance_fix;
                    cmd.Parameters.Add("@NonTaxableNightShiftDifference_prorated_dr", SqlDbType.VarChar, 50).Value = NonTaxableNightShiftDifference_prorated_dr;
                    cmd.Parameters.Add("@NonTaxableNightShiftDifference_prorated_hr", SqlDbType.VarChar, 50).Value = NonTaxableNightShiftDifference_prorated_hr;
                    cmd.Parameters.Add("@NonTaxableAllowance_prorated_dr", SqlDbType.VarChar, 50).Value = NonTaxableAllowance_prorated_dr;
                    cmd.Parameters.Add("@NonTaxableAllowance_prorated_hr", SqlDbType.VarChar, 50).Value = NonTaxableAllowance_prorated_hr;
                    cmd.Parameters.Add("@NonTaxableNightShiftDifference_prorated_total_amt", SqlDbType.VarChar, 50).Value = NonTaxableNightShiftDifference_prorated_total_amt;
                    cmd.Parameters.Add("@NonTaxableAllowance_prorated_total_amt", SqlDbType.VarChar, 50).Value = NonTaxableAllowance_prorated_total_amt;
                    cmd.Parameters.Add("@surgepay", SqlDbType.VarChar, 50).Value = surgepay;
                    cmd.Parameters.Add("@MWE", SqlDbType.VarChar, 50).Value = MWE;

                    cmd.Parameters.Add("@savingspercentage_amt", SqlDbType.VarChar, 50).Value = savingspercentage_amt;
                    cmd.Parameters.Add("@hdmfpercentage_amt", SqlDbType.VarChar, 50).Value = hdmfpercentage_amt;
                    cmd.Parameters.Add("@thirteenmonth", SqlDbType.VarChar, 50).Value = thirteenmonth;
                    //MPF
                    cmd.Parameters.Add("@mpf_er", SqlDbType.VarChar, 50).Value = mpf_er;
                    cmd.Parameters.Add("@mpf_ee", SqlDbType.VarChar, 50).Value = mpf_ee;
                    
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    paylineid = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }
            string qq ="select id daytypeid,daytype breakdown, " +
                      "round(TotalRegularWorkingHours + TotalRegularNightHours + TotaloffsetHours  ,2)reghrs, " +
                      "round((TotalRegularWorkingAmount-hdpremium) + (TotalRegularNightAmount-TotalRegularNightPremium) + TotaloffsetAmount ,2)regamt, " +
                      "round(hdpremium ,2)hdpremium, " +
                      "convert(varchar,round(TotalRegularNightHours,2))nighthrs, " +
                      "convert(varchar,round(TotalRegularNightPremium,2))nightpremium, " +
                      "convert(varchar,round(TotalRegularOvertimeHours,2))regothrs, " +
                      "convert(varchar,round(TotalRegularOvertimeAmount,2))regotamt, " +
                      "convert(varchar,round(TotalRegularNightOvertimeHours,2))nightothrs, " +
                      "convert(varchar,round(TotalRegularNightOvertimeAmount,2))nightotamt,  " +

                      "case when " + getemp.Rows[0]["payrolltypeid"].ToString() + "=1 then " +
                      "case when cast((TotalRegularWorkingAmount-hdpremium) + (TotalRegularNightAmount-TotalRegularNightPremium)+hdpremium +TotalRegularNightPremium +TotalRegularOvertimeAmount +TotalRegularNightOvertimeAmount + TotaloffsetAmount as numeric(36,2)) > 0 then cast((TotalRegularWorkingAmount-hdpremium) + (TotalRegularNightAmount-TotalRegularNightPremium)+hdpremium +TotalRegularNightPremium +TotalRegularOvertimeAmount +TotalRegularNightOvertimeAmount + TotaloffsetAmount as numeric(36,2)) else rdhrs*tdlrate end  " +
                      "else cast((TotalRegularWorkingAmount-hdpremium) + (TotalRegularNightAmount-TotalRegularNightPremium) + hdpremium + TotalRegularNightPremium + TotalRegularOvertimeAmount +TotalRegularNightOvertimeAmount + TotaloffsetAmount as numeric(36,2)) end totalamt " +
                      "from " +
                      "( " +
                      "select a.daytype,a.id, " +
                      "case when (select count(*)  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') is null then '0' else (select count(*)  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') end rdhrs,  " +
                      "case when (select top 1 rateperabsentday from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='False')is null then '0' else (select top 1 rateperabsentday from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='False') end tdlrate, " +

                      "(select case when sum(RegularHours) is null then '0' else sum(RegularHours) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='False')TotalRegularWorkingHours, " +
                      "(select case when sum((RegularHours)*rateperhour) is null then '0' else sum((RegularHours)*rateperhour) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='False')TotalRegularWorkingAmount,  " + 

                      "(select case when sum(totaloffsethrs) is null then '0' else sum(totaloffsethrs) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + " and daytypeid=a.id)TotaloffsetHours, " +
                      "(select case when sum((totaloffsethrs)*rateperhour) is null then '0' else sum((totaloffsethrs)*rateperhour) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + " and daytypeid=a.id)TotaloffsetAmount,  " +

                      "(select case when sum(((RegularHours)*rateperhour)-((RegularHours)*RatePerHourTardy)) is null then '0' else sum(((RegularHours+nighthours)*rateperhour)-((RegularHours+nighthours)*RatePerHourTardy)) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='False')hdpremium, " +
                      "(select case when SUM(nighthours) is null then '0' else  SUM(nighthours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightHours,  " +
                      "(select case when SUM(nighthours * (ratepernighthour)) is null then '0' else  SUM(nighthours * (ratepernighthour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightAmount, " +
                      "(select case when SUM(nighthours * (ratepernighthour-RatePerHour)) is null then '0' else  SUM(nighthours * (ratepernighthour-RatePerHour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightPremium, " +
                      "(select case when SUM(overtimehours) is null then '0' else  SUM(overtimehours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularOvertimeHours, " +
                      "(select case when SUM(overtimehours * rateperovertimehour) is null then '0' else  SUM(overtimehours *rateperovertimehour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularOvertimeAmount, " +
                      "(select case when SUM(OvertimeNightHours) is null then '0' else  SUM(OvertimeNightHours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightOvertimeHours, " +
                      "(select case when SUM(OvertimeNightHours * rateperovertimenighthour) is null then '0' else  SUM(OvertimeNightHours * rateperovertimenighthour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='False') TotalRegularNightOvertimeAmount " +
                      "from mdaytype a where a.status is null " +
                      "union " +
                      "select 'Rest Day '+a.daytype,'20', " +
                      "case when (select count(*)  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') is null then '0' else (select count(*)  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') end rdhrs,  " +
                      "case when (select top 1 rateperabsentday from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='True')is null then '0' else (select top 1 rateperabsentday from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='True') end tdlrate, " +

                      "(select case when SUM(RegularHours) is null then '0' else  SUM(RegularHours) end  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True')TotalRegularRestdayHours,  " +
                      "(select case when SUM((RegularHours)*rateperhour) is null then '0' else  SUM((RegularHours)*rateperhour) end  from TDTRLine where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True')TotalRegularRestdayAmount, " +

                      "(select case when sum(totaloffsethrs) is null then '0' else sum(totaloffsethrs) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + " and daytypeid=a.id and restday='True')TotaloffsetHours, " +
                      "(select case when sum((totaloffsethrs)*rateperhour) is null then '0' else sum((totaloffsethrs)*rateperhour) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + " and daytypeid=a.id and restday='True')TotaloffsetAmount, " +
 

                      "(select case when sum(((RegularHours)*rateperhour)-((RegularHours)*RatePerHourTardy)) is null then '0' else sum(((RegularHours+nighthours)*rateperhour)-((RegularHours+nighthours)*RatePerHourTardy)) end  from TDTRLine where DTRId=" + ddl_dtr.SelectedValue + "  and EmployeeId=" + dr["empid"] + "  and daytypeid=a.id and restday='True')rdpremium, " +
                      
                      "(select case when SUM(nighthours) is null then '0' else  SUM(nighthours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightHours, " +
                      "(select case when SUM(nighthours * (ratepernighthour)) is null then '0' else  SUM(nighthours * (ratepernighthour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightAmount, " +
                      "(select case when SUM(nighthours * (ratepernighthour-RatePerHour)) is null then '0' else  SUM(nighthours * (ratepernighthour-RatePerHour)) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightPremium, " +
                      "(select case when SUM(overtimehours) is null then '0' else  SUM(overtimehours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayOvertimeHours, " +
                      "(select case when SUM(overtimehours * rateperovertimehour) is null then '0' else  SUM(overtimehours * rateperovertimehour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayOvertimeAmount, " +
                      "(select case when SUM(OvertimeNightHours) is null then '0' else  SUM(OvertimeNightHours) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True') TotalRestdayNightOvertimeHours, " +
                      "(select case when SUM(OvertimeNightHours * rateperovertimenighthour) is null then '0' else  SUM(OvertimeNightHours * rateperovertimenighthour) end  from tdtrline where DTRID=" + ddl_dtr.SelectedValue + "  and employeeid=" + dr["empid"] + "  and daytypeid=a.id and restday='True')  TotalRestdayNightOvertimeAmount  " +
                      "from mdaytype a where a.status is null " +
                      ")hahay order by ID asc";
           DataTable dtinsertpaybreakdown= dbhelper.getdata(qq);

           
           foreach (DataRow bdbr in dtinsertpaybreakdown.Rows)
           {

                if(getemp.Rows[0]["DivisionId"].ToString() == "1" || getemp.Rows[0]["DivisionId"].ToString() == "4" || getemp.Rows[0]["DivisionId"].ToString() == "5")
                { 
                    bdbr["nightpremium"] = "0";
                    bdbr["hdpremium"] = "0";
                }

                /**BTK 04272020
                * IF EMPLOYEE WAS GRANT HOLIDAY OFFSET NO PREMIUM PAY**/
                if (getemp.Rows[0]["DivisionId"].ToString() == "2" || getemp.Rows[0]["DivisionId"].ToString() == "3")
                {
                    if (getemp.Rows[0]["allowhdoffset"].ToString() == "True")
                        bdbr["hdpremium"] = "0";
                }
                 
               string qqqq = "insert into [TPayrollLineBreakDown] ([payrolllineid],[daytypeid],[brekdown],[reghrs],[regamt],[hdpremium],[nighthrs],[nightpremium],[regothrs],[regotamt],[nightothrs],[nightotamt],[totalamt]) values " +
                              "(" + paylineid + "," + bdbr["daytypeid"] + ",'" + bdbr["breakdown"] + "','" + bdbr["reghrs"] + "','" + bdbr["regamt"] + "','" + bdbr["hdpremium"] + "','" + bdbr["nighthrs"] + "','" + bdbr["nightpremium"] + "','" + bdbr["regothrs"] + "','" + bdbr["regotamt"] + "','" + bdbr["nightothrs"] + "','" + bdbr["nightotamt"] + "','" + bdbr["totalamt"] + "')";
               dbhelper.getdata(qqqq);
           }
        }
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master_payroll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "processpayroll";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "" + ddl_pg.SelectedItem.ToString() + "";
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = "" + ddl_pg.SelectedItem.ToString() + " - " + ddl_dtr.SelectedItem.ToString() + "";
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "" + ryear[2] + "-000" + inspayrol.Rows[0]["id"].ToString() + "";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        Response.Redirect("procpay");
    }
    protected void click_computehdmfphic(object sender, EventArgs e)
    {
        int i = 0;
        string query = "";
        DataTable dttt = (DataTable)ViewState["master"];
        foreach (DataRow drr in dttt.Rows)
        {
            string phic = "0";
            string mdmf = "0";

            string phicemp = "0";
            string hdmfemp = "0";

            query = "select * from memployee where ID=" + drr["empid"].ToString() + "";
            DataTable dtgetemp = dbhelper.getdata(query);
            query = "SELECT * FROM MCOMPANY WHERE ID=" + dtgetemp.Rows[0]["companyid"].ToString() + "";
            DataTable dtcompany = dbhelper.getdata(query);
            string taxtble = "0";
            if (dtgetemp.Rows[0]["TaxTable"].ToString() == "SEMI-MONTHLY")
                taxtble = "3";
            else if (dtgetemp.Rows[0]["TaxTable"].ToString() == "Monthly")
                taxtble = "4";
            else if (dtgetemp.Rows[0]["TaxTable"].ToString() == "Daily")
                taxtble = "1";
            else if (dtgetemp.Rows[0]["TaxTable"].ToString() == "Weekly")
                taxtble = "2";

            //DataTable dtgetssstables = dbhelper.getdata(query);
            decimal amount_mandatory_deduct = dtgetemp.Rows[0]["payrolltypeid"].ToString() == "1" ? decimal.Parse(dtgetemp.Rows[0]["MonthlyRate"].ToString()) : decimal.Parse(drr["grossincome"].ToString());
            query = "select * from MstTablePHIC where  AmountStart <= " + amount_mandatory_deduct + "  and AmountEnd  >= " + amount_mandatory_deduct + "";
            DataTable dtgetphictables = dbhelper.getdata(query);

            decimal fmr = 0;
            decimal mmrate = dtgetemp.Rows[0]["MonthlyRate"].ToString().Length > 0 ? decimal.Parse(dtgetemp.Rows[0]["MonthlyRate"].ToString()) : 0;
            decimal ddrate = dtgetemp.Rows[0]["dailyrate"].ToString().Length > 0 ?
            decimal.Parse(dtgetemp.Rows[0]["dailyrate"].ToString()) > 0 ? decimal.Parse(dtgetemp.Rows[0]["dailyrate"].ToString()) : 0 : 0;


            //PHIC TRAIN LAW 2018
            string phicsetup = dtcompany.Rows.Count > 0 ? dtcompany.Rows[0]["PHICLAWYEAR"].ToString() : "0";
            string hdmfsetup = dtcompany.Rows.Count > 0 ? dtcompany.Rows[0]["HDMFLAWYEAR"].ToString() : "0";

            if (dtgetphictables.Rows.Count > 0)
            {
                if (dtgetemp.Rows[0]["payrolltypeid"].ToString() == "1")
                {
                    mmrate = mmrate > decimal.Parse(dtgetphictables.Rows[0]["amountend"].ToString()) ? decimal.Parse(dtgetphictables.Rows[0]["amountend"].ToString()) : mmrate;

                    if (dtgetphictables.Rows[0]["EmployeeContribution"].ToString().Contains("-"))
                    {
                        phic = (mmrate * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())).ToString();
                        phicemp = (mmrate * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())).ToString();
                    }
                    else
                    {
                        phic = dtgetphictables.Rows[0]["EmployeeContribution"].ToString();
                        phicemp = dtgetphictables.Rows[0]["EmployeeContribution"].ToString();
                    }
                }
                else
                {
                    mmrate = (ddrate * decimal.Parse(dtgetemp.Rows[0]["fixnumberofdays"].ToString()) / 12);
                    if (dtgetphictables.Rows[0]["EmployeeContribution"].ToString().Contains("-"))
                    {
                        phic = (mmrate * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())).ToString();
                        phicemp = (mmrate * decimal.Parse(dtgetphictables.Rows[0]["Percentage"].ToString())).ToString();
                    }
                    else
                    {
                        phic = dtgetphictables.Rows[0]["EmployeeContribution"].ToString();
                        phicemp = dtgetphictables.Rows[0]["EmployeeContribution"].ToString();
                    }
                }
            }
            // NOT TRAIN LAW 2017 BELOW
            //    phic = dtgetphictables.Rows[0]["EmployeeContribution"].ToString();
            //    phicemp = dtgetphictables.Rows[0]["EmployeRContribution"].ToString();


            query = "select * from MstTableHDMF where  AmountStart <= " + amount_mandatory_deduct + "  and AmountEnd  >= " + amount_mandatory_deduct + "";
            DataTable dtgethdmftables = dbhelper.getdata(query);
            mdmf = (decimal.Parse(dtgethdmftables.Rows.Count > 0 ? dtgethdmftables.Rows[0]["EmployeeValue"].ToString() : "0.00")).ToString();
            hdmfemp = (decimal.Parse(dtgethdmftables.Rows.Count > 0 ? dtgethdmftables.Rows[0]["EmployerValue"].ToString() : "0.00")).ToString();

            string emp_phic = decimal.Parse(drr["grossincome"].ToString()) > 0 ? Math.Round(decimal.Parse(phic), 2).ToString() : "0.00";
            string emp_mdmf = decimal.Parse(drr["grossincome"].ToString()) > 0 ? Math.Round(decimal.Parse(mdmf), 2).ToString() : "0.00";

            string employer_phic = decimal.Parse(drr["grossincome"].ToString()) > 0 ? phicemp : "0.00";
            string employer_mdmf = decimal.Parse(drr["grossincome"].ToString()) > 0 ? hdmfemp : "0.00";

            dttt.Rows[i]["phiccontribution"] = string.Format("{0:n2}", emp_phic);
            dttt.Rows[i]["mdmfcontribution"] = string.Format("{0:n2}", emp_mdmf);
            dttt.Rows[i]["employer_phiccontribution"] = string.Format("{0:n2}", employer_phic);
            dttt.Rows[i]["employer_mdmfcontribution"] = string.Format("{0:n2}", employer_mdmf);
            string tax = "";
            DataTable gettax = new DataTable();
            decimal grossminusmandatorydedduction = new decimal();
            grossminusmandatorydedduction = decimal.Parse(drr["grossincome"].ToString()) - decimal.Parse(drr["ssscontribution"].ToString()) - decimal.Parse(drr["phiccontribution"].ToString()) - decimal.Parse(drr["mdmfcontribution"].ToString());
            query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='" + taxtble + "' and  Amount<='" + grossminusmandatorydedduction + "' order by Amount desc";
            gettax = dbhelper.getdata(query);
            string getpercent = "";
            string amounttax = "";
            string amountcolumn = "";
            if (gettax.Rows.Count > 0)
            {
                getpercent = gettax.Rows[0]["percentage"].ToString();
                amounttax = gettax.Rows[0]["tax"].ToString();
                amountcolumn = gettax.Rows[0]["amount"].ToString();
            }
            else
            {
                getpercent = "0";
                amounttax = "0";
                amountcolumn = "0";
            }
            getpercent = getpercent.Length == 1 ? "0.0" + getpercent : "0." + getpercent;
            decimal getwht = decimal.Parse(getpercent) * (grossminusmandatorydedduction - decimal.Parse(amountcolumn)) + decimal.Parse(amounttax);
            string wht = Math.Round(getwht, 2).ToString();
            string ni = (Math.Round(grossminusmandatorydedduction + decimal.Parse(drr["otherincomenontax"].ToString()) - decimal.Parse(wht) - decimal.Parse(drr["otherdeduction"].ToString()), 5)).ToString();
            ni = ni.ToString().Contains('-') ? "0.00" : ni;
            dttt.Rows[i]["withholdingtax"] = string.Format("{0:n2}", wht);
            dttt.Rows[i]["netincome"] = string.Format("{0:n2}", ni);
            i++;
        }
        ViewState["master"] = dttt;
        grid_item.DataSource = dttt;
        grid_item.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");

    }
    protected void recom(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            DataTable master = ViewState["master"] as DataTable;
            panelPopUpPanel.Style.Add("top", "5%");
            int c = grid_item.Columns.Count - 1;
            int hh = int.Parse(row.Cells[c].Text);
            hdn_row.Value = hh.ToString();
            string name = master.Rows[hh][2].ToString();


            lbl_empppp.Text = name;
            hdn_gross.Value = master.Rows[hh][25].ToString();
            hdn_grossdummy.Value = master.Rows[hh][40].ToString();
            txt_ssssss.Text = master.Rows[hh][26].ToString();
            txt_pppppp.Text = master.Rows[hh][27].ToString();
            txt_hhhhhh.Text = master.Rows[hh][28].ToString();

            hdn_od.Value = master.Rows[hh][30].ToString();
            hdn_nti.Value = master.Rows[hh][39].ToString();


            txt_ssssss_er.Text = master.Rows[hh][33].ToString();
            txt_ssssss_er_ec.Text = master.Rows[hh][34].ToString();
            txt_pppppp_er.Text = master.Rows[hh][35].ToString();
            txt_hhhhhh_er.Text = master.Rows[hh][36].ToString();

            if (decimal.Parse(master.Rows[hh][25].ToString()) > 1000)
            {
                ppop(true);
            }

            //grid_item.DataSource = master;
            //grid_item.DataBind();
        }
    }
    protected void save_recom(object sender, EventArgs e)
    {
        decimal grosspay = 0;
        decimal grosspaydummy = 0;
        decimal Net = 0;
        decimal Wht = 0;

        grosspay = decimal.Parse(hdn_gross.Value) - (decimal.Parse(txt_ssssss.Text) + decimal.Parse(txt_pppppp.Text) + decimal.Parse(txt_hhhhhh.Text));
        grosspaydummy = decimal.Parse(hdn_grossdummy.Value) - (decimal.Parse(txt_ssssss.Text) + decimal.Parse(txt_pppppp.Text) + decimal.Parse(txt_hhhhhh.Text));

        string query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='3' and  Amount<='" + grosspaydummy + "' order by Amount desc";
        DataTable gettax = dbhelper.getdata(query);
        decimal getpercent = 0;
        decimal amounttax = 0;
        decimal amountcolumn = 0;
        if (gettax.Rows.Count > 0)
        {
            getpercent = decimal.Parse(gettax.Rows[0]["percentage"].ToString());
            amounttax = decimal.Parse(gettax.Rows[0]["tax"].ToString());
            amountcolumn = decimal.Parse(gettax.Rows[0]["amount"].ToString());
        }
        else
        {
            getpercent = 0;
            amounttax = 0;
            amountcolumn = 0;
        }
        getpercent = getpercent.ToString().Length == 1 ? decimal.Parse("0.0" + getpercent) : decimal.Parse("0." + getpercent);
        Wht = getpercent * (grosspaydummy - amountcolumn) + amounttax;

        Net = grosspay - (Wht + decimal.Parse(hdn_od.Value));
        Net = Net + decimal.Parse(hdn_nti.Value);



        DataTable master = ViewState["master"] as DataTable;
        int rr = int.Parse(hdn_row.Value);
        master.Rows[rr][26] = txt_ssssss.Text;
        master.Rows[rr][27] = txt_pppppp.Text;
        master.Rows[rr][28] = txt_hhhhhh.Text;

        master.Rows[rr][29] = Wht;
        master.Rows[rr][32] = Net;



        ViewState["master"] = master;
        grid_item.DataSource = master;
        grid_item.DataBind();

        ppop(false);

    }
    protected void close(object sender, EventArgs e)
    {
        ppop(false);
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
 
}