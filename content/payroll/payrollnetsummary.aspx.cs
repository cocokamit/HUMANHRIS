using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.Reporting.WebForms;

public partial class content_payroll_payrollnetsummary : System.Web.UI.Page
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

        string query = "select id,idnumber,employee YY, case when employee = 'ZZZZZ' then '' else employee end employee ,account,netpay from (select c.Id, c.IdNumber, case when c.Id is null then 'ZZZZZ' else c.LastName+', '+c.FirstName+' '+c.MiddleName end employee,case when c.Id is null then 'Grand Total' else c.ATMAccountNumber end account,(cast(round(sum(a.NetIncome),2)as numeric(12,2)))+(case when (select SUM(otmeal)otmeal from TDTRLine where EmployeeId = c.Id and DTRId = b.DTRId)is null then '0.00' else (select SUM(otmeal)otmeal from TDTRLine where EmployeeId = c.Id and DTRId = b.DTRId)end) netpay " +
         "from TPayrollLine a " +
         "left join TPayroll b on a.PayrollId=b.Id " +
         "left join MEmployee c on a.EmployeeId=c.Id " +
         "where a.PayrollId = " + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "   " +
         "group by grouping sets ((c.Id,c.IdNumber,c.LastName+', '+c.FirstName+' '+c.MiddleName,c.ATMAccountNumber,a.NetIncome,b.DTRId),())) tt  " +
         "order by YY asc ";
  
        DataTable dt = dbhelper.getdata(query);
        int dtTE = dt.Rows.Count - 1;

        query="select c.PayrollGroup divission,left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10)pp " +
                "from tpayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "left join MPayrollGroup c on a.PayrollGroupId=c.Id where a.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + " ";
        DataTable dthdft = dbhelper.getdata(query);

        List<CustomDS.PNSdatatable> data = CustomDS.GetAllPNSdatatable(dt);


        query = "select PB,CB,CBONE,AB," + dtTE + " TE,'" + dthdft.Rows[0]["divission"].ToString() + "' division,'" + dthdft.Rows[0]["pp"].ToString() + "' pp from Signatories";
        DataTable dtsign = dbhelper.getdata(query);
        List<CustomDS.signaturiespns> datasignatories = CustomDS.GetAllsegnatoriespns(dtsign);

        rv_pns.ProcessingMode = ProcessingMode.Local;

       
        rv_pns.LocalReport.ReportPath = Server.MapPath("~/content/RDLC/banktextfile.rdlc");
        ReportDataSource datasource = new ReportDataSource("PNSDataset", data);
        ReportDataSource datasourcesign = new ReportDataSource("signatories", datasignatories);

        rv_pns.LocalReport.DataSources.Clear();
        rv_pns.LocalReport.DataSources.Add(datasource);

        rv_pns.LocalReport.DataSources.Add(datasourcesign);
        rv_pns.LocalReport.Refresh();
    }
}