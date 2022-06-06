using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_report_New_PHICreportnew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
            LoadData();
        }
    }
    protected void LoadData()
    {
        DataTable dt = getdata.ReportFields(4);
        ViewState["fields"] = dt;
    }
    protected void BindData()
    {
        string query = "select case when PhilHealth_ID is null then 'Grand Total' else PhilHealth_ID end PhilHealth_ID,"
            +"Employee_ID,Department,"
            +"case when(select UPPER(LEFT(LastName,1))+LOWER(RIGHT(LastName,len(LastName)-1)))is null then 'ZZZZZ' else (select UPPER(LEFT(LastName,1))+LOWER(RIGHT(LastName,len(LastName)-1)))end LastName,"
            +"(select UPPER(LEFT(FirstName,1))+LOWER(RIGHT(FirstName,len(FirstName)-1)))FirstName,"
            +"case when MiddleName = '' then '' else ((select UPPER(LEFT(MiddleName,1))+LOWER(RIGHT(MiddleName,len(MiddleName)-1)))) end MiddleName,"
            +"sum(ee_share)Employee_Contribution,sum(er_share)Employer_Contribution,TIN,Birth_Date,sum(ee_share + er_share)Total "
            +"from(select b.ID,a.IdNumber Employee_ID,a.LastName,a.FirstName,a.MiddleName,(select top 1 department from mdepartment where id=a.departmentid)Department,a.phicnumber PhilHealth_ID,c.payrollnumber,b.netincome,b.phiccontribution ee_share,b.phiccontributionemployer er_share,a.TIN,(select convert(varchar,a.DateOfBirth,111))Birth_Date "
            +"from memployee a "
            +"left join tpayrollline b on a.id = b.employeeid "
            +"left join tpayroll c on b.payrollid = c.id "
            +"left join tdtr d on c.dtrid = d.id "
            + "where month(d.datestart) = " + ddl_month.SelectedValue + " and year(d.datestart) = " + ddl_year.SelectedItem.Text + " "
            +"and c.status is null)phic_con "
            +"group by GROUPING sets((PhilHealth_ID,Employee_ID,Department,LastName,FirstName,MiddleName,TIN,Birth_Date),()) "
            +"order by LastName asc";
        
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;

        DataTable dt = dbhelper.getdata(query);
        ViewState["data"] = dt;
        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_view.PageIndex = e.NewPageIndex;
        this.BindData();
        //grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        BindData();
    }
    protected void lodable()
    {
        ddl_month.SelectedValue = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
        ddl_year.Items.Clear();
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("Select Year", "0"));
        for (int i = 2017; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
        }
        ddl_year.SelectedValue = DateTime.Now.Year.ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "PHIC_Contribution_" + ddl_month.SelectedItem.Text + "_" + ddl_year.SelectedItem.Text;
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