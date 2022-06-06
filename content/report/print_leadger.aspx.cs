using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Cryptography;

public partial class content_report_print_leadger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    protected void bind()
    {
        hf_id.Value = Request.QueryString["key"].ToString();

        DataTable ew = dbhelper.getdata("select top 1 CONVERT(varchar,CAST(a.balance AS money), 1)balance " +
                                        "from TPayrollOtherDeductionLine a " +
                                        "left join TPayrollOtherDeduction b on a.PayOtherDeduction_id=b.id " +
                                        "left join TPayroll c on b.payroll_id=c.id " +
                                        "where a.loan_id='" + function.Decrypt(hf_id.Value, true) + "' and b.payroll_id is not null order by c.PayrollDate desc");
        if (ew.Rows.Count == 0)
            lbl_bal.Text = "0.00";
        else
            lbl_bal.Text = ew.Rows[0]["balance"].ToString();

        DataTable aw = dbhelper.getdata("select LoanAmount,convert(decimal,'0.00') as credit,LoanAmount as balance,DateStart from MEmployeeLoan where id='" + function.Decrypt(hf_id.Value, true) + "'");

        string query = "select convert(decimal,'0.00') as LoanAmount ,a.Amount as credit,a.balance, " +
                       "c.PayrollDate as DateStart, " +
                       "b.payroll_id, " +
                       "e.LastName+','+e.FirstName as fullname,d.LoanNumber,e.Idnumber,f.Otherdeduction " +
                       "from TPayrollOtherDeductionLine a " +
                       "left join TPayrollOtherDeduction b on a.PayOtherDeduction_id=b.id " +
                       "left join TPayroll c on b.payroll_id=c.id " +
                       "left join MEmployeeLoan d on d.Id=a.loan_id " +
                       "left join memployee e on e.Id=d.EmployeeId " +
                       "left join MOtherDeduction f on f.Id=d.OtherDeductionId " +
                       "where a.loan_id='" + function.Decrypt(hf_id.Value,true) + "' and b.payroll_id is not null order by c.PayrollDate";
        DataTable dt = dbhelper.getdata(query);
        aw.Merge(dt);

        grid_released.DataSource = aw;
        grid_released.DataBind();

        DataTable te = dbhelper.getdata("select a.id,a.LoanNumber, " +
                                "b.OtherDeduction, " +
                                "c.LastName+','+c.FirstName as fullname,c.Idnumber " +
                                "from MEmployeeLoan a " +
                                "left join MOtherDeduction b on b.Id=a.OtherDeductionId " +
                                "left join memployee c on c.Id=a.EmployeeId " +
                                "where a.id='" + function.Decrypt(hf_id.Value, true) + "' ");

        lbl_no.Text = te.Rows[0]["LoanNumber"].ToString();
        lbl_type.Text = te.Rows[0]["Otherdeduction"].ToString();
        lbl_id.Text = te.Rows[0]["Idnumber"].ToString();
        lbl_name.Text = te.Rows[0]["fullname"].ToString();

        if (grid_released.Rows.Count == 0)
            lbl_err.Text = "NO Data Found";
        else
            lbl_err.Text = "";
    }
}