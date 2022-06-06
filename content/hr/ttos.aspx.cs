using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_hr_ttos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
            load();
        }
    }
    protected void load()
    {
        ddl_year_search.Items.Clear();
        for (int i = 2015; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_year_search.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        string query = "select * from memployee where action is null";
        DataTable dt = dbhelper.getdata(query);
        ddl_emp_search.Items.Clear();
        ddl_emp_search.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_emp_search.Items.Add(new ListItem(dr["lastname"].ToString() + ", " + dr["firstname"].ToString() + " " + dr["middlename"].ToString(), dr["id"].ToString()));
        }

        query = "select * from mcompany";
        DataTable dtcompnay = dbhelper.getdata(query);

        ddl_company.Items.Clear();
        foreach (DataRow drc in dtcompnay.Rows)
        {
            ddl_company.Items.Add(new ListItem(drc["company"].ToString(), drc["id"].ToString()));
        }
    }
    protected void click_class(object sender, EventArgs e)
    {
        loadable();
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id,a.empid,a.yr,a.frommmdd,a.tommdd,a.emp_name from alphalist a where a.action is null");
        grid_alpha_trn.DataSource = dt;
        grid_alpha_trn.DataBind();
    }
    protected void loadable()
    {
        //MASTER DATA
        DataTable master = new DataTable();
        master.Columns.Add(new DataColumn("empid", typeof(string)));
        master.Columns.Add(new DataColumn("yr", typeof(string)));
        master.Columns.Add(new DataColumn("frommmdd", typeof(string)));
        master.Columns.Add(new DataColumn("tommdd", typeof(string)));
        master.Columns.Add(new DataColumn("nineA", typeof(string)));
        master.Columns.Add(new DataColumn("taxcode", typeof(string)));
        master.Columns.Add(new DataColumn("ctcno", typeof(string)));
        master.Columns.Add(new DataColumn("ctcamtpaid", typeof(string)));
        master.Columns.Add(new DataColumn("prev_tin", typeof(string)));
        master.Columns.Add(new DataColumn("prev_comp_name", typeof(string)));
        master.Columns.Add(new DataColumn("prev_regaddress", typeof(string)));
        master.Columns.Add(new DataColumn("prev_zipcode", typeof(string)));
        master.Columns.Add(new DataColumn("nt_13month", typeof(string)));
        master.Columns.Add(new DataColumn("nt_denimis", typeof(string)));
        master.Columns.Add(new DataColumn("nt_govt_dues", typeof(string)));
        master.Columns.Add(new DataColumn("nt_salaries_others", typeof(string)));
        master.Columns.Add(new DataColumn("t_basic_salaries", typeof(string)));
        master.Columns.Add(new DataColumn("t_13month", typeof(string)));
        master.Columns.Add(new DataColumn("t_salaries_others", typeof(string)));
        master.Columns.Add(new DataColumn("gci", typeof(string)));
        master.Columns.Add(new DataColumn("nontaxable", typeof(string)));
        master.Columns.Add(new DataColumn("pressent_tci", typeof(string)));
        master.Columns.Add(new DataColumn("previous_tci", typeof(string)));
        master.Columns.Add(new DataColumn("exemption", typeof(string)));
        master.Columns.Add(new DataColumn("insurance_health", typeof(string)));
        master.Columns.Add(new DataColumn("net_tci", typeof(string)));
        master.Columns.Add(new DataColumn("taxdue", typeof(string)));

        master.Columns.Add(new DataColumn("pressent_taxwithheld", typeof(string)));
        master.Columns.Add(new DataColumn("previous_taxwithheld", typeof(string)));

        master.Columns.Add(new DataColumn("withheld_paid_december", typeof(string)));
        master.Columns.Add(new DataColumn("overwithheld", typeof(string)));
        master.Columns.Add(new DataColumn("total_amt_tax_withheld", typeof(string)));



        string query = "select * from memployee where action is null";
        DataTable dt = dbhelper.getdata(query);
        ddl_emplist.Items.Clear();
        if (txt_pressentorprev.SelectedValue == "1")
            ddl_emplist.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_emplist.Items.Add(new ListItem(dr["lastname"].ToString() + ", " + dr["firstname"].ToString() + " " + dr["middlename"].ToString(), dr["id"].ToString()));
        }

        ddl_year.Items.Clear();
        for (int i = 2015; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_year.Items.Add(new ListItem(i.ToString(),i.ToString()));
        }

    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("ttos");
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_status = (LinkButton)e.Row.FindControl("LinkButton1");
            if (lnk_status.Text == "Incomplete")
                lnk_status.BackColor = System.Drawing.Color.Red;
        }
    }
    protected void ttos(bool oi)
    {
        ttospo.Visible = oi;
        ttosppp.Visible = oi;
    }
    protected void processannualization(object sender, EventArgs e)
    {
        ttosppp.Style.Add("width", "1380px");
        ttosppp.Style.Add("left", "250px");
        ttosppp.Style.Add("top", "5%");
        ttos(true);
    }
    protected void processsss(object sender, EventArgs e)
    {
        if (txt_pressentorprev.Text.Length > 0)
        {
            string query = "select *,taxable_allowance+BasicPay+thirteenmonthtaxable taxable_compensation,non_taxable_allowance+govt_union+thirteenmonthnontax nontax_compensation, " +
                            "taxable_allowance+BasicPay + thirteenmonthtaxable + non_taxable_allowance + govt_union + thirteenmonthnontax gross_income, " +
                            "taxable_allowance+BasicPay+thirteenmonthtaxable+non_taxable_allowance+govt_union+thirteenmonthnontax gci,DateResigned,rdo_code,civilstatus,action,dateofbirth,present_comp_company,present_comp_add,present_comp_tin,autorized_agent_for_BIR,companyid,case when taxcodeid is null then '0' else taxcodeid end taxcodeidf,case when taxcode is null then '-' else taxcode end taxcodef, " +
                            "(select case when SUM(aa.tax) is null then '0' else SUM(aa.tax) end from TPayrollLine aa " +
                            "left join tpayroll bb on aa.PayrollId=bb.Id " +
                            "left join tdtr cc on bb.DTRId=cc.Id " +
                            "where " +
                            "bb.status is null and cc.status is null  " +
                            "and YEAR(cc.DateStart)=" + ddl_year.SelectedValue + " and  month(cc.DateStart)>=1 and  month(cc.DateStart)<=11 " +
                            "and aa.EmployeeId=tt.empid) tax_wh_jan_nov  " +
                            "from  " +
                            "( " +
                            "select b.id empid,b.lastname+', '+b.firstname+' '+b.middlename empname,f.position,b.tin,LEFT(CONVERT(varchar,b.datehired,101),10)datehired,LEFT(CONVERT(varchar,b.DateResigned,101),10)DateResigned,b.rdo_code,b.civilstatus,b.action,(select top 1 limitvaluefortaxable from thirteenmonthsetup where action is null order by ID desc)thrtlimit, " +
                            "sum(a.TotalOtherIncomeNonTaxable)non_taxable_allowance,SUM(a.SSSContribution + a.PHICContribution + a.HDMFContribution)govt_union, " +
                            "case when sum(a.NetIncome-a.TotalOtherIncomeNonTaxable-a.TotalOtherIncomeTaxable)/12<(select top 1 limitvaluefortaxable from thirteenmonthsetup where action is null order by ID desc) then sum(a.NetIncome-a.TotalOtherIncomeNonTaxable-a.TotalOtherIncomeTaxable)/12  else '0.00' end thirteenmonthnontax,  " +
                            "case when sum(a.NetIncome-a.TotalOtherIncomeNonTaxable-a.TotalOtherIncomeTaxable)/12>=(select top 1 limitvaluefortaxable from thirteenmonthsetup where action is null order by ID desc) then sum(a.NetIncome-a.TotalOtherIncomeNonTaxable-a.TotalOtherIncomeTaxable)/12  else '0.00' end thirteenmonthtaxable,  " +
                            "sum (a.TotalOtherIncomeTaxable)taxable_allowance,sum((a.NetIncome-a.TotalOtherIncomeNonTaxable)+a.Tax)BasicPay,LEFT(CONVERT(varchar,b.dateofbirth,101),10)dateofbirth,g.company present_comp_company,g.address present_comp_add,g.tin present_comp_tin,g.autorized_agent_for_BIR, " +
                            "sum(a.Tax)tax_due,b.companyid,h.id taxcodeid,h.taxcode " +
                            "from TPayrollLine a  " +
                            "left join memployee b on a.EmployeeId=b.Id  " +
                            "left join mdepartment c on b.departmentid=c.Id " +
                            "left join tpayroll d on a.PayrollId=d.id  " +
                            "left join tdtr e on d.DTRId=e.id " +
                            "left join mposition f on b.positionid=f.id " +
                            "left join mcompany g on b.companyid=g.id " +
                            "left join mtaxcode h on b.taxcodeid=h.id " +
                            "where d.status is null and e.status is null and YEAR(e.DateStart)=" + ddl_year.SelectedValue + " group by b.id,b.lastname+', '+b.firstname+' '+b.middlename,f.position,b.tin,LEFT(CONVERT(varchar,b.datehired,101),10),LEFT(CONVERT(varchar,b.DateResigned,101),10),b.rdo_code,b.civilstatus,b.action,LEFT(CONVERT(varchar,b.dateofbirth,101),10),g.company,g.address,g.tin,g.autorized_agent_for_BIR,b.companyid,h.id,h.taxcode  " +
                            ")tt ";
            if (int.Parse(ddl_emplist.SelectedValue) > 0)
            {
                query += "Where empid=" + ddl_emplist.SelectedValue + "";
            }
            DataTable dt = dbhelper.getdata(query);

            grid_view.DataSource = dt;
            grid_view.DataBind();
        }
        else
            Response.Write("<script>alert('No Data Found!')</script>");
    }
    DateTime LastDayOfYear(string year)
    {
        DateTime date = Convert.ToDateTime("01/01/" + year);
        DateTime newdate = new DateTime(date.Year + 1, 1, 1);
        return newdate.AddDays(-1);
    }
    protected void process_annualization(object sender, EventArgs e)
    {
        DateTime current_time = LastDayOfYear(ddl_year.SelectedValue);
        for (int i = 0; i <= grid_view.Rows.Count-1; i++)
        {
            string ffrom = null;
            string tto = null;
            string yy = null;

            string empid = grid_view.Rows[i].Cells[0].Text;
            string rdocode = grid_view.Rows[i].Cells[1].Text;
            string dateresigned = grid_view.Rows[i].Cells[2].Text;
            string civilstatus = grid_view.Rows[i].Cells[3].Text;
            string action = grid_view.Rows[i].Cells[4].Text;
            string dateofbirth = grid_view.Rows[i].Cells[5].Text.Replace("/","");
            string pres_company_name = grid_view.Rows[i].Cells[6].Text;
            string pres_company_address = grid_view.Rows[i].Cells[7].Text;
            string pres_company_tin = grid_view.Rows[i].Cells[8].Text.Replace("-","");
            string autorized_agent = grid_view.Rows[i].Cells[9].Text;
            ffrom = "0101";
            DateTime datee=new DateTime();
            if (action == "InActive")
            {
                datee = Convert.ToDateTime(dateresigned);
                tto = datee.Month.ToString() + "" + datee.Day.ToString();
                yy = datee.Year.ToString();
            }
            else
            {
                datee = current_time;
                tto = datee.Month.ToString() + "" + datee.Day.ToString();
                yy = datee.Year.ToString();
            }
            string ret = null;


            Label lbl_empname=(Label)grid_view.Rows[i].Cells[10].FindControl("lbl_empname");
            Label lbl_tin = (Label)grid_view.Rows[i].Cells[10].FindControl("lbl_tin");
            //previous
            TextBox lbl_prevemp = (TextBox)grid_view.Rows[i].Cells[10].FindControl("lbl_prevemp");
            TextBox lbl_preveemptin = (TextBox)grid_view.Rows[i].Cells[10].FindControl("lbl_preveemptin");
            TextBox txt_prev_address = (TextBox)grid_view.Rows[i].Cells[10].FindControl("txt_prev_address");
          

            //non tax
            Label lbl_nt_thirteenmonth = (Label)grid_view.Rows[i].Cells[11].FindControl("lbl_nt_thirteenmonth");
            Label lbl_nt_govtdeduction = (Label)grid_view.Rows[i].Cells[11].FindControl("lbl_nt_govtdeduction");
            Label lbl_nt_salariesandforms = (Label)grid_view.Rows[i].Cells[11].FindControl("lbl_nt_salariesandforms");
            Label lbl_nt_compensition = (Label)grid_view.Rows[i].Cells[11].FindControl("lbl_nt_compensition");

            //taxable
            Label lbl_t_bs = (Label)grid_view.Rows[i].Cells[12].FindControl("lbl_t_bs");
            Label lbl_t_thirteenmonth = (Label)grid_view.Rows[i].Cells[12].FindControl("lbl_t_thirteenmonth");
            Label lbl_t_salariesandforms = (Label)grid_view.Rows[i].Cells[12].FindControl("lbl_t_salariesandforms");
            Label lbl_t_compensition = (Label)grid_view.Rows[i].Cells[12].FindControl("lbl_t_compensition");

            //summary
            Label lbl_summary_gci = (Label)grid_view.Rows[i].Cells[13].FindControl("lbl_summary_gci");
            TextBox lbl_prev_emptci = (TextBox)grid_view.Rows[i].Cells[13].FindControl("lbl_prev_emptci");
            TextBox lbl_premiumhealth = (TextBox)grid_view.Rows[i].Cells[13].FindControl("lbl_premiumhealth");
            Label lbl_present_taxwithheld = (Label)grid_view.Rows[i].Cells[13].FindControl("lbl_present_taxwithheld");
            TextBox lbl_prev_taxwithheld = (TextBox)grid_view.Rows[i].Cells[13].FindControl("lbl_prev_taxwithheld");

            string prev_emptci = lbl_prev_emptci.Text.Length > 0 ? lbl_prev_emptci.Text.Replace(",", "") : "0";
            string premiumhealth = lbl_premiumhealth.Text.Length > 0 ? lbl_premiumhealth.Text.Replace(",", "") : "0";
            string present_taxwithheld = lbl_present_taxwithheld.Text.Length > 0 ? lbl_present_taxwithheld.Text.Replace(",", "") : "0";
            string prev_taxwithheld = lbl_prev_taxwithheld.Text.Length > 0 ? lbl_prev_taxwithheld.Text.Replace(",", "") : "0";

            decimal gci = decimal.Parse(lbl_nt_compensition.Text) + decimal.Parse(lbl_t_compensition.Text);
            decimal total_exemption = 0;
            decimal gross_tci = decimal.Parse(lbl_t_compensition.Text) + decimal.Parse(prev_emptci);
            decimal net_tci = gross_tci - decimal.Parse(premiumhealth) - total_exemption;

            DataTable gettax = new DataTable();

            string query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='5' and  Amount<='" + net_tci + "' order by Amount desc";
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
            decimal getwht = getpercent * (net_tci - amountcolumn) + amounttax;



            decimal over_tax_withheld = (decimal.Parse(lbl_present_taxwithheld.Text) + decimal.Parse(prev_taxwithheld)) - getwht;
            decimal amt_taxwithheld_as_adjusted = decimal.Parse(present_taxwithheld) + decimal.Parse(prev_taxwithheld);




            int n = 3;
            string original_emp_tin = lbl_tin.Text.Replace("-","");
            string splitstring_emp_tin = string.Join(string.Empty, original_emp_tin.Select((x, ii) => ii > 0 && ii % n == 0 ? string.Format(" {0}", x) : x.ToString()));

            string original_comp_tin = pres_company_tin;
            string splitstring_comp_tin = string.Join(string.Empty, original_comp_tin.Select((x, ii) => ii > 0 && ii % n == 0 ? string.Format(" {0}", x) : x.ToString()));

            string original_prev_tin = lbl_preveemptin.Text.Replace("-","");
            string splitstring_prev_tin = string.Join(string.Empty, original_prev_tin.Select((x, ii) => ii > 0 && ii % n == 0 ? string.Format(" {0}", x) : x.ToString()));

            string agent = grid_view.Rows[i].Cells[9].Text;

            string tc = grid_view.Rows[i].Cells[15].Text.Length == 0 ? "0" : grid_view.Rows[i].Cells[15].Text;
            string tc_desc = grid_view.Rows[i].Cells[16].Text.Length == 0 ? "0" : grid_view.Rows[i].Cells[16].Text;

            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("alphalist_trn", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = empid;
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@yr", SqlDbType.VarChar, 50).Value = yy;
                    cmd.Parameters.Add("@frommmdd", SqlDbType.VarChar, 50).Value = ffrom;
                    cmd.Parameters.Add("@tommdd", SqlDbType.VarChar, 50).Value = tto;
                    cmd.Parameters.Add("@nineA", SqlDbType.VarChar, 50).Value = "";
                    cmd.Parameters.Add("@rdocode", SqlDbType.VarChar, 50).Value = rdocode;
                    cmd.Parameters.Add("@ctcno", SqlDbType.VarChar, 50).Value = "";
                    cmd.Parameters.Add("@ctc_amtpaid", SqlDbType.VarChar, 50).Value = "0.00";
                    cmd.Parameters.Add("@company_address", SqlDbType.VarChar, 500000).Value = pres_company_address;
                    cmd.Parameters.Add("@company_name", SqlDbType.VarChar, 500000).Value = pres_company_name;
                    cmd.Parameters.Add("@compnay_TIN", SqlDbType.VarChar, 500000).Value = splitstring_comp_tin;
                    cmd.Parameters.Add("@emp_name", SqlDbType.VarChar, 500000).Value = lbl_empname.Text;
                    cmd.Parameters.Add("@emp_TIN", SqlDbType.VarChar, 500000).Value = splitstring_emp_tin;
                    cmd.Parameters.Add("@civil_status", SqlDbType.VarChar, 500000).Value = civilstatus;
                    cmd.Parameters.Add("@dateofbirth", SqlDbType.VarChar, 50).Value = dateofbirth;
                    cmd.Parameters.Add("@autorized_agent", SqlDbType.VarChar, 500000).Value = agent;
                    cmd.Parameters.Add("@class", SqlDbType.Int).Value = txt_pressentorprev.SelectedValue;
                    cmd.Parameters.Add("@companyid", SqlDbType.Int).Value = grid_view.Rows[i].Cells[14].Text;

                    //prev
                    cmd.Parameters.Add("@prevemp", SqlDbType.VarChar, 500000).Value = lbl_prevemp.Text;
                    cmd.Parameters.Add("@preveemptin", SqlDbType.VarChar, 50).Value = splitstring_prev_tin;
                    cmd.Parameters.Add("@prev_emptci", SqlDbType.VarChar, 50).Value = lbl_prev_emptci.Text;
                    cmd.Parameters.Add("@premiumhealth", SqlDbType.VarChar, 50).Value = lbl_premiumhealth.Text;
                    cmd.Parameters.Add("@prev_taxwithheld", SqlDbType.VarChar, 50).Value = lbl_prev_taxwithheld.Text;
                    cmd.Parameters.Add("@prev_address", SqlDbType.VarChar, 500000).Value = txt_prev_address.Text;

                    //non taxable
                    cmd.Parameters.Add("@thirteenmonthpay", SqlDbType.VarChar, 500000).Value = lbl_nt_thirteenmonth.Text;
                    cmd.Parameters.Add("@denimis", SqlDbType.VarChar, 50).Value = "0.00";
                    cmd.Parameters.Add("@govtdeductionanduniondues", SqlDbType.VarChar, 50).Value = lbl_nt_govtdeduction.Text;
                    cmd.Parameters.Add("@salariesandotherformofcompensation", SqlDbType.VarChar, 50).Value = lbl_nt_salariesandforms.Text;
                    cmd.Parameters.Add("@total_nt", SqlDbType.VarChar, 50).Value = lbl_nt_compensition.Text;

                    //taxable
                    cmd.Parameters.Add("@t_bs", SqlDbType.VarChar, 50).Value = lbl_t_bs.Text;
                    cmd.Parameters.Add("@t_thirteenmonth", SqlDbType.VarChar, 50).Value = lbl_t_thirteenmonth.Text;
                    cmd.Parameters.Add("@t_salariesandforms", SqlDbType.VarChar, 50).Value = lbl_t_salariesandforms.Text;
                    cmd.Parameters.Add("@t_compensition", SqlDbType.VarChar, 50).Value = lbl_t_compensition.Text;

                    //summary
                    cmd.Parameters.Add("@present_gci", SqlDbType.VarChar, 50).Value = gci;
                    cmd.Parameters.Add("@total_prev_tci", SqlDbType.VarChar, 50).Value = lbl_prev_emptci.Text.Length == 0 ? "0.00" : lbl_prev_emptci.Text.Replace(",", "");
                    cmd.Parameters.Add("@gross_tci", SqlDbType.VarChar, 50).Value = gross_tci;
                    cmd.Parameters.Add("@less_total_exemption", SqlDbType.VarChar, 50).Value = "0.00";
                    cmd.Parameters.Add("@less_premiumpaidonhealth", SqlDbType.VarChar, 50).Value = premiumhealth;
                    cmd.Parameters.Add("@net_tci", SqlDbType.VarChar, 50).Value = net_tci;
                    cmd.Parameters.Add("@taxdue", SqlDbType.VarChar, 50).Value = getwht;
                    cmd.Parameters.Add("@present_tax_withheld", SqlDbType.VarChar, 50).Value = lbl_present_taxwithheld.Text.Replace(",", "");
                    cmd.Parameters.Add("@prev_tax_withheld", SqlDbType.VarChar, 50).Value = prev_taxwithheld;
                    cmd.Parameters.Add("@over_tax_withheld", SqlDbType.VarChar, 50).Value = over_tax_withheld;
                    cmd.Parameters.Add("@amt_taxwithheld_as_adjusted", SqlDbType.VarChar, 50).Value = amt_taxwithheld_as_adjusted;

                    cmd.Parameters.Add("@tax_code", SqlDbType.Int).Value = grid_view.Rows[i].Cells[15].Text.Length == 0 ? "0" : grid_view.Rows[i].Cells[15].Text;
                    cmd.Parameters.Add("@tax_code_desc", SqlDbType.VarChar, 50000).Value = grid_view.Rows[i].Cells[16].Text.Length == 0 ? "0" : grid_view.Rows[i].Cells[16].Text;
                    cmd.Parameters.Add("@present_tax_withheld_jan_nov", SqlDbType.VarChar, 50).Value = grid_view.Rows[i].Cells[17].Text.Replace(",","");

                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }
            lbl_error_msg.Text = ret;

        }
    }
   
    protected void datarowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           // Label lbl_summary_tci = (Label)e.Row.FindControl("lbl_summary_tci");
           // Label lbl_taxdue = (Label)e.Row.FindControl("lbl_taxdue");
           // TextBox lbl_prev_emptci = (TextBox)e.Row.FindControl("lbl_prev_emptci");
           // lbl_prev_emptci.Text=lbl_prev_emptci.Text.Length>0?lbl_prev_emptci.Text:"0";
           // decimal total_taxable_compensation = decimal.Parse(lbl_summary_tci.Text) + decimal.Parse(lbl_prev_emptci.Text);
           // string query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='5' and  Amount<='" + total_taxable_compensation + "' order by Amount desc";
           //DataTable gettax = dbhelper.getdata(query);
           // decimal getpercent = 0;
           // decimal amounttax = 0;
           // decimal amountcolumn = 0;
           // if (gettax.Rows.Count > 0)
           // {
           //     getpercent = decimal.Parse(gettax.Rows[0]["percentage"].ToString());
           //     amounttax = decimal.Parse(gettax.Rows[0]["tax"].ToString());
           //     amountcolumn = decimal.Parse(gettax.Rows[0]["amount"].ToString());
           // }
           // else
           // {
           //     getpercent = 0;
           //     amounttax = 0;
           //     amountcolumn = 0;
           // }
           // getpercent = getpercent.ToString().Length == 1 ? decimal.Parse("0.0" + getpercent) : decimal.Parse("0." + getpercent);
           // decimal getwht = getpercent * (total_taxable_compensation - amountcolumn) + amounttax;

           // lbl_taxdue.Text = getwht.ToString();
        }
    }
    protected void PDF(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?twothree?key="+function.Encrypt(row.Cells[0].Text,true)+"'", true);
        }
    }
    protected void process(object sender, EventArgs e)
    {
       // Response.Redirect("sevenone?kyy=" + function.Encrypt(ddl_year_search.SelectedValue, true) + "&kclass=" + function.Encrypt(ddl_class.SelectedValue, true) + "&kc=" + function.Encrypt(ddl_company.SelectedValue, true) + "'",true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='sevenone?kyy=" + function.Encrypt(ddl_year_search.SelectedValue, true) + "&kclass=" + function.Encrypt(ddl_class.SelectedValue, true) + "&kc=" + function.Encrypt(ddl_company.SelectedValue, true) + "'", true);

    }
    

}