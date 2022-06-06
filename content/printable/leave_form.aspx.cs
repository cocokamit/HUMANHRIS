using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_printable_leave_form : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        id.Value = function.Decrypt(Request.QueryString["key"].ToString(), true);
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
        string query = "select distinct(a.id),a.delegate,a.sysdate, " +
                    "b.remarks,(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                    "(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                    "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                    "d.Leave, " +
                    "e.Department  " +
                    "from Tleave a " +
                    "left join TLeaveApplicationLine b on a.id=b.l_id " +
                    "left join MEmployee c on b.EmployeeId=c.id " +
                    "left join MLeave d on b.LeaveId=d.Id " +
                    "left join MDepartment e on c.DepartmentId=e.id " +
                    "where a.id = " + id.Value + "";
        DataTable dt = new DataTable();
        dt = dbhelper.getdata(query);

        lbl_date.Text = DateTime.Parse(dt.Rows[0]["sysdate"].ToString()).ToShortDateString();
        lbl_name.Text = dt.Rows[0]["fullname"].ToString();
        lbl_reason.Text = dt.Rows[0]["remarks"].ToString();
        lbl_l.Text = dt.Rows[0]["Leave"].ToString();
        lbl_from.Text = DateTime.Parse(dt.Rows[0]["leavefrom"].ToString()).ToShortDateString();
        lbl_to.Text = DateTime.Parse(dt.Rows[0]["leaveto"].ToString()).ToShortDateString();
        lbl_delegate.Text = dt.Rows[0]["delegate"].ToString();
        lbl_dept.Text = dt.Rows[0]["Department"].ToString();
    }
}