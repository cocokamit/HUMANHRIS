using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;
using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_report_WithoutApprovers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            if (grid_view.Rows.Count > 0)
                exp_report.Visible = true;
            else
                exp_report.Visible = false;
        }
    }

    [WebMethod]
    public static string[] GetEmployees(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.action is null and a.firstname+' '+a.lastname like '%{0}%'", term);
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

    protected void btn_search(object sender, EventArgs e)
    {
        string query = "select a.Id,a.LastName+', '+a.FirstName+' '+a.MiddleName+'.'FullName,(select Department from MDepartment "
            + "where Id=a.DepartmentId)Department,(select Position from MPosition where Id=a.PositionId)Position,"
            + "(Select PayrollGroup from MPayrollGroup where Id=a.PayrollGroupId)PayrollGroup, b.emp_id,b.under_id from MEmployee a "
            + "left join Approver b on b.emp_id=a.Id where (select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.Id) like '%" + ddl_emp.Text + "%'"
            +"and b.emp_id is NULL order by FullName asc";

        DataTable dt = dbhelper.getdata(query);

        grid_view.DataSource = dt;
        grid_view.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "No Approvers List" + DateTime.Now.ToString();
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void addreportrt(object sender, EventArgs e)
    {
        //BUTYOK APPROVER
        LinkButton lnkUp = (grid_view.Rows[0].FindControl("lnk_view") as LinkButton);
        DataTable dtrt = new DataTable();
        if (txt_reportto.Text.Length == 0)
        { }
        else
        {
            if (Session["emp_id"].ToString().Length > 0 || lbl_bals.Value.Length > 0)
            {
                DataTable hercnt = dbhelper.getdata("select * from Approver a where a.emp_id ='" + lnkUp.CommandName + "' and a.status is null");
                int x = (hercnt.Rows.Count) + 1;
                dbhelper.getdata("insert into Approver (date,emp_id,userid,under_id,herarchy)values(getdate(),'" + lnkUp.CommandName + "'," + Session["emp_id"].ToString() + ",'" + lbl_bals.Value + "'," + x + ")");
            }
            else
            { }
        }
        ViewState["approver"] = 1;
        Response.Redirect("approverscheck");
    }
}