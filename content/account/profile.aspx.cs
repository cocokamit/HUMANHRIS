using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_account_profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void changeprofile(object sender, EventArgs e)
    {
        //UPLOAD PROFILE
        if (dataURLInto.InnerText.Length > 0)
        {
            DataTable dt = dbhelper.getdata("select * from file_details where empid=" + Session["emp_id"] + " and classid=1 and location = 'files/adminprofile'");
            if (dt.Rows.Count == 0)
                dbhelper.getdata("insert into file_details values (getdate(),0," + Session["emp_id"] + ",0,1,'files/adminprofile','.png','adminprofile','Active','image/png',0,0,0)");

            string base64 = dataURLInto.InnerText;
            byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
            string path = Server.MapPath("~/files/adminprofile/");
            using (System.IO.FileStream stream = new System.IO.FileStream(path + Session["emp_id"] + ".png", System.IO.FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            System.Drawing.Image image = System.Drawing.Image.FromFile(path + Session["emp_id"] + ".png");
            System.Drawing.Image thumb = image.GetThumbnailImage(50, 50, delegate() { return false; }, (IntPtr)0);
            thumb.Save(path + "/" + Session["emp_id"] + "-thumb.png");
            thumb.Dispose();
        }
    }
    protected void viewupdate(object sender, EventArgs e)
    {
        pgridview.Visible = true;
        pgridview1.Visible = true;
    }

    protected void closepopup(object sender, EventArgs e)
    {
        pgridview.Visible = false;
        pgridview1.Visible = false;
    }

    protected void updatepassword(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select Password from MUser where Id=1");
        if (dt.Rows[0]["Password"].ToString() == txtoldpass.Text)
        {
            if (txtnewpass.Text == txtconpass.Text)
            {
                dbhelper.getdata("update MUser set Password='" + txtconpass.Text + "'where Id=1");
                Response.Write("<script>alert('Successfully Changed Password!')</script>");
            }
            else
            {
                Response.Write("<script>alert('Password Not Match!')</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('Password Not Recognize!')</script>");
        }
        txtoldpass.Text = "";
    }
}