using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class content_payroll_2316 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        DataTable dtemp = getdata.emp(Request.QueryString["empid"].ToString());
        DataTable dtthirteenmonthpay = getdata.thirteenmonthpay(Request.QueryString["empid"].ToString(), Request.QueryString["y"].ToString());
        if (dtemp.Rows.Count > 0)
        {
            DataTable dtcompany = getdata.company(dtemp.Rows[0]["CompanyId"].ToString());
            txt_fortheyear.Text = Request.QueryString["y"].ToString();
            txt_emptin.Text = dtemp.Rows[0]["TIN"].ToString();
            txt_empname.Text = dtemp.Rows[0]["lastname"].ToString() + ", " + dtemp.Rows[0]["firstname"].ToString() + " " + dtemp.Rows[0]["middlename"].ToString();
            if (dtemp.Rows[0]["CivilStatus"].ToString() == "Single")
            {
                txt_es_single.Text = "x";
                txt_es_married.Text = "";
            }
            else
            {
                txt_es_married.Text = "x";
                txt_es_single.Text = "";
            }
            txt_employername.Text = dtcompany.Rows.Count > 0 ? dtcompany.Rows[0]["Company"].ToString() : "";
            txt_tin_employer_present.Text = dtcompany.Rows.Count > 0 ? dtcompany.Rows[0]["tin"].ToString() : "";
            txt_empregadd.Text = dtcompany.Rows.Count > 0 ? dtcompany.Rows[0]["address"].ToString() : "";

            decimal other_salaries = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["otherform"].ToString()) : decimal.Parse("0.00");
            decimal nd = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["nightdiff"].ToString()) : decimal.Parse("0.00");
            decimal hp = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["holidaypay"].ToString()) : decimal.Parse("0.00");
            decimal overtime = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["overtime"].ToString()) : decimal.Parse("0.00");
            decimal bp = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["basicpay"].ToString()) : decimal.Parse("0.00");
            decimal mp = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["monthpay"].ToString()) : decimal.Parse("0.00");
            decimal gov_deduct = dtthirteenmonthpay.Rows.Count > 0 ? decimal.Parse(dtthirteenmonthpay.Rows[0]["employee_contribution"].ToString()) : decimal.Parse("0.00");
            decimal hazardpay=0;
            decimal dmb=0;
            
            //non tax
            txt_basic_salary_mwe.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(bp, 2).ToString())) == 0 ? Math.Round(bp, 2).ToString() : "0.00";
            txt_holiday_pay_mwe.Text = Math.Round(hp, 2).ToString();
            txt_overtime_pay_mwe.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(overtime, 2).ToString())) == 0 ? Math.Round(overtime, 2).ToString() : "0.00";
            txt_nightshiftdif_mwe.Text = Math.Round(nd, 2).ToString();
            txt_hazard_pay_mwe.Text = hazardpay.ToString();
            txt_thirtenmonth_pay_mwe.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(mp, 2).ToString())) == 0 ? Math.Round(mp, 2).ToString() : "0.00";
            txt_dmb.Text = dmb.ToString();
            txt_goverment_deduction.Text = Math.Round(gov_deduct, 2).ToString();
            txt_sofoc.Text = Math.Round(other_salaries, 2).ToString();
            decimal nontax=decimal.Parse(txt_thirtenmonth_pay_mwe.Text)+decimal.Parse(txt_basic_salary_mwe.Text)+decimal.Parse(txt_overtime_pay_mwe.Text)+hp+nd+hazardpay+dmb+gov_deduct+other_salaries;
            txt_non_tax.Text = Math.Round(nontax, 2).ToString();


            decimal trans = 0;
            decimal rep = 0;
            decimal cola = 0;
            decimal fha = 0;
            decimal reg_other_a_value = 0;
            decimal reg_other_b_value = 0;
            decimal comm = 0;
            decimal ps = 0;
            decimal fidf = 0;
            decimal supp_other_a_value = 0;
            decimal supp_other_b_value = 0;
            decimal total_taxable_income = 0;
            
            //taxable 
            txt_basic_pay_taxable.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(bp, 2).ToString())) > 0 ? Math.Round(bp, 2).ToString() : "0.00";
            txt_repasentation_taxable.Text = rep.ToString();
            txt_transportation_taxable.Text = trans.ToString();
            txt_cola.Text = cola.ToString();
            txt_fha.Text = fha.ToString();
            txt_value_others_a_regular.Text = reg_other_a_value.ToString();
            txt_value_others_b_regular.Text = reg_other_b_value.ToString();
            txt_commission.Text = comm.ToString();
            txt_pf.Text = ps.ToString();
            txt_fidf.Text = fidf.ToString();
            txt_taxable_13monthpay_others.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(mp, 2).ToString())) > 0 ? Math.Round(mp, 2).ToString() : "0.00";
            txt_taxable_hazard_pay.Text=hazardpay.ToString();
            txt_taxable_overtime_pay.Text = decimal.Parse(getdata.detectiftaxableornot(Math.Round(overtime, 2).ToString())) > 0 ? Math.Round(overtime, 2).ToString() : "0.00";
            txt_value_others_a_supplemen.Text = supp_other_a_value.ToString();
            txt_value_others_b_supplemen.Text=supp_other_b_value.ToString();

            total_taxable_income = decimal.Parse(txt_basic_pay_taxable.Text) + decimal.Parse(txt_repasentation_taxable.Text) + decimal.Parse(txt_transportation_taxable.Text) 
            + decimal.Parse(txt_cola.Text)+decimal.Parse(txt_fha.Text)+decimal.Parse(txt_value_others_a_regular.Text)+decimal.Parse(txt_value_others_b_regular.Text)
            +decimal.Parse(txt_commission.Text)+decimal.Parse(txt_pf.Text)+decimal.Parse(txt_fidf.Text)+decimal.Parse(txt_taxable_13monthpay_others.Text)
            +decimal.Parse(txt_taxable_hazard_pay.Text)+decimal.Parse(txt_taxable_overtime_pay.Text)+decimal.Parse(txt_value_others_a_supplemen.Text)
            +decimal.Parse(txt_value_others_b_supplemen.Text);
            txt_total_taxable_compensation_income.Text = Math.Round(total_taxable_income, 2).ToString();
            //final
            decimal gci_employer_present=decimal.Parse(txt_total_taxable_compensation_income.Text) + decimal.Parse(txt_non_tax.Text);
            decimal ltnt=decimal.Parse(txt_non_tax.Text);
            decimal taxable_compensation_inc = gci_employer_present - ltnt;
            decimal gross_taxable_income_previous_employer = 0;
            decimal gici = taxable_compensation_inc + gross_taxable_income_previous_employer;
            decimal totalexemption = 0;
            decimal premiuminhealth = 0;
            decimal taxdue_prev_memployer=0;
        
            txt_gci_present_employer.Text = gci_employer_present.ToString();
            txt_ltnt.Text = ltnt.ToString();
            txt_tci_present_employer.Text = taxable_compensation_inc.ToString();

            txt_tci_previous_employer.Text = gross_taxable_income_previous_employer.ToString();
            txt_gici.Text = gici.ToString();
            txt_total_exemptions.Text = totalexemption.ToString();
            txt_ppoh.Text = premiuminhealth.ToString();
            txt_ntci.Text = (gici - totalexemption - premiuminhealth).ToString();
            decimal taxdue = decimal.Parse(getdata.detectiftaxableornot(txt_ntci.Text));
            txt_taxdue.Text = taxdue.ToString();
            txt_aotw_present_employer.Text = taxdue.ToString();
            txt_aotw_previous_employer.Text = taxdue_prev_memployer.ToString();
            txt_taotwau.Text=(taxdue+taxdue_prev_memployer).ToString();

        }
    }
    protected void click_process_gettaxdue(object sender, EventArgs e)
    {
        hdn_btn_com_tax.Value = "computed";
        decimal taxdue = decimal.Parse(getdata.detectiftaxableornot(txt_ntci.Text));
        txt_taxdue.Text =Math.Round(taxdue,2).ToString();
        txt_aotw_present_employer.Text = Math.Round(taxdue, 2).ToString();
        txt_taotwau.Text = Math.Round(taxdue, 2).ToString();
    }
    protected void process_itr(object sender, EventArgs e)
    {
        if (hdn_btn_com_tax.Value == "computed")
        {
            if (txt_peaasopn.Text.Length > 0)
            {
                DataTable chekitr = getdata.dtitr(DateTime.Now.Year.ToString(),Request.QueryString["empid"].ToString());
                if (chekitr.Rows.Count > 0)
                {
                    string act = "cancel-" + function.Decrypt(Request.QueryString["user_id"].ToString(), true) + "-" + DateTime.Now.ToShortDateString();
                    dbhelper.getdata("update itr set action='" + act + "' where itr_id="+chekitr.Rows[0]["itr_id"].ToString()+"");
                }
                string ret = "0";
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {

                    using (SqlCommand cmd = new SqlCommand("process_itr", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["empid"].ToString();
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true).ToString();
                        cmd.Parameters.Add("@yyyy", SqlDbType.VarChar, 5000).Value = txt_fortheyear.Text;
                        cmd.Parameters.Add("@period_from", SqlDbType.VarChar, 5000).Value = txt_period_from_mmdd.Text;
                        cmd.Parameters.Add("@period_to", SqlDbType.VarChar, 5000).Value = txt_period_to_mmdd.Text;
                        cmd.Parameters.Add("@presentemployerprintednameorauthorized", SqlDbType.VarChar, 5000).Value = txt_peaasopn.Text;
                        cmd.Parameters.Add("@employeeprintedname", SqlDbType.VarChar, 5000).Value = txt_empname.Text;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("process_itr_employee_info", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@taxpayer_identification_no", SqlDbType.VarChar, 5000).Value = txt_emptin.Text;
                        cmd.Parameters.Add("@employee_name", SqlDbType.VarChar, 5000).Value = txt_empname.Text;
                        cmd.Parameters.Add("@rdo_code", SqlDbType.VarChar, 5000).Value = txt_rdocode.Text;
                        cmd.Parameters.Add("@registered_add", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@registered_zipcode", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@localhome_add", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@localhome_zipcode", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@foriegn_add", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@foriegn_zipcode", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@dateofbirth", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@telephone_no", SqlDbType.VarChar, 5000).Value = "";
                        string xx = "";
                        if (txt_es_married.Text == "x" || txt_es_single.Text == "x")
                            xx = "x";
                        else
                            xx = "";
                        cmd.Parameters.Add("@exemption_status", SqlDbType.VarChar, 5000).Value = xx;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //  ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("process_itr_nontaxorexemtcompensationincome", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@basicsalary", SqlDbType.VarChar, 5000).Value = txt_basic_salary_mwe.Text;
                        cmd.Parameters.Add("@holidaypay", SqlDbType.VarChar, 5000).Value = txt_holiday_pay_mwe.Text;
                        cmd.Parameters.Add("@overtimepay", SqlDbType.VarChar, 5000).Value = txt_overtime_pay_mwe.Text;
                        cmd.Parameters.Add("@nightdiff", SqlDbType.VarChar, 5000).Value = txt_nightshiftdif_mwe.Text;
                        cmd.Parameters.Add("@hazardpay", SqlDbType.VarChar, 5000).Value = txt_hazard_pay_mwe.Text;
                        cmd.Parameters.Add("@thirteenmonthpay", SqlDbType.VarChar, 5000).Value = txt_thirtenmonth_pay_mwe.Text;
                        cmd.Parameters.Add("@deminisbenefits", SqlDbType.VarChar, 5000).Value = txt_dmb.Text;
                        cmd.Parameters.Add("@sssgsisphicpagibigcontribution", SqlDbType.VarChar, 5000).Value = txt_goverment_deduction.Text;
                        cmd.Parameters.Add("@salariesandotherformofcompensation", SqlDbType.VarChar, 5000).Value = txt_sofoc.Text;
                        cmd.Parameters.Add("@totalnontaxableorexemptcompensationincom", SqlDbType.VarChar, 5000).Value = txt_non_tax.Text;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        // ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("process_itr_taxablecompensationincome", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@regularbasicsalary", SqlDbType.VarChar, 5000).Value = txt_basic_pay_taxable.Text;
                        cmd.Parameters.Add("@regularrepresentation", SqlDbType.VarChar, 5000).Value = txt_repasentation_taxable.Text;
                        cmd.Parameters.Add("@regulartransportation", SqlDbType.VarChar, 5000).Value = txt_transportation_taxable.Text;
                        cmd.Parameters.Add("@costofliving", SqlDbType.VarChar, 5000).Value = txt_cola.Text;
                        cmd.Parameters.Add("@regularfixedhousingallowance", SqlDbType.VarChar, 5000).Value = txt_fha.Text;
                        cmd.Parameters.Add("@regularothers_label_a", SqlDbType.VarChar, 5000).Value = txt_label_others_a_regular.Text;
                        cmd.Parameters.Add("@regularother_label_b", SqlDbType.VarChar, 5000).Value = txt_label_others_b_regular.Text;
                        cmd.Parameters.Add("@regularother_value_a", SqlDbType.VarChar, 5000).Value = txt_value_others_a_regular.Text;
                        cmd.Parameters.Add("@regularother_value_b", SqlDbType.VarChar, 5000).Value = txt_value_others_b_regular.Text;
                        cmd.Parameters.Add("@supplementarycommission", SqlDbType.VarChar, 5000).Value = txt_commission.Text;
                        cmd.Parameters.Add("@supplementaryprofitsharing", SqlDbType.VarChar, 5000).Value = txt_pf.Text;
                        cmd.Parameters.Add("@supplementaryfeesincludingderictorsfee", SqlDbType.VarChar, 5000).Value = txt_fidf.Text;
                        cmd.Parameters.Add("@supplementarytaxablethirteenmonthpayandothersbenefits", SqlDbType.VarChar, 5000).Value = txt_taxable_13monthpay_others.Text;
                        cmd.Parameters.Add("@supplementaryhazardpay", SqlDbType.VarChar, 5000).Value = txt_taxable_hazard_pay.Text;
                        cmd.Parameters.Add("@supplementaryovertimepay", SqlDbType.VarChar, 5000).Value = txt_taxable_overtime_pay.Text;
                        cmd.Parameters.Add("@supplementaryothers_label_a", SqlDbType.VarChar, 5000).Value = txt_label_others_a_supplemen.Text;
                        cmd.Parameters.Add("@supplementaryother_label_b", SqlDbType.VarChar, 5000).Value = txt_label_others_b_supplemen.Text;
                        cmd.Parameters.Add("@supplementaryother_value_a", SqlDbType.VarChar, 5000).Value = txt_value_others_a_supplemen.Text;
                        cmd.Parameters.Add("@supplementaryother_value_b", SqlDbType.VarChar, 5000).Value = txt_value_others_b_supplemen.Text;
                        cmd.Parameters.Add("@totaltaxablecompensationincome", SqlDbType.VarChar, 5000).Value = txt_total_taxable_compensation_income.Text;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        // ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("process_itr_present_employeer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@taxpayer_identification_no", SqlDbType.VarChar, 5000).Value = txt_tin_employer_present.Text;
                        cmd.Parameters.Add("@employername", SqlDbType.VarChar, 5000).Value = txt_employername.Text;
                        cmd.Parameters.Add("@registered_add", SqlDbType.VarChar, 5000).Value = txt_empregadd.Text;
                        cmd.Parameters.Add("@registered_zipcode", SqlDbType.VarChar, 5000).Value = txt_empregaddzipcode.Text;
                        cmd.Parameters.Add("@mainemployer", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@secondaryemployer", SqlDbType.VarChar, 5000).Value = "";
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        // ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("process_itr_previous_employeer", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@taxpayer_identification_no", SqlDbType.VarChar, 5000).Value = txt_tin_employer_previous.Text;
                        cmd.Parameters.Add("@employername", SqlDbType.VarChar, 5000).Value = txt_employer_previous.Text;
                        cmd.Parameters.Add("@registered_add", SqlDbType.VarChar, 5000).Value = txt_regadd_employer_previous.Text;
                        cmd.Parameters.Add("@registered_zipcode", SqlDbType.VarChar, 5000).Value = txt_regzipcode_employer_previous.Text;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        // ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("process_itr_summary", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@itr_id", SqlDbType.Int).Value = ret;
                        cmd.Parameters.Add("@grosscompensatioincomepresentemployer", SqlDbType.VarChar, 5000).Value = txt_gci_present_employer.Text;
                        cmd.Parameters.Add("@lesstotalnontaxble", SqlDbType.VarChar, 5000).Value = txt_ltnt.Text;
                        cmd.Parameters.Add("@taxablecompensationincomepresentemployer", SqlDbType.VarChar, 5000).Value = txt_tci_present_employer.Text;
                        cmd.Parameters.Add("@addtaxablecompensationincomepreviousemployer", SqlDbType.VarChar, 5000).Value = txt_tci_previous_employer.Text;
                        cmd.Parameters.Add("@grosstaxablecompensationincome", SqlDbType.VarChar, 5000).Value = txt_gici.Text;
                        cmd.Parameters.Add("@lesstotalexemption", SqlDbType.VarChar, 5000).Value = txt_total_exemptions.Text;
                        cmd.Parameters.Add("@lesspremiumpaidonhealth", SqlDbType.VarChar, 5000).Value = txt_ppoh.Text;
                        cmd.Parameters.Add("@nettaxablecompensationincome", SqlDbType.VarChar, 5000).Value = txt_ntci.Text;
                        cmd.Parameters.Add("@taxdue", SqlDbType.VarChar, 5000).Value = txt_taxdue.Text;
                        cmd.Parameters.Add("@amounttaxwithheldpresentemployer", SqlDbType.VarChar, 5000).Value = txt_aotw_present_employer.Text;
                        cmd.Parameters.Add("@amounttaxwithheldpreviousemployer", SqlDbType.VarChar, 5000).Value = txt_aotw_previous_employer.Text;
                        cmd.Parameters.Add("@totalamounttaxwithheldasadjusted", SqlDbType.VarChar, 5000).Value = txt_taotwau.Text;
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                }
                Response.Redirect("quitclaim?user_id="+function.Encrypt(Session["user_id"].ToString(),true)+"");
            }
            else
                Response.Write("<script>alert('Please specify Employer Name/Authorized Agent!')</script>");
         }
        else
            Response.Write("<script>alert('Please click compute button to get the tax due!')</script>");
    }

    
}