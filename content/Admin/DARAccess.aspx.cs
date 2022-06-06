using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Admin_DARAccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {

        }
        loader();

        modal.Style.Add("display", "none");
        viewForm.Style.Add("display", "none");
    }

    protected void editor(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        if (lb.ID == "btn_compose")
        {
            txtmc.Text = "";
            txtlocation.Text = "";
            chkallow.Checked=true;

            modal.Style.Add("display", "block");
        }
        if (lb.ID == "viewbtn")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                DataTable dtt = dbhelper.getdata("Select * from bio_device where id=" + row.Cells[0].Text + "");

                if (dtt.Rows.Count > 0)
                {
                    txt_mc.Text = dtt.Rows[0]["mcaddress"].ToString();
                    txt_loc.Text = dtt.Rows[0]["location"].ToString();
                    if (dtt.Rows[0]["status"].ToString() == "allow")
                    {
                        chk_allow.Checked = true;
                    }
                    else
                    {
                        chk_allow.Checked = false;
                    }
                }
                ViewState["iddevice"] = row.Cells[0].Text;

                viewForm.Style.Add("display", "block");
            }
        }
        else if (lb.ID == "delbtn")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                DataTable dt = dbhelper.getdata("Delete from bio_device where id =" + row.Cells[0].Text + "");
                loader();
            }
        }
    }

    protected void loader()
    {
        DataTable dt = dbhelper.getdata("Select * from bio_device order by id desc");
        grid_forms.DataSource = dt;
        grid_forms.DataBind();


        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;
    }
    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");

        viewForm.Style.Add("display", "none");
    }

    protected void save_device(object sender, EventArgs e)
    { 
        string allower="allow";
        if(!chkallow.Checked)
        {
            allower="NULL";
        }
        DataTable dt = dbhelper.getdata("Insert into bio_device([status],mcaddress,location) values('" + allower + "','" + txtmc.Text.Replace(" ","").Replace("-","") + "','"+txtlocation.Text+"')");
        loader();
    }

    protected void edit_device(object sender, EventArgs e)
    {
        string allower = "allow";
        if (!chk_allow.Checked)
        {
            allower = "NULL";
        }
        string id=ViewState["iddevice"].ToString();
        DataTable dt = dbhelper.getdata("Update bio_device set [status]='" + allower + "', mcaddress='" + txt_mc.Text + "',location='" + txt_loc.Text + "' where id=" + id + "");
        loader();
    }
}