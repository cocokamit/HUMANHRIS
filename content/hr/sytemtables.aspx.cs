using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_hr_mandatorytables : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            lodable();
    }
    protected void change_choice(object sender, EventArgs e)
    {
        transition();
        choice();
    }
    protected void falls()
    {
        plocation.Visible = false;
        plocation2.Visible = false;
        pgrid_civil.Visible = false;
        pgrid_civil2.Visible = false;
        pgrid_role.Visible = false;
        pgrid_role2.Visible = false;
        pgrid_level.Visible = false;
        pgrid_level2.Visible = false;
        pgrid_payg.Visible = false;
        pgrid_payg2.Visible = false;
        pgrid_department.Visible = false;
        pgrid_department2.Visible = false;
        pgrid_position.Visible = false;
        pgrid_position2.Visible = false;
        pgrid_coa.Visible = false;
        pgrid_coa2.Visible = false;
        panelOverlay.Visible = false;
        panelPopUpPanel.Visible = false;
        pgrid_relegion.Visible = false;
        pgrid_relegion2.Visible = false;
        pgrid_citizenship.Visible = false;
        pgrid_citizenship2.Visible = false;
        pgrid_zipcode.Visible = false;
        pgrid_zipcode2.Visible = false;
        pgrid_division.Visible = false;
        pgrid_division2.Visible = false;
        pgrid_company.Visible = false;
        pgrid_company2.Visible = false;
        plocation.Visible = false;
        plocation2.Visible = false;
        pgrid_branch.Visible = false;
        pgrid_branch2.Visible = false;
        pgrid_internalorder.Visible = false;
        pgrid_internalorder2.Visible = false;
        pgrid_store.Visible = false;
        pgrid_store2.Visible = false;
        Div1.Visible = false;
        pAdd.Visible = false;
    }
    protected void transition()
    {
        grid_payg.Visible = false;
        p_payg.Visible = false;
        grid_paytype.Visible = false;
        p_paytype.Visible = false;
        grid_department.Visible = false;
        p_department.Visible = false;
        grid_position.Visible = false;
        p_position.Visible = false;
        grid_coa.Visible = false;
        p_coa.Visible = false;
        grid_requirements.Visible = false;
        p_requirements.Visible = false;
        grid_relegion.Visible = false;
        p_relegion.Visible = false;
        grid_citizenship.Visible = false;
        p_citizenship.Visible = false;
        grid_zipcode.Visible = false;
        p_zipcode.Visible = false;
        grid_division.Visible = false;
        p_division.Visible = false;
        grid_bloodtype.Visible = false;
        p_bloodtype.Visible = false;
        grid_company.Visible = false;
        p_company.Visible = false;
        grid_branch.Visible = false;
        p_branch.Visible = false;
        grid_section.Visible = false;
        p_section.Visible = false;
        p_internalorder.Visible = false;
        grid_internalorder.Visible = false;
        grid_leve.Visible = false;
        p_leave.Visible = false;
        grid_store.Visible = false;
        p_store.Visible = false;
        grid_level.Visible = false;
        p_level.Visible = false;
        grid_role.Visible = false;
        p_role.Visible = false;
        grid_civil.Visible = false;
        p_civil.Visible = false;
    }
    protected void choice()
    {
        DataTable dt;
        switch (dl_choice.Text)
        {
            case "Payroll Group":
                payrolgroup();
                break;
            case "Civil Status":
                dt = dbhelper.getdata("select * from MCivilStatus");
                grid_civil.DataSource = dt;
                grid_civil.DataBind();
                grid_civil.Visible = true;
                p_civil.Visible = true;
                break;
            case"Level":
                dt = dbhelper.getdata("select * from MDivision2 order by Id asc");
                grid_level.DataSource = dt;
                grid_level.DataBind();
                grid_level.Visible = true;
                p_level.Visible = true;
                break;
            case "Role":
                dt = dbhelper.getdata("select * from MLevel");
                grid_role.DataSource = dt;
                grid_role.DataBind();
                grid_role.Visible = true;
                p_role.Visible = true;
                break;
            case "Payroll Type":
                dt = dbhelper.getdata("select * from MPayrollType");
                grid_paytype.DataSource = dt;
                grid_paytype.DataBind();
                grid_paytype.Visible = true;
                p_paytype.Visible = true;
                break;
            case "Department":
                dt = dbhelper.getdata("select * from MDepartment order by Department asc");
                grid_department.DataSource = dt;
                grid_department.DataBind();
                grid_department.Visible = true;
                p_department.Visible = true;
                break;
            case "Position":
                dt = dbhelper.getdata("select * from MPosition order by Position asc");
                grid_position.DataSource = dt;
                grid_position.DataBind();
                grid_position.Visible = true;
                p_position.Visible = true;
                break;
            case "COA":
                dt = dbhelper.getdata("select * from MAccount");
                grid_coa.DataSource = dt;
                grid_coa.DataBind();
                grid_coa.Visible = true;
                p_coa.Visible = true;
                break;
            case "Requirement/s":
                dt = dbhelper.getdata("select * from orley_empsrequirement order by [description] asc");
                grid_requirements.DataSource = dt;
                grid_requirements.DataBind();
                grid_requirements.Visible = true;
                p_requirements.Visible = true;
                break;
            case "Religion":
                dt = dbhelper.getdata("select * from MReligion order by Religion asc");
                grid_relegion.DataSource = dt;
                grid_relegion.DataBind();
                grid_relegion.Visible = true;
                p_relegion.Visible = true;
                break;
            case "Citizenship":
                dt = dbhelper.getdata("select * from MCitizenship order by Citizenship asc");
                grid_citizenship.DataSource = dt;
                grid_citizenship.DataBind();
                grid_citizenship.Visible = true;
                p_citizenship.Visible = true;
                break;
            case "Zip Code":
                dt = dbhelper.getdata("select * from MZipCode order by Area asc");
                grid_zipcode.DataSource = dt;
                grid_zipcode.DataBind();
                grid_zipcode.Visible = true;
                p_zipcode.Visible = true;
                break;
            case "Band":
                dt = dbhelper.getdata("select * from MDivision");
                grid_division.DataSource = dt;
                grid_division.DataBind();
                grid_division.Visible = true;
                p_division.Visible = true;
                break;
            case "Blood Type":
                dt = dbhelper.getdata("select * from Mbloodtype");
                grid_bloodtype.DataSource = dt;
                grid_bloodtype.DataBind();
                grid_bloodtype.Visible = true;
                p_bloodtype.Visible = true;
                break;
            case "Company":
                dt = dbhelper.getdata("select b.ZipCode fuck,* from MCompany a left join MZipCode b on a.zipcode = b.Id order by Company asc");
                grid_company.DataSource = dt;
                grid_company.DataBind();
                grid_company.Visible = true;
                p_company.Visible = true;
                break;
            case "Location":
                dt = dbhelper.getdata("select (select company from mcompany where id=a.CompanyId)company,* from [MBranch] a order by a.Branch asc");
                grid_branch.DataSource = dt;
                grid_branch.DataBind();
                grid_branch.Visible = true;
                p_branch.Visible = true;
                break;
            case "Department Code":
                dt = dbhelper.getdata("select (select Department from MDepartment where Id = a.dept_id)departmentarea, * from Msection a order by a.seccode asc");
                grid_section.DataSource = dt;
                grid_section.DataBind();
                grid_section.Visible = true;
                p_section.Visible = true;
                break;
            case"Internal Order":
                dt = dbhelper.getdata("select a.Id, a.DeptId, b.Department, a.InternalOrder from MInternalOrder a left join MDepartment b on b.Id = a.DeptId order by a.InternalOrder asc");
                grid_internalorder.DataSource = dt;
                grid_internalorder.DataBind();
                grid_internalorder.Visible = true;
                p_internalorder.Visible = true;
                break;
            case "Insurance":
                dt = dbhelper.getdata("select * from Minsurance where action is null");
                grid_insuarnce.DataSource = dt;
                grid_insuarnce.DataBind();
                grid_insuarnce.Visible = true;
                p_insurance.Visible = true;
                break;
            case "Leave":
                dt = dbhelper.getdata("select * from Mleave where action is null");
                grid_leve.DataSource = dt;
                grid_leve.DataBind();
                grid_leve.Visible = true;
                p_leave.Visible = true;
                break;
            case "Outlet":
                dt = dbhelper.getdata("select * from Mstore where status is null");
                grid_store.DataSource = dt;
                grid_store.DataBind();
                grid_store.Visible = true;
                p_store.Visible = true;
                break;
        }
    }
    protected void lodable()
    {
        transition();
        payrolgroup();

        DataTable dt;
        string query = "select * from MCompany order by Company asc";
        dt = dbhelper.getdata(query);
        ddl_com.Items.Clear();
        ddl_com.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_com.Items.Add(new ListItem(dr["Company"].ToString(), dr["Id"].ToString()));
        }

        query = "select ZipCode+' - '+Location+' '+Area ZipAreas,* from MZipCode order by Location asc";
        dt = dbhelper.getdata(query);
        ddl_zipcomp.Items.Clear();
        ddl_zipcomp.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_zipcomp.Items.Add(new ListItem(dr["ZipAreas"].ToString(), dr["Id"].ToString()));
            ddlcompzip.Items.Add(new ListItem(dr["ZipAreas"].ToString(), dr["Id"].ToString()));
        }

        query = "select * from MDepartment order by Department asc";
        dt = dbhelper.getdata(query);
        ddl_dept.Items.Clear();
        ddl_dept.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_dept.Items.Add(new ListItem(dr["Department"].ToString(), dr["Id"].ToString()));
            ddl_deptsec.Items.Add(new ListItem(dr["Department"].ToString(), dr["Id"].ToString()));
            ddl_compint.Items.Add(new ListItem(dr["Department"].ToString(), dr["Id"].ToString()));
            ddl_departinternal.Items.Add(new ListItem(dr["Department"].ToString(), dr["Id"].ToString()));
        }
    }
    protected void payrolgroup()
    {
        DataTable dt = dbhelper.getdata("select Id,PayrollGroup,case when status = '1' then 'Enable' else 'Disable' end status from MPayrollGroup");
        grid_payg.DataSource = dt;
        grid_payg.DataBind();
        grid_payg.Visible = true;
        p_payg.Visible = true;
    }

    protected void save_payrollgroup(object sender, EventArgs e)
    {
        if (txt_payrollgroup.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MPayrollGroup where PayrollGroup='" + txt_payrollgroup.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtinsert = dbhelper.getdata("insert into MPayrollGroup values ('" + txt_payrollgroup.Text + "','1')select scope_identity() idd");
                choice();
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "addpgroup";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New PayrollGroup";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_payrollgroup.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtinsert.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void hidepgroup(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MPayrollGroup where Id = '" + seriesid.Value + "'");
            if (dt.Rows[0]["status"].ToString() == "1")
            {
                chckdisp_btn.Checked = true;
            }
            txtpgrp.Text = dt.Rows[0]["PayrollGroup"].ToString();
            pgrid_payg.Visible = true;
            pgrid_payg2.Visible = true;
        }
    }
    protected void editpgrp(object sender, EventArgs e)
    {
        if (txtpgrp.Text != "")
        {
            string val = "0";
            DataTable dt = dbhelper.getdata("select * from MPayrollGroup where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtpgroup";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["PayrollGroup"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpgrp.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            if (chckdisp_btn.Checked)
            {
                val = "1";
            }
            dbhelper.getdata("update MPayrollGroup set PayrollGroup = '" + txtpgrp.Text.Replace("'", "").ToString() + "',status = '" + val + "' where id = '" + seriesid.Value + "' and id <> 4 ");
            choice();
            falls();
        }
    }
    
    protected void save_civil(object sender, EventArgs e)
    {
        if (txt_civil.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MCivilStatus where CivilStatus = '" + txt_civil.Text.Replace("'", "").ToString() + "'");
            if (dtcheck.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MCivilStatus values ('" + txt_civil.Text.Replace("'", "").ToString() + "','1')");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "addCivil";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Civil";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_civil.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "CivilStatus";
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void showpcivil(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MCivilStatus where Id = '" + seriesid.Value + "'");
            txt_civilupdate.Text = dt.Rows[0]["CivilStatus"].ToString();
            pgrid_civil.Visible = true;
            pgrid_civil2.Visible = true;
        }
    }
    protected void updatecivil(object sender, EventArgs e)
    {
        if (txt_civilupdate.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MCivilStatus where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updatecivil";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["CivilStatus"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_civil.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Update CivilStatus";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MCivilStatus set CivilStatus = '" + txt_civilupdate.Text.Replace("'", "").ToString() + "'where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    
    protected void save_role(object sender, EventArgs e)
    {
        if (txt_role.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MLevel where Level = '" + txt_role.Text.Replace("'", "").ToString() + "'");
            if (dtcheck.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MLevel values ('" + txt_role.Text.Replace("'", "").ToString() + "')");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "addrole";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Role";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_role.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Role";
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void showprole(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MLevel where Id = '" + seriesid.Value + "'");
            txt_roleupdate.Text = dt.Rows[0]["Level"].ToString();
            pgrid_role.Visible = true;
            pgrid_role2.Visible = true;
        }
    }
    protected void updaterole(object sender, EventArgs e)
    {
        if (txt_roleupdate.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MLevel where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updaterole";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Level"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_roleupdate.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MLevel set Level = '" + txt_roleupdate.Text.Replace("'", "").ToString() + "'where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    
    protected void save_level(object sender, EventArgs e)
    {
        if (txt_level.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MDivision2 where Division2 = '" + txt_level.Text.Replace("'", "").ToString() + "'");
            if (dtcheck.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MDivision2 values ('" + txt_level.Text.Replace("'", "").ToString() + "')");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "addlevel";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Level";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_level.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Level";
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void showplevel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MDivision2 where Id = '" + seriesid.Value + "'");
            if (dt.Rows[0]["AllowOtMeal"].ToString() == "True")
            {
                chckallowot.Checked = true;
            }
            txt_levelupdate.Text = dt.Rows[0]["Division2"].ToString();
            pgrid_level.Visible = true;
            pgrid_level2.Visible = true;
        }
    }
    protected void updatelevel(object sender, EventArgs e)
    {
        if (txt_levelupdate.Text != "")
        {
            string ck = "";
            DataTable dt = dbhelper.getdata("select * from MDivision2 where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updatelevel";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Division2"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_levelupdate.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Update Level";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            if (chckallowot.Checked == true)
            {
                ck = "True";
            }
            else
                ck = "False";
            dbhelper.getdata("update MDivision2 set Division2 = '" + txt_levelupdate.Text.Replace("'", "").ToString() + "',AllowOtMeal='" + ck + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }

    protected void save_payrolltype(object sender, EventArgs e)
    {
        if (txt_payrolltype.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MPayrolltype where Payrolltype='" + txt_payrolltype.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MPayrolltype values ('" + txt_payrolltype.Text + "')");
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_department(object sender,EventArgs e)
    {
        if (txt_department.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MDepartment where department='" + txt_department.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtinsert = dbhelper.getdata("insert into MDepartment values ('" + txt_department.Text + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddDepartment";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Department";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_department.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtinsert.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_position(object sender, EventArgs e)
    {
        if (txt_position.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MPosition where position='" + txt_position.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselect = dbhelper.getdata("insert into MPosition values ('" + txt_position.Text + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddPosition";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Position";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_position.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_coa(object sender, EventArgs e)
    {
        if (txt_coacode.Text != "" || txt_account.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Maccount where accountcode='" + txt_coacode.Text + "' or account='" + txt_account.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselect = dbhelper.getdata("insert into Maccount values ('" + txt_coacode.Text.Replace("'", "").ToString() + "','" + txt_account.Text.Replace("'", "").ToString() + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddCOA";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New COA";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_coacode.Text.Replace("'", "").ToString() + " " + txt_account.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    
    protected void goidetrow(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from orley_empsrequirement where id = '" + seriesid.Value + "'");
            txtupdatereqs.Text = dt.Rows[0]["description"].ToString();
            panelOverlay.Visible = true;
            panelPopUpPanel.Visible = true;
        }
    }
    protected void editreqname(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select id, [description] from orley_empsrequirement where id = '" + seriesid.Value + "'");
        if (txtupdatereqs.Text != "")
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditRequirements";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["description"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtupdatereqs.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update orley_empsrequirement set [description] = '" + txtupdatereqs.Text + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void save_requirements(object sender, EventArgs e)
    {
        if (txtprequirements.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from orley_empsrequirement where description = '" + txtprequirements.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselec = dbhelper.getdata("insert into orley_empsrequirement([description],[status])values('" + txtprequirements.Text.Replace("'", "").ToString() + "','ACTIVE')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddRequirements";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Requirements";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtprequirements.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselec.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_religion(object sender, EventArgs e)
    {
        if (txt_religion.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Mreligion where religion='" + txt_religion.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable select = dbhelper.getdata("insert into Mreligion values ('" + txt_religion.Text + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddRelegion";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Religion";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_religion.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + select.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_citizenship(object sender, EventArgs e)
    {
        if (txt_citizenship.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MCitizenship where Citizenship='" + txt_citizenship.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtinsert = dbhelper.getdata("insert into MCitizenship values ('" + txt_citizenship.Text + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddCitizenship";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Citizenship";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_citizenship.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtinsert.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_zipcode(object sender, EventArgs e)
    {
        if (txt_zipcode.Text != "" || txt_location.Text != "" || txt_Area.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MZipCode where ZipCode='" + txt_zipcode.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtinsert = dbhelper.getdata("insert into MZipCode values ('" + txt_zipcode.Text.Replace("'", "").ToString() + "','" + txt_location.Text.Replace("'", "").ToString() + "','" + txt_Area.Text.Replace("'", "").ToString() + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddZipCode";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New ZipCode";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_zipcode.Text.Replace("'", "").ToString() + " " + txt_location.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtinsert.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_divission(object sender, EventArgs e)
    {
        if (txt_division.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MDivision where division='" + txt_division.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtisner = dbhelper.getdata("insert into MDivision values ('" + txt_division.Text.Replace("'", "").ToString() + "')select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddDivision";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Division";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_division.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtisner.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void hideinternalorder(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MInternalOrder where Id = '" + seriesid.Value + "'");
            txt_internal.Text = dt.Rows[0]["InternalOrder"].ToString();
            pgrid_internalorder.Visible = true;
            pgrid_internalorder2.Visible = true;
        }
    }
    protected void editinternalorder(object sender, EventArgs e)
    {
        if (txt_internal.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MInternalOrder where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateInternal";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["InternalOrder"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_internal.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MInternalOrder set DeptId='" + ddl_compint.SelectedValue + "', InternalOrder='" + txt_internal.Text.Replace("'", "") + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void save_internal(object sender, EventArgs e)
    {
        if (txt_internalorder.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from MInternalOrder where InternalOrder like '%" + txt_internalorder.Text.Replace("'", "") + "%'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dt = dbhelper.getdata("insert into MInternalOrder(DeptId,InternalOrder)values('" + ddl_departinternal.SelectedValue + "','" + txt_internalorder.Text.Replace("'", "") + "')select scope_identity()idd");
                choice();
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddInternalOrder";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Internal Order";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_internalorder.Text.Replace("'", "").ToString();
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
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_section(object sender, EventArgs e)
    {
        if (txt_section.Text != "" || txt_description.Text != "" || ddl_dept.SelectedValue != "0")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Msection where seccode='" + txt_section.Text + "' and status is null and dept_id = '" + ddl_dept.SelectedValue + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselet = dbhelper.getdata("insert into Msection values ('" + txt_section.Text.Replace("'", "").ToString() + "','" + txt_description.Text.Replace("'", "").ToString() + "',NULL,'" + ddl_dept.SelectedValue + "')select scope_identity()idd");
                DataTable select = dbhelper.getdata("select * from msection a left join MDepartment b on a.dept_id = b.Id where sectionid = " + dtselet.Rows[0]["idd"].ToString() + "");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSection";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New DepartmentCode";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_section.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = select.Rows[0]["Department"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_bloodtype(object sender, EventArgs e)
    {
        if (txt_section.Text != "" || txt_description.Text != "" || ddl_dept.SelectedValue != "0")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Msection where seccode='" + txt_section.Text + "' and status is null ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselet = dbhelper.getdata("insert into Msection values ('" + txt_section.Text.Replace("'", "").ToString() + "','" + txt_description.Text.Replace("'", "").ToString() + "',NULL)select scope_identity()idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddSection";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Section";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_section.Text.Replace("'", "").ToString() + " " + txt_description.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselet.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_company(object sender, EventArgs e)
    {
        if (txt_company.Text != "" || txt_address.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from [MCompany] where company='" + txt_company.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselect = dbhelper.getdata("insert into MCompany (Company,Address,SSSNumber,PHICNumber,HDMFNumber,TIN,EntryUserId,EntryDateTime,gdr_roles,ob_roles,zipcode)values ('" + txt_company.Text + "','" + txt_address.Text + "','" + txt_sssnumber.Text + "','" + txt_phicnumber.Text + "','" + txt_hdmfnumber.Text + "','" + txt_tin.Text + "'," + Session["user_id"].ToString() + ",getdate()," + ddl_gdr.SelectedValue + "," + ddl_obr.SelectedValue + "," + ddl_zipcomp.SelectedValue + ")select scope_identity() idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddCompany";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Company";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_company.Text.Replace("'", "").ToString() + " " + txt_address.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_branch(object sender, EventArgs e)
    {
        if (ddl_com.SelectedValue != "0" || txt_branch.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from [MBranch] where branch='" + txt_branch.Text + "' ");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselect = dbhelper.getdata("insert into MBranch values ('" + txt_branch.Text.Replace("'", "").ToString() + "'," + ddl_com.SelectedValue + ")select scope_identity()idd");
                dbhelper.getdata("Insert into MDayTypeDay(DayTypeId,BranchId,Date,DateAfter,DateBefore,ExcludedInFixed,Remarks,WithAbsentInFixed,HolidayId,status) Select distinct a.DayTypeId," + dtselect.Rows[0]["idd"].ToString() + ",a.Date,a.DateAfter,a.DateBefore,a.ExcludedInFixed,a.Remarks,a.WithAbsentInFixed,a.HolidayId,a.status from MDayTypeDay a where YEAR(a.DateAfter)=" + DateTime.Now.ToString("yyyy") + " and YEAR(DateBefore)!='1900' order by a.DateAfter asc");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddBranch";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Branch";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_branch.Text.Replace("'", "").ToString() + ddl_com.SelectedValue.ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }

    protected void save_insurance(object sender, EventArgs e)
    {
        DataTable dtcheck = dbhelper.getdata("select * from Minsurance where insure_name='" + txt_insurance.Text + "'");
        if (dtcheck.Rows.Count == 0)
        {
            dbhelper.getdata("insert into Minsurance values ('" + txt_insurance.Text + "',NULL)");
            choice();
        }
        else
        {
            Response.Write("<script>alert('Data Already Exist!')</script>");
        }
    }

    protected void save_leave(object sender, EventArgs e)
    {
        if (txt_leavetype.Text != "" || txt_leavedescription.Text != "" || txt_leavetotal.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Mleave where Leave='" + txt_leavedescription.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                string aw = "";
                if (check_leave.Checked == true)
                    aw = "yes";
                else
                    aw = "no";

                int attachment = cb_attachment.Checked ? 1 : 0;

                DataTable dtselect = dbhelper.getdata("insert into Mleave (LeaveType,Leave,yearlytotal,converttocash,reqAttachment) values ('" + txt_leavetype.Text + "','" + txt_leavedescription.Text + "', " + txt_leavetotal.Text + ",'" + aw + "'," + attachment + ")select scope_identity()idd");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button ", SqlDbType.VarChar, 50).Value = "AddLeave";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New LeaveType";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_leavetype.Text.Replace("'", "").ToString() + " " + txt_leavedescription.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void save_store(object sender, EventArgs e)
    {
        if (txt_store.Text != "")
        {
            DataTable dtcheck = dbhelper.getdata("select * from Mstore where store='" + txt_store.Text + "'");
            if (dtcheck.Rows.Count == 0)
            {
                DataTable dtselect = dbhelper.getdata("insert into Mstore (store) values ('" + txt_store.Text.Replace("'", "").ToString() + "')select scope_identity() idd");
                choice();
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddStore";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Store";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_store.Text.Replace("'", "").ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dtselect.Rows[0]["idd"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                choice();
            }
            else
            {
                Response.Write("<script>alert('Data Already Exist!')</script>");
            }
        }
    }
    protected void add(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            leave_id.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Mleave where id='" + leave_id.Value + "' ");
            txt_lt.Text = dt.Rows[0]["LeaveType"].ToString();
            txt_desc.Text = dt.Rows[0]["Leave"].ToString();
            txt_yt.Text = dt.Rows[0]["yearlytotal"].ToString();

            if (dt.Rows[0]["converttocash"].ToString() == "yes")
                chk_convertable.Checked = true;
            else
                chk_convertable.Checked = false;

            cb_reqAttachment.Checked = dt.Rows[0]["reqAttachment"].ToString() == "1" ? true : false;

            Div1.Visible = true;
            pAdd.Visible = true;
            pAdd.Style.Add("width", "450px");
        }
    }

    protected void update_leave(object sender, EventArgs e)
    {
        if (txt_desc.Text != "" || txt_lt.Text != "" || txt_yt.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MLeave where Id = '" + leave_id.Value + "'");
            string aw = "";
            if (check_leave.Checked == true)
            {
                aw = "yes";
            }
            else
                aw = "no";

            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateLeaveType";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["LeaveType"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_lt.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateLeaveDesc";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Leave"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_desc.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateLeaveTotal";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["yearlytotal"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_yt.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            int attachment = cb_reqAttachment.Checked ? 1 : 0;
            dbhelper.getdata("update Mleave set reqAttachment=" + attachment + ", LeaveType='" + txt_lt.Text + "',Leave='" + txt_desc.Text + "',yearlytotal='" + txt_yt.Text + "',converttocash='" + aw + "' where id='" + leave_id.Value + "' ");
            choice();
            falls();
        }
    }

    protected void delete_leave(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                leave_id.Value = row.Cells[0].Text;
                dbhelper.getdata("update Mleave set action='1' where id='" + leave_id.Value + "' ");
            }
            choice();
        }
    }
    protected void editstores(object sender, EventArgs e)
    {
        if (txtedstore.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from Mstore where id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateStore";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["store"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtedstore.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update Mstore set store = '" + txtedstore.Text.Replace("'", "").ToString() + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void showupstore(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Mstore where id = '" + seriesid.Value + "'");
            txtedstore.Text = dt.Rows[0]["store"].ToString();
            pgrid_store.Visible = true;
            pgrid_store2.Visible = true;
        }
    }
    
    protected void hidedept(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MDepartment where Id = '" + seriesid.Value + "'");
            txtdepta.Text = dt.Rows[0]["Department"].ToString();
            pgrid_department.Visible = true;
            pgrid_department2.Visible = true;
        }
    }
    protected void editdepartme(object sender, EventArgs e)
    {
        if (txtdepta.Text != "")
        {
            DataTable dt = dbhelper.getdata("select Id, Department from MDepartment where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "upddept";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Department"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtdepta.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MDepartment set Department = '" + txtdepta.Text.Replace("'", "").ToString() + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidepposition(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MPosition where Id = '" + seriesid.Value + "'");
            txtpost.Text = dt.Rows[0]["Position"].ToString();
            pgrid_position.Visible = true;
            pgrid_position2.Visible = true;
        }
    }
    protected void editposits(object sender, EventArgs e)
    {
        if (txtpost.Text != "")
        {
            DataTable dt = dbhelper.getdata("select Id, Position from MPosition where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtpost";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Position"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpost.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MPosition set Position = '" + txtpost.Text.Replace("'", "").ToString() + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidepcoa(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MAccount where Id = '" + seriesid.Value + "'");
            txtcoas.Text = dt.Rows[0]["AccountCode"].ToString();
            txtpsition.Text = dt.Rows[0]["Account"].ToString();
            pgrid_coa.Visible = true;
            pgrid_coa2.Visible = true;
        }
    }
    protected void editcoaxs(object sender, EventArgs e)
    {
        if (txtcoas.Text != "" || txtpsition.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MAccount where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtcoacode";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["AccountCode"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcoas.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtcoaaccnt";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Account"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtpsition.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MAccount set AccountCode = '" + txtcoas.Text.Replace("'", "").ToString() + "', Account = '" + txtpsition.Text.Replace("'", "").ToString() + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hideprelegion(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MReligion where Id = '" + seriesid.Value + "'");
            txtrelegion.Text = dt.Rows[0]["Religion"].ToString();
            pgrid_relegion.Visible = true;
            pgrid_relegion2.Visible = true;
        }
    }
    protected void editreleg(object sender, EventArgs e)
    {
        if (txtrelegion.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MReligion where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtrelg";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Religion"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtrelegion.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MReligion set Religion = '" + txtrelegion.Text.Replace("'", "").ToString() + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidepcitizen(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MCitizenship where Id = '" + seriesid.Value + "'");
            txtcit.Text = dt.Rows[0]["Citizenship"].ToString();
            pgrid_citizenship.Visible = true;
            pgrid_citizenship2.Visible = true;
        }
    }
    protected void editcitising(object sender, EventArgs e)
    {
        if (txtcit.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MCitizenship where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtcitz";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Citizenship"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcit.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MCitizenship set Citizenship = '" + txtcit.Text.Replace("'", "").ToString() + "' where id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hideziping(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MZipCode where Id = '" + seriesid.Value + "'");
            txtzipcode.Text = dt.Rows[0]["ZipCode"].ToString();
            txtziploc.Text = dt.Rows[0]["Location"].ToString();
            txtziparea.Text = dt.Rows[0]["Area"].ToString();
            pgrid_zipcode.Visible = true;
            pgrid_zipcode2.Visible = true;
        }
    }
    protected void editziping(object sender, EventArgs e)
    {
        if (txtzipcode.Text != "" || txtziploc.Text != "" || txtziparea.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MZipCode where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtzipcode";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["ZipCode"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtzipcode.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtzipcodeloc";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Location"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtziploc.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "updtzipcodearea";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Area"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtziparea.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MZipCode set ZipCode = '" + txtzipcode.Text.Replace("'", "").ToString() + "', Location = '" + txtziploc.Text.Replace("'", "").ToString() + "', Area = '" + txtziparea.Text.Replace("'", "").ToString() + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidedivisions(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MDivision where Id = '" + seriesid.Value + "'");
            txtbandlevel.Text = dt.Rows[0]["Division"].ToString();
            pgrid_division.Visible = true;
            pgrid_division2.Visible = true;
        }
    }
    protected void editdivisionings(object sender, EventArgs e)
    {
        if (txtbandlevel.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from MDivision where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditDivision";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Division"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtbandlevel.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MDivision set Division = '" + txtbandlevel.Text.Replace("'", "").ToString() + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidecomps(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MCompany where Id = '" + seriesid.Value + "'");
            txtcomp.Text = dt.Rows[0]["Company"].ToString();
            txtcomadd.Text = dt.Rows[0]["Address"].ToString();
            ddlcompzip.SelectedValue = dt.Rows[0]["zipcode"].ToString();
            txtcsss.Text = dt.Rows[0]["SSSNumber"].ToString();
            txtcphic.Text = dt.Rows[0]["PHICNumber"].ToString();
            txtchdmf.Text = dt.Rows[0]["HDMFNumber"].ToString();
            txtctin.Text = dt.Rows[0]["TIN"].ToString();
            pgrid_company.Visible = true;
            pgrid_company2.Visible = true;
        }
    }
    protected void editcompings(object sender, EventArgs e)
    {
        if (txtcomp.Text != "" || txtcomadd.Text != "")
        {
            DataTable dt = dbhelper.getdata("select Id, Company, Address, SSSNumber, PHICNumber, HDMFNumber, TIN from MCompany where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompany";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Company"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcomp.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompanyAdd";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Address"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcomadd.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompanySSS";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["SSSNumber"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcsss.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompanyPHIC";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["PHICNumber"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtcphic.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompanyHDMF";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["HDMFNumber"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtchdmf.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditCompanyTIN";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["TIN"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtctin.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MCompany set Company = '" + txtcomp.Text.Replace("'", "").ToString() + "', Address = '" + txtcomadd.Text.Replace("'", "").ToString() + "', SSSNumber = '" + txtcsss.Text.Replace("'", "").ToString() + "', PHICNumber = '" + txtcphic.Text.Replace("'", "").ToString() + "', HDMFNumber = '" + txtchdmf.Text.Replace("'", "").ToString() + "', TIN = '" + txtctin.Text.Replace("'", "").ToString() + "',zipcode = '" + ddlcompzip.SelectedValue + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidebranch(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from msection where sectionid = '" + seriesid.Value + "'");
            txtsecs.Text = dt.Rows[0]["seccode"].ToString();
            txtdecs.Text = dt.Rows[0]["sec_desc"].ToString();
            pgrid_branch.Visible = true;
            pgrid_branch2.Visible = true;
        }
    }
    protected void editbrang(object sender, EventArgs e)
    {
        if (txtsecs.Text != "" || txtdecs.Text != "")
        {
            DataTable dt = dbhelper.getdata("select * from msection a left join MDepartment b on a.dept_id = b.Id where sectionid = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateSection";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["seccode"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtsecs.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Department"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "UpdateSectionDesc";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["sec_desc"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtdecs.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Department"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update msection set seccode = '" + txtsecs.Text.Replace("'", "").ToString() + "', sec_desc = '" + txtdecs.Text.Replace("'", "").ToString() + "',dept_id = '" + ddl_deptsec.SelectedValue + "' where sectionid = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void hidelocation(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select Branch from [MBranch] where Id = '" + seriesid.Value + "'");
            txtlocaiton.Text = dt.Rows[0]["Branch"].ToString();
            plocation.Visible = true;
            plocation2.Visible = true;
        }
    }
    protected void editlocing(object sender, EventArgs e)
    {
        if (txtlocaiton.Text != "")
        {
            DataTable dt = dbhelper.getdata("select Id, Branch from [MBranch] where Id = '" + seriesid.Value + "'");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "EditLocation";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Branch"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txtlocaiton.Text.Replace("'", "").ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["Id"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update MBranch set Branch = '" + txtlocaiton.Text.Replace("'", "").ToString() + "' where Id = '" + seriesid.Value + "'");
            choice();
            falls();
        }
    }
    protected void closepopup(object sender, EventArgs e)
    {
        falls();
    }
}
