using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_rest_day_list : System.Web.UI.Page
{
   // public static string user_id,query;
    protected void Page_Load(object sender, EventArgs e)
    {
       // user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
       string query = "select * from TRestdaylogs where EmployeeId='" + Session["emp_id"].ToString() + "' order by Id desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void addOT(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addRestday");
    }

    protected void cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           string query = "update TRestdaylogs set status='" + "Cancel-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + " ";
            dbhelper.getdata(query);
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_Restday'", true);
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            string[] stat = e.Row.Cells[5].Text.Split('-');
            e.Row.Cells[5].Text = stat[0];
            if (e.Row.Cells[5].Text == "Approved")
                lnkcan.Visible = false;
        }
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}