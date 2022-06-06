using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_printable_request4budget : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {
        string query = "select " +
                        "c.IdNumber,c.LastName+', '+c.FirstName+' '+c.MiddleName as c_name,i.Company,i.Address, g.Department,h.Position,left(convert(varchar,f.DateStart,101),10)DateStart,left(convert(varchar,f.DateEnd,101),10)DateEnd ,  " +
                        "a.totalregularpay " +
                        ",a.totallateamt " +
                        ",a.totalundertimeamt " +
                        ",a.totalot " +
                        ",a.totalrdpay " +
                        ",a.totalleavepay " +
                        ",a.totalhdpay " +
                        ",a.TotalAbsentAmount, " +
                        "a.NetIncome " +
                        "from TPayrollLine a " +  
                        "left join TPayroll b on a.PayrollId=b.Id   " +
                        "left join MEmployee c on a.EmployeeId=c.Id  " +
                        "left join MPayrollType d on a.PayrollTypeId=d.Id  " +
                        "left join MTaxCode e on a.TaxCodeId=e.Id  " +
                        "left join TDTR f on b.DTRId=f.Id " +
                        "left join MDepartment g on c.DepartmentId=g.Id " +
                        "left join MPosition h on c.PositionId=h.Id " +
                        "left join MCompany i on c.companyId=i.Id " +
                        "where a.PAYROLLID="+ function.Decrypt(Request.QueryString["payid"],true)+" " +
                        "order by i.Company,g.Department,h.Position";
        DataTable dt = dbhelper.getdata(query);
        lbl_comapany.Text=dt.Rows[0]["company"].ToString();
        lbl_compadress.Text = dt.Rows[0]["Address"].ToString();
        lbl_period.Text = dt.Rows[0]["DateStart"].ToString() + " - " + dt.Rows[0]["DateEnd"].ToString();
       
        grid_item.DataSource = dt;
        grid_item.DataBind();
    }
    decimal tni = 0;
    decimal tot = 0;
    decimal trdpay = 0;
    decimal tleavepay = 0;
    decimal thdpay = 0;
    protected void rowdatabound(object sndr, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_NetIncome = (Label)e.Row.FindControl("lbl_NetIncome");
            Label lbl_totalot = (Label)e.Row.FindControl("lbl_totalot");
            Label lbl_totalrdpay = (Label)e.Row.FindControl("lbl_totalrdpay");
            Label lbl_totalleavepay = (Label)e.Row.FindControl("lbl_totalleavepay");
            Label lbl_totalhdpay = (Label)e.Row.FindControl("lbl_totalhdpay");
            tni +=Math.Round(decimal.Parse(lbl_NetIncome.Text),2);
            tot += Math.Round(decimal.Parse(lbl_totalot.Text), 2);
            trdpay += Math.Round(decimal.Parse(lbl_totalrdpay.Text), 2);
            tleavepay += Math.Round(decimal.Parse(lbl_totalleavepay.Text), 2);
            thdpay += Math.Round(decimal.Parse(lbl_totalhdpay.Text), 2);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbl_footerNetIncome = (Label)e.Row.FindControl("lbl_footerNetIncome");
            Label lbl_footertotalot = (Label)e.Row.FindControl("lbl_footertotalot");
            Label lbl_footertotalrdpay = (Label)e.Row.FindControl("lbl_footertotalrdpay");
            Label lbl_footerleavepay = (Label)e.Row.FindControl("lbl_footertotalleavepay");
            Label lbl_footerhdpay = (Label)e.Row.FindControl("lbl_footertotalhdpay");
            lbl_footerNetIncome.Text = tni.ToString();
            lbl_footertotalot.Text=tot.ToString();
            lbl_footertotalrdpay.Text = trdpay.ToString();
            lbl_footerleavepay.Text = tleavepay.ToString();
            lbl_footerhdpay.Text = thdpay.ToString();
        }
    }

}