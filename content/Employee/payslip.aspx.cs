using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_payslip : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            //payid.Value = function.Decrypt(Request.QueryString["payid"].ToString(), true);
            lodable();
            disp();
        }
    }
    protected void lodable()
    {
        ddl_month.SelectedValue = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
        ddl_year.Items.Clear();
       // ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("Select Year", "0"));
        for (int i = 2017; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
        }
        ddl_year.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected void disp()
    {
        string ggg = "select top 5 b.id,(select DATENAME(month,b.PayrollDate))+' '+(case when(select DAY(LEFT(CONVERT(varchar,f.DateStart,101),10)))=11 then '10th' else '25th' end)+' '+(select CONVERT(varchar(10),(select year(b.PayrollDate))))remss,b.status_1,c.id,a.id as payrolllineid,a.EmployeeId,left(convert(varchar,b.PayrollDate,101),10)PayrollDate, left(convert(varchar,f.DateStart,101),10)+' - '+left(convert(varchar,f.DateEnd,101),10) datedtrrange, c.LastName+', '+c.FirstName+' '+c.MiddleName as c_name,c.IdNumber, d.PayrollType,e.TaxCode,a.NetIncome from TPayrollLine a left join TPayroll b on a.PayrollId=b.Id left join MEmployee c on a.EmployeeId=c.Id left join MPayrollType d on a.PayrollTypeId=d.Id left join MTaxCode e on a.TaxCodeId=e.Id left join TDTR f on b.DTRId=f.Id where a.EmployeeId=" + Session["emp_id"].ToString() + " and b.status is null and b.status_1 is not null and convert(date,b.PayrollDate)>=convert(date,'09/19/2018') order by b.PayrollDate desc";
        
        DataTable dt = dbhelper.getdata(ggg);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    protected void search(object sender, EventArgs e)
    {
        string ggg = "select b.id,b.status_1,(select DATENAME(month,b.PayrollDate))+' '+(case when(select DAY(LEFT(CONVERT(varchar,f.DateStart,101),10)))=11 then '10th' else '25th' end)+' '+(select CONVERT(varchar(10),(select year(b.PayrollDate))))remss,c.id,a.id as payrolllineid,a.EmployeeId,left(convert(varchar,b.PayrollDate,101),10)PayrollDate, left(convert(varchar,f.DateStart,101),10)+' - '+left(convert(varchar,f.DateEnd,101),10) datedtrrange, c.LastName+', '+c.FirstName+' '+c.MiddleName as c_name,c.IdNumber, " +
                   "d.PayrollType,e.TaxCode,a.NetIncome " +
                   "from TPayrollLine a " +
                   "left join TPayroll b on a.PayrollId=b.Id " +
                   "left join MEmployee c on a.EmployeeId=c.Id " +
                   "left join MPayrollType d on a.PayrollTypeId=d.Id " +
                   "left join MTaxCode e on a.TaxCodeId=e.Id " +
                   "left join TDTR f on b.DTRId=f.Id " +
                     "where a.EmployeeId=" + Session["emp_id"].ToString() + " and b.status is null and convert(date,b.PayrollDate)>=convert(date,'09/19/2018') and (select month(b.PayrollDate))=" + ddl_month.SelectedValue + " and (select year(b.PayrollDate))=" + ddl_year.SelectedValue + " order by b.PayrollDate desc ";
                  // " where month(f.DateStart)=" + ddl_month.SelectedValue + " and  year(f.DateStart)=" + ddl_year.SelectedValue + " ";

                  // "where (c.FirstName+c.MiddleName+c.LastName+c.IdNumber) like '%" + txt_search.Text + "%' and b.status is null order by b.PayrollDate desc ";
        DataTable dt = dbhelper.getdata(ggg);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void viewpayslip(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (decimal.Parse(row.Cells[4].Text) > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='printablepayslip?empid=" + function.Encrypt(row.Cells[1].Text, true) + "&payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=single'", true);
            else
                Response.Write("<script>alert('Invalid!')</script>");
        }
    }
   

}