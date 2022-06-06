using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_printable_terminalcomputation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            this.disp();
    }
    protected void disp()
    {
        string query = "select *,left(convert(varchar,date,101),10)dateinput from quit_claim_details where quitclaimdetails_id=" + Request.QueryString["ret"].ToString() + "";
        DataTable dt = dbhelper.getdata(query);
        DataTable getemp = getdata.empsearch("none",dt.Rows[0]["empid"].ToString());
        lbl_name.Text = getemp.Rows[0]["emp_name"].ToString();
        lbl_pos.Text = getemp.Rows[0]["position"].ToString();
        lbl_date_resigned.Text = getemp.Rows[0]["dateresigned"].ToString();
        lbl_date_released.Text = dt.Rows[0]["dateinput"].ToString();
        lbl_us.Text = dt.Rows[0]["lastpayroll_amt"].ToString();
        lbl_13monthpay.Text = dt.Rows[0]["thirteenmonthpay_amt"].ToString();
        lbl_sil.Text = dt.Rows[0]["sil"].ToString();
        lbl_total.Text = (decimal.Parse(dt.Rows[0]["lastpayroll_amt"].ToString()) + decimal.Parse(dt.Rows[0]["thirteenmonthpay_amt"].ToString()) + decimal.Parse(dt.Rows[0]["sil"].ToString())).ToString();
        lbl_tp.Text = dt.Rows[0]["tax_payable"].ToString();
        lbl_other.Text = dt.Rows[0]["internal_deduction"].ToString();
        lbl_total_deduction.Text = (decimal.Parse(dt.Rows[0]["tax_payable"].ToString()) + decimal.Parse(dt.Rows[0]["internal_deduction"].ToString())).ToString();
        lbl_gt.Text = lbl_total.Text;
        lbl_tr.Text = dt.Rows[0]["tax_refund"].ToString();
        lbl_is.Text = dt.Rows[0]["internalsavings_amt"].ToString();
        lbl_netpay.Text = (decimal.Parse(lbl_total.Text) + decimal.Parse(dt.Rows[0]["tax_refund"].ToString()) + decimal.Parse(dt.Rows[0]["internalsavings_amt"].ToString()) -decimal.Parse(lbl_total_deduction.Text)).ToString(); 
    }
}