using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_Employee_downloadform : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select id, filename+'_'+convert(varchar,id)+filename2 filename from file_details where classid=3 and status='Active' ");
        grid_det.DataSource = dt;
        grid_det.DataBind();
    }
    protected void download(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_viewreq = (LinkButton)grid_det.Rows[row.RowIndex].Cells[2].FindControl("lnk_download");
            DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
            string input = Server.MapPath("~/" + dt.Rows[0]["location"].ToString() + "/") + dt.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString();
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
}