using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_leavemonitoring : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            loadable();
            leave();
        }
    }

    protected void loadable()
    {
        ddl_yyyy.Items.Clear();

        for (int i = 2019; i <= DateTime.Now.Year + 4; i++)
            ddl_yyyy.Items.Add(new ListItem(i.ToString(), i.ToString()));

        ddl_yyyy.SelectedValue = DateTime.Now.Year.ToString();
    }

    protected void btn_search(object sender, EventArgs e)
    {
        leave();
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gv = (GridView)e.Row.FindControl("gv");
            string query = "select a.id,a.yyyyear,a.empid,a.leaveid, b.Leave,a.Credit, " +
            "(case when(a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) is not null " +
            "then " +
            "case when (a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) <0  " +
            "then 0 else (a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) end  " +
            "else a.credit end ) as Balance  " +
            "from leave_credits a   " +
            "left join MLeave b on a.leaveid=b.Id  " +
            "left join memployee c on a.empid=c.id  " +
            "where c.id = "+e.Row.Cells[0].Text +" " +
            "and a.yyyyear = "+ddl_yyyy.SelectedValue +"";

            DataTable dt = dbhelper.getdata(query);
            gv.DataSource = dt;
            gv.DataBind();


        }
    }

    protected void leave()
    {
        string query = "select b.id, b.IdNumber, b.FullName " +
        "from Approver a " +
        "left join memployee b on a.emp_id = b.Id " +
        "where under_id=" + Session["emp_id"] + " " +
        "and a.under_id <> b.Id";

        //string query = "select c.IdNumber,c.LastName+', '+c.FirstName+' '+c.MiddleName emp_name, a.id,a.yyyyear,a.empid,a.leaveid, b.Leave,a.Credit,  " +
        //"( " +
        //"case when(a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) is not null  " +
        //"then  " +
        //"case when (a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) <0  " +
        //"then 0 else (a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' ) is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where EmployeeId=a.empid and LeaveId=a.leaveid  and status like '%Approved%' )end)) end  " +
        //"else a.credit end  " +
        //") " +
        //"as Balance  " +
        //"from leave_credits a   " +
        //"left join MLeave b on a.leaveid=b.Id  " +
        //"left join memployee c on a.empid=c.id  " +
        //"where c.departmentid='" + Session["department"].ToString() + "' and a.yyyyear=" + ddl_yyyy.SelectedValue + " " +
        //"order by c.LastName";

        DataTable dt = dbhelper.getdata(query);

        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }

    protected void cpop(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal.Style.Add("display", "block");

            string query = "select c.LastName+', '+c.FirstName+' '+c.MiddleName Employee,d.Department,left(CONVERT(varchar,a.date,101),10)Date,e.Leave,a.NumberOfHours NoDAYS,a.WithPay,a.Remarks from TLeaveApplicationLine a " +
                                "left join Tleave b on a.LeaveId=b.id " +
                                "left join memployee c on a.EmployeeId=c.Id " +
                                "left join MDepartment d on c.DepartmentId=d.Id " +
                                "left join MLeave e on a.LeaveId=e.Id " +
                                "where a.status like'%Approved%' and a.EmployeeId='" + row.Cells[0].Text + "' and a.LeaveId='" + row.Cells[1].Text + "' ";
            DataTable dt = dbhelper.getdata(query);


            grid_history.DataSource = dt;
            grid_history.DataBind();

            //if (ddl_year.SelectedValue != "0")
            //    query += " and YEAR(a.date)=" + ddl_year.SelectedValue + " ";
        }


    }

}