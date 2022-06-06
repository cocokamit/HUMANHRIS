using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_editloan : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getdata();
            //fload();
        }
    }

    protected void getdata()
    {
        fload();

       string query="select a.id, a.IdNumber,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,b.Position, " +
            "c.id,c.Balance,c.DateStart,c.LoanNumber,c.LoanAmount,c.MonthlyAmortization,c.NumberOfMonths,c.Remarks,c.OtherDeductionId,c.EmployeeId, " +
            "d.OtherDeduction " +
            "from MEmployee a  " +
            "left join MPosition b on a.PositionId=b.Id " +
            "left join MEmployeeLoan c on a.id=c.EmployeeId " +
            "left join MOtherDeduction d on c.OtherDeductionId = d.Id " +
            "where c.id="  +Request.QueryString["app_id"].ToString();

       DataTable dt = dbhelper.getdata(query);

        txt_amortization.Text = dt.Rows[0]["MonthlyAmortization"].ToString();
        txt_amount.Text = dt.Rows[0]["LoanAmount"].ToString();
        txt_memo.Text = dt.Rows[0]["Remarks"].ToString();
        txt_no.Text = dt.Rows[0]["NumberOfMonths"].ToString();
        dll_no.Text = dt.Rows[0]["LoanNumber"].ToString();
        txt_emp.Text = dt.Rows[0]["Fullname"].ToString();

        dll_deduction.SelectedValue = dt.Rows[0]["OtherDeductionId"].ToString();

     
        
    }

    protected void txt_no_TextChanged(object sender, EventArgs e)
    {
        double total = 0;



        total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) / double.Parse(txt_no.Text));

        txt_amortization.Text = total.ToString();
    }

    protected void fload()
    {
       string query = "select OtherDeduction,id from MOtherDeduction";
        DataTable dt = dbhelper.getdata(query);
        dll_deduction.Items.Clear();
        for (int i = 0; i < dt.Rows.Count; i++)
            dll_deduction.Items.Insert(0, new ListItem(dt.Rows[i]["OtherDeduction"].ToString(), dt.Rows[i]["id"].ToString()));
        dll_deduction.Items.Insert(0, "");
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        stateclass a = new stateclass();

        a.sb = dll_deduction.SelectedValue;
        a.sc = dll_no.Text;
        a.sd = txt_amount.Text;
        a.se = txt_amortization.Text;
        a.sf = txt_no.Text;
        if (check_paid.Checked == true)
            a.sg = "True";
        else
            a.sg = "False";
        a.sh = Session["user_id"].ToString();
        a.si = txt_memo.Text;
        a.sj = Request.QueryString["app_id"].ToString();

        string val = bol.edit_loan(a);

    }
}