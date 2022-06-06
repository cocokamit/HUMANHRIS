using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_approve_loan : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

        ap.Value = function.Decrypt(Request.QueryString["ap"].ToString(), true);
        if(!IsPostBack)
            loadable();
    }

    protected void loadable()
    {


       string query = "select * from MOtherDeduction";
       DataTable dt = dbhelper.getdata(query);
        foreach (DataRow dr in dt.Rows)
        {
            dll_deduction.Items.Add(new ListItem(dr["OtherDeduction"].ToString(), dr["Id"].ToString()));
        }


        //get emplooyee
        query = "select a.Id,a.EmployeeId,a.LoanAmount,a.MonthlyAmortization,a.NumberOfMonths,a.interest,a.OtherDeductionId,a.Remarks, " +
                "b.LastName +', '+ b.FirstName +' '+b.MiddleName as Fullname " +
                "from MEmployeeLoan a " +
                "left join MEmployee b on a.EmployeeId=b.Id " +
                "where a.Id=" + ap.Value + "";
        dt = dbhelper.getdata(query);
       
        txt_name.Text = dt.Rows[0]["Fullname"].ToString();
        txt_amount.Text = dt.Rows[0]["LoanAmount"].ToString();
        txt_memo.Text = dt.Rows[0]["Remarks"].ToString();
        txt_amortization.Text = dt.Rows[0]["MonthlyAmortization"].ToString();
        txt_no.Text = dt.Rows[0]["NumberOfMonths"].ToString();
        lbl_interest.Text = dt.Rows[0]["interest"].ToString();
        dll_deduction.SelectedValue = dt.Rows[0]["OtherDeductionId"].ToString();


    }
    protected void txt_aw(object sender, EventArgs e)
    {
       string query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
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
    protected void btn_save_Click(object sender, EventArgs e)
    {
        stateclass a = new stateclass();

       string query = "update MEmployeeLoan set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id="+ ap.Value +" ";
        dbhelper.getdata(query);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Mloan?user_id=" + function.Encrypt(key.Value, true) + "'", true);

        //a.sa = txt_name.Text;
        //a.sb = "0";
        //a.sc = dll_no.Text;
        //a.sd = txt_amount.Text;
        //a.se = txt_amortization.Text;
        //a.sf = txt_no.Text;
        //a.sg = txt_amount.Text;
        //a.sh = Session["user_id"].ToString();
        //a.si = Session["user_id"].ToString();
        //a.sj = "Approved-" + user_id + "-" + DateTime.Now.ToShortDateString().ToString();
        //a.sk = txt_memo.Text;
        //string val = bol.employee_loan(a);
    }
}