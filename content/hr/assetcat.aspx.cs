using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_hr_assetcat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            display();
        }
    }
    protected void display()
    {
        string query = "select * from asset_cat order by Description asc";
        DataTable dt = dbhelper.getdata(query);
        
        grid_category.DataSource = dt;
        grid_category.DataBind();

    }
    protected void addcategory(object sender, EventArgs e)
    {
        catPanel.Style.Add("top", "10%");
        overlay.Visible = true;
        catPanel.Visible = true;
    }
    protected void closepopup(object sender, EventArgs e)
    {
        overlay.Visible = false;
        catPanel.Visible = false;
        overlay2.Visible = false;
        catPanel2.Visible = false;
    }
    protected void savecategory(object sender, EventArgs e)
    {
        DataTable dtchek = dbhelper.getdata("select * from asset_cat where description='" + txt_category.Text + "'");
        if (dtchek.Rows.Count == 0)
        {
            string ser = chk_srlized.Checked == true ? "True" : "False";
            DataTable dt = dbhelper.getdata("insert into asset_cat(description,serialized,um) values('" + txt_category.Text + "','" + ser + "','" + ddl_um.SelectedItem.Text + "')select scope_identity() idd");
            display();
            Response.Redirect("assetcat", true);
        }
        else
            lbl_err.Text = "Data Exist!";
    }
    protected void updatearea(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from asset_cat where id = '" + seriesid.Value + "'");
            txt_desc.Text = dt.Rows[0]["Description"].ToString();
            ddl_unitm.Text = dt.Rows[0]["um"].ToString();
            if (dt.Rows[0]["serialized"].ToString() == "True")
            {
                chk_serial.Checked = true;
            }
            catPanel2.Style.Add("top", "10%");
            overlay2.Visible = true;
            catPanel2.Visible = true;
        }
    }
    protected void updatecategory(object sender, EventArgs e)
    {
        string val = "0";
        if (chk_serial.Checked)
        {
            val = "1";
            dbhelper.getdata("update asset_inventory set serial = '' where categoryid = " + seriesid.Value + "");
        }
        dbhelper.getdata("update asset_cat set Description = '" + txt_desc.Text + "', serialized = '" + val + "', um = '" + ddl_unitm.Text + "' where id = '" + seriesid.Value + "'");
        dbhelper.getdata("update asset_inventory set serial = '-' where categoryid = " + seriesid.Value + "");
        Response.Redirect("~/assetcat");
    }
}