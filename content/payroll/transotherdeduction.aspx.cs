using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class content_payroll_transotherdeduction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            loadable();
            disp();
        }
    }
    protected void loadable()
    {
        string query = "select * from MPayrollGroup where payrollgroup<>'Resigned' and status <> 0 order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select  a.id,left(convert(varchar,a.entrydate,101),10)entrydate,b.payrollgroup pg,a.remarks from TPayrollOtherDeduction a " +
                                        "left join MPayrollGroup b on a.PayrollGroupId=b.Id where  a.action is null order by a.id desc ");
        grid_view1.DataSource = dt;
        grid_view1.DataBind();
    }

   
    protected void btn_search_Click(object sender, EventArgs e)
    {
        string query = "select a.id,left(convert(varchar,a.entrydate,101),10)entrydate,b.payrollgroup pg,a.remarks from TPayrollOtherDeduction a " +
                        "left join MPayrollGroup b on a.PayrollGroupId=b.Id where  a.action is null ";
        if (int.Parse(ddl_pg.SelectedValue) > 0)
            query += "and a.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        if (txt_f.Text.Length > 0 && txt_t.Text.Length > 0)
            query += " and left(convert(varchar,a.entrydate,120),10)>='" + txt_f.Text + "' and left(convert(varchar,a.entrydate,120),10) <='" + txt_t.Text + "'";
        query += "order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view1.DataSource = dt;
        grid_view1.DataBind();
    }
    protected void click_add_other(object sender, EventArgs e)
    {
        Response.Redirect("addOtherDeduction", false);
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='viewdetailsotherdeduction?&trid=" + function.Encrypt(row.Cells[0].Text, true) + "'", true);
        }
    }
    protected void cancel_tran(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Value == "Yes")
            {
                DataTable dt = dbhelper.getdata("select * from TPayrollOtherDeduction where id=" + row.Cells[0].Text + " and payroll_id is null");
                if (dt.Rows.Count > 0)
                {
                    string stat = "Cancelled-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString();
                    dbhelper.getdata("update TPayrollOtherDeduction set action='" + stat + "' where id=" + row.Cells[0].Text + "");
                    Response.Redirect("transotherdeduction");
                }
                else
                    Response.Write("<script>alert('Transaction Denied! Due to payroll transaction is already done.')</script>");
            }
            else { }
        }
    }
}