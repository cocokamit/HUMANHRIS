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

public partial class content_report_monitoring_otmeal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            execute();
            if (grid_view.Rows.Count > 0)
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
        execute();
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;
    }
    protected void execute()
    {
        string query = "select a.Id,a.EmployeeId,convert(varchar,a.Date,101)Date,convert(varchar,a.sysdate,101)sysdate," +
            "convert(varchar,a.OvertimeHoursIn,101)OTIn,convert(varchar,a.OvertimeHoursOut,101)OTOut,a.OvertimeHours othrs,a.Remarks," +
            "a.status,b.LastName+', '+b.FirstName employee, c.PayrollGroup from TMealHours a left join MEmployee b" +
            " on a.EmployeeId=b.Id left join MPayrollGroup c on c.Id=b.PayrollGroupId ";
        if (txtemp.Text != "")
            query += "where b.LastName+', '+b.FirstName like '%" + txtemp.Text + "%' ";
        if (txtfrom.Text != "" && txtto.Text != "")
            query += "and Date between '" + txtfrom.Text + "' and '" + txtto.Text + "' order by a.Date desc";
        else
            query += "order by a.Date desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "OTMeal_Monitoring";
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

            grid_view.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in grid_view.HeaderRow.Cells)
            {
                cell.BackColor = grid_view.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grid_view.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grid_view.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grid_view.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grid_view.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}