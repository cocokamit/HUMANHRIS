using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_Approval : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {
        id.Value = Request.QueryString["id"].ToString();
        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,SUBSTRING(status,0, CHARINDEX('-',b.status)) status, " +
                   "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                   "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                   "c.LastName +', '+c.FirstName +' '+ c.MiddleName as fullname, " +
                   "d.Leave, " +
                   "e.Department  " +
                   "from Tleave a " +
                   "left join TLeaveApplicationLine b on a.id=b.l_id " +
                   "left join MEmployee c on b.EmployeeId=c.id " +
                   "left join MLeave d on b.LeaveId=d.Id " +
                   "left join MDepartment e on c.DepartmentId=e.id " +
                   "where a.id = " + id.Value + "";
        // and b.BranchId='" + dt.Rows[0]["BranchId"].ToString() + "' and b.DepartmentId='" + dt.Rows[0]["DepartmentId"].ToString() + "'

        DataTable dt = new DataTable();
        dt = dbhelper.getdata(query);
        if (dt.Rows[0]["status"].ToString() == "Approved")
        {
            Button1.Visible = false;
        }
        else
        {
            Button1.Visible = true;
        }
        grid_view.DataSource = dt;
        grid_view.DataBind();
        query = "select b.id lineid, a.id transid,a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,SUBSTRING(status,0, CHARINDEX('-',b.status)) status, " +
                  "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                  "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                  "c.LastName +', '+ c.FirstName+' '+ c.MiddleName as fullname, " +
                  "c.id empid, " +
                  "d.Leave, " +
                  "d.id leaveid, " +
                  "e.Department  " +
                  "from Tleave a " +
                  "left join TLeaveApplicationLine b on a.id=b.l_id " +
                  "left join MEmployee c on b.EmployeeId=c.id " +
                  "left join MLeave d on b.LeaveId=d.Id " +
                  "left join MDepartment e on c.DepartmentId=e.id " +
                  "where a.id = " + id.Value + "";
        dt = dbhelper.getdata(query);
        ViewState["leavetrans"] = dt;
    }
    protected void approved(object sender, EventArgs e)
    {
        DataTable dt = ViewState["leavetrans"] as DataTable;
        foreach (DataRow dr in dt.Rows)
        {
            DataTable dtchkleavebal =dbhelper.getdata("select a.id,a.Credit, " +
                        "case when (a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.leaveid  and status like '%Approve%') is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.leaveid  and status like '%Approve%')  end))>0 " +
                        "then " +
                        "(a.credit-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.leaveid  and status like '%Approve%') is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.leaveid  and status like '%Approve%')  end)) " +
                        "else '0' end as Balance " +
                        "from leave_credits a " +
                        "where a.empid='" + dr["empid"] + "' and a.leaveid='" + dr["leaveid"] + "'  and a.action is null ");
            decimal bal = 0;
            if (dtchkleavebal.Rows.Count > 0)
            {
                if (decimal.Parse(dtchkleavebal.Rows[0]["Balance"].ToString()) > 0)
                    bal = 1;
            }
            string wp=bal>0?"True":"False";
            dbhelper.getdata("update TLeaveApplicationLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "',WithPay='" + wp + "' where id=" + dr["lineid"] + "");
        }
        dbhelper.getdata("update Tleave set admin_remarks='" + txt_remarks.Text + "' where id=" + id.Value + "");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Approved Successfully'); window.location='Approval?id="+ id.Value + "'", true);
    }

    protected void delete3(object sender, EventArgs e)
    {
        if (TextBox1.Value == "Yes")
        {
            dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + id.Value + "");
            dbhelper.getdata("update Tleave set admin_remarks='" + txt_remarks.Text + "' where id=" + id.Value + "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancelled Successfully'); window.location='Approval?id=" + id.Value + "'", true);
        }
        else
        { }
    }
}