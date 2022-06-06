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
using System.Configuration;
using System.Data.SqlClient;

public partial class content_Employee_Employee201 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        PageInit();

        if (!IsPostBack)
        {
            getminwage();
            //hdn_empid.Value = Session["emp_id"].ToString();
            loadable();
            allgrid();
            dtdraft();

            if (int.Parse(Session["emp_id"].ToString()) > 0)
                disp();
        }
    }
    protected void PageInit()
    {
        int x = int.Parse(Session["emp_id"].ToString());
        ViewState["action"] = x == 0 ? "New" : "Edit";
        hf_action.Value = x.ToString();
        btn_submit.Text = x == 0 ? "Next" : "Update";
    }
    [WebMethod]
    public static string[] GetEmployees(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.action is null and a.firstname+' '+a.lastname like '%{0}%'", term);
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
    protected void selectchange(object sender, EventArgs e)
    {
        if (sender != null)
        {
            try
            {
                if (((CheckBox)sender).Checked)
                {
                    //delete file uploaded?
                }
            }
            catch { }
        }
    }
    protected void kanakanaas(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chbx = (CheckBox)e.Row.FindControl("cbrequirements");
            if (e.Row.Cells[0].Text == "0")
            {
                chbx.Checked = false;
            }
            else
            {
                chbx.Checked = true;
            }
        }
    }

    protected void clickcancelforms(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_forms.Rows[row.RowIndex].Cells[2].FindControl("lnk_view");
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update file_details set status='cancel' where id=" + lb.CommandName + "");
            }
            else { }
        }
    }

    protected void downloadformfiles(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_viewforms = (LinkButton)grid_forms.Rows[row.RowIndex].Cells[2].FindControl("lnk_viewforms");
            DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewforms.CommandName + " ");
            string classs = dt.Rows[0]["classid"].ToString() == "3" ? "forms" : "peremp";
            string input = Server.MapPath("~/files/" + classs + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename"].ToString();

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
    protected void closepopup(object sender, EventArgs e)
    {
        Response.Redirect("employee-profile?user_id=TIdS9+05Aas=&app_id=" + hdn_empid.Value + "&tp=ed", false);
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }

    protected void getminwage()
    {
        DataTable dtminwage = dbhelper.getdata("select top 1 * from mminimum where status is null order by id desc");
        hdn_minimumwage.Value = dtminwage.Rows[0]["min_amt"].ToString();
    }
    protected void disp()
    {
        btn_identity.Text = "Update";
        btn_personal.Text = "Update";
        btn_payroll.Text = "Update";
        ddl_status.Enabled = false;

        if (Session["emp_id"] != null)
            function.ReadNotification(Session["emp_id"].ToString());

        string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile,*,left(convert(varchar,a.surgepay_effective,101),10)surgepay_effective1, case when (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) is null then a.emp_status  else (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) end   empstatus,left(convert(varchar,a.sbudate,101),10)sbudate1,left(convert(varchar,a.DateHired,101),10)DateHired1,a.allowsc,left(convert(varchar,a.health_card,101),10)health_card1 from memployee a where a.id=" + Session["emp_id"].ToString() + "";
        DataTable dt = dbhelper.getdata(query);

        string compen = "select(SELECT CONVERT(varchar,CONVERT(money,a.mr),1))mr from app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id where a.app_trn_id=(SELECT TOP 1 id From(select Top 2 * from app_trn where empid = " + Session["emp_id"].ToString() + " ORDER BY id DESC)x ORDER BY id) and empid = " + Session["emp_id"].ToString() + "";
        DataTable comp = dbhelper.getdata(compen);
        if (dt.Rows.Count <= 0)
        {
            if (txt_idnumber.Text == "")
            {

            }
        }
        else
        {
            if (comp.Rows.Count > 0)
            {
                txt_cmr.Text = string.Format("{0:n2}", decimal.Parse(comp.Rows[0]["mr"].ToString()));
            }
            profile.Value = dt.Rows[0]["Id"].ToString();
            txt_health.Text = dt.Rows[0]["health_card1"].ToString() == "01/01/1900" ? "" : dt.Rows[0]["health_card1"].ToString();
            ddl_store.SelectedValue = dt.Rows[0]["store_id"].ToString().Length == 0 ? "0" : dt.Rows[0]["store_id"].ToString();
            ViewState["name"] = dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["lastname"].ToString() + " " + dt.Rows[0]["MiddleName"].ToString() + " " + dt.Rows[0]["ExtensionName"].ToString();
            txt_sapnumber.Text = dt.Rows[0]["SAPNumber"].ToString();
            txt_lname.Text = dt.Rows[0]["lastname"].ToString();
            txt_fname.Text = dt.Rows[0]["firstname"].ToString();
            txt_mname.Text = dt.Rows[0]["MiddleName"].ToString();
            txt_exname.Text = dt.Rows[0]["ExtensionName"].ToString();
            ddl_bzc.SelectedValue = dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString();
            DateTime datehired = DateTime.Parse(dt.Rows[0]["DateHired1"].ToString());
            txt_datehired.Text = datehired.ToString("MM/dd/yyyy");
            txt_sbudate.Text = dt.Rows[0]["sbudate1"].ToString() == "01/01/1900" ? "" : dt.Rows[0]["sbudate1"].ToString();
            ddl_zipcode.SelectedValue = dt.Rows[0]["ZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["ZipCodeId"].ToString();
            ddl_bzc.SelectedValue = dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString();
            ddl_section.SelectedValue = dt.Rows[0]["sectionid"].ToString().Length > 0 ? dt.Rows[0]["sectionid"].ToString() : "0";
            ViewState["id"] = txt_idnumber.Text = dt.Rows[0]["idnumber"].ToString();
            ViewState["phone"] = txt_pnumber.Text = dt.Rows[0]["PhoneNumber"].ToString().Length > 0 ? dt.Rows[0]["PhoneNumber"].ToString() : "";
            ViewState["address"] = txt_presentaddres.Text = dt.Rows[0]["Address"].ToString().Length > 0 ? dt.Rows[0]["Address"].ToString() : "";
            ViewState["email"] = txt_email.Text = dt.Rows[0]["EmailAddress"].ToString().Length > 0 ? dt.Rows[0]["EmailAddress"].ToString() : "";
            ViewState["pemail"] = txt_pemail.Text = dt.Rows[0]["personalemail"].ToString().Length > 0 ? dt.Rows[0]["personalemail"].ToString() : "";
            txt_noc.Text = dt.Rows[0]["noc"].ToString();
            ddl_section.SelectedValue = dt.Rows[0]["sectionid"].ToString();
            txt_cnumber.Text = dt.Rows[0]["CellphoneNumber"].ToString();
            txt_dob.Text = DateTime.Parse(dt.Rows[0]["DateOfBirth"].ToString()).ToString("MM/dd/yyyy");
            txt_pob.Text = dt.Rows[0]["PlaceOfBirth"].ToString();
            ddl_sex.Text = dt.Rows[0]["sex"].ToString();
            ddl_cs.SelectedValue = dt.Rows[0]["CivilStatus"].ToString();
            ddl_citizenship.SelectedValue = dt.Rows[0]["CitizenshipId"].ToString();
            ddl_relegion.SelectedValue = dt.Rows[0]["ReligionId"].ToString();
            txt_height.Text = dt.Rows[0]["Height"].ToString();
            txt_weight.Text = dt.Rows[0]["Weight"].ToString();
            ddl_company.SelectedValue = dt.Rows[0]["CompanyId"].ToString();
            query = "select * from MBranch where CompanyId=" + ddl_company.SelectedValue + "";
            DataTable dts = dbhelper.getdata(query);
            ddl_branch.Items.Clear();
            ddl_branch.Items.Add(new ListItem("", "0"));
            txt_cmr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["MonthlyRate"].ToString()));
            foreach (DataRow dr in dts.Rows)
            {
                ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
            }
            ddl_branch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
            ddl_department.SelectedValue = dt.Rows[0]["DepartmentId"].ToString();
            ddl_divission.SelectedValue = dt.Rows[0]["DivisionId"].ToString();
            ddl_divission2.SelectedValue = dt.Rows[0]["DivisionId2"].ToString();
            ddl_lvel.SelectedValue = dt.Rows[0]["level"].ToString();
            ddl_position.SelectedValue = dt.Rows[0]["PositionId"].ToString();
            ViewState["position"] = ddl_position.SelectedItem.ToString();
            ddl_pg.SelectedValue = dt.Rows[0]["PayrollGroupId"].ToString();
            ddl_hdmftype.Text = dt.Rows[0]["HDMFType"].ToString();
            ddl_status.SelectedValue = dt.Rows[0]["empstatus"].ToString();
            if (dt.Rows[0]["SSSIsGrossAmount"].ToString() == "true")
                chk_sssgs.Checked = true;
            else
                chk_sssgs.Checked = false;
            txt_permanentadress.Text = dt.Rows[0]["permanentaddress"].ToString();
            ddl_bloodtype.SelectedValue = dt.Rows[0]["bloodtype"].ToString();
            DataTable nti = dbhelper.getdata("select * from NTI where empid=" + Session["emp_id"].ToString() + " and action is null order by id desc");
            DataTable allowance = dbhelper.getdata("select * from mealallowance where empid=" + Session["emp_id"].ToString() + " and action is null order by id desc");
            string offset = dt.Rows[0]["allowoffset"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowoffset"].ToString();
            string hdoffset = dt.Rows[0]["allowhdoffset"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowhdoffset"].ToString();
            string offsc = dt.Rows[0]["allowsc"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowsc"].ToString();
            if (dt.Rows[0]["empstatus"].ToString() == "4")
                ddl_pg.Enabled = false;
            
            alldisp();
        }
    }
    protected void refresh(object sender, EventArgs e)
    {
        Response.Redirect("addemployee?app_id=0&tp=dd");
    }

    [WebMethod]
    public static string[] GetEmployee(string term, string sender)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query;

            switch (sender)
            {
                case "txt_ser":
                    //query=string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);

                    query = string.Format("select cat_id,Description from  " +
                            "( " +
                            "select convert(varchar,b.id) cat_id,'Category:'+a.description+' Serial:'+b.serial +' Product Description:'+b.description+' Qty:'+ convert(varchar,cast(round(c.qty,0)as numeric(16,0)))+' '+a.um Description " +
                            "from asset_cat a " +
                            "left join asset_inventory b on a.id=b.categoryid " +
                            "left join asset_details c on b.id=c.inventid " +
                            "where a.serialized=1 and b.action is null and c.status='On Stock' " +
                             ") tt where Description like '%{0}%'", term);
                    //"UNION " +
                    //"select convert(varchar,a.id)+':0'cat_id,'Category:'+a.description+' Serial:'+a.description+' Product Description:'+a.description+' Qty:'+convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0))- (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id))+' '+a.um Description " +
                    //"from asset_cat a " +
                    //"where a.serialized=0 " +

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            retCategory.Add(string.Format("{0}-{1}", reader["cat_id"], reader["Description"]));
                        }
                    }
                    con.Close();
                    break;
                case "txt_reportto":
                    query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.action is null and a.firstname+' '+a.lastname like '%{0}%'", term);
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
                    break;
            }
        }
        return retCategory.ToArray();
    }

    protected void click_company(object sender, EventArgs e)
    {
        string query = "select * from MBranch where CompanyId=" + ddl_company.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        ddl_branch.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadable()
    {
        ViewState["approver"] = "0";
        ViewState["leave-credit"] = "0";

        string query = "";

        DataTable dt;

        query = "select * from MCompany order by id desc";
        dt = dbhelper.getdata(query);

        ddl_company.Items.Clear();
        ddl_company.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_company.Items.Add(new ListItem(dr["company"].ToString(), dr["id"].ToString()));
        }
        query = "select ID,zipcode,zipcode+'-'+location+'/'+area as description from MZipCode order by location asc";
        dt = dbhelper.getdata(query);
        ddl_zipcode.Items.Clear();
        ddl_zipcode.Items.Add(new ListItem(" ", "0"));
        ddl_bzc.Items.Clear();
        ddl_bzc.Items.Add(new ListItem("", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_zipcode.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
            ddl_bzc.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MReligion order by id desc";
        dt = dbhelper.getdata(query);
        ddl_relegion.Items.Clear();
        ddl_relegion.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_relegion.Items.Add(new ListItem(dr["Religion"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MDepartment order by id desc";
        dt = dbhelper.getdata(query);
        ddl_department.Items.Clear();
        ddl_department.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_department.Items.Add(new ListItem(dr["department"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPosition order by id desc";
        dt = dbhelper.getdata(query);
        ddl_position.Items.Clear();
        ddl_position.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_position.Items.Add(new ListItem(dr["position"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MDivision order by id";
        dt = dbhelper.getdata(query);
        ddl_divission.Items.Clear();
        ddl_divission.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_divission.Items.Add(new ListItem(dr["division"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MDivision2 order by id";
        dt = dbhelper.getdata(query);
        ddl_divission2.Items.Clear();
        ddl_divission2.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_divission2.Items.Add(new ListItem(dr["division2"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MLevel order by id";
        dt = dbhelper.getdata(query);
        ddl_lvel.Items.Clear();
        ddl_lvel.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_lvel.Items.Add(new ListItem(dr["level"].ToString(), dr["id"].ToString()));
        }
        query = "select * from Msection order by sectionid desc";
        dt = dbhelper.getdata(query);
        ddl_section.Items.Clear();
        ddl_section.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
        }

        query = "select * from MPayrollGroup order by id desc";
        dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Mstore order by id desc";
        dt = dbhelper.getdata(query);
        ddl_store.Items.Clear();
        ddl_store.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_store.Items.Add(new ListItem(dr["store"].ToString(), dr["id"].ToString()));
        }
        query = "select * from Mbloodtype order by id desc";
        dt = dbhelper.getdata(query);
        ddl_bloodtype.Items.Clear();
        ddl_bloodtype.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_bloodtype.Items.Add(new ListItem(dr["bloodtype"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MCitizenship order by id desc";
        dt = dbhelper.getdata(query);
        ddl_citizenship.Items.Clear();
        ddl_citizenship.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_citizenship.Items.Add(new ListItem(dr["Citizenship"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MedHospital where HostStatus = 'Active'";
        DataTable dthost = dbhelper.getdata(query);
        ddl_medical.Items.Clear();
        ddl_medical.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow drhost in dthost.Rows)
        {
            ddl_medical.Items.Add(new ListItem(drhost["HostDesc"].ToString(), drhost["ID"].ToString()));
            ddluphost.Items.Add(new ListItem(drhost["HostDesc"].ToString(), drhost["ID"].ToString()));
        }

        query = "select * from MedIllness where Status = 'ACTIVE'";
        DataTable dtillness = dbhelper.getdata(query);
        ddl_illness.Items.Clear();
        ddl_illness.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow drillness in dtillness.Rows)
        {
            ddl_illness.Items.Add(new ListItem(drillness["illness"].ToString(), drillness["Id"].ToString()));
            ddlupillness.Items.Add(new ListItem(drillness["illness"].ToString(), drillness["Id"].ToString()));
        }

        query = "select * from MCivilStatus";
        DataTable dtcvil = dbhelper.getdata(query);
        ddl_cs.Items.Clear();
        ddl_cs.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow drcvil in dtcvil.Rows)
        {
            ddl_cs.Items.Add(new ListItem(drcvil["CivilStatus"].ToString(), drcvil["Id"].ToString()));
        }

        query = "select * from  mempstatus_setup where action='Default' or action is null order by status";
        dt = dbhelper.getdata(query);
        ddl_status.Items.Clear();
        ddl_status.Items.Add(new ListItem("Probationary", "1"));
        foreach (DataRow drr in dt.Rows)
        {
            ddl_status.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
            ddl_statusfinal.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
        }
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
        else if (ddl_status.SelectedValue.Trim().Length == 0)
        {
            l_msg.Text = "Status must be Required!";
            err = false;
        }
        else
        {
            l_msg.Text = "";
        }

        return err;
    }

    protected void submit(object sender, EventArgs e)
    {
        int action = int.Parse(hf_action.Value);
        int index = 0;
        string ret;
        DataTable dtemps = dbhelper.getdata("select b.Id from MEmployee a left join MUser b on b.emp_id = a.Id where b.emp_id = '" + action + "'");
        switch (index)
        {
            case 0:
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("201identity", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@idnumber", SqlDbType.VarChar, 50).Value = txt_idnumber.Text;
                            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 5000).Value = txt_fname.Text;
                            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 5000).Value = txt_lname.Text;
                            cmd.Parameters.Add("@mname", SqlDbType.VarChar, 5000).Value = txt_mname.Text;
                            cmd.Parameters.Add("@exname", SqlDbType.VarChar, 5000).Value = txt_exname.Text;
                            cmd.Parameters.Add("@gender", SqlDbType.VarChar, 50).Value = ddl_sex.SelectedItem.ToString();
                            cmd.Parameters.Add("@dob", SqlDbType.VarChar, 5000).Value = txt_dob.Text;
                            cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_identity.Text;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"];
                            cmd.Parameters.Add("@employid", SqlDbType.Int).Value = dtemps.Rows[0]["Id"].ToString();
                            cmd.Parameters.Add("@emp_id", SqlDbType.Int).Value = hf_action.Value;
                            cmd.Parameters.Add("@pob", SqlDbType.VarChar, 5000).Value = txt_pob.Text;
                            cmd.Parameters.Add("@pobzipcode", SqlDbType.Int).Value = ddl_bzc.SelectedValue;
                            cmd.Parameters.Add("@preaddress", SqlDbType.VarChar, 5000).Value = txt_presentaddres.Text;
                            cmd.Parameters.Add("@peraddress", SqlDbType.VarChar, 5000).Value = txt_permanentadress.Text;
                            cmd.Parameters.Add("@zipcodeadd", SqlDbType.Int).Value = ddl_zipcode.SelectedValue;
                            cmd.Parameters.Add("@store", SqlDbType.Int).Value = ddl_store.SelectedValue;
                            cmd.Parameters.Add("@cell", SqlDbType.VarChar, 50).Value = txt_cnumber.Text;
                            cmd.Parameters.Add("@phone", SqlDbType.VarChar, 50).Value = txt_pnumber.Text;
                            cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = txt_email.Text;
                            cmd.Parameters.Add("@pemail", SqlDbType.VarChar, 50).Value = txt_pemail.Text;
                            cmd.Parameters.Add("@noc", SqlDbType.VarChar, 50).Value = txt_noc.Text;
                            cmd.Parameters.Add("@ctzen", SqlDbType.Int).Value = ddl_citizenship.SelectedValue;
                            cmd.Parameters.Add("@rel", SqlDbType.Int).Value = ddl_relegion.SelectedValue;
                            cmd.Parameters.Add("@cvil", SqlDbType.Int).Value = ddl_cs.SelectedValue;
                            cmd.Parameters.Add("@hyt", SqlDbType.VarChar, 50).Value = txt_height.Text.Length > 0 ? txt_height.Text : "0";
                            cmd.Parameters.Add("@wyt", SqlDbType.VarChar, 50).Value = txt_weight.Text.Length > 0 ? txt_weight.Text : "0";
                            cmd.Parameters.Add("@bld", SqlDbType.Int).Value = ddl_bloodtype.SelectedValue;
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();

                            switch (ret)
                            {
                                case "Successfully Updated":
                                    break;
                                default:
                                    hdn_empid.Value = ret;
                                    break;
                            }
                        }
                    }

                    if (ret != "exist")
                    {
                        {
                        }

                    
                    }

                break;
        }
        //UPLOAD PROFILE
        if (dataURLInto.InnerText.Length > 0)
        {
            DataTable dt = dbhelper.getdata("select * from file_details where empid=" + hf_action.Value + " and classid=1");
            if (dt.Rows.Count == 0)
                dbhelper.getdata("insert into file_details values (getdate(),0," + hf_action.Value + ",0,1,'files/profile','.png','profile','Active','image/png',0,0,0)");

            string base64 = dataURLInto.InnerText;
            byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
            string path = Server.MapPath("~/files/profile/");
            using (System.IO.FileStream stream = new System.IO.FileStream(path + hf_action.Value + ".png", System.IO.FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            System.Drawing.Image image = System.Drawing.Image.FromFile(path + hf_action.Value + ".png");
            System.Drawing.Image thumb = image.GetThumbnailImage(50, 50, delegate() { return false; }, (IntPtr)0);
            thumb.Save(path + "/" + hf_action.Value + "-thumb.png");
            thumb.Dispose();
        }

        //LOAD PROFILE
        disp();
    }
    protected void next(object sender, EventArgs e)
    {
        //string ret;
        //string filename = "0";
        //string contenttype = "0";
        //string fileconcat = "";
        //if (checkidentity())
        //{
        //    using (SqlConnection con = new SqlConnection(dbconnection.conn))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("emp_identity", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@idnumber", SqlDbType.Int).Value = txt_idnumber.Text;
        //            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 5000).Value = txt_fname.Text;
        //            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 5000).Value = txt_lname.Text;
        //            cmd.Parameters.Add("@mname", SqlDbType.VarChar, 5000).Value = txt_mname.Text;
        //            cmd.Parameters.Add("@exname", SqlDbType.VarChar, 5000).Value = txt_exname.Text;
        //            cmd.Parameters.Add("@datehired", SqlDbType.VarChar, 50).Value = txt_datehired.Text;
        //            cmd.Parameters.Add("@empstatus", SqlDbType.VarChar, 50).Value = ddl_status.SelectedValue;
        //            cmd.Parameters.Add("@companyid", SqlDbType.Int).Value = ddl_company.SelectedValue;
        //            cmd.Parameters.Add("@branchid", SqlDbType.Int).Value = ddl_branch.SelectedValue;
        //            cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = ddl_department.SelectedValue;
        //            cmd.Parameters.Add("@divid", SqlDbType.Int).Value = ddl_divission.SelectedValue;
        //            cmd.Parameters.Add("@secid", SqlDbType.Int).Value = ddl_section.SelectedValue;
        //            cmd.Parameters.Add("@posid", SqlDbType.Int).Value = ddl_position.SelectedValue;
        //            cmd.Parameters.Add("@pgid", SqlDbType.Int).Value = ddl_pg.SelectedValue;
        //            cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_identity.Text;
        //            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
        //            cmd.Parameters.Add("@employid", SqlDbType.Int).Value = hdn_empid.Value;
        //            cmd.Parameters.Add("@sbudate", SqlDbType.VarChar, 50).Value = txt_sbudate.Text;
        //            cmd.Parameters.Add("@store", SqlDbType.Int).Value = ddl_store.SelectedValue;
        //            cmd.Parameters.Add("@health_card", SqlDbType.VarChar, 50).Value = txt_health.Text;
        //            cmd.Parameters.Add("@isminimumwageearner", SqlDbType.VarChar, 50).Value = chk_mwe.Checked == true ? "True" : "False";
        //            cmd.Parameters.Add("@surgepay_effective", SqlDbType.VarChar, 50).Value = txt_surge_effective_date.Text;
        //            cmd.Parameters.Add("@mop", SqlDbType.VarChar, 50).Value = ddl_mop.SelectedItem.Text;
        //            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            ret = cmd.Parameters["@out"].Value.ToString();
        //            con.Close();

        //            dbhelper.getdata("update Memployee set healthcard_status=NULL where id=" + hdn_empid.Value + " ");
        //        }
        //    }
        //    if (ret != "exist" && ret.Length > 0)
        //    {
        //        //tb_tab.Text = "2";
        //        if (btn_identity.Text == "Update")
        //        {
        //            l_msg.Text = ret;
        //        }
        //        else
        //        {
        //            hdn_empid.Value = ret.ToString();
        //            using (SqlConnection con = new SqlConnection(dbconnection.conn))
        //            {
        //                filename = Server.MapPath("~/files/peremp/" + hdn_empid.Value + "/Employee_status");
        //                using (SqlCommand cmd = new SqlCommand("process_mempstatus", con))
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    fileconcat = ddl_status.Text;
        //                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
        //                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
        //                    cmd.Parameters.Add("@statusid", SqlDbType.Int).Value = ddl_status.SelectedValue;
        //                    cmd.Parameters.Add("@effectivedate", SqlDbType.VarChar, 5000).Value = DateTime.Now.ToShortDateString();//effdate[1] + "/" + effdate[0] + "/" + effdate[2];
        //                    cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = "setup";
        //                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    ret = cmd.Parameters["@out"].Value.ToString();
        //                    con.Close();
        //                }
        //                HttpFileCollection uploadedFiles = Request.Files;
        //                for (int i = 0; i < uploadedFiles.Count; i++)
        //                {
        //                    HttpPostedFile userPostedFile = uploadedFiles[i];
        //                    string[] fileName0 = userPostedFile.FileName.Split('.');
        //                    contenttype = userPostedFile.ContentType.ToString();
        //                    using (SqlCommand cmd = new SqlCommand("process_memployeestatusfile", con))
        //                    {
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add("@empstatid", SqlDbType.Int).Value = ret;
        //                        cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
        //                        cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileName0[1];
        //                        cmd.Parameters.Add("@filename", SqlDbType.VarChar, 5000).Value = fileName0[0];
        //                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //                        con.Open();
        //                        cmd.ExecuteNonQuery();
        //                        string statusfile_ret = cmd.Parameters["@out"].Value.ToString();
        //                        con.Close();
        //                        if (!Directory.Exists(filename))
        //                            Directory.CreateDirectory(filename);
        //                        userPostedFile.SaveAs(filename + "\\" + fileName0[0].Replace(" ", "") + "_" + statusfile_ret + "." + fileName0[1].ToString());
        //                    }
        //                }
        //            }
        //            l_msg.Text = "";
        //            txt_idnumber.Enabled = false;
        //        }
        //    }
        //    else
        //    {
        //        l_msg.Text = "ID NUMBER is already Exist!";
        //        hdn_empid.Value = "";
        //    }
        //}

    }
    protected bool checkidentity()
    {
        bool ret = true;
        if (txt_idnumber.Text.Length == 0)
        {
            l_msg.Text = "Id Number must be supplied!";
            ret = false;
        }
        else if (ddl_company.SelectedValue == "0")
        {
            l_msg.Text = "Company must be supplied!";
            ret = false;
        }
        else if (ddl_pg.SelectedValue == "0")
        {
            l_msg.Text = "Payroll Group must be supplied!";
            ret = false;
        }
        else if (txt_fname.Text.Length == 0)
        {
            l_msg.Text = "First Name must be supplied!";
            ret = false;
        }
        else if (txt_lname.Text.Length == 0)
        {
            l_msg.Text = "Last Name must be supplied!";
            ret = false;
        }
        else if (txt_datehired.Text.Length == 0)
        {
            l_msg.Text = "Date Hired must be supplied!";
            ret = false;
        }
        else if (ddl_status.SelectedValue == "0")
        {
            l_msg.Text = "Employee Status must be supplied!";
            ret = false;
        }
 

        alert.Visible = ret == false ? true : false;

        return ret;
    }
    protected void next1(object sender, EventArgs e)
    {
    }
    protected void next2(object sender, EventArgs e)
    {
    }
    protected bool checkpayrollinfo()
    {
        bool ret = true;
        alert.Visible = ret == false ? true : false;
        return ret;
    }
    protected void allgrid()
    {
        grid_skill1.DataSource = null;
        grid_skill1.DataBind();

        grid_jobhistory1.DataSource = null;
        grid_jobhistory1.DataBind();

        grid_educhistory1.DataSource = null;
        grid_educhistory1.DataBind();

        grid_fmember1.DataSource = null;
        grid_fmember1.DataBind();

        txt_year.Items.Clear();
        txt_datetoyear.Items.Clear();
        txt_yearf.Items.Clear();
        txt_yeart.Items.Clear();
        txt_year.Items.Add(new ListItem("Year", "0"));
        txt_datetoyear.Items.Add(new ListItem("Year", "0"));
        txt_yearf.Items.Add(new ListItem("Year", "0"));
        txt_yeart.Items.Add(new ListItem("Year", "0"));
        for (int i = 1900; i <= DateTime.Now.Year + 1; i++)
        {
            string hh = i.ToString().Length > 1 ? i.ToString() : "0" + i.ToString();
            txt_year.Items.Add(new ListItem(hh, hh));
            txt_datetoyear.Items.Add(new ListItem(hh, hh));
            txt_yearf.Items.Add(new ListItem(hh, hh));
            txt_yeart.Items.Add(new ListItem(hh, hh));
        }
        DataTable dtinsurance = dbhelper.getdata("select * from Minsurance where action is null");
    }
    protected void addss(object sender, EventArgs e)
    {
        DataTable dtskilline = new DataTable();
        if (txt_skill.Text.Length == 0)
            l_msg.Text = "Skill must be supplied!";
        else
        {
            if (int.Parse(Session["emp_id"].ToString()) > 0)
            {
                dbhelper.getdata("insert into Mskilline (date,empid,userid,skill)values(getdate()," + hf_action.Value + "," + hf_action.Value + ",'" + txt_skill.Text + "')");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        backlag();
    }
    protected void openskills(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Mskilline where id = " + seriesid.Value + "");
            txtupskill.Text = dt.Rows[0]["skill"].ToString();
            pgridskills.Visible = true;
            pgridskills2.Visible = true;
        }
    }
    protected void updatemskills(object sender, EventArgs e)
    {
        dbhelper.getdata("update Mskilline set skill='" + txtupskill.Text + "' where id = " + seriesid.Value + "");
        backlag();
    }
    protected void deleteskill(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update mskilline set status='cancel' where id=" + row.Cells[0].Text + "");
            }
        }
        backlag();
    }
    protected void addemergencycontact(object sender, EventArgs e)
    {
        if (txtnameemergency.Text == "" && txtaddressemergency.Text == "" && txtcontactemergency.Text == "")
        {
        }
        else
        {
            dbhelper.getdata("insert into EmergencyContact (UserId,EmpId,Name,Address,Contact,Sysdate) values (" + hf_action.Value + "," + hf_action.Value + ",'" + txtnameemergency.Text + "','" + txtaddressemergency.Text + "','" + txtcontactemergency.Text + "',GETDATE())");
        }
        backlag();
    }
    protected void openemcontact(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from EmergencyContact where id = " + seriesid.Value + "");
            txtupname.Text = dt.Rows[0]["Name"].ToString();
            txtupconadd.Text = dt.Rows[0]["Address"].ToString();
            txtupconnum.Text = dt.Rows[0]["Contact"].ToString();
            pgridcontact.Visible = true;
            pgridcontact1.Visible = true;
        }
    }
    protected void updatemcontact(object sender, EventArgs e)
    {
        dbhelper.getdata("update EmergencyContact set Name='" + txtupname.Text + "',Address='" + txtupconadd.Text.Replace("'", "") + "',Contact='" + txtupconnum.Text + "' where Id = " + seriesid.Value + "");
        backlag();
    }
    protected void deleteContact(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = row.Cells[0].Text;
                dbhelper.getdata("update EmergencyContact set Status = 'Cancel' where Id = '" + id + "'");
            }
        }
        backlag();
    }

    protected void addillness(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MedIllness where Illness like '%" + txtillness.Text + "%' and Status = 'ACTIVE'");
        if (txtillness.Text == "")
        { }
        else if (dt.Rows.Count <= 0)
        {
            DataTable dtselect = dbhelper.getdata("insert into MedIllness values ('" + txtillness.Text.Replace("'", "").ToString() + "','ACTIVE')select scope_identity()idd");
        }
        backlag();
    }
    protected void savehost(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MedHospital where HostAddress like '%" + txthostadd.Text + "%' and HostDesc like '%" + txthostdesc.Text + "%'");
        if (txthostdesc.Text == "" && txthostadd.Text == "")
        { }
        else if (dt.Rows.Count <= 0)
        {
            DataTable dtselect = dbhelper.getdata("insert into MedHospital values ('" + txthostdesc.Text.Replace("'", "").ToString() + "','" + txthostadd.Text.Replace("'", "").ToString() + "','" + txthostcont.Text.Replace("'", "").ToString() + "','Active')");
        }
        backlag();
    }

    protected void savemedrec(object sender, EventArgs e)
    {
        if (ddl_medical.SelectedValue != "0" && ddl_illness.SelectedValue != "0" && ddlcondition.SelectedValue != "" && ddlfindings.SelectedValue != "")
        {
            if (fuhostdoc.HasFile)
            {
                string filename = Path.GetFileNameWithoutExtension(fuhostdoc.PostedFile.FileName);
                string fileExtension = Path.GetExtension(fuhostdoc.PostedFile.FileName);
                string contenttype = fuhostdoc.PostedFile.ContentType;
                string nameforfile = hf_action.Value;

                DataTable dtmedrecords = dbhelper.getdata("insert into MedRecords values('" + hf_action.Value + "','" + hf_action.Value + "','" + ddl_illness.SelectedValue + "',GETDATE(),'" + txt_meddate.Text + "','" + ddl_medical.SelectedValue + "','" + txt_medphysician.Text + "','" + ddlfindings.SelectedItem + "','" + txt_mednote.Text + "','" + ddlcondition.SelectedItem + "','Has File','Active')select scope_identity() illid");

                DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/medicalfile/") + nameforfile);
                DataTable dtinsertirfile = dbhelper.getdata("insert into MedFile values(GETDATE(),'" + hf_action.Value + "','" + hf_action.Value + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + dtmedrecords.Rows[0]["illid"].ToString() + "') select scope_identity() idd");
                fuhostdoc.SaveAs(Server.MapPath("~/files/medicalfile/") + nameforfile + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + filename + fileExtension);
            }
            else
            {
                DataTable seletx = dbhelper.getdata("insert into MedRecords values('" + hf_action.Value + "','" + hf_action.Value + "','" + ddl_illness.SelectedValue + "',GETDATE(),'" + txt_meddate.Text + "','" + ddl_medical.SelectedValue + "','" + txt_medphysician.Text + "','" + ddlfindings.SelectedItem + "','" + txt_mednote.Text + "','" + ddlcondition.SelectedItem + "','No File','Active')select scope_identity() illid");
            }
            backlag();
        }
    }
    protected void openemmedrec(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from MedRecords where id = " + seriesid.Value + "");
            txtupdate.Text = dt.Rows[0]["meddate"].ToString();
            ddlupillness.SelectedValue = dt.Rows[0]["saidid"].ToString();
            ddluphost.SelectedValue = dt.Rows[0]["hospital"].ToString();
            txtuphys.Text = dt.Rows[0]["doctor"].ToString();
            pgridmedrec.Visible = true;
            pgridmedrec1.Visible = true;
        }
    }
    protected void updatemmedrecs(object sender, EventArgs e)
    {
        dbhelper.getdata("update MedRecords set saidid='" + ddlupillness.SelectedValue + "',hospital='" + ddluphost.SelectedValue + "',meddate='" + txtupdate.Text + "',doctor='" + txtuphys.Text.Replace("'", "") + "' where id = " + seriesid.Value + "");
        backlag();
    }
    protected void rmvmedrecord(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update MedRecords set [status] = 'Cancel' where ID = '" + row.Cells[0].Text + "'");
            }
        }
        backlag();
    }
    protected void medrecscell(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton draft = (LinkButton)e.Row.FindControl("lbdwnmedrecs");
            DataTable dt = dbhelper.getdata("select * from MedRecords where status = 'Active' and content = 'Has File' and Id = " + draft.CommandName + "");
            if (dt.Rows.Count > 0)
            {
                draft.Visible = true;
            }
            else
                draft.Visible = false;
        }
    }
    protected void dwnmedrecs(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnkdownmedrecs = (LinkButton)gridmedrics.Rows[row.RowIndex].Cells[7].FindControl("lbdwnmedrecs");
            DataTable dts = dbhelper.getdata("select * from MedRecords where id = '" + lnkdownmedrecs.CommandName + "' and content like '%Has File%'");
            if (dts.Rows.Count > 0)
            {
                string folderdir = hf_action.Value;
                DataTable dt = dbhelper.getdata("select * from MedFile where saidid =" + lnkdownmedrecs.CommandName + " ");
                string input = Server.MapPath("~/files/medicalfile/" + folderdir + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename"].ToString().Replace(" ", "") + dt.Rows[0]["extension"].ToString();

                //Download the Decrypted File.
                Response.Clear();
                Response.ContentType = dt.Rows[0]["content"].ToString();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
                Response.WriteFile(input);
                Response.Flush();
                Response.End();
            }
            else
                Response.Write("<script>alert('No File Content!')</script>");
        }
    }
    protected void add_jobhistory(object sender, EventArgs e)
    {
        DataTable dtjobhistory = new DataTable();
        if (checkjobhistory())
        {
            if (int.Parse(Session["emp_id"].ToString()) > 0)
            {
                dbhelper.getdata("insert into Mjobhistory (date,empid,userid,position,company,datef,datet)values(getdate()," + hf_action.Value + "," + hf_action.Value + ",'" + txt_position.Text + "','" + txt_company.Text + "','" + txt_month.Text + "/01/" + txt_year.Text + "','" + txt_datetomonth.Text + "/01/" + txt_datetoyear.Text + "')");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        backlag();
    }
    protected void openjobhist(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Mjobhistory where id = " + seriesid.Value + "");
            txtupposition.Text = dt.Rows[0]["position"].ToString();
            txtupcompany.Text = dt.Rows[0]["company"].ToString();
            txtupfromyear.Text = dt.Rows[0]["datef"].ToString();
            txtuptoyear.Text = dt.Rows[0]["datet"].ToString();
            pgridjobhist.Visible = true;
            pgridjobhist1.Visible = true;
        }
    }
    protected void updatemjobhist(object sender, EventArgs e)
    {
        dbhelper.getdata("update Mjobhistory set position='" + txtupposition.Text.Replace("'", "") + "',company='" + txtupcompany.Text.Replace("'", "") + "',datef='" + txtupfromyear.Text + "',datet='" + txtuptoyear.Text + "' where id = " + seriesid.Value + "");
        backlag();
    }
    protected void delete_jobhistory(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update Mjobhistory set status='cancel' where id=" + row.Cells[0].Text + "");
            }
        }
        backlag();
    }
    protected bool checkjobhistory()
    {
        bool ret = true;
        if (txt_position.Text.Length == 0)
        {
            l_msg.Text = "Position must be supplied!";
            ret = false;
        }
        else if (txt_company.Text.Length == 0)
        {
            l_msg.Text = "Company must be supplied!";
            ret = false;
        }
        else if (txt_month.Text.Length == 0)
        {
            l_msg.Text = "Date From must be supplied!";
            ret = false;
        }
        else if (txt_year.Text.Length == 0)
        {
            l_msg.Text = "Date From must be supplied!";
            ret = false;
        }
        else if (txt_datetomonth.Text.Length == 0)
        {
            l_msg.Text = "Date To must be supplied!";
            ret = false;
        }
        else if (txt_datetoyear.Text.Length == 0)
        {
            l_msg.Text = "Date To must be supplied!";
            ret = false;
        }
        return ret;
    }
    protected void edhistory(object sender, EventArgs e)
    {
        DataTable dteduchistory = new DataTable();
        if (txt_school.Text.Length == 0 || txt_address.Text.Length == 0 || txt_yearf.Text.Length == 0 || txt_yeart.Text.Length == 0)
            l_msg.Text = "Invalid Input!";
        else
        {
            if (int.Parse(Session["emp_id"].ToString()) > 0)
            {
                dbhelper.getdata("insert into Meducattainment(date,empid,userid,class,school,address,yearf,yeart)values(getdate()," + hf_action.Value + "," + hf_action.Value + ",'" + ddl_level.Text + "','" + txt_school.Text.Replace("'", "") + "','" + txt_address.Text.Replace("'", "") + "','" + txt_yearf.Text + "','" + txt_yeart.Text + "') ");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        backlag();
    }
    protected void openattainement(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Meducattainment where id = " + seriesid.Value + "");
            ddllevel.SelectedItem.Text = dt.Rows[0]["class"].ToString();
            txtupschool.Text = dt.Rows[0]["school"].ToString();
            txtupaddress.Text = dt.Rows[0]["address"].ToString();
            txtupfrom.Text = dt.Rows[0]["yearf"].ToString();
            txtupto.Text = dt.Rows[0]["yeart"].ToString();
            pgridattainment.Visible = true;
            pgridattainment1.Visible = true;
        }
    }
    protected void updatemattainment(object sender, EventArgs e)
    {
        dbhelper.getdata("update Meducattainment set class='" + ddllevel.SelectedItem + "',school='" + txtupschool.Text.Replace("'", "") + "',address='" + txtupaddress.Text.Replace("'", "") + "',yearf='" + txtupfrom.Text + "',yeart='" + txtupto.Text + "' where id=" + seriesid.Value + "");
        backlag();
    }
    protected void delete_edhistory(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update Meducattainment set status='cancel' where id=" + row.Cells[0].Text + "");
            }
        }
        backlag();
    }

    protected void fmember(object sender, EventArgs e)
    {
        DataTable dtfmem = new DataTable();
        if (txt_fmem_firstname.Text.Length == 0 || txt_fmem_lname.Text.Length == 0 || txt_fmem_mname.Text.Length == 0 || txt_fmem_relation.Text.Length == 0)
            l_msg.Text = "Invalid Input!";
        else
        {
            if (hdn_empid.Value.Length > 0)
            {
                dbhelper.getdata("insert into MFmembers (date,empid,userid,firstname,lastname,middlename,extensionname,relation)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + txt_fmem_firstname.Text + "','" + txt_fmem_lname.Text + "','" + txt_fmem_mname.Text + "','" + txt_fmem_ename.Text + "','" + txt_fmem_relation.Text + "') ");
                dtfmem = dbhelper.getdata("select * from MFmembers a where a.empid =" + hdn_empid.Value + " and a.status is null");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        grid_fmember1.DataSource = dtfmem;
        grid_fmember1.DataBind();

    }
    protected void delete_fmember(object sender, EventArgs e)
    {
        DataTable dtmember = new DataTable();
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update MFmembers set status='cancel' where id=" + row.Cells[0].Text + "");
        }
        backlag();
    }
    public static int GetMonthsDiff(DateTime start, DateTime end)
    {
        if (start > end)
            return GetMonthsDiff(end, start);

        int months = 0;
        do
        {
            start = start.AddMonths(1);
            if (start > end)
                return months;

            months++;
        }
        while (true);
    }

    protected void close(object sender, EventArgs e)
    {
        panelOverlay.Visible = false;
        div_status.Visible = false;
        div_releasing.Visible = false;
    }
    protected void change_status(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
        div_status.Visible = true;
        div_status.Style.Add("width", "415px");
        div_status.Style.Add("top", "30px");
        div_status.Style.Add("left", "450px");
        ddl_statusfinal.Enabled = true;
        span_id.Visible = true;
        txt_effdate.Visible = true;
        ddl_statusfinal.SelectedValue = ddl_status.SelectedValue;
        lbl_header.Text = "Change Status";
        hdn_proc.Value = "process_mempstatus";
        DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Session["emp_id"].ToString() + " and action is null");
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
            div_status.Style.Add("width", "415px");
            div_status.Style.Add("top", "30px");
            div_status.Style.Add("left", "450px");
            ddl_statusfinal.SelectedValue = ddl_status.SelectedValue;
            ddl_statusfinal.Enabled = false;
            span_id.Visible = false;
            txt_effdate.Visible = false;
            lbl_header.Text = "Quit Claim Request";
            hdn_proc.Value = "quit_claim";
            DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Session["emp_id"].ToString() + " and action is null");
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
            string contenttype = "0";
            string fileconcat = "";
            if (FileUpload2.HasFile)
            {
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    switch (hdn_proc.Value)
                    {
                        case "process_mempstatus":
                            filename = Server.MapPath("~/files/peremp/" + Session["emp_id"].ToString() + "/Employee_status");
                            using (SqlCommand cmd = new SqlCommand("process_mempstatus", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                string[] effdate = txt_effdate.Text.Split('/');
                                fileconcat = ddl_statusfinal.SelectedItem.Text;
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
                                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                                cmd.Parameters.Add("@statusid", SqlDbType.Int).Value = ddl_statusfinal.SelectedValue;
                                cmd.Parameters.Add("@effectivedate", SqlDbType.VarChar, 5000).Value = effdate[0] + "/" + effdate[1] + "/" + effdate[2];
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
                                    cmd.Parameters.Add("@filename", SqlDbType.VarChar, 5000).Value = fileName0[0];
                                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    string statusfile_ret = cmd.Parameters["@out"].Value.ToString();
                                    con.Close();
                                    if (!Directory.Exists(filename))
                                        Directory.CreateDirectory(filename);
                                    userPostedFile.SaveAs(filename + "\\" + fileName0[0].Replace(" ", "") + "_" + statusfile_ret + "." + fileName0[1].ToString());

                                }
                            }

                            break;
                        case "quit_claim":
                            filename = Server.MapPath("~/files/peremp/" + Session["emp_id"].ToString() + "/Clearance");
                            using (SqlCommand cmd = new SqlCommand("quit_claim", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                fileconcat = "Clearance";
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
                                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
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
                Response.Redirect("addemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Session["emp_id"].ToString() + "&tp=ed");

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
        if (!FileUpload2.HasFile)
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
        DataTable dt = dbhelper.getdata("select * from quit_claim_request where empid=" + Session["emp_id"].ToString() + " and action is null and (status='For Released' or status='Released')");
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
            string filename = Server.MapPath("~/files/peremp/" + Session["emp_id"].ToString() + "/QCReleasing");
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
        Response.Redirect("editemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Session["emp_id"].ToString() + "");
    }
    protected void gen2316(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
    }
    protected void alldisp()
    {
        DataTable dt = new DataTable();

        dt = dbhelper.getdata("select TOP 1 (SELECT CONVERT(varchar,CONVERT(money,a.mr),1))mr,a.id,a.app_trn_id,c.PayrollType,a.fnod,a.fnoh," +
            "(SELECT CONVERT(varchar,CONVERT(money,a.pr),1))pr,(SELECT CONVERT(varchar,CONVERT(money,a.dr),1))dr," +
            "(SELECT CONVERT(varchar,CONVERT(money,a.hr),1))hr,case when Year(LEFT(CONVERT(varchar,a.effective_date,101),10))='1900'then " +
            "LEFT(CONVERT(varchar,d.DateHired,101),10)else LEFT(CONVERT(varchar,a.effective_date,101),10)end effective_date,(SELECT CONVERT(varchar,CONVERT(money,a.lastsal),1))ls from " +
            "app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id left join MPayrollType c on a.paytypeid=c.Id left join MEmployee d " +
            "on d.Id=b.empid where b.empid=" + Session["emp_id"].ToString() + " order by a.id desc");
        if (dt.Rows.Count > 0)
        {
            txt_cmr.Text = dt.Rows[0]["mr"].ToString();
        }
        else
        {
            dt = dbhelper.getdata("select MonthlyRate from MEmployee where Id=" + Session["emp_id"].ToString() + "");
            txt_cmr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["MonthlyRate"].ToString()));
        }

        dt = dbhelper.getdata("select TOP 2 (SELECT CONVERT(varchar,CONVERT(money,a.mr),1))mr,a.id,a.app_trn_id,c.PayrollType,a.fnod,a.fnoh," +
            "(SELECT CONVERT(varchar,CONVERT(money,a.pr),1))pr,(SELECT CONVERT(varchar,CONVERT(money,a.dr),1))dr," +
            "(SELECT CONVERT(varchar,CONVERT(money,a.hr),1))hr,case when Year(LEFT(CONVERT(varchar,a.effective_date,101),10))='1900'then " +
            "LEFT(CONVERT(varchar,d.DateHired,101),10)else LEFT(CONVERT(varchar,a.effective_date,101),10)end effective_date,(SELECT CONVERT(varchar,CONVERT(money,a.lastsal),1))ls from " +
            "app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id left join MPayrollType c on a.paytypeid=c.Id left join MEmployee d " +
            "on d.Id=b.empid where b.empid=" + Session["emp_id"].ToString() + " order by a.id desc");
        
        if (dt.Rows.Count > 1)
        {
            txt_pmr.Text = dt.Rows[1]["mr"].ToString();
            txt_chde.Text = dt.Rows[0]["effective_date"].ToString();
        }
        else if (dt.Rows.Count > 0)
        {
            txt_pmr.Text = dt.Rows[0]["ls"].ToString();
            txt_chde.Text = dt.Rows[0]["effective_date"].ToString();
        }
        else
            txt_pmr.Text = "0.00";

        dt = dbhelper.getdata("select *,class level from Meducattainment where empid =" + Session["emp_id"].ToString() + " and status is null ");
        grid_educhistory1.DataSource = dt;
        grid_educhistory1.DataBind();

        dt = dbhelper.getdata("select *,convert(varchar,month(datef))+'/'+ convert(varchar,year(datef))froms, convert(varchar,month(datet))+'/'+ convert(varchar,year(datet))tos from Mjobhistory where empid =" + Session["emp_id"].ToString() + " and status is null");
        grid_jobhistory1.DataSource = dt;
        grid_jobhistory1.DataBind();

        dt = dbhelper.getdata("select * from Mskilline where empid =" + Session["emp_id"].ToString() + " and status is null");
        grid_skill1.DataSource = dt;
        grid_skill1.DataBind();

        dt = dbhelper.getdata("select  a.id, b.id fileid, a.seminarsattended, a.seminarsheld,LEFT(convert(varchar, dateconducted,101),10) as dateconducted from Mseminarattended a left join MseminarattndFile b on a.id = b.said  where a.empid =" + Session["emp_id"].ToString() + " and a.status is null");
        grid_seminarsattended1.DataSource = dt;
        grid_seminarsattended1.DataBind();

        dt = dbhelper.getdata("select * from EmergencyContact where EmpId = '" + Session["emp_id"] + "' and Status is NULL");
        grid_emergencycontact.DataSource = dt;
        grid_emergencycontact.DataBind();

        dt = dbhelper.getdata("select a.Id,(select illness from MedIllness where Id = a.saidid)illness,(select HostDesc from MedHospital where Id = a.hospital)hospital,LEFT(convert(varchar, a.meddate,101),10)meddate, a.doctor, a.findings, a.condition from MedRecords a where  a.status = 'Active' and a.empid= " + hf_action.Value + " order by sysdate desc");
        gridmedrics.DataSource = dt;
        gridmedrics.DataBind();

        dt = dbhelper.getdata("select a.Id,(select LastName+', '+FirstName from MEmployee where Id = a.EmpId)Employee,a.LicenseNum,Left(convert(varchar,a.ValidUntil,101),10)ValidUntil,a.Location,a.Cost,Left(convert(varchar,a.DurationFrom,101),10)DF,Left(convert(varchar,a.DurationTo,101),10)DT,a.Provider,a.About,a.Resources,Left(convert(varchar,a.Sysdate,101),10)Sysdate from Preparatory a where a.EmpId = '" + Session["emp_id"] + "' and a.Action is NULL");
        grid_preparatory.DataSource = dt;
        grid_preparatory.DataBind();

        DataTable empdet = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
    }
    protected void addseminarattended(object sender, EventArgs e)
    {
        string filename = Path.GetFileNameWithoutExtension(fuseminarsphoto.PostedFile.FileName);
        string fileExtension = Path.GetExtension(fuseminarsphoto.PostedFile.FileName);
        string contenttype = fuseminarsphoto.PostedFile.ContentType;

        DataTable dtaddseminar = new DataTable();
        if (txt_seminar.Text.Length == 0 || txt_heldat.Text.Length == 0 || txt_dateseminars.Text.Length == 0)
            l_msg.Text = "Attended Seminars must be supplied!";
        else
        {

            DataTable dtinsert = dbhelper.getdata("insert into Mseminarattended (date,empid,userid,seminarsattended,dateconducted,status,seminarsheld,fileattached) values ('" + DateTime.Now.ToString() + "','" + hf_action.Value + "','" + hf_action.Value + "','" + txt_seminar.Text.Replace(" ", " ") + "','" + txt_dateseminars.Text + "',NULL,'" + txt_heldat.Text.Trim() + "','Without Attachment') select scope_identity() id");
            if (fuseminarsphoto.HasFile)
            {
                string nameofiles = hf_action.Value;
                DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/seminarfiles/") + nameofiles);
                string query = "insert into MseminarattndFile(dateupload, empid, userid, location, extension, status, contenttype, filename, action,said) values ('" + DateTime.Now.ToString() + "','" + hf_action.Value + "','" + hf_action.Value + "','c','" + fileExtension + "','Confirmed Uploaded','" + contenttype + "','" + filename + "','Active'," + dtinsert.Rows[0]["id"].ToString() + ") select scope_identity() id";
                DataTable dt = dbhelper.getdata(query);
                fuseminarsphoto.SaveAs(Server.MapPath("~/files/seminarfiles/") + nameofiles + "/" + dt.Rows[0]["id"].ToString() + fileExtension);
            }
        }
        backlag();
    }
    protected void openseminar(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Mseminarattended where id = " + seriesid.Value + "");
            txtupseminaratt.Text = dt.Rows[0]["seminarsattended"].ToString();
            txtupheld.Text = dt.Rows[0]["seminarsheld"].ToString();
            txtupconducted.Text = dt.Rows[0]["dateconducted"].ToString();
            pgridseminar.Visible = true;
            pgridseminar1.Visible = true;
        }
    }
    protected void updatemseminar(object sender, EventArgs e)
    {
        dbhelper.getdata("update Mseminarattended set seminarsattended='" + txtupseminaratt.Text.Replace("'", "") + "',seminarsheld='" + txtupheld.Text.Replace("'", "") + "',dateconducted='" + txtupconducted.Text + "' where id=" + seriesid.Value + "");
        backlag();
    }
    protected void deleteseminarsattended(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update Mseminarattended set status='cancel',lastupdateuserid=" + hf_action.Value + ",lastupdatedate=getdate() where id=" + row.Cells[0].Text + "");
                dbhelper.getdata("update MseminarattndFile set action='Deleted' where said=" + row.Cells[0].Text + "");
            }
        }
        backlag();
    }

    protected void saDatabound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton downloadni = (LinkButton)e.Row.FindControl("lbdownloadsem");
            DataTable dtt = dbhelper.getdata("select * from MseminarattndFile where said=" + downloadni.CommandName + "");
            if (dtt.Rows.Count > 0)
                downloadni.Visible = true;
            else
                downloadni.Visible = false;
        }
    }

    protected void downloadseminarsfile(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton downloadni = (LinkButton)grid_seminarsattended1.Rows[row.RowIndex].Cells[5].FindControl("lbdownloadsem");
            DataTable dt = dbhelper.getdata("select * from MseminarattndFile where said = " + downloadni.CommandName + "");
            string fileout = Server.MapPath("~/files/seminarfiles/" + hf_action.Value + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileout));
            Response.WriteFile(fileout);
            Response.Flush();
            Response.End();
        }
    }

    protected void chngestatus(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string id = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from sanctionentry where id = '" + id + "'");
            Response.Redirect("Memo?empid=" + hdn_empid.Value + "&entid=" + id + "", false);
        }
    }

    //butyok
    protected void closeupload(object sender, EventArgs e)
    {
        overlay.Visible = false;
        invetPanel.Visible = false;
        suspendpanel.Visible = false;
    }

    protected void dtdraft()
    {
        DataTable ndt = new DataTable();
        ndt.Columns.Add(new DataColumn("suspendate", typeof(string)));
        ViewState["dtdraft"] = ndt;
    }


    //butyok
    protected void tawesie(object seder, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton draft = (LinkButton)e.Row.FindControl("lbupload");
            LinkButton stat = (LinkButton)e.Row.FindControl("itemstatus");
            if (stat.Text == "Verified" || stat.Text == "For Clearing")
            {
                stat.Enabled = false;
            }
            else if (stat.Text == "Draft")
                draft.Visible = false;
            LinkButton lbshowdownload = (LinkButton)e.Row.FindControl("lbdonwloadir");
            DataTable dt = dbhelper.getdata("select * from irfiles where said=" + lbshowdownload.CommandName + "");
            if (dt.Rows.Count > 0)
                lbshowdownload.Visible = true;
            else
                lbshowdownload.Visible = false;
            LinkButton lb = (LinkButton)e.Row.FindControl("trash");
            if (stat.Text == "Verified")
            {
                lb.Visible = false;
                draft.ToolTip = "Re Verified";
            }
            else if (stat.Text == "Draft")
                lb.ToolTip = "Cancel Draft";
            else
                lb.Enabled = true;
        }
    }
    protected void preparatoryentry(object sender, EventArgs e)
    {
        if (txt_durationfrom.Text == "" || txt_durationto.Text == "" || txt_location.Text == "" || txt_provider.Text == "" || txt_about.Text == "")
        {
            Response.Write("<script>alert('Please Input Data!')</script>");
        }
        else
        {
            string filename = Path.GetFileNameWithoutExtension(fupreparatory.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fupreparatory.PostedFile.FileName);
            string contenttype = fupreparatory.PostedFile.ContentType;
            string nameofiles = hdn_empid.Value;

            DataTable dt = dbhelper.getdata("select * from Preparatory where DurationFrom = '" + txt_durationfrom.Text + "' and DurationTo = '" + txt_durationto.Text + "' and EmpId = '" + hdn_empid.Value + "'");
            if (dt.Rows.Count > 0)
            {
                Response.Write("<script>alert('Days Duration Has Already Planed!')</script>");
            }
            else
            {
                if (fupreparatory.HasFile)
                {
                    DataTable dtprep = dbhelper.getdata("insert into Preparatory(UserId, EmpId, LicenseNum, ValidUntil, Location, Cost, DurationFrom, DurationTo, Provider, About, Resources, Sysdate)values('" + Session["emp_id"].ToString() + "'," + Session["emp_id"].ToString() + ",'" + txt_license.Text.Replace("'", "").ToString() + "','" + txt_validity.Text + "','" + txt_location.Text + "','" + txt_cost.Text + "','" + txt_durationfrom.Text + "','" + txt_durationto.Text + "','" + txt_provider.Text + "','" + txt_about.Text + "','" + txt_resources.Text + "',GETDATE()) select scope_identity() saidid");
                    DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/preparatoryfiles/") + nameofiles);
                    DataTable dtinsertirfile = dbhelper.getdata("insert into irfiles (empid, userid, extension, status, contenttype, filename, said, action, date) values ('" + hdn_empid.Value + "','" + Session["emp_id"].ToString() + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + dtprep.Rows[0]["saidid"].ToString() + "','Active','" + DateTime.Now.ToString() + "') select scope_identity() idd");
                    fupreparatory.SaveAs(Server.MapPath("~/files/preparatoryfiles/") + nameofiles + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + fileExtension);
                }
                else
                {
                    DataTable dtprep = dbhelper.getdata("insert into Preparatory(EmpId, UserId, LicenseNum, ValidUntil, Location, Cost, DurationFrom, DurationTo, Provider, About, Resources, Sysdate)values('" + hdn_empid.Value + "'," + Session["emp_id"].ToString() + ",'" + txt_license.Text.Replace("'", "").ToString() + "','" + txt_validity.Text + "','" + txt_location.Text + "','" + txt_cost.Text + "','" + txt_durationfrom.Text + "','" + txt_durationto.Text + "','" + txt_provider.Text + "','" + txt_about.Text + "','" + txt_resources.Text + "',GETDATE())");
                }
            }
        }
        backlag();
    }
    protected void opendownloadprep(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_preparatory.Rows[row.RowIndex].Cells[1].FindControl("lnkbtnprep");
            trn_det_id.Value = lb.CommandName;
            DataTable dt = dbhelper.getdata("select * from irfiles where said = '" + trn_det_id.Value + "' and empid = '" + hdn_empid.Value + "' and [action] = 'Active'");
            grid_prepdownloadables.DataSource = dt;
            grid_prepdownloadables.DataBind();
            prepdownload.Style.Add("display", "block");
        }
    }
    protected void downloadprepdocs(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string folderdir = hf_action.Value;
            LinkButton lnkbtn = (LinkButton)grid_prepdownloadables.Rows[row.RowIndex].Cells[1].FindControl("lnkbtndownloadpreps");
            DataTable dt = dbhelper.getdata("select * from irfiles where id = " + lnkbtn.CommandName + "");
            string input = Server.MapPath("~/files/preparatoryfiles/" + folderdir + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();
            //Download the Decrypted File.

            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
    protected void opentraining(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from Preparatory where Id = " + seriesid.Value + "");
            txtuplicense.Text = dt.Rows[0]["LicenseNum"].ToString();
            txtupval.Text = dt.Rows[0]["ValidUntil"].ToString();
            txtuploc.Text = dt.Rows[0]["Location"].ToString();
            txtupamo.Text = dt.Rows[0]["Cost"].ToString();
            txtupduf.Text = dt.Rows[0]["DurationFrom"].ToString();
            txtupdut.Text = dt.Rows[0]["DurationTo"].ToString();
            txtuppro.Text = dt.Rows[0]["Provider"].ToString();
            txtupab.Text = dt.Rows[0]["About"].ToString();
            txtupres.Text = dt.Rows[0]["Resources"].ToString();
            pgridtraining.Visible = true;
            pgridtraining1.Visible = true;
        }
    }
    protected void updatemtraining(object sender, EventArgs e)
    {
        dbhelper.getdata("update Preparatory set LicenseNum='" + txtuplicense.Text.Replace("'", "") + "',ValidUntil='" + txtupval.Text + "',Location='" + txtuploc.Text.Replace("'", "") + "',"
            + "Cost='" + txtupamo.Text + "',DurationFrom='" + txtupduf.Text + "',DurationTo='" + txtupdut.Text + "',Provider='" + txtuppro.Text.Replace("'", "") + "',"
            + "About='" + txtupab.Text.Replace("'", "") + "',Resources='" + txtupres.Text.Replace("'", "") + "' where Id = " + seriesid.Value + "");
        backlag();
    }
    protected void cancelprep(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = row.Cells[0].Text;
                DataTable dtupdate = dbhelper.getdata("update Preparatory set Action = 'Cancel' where Id = '" + id + "'");
                dbhelper.getdata("update irfiles set canceldate = GETDATE() where id = '" + id + "'");
            }
        }
        backlag();
    }
    protected void calculatedate(object sender, EventArgs e)
    {
        if (txt_durationfrom.Text == "")
        {
            Response.Write("<script>alert('Select Duration From!')</script>");
            txt_durationto.Text = "";
        }
        else
        {
            DateTime BeginDate = Convert.ToDateTime(txt_durationfrom.Text);
            DateTime EndDate = Convert.ToDateTime(txt_durationto.Text).AddDays(1);
            TimeSpan diff = EndDate.Subtract(BeginDate);
            txt_days.Text = diff.Days.ToString();
        }
    }
    protected void backlag()
    {
        Response.Redirect("employee-profile?user_id=TIdS9+05Aas=&app_id=" + hdn_empid.Value + "&tp=ed", false);
    }
    protected void closingremarks(object sender, EventArgs e)
    {
        prepdownload.Style.Add("display", "none");
    }
}