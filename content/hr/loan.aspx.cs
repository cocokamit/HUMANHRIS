using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Drawing;

public partial class content_hr_loan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
            disp();
        }
    }
    protected void lodable()
    {
        var Pg_List = from pglist in datacontext.Context.GetTable<MPayrollGroup>() where pglist.status == 1 orderby pglist.PayrollGroup ascending select new { PayrollGroup = pglist.PayrollGroup, id = pglist.Id };
        ddl_payroll_group.Items.Clear();
        ddl_payroll_group.Items.Add(new ListItem("None", "0"));
        foreach (var lista in Pg_List)
        {
            ddl_payroll_group.Items.Add(new ListItem(lista.PayrollGroup.ToString(), lista.id.ToString()));
        }

        var deduction = from utang in datacontext.Context.GetTable<MOtherDeduction>() where utang.action == null orderby utang.OtherDeduction ascending select new { OtherDeduction = utang.OtherDeduction, id = utang.Id };
        dll_deduction.Items.Clear();
        dll_deduction.Items.Add(new ListItem("None", "0"));
        foreach (var lista in deduction)
        {
            dll_deduction.Items.Add(new ListItem(lista.OtherDeduction.ToString(), lista.id.ToString()));
        }
        var Period_setup = from period in datacontext.Context.GetTable<sysLoanPeriodSetup>() orderby period.id ascending select new { datestart = period.datestart, dateend = period.dateend };
        rbl_schedule.Items.Clear();
        foreach (var lista in Period_setup)
        {
            rbl_schedule.Items.Add(new ListItem(lista.datestart + "-" + lista.dateend + " payroll", lista.datestart + "-" + lista.dateend + " payroll"));
        }
    }
    protected void disp()
    {
        string query = BaseQuery() + " order by c.OtherDeduction,b.LastName,b.FirstName asc,a.Id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }


    protected string BaseQuery()
    {
        return "select b.Id as emp_id,case when a.IsPaid = 'True' then 'Yes' else 'No' end IsPaid,b.PayrollGroupId,b.IdNumber,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, " +
                 "a.id,left(convert(varchar,a.loandate,101),10)loandate,left(convert(varchar,a.DateStart,101),10)DateStart,left(convert(varchar,a.Dateend,101),10)Dateend,a.status,a.interest, " +
                 "c.OtherDeduction,a.LoanAmount,cast(round(a.MonthlyAmortization,2)as numeric(12,2))MonthlyAmortization,a.schedule, " +
                 "cast(round((a.Balance -(select case when SUM(z.Amount) is null then '0' else SUM(z.Amount) end amt from TPayrollOtherDeductionLine z " +
                 "left join TPayrollOtherDeduction y on z.PayOtherDeduction_id=y.id " +
                 "where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null)),2)as numeric(12,2)) balance " +
                 "from MEmployeeLoan a " +
                 "left join MEmployee b on a.EmployeeId=b.id  " +
                 "left join MOtherDeduction c on a.OtherDeductionId=c.Id  " +
                 "where a.status like '%Approved%' and " +
                 "(round(a.Balance -(select case when SUM(z.Amount) is null then '0' else SUM(z.Amount) end amt from TPayrollOtherDeductionLine z " +
                 "left join TPayrollOtherDeduction y on z.PayOtherDeduction_id=y.id " +
                 "where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null),2) > 0)";
    }

    protected void click_add_employee(object sender, EventArgs e)
    {
        Response.Redirect("deduction-applicaiton", false);
    }
    protected void click_approve(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("apl?ap=" + function.Encrypt(row.Cells[0].Text, true) + "", false);
        }
    }
    protected void click_edit_employee(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("editloan?app_id=" + row.Cells[0].Text + "", false);
        }
    }
    protected void click_delete_employee(object sender, EventArgs e)
    {
        if (txt_remarks.Text.Length > 0)
        {
            string query = "update MEmployeeLoan set status ='" + "Cancel-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "' where id= " + hdn_trnid.Value + "";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='deduction'", true);
        }
    }
    protected void click_paid_loan(object sender, EventArgs e)
    {
        if (txt_remarks.Text.Length > 0)
        {
            string query = "update MEmployeeLoan set IsPaid = 'True' where Id = " + hdn_trnid.Value + "";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='deduction'", true);
        }
    }
    protected void search(object sender, EventArgs e)
    {
        string query = BaseQuery() + " and b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname like'%" + txt_search.Text + "%' ";

        DataTable dt = dbhelper.getdata(query + " order by a.Id desc");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_pg(object sender, EventArgs e)
    {
        dll_deduction.Enabled = true;
    }
    protected void txt_ew(object sender, EventArgs e)
    {
        if (int.Parse(dll_deduction.SelectedValue) > 0)
        {
            string query = "select a.Balance - case when ( select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) IS null then 0 else (select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) end as balansing " +
                    "from MEmployeeLoan a where a.EmployeeId=" + lbl_bals.Value + " and a.OtherDeductionId='" + dll_deduction.SelectedValue + "' and a.status like'%Approved%' ";
            DataTable dtt = dbhelper.getdata(query);
            if (dtt.Rows.Count != 0)
            {
                if (double.Parse(dtt.Rows[0]["balansing"].ToString()) <= 0)
                {
                    query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
                    DataTable dt = dbhelper.getdata(query);
                    double aw = 0;
                    aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
                    lbl_interest.Text = aw.ToString();
                    double total = 0;
                    total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
                    txt_amortization.Text = total.ToString();
                    txt_amount.Enabled = true;
                    lbl_la.Text = "";
                }
                else
                {
                    txt_amount.Enabled = false;
                    txt_amortization.Enabled = false;
                    txt_no.Enabled = false;
                    txt_memo.Text = "";
                    txt_amortization.Text = "";
                    txt_amount.Text = "";
                    lbl_interest.Text = "";
                    txt_no.Text = "";
                    lbl_la.Text = "Pending Loan";
                }

            }
            else
            {
                query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
                DataTable dt = dbhelper.getdata(query);
                double aw = 0;
                aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
                lbl_interest.Text = aw.ToString();

                double total = 0;
                total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
                txt_amortization.Text = total.ToString();

                lbl_la.Text = "";
                txt_amount.Enabled = true;
            }
        }
        else
        {
            txt_amount.Enabled = false;
        }

    }
    protected void txt_aw(object sender, EventArgs e)
    {
        txt_no.Enabled = true;
    }
    protected void txt_no_TextChanged(object sender, EventArgs e)
    {
        string query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        double aw = 0;
        aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * (double.Parse(dt.Rows[0]["interest"].ToString()));
        lbl_interest.Text = aw.ToString();
        double total = 0;
        total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        txt_amortization.Text = total.ToString();
    }
    protected void click_select_scheduletype(object sender, EventArgs e)
    {
        if (ddl_schedule_type.SelectedValue == "Per Month")
            rbl_schedule.Visible = true;
        else
        {
            rbl_schedule.Visible = false;
            rbl_schedule.ClearSelection();
        }

    }
    protected void save_loans(string classification,string schedule)
    {
        schedule = rbl_schedule.Text.Length > 0 ? rbl_schedule.Text : "Per payroll";
        string sc=schedule.Length>0? schedule.Contains("26-10")?"26-10 payroll":schedule.Contains("11-25")?"11-25 payroll":schedule
                   :rbl_schedule.Text.Length > 0 ? rbl_schedule.Text : "Per payroll";
        string query = "select * from MEmployeeLoan where EmployeeId = " + lbl_bals.Value + " and OtherDeductionId = "+dll_deduction.SelectedValue+" and IsPaid = 'False' and status like '%Approved%'";
        DataTable dt = dbhelper.getdata(query);
        if (dt.Rows.Count > 0)
        {
            Response.Write("<script>alert('Record Already Exists!')</script>");
        }
        else
        {
            if (classification == "ManualInput")
            {
                if (chk())
                    goto x;
                else
                    goto y;
            }
            else
                goto x;
                x:
                
                MEmployeeLoan loan = new MEmployeeLoan();
                loan.EmployeeId = int.Parse(lbl_bals.Value);
                loan.OtherDeductionId = int.Parse(dll_deduction.SelectedValue);
                loan.LoanNumber = dll_no.Text;
                loan.LoanAmount = decimal.Parse(txt_amount.Text.Replace(",", ""));
                loan.MonthlyAmortization = decimal.Parse(txt_amortization.Text.Replace(",", ""));
                loan.NumberOfMonths = 0;
                if (txt_loandate.Text.Length > 0)
                    loan.DateStart = DateTime.Parse(txt_loandate.Text); 
               
                loan.TotalPayment = decimal.Parse("0.0000");
                loan.Balance = decimal.Parse(txt_amount.Text.Replace(",", ""));
                loan.IsPaid = "False";
                loan.EntryUserId = int.Parse(Session["user_id"].ToString());
                loan.EntryDateTime = DateTime.Now;
                loan.UpdateUserId = int.Parse(Session["user_id"].ToString());
                loan.UpdateDateTime = DateTime.Now;
                loan.status = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                loan.Remarks = txt_memo.Text;
                loan.interest = 0;
                loan.scheduletype = ddl_schedule_type.SelectedValue;
                loan.schedule = sc;
                if (txt_loanstart.Text.Length > 0)
                {
                    loan.loandate = DateTime.Parse(txt_loanstart.Text);
                }
                else loan.loandate = null;
                if (txt_loanend.Text.Length > 0)
                {
                    loan.dateend = DateTime.Parse(txt_loanend.Text);
                }
                datacontext.Context.MEmployeeLoans.InsertOnSubmit(loan);
                try
                {
                    datacontext.Context.SubmitChanges();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Mloan?pg=" + function.Encrypt(pg1.Value, true) + " '", true);
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Record inserted Failed!')</script>");
                }
            y:
                string exit = "Yes";
            
        }
    }
    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;
        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();
            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    private void getexcel()
    {
        if (dll_deduction.SelectedValue == "0")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Other Income Type');", true);
        }
        else
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload2.FileName));
            FileUpload2.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "] where len(trim(IDNumber))>0 ", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();
            ViewState["excel_data"] = DtSet.Tables[0];
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

        if (FileUpload2.HasFile)
        {
            getexcel();
            var gg = ViewState["excel_data"] as DataTable;
            try
            {
                foreach (DataRow datalist in gg.Rows)
                {
                    if (datalist["IDNUMBER"].ToString().Replace(" ", "").Length > 0)
                    {
                        string idno = datalist["IDNUMBER"].ToString().Replace(" ", "");
                        string loan_amt = datalist["Balance"].ToString().Replace(",", "");
                        string loan_amortization = datalist["Amortization"].ToString().Replace(",", "");

                        var emplist = from emp in datacontext.Context.GetTable<MEmployee>() where emp.IdNumber == idno select new { empid = emp.Id };

                        lbl_bals.Value = emplist.FirstOrDefault().empid.ToString();
                        dll_no.Text = datalist["Reference Number"].ToString();
                        txt_amortization.Text = loan_amortization;
                        txt_loanstart.Text = datalist["Loan Date"].ToString();
                        txt_amount.Text = loan_amt;

                        txt_loandate.Text = datalist["Start"].ToString();
                        txt_loanend.Text = datalist["End"].ToString();

                        DataTable dt = dbhelper.getdata("select b.Id as emp_id,b.PayrollGroupId,b.IdNumber,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, a.id,left(convert(varchar,a.loandate,101),10)loandate,left(convert(varchar,a.DateStart,101),10)DateStart,left(convert(varchar,a.Dateend,101),10)Dateend,a.status,a.interest, c.OtherDeduction,a.LoanAmount,cast(round(a.MonthlyAmortization,2)as numeric(12,2))MonthlyAmortization,a.schedule, cast(round((a.Balance -(select case when SUM(z.Amount) is null then '0' else SUM(z.Amount) end amt from TPayrollOtherDeductionLine z left join TPayrollOtherDeduction y on z.PayOtherDeduction_id=y.id where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null)),2)as numeric(12,2)) balance from MEmployeeLoan a left join MEmployee b on a.EmployeeId=b.id left join MOtherDeduction c on a.OtherDeductionId=c.Id where a.status like '%Approved%' and b.IdNumber = '" + idno + "' and c.Id = " + dll_deduction.SelectedValue + "");
                        foreach (DataRow dr in dt.Rows)
                            if (dr["balance"].ToString() == "0.00")
                            {
                                dbhelper.getdata("update MEmployeeLoan set IsPaid = 'True' where Id = " + dr["id"].ToString() + "");
                            }
                        save_loans("BatchUpload", datalist["Schedule"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Please check your Excel Format!')</script>");
            }
        }
        else
            save_loans("ManualInput", "");
    }
    protected bool chk()
    {
        bool err = true;
        if (int.Parse(ddl_payroll_group.SelectedValue) == 0)
        {
            lbl_pg.Text = "*";
            err = false;
        }
        else if (ddl_emp.Text.Length == 0 || lbl_bals.Value.Length==0)
        {
            lbl_emp.Text = "*";
            err = false;
        }
        else if (int.Parse(dll_deduction.SelectedValue) == 0)
        {
            lbl_loan.Text = "*";
            err = false;
        }
        else if (txt_amount.Text.Length == 0)
        {
            lbl_la.Text = "*";
            err = false;
        }
        
        else if (txt_amortization.Text.Length == 0)
        {
            lbl_amort.Text = "*";
            err = false;
        }
        else if (txt_memo.Text.Length == 0)
        {
            lbl_memo.Text = "*";
            err = false;
        }

        else if (txt_loandate.Text.Length == 0)
        {
            lbl_loandate.Text = "*";
            err = false;
        }
        else if (txt_loanstart.Text.Length == 0)
        {
            lbl_datestart.Text = "*";
            err = false;
        }
        else if (txt_loanend.Text.Length == 0)
        {
            lbl_dateend.Text = "*";
            err = false;
        }
        return err;
    }
    protected void delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            l_name.Text=row.Cells[4].Text+"("+row.Cells[5].Text+")";
            hdn_trnid.Value=row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
        }
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("deduction");
    }
    protected void generatereport(object sender, EventArgs e)
    {
        if (grid_view.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Loan_Report.xls");
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}