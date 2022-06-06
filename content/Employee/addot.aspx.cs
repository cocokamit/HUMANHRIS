using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_addot : System.Web.UI.Page
{
    public static string user_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        disp();
    }

    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select * from PreOTRequest where EmpId = '" + Session["emp_id"] + "' and Action is NULL order by Sysdate desc");
        grid_view.DataSource = dt;
        grid_view.DataBind();
        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            string[] stat = e.Row.Cells[5].Text.Split('-');
            if (stat[0] == "Approved")
                lnkcan.Visible = false;
        }
    }

    protected void cancelApplication(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update PreOTRequest set Action = 'Cancel' where Id = '" + row.Cells[0].Text + "'");
            Response.Redirect("KOISK_addOT");
        }
    }

    protected void addpreot(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_preOT");
    }
}