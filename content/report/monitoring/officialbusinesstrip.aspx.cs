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

public partial class content_report_monitoring_officialbusinesstrip : System.Web.UI.Page
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
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup" +
                " b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
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

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal.Style.Add("display", "block");
            string query = "select c.LastName+', '+c.FirstName+' '+c.MiddleName Employee,d.Department," +
                "left(CONVERT(varchar,a.date,101),10)Date,e.Leave,a.NumberOfHours NoDAYS,a.WithPay,a.Remarks from TLeaveApplicationLine a" +
                " left join Tleave b on a.LeaveId=b.id left join memployee c on a.EmployeeId=c.Id left join MDepartment d on " +
                "c.DepartmentId=d.Id left join MLeave e on a.LeaveId=e.Id where a.status like'%Approved%' and " +
                "a.EmployeeId='" + row.Cells[0].Text + "' and a.LeaveId='" + row.Cells[1].Text + "' ";
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
            string sql = "select a.Id,a.emp_id,convert(varchar,a.date_input,101)sysdate,convert(varchar,a.travel_start,101)tstart," +
                "convert(varchar,a.travel_end,101)tend,a.purpose,a.status,b.LastName+', '+b.FirstName employee from Ttravel a left join " +
                "MEmployee b on a.emp_id=b.Id left join MPayrollGroup c on c.Id=b.PayrollGroupId where b.LastName+', '+b.FirstName" +
                " like'%" + txtemp.Text + "%' order by a.date_input desc";
            DataTable leaves = dbhelper.getdata(sql);
            gridtrans.DataSource = leaves;
            gridtrans.DataBind();
        }
        else
        {
            string sql = "select a.Id,a.emp_id,convert(varchar,a.date_input,101)sysdate,convert(varchar,a.travel_start,101)tstart," +
                "convert(varchar,a.travel_end,101)tend,a.purpose,a.status,b.LastName+', '+b.FirstName employee from Ttravel a left" +
                " join MEmployee b on a.emp_id=b.Id left join MPayrollGroup c on c.Id=b.PayrollGroupId where b.LastName+', '+b.FirstName" +
                " like'%" + txtemp.Text + "%' and a.travel_start between'" + txtfrom.Text + "' and '" + txtto.Text + "' and a.travel_end" +
                " between'" + txtfrom.Text + "' and '" + txtto.Text + "' order by a.date_input desc";
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

           
            string query = "select a.emp_id,a.under_id,a.herarchy, b.FirstName +' '+b.LastName approver," +
                "(select status from sys_applog where app_id = " + row.Cells[0].Text + " and type='OBT' and emp_id=152)status," +
                "(select date from sys_applog where app_id = " + row.Cells[0].Text + " and type='OBT' and emp_id=152)date from Approver a " +
                "left join MEmployee b on a.under_id=b.Id where a.emp_id=" + lbl_bals.Value + " and a.under_id<>" + lbl_bals.Value + "";

            nadarang();
            modaltrans.Style.Add("display", "block");
        }
    }

    protected void nadarang()
    {
        string query = "select a.id,d.meals,d.transportation,d.accommodation,d.otherexpense,d.totalcashapproved,c.LastName+', '+c.FirstName " +
            "employee,convert(varchar,a.travel_start,101)tstart,convert(varchar,a.travel_end,101)tend from Ttravel a left join TtravelLine" +
            " b on a.id=b.travel_id left join MEmployee c on c.Id=a.emp_id left join Ttravel_budget d on a.id=d.travel_id where " +
            "a.id=" + id.Value + "";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        gridallowance.DataSource = ds;
        gridallowance.DataBind();
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = gridtrans.Rows.Count.ToString();
        string filename = "OBT_Monitoring";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

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
}