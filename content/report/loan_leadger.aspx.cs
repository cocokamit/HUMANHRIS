using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_report_loan_leadger : System.Web.UI.Page
{
    public static string query;
    public static DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
            loadable();

    }

    protected void loadable()
    {
        string query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee order by LastName asc";
        DataTable dt = dbhelper.getdata(query);

        ddl_employee.Items.Clear();
        ddl_employee.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
        //get Other Deduction
        query = "select * from MOtherDeduction where action is null";
        dt = dbhelper.getdata(query);
        ddl_loantype.Items.Clear();
        ddl_loantype.Items.Add(new ListItem("N/A", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_loantype.Items.Add(new ListItem(dr["OtherDeduction"].ToString(), dr["Id"].ToString()));
        }
    }


    protected void btn_search(object sender, EventArgs e)
    {
        query = "select a.id,a.EmployeeId,OtherDeductionId,a.LoanAmount,a.LoanNumber,a.DateStart,a.Remarks, " +
                "b.LastName+','+b.FirstName as fullname, " +
                "c.OtherDeduction " +
                "from Memployeeloan a " +
                "left join Memployee b on a.EmployeeId=b.id " +
                "left join MOtherDeduction c on a.OtherDeductionId=c.id " +
                "where a.EmployeeId='" + ddl_employee.SelectedValue + "' and a.OtherDeductionId='" + ddl_loantype.SelectedValue + "'  " +
                "order by id desc ";

        ds = bol.display(query);

        grid_view.DataSource = ds;
        grid_view.DataBind();
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

           // Response.Redirect("print_leadger", false);

            hf_id.Value = row.Cells[0].Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='loan_leadger';window.open('','_new').location.href='print_leadger?key=" + function.Encrypt(hf_id.Value, true) + "'", true);

        }
    }
}