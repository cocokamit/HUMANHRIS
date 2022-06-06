using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_coop : System.Web.UI.Page
{
   // public static string user_id,query;
    public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
       string query = "select * from MOtherDeduction";
        dt = dbhelper.getdata(query);
        dll_deduction.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            dll_deduction.Items.Add(new ListItem(dr["OtherDeduction"].ToString(), dr["Id"].ToString()));
        }
    }

    protected void txt_ew(object sender, EventArgs e)
    {
       string query = "select a.Balance - case when ( select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) IS null then 0 else (select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) end as balansing " +
                "from MEmployeeLoan a where a.EmployeeId=" + Session["emp_id"].ToString() + " and a.OtherDeductionId='" + dll_deduction.SelectedValue + "' ";


        DataTable dtt = dbhelper.getdata(query);

        if (dtt.Rows.Count != 0)
        {
            if (double.Parse(dtt.Rows[0]["balansing"].ToString()) <= 0)
            {

                query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
                dt = dbhelper.getdata(query);
                double aw = 0;
                aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
                lbl_interest.Text = aw.ToString();

                double total = 0;
                total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
                txt_amortization.Text = total.ToString();

                txt_amount.Enabled = true;
            }
            else
            {
                txt_amount.Enabled = false;
                txt_amortization.Enabled = false;
                txt_no.Enabled = false;
                txt_memo.Text = "";
                txt_amortization.Text = "";
                txt_amount.Text="";
                lbl_interest.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pending Loan'); window.location='KOISK_addLOAN'", true);
            }
        }
        else
        {
            query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
            dt = dbhelper.getdata(query);
            double aw = 0;
            aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
            lbl_interest.Text = aw.ToString();

            double total = 0;
            total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
            txt_amortization.Text = total.ToString();

            txt_amount.Enabled = true;
        }
    }

    protected void txt_aw(object sender, EventArgs e)
    {
       string query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
        dt = dbhelper.getdata(query);
        double aw = 0;
        aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
        lbl_interest.Text = aw.ToString();


        double total = 0;
        total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        txt_amortization.Text = total.ToString();

        txt_no.Enabled = true;
    }
    protected void txt_no_TextChanged(object sender, EventArgs e)
    {
        double total = 0;
        total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        txt_amortization.Text = total.ToString();
    }
    protected void click_save_coop(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        a.sa = Session["emp_id"].ToString();
        a.sb = dll_deduction.SelectedValue;
        a.sc = dll_no.Text;
        a.sd = txt_amount.Text;
        a.se = txt_amortization.Text;
        a.sf = txt_no.Text;
        a.sg = (double.Parse(txt_amount.Text) + double.Parse(lbl_interest.Text)).ToString();
        a.sh = Session["user_id"].ToString();
        a.si = Session["user_id"].ToString();
        a.sj = "For Approval-" + Session["user_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString();
        a.sk = txt_memo.Text.Replace("'", "");
        a.sl = lbl_interest.Text;
        string val = bol.employee_loan(a);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_LOAN'", true);
    }
}