using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_credentials : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id,b.firstname+' '+b.middlename+' '+b.lastname fullname,a.username,a.password,c.branch from MUser a " +
                                        "left join memployee b on a.emp_id=b.id " +
                                        "left join mbranch c on b.branchid=c.id");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='addCredentials?user_id=" + function.Encrypt(row.Cells[0].Text, true)+"'", true);

          //  Response.Redirect("addCredentials?user_id=" + function.Encrypt(row.Cells[0].Text, true));
        }
    }
}