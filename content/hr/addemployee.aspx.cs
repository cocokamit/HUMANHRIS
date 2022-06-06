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

public partial class content_hr_addemployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Session["role"].ToString() == "Admin" ? "0" : Session["emp_id"].ToString();
        Page.ClientScript.RegisterStartupScript(GetType(), "IsPostBack", "SinglePage(" + tbPage.Text + "," + id + ")", true);

        PageInit();

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            getminwage();
            hdn_empid.Value = Request.QueryString["app_id"].ToString();
            loadable();
            allgrid();
            dtdraft();

            if (int.Parse(Request.QueryString["app_id"].ToString()) > 0)
                disp();

            displaylistreq();
        }
    }

    [WebMethod]
    public static bool readUpdate(string rid, string uid)
    {
        bool check = true;
        List<string> people = new List<string>();

        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("Select * from nobel_userRight where [user_id]=" + uid + " and route_id=" + rid);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["read"].ToString() == "1")
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                    }
                }
            }
            con.Close();
        }

        return check;
    }


    protected void addStatus(object sender, EventArgs e)
    {
        modal_status.Style.Add("display","block");
    }

    protected void closeModalStatus(object sender, EventArgs e)
    {
        modal_status.Style.Add("display", "none");
    }

    protected void clickNote(object sender, EventArgs e)
    {
        if (tbNote.Text.Replace(" ", "").Length > 0)
        {
            string query = string.Empty;
            switch (hfAction.Value)
            {
                case "EDIT":
                    query = "update MEmployeeNote set note='" + tbNote.Text.Trim() + "' where id=" + hfTID.Value;
                    break;
                default:
                    query = "insert into MEmployeeNote (empID,tdate,note) values (" + hdn_empid.Value + " ,getdate(),'" + tbNote.Text.Trim() + "')";
                    break;
            }
            
            dbhelper.getdata(query);
            alertNOte.Visible = false;
            hfAction.Value = string.Empty;
            tbNote.Text = string.Empty;
            ShowNote();
        }
        else
            alertNOte.Visible = true;
    }

    protected void clickDelete(object sender, EventArgs e)
    {
        string id = hfTID.Value;

        switch (hfTransaction.Value)
        {
            case "note":
                dbhelper.getdata("update MEmployeeNote set status=1 where id=" + id);
              
                break;
        }

        ShowNote();
    }

    protected void clickEditNote(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hfAction.Value = "EDIT";
            hfTID.Value = row.Cells[0].Text;
            tbNote.Text = row.Cells[2].Text;
        }
    }

    protected void PageInit()
    {
        int x = int.Parse(Request.QueryString["app_id"].ToString());
        l_page.Text = l_pagetitle.Text = x == 0 ? "New Employee" : "Employee";
        ViewState["action"] = x == 0 ? "New" : "Edit";
        hf_action.Value = x.ToString();
        btn_submit.Text = x == 0 ? "Create" : "Update";
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
    protected void displaylistreq()
    {
        DataTable data = dbhelper.getdata("select a.id, a.[description],(select COUNT(*) from file_details where empid = '" + hdn_empid.Value + "' and filecode =a.id and status='Active' ) cnt From orley_empsrequirement a");
        grid_reqlistcheck.DataSource = data;
        grid_reqlistcheck.DataBind();
        div_nolaman.Visible = data.Rows.Count == 0 ? true : false;
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
    protected void clickcancelreq(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_reqlistcheck.Rows[row.RowIndex].Cells[3].FindControl("lnk_req");
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update file_trn set action='cancel-" + Session["user_id"].ToString() + "' where id=" + lb.CommandName + "");
                dbhelper.getdata("update file_details set status='cancel' where trn_file_id=" + lb.CommandName + "");
                string query = "select a.id, a.[description],(select COUNT(*) from file_trn where empid = '" + hdn_empid.Value + "' and file_codes=a.id ) cnt From orley_empsrequirement a";
                DataTable dt = dbhelper.getdata(query);
                grid_reqlistcheck.DataSource = dt;
                grid_reqlistcheck.DataBind();
            }
            else { }
        }
    }
    protected void clickclass(object sender, EventArgs e)
    {
        displaylistreq();
    }
    protected bool chk()
    {
        bool err = true;
        if (!fuempreq.HasFile)
        {
            lbl_sf.Text = "*";
            err = false;
        }
        else
            lbl_sf.Text = "";

        if (txt_desc.Text.Trim().Length == 0)
        {
            lbl_desc.Text = "*";
            err = false;
        }
        else
            lbl_desc.Text = "";

        return err;
    }
    protected void clickupload(object sender, EventArgs e)
    {
        if (ddl_reqlist.SelectedValue == "0")
        {
            Response.Write("<script>alert('Selection Failed!')</script>");
        }
        else
        {
            string fldername = hdn_empid.Value;
            string classs = hfEmp.Value;
            string filename = "";

            if (chk())
            {
                if (fuempreq.HasFile)
                {
                    string filepath = Server.MapPath("files/archiving/" + fldername + "" + classs + "/");
                    DirectoryInfo di = Directory.CreateDirectory(filepath);
                    HttpFileCollection uploadedFiles = Request.Files;
                    string empid = classs == "forms" ? "0" : hfEmp.Value;
                    DataTable dttrnfile = dbhelper.getdata("insert into file_trn values(getdate()," + Session["user_id"].ToString() + "," + hdn_empid.Value + ",'2','" + txt_desc.Text + "','" + ddl_reqlist.SelectedValue + "','1') select scope_identity() id");
                    for (int i = 0; i < uploadedFiles.Count; i++)
                    {
                        HttpPostedFile userPostedFile = uploadedFiles[i];
                        if (userPostedFile.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(userPostedFile.FileName);
                            string fileExtension = Path.GetExtension(userPostedFile.FileName);
                            string contenttype = userPostedFile.ContentType;

                            string query = "insert into file_details(date,userid,empid,classid,location,filename,description,status,contenttype,trn_file_id,filecode,filename2) " +
                                "values (getdate(),'" + Session["user_id"].ToString() + "','" + hdn_empid.Value + "','2','files/archiving/" + fldername + "','" + fileName + "','" + txt_desc.Text + "','Active','" + contenttype + "'," + dttrnfile.Rows[0]["id"].ToString() + ",'" + ddl_reqlist.SelectedValue + "','" + fileExtension + "') select scope_identity() id";
                            DataTable dt = dbhelper.getdata(query);
                            filename = filepath + fileName.Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + fileExtension;
                            userPostedFile.SaveAs(filename);
                        }
                    }
                    string list = "select a.id, a.[description],(select COUNT(*) from file_trn where empid ='" + hdn_empid.Value + "' and file_codes=a.id ) cnt From orley_empsrequirement a";
                    DataTable dtlist = dbhelper.getdata(list);
                    grid_reqlistcheck.DataSource = dtlist;
                    grid_reqlistcheck.DataBind();
                    txt_desc.Text = "";
                }
            }
        }
    }
    protected void download(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string folderdir = hdn_empid.Value;
            LinkButton lnk_viewreq = (LinkButton)grid_det.Rows[row.RowIndex].Cells[2].FindControl("lnk_download");
            DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
            string input = Server.MapPath("~/files/archiving/" + folderdir + "/") + dt.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString();

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
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
    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_reqlistcheck.Rows[row.RowIndex].Cells[1].FindControl("lbviewreq");
            trn_det_id.Value = lb.CommandName;
            DataTable dts = dbhelper.getdata("select a.id, a.filename+'_'+convert(varchar,id)+a.filename2 filename," +
                "(select id from orley_empsrequirement where id = a.filecode)trncode,a.empid from file_details a where " +
                "filecode = '" + trn_det_id.Value + "' and status ='Active' and a.empid = '" + hdn_empid.Value + "'");
            grid_det.DataSource = dts;
            grid_det.DataBind();
            modal_requirementlist.Style.Add("display", "block");
        }
    }
    protected void closingremarks(object sender, EventArgs e)
    {
        modal_requirementlist.Style.Add("display", "none");
        irsanctionlist.Style.Add("display", "none");
        prepdownload.Style.Add("display", "none");
    }
    protected void candetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //LinkButton lbrowselect = (LinkButton)grid_det.Rows[row.RowIndex].Cells[2].FindControl("lnk_can");
            //string nhnh=lbrowselect.CommandName;
            //ppop(false);


            if (TextBox1.Text == "Yes")
            {
                // DataTable dt2 = dbhelper.getdata("update file_trn set action='0' where id = " + row.Cells[0].Text + "");
                DataTable dt = dbhelper.getdata("update file_details set status='can' where id=" + row.Cells[0].Text + " select * from file_details where  id=" + row.Cells[0].Text + " ");
                string hh = "select b.id, b.filename+'_'+convert(varchar,b.id)+b.filename2 filename from file_details b " +
                            "where b.status ='Active' and b.filecode=" + trn_det_id.Value + " ";
                DataTable dt1 = dbhelper.getdata(hh);
                grid_det.DataSource = dt1;
                grid_det.DataBind();
            }
            else
            { }
        }
    }
    protected void closepopup(object sender, EventArgs e)
    {
        Response.Redirect("addemployee?user_id=TIdS9+05Aas=&app_id=" + hdn_empid.Value + "&tp=ed", false);
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
        /**NOTIFICATION**/
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        hdn_empid.Value = Request.QueryString["app_id"].ToString();
        string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile,*,left(convert(varchar,a.surgepay_effective,101),10)surgepay_effective1, case when (select top 1 statusid from memployeestatus where empid=a.id and effectivedate<=GETDATE() order by empstatid desc) is null then a.emp_status  else (select top 1 statusid from memployeestatus where empid=a.id and effectivedate<=GETDATE() order by empstatid desc) end   empstatus,left(convert(varchar,a.sbudate,101),10)sbudate1,left(convert(varchar,a.DateHired,101),10)DateHired1,a.allowsc,left(convert(varchar,a.health_card,101),10)health_card1,case when allowAccess is null then 'False' else allowAccess end allowAccess,case when allowOTMeal is null then 'False' else allowOTMeal end allowOTMeal,case when nonepunching is null then 'False' else nonepunching end nonepunching,case when allowOT  is null then 'False' else allowOT end allowOT from memployee a where a.id=" + hdn_empid.Value + "";
        DataTable dt = dbhelper.getdata(query);
        if (dt.Rows.Count >0)
        {
            string employe = Request.QueryString["app_id"].ToString();
            ddl_status.Enabled = false;
           

            ShowTrail();
            ShowNote();

            profile.Value = dt.Rows[0]["profile"].ToString();

            txt_health.Text = dt.Rows[0]["health_card1"].ToString() == "01/01/1900" ? "" : dt.Rows[0]["health_card1"].ToString();
            txt_sapnumber.Text = dt.Rows[0]["SAPNumber"].ToString();
            ddl_internalorder.SelectedValue = dt.Rows[0]["InternalOrderId"].ToString().Length > 0 ? dt.Rows[0]["InternalOrderId"].ToString() : "0";
            ddl_store.SelectedValue = dt.Rows[0]["store_id"].ToString().Length == 0 ? "0" : dt.Rows[0]["store_id"].ToString();
            ViewState["id"] = txt_idnumber.Text = dt.Rows[0]["idnumber"].ToString();
            ViewState["name"] = dt.Rows[0]["lastname"].ToString() + ", " + dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["ExtensionName"].ToString() + " " + dt.Rows[0]["MiddleName"].ToString();
            txt_lname.Text = dt.Rows[0]["lastname"].ToString();
            txt_fname.Text = dt.Rows[0]["firstname"].ToString();
            txt_mname.Text = dt.Rows[0]["MiddleName"].ToString();
            txt_exname.Text = dt.Rows[0]["ExtensionName"].ToString();
            txt_noc.Text = dt.Rows[0]["noc"].ToString();
            ViewState["phone"] = txt_pnumber.Text = dt.Rows[0]["PhoneNumber"].ToString().Length > 0 ? dt.Rows[0]["PhoneNumber"].ToString() : "";
            ViewState["address"] = txt_presentaddres.Text = dt.Rows[0]["Address"].ToString().Length > 0 ? dt.Rows[0]["Address"].ToString() : "";
            ViewState["email"] = txt_email.Text = dt.Rows[0]["EmailAddress"].ToString().Length > 0 ? dt.Rows[0]["EmailAddress"].ToString() : "";
            ViewState["pemail"] = txt_pemail.Text = dt.Rows[0]["personalemail"].ToString().Length > 0 ? dt.Rows[0]["personalemail"].ToString() : "";
           
            DateTime datehired = dt.Rows[0]["DateHired1"].ToString().Length == 0 ? DateTime.Now : DateTime.Parse(dt.Rows[0]["DateHired1"].ToString());
            txt_datehired.Text = datehired.ToString("MM/dd/yyyy");
            //txt_datehired.Text = dt.Rows[0]["DateHired1"].ToString().Replace(" 1", "");

            txt_sbudate.Text = dt.Rows[0]["sbudate1"].ToString() == "01/01/1900" ? "" : dt.Rows[0]["sbudate1"].ToString();
            

            //NEED UPDATE table MZipCode
            ddl_zipcode.SelectedValue = dt.Rows[0]["ZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["ZipCodeId"].ToString();

            ddl_bzc.SelectedValue = dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString();
            ddl_section.SelectedValue = dt.Rows[0]["sectionid"].ToString();
            txt_cnumber.Text = dt.Rows[0]["CellphoneNumber"].ToString();
            txt_dob.Text = dt.Rows[0]["DateOfBirth"].ToString().Length == 0 ? "" : DateTime.Parse(dt.Rows[0]["DateOfBirth"].ToString()).ToString("MM/dd/yyyy");
            txt_pob.Text = dt.Rows[0]["PlaceOfBirth"].ToString();

            //NEED UPDATE table MZipCode
            //ddl_bzc.SelectedValue = "0"; // dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString().Length == 0 ? "0" : dt.Rows[0]["PlaceOfBirthZipCodeId"].ToString();

            ddl_sex.Text = dt.Rows[0]["sex"].ToString();
            ddl_cs.SelectedValue = dt.Rows[0]["CivilStatus"].ToString();
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
            ddl_branch.Items.Add(new ListItem("", "0"));
            foreach (DataRow dr in dts.Rows)
            {
                ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
            }
            ddl_branch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
            ddl_department.SelectedValue = dt.Rows[0]["DepartmentId"].ToString();
            ddl_divission.SelectedValue = dt.Rows[0]["DivisionId"].ToString();
            ddl_divission2.SelectedValue = dt.Rows[0]["DivisionId2"].ToString();
            ddl_position.SelectedValue = dt.Rows[0]["PositionId"].ToString();
            ViewState["position"] = ddl_position.SelectedItem.ToString();
            ddl_pg.SelectedValue = dt.Rows[0]["PayrollGroupId"].ToString();
            ddl_gl.SelectedValue = dt.Rows[0]["AccountId"].ToString();
            ddl_payrolltype.SelectedValue = dt.Rows[0]["PayrollTypeId"].ToString();
            txt_fnod.SelectedValue = dt.Rows[0]["FixNumberOfDays"].ToString().Replace(".00000", "");
            txt_fnoh.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["FixNumberOfHours"].ToString()));
            ddl_lvel.SelectedValue = dt.Rows[0]["level"].ToString();
            //PAYROLL SETUP
            tb_gmr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["GrossMonthlyRate"].ToString()));
            txt_mr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["MonthlyRate"].ToString()));
            tb_ntnsd.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["NonTaxableNightShiftDifference"].ToString()));
            tb_nta.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["NonTaxableAllowance"].ToString()));
            tb_ntaca.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["NonTaxableActingCapacityAllowance"].ToString()));
            txt_pr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["PayrollRate"].ToString()));

            txt_dr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["DailyRate"].ToString()));
            txt_adr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["AbsentDailyRate"].ToString()));
            txt_hr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["HourlyRate"].ToString()));
            txt_nhr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["NightHourlyRate"].ToString()));
            txt_ohr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["OvertimeHourlyRate"].ToString()));
            txt_onhr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["OvertimeNightHourlyRate"].ToString()));
            txt_thr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["TardyHourlyRate"].ToString()));
            ddl_taxtable.Text = dt.Rows[0]["TaxTable"].ToString();
            txt_hdmfaddon.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["HDMFAddOn"].ToString().Length == 0 ? "0" : dt.Rows[0]["HDMFAddOn"].ToString()));
            txt_sssaddon.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["SSSAddOn"].ToString().Length == 0 ? "0" : dt.Rows[0]["SSSAddOn"].ToString()));
            ddl_hdmftype.Text = dt.Rows[0]["HDMFType"].ToString();
            ddl_status.SelectedValue = dt.Rows[0]["empstatus"].ToString();
            if (dt.Rows[0]["SSSIsGrossAmount"].ToString() == "true")
                chk_sssgs.Checked = true;
            else
                chk_sssgs.Checked = false;
            //if (dt.Rows[0]["IsMinimumWageEarner"].ToString() == "true")
            //    chk_imw.Checked = true;
            //else
            //    chk_imw.Checked = false;

            if (dt.Rows[0]["leavestatus"].ToString() == "True")
                chk_l.Checked = true;
            else
                chk_l.Checked = false;

            //if (dt.Rows[0]["isminimumwageearner"].ToString() == "True")
            //    chk_mwe.Checked = true;
            //else
            //    chk_mwe.Checked = false;

            txt_permanentadress.Text = dt.Rows[0]["permanentaddress"].ToString();
            ddl_bloodtype.SelectedValue = dt.Rows[0]["bloodtype"].ToString();
            ddl_shiftcode.SelectedValue = dt.Rows[0]["ShiftCodeId"].ToString();
            txt_fnod.SelectedValue = dt.Rows[0]["FixNumberOfDays"].ToString().Replace(".00000", "");
            DataTable nti = dbhelper.getdata("select * from NTI where empid=" + Request.QueryString["app_id"].ToString() + " and action is null order by id desc");
            DataTable allowance = dbhelper.getdata("select * from mealallowance where empid=" + Request.QueryString["app_id"].ToString() + " and action is null order by id desc");
            txt_meal_allow.Text = allowance.Rows.Count > 0 ? allowance.Rows[0]["amt"].ToString() : "0.00";
            txt_nti.Text = nti.Rows.Count > 0 ? nti.Rows[0]["amt"].ToString() : "0.00";
            string offset = dt.Rows[0]["allowoffset"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowoffset"].ToString();
            string hdoffset = dt.Rows[0]["allowhdoffset"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowhdoffset"].ToString();
            string offsc = dt.Rows[0]["allowsc"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowsc"].ToString();
            string ismwe = dt.Rows[0]["IsMinimumWageEarner"].ToString().Length == 0 ? "False" : dt.Rows[0]["IsMinimumWageEarner"].ToString();
            string nonpunch = dt.Rows[0]["nonepunching"].ToString().Length == 0 ? "False" : dt.Rows[0]["nonepunching"].ToString();
            chk_nonpunch.Checked = nonpunch == "False" ? false : true;
            string allowot = dt.Rows[0]["allowOT"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowOT"].ToString();
            chk_allowot.Checked = allowot == "False" ? false : true;
            string otmeal = dt.Rows[0]["allowOTMeal"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowOTMeal"].ToString();
            chk_otmeal.Checked = otmeal == "False" ? false : true;
            string newhire = dt.Rows[0]["newhire"].ToString().Length == 0 ? "False" : dt.Rows[0]["newhire"].ToString();
            chk_nhs.Checked = newhire == "False" ? false : true;
            string allowaccess = dt.Rows[0]["allowAccess"].ToString().Length == 0 ? "False" : dt.Rows[0]["allowAccess"].ToString();
            chk_access.Checked = allowaccess == "False" ? false : true;
            chk_allowoffset.Checked = offset == "False" ? false : true;
            chk_allowhdoffset.Checked = hdoffset == "False" ? false : true;
            chk_imw.Checked = ismwe == "False" ? false : true;
            chk_allow_sc.Checked = offsc == "False" ? false : true;
            if (dt.Rows[0]["empstatus"].ToString() == "4")
                ddl_pg.Enabled = false;
            ddl_mop.SelectedItem.Text = dt.Rows[0]["mop"].ToString();
            txt_surge_effective_date.Text = dt.Rows[0]["surgepay_effective1"].ToString();
            alldisp();

        }
    }

    protected void click_paytype(object sender, EventArgs e)
    {


        if (ddl_payrolltype.SelectedValue == "2")
        {
            txt_pr.Enabled = false;
            txt_pr.Text = "0";
            txt_mr.Enabled = false;
            txt_mr.Text = "0";

        }
        else if (ddl_payrolltype.SelectedValue == "1")
        {
            txt_pr.Enabled = true;
            txt_mr.Enabled = true;
            txt_hr.Text = "0";
            txt_dr.Text = "0";

        }
        else
        {
            txt_mr.Text = "0";
            txt_pr.Text = "0";
            txt_dr.Text = "0";
            txt_hr.Text = "0";

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
        
    }

    protected void loadable()
    {
        ViewState["approver"] = "0";
        ViewState["leave-credit"] = "0";

        string query = "";

        DataTable dt;

        query = "select * from Approver where emp_id = " + hdn_empid.Value + " and herarchy = 0 and status is NULL";
        dt = dbhelper.getdata(query);
        if (dt.Rows.Count > 0)
        {
            chck_scheduler.Enabled = false;
        }

        query = "select * from MCompany order by company";
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

        query = "select * from MReligion order by Religion";
        dt = dbhelper.getdata(query);
        ddl_relegion.Items.Clear();
        ddl_relegion.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_relegion.Items.Add(new ListItem(dr["Religion"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MTaxCode order by TaxCode";
        dt = dbhelper.getdata(query);
        ddl_taxcode.Items.Clear();
        ddl_taxcode.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_taxcode.Items.Add(new ListItem(dr["TaxCode"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MDepartment order by department";
        dt = dbhelper.getdata(query);
        ddl_department.Items.Clear();
        ddl_department.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_department.Items.Add(new ListItem(dr["department"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MPosition order by position";
        dt = dbhelper.getdata(query);
        ddl_position.Items.Clear();
        ddl_position.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_position.Items.Add(new ListItem(dr["position"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MDivision order by division";
        dt = dbhelper.getdata(query);
        ddl_divission.Items.Clear();
        ddl_divission.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_divission.Items.Add(new ListItem(dr["division"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Msection order by seccode";
        dt = dbhelper.getdata(query);
        ddl_section.Items.Clear();
        ddl_section.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
        }

        dt = getdata.PayrollGroup("1");
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Mstore order by store";
        dt = dbhelper.getdata(query);
        ddl_store.Items.Clear();
        ddl_store.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_store.Items.Add(new ListItem(dr["store"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MAccount order by Account";
        dt = dbhelper.getdata(query);
        ddl_gl.Items.Clear();
        ddl_gl.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_gl.Items.Add(new ListItem(dr["Account"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Mbloodtype order by bloodtype";
        dt = dbhelper.getdata(query);
        ddl_bloodtype.Items.Clear();
        ddl_bloodtype.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_bloodtype.Items.Add(new ListItem(dr["bloodtype"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MCitizenship order by Citizenship";
        dt = dbhelper.getdata(query);
        ddl_citizenship.Items.Clear();
        ddl_citizenship.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_citizenship.Items.Add(new ListItem(dr["Citizenship"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MPayrollType where PayrollType = 'Fixed'";
        dt = dbhelper.getdata(query);
        ddl_payrolltype.Items.Clear();
        //ddl_payrolltype.Items.Add(new ListItem("Fixed", "1"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payrolltype.Items.Add(new ListItem(dr["payrolltype"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MShiftCode where status is null order by shiftcode";
        dt = dbhelper.getdata(query);
        ddl_shiftcode.Items.Clear();
        ddl_shiftcode.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_shiftcode.Items.Add(new ListItem(dr["shiftcode"].ToString() + " (" + dr["remarks"].ToString() + ")", dr["id"].ToString()));
        }

        query = "select * from mempstatus_setup where action='Default' or action is null order by status";
        dt = dbhelper.getdata(query);
        ddl_status.Items.Clear();
        ddl_status.Items.Add(new ListItem("Probationary", "1"));
        ddl_statusfinal.Items.Clear();
        ddl_status_emp.Items.Clear();
        ddl_status_emp.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow drr in dt.Rows)
        {
            ddl_status.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
            ddl_statusfinal.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
            ddl_status_emp.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
        }


        ddl_yyyy.Items.Clear();
        ddl_yyyy.Items.Add(new ListItem("Select", "0"));
        for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 1; i++)
            ddl_yyyy.Items.Add(new ListItem(i.ToString(), i.ToString()));

        query = "select * from  asset_cat where  action is null";
        dt = dbhelper.getdata(query);
        ddl_cat.Items.Clear();
        ddl_cat.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow drr in dt.Rows)
        {
            ddl_cat.Items.Add(new ListItem(drr["Description"].ToString(), drr["id"].ToString()));
        }
        query = "select * from orley_empsrequirement where [status] = 'ACTIVE'";
        DataTable dtrequire = dbhelper.getdata(query);
        ddl_reqlist.Items.Clear();
        ddl_reqlist.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow ddrer in dtrequire.Rows)
        {
            ddl_reqlist.Items.Add(new ListItem(ddrer["Description"].ToString(), ddrer["id"].ToString()));
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

        query = "select * from MLevel where [Level] ! = 'MEMBER'";
        DataTable dtlevel = dbhelper.getdata(query);
        ddl_lvel.Items.Clear();
        ddl_lvel.Items.Add(new ListItem("MEMBER", "3"));
        foreach (DataRow level in dtlevel.Rows)
        {
            ddl_lvel.Items.Add(new ListItem(level["Level"].ToString(), level["Id"].ToString()));
        }

        query = "select * from MCivilStatus";
        DataTable dtcvil = dbhelper.getdata(query);
        ddl_cs.Items.Clear();
        foreach (DataRow drcvil in dtcvil.Rows)
        {
            ddl_cs.Items.Add(new ListItem(drcvil["CivilStatus"].ToString(), drcvil["Id"].ToString()));
        }

        query = "select * from MBranch";
        dt = dbhelper.getdata(query);
        ddl_branch.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MDivision2 order by Id asc";
        dt = dbhelper.getdata(query);
        ddl_divission2.Items.Clear();
        ddl_divission2.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_divission2.Items.Add(new ListItem(dr["Division2"].ToString(), dr["Id"].ToString()));
        }

        query = "select * from MInternalOrder order by InternalOrder asc";
        DataTable dtinternal = dbhelper.getdata(query);
        ddl_internalorder.Items.Clear();
        ddl_internalorder.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow drintenal in dtinternal.Rows)
        {
            ddl_internalorder.Items.Add(new ListItem(drintenal["InternalOrder"].ToString(), drintenal["Id"].ToString()));
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

        else if (ddl_status.SelectedValue.Trim().Length == 0)
        {
            l_msg.Text = "Status must be Required!";
            err = false;
        }
        else if (ddl_taxcode.SelectedValue == "0")
        {
            l_msg.Text = "Tax Code must be Required!";
            err = false;
        }
        else if (ddl_taxtable.SelectedValue.Length == 0)
        {
            l_msg.Text = "Tax Table must be Required!";
            err = false;
        }
        else if (ddl_gl.SelectedValue == "0")
        {
            l_msg.Text = "GL Account must be Required!";
            err = false;
        }
        else if (ddl_shiftcode.SelectedValue == "0")
        {
            l_msg.Text = "Shift Code must be Required!";
            err = false;
        }
        else if (ddl_payrolltype.SelectedValue == "0")
        {
            l_msg.Text = "Payroll Type must be Required!";
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
        string args = ViewState["action"].ToString();
        int action = int.Parse(hf_action.Value);
        int index = int.Parse(tab_index.Text);

        string ret;
        string filename = "0";
        string contenttype = "0";
        string fileconcat = "";

        switch (index)
        {
            case 0:
                if (checkidentity())
                {
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("emp_identity", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@idnumber", SqlDbType.VarChar, 5000).Value = txt_idnumber.Text;
                            cmd.Parameters.Add("@SAPNumber", SqlDbType.VarChar, 5000).Value = txt_sapnumber.Text;
                            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 5000).Value = txt_fname.Text;
                            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 5000).Value = txt_lname.Text;
                            cmd.Parameters.Add("@mname", SqlDbType.VarChar, 5000).Value = txt_mname.Text;
                            cmd.Parameters.Add("@exname", SqlDbType.VarChar, 5000).Value = txt_exname.Text;
                            cmd.Parameters.Add("@datehired", SqlDbType.VarChar, 50).Value = txt_datehired.Text;
                            cmd.Parameters.Add("@empstatus", SqlDbType.VarChar, 50).Value = ddl_status.SelectedValue;
                            cmd.Parameters.Add("@companyid", SqlDbType.Int).Value = ddl_company.SelectedValue;
                            cmd.Parameters.Add("@branchid", SqlDbType.Int).Value = ddl_branch.SelectedValue;
                            cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = ddl_department.SelectedValue;
                            cmd.Parameters.Add("@divid", SqlDbType.Int).Value = ddl_divission.SelectedValue;
                            cmd.Parameters.Add("@divid2", SqlDbType.Int).Value = ddl_divission2.SelectedValue;
                            cmd.Parameters.Add("@secid", SqlDbType.Int).Value = ddl_section.SelectedValue;
                            cmd.Parameters.Add("@posid", SqlDbType.Int).Value = ddl_position.SelectedValue;
                            cmd.Parameters.Add("@pgid", SqlDbType.Int).Value = ddl_pg.SelectedValue;
                            cmd.Parameters.Add("@lvel", SqlDbType.Int).Value = ddl_lvel.SelectedValue.Length > 0 ? ddl_lvel.SelectedValue : "0";
                            cmd.Parameters.Add("@action", SqlDbType.VarChar, 50).Value = ViewState["action"].ToString();
                            cmd.Parameters.Add("@internalorder", SqlDbType.Int).Value = ddl_internalorder.SelectedValue.Length > 0 ? ddl_internalorder.SelectedValue : "0";
                           // cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_identity.Text;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                            cmd.Parameters.Add("@employid", SqlDbType.Int).Value = hdn_empid.Value;
                            cmd.Parameters.Add("@sbudate", SqlDbType.VarChar, 50).Value = txt_sbudate.Text;
                            cmd.Parameters.Add("@allowoffset", SqlDbType.VarChar, 50).Value = chk_allowoffset.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@allowhdoffset", SqlDbType.VarChar, 50).Value = chk_allowhdoffset.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@allowsc", SqlDbType.VarChar, 50).Value = chk_allow_sc.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@store", SqlDbType.Int).Value = ddl_store.SelectedValue;
                            cmd.Parameters.Add("@health_card", SqlDbType.VarChar, 50).Value = txt_health.Text;
                            cmd.Parameters.Add("@isminimumwageearner", SqlDbType.VarChar, 50).Value = chk_imw.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@nonepunching", SqlDbType.VarChar, 50).Value = chk_nonpunch.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@otmeal", SqlDbType.VarChar, 50).Value = chk_otmeal.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@allowot", SqlDbType.VarChar, 50).Value = chk_allowot.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@allowaccess", SqlDbType.VarChar, 50).Value = chk_access.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@newhire", SqlDbType.VarChar, 50).Value = chk_nhs.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@surgepay_effective", SqlDbType.VarChar, 50).Value = txt_surge_effective_date.Text;
                            cmd.Parameters.Add("@mop", SqlDbType.VarChar, 50).Value = ddl_mop.SelectedItem.Text;
                            cmd.Parameters.Add("@dob", SqlDbType.VarChar, 50000).Value = txt_dob.Text;
                            cmd.Parameters.Add("@GMR", SqlDbType.VarChar).Value = tb_gmr.Text.Replace("'", "").ToString().Length > 0 ? tb_gmr.Text.Replace("'", "").ToString() : "0.00";
                            cmd.Parameters.Add("@NTNSD", SqlDbType.VarChar).Value = tb_ntnsd.Text.Replace("'", "").ToString().Length > 0 ? tb_ntnsd.Text.Replace("'", "").ToString() : "0.00";
                            cmd.Parameters.Add("@NTA", SqlDbType.VarChar).Value = tb_nta.Text.Replace("'", "").ToString().Length > 0 ? tb_nta.Text.Replace("'", "").ToString() : "0.00";
                            cmd.Parameters.Add("@NTACA", SqlDbType.VarChar).Value = tb_ntaca.Text.Replace("'", "").ToString().Length > 0 ? tb_ntaca.Text.Replace("'", "").ToString() : "0.00";
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();

                            hdn_empid.Value = ret;
                        }
                    }
                               
        
                    if (ret != "exist")
                    {
                        using (SqlConnection con = new SqlConnection(dbconnection.conn))
                        {
                            using (SqlCommand cmd = new SqlCommand("master_personal_info", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@pressentad", SqlDbType.VarChar, 5000).Value = txt_presentaddres.Text;
                                cmd.Parameters.Add("@pressentadzipcode", SqlDbType.VarChar, 5000).Value = ddl_zipcode.SelectedValue;
                                cmd.Parameters.Add("@phonenumber", SqlDbType.VarChar, 50).Value = txt_pnumber.Text;
                                cmd.Parameters.Add("@cellphone", SqlDbType.VarChar, 50).Value = txt_cnumber.Text;
                                cmd.Parameters.Add("@email", SqlDbType.VarChar, 5000).Value = txt_email.Text;
                                cmd.Parameters.Add("@noc", SqlDbType.VarChar, 5000).Value = txt_noc.Text;
                                cmd.Parameters.Add("@pemail", SqlDbType.VarChar, 5000).Value = txt_pemail.Text;
                                cmd.Parameters.Add("@dateofbirth", SqlDbType.VarChar, 50).Value = txt_dob.Text;
                                cmd.Parameters.Add("@placeofbirth", SqlDbType.VarChar, 5000).Value = txt_pob.Text;
                                cmd.Parameters.Add("@birthzipcode", SqlDbType.Int).Value = ddl_bzc.SelectedValue;
                                cmd.Parameters.Add("@sex", SqlDbType.VarChar, 50).Value = ddl_sex.Text;
                                cmd.Parameters.Add("@civilstatus", SqlDbType.VarChar, 50).Value = ddl_cs.SelectedValue.Length > 0 ? ddl_cs.SelectedValue : "0";
                                cmd.Parameters.Add("@citizenshipid", SqlDbType.Int).Value = ddl_citizenship.SelectedValue;
                                cmd.Parameters.Add("@relegionid", SqlDbType.Int).Value = ddl_relegion.SelectedValue;
                                cmd.Parameters.Add("@height", SqlDbType.VarChar, 50).Value = txt_height.Text.Length > 0 ? txt_height.Text : "0";
                                cmd.Parameters.Add("@weight", SqlDbType.VarChar, 50).Value = txt_weight.Text.Length > 0 ? txt_weight.Text : "0";
                                cmd.Parameters.Add("@bloodtypeid", SqlDbType.Int).Value = ddl_bloodtype.SelectedValue;
                                cmd.Parameters.Add("@permanentad", SqlDbType.VarChar, 5000).Value = txt_permanentadress.Text;
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                ret = cmd.Parameters["@out"].Value.ToString();
                                con.Close();
                            }
                        }

                        if (action == 0)
                        {
                            btn_submit.Text = "Finish";
                            tab_index.Text = "1";
                        }
                    }
                }

                break;
            case 1:
                if (checkpayrollinfo())
                {

                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("master_payroll_info", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@gsis", SqlDbType.VarChar, 50).Value = txt_gsisno.Text;
                            cmd.Parameters.Add("@sss", SqlDbType.VarChar, 50).Value = txt_sssno.Text;
                            cmd.Parameters.Add("@hdmf", SqlDbType.VarChar, 50).Value = txt_hdmfno.Text;
                            cmd.Parameters.Add("@sssaddon", SqlDbType.VarChar, 50).Value = txt_sssaddon.Text;
                            cmd.Parameters.Add("@hdmfaddon", SqlDbType.VarChar, 50).Value = txt_hdmfaddon.Text;
                            cmd.Parameters.Add("@phic", SqlDbType.VarChar, 50).Value = txt_phicno.Text;
                            cmd.Parameters.Add("@tin", SqlDbType.VarChar, 50).Value = txt_tin.Text;
                            cmd.Parameters.Add("@atm", SqlDbType.VarChar, 50).Value = txt_atm.Text;
                            cmd.Parameters.Add("@taxcodeid", SqlDbType.Int).Value = ddl_taxcode.SelectedValue;
                            cmd.Parameters.Add("@glid", SqlDbType.Int).Value = ddl_gl.SelectedValue;
                            cmd.Parameters.Add("@shiftid", SqlDbType.Int).Value = ddl_shiftcode.SelectedValue;
                            cmd.Parameters.Add("@taxtable", SqlDbType.VarChar, 50).Value = ddl_taxtable.Text;
                            cmd.Parameters.Add("@pgid", SqlDbType.Int).Value = ddl_pg.SelectedValue;
                            //cmd.Parameters.Add("@isminimumwageearner", SqlDbType.VarChar, 50).Value = chk_imw.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@minimum", SqlDbType.VarChar, 50).Value = chk_imw.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@leave", SqlDbType.VarChar, 50).Value = chk_l.Checked == true ? "True" : "False";
                            cmd.Parameters.Add("@mealallow", SqlDbType.VarChar, 50).Value = txt_meal_allow.Text.Length > 0 ? txt_meal_allow.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@nti", SqlDbType.VarChar, 50).Value = txt_nti.Text.Length > 0 ? txt_nti.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"];
                            //cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_payroll.Text;
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();

                        }
                    }

                    //string query = "select * from memployee where ";
                    //DataTable dt = dbhelper.getdata(query);
                    if (ret != "exist" && ret.Length > 0)
                    {
                        if (Request.QueryString["app_id"].ToString() == "0")
                            Response.Redirect("employee?user_id=" + function.Encrypt(ret, true) + "&app_id=" + ret + "&tp=ed");
                        else
                            disp();
                    }
                    else
                        hdn_empid.Value = "";
                }
                break;
            case 2:
                if (checkcompensation())
                {
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("master_compensation_info", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ptid", SqlDbType.Int).Value = ddl_payrolltype.SelectedValue;
                            cmd.Parameters.Add("@fnod", SqlDbType.VarChar, 50).Value = txt_fnod.Text.Length > 0 ? txt_fnod.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@fnoh", SqlDbType.VarChar, 50).Value = txt_fnoh.Text.Length > 0 ? txt_fnoh.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@mr", SqlDbType.VarChar, 50).Value = txt_mr.Text.Length > 0 ? txt_mr.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@pr", SqlDbType.VarChar, 50).Value = txt_pr.Text.Length > 0 ? txt_pr.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@dr", SqlDbType.VarChar, 50).Value = txt_dr.Text.Length > 0 ? txt_dr.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@hr", SqlDbType.VarChar, 50).Value = txt_hr.Text.Length > 0 ? txt_hr.Text.Replace(",", "") : "0";
                            cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"];
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();
                        }
                    }
                    if (ret != "exist" && ret.Length > 0)
                    {
                        if (Request.QueryString["app_id"].ToString() == "0")
                            Response.Redirect("employee?user_id=" + function.Encrypt(ret, true) + "&app_id=" + ret + "&tp=ed");
                        else
                            disp();
                    }
                    else
                        hdn_empid.Value = "";
                }
                break;
        }

        //UPLOAD PROFILE
        if (dataURLInto.InnerText.Length > 0)
        {
            DataTable dt = dbhelper.getdata("select * from file_details where empid=" + hdn_empid.Value + " and classid=1");
            DataTable dtemps = dbhelper.getdata("select * from MEmployee where Id = '" + hdn_empid.Value + "'");
            if (dt.Rows.Count == 0)
                dbhelper.getdata("insert into file_details values (getdate()," + Session["user_id"].ToString() + "," + hdn_empid.Value + ",'" + dtemps.Rows[0]["DepartmentId"] + "',1,'files/profile','.png','profile','Active','image/png',0,0,0)");

            string base64 = dataURLInto.InnerText;
            byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
            string path = Server.MapPath("~/files/profile/");
            using (System.IO.FileStream stream = new System.IO.FileStream(path + hdn_empid.Value + ".png", System.IO.FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            System.Drawing.Image image = System.Drawing.Image.FromFile(path + hdn_empid.Value + ".png");
            System.Drawing.Image thumb = image.GetThumbnailImage(50, 50, delegate() { return false; }, (IntPtr)0);
            thumb.Save(path + "/" + hdn_empid.Value + "-thumb.png");
            thumb.Dispose();

        }

        //LOAD PROFILE
        //disp();
    }

    protected void nexts(object sender, EventArgs e)
    {
        string ret;
        string filename = "0";
        string contenttype = "0";
        string fileconcat = "";
        if (checkidentity())
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("emp_identity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idnumber", SqlDbType.Int).Value = txt_idnumber.Text;
                    cmd.Parameters.Add("@SAPNumber", SqlDbType.VarChar, 50).Value = txt_sapnumber.Text;
                    cmd.Parameters.Add("@fname", SqlDbType.VarChar, 5000).Value = txt_fname.Text;
                    cmd.Parameters.Add("@lname", SqlDbType.VarChar, 5000).Value = txt_lname.Text;
                    cmd.Parameters.Add("@mname", SqlDbType.VarChar, 5000).Value = txt_mname.Text;
                    cmd.Parameters.Add("@exname", SqlDbType.VarChar, 5000).Value = txt_exname.Text;
                    cmd.Parameters.Add("@datehired", SqlDbType.VarChar, 50).Value = txt_datehired.Text;
                    cmd.Parameters.Add("@empstatus", SqlDbType.VarChar, 50).Value = ddl_status.SelectedValue;
                    cmd.Parameters.Add("@companyid", SqlDbType.Int).Value = ddl_company.SelectedValue;
                    cmd.Parameters.Add("@branchid", SqlDbType.Int).Value = ddl_branch.SelectedValue;
                    cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = ddl_department.SelectedValue;
                    cmd.Parameters.Add("@divid", SqlDbType.Int).Value = ddl_divission.SelectedValue;
                    cmd.Parameters.Add("@divid2", SqlDbType.Int).Value = ddl_divission2.SelectedValue;
                    cmd.Parameters.Add("@secid", SqlDbType.Int).Value = ddl_section.SelectedValue;
                    cmd.Parameters.Add("@posid", SqlDbType.Int).Value = ddl_position.SelectedValue;
                    cmd.Parameters.Add("@pgid", SqlDbType.Int).Value = ddl_pg.SelectedValue;
                    cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value =  btn_identity.Text;
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@employid", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@sbudate", SqlDbType.VarChar, 50).Value = txt_sbudate.Text;
                    cmd.Parameters.Add("@allowoffset", SqlDbType.VarChar, 50).Value = chk_allowoffset.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@allowhdoffset", SqlDbType.VarChar, 50).Value = chk_allowhdoffset.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@allowsc", SqlDbType.VarChar, 50).Value = chk_allow_sc.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@store", SqlDbType.Int).Value = ddl_store.SelectedValue;
                    cmd.Parameters.Add("@health_card", SqlDbType.VarChar, 50).Value = txt_health.Text;
                    cmd.Parameters.Add("@isminimumwageearner", SqlDbType.VarChar, 50).Value = chk_imw.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@surgepay_effective", SqlDbType.VarChar, 50).Value = txt_surge_effective_date.Text;
                    cmd.Parameters.Add("@mop", SqlDbType.VarChar, 50).Value = ddl_mop.SelectedItem.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                    dbhelper.getdata("update Memployee set healthcard_status=NULL where id=" + hdn_empid.Value + " ");
                }
            }
            if (ret != "exist" && ret.Length > 0)
            {
                //tb_tab.Text = "2";
                if (btn_identity.Text == "Update")
                {
                    l_msg.Text = ret;
                }
                else
                {
                    hdn_empid.Value = ret.ToString();
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        filename = Server.MapPath("~/files/peremp/" + hdn_empid.Value + "/Employee_status");
                        using (SqlCommand cmd = new SqlCommand("process_mempstatus", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            fileconcat = ddl_status.Text;
                            cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                            cmd.Parameters.Add("@statusid", SqlDbType.Int).Value = ddl_status.SelectedValue;
                            cmd.Parameters.Add("@effectivedate", SqlDbType.VarChar, 5000).Value = DateTime.Now.ToShortDateString();//effdate[1] + "/" + effdate[0] + "/" + effdate[2];
                            cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = "setup";
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
                    }
                    l_msg.Text = "";
                    txt_idnumber.Enabled = false;
                }
            }
            else
            {
                l_msg.Text = "ID NUMBER is already Exist!";
                hdn_empid.Value = "";
            }
        }

    }
    protected bool checkidentity()
    {
        bool ret = true;

        if (txt_idnumber.Text.Length == 0)
        {
            l_msg.Text = "Id Number Must Be Supplied!";
            ret = false;
        }
        else if (txt_sapnumber.Text.Length == 0)
        {
            l_msg.Text = "SAP Number Must Be Supplied!";
            ret = false;
        }
        else if (ddl_company.SelectedValue == "0")
        {
            l_msg.Text = "Company Must Be Supplied!";
            ret = false;
        }
        else if (ddl_branch.SelectedValue == "0")
        {
            l_msg.Text = "Location Must Be Supplied!";
            ret = false;
        }
        else if (ddl_department.SelectedValue == "0")
        {
            l_msg.Text = "Department Must Be Supplied!";
            ret = false;
        }
        else if (ddl_section.SelectedValue == "0")
        {
            l_msg.Text = "Section Must Be Supplied!";
            ret = false;
        }
        else if (ddl_internalorder.SelectedValue == "0")
        {
            l_msg.Text = "Internal Order Must Be Supplied!";
            ret = false;
        }
        else if (ddl_divission.SelectedValue == "0")
        {
            l_msg.Text = "Band/Level Must Be Supplied!";
            ret = false;
        }
        else if (ddl_pg.SelectedValue == "0")
        {
            l_msg.Text = "Payroll Group Must Be Supplied!";
            ret = false;
        }
        else if (ddl_lvel.SelectedValue == "0")
        {
            l_msg.Text = "Role Must Be Supplied!";
            ret = false;
        }
        else if (txt_datehired.Text.Length < 10)
        {
            l_msg.Text = "Incorrect Date Hired format.";
            ret = false;
        }
        else if (txt_lname.Text.Length == 0)
        {
            l_msg.Text = "Last Name Must Be Supplied!";
            ret = false;
        }
        else if (txt_fname.Text.Length == 0)
        {
            l_msg.Text = "First Name Must Be Supplied!";
            ret = false;
        }
        else if (txt_dob.Text.Length == 0)
        {
            l_msg.Text = "Date of Birth Must Be Supplied!";
            ret = false;
        }
        else if (ddl_status.SelectedValue == "0")
        {
            l_msg.Text = "Employee Status Must Be Supplied!";
            ret = false;
        }
        else if (txt_datehired.Text.Length == 0)
        {
            l_msg.Text = "Date Hired Must Be Supplied!";
            ret = false;
        }
        else if (ddl_divission.SelectedValue == "0")
        {
            l_msg.Text = "Band/Level Must Be Supplied!";
            ret = false;
        }
        else if (ddl_department.SelectedValue == "0")
        {
            l_msg.Text = "Department Must Be Supplied!";
            ret = false;
        }

        if (Request.QueryString["tp"].ToString() == "dd")
        {
            DataTable dt = dbhelper.getdata("select * from memployee where idnumber='" + txt_idnumber.Text.Replace(" ", "") + "'");
            if (dt.Rows.Count > 0)
            {
                l_msg.Text = "Duplicate ID Number";
                ret = false;
            }
        }

        alert.Visible = ret == false ? true : false;

        return ret;
    }
    protected void next1(object sender, EventArgs e)
    {
        string ret;
        if (hdn_empid.Value.Length > 0)
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("master_personal_info", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pressentad", SqlDbType.VarChar, 5000).Value = txt_presentaddres.Text;
                    cmd.Parameters.Add("@pressentadzipcode", SqlDbType.VarChar, 5000).Value = ddl_zipcode.SelectedValue;
                    cmd.Parameters.Add("@phonenumber", SqlDbType.VarChar, 50).Value = txt_pnumber.Text;
                    cmd.Parameters.Add("@cellphone", SqlDbType.VarChar, 50).Value = txt_cnumber.Text;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 5000).Value = txt_email.Text;
                    cmd.Parameters.Add("@pemail", SqlDbType.VarChar, 5000).Value = txt_pemail.Text;
                    cmd.Parameters.Add("@noc", SqlDbType.VarChar, 5000).Value = txt_noc.Text;
                    cmd.Parameters.Add("@dateofbirth", SqlDbType.VarChar, 50).Value = txt_dob.Text;
                    cmd.Parameters.Add("@placeofbirth", SqlDbType.VarChar, 5000).Value = txt_pob.Text;
                    cmd.Parameters.Add("@birthzipcode", SqlDbType.Int).Value = ddl_bzc.SelectedValue;
                    cmd.Parameters.Add("@sex", SqlDbType.VarChar, 50).Value = ddl_sex.Text;
                    cmd.Parameters.Add("@civilstatus", SqlDbType.VarChar, 50).Value = ddl_cs.SelectedValue.Length > 0 ? ddl_cs.SelectedValue : "0";
                    cmd.Parameters.Add("@citizenshipid", SqlDbType.Int).Value = ddl_citizenship.SelectedValue;
                    cmd.Parameters.Add("@relegionid", SqlDbType.Int).Value = ddl_relegion.SelectedValue;
                    cmd.Parameters.Add("@height", SqlDbType.VarChar, 50).Value = txt_height.Text.Length > 0 ? txt_height.Text : "0";
                    cmd.Parameters.Add("@weight", SqlDbType.VarChar, 50).Value = txt_weight.Text.Length > 0 ? txt_weight.Text : "0";
                    cmd.Parameters.Add("@bloodtypeid", SqlDbType.Int).Value = ddl_bloodtype.SelectedValue;
                    cmd.Parameters.Add("@permanentad", SqlDbType.VarChar, 5000).Value = txt_permanentadress.Text;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                    //cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_personal.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }
            if (ret != "exist" && ret.Length > 0)
            {
                //tb_tab.Text = "3";
                hdn_empid.Value = ret.ToString();
                l_msg.Text = "";
            }
            else
                hdn_empid.Value = "";
        }

    }
    protected void next2(object sender, EventArgs e)
    {
        string ret;
        if (checkpayrollinfo())
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("master_payroll_info", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@gsis", SqlDbType.VarChar, 50).Value = txt_gsisno.Text;
                    cmd.Parameters.Add("@sss", SqlDbType.VarChar, 50).Value = txt_sssno.Text;
                    cmd.Parameters.Add("@hdmf", SqlDbType.VarChar, 50).Value = txt_hdmfno.Text;
                    cmd.Parameters.Add("@sssaddon", SqlDbType.VarChar, 50).Value = txt_sssaddon.Text;
                    cmd.Parameters.Add("@hdmfaddon", SqlDbType.VarChar, 50).Value = txt_hdmfaddon.Text;
                    cmd.Parameters.Add("@phic", SqlDbType.VarChar, 50).Value = txt_phicno.Text;
                    cmd.Parameters.Add("@tin", SqlDbType.VarChar, 50).Value = txt_tin.Text;
                    cmd.Parameters.Add("@atm", SqlDbType.VarChar, 50).Value = txt_atm.Text;
                    cmd.Parameters.Add("@taxcodeid", SqlDbType.Int).Value = ddl_taxcode.SelectedValue;
                    cmd.Parameters.Add("@glid", SqlDbType.Int).Value = ddl_gl.SelectedValue;
                    cmd.Parameters.Add("@shiftid", SqlDbType.Int).Value = ddl_shiftcode.SelectedValue;
                    cmd.Parameters.Add("@taxtable", SqlDbType.VarChar, 50).Value = ddl_taxtable.Text;
                    cmd.Parameters.Add("@pgid", SqlDbType.Int).Value = ddl_pg.SelectedValue;
                    cmd.Parameters.Add("@minimum", SqlDbType.VarChar, 50).Value = chk_imw.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@leave", SqlDbType.VarChar, 50).Value = chk_l.Checked == true ? "True" : "False";
                    cmd.Parameters.Add("@mealallow", SqlDbType.VarChar, 50).Value = txt_meal_allow.Text.Length > 0 ? txt_meal_allow.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@nti", SqlDbType.VarChar, 50).Value = txt_nti.Text.Length > 0 ? txt_nti.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"];
                    //cmd.Parameters.Add("@btnstatus", SqlDbType.VarChar, 50).Value = btn_payroll.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }

            //string query = "select * from memployee where ";
            //DataTable dt = dbhelper.getdata(query);
            if (ret != "exist" && ret.Length > 0)
            {
                //tb_tab.Text = "4";
                hdn_empid.Value = ret.ToString();
                l_msg.Text = "";
            }
            else
                hdn_empid.Value = "";
        }

    }
    protected void next3(object sender, EventArgs e)
    {
        string ret;
        if (checkcompensation())
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("master_compensation_info", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ptid", SqlDbType.Int).Value = ddl_payrolltype.SelectedValue;
                    cmd.Parameters.Add("@fnod", SqlDbType.VarChar, 50).Value = txt_fnod.Text.Length > 0 ? txt_fnod.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@fnoh", SqlDbType.VarChar, 50).Value = txt_fnoh.Text.Length > 0 ? txt_fnoh.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@mr", SqlDbType.VarChar, 50).Value = txt_mr.Text.Length > 0 ? txt_mr.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@pr", SqlDbType.VarChar, 50).Value = txt_pr.Text.Length > 0 ? txt_pr.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@dr", SqlDbType.VarChar, 50).Value = txt_dr.Text.Length > 0 ? txt_dr.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@hr", SqlDbType.VarChar, 50).Value = txt_hr.Text.Length > 0 ? txt_hr.Text.Replace(",", "") : "0";
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
            }
        }
    }
    protected bool checkpayrollinfo()
    {
        bool ret = true;
        if (ddl_taxcode.SelectedValue == "0")
        {
            l_msg.Text = "Tax Code Must be supplied!";
            ret = false;
        }
        //else if (ddl_gl.SelectedValue == "0")
        //{
        //    l_msg.Text = "GL Account Must be supplied!";
        //    ret = false;
        //}
        else if (ddl_shiftcode.SelectedValue == "0")
        {
            l_msg.Text = "Shift Code Must be supplied!";
            ret = false;
        }
        else if (ddl_taxtable.Text.Length == 0)
        {
            l_msg.Text = "Tax Table Must be supplied!";
            ret = false;
        }

        alert.Visible = ret == false ? true : false;
        return ret;
    }
    protected bool checkcompensation()
    {
        bool ret = true;

        if (txt_fnod.Text.Length == 0)
        {
            l_msg.Text = "Fix No. of Days Must be supplied!";
            ret = false;
        }
        else if (txt_fnoh.Text.Length == 0)
        {
            l_msg.Text = "Fix No. of Hours Must be supplied!";
            ret = false;
        }
        else if (ddl_payrolltype.SelectedValue == "0")
        {
            l_msg.Text = "Payroll Type Must be supplied!";
            ret = false;
        }
        else if (txt_mr.Text.Length == 0)
        {
            l_msg.Text = "Monthly Rate Must be supplied!";
            ret = false;
        }
        else if (txt_pr.Text.Length == 0)
        {
            l_msg.Text = "Payroll Rate Must be supplied!";
            ret = false;
        }
        else if (txt_dr.Text.Length == 0)
        {
            l_msg.Text = "Daily Rate Must be supplied!";
            ret = false;
        }
        else if (txt_hr.Text.Length == 0)
        {
            l_msg.Text = "Hourly Rate Must be supplied!";
            ret = false;
        }

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

        grid_asset.DataSource = null;
        grid_asset.DataBind();

        grid_fmember1.DataSource = null;
        grid_fmember1.DataBind();

        grid_leavecredits.DataSource = null;
        grid_leavecredits.DataBind();

        gridreport.DataSource = null;
        gridreport.DataBind();

        grid_hmo.DataSource = null;
        grid_hmo.DataBind();

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

        //DataTable getinv = dbhelper.getdata("select a.id,a.serial,a.Description,b.qty from asset_inventory a " +
        //                                     "left join asset_details b on a.id=b.inventid " +
        //                                     "where b.status='On Stock'");

        //txt_sn.Items.Clear();
        //txt_sn.Items.Add(new ListItem("", "0"));
        //foreach (DataRow drr in getinv.Rows)
        //{
        //    txt_sn.Items.Add(new ListItem(drr["serial"].ToString(), drr["id"].ToString()));
        //}
        DataTable getleavetype = dbhelper.getdata("select * from Mleave");
        ddl_leave.Items.Clear();
        ddl_leave.Items.Add(new ListItem("", "0"));
        foreach (DataRow drr in getleavetype.Rows)
        {
            ddl_leave.Items.Add(new ListItem(drr["leave"].ToString(), drr["id"].ToString()));
        }

        DataTable dtinsurance = dbhelper.getdata("select * from Minsurance where action is null");
        //ddl_insurance.Items.Clear();
        //ddl_insurance.Items.Add(new ListItem("", "0"));
        //foreach (DataRow drr in dtinsurance.Rows)
        //{
        //    ddl_insurance.Items.Add(new ListItem(drr["insure_name"].ToString(), drr["id"].ToString()));
        //}
    }
    protected void sn(object sender, EventArgs e)
    {
        //if (txt_sn.SelectedValue == "0")
        //{
        //    l_msg.Text = "Invalid Input!";
        //    txt_desc.Text = "";
        //}
        //else
        //{
        //    DataTable dt = dbhelper.getdata("select * from asset_inventory where id=" + txt_sn.SelectedValue + "");
        //    txt_desc.Text = dt.Rows[0]["Description"].ToString();
        //}
    }
    protected void addss(object sender, EventArgs e)
    {
        DataTable dtskilline = new DataTable();
        if (txt_skill.Text.Length == 0)
            l_msg.Text = "Skill must be supplied!";
        else
        {
            if (hdn_empid.Value.Length > 0)
            {
                dbhelper.getdata("insert into Mskilline (date,empid,userid,skill)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + txt_skill.Text + "')");
                dtskilline = dbhelper.getdata("select * from Mskilline where empid =" + hdn_empid.Value + " and status is null");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        grid_skill1.DataSource = dtskilline;
        grid_skill1.DataBind();
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
        disp();
    }
    protected void addemergencycontact(object sender, EventArgs e)
    {
        if (txtnameemergency.Text == "" && txtaddressemergency.Text == "" && txtcontactemergency.Text == "")
        {

        }
        else
        {
            dbhelper.getdata("insert into EmergencyContact (UserId,EmpId,Name,Address,Contact,Sysdate) values (" + Session["user_id"].ToString() + "," + hdn_empid.Value + ",'" + txtnameemergency.Text + "','" + txtaddressemergency.Text + "','" + txtcontactemergency.Text + "',GETDATE())");
        }
        disp();
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
                dbhelper.getdata("update EmergencyContact set Status = 'Cancel' where Id = " + row.Cells[0].Text + "");
            }
        }
        disp();
    }

    protected void addillness(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MedIllness where Illness like '%" + txtillness.Text + "%'");
        if (txtillness.Text == "")
        { }
        else if (dt.Rows.Count <= 0)
        {
            DataTable dtselect = dbhelper.getdata("insert into MedIllness values ('" + txtillness.Text.Replace("'", "").ToString() + "','ACTIVE')select scope_identity()idd");
        }
        Response.Write("<script>alert('Item has already exists!')</script>");
        Response.Redirect("addemployee?user_id=TIdS9+05Aas=&app_id=" + hdn_empid.Value + "&tp=ed", false);
    }
    protected void savehost(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MedHospital where HostAddress like '%" + txthostadd.Text + "%' and HostDesc like '%" + txthostdesc.Text + "%'");
        if (txthostdesc.Text == "" && txthostadd.Text == "")
        { }
        else if (dt.Rows.Count <= 0)
        {
            DataTable dtselect = dbhelper.getdata("insert into MedHospital values ('" + txthostdesc.Text.Replace("'", "").ToString() + "','" + txthostadd.Text.Replace("'", "").ToString() + "','" + txt_hostcont.Text.Replace("'", "").ToString() + "','Active')");
        }
        Response.Write("<script>alert('Item has already exists!')</script>");
        Response.Redirect("addemployee?user_id=TIdS9+05Aas=&app_id=" + hdn_empid.Value + "&tp=ed", false);
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
                string nameforfile = hdn_empid.Value;

                DataTable dtmedrecords = dbhelper.getdata("insert into MedRecords values('" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + ddl_illness.SelectedValue + "',GETDATE(),'" + txt_meddate.Text + "','" + ddl_medical.SelectedValue + "','" + txt_medphysician.Text + "','" + ddlfindings.SelectedItem + "','" + txt_mednote.Text + "','" + ddlcondition.SelectedItem + "','Has File','Active')select scope_identity() illid");

                DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/medicalfile/") + nameforfile);
                DataTable dtinsertirfile = dbhelper.getdata("insert into MedFile values(GETDATE(),'" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + dtmedrecords.Rows[0]["illid"].ToString() + "') select scope_identity() idd");
                fuhostdoc.SaveAs(Server.MapPath("~/files/medicalfile/") + nameforfile + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + filename.Replace(" ","").ToString() + fileExtension);
            }
            else
            {
                DataTable seletx = dbhelper.getdata("insert into MedRecords values('" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + ddl_illness.SelectedValue + "',GETDATE(),'" + txt_meddate.Text + "','" + ddl_medical.SelectedValue + "','" + txt_medphysician.Text + "','" + ddlfindings.SelectedItem + "','" + txt_mednote.Text + "','" + ddlcondition.SelectedItem + "','No File','Active')select scope_identity() illid");
            }
            disp();
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
                dbhelper.getdata("update MedRecords set [status] = 'Cancel' where ID = " + row.Cells[0].Text + "");
            }
        }
        disp();
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
                string folderdir = hdn_empid.Value;
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
            if (hdn_empid.Value.Length > 0)
            {
                dbhelper.getdata("insert into Mjobhistory (date,empid,userid,position,company,datef,datet)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + txt_position.Text + "','" + txt_company.Text + "','" + txt_month.Text + "/01/" + txt_year.Text + "','" + txt_datetomonth.Text + "/01/" + txt_datetoyear.Text + "')");
                dtjobhistory = dbhelper.getdata("select *,convert(varchar,month(datef))+'/'+ convert(varchar,year(datef))froms, convert(varchar,month(datet))+'/'+ convert(varchar,year(datet))tos from Mjobhistory where empid =" + hdn_empid.Value + " and status is null");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        disp();
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
        disp();
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
    //ubos update
    protected void edhistory(object sender, EventArgs e)
    {
        DataTable dteduchistory = new DataTable();
        if (txt_school.Text.Length == 0 || txt_address.Text.Length == 0 || txt_yearf.Text.Length == 0 || txt_yeart.Text.Length == 0)
            l_msg.Text = "Invalid Input!";
        else
        {
            if (hdn_empid.Value.Length > 0)
            {
                dbhelper.getdata("insert into Meducattainment(date,empid,userid,class,school,address,yearf,yeart)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + ddl_level.Text + "','" + txt_school.Text.Replace("'", "").ToString() + "','" + txt_address.Text.Replace("'", "").ToString() + "','" + txt_yearf.Text + "','" + txt_yeart.Text + "') ");
                dteduchistory = dbhelper.getdata("select *,class level from Meducattainment where empid =" + hdn_empid.Value + " and status is null ");
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }

        grid_educhistory1.DataSource = dteduchistory;
        grid_educhistory1.DataBind();
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
            dtmember = dbhelper.getdata("select * from MFmembers a where a.empid =" + hdn_empid.Value + " and a.status is null");
        }
        grid_fmember1.DataSource = dtmember;
        grid_fmember1.DataBind();
    }
    protected void lc(object sender, EventArgs e)
    {
        //DataTable dtlc = new DataTable();
        //if (ddl_leave.SelectedValue == "0" || txt_noofcredits.Text.Length == 0 || ddl_yyyy.SelectedValue == "0")
        //    l_msg.Text = "Invalid Input!";
        //else
        //{
        //    if (hdn_empid.Value.Length > 0)
        //    {
        //        DataTable dt = dbhelper.getdata("select * from leave_credits where empid= " + hdn_empid.Value + " and leaveid=" + ddl_leave.SelectedValue + " and yyyyear=" + ddl_yyyy.SelectedValue + " and action is null");

        //        if (dt.Rows.Count > 0)
        //        {
        //            l_msg.Text = "Leave Type Exist";
        //            dtlc = dbhelper.getdata("select a.id,b.Leave,a.credit,a.convertocash from leave_credits a left join MLeave b on a.leaveid=b.Id where a.empid =" + hdn_empid.Value + " and a.yyyyear=" + DateTime.Now.Year + " and a.action is null");
        //            grid_leavecredits.DataSource = dtlc;
        //            grid_leavecredits.DataBind();
        //        }
        //        else
        //        {
        //            l_msg.Text = "";
        //            string ctc = chk_tocash.Checked == true ? "yes" : "no";
        //            dbhelper.getdata("insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + ddl_leave.SelectedValue + "','" + txt_noofcredits.Text + "','" + ctc + "','" + ddl_renew.Text + "'," + ddl_yyyy.SelectedValue + ",'2') ");
        //            if (ctc == "yes")
        //            {
        //                ctc = "Convertable to Cash";
        //            }
        //            else
        //                ctc = "Not Convertable to Cash";
        //            using (SqlConnection con = new SqlConnection(dbconnection.conn))
        //            {
        //                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "LCredit";
        //                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = ctc + " for " + ddl_yyyy.SelectedValue;
        //                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_noofcredits.Text + " " + ddl_leave.SelectedItem.ToString();
        //                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Add Leave Credits";
        //                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
        //                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
        //                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //    }
        //    else
        //        l_msg.Text = "Please Proccess Personal Identity first!";
        //}
        //ViewState["leave-credit"] = 1;
        //disp();
        DataTable dtlc = new DataTable();
        if (ddl_leave.SelectedValue == "0" || txt_noofcredits.Text.Length == 0 || ddl_yyyy.SelectedValue == "0")
            l_msg.Text = "Invalid Input!";
        else
        {
            if (hdn_empid.Value.Length > 0)
            {
                DataTable dt = dbhelper.getdata("select * from leave_credits where empid= " + hdn_empid.Value + " and leaveid=" + ddl_leave.SelectedValue + " and yyyyear=" + ddl_yyyy.SelectedValue + " and action is null");

                if (dt.Rows.Count > 0)
                {
                    l_msg.Text = "Leave Type Exist";
                }
                else
                {
                    l_msg.Text = "";
                    string ctc = chk_tocash.Checked == true ? "yes" : "no";
                    dbhelper.getdata("insert into leave_credits values (GETDATE()," + Session["user_id"].ToString() + ",'" + hdn_empid.Value + "','" + ddl_leave.SelectedValue + "','" + txt_noofcredits.Text + "','" + ctc + "','Yearly',NULL,'" + ddl_yyyy.SelectedValue + "','2')");
                    //dbhelper.getdata("insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + ddl_leave.SelectedValue + "','" + txt_noofcredits.Text + "','" + ctc + "','" + ddl_renew.Text + "'," + ddl_yyyy.SelectedValue + ",'2') ");
                    if (ctc == "yes")
                    {
                        ctc = "Convertable to Cash";
                    }
                    else
                        ctc = "Not Convertable to Cash";
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "LCredit";
                            cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = ctc + " for " + ddl_yyyy.SelectedValue;
                            cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_noofcredits.Text + " " + ddl_leave.SelectedItem.ToString();
                            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Add Leave Credits";
                            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
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
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        ViewState["leave-credit"] = 1;
        backlag();
    }
    protected void delete_lc(object sender, EventArgs e)
    {
        DataTable dtlc = new DataTable();
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable leavecrdit = dbhelper.getdata("select * from leave_credits a left join MLeave b on a.leaveid=b.Id where a.id=" + row.Cells[0].Text + "");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "CancelLeave";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "Delete Leave Credits";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = leavecrdit.Rows[0]["Leave"];
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Delete Leave Credits";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("update leave_credits set action='cancel' where id=" + row.Cells[0].Text + "");
        }
        disp();
    }
   //search reportsto
    protected void rt(object sender, EventArgs e)
    {
        //BUTYOK APPROVER
        //DataTable dtrt = new DataTable();
        //if (txt_reportto.Text.Length == 0)
        //    l_msg.Text = "Invalid Input!";
        //else
        //{
        //    if (hdn_empid.Value.Length > 0 || lbl_bals.Value.Length > 0)
        //    {

        //        string ctc = chk_tocash.Checked == true ? "yes" : "no";
        //        DataTable hercnt = dbhelper.getdata("select * from Approver a where a.emp_id =" + hdn_empid.Value + " and a.status is null");

        //        int x = (hercnt.Rows.Count) + 1;

        //         dbhelper.getdata("insert into Approver (date,emp_id,userid,under_id,herarchy)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + lbl_bals.Value + "'," + x + ") ");
        //         DataTable dtreportto = dbhelper.getdata("select LastName+', '+FirstName FullName,* from MEmployee where Id = '" + lbl_bals.Value + "'");
        //         using (SqlConnection con = new SqlConnection(dbconnection.conn))
        //         {
        //             using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
        //             {
        //                 cmd.CommandType = CommandType.StoredProcedure;
        //                 cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddApprover";
        //                 cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Approver";
        //                 cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = dtreportto.Rows[0]["FullName"].ToString();
        //                 cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Add Approver";
        //                 cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
        //                 cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
        //                 cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //                 cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //                 con.Open();
        //                 cmd.ExecuteNonQuery();
        //                 con.Close();
        //             }
        //         }
        //    }
        //    else
        //        l_msg.Text = "Please Proccess Personal Identity first!";
        //}

        //Approver();
        //ViewState["approver"] = 1;

        string query = "";
        string ee;
        DataTable hercnt = dbhelper.getdata("select * from Approver a where a.emp_id =" + hdn_empid.Value + " and a.status is null and herarchy <> 0");

        if (txt_reportto.Text.Length == 0)
            l_msg.Text = "Invalid Input!";
        else
        {
            if (hdn_empid.Value.Length > 0 || lbl_bals.Value.Length > 0)
            {
                if (chck_scheduler.Checked == true)
                {
                    query = "insert into Approver(date,emp_id,userid,under_id,herarchy)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + lbl_bals.Value + "',0)";
                }
                else
                {
                    int x = (hercnt.Rows.Count) + 1;
                    query = "insert into Approver(date,emp_id,userid,under_id,herarchy)values(getdate()," + hdn_empid.Value + "," + Session["user_id"].ToString() + ",'" + lbl_bals.Value + "'," + x + ")";


                    DataTable dtttts = dbhelper.getdata("select under_id,* from Approver where emp_id = " + hdn_empid.Value + " and herarchy = 1 ");
                    if (dtttts.Rows.Count > 0)
                    {
                        ee = dtttts.Rows[0]["under_id"].ToString();
                        query = "Update tmanuallogline set approver_id = " + ee + ",status = 'for approval' where EmployeeId = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update TOverTimeLine set status = 'for approval',approver_id = " + ee + " where EmployeeId = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update TRestdaylogs set status = 'for approval',approver_id = " + ee + " where EmployeeId = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update Ttravel set status = 'for approval',approver_id = " + ee + " where emp_id = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update Tundertime set status = 'for approval',approver_id = " + ee + " where emp_id = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update temp_shiftcode set status = 'for approval',approver_id = " + ee + " where emp_id = " + hdn_empid.Value + " and status = 'for approval' and approver_id = " + ee + ""
                            + " Update toffset set status = 'for approval',aproverid=" + ee + " where empid = " + hdn_empid.Value + " and status = 'for approval' and aproverid = " + ee + ""
                            + " BEGIN TRANSACTION update a set a.status = 'for approval' from TLeaveApplicationLine a inner join Tleave b on b.id = a.l_id where b.approver_id=" + hdn_empid.Value + ""
                            + " update b set b.approver_id = " + ee + " from Tleave b inner join TLeaveApplicationLine a on b.id = a.l_id where b.approver_id=" + ee + " and status = 'for approval' COMMIT "
                            + " Update texitclearance set status = 'for approval',nextapproverid = " + ee + " where empid = " + hdn_empid.Value + " and status = 'for approval' and nextapproverid = " + ee + ""
                            + " Update tfinalexit set status = 'for approval',nextapproverid = " + ee + " where empid = " + hdn_empid.Value + " and status = 'for approval' and nextapproverid = " + ee + "";

                        DataTable dt = dbhelper.getdata(query);
                        Response.Redirect(Request.RawUrl.ToString());
                    }
                }

                dbhelper.getdata(query);

                DataTable dtreportto = dbhelper.getdata("select LastName+', '+FirstName FullName,* from MEmployee where Id = '" + lbl_bals.Value + "'");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "AddApprover";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "New Approver";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = dtreportto.Rows[0]["FullName"].ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Add Approver";
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["emp_id"];
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }

        Approver();
        ViewState["approver"] = 1;
    }

    
    protected void delete_rt(object sender, EventArgs e)
    {
        //BUTYOK APPROVER
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            DataTable dtreportto = dbhelper.getdata("select a.LastName+', '+a.FirstName FullName,* from MEmployee a left join Approver b on a.Id=b.under_id where b.id = " + row.Cells[0].Text + "");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "CancelApprover";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "Remove Approver";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = dtreportto.Rows[0]["FullName"].ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Remove Approver";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dbhelper.getdata("delete from Approver where id=" + row.Cells[0].Text + "");
            Approver();

            int i = 0;
            string query = string.Empty;
            DataTable dt = ViewState["tblApprover"] as DataTable;
            foreach (DataRow result in dt.Rows)
            {
                query += "update Approver set herarchy=" + i + " where id=" + result[0] + ";";
                i++;
            }

            if (query.Length > 0)
            {
                dbhelper.getdata(query);
                Approver();
            }
        }
        
    }

    protected void get_emp(object sender, EventArgs e)
    {
        chk_ins_payable.Checked = false;
        if (ddl_mt.SelectedValue == "Principal")
        {
            DataTable dt = getdata.empsearch("", hdn_empid.Value);
            if (dt.Rows.Count > 0)
            {
                txt_ins_fname.Text = dt.Rows[0]["firstname"].ToString();
                txt_ins_lname.Text = dt.Rows[0]["lastname"].ToString();
                txt_ins_mname.Text = dt.Rows[0]["middlename"].ToString();
                txt_rtp.Text = "Principal";
                txt_ins_mname.Enabled = false;
                txt_ins_lname.Enabled = false;
                txt_ins_fname.Enabled = false;
                txt_rtp.Enabled = false;
            }
        }
        else
        {
            txt_ins_mname.Enabled = true;
            txt_ins_lname.Enabled = true;
            txt_ins_fname.Enabled = true;
            txt_rtp.Enabled = true;
            txt_ins_mname.Text = "";
            txt_ins_lname.Text = "";
            txt_ins_fname.Text = "";
            txt_rtp.Text = "";
        }
    }
    protected void hmo(object sender, EventArgs e)
    {
        DataTable dthmo = new DataTable();
        string ret;
        if (errhmo())
        {
            if (hdn_empid.Value.Length > 0 || lbl_bals.Value.Length > 0)
            {
                string ctc = chk_ins_payable.Checked == true ? "yes" : "no";
                decimal amort = 0;
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("allow_hmo", con))
                    {
                        string uu = Session["user_id"].ToString();
                        if (ctc == "yes")
                        {
                            DateTime start = DateTime.Parse(txt_cdf.Text);
                            DateTime end = DateTime.Parse(txt_cdt.Text);
                            int nom = GetMonthsDiff(start, end);
                            amort = decimal.Parse(txt_pa.Text.Replace(",", "")) / nom;
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                        cmd.Parameters.Add("@prinid", SqlDbType.Int).Value = hdn_empid.Value;
                        cmd.Parameters.Add("@membertype", SqlDbType.VarChar, 5000).Value = ddl_mt.Text;
                        cmd.Parameters.Add("@fname", SqlDbType.VarChar, 5000).Value = txt_ins_fname.Text;
                        cmd.Parameters.Add("@mname", SqlDbType.VarChar, 5000).Value = txt_ins_mname.Text;
                        cmd.Parameters.Add("@lname", SqlDbType.VarChar, 5000).Value = txt_ins_lname.Text;
                        cmd.Parameters.Add("@reltoprincipal", SqlDbType.VarChar, 5000).Value = txt_rtp.Text;
                        cmd.Parameters.Add("@insurancename", SqlDbType.VarChar, 5000).Value = txt_insurance.Text;
                        cmd.Parameters.Add("@roomcategory", SqlDbType.VarChar, 5000).Value = txt_rc.Text;
                        cmd.Parameters.Add("@contractno", SqlDbType.VarChar, 50).Value = txt_cn.Text;
                        cmd.Parameters.Add("@coverage_date_from", SqlDbType.VarChar, 50).Value = txt_cdf.Text;
                        cmd.Parameters.Add("@coverage_date_to", SqlDbType.VarChar, 50).Value = txt_cdt.Text;
                        cmd.Parameters.Add("@total_limit_amt", SqlDbType.VarChar, 50).Value = txt_la.Text.Replace(",", "");
                        cmd.Parameters.Add("@total_limit_premium", SqlDbType.VarChar, 50).Value = txt_pa.Text.Replace(",", "");
                        cmd.Parameters.Add("@remarks", SqlDbType.VarChar, 5000).Value = txt_remarks.Text;
                        cmd.Parameters.Add("@payable", SqlDbType.VarChar, 50).Value = ctc;
                        cmd.Parameters.Add("@amortization", SqlDbType.VarChar, 50).Value = amort.ToString().Replace(",", "");
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        ret = cmd.Parameters["@out"].Value.ToString();
                        con.Close();
                    }
                }
                if (ret != "exist" && ret.Length > 0)
                {
                    dthmo = dbhelper.getdata("select *,left(convert(varchar,a.coverage_date_to,101),10)coverage_date_to1,left(convert(varchar,a.coverage_date_from,101),10)coverage_date_from1, b.insure_name insurance from Mhmo a left join Minsurance b on a.insid=b.id where a.action is null and  a.prinid =" + hdn_empid.Value + " ");
                    lbl_bals.Value = "";
                }
                else
                {
                    dthmo = dbhelper.getdata("select *,left(convert(varchar,a.coverage_date_to,101),10)coverage_date_to1,left(convert(varchar,a.coverage_date_from,101),10)coverage_date_from1, b.insure_name insurance from Mhmo a left join Minsurance b on a.insid=b.id where a.action is null and  a.prinid =" + hdn_empid.Value + " ");
                    l_msg.Text = "Data is already Exist!";
                }

            }
            else
                l_msg.Text = "Please Proccess Personal Identity first!";
        }
        grid_hmo.DataSource = dthmo;
        grid_hmo.DataBind();
    }
    protected void delete_hmo(object sender, EventArgs e)
    {
        DataTable dthmo = new DataTable();
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update Mhmo set action='cancel' where id=" + row.Cells[0].Text + "");
            dthmo = dbhelper.getdata("select *,left(convert(varchar,a.coverage_date_to,101),10)coverage_date_to1,left(convert(varchar,a.coverage_date_from,101),10)coverage_date_from1,b.insure_name insurance from Mhmo a left join Minsurance b on a.insid=b.id where a.action is null and  a.prinid =" + hdn_empid.Value + " ");
        }
        grid_hmo.DataSource = dthmo;
        grid_hmo.DataBind();
    }
    protected bool errhmo()
    {
        bool ret = true;
        if (ddl_mt.SelectedValue.Length == 0)
        {
            l_msg.Text = "Member Type must be required!";
            ret = false;
        }
        else if (txt_lname.Text.Length == 0)
        {
            l_msg.Text = "Last Name must be required!";
            ret = false;
        }
        else if (txt_fname.Text.Length == 0)
        {
            l_msg.Text = "First Name must be required!";
            ret = false;
        }
        else if (txt_mname.Text.Length == 0)
        {
            l_msg.Text = "Middle Name must be required!";
            ret = false;
        }
        else if (txt_rtp.Text.Length == 0)
        {
            l_msg.Text = "Relationship to Principal must be required!";
            ret = false;
        }
        //else if (ddl_insurance.SelectedValue == "0")
        //{
        //    l_msg.Text = "Insurance Name must be required!";
        //    ret = false;
        //}
        else if (txt_rc.Text.Length == 0)
        {
            l_msg.Text = "Room Category must be required!";
            ret = false;
        }
        else if (txt_cn.Text.Length == 0)
        {
            l_msg.Text = "Contract Number must be required!";
            ret = false;
        }
        else if (txt_cdf.Text.Length == 0)
        {
            l_msg.Text = "Coverage Date From must be required!";
            ret = false;
        }
        else if (txt_cdt.Text.Length == 0)
        {
            l_msg.Text = "Coverage Date To must be required!";
            ret = false;
        }
        else if (txt_la.Text.Length == 0)
        {
            l_msg.Text = "Limit Amount must be required!";
            ret = false;
        }
        else if (txt_pa.Text.Length == 0)
        {
            l_msg.Text = "Premium Amount must be required!";
            ret = false;
        }
        else if (txt_remarks.Text.Length == 0)
        {
            l_msg.Text = "Remarks must be required!";
            ret = false;
        }
        return ret;
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
        div_gen2316.Visible = false;
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
            div_status.Style.Add("width", "415px");
            div_status.Style.Add("top", "30px");
            div_status.Style.Add("left", "450px");
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
            if (FileUpload2.HasFile)
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
                            filename = Server.MapPath("~/files/peremp/" + Request.QueryString["app_id"].ToString() + "/Clearance");
                            using (SqlCommand cmd = new SqlCommand("quit_claim", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                fileconcat = "Clearance";
                                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["app_id"].ToString();
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
                Response.Redirect("addemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Request.QueryString["app_id"].ToString() + "&tp=ed");

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


    }

    protected void Approver()
    {
        //BUTYOK APPROVER
        DataTable dt = getdata.EmpApprover(hdn_empid.Value);
        gridreport.DataSource = dt;
        gridreport.DataBind();
        ViewState["approver"] = dt.Rows.Count;
        ViewState["tblApprover"] = dt;
    
        if (dt.Rows.Count > 0)
        {
            LinkButton lnkUp = (gridreport.Rows[0].FindControl("lnkUp") as LinkButton);
            LinkButton lnkDown = (gridreport.Rows[gridreport.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
            lnkUp.Enabled = false;
            lnkUp.CssClass = "disabled";
            lnkDown.Enabled = false;
            lnkDown.CssClass = "disabled";
        }
    }

    protected void notePagination(object sender, GridViewPageEventArgs e)
    {
        gvNote.PageIndex = e.NewPageIndex;
        ShowNote();
    }

    protected void auditPagination(object sender, GridViewPageEventArgs e)
    {
        gridaudit.PageIndex = e.NewPageIndex;
        ShowTrail();
    }

    protected void ShowTrail()
    {
        string query = "select a.Id, a.empid,(select name from nobel_user where emp_id = a.UserId)UserId, (select LastName+', '+FirstName from MEmployee where Id = a.EmpId)FullName, a.Transact, a.Subject, a.Particular, a.Action, a.AlterF, a.AlterT, a.Remarks, " +
        "LEFT(convert(varchar, a.Sysdate,101),10)Sysdate " +
        "from audittrail a " +
        "where a.EmpId=" + hdn_empid.Value + " " +
        "and a.UserId = 1 " +
        "order by a.Sysdate desc";
        DataTable dt = dbhelper.getdata(query);
        gridaudit.DataSource = dt;
        gridaudit.DataBind();
    }

    protected void ShowNote()
    {
        DataTable dt = dbhelper.getdata("select * from MEmployeeNote where empID=" + hdn_empid.Value + " and status=0 order by tdate desc");
        gvNote.DataSource = dt;
        gvNote.DataBind();
    }

    protected void alldisp()
    {
        /**
         * BUTYOK
         * Remove ang approver nag grid bind
         * **/
        Approver();

        DataTable dt;

        dt = dbhelper.getdata("Select * from SetUpTable");
        if (dt.Rows[0]["LeaveType"].ToString() == "4")
        {


            string[] twos = SetUp.leavetype(hdn_empid.Value, DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

            DataTable dpres = dbhelper.getdata(twos[0]);
            DataTable dprev = dbhelper.getdata(twos[1]);

            for (int i = 0; i < dpres.Rows.Count; i++)
            {
                double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                double diff = asd <= 0 ? 0 : asd;
                if (diff > 5)
                {
                    diff = 5;
                }
                for (int j = 0; j < dprev.Rows.Count; j++)
                {
                    if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                    {
                        dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";

                    }
                }
            }

            dpres.Merge(dprev);

            grid_leavecredits.DataSource = dpres;
            grid_leavecredits.DataBind();
            ViewState["leave-credit"] = dpres.Rows.Count;
        }
        else
        {
            string query = SetUp.leavetype(hdn_empid.Value, DateTime.Now.Year.ToString(), "");
            dt = dbhelper.getdata(query);
            grid_leavecredits.DataSource = dt;
            grid_leavecredits.DataBind();
        }
        //DataTable dt = dbhelper.getdata("select a.id,b.Leave,a.credit,a.convertocash from leave_credits a left join MLeave b on a.leaveid=b.Id where a.empid =" + hdn_empid.Value + " and a.yyyyear=" + DateTime.Now.Year + " and a.action is null");
        //grid_leavecredits.DataSource = dt;
        //grid_leavecredits.DataBind();
        //ViewState["leave-credit"] = dt.Rows.Count;

        dt = dbhelper.getdata("select * from MFmembers a where a.empid =" + hdn_empid.Value + " and a.status is null");
        grid_fmember1.DataSource = dt;
        grid_fmember1.DataBind();

        string query1 = "select a.empid,d.inventid, a.id, b.Description cat, a.qty,c.propertycode, a.serialno, c.Description name, b.id, b.um, case when a.serialno is not null then c.Description else b.Description end description from asset_assign a left join asset_cat b on a.categoryid = b.id left join asset_inventory c on c.id = a.invid left join asset_details d on d.inventid = a.invid where a.action is null and a.status is not null and a.status != 'Returned' and a.empid = " + hdn_empid.Value + "";
        DataTable dtasset = dbhelper.getdata(query1);
        grid_asset.DataSource = dtasset;
        grid_asset.DataBind();
        if (dtasset.Rows.Count > 0)
        {
            ViewState["assign"] = dtasset.Rows[0]["id"].ToString();
            ViewState["detailid"] = dtasset.Rows[0]["inventid"].ToString();
            ViewState["empid"] = dtasset.Rows[0]["empid"].ToString();
        }

        dt = dbhelper.getdata("select *,class level from Meducattainment where empid =" + hdn_empid.Value + " and status is null ");
        grid_educhistory1.DataSource = dt;
        grid_educhistory1.DataBind();

        dt = dbhelper.getdata("select *,convert(varchar,month(datef))+'/'+ convert(varchar,year(datef))froms, convert(varchar,month(datet))+'/'+ convert(varchar,year(datet))tos from Mjobhistory where empid =" + hdn_empid.Value + " and status is null");
        grid_jobhistory1.DataSource = dt;
        grid_jobhistory1.DataBind();

        dt = dbhelper.getdata("select * from Mskilline where empid =" + hdn_empid.Value + " and status is null");
        grid_skill1.DataSource = dt;
        grid_skill1.DataBind();

        dt = dbhelper.getdata("select  a.id, b.id fileid, a.seminarsattended, a.seminarsheld,LEFT(convert(varchar, dateconducted,101),10) as dateconducted from Mseminarattended a left join MseminarattndFile b on a.id = b.said  where a.empid =" + hdn_empid.Value + " and a.status is null");
        grid_seminarsattended1.DataSource = dt;
        grid_seminarsattended1.DataBind();

        dt = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + hdn_empid.Value + " and action = 'Active' order by id desc");
        grid_sanctions.DataSource = dt;
        grid_sanctions.DataBind();

        dt = dbhelper.getdata("select * from EmergencyContact where Status is NULL and EmpId = " + hdn_empid.Value + "");
        grid_emergencycontact.DataSource = dt;
        grid_emergencycontact.DataBind();

        dt = dbhelper.getdata("select a.Id,(select illness from MedIllness where Id = a.saidid)illness,(select HostDesc from MedHospital where Id = a.hospital)hospital,LEFT(convert(varchar, a.meddate,101),10)meddate, a.doctor, a.findings, a.condition from MedRecords a where a.empid = '" + hdn_empid.Value + "' and a.status = 'Active' order by sysdate desc");
        gridmedrics.DataSource = dt;
        gridmedrics.DataBind();

        dt = dbhelper.getdata("select Id,(select LastName+', '+FirstName from MEmployee where Id = EmpId)Employee,LicenseNum,Left(convert(varchar,ValidUntil,101),10)ValidUntil,Location,Cost,Left(convert(varchar,DurationFrom,101),10)DF,Left(convert(varchar,DurationTo,101),10)DT,Provider,About,Resources,Left(convert(varchar,Sysdate,101),10)Sysdate from Preparatory a where EmpId = '" + hdn_empid.Value + "' and Action is NULL");
        grid_preparatory.DataSource = dt;
        grid_preparatory.DataBind();

        DataTable empdet = dbhelper.getdata("select * from memployee where id=" + hdn_empid.Value + "");

        query1 = "select a.empstatid, left(CONVERT(varchar,a.datechange,101),10)date_change,b.status,left(convert(varchar,a.effectivedate,101),10)effectivedate, " +
                "a.notes " +
                "from memployeestatus a " +
                "left join mempstatus_setup b on a.statusid=b.id " +
                "where a.empid=" + hdn_empid.Value + " " +
                "order by a.empstatid  desc";
        dt = dbhelper.getdata(query1);
        if (dt.Rows.Count == 0)
        {
            query1 = "select '0'empstatid,'--'date_change,status,'--'effectivedate,'--'notes from mempstatus_setup where id=" + empdet.Rows[0]["emp_status"].ToString() + "";
            dt = dbhelper.getdata(query1);
        }

        grid_status_details.DataSource = dt;
        grid_status_details.DataBind();

        DataTable dtcompensationmovement = dbhelper.getdata("select a.id,a.app_trn_id,c.PayrollType,a.fnod,a.fnoh,(SELECT CONVERT(varchar,CONVERT(money,a.mr),1))mr," +
                                        "(SELECT CONVERT(varchar,CONVERT(money,a.pr),1))pr,(SELECT CONVERT(varchar,CONVERT(money,a.dr),1))dr,(SELECT CONVERT(varchar," +
                                        "CONVERT(money,a.hr),1))hr,case when Year(LEFT(CONVERT(varchar,a.effective_date,101),10))='1900'then " +
                                        "LEFT(CONVERT(varchar,d.DateHired,101),10)else LEFT(CONVERT(varchar,a.effective_date,101),10)end effective_date from " +
                                        "app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id left join MPayrollType c on a.paytypeid=c.Id left join MEmployee d" +
                                        " on d.Id=b.empid where b.empid=" + hdn_empid.Value + " order by a.id desc");
        grid_compensation.DataSource = dtcompensationmovement;
        grid_compensation.DataBind();

        DataTable dtpromotionmovement = dbhelper.getdata("select a.id,a.app_trn_id,c.Position,LEFT(CONVERT(varchar,a.effective_date,101),10)effective_date  from app_trn_promotion a " +
                                        "left join app_trn b on a.app_trn_id=b.id " +
                                        "left join MPosition c on a.positionid=c.Id " +
                                        "where b.empid=" + hdn_empid.Value + " order by a.id desc ");
        grid_promotion.DataSource = dtpromotionmovement;
        grid_promotion.DataBind();

        DataTable dtregularmovement = dbhelper.getdata("select a.id,a.app_trn_id,c.status,LEFT(CONVERT(varchar,a.effective_date,101),10)effective_date  from app_trn_regularization a " +
                                       "left join app_trn b on a.app_trn_id=b.id " +
                                       "left join mempstatus_setup c on a.statusid=c.id " +
                                       "where b.empid=" + hdn_empid.Value + " order by a.id desc ");

        grid_regularization.DataSource = dtregularmovement;
        grid_regularization.DataBind();
    }

    protected void ChangePreference(object sender, EventArgs e)
    {
        string commandArgument = (sender as LinkButton).CommandArgument;

        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        int locationId = Convert.ToInt32(gridreport.Rows[rowIndex].Cells[0].Text);
        int preference = Convert.ToInt32(gridreport.Rows[rowIndex].Cells[1].Text);
        preference = commandArgument == "up" ? preference - 1 : preference + 1;
        this.UpdatePreference(locationId, preference, Convert.ToInt32(gridreport.Rows[rowIndex].Cells[2].Text));

        rowIndex = commandArgument == "up" ? rowIndex - 1 : rowIndex + 1;
        locationId = Convert.ToInt32(gridreport.Rows[rowIndex].Cells[0].Text);
        preference = Convert.ToInt32(gridreport.Rows[rowIndex].Cells[1].Text);
        preference = commandArgument == "up" ? preference + 1 : preference - 1;
        this.UpdatePreference(locationId, preference, Convert.ToInt32(gridreport.Rows[rowIndex].Cells[2].Text));

        Approver();
    }

    private void UpdatePreference(int locationId, int preference, int employee)
    {
        /**
         * IF EVER WALA NA NAY NAKA TAG SA EMPLOYEE AS APPROVER
         * ANG SITE MASTER TRAP NA DLI MA SHOW ANG SCHEDULER NA TAB
         * 
         * "UPDATE nobel_user set acc_id=" + (preference == 0 ? "6" : "4") + " where emp_id=" + employee + ";";
         * **/

        dbhelper.getdata("UPDATE Approver SET herarchy = " + preference + " WHERE Id =" + locationId);
        
    }

    protected void item()
    {
        string query = "select catid,convert(varchar,itemid)+'-qty'+qty itemid,Description,qty from  " +
                        "( " +
                        "select a.id catid, b.id itemid,'Category:'+a.description+' Serial:'+b.serial +' Product Description:'+b.description+' Qty:'+ convert(varchar,cast(round(c.qty,0)as numeric(16,0)))+' '+a.um Description,convert(varchar,cast(round(c.qty,0)as numeric(16,0))) qty " +
                        "from asset_cat a " +
                        "left join asset_inventory b on a.id=b.categoryid " +
                        "left join asset_details c on b.id=c.inventid " +
                        "where a.serialized=1 and b.action is null and c.status='On Stock' " +
                        "UNION " +
                        "select a.id catid, '0'itemid,'Category:'+a.description+' Serial:- Product Description:- Qty:'+convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id))+' '+a.um Description, " +
                        "convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id)) qty " +
                        "from asset_cat a " +
                        "where a.serialized=0 " +
                        ") tt where catid=" + ddl_cat.SelectedValue + " and convert(decimal,qty)>0";

        DataTable cattegory = dbhelper.getdata(query);
        txt_ser.Items.Clear();
        txt_ser.Items.Add(new ListItem("Select", ""));
        foreach (DataRow drcat in cattegory.Rows)
        {
            txt_ser.Items.Add(new ListItem(drcat["Description"].ToString(), drcat["itemid"].ToString()));
        }

        DataTable dt = dbhelper.getdata("select * FROM asset_cat where id=" + ddl_cat.SelectedValue + "");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["serialized"].ToString() == "False")
                txt_quantity.Enabled = true;
            else
            {
                txt_quantity.Enabled = false;
                txt_quantity.Text = "1";
            }
        }
        if (cattegory.Rows.Count == 0)
        {
            Button4.Enabled = false;
            txt_quantity.Text = "0";
        }
    }
    protected void select_category(object sender, EventArgs e)
    {
        item();
    }
    protected void asset_assign(object sender, EventArgs e)
    {
        if (hdn_empid.Value.Length > 0)
        {
            string query = "select catid,convert(varchar,itemid)+'-qty'+qty itemid,Description,qty,serial,itemid inventoryid from(select a.id catid, b.id itemid,b.serial,a.Description name, b.description Description,convert(varchar,cast(round(c.qty,0)as numeric(16,0)))qty from asset_cat a left join asset_inventory b on a.id=b.categoryid left join asset_details c on c.inventid = b.id where a.serialized=1 and b.action is null and c.status='On Stock' UNION select a.id catid,b.id itemid,b.serial,a.Description name,'Category:'+a.description+' Serial:- Product Description:- Qty:'+convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty)end qty from asset_assign where b.categoryid=a.id and action is null and status is null)from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id))+' '+a.um Description,convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty)end qty from asset_assign where categoryid=a.id and action is null and status is null)from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id))qty from asset_cat a left join asset_inventory b on a.id = b.categoryid where a.serialized=0 ) tt where catid = " + ddl_cat.SelectedValue + " and convert(decimal,qty)>0";
            DataTable dt = dbhelper.getdata(query);
            if (dt.Rows.Count <= 0)
            {
                Response.Write("<script>alert('Zero Inventory!')</script>");
            }
            else
            {
                string available = dt.Rows[0]["qty"].ToString();
                string deployed = txt_quantity.Text;
                decimal total = decimal.Parse(available) - decimal.Parse(deployed);

                if (decimal.Parse(available) >= decimal.Parse(deployed) && txt_quantity.Text.Length > 0 && int.Parse(ddl_cat.SelectedValue) > 0)
                {
                    query = "Insert into asset_assign([date],invid,empid,userid,useridlastupdate,datelastupdate,serialno,[description],itemname,qty,categoryid,status)"
                        + "values" +
                       "(GETDATE()," + dt.Rows[0]["inventoryid"].ToString() + "," + hdn_empid.Value + "," + Session["user_id"].ToString() + "," + Session["user_id"].ToString() + ",GETDATE(),'" + dt.Rows[0]["serial"].ToString() + "','" + dt.Rows[0]["Description"].ToString() + "','" + dt.Rows[0]["Description"].ToString() + "','" + txt_quantity.Text + "','" + ddl_cat.SelectedValue.ToString() + "','Deployed')update asset_details set status='Deployed',qty = '" + total.ToString() + "' where inventid = '" + dt.Rows[0]["inventoryid"].ToString() + "' ";
                    dbhelper.getdata(query);
                }
                else
                {
                    Response.Write("<script>alert('Exceed Quantity!')</script>");
                }
            }
        }
        alldisp();
    }

    protected void delete_asset_assign(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                DataTable dtreturn = dbhelper.getdata("select * from asset_assign where empid = '" + ViewState["empid"].ToString() + "' and id = '" + ViewState["assign"].ToString() + "'");
                DataTable dtdetails = dbhelper.getdata("select * from asset_details where inventid = " + ViewState["detailid"].ToString() + "");
                string assign = dtreturn.Rows[0]["qty"].ToString();
                string bal = dtdetails.Rows[0]["qty"].ToString();
                decimal total = decimal.Parse(assign) + decimal.Parse(bal);

                dbhelper.getdata("update asset_assign set action='cancel',useridlastupdate=" + Session["user_id"].ToString() + ",datelastupdate=getdate(),status='Cancel' where empid = '" + ViewState["empid"].ToString() + "' and id=" + ViewState["assign"].ToString() + "");
                dbhelper.getdata("update asset_details set status='On Stock',qty='" + total.ToString() + "' where inventid=" + ViewState["detailid"].ToString() + "");
            }
        }
        alldisp();
    }

    protected void return_asset_assign(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            string query = "";
            DataTable dt;
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                DataTable dtreturn = dbhelper.getdata("select * from asset_assign where empid = '" + ViewState["empid"].ToString() + "' and id = '" + ViewState["assign"].ToString() + "'");
                DataTable dtdetails = dbhelper.getdata("select * from asset_details where inventid = " + ViewState["detailid"].ToString() + "");
                string assign = dtreturn.Rows[0]["qty"].ToString();
                string available = dtdetails.Rows[0]["qty"].ToString();
                decimal total = decimal.Parse(assign) + decimal.Parse(available);

                query = "update asset_assign set status='Returned',useridlastupdate=" + Session["user_id"].ToString() + ",datelastupdate=getdate() where empid = '" + ViewState["empid"].ToString() + "' and id='" + ViewState["assign"].ToString() + "'";
                dt = dbhelper.getdata(query);
                query = "update asset_details set status='On Stock',qty='" + total.ToString() + "' where inventid=" + ViewState["detailid"].ToString() + "";
                dt = dbhelper.getdata(query);
            }
        }
        alldisp();
    }

    protected void asset_redirect(object sender, EventArgs e)
    {
        Response.Redirect("assetassign");
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

            DataTable dtinsert = dbhelper.getdata("insert into Mseminarattended (date,empid,userid,seminarsattended,dateconducted,status,seminarsheld,fileattached)values ('" + DateTime.Now.ToString() + "','" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + txt_seminar.Text.Replace(" ", " ") + "','" + txt_dateseminars.Text + "',NULL,'" + txt_heldat.Text.Trim() + "','Without Attachment') select scope_identity() id");
            if (fuseminarsphoto.HasFile)
            {
                string nameofiles = hdn_empid.Value;
                DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/seminarfiles/") + nameofiles);
                string query = "insert into MseminarattndFile(dateupload, empid, userid, location, extension, status, contenttype, filename, action,said) values('" + DateTime.Now.ToString() + "','" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','c','" + fileExtension + "','Confirmed Uploaded','" + contenttype + "','" + filename + "','Active'," + dtinsert.Rows[0]["id"].ToString() + ") select scope_identity() id";
                DataTable dt = dbhelper.getdata(query);
                fuseminarsphoto.SaveAs(Server.MapPath("~/files/seminarfiles/") + nameofiles + "/" + dt.Rows[0]["id"].ToString() + fileExtension);
            }

        }
        dtaddseminar = dbhelper.getdata("select  a.id, b.id fileid, a.seminarsattended, a.seminarsheld,LEFT(convert(varchar, dateconducted,101),10) as dateconducted from Mseminarattended a left join MseminarattndFile b on a.id = b.said  where a.empid =" + hdn_empid.Value + " and a.status is null");
        grid_seminarsattended1.DataSource = dtaddseminar;
        grid_seminarsattended1.DataBind();
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
                dbhelper.getdata("update Mseminarattended set status='cancel',lastupdateuserid=" + Session["user_id"].ToString() + ",lastupdatedate=getdate() where id=" + row.Cells[0].Text + "");
                dbhelper.getdata("update MseminarattndFile set action='Deleted' where said=" + row.Cells[0].Text + "");
            }
        }
        disp();
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
            string fileout = Server.MapPath("~/files/seminarfiles/" + Request.QueryString["app_id"].ToString() + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();
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

    protected void addsanctions(object sender, EventArgs e)
    {
        DataTable dtaddsanctions = new DataTable();
        if (ddlsanctioncodes.SelectedItem.Text == "Sanctions/s") l_msg.Text = "Sanctions must be supplied!";
        else if (txt_incidentdate.Text.Length == 0) l_msg.Text = "Incident Date must be supplied!";
        else if (txt_incidentabout.Text.Length == 0) l_msg.Text = "Incident About must be supplied!";
        else if (txt_incidentremarks.Text.Length == 0) l_msg.Text = "Remarks must be supplied!";
        else
        {
            if (hdn_empid.Value.Length > 0)
            {
                DataTable dtid = dbhelper.getdata("insert into sanctionentry (dateapply, sanctioncode, empid, userid, rootcause, action, incidentdate, status, stat, remarks, cancelledate) values ('" + DateTime.Now.ToString() + "','" + ddlsanctioncodes.SelectedValue + "'," + hdn_empid.Value + ",'" + Session["user_id"].ToString() + "','" + txt_incidentabout.Text.Replace("'", "") + "','Active','" + txt_incidentdate.Text + "','Draft',NULL,'" + txt_incidentremarks.Text.Replace("'", "") + "',NULL) select scope_identity() as santionid ");
                DataTable suspensiondate = ViewState["dtdraft"] as DataTable;
                foreach (DataRow dr in suspensiondate.Rows)
                {
                    dbhelper.getdata("insert into suspension(empid, userid, date, sanctionid, code, suspendate, action, canceldate) values ('" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + DateTime.Now.ToString() + "', '" + dtid.Rows[0]["santionid"].ToString() + "','" + ddlsanctioncodes.SelectedValue + "','" + dr["suspendate"] + "','Active',Null)");
                }
                Response.Redirect("Memo?empid=" + hdn_empid.Value + "&entid=" + dtid.Rows[0]["santionid"].ToString() + "", false);
            }
            grid_sanctions.DataSource = dtaddsanctions;
            grid_sanctions.DataBind();
        }
    }

    protected void RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable sourceData = (DataTable)ViewState["dtdraft"];
        sourceData.Rows[e.RowIndex].Delete();
        susdate.DataSource = sourceData;
        susdate.DataBind();
    }

    protected void uploadirfiles(object sender, EventArgs e)
    {
        if (fuincidentreport.HasFile)
        {
            string filename = Path.GetFileNameWithoutExtension(fuincidentreport.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuincidentreport.PostedFile.FileName);
            string contenttype = fuincidentreport.PostedFile.ContentType;

            string nameofiles = hdn_empid.Value;
            DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/incidentreportfiles/") + nameofiles);
            DataTable dtinsertirfile = dbhelper.getdata("insert into irfiles (empid, userid, extension, status, contenttype, filename, said, action, date, canceldate) values ('" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + sanctionid.Value + "','Active','" + DateTime.Now.ToString() + "',Null) select scope_identity() idd");
            dbhelper.getdata("update sanctionentry set status = 'Verified', stat = '1' where id =" + sanctionid.Value + "");

            DataTable dtselect = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + hdn_empid.Value + " and action = 'Active' order by id desc");
            fuincidentreport.SaveAs(Server.MapPath("~/files/incidentreportfiles/") + nameofiles + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + fileExtension);

            grid_sanctions.DataSource = dtselect;
            grid_sanctions.DataBind();

            modal_SancAttachment.Style.Add("display", "none");
        }
    }
    protected void incidentreports(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_sanctions.Rows[row.RowIndex].Cells[2].FindControl("lbirlistfiles");
            trn_det_id.Value = lb.CommandName;
            DataTable dt = dbhelper.getdata("select * from irfiles where said = '" + trn_det_id.Value + "' and empid = '" + hdn_empid.Value + "' and [action] = 'Active'");
            incidentfiles.DataSource = dt;
            incidentfiles.DataBind();
            irsanctionlist.Style.Add("display", "block");
        }
    }
    protected void downlaodirfilesbypiece(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string folderdir = hdn_empid.Value;
            LinkButton lnkbtn = (LinkButton)incidentfiles.Rows[row.RowIndex].Cells[1].FindControl("lnk_downloadirpiece");
            DataTable dt = dbhelper.getdata("select * from irfiles where id = " + lnkbtn.CommandName + "");
            string input = Server.MapPath("~/files/incidentreportfiles/" + folderdir + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
    protected void downloadirfiles(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton laodirfile = (LinkButton)grid_sanctions.Rows[row.RowIndex].Cells[6].FindControl("lbdonwloadir");
            DataTable dt = dbhelper.getdata("select top 1 * from irfiles where said = " + laodirfile.CommandName + " order by id desc");

            string fileout = Server.MapPath("~/files/incidentreportfiles/" + Request.QueryString["app_id"].ToString() + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileout));
            Response.WriteFile(fileout);
            Response.Flush();
            Response.End();
        }
    }
    protected void deletesanctions(object sender, EventArgs e)
    {
        DataTable dtdeltesanctioins = new DataTable();
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update irfiles set action = 'Cancel', canceldate = '" + DateTime.Now.ToString() + "' where said = '" + sanctionid.Value + "'");
            dbhelper.getdata("update sanctionentry set action = 'Cancel', cancelledate = '" + DateTime.Now.ToString() + "' where id=" + row.Cells[0].Text + "");
            dbhelper.getdata("update suspension set action = 'Cancel', canceldate = '" + DateTime.Now.ToShortDateString() + "' where id = " + row.Cells[0].Text + "");
            dtdeltesanctioins = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + hdn_empid.Value + " and action = 'Active' order by id desc");
        }
        grid_sanctions.DataSource = dtdeltesanctioins;
        grid_sanctions.DataBind();
    }

    protected void ddlvalue_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsanctioncodes.SelectedValue == "5") { btnsusdate.Enabled = true; pnl_SacnOption.Visible = true; }
        else if (ddlsanctioncodes.SelectedValue == "6") { btnsusdate.Enabled = true; pnl_SacnOption.Visible = true; }
        else if (ddlsanctioncodes.SelectedValue == "7") { btnsusdate.Enabled = true; pnl_SacnOption.Visible = true; }
        else if (ddlsanctioncodes.SelectedValue == "8") { btnsusdate.Enabled = true; pnl_SacnOption.Visible = true; }
        else if (ddlsanctioncodes.SelectedValue == "9") { btnsusdate.Enabled = true; pnl_SacnOption .Visible = true; }
        else if (ddlsanctioncodes.SelectedValue == "10") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "1") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "2") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "3") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "4") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "0") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
    }

    protected void irfilesupload(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            sanctionid.Value = row.Cells[0].Text;
        }

        modal_SancAttachment.Style.Add("display", "block");
    }

    protected void close_sacnAttachment(object sender, EventArgs e)
    {
        modal_SancAttachment.Style.Add("display", "none");
    }

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
 
    protected void addsusdate(object sender, EventArgs e)
    {
        if (txtsuspend.Text.Length > 0)
        {
            lblsusdate.Text = string.Empty;
            DataTable dtsunctionsetup = dbhelper.getdata("select * from sanctioncodes where id =" + ddlsanctioncodes.SelectedValue + "");
            DataRow dtrdr;
            DataTable dtdr = ViewState["dtdraft"] as DataTable;
            dtrdr = dtdr.NewRow();
            dtrdr["suspendate"] = txtsuspend.Text;
            dtdr.Rows.Add(dtrdr);
            ViewState["dtdraft"] = dtdr;
            susdate.DataSource = dtdr;
            susdate.DataBind();

            if (dtdr.Rows.Count >= decimal.Parse(dtsunctionsetup.Rows[0]["suspensiondays"].ToString()))
            {
                btnsusdate.Enabled = false;
                overlay.Visible = false;
                invetPanel.Visible = false;
                suspendpanel.Visible = false;
            }
            else
                btnsusdate.Visible = true;
        }
        else
            lblsusdate.Text = "*";  
    }
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

    protected void manual_change_status(object sender, EventArgs e)
    {
        /**BTK
         * 111419
         * -Add successfull notification
         * **/
        string filename = "0";
        string ret = "0";
        string fileExtension = "0";
        string contenttype = "0";
        string fileconcat = "";
        string err = "";

        DataTable dt = dbhelper.getdata("select (select [status] from mempstatus_setup where id = emp_status)emp_status from MEmployee where Id = " + Request.QueryString["app_id"].ToString() + "");

        if (ddl_status_emp.SelectedValue == "0" || txt_effective_date.Text.Length == 0 || txt_notes_status.Text.Length == 0)
            err = "Invalid";
        bool ferr = err.Contains("Invalid");
        if (!ferr)
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                filename = Server.MapPath("~/files/peremp/" + Request.QueryString["app_id"].ToString() + "/Employee_status");
                using (SqlCommand cmd = new SqlCommand("process_mempstatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["app_id"].ToString();
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                    cmd.Parameters.Add("@statusid", SqlDbType.Int).Value = ddl_status_emp.SelectedValue;
                    cmd.Parameters.Add("@effectivedate", SqlDbType.VarChar, 5000).Value = txt_effective_date.Text;
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar, 5000).Value = txt_notes_status.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                }
                if (file_to_be_attached.HasFile)
                {
                    HttpFileCollection uploadedFiles = Request.Files;
                    for (int i = 0; i < uploadedFiles.Count; i++)
                    {

                        HttpPostedFile userPostedFile = uploadedFiles[i];
                        string[] fileName0 = userPostedFile.FileName.Split('.');
                        string fileName1 = Path.GetExtension(userPostedFile.FileName);
                        contenttype = userPostedFile.ContentType.ToString();
                        if (userPostedFile.ContentLength > 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("process_memployeestatusfile", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@empstatid", SqlDbType.Int).Value = ret;
                                cmd.Parameters.Add("@filetype", SqlDbType.VarChar, 5000).Value = contenttype;
                                cmd.Parameters.Add("@fileextension", SqlDbType.VarChar, 5000).Value = fileName1;
                                cmd.Parameters.Add("@filename", SqlDbType.VarChar, 5000).Value = fileName0[0];
                                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                string statusfile_ret = cmd.Parameters["@out"].Value.ToString();
                                con.Close();
                                if (!Directory.Exists(filename))
                                    Directory.CreateDirectory(filename);
                                userPostedFile.SaveAs(filename + "\\" + fileName0[0].Replace(" ", "") + "_" + statusfile_ret + fileName1.ToString());
                            }
                        }
                    }
                }

                /**BTK 111419**/
                disp();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Successfully Save');", true);
                modal_status.Style.Add("display", "none");
                /**END**/

            }
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "empstat";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dt.Rows[0]["emp_status"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = ddl_status_emp.SelectedItem.ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Change Status";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = hdn_empid.Value;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        else
            lbl_err.Text = "All asterisk(*) are required!";
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
                    DataTable dtprep = dbhelper.getdata("insert into Preparatory(EmpId, UserId, LicenseNum, ValidUntil, Location, Cost, DurationFrom, DurationTo, Provider, About, Resources, Sysdate)values('" + hdn_empid.Value + "'," + Session["user_id"].ToString() + ",'" + txt_license.Text.Replace("'", "").ToString() + "','" + txt_validity.Text + "','" + txt_location.Text + "','" + txt_cost.Text + "','" + txt_durationfrom.Text + "','" + txt_durationto.Text + "','" + txt_provider.Text + "','" + txt_about.Text + "','" + txt_resources.Text + "',GETDATE()) select scope_identity() saidid");
                    DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/preparatoryfiles/") + nameofiles);
                    DataTable dtinsertirfile = dbhelper.getdata("insert into irfiles (empid, userid, extension, status, contenttype, filename, said, action, date) values ('" + hdn_empid.Value + "','" + Session["user_id"].ToString() + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + dtprep.Rows[0]["saidid"].ToString() + "','Active','" + DateTime.Now.ToString() + "') select scope_identity() idd");
                    fupreparatory.SaveAs(Server.MapPath("~/files/preparatoryfiles/") + nameofiles + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + fileExtension);
                }
                else
                {
                    DataTable dtprep = dbhelper.getdata("insert into Preparatory(EmpId, UserId, LicenseNum, ValidUntil, Location, Cost, DurationFrom, DurationTo, Provider, About, Resources, Sysdate)values('" + hdn_empid.Value + "'," + Session["user_id"].ToString() + ",'" + txt_license.Text.Replace("'", "").ToString() + "','" + txt_validity.Text + "','" + txt_location.Text + "','" + txt_cost.Text + "','" + txt_durationfrom.Text + "','" + txt_durationto.Text + "','" + txt_provider.Text + "','" + txt_about.Text + "','" + txt_resources.Text + "',GETDATE())");
                }
            }
        }
        disp();
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
            string folderdir = hdn_empid.Value;
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
        disp();
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
    protected void ddl_section_SelectedIndexChanged(object sender, EventArgs e)
    {


        if (ddl_department.Text == "add")
        {
            ddl_department.SelectedValue = "";
        }
        else
        {
            TextBox2.Text = ddl_department.Text;
            string query = "select a.Department,b.sectionid,b.seccode+' ('+ b.sec_desc + ')' as seccode " +
                         "from Mdepartment a " +
                         "left join Msection b on a.id=b.dept_id " +
                         "where a.id='" + TextBox2.Text + "' order by a.id ";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);

            ddl_section.Items.Clear();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                ddl_section.Items.Insert(0, new ListItem(dt.Rows[i]["seccode"].ToString(), dt.Rows[i]["sectionid"].ToString()));

            }
        }
    }
    protected void backlag()
    {
        Response.Redirect("addemployee?user_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + Request.QueryString["app_id"].ToString() + "&tp=ed");
    }
    protected void massupload(object sender, EventArgs e)
    {
        Response.Redirect("upload?");
    }
    protected void adminsideleave(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addLEAVEadminside?emp_id=" + Request.QueryString["user_id"].ToString() + "&app_id=" + "" + Request.QueryString["user_id"].ToString() + "" + "&tp=ed");
    }
}