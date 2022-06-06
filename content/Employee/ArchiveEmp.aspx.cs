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
using System.Web.UI.HtmlControls;

public partial class content_Employee_ArchiveEmp : System.Web.UI.Page
{
    public static int emp_id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        disp();
    }

    protected void disp()
    {
        int empid = Convert.ToInt32(Session["emp_id"].ToString());
        emp_id = empid;
        DataTable dt = dbhelper.getdata("select distinct d.status,d.description category,'('+CONVERT(varchar,(select COUNT(*) from file_details a left join file_viewers b on a.id=b.archiveId left join file_class c on a.classid=c.id where a.status='Active' and c.description=d.description and (b.empid=" + empid + " or b.depid=(Select DepartmentId from MEmployee where id=" + empid + ") or sanaol=1)))+')' counts from file_details a left join file_viewers b on a.id=b.archiveId left join file_class d on a.classid=d.id where a.status='Active' and d.status='Active' and (b.empid=" + empid + " or b.depid=(Select DepartmentId from MEmployee where id=" + empid + ") or sanaol=1) order by d.description asc");
        
        //DataTable dt = dbhelper.getdata("select a.date,a.id,a.location, filename+'_'+convert(varchar,a.id)+filename2 filename,b.empid,c.description category from file_details a left join file_viewers b on a.id=b.archiveId left join file_class c on a.classid=c.id where a.status='Active' and (b.empid=" + empid + " or b.depid=(Select DepartmentId from MEmployee where id=" + empid + ") or sanaol=1) order by a.date desc");
        //DataTable dt = dbhelper.getdata("select id,location, filename+'_'+convert(varchar,id)+filename2 filename from file_details where status='Active' ");
        //grid_req.DataSource = dt;
        //grid_req.DataBind();
        gvCustomers.DataSource = dt;
        gvCustomers.DataBind();
        div_msg.Visible = dt.Rows.Count == 0 ? true : false;
    }

    protected void download(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_viewreq = (LinkButton)sender;
            DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
            string input = Server.MapPath("~/" + dt.Rows[0]["location"].ToString() + "/") + dt.Rows[0]["filename"].ToString().Replace(" ", "") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString();

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }

    //Get all document viewers depend on document id 
    [WebMethod]
    public static string[] viewers(string id)
    {
        int idd = Convert.ToInt32(id);

        List<string> people = new List<string>();

        using (SqlConnection con = new SqlConnection(Config.connection()))
            {
                string query = string.Format("select a.empid id,(Select LastName+', '+FirstName from MEmployee where Id=a.empid) fullname from file_viewers a left join file_details b on a.archiveId=b.id where a.archiveId='{0}'  ", idd);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        people.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                    }
                }
                con.Close();
            }

            return people.ToArray();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string customerId = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
            gvOrders.DataSource = dbhelper.getdata("select a.date,a.id,a.location,c.status, filename+'_'+convert(varchar,a.id)+filename2 filename,b.empid,c.description category from file_details a left join file_viewers b on a.id=b.archiveId left join file_class c on a.classid=c.id where a.status='Active' and c.description='" + customerId + "' and (b.empid=" + emp_id + " or b.depid=(Select DepartmentId from MEmployee where id=" + emp_id + ") or sanaol=1) order by a.date desc");
            gvOrders.DataBind();

        }
    }
    protected void pageindexing(object sender, GridViewPageEventArgs e)
    {
        disp();
        gvCustomers.PageIndex = e.NewPageIndex;
        gvCustomers.DataBind();
    }
}