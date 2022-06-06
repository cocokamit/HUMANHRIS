using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_dtrrangesetup : System.Web.UI.Page
{
    void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (Session["role"].ToString() != "Admin")
            this.MasterPageFile = "~/content/site.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            disp();
    }

    protected void disp()
    {
        string query = "select id ,left(convert(varchar,dfrom,101),10)+'-'+left(convert(varchar,dtoo,101),10) dtrrange from payroll_range where action is null order by dfrom desc";
        DataTable dt = dbhelper.getdata(query);
        griddtrlist.DataSource = dt;
        griddtrlist.DataBind();
        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }

    protected void newdtrlogs(object sender, EventArgs e)
    {
        ppop(true);
    }

    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("dtrrange");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void saverange(object sender, EventArgs e)
    {
        try
        {
            DataTable dtchk = dbhelper.getdata("select * from payroll_range where ( '" + txt_f.Text + "' BETWEEN  convert(date,dfrom) AND convert(date,dtoo)  or  '" + txt_t.Text + "' BETWEEN  convert(date,dfrom) AND convert(date,dtoo) ) and action is null");
            if (dtchk.Rows.Count == 0 && Convert.ToDateTime(txt_t.Text) >= Convert.ToDateTime(txt_f.Text))
            {
                string query = "insert into payroll_range (date,userid,lastupdateuserid,lastupdatedate,dfrom,dtoo) values (getdate()," + Session["user_id"] + "," + Session["user_id"] + ",getdate(),'" + txt_f.Text + "','" + txt_t.Text + "')";
                dbhelper.getdata(query);
                l_msg.Text = "Successfully Saved.";
            }
            else
                l_msg.Text = "Invalid Date / Existing Range.";

            txt_t.Text = "";
            txt_f.Text = "";
        }
        catch
        {
 
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        l_msg.Text = "";
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                LinkButton lnkcan = (LinkButton)row.FindControl("lnkcan");
                dbhelper.getdata("update payroll_range set action='Cancel',lastupdateuserid=" + Session["user_id"] + ",lastupdatedate=getdate() where Id=" + lnkcan.CommandName + " ");
                //dbhelper.getdata("delete from tdtrperpayrolperline where dtrperpayrol_id=" + lnkcan.CommandName + " ");
                Response.Redirect("dtrrange", false);
            }
            else
            {
            }
        }
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnkcan");// griddtrlist.FindControl("lnkcan");
            string[] rrange=e.Row.Cells[1].Text.Split('-');
            DataTable dtt = dbhelper.getdata("select * from tdtr where CONVERT(date,DateStart)=CONVERT(date,'" + rrange[0] + "') and CONVERT(date,DateEnd)=CONVERT(date,'" + rrange[1] + "')  and status is null");
            if (dtt.Rows.Count > 0)
                lnkcan.Visible = false;
            else
                lnkcan.Visible = true;

        }
    }
}