using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class content_hr_Otherincome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
            disp();
        }
    }
    protected void loadable()
    {



        frequency();   
        nontaxable("paymentkind");
        variability(ddl_frequency.SelectedItem.Text);

        DataTable dt_tax_rule = dbhelper.getdata("select * from MOtherIncomeTaxRule where status is null and paymentkind='No Rule'");
        ddl_tax_rule.Items.Clear();
        ddl_tax_rule.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt_tax_rule.Rows)
        {
            ddl_tax_rule.Items.Add(new ListItem(dr["description"].ToString() + " (" + Math.Round(decimal.Parse(dr["ceilingg"].ToString()), 2) +" " + dr["mode"].ToString() + ") ", dr["id"].ToString()));
        }


        
    }
    protected void frequency()
    {
        DataTable dtfrequency = dbhelper.getdata("select * from motherincomefrequency where status is null");
        ddl_frequency.Items.Clear();
        ddl_frequency.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dtfrequency.Rows)
        {
            ddl_frequency.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_frequency(object sender, EventArgs e)
    {
        if (ddl_frequency.SelectedValue == "2")
        {
            variability(ddl_frequency.SelectedItem.Text);
            schedule();
            lbl_schedule.Visible = true;
            alltaxrule("");
        }
        else if (ddl_frequency.SelectedValue == "3")
        {
            alltaxrule("No Rule");
        }
        else
        {
            alltaxrule("");
            rbl_range.Items.Clear();
            lbl_schedule.Visible = false;
        }
    }
    protected void variability(string variable)
    {
        string conditionis = variable == "one time set up" ? "where typee='one time set up'" : "where typee<>'one time set up'";
        DataTable dtOItype = dbhelper.getdata("select * from MOtherIncomeType  " + conditionis + " and status is null");
        ddl_type.Items.Clear();
        ddl_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dtOItype.Rows)
        {
            ddl_type.Items.Add(new ListItem(dr["typee"].ToString(), dr["id"].ToString()));
        }
    }
    protected void schedule()
    {
        DataTable dt_range = dbhelper.getdata("select * from riesol_amoroto_bagtasos_jr where status is null");
        rbl_range.Items.Clear();

        foreach (DataRow dr in dt_range.Rows)
        {
            rbl_range.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString() + " cut-off"));
        }
    }
    protected void disp()
    {
        string query = "select a.id,a.type,a.OtherIncome,a.Taxable,e.description frequency,b.typee variability,c.description taxrule,  " +
                    "d.typeeone incometype,case when len(a.schedule)>0 then a.schedule  else '-' end schedule, c.notes from MOtherIncome a " +
                    "left join motherincometype b on a.type=b.id " +
                    "left join MOtherIncomeTaxRule c on a.taxruleid=c.id " +
                    "left join MOtherIncomeTypeOne d on a.incometypeid=d.id " +
                    "left join motherincomefrequency e on a.frequencyid=e.id " +
                    "where action is null order by a.OtherIncome asc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void nontaxable(string paymentkind)
    {
        string condtionis = paymentkind == "No Rule" ? "" : "and (paymentkind='" + paymentkind + "')";
        DataTable dtOItypeOne = dbhelper.getdata("select * from MOtherIncomeTypeOne where status is null and istaxable='false' " + condtionis + " ");
        ddl_income_type.Items.Clear();
        ddl_income_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dtOItypeOne.Rows)
        {
            ddl_income_type.Items.Add(new ListItem(dr["typeeOne"].ToString() + "(" + dr["paymentkind"].ToString() + ")", dr["id"].ToString()));
        }
    }
    protected void alltaxrule(string paymentkind)
    {
        string condtionis = paymentkind == "No Rule" ? "and ((paymentkind='No Rule' and istaxable='True') or (paymentkind='No Rule' and istaxable='False') or (paymentkind='General' and istaxable='True')) " : " and paymentkind<>'No Rule'";
        DataTable dt_tax_rule = dbhelper.getdata("select * from MOtherIncomeTaxRule where status is null "); //"+condtionis+"
        ddl_tax_rule.Items.Clear();
        ddl_tax_rule.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt_tax_rule.Rows)
        {
            ddl_tax_rule.Items.Add(new ListItem(dr["description"].ToString() + " (" + Math.Round(decimal.Parse(dr["ceilingg"].ToString()), 2) + " " + dr["mode"].ToString() + ") ", dr["id"].ToString()));
        }
    }
    protected void taxable(string paymentkind)
    {
        string condtionis = paymentkind == "No Rule" ? "" : "and (paymentkind='" + paymentkind + "')";
        DataTable dtOItypeOne = dbhelper.getdata("select * from MOtherIncomeTypeOne where status is null and istaxable='True' " + condtionis + " ");
        ddl_income_type.Items.Clear();
        ddl_income_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dtOItypeOne.Rows)
        {
            ddl_income_type.Items.Add(new ListItem(dr["typeeOne"].ToString() +"("+ dr["paymentkind"].ToString()+")", dr["id"].ToString()));
        }
    }
    protected void taxablegenaral(string paymentkind)
    {
        string condtionis = paymentkind == "No Rule" ? "" : "and (paymentkind='" + paymentkind + "')";
        DataTable dtOItypeOne = dbhelper.getdata("select * from MOtherIncomeTypeOne where status is null and istaxable='False' " + condtionis + " ");
        ddl_income_type.Items.Clear();
        ddl_income_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dtOItypeOne.Rows)
        {
            ddl_income_type.Items.Add(new ListItem(dr["typeeOne"].ToString() + "(" + dr["paymentkind"].ToString() + ")", dr["id"].ToString()));
        }
    }
    protected void checkistaxable(object sender, EventArgs e)
    {
        //if (chk_taxable.Checked == true)
        //    taxable();
        //else
        //    nontaxable("paymentkind");
    }
    protected void add_income(object sender, EventArgs e)
    {
        if (checking())
        {
            DataTable dtcheck = dbhelper.getdata("select * from MOtherIncome where OtherIncome='" + txt_oi.Text.Trim() + "' and action is null");
            if (dtcheck.Rows.Count == 0)
            {
                string istaxable = chk_taxable.Checked == true ? "True" : "False";
                string ismandatory = chk_mandatory.Checked == true ? "True" : "False";
                DataTable dt = dbhelper.getdata("insert into MOtherIncome values('" + txt_oi.Text.Trim() + "','" + istaxable + "',0,'" + ismandatory + "',3,0,NULL,"+ddl_type.SelectedValue+","+ddl_income_type.SelectedValue+","+ddl_tax_rule.SelectedValue+",'"+rbl_range.SelectedItem+"',"+ddl_frequency.SelectedValue+",'Default')");
                disp();
            }
            else
                lbl_error.Text = "Already Exist!";
        }
    }
    protected void clicktypeone(object sender, EventArgs e)
    {
        //if (ddl_income_type.SelectedValue == "1")
        //{
        //    chk_taxable.Checked = true;
        //    chk_taxable.Enabled = false;
        //}
    }
    protected void clicktaxrule(object sender, EventArgs e)
    {
        DataTable dttaxrule = dbhelper.getdata("select * from MOtherIncomeTaxRule where id=" + ddl_tax_rule.SelectedValue + "");
        if (int.Parse(ddl_tax_rule.SelectedValue) >= 1 && int.Parse(ddl_tax_rule.SelectedValue) <= 9)
            nontaxable(dttaxrule.Rows[0]["paymentkind"].ToString());
        else if (int.Parse(ddl_tax_rule.SelectedValue) == 10)
            taxable(dttaxrule.Rows[0]["paymentkind"].ToString());
        else if (int.Parse(ddl_tax_rule.SelectedValue) == 11)
            taxablegenaral(dttaxrule.Rows[0]["paymentkind"].ToString());

    }
    protected void clicktype(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "2")
        {
            chk_taxable.Checked = true;
            chk_taxable.Enabled = false;
        }
        else
        {
            chk_taxable.Checked = false;
            chk_taxable.Enabled = true;
        }
    }
    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_can = (LinkButton)e.Row.FindControl("lnk_can");
            LinkButton LinkButton2 = (LinkButton)e.Row.FindControl("LinkButton2");
            if (e.Row.Cells[3].Text == "one time set up")
            {
                LinkButton2.Visible = false;
                lnk_can.Visible = true;
            }
            else
                lnk_can.Visible = false;
            //DataTable dtOI = dbhelper.getdata("select * from MOtherIncome where id=" + e.Row.Cells[0].Text + "");
            //if (dtOI.Rows[0]["status"].ToString() == "Default")
            //    lnk_can.Visible = false;
        }
    }
    protected bool checking()
    {
        bool tt = true;
        if (txt_oi.Text.Trim().Length == 0)
        {
            lbl_ioerr.Text = "*";
            tt = false;
        }
        else if (ddl_frequency.SelectedValue == "0")
        {
            Label3.Text = "*";
            tt = false;
        }
        else if (ddl_type.SelectedValue == "0")
        {
            Label1.Text = "*";
            tt = false;
        }
        else if (ddl_tax_rule.SelectedValue == "0")
        {
            Label2.Text = "*";
            tt = false;
        }
        else if (ddl_income_type.SelectedValue == "0")
        {
            lbl_income_type.Text = "*";
            tt = false;
        }
        else if (ddl_frequency.SelectedValue == "2")
        {
            if (rbl_range.SelectedValue.Length == 0)
            {
                lbl_sechedule.Text = "*";
                lbl_schedule.Visible = false;
                tt = false;
            }
            else
            {
                lbl_sechedule.Text = "";
                lbl_schedule.Visible = true;
                tt = true;
            }

        }
        else
        {
            lbl_ioerr.Text = "";
            Label3.Text = "";
            Label1.Text = "";
            Label2.Text = "";
            lbl_income_type.Text = "";
        }

        //if (txt_ceiling.Text.Trim().Length == 0)
        //{
        //    lbl_taxceilingerr.Text = "*";
        //    tt = false;
        //}
        //else
        //{
        //    lbl_taxceilingerr.Text = "";
        //}
        //if (txt_amount.Text.Trim().Length == 0)
        //{
        //    lbl_amterr.Text = "*";
        //    tt = false;
        //}
        //else
        //{
        //    lbl_amterr.Text = "";
        //}
        return tt;
    }
    protected void click_can(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                //dbhelper.getdata("update MOtherIncome set action='cancel-" + Session["user_id"] + "' where Id=" + row.Cells[0].Text + " ");
                dbhelper.getdata("update allowed_otherincome set action = 'cancel' where other_income_id = " + row.Cells[0].Text + "");
                Response.Write("<script>alert('Successfully Saved!')</script>");
                Response.Redirect("Motherincome", false);
            }
        }
    }
    protected void click_view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string query = "select a.id,a.amount,b.LastName+', '+b.FirstName+' '+b.MiddleName as employee,(select OtherIncome from MOtherIncome where Id = a.other_income_id)otherincome from allowed_otherincome a left join MEmployee b on a.empid = b.Id where a.action is null and a.other_income_id = " + row.Cells[0].Text + "";
            DataTable dt = dbhelper.getdata(query);
            viewdetails.DataSource = dt;
            viewdetails.DataBind();
            modalview.Style.Add("display", "block");
        }
    }
    protected void close(object sender, EventArgs e)
    {
        modalview.Style.Add("display", "none");
        panelOverlay.Visible = false;
        panelPopUpPanel.Visible = false;
        div_view_edit.Visible = false;
    }
    protected void click_allow(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            otherincomeid.Value=row.Cells[0].Text;
            grid_emp.DataBind();
            panelOverlay.Visible = true;
            panelPopUpPanel.Visible = true;
            panelPopUpPanel.Style.Add("left", "450px");
            panelPopUpPanel.Style.Add("width", "650px");
            panelPopUpPanel.Style.Add("top", "10%");
        }
    }
    protected void search(object sender, EventArgs e)
    {
        DataTable dtempall = getdata.empsearch(txt_search.Text,"a");
        grid_emp.DataSource = dtempall;
        grid_emp.DataBind();
    }
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grid_emp.HeaderRow.FindControl("chkboxSelectAll");
        int i = 0;
        foreach (GridViewRow row in grid_emp.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkEmp");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
                i++;
            }
            else
            {

                ChkBoxRows.Checked = false;
                if (i > 0)
                {
                    i--;
                }
            }
        }
    }
    protected void process(object sender, EventArgs e)
    {
        int i = 0;
        string selection = lbl_bals.Value;
        string amt = txt_income_amt.Text.Length > 0 ? txt_income_amt.Text.Replace(",", "") : "0";
        if (file_data.HasFile)
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" +DateTime.Now.ToString().Replace("/","").Replace(":","").Replace(" ","")+""+ file_data.FileName));
            file_data.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();
              try
               { 
                foreach (DataRow dr in DtSet.Tables[0].Rows)
                {
                    string idno = dr["IDNUMBER"].ToString();
                    DataTable getemp = dbhelper.getdata("select * from memployee where IdNumber='" + idno + "'");
                    if (getemp.Rows.Count > 0 && decimal.Parse(dr["amount"].ToString()) > 0)
                    {
                        dbhelper.getdata("insert into allowed_otherincome (date,other_income_id,empid,userid,amount) values (GETDATE()," + otherincomeid.Value + "," + getemp.Rows[0]["id"].ToString() + "," + Session["user_id"].ToString() + ",'" + dr["amount"].ToString().Replace(",","").Trim() + "')");
                    }
                }
              }
              catch (Exception ex)
              {
                  Response.Write("<script>alert('Please check your Excel Format!')</script>");
              }
        }
        else
        {
            DataTable dt = dbhelper.getdata("select * from allowed_otherincome where empid=" + lbl_bals.Value + " and other_income_id=" + otherincomeid.Value + " and action is null");
            if (dt.Rows.Count == 0)
                dbhelper.getdata("insert into allowed_otherincome (date,other_income_id,empid,userid,amount) values (GETDATE()," + otherincomeid.Value + "," + lbl_bals.Value + "," + Session["user_id"].ToString() + ",'" + amt + "')");
            else
                lbl_allowederr.Text = "Already Exist!";
        }
          
          
            
        txt_searchemp.Text = "";
        string query = "select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from allowed_otherincome a " +
             "left join memployee b on a.empid=b.id " +
             "left join MCompany c on b.CompanyId=c.Id " +
             "left join MBranch d on b.BranchId=d.Id " +
             "left join MDepartment e on b.DepartmentId=e.Id " +
             "where a.action is null and a.other_income_id=" + otherincomeid.Value + "";
        DataTable dtdetails = dbhelper.getdata(query);
        grid_list.DataSource = dtdetails;
        grid_list.DataBind();
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           
            otherincomeid.Value = row.Cells[0].Text;
            string query = "select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from allowed_otherincome a " +
                            "left join memployee b on a.empid=b.id " + 
                            "left join MCompany c on b.CompanyId=c.Id " +
                            "left join MBranch d on b.BranchId=d.Id " +
                            "left join MDepartment e on b.DepartmentId=e.Id " +
                            "where a.action is null and a.other_income_id="+row.Cells[0].Text+"";
            DataTable dtdetails = dbhelper.getdata(query);
            grid_list.DataSource = dtdetails;
            grid_list.DataBind();
            panelOverlay.Visible = true;
            div_view_edit.Visible = true;
            div_view_edit.Style.Add("left", "450px");
            div_view_edit.Style.Add("width", "650px");
            div_view_edit.Style.Add("top", "10%");
        }
    }
    protected void deleteall(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            string query = "update allowed_otherincome set action='cancel-" + Session["user_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "' where other_income_id = " + otherincomeid.Value + " and action is null ";
            DataTable dt = dbhelper.getdata(query);
            Response.Redirect("Motherincome");
        }
    }
    protected void click_cancel_det(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update allowed_otherincome set action='cancel-" + Session["user_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + " ");
                string query = "select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from allowed_otherincome a " +
                                  "left join memployee b on a.empid=b.id " +
                                  "left join MCompany c on b.CompanyId=c.Id " +
                                  "left join MBranch d on b.BranchId=d.Id " +
                                  "left join MDepartment e on b.DepartmentId=e.Id " +
                                  "where a.action is null and a.other_income_id=" + otherincomeid.Value + "";
                DataTable dtdetails = dbhelper.getdata(query);
                grid_list.DataSource = dtdetails;
                grid_list.DataBind();
            }
            else
            {
            }
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

}