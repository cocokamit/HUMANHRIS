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

public partial class content_hr_memo_list : System.Web.UI.Page
{
    public string id,query,message;
    protected void Page_Load(object sender, EventArgs e)
    {
        load();
        if (!IsPostBack)
            disp();
    }

    protected void load()
    {
        //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        panel_read.Visible = Request.QueryString["r"] == null ? false : true;
    }

    protected void click_add(object sender, EventArgs e)
    {
        Response.Redirect("Memo?key=" +Session["user_id"]);
    }

    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select * from Tmemo order by id desc");
        grid_view.DataSource = dt;
        grid_view.DataBind();
        div_msg.Visible = dt.Rows.Count == 0 ? true : false;
    }

    protected void recipient(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            l_subject.Text = row.Cells[3].Text;
            l_date.Text = row.Cells[2].Text;
            l_from.Text = row.Cells[4].Text;
            viewer.InnerHtml = HttpUtility.HtmlDecode(row.Cells[5].Text);

            id = row.Cells[0].Text;
            query = "select (case when a.status IS NULL then 'unread' else 'read' end) as stat, " +
            "b.Id, " +
            "c.firstname+' '+ c.lastname Fullname " +
            "from TmemoLine a " +
            "left join Tmemo b on a.memo_id=b.id " +
            "left join MEmployee c on a.recipient=c.Id " +
            "where b.Id=" + id + " ";
            string nakadawat=null;
            DataTable dt = dbhelper.getdata(query);
            foreach (DataRow dr in dt.Rows)
            {
                nakadawat += dr["Fullname"] + ", ";
            }

            l_to.Text = nakadawat.Length > 100 ?  nakadawat.Substring(0,100) + "..." : nakadawat.Substring(0,nakadawat.Length-2);

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
            //DataSet ds = new DataSet();
            //ds = bol.display(query);
            //grid_rep.DataSource = ds;
            //grid_rep.DataBind();

            //Div1.Visible = true;
            //Div2.Visible = true;
        }
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
        panel_display.Visible = true;
        panel_read.Visible = false;
    }

    protected void click_filter(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }

    protected void cpop(object sender, EventArgs e)
    {
        Div1.Visible = false;
        Div2.Visible = false;
       
    }
}