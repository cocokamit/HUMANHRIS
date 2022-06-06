using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

public partial class content_hr_NotificationList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            load();
        }
    }
    protected void load()
    {
        DataTable dt = dbhelper.getdata("select id, url, left(convert(varchar,date,101),10)[date],[subject], content from sys_notification where [status] = '0'");
        gridNotificationlist.DataSource = dt;
        gridNotificationlist.DataBind();
        if (dt.Rows.Count == 0)
        {
            btnmark.Visible = false;
            btnsee.Visible = true;
        }
    }
    protected void markallasread(object sender, EventArgs e)
    {
        dbhelper.getdata("update sys_notification set [status] = '1' where [type] = '0'");
        btnsee.Visible = true;
        btnmark.Visible = false;
        gridNotificationlist.Visible = false;
    }
    protected void seelatestnoti(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select MONTH(GETDATE())MonthNow, * from sys_notification where MONTH(date)=MONTH(GETDATE())");
        gridcurrent.DataSource = dt;
        gridcurrent.DataBind();
        gridNotificationlist.Visible = false;
        btnmark.Visible = false;
    }
    protected void notificationcontent(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)gridNotificationlist.Rows[row.RowIndex].Cells[4].FindControl("lnkbtncontent");
            trn_det_id.Value = lb.CommandName;
            DataTable dt = dbhelper.getdata("select id, url, left(convert(varchar,date,101),10)[date],[subject], content from sys_notification where [status] = '0' and id = '" + trn_det_id.Value + "'");
            dbhelper.getdata("update sys_notification set [status] = '1' where id = '" + trn_det_id.Value + "'");
            //Response.Redirect("" + dt.Rows[0]["url"] + "&tp=ed", false);
        }
    }
    protected void hapitda(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)gridcurrent.Rows[row.RowIndex].Cells[4].FindControl("lnkbtncontent");
            trn_det_id.Value = lb.CommandName;
            DataTable dt = dbhelper.getdata("select id, url, left(convert(varchar,date,101),10)[date],[subject], content from sys_notification where id = '" + trn_det_id.Value + "'");
            //dbhelper.getdata("update sys_notification set [status] = '1' where id = '" + trn_det_id.Value + "'");
            //Response.Redirect("" + dt.Rows[0]["url"] + "&tp=ed", false);
        }
    }
}