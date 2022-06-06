using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_hr_mandatorytable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            transition();
            sss();

        }
    }

    protected void transition()
    {
        grid_sss.Visible = false;
        p_sss.Visible = false;
        grid_hdmf.Visible = false;
        p_hdmf.Visible = false;
        grid_phic.Visible = false;
        p_phic.Visible = false;
        grid_semi_monthly_wht.Visible = false;
        p_semi_monthly_wht.Visible = false;
        grid_monthly_wht.Visible = false;
        p_monthly_wht.Visible = false;
        grid_yearly_wht.Visible = false;
        p_yearly_wht.Visible = false;
        grid_taxcode.Visible = false;
        p_taxcode.Visible = false;
        gridtaxtable.Visible = false;
        //grid_train.Visible = false;
        p_tax_train.Visible = false;
      
      
    }

    protected void change_choice(object sender, EventArgs e)
    {
        transition();
        DataTable dt;
        switch (dl_choice.Text)
        {
            case "SSS":
                sss();
                break;
            case "HDMF":
                dt = dbhelper.getdata("select * from MstTableHDMF");
                grid_hdmf.DataSource = dt;
                grid_hdmf.DataBind();
                grid_hdmf.Visible = true;
                p_hdmf.Visible = true;
                break;
            case "PHIC":
                dt = dbhelper.getdata("select * from MstTablePHIC");
                grid_phic.DataSource = dt;
                grid_phic.DataBind();
                grid_phic.Visible = true;
                p_phic.Visible = true;
                break;
            case "Train Law":
                dt = dbhelper.getdata("select * from taxtable order by id asc");
                gridtaxtable.DataSource = dt;
                gridtaxtable.DataBind();
                gridtaxtable.Visible = true;
                p_tax_train.Visible = true;
               




                break;
            case "Semi-Monthly Withholding Tax":
                dt = dbhelper.getdata("select (select taxcode from MTaxCode where ID=a.taxcodeid )taxcode,* from MstTableWTaxSemiMonthly a");
                grid_semi_monthly_wht.DataSource = dt;
                grid_semi_monthly_wht.DataBind();
                grid_semi_monthly_wht.Visible = true;
                p_semi_monthly_wht.Visible = true;
                break;
            case "Monthly Withholding Tax":
                dt = dbhelper.getdata("select (select taxcode from MTaxCode where ID=a.taxcodeid )taxcode,* from MstTableWTaxMonthly a");
                grid_monthly_wht.DataSource = dt;
                grid_monthly_wht.DataBind();
                grid_monthly_wht.Visible = true;
                p_monthly_wht.Visible = true;
                break;
            case "Yearly Withholding Tax":
                dt = dbhelper.getdata("select * from MstTableWTaxYearly");
                grid_yearly_wht.DataSource = dt;
                grid_yearly_wht.DataBind();
                grid_yearly_wht.Visible = true;
                p_yearly_wht.Visible = true;
                break;
            case "Tax Code":
                dt = dbhelper.getdata("select * from MTaxCode");
                grid_taxcode.DataSource = dt;
                grid_taxcode.DataBind();
                grid_taxcode.Visible = true;
                p_taxcode.Visible = true;
                break;
        }
    }
    protected void selecttaxtable(object sender, EventArgs e)
    {
        if (ddl_taxtable.SelectedItem.Text == "Yearly")
        {
            ttt_year.Visible = true;
            ttttt.Visible = false;
        }
        else
        {
            ttttt.Visible = true;
            ttt_year.Visible = false;
        }
    }
    protected void sss()
    {
        DataTable dt = dbhelper.getdata("select * from MstTableSSS");
        grid_sss.DataSource = dt;
        grid_sss.DataBind();
        grid_sss.Visible = true;
        p_sss.Visible = true;
    }

    protected void click_add_sss(object sender, EventArgs e)
    {
        if (txt_start.Text.Trim().Length == 0 || txt_end.Text.Trim().Length == 0 || txt_er.Text.Trim().Length == 0 || txt_ee.Text.Trim().Length == 0 || txt_erec.Text.Trim().Length == 0 || txt_eeec.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            DataTable dt = dbhelper.getdata("insert into MstTableSSS values (" + txt_start.Text.Trim().Replace(",", "") + "," + txt_end.Text.Trim().Replace(",", "") + "," + txt_er.Text.Trim().Replace(",", "") + "," + txt_ee.Text.Trim().Replace(",", "") + "," + txt_erec.Text.Trim().Replace(",", "") + "," + txt_eeec.Text.Trim().Replace(",", "") + "," + txt_mpf.Text.Trim().Replace(",", "") + "," + txt_ermpf.Text.Trim().Replace(",", "") + "," + txt_eempf.Text.Trim().Replace(",", "") + ")select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSS";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New SSS Table";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_start.Text.Replace("'", "").ToString() + "-" + txt_end.Text.Trim().Replace(",", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void click_add_hdmf(object sender, EventArgs e)
    {
        if (txt_hdmf_start.Text.Trim().Length == 0 || txt_hdmf_end.Text.Trim().Length == 0 || txt_hdmf_er.Text.Trim().Length == 0 || txt_hdmf_ee.Text.Trim().Length == 0 || txt_hdmf_erv.Text.Trim().Length == 0 || txt_hdmf_eev.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            DataTable dt = dbhelper.getdata("insert into MstTableHDMF values (" + txt_hdmf_start.Text.Trim().Replace(",", "") + "," + txt_hdmf_end.Text.Trim().Replace(",", "") + "," + txt_hdmf_er.Text.Trim().Replace(",", "") + "," + txt_hdmf_ee.Text.Trim().Replace(",", "") + "," + txt_hdmf_erv.Text.Trim().Replace(",", "") + "," + txt_hdmf_eev.Text.Trim().Replace(",", "") + ")select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMF";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New HDMF Table";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_hdmf_start.Text.Replace("'", "").ToString() + "-" + txt_hdmf_end.Text.Trim().Replace(",", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void click_add_phic(object sender, EventArgs e)
    {
        if (txt_phic_start.Text.Trim().Length == 0 || txt_phic_end.Text.Trim().Length == 0 || txt_phic_salarybracket.Text.Trim().Length == 0 || txt_phic_employercontribution.Text.Trim().Length == 0 || txt_phic_employeecontribution.Text.Trim().Length == 0 )
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            DataTable dt = dbhelper.getdata("insert into MstTablePHIC values (" + txt_phic_start.Text.Trim().Replace(",", "") + "," + txt_phic_end.Text.Trim().Replace(",", "") + "," + txt_phic_salarybracket.Text.Trim().Replace(",", "") + "," + txt_phic_employercontribution.Text.Trim().Replace(",", "") + "," + txt_phic_employeecontribution.Text.Trim().Replace(",", "") + ")select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHIC";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New PHIC Table";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_phic_start.Text.Replace("'", "").ToString() + "-" + txt_phic_end.Text.Trim().Replace(",", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void click_add_semiwht(object sender, EventArgs e)
    {
        if (txt_semi_monthly_wht_exemption.Text.Trim().Length == 0 || txt_semi_monthly_wht_amount.Text.Trim().Length == 0 || txt_semi_monthly_wht_tax.Text.Trim().Length == 0 || txt_semi_monthly_wht_excesspercentage.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            dbhelper.getdata("insert into MstTableWTaxSemiMonthly values (" + ddl_semi_monthly_wht_taxcode.SelectedValue + "," + txt_semi_monthly_wht_exemption.Text.Trim().Replace(",", "") + "," + txt_semi_monthly_wht_amount.Text.Trim().Replace(",", "") + "," + txt_semi_monthly_wht_tax.Text.Trim().Replace(",", "") + "," + txt_semi_monthly_wht_excesspercentage.Text.Trim().Replace(",", "") + ")");
        }
    }
    protected void click_add_monthlywht(object sender, EventArgs e)
    {
        if (txt_monthly_wht_exemption.Text.Trim().Length == 0 || txt_monthly_wht_amount.Text.Trim().Length == 0 || txt_monthly_wht_tax.Text.Trim().Length == 0 || txt_monthly_wht_percentage.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            dbhelper.getdata("insert into MstTableWTaxMonthly values (" + ddl_monthly_wht_taxcode.SelectedValue + "," + txt_monthly_wht_exemption.Text.Trim().Replace(",", "") + "," + txt_monthly_wht_amount.Text.Trim().Replace(",", "") + "," + txt_monthly_wht_tax.Text.Trim().Replace(",", "") + "," + txt_monthly_wht_percentage.Text.Trim().Replace(",", "") + ")");
        }
    }
    protected void click_add_yearlywht(object sender, EventArgs e)
    {
        if (txt_yearly_wht_start.Text.Trim().Length == 0 || txt_yearly_wht_end.Text.Trim().Length == 0 || txt_yearly_wht_percentage.Text.Trim().Length == 0 || txt_yearly_wht_addamount.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            dbhelper.getdata("insert into MstTableWTaxYearly values (" + txt_yearly_wht_start.Text.Trim().Replace(",", "") + "," + txt_yearly_wht_end.Text.Trim().Replace(",", "") + "," + txt_yearly_wht_percentage.Text.Trim().Replace(",", "") + "," + txt_yearly_wht_addamount.Text.Trim().Replace(",", "") + ")");
        }
    }
    protected void click_add_taxcode(object sender, EventArgs e)
    {
        if (txt_tax_code_taxcode.Text.Trim().Length == 0 || txt_tax_code_exemption.Text.Trim().Length == 0 || txt_tax_code_dependent.Text.Trim().Length == 0 )
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            DataTable dt = dbhelper.getdata("insert into MTaxCode values ('" + txt_tax_code_taxcode.Text.Trim() + "'," + txt_tax_code_exemption.Text.Trim().Replace(",", "") + "," + txt_tax_code_dependent.Text.Trim().Replace(",", "") + ")select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddTaxCode";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New TaxCode Table";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_tax_code_taxcode.Text.Replace("'", "").ToString() + "-" + txt_tax_code_exemption.Text.Trim().Replace(",", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void click_train_law(object sender,EventArgs e)
    {
      
        if (txt_tt_amt.Text.Trim().Length == 0 || txt_tt_tax.Text.Trim().Length == 0 || txt_tt_excess.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {
            DataTable dt = dbhelper.getdata("insert into tax_train values ('" + ddl_taxtable.SelectedValue + "','" + txt_tt_amt.Text.Trim() + "'," + txt_tt_tax.Text.Trim().Replace(",", "") + "," + txt_tt_excess.Text.Trim().Replace(",", "") + ")select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddTrain";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Train Table";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_tt_amt.Text.Replace("'", "").ToString() + "-" + txt_tt_tax.Text.Trim().Replace(",", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString() + "TaxTable: " + ddl_taxtable.SelectedValue.ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    protected void click_train_law_yearly(object sender, EventArgs e)
    {

        if (txt_amtstart.Text.Trim().Length == 0 || txt_amtend.Text.Trim().Length == 0 || txt_amtadd.Text.Trim().Length == 0 || txt_rate.Text.Trim().Length == 0)
        { Response.Write("<script>alert('Dont leave with empty fields!')</script>"); }
        else
        {

            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("train_law_yearly", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlConnection conn = new SqlConnection(dbconnection.conn);
                    cmd.Parameters.Add("@amountstart", SqlDbType.VarChar, 5000).Value = txt_amtstart.Text;
                    cmd.Parameters.Add("@amountend", SqlDbType.VarChar, 5000).Value = txt_amtend.Text;
                    cmd.Parameters.Add("@amtadd", SqlDbType.VarChar, 5000).Value = txt_amtadd.Text;
                    cmd.Parameters.Add("@rate", SqlDbType.VarChar, 5000).Value = txt_rate.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    string ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }

           // dbhelper.getdata("insert into tax_train values ('" + ddl_taxtable.SelectedItem + "','" + txt_tt_amt.Text.Trim() + "'," + txt_tt_tax.Text.Trim().Replace(",", "") + "," + txt_tt_excess.Text.Trim().Replace(",", "") + ")");
        }
    }
    protected void rowboundtaxtable(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView grid = (GridView)e.Row.FindControl("grid_train");
            GridView yearly = (GridView)e.Row.FindControl("yearly");
            DataTable dt;
            if (e.Row.Cells[0].Text == "6")
            {
                dt = dbhelper.getdata("select * from tax_train_yearly");
                yearly.DataSource = dt;
                yearly.DataBind();
            }
            else
            {
                 dt = dbhelper.getdata("select * from tax_train where taxtable=" + e.Row.Cells[0].Text + "");
                grid.DataSource = dt;
                grid.DataBind();
            }

        }
    }
    protected void showsatrainlaw(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from tax_train where id = '" + seriesid.Value + "'");
            txttraintamount.Text = dt.Rows[0]["amount"].ToString();
            txttrainttax.Text = dt.Rows[0]["tax"].ToString();
            txttraintpercent.Text = dt.Rows[0]["percentage"].ToString();
            pgrid_train.Visible = true;
            pgrid_train2.Visible = true;
        }
    }
    protected void edittrainlaw(object sender, EventArgs e)
    {
        DataTable dts = dbhelper.getdata("select * from tax_train where id = '" + seriesid.Value + "'");
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditTrainA";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dts.Rows[0]["amount"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txttraintamount.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dts.Rows[0]["id"].ToString() + " Table: " + dts.Rows[0]["taxtable"].ToString() + "Amount";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditTrainT";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dts.Rows[0]["tax"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txttrainttax.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dts.Rows[0]["id"].ToString() + " Table: " + dts.Rows[0]["taxtable"].ToString() + "Tax";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditTrainP";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dts.Rows[0]["percentage"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txttraintpercent.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dts.Rows[0]["id"].ToString() + " Table: " + dts.Rows[0]["taxtable"].ToString() + "Percentage";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        DataTable dt = dbhelper.getdata("update tax_train set amount = '" + txttraintamount.Text + "', tax = '" + txttrainttax.Text + "', percentage = '" + txttraintpercent.Text + "' where id = '" + seriesid.Value + "'");
        Response.Redirect("Mmandatorytable");
    }
    protected void showtaxtable(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MTaxCode where Id ='" + seriesid.Value + "'");
            txttaxcode.Text = dt.Rows[0]["TaxCode"].ToString();
            txtexcemt.Text = dt.Rows[0]["ExemptionAmount"].ToString();
            txtdepend.Text = dt.Rows[0]["DependentAmount"].ToString();
            pgrid_taxcode.Visible = true;
            pgrid_taxcode2.Visible = true;
        }
    }
    protected void edittcode(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MTaxCode where Id = '" + seriesid.Value + "'");
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddTaxCodeT";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["TaxCode"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txttaxcode.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "TaxCode";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddTaxCodeE";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["ExemptionAmount"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtexcemt.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Excemption Amount";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddTaxCodeD";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["DependentAmount"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtdepend.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Dependent Amount";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        dbhelper.getdata("update MTaxCode set TaxCode = '" + txttaxcode.Text + "', ExemptionAmount = '" + txtexcemt.Text + "', DependentAmount = '" + txtdepend.Text + "' where Id = '" + seriesid.Value + "'");
        Response.Redirect("Mmandatorytable");
    }
    protected void showphicc(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MstTablePHIC where Id = '" + seriesid.Value + "'");
            txtpstart.Text = dt.Rows[0]["AmountStart"].ToString();
            txtpend.Text = dt.Rows[0]["AmountEnd"].ToString();
            txtpsb.Text = dt.Rows[0]["SalaryBracket"].ToString();
            txtppec.Text = dt.Rows[0]["EmployerContribution"].ToString();
            txtppec2.Text = dt.Rows[0]["EmployeeContribution"].ToString();
            txtppercent.Text = dt.Rows[0]["percentage"].ToString();
            pgrid_phic.Visible = true;
            pgrid_phic2.Visible = true;
        }
    }
    protected void editphiccs(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MstTablePHIC where Id = '" + seriesid.Value + "'");
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHIAS";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountStart"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpstart.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount Start";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHIAE";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountEnd"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpend.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount End";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHISB";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["SalaryBracket"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpsb.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Salary Bracket";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHIERC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployerContribution"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtppec.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employer Contri";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPHIEEC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployeeContribution"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtppec2.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employee Contri";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        dbhelper.getdata("update MstTablePHIC set AmountStart='" + txtpstart.Text + "', AmountEnd='" + txtpend.Text + "', SalaryBracket='" + txtpsb.Text + "',EmployerContribution='" + txtppec.Text + "',EmployeeContribution='" + txtppec2.Text + "',percentage='" + txtppercent.Text + "' where Id='" + seriesid.Value + "'");
        Response.Redirect("Mmandatorytable");
    }
    protected void showhdmf(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MstTableHDMF where Id ='" + seriesid.Value + "'");
            txthdmfstat.Text = dt.Rows[0]["AmountStart"].ToString();
            txthdmfend.Text = dt.Rows[0]["AmountEnd"].ToString();
            txthdmfemyr.Text = dt.Rows[0]["EmployerPercentage"].ToString();
            txthdmfemye.Text = dt.Rows[0]["EmployeePercentage"].ToString();
            txthdmfemyrval.Text = dt.Rows[0]["EmployerValue"].ToString();
            txtdhmfemyeval.Text = dt.Rows[0]["EmployeeValue"].ToString();
            pgrid_hdmf.Visible = true;
            pgrid_hdmf2.Visible = true;
        }
    }
    protected void edithdmff(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MstTableHDMF where Id = '" + seriesid.Value + "'");
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFAS";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountStart"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txthdmfstat.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount Start";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFAE";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountEnd"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txthdmfend.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount End";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFERP";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployerPercentage"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txthdmfemyr.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employer %";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFEEP";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployeePercentage"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txthdmfemye.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employee %";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFERV";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployerValue"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txthdmfemyrval.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employer Val";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddHDMFEEV";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployeeValue"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtdhmfemyeval.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employee Val";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        dbhelper.getdata("update MstTableHDMF set AmountStart = '" + txthdmfstat.Text + "', AmountEnd = '" + txthdmfend.Text + "', EmployerPercentage = '" + txthdmfemyr.Text + "', EmployeePercentage = '" + txthdmfemye.Text + "', EmployerValue = '" + txthdmfemyrval.Text + "', EmployeeValue = '" + txtdhmfemyeval.Text + "' where Id = '" + seriesid.Value + "'");
        Response.Redirect("Mmandatorytable");
    }
    protected void showsss(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MstTableSSS where Id = '" + seriesid.Value + "'");
            txtstart.Text = dt.Rows[0]["AmountStart"].ToString();
            txtend.Text = dt.Rows[0]["AmountEnd"].ToString();
            txter.Text = dt.Rows[0]["EmployerContribution"].ToString();
            txtee.Text = dt.Rows[0]["EmployeeContribution"].ToString();
            txterec.Text = dt.Rows[0]["EmployerEC"].ToString();
            txteeec.Text = dt.Rows[0]["EmployeeEC"].ToString();
            txtmpf.Text = dt.Rows[0]["mpf"].ToString();
            txtermpf.Text = dt.Rows[0]["er_mpf"].ToString();
            txteempf.Text = dt.Rows[0]["ee_mpf"].ToString();
            pgrid_sss.Visible = true;
            pgrid_sss2.Visible = true;
        }
    }
    protected void editssss(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MstTableSSS where Id = '" + seriesid.Value + "'");
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSAS";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountStart"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtstart.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount Start";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSAE";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AmountEnd"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtstart.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Amount End";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSERC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployerContribution"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txter.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employer Contri";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSEEC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployeeContribution"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtee.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employee Contri";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSEREC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployerEC"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txterec.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employer EC";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSSSEEEC";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["EmployeeEC"].ToString();
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txteeec.Text.Replace("'", "").ToString();
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString() + "Employee EC";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        dbhelper.getdata("UPDATE MstTableSSS set AmountStart = '" + txtstart.Text + "',AmountEnd='" + txtend.Text + "',EmployerContribution='" + txter.Text + "',EmployeeContribution='" + txtee.Text + "',EmployerEC='" + txterec.Text + "',EmployeeEC='" + txteeec.Text + "',er_mpf='" + txtermpf.Text + "',ee_mpf='" + txteempf.Text + "',mpf='" + txtmpf.Text + "' where id = '" + seriesid.Value + "'");
        Response.Redirect("Mmandatorytable");
    }
    protected void closepopup(object sender, EventArgs e)
    {
        Response.Redirect("Mmandatorytable");
    }
}