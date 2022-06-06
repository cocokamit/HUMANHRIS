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

public partial class content_hr_archive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
     
            loadable();
            disp();
       
    }
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string[] recips = term.Split(';');
            string query = string.Format("select CONVERT(varchar(10),a.id)+CONVERT(varchar(10),'#') id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%' union all select CONVERT(varchar(10),id)+CONVERT(varchar(10),'?') id,Department as fullname from MDepartment where Department like '%{0}%'", recips[recips.Length - 1]);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                    //retCategory.Add(reader.GetString(0));
                }
            }
          
            con.Close();
        }
        return retCategory.ToArray();
    }
    protected void loadable()
    {
    
        DataTable dt = dbhelper.getdata("select * from file_class order by description asc");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_class2.Items.Add(new ListItem(dr["description"].ToString(), dr["id"].ToString()));
            //populate folder list-------------------------------------------------------------
            
            LinkButton li = new LinkButton();
            LinkButton delbtn = new LinkButton();

            li.Text = "  \u00a0 \u00a0" + dr["description"].ToString();
            li.Attributes.Add("Class", "col-md-10 list-group-item fa fa-folder");
            li.Attributes.Add("href", "archive?open=" + dr["id"].ToString());
            delbtn.Attributes.Add("Class", "col-md-2 list-group-item fa fa-trash");
            delbtn.Attributes.Add("Value", dr["description"].ToString());
            delbtn.Attributes.Add("runat", "server");
            delbtn.OnClientClick = "return confirm('Are you sure you want to export?')";
            delbtn.Click += (sender, EventArgs) => { delbtn_Click(sender, EventArgs, dr["id"].ToString()); }; ;
           
            Folders.Controls.Add(li);

            Folders.Controls.Add(delbtn);
        }
        LinkButton li2 = new LinkButton();
        li2.Text = " New Folder";
        li2.Attributes.Add("Class", "col-md-12 list-group-item fa fa-plus ");
        li2.Attributes.Add("data-toggle", "modal");
        li2.Attributes.Add("data-target", "#modalCreateNewFolder");
        Folders.Controls.Add(li2);
    }

   
    private void delbtn_Click(object sender, EventArgs e,string id)
    {
        int idd = Convert.ToInt32(id);
        dbhelper.getdata("delete from file_class where id="+idd+"");
        Response.Redirect("archive?open=''");
    }

    protected void disp()
    {   
        string folderID =Request.QueryString["open"].ToString()!=""?Request.QueryString["open"].ToString():"1";
        DataTable dt = dbhelper.getdata("select id,location, filename+'_'+convert(varchar,id)+filename2 filename from file_details where classid=" + folderID + " and status='Active' and filecode is NULL order by id desc");
        grid_req.DataSource = dt;
        grid_req.DataBind();
        div_msg.Visible = dt.Rows.Count == 0 ? true : false;

        lb_folname.Text=dt.Rows.Count>0?dt.Rows[0]["location"].ToString():"";
    }
    protected void clickcancelreq(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_req.Rows[row.RowIndex].Cells[3].FindControl("lnk_req");
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update file_trn set action='cancel-"+Session["user_id"].ToString()+"' where id=" + lb.CommandName + "");
                dbhelper.getdata("update file_details set status='cancel' where trn_file_id=" + lb.CommandName + "");
                string query = "select left(convert(varchar,a.date,101),10)date, a.id,a.description,b.LastName+', '+b.FirstName+' '+b.MiddleName fullname,c.description class from file_trn a " +
                "left join MEmployee b on a.empid=b.Id " +
                "left join file_class c on a.classid=c.Id " +
                "where a.action is null and a.classid=" + ddl_class2.SelectedValue + " order by a.date desc ";
                DataTable dt = dbhelper.getdata(query);
                grid_req.DataSource = dt;
                grid_req.DataBind();
                div_msg.Visible = dt.Rows.Count == 0 ? true : false;
            }
            else { }
        }
    }
    protected void clickclass(object sender, EventArgs e)
    {
    }
    protected bool chk()
    {
       bool err = true;

        return err;
 
    }
    protected void clickupload(object sender, EventArgs e)
    {
        string classs = "";
        string filename = "";
        if (ddl_class2.SelectedValue == "3")
            classs = "forms";
        else
            classs = "peremp"; 

        if (chk())
        {
            if (FileUpload1.HasFile)
            {
                string filepath = Server.MapPath("files/" + classs + "/");
                DirectoryInfo di = Directory.CreateDirectory(filepath);
                HttpFileCollection uploadedFiles = Request.Files;

                string[] recips = Label1.Value.Split(';');
                int[] dep=new int[recips.Count()-1];
                int[] emp=new int[recips.Count()-1];
                for (int i = 0; i < recips.Count()-1;i++ )
                    {
                        if (recips[i].Contains('#'))
                        { 
                            emp[i]=Convert.ToInt32(recips[i].Replace('#',' ').Trim());
                            dep[i] = 0;
                        }
                        else if (recips[i].Contains('?'))
                        {
                            emp[i] =0;
                            dep[i] = Convert.ToInt32(recips[i].Replace('?', ' ').Trim());
                        }
                    }
              

                string empid = classs == "forms" ? "0" : hfEmp.Value;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];
                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userPostedFile.FileName);
                        string fileExtension = Path.GetExtension(userPostedFile.FileName);
                        string contenttype = userPostedFile.ContentType;
                       
                        string query = "insert into file_details(date,userid,empid,depid,classid,location,filename,description,status,contenttype,trn_file_id,filename2) " +
                            "values (getdate(),'" + Session["user_id"].ToString() + "',0,0,'" + ddl_class2.SelectedValue + "','files/" + classs + "','" + fileName + "','" + txt_desc.Text + "','Active','" + contenttype + "',0,'" + fileExtension + "') select scope_identity() id";
                        DataTable dt = dbhelper.getdata(query);

                        if (recips[0] == "All") {
                            string recipient = "insert into file_viewers(archiveId,empid,depid,sanaol) values (" + dt.Rows[0]["id"] + ",0,0,1)";
                            DataTable dt2 = dbhelper.getdata(recipient);
                        }
                        for(int j=0;j<emp.Count();j++)
                        {
                            string recipient = "insert into file_viewers(archiveId,empid,depid) values ("+dt.Rows[0]["id"]+","+emp[j]+","+dep[j]+")";
                            DataTable dt2 = dbhelper.getdata(recipient);
                        }
                        filename = filepath + fileName.Replace(" ","")+"_"+dt.Rows[0]["id"].ToString() + fileExtension;
                        userPostedFile.SaveAs(filename);
                    }
                }
                string  list="select id, filename+'_'+convert(varchar,id)+filename2 filename from file_details where classid=" + ddl_class2.SelectedValue + " and status='Active' ";
                DataTable dtlist = dbhelper.getdata(list);
                grid_req.DataSource = dtlist;
                grid_req.DataBind();
                div_msg.Visible = grid_req.Rows.Count == 0 ? true : false;
                ddl_emp.Text = "";
            }
        }

        Response.Redirect("archive?open=3");
    }
    protected void download(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_viewreq = (LinkButton)grid_req.Rows[row.RowIndex].Cells[1].FindControl("lnk_download");
            DataTable dt = dbhelper.getdata("select * from file_details where id=" + lnk_viewreq.CommandName + " ");
           // string classs = row.Cells[2].Text == "FORMS" ? "forms" : "peremp";
            string input = Server.MapPath("~/" + dt.Rows[0]["location"].ToString() + "/") + dt.Rows[0]["filename"].ToString().Replace(" ","") + "_" + dt.Rows[0]["id"].ToString() + dt.Rows[0]["filename2"].ToString(); 

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
    protected void candetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_can = (LinkButton)grid_req.Rows[row.RowIndex].Cells[1].FindControl("lnk_can");
            DataTable dt = dbhelper.getdata("update file_details set status='can' where id=" + lnk_can.CommandName + " ");
                DataTable dt1 = dbhelper.getdata("select id, filename+'_'+convert(varchar,id)+filename2 filename from file_details where classid=" + ddl_class2.SelectedValue + " and status='Active' ");
                grid_req.DataSource = dt1;
                grid_req.DataBind();
        }

    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("archive");
    }

    protected void clickaddfolder(object sender, EventArgs e)
    {
        string query = "insert into file_class(description,status) values (UPPER('" + txt_Nfolder.Text+ "'),'Active') select scope_identity() id";
        DataTable dt = dbhelper.getdata(query);
        Response.Redirect("archive?open=3");
    }

    //Get all document viewers depend on document id 
    [WebMethod]
    public static string[] viewers(string id)
    {
        int idd = Convert.ToInt32(id);

        List<string> people = new List<string>();

        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select case when a.empid=0 then a.depid else a.empid end id, case when (Select Count(LastName+', '+FirstName) from MEmployee where Id=a.empid)=0  then (case when (Select Count(Department) from MDepartment where Id=a.depid)=0 then 'All' else (Select Department from MDepartment where Id=a.depid) end) else (Select LastName+', '+FirstName from MEmployee where Id=a.empid) end fullname from file_viewers a left join file_details b on a.archiveId=b.id where a.archiveId='{0}'", idd);
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

    public EventHandler ButtonFinish_Click { get; set; }
}