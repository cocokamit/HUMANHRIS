using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_allow_admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }

    }

    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select id,name,allow from allow_admin");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void rowdatabound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            CheckBox lnkcan = (CheckBox)e.Row.FindControl("check_a");

            DataTable aw = dbhelper.getdata("select * from allow_admin where id=" + e.Row.Cells[0].Text + " ");
            if (aw.Rows[0]["allow"].ToString() == "yes")
                lnkcan.Checked = true;
            else
                lnkcan.Checked = false;
            
        }
    }

    protected void update(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in grid_view.Rows)
        {
            CheckBox lnkcan = (CheckBox)gvrow.FindControl("check_a");

            string aa = lnkcan.Checked == true ? "yes" : "no";
            dbhelper.getdata("update allow_admin set allow='" + aa + "' where id='" + gvrow.Cells[0].Text + "' ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Update Successfully'); window.location='"+Request.RawUrl+"'", true);
            
        }
    }

}