using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_proccesspayroll : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            // user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disppayroll();
           
        }
    }

    protected void lodable()
    {
        string query = "select * from MPayrollGroup where status = '1' order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_add_dtr(object sender, EventArgs e)
    {
        //if (ddl_pg.SelectedValue =="0")
        //    Response.Write("<script>alert('Please select PayrollGroup!')</script>");
        //else
          Response.Redirect("addproccesspayroll", false);
    }
    protected void disppayroll()
    {
        DataTable dt = dbhelper.getdata("select a.id,LEFT(CONVERT(varchar,a.PayrollDate,101),10)PayrollDate, " +
                                     "left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10) datedtrrange,c.PayrollGroup pg  " +
                                     "from TPayroll a " +
                                     "left join TDTR b on a.DTRId=b.Id  " +
                                     "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                                     "where a.status is null ");
        grid_paylist.DataSource = dt;
        grid_paylist.DataBind();
    
    }
    protected void click_search(object sender, EventArgs e)
    {
        string query="select a.id,LEFT(CONVERT(varchar,a.PayrollDate,101),10)PayrollDate, " +
                                     "left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10) datedtrrange,c.PayrollGroup pg  " +
                                     "from TPayroll a " +
                                     "left join TDTR b on a.DTRId=b.Id  " +
                                     "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                                    "where a.status is null ";
                                    if (int.Parse(ddl_pg.SelectedValue) > 0)
                                        query += " and a.payrollgroupid=" + ddl_pg.SelectedValue + " ";
                                    if (txt_f.Text.Length > 0 && txt_t.Text.Length > 0)
                                        query += " and left(convert(varchar,a.PayrollDate,101),10)>='" + txt_f.Text + "' and left(convert(varchar,a.PayrollDate,101),10) <='" + txt_t.Text + "'";
                                    query += "order by a.id desc";
                        
        DataTable dt = dbhelper.getdata(query);
        grid_paylist.DataSource = dt;
        grid_paylist.DataBind();
    }

    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='payrolldetails?&payid=" + function.Encrypt(row.Cells[0].Text, true) + "'", true);
        }
    }

    protected void click_printbatch(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?key=" + function.Encrypt(dtinsappform.Rows[0]["trnid"].ToString(), true) + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='printablepayslip?payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=batch'", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?printablepayslip?empid=" + function.Encrypt(row.Cells[1].Text, true) + "&payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=single'", true);
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {

                DateTime sd = Convert.ToDateTime(DateTime.Now.ToShortDateString());//DateTime.Now.ToShortDateString()
                DateTime gg = Convert.ToDateTime(row.Cells[1].Text.ToString());
                TimeSpan result = sd - gg;
                if (result.TotalHours==0)
                {
                    dbhelper.getdata("update TPayrollOtherDeduction set payroll_id=NULL where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update TPayrollOtherIncome set payroll_id=NULL where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update tdtr set payroll_id=NULL  where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update TPayroll set  status='" + "Cancelled-" + DateTime.Now.ToShortDateString() + "' where id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update govt_deduction_schedule set  status='cancel' where payrollid=" + row.Cells[0].Text + "");
                    Response.Redirect("procpay", false);
                }
                else
                {
                    Response.Write("<script>alert('Cancellation Denied!')</script>");
                }
            }
            else
            { 
            }
        }
    }
    protected void generatetexfile(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string ggg = "select c.ATMAccountNumber Account_No,a.NetIncome Net_Income " +
           "from TPayrollLine a " +
           "left join TPayroll b on a.PayrollId=b.Id " + 
           "left join MEmployee c on a.EmployeeId=c.Id " +
           "where a.PayrollId=" + row.Cells[0].Text + " and len(c.ATMAccountNumber)>0  and a.NetIncome>0 order by b.PayrollDate desc ";
            DataTable dt = dbhelper.getdata(ggg);

            //Build the Text file data.
            string txt = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                txt += column.ColumnName + "\t";
            }
            txt += "\r\n";
            foreach (DataRow rows in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    txt += rows[column.ColumnName].ToString() + "\t";
                }
                txt += "\r\n";
            }
            string filename =  row.Cells[3].Text.Replace("/", "").Replace(" - ","to")+".txt";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + "");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(txt);
            Response.Flush();
            Response.End();
        }
    }


}