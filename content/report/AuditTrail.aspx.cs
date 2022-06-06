using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Services;


public partial class content_report_AuditTrail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();
            graphicaldisp();
        }
    }
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname+' '+a.MiddleName fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //retCategory.Add(string.Format("{0}-{1}", "0", "All"));
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }
    protected void search()
    {
        DataTable dt = dbhelper.getdata("select Id,case when(select name from nobel_user where Id=UserId)is NULL then (select name from nobel_user where emp_id = UserId) else (select name from nobel_user where Id = UserId) end UserId,case when EmpId='0'then'System Setup'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id = EmpId)end FullName,Transact,[Subject],Particular,[Action],AlterF,AlterT,Remarks,LEFT(convert(varchar,Sysdate,101),10)Sysdate from audittrail where EmpId = '" + lbl_bals.Value + "' and AlterF is not NULL order by Sysdate desc");
        alert.Visible = dt.Rows.Count == 0 ? true : false;
        gridaudit.DataSource = dt;
        gridaudit.DataBind();
    }
    protected void namesearch(object sender, EventArgs e)
    {
        search();
        btnallname.Visible = true;
    }
    protected void namesearchall(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select Id,case when(select name from nobel_user where Id=UserId)is NULL then (select name from nobel_user where emp_id = UserId) else (select name from nobel_user where Id = UserId) end UserId,case when EmpId='0'then'System Setup'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id = EmpId)end FullName,Transact,[Subject],Particular,[Action],AlterF,AlterT,Remarks,LEFT(convert(varchar,Sysdate,101),10)Sysdate from audittrail where EmpId = '" + lbl_bals.Value + "' and AlterF is not NULL order by Sysdate desc");
        gridallview.DataSource = dt;
        gridallview.DataBind();
        gridaudit.Visible = false;
        gridallview.Visible = true;

        LinkButton1.Visible = false;
        LinkButton2.Visible = true;
    }
    protected void viewall(object sender, EventArgs e)
    {
        if (ddl_range.SelectedValue == "1")
        {
            gridallview.DataSource = ViewState["daily"] as DataTable;
            gridallview.DataBind();
            gridaudit.Visible = false;
            gridallview.Visible = true;
        }
        else if (ddl_range.SelectedValue == "2")
        {
            gridallview.DataSource = ViewState["monthly"] as DataTable;
            gridallview.DataBind();
            gridaudit.Visible = false;
            gridallview.Visible = true;
        }
        else if (ddl_range.SelectedValue == "3")
        {
            gridallview.DataSource = ViewState["yearly"] as DataTable;
            gridallview.DataBind();
            gridaudit.Visible = false;
            gridallview.Visible = true;
        }
        LinkButton1.Visible = false;
        LinkButton2.Visible = true;
    }

    protected void clckserch(object sender, EventArgs e)
    {
        if (ddl_range.SelectedValue == "1")
        {
            DataTable dt = dbhelper.getdata("select a.Id,case when (select name from nobel_user where Id = a.UserId) is NULL then (select name from nobel_user where emp_id = a.UserId) else (select name from nobel_user where Id = a.UserId) end UserId,case when a.EmpId='0'then'System Configuration'when(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)is NULL then 'Not Registered'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)end FullName,a.Transact,a.Subject,a.Particular,a.Action,a.AlterF,a.AlterT,a.Remarks,LEFT(convert(varchar,a.Sysdate,101),10)Sysdate from audittrail a where a.AlterF is not NULL and CAST([Sysdate] as DATE) = '" + txtbydate.Text + "' order by [Sysdate] desc");
            gridaudit.DataSource = dt;
            gridaudit.DataBind();
            gridaudit.Visible = true;
            ViewState["daily"] = dt;
        }
        else if (ddl_range.SelectedValue == "2")
        {
            DataTable dt = dbhelper.getdata("select a.Id,case when (select name from nobel_user where Id = a.UserId) is NULL then (select name from nobel_user where emp_id = a.UserId) else (select name from nobel_user where Id = a.UserId) end UserId,case when a.EmpId='0'then'System Configuration'when(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)is NULL then 'Not Registered'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)end FullName,a.Transact,a.Subject,a.Particular,a.Action,a.AlterF,a.AlterT,a.Remarks,LEFT(convert(varchar,a.Sysdate,101),10)Sysdate from audittrail a where a.AlterF is not NULL and MONTH([Sysdate]) = '" + ddlmonth.SelectedValue.ToString() + "' order by [Sysdate] desc");
            gridaudit.DataSource = dt;
            ViewState["monthly"] = dt;
            gridaudit.DataBind();
            gridaudit.Visible = true;
        }
        else if (ddl_range.SelectedValue == "3")
        {
            DataTable dt = dbhelper.getdata("select a.Id,case when (select name from nobel_user where Id = a.UserId) is NULL then (select name from nobel_user where emp_id = a.UserId) else (select name from nobel_user where Id = a.UserId) end UserId,case when a.EmpId='0'then'System Configuration'when(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)is NULL then 'Not Registered'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)end FullName,a.Transact,a.Subject,a.Particular,a.Action,a.AlterF,a.AlterT,a.Remarks,LEFT(convert(varchar,a.Sysdate,101),10)Sysdate from audittrail a where a.AlterF is not NULL and YEAR([Sysdate]) = '" + ddlyeaer.SelectedValue.ToString() + "' order by [Sysdate] desc");
            gridaudit.DataSource = dt;
            ViewState["yearly"] = dt;
            gridaudit.DataBind();
            gridaudit.Visible = true;
        }
        btnviewall.Visible = true;
        gridallview.Visible = false;
        LinkButton1.Visible = true;
        LinkButton2.Visible = false;
    }

    protected void changeonpage(object sender, GridViewPageEventArgs e)
    {
        gridaudit.PageIndex = e.NewPageIndex;
        disp();
    }
    protected void graphicaldisp()
    {
        DataTable dt = dbhelper.getdata("select a.Id,(select FullName from MUser where Id = a.UserId)UserId,case when a.EmpId = '0' then 'System Configuration'else(select LastName+', '+FirstName from MEmployee where Id = a.EmpId)end FullName, a.Transact, a.Subject, a.Particular, a.Action, a.AlterF, a.AlterT, a.Remarks, LEFT(convert(varchar, a.Sysdate,101),10)Sysdate from audittrail a where a.AlterF is not NULL order by a.Sysdate desc");
        gridaudit.DataSource = dt;
        gridaudit.DataBind();

        dt = dbhelper.getdata("select [Subject], COUNT(*) as CNT from audittrail Group BY [Subject]");
        foreach (DataRow row in dt.Rows)
        {
            lbtrailcnt.Text += "{ y: '" + row["Subject"].ToString() + "', a: " + row["CNT"].ToString() + " },";
        }
        dt = dbhelper.getdata("select(select COUNT(*)from audittrail where [Action]='Update') [Update],(select COUNT(*)from audittrail where [Action]='Add') [Add], (select COUNT(*)from audittrail where [Action]='Cancel') [Cancel], (select COUNT(*)from audittrail where [Action]='Download') [Download], (select COUNT(*)from audittrail where [Action]='Upload') [Upload]");
        lbtrailcnt2.Text = "{value: " + dt.Rows[0]["Update"].ToString() + ", label: 'Update'}, {value: " + dt.Rows[0]["Add"].ToString() + ", label: 'Add'}, {value: " + dt.Rows[0]["Cancel"].ToString() + ", label: 'Cancel'},{value: " + dt.Rows[0]["Download"].ToString() + ", label: 'Download'},{value: " + dt.Rows[0]["Upload"].ToString() + ", label: 'Upload'}";
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.Id,case when (select name from nobel_user where Id = a.UserId) is NULL then (select name from nobel_user where emp_id = a.UserId) else (select name from nobel_user where Id = a.UserId) end UserId,case when a.EmpId = '0' then 'System Configuration'when(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id=a.EmpId)is NULL then 'Not Registered'else(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id = a.EmpId)end FullName, a.Transact, a.Subject, a.Particular, a.Action, a.AlterF, a.AlterT, a.Remarks, LEFT(convert(varchar, a.Sysdate,101),10)Sysdate from audittrail a where a.AlterF is not NULL order by a.Sysdate desc");
        gridaudit.DataSource = dt;
        gridaudit.DataBind();

        DataTable dtdate = dbhelper.getdata("SELECT YEAR(GETDATE())todate");
        ddlyeaer.Text = dtdate.Rows[0]["todate"].ToString();

        //Insert Current Year
        DataTable dtyearnow = dbhelper.getdata("select * from MYear where [Year] like " + dtdate.Rows[0]["todate"] + "");
        if (dtyearnow.Rows.Count == 0)
        {
            dbhelper.getdata("insert into MYear values ('" + dtdate.Rows[0]["todate"] + "')");
        }

        DataTable dtyear = dbhelper.getdata("select * from MYear order by [Year] desc");
        ddlyeaer.Items.Clear();
        foreach (DataRow dr in dtyear.Rows)
        {
            ddlyeaer.Items.Add(new ListItem(dr["Year"].ToString(), dr["Year"].ToString()));
        }

        DataTable dtmonth = dbhelper.getdata("select * from MMonth order by [Id] asc");
        ddlmonth.Items.Clear();
        foreach (DataRow dr in dtmonth.Rows)
        {
            ddlmonth.Items.Add(new ListItem(dr["Month"].ToString(), dr["id"].ToString()));
        }
    }
    protected void filterby(object sender, EventArgs e)
    {
        if (ddl_range.SelectedValue == "1")
        {
            txtbydate.Visible = true;
            ddlmonth.Visible = false;
            ddlyeaer.Visible = false;
            btnsearch.Visible = true;
        }
        else if (ddl_range.SelectedValue == "2")
        {
            ddlmonth.Visible = true;
            txtbydate.Visible = false;
            ddlyeaer.Visible = false;
            btnsearch.Visible = true;
        }
        else if (ddl_range.SelectedValue == "3")
        {
            ddlyeaer.Visible = true;
            txtbydate.Visible = false;
            ddlmonth.Visible = false;
            btnsearch.Visible = true;
        }
        btnviewall.Visible = false;
    }
    protected void generatereportall(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=AuditTrailReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages

            gridallview.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in gridallview.HeaderRow.Cells)
            {
                cell.BackColor = gridallview.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gridallview.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gridallview.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gridallview.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }
            gridallview.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    protected void generatereport(object sender, EventArgs e)
    {
        if (gridaudit.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=AuditTrailReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                gridaudit.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gridaudit.HeaderRow.Cells)
                {
                    cell.BackColor = gridaudit.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gridaudit.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gridaudit.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gridaudit.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                gridaudit.RenderControl(hw);

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
        gridaudit.PageIndex = e.NewPageIndex;
    }
}