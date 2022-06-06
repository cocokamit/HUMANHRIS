using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_report_monitoring_leavelist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            transaction();
            if (gridtrans.Rows.Count > 0)
                exp_report.Visible = true;
            else
                exp_report.Visible = false;
        }
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void click_search(object sender, EventArgs e)
    {
        transaction();
        if (gridtrans.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = gridtrans.Rows.Count.ToString();
        string filename = "Leave_Monitoring";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            //grid_view.AllowPaging = false;
            //this.disp();
            // this.DataBind();
            //grid_view.DataBind();
            //this.BindGrid();

            gridtrans.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in gridtrans.HeaderRow.Cells)
            {
                cell.BackColor = gridtrans.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gridtrans.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gridtrans.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gridtrans.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gridtrans.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
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
        }
    }

    protected void cpop(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }
    protected void transaction()
    {
        if (txtfrom.Text == "" && txtto.Text == "")
        {
            string sql = "select b.id,d.Id empid,convert(varchar,a.sysdate,101)sysdate,a.Date,c.Leave,a.Remarks,a.status,d.LastName+', '+d.FirstName employee" +
                " from TLeaveApplicationLine a left join Tleave b on a.l_id=b.id left join MLeave c on a.LeaveId=c.Id left join MEmployee d on " +
                "a.EmployeeId=d.Id where d.LastName+', '+d.FirstName like '%" + txtemp.Text + "%' order by a.Date desc";
            DataTable leaves = dbhelper.getdata(sql);
            gridtrans.DataSource = leaves;
            gridtrans.DataBind();
        }
        else
        {
            string sql = "select b.id,d.Id empid,convert(varchar,a.sysdate,101)sysdate,a.Date,c.Leave,a.Remarks,a.status,d.LastName+', '+d.FirstName employee" +
                           " from TLeaveApplicationLine a left join Tleave b on a.l_id=b.id left join MLeave c on a.LeaveId=c.Id left join MEmployee d on " +
                           "a.EmployeeId=d.Id where d.LastName+', '+d.FirstName like '%" + txtemp.Text + "%' and a.Date between'" + txtfrom.Text + "' and" +
                           "'" + txtto.Text + "' order by a.Date desc";
            DataTable leaves = dbhelper.getdata(sql);
            gridtrans.DataSource = leaves;
            gridtrans.DataBind();
        }
    }
    protected void view1(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            lbl_lt.Text = row.Cells[2].Text;
            lbl_bals.Value = row.Cells[1].Text;

            string query = "select a.emp_id,a.under_id,a.herarchy, " +
             "b.FirstName +' '+b.LastName approver, " +
             "case when (select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) = 1 " +
             "then (select convert(varchar,date,101) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) else '--/--/--'  end date, " +
             "case when " +
             "(select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) = 1 " +
             "then (select SUBSTRING(status,0,CHARINDEX('-',status)) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) " +
             "else 'pending'  end [status] " +
             "from Approver a " +
             "left join memployee b on a.under_id = b.Id " +
             "where a.emp_id=" + lbl_bals.Value + " and a.under_id<> " + lbl_bals.Value + " ";


            DataTable tt = dbhelper.getdata("select * from allow_admin where id='6' and allow='no'");
            if (tt.Rows.Count == 0)
            {

                query += "union " +
                  "select " + lbl_bals.Value + ",0, " +
                  "(select COUNT(*) from Approver a where emp_id=" + lbl_bals.Value + "),'Admins', " +
                  "case when (select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
                  "then (select CONVERT(varchar,date,101) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
                  "else '--/--/--' end date," +
                  "case when " +
                  "(select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
                  "then (select SUBSTRING(status,0,CHARINDEX('-',status)) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
                  "else 'pending' end " +
                  "order by herarchy";
            }


            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_approver.DataSource = ds;
            grid_approver.DataBind();

            pnlApproverHistory.Visible = ds.Tables[0].Rows.Count == 0 ? false : true;

            nadarang();
            modaltrans.Style.Add("display", "block");
        }
    }

    protected void nadarang()
    {
        string query = "select employeeid request_id,id,l_id,DATE,case when WithPay='True' then 'Yes'else'No' end WithPay,case when numberofhours<1 then 'Halfday' else 'Wholeday' end nod, " +
        "case when LEN( " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end)=0 then status else  " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end end " +
        "status," +
        "remarks,case when inoutduringhalfdayleave=0 then 'AM-PM'when inoutduringhalfdayleave=1 then 'AM' else 'PM' end designation from TLeaveApplicationLine where l_id=" + id.Value + "";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_pay.DataSource = ds;
        grid_pay.DataBind();

        query = "select LEFT(CONVERT(varchar,date,101),10)sysdate,case when a.emp_id=0 then 'HR Admin' else  (select lastname+', '+firstname from memployee where id=a.emp_id) end approver,a.remarks,SUBSTRING(a.status,0, CHARINDEX('-',status)) status from sys_applog a where app_id=" + id.Value + " and a.type='L' order by a.id asc";
        DataTable dt = dbhelper.getdata(query);
        gridhistory.DataSource = dt;
        gridhistory.DataBind();
    }

    protected void rowboundgrid_pay(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_can = (LinkButton)e.Row.FindControl("lnk_can");
            string stat = e.Row.Cells[4].Text;
            if (stat == "Approved" || stat == "Cancelled")
                lnk_can.Visible = false;

            if (e.Row.Cells[2].Text == "No")
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
            else
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
        }
    }

    protected void close(object sender, EventArgs e)
    {
        modaltrans.Style.Add("display", "none");
    }
}