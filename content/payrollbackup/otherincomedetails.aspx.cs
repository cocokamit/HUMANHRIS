using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_otherincomedetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["trid"].ToString(), true);
            disp();

        }
    }
    protected void disp()
    {
        string query = "select " +
                        "a.id, c.IdNumber+' - '+ c.LastName+', '+c.FirstName e_name, " +
                        "b.Otherincome,a.nontaxable_amt,a.Taxable_amt,a.worked_hrs " +
                        "from TPayrollotherincomeline a " +
                        "left join MOtherincome b on a.Otherincome_id=b.Id " +
                        "left join MEmployee c on a.Emp_id=c.Id " +
                        "where a.PayOtherincome_id=" + key.Value + "";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        grid_view.HeaderRow.TableSection = TableRowSection.TableHeader;
        grid_view.UseAccessibleHeader = true;
    }
    protected void updateTPayrollotherincomeline(object sender, EventArgs e)
    {
       
        int ii = 0;
        foreach (GridViewRow gr in grid_view.Rows)
        {
            TextBox txt_amt = (TextBox)grid_view.Rows[ii].Cells[2].FindControl("txt_amt");
            dbhelper.getdata("update TPayrollotherincomeline set Amount='" + txt_amt .Text+ "' where id=" + gr.Cells[3].Text + "");
            ii++;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='viewdetailsotherincome?trid=" + Request.QueryString["trid"].ToString() + "'", true);
    }
}