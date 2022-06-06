using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class content_payroll_PAR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {

        string query = "select c.Id, c.IdNumber,c.LastName+', '+c.FirstName+' '+c.MiddleName employee,case when c.Id is null then 'Grand Total' else c.ATMAccountNumber end account,cast(round(a.NetIncome,2)as numeric(12,2)) netpay " +
         "from TPayrollLine a " +
         "left join TPayroll b on a.PayrollId=b.Id " +
         "left join MEmployee c on a.EmployeeId=c.Id " +
         "where a.PayrollId = " + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "  order by c.LastName+', '+c.FirstName+' '+c.MiddleName asc ";
        //"group by grouping sets ((c.Id,c.IdNumber,c.LastName+', '+c.FirstName+' '+c.MiddleName,c.ATMAccountNumber,a.NetIncome),()) ";
        // "order by b.PayrollDate desc ";

        DataTable dt = dbhelper.getdata(query);
        int dtTE = dt.Rows.Count - 1;

        query = "select c.PayrollGroup divission,left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10)pp " +
                "from tpayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "left join MPayrollGroup c on a.PayrollGroupId=c.Id where a.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + " ";
        DataTable dthdft = dbhelper.getdata(query);

        List<CustomDS.PNSdatatable> data = CustomDS.GetAllPNSdatatable(dt);


        query = "select PB,CB,CBONE,AB," + dtTE + " TE,'" + dthdft.Rows[0]["divission"].ToString() + "' division,'" + dthdft.Rows[0]["pp"].ToString() + "' pp from Signatories";
        DataTable dtsign = dbhelper.getdata(query);
        List<CustomDS.signaturiespns> datasignatories = CustomDS.GetAllsegnatoriespns(dtsign);

        rv_PAR.ProcessingMode = ProcessingMode.Local;


        rv_PAR.LocalReport.ReportPath = Server.MapPath("~/content/RDLC/PAR.rdlc");
        ReportDataSource datasource = new ReportDataSource("Content", data);
        ReportDataSource datasourcesign = new ReportDataSource("Signatories", datasignatories);

        rv_PAR.LocalReport.DataSources.Clear();
        rv_PAR.LocalReport.DataSources.Add(datasource);

        rv_PAR.LocalReport.DataSources.Add(datasourcesign);
        rv_PAR.LocalReport.Refresh();
    }
}