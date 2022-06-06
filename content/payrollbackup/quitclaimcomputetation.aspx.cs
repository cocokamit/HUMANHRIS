using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class content_payroll_quitclaimcomputetation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            master();

    }
    protected void master()
    {
        //MASTER DATA
        DataTable master = new DataTable();
        master.Columns.Add(new DataColumn("empid", typeof(string)));
        master.Columns.Add(new DataColumn("e_name", typeof(string)));
        master.Columns.Add(new DataColumn("unclaimsalary", typeof(string)));
        master.Columns.Add(new DataColumn("13monthpay", typeof(string)));
        master.Columns.Add(new DataColumn("saving", typeof(string)));
        master.Columns.Add(new DataColumn("taxrefund", typeof(string)));
   
        master.Columns.Add(new DataColumn("serviceincentiveleave", typeof(string)));
        master.Columns.Add(new DataColumn("separationpay", typeof(string)));
        master.Columns.Add(new DataColumn("retirementpay", typeof(string)));
        master.Columns.Add(new DataColumn("taxpayable", typeof(string)));
        master.Columns.Add(new DataColumn("deduction", typeof(string)));
        master.Columns.Add(new DataColumn("totalamt", typeof(string)));
        ViewState["master"] = master;

        DataTable dt_itr = getdata.dtitr(function.Decrypt(Request.QueryString["yyyy"].ToString(),true), function.Decrypt(Request.QueryString["empid"].ToString(),true));
        if(dt_itr.Rows.Count>0)
        {
            ViewState["itr"] = dt_itr;
            compute(function.Decrypt(Request.QueryString["empid"].ToString(), true), dt_itr.Rows[0]["taxdue"].ToString());
            conertabletocash();
        }
    }
    protected void proccess(object sender, EventArgs e)
    {
        string ret = "0";
        DataTable itr = (DataTable)ViewState["itr"];
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("process_quit_claim_details", con))
            {

                TextBox txt = (TextBox)grid_compute.Rows[0].Cells[7].FindControl("txt_sp");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlConnection conn = new SqlConnection(dbconnection.conn);
                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = function.Decrypt(Request.QueryString["empid"].ToString(), true);
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["user_id"].ToString();
                cmd.Parameters.Add("@lastpayroll_amt", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[2].Text;
                cmd.Parameters.Add("@thirteenmonthpay_amt", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[3].Text;
                cmd.Parameters.Add("@internalsavings_amt", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[4].Text;
                cmd.Parameters.Add("@tax_refund", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[5].Text;
                cmd.Parameters.Add("@retirement_amt", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[8].Text;
                cmd.Parameters.Add("@separation_amt", SqlDbType.VarChar, 5000).Value = txt.Text.Length > 0 ? txt.Text : "0.00";
                cmd.Parameters.Add("@sil", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[6].Text;
                cmd.Parameters.Add("@tax_payable", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[9].Text;
                cmd.Parameters.Add("@internal_deduction", SqlDbType.VarChar, 5000).Value = grid_compute.Rows[0].Cells[10].Text;
                cmd.Parameters.Add("@itr_id", SqlDbType.VarChar, 5000).Value = itr.Rows[0]["itr_id"].ToString();
                cmd.Parameters.Add("@reqid", SqlDbType.VarChar, 5000).Value = function.Decrypt(Request.QueryString["reqid"].ToString(), true);
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                ret = cmd.Parameters["@out"].Value.ToString();
                con.Close();
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", " window.open('','_new').location.href='printquitsummary?&user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&ret="+ret+"'", true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='printquitsummary?&user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "'&ret=" + ret + "", true);
        //quitclaim?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "
        //Response.Redirect("quitclaim?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "");
    }
    protected void compute(string empid,string taxdue)
    {
        DataTable master=(DataTable)ViewState["master"];
        DataRow mdr;
        string query = "select a.NetIncome from TPayrollLine a " +
                        "left join TPayroll b on a.PayrollId=b.id " +
                        "where a.empstatus='4' and b.status is null and a.employeeid='" + empid + "'";
        DataTable dt = dbhelper.getdata(query);
       

        decimal ni = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["NetIncome"].ToString()) : decimal.Parse("0.00");
        dt = getdata.thirteenmonthpay(empid, function.Decrypt(Request.QueryString["yyyy"].ToString(), true));
        decimal itr = 0;
        decimal itp = 0;
        if (decimal.Parse(taxdue) >= decimal.Parse(dt.Rows[0]["taxasdeducted"].ToString()))
            itp = decimal.Parse(taxdue) - decimal.Parse(dt.Rows[0]["taxasdeducted"].ToString());
        if ( decimal.Parse(dt.Rows[0]["taxasdeducted"].ToString())>=decimal.Parse(taxdue))
            itr= decimal.Parse(dt.Rows[0]["taxasdeducted"].ToString()) - decimal.Parse(taxdue);
       
        decimal mp = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["monthpay"].ToString()) : decimal.Parse("0.00");
        query = "select case when SUM(b.Amount) is null then '0' else SUM(b.Amount) end  amt from TPayrollOtherDeduction a " +
                "left join TPayrollOtherDeductionLine b on a.id=b.PayOtherDeduction_id " +
                "left join MOtherDeduction c on b.OtherDeduction_id=c.Id " +
                "where payroll_id is not null and " +
                "c.LoanType='False' and c.Amount>0 and b.Emp_id=" + empid + "";
        dt = dbhelper.getdata(query);
        decimal amt = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["amt"].ToString()) : decimal.Parse("0.00");
        dt = dbhelper.getdata(emp_pay(empid));
        decimal sp = 0;
        decimal rp = 0;
        if(decimal.Parse(dt.Rows[0]["year_service_rendered"].ToString())>0)
        {
            if (decimal.Parse(dt.Rows[0]["age"].ToString()) >= 60)
                rp = decimal.Parse(dt.Rows[0]["DailyRate"].ToString()) * decimal.Parse("22.5") * decimal.Parse(dt.Rows[0]["year_service_rendered"].ToString());
            if (dt.Rows[0]["PayrollType"].ToString() == "Fixed")
                sp = decimal.Parse(dt.Rows[0]["rate"].ToString()) * decimal.Parse("0.5") * decimal.Parse(dt.Rows[0]["year_service_rendered"].ToString());
            else
                sp = decimal.Parse(dt.Rows[0]["rate"].ToString()) * decimal.Parse("13") * decimal.Parse(dt.Rows[0]["year_service_rendered"].ToString());
        }
        deduction(empid);
        conertabletocash();
        mdr = master.NewRow();
        mdr["empid"] = empid.ToString();
        mdr["e_name"] = dt.Rows[0]["e_name"].ToString();
        mdr["unclaimsalary"] = Math.Round(ni, 2).ToString();
        mdr["13monthpay"] = Math.Round(mp, 2).ToString();
        mdr["saving"] = Math.Round(amt, 2).ToString();
        mdr["taxrefund"] = Math.Round(itr, 2).ToString(); 
        mdr["serviceincentiveleave"] = hdn_total_sil.Value;
        mdr["separationpay"] = Math.Round(sp, 2).ToString();
        mdr["retirementpay"] = Math.Round(rp, 2).ToString();
        mdr["taxpayable"] = Math.Round(itp, 2).ToString();
        mdr["deduction"] = hdn_total_deduction.Value;
        mdr["totalamt"] = "0";
        master.Rows.Add(mdr);
        ViewState["master"] = master;
        grid_compute.DataSource = master;
        grid_compute.DataBind();
    }
    protected void deduction(string empid)
    {
        string query = "select  b.Id as emp_id,b.PayrollGroupId,b.IdNumber,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, " +
                        "a.id,a.DateStart,a.status,a.interest, " +
                        "c.OtherDeduction,a.LoanAmount, " +
                        "(a.Balance -(select case when SUM(z.Amount) is null then '0' else SUM(z.Amount) end amt from TPayrollOtherDeductionLine z " +
                        "left join TPayrollOtherDeduction y on z.PayOtherDeduction_id=y.id " +
                        "where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null)) balance " +
                        "from MEmployeeLoan a " +
                        "left join MEmployee b on a.EmployeeId=b.id  " +
                        "left join MOtherDeduction c on a.OtherDeductionId=c.Id  " +
                        "where a.status like '%Approved%' and " +
                        "(a.Balance -(select case when SUM(z.Amount) is null then '0' else SUM(z.Amount) end amt from TPayrollOtherDeductionLine z " +
                        "left join TPayrollOtherDeduction y on z.PayOtherDeduction_id=y.id " +
                        "where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null)>0) and a.employeeid=" + empid + " and c.status='2' " +
                        "order by a.Id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        if (grid_view.Rows.Count == 0)
            hdn_total_deduction.Value = "0.00";
    }
    decimal t_deduct = 0;
    protected void deductionbound(object sender, GridViewRowEventArgs e)
    {
       // DataTable dt = dbhelper.getdata(emp_pay(Request.QueryString["empid"].ToString()));
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            t_deduct = t_deduct + decimal.Parse(e.Row.Cells[8].Text);
            hdn_total_deduction.Value = Math.Round(t_deduct, 2).ToString();
        }
    }
    protected void conertabletocash()
    {
        string query = "select a.id, a.leave,a.leavetype,case when (select leavestatus from memployee where ID= " + function.Decrypt(Request.QueryString["empid"].ToString(),true) + ")='False' then '0' else a.yearlytotal end yearlytotal,(select leavestatus from memployee where ID=" + function.Decrypt(Request.QueryString["empid"].ToString(),true) + ") leavestatus,  " +
                    "case when (select leavestatus from memployee where ID=" + function.Decrypt(Request.QueryString["empid"].ToString(),true) + ")='False' then 0 else case when (a.yearlytotal)-(select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + function.Decrypt(Request.QueryString["empid"].ToString(),true) + " and withpay='True') is null then a.yearlytotal  " +
                    "else " +
                    "(a.yearlytotal)-(case when (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId=" + function.Decrypt(Request.QueryString["empid"].ToString(),true) + " and withpay='True' and status like '%Approve%') is null then 0 else (select SUM(NumberOfHours) from TLeaveApplicationLine where LeaveId=a.Id and EmployeeId= " + function.Decrypt(Request.QueryString["empid"].ToString(),true) + " and withpay='True' and status like '%Approve%')  end) " +
                    "end end leave_bal " +
                    "from MLeave a where a.action is null  and a.converttocash='yes'";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_sil.DataSource = ds.Tables["table"];
        grid_sil.DataBind();
        if (grid_sil.Rows.Count == 0)
            hdn_total_sil.Value = "0.00";
    }
    public static string emp_pay(string empid)
    {
        string query = "SELECT a.firstname,a.lastname+', '+a.firstname+' '+a.middlename e_name ,FLOOR(DATEDIFF(DAY, CONVERT(datetime,RIGHT(a.dateofbirth,4)), GETDATE()) / 365.25) age,datediff(month, CONVERT(datetime,a.DateHired), CONVERT(datetime,a.DateResigned)) / 12.0 year_service_rendered,case when b.PayrollType = 'Fixed' then a.MonthlyRate else a.DailyRate end rate,b.PayrollType,a.DailyRate  from memployee a " +
                "left join MPayrollType b on a.PayrollTypeId=b.Id " +
                "where a.id=" + empid + "";
       return query;
    }
    decimal t_sil = 0;
    protected void silrowbound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = dbhelper.getdata(emp_pay(function.Decrypt(Request.QueryString["empid"].ToString(),true)));
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            t_sil = t_sil + decimal.Parse(e.Row.Cells[4].Text);
            t_sil = t_sil * decimal.Parse(dt.Rows[0]["DailyRate"].ToString());
            hdn_total_sil.Value =Math.Round(t_sil,2).ToString();
        }
    }
   
}