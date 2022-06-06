using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_coop_list : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }
    protected void addOT(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addLOAN");
    }
    protected void click_delete_employee(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {


            string query = "update MEmployeeLoan set status ='" + "Cancel-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "' where id= " + row.Cells[0].Text + "";
            dbhelper.getdata(query);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_LOAN'", true);
        }
    }
    protected void getdata()
    {
       

       string query = "select a.Id,a.LoanAmount,a.interest,a.MonthlyAmortization,a.Balance,a.Remarks,a.status, " +
               "b.OtherDeduction " +
               "from MEmployeeLoan a " +
               "left join MOtherDeduction b on a.OtherDeductionId=b.Id " +
               "where a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
        

            
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           string id = row.Cells[0].Text;
           // Div1.Visible = true;
           // Div2.Visible = true;
            

           //string query = " select a.Amount,a.balance, " +
           //          "b.payroll_id, " +
           //          "c.PayrollDate " +
           //          "from TPayrollOtherDeductionLine a " +
           //          "left join TPayrollOtherDeduction b on a.PayOtherDeduction_id=b.id " +
           //          "left join TPayroll c on b.payroll_id=c.id " +
           //          "where a.loan_id=" + id + " and b.payroll_id is not null order by c.PayrollDate desc ";
           // DataTable dt = dbhelper.getdata(query);
           // grid_bal.DataSource = dt;
           // grid_bal.DataBind();

           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='KOISK_LOAN';window.open('','_new').location.href='print_leadger?key=" + id + "'", true);

            //DataTable dt = dbhelper.getdata(query);
           // grid_bal.DataSource = dt;
           // grid_bal.DataBind();

           
        }
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_LOAN", false);
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}