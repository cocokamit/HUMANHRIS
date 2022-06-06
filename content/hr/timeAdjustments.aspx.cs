using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_timeAdjustments : System.Web.UI.Page
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
        DataTable dt = dbhelper.getdata("select id,manual_type from time_adjustment");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void add_adjustment(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("insert into time_adjustment values('" + txt_aa.Text.Replace("'", "").Trim() + "')");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Mtimeadjust'", true);
    }



    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
           string query = "select * from time_adjustment where Id=" + id.Value + "";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);

            txt_type.Text = dt.Rows[0]["manual_type"].ToString();
            
        }
    }

    protected void update(object sender, EventArgs e)
    {
       string query = "update time_adjustment set manual_type='" + txt_type.Text.Replace("'","").Trim() + "' where id=" + id.Value + "  ";
        dbhelper.getdata(query);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Updated Successfully'); window.location='Mtimeadjust'", true);
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("Mtimeadjust?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void click_add_adj(object sender, EventArgs e)
    {

    }
}