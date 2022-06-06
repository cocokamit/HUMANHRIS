using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_MealAllowanceList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            Bind();
    }

    protected void Bind()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        DataTable dt = dbhelper.getdata("select * from TMealAllowanceApplicationLine where EmployeeId='" + Session["emp_id"].ToString() + "' and status not like '%deleted%' order by Id desc");
        alert.Visible = dt.Rows.Count == 0 ? true : false;
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void cancel(object sender, EventArgs e)
    {
        if (checkerCan())
        {
            string query = "update TMealAllowanceApplicationLine set status='" + "deleted-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id.Value + " ";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully!'); window.location='KOISK_MA'", true);
        }
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            string[] stat = e.Row.Cells[5].Text.Split('-');
            if (stat[0] == "Approved")
            {
                lnkcan.Visible = false;
            }
            if (stat[0] == "Cancel")
            {
                lnkcan.Visible = false;
            }
        }
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Div2.Visible = true;
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_MA", false);
    }

    protected bool checkerCan()
    {
        bool oi = true;
        if (txt_reason.Text == "")
        {
            oi = false;
            lbl_reason.Text = "*";
        }
        else
            lbl_reason.Text = "";
        return oi;
    }
}