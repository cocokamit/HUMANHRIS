using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_thirteendetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
        { Response.Redirect("quit"); }
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        string query = "select a.empid, a.detid,a.tmtrnid,b.LastName+', '+b.FirstName+' '+b.MiddleName employee,a.grossamt,a.wht,a.net from thirteenmonth_trn_details a left join memployee b on a.empid=b.Id where b.LastName+', '+b.FirstName+' '+b.MiddleName like'%" + txt_search.Text + "%' and  a.tmtrnid=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        disp();
    }
    protected void viewpayslip(object sender, EventArgs e)
    {

        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (decimal.Parse(row.Cells[5].Text) > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='tmpslip?empid=" + function.Encrypt(row.Cells[0].Text, true) + "&payid=" + function.Encrypt(row.Cells[1].Text, true) + "&b=single'", true);
            else
                Response.Write("<script>alert('Invalid!')</script>");
        }
    }
}