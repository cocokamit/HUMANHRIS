using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_transotherincome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
           // key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            loadable();
            disp();
        }

    }

    protected void ChangeGroup(object sender, EventArgs e)
    {
        disp();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("lblStatus");
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton2");

            switch (e.Row.Cells[1].Text)
            {
                case "0":
                    lbl.Text = "-";
                    break;
                default:
                    lbl.CssClass += "fa-check-circle";
                     lb.Enabled = false;
                    lb.OnClientClick = null;
                    break;
            }

        }
    }

    protected void loadable()
    {
        string query = "select * from MPayrollGroup where status = '1' order by PayrollGroup";
        DataTable dt = dbhelper.getdata(query);

        ddl_payroll_group.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }

    protected void disp()
    {
        string query = "select top 50 a.id,left(convert(varchar,a.entrydate,101),10)entrydate,b.payrollgroup pg,a.remarks, " +
        "a.action,case when a.payroll_id is null then 0 else a.payroll_id end payroll_id, " +
        "CONVERT(varchar,d.DateStart, 101) +'-'+ CONVERT(varchar,d.DateEnd, 101) period " +
        "from TPayrollOtherIncome a  " +
        "left join MPayrollGroup b on a.PayrollGroupId=b.Id  " +
        "left join TPayroll c on a.payroll_id=c.Id " +
        "left join TDTR d on c.DTRId=d.Id " +
        "where a.action is null and b.id = " + ddl_payroll_group.SelectedValue + " order by d.DateStart desc";

        //DataTable dt = dbhelper.getdata("select top 50 a.id,left(convert(varchar,a.entrydate,101),10)entrydate,b.payrollgroup pg,a.remarks from TPayrollOtherIncome a " +
        //"left join MPayrollGroup b on a.PayrollGroupId=b.Id  where a.payroll_id is null and a.action is null order by a.id desc");

        DataTable dt = dbhelper.getdata(query); 
        grid_view1.DataSource = dt;
        grid_view1.DataBind();
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        string query = "select top 50 a.id,left(convert(varchar,a.entrydate,101),10)entrydate,b.payrollgroup pg,a.remarks from TPayrollOtherIncome a " +
                         "left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.payroll_id is null and a.action is null ";
        if (int.Parse(ddl_payroll_group.SelectedValue) > 0)
            query += "and b.id=" + ddl_payroll_group.SelectedValue + "";
        if (txt_f.Text.Length > 0 && txt_t.Text.Length > 0)
            query += " and left(convert(varchar,a.entrydate,120),10)>='" + txt_f.Text + "' and left(convert(varchar,a.entrydate,120),10) <='" + txt_t.Text + "'";
        query += "order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view1.DataSource = dt;
        grid_view1.DataBind();
    }

    protected void click_add_other(object sender, EventArgs e)
    {
        Response.Redirect("addOtherIncome");
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('viewdetailsotherincome?&trid=" + function.Encrypt(row.Cells[0].Text, true) + "')", true);
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Value == "Yes")
            {
                DataTable dt = dbhelper.getdata("select * from TPayrollotherincome where id=" + row.Cells[0].Text + " and payroll_id is null");
                if (dt.Rows.Count > 0)
                {
                    string stat = "Cancelled-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString();
                    dbhelper.getdata("update TPayrollOtherincome set action='" + stat + "' where id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update pay_adjustment_details set pay_id=NULL where pay_id=" + row.Cells[0].Text + "");
                    Response.Redirect("transotherincome");
                }
                else
                    Response.Write("<script>alert('Transaction Denied! Due to payroll transaction is already done.')</script>");
            }
            else { }
        }
    }

  

  
}