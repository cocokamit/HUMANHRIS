using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_detailsperpayroll : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            payid.Value = function.Decrypt(Request.QueryString["payid"].ToString(), true);
            disp();
        }
    }
    protected void disp()
    {
        string ggg = "select b.id,c.id,a.id as payrolllineid,a.EmployeeId,left(convert(varchar,b.PayrollDate,101),10)PayrollDate, left(convert(varchar,f.DateStart,101),10)+' - '+left(convert(varchar,f.DateEnd,101),10) datedtrrange, c.LastName+', '+c.FirstName+' '+c.MiddleName as c_name,c.IdNumber, " +
          
            "d.PayrollType,e.TaxCode,a.NetIncome " +
            "from TPayrollLine a " +
            "left join TPayroll b on a.PayrollId=b.Id " +
            "left join MEmployee c on a.EmployeeId=c.Id " +
            "left join MPayrollType d on a.PayrollTypeId=d.Id " +
            "left join MTaxCode e on a.TaxCodeId=e.Id " +
            "left join TDTR f on b.DTRId=f.Id " +
            "where a.PayrollId=" + payid.Value + " and b.status is null order by b.PayrollDate desc ";
        DataTable dt = dbhelper.getdata(ggg);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        string ggg = "select b.id,c.id,a.id as payrolllineid,a.EmployeeId,left(convert(varchar,b.PayrollDate,101),10)PayrollDate, left(convert(varchar,f.DateStart,101),10)+' - '+left(convert(varchar,f.DateEnd,101),10) datedtrrange, c.LastName+', '+c.FirstName+' '+c.MiddleName as c_name,c.IdNumber, " +
                   "d.PayrollType,e.TaxCode,a.NetIncome " +
                   "from TPayrollLine a " +
                   "left join TPayroll b on a.PayrollId=b.Id " +
                   "left join MEmployee c on a.EmployeeId=c.Id " +
                   "left join MPayrollType d on a.PayrollTypeId=d.Id " +
                   "left join MTaxCode e on a.TaxCodeId=e.Id " +
                   "left join TDTR f on b.DTRId=f.Id " +
                   "where a.PayrollId=" + payid.Value + " and (c.FirstName+c.MiddleName+c.LastName+c.IdNumber) like '%" + txt_search.Text + "%' and b.status is null order by b.PayrollDate desc ";
        DataTable dt = dbhelper.getdata(ggg);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void viewpayslip(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (decimal.Parse(row.Cells[8].Text) > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?printablepayslip?empid=" + function.Encrypt(row.Cells[1].Text, true) + "&payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=single'", true);
            else
                Response.Write("<script>alert('Invalid!')</script>");
        }
    }
}