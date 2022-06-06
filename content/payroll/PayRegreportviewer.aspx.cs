using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

public partial class content_payroll_PayRegreportviewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        string query = "select case when RowNumber is null then 0 else RowNumber end RowNumber,EMPLOYEE,case when RowNumber  is null then 'GRAND TOTAL' else DEPARTMENT end DEPARTMENT,sum(OTMEAL)OTMEAL,sum(CONVERT(decimal,replace(mr,',','')))mr,sum(RATE)RATE,TotalTardyLateHours,sum(totallateamt)totallateamt,TotalTardyUndertimeHours, " +
            "sum(totalundertimeamt)totalundertimeamt,absentdays,sum(TotalAbsentAmount)TotalAbsentAmount,t_reg_hrs,sum(t_reg_amt)t_reg_amt,totalleavehrs,sum(totalleavepay)totalleavepay,ot_hrs, " +
            "sum(ot_amt)ot_amt,night_hrs,sum(night_amt)night_amt,tholidayhrs,sum(hd_amt)hd_amt,rd_hrs,sum(rd_amt)rd_amt,sum(case when NET_INCOME >0 then OIT else 0 end) OIT, " +
            "sum(GROSS_INCOME)GROSS_INCOME,sum(SSS_CONTRIBUTION)SSS_CONTRIBUTION,(case when (sum(SSS_MPF)) is null then '0.00' else SUM(SSS_MPF)end) SSS_MPF,sum(PHIC_CONTRIBUTION)PHIC_CONTRIBUTION,sum(HDMF_CONTRIBUTION)HDMF_CONTRIBUTION,sum(WHT)WHT,sum(case when NET_INCOME >0 then OD else 0 end) OD, " +
            "sum(case when NET_INCOME >0 then OINT else 0 end) OINT,sum(NET_INCOME+OTMEAL) NET_INCOME  " +

            "from (select ROW_NUMBER() OVER (Order by a.Id) AS RowNumber,  " +
            "b.LastName+', '+b.FirstName+' '+b.MiddleName EMPLOYEE,c.department DEPARTMENT,case when (select SUM(otmeal) from TDTRLine where EmployeeId = b.Id and DTRId = e.DTRId) is null then '0.00' else(select SUM(otmeal) from TDTRLine where EmployeeId = b.Id and DTRId = e.DTRId) end OTMEAL, " +
            "b.monthlyrate mr, " +
            "case when a.PayrollTypeId='1' then a.payrollrate else a.dailyrate end RATE, " +
            "a.TotalTardyLateHours,	" +
            "a.totallateamt, " +
            "a.TotalTardyUndertimeHours, " +
            "a.totalundertimeamt, " +
            "case when (select SUM(case when [Absent]='True'  then 1 else 0 end + case when [HalfdayAbsent]='True'  then 0.5 else 0 end) from TDTRLine where employeeid=a.EmployeeId and dtrid=e.dtrid) is null then 0 else (select SUM(case when [Absent]='True'  then 1 else 0 end + case when [HalfdayAbsent]='True'  then 0.5 else 0 end) from TDTRLine where employeeid=a.EmployeeId and dtrid=e.dtrid) end as absentdays, " +
            "a.TotalAbsentAmount, " +
            //Regular
            "(select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1')t_reg_hrs, " +
            "(case when a.PayrollTypeId=1 then case when (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1')>0 then  a.payrollrate-(a.totaltardyamount + a.totalabsentamount + totalleavepay) else 0 end else (select SUM(regamt) from TPayrollLineBreakDown where payrolllineid=a.Id) end) t_reg_amt, " + //(select SUM(regamt) from TPayrollLineBreakDown where payrolllineid=a.Id)
            //Leave
            "case when a.totalleavehrs IS null then 0 else a.totalleavehrs end totalleavehrs, " +
            "a.totalleavepay, " +
            //OT
            "(select SUM(regothrs+nightothrs) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_hrs, " +
            "(select SUM(regotamt+nightotamt) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_amt, " +
            //Night
            "(select SUM(nighthrs) from TPayrollLineBreakDown where payrolllineid=a.Id)night_hrs, " +
            "(select SUM(nightpremium) from TPayrollLineBreakDown where payrolllineid=a.Id)night_amt, " +
            //Holiday
            "case when (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20 and hdpremium>0) is null then 0 else (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20 and hdpremium>0) end tholidayhrs, " +
            "(select SUM(hdpremium) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20)hd_amt, " +
            //Resday
            "case when (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20 and hdpremium>0) is null then 0 else (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20 and hdpremium>0) end rd_hrs, " +
            "(select SUM(totalamt) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20)rd_amt, " +

            "(select case when sum(y.taxable_amt) is null then 0 else sum(y.taxable_amt) end from TPayrollOtherIncome z left join TPayrollOtherIncomeLine y on z.id=y.payotherincome_id where z.action is null and z.payroll_id=a.PayrollId and y.Emp_id=a.EmployeeId) OIT, " +
            "a.GrossIncome GROSS_INCOME, " +
            "a.SSSContribution SSS_CONTRIBUTION,(case when (a.mpf_ee) is null then '0.00' else a.mpf_ee end) SSS_MPF, " +
            "a.PHICContribution PHIC_CONTRIBUTION, " +
            "a.HDMFContribution HDMF_CONTRIBUTION, " +
            "a.Tax WHT, " +
            "(select case when SUM(y.amount) is null then 0 else SUM(y.amount) end OD from TPayrollOtherDeduction z left join TPayrollOtherDeductionline y on z.id=y.payotherdeduction_id where z.action is null and y.Emp_id= a.EmployeeId and z.payroll_id= a.PayrollId ) OD, " +
            "(select case when sum(y.nontaxable_amt) is null then 0 else sum(y.nontaxable_amt) end from TPayrollOtherIncome z left join TPayrollOtherIncomeLine y on z.id=y.payotherincome_id where z.action is null and z.payroll_id=a.PayrollId and y.Emp_id=a.EmployeeId) OINT, " +
            "a.NetIncome NET_INCOME " +
            "from TPayrollLine a  " +
            "left join memployee b on a.EmployeeId=b.Id  " +
            "left join mdepartment c on b.departmentid=c.Id " +
            "left join mstore d on b.store_id=d.Id  " +
            "left join TPayroll e on a.payrollid=e.id " +
            "where a.PayrollId=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + ") " +
            "tt  " +
            "group by grouping sets " +
            "( " +
            "	( " +
            "		RowNumber,EMPLOYEE,DEPARTMENT,OTMEAL,RATE,TotalTardyLateHours,totallateamt,TotalTardyUndertimeHours, " +
            "		totalundertimeamt,absentdays,TotalAbsentAmount,t_reg_hrs,t_reg_amt,totalleavehrs,totalleavepay,ot_hrs, " +
            "		ot_amt,night_hrs,night_amt,tholidayhrs,hd_amt,rd_hrs,rd_amt,case when NET_INCOME >0 then OIT else 0 end , " +
            "		GROSS_INCOME,SSS_CONTRIBUTION,PHIC_CONTRIBUTION,HDMF_CONTRIBUTION,WHT,case when NET_INCOME >0 then OD else 0 end , " +
            "		case when NET_INCOME >0 then OINT else 0 end,NET_INCOME " +
            "	), " +
            "	( " +
            "	) " +
            ")  ";

        DataTable dt = dbhelper.getdata(query);
        int dtTE = dt.Rows.Count - 2;



        List<CustomDS.Payreg> data = CustomDS.GetAllPayReg(dt);


        query = "select PB,CB,CBONE,AB," + dtTE + " TE from Signatories";
        DataTable dtsign = dbhelper.getdata(query);
        List<CustomDS.signaturies> datasignatories = CustomDS.GetAllsegnatories(dtsign);


        rv_payreg.ProcessingMode = ProcessingMode.Local;
        rv_payreg.LocalReport.ReportPath = Server.MapPath("~/content/RDLC/Payreg.rdlc");
        ReportDataSource datasource = new ReportDataSource("Payreg", data);
        ReportDataSource datasourcesign = new ReportDataSource("signatories", datasignatories);

        rv_payreg.LocalReport.DataSources.Clear();
        rv_payreg.LocalReport.DataSources.Add(datasource);

        rv_payreg.LocalReport.DataSources.Add(datasourcesign);
        rv_payreg.LocalReport.Refresh();
    }
}