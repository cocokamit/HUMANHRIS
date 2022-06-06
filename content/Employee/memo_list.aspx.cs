using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_Employee_memo_list : System.Web.UI.Page
{
    public string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        load();
        if (!IsPostBack)
            getdata();
    }

    protected void load()
    {
        key.Value = Session["user_id"].ToString();
        panel_read.Visible = Request.QueryString["not"] == null ? false : true;
    }

    protected void gv_bound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("Label3");
            if (e.Row.Cells[1].Text == "1")
                lbl.Text = "<i class='fa fa-circle-o'></i>";

        }
    }

    protected void getdata()
    {
        if (Request.QueryString["not"] == null)
        {
        query = "select b.status rd, a.id,a.date_input,a.memo_from,a.memo_subject,a.memo_date,a.memo_description,a.notes, " +
                "b.recipient " +
                "from Tmemo a " +
                "left join TmemoLine b on a.id=b.memo_id " +
                "where b.recipient = '" + Session["emp_id"].ToString() + "' order by a.date_input desc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        //alert.Visible = dt.Rows.Count == 0 ? true : false;

      

        }
        else
            view_memo(Request.QueryString["not"].ToString());

        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            view_memo(row.Cells[0].Text);
            getdata();
        }
    }

    protected void view_memo(string id)
    {
        query = "select b.id rd, a.id,a.date_input,a.memo_from,a.memo_subject,a.memo_date,a.memo_description,a.notes, " +
        "c.lastname+', '+c.firstname+' '+c.middlename as fullname " +
        "from Tmemo a " +
        "left join TmemoLine b on a.id=b.memo_id " +
        "left join Memployee c on b.recipient=c.id " +
        "where a.id='" + id + "'";
        DataTable dt = dbhelper.getdata(query);

        l_subject.Text = dt.Rows[0]["memo_subject"].ToString();
        l_date.Text = dt.Rows[0]["memo_date"].ToString();
        l_from.Text = dt.Rows[0]["memo_from"].ToString();
        l_to.Text = dt.Rows[0]["fullname"].ToString();
        viewer.InnerHtml = HttpUtility.HtmlDecode(dt.Rows[0]["memo_description"].ToString());


        string qq = "select id,filename,CONVERT(varchar, memo_id)+ '\\' +filename as fck from memo_attachments where status is null and memo_id=" + id;
        DataSet ds = new DataSet();
        ds = bol.display(qq);
        grid_img.DataSource = ds;
        grid_img.DataBind();

        if (grid_img.Rows.Count == 0)
            lbl_attach.Text = "";
        else
            lbl_attach.Text = "Attachment Files";


        panel_display.Visible = false;
        panel_read.Visible = true;
        dbhelper.getdata("update TmemoLine set status=1 where recipient = " + Session["emp_id"].ToString() + " and memo_id=" + dt.Rows[0]["id"].ToString());
    }

    protected void downloadreqfiles(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            DataTable dt = dbhelper.getdata("select id,filename,type,CONVERT(varchar, memo_id)+ '\\' +filename as fck from memo_attachments where status is null and id=" + row.Cells[0].Text + " ");
            string input = Server.MapPath("~/files/memo/" + dt.Rows[0]["fck"].ToString() + "");

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["type"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }


    protected void click_back(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_memo?user_id=" + Request.QueryString["user_id"]);
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_memo?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}