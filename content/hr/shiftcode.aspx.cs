using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_shiftcode : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp_shiftcode();
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        }
    }
    protected void disp_shiftcode()
    {
        DataTable dt = dbhelper.getdata("select * from MShiftCode where status is null");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_add_shiftcode(object sender, EventArgs e)
    {
        Response.Redirect("addshiftcode", false);
    }
    protected void click_viewshiftcode(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("editshiftcode?shift_code_id=" + row.Cells[0].Text + "", false);
        }
    }
    protected void click_can(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MShiftCode set status='cancel-" + Session["user_id"] + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("Mshiftcode", false);
            }
            else
            {
            }
        }
    }
}