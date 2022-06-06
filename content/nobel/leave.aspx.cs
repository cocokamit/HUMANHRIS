using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

public partial class content_nobel_leave : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
  

        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
            bind();
    }

    protected void bind()
    {
        DataTable dt = dbhelper.getdata("select * from Mleave order by action,  leavetype");
        gv_data.DataSource = dt;
        gv_data.DataBind();
        alert.Visible = gv_data.Rows.Count == 0 ? true : false;
    }

    protected void dataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("lb_action");
            lb.CssClass = e.Row.Cells[5].Text == "disable" ? "fa fa-toggle-off" : "fa fa-toggle-on";

            Label l = (Label)e.Row.FindControl("l_status");
            l.Text = e.Row.Cells[5].Text == "disable" ? "disable" : "enable";
            l.CssClass = e.Row.Cells[5].Text == "disable" ? "label label-danger text-capitalize" : "label label-primary text-capitalize";
        }
    }

    protected void Submit(object sender, EventArgs e)
    {
        string condition = hf_action.Value == "0" ? "" : " id!=" + hf_id.Value + " and ";
        DataTable dtcheck = dbhelper.getdata("select * from Mleave where " + condition + " Leave='" + txt_leavedescription.Text.Trim() + "'");
        if (dtcheck.Rows.Count == 0)
        {
            string credit = check_leave.Checked == true ? "yes" : "no";
            if (hf_action.Value == "0")
                dbhelper.getdata("insert into Mleave (LeaveType,Leave,yearlytotal,converttocash) values ('" + txt_leavetype.Text.Trim() + "','" + txt_leavedescription.Text.Trim() + "', " + txt_leavetotal.Text.Trim() + ",'" + credit + "')");
            else
                dbhelper.getdata("update Mleave set LeaveType='" + txt_leavetype.Text.Trim() + "',Leave='" + txt_leavedescription.Text.Trim() + "',yearlytotal='" + txt_leavetotal.Text.Trim() + "',converttocash='" + credit + "' where id='" + hf_id.Value + "' ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='" + Request.RawUrl + "'", true);
        }
        else
            l_type.Text = "already exists";
    }

    protected void click_edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hf_id.Value = row.Cells[0].Text;
            hf_action.Value = "1";

            DataTable dt = dbhelper.getdata("select * from Mleave where id='" + hf_id.Value + "' ");
            txt_leavetype.Text = dt.Rows[0]["LeaveType"].ToString();
            txt_leavedescription.Text = dt.Rows[0]["Leave"].ToString();
            txt_leavetotal.Text = dt.Rows[0]["yearlytotal"].ToString();
            check_leave.Checked = dt.Rows[0]["converttocash"].ToString() == "Yes" ? true : false;

            Modal(true);
        }
    }

    protected void action(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string action = row.Cells[5].Text == "disable" ? "NULL" : "'disable'";
            dbhelper.getdata("update Mleave set action=" + action + " where id='" + row.Cells[0].Text + "'");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='" + Request.RawUrl + "'", true);
        }
    }

    protected void CreateLeave(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        Modal(true);
    }

    protected void CloseModal(object sender, EventArgs e)
    {
        Modal(false);
        txt_leavetype.Text = string.Empty;
        txt_leavedescription.Text = string.Empty;
        txt_leavetotal.Text = string.Empty;
        check_leave.Checked = false;
    }

    protected void Modal(bool x)
    {
        string set = x ? "block" : "none";
        modal.Style.Add("display", set);
        modal_title.InnerHtml = hf_action.Value == "0" ? "New Leave" : "Edit Leave";
    }
}