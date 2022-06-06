using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_hr_editemployee : System.Web.UI.Page
{

    public static string user_id;

    public static int locationId, preference;
    protected void Page_Load(object sender, EventArgs e)
    {
        user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (!IsPostBack)
        {
            loadable();
            disp();
           
        }
    }

    protected void cancelreport(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update Approver set status='cancel' where id=" + row.Cells[0].Text + "");

            string query = "select count(*),under_id,herarchy from Approver where emp_id=" + row.Cells[1].Text + " and status is null group by under_id,herarchy";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dbhelper.getdata("update Approver set herarchy='" + i + "' where under_id=" + dt.Rows[i]["under_id"].ToString() + "");
            }
            gridviewrow_report();
            tb_tab.Text = "10";
        }
    }
    protected void ChangePreference(object sender, EventArgs e)
    {
        string commandArgument = (sender as LinkButton).CommandArgument;
        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        locationId = Convert.ToInt32(grid_reporting.Rows[rowIndex].Cells[0].Text);
        preference = Convert.ToInt32(grid_reporting.DataKeys[rowIndex].Value);
        preference = commandArgument == "up" ? preference - 1 : preference + 1;
        this.UpdatePreference(locationId, preference);
        rowIndex = commandArgument == "up" ? rowIndex - 1 : rowIndex + 1;
        locationId = Convert.ToInt32(grid_reporting.Rows[rowIndex].Cells[0].Text);
        preference = Convert.ToInt32(grid_reporting.DataKeys[rowIndex].Value);
        preference = commandArgument == "up" ? preference + 1 : preference - 1;
        this.UpdatePreference(locationId, preference);
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    private void UpdatePreference(int locationId, int preference)
    {
        string constr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE approver SET herarchy = @under_id WHERE Id = @Id"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", locationId);
                    cmd.Parameters.AddWithValue("@under_id", preference);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                    //retCategory.Add(reader.GetString(0));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    //report
    private void gridviewrow_report()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("col_1", typeof(string)));
        dt.Columns.Add(new DataColumn("col_2", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["col_1"] = string.Empty;
        dr["col_2"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["Item_List_report"] = dt;
        grid_report.DataSource = dt;
        grid_report.DataBind();
    }
    public void setrow_report()
    {
        int rowIndex = 0;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List_report"];
        dtCurrentTable.Clear();
        DataRow drCurrentRow = null;
        if (dtCurrentTable != null)
        {
            for (int i = 0; i < grid_skill.Rows.Count; i++)
            {
                TextBox ddl_reportto = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("ddl_reportto");
                TextBox lbl_bal = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_bal");
                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                drCurrentRow["col_1"] = ddl_reportto.Text;
                drCurrentRow["col_2"] = lbl_bal.Text;
                dtCurrentTable.Rows.Add(drCurrentRow);
                rowIndex++;
            }

            ViewState["Item_List_report"] = dtCurrentTable;
        }
    }
    protected void ButtonAdd_Click_report(object sender, EventArgs e)
    {
        int x = addnewrow_report();
    }
    protected int addnewrow_report()
    {
        int rowIndex = 0;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List_report"];
        DataRow drCurrentRow = null;

        if (dtCurrentTable.Rows.Count > 0)
        {
            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
            {

                TextBox ddl_reportto = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("ddl_reportto");


                Label lbl_report = (Label)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_report");
                Label lbl_report_desp = (Label)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_report_desp");

                TextBox lbl_bal = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_bal");


                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                dtCurrentTable.Rows[i - 1]["col_1"] = ddl_reportto.Text.Length == 0 ? lbl_report_desp.Text : ddl_reportto.Text;
                dtCurrentTable.Rows[i - 1]["col_2"] = lbl_bal.Text;

                if (i == dtCurrentTable.Rows.Count)
                {
                    lbl_report.Text = ddl_reportto.Text.Length == 0 ? "empty" : "";

                }

                if ((lbl_report.Text).Contains("empty"))
                    goto exit;
                rowIndex++;
            }

            bool x = check();
            if (x)
            {

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["Item_List_report"] = dtCurrentTable;

                grid_report.DataSource = dtCurrentTable;
                grid_report.DataBind();
            }
        }
        setPreviousData_report();
        return 1;
    exit:
        return rowIndex;
    }
    protected void setPreviousData_report()
    {
        int rowIndex = 0;
        if (ViewState["Item_List_report"] != null)
        {
            DataTable dt = (DataTable)ViewState["Item_List_report"];

            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox ddl_reportto = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("ddl_reportto");


                    Label lbl_report = (Label)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_report");
                    Label lbl_report_desp = (Label)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_report_desp");
                    TextBox lbl_bal = (TextBox)grid_report.Rows[rowIndex].Cells[1].FindControl("lbl_bal");
                    ImageButton can = (ImageButton)grid_report.Rows[rowIndex].Cells[2].FindControl("can");

                    if (i != dt.Rows.Count - 1)
                        ddl_reportto.Visible = false;
                    else
                        can.Visible = false;

                    //grid_report.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    lbl_report_desp.Text = dt.Rows[i]["col_1"].ToString();
                    lbl_bal.Text = dt.Rows[i]["col_2"].ToString();
                    rowIndex++;
                }
            }
        }
    }
    protected void grid_report_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["Item_List_report"] != null)
        {

            DataTable dt = (DataTable)ViewState["Item_List_report"];
            DataRow drCurrentRow = null;
            int rowIndex = Convert.ToInt32(e.RowIndex);

            if (dt.Rows.Count > 1)
            {
                dt.Rows.Remove(dt.Rows[rowIndex]);
                drCurrentRow = dt.NewRow();
                ViewState["Item_List_report"] = dt;
                grid_report.DataSource = dt;
                grid_report.DataBind();

                for (int i = 0; i <= grid_skill.Rows.Count - 1; i++)
                {
                    grid_report.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                }
                setPreviousData_report();
            }
        }
    }
    protected void btn_report(object sender, EventArgs e)
    {
     

        //if (dt.Rows.Count == 0)
        //{

            int ii = 0;
            foreach (GridViewRow gr in grid_report.Rows)
            {
                string query = "select count(*) as cnt from Approver where emp_id=" + Request.QueryString["app_id"].ToString() + "";
                DataTable dt = new DataTable();
                dt = dbhelper.getdata(query);
                TextBox lbl_bal = (TextBox)gr.Cells[1].FindControl("lbl_bal");

                if (dt.Rows.Count == 0)
                {
                    dbhelper.getdata("insert into approver values(" + Request.QueryString["app_id"].ToString() + ",'" + lbl_bal.Text.ToString().Replace("'", "''") + "'," + ii + ",NULL)");
                    ii++;
                }
                else
                {
                    int aa = (int.Parse(dt.Rows[0]["cnt"].ToString())-1) + 1;
                    dbhelper.getdata("insert into approver values(" + Request.QueryString["app_id"].ToString() + ",'" + lbl_bal.Text.ToString().Replace("'", "''") + "'," +  aa + ",NULL)");
                    ii++;
                }

            }

            disp();
            tb_tab.Text = "10";
            panelOverlay.Visible = false;
            panelReport.Visible = false;
        //}
        //else
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        TextBox lbl_bal = (TextBox)grid_report.c.Cells[1].FindControl("lbl_bal");

        //        dbhelper.getdata("insert into approver values(" + Request.QueryString["app_id"].ToString() + ",'" + lbl_bal.Text.ToString().Replace("'", "''") + "'," +  + ",NULL)");
        //    }
        //}
    }
    private bool check()
    {
        int cnt = 0;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List_report"];
        int rowCnt = dtCurrentTable.Rows.Count - 1;

        if (rowCnt > 0)
        {
            for (int k = rowCnt; k > 0; k--)
            {
                for (int i = 0; i < rowCnt; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        Label lblCs = (Label)grid_report.Rows[rowCnt].FindControl("lbl_report");
                        string last = dtCurrentTable.Rows[rowCnt].ItemArray[1 + j].ToString();
                        string one = dtCurrentTable.Rows[i].ItemArray[1 + j].ToString();

                        if (last == one)
                        {
                            if (j == 0)
                            {
                                cnt += 1;
                                lblCs.Text = "already taken";
                            }
                        }
                        else
                        {
                            if (j == 0 && cnt == 0)
                                lblCs.Text = "";

                        }
                    }
                }
            }

        }
        if (cnt == 0)
            return true;
        else
            return false;


    }
    protected void click_add(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
        Button btn = (Button)sender;
        switch (btn.CommandName)
        {
            case "jobhistory":
                panelPopUpPanel.Visible = true;
                break;
            case "educational":
                panelAttainment.Visible = true;
                break;
            case "asset":
                panelAsset.Visible = true;
                break;
            case "fmember":
                panelFamily.Visible = true;
                break;
            case "bankdetails":
                panelBank.Visible = true;
                break;
            case "ReportTo":
                {       
                    panelReport.Visible = true;
                    gridviewrow_report();
                   
                    break;
                }
           
        }
    }
    protected void close(object sender, EventArgs e)
    {
        panelOverlay.Visible = false;
        panelPopUpPanel.Visible = false;
        panelAttainment.Visible = false;
        panelAsset.Visible = false;
        panelFamily.Visible = false;
        panelBank.Visible = false;
        divstat.Visible = false;
        panelReport.Visible = false;
        div_status.Visible = false;
        div_releasing.Visible = false;
        div_gen2316.Visible = false;
    }
    protected void click_company(object sender, EventArgs e)
    {
        string query = "";
        DataTable dt;
        query = "select * from MBranch where CompanyId=" + ddl_company.SelectedValue + "";
        dt = dbhelper.getdata(query);
        ddl_branch.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }

        query = "select a.id,a.lastname+', '+a.firstname+' '+a.middlename as fullname from Memployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where b.PayrollGroup<>'Resigned' order by a.id desc";
        dt = dbhelper.getdata(query);

        ddl_reportto.Items.Clear();
        ddl_reportto.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_reportto.Items.Add(new ListItem(dr["fullname"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadable()
    {
        string query = "";

        DataTable dt;

        query = "select * from MCompany order by id desc";
        dt = dbhelper.getdata(query);

        ddl_company.Items.Clear();
        ddl_company.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_company.Items.Add(new ListItem(dr["company"].ToString(), dr["id"].ToString()));
        }

        

        query = "select ID,zipcode,zipcode+'-'+location+'/'+area as description from MZipCode order by location desc";
        dt = dbhelper.getdata(query);
        ddl_zipcode.Items.Clear();
        ddl_bzc.Items.Clear();
        ddl_bzc.Items.Add(new ListItem("N/A", "0"));
        ddl_zipcode.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_zipcode.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
            ddl_bzc.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MReligion order by id desc";
        dt = dbhelper.getdata(query);

        ddl_relegion.Items.Clear();
        ddl_relegion.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_relegion.Items.Add(new ListItem(dr["Religion"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MTaxCode order by id desc";
        dt = dbhelper.getdata(query);

        ddl_taxcode.Items.Clear();
        ddl_taxcode.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_taxcode.Items.Add(new ListItem(dr["TaxCode"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MDepartment order by id desc";
        dt = dbhelper.getdata(query);

        ddl_department.Items.Clear();
        ddl_department.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_department.Items.Add(new ListItem(dr["department"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPosition order by id desc";
        dt = dbhelper.getdata(query);

        ddl_position.Items.Clear();
        ddl_position.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_position.Items.Add(new ListItem(dr["position"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MDivision order by id desc";
        dt = dbhelper.getdata(query);

        ddl_divission.Items.Clear();
        ddl_divission.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_divission.Items.Add(new ListItem(dr["division"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPayrollGroup order by id desc";
        dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MAccount order by id desc";
        dt = dbhelper.getdata(query);

        ddl_gl.Items.Clear();
        ddl_gl.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_gl.Items.Add(new ListItem(dr["Account"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Mbloodtype order by id desc";
        dt = dbhelper.getdata(query);

        ddl_bloodtype.Items.Clear();
        ddl_bloodtype.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_bloodtype.Items.Add(new ListItem(dr["bloodtype"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MCitizenship order by id desc";
        dt = dbhelper.getdata(query);

        ddl_citizenship.Items.Clear();
        ddl_citizenship.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_citizenship.Items.Add(new ListItem(dr["Citizenship"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MPayrollType order by id desc";
        dt = dbhelper.getdata(query);

        ddl_payrolltype.Items.Clear();
        ddl_payrolltype.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payrolltype.Items.Add(new ListItem(dr["payrolltype"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MShiftCode where status is null order by id desc";
        dt = dbhelper.getdata(query);

        ddl_shiftcode.Items.Clear();
        ddl_shiftcode.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_shiftcode.Items.Add(new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString()));
        }


       DataTable dtCurrentTable = dbhelper.getdata("select a.id,a.serial,a.Description,b.qty from asset_inventory a " +
                                                "left join asset_details b on a.id=b.inventid " +
                                                "where b.status='On Stock'");
       ddl_serial.Items.Clear();
       ddl_serial.Items.Add(new ListItem("N/A", "0"));
       foreach (DataRow dr in dtCurrentTable.Rows)
       {
           ddl_serial.Items.Add(new ListItem(dr["serial"].ToString() + " - " + dr["Description"].ToString(), dr["id"].ToString()));
       }

    }
    protected void disp()
    {
        string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile, *,case when (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) is null then a.emp_status  else (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) end   empstatus from memployee a where a.id=" + Request.QueryString["app_id"].ToString() + "";
        DataTable dt = dbhelper.getdata(query);
        profile.Value = dt.Rows[0]["profile"].ToString();
        lb_name.Text = dt.Rows[0]["lastname"].ToString() + ", " + dt.Rows[0]["firstname"].ToString() + " " +  dt.Rows[0]["MiddleName"].ToString();
        txt_idnumber.Text=dt.Rows[0]["idnumber"].ToString();
        txt_lname.Text = dt.Rows[0]["lastname"].ToString();
        txt_fname.Text = dt.Rows[0]["firstname"].ToString();
        txt_mname.Text = dt.Rows[0]["MiddleName"].ToString();
        txt_exname.Text = dt.Rows[0]["ExtensionName"].ToString();
        txt_datehired.Text = dt.Rows[0]["DateHired"].ToString();
        txt_dateresigned.Text = dt.Rows[0]["DateResigned"].ToString();
        txt_presentaddres.Text = dt.Rows[0]["Address"].ToString();
        ddl_zipcode.SelectedValue = dt.Rows[0]["ZipCodeId"].ToString();
        txt_pnumber.Text = dt.Rows[0]["PhoneNumber"].ToString();
        txt_cnumber.Text = dt.Rows[0]["CellphoneNumber"].ToString();
        txt_email.Text = dt.Rows[0]["EmailAddress"].ToString();
        txt_dob.Text = dt.Rows[0]["DateOfBirth"].ToString();
        txt_pob.Text = dt.Rows[0]["PlaceOfBirth"].ToString();
        ddl_bzc.SelectedValue = dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString();
        ddl_sex.Text = dt.Rows[0]["sex"].ToString();
        ddl_cs.Text = dt.Rows[0]["CivilStatus"].ToString();
        ddl_citizenship.SelectedValue = dt.Rows[0]["CitizenshipId"].ToString();
        ddl_relegion.SelectedValue = dt.Rows[0]["ReligionId"].ToString();
        ddl_payrolltype.SelectedValue = dt.Rows[0]["PayrollTypeId"].ToString();
        txt_height.Text = dt.Rows[0]["Height"].ToString();
        txt_weight.Text = dt.Rows[0]["Weight"].ToString();
        txt_gsisno.Text = dt.Rows[0]["GSISNumber"].ToString();
        txt_sssno.Text = dt.Rows[0]["SSSNumber"].ToString();
        txt_hdmfno.Text = dt.Rows[0]["HDMFNumber"].ToString();
        txt_phicno.Text = dt.Rows[0]["PHICNumber"].ToString();
        txt_tin.Text = dt.Rows[0]["TIN"].ToString();
        ddl_taxcode.SelectedValue = dt.Rows[0]["TaxCodeId"].ToString();
        txt_atm.Text = dt.Rows[0]["ATMAccountNumber"].ToString();
        ddl_company.SelectedValue = dt.Rows[0]["CompanyId"].ToString();
        query = "select * from MBranch where CompanyId=" + ddl_company.SelectedValue + "";
        DataTable dts = dbhelper.getdata(query);
        ddl_branch.Items.Clear();
        foreach (DataRow dr in dts.Rows)
        {
            ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }
        ddl_branch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
        ddl_department.SelectedValue = dt.Rows[0]["DepartmentId"].ToString();
        ddl_divission.SelectedValue = dt.Rows[0]["DivisionId"].ToString();
        ddl_position.SelectedValue = dt.Rows[0]["PositionId"].ToString();
        l_position.Text = ddl_position.SelectedItem.ToString();
        ddl_pg.SelectedValue = dt.Rows[0]["PayrollGroupId"].ToString();
        ddl_gl.SelectedValue = dt.Rows[0]["AccountId"].ToString();
        ddl_payrolltype.SelectedValue = dt.Rows[0]["PayrollTypeId"].ToString();
        txt_fnod.Text = dt.Rows[0]["FixNumberOfDays"].ToString();
        txt_fnoh.Text = dt.Rows[0]["FixNumberOfHours"].ToString();
        txt_mr.Text = dt.Rows[0]["MonthlyRate"].ToString();
        txt_pr.Text = dt.Rows[0]["PayrollRate"].ToString();
        txt_dr.Text = dt.Rows[0]["DailyRate"].ToString();
        txt_adr.Text = dt.Rows[0]["AbsentDailyRate"].ToString();
        txt_hr.Text = dt.Rows[0]["HourlyRate"].ToString();
        txt_nhr.Text = dt.Rows[0]["NightHourlyRate"].ToString();
        txt_ohr.Text = dt.Rows[0]["OvertimeHourlyRate"].ToString();
        txt_onhr.Text = dt.Rows[0]["OvertimeNightHourlyRate"].ToString();
        txt_thr.Text = dt.Rows[0]["TardyHourlyRate"].ToString();
        ddl_taxtable.Text = dt.Rows[0]["TaxTable"].ToString();
        txt_hdmfaddon.Text = dt.Rows[0]["HDMFAddOn"].ToString();
        txt_sssaddon.Text = dt.Rows[0]["SSSAddOn"].ToString();
        ddl_hdmftype.Text = dt.Rows[0]["HDMFType"].ToString();
       // DataTable dt_chk_status_summary = dbhelper.getdata("select top 1 * from status_summary where emp_id=" + Request.QueryString["app_id"].ToString() + " order by id desc ");
        ddl_status.SelectedValue = dt.Rows[0]["empstatus"].ToString();//dt_chk_status_summary.Rows.Count > 0 ? dt_chk_status_summary.Rows[0]["description"].ToString() : dt.Rows[0]["emp_status"].ToString();
        if (dt.Rows[0]["SSSIsGrossAmount"].ToString() == "true")
            chk_sssgs.Checked = true;
        else
            chk_sssgs.Checked = false;
        if (dt.Rows[0]["IsMinimumWageEarner"].ToString() == "true")
            chk_imw.Checked = true;
        else
            chk_imw.Checked = false;
        if (dt.Rows[0]["leavestatus"].ToString() == "True")
            chk_l.Checked = true;
        else
            chk_l.Checked = false;
        txt_permanentadress.Text = dt.Rows[0]["permanentaddress"].ToString();
        ddl_bloodtype.SelectedValue = dt.Rows[0]["bloodtype"].ToString();
        ddl_shiftcode.SelectedValue = dt.Rows[0]["ShiftCodeId"].ToString();



        query = "select a.id,a.lastname+', '+a.firstname+' '+a.middlename as fullname from Memployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where b.PayrollGroup<>'Resigned' order by a.id desc";
        DataTable dt1 = dbhelper.getdata(query);
        ddl_reportto.Items.Clear();
        ddl_reportto.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt1.Rows)
        {
            ddl_reportto.Items.Add(new ListItem(dr["fullname"].ToString(), dr["id"].ToString()));
        }
        ddl_reportto.SelectedValue = dt.Rows[0]["reportto"].ToString();
        skilldisp();
    }
    protected void skilldisp()
    {
        DataTable dtCurrentTable = dbhelper.getdata("select * from Mskilline where empid=" + Request.QueryString["app_id"].ToString() + " and status is null");
        grid_skill.DataSource = dtCurrentTable;
        grid_skill.DataBind();

        dtCurrentTable = dbhelper.getdata("select id, company,position,left(convert(varchar,datef,101),10)datef,left(convert(varchar,datet,101),10)datet from mjobhistory where empid=" + Request.QueryString["app_id"].ToString() + " and status is null");
        grid_jobhistory.DataSource = dtCurrentTable;
        grid_jobhistory.DataBind();

        dtCurrentTable = dbhelper.getdata("select * from Meducattainment where empid=" + Request.QueryString["app_id"].ToString() + " and status is null");
        grid_educ.DataSource = dtCurrentTable;
        grid_educ.DataBind();

        dtCurrentTable = dbhelper.getdata("select a.id,b.id invid, b.serial,b.Description,b.Description cat,b.type,a.status from asset_assign a " +
                                        "left join asset_inventory b on a.assetid=b.id " +
                                        "where a.empid=" + Request.QueryString["app_id"].ToString() + " and a.status<>'cancel' ");
        grid_asset.DataSource = dtCurrentTable;
        grid_asset.DataBind();

        dtCurrentTable = dbhelper.getdata("select * from MFmembers where empid=" + Request.QueryString["app_id"].ToString() + " and status is null");
        grid_fmember.DataSource = dtCurrentTable;
        grid_fmember.DataBind();

        dtCurrentTable = dbhelper.getdata("select * from Mbankdetails where empid=" + Request.QueryString["app_id"].ToString() + " and status is null");
        grid_bankdetails.DataSource = dtCurrentTable;
        grid_bankdetails.DataBind();

        
        dtCurrentTable = dbhelper.getdata("select a.id,a.emp_id,a.under_id,a.herarchy,a.status, " +
                                        "b.LastName +', '+b.Firstname as fullname " +
                                        "from approver a " +
                                        "left join MEmployee b on a.under_id=b.id " +
                                        "where a.emp_id=" + Request.QueryString["app_id"].ToString() + " and a.status is null order by a.herarchy asc");

        grid_reporting.DataSource = dtCurrentTable;
        grid_reporting.DataBind();
            
    }
    protected void addskill(object sender, EventArgs e)
    {
        if (txt_skill.Text.Trim().Length > 0)
        {
            dbhelper.getdata("insert into Mskilline values(getdate()," + Request.QueryString["app_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + txt_skill.Text.ToString().Replace("'", "''") + "',NULL)");
            disp();
            tb_tab.Text = "4";
        }
        else
        {
            l_msg.Text = "Emtpy field";
        }
    }
    protected void addjobhistory(object sender, EventArgs e)
    {
        if (txt_company.Text.Trim().Length > 0|| txt_position.Text.Trim().Length > 0|| txt_datef.Text.Trim().Length > 0|| txt_datet.Text.Trim().Length > 0)
        {
            string[] date1 = txt_datef.Text.Split('/');
            string datef=date1[1].ToString()+"/"+date1[0].ToString()+"/"+date1[2].ToString();
            string[] date2 = txt_datet.Text.Split('/');
            string datet=date2[1].ToString()+"/"+date2[0].ToString()+"/"+date2[2].ToString();
            dbhelper.getdata("insert into Mjobhistory values(getdate()," + Request.QueryString["app_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + txt_position.Text.ToString().Replace("'", "''") + "','" + txt_company.Text.ToString().Replace("'", "''") + "','" + datef.ToString().Replace("'", "''") + "','" + datet.ToString().Replace("'", "''") + "',NULL)");
            disp();
            tb_tab.Text = "5";
            panelOverlay.Visible = false;
            panelPopUpPanel.Visible = false;
        }
        else
            l_msg.Text = "Invalid Input!";
    }
    protected void addeduc(object sender, EventArgs e)
    {
        if (txt_school.Text.Trim().Length > 0 || txt_address.Text.Trim().Length > 0 || txt_yearf.Text.Trim().Length > 0 || txt_yeart.Text.Trim().Length > 0)
        {
            dbhelper.getdata("insert into Meducattainment values(getdate()," + Request.QueryString["app_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + ddl_cat.Text.ToString().Replace("'", "''") + "','" + txt_school.Text.ToString().Replace("'", "''") + "','" + txt_address.Text.ToString().Replace("'", "''") + "','" + txt_yearf.Text.ToString().Replace("'", "''") + "','" + txt_yeart.Text.ToString().Replace("'", "''") + "',NULL)");
            disp();
            tb_tab.Text = "6";
            panelOverlay.Visible = false;
            panelAttainment.Visible = false;
        }
        else
            l_msg.Text = "Invalid Input!";
    }
    protected void clickserial(object sender, EventArgs e)
    {
        string[] serdesc = ddl_serial.SelectedItem.Text.Trim().Split('-');
        txt_description.Text = serdesc[1];
    }
    protected void addasset(object sender, EventArgs e)
    {
        if (ddl_serial.SelectedValue !="0" || txt_remarks.Text.Length > 0)
        {
            dbhelper.getdata("insert into asset_assign values(getdate()," + Request.QueryString["app_id"].ToString() + "," + ddl_serial.SelectedValue + ",'" + txt_remarks.Text + "','Active')");
            dbhelper.getdata("update asset_details set status='Deployed' where inventid=" + ddl_serial.SelectedValue + " ");
            disp();
            tb_tab.Text = "7";
            panelOverlay.Visible = false;
            panelAsset.Visible = false;
        }
        else
           l_msg.Text = "Invalid Input!";
       
        
    }
    protected void clickreturn(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {


            panelOverlay.Visible = true;
            divstat.Visible = true;
            chngstatid.Value = row.Cells[0].Text;
            chngremarksid.Value = row.Cells[1].Text;
            //dbhelper.getdata("update asset_assign set status='Return' where id="+row.Cells[0].Text+" ");
            //dbhelper.getdata("update asset_inventory set status='Return' where id=" + row.Cells[1].Text + " ");
        }
    }
    protected void clickchangestat(object sender, EventArgs e)
    {
        ddl_remarks.Items.Clear();
        if (ddl_statassign.Text == "Return")
        {
            ddl_remarks.Items.Add(new ListItem("N/A", "0"));
            ddl_remarks.Items.Add(new ListItem("Damaged", "Damage"));
            ddl_remarks.Items.Add(new ListItem("Resigned", "On Stock"));
            ddl_remarks.Items.Add(new ListItem("Confiscated", "On Stock"));
        }
        else if (ddl_statassign.Text == "Lost")
        {
            ddl_remarks.Items.Add(new ListItem("N/A", "0"));
            ddl_remarks.Items.Add(new ListItem("Lost", "Lost"));
        }
        else
        {
            ddl_remarks.Items.Clear();
            ddl_remarks.Items.Add(new ListItem("N/A", "0"));
        }
    }
    protected void updatechangestat(object sender, EventArgs e)
    {
        if (ddl_remarks.SelectedValue == "0" || ddl_statassign.Text == "")
            Response.Write("<script>alert('Empty Feilds!')</script>");
        else
        {
            dbhelper.getdata("update asset_assign set status='" + ddl_statassign.Text + "-" + ddl_remarks.SelectedValue + "*" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + chngstatid.Value + " ");
            dbhelper.getdata("update asset_details set status='" + ddl_remarks.SelectedValue + "' where inventid=" + chngremarksid.Value + " ");
            disp();
            tb_tab.Text = "7";
            panelOverlay.Visible = false;
            divstat.Visible = false;
        }
    }
    protected void rowboundasset(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton can = (ImageButton)e.Row.Cells[7].FindControl("can");
            LinkButton LinkButton1 = (LinkButton)e.Row.Cells[7].FindControl("LinkButton1");
            if (e.Row.Cells[6].Text != "Active")
            {
                can.Visible = false;
                LinkButton1.Visible = false;
            }
           string[] stat= e.Row.Cells[6].Text.Trim().Split('-');
           e.Row.Cells[6].Text = stat[0].ToString();

        }
    }
    protected void addfmember(object sender, EventArgs e)
    {
        if (txt_fname1.Text.Length > 0 || txt_lname1.Text.Length > 0 || txt_mname1.Text.Length > 0 || txt_exname1.Text.Length > 0 || txt_relation.Text.Length > 0)
        {
            dbhelper.getdata("insert into MFmembers values(getdate()," + Request.QueryString["app_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + txt_fname1.Text.ToString().Replace("'", "''") + "','" + txt_lname1.Text.ToString().Replace("'", "''") + "','" + txt_mname1.Text.ToString().Replace("'", "''") + "','" + txt_exname1.Text.ToString().Replace("'", "''") + "','" + txt_relation.Text.ToString().Replace("'", "''") + "',NULL)");
            disp();
            tb_tab.Text = "8";
            panelOverlay.Visible = false;
            panelFamily.Visible = false;
           
        }
        else
            l_msg.Text = "Invalid Input!";
    }
    protected void addbdetails(object sender, EventArgs e)
    {
        if (txt_accno.Text.Length > 0 || txt_accname.Text.Length > 0 || txt_location.Text.Length > 0 )
        {
            dbhelper.getdata("insert into Mbankdetails values(getdate()," + Request.QueryString["app_id"].ToString() + "," + Session["user_id"].ToString() + ",'" + txt_accno.Text.ToString().Replace("'", "''") + "','" + txt_accname.Text.ToString().Replace("'", "''") + "','" + txt_location.Text.ToString().Replace("'", "''") + "',NULL)");
            disp();
            tb_tab.Text = "9";
            panelOverlay.Visible = false;
            panelBank.Visible = false;
        }
        else
            l_msg.Text = "Invalid Input!";
    }
    protected void cancelskill(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            ImageButton ib = (ImageButton)row.FindControl("can");
            dbhelper.getdata("update Mskilline set status='cancel-" + DateTime.Now.ToString() + "-" + Session["user_id"].ToString() + "' where id=" + ib.CommandName + "");
            disp();
            tb_tab.Text = "4";
        }
    }
    protected void canceljobhistory(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            ImageButton ib = (ImageButton)row.FindControl("can");
            dbhelper.getdata("update Mjobhistory set status='cancel-" + DateTime.Now.ToString() + "-" + Session["user_id"].ToString() + "' where id=" + ib.CommandName + "");
            disp();
            tb_tab.Text = "5";
        }
    }
    protected void canceleduc(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            ImageButton ib = (ImageButton)row.FindControl("can");
            dbhelper.getdata("update Meducattainment set status='cancel-" + DateTime.Now.ToString() + "-" + Session["user_id"].ToString() + "' where id=" + ib.CommandName + "");
            disp();
            tb_tab.Text = "6";
        }
    }
    protected void cancelasset(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                ImageButton ib = (ImageButton)row.FindControl("can");
               
                dbhelper.getdata("update asset_assign set status='cancel' where id=" + ib.CommandName + "");
                dbhelper.getdata("update asset_details set status='On Stock' where inventid=" + row.Cells[1].Text + " ");
                disp();
                tb_tab.Text = "7";
            }
            else
            { 
            }
        }
    }
    protected void cancelfmember(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            ImageButton ib = (ImageButton)row.FindControl("can");
            dbhelper.getdata("update MFmembers set status='cancel-" + DateTime.Now.ToString() + "-" + Session["user_id"].ToString() + "' where id=" + ib.CommandName + "");
            disp();
            tb_tab.Text = "8";
        }
    }
    protected void cancelbdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update Mbankdetails set status='cancel-" + DateTime.Now.ToString() + "-" + Session["user_id"].ToString() + "' where id=" + row.Cells[0].Text + "");
            disp();
            tb_tab.Text = "9";
        }
    }
    protected bool errchkidentity()
    {
        bool err = true;
        if (txt_idnumber.Text.Length == 0)
        {
            l_msg.Text = "Incorrect Input Id Number!";
            err = false;
        }
        else if (ddl_company.SelectedValue == "0")
        {
            l_msg.Text = "Incorrect Input Company!";
            err = false;
        }
        else if (int.Parse(ddl_branch.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Branch!";
            err = false;
        }
        else if (int.Parse(ddl_department.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Department!";
            err = false;
        }
        else if (int.Parse(ddl_divission.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Divission!";
            err = false;
        }
        else if (int.Parse(ddl_pg.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Payroll Group!";
            err = false;
        }
        else if (int.Parse(ddl_position.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Position!";
            err = false;
        }
        else if (int.Parse(ddl_reportto.SelectedValue) == 0)
        {
            l_msg.Text = "Incorrect Input Report To!";
            err = false;
        }
        else if (ddl_status.SelectedValue == " ")
        {
            l_msg.Text = "Incorrect Input Status!";
            err = false;
        }
        else if (txt_datehired.Text.Length == 0)
        {
            l_msg.Text = "Incorrect Input Date Hired!";
            err = false;
        }
        return err;
    }
    protected bool errchkempinfo()
    {
        bool err = true;
        if (txt_lname.Text.Length == 0)
        {
            l_msg.Text = "Last Name must be Required!";
            err = false;
        }
        else if (txt_fname.Text.Length == 0)
        {
            l_msg.Text = "First Name must be Required!";
            err = false;
        }
        else if (txt_mname.Text.Length == 0)
        {
            l_msg.Text = "Middle Name must be Required!";
            err = false;
        }
        return err;
    }
    protected bool errchkemppayroll()
    {
        bool err = true;
        if (txt_sssno.Text.Length == 0)
        {
            l_msg.Text = "SSS Number must be Required!";
            err = false;
        }
        else if (txt_hdmfno.Text.Length == 0)
        {
            l_msg.Text = "Pag-ibig Number must be Required!";
            err = false;
        }
        else if (txt_phicno.Text.Length == 0)
        {
            l_msg.Text = "Phil. Health Number must be Required!";
            err = false;
        }
        else if (int.Parse(ddl_taxcode.SelectedValue) == 0)
        {
            l_msg.Text = "Tax Code must be Required!";
            err = false;
        }
        else if (ddl_taxtable.SelectedValue == "")
        {
            l_msg.Text = "Tax Table must be Required!";
            err = false;
        }
        else if (int.Parse(ddl_gl.SelectedValue) == 0)
        {
            l_msg.Text = "Gl Accoount must be Required!";
            err = false;
        }
        else if (int.Parse(ddl_shiftcode.SelectedValue) == 0)
        {
            l_msg.Text = "Shift Code must be Required!";
            err = false;
        }
        else if (int.Parse(ddl_payrolltype.SelectedValue) == 0)
        {
            l_msg.Text = "Payroll Type must be Required!";
            err = false;
        }
        else if (txt_fnod.Text.Length == 0)
        {
            l_msg.Text = "Fix No. of Days must be Required!";
            err = false;
        }
        else if (txt_fnoh.Text.Length == 0)
        {
            l_msg.Text = "Fix No. of Hours must be Required!";
            err = false;
        }
        else if (txt_mr.Text.Length == 0)
        {
            l_msg.Text = "Monthly Rate must be Required!";
            err = false;
        }
        else if (txt_pr.Text.Length == 0)
        {
            l_msg.Text = "Payroll Rate must be Required!";
            err = false;
        }
        else if (txt_dr.Text.Length == 0)
        {
            l_msg.Text = "Daily Rate must be Required!";
            err = false;
        }
        else if (txt_adr.Text.Length == 0)
        {
            l_msg.Text = "Absent Daily Rate must be Required!";
            err = false;
        }
        else if (txt_hr.Text.Length == 0)
        {
            l_msg.Text = "Hourly Rate must be Required!";
            err = false;
        }
        else if (txt_nhr.Text.Length == 0)
        {
            l_msg.Text = "Night Hourly Rate must be Required!";
            err = false;
        }
        else if (txt_ohr.Text.Length == 0)
        {
            l_msg.Text = "Overtime Hourly Rate must be Required!";
            err = false;
        }
        else if (txt_onhr.Text.Length == 0)
        {
            l_msg.Text = "Overtime Night Hourly Rate must be Required!";
            err = false;
        }
        else if (txt_thr.Text.Length == 0)
        {
            l_msg.Text = "Tardy Hourly Rate must be Required!";
            err = false;
        }
        return err;
    }
    protected bool errchk()
    {
        //bool isNumeric = true;

        bool err = true;

        if (txt_fname.Text.Trim().Length == 0)
        {
            l_msg.Text = "First Name must be Required!";
            err = false;
        }
        else if (txt_lname.Text.Trim().Length == 0)
        {
            l_msg.Text = "Last Name must be Required!";
            err = false;
        }
        else if (txt_mname.Text.Trim().Length == 0)
        {
            l_msg.Text = "Middle Name must be Required!";
            err = false;
        }
        else if (txt_idnumber.Text.Trim().Length == 0)
        {
            l_msg.Text = "Id Number must be Required!";
            err = false;
        }
        else if (ddl_pg.SelectedValue == "0")
        {
            l_msg.Text = "Payroll Group must be Required!";
            err = false;
        }
        else if (ddl_company.SelectedValue == "0")
        {
            l_msg.Text = "Company must be Required!";
            err = false;
        }
        else if (ddl_branch.SelectedValue.Trim().Length == 0)
        {
            l_msg.Text = "Branch must be Required!";
            err = false;
        }
        else if (ddl_department.SelectedValue == "0")
        {
            l_msg.Text = "Department must be Required!";
            err = false;
        }
        else if (ddl_position.SelectedValue == "0")
        {
            l_msg.Text = "Position must be Required!";
            err = false;
        }
        else if (ddl_reportto.SelectedValue == "0")
        {
            l_msg.Text = "Report To must be Required!";
            err = false;
        }
        else if (ddl_status.SelectedValue.Trim().Length == 0)
        {
            l_msg.Text = "Status must be Required!";
            err = false;
        }
        else
        {
            l_msg.Text = "Successfully Save";
        }

        return err;
    }
    protected void click_update_employee(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from tdtr where status is null and payroll_id is null ");
        if (dt.Rows.Count == 0)
        {
            //if (errchk())
            //{
            //employee identity
            stateclass a = new stateclass();
            a._idnumber = txt_idnumber.Text;
            a._lastname = txt_lname.Text;
            a._firstname = txt_fname.Text;
            a._middlename = txt_mname.Text;
            a._extensionname = txt_exname.Text;
            a._presentaddress = txt_presentaddres.Text;
            a._zipcode = ddl_zipcode.SelectedValue;
            a._phonenumber = txt_pnumber.Text;
            a._cellphonenumber = txt_cnumber.Text;
            a._emailaddress = txt_email.Text;
            a._dateofbirth = txt_dob.Text;
            a._placeofbirth = txt_pob.Text;
            a._birthzipcode = ddl_bzc.SelectedValue;
            a._datehired = txt_datehired.Text;
            a._dateresigned = txt_dateresigned.Text;
            a._sex = ddl_sex.SelectedValue;
            a._civilstatus = ddl_cs.Text;
            a._citizenship = ddl_citizenship.SelectedValue;
            a._religion = ddl_relegion.SelectedValue;
            a._height = txt_height.Text.Length == 0 ? "0" : txt_height.Text.Trim().Replace(",", "");
            a._weight = txt_weight.Text.Length == 0 ? "0" : txt_weight.Text.Trim().Replace(",", "");
            a._gsisnumber = txt_gsisno.Text;
            a._sssnumber = txt_sssno.Text;
            a._hdmfnumber = txt_hdmfno.Text;
            a._phicnumber = txt_phicno.Text;
            a._tin = txt_tin.Text;
            a._taxcode = ddl_taxcode.SelectedValue;
            a._atmaccountnumber = txt_atm.Text;
            a._company = ddl_company.SelectedValue;
            a._branch = ddl_branch.SelectedValue.Length == 0 ? "0" : ddl_branch.SelectedValue;
            a._department = ddl_department.SelectedValue;
            a._division = ddl_divission.SelectedValue;
            a._position = ddl_position.SelectedValue;
            a._payrollgroup = ddl_pg.SelectedValue;
            a._glaccount = ddl_gl.SelectedValue;
            a._payrolltype = ddl_payrolltype.SelectedValue;
            a._shiftcode = ddl_shiftcode.SelectedValue;
            a._fixnoofdays = txt_fnod.Text.Length == 0 ? "0" : txt_fnod.Text.Trim().Replace(",", "");
            a._fixnoofhours = txt_fnoh.Text.Length == 0 ? "0" : txt_fnoh.Text.Trim().Replace(",", "");
            a._monthlyrate = txt_mr.Text.Length == 0 ? "0" : txt_mr.Text.Trim().Replace(",", "");
            a._payrollrate = txt_pr.Text.Length == 0 || txt_pr.Text == "NaN" || txt_pr.Text == "Infinity" ? "0" : txt_pr.Text.Trim().Replace(",", "");
            a._dailyrate = txt_dr.Text.Length == 0 || txt_dr.Text == "NaN" || txt_dr.Text == "Infinity" ? "0" : txt_dr.Text.Trim().Replace(",", "");
            a._absentdailyrate = txt_adr.Text.Length == 0 || txt_adr.Text == "NaN" || txt_adr.Text == "Infinity" ? "0" : txt_adr.Text.Trim().Replace(",", "");
            a._hourlyrate = txt_hr.Text.Length == 0 || txt_hr.Text == "NaN" || txt_hr.Text == "Infinity" ? "0" : txt_hr.Text.Trim().Replace(",", "");
            a._nighthourlyrate = txt_nhr.Text.Length == 0 || txt_nhr.Text == "NaN" || txt_nhr.Text == "Infinity" ? "0" : txt_nhr.Text.Trim().Replace(",", "");
            a._overtimehourlyrate = txt_ohr.Text.Length == 0 || txt_ohr.Text == "NaN" || txt_ohr.Text == "Infinity" ? "0" : txt_ohr.Text.Trim().Replace(",", "");
            a._overtimenighthourlyrate = txt_onhr.Text.Length == 0 || txt_onhr.Text == "NaN" || txt_onhr.Text == "Infinity" ? "0" : txt_onhr.Text.Trim().Replace(",", "");
            a._tardyhourlyrate = txt_thr.Text.Length == 0 || txt_thr.Text == "NaN" || txt_thr.Text == "Infinity" ? "0" : txt_thr.Text.Trim().Replace(",", "");
            a.userid = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            a._taxtable = ddl_taxtable.SelectedValue.Trim().Length == 0 ? "0" : ddl_taxtable.SelectedValue;
            a._hdmfaddon = txt_hdmfaddon.Text.Length == 0 ? "0" : txt_hdmfaddon.Text.Trim().Replace(",", "");
            a._sssaddon = txt_sssaddon.Text.Length == 0 ? "0" : txt_sssaddon.Text.Trim().Replace(",", "");
            a._hdmftype = ddl_hdmftype.Text;
            a._sssgrosssalary = chk_sssgs.Checked == true ? "true" : "false";
            a._isminimum = chk_imw.Checked == true ? "true" : "false";
            a._permanentaddress = txt_permanentadress.Text;
            a._bloodtype = ddl_bloodtype.SelectedValue;
            a._unqueid = Request.QueryString["app_id"].ToString();
            a.saa = chk_l.Checked == true ? "True" : "False";
            a.sbb = ddl_status.SelectedValue.Trim().Length == 0 ? "0" : ddl_status.SelectedValue;
            a.scc = ddl_reportto.SelectedValue.Trim().Length == 0 ? "0" : ddl_reportto.SelectedValue;

            //chk by idnumber
            DataTable dtgetidnumber = dbhelper.getdata("select * from MEmployee where IdNumber='" + txt_idnumber.Text + "' ");
            //chk by empid
            DataTable dtgetempid = dbhelper.getdata("select * from MEmployee where Id=" + a._unqueid + "");
            string query = "";
            if (tb_tab.Text == "1")
            {
                if (errchkidentity())
                {
                    //change data
                    string dater = a._dateresigned.Length == 0 ? " " : a._dateresigned;
                    query = "update MEmployee set [CompanyId]=" + a._company + ",[BranchId]=" + a._branch + ",[DepartmentId]=" + a._department + ",[DivisionId]=" + a._division + ",[PositionId]=" + a._position + ",[PayrollGroupId]=" + a._payrollgroup + ",reportto=" + a.scc + ",emp_status=" + a.sbb + ",[DateHired]='" + a._datehired + "',[DateResigned]='" + dater + "' ";
                    if (dtgetidnumber.Rows.Count < 2)
                        query += ",[IdNumber]=" + a._idnumber + "";
                    query += " where id=" + a._unqueid + "";
                    dbhelper.getdata(query);
                    //create logs
                    query = "insert into history_emp_identity (date,emp_id,user_id,idnumber,company_id,branch_id,department_id,divission_id,payrollgroup_id,position_id,reportto_id,status_id,datehired) " +
                          "values" +
                          "(GETDATE()," + a._unqueid + "," + Session["user_id"] + "," + dtgetempid.Rows[0]["IdNumber"].ToString() + "," + dtgetempid.Rows[0]["CompanyId"].ToString() + "," + dtgetempid.Rows[0]["BranchId"].ToString() + "," + dtgetempid.Rows[0]["DepartmentId"].ToString() + ", " +
                          "" + dtgetempid.Rows[0]["DivisionId"].ToString() + "," + dtgetempid.Rows[0]["PayrollGroupId"].ToString() + "," + dtgetempid.Rows[0]["PositionId"].ToString() + "," + dtgetempid.Rows[0]["reportto"].ToString() + "," + dtgetempid.Rows[0]["emp_status"].ToString() + ",'" + dtgetempid.Rows[0]["DateHired"].ToString() + "')";
                    dbhelper.getdata(query);
                    l_msg.Text = "Successfully Save";
                }
            }
            //personal info
            if (tb_tab.Text == "2")
            {
                if (errchkempinfo())
                {
                    //change data
                    query = "update MEmployee set  [LastName]='" + a._lastname + "',[FirstName]='" + a._firstname + "' ,[MiddleName]='" + a._middlename + "' ,[ExtensionName]='" + a._extensionname + "' " +
                            ",[DateOfBirth]='" + a._dateofbirth + "',[PlaceOfBirth]='" + a._placeofbirth + "' ,[PlaceOfBirthZipCodeId]=" + a._birthzipcode + " ,[Sex]='" + a._sex + "' ,[CivilStatus]='" + a._civilstatus + "' " +
                            ",[CitizenshipId]=" + a._citizenship + ",[ReligionId]=" + a._religion + " ,[Height]=" + a._height + " ,[Weight]=" + a._weight + " ,[bloodtype]=" + a._bloodtype + "  " +
                            ",[permanentaddress]='" + a._permanentaddress + "',[ZipCodeId]=" + a._zipcode + " ,[Address]='" + a._presentaddress + "' ,[PhoneNumber]='" + a._phonenumber + "' " +
                            ",[CellphoneNumber]='" + a._cellphonenumber + "',[EmailAddress]='" + a._emailaddress + "' where id=" + a._unqueid + " ";
                    dbhelper.getdata(query);
                    //create logs
                    query = "insert into history_personal_info " +
                            "(date,emp_id,user_id,sex,civilstatus,citizenship_id,relegion_id,hieght,wieght,pressentaddress,permanentaddress,birthzipcode_id,addresszipcode_id,phonenumber,celphonenumber,emailaddress,firstname,lastname,middlename,ext_name) " +
                            "values " +
                            "(GETDATE()," + a._unqueid + "," + Session["user_id"] + ",'" + dtgetempid.Rows[0]["Sex"].ToString() + "','" + dtgetempid.Rows[0]["CivilStatus"].ToString() + "'," + dtgetempid.Rows[0]["CitizenshipId"].ToString() + " " +
                            "," + dtgetempid.Rows[0]["ReligionId"].ToString() + ",'" + dtgetempid.Rows[0]["Height"].ToString() + "','" + dtgetempid.Rows[0]["Weight"].ToString() + "','" + dtgetempid.Rows[0]["Address"].ToString() + "' " +
                            ",'" + dtgetempid.Rows[0]["permanentaddress"].ToString() + "'," + dtgetempid.Rows[0]["PlaceOfBirthZipCodeId"].ToString() + "," + dtgetempid.Rows[0]["ZipCodeId"].ToString() + ",'" + dtgetempid.Rows[0]["PhoneNumber"].ToString() + "','" + dtgetempid.Rows[0]["CellphoneNumber"].ToString() + "','" + dtgetempid.Rows[0]["EmailAddress"].ToString() + "' " +
                            ",'" + dtgetempid.Rows[0]["FirstName"].ToString() + "','" + dtgetempid.Rows[0]["LastName"].ToString() + "','" + dtgetempid.Rows[0]["MiddleName"].ToString() + "','" + dtgetempid.Rows[0]["ExtensionName"].ToString() + "')";
                    dbhelper.getdata(query);
                    l_msg.Text = "Successfully Save";
                }
            }
            //payroll info
            if (tb_tab.Text == "3")
            {
                if (errchkemppayroll())
                {
                    //change data
                    query = "update MEmployee set " +
                           "[GSISNumber]='" + a._gsisnumber + "',[SSSNumber]='" + a._sssnumber + "',[HDMFNumber]='" + a._hdmfnumber + "',[PHICNumber]='" + a._phicnumber + "',[TIN]='" + a._tin + "',[ATMAccountNumber]='" + a._atmaccountnumber + "' " +
                           ",[TaxCodeId]=" + a._taxcode + ",[TaxTable]='" + a._taxtable + "',[AccountId]=" + a._glaccount + ",[ShiftCodeId]=" + a._shiftcode + ",[FixNumberOfDays]='" + a._fixnoofdays + "' " +
                           ",[FixNumberOfHours]='" + a._fixnoofhours + "',[MonthlyRate]='" + a._monthlyrate + "',[PayrollRate]='" + a._payrollrate + "',[DailyRate]='" + a._dailyrate + "' " +
                           ",[AbsentDailyRate]='" + a._absentdailyrate + "',[HourlyRate]='" + a._hourlyrate + "',[NightHourlyRate]='" + a._nighthourlyrate + "',[OvertimeHourlyRate]='" + a._overtimehourlyrate + "'  " +
                           ",[OvertimeNightHourlyRate]='" + a._overtimenighthourlyrate + "',[TardyHourlyRate]='" + a._tardyhourlyrate + "',[PayrollTypeId]=" + a._payrolltype + ",[IsMinimumWageEarner]='" + a._isminimum + "',leavestatus='" + a.saa + "' " +
                           "where id=" + a._unqueid + "";
                    dbhelper.getdata(query);
                    //create logs
                    query = "insert into history_payroll " +
                            "(date,emp_id,user_id,taxcode_id,taxtable,gl_id,shift_id,paytype_id,fixnoofdays,fixnoofhours,monthlyrate,payrollrate,dailyrate,absentdailyrate,hourlyrate,nighthourlyrate,overtimehourlyrate,overtimenighthourlyrate,tardyhourlyrate,isminimum,isleavewpay) " +
                            "values" +
                            "(GETDATE()," + a._unqueid + "," + Session["user_id"] + "," + dtgetempid.Rows[0]["TaxCodeId"].ToString() + ",'" + dtgetempid.Rows[0]["TaxTable"].ToString() + "'," + dtgetempid.Rows[0]["AccountId"].ToString() + "," + dtgetempid.Rows[0]["ShiftCodeId"].ToString() + " " +
                            "," + dtgetempid.Rows[0]["PayrollTypeId"].ToString() + ",'" + dtgetempid.Rows[0]["FixNumberOfDays"].ToString() + "','" + dtgetempid.Rows[0]["FixNumberOfHours"].ToString() + "','" + dtgetempid.Rows[0]["MonthlyRate"].ToString() + "','" + dtgetempid.Rows[0]["PayrollRate"].ToString() + "' " +
                            ",'" + dtgetempid.Rows[0]["DailyRate"].ToString() + "','" + dtgetempid.Rows[0]["AbsentDailyRate"].ToString() + "','" + dtgetempid.Rows[0]["HourlyRate"].ToString() + "','" + dtgetempid.Rows[0]["NightHourlyRate"].ToString() + "','" + dtgetempid.Rows[0]["OvertimeHourlyRate"].ToString() + "' " +
                            ",'" + dtgetempid.Rows[0]["OvertimeNightHourlyRate"].ToString() + "','" + dtgetempid.Rows[0]["TardyHourlyRate"].ToString() + "','" + dtgetempid.Rows[0]["IsMinimumWageEarner"].ToString() + "','" + dtgetempid.Rows[0]["leavestatus"].ToString() + "')";
                    dbhelper.getdata(query);
                    l_msg.Text = "Successfully Save";
                }
            }

        }
        else
        {
            Response.Write("<script>alert('Transaction Denied!')</script>");
        }

    }

   
    protected void change_status(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
        div_status.Visible = true;
        ddl_statusfinal.Enabled = true;
        span_id.Visible = true;
        txt_effdate.Visible = true;
        ddl_statusfinal.SelectedValue = ddl_status.SelectedValue;
        lbl_header.Text = "Change Status";
        hdn_proc.Value = "process_mempstatus";
        DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Request.QueryString["app_id"].ToString() + " and action is null");
        if (dt.Rows.Count > 0)
        {
            btn_proc_16.Visible = false;
            if (dt.Rows[0]["status"].ToString() == "For Request")
                lbl_errmsg.Text = "Quit Claim is Requested Already";
            else if (dt.Rows[0]["status"].ToString() == "For Released")
                lbl_errmsg.Text = "Quit Claim is For Released Already";
            else
                lbl_errmsg.Text = "Quit Claim is Released Already";
            ddl_statusfinal.Enabled = false;
        }

    }
    protected void quit_claim(object sender, EventArgs e)
    {
        if (ddl_status.SelectedValue == "4")
        {
            panelOverlay.Visible = true;
            div_status.Visible = true;
            ddl_statusfinal.SelectedValue = ddl_status.SelectedValue;
            ddl_statusfinal.Enabled = false;
            span_id.Visible = false;
            txt_effdate.Visible = false;
            lbl_header.Text = "Quit Claim Request";
            hdn_proc.Value = "quit_claim";
            DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Request.QueryString["app_id"].ToString() + " and action is null");
            if (dt.Rows.Count > 0)
            {
                btn_proc_16.Visible = false;
                if (dt.Rows[0]["status"].ToString() == "For Request")
                    lbl_errmsg.Text = "Quit Claim is Requested Already";
                else if (dt.Rows[0]["status"].ToString() == "For Released")
                    lbl_errmsg.Text = "Quit Claim is For Released Already";
                else
                    lbl_errmsg.Text = "Quit Claim is Released Already";
            }
        }
    }
    protected void select_status(object sender, EventArgs e)
    {
        if (ddl_statusfinal.SelectedValue != "0")
        {
            txt_effdate.Visible = true;  //FileUpload1.Visible = true;
            span_id.Visible = true;

        }
        else
        {
            txt_effdate.Visible = true;//FileUpload1.Visible = false;
            span_id.Visible = false;
        }
    }
    protected void process(object sender, EventArgs e)
    {
        if (chek_intput_changestatus())
        {
            string filename = "0";
            string ret = "0";
            string fileExtension = "0";
            string contenttype = "0";
            string fileconcat = "";
            if (FileUpload1.HasFile)
            {

                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    switch (hdn_proc.Value)
                    {

                        case "process_mempstatus":
                            filename = Server.MapPath("~/files/peremp/" + Request.QueryString["app_id"].ToString() + "/Employee_status");
                            using (SqlCommand cmd = new SqlCommand("process_mempstatus", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                string[] effdate = txt_effdate.Text.Split('/');
                                fileconcat = ddl_statusfinal.SelectedItem.Text;
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["app_id"].ToString();
                                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                                cmd.Parameters.Add("@statusid", SqlDbType.Int).Value = ddl_statusfinal.SelectedValue;
                                cmd.Parameters.Add("@effectivedate", SqlDbType.VarChar, 5000).Value = effdate[1] + "/" + effdate[0] + "/" + effdate[2];
                                cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = txt_notes.Text;
                                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                ret = cmd.Parameters["@out"].Value.ToString();
                                con.Close();
                            }
                            HttpFileCollection uploadedFiles = Request.Files;
                            for (int i = 0; i < uploadedFiles.Count; i++)
                            {
                                HttpPostedFile userPostedFile = uploadedFiles[i];
                                string[] fileName0 = userPostedFile.FileName.Split('.');
                                contenttype = userPostedFile.ContentType.ToString();
                                using (SqlCommand cmd = new SqlCommand("process_memployeestatusfile", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@empstatid", SqlDbType.Int).Value = ret;
                                    cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
                                    cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileName0[1];
                                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    string statusfile_ret = cmd.Parameters["@out"].Value.ToString();
                                    con.Close();
                                    if (!Directory.Exists(filename))
                                        Directory.CreateDirectory(filename);
                                    userPostedFile.SaveAs(filename + "\\" + statusfile_ret + "_" + fileconcat + "." + fileName0[1].ToString());

                                }
                            }

                            break;
                        case "quit_claim":
                            filename = Server.MapPath("~/files/peremp/" + Request.QueryString["app_id"].ToString() + "/Clearance");
                            using (SqlCommand cmd = new SqlCommand("quit_claim", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                fileconcat = "Clearance";
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["app_id"].ToString();
                                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true).ToString();
                                //cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
                                //cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileExtension;
                                cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = txt_notes.Text;
                                cmd.Parameters.Add("@status", SqlDbType.VarChar, 5000).Value = "For Request";
                                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                ret = cmd.Parameters["@out"].Value.ToString();
                                con.Close();
                            }
                            HttpFileCollection uploadedFiles1 = Request.Files;
                            for (int i = 0; i < uploadedFiles1.Count; i++)
                            {
                                HttpPostedFile userPostedFile = uploadedFiles1[i];
                                string[] fileName1 = userPostedFile.FileName.Split('.');
                                contenttype = userPostedFile.ContentType.ToString();
                                using (SqlCommand cmd = new SqlCommand("process_memployeeclearancefile", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@reqid", SqlDbType.Int).Value = ret;
                                    cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
                                    cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileName1[1];
                                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    string clearance_file_ret = cmd.Parameters["@out"].Value.ToString();
                                    con.Close();
                                    if (!Directory.Exists(filename))
                                        Directory.CreateDirectory(filename);
                                    userPostedFile.SaveAs(filename + "\\" + clearance_file_ret + "_" + fileconcat + "." + fileName1[1].ToString());
                                }
                            }
                            break;
                    }
                }
                Response.Redirect("editemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Request.QueryString["app_id"].ToString() + "");

            }
            else
                Response.Write("<script>alert('Incorrect File Type!')</script>");
        }
    }
    protected bool chek_intput_changestatus()
    {
        bool err = true;
        if (ddl_statusfinal.SelectedValue == "0")
        {
            lbl_errmsg.Text = "*Incorrect input for Status!";
            err = false;
        }
        else
            lbl_errmsg.Text = "";
        if (!FileUpload1.HasFile)
        {
            lbl_errmsg.Text = "*Incorrect input for Attachment!";
            err = false;
        }
        else
            lbl_errmsg.Text = "";
        if (txt_notes.Text.Length == 0)
        {
            lbl_errmsg.Text = "*Incorrect input for Notes!";
            err = false;
        }
        else
            lbl_errmsg.Text = "";


        return err;
    }


    protected void releasing(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Request.QueryString["app_id"].ToString() + " and action is null and (status='For Released' or status='Released')");
        if (dt.Rows.Count > 0)
        {
            reqid.Value = dt.Rows[0]["id"].ToString();
            panelOverlay.Visible = true;
            div_releasing.Visible = true;
            if (dt.Rows[0]["status"].ToString() == "Released")
            {
                btn_proc_17.Visible = false;
                lbl_rel_errmsg.Text = "Quit Claim is already Released!";
            }
            else
                btn_proc_17.Visible = true;
        }
        else
            Response.Write("<script>alert('Pending Request!')</script>");

    }
    protected void releasedqc(object sender, EventArgs e)
    {
        if (FileUpload2.HasFile)
        {
            dbhelper.getdata("update quit_claim_request set status='Released',releaseddate=getdate() where id=" + reqid.Value + "");
            string filename = Server.MapPath("~/files/peremp/" + Request.QueryString["app_id"].ToString() + "/QCReleasing");
            string ret = "0";
            if (!Directory.Exists(filename))
                Directory.CreateDirectory(filename);
            HttpFileCollection uploadedFiles = Request.Files;
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];
                if (userPostedFile.ContentLength > 0)
                {
                    string[] fileex = userPostedFile.FileName.Split('.');
                    string contenttype = userPostedFile.ContentType.ToString();
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("process_quit_claim_released", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                            cmd.Parameters.Add("@reqid", SqlDbType.Int).Value = reqid.Value;
                            cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
                            cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileex[1].ToString();
                            cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = txt_notes_released.Text;
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();

                        }
                    }
                    userPostedFile.SaveAs(filename + "\\" + ret + "." + fileex[1].ToString());
                }
            }
        }
        Response.Redirect("editemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Request.QueryString["app_id"].ToString() + "");
    }
    protected void gen2316(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
        div_gen2316.Visible = true;

    }
    protected void process2316(object sender, EventArgs e)
    {
        DataTable dtchek = getdata.dtitr(txt_yyyy.Text, Request.QueryString["app_id"].ToString());
        if (dtchek.Rows.Count > 0)
        {
            //ClientScriptManager cs = Page.ClientScript;
            //cs.RegisterStartupScript(this.GetType(), "Confirm", "Confirm()", true);
        }
        else
            Response.Redirect("2316process?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&empid=" + Request.QueryString["app_id"].ToString() + "&y=" + txt_yyyy.Text + "");


        //Response.Redirect("2316process?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&empid=" + row.Cells[1].Text + "");
    }
}