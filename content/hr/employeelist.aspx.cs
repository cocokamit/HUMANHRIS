using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_hrms_hr_emp_list : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
           // hf_id.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            view();
        }
    }

    protected void click_grid(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        dbhelper.getdata("update Sys_viewconfig set value='" + lb.Text + "' where id=1");
        Response.Redirect(Request.RawUrl);
    }

    protected void view()
    {
        DataTable dt = dbhelper.getdata("select value from Sys_viewconfig where id=1");
        hf_view.Value = dt.Rows[0]["value"].ToString();
        if (dt.Rows[0]["value"].ToString() == "List")
        {
            disp();
            div_list.Visible = true;
        }
    }

    protected void lodable()
    {
        DataTable dt = dbhelper.getdata("select * from MPayrollGroup order by id asc");
        ddl_payroll_group.Items.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
        ddl_payroll_group.Items.Add(new ListItem(" ", "0"));
    }

    protected void disp()
    {
        //string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile, a.id, a.IdNumber,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,b.Position from MEmployee a left join MPosition b on a.PositionId=b.Id where PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and (a.IdNumber+''+a.lastname+''+a.firstname+''+ a.middlename+''+a.extensionname) like '%" + txt_search.Text + "%' order by a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname";
        DataTable dt = dbhelper.getdata(adjustdtrformat.allemployee() + " where PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and Fullname like '%" + txt_search.Text + "%' order by Fullname asc  ");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void click_add_employee(object sender, EventArgs e)
    {
        Response.Redirect("addemployee?user_id=" + function.Encrypt(hf_id.Value, true) + "&app_id=0&tp=dd", false);
    }

    protected void click_edit_employee(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("addemployee?user_id=" + function.Encrypt(hf_id.Value, true) + "&app_id=" + lb.CommandName + "&tp=ed", false);
        }
    }

    protected void search(object sender, EventArgs e)
    {
        if (hf_view.Value == "List")
        {
            disp();
        }
    }
    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton LinkButton2 = (LinkButton)grid_view.Rows[row.RowIndex].Cells[5].FindControl("LinkButton2");
            trn_det_id.Value = LinkButton2.CommandName;
            string query = "select id, filename+'_'+convert(varchar,id)+filename2 filename from file_details where empid=" + LinkButton2.CommandName + " and status='Active' ";
            DataTable dt = dbhelper.getdata(query);
            grid_det.DataSource = dt;
            grid_det.DataBind();
            DataTable dtlaststaus=dbhelper.getdata("select top 1 * from memployeestatus  where empid="+LinkButton2.CommandName+" order by empstatid desc");
            if (dtlaststaus.Rows.Count > 0)
            {
                DataTable dtjobpost = dbhelper.getdata(" select id, convert(varchar,id)+'_'+convert(varchar," + dtlaststaus.Rows[0]["statusid"].ToString() + "),[filename]+'.'+fileextension filename from memployeestatusfile  where empstatid=" + dtlaststaus.Rows[0]["empstatid"].ToString() + " ");

                grid_jobpost.DataSource = dtjobpost;
            }
            grid_jobpost.DataBind();

            
        }
        ppop(true);
    }
    protected void download(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            try
            {
                LinkButton lnk_viewreq = (LinkButton)grid_det.Rows[row.RowIndex].Cells[2].FindControl("lnk_download");
                DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
                // string classs = row.Cells[2].Text == "FORMS" ? "forms" : "peremp";
                string input = Server.MapPath("~/" + dt.Rows[0]["location"].ToString() + "/") + dt.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString();

                //Download the Decrypted File.
                Response.Clear();
                Response.ContentType = dt.Rows[0]["contenttype"].ToString();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
                Response.WriteFile(input);
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            { }
        }
    }
    protected void downloadjobpost(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            try
            {
                LinkButton lnk_viewreq = (LinkButton)grid_jobpost.Rows[row.RowIndex].Cells[2].FindControl("lnk_download");
                DataTable dtdetails = dbhelper.getdata("select * from memployeestatusfile where id=" + row.Cells[0].Text + "");
                string input = Server.MapPath("~/files/peremp/") + trn_det_id.Value + "/Employee_status/" + dtdetails.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dtdetails.Rows[0]["id"].ToString() + "." + dtdetails.Rows[0]["fileextension"].ToString();
                //DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
                //string classs = row.Cells[2].Text == "FORMS" ? "forms" : "peremp";
                //string input = Server.MapPath("~/" + dt.Rows[0]["location"].ToString() + "/") + dt.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString();

                //Download the Decrypted File.
                Response.Clear();
                Response.ContentType = dtdetails.Rows[0]["filetype"].ToString();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
                Response.WriteFile(input);
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            { }
        }
    }
    protected void candetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_can = (LinkButton)grid_det.Rows[row.RowIndex].Cells[2].FindControl("lnk_can");
            if (TextBox1.Value == "Yes")
            {
                DataTable dt = dbhelper.getdata("update file_details set status='can' where id=" + lnk_can.CommandName + " ");
                DataTable dt1 = dbhelper.getdata("select id, filename+'_'+convert(varchar,id)+filename2 filename from file_details where empid=" + trn_det_id.Value + " and status='Active' ");
                grid_det.DataSource = dt1;
                grid_det.DataBind();
            }
            else
            { }


        }

    }

    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("MEmployee");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_status = (LinkButton)e.Row.FindControl("LinkButton1");
            if (e.Row.Cells[0].Text == "Incomplete")
            {
                lnk_status.Visible = true;
            }
        }
    }
}