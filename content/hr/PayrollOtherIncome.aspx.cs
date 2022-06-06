using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_PayrollOtherIncome : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            disp();
        }
    }

    protected void disp()
    {
        string query = "select * from MOtherIncome";
        DataTable dt = dbhelper.getdata(query);
        grid_oi.DataSource = dt;
        grid_oi.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        disp();
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gr = (GridView)e.Row.FindControl("grid_oilist");
            string query = "select  " +
                            "case when left(convert(varchar,e.payrolldate,101),10)is null then 'pending' else left(convert(varchar,e.payrolldate,101),10) end payrolldate, " +
                            "case when left(convert(varchar,f.datestart,101),10)+'-'+left(convert(varchar,f.dateend,101),10) IS NULL then 'pending' else left(convert(varchar,f.datestart,101),10)+'-'+left(convert(varchar,f.dateend,101),10) end range, " +
                            "c.firstname+' '+c.lastname ename, d.OtherIncome,a.amount FROM TPayrollOtherIncomeLine a " +
                            "left join TPayrollOtherIncome b on a.PayOtherIncome_id=b.id " +
                            "left join memployee c on a.emp_id=c.id " +
                            "left join MOtherIncome d on a.OtherIncome_id=d.id " +
                            "left join TPayroll e on b.payroll_id=e.id " +
                            "left join TDTR f on b.payroll_id=f.payroll_id " +
                            "where a.OtherIncome_id=" + e.Row.Cells[0].Text + " and  c.firstname+''+c.middlename+''+c.lastname+''+c.IdNumber like '%" + txt_search.Text + "%'";
            DataTable dt = dbhelper.getdata(query);
            gr.DataSource = dt;
            gr.DataBind();
        }
    }
    protected void click_add_other(object sender, EventArgs e)
    {

      //  string c = "addemployee?user_id=" + function.Encrypt(user_id, true) + "";

        //string b = "addloan?user_id=" + function.Encrypt(user_id, true) + "&pgg=" + function.Encrypt(ddl_payroll_group.SelectedValue, true) + "";

       // string a = "addOtherIncome?user_id=" + function.Encrypt(user_id, true) + "&pgg=" + function.Encrypt(ddl_payroll_group.SelectedValue, true) + "";
   
        
        Response.Redirect("addOtherIncome?user_id=" + function.Encrypt(key.Value, true) +  "", false);//"&pg=" + function.Encrypt(ddl_payroll_group.SelectedValue, true) +

        //Response.Redirect("addOtherIncome?user_id=" + user_id + "&pg=" + ddl_payroll_group.SelectedValue + "", false);

        //Response.Redirect("addOtherIncome", false);

    }

   
}