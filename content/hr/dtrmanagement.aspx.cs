using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.IO;

public partial class content_hr_dtrmanagement : System.Web.UI.Page
{
    void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["role"].ToString() != "Admin")
            this.MasterPageFile = "~/content/site.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
        }
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id,c.name from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id left join nobel_user c on c.emp_id = a.Id where c.name like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["name"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void lodable()
    {
        string query = "select * from MPayrollGroup where status = '1' order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }

    protected void search(object sender, EventArgs e)
    {
        DataTable dt;
        string query = "select b.IdNumber,b.FullName,(select Department from MDepartment where Id = b.DepartmentId)Department,a.Biotime "
            + "from tdtrperpayrolperline a "
            + "left join MEmployee b on a.empid = b.Id "
            + "left join MPayrollGroup c on c.Id = b.PayrollGroupId "
            + " where c.Id = " + ddl_pg.SelectedValue + " "
            + " and a.Biotime between convert(varchar,'" + txt_f.Text + "',101) and convert(varchar,'" + txt_t.Text + "',101) "
            + " and b.FullName like '%" + txt_emp.Text + "%'";
        dt = dbhelper.getdata(query);
        griddtr.DataSource = dt;
        griddtr.DataBind();
    }
    protected void generatereport(object sender, EventArgs e)
    {
        if (griddtr.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DTR.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                griddtr.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in griddtr.HeaderRow.Cells)
                {
                    cell.BackColor = griddtr.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in griddtr.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = griddtr.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = griddtr.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                griddtr.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        griddtr.PageIndex = e.NewPageIndex;
    }
}