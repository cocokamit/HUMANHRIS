using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_undertimeList : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
        string query = "select *,case when dtunder IS null then LEFT(CONVERT(varchar,date_filed,101),10)+' '+[time] else dtunder  end  timeout from Tundertime where emp_id='" + Session["emp_id"].ToString() + "' and status not like '%deleted%' order by Id desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    protected void change(object sender, EventArgs e)
    {
        string query = "select *,case when dtunder IS null then LEFT(CONVERT(varchar,date_filed,101),10)+' '+[time] else dtunder  end  timeout from Tundertime where emp_id='" + Session["emp_id"].ToString() + "' and class='" + rbl_class.Text + "' and status not like '%deleted%' order by Id desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    protected void addUnder(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_addUndertime");
    }

    protected void cancel(object sender, EventArgs e)
    {
        if (checkerRe())
        {
            string query = "update Tundertime set status='deleted',reason='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id.Value + " ";
            query += "update toffset set status='" + "deleted-" + Session["user_id"] + "' where CONVERT(date,appliedfrom)='" + hdn_datefiled.Value + "' and empid=" + Session["emp_id"].ToString() + "";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='KIOSK_undertime'", true);
        }
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
              LinkButton lnk = (LinkButton)row.Cells[5].FindControl("lnk_can");
              hdn_datefiled.Value=row.Cells[1].Text;
              DataTable chk = dbhelper.getdata("select * from TDTR where CONVERT(date,dateend)>'" + row.Cells[1].Text+ "' and status is null");
              if (chk.Rows.Count == 0)
              {
                  id.Value = row.Cells[0].Text;
                  Div1.Visible = true;
                  Div2.Visible = true;
              }
              else
                  Response.Write("<script>alert('Transaction Denied!')</script>");
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_undertime", false);
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
    protected bool checkerRe()
    {
        bool oi = true;

        if (txt_reason.Text == "")
        {
            oi = false;
            lbl_re.Text = "*";
        }
        else
            lbl_re.Text = "";

        return oi;

    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}