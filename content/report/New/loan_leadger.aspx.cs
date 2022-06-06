using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;

public partial class content_report_New_loan_leadger : System.Web.UI.Page
{
    public static string query;
    public static string gridname = "";
    public static DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            loadable();
    }
    protected void loadable()
    {
        string query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee order by LastName asc";
        DataTable dt = dbhelper.getdata(query);

        ddl_employee.Items.Clear();
        ddl_employee.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
        //get Other Deduction
        query = "select * from MOtherDeduction where status <> '4' and action is null";
        dt = dbhelper.getdata(query);
        ddl_loantype.Items.Clear();
        ddl_loantype.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_loantype.Items.Add(new ListItem(dr["OtherDeduction"].ToString(), dr["Id"].ToString()));
        }

        transition();
    }

    protected void transition()
    {
        grid_view.Visible = false;
        gridsss.Visible = false;
        gridhdmf.Visible = false;
    }

    protected void choice()
    {
        query = "select a.Id,a.EmployeeId,b.IdNumber,b.TIN,b.HDMFNumber,b.HDMFAddOn,b.SSSAddOn,b.SSSNumber,b.PHICNumber,"
            + "c.OtherDeduction,a.loandate,a.OtherDeductionId,a.LoanAmount,a.LoanNumber,a.DateStart,a.Remarks,"
            + "b.FirstName,b.LastName,b.MiddleName,d.balance,d.Amount,(select convert(varchar,b.DateOfBirth,111))Birthdate,"
            + "f.HDMFContribution,b.FirstName+', '+b.LastName as Employee "
            + "from Memployeeloan a "
            + "left join MEmployee b on a.EmployeeId = b.Id "
            + "left join MOtherDeduction c on c.Id = a.OtherDeductionId "
            + "left join TPayrollOtherDeductionLine d on d.loan_id = a.Id "
            + "left join TPayrollOtherDeduction e on d.PayOtherDeduction_id = e.id "
            + "left join TPayrollLine f on e.payroll_id = f.PayrollId where ";
        if (ddl_employee.SelectedValue != "0")
            query += " a.EmployeeId = '" + ddl_employee.SelectedValue + "' "
                +" and f.EmployeeId = '" + ddl_employee.SelectedValue + "' and";
        if (ddl_loantype.SelectedValue != "0")
            query += " a.OtherDeductionId = '" + ddl_loantype.SelectedValue + "' and a.status not like '%cancel%' and e.payroll_id is not null order by d.Id,b.LastName,b.FirstName,b.MiddleName desc";
        else
            query += " a.status not like '%cancel%' and e.payroll_id is not null order by d.Id,b.LastName,b.FirstName,b.MiddleName desc";

        DataTable dt = dbhelper.getdata(query);

        switch (ddl_loantype.SelectedValue)
        {
            case "0": //Car Loan
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
                //1
            case "1": //Car Loan
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
                //2
            case "2": //Cash Advance
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "7": //Coop
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "9": //Insurance
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "11": //Emergency Loan
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "12": //Housing Loan
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "13": //Bereavement
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "17": //Cash Advance2
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "18": //Insurance 2
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;
            case "19": //Affiliates
                grid_view.DataSource = dt;
                grid_view.DataBind();
                grid_view.Visible = true;
                gridname = "grid_view";
                break;

                //3
            case "3": //SSS LOAN
                gridsss.DataSource = dt;
                gridsss.DataBind();
                gridsss.Visible = true;
                gridname = "gridsss";
                break;
            case "16": //SSS Emergency Loan
                gridsss.DataSource = dt;
                gridsss.DataBind();
                gridsss.Visible = true;
                gridname = "gridsss";
                break;

                //4
            case "4": //HDMF LOAN
                gridhdmf.DataSource = dt;
                gridhdmf.DataBind();
                gridhdmf.Visible = true;
                gridname = "gridhdmf";
                break;
            case "5": //HDMF2
                gridhdmf.DataSource = dt;
                gridhdmf.DataBind();
                gridhdmf.Visible = true;
                gridname = "gridhdmf";
                break;
            case "14": //HDMF CLoan
                gridhdmf.DataSource = dt;
                gridhdmf.DataBind();
                gridhdmf.Visible = true;
                gridname = "gridhdmf";
                break;
        }
    }
    protected void selectchange(object sender, EventArgs e)
    {
        transition();
        choice();
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            // Response.Redirect("print_leadger", false);

            hf_id.Value = row.Cells[0].Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='loan_leadger';window.open('','_new').location.href='print_leadger?key=" + function.Encrypt(hf_id.Value, true) + "'", true);

        }
    }
    protected void generatereport(object sender, EventArgs e)
    {
        if (gridname == "gridhdmf")
        {
            if (gridhdmf.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Loan_Ledger.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages

                    gridhdmf.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in gridhdmf.HeaderRow.Cells)
                    {
                        cell.BackColor = gridhdmf.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in gridhdmf.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gridhdmf.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gridhdmf.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }
                    gridhdmf.RenderControl(hw);

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

        if (gridname == "gridsss")
        {
            if (gridsss.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Loan_Ledger.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages

                    gridsss.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in gridsss.HeaderRow.Cells)
                    {
                        cell.BackColor = gridsss.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in gridsss.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gridsss.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gridsss.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }
                    gridsss.RenderControl(hw);

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

        if (gridname == "grid_view")
        {
            if (grid_view.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Loan_Ledger.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages

                    grid_view.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in grid_view.HeaderRow.Cells)
                    {
                        cell.BackColor = grid_view.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in grid_view.Rows)
                    {
                        row.BackColor = Color.White;
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
            Response.Write("<script>alert('Empty File!')</script>");
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}