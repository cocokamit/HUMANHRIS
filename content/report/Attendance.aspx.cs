using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_report_Attendance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
        }
    }
    protected void selectsearch(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select b.Id, a.Id, a.LastName+', '+FirstName+' '+MiddleName+'.' FullName, d.ShiftCode, c.Department, LEFT(convert(varchar, b.Date,101),10)Date, RIGHT(CONVERT(VARCHAR, b.TimeIn1, 100),7) as TimeIn1, RIGHT(CONVERT(VARCHAR, b.TimeOut2, 100),7) as TimeOut1 from MEmployee a left join TDTRLine b on b.EmployeeId = a.Id left join MDepartment c on a.DepartmentId = c.Id left join MShiftCode d on a.ShiftCodeId = d.Id where CAST([Date] as DATE) = '" + txt_inout.Text + "' order by a.FullName asc");
        if (dt.Rows.Count <= 0)
        {
            Response.Write("<script>alert('No Data Found!')</script>");
        }
        else
        {
            grid_inout.DataSource = dt;
            grid_inout.DataBind();
        }
        grid_inout.DataSource = dt;
        grid_inout.DataBind();
    }
    protected void selectdepartment(object sender, EventArgs e)
    {
        if (ddl_deptsearch.SelectedValue == "0")
        {
            DataTable dt = dbhelper.getdata("select b.Id, a.Id, a.LastName+', '+FirstName+' '+MiddleName+'.' FullName, d.ShiftCode, c.Department, LEFT(convert(varchar, b.Date,101),10)Date, RIGHT(CONVERT(VARCHAR, b.TimeIn1, 100),7) as TimeIn1, RIGHT(CONVERT(VARCHAR, b.TimeOut2, 100),7) as TimeOut1 from MEmployee a left join TDTRLine b on b.EmployeeId = a.Id left join MDepartment c on a.DepartmentId = c.Id left join MShiftCode d on a.ShiftCodeId = d.Id where c.Department = 'MARKETING' and CAST([Date] as DATE) = CAST(GETDATE() AS DATE)");
            grid_inout.DataSource = dt;
            grid_inout.DataBind();
        }
        else if (ddl_deptsearch.SelectedValue != "0")
        {
            DataTable dt = dbhelper.getdata("select b.Id, a.Id, a.LastName+', '+FirstName+' '+MiddleName+'.' FullName, d.ShiftCode, c.Department, LEFT(convert(varchar, b.Date,101),10)Date, RIGHT(CONVERT(VARCHAR, b.TimeIn1, 100),7) as TimeIn1, RIGHT(CONVERT(VARCHAR, b.TimeOut2, 100),7) as TimeOut1 from MEmployee a left join TDTRLine b on b.EmployeeId = a.Id left join MDepartment c on a.DepartmentId = c.Id left join MShiftCode d on a.ShiftCodeId = d.Id where c.Department like '%" + ddl_deptsearch.SelectedItem.ToString() + "%' and CAST([Date] as DATE) = '" + txt_inout.Text + "' order by a.FullName asc");
            grid_inout.DataSource = dt;
            grid_inout.DataBind();
        }
    }
    protected void loadable()
    {
        DataTable dt2 = dbhelper.getdata("select Department from MDepartment");
        ddl_deptsearch.Items.Clear();
        ddl_deptsearch.Items.Add(new System.Web.UI.WebControls.ListItem("", "0"));
        foreach (DataRow dr in dt2.Rows)
        {
            ddl_deptsearch.Items.Add(new System.Web.UI.WebControls.ListItem(dr["Department"].ToString()));
        }

        DataTable dt = dbhelper.getdata("select b.Id, a.Id, a.LastName+', '+FirstName+' '+MiddleName+'.' FullName, d.ShiftCode, c.Department, LEFT(convert(varchar, b.Date,101),10)Date, RIGHT(CONVERT(VARCHAR, b.TimeIn1, 100),7) as TimeIn1, RIGHT(CONVERT(VARCHAR, b.TimeOut2, 100),7) as TimeOut1 from MEmployee a left join TDTRLine b on b.EmployeeId = a.Id left join MDepartment c on a.DepartmentId = c.Id left join MShiftCode d on a.ShiftCodeId = d.Id where c.Department = 'MARKETING' and CAST([Date] as DATE) = CAST(GETDATE() AS DATE)");
        grid_inout.DataSource = dt;
        grid_inout.DataBind();
    }
}