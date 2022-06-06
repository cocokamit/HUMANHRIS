using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_hr_appraisallist : System.Web.UI.Page
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
        string query = "";

        DataTable dt;

        
        query = "select * from MPosition order by id desc";
        dt = dbhelper.getdata(query);
        ddl_position.Items.Clear();
        ddl_position.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_position.Items.Add(new ListItem(dr["position"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPayrollType order by id desc";
        dt = dbhelper.getdata(query);
        ddl_payrolltype.Items.Clear();
        ddl_payrolltype.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payrolltype.Items.Add(new ListItem(dr["payrolltype"].ToString(), dr["id"].ToString()));
        }
        query = "select * from mempstatus_setup where  action='Default' or action is null";
        dt = dbhelper.getdata(query);
        ddl_status.Items.Clear();
        foreach (DataRow drr in dt.Rows)
        {
            ddl_status.Items.Add(new ListItem(drr["status"].ToString(), drr["id"].ToString()));
        }
       

    }
    protected void disp()
    {
        string query = "select a.empid, a.id, a.purposeid,b.lastname+', '+b.firstname+' '+b.middlename as fullname,b.idnumber,c.department,d.position,LEFT(CONVERT(varchar,a.date,101),10)date,a.recommendid,a.totalratings,(select name from nobel_user where ID=a.userid)appraiser,e.[desc] recommend,f.descc purpose,case when a.action is null then 'For Evalaution' else a.action end status  from app_form_trn a " +
                           "left join memployee b on a.empid=b.id " +
                           "left join mdepartment c on b.departmentid=c.id " +
                           "left join mposition d on b.positionid=d.id " +
                           "left join app_setup_reccomendation e on a.recommendid=e.id " +
                           "left join app_setup_purposeofappraisal f on a.purposeid=f.id " +
                           "where a.purposeid>0 and a.action is null  order by a.action ";
        grid_det.DataSource = dbhelper.getdata(query);
        grid_det.DataBind();
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
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("applist");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void process(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string purposeid=row.Cells[1].Text;
            hdn_purposeid.Value = purposeid;
            hnd_fromid.Value = row.Cells[0].Text;
            hnd_empid.Value = row.Cells[2].Text;
            string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile, *,case when (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) is null then a.emp_status  else (select top 1 statusid from memployeestatus where empid=a.id order by empstatid desc) end   empstatus,left(convert(varchar,a.sbudate,101),10)sbudate1,left(convert(varchar,a.DateHired,101),10)DateHired1,a.allowsc,left(convert(varchar,a.health_card,101),10)health_card1 from memployee a where a.id=" + row.Cells[2].Text + "";
            DataTable dt = dbhelper.getdata(query);
            ddl_status.SelectedValue = dt.Rows[0]["empstatus"].ToString();
            hnd_prev_stat.Value = dt.Rows[0]["empstatus"].ToString();
            ddl_position.SelectedValue = dt.Rows[0]["PositionId"].ToString();
            hdn_prev_position.Value = dt.Rows[0]["PositionId"].ToString();
            txt_fnod.SelectedValue = dt.Rows[0]["FixNumberOfDays"].ToString().Replace(".00000","");
            ddl_payrolltype.SelectedValue = dt.Rows[0]["PayrollTypeId"].ToString();
            txt_fnoh.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["FixNumberOfHours"].ToString()));
            txt_mr.Text = string.Format("{0:n2}", ddl_payrolltype.SelectedValue == "2" ? 0 : decimal.Parse(dt.Rows[0]["MonthlyRate"].ToString()));
            txt_pr.Text = string.Format("{0:n2}", ddl_payrolltype.SelectedValue == "2" ? 0 : decimal.Parse(dt.Rows[0]["PayrollRate"].ToString()));
            txt_dr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["DailyRate"].ToString()));
            txt_hr.Text = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["HourlyRate"].ToString()));


            hdn_fnod.Value = dt.Rows[0]["FixNumberOfDays"].ToString().Replace(".00000", "");
            hdn_payrolltype.Value = dt.Rows[0]["PayrollTypeId"].ToString();
            hdn_fnoh.Value = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["FixNumberOfHours"].ToString()));
            hdn_mr.Value = string.Format("{0:n2}", ddl_payrolltype.SelectedValue == "2" ? 0 : decimal.Parse(dt.Rows[0]["MonthlyRate"].ToString()));
            hdn_pr.Value = string.Format("{0:n2}", ddl_payrolltype.SelectedValue == "2" ? 0 : decimal.Parse(dt.Rows[0]["PayrollRate"].ToString()));
            hdn_dr.Value = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["DailyRate"].ToString()));
            hdn_hr.Value = string.Format("{0:n2}", decimal.Parse(dt.Rows[0]["HourlyRate"].ToString()));
            string recomendation="";
            if (row.Cells[6].Text.Contains("Promotion"))
            {
                recomendation += " Promotion";
                div_promotion.Attributes.Add("class", "enabled");
            }
            else
                div_promotion.Attributes.Add("class", "disabled");
            if (row.Cells[6].Text.Contains("Regularization") || row.Cells[6].Text.Contains("End of Contract"))
            {
                if (row.Cells[6].Text.Contains("Regularization"))
                    recomendation += " Regularization";
                if (row.Cells[6].Text.Contains("End of Contract"))
                    recomendation += " End of Contract";
                div_regularization.Attributes.Add("class", "enabled");
            }
            else
                div_regularization.Attributes.Add("class", "disabled");

           

            if (row.Cells[6].Text.Contains("Salary Increase"))
            {
                recomendation += " Salary Increase";
                pay_det.Attributes.Add("class", "enabled");
            }
            else
                pay_det.Attributes.Add("class", "disabled");


            hdn_recomendation.Value = recomendation;
            ppop(true);
            panelPopUpPanel.Style.Add("width", "850px");
            panelPopUpPanel.Style.Add("left", "450px");
        }
    }
    protected void savetrn(object sender, EventArgs e)
    {
        string filename = "";
        if (txt_effective_date.Text.Length > 0)
        {
            if (FileUpload2.HasFile)
            {
                DataTable dt = dbhelper.getdata("insert into app_trn (userid,lastupdateuserid,date,lastupdatedate,formid,empid) " +
                "values " +
                "(" + Session["user_id"] + "," + Session["user_id"] + ",getdate(),getdate()," + hnd_fromid.Value + "," + hnd_empid.Value + ") select scope_identity() id");

                string filepath = Server.MapPath("files/Appraisal/");
                DirectoryInfo di = Directory.CreateDirectory(filepath);
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];
                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userPostedFile.FileName);
                        string fileExtension = Path.GetExtension(userPostedFile.FileName);
                        string contenttype = userPostedFile.ContentType;


                        string query = "insert into app_trn_file(app_trn_id,location,filename,contenttype,extenssion) " +
                            "values (" + dt.Rows[0]["id"].ToString() + ", 'files/Appraisal/','" + fileName.Replace(fileExtension, "") + "','" + contenttype + "','" + fileExtension + "') select scope_identity() id";
                        DataTable dt1 = dbhelper.getdata(query);
                        filename = filepath + fileName.Replace(" ", "") + "_" + dt1.Rows[0]["id"].ToString() + fileExtension;
                        userPostedFile.SaveAs(filename);
                    }
                }

                if (hdn_recomendation.Value.Contains("Promotion"))
                {
                    DataTable dtchkifexistpromotion = dbhelper.getdata("select * from app_trn_promotion a left join app_trn b on a.app_trn_id=b.id where b.empid=" + hnd_empid.Value + "");
                    if (dtchkifexistpromotion.Rows.Count==0)
                        dbhelper.getdata("insert into app_trn_promotion (app_trn_id,positionid,effective_date) values (" + dt.Rows[0]["id"].ToString() + "," + hdn_prev_position.Value + ",'01/01/1900')");



                    dbhelper.getdata("insert into app_trn_promotion (app_trn_id,positionid,effective_date) values (" + dt.Rows[0]["id"].ToString() + "," + ddl_position.SelectedValue + ",'" + txt_effective_date.Text + "')");
                    dbhelper.getdata("update memployee set PositionId=" + ddl_position.SelectedValue + " where id=" + hnd_empid.Value + "   ");
                    if (ddl_position.SelectedItem.Text.Contains("Manager"))
                        dbhelper.getdata("update memployee set DivisionId=1 where id=" + hnd_empid.Value + "   ");
                    else if (ddl_position.SelectedItem.Text.Contains("Supervisor"))
                        dbhelper.getdata("update memployee set DivisionId=2 where id=" + hnd_empid.Value + "   ");
                    else
                        dbhelper.getdata("update memployee set DivisionId=3 where id=" + hnd_empid.Value + "   ");
                }
                if (hdn_recomendation.Value.Contains("Salary Increase"))
                {
                    decimal meal = txt_meal_allow.Text.Length == 0 ? 0 : decimal.Parse(txt_meal_allow.Text);
                    decimal nti = txt_nti.Text.Length == 0 ? 0 : decimal.Parse(txt_nti.Text);

                    DataTable dtchkifexist = dbhelper.getdata("select * from app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id where b.empid=" + hnd_empid.Value + "");

                    if (dtchkifexist.Rows.Count == 0)
                    {
                        dbhelper.getdata("insert into app_trn_salaryinc (app_trn_id,paytypeid,fnod,fnoh,mr,pr,dr,hr,meallallowance,nti,effective_date) values " +
                        "(" + dt.Rows[0]["id"].ToString() + "," + hdn_payrolltype.Value + "," + hdn_fnod.Value.Replace(",", "") + ",'" + hdn_fnoh.Value.Replace(",", "") + "','" + hdn_mr.Value.Replace(",", "") + "','" + hdn_pr.Value.Replace(",", "") + "','" + hdn_dr.Value.Replace(",", "") + "','" + hdn_hr.Value.Replace(",", "") + "',0,0,'01/01/1900')");
                    }

                    dbhelper.getdata("insert into app_trn_salaryinc (app_trn_id,paytypeid,fnod,fnoh,mr,pr,dr,hr,meallallowance,nti,effective_date) values " +
                   "(" + dt.Rows[0]["id"].ToString() + "," + ddl_payrolltype.SelectedValue + "," + txt_fnod.Text.Replace(",", "") + ",'" + txt_fnoh.Text.Replace(",", "") + "','" + txt_mr.Text.Replace(",", "") + "','" + txt_pr.Text.Replace(",", "") + "','" + txt_dr.Text.Replace(",", "") + "','" + txt_hr.Text.Replace(",", "") + "','" + meal.ToString().Replace(",", "") + "','" + nti.ToString().Replace(",", "") + "','" + txt_effective_date.Text + "')");
                    dbhelper.getdata("update memployee set PayrollTypeId=" + ddl_payrolltype.SelectedValue + ",FixNumberOfDays=" + txt_fnod.Text.Replace(",", "") + ",FixNumberOfHours='" + txt_fnoh.Text.Replace(",", "") + "',MonthlyRate='" + txt_mr.Text.Replace(",", "") + "',PayrollRate='" + txt_pr.Text.Replace(",", "") + "',DailyRate='" + txt_dr.Text.Replace(",", "") + "',absentdailyrate='" + txt_dr.Text.Replace(",", "") + "',HourlyRate='" + txt_hr.Text.Replace(",", "") + "' where id=" + hnd_empid.Value + "   ");

                }

                if (hdn_recomendation.Value.Contains("Regularization") || hdn_recomendation.Value.Contains("End of Contract"))
                {
                    DataTable dtchkifexistregular = dbhelper.getdata("select * from app_trn_regularization a left join app_trn b on a.app_trn_id=b.id where b.empid=" + hnd_empid.Value + "");
                    if (dtchkifexistregular.Rows.Count == 0)
                        dbhelper.getdata("insert into app_trn_regularization (app_trn_id,statusid,effective_date) values (" + dt.Rows[0]["id"].ToString() + "," + hnd_prev_stat.Value + ",'01/01/1900')");


                    dbhelper.getdata("insert into app_trn_regularization (app_trn_id,statusid,effective_date) values (" + dt.Rows[0]["id"].ToString() + "," + ddl_status.SelectedValue + ",'"+txt_effective_date.Text+"')");
                    dbhelper.getdata("update memployee set emp_status=" + ddl_status.SelectedValue + " where id=" + hnd_empid.Value + "   ");
                    if (ddl_status.SelectedItem.Text.Contains("End of Contract"))
                        dbhelper.getdata("update memployee set PayrollGroupId='4' where id=" + hnd_empid.Value + " ");
                    
                }
                dbhelper.getdata("update app_form_trn set action='Approved' where id=" + hnd_fromid.Value + " ");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='applist'", true);
            }
            else
                Response.Write("<script>alert('Invalid File!')</script>");
        }
        else
            Response.Write("<script>alert('Effective Date must be supplied!')</script>");

    }
    protected bool errchk()
    {
        //bool isNumeric = true;

        bool err = true;
        if (ddl_payrolltype.SelectedValue == "0")
        {
            Response.Write("<script>alert(Invalid Input Payroll Type!)</script>");
            err = false;
        }
        else if (txt_fnod.SelectedValue.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Fix No of Days!)</script>");
            err = false;
        }
        else if (txt_fnoh.Text.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Fix No of Hours!)</script>");
            err = false;
        }
        else if (txt_mr.Text.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Monthly Rate!)</script>");
            err = false;
        }
        else if (txt_pr.Text.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Payroll Rate!)</script>");
            err = false;
        }
        else if (txt_dr.Text.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Daily Rate!)</script>");
            err = false;
        }
        else if (txt_hr.Text.Length == 0)
        {
            Response.Write("<script>alert(Invalid Input Hourly Rate!)</script>");
            err = false;
        }
  



       

        return err;
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update app_form_trn set action='DisApproved' where id=" + row.Cells[0].Text + " ");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='applist'", true);
            }
            else
            { }
        }
    }
    protected void robound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkdel = (LinkButton)e.Row.FindControl("LinkButton1");
            LinkButton lnkapp = (LinkButton)e.Row.FindControl("lnk_download");

            if (e.Row.Cells[8].Text == "Approved" || e.Row.Cells[8].Text == "DisApproved")
            {
                lnkdel.Visible = false;
                lnkapp.Visible = false;
            }

        }
    }
    protected void print(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           LinkButton lnk_print=(LinkButton)row.Cells[9].FindControl("lnk_print");
           //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?prntapp?key=" + function.Encrypt(lnk_print.CommandName, true) + "'", true);
           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='prntapp?key=" + function.Encrypt(lnk_print.CommandName, true) + "'", true);
        }
    }

}