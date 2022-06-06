using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_attinout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            Loadable();
            BindData();
        }
    }

    protected void BindData()
    {
            if (tb_from.Text.Count() > 0 && tb_to.Text.Count() > 0)
            {
                //+ ",section_" + ddl_section.SelectedValue
                DataTable dtr = Core.DTRF(tb_from.Text, tb_to.Text, "attlogs_" + ddl_dept.SelectedValue );
                grid_item.DataSource = dtr;
                grid_item.DataBind();
            }

            alert.Visible = grid_item.Rows.Count == 0 ? true : false;
    }

    protected void Loadable()
    {
        GetDepartment();
        GetSection();
    }

    protected void GetDepartment()
    {
        string query = "select * from MDepartment order by department asc";
        DataTable dt = dbhelper.getdata(query);
        ddl_dept.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_dept.Items.Add(new ListItem(dr["Department"].ToString(), dr["id"].ToString()));
        }
    }

    protected void GetSection()
    {
        try
        {
            string query = "select * from msection where dept_id=" + ddl_dept.SelectedValue + " order by seccode";
            DataTable dt = dbhelper.getdata(query);
            ddl_section.Items.Clear();
            ddl_section.Items.Add(new ListItem("All", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
            }

            ddl_section.SelectedIndex = 1;
            pnlSection.Visible = dt.Rows.Count == 1 ? false : true;
        }
        catch
        { }
    }

    protected void OnChangeDept(object sender, EventArgs e)
    {
        GetSection();
        BindData();
    }

    protected void OnChangeSection(object sender, EventArgs e)
    {
        BindData();
    }

    protected void clickSearch(object sender, EventArgs e)
    {
        BindData();
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace("-", " ");
        }
    }
}