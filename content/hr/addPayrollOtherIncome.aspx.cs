using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Services;
using System.Configuration;
using System.Data.OleDb;

public partial class content_hr_addPayrollOtherIncome : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {
            loadable();
            key.Value = Session["user_id"].ToString();
            
            manaualdatatable();
            disp();
        }
    }
    protected void loadable()
    {
        string query = "select * from MOtherIncome where action is null and frequencyid=3";
        DataTable dt = dbhelper.getdata(query);

        ddl_type.Items.Clear();
        ddl_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_type.Items.Add(new ListItem(dr["OtherIncome"].ToString(), dr["id"].ToString()));
        }

        query = "select sctrnid,LEFT(CONVERT(varchar,dfrom,101),10)+'-'+LEFT(CONVERT(varchar,dtoo,101),10)descc from sc_transaction where action is null and payid=0 ";
        dt = dbhelper.getdata(query);
        ddl_sc_range.Items.Clear();
        ddl_sc_range.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_sc_range.Items.Add(new ListItem(dr["descc"].ToString(), dr["sctrnid"].ToString()));
        }
    }
    protected void click_pgpg(object sender, EventArgs e)
    {
        dtr_range();
    }
    protected void dtr_range()
    {
        string query = "select id,left(convert(varchar,datestart,101),10)datestart,left(convert(varchar,dateend,101),10)dateend from TDTR  where status is null and PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and payroll_id is null ";
        DataTable dtover = dbhelper.getdata(query);

        if (dtover.Rows.Count > 0)
            btn_verify.Enabled = true;
        else
            btn_verify.Enabled = false;
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }
       
    }
    protected void manaualdatatable()
    {
        DataTable additionalamt = new DataTable();
        additionalamt.Columns.Add(new DataColumn("rown", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("empid", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("incomeid", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("empname", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("otherincome", typeof(string)));



        additionalamt.Columns.Add(new DataColumn("type", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("schedule", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("Taxable", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("total_hours_worked", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("amt_to_bepaid", typeof(string)));

        additionalamt.Columns.Add(new DataColumn("amt_to_be_tax", typeof(string)));
        
        additionalamt.Columns.Add(new DataColumn("amount", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("notes", typeof(string)));
        ViewState["additionalamt"] = additionalamt;


        string query = "select * from MPayrollGroup where status=1 order by id asc";
        DataTable dtt = dbhelper.getdata(query);
        ddl_payroll_group.Items.Clear();
        ddl_payroll_group.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();

            //String[] excelSheets = new String[dt.Rows.Count];
            //int i = 0;

            //// Add the sheet name to the string array.
            //foreach (DataRow row in dt.Rows)
            //{
            //    excelSheets[i] = row["TABLE_NAME"].ToString();
            //    i++;
            //}

            //// Loop through all of the sheets if you want too...
            //for (int j = 0; j < excelSheets.Length; j++)
            //{
            //    // Query each excel sheet.
            //}

            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    protected void click_sc(object sender, EventArgs e)
    {
        if (Button1.Text == "Process")
        {
            DataTable dtscdetails = dbhelper.getdata("select a.empid,a.manhr,a.grossamt,a.sctrnid,b.LastName+', '+b.FirstName+' '+b.MiddleName e_name from sc_trn_details a left join memployee b on a.empid=b.Id where sctrnid=" + ddl_sc_range.SelectedValue + "");
            DataTable final_disp = new DataTable();
            final_disp.Columns.Add(new DataColumn("empid", typeof(string)));
            final_disp.Columns.Add(new DataColumn("e_name", typeof(string)));
            final_disp.Columns.Add(new DataColumn("tnod", typeof(string)));
            final_disp.Columns.Add(new DataColumn("amount", typeof(string)));
            DataRow final_dr;
            foreach (DataRow dr in dtscdetails.Rows)
            {
                final_dr = final_disp.NewRow();
                final_dr["empid"] = dr["empid"].ToString();
                final_dr["e_name"] = dr["e_name"].ToString();
                final_dr["tnod"] = dr["manhr"].ToString();
                final_dr["amount"] = dr["grossamt"].ToString();
                final_disp.Rows.Add(final_dr);

                grid_sc.DataSource = final_disp;
                grid_sc.DataBind();
            }
            if (grid_sc.Rows.Count > 0)
            {
                ddl_sc_range.Enabled = false;
                Button1.Text = "Change";
            }
        }
        else if (Button1.Text == "Change")
        {
            Button1.Text = "Process";
            ddl_sc_range.Enabled = true;
            grid_sc.DataSource = null;
            grid_sc.DataBind();
        }


    }
    protected void disp()
    {

        //grid_view.DataBind();
        grid_view.DataBind();
        DataTable dtt = dbhelper.getdata("select a.emp_id, a.id pa_id,LEFT(CONVERT(varchar,a.date,101),10)Date,a.class,regamt+nightamt total_amt,(b.lastname+', '+b.firstname+' '+b.middlename) emp_name from pay_adjustment_details a left join memployee b on a.emp_id=b.Id where b.PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and a.pay_id is null");
        grid_pa.DataSource = dtt;
        grid_pa.DataBind();
        //DataTable dt = ViewState["additionalamt"] as DataTable;
        //DataRow dr;
        //foreach (DataRow drr in dtt.Rows)
        //{
        //    dr = dt.NewRow();
        //    dr["empid"] = drr["emp_id"];
        //    dr["incomeid"] = 3;
        //    dr["empname"] = drr["emp_name"];
        //    dr["otherincome"] = "Payroll Adjustment";
        //    dr["amount"] = drr["total_amt"];
        //    dr["notes"] = drr["class"];
        //    dt.Rows.Add(dr);

        //    ViewState["additionalamt"] = dt;

        //    grid_pa.DataSource = dt;
        //    grid_pa.DataBind();
        //}

        //grid_manualincome.DataBind();
        if (grid_view.Rows.Count > 0)
            btn_save.Visible = true;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (txt_remarks.Text.Length > 0 && ddl_payroll_group.SelectedValue != "0" && ddl_pg.SelectedValue != "0" && ddl_pg.SelectedValue != "")
        {
            int rowcount = 0;
            stateclass a = new stateclass();

            a.sa = DateTime.Now.Year.ToString();
            a.sb = ddl_payroll_group.SelectedValue;    //ddl_payroll_group.SelectedValue;
            a.sc = Session["user_id"].ToString();
            a.sd = txt_remarks.Text;
            string x = bol.payotherincome(a);
                int i = 0;
                int ii=0;
                foreach (GridViewRow gv in grid_view.Rows)
                {
                    TextBox txt_amt=(TextBox)grid_view.Rows[ii].Cells[3].FindControl("txt_amt");
                    TextBox txt_amttotax = (TextBox)grid_view.Rows[ii].Cells[3].FindControl("txt_amttotax");
                    a.sa = x;
                    a.sb = gv.Cells[8].Text; // emp id 
                    a.sc = gv.Cells[9].Text; // oi id
                    a.sd = txt_amt.Text.Length == 1 || txt_amt.Text.Length == 0 ? "0" : txt_amt.Text.Replace("&nbsp;", "0"); // amount to be paid
                    a.se = txt_amttotax.Text.Length == 1 || txt_amttotax.Text.Length == 0 ? "0" : txt_amttotax.Text.Replace("&nbsp;", "0"); // amount to be tax
                    a.sf = gv.Cells[3].Text.Length == 1 || gv.Cells[3].Text.Length == 0 ? "0" : gv.Cells[3].Text.Replace("&nbsp;", "0"); // worked hours
                    a.sh = gv.Cells[2].Text.Length == 1 || gv.Cells[2].Text.Length == 0 ? "0" : gv.Cells[2].Text.Replace("&nbsp;", "0"); // allowed income
                    bol.payotherincomeline(a);
                    ii++;
                }
                i++;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='transotherincome'", true);
        }
        else
            lbl_remarks.Text = "*";
    }
    protected void click_pg(object sender, EventArgs e)
    {
        DataTable dtdt = ViewState["additionalamt"] as DataTable;
        hdn_dtr_tardy.Value = "0";
        hdn_dtr_absent.Value = "0";
        DataTable dt = getdata.queryotherincome(ddl_payroll_group.SelectedValue, ddl_pg.SelectedValue, ddl_pg.SelectedItem.Text);
        DataTable dtt = dbhelper.getdata("select a.emp_id,a.class,(b.lastname+', '+b.firstname+' '+b.middlename) emp_name,sum(regamt+nightamt) total_amt from pay_adjustment_details a left join memployee b on a.emp_id=b.Id  where b.PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and a.pay_id is null group by a.emp_id,a.class,(b.lastname+', '+b.firstname+' '+b.middlename)");
        DataRow drr;
        foreach (DataRow dr in dtt.Rows)
        {
            string empid = dr["emp_id"].ToString();
           // string otherid = dr["otherincomid"].ToString();
            DataRow[] addchecking = dtdt.Select(" empid='" + empid + "' and otherincome='Payroll Adjustment - "+ dr["class"].ToString()+" '");
            if (addchecking.Count() == 0)
            {
                drr = dtdt.NewRow();
                drr["rown"] = dtdt.Rows.Count;
                drr["empid"] = dr["emp_id"].ToString();
                drr["incomeid"] = "1";
                drr["empname"] = dr["emp_name"].ToString();
                drr["otherincome"] = "Payroll Adjustment - "+dr["class"].ToString();
                drr["type"] = "1";
                drr["schedule"] = "-";
                drr["Taxable"] = "True";
                drr["total_hours_worked"] = "-";
                drr["amt_to_bepaid"] = "0.00";
                drr["amt_to_be_tax"] = dr["total_amt"].ToString();
                drr["amount"] = dr["total_amt"].ToString();
                drr["notes"] = "";
                dtdt.Rows.Add(drr);
            }
            else
            {
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["empid"] = dr["emp_id"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["incomeid"] = "1";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["empname"] = dr["emp_name"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["otherincome"] = "Payroll Adjustment - " + dr["class"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["type"] = "1";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["schedule"] = "-";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["Taxable"] = "True";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["total_hours_worked"] = "-";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amt_to_bepaid"] = "0.00";
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amt_to_be_tax"] = dr["total_amt"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amount"] = dr["total_amt"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["notes"] = "";
            }
        }
        foreach (DataRow dr in dt.Rows)
        {
            string empid=dr["empid"].ToString();
            string otherid=dr["otherincomid"].ToString();
            DataRow[] addchecking = dtdt.Select(" empid='" + empid + "' and incomeid='" + otherid + "'");
            if (addchecking.Count() == 0)
            {
                drr = dtdt.NewRow();
                drr["rown"] = dtdt.Rows.Count;
                drr["empid"] = dr["empid"].ToString();
                drr["incomeid"] = dr["otherincomid"].ToString();
                drr["empname"] = dr["emp_name"].ToString();
                drr["otherincome"] = dr["OtherIncome"].ToString();
                drr["type"] = dr["type"].ToString();
                drr["schedule"] = dr["schedule"].ToString();
                drr["Taxable"] = dr["Taxable"].ToString();
                drr["total_hours_worked"] = dr["total_hours_worked"].ToString();
                drr["amt_to_bepaid"] = dr["amt_to_bepaid"].ToString();
                drr["amt_to_be_tax"] = dr["amt_to_be_tax"].ToString();
                drr["amount"] = dr["amount"].ToString();
                drr["notes"] = "";
                dtdt.Rows.Add(drr);
            }
            else
            {
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["empid"] = dr["empid"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["incomeid"] = dr["otherincomid"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["empname"] = dr["emp_name"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["otherincome"] = dr["OtherIncome"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["type"] = dr["type"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["schedule"] = dr["schedule"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["Taxable"] = dr["Taxable"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["total_hours_worked"] = dr["total_hours_worked"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amt_to_bepaid"] = dr["amt_to_bepaid"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amt_to_be_tax"] = dr["amt_to_be_tax"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["amount"] = dr["amount"].ToString();
                dtdt.Rows[int.Parse(addchecking[0]["rown"].ToString())]["notes"] = "";
            }
        }
        ViewState["additionalamt"] = dtdt;
        txt_searchemp.Text = "";
        lbl_err_msg.Text = "";
        grid_view.DataSource = dtdt;
        grid_view.DataBind();
        if (grid_view.Rows.Count > 0)
        {
            grid_sc.DataBind();
            grid_pa.DataBind();
        }
        disp();
    }
    protected void bounddata(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TextBox txt_amt = (TextBox)e.Row.FindControl("txt_amt");
            //DataTable get_absent_hours = dbhelper.getdata("select " +
            // "SUM(case when absent='True' then fixnofhours else 0 end " +
            // "+  " +
            // "case when lwop =1 then fixnofhours else 0 end) absent_hrs from  " +
            // "( " +
            // "select (select COUNT(*) from tleaveapplicationline where numberofhours='1' and withpay='False' and CONVERT(date,date)=convert(date,a.date) and EmployeeId=a.EmployeeId )lwop, a.onleave,a.absent,a.date,a.fixnofhours from TDTRLine a " +
            // "where a.dtrid=" + ddl_pg.SelectedValue + " and a.EmployeeId="+e.Row.Cells[0].Text+"  " +
            // ") absent_hrs ");
            //DataTable get_tardy_hours = dbhelper.getdata("select SUM(a.tardylatehours + a.tardyundertimehours) tardy_hrs from TDTRLine a where a.dtrid=" + ddl_pg.SelectedValue + " and a.EmployeeId=" + e.Row.Cells[0].Text + "");
            //DataTable emp = dbhelper.getdata("select * from memployee where id=" + e.Row.Cells[0].Text + "");
            //string taxtable = emp.Rows[0]["taxtable"].ToString() == "Semi-Monthly"?"2":"1";
            //decimal dbp_daily = decimal.Parse(txt_amt.Text) * 12 / decimal.Parse(emp.Rows[0]["fixnumberofdays"].ToString());
            //decimal dbp_hourly = dbp_daily / decimal.Parse(emp.Rows[0]["fixnumberofhours"].ToString());
            //decimal dbp_perpayroll = decimal.Parse(txt_amt.Text) / decimal.Parse(taxtable);
            //decimal total_amt=0;
            //if (e.Row.Cells[2].Text == "3")
            //{
            //    total_amt = dbp_perpayroll - (dbp_hourly * decimal.Parse(get_absent_hours.Rows[0]["absent_hrs"].ToString()));
            //    txt_amt.Text = string.Format("{0:n2}", total_amt); 
            //}

            //if (e.Row.Cells[2].Text == "4")
            //{
            //    total_amt = (dbp_perpayroll - (dbp_hourly * (decimal.Parse(get_absent_hours.Rows[0]["absent_hrs"].ToString()) + decimal.Parse(get_tardy_hours.Rows[0]["tardy_hrs"].ToString()))));
            //    txt_amt.Text = string.Format("{0:n2}", total_amt); 
            //}
            
        }
    }
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            string query = string.Format("select a.id, a.firstname+', '+a.lastname+' '+a.Middlename fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.idnumber+' '+a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}~{1}", reader["id"], reader["fullname"]));
                    //retCategory.Add(reader.GetString(0));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }
    private string GetExcelSheetNames1(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();

         

            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    protected void insertdatatableaddincome(object sender, EventArgs e)
    {
        DataTable dt = ViewState["additionalamt"] as DataTable;
        DataTable dtemp=dbhelper.getdata("select * from memployee");

        if (ddl_pg.SelectedValue == "0" || ddl_pg.SelectedValue.Length==0)
            Response.Write("<script>alert('Pls. specify payroll range!')</script>");
        else
        {
            if (file_data.HasFile)
            {
                if (ddl_type.Text == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Other Income Type');", true);
                }
                else
                {

                    System.Data.DataSet DtSet;
                    System.Data.OleDb.OleDbDataAdapter MyCommand;
                    string path = string.Concat(Server.MapPath("~/Excel/" + file_data.FileName));
                    file_data.SaveAs(path);
                    string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                    OleDbConnection MyConnection = new OleDbConnection();
                    MyConnection.ConnectionString = excelConnectionString;
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
                    MyCommand.TableMappings.Add("Table", "TestTable");
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet);
                    MyConnection.Close();

                    foreach (DataRow dr in DtSet.Tables[0].Rows)
                    {
                        string idno = dr["id number"].ToString();
                        string amount = dr["amount"].ToString().Replace(",", "");
                        DataRow[] dtemprow = dtemp.Select("BiometricIdNumber='" + idno + "'");
                        //DataTable dtt = dbhelper.getdata("select id,LastName+ +FirstName as name,* from Memployee where IdNumber='" + idno + "'");

                        if (dtemprow.Count() > 0 && decimal.Parse(dtemprow[0]["FixNumberOfDays"].ToString()) > 0 && decimal.Parse(dtemprow[0]["FixNumberOfHours"].ToString())>0)
                        {
                            DataTable dtincome = getdata.addotherincome(ddl_type.SelectedValue, amount.Replace(",", ""), dtemprow[0]["FixNumberOfDays"].ToString(), dtemprow[0]["FixNumberOfHours"].ToString(), ddl_pg.SelectedValue, ddl_pg.SelectedItem.Text, dtemprow[0]["id"].ToString());

                            if (dtemprow.Count() != 0)
                            {
                                DataRow[] drchk = dt.Select("empid=" + dtemprow[0]["id"].ToString() + " and incomeid=" + ddl_type.SelectedValue + " ");
                                DataRow drr;
                                if (drchk.Count() == 0)
                                {
                                    drr = dt.NewRow();
                                    drr["rown"] = dt.Rows.Count;
                                    drr["empid"] = dtemprow[0]["id"].ToString();
                                    drr["incomeid"] = ddl_type.SelectedValue;
                                    drr["empname"] = dtemprow[0]["lastname"].ToString() + "," + dtemprow[0]["firstname"].ToString();
                                    drr["otherincome"] = ddl_type.SelectedItem.Text;
                                    drr["type"] = dtincome.Rows[0]["type"].ToString();
                                    drr["schedule"] = dtincome.Rows[0]["schedule"].ToString();
                                    drr["Taxable"] = dtincome.Rows[0]["istaxable"].ToString();
                                    drr["total_hours_worked"] = "0";
                                    drr["amt_to_bepaid"] = dtincome.Rows[0]["amt_to_bepaid"].ToString();
                                    drr["amt_to_be_tax"] = dtincome.Rows[0]["amt_to_be_tax"].ToString();
                                    drr["amount"] = amount.Replace(",", "");
                                    drr["notes"] = txt_addinc_remarks.Text;
                                    dt.Rows.Add(drr);
                                    ViewState["additionalamt"] = dt;
                                    grid_view.DataSource = dt;
                                    grid_view.DataBind();
                                    txt_searchemp.Text = "";
                                    lbl_err_msg.Text = "";
                                }
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('idnumber does not register');", true);
                        }

                    }
                }

            }

            else
            {

                if (chkadditional())
                {
                    if (lbl_bals.Value.Length > 0)
                    {
                        DataRow[] dtemprow = dtemp.Select("id=" + lbl_bals.Value + "");
                        DataTable dtincome = getdata.addotherincome(ddl_type.SelectedValue, txt_addinc_amt.Text.Replace(",", ""), dtemprow[0]["FixNumberOfDays"].ToString(), dtemprow[0]["FixNumberOfHours"].ToString(), ddl_pg.SelectedValue, ddl_pg.SelectedItem.Text, lbl_bals.Value);
                        DataRow[] drchk = dt.Select("empid=" + lbl_bals.Value + " and incomeid=" + ddl_type.SelectedValue + "");
                        DataRow dr;
                        if (drchk.Count() == 0)
                        {
                            dr = dt.NewRow();
                            dr["rown"] = dt.Rows.Count;
                            dr["empid"] = lbl_bals.Value;
                            dr["incomeid"] = ddl_type.SelectedValue;
                            dr["empname"] = txt_searchemp.Text;
                            dr["otherincome"] = ddl_type.SelectedItem.Text;
                            dr["type"] = dtincome.Rows[0]["type"].ToString();
                            dr["schedule"] = dtincome.Rows[0]["schedule"].ToString();
                            dr["Taxable"] = dtincome.Rows[0]["istaxable"].ToString();
                            dr["total_hours_worked"] = dtincome.Rows[0]["total_hours_worked"].ToString();
                            dr["amt_to_bepaid"] = dtincome.Rows[0]["amt_to_bepaid"].ToString();
                            dr["amt_to_be_tax"] = dtincome.Rows[0]["amt_to_be_tax"].ToString();
                            dr["amount"] = txt_addinc_amt.Text.Replace(",", "");
                            dr["notes"] = txt_addinc_remarks.Text;
                            dt.Rows.Add(dr);

                            ViewState["additionalamt"] = dt;

                            grid_view.DataSource = dt;
                            grid_view.DataBind();
                            txt_searchemp.Text = "";
                            lbl_err_msg.Text = "";
                        }
                        else
                            lbl_err_msg.Text = "Exist!";
                    }
                    else
                        lbl_err_msg.Text = "Not yet registered!";
                }

            }
        }
    }
    protected bool chkadditional()
    {
        bool err=true;
        if(lbl_bals.Value.Length==0)
        {
            lbl_err_msg.Text="Not Registered!";
            err=false;
        }
        else if(txt_addinc_amt.Text.Length==0)
        {
            lbl_err_msg.Text="Invalid Input Amount";
               err=false;
        }
        else if (ddl_type.SelectedValue=="0")
        {
            lbl_err_msg.Text = "Invalid Input Type";
            err = false;
        }
        else
             lbl_err_msg.Text="";

        return err;
    }
    protected void delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            DataTable dt = ViewState["additionalamt"] as DataTable;
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["additionalamt"] = dt;
            grid_view.DataSource = dt;
            grid_view.DataBind();
        }
    }
}