using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_manual_login_list : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {
        string query = "select *,CONVERT(datetime,a.time_out) timeoutf,CONVERT(datetime,a.time_in) timeinf,case when time_in1!='0' then LEFT(convert(varchar,CONVERT(datetime,time_in1),101),10) else '0' end timein1f,case when time_out1!='0' then LEFT(convert(varchar,CONVERT(datetime,time_out1),101),10) else '0' end timeout1f from Tmanuallogline a left join time_adjustment b on a.reason=b.id where a.EmployeeId='" + Session["emp_id"].ToString() + "'  and status not like '%deleted%' order by a.Id desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    protected void addManual(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addMANUAL");
    }
    protected void cancel(object sender, EventArgs e)
    {
       string query = "update Tmanuallogline set status='" + "delete-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id.Value + " ";
       dbhelper.getdata(query);

       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='KOISK_MANUAL'", true);
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_MANUAL", false);
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            string[] stat = e.Row.Cells[10].Text.Split('-');
            if (stat[0] == "Approved")
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