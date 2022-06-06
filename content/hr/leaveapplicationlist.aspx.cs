using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_leaveapplicationlist : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }
    }
    protected void lodable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_add_leave(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addLEAVE?user_id=" + function.Encrypt(key.Value, true) + "", false);
       
    }
    protected void disp()
    {

        DataTable dt = dbhelper.getdata("select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10)sysdate,b.status, " +
                                       "b.remarks,(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                                       "(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                                       "c.LastName +', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                                       "d.Leave, " +
                                       "e.Department  " +
                                       "from Tleave a " +
                                       "left join TLeaveApplicationLine b on a.id=b.l_id " +
                                       "left join MEmployee c on b.EmployeeId=c.id " +
                                       "left join MLeave d on b.LeaveId=d.Id " +
                                       "left join MDepartment e on c.DepartmentId=e.id " +
                                       "where b.status like '%Approved%' and b.dtr_id is null");

        grid_view.DataSource = dt;
        grid_view.DataBind();

    }
    protected void click_search(object sender, EventArgs e)
    {

        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10)sysdate,b.status, " +
                                      "b.remarks,(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                                      "(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                                      "c.LastName +', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                                      "d.Leave, " +
                                      "e.Department  " +
                                      "from Tleave a " +
                                      "left join TLeaveApplicationLine b on a.id=b.l_id " +
                                      "left join MEmployee c on b.EmployeeId=c.id " +
                                      "left join MLeave d on b.LeaveId=d.Id " +
                                      "left join MDepartment e on c.DepartmentId=e.id " +
                                      "where b.status like '%Approved%' and b.dtr_id is null ";
        if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
        {
            query += "and LEFT(CONVERT(varchar,a.sysdate,101),10)>='" + txt_from.Text + "' and  LEFT(CONVERT(varchar,a.sysdate,101),10)<='" + txt_to.Text + "'";
        }
        if (int.Parse(ddl_pg.SelectedValue) > 0)
        {
            query += " and c.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        }
        if (txt_search.Text.Length > 0)
        {
            query += " and c.LastName + '' + c.FirstName + '' + c.MiddleName+''+c.IdNumber like '%" + txt_search.Text + "%'";
        }
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
}