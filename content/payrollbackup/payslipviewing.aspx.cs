using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_payslipviewing : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            lodable();
            //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            //ddl_monthly.SelectedValue = DateTime.Now.Month.ToString();
            //drop_year.SelectedValue = DateTime.Now.Year.ToString();

        }
    }
    protected void lodable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void ddl_monthly_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void search(object sender, EventArgs e)
    {
 
    }
}