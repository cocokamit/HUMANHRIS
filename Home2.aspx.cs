using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Home2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {
            //autocs();
        }
        loader();
        modal.Style.Add("display", "none");
    }

    protected void autocs()
    {
        //DataTable dtday = dbhelper.getdata("select datename(dw,GETDATE())dayname");
        //if (dtday.Rows[0]["dayname"].ToString() == "Sunday" || dtday.Rows[0]["dayname"].ToString() == "Saturday")
        //{ }
        //else
        //{
        //    DataTable todate = dbhelper.getdata("select convert(varchar,GETDATE(),23)todate");
        //    DataTable empid = dbhelper.getdata("select Id from MEmployee where PayrollGroupId <> 4 and PayrollGroupId <> 7");
        //    foreach (DataRow dr in empid.Rows)
        //    {
        //        DataTable dtcheckadd = dbhelper.getdata("select * from TChangeShiftLine where EmployeeId=" + dr["Id"] + " and Date='" + todate.Rows[0]["todate"].ToString() + "'");
        //        if (dtcheckadd.Rows.Count == 0)
        //        {
        //            DataTable approver = dbhelper.getdata("select TOP 1 under_id from Approver where emp_id=" + dr["Id"].ToString() + "");
        //            if (approver.Rows.Count > 0)
        //            {
        //                dbhelper.getdata("insert into TChangeShiftLine values('0','" + dr["Id"].ToString() + "','" + todate.Rows[0]["todate"].ToString() + "','13','scheduler','approved',NULL)");
        //                dbhelper.getdata("insert into temp_shiftcode values('" + todate.Rows[0]["todate"].ToString() + "','" + dr["Id"].ToString() + "','2','13','Approved','COVID19','" + approver.Rows[0]["under_id"].ToString() + "','2')");
        //            }
        //            else
        //            {
        //                dbhelper.getdata("insert into TChangeShiftLine values('0','" + dr["Id"].ToString() + "','" + todate.Rows[0]["todate"].ToString() + "','13','scheduler','approved',NULL)");
        //                dbhelper.getdata("insert into temp_shiftcode values('" + todate.Rows[0]["todate"].ToString() + "','" + dr["Id"].ToString() + "','2','13','Approved','COVID19','0','2')");
        //            }
        //        }
        //    }
        //}
    }

    protected void loader()
    {
        DataTable dt = dbhelper.getdata("Select * from HomeUpload where type='banner'");

        int counter = 1;

        if (dt.Rows.Count > 0)
        {

            homebanner.ImageUrl = "~/files/HomeImages/" + dt.Rows[0]["imageurl"].ToString();
        }

        dt = dbhelper.getdata("Select * from HomeUpload where type!='banner' order by id desc");

        if (dt.Rows.Count > 0)
        {
           
                if (dt.Rows.Count == 1)
                { foreach (DataRow rows in dt.Rows)
                     {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    HtmlGenericControl imager = new HtmlGenericControl("img");
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("href", rows["url"].ToString());
                    anchor.Attributes.Add("target", "_blank");
                    anchor.Attributes.Add("style", "curser:pointer");

                    div1.Attributes.Add("class","columnr");
                    div1.Attributes.Add("style", " -ms-flex: 50%; flex: 50%; max-width: 50%;");
                    imager.Attributes.Add("style", "width:100%;");
                    imager.Attributes.Add("src","files/HomeImages/" +rows["imageurl"].ToString());

                    anchor.Controls.Add(imager);
                    div1.Controls.Add(anchor);

                    divannouncement.Controls.Add(div1);
                    }
                }
                else if (dt.Rows.Count == 2)
                {
                    foreach (DataRow rows in dt.Rows)
                    {
                        HtmlGenericControl div1 = new HtmlGenericControl("div");
                        HtmlGenericControl imager = new HtmlGenericControl("img");
                       
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("href", rows["url"].ToString());
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("style", "curser:pointer");

                        div1.Attributes.Add("class", "columnr");
                        div1.Attributes.Add("style", " -ms-flex: 50%; flex: 50%; max-width: 50%;");
                        imager.Attributes.Add("style", "width:100%;");
                        imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                        anchor.Controls.Add(imager);
                        div1.Controls.Add(anchor);

                        divannouncement.Controls.Add(div1);
                    }
                }
                else
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    HtmlGenericControl div2 = new HtmlGenericControl("div");
                    HtmlGenericControl div3 = new HtmlGenericControl("div");

                    div1.Attributes.Add("class", "columnr");
                    div2.Attributes.Add("class", "columnr");
                    div3.Attributes.Add("class", "columnr");

                    foreach (DataRow rows in dt.Rows)
                    {
                        HtmlGenericControl imager = new HtmlGenericControl("img");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");

                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("style", "curser:pointer;");
                        if (counter == 1)
                        {
                            anchor.Attributes.Add("href", rows["url"].ToString());
                            
                            imager.Attributes.Add("style", "width:100%;");
                            imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                            anchor.Controls.Add(imager);

                            div1.Controls.Add(anchor);
                            counter = 2;
                        }
                        else if (counter == 2)
                        {
                            anchor.Attributes.Add("href", rows["url"].ToString());

                            imager.Attributes.Add("style", "width:100%;");
                            imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                            anchor.Controls.Add(imager);

                            div2.Controls.Add(anchor);
                            counter = 3;
                        }
                        else if (counter == 3)
                        {
                            anchor.Attributes.Add("href", rows["url"].ToString());

                            imager.Attributes.Add("style", "width:100%;");
                            imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                            anchor.Controls.Add(imager);

                            div3.Controls.Add(anchor);
                            counter = 1;
                        }
                    }

                    divannouncement.Controls.Add(div1);
                    divannouncement.Controls.Add(div2);
                    divannouncement.Controls.Add(div3);
                }
                  
        }
        grid_forms.DataSource = dt;
        grid_forms.DataBind();

        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;
    }
    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }

    protected void onmodal(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        txt_url.Text = "";
        ViewState["type"] = lb.ID;
        if (lb.ID == "banner")
        {
            txt_url.Visible = false;
            lb3.Visible = false;
            divgrid.Visible = false;
        }
        else
        {
            divgrid.Visible = true;
            txt_url.Visible = true;
            lb3.Visible = true;
        }
        modal.Style.Add("display", "block");
    }

    protected void announce(object sender, EventArgs e)
    {
        string type = ViewState["type"].ToString();
        if (FileUpload1.HasFile)
        {
            string filename = null;
            string filepath = Server.MapPath("files/HomeImages/");
            DirectoryInfo di = Directory.CreateDirectory(filepath);
            HttpFileCollection uploadedFiles = Request.Files;
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                int n = new Random().Next(1, 100);

                HttpPostedFile userPostedFile = uploadedFiles[i];

                if (userPostedFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(userPostedFile.FileName);
                    string fileExtension = Path.GetExtension(userPostedFile.FileName);
                    string contenttype = userPostedFile.ContentType;
                    filename = filepath + fileName.Replace(" ", "") + "_" + Convert.ToString(n) + fileExtension;
                    userPostedFile.SaveAs(filename);

                    if (ViewState["type"].ToString() == "banner")
                    {
                        DataTable dtt = dbhelper.getdata("Delete from HomeUpload where type='banner'");
                    }

                    DataTable dt = dbhelper.getdata("Insert into HomeUpload values('" + fileName.Replace(" ", "") + "_" + Convert.ToString(n) + fileExtension + "','" +DateTime.Now.ToString("MM/dd/yyyy")+ "','','" + type + "','" + txt_url.Text + "')");
                 }
            }
        }
        Response.Redirect("homepage");

    }


    protected void deleteer(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dtt = dbhelper.getdata("Select * from HomeUpload where id=" + row.Cells[0].Text + "");
            var filePath = Server.MapPath("~/files/HomeImages/" + dtt.Rows[0]["imageurl"].ToString());
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
             dtt = dbhelper.getdata("Delete from HomeUpload where id=" + row.Cells[0].Text + "");

            Response.Redirect("homepage");
        }
    }
}