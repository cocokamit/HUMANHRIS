using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;

public partial class content_payroll_scdetails : System.Web.UI.Page
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
       // string query = "select b.LastName+', '+b.FirstName+' '+b.MiddleName employee,a.grossamt,a.wht,a.net from sc_trn_details a left join memployee b on a.empid=b.Id where b.LastName+', '+b.FirstName+' '+b.MiddleName like'%"+txt_search.Text+"%' and  a.sctrnid=" + function.Decrypt(Request.QueryString["sctrnid"].ToString(), true) + "";



        string query = "select a.empid, a.detid,a.sctrnid,b.LastName+', '+b.FirstName+' '+b.MiddleName employee,c.department,d.position,a.grossamt,a.wht,a.net,a.manhr from sc_trn_details a  " +
        "left join memployee b on a.empid=b.Id " +
             "left join MDepartment c on b.departmentid=c.Id " +
             "left join Mposition d on b.positionid=d.Id " +
            "where b.LastName+', '+b.FirstName+' '+b.MiddleName like'%" + txt_search.Text + "%' and  a.sctrnid=" + function.Decrypt(Request.QueryString["sctrnid"].ToString(), true) + "";
        DataTable dt = dbhelper.getdata(query);
        grid_item.DataSource = dt;
        grid_item.DataBind();
    }
    protected void clicksearch(object sender, EventArgs e)
    {
        disp();
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string query = "select 'T-'+convert(varchar,a.sctrnid)+'-'+convert(varchar,CONVERT(date,a.dfrom))+'-'+convert(varchar,CONVERT(date,a.dtoo))filename from sc_transaction a where a.sctrnid=" + function.Decrypt(Request.QueryString["sctrnid"].ToString(), true) + "";

        DataTable dt = dbhelper.getdata(query);
        if (dt.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["filename"].ToString().Replace(" ", "") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grid_item.AllowPaging = false;
                this.disp();


                grid_item.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_item.HeaderRow.Cells)
                {
                    cell.BackColor = grid_item.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_item.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_item.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_item.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid_item.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
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
}