using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_addloan : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
           // key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            loadable();
        }
    }
    protected void loadable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
        //get Other Deduction
        query = "select * from MOtherDeduction where action is null";
        dt = dbhelper.getdata(query);
        dll_deduction.Items.Clear();
        dll_deduction.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            dll_deduction.Items.Add(new ListItem(dr["OtherDeduction"].ToString(), dr["Id"].ToString()));
        }
    }
    protected void click_pg(object sender, EventArgs e)
    {
       //get emplooyee
       string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                       "left join MPosition b on a.PositionId=b.Id where a.PayrollGroupId=" +ddl_pg.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        ddl_emp.Items.Clear();
        ddl_emp.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
        dll_deduction.Enabled = true;
    }
    protected void txt_ew(object sender, EventArgs e)
    {
        if (int.Parse(dll_deduction.SelectedValue) > 0)
        {
            string query = "select a.Balance - case when ( select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) IS null then 0 else (select SUM(amount) from TPayrollOtherDeductionLine where Loan_id=a.Id ) end as balansing " +
                    "from MEmployeeLoan a where a.EmployeeId=" + ddl_emp.SelectedValue + " and a.OtherDeductionId='" + dll_deduction.SelectedValue + "' and a.status like'%Approved%' ";
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
        //string query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
        //DataTable dt = dbhelper.getdata(query);
        //double aw = 0;
        //aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * double.Parse(dt.Rows[0]["interest"].ToString());
        //lbl_interest.Text = aw.ToString();
        //double total = 0;
        //total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        //txt_amortization.Text = total.ToString();
        txt_no.Enabled = true;
    }

    protected void txt_no_TextChanged(object sender, EventArgs e)
    {
        string query = "select * from MOtherDeduction where Id=" + dll_deduction.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        double aw = 0;
        aw = txt_amount.Text.Trim().Length == 0 ? 0 : double.Parse(txt_amount.Text) * (double.Parse(dt.Rows[0]["interest"].ToString()) * double.Parse(txt_no.Text));
        lbl_interest.Text = aw.ToString();
        //double total = 0;
        //total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        //txt_amortization.Text = total.ToString();


        double total = 0;
        total = txt_no.Text.Trim().Length == 0 ? 0 : (double.Parse(txt_amount.Text) + (double.Parse(lbl_interest.Text))) / (double.Parse(txt_no.Text) * 2);
        txt_amortization.Text = total.ToString();
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (chk())
        {
            stateclass a = new stateclass();
            a.sa = ddl_emp.SelectedValue;
            a.sb = dll_deduction.SelectedValue;
            a.sc = dll_no.Text;
            a.sd = txt_amount.Text;
            a.se = txt_amortization.Text;
            a.sf = txt_no.Text;
            a.sg = (double.Parse(txt_amount.Text) + double.Parse(lbl_interest.Text)).ToString();
            a.sh = Session["user_id"].ToString();
            a.si = Session["user_id"].ToString();
            a.sj = "Approved-" + DateTime.Now.ToShortDateString().ToString();
            a.sk = txt_memo.Text;
            a.sl = lbl_interest.Text;
            string val = bol.employee_loan(a);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Mloan?pg=" + function.Encrypt(pg1.Value, true) + " '", true);
        }
    }
    protected bool chk()
    {
        bool err = true;
        if (int.Parse(ddl_pg.SelectedValue) == 0)
        {
            lbl_pg.Text = "*";
            err = false;
        }
        else if (int.Parse(ddl_emp.SelectedValue) == 0)
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
        else if (txt_no.Text.Length == 0)
        {
            lbl_not.Text = "*";
            err = false;
        }
        else if (lbl_interest.Text.Length == 0)
        {
            lbl_li.Text = "*";
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
        return err;
    }
}