using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_DTR_logs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Loadable();
            SetRange();
            BindData();
        }
    }

    protected void GetDepartment()
    {
        string query = "select * from MDepartment order by department asc";
        DataTable dt = dbhelper.getdata(query);
        ddl_dept.Items.Clear();
        ddl_dept.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_dept.Items.Add(new ListItem(dr["Department"].ToString(), dr["id"].ToString()));
        }
        ddl_dept.SelectedIndex = 1;
    }

    protected void GetSection()
    {
        string query = "select * from msection where dept_id=" + ddl_dept.SelectedValue + " order by seccode";
        DataTable dt = dbhelper.getdata(query);
        ddl_section.Items.Clear();

        if (dt.Rows.Count > 0)
        {
            ddl_section.Items.Add(new ListItem("All", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
            }

            ddl_section.SelectedIndex = 1;
        }

        pnlSection.Visible = dt.Rows.Count <= 1 ? false : true;
    }

    protected void Loadable()
    {
        GetDepartment();
        GetSection();
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

    protected void SetRange()
    {
        txt_from.Text = DateTime.Today.ToShortDateString();
        txt_to.Text = DateTime.Today.ToShortDateString();
    }

    protected void BindData()
    {
        string dept = ddl_dept.SelectedIndex == 0 ? "" : "and b.DepartmentId= " + ddl_dept.SelectedValue.ToString();
        string section = string.Empty;
        if(pnlSection.Visible)
            section = ddl_section.SelectedIndex == 0 ? "" : " and b.sectionid=" + ddl_section.SelectedValue.ToString();

        string query = "where a.biotime between convert(datetime,'" + txt_from.Text.Replace(" ", "") + "') and convert(datetime,'" + txt_to.Text.Replace(" ", "") + "') + 1 " +
        dept + "  " + section + " " +
        "order by a.biotime desc";

        DataTable dt = getdata.GetEmpRawLogs(query);
        ViewState["data"] = dt;
        pnl_grid.Visible = dt.Rows.Count == 0 ? false : true;
        pnl_alert.Visible = dt.Rows.Count == 0 ? true : false;
    }

    protected void Filter(object sender,EventArgs e)
    {
        BindData();
    }
}