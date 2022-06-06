using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;


public partial class content_payroll_otherincomedetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["trid"].ToString(), true);
            loadble();
            this.disp();
        }
        
    }
    protected void loadble()
    {
        string query = "select * from MOtherIncome where action is null and frequencyid=3";
        DataTable dt = dbhelper.getdata(query);

        ddl_type.Items.Clear();
        ddl_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_type.Items.Add(new ListItem(dr["OtherIncome"].ToString(), dr["id"].ToString()));
        }
    }
    protected void disp()
    {
        string query = "select " +
                        "a.id, c.IdNumber+' - '+ c.LastName+', '+c.FirstName e_name, " +
                        "b.Otherincome,a.nontaxable_amt,a.Taxable_amt,a.worked_hrs,a.OtherIncome_id,a.Emp_id " +
                        "from TPayrollotherincomeline a " +
                        "left join MOtherincome b on a.Otherincome_id=b.Id " +
                        "left join MEmployee c on a.Emp_id=c.Id " +
                        "where a.PayOtherincome_id=" + key.Value + " and a.status is null order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        ViewState["OtherIncome"] = dt;
        grid_view.DataSource = dt;
        grid_view.DataBind();
        grid_view.HeaderRow.TableSection = TableRowSection.TableHeader;
        grid_view.UseAccessibleHeader = true;

        DataTable dtchecking = dbhelper.getdata("select * from TPayrollotherincome where id=" + key.Value + " and payroll_id is null");
        btn_submit.Visible = true;
        //if (dtchecking.Rows.Count > 0)
        //    btn_submit.Visible = true;
        //else
        //    btn_submit.Visible = false;

    }
    protected void updateTPayrollotherincomeline(object sender, EventArgs e)
    {
        int ii = 0;
        foreach (GridViewRow gr in grid_view.Rows)
        {
            TextBox txt_nontax_amt = (TextBox)grid_view.Rows[ii].Cells[2].FindControl("txt_nontax_amt");
            TextBox txt_taxt_amt = (TextBox)grid_view.Rows[ii].Cells[3].FindControl("txt_taxt_amt");

            string query = "update TPayrollotherincomeline set taxable_amt='" + txt_taxt_amt.Text + "',nontaxable_amt='" + txt_nontax_amt.Text + "' where id=" + gr.Cells[5].Text + "";
            dbhelper.getdata(query);
            ii++;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='viewdetailsotherincome?trid=" + Request.QueryString["trid"].ToString() + "'", true);
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select convert(varchar,ID,50)+'-'+remarks filename from TPayrollotherincome where id=" + key.Value + "");
        if (dt.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["filename"].ToString() + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                this.disp();
                grid_view.Style.Add("text-transform", "uppercase");
                grid_view.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_view.HeaderRow.Cells)
                {
                    cell.BackColor = grid_view.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_view.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_view.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_view.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid_view.RenderControl(hw);
                string style = @"<style> .textmode { text-transform:uppercase } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
            Response.Write("<script>alert('No Data to be downloaded!')</script>");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void addincome(object sender, EventArgs e)
    {
        DataTable dt = ViewState["OtherIncome"] as DataTable;
        if (lbl_bals.Value.Length > 0)
        {
            DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + lbl_bals.Value + " ");
            DataTable dtincome = getdata.addotherincome(ddl_type.SelectedValue, txt_addinc_amt.Text.Replace(",", ""), dtemp.Rows[0]["FixNumberOfDays"].ToString(), dtemp.Rows[0]["FixNumberOfHours"].ToString(), dtemp.Rows[0]["payrollgroupid"].ToString(), "", lbl_bals.Value);
            DataTable dtchk = dbhelper.getdata("select * from TPayrollotherincomeline a where a.PayOtherincome_id=" + key.Value + " and a.status is null ");
            DataRow[] drchk = dtchk.Select("Emp_id=" + lbl_bals.Value + " and OtherIncome_id=" + ddl_type.SelectedValue + "");
            
            if (drchk.Count() == 0)
            {
                stateclass a = new stateclass();
                a.sa = key.Value;
                a.sb = lbl_bals.Value;//emp id 
                a.sc = ddl_type.SelectedValue;// oi id
                a.sd = dtincome.Rows[0]["amt_to_bepaid"].ToString();// amount to be paid
                a.se = dtincome.Rows[0]["amt_to_be_tax"].ToString();// amount to be tax
                a.sf = dtincome.Rows[0]["total_hours_worked"].ToString();// worked hours
                a.sh = txt_addinc_amt.Text.Replace(",", "");// allowed income
                bol.payotherincomeline(a);
                disp();
            }
            else
                Response.Write("<script>alert('Exist!')</script>");
        }
        else
            Response.Write("<script>alert('Not yet registered!')</script>");
          
    }

    protected void click_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Value == "Yes")
            {
                dbhelper.getdata("update TPayrollOtherIncomeLine set status='Cancelled',lastidupdate=" + Session["user_id"] + ", lastdateupdate=getdate() where id="+row.Cells[5].Text+"");
                disp();
            }
            else { }
        }
    }
}