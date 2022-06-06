using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_masterfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PreLoad();
            MasterFile();
        }
    }

    protected void MasterFile()
    {
        //string dept = ddl_department_group.SelectedValue == "0" ? "" : "and DepartmentId=" + ddl_department_group.SelectedValue;
        //string filter = ddl_payroll_group.SelectedItem.ToString() == "Resigned" ? "and statusID=1" : "and statusID=0";
        //DataTable dt = dbhelper.getdata(adjustdtrformat.allemployee() + " where PayrollGroupId=" + ddl_payroll_group.SelectedValue + " " + dept + " " + filter + " order by Fullname asc");
        //ViewState["data"] = dt;

        DataTable dt = dbhelper.getdata(adjustdtrformat.allemployee() + " where PayrollGroupId=" + ddl_payroll_group.SelectedValue + " order by Fullname asc");
        ViewState["data"] = dt;
    }

    protected void PreLoad()
    {
        DataTable dt = dbhelper.getdata("select * from MPayrollGroup where status=1 order by id asc");
        ddl_payroll_group.Items.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        DataTable dt2 = dbhelper.getdata("select * from MDepartment order by department asc");
        ddl_department_group.Items.Clear();

        ddl_department_group.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt2.Rows)
        {
            ddl_department_group.Items.Add(new ListItem(dr["Department"].ToString(), dr["Id"].ToString()));
        }
       

    }

    protected void OnChange_PayrollGroup(object sender, EventArgs e)
    {
        MasterFile();
    }

    protected void OnChange_DepartmentGroup(object sender, EventArgs e)
    {
        MasterFile();
    }

    protected void Click_Search(Object sender, EventArgs e)
    {
        MasterFile();
    }
}