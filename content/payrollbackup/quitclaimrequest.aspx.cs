using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_payroll_quitclaimrequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            this.disp();
    }
    protected void close(object sender, EventArgs e)
    {
        panelOverlay.Visible = false;
        panelPopUpPanel.Visible = false;
        div_compute_quit.Visible = false;
       
    }
    protected void disp()
    {
        string query = "select b.id empid,a.id,left(convert(varchar,a.date,101),10)date,b.LastName+', '+b.FirstName empname,a.notes from quit_claim_request a " +
                        "left join memployee b on a.empid=b.Id " +
                        "where a.action is null and a.status='Pending'";
        DataTable dt = dbhelper.getdata(query);
        grid_quitclaimrequest.DataSource = dt;
        grid_quitclaimrequest.DataBind();
    }
    protected void view_files(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            panelOverlay.Visible = true;
            panelPopUpPanel.Visible = true;
            DataTable dt = dbhelper.getdata("select * from memployeeclearancefile where reqid=" + row.Cells[0].Text + "");
            foreach(DataRow dr in dt.Rows)
            {
                Image img = new Image();
                img.ID=dr["id"].ToString();
                img.ImageUrl = "~/files/peremp/" + row.Cells[1].Text + "/Clearance/" + dr["id"].ToString() + "_Clearance." + dr["fileextension"].ToString();
                img.Width = 420;
                img_id.Controls.Add(img);
                img_id.Controls.Add(new LiteralControl("<br />"));
            }
            panelPopUpPanel.Style.Add("left","450px");
            panelPopUpPanel.Style.Add("top", "10%");
        }
    }
    protected void compute_quit_2316(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            slect_empid.Value = row.Cells[1].Text;
            DataTable dtchek = getdata.dtitr(DateTime.Now.Year.ToString(),row.Cells[1].Text);
            if (dtchek.Rows.Count > 0)
            {
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(this.GetType(), "Confirm", "Confirm()", true);
            }
            else
                Response.Redirect("2316process?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&empid=" + row.Cells[1].Text + "&y=" + DateTime.Now.Year.ToString() + "");
        }
    }
    protected void textchange(object sender, EventArgs e)
    {
        if (TextBox2.Text == "Yes")
        {
            Response.Redirect("2316process?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&empid=" + slect_empid.Value + "&y=" + DateTime.Now.Year.ToString() + "");
        }
        else { }
    }
    protected void compute_quit_claim(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
             DataTable dtchek = getdata.dtitr(DateTime.Now.Year.ToString(),row.Cells[1].Text);
             if (dtchek.Rows.Count > 0)
             {
                 string yyyy = DateTime.Now.Year.ToString();
                 Response.Redirect("quitclaimcompute?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "&empid=" + function.Encrypt(row.Cells[1].Text, true) + "&yyyy=" + function.Encrypt(yyyy, true) + "&reqid=" + function.Encrypt(row.Cells[0].Text, true) + "");
             }
             else
                 Response.Write("<script>alert('Please compute 2316 first!')</script>");
        }
    }
    protected void compute(string empid)
    {
        string query = "select a.NetIncome from TPayrollLine a " +
                        "left join TPayroll b on a.PayrollId=b.id " +
                        "where a.empstatus='4' and b.status is null and a.employeeid='" + empid + "'";
        DataTable dt = dbhelper.getdata(query);
        decimal ni = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["NetIncome"].ToString()) : decimal.Parse("0.00");
        lbl_lp.Text = Math.Round(ni, 2).ToString();

        query = "select SUM(b.NetIncome)/12 monthpay from TPayroll a " +
                "left join TPayrollLine b on a.Id=b.PayrollId " +
                "where YEAR(a.PayrollDate)=YEAR(GETDATE()) and a.status is null and b.EmployeeId=" + empid + "";
        dt = dbhelper.getdata(query);
        decimal mp = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["monthpay"].ToString()) : decimal.Parse("0.00");
        lbl_13monthpay.Text = Math.Round(mp, 2).ToString();

        query = "select SUM(b.Amount) amt from TPayrollOtherDeduction a " +
                "left join TPayrollOtherDeductionLine b on a.id=b.PayOtherDeduction_id " +
                "left join MOtherDeduction c on b.OtherDeduction_id=c.Id " +
                "where payroll_id is not null and " +
                "c.LoanType='False' and c.Amount>0 and b.Emp_id=" + empid + "";
        dt = dbhelper.getdata(query);
        decimal amt = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["amt"].ToString()) : decimal.Parse("0.00");
        lbl_internalsavings.Text = Math.Round(amt, 2).ToString();

        decimal itr = decimal.Parse("0.00");
        lbl_titr.Text = Math.Round(itr, 2).ToString();

        query = "SELECT a.firstname, FLOOR(DATEDIFF(DAY, CONVERT(datetime,RIGHT(a.dateofbirth,4)), GETDATE()) / 365.25) age,FLOOR(DATEDIFF(DAY, CONVERT(datetime,RIGHT(a.DateHired,4)), CONVERT(datetime,RIGHT(a.DateResigned,4)) ) / 365.25)year_service_rendered,case when b.PayrollType = 'Fixed' then a.MonthlyRate else a.DailyRate end rate,b.PayrollType  from memployee a " +
            "left join MPayrollType b on a.PayrollTypeId=b.Id " +
            "where a.id=" + empid + "";
        dt = dbhelper.getdata(query);

        if (decimal.Parse(dt.Rows[0]["year_service_rendered"].ToString()) > 0)
        {
           if (decimal.Parse(dt.Rows[0]["age"].ToString()) >= 60)
           {
               if (dt.Rows[0]["PayrollType"].ToString() == "Fixed")
               {
 
               }
           }
        }
        decimal rf = decimal.Parse("0.00");
        lbl_rf.Text = Math.Round(rf, 2).ToString();
        deduction(empid);
        decimal tax = decimal.Parse("0.00");
        lbl_tax.Text = Math.Round(tax, 2).ToString();
    }
    protected void deduction(string empid)
    {
        string query = "select top 20 b.Id as emp_id,b.PayrollGroupId,b.IdNumber,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, " +
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
                        "where z.Emp_id=a.EmployeeId and z.loan_id=a.id and y.payroll_id is not null)>0) and a.employeeid=" + empid + " " +
                        "order by a.Id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void proccess(object sender, EventArgs e)
    {
        string ret = "0";
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("quit_claim_details_proc", con))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                SqlConnection conn = new SqlConnection(dbconnection.conn);
                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Request.QueryString["app_id"].ToString();
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true).ToString();
                cmd.Parameters.Add("@lastpayroll_amt", SqlDbType.VarChar, 5000).Value = lbl_lp.Text;
                cmd.Parameters.Add("@internalsavings_amt", SqlDbType.VarChar, 5000).Value = lbl_internalsavings.Text;
                cmd.Parameters.Add("@itr_amt", SqlDbType.VarChar, 5000).Value = lbl_titr.Text;
                cmd.Parameters.Add("@retirement_amt", SqlDbType.VarChar, 5000).Value = lbl_rf.Text;
                cmd.Parameters.Add("@separation_amt", SqlDbType.VarChar, 5000).Value = txt_separationfee.Text.Length>0?txt_separationfee.Text:"0.00";
                cmd.Parameters.Add("@totaltax_amt", SqlDbType.VarChar, 5000).Value = lbl_tax.Text;
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                ret = cmd.Parameters["@out"].Value.ToString();
                con.Close();
            }
        }
    }
    //protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox ChkBoxHeader = (CheckBox)grid_quitclaimrequest.HeaderRow.FindControl("chkboxSelectAll");
    //    int i = 0;
    //    foreach (GridViewRow row in grid_quitclaimrequest.Rows)
    //    {
    //        CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkEmp");

    //        if (ChkBoxHeader.Checked == true)
    //        {
    //            ChkBoxRows.Checked = true;
    //            i++;
    //        }
    //        else
    //        {

    //            ChkBoxRows.Checked = false;
    //            if (i > 0)
    //            {
    //                i--;
    //            }
    //        }

    //    }
    //    lbl_del_notify.Text = i + " rows".ToString();
    //}
}