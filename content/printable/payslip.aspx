<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payslip.aspx.cs" Inherits="printable_payslip" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payslip</title>
    <style type="text/css">
        *{margin:0; padding:0; font-family:"Lucida Sans Unicode", "Lucida Grande", sans-serif;}
        .content { min-width:1200px; width:70%; margin:0 auto; font-size:9px }
        .slip {border:1px solid #eee; padding:40px; margin:20px;}
        table { text-align:left; width:100%; margin:10px 0 0; border:none;}
        table th { text-transform:uppercase; font-size:8px;}
        table tr { vertical-align:top;}
        .Grid td,.Grid th {padding:2px 4px; border:none;}
        .dnone{ display:none;}
        .style1
        {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
    <%
        string empidd = empid;

        string query = "select left(convert(varchar,b.pp_from,101),10) pp_from,left(convert(varchar,b.pp_to,101),10) pp_to,left(convert(varchar,b.Payrolldate_1,101),10)Payrolldate_1, a.payrollrate,CONVERT(float,a.totalregularpay)totalregularpay, b.dtrid, a.payrolltypeid,a.id as payrolllineid,a.EmployeeId,case when ((select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId)+a.NetIncome) is null then a.NetIncome else ((select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId)+a.NetIncome)end NetIncome,a.TotalTardyAmount,a.TotalAbsentAmount,a.ssscontribution,a.phiccontribution,a.hdmfcontribution,a.tax,    " +
                                       "b.id,left(convert(varchar,b.PayrollDate,101),10)PayrollDate,(select DATENAME(month,b.PayrollDate))+' '+(case when(select DAY(LEFT(CONVERT(varchar,f.DateStart,101),10)))=11 then '10th' else '25th' end)+' '+(select CONVERT(varchar(10),(select year(b.PayrollDate))))remss, " +
                                       "c.IdNumber,c.FirstName+' '+c.MiddleName+' '+c.LastName as c_name, c.tin,c.sssnumber,c.hdmfnumber,c.phicnumber,c.atmaccountnumber,   " +
                                       "d.PayrollType,case when a.payrolltypeid=1 then c.MonthlyRate else c.DailyRate end rate, " +
                                       "e.TaxCode, " +
                                       "left(convert(varchar,f.DateStart,101),10)DateStart,left(convert(varchar,f.DateEnd,101),10)DateEnd , " +
                                       "g.Department, " +
                                       "h.Position, " +
                                       "case when (select SUM(case when [Absent]='True'  then 1 else 0 end + case when [HalfdayAbsent]='True'  then 0.5 else 0 end) from TDTRLine where employeeid=a.EmployeeId and dtrid=b.dtrid) is null then 0 else (select SUM(case when [Absent]='True'  then 1 else 0 end + case when [HalfdayAbsent]='True'  then 0.5 else 0 end) from TDTRLine where employeeid=a.EmployeeId and dtrid=b.dtrid) end as absentdays," +
                                       "a.TotalOtherIncomeTaxable,a.TotalOtherIncomeNonTaxable,a.taxtable, " +
                                       "a.totalregularpay " +
                                       ",a.totallateamt " +
                                       ",a.totalundertimeamt " +
                                       ",a.totalot " +
                                       ",a.totalrdpay " +
                                       ",a.totalleavepay " +
                                       ",a.totalhdpay " +
                                       ",a.TotalAbsentAmount, " +
                                       "a.TotalTardyLateHours,a.TotalTardyUndertimeHours,case when a.mpf_ee is null then '0.00' else a.mpf_ee end mpf_ee " +
                                       ",i.Company,a.dailyrate,a.payrollrate,c.FixNumberOfHours,a.TotalnetsalaryAmount,a.grossincome,a.netincome, " +
                                       "a.NonTaxableNightShiftDifference_prorated_total_amt,a.NonTaxableAllowance_prorated_total_amt,a.NonTaxableActingCapacityAllowance_fix,a.surge_pay,cast(round(a.DailyRate,12)as numeric(12,2))dailyRate,k.Division2,case when (select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId) is null then '0.00'else(select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId)end otmeal " +
                                       ",c.DateHired "+
                                       "from TPayrollLine a  " +
                                       "left join TPayroll b on a.PayrollId=b.Id  " +
                                       "left join MEmployee c on a.EmployeeId=c.Id  " +
                                       "left join MPayrollType d on a.PayrollTypeId=d.Id  " +
                                       "left join MTaxCode e on a.TaxCodeId=e.Id  " +
                                       "left join TDTR f on b.DTRId=f.Id " +
                                       "left join MDepartment g on c.DepartmentId=g.Id " +
                                       "left join MPosition h on c.PositionId=h.Id " +
                                       "left join MCompany i on c.companyId=i.Id " +
                                       "left join MDivision2 k on c.DivisionId2 = k.id where  ";
                                        if (Request.QueryString["b"].ToString() == "single")
                                            query += "a.EmployeeId=" + empidd + " and b.id=" + payid + " ";
                                        else 
                                            query += "b.id=" + payid + " ";

        System.Data.DataTable dt = dbhelper.getdata(query);
        decimal loop = 1;
        foreach (System.Data.DataRow dr in dt.Rows)
        {
            empidd = dr["EmployeeId"].ToString();
            
            System.Data.DataTable dtotperemp = dbhelper.getdata("select case when (select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId) is null then '0.00'else(select SUM(otmeal)otmeal from TDTRLine where DTRId=b.dtrid and employeeid=a.EmployeeId)end otmeal from TPayrollLine a  left join TPayroll b on a.PayrollId=b.Id  left join MEmployee c on a.EmployeeId=c.Id  left join MPayrollType d on a.PayrollTypeId=d.Id  left join MTaxCode e on a.TaxCodeId=e.Id  left join TDTR f on b.DTRId=f.Id left join MDepartment g on c.DepartmentId=g.Id left join MPosition h on c.PositionId=h.Id left join MCompany i on c.companyId=i.Id left join MDivision2 k on c.DivisionId2 = k.id "
                + "where  b.id=" + payid + " and a.EmployeeId = " + empidd + "");
            
           string tests = "select case when brekdown ='Working  Day' then 'Regular Working Day' else brekdown end brekdown , "+
               "case when " + dr["payrolltypeid"].ToString() + " = 1 then 0 else reghrs end reghrs, "+
                "(case when " + dr["payrolltypeid"].ToString() + " = 1 then  case when brekdown ='Working  Day' then " + Math.Round(decimal.Parse(dr["payrollrate"].ToString()), 2) + " when brekdown = 'Rest Day Special Holiday' then (select regamt from TPayrollLineBreakDown aa where aa.payrolllineid=" + dr["payrolllineid"] + " and aa.brekdown = 'Regular Holiday') "+
                "when hdpremium > 0 and brekdown !='Special Holiday' and brekdown = 'Rest Day Working  Day' " +
                "then ((select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2)))from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and a.DayTypeId = 1 and c.Id = aa.payrolllineid))" +
                "when hdpremium > 0 and brekdown = 'Rest Day Regular Holiday' "+
                "then ((select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2)))from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and a.DayTypeId = 4 and c.Id = aa.payrolllineid))"+
                "when hdpremium > 0 and brekdown = 'Company Holiday' "+
                "then ((select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2)))from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and a.DayTypeId = 6 and c.Id = aa.payrolllineid))" +
                "when hdpremium > 0 and brekdown = 'Double Holiday' "+
                "then ((select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2)))from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and a.DayTypeId = 5 and c.Id = aa.payrolllineid))" +
                " else 0 end else regamt end)regamt, " +
                "case when regamt > 0 then '0.00' else hdpremium end hdpremium, regothrs,regotamt, nighthrs,nightpremium-nightpremium nightpremium, nightothrs,nightotamt from TPayrollLineBreakDown aa where aa.payrolllineid=" + dr["payrolllineid"] + " order by aa.id asc";
            
            System.Data.DataTable dtbreakdown = dbhelper.getdata(tests);
            
            
            string dtreg = "select case when(select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2))) from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and c.Id = aa.payrolllineid) is null then '0.00' else (select SUM(cast(round((RegularHours)*rateperhour,2)as numeric(36,2))) from TDTRLine a left join TPayroll b on a.DTRId = b.DTRId left join TPayrollLine c on b.Id=c.PayrollId where a.DTRId = " + dt.Rows[0]["dtrid"].ToString() + " and a.EmployeeId= " + empidd + " and a.restday='True' and a.RegularHours > 0 and c.Id = aa.payrolllineid)-(hdpremium)end regamt from TPayrollLineBreakDown aa where aa.payrolllineid = " + dr["payrolllineid"] + " and brekdown = 'Rest Day Working  Day'";
            System.Data.DataTable dthdreg = dbhelper.getdata(dtreg);
            
            System.Data.DataTable dttotalbreakdown =dbhelper.getdata("SELECT SUM(regamt)+ SUM(hdpremium)+ SUM(regotamt) + SUM(nightpremium) + SUM(nightotamt) total_labor_inc FROM  " +
                                                    "( " +
                                                    "select   " +
                                                    "case when " + dr["payrolltypeid"].ToString() + " = 1 then  case when brekdown ='Working  Day' then " + Math.Round(decimal.Parse(dr["payrollrate"].ToString()), 2) + " else 0 end else  regamt end regamt,  " +
                                                    "hdpremium,  " +
                                                    "regotamt,  " +
                                                    "nightpremium,  " +
                                                    "nightotamt from TPayrollLineBreakDown  " +
                                                    "where payrolllineid=" + dr["payrolllineid"] + "" +
                                                    ")tt");

            grid_breakdown.DataSource = dtbreakdown;
            grid_breakdown.DataBind();
           
            
            lbl_idnumber.Text = dr["Company"].ToString();
            lbl_empname.Text = dr["c_name"].ToString();
            lbl_payrolltype.Text = dr["taxtable"].ToString();
            lbl_rate.Text =dr["payrolltypeid"].ToString() == "1"? decimal.Parse(dr["rate"].ToString()).ToString("0#,###.00"):"0";
            lbl_start.Text = dr["DateStart"].ToString() + " - " + dr["dateend"].ToString();
            lbl_end.Text = dr["dateend"].ToString();
            lbl_deptpartment.Text = dr["Department"].ToString();
            lbl_position.Text = dr["Position"].ToString();
            lbl_account_no.Text = dr["atmaccountnumber"].ToString();
            lbl_tin_no.Text = dr["tin"].ToString();
            lbl_sss_no.Text = dr["sssnumber"].ToString();
            lbl_phic_no.Text = dr["phicnumber"].ToString();
            lbl_hdmf_no.Text = dr["hdmfnumber"].ToString();
             lbl_dr.Text = dr["dailyRate"].ToString();
              lbl_level.Text = dr["Division2"].ToString();
          
            
            // lbl_paytype.Text = " ("+dr["payrolltype"].ToString()+")";
            lbl_bp_amt.Text = dr["payrolltype"].ToString() == "Daily" ? Math.Round(decimal.Parse(dr["dailyrate"].ToString()), 2).ToString()+" / Day" : Math.Round(decimal.Parse(dr["payrollrate"].ToString()), 2).ToString("#,###.00");
           
            lbl_late.Text = Math.Round(decimal.Parse(dr["totallateamt"].ToString()), 2).ToString("#,###.00");
            lbl_lhrs.Text = " (" + Math.Round(decimal.Parse(dr["TotalTardyLateHours"].ToString()), 2).ToString() + " Hour)";

            lbl_undertime.Text = Math.Round(decimal.Parse(dr["totalundertimeamt"].ToString()), 2).ToString("#,###.00");
            lbl_uhrs.Text = " (" + Math.Round(decimal.Parse(dr["TotalTardyUndertimeHours"].ToString()), 2).ToString() + " Hour)";

            //lbl_absent.Text = Math.Round(decimal.Parse(dr["TotalAbsentAmount"].ToString()), 2).ToString("#,###.00");
            //for nonpunching
            if (dr["Division2"].ToString() == "Officer 3" || dr["Division2"].ToString() == "Officer 4" || dr["Division2"].ToString() == "Executive 1" || dr["Division2"].ToString() == "Executive 2" || dr["Division2"].ToString() == "Executive 3" || dr["Division2"].ToString() == "Executive 4" || dr["Division2"].ToString() == "Executive 5" || dr["Division2"].ToString() == "Executive 6" || dr["Division2"].ToString() == "EVP" || dr["Division2"].ToString() == "President" || dr["Division2"].ToString() == "Top Management" || dr["Division2"].ToString() == "Officer 4C" || dr["Division2"].ToString() == "Officer 3L" || dr["Division2"].ToString() == "Non-punching")
            {
                lbl_absent.Text = ".00";
            }
            else
            {
                if (Convert.ToDateTime(dr["DateHired"].ToString()) > Convert.ToDateTime(dr["DateStart"].ToString()))
                {
                    double dds = (Convert.ToDateTime(dr["DateHired"].ToString()) - Convert.ToDateTime(dr["DateStart"].ToString())).TotalDays;
                    dds = dds < 0 ? 0 : dds;
                    if (dr["grossincome"].ToString() == "0.00000")
                    {
                        lbl_absent.Text = Math.Round(decimal.Parse(dr["TotalAbsentAmount"].ToString()), 2).ToString("#,###.00"); 
                    }
                    else
                    {
                        lbl_absent.Text = ((Math.Round(decimal.Parse(dr["absentdays"].ToString())) - Convert.ToDecimal(dds)) * Math.Round(decimal.Parse(dr["dailyrate"].ToString()), 2)).ToString("#,###.00");
                    }
                }
                else
                {
                    lbl_absent.Text = Math.Round(decimal.Parse(dr["TotalAbsentAmount"].ToString()), 2).ToString("#,###.00");
                }
                //lbl_absent.Text = Math.Round(decimal.Parse(dr["TotalAbsentAmount"].ToString()), 2).ToString("#,###.00");
            }
            lbl_absentdays.Text = " (" + Math.Round((decimal.Parse(dr["absentdays"].ToString()) * decimal.Parse(dr["FixNumberOfHours"].ToString())),2) + " Hour)";

            //lbl_ntiotmeal.Text = Math.Round(decimal.Parse(dr["otmeal"].ToString()), 2).ToString("#,###.00");

            lbl_lpay.Text = Math.Round(decimal.Parse(dr["totalleavepay"].ToString()), 2).ToString("#,###.00");
            lbl_oit.Text = Math.Round(decimal.Parse(dr["TotalOtherIncomeTaxable"].ToString()), 2).ToString("#,###.00");
            //string gp = Math.Round(decimal.Parse(dr["TotalRegularpay"].ToString()) + decimal.Parse(dr["totalot"].ToString())
            //    + decimal.Parse(dr["totalleavepay"].ToString()) + decimal.Parse(dr["totalrdpay"].ToString()) + decimal.Parse(dr["totalhdpay"].ToString())
            //    + decimal.Parse(dr["TotalOtherIncomeTaxable"].ToString()) + decimal.Parse(dr["TotalRegularNightAmount"].ToString()), 2).ToString("#,###.00");
            lbl_gp.Text = decimal.Parse(dr["grossincome"].ToString()).ToString("#,###.00"); 
       
            //lbl_payrate.Text = dr["PayrollType"].ToString() == "Fixed" ? "Monthly Rate" : "Daily Rate";


            //deductions
            lbl_mpf.Text = Math.Round(decimal.Parse(dr["mpf_ee"].ToString()), 2).ToString("#,###.00");
            lbl_deduction_sss.Text = Math.Round(decimal.Parse(dr["ssscontribution"].ToString()), 2).ToString("#,###.00");
            lbl_phil.Text = Math.Round(decimal.Parse(dr["phiccontribution"].ToString()), 2).ToString("#,###.00");
            lbl_hdmf.Text = Math.Round(decimal.Parse(dr["hdmfcontribution"].ToString()), 2).ToString("#,###.00");
            lbl_whtt.Text = Math.Round(decimal.Parse(dr["tax"].ToString()), 2).ToString("#,###.00");

            lbl_ntnsd.Text = Math.Round(decimal.Parse(dr["NonTaxableNightShiftDifference_prorated_total_amt"].ToString()), 2).ToString("#,###.00");
            lbl_nta.Text = Math.Round(decimal.Parse(dr["NonTaxableAllowance_prorated_total_amt"].ToString()), 2).ToString("#,###.00");
            lbl_ntaca.Text = Math.Round(decimal.Parse(dr["NonTaxableActingCapacityAllowance_fix"].ToString()), 2).ToString("#,###.00");
            lbl_sp.Text = Math.Round(decimal.Parse(dr["surge_pay"].ToString()), 2).ToString("#,###.00");
            lbl_pd.Text = dr["remss"].ToString();
            lbl_pp.Text = dr["pp_from"].ToString()+"-"+dr["pp_to"].ToString();
            totaldeductions = decimal.Parse(dr["ssscontribution"].ToString()) + decimal.Parse(dr["phiccontribution"].ToString())
                + decimal.Parse(dr["hdmfcontribution"].ToString()) + decimal.Parse(dr["tax"].ToString())+ decimal.Parse(dr["mpf_ee"].ToString());
           
            string hhh = " select " +
            "case when b.otherdeduction_id = 0 then 'Additional Deduction' else (select otherdeduction from motherdeduction where ID=b.otherdeduction_id )end OtherDeduction, " +
            "convert(decimal(18,2),b.amount) loanamt " +
            "from TPayrollOtherDeduction a " +
            "left join TPayrollOtherDeductionline b on a.id=b.payotherdeduction_id " +
            "left join tpayroll c on a.id=c.payrollotherdeductionid " +
            "where a.action is null and c.id=" + payid + " and b.emp_id=" + dr["EmployeeId"] + "";
            System.Data.DataTable dtotherdeductions = dbhelper.getdata(hhh);


            foreach (DataRow gr in dtotherdeductions.Rows)
            {
                totaldeductions = totaldeductions + decimal.Parse(gr[1].ToString());
            }
        
            
            
            lbl_td.Text = Math.Round(totaldeductions, 2).ToString("#,###.00");
            lbl_oint.Text = Math.Round(decimal.Parse(dr["TotalOtherIncomeNonTaxable"].ToString()), 2).ToString("#,###.00");
            lbl_np.Text = Math.Round(decimal.Parse(dr["netincome"].ToString()), 2).ToString("#,###.00");
            txt_.Text=dr["c_name"].ToString();

             string otherincomequeryti = "select c.OtherIncome,a.taxable_amt Amount from TPayrollOtherIncomeLine a " +
                              "left join TPayrollOtherIncome b on a.PayOtherIncome_id=b.id " +
                              "left join MOtherIncome c on a.OtherIncome_id=c.Id " +
                              "where b.payroll_id=" + payid + " and  a.Emp_id=" + empidd + " and a.taxable_amt<>0";
            string otherincomequerynti = "select c.OtherIncome,a.nontaxable_amt Amount from TPayrollOtherIncomeLine a " +
                               "left join TPayrollOtherIncome b on a.PayOtherIncome_id=b.id " +
                               "left join MOtherIncome c on a.OtherIncome_id=c.Id " +
                               "where b.payroll_id=" + payid + " and  a.Emp_id=" + empidd + " and a.nontaxable_amt>0";

            System.Data.DataTable dtoit = dbhelper.getdata(otherincomequeryti);
            System.Data.DataTable dtointi = dbhelper.getdata(otherincomequerynti);

            string loan = "select c.Otherdeduction, d.Balance,a.Amount,a.balance bal_per_payroll from TPayrollOtherDeductionLine a " +
                            "left join TPayrollOtherDeduction b on a.PayOtherDeduction_id=b.id " +
                            "left join MOtherDeduction c on a.OtherDeduction_id=c.Id " +
                            "left join MEmployeeLoan d on a.loan_id=d.Id " +
                            "where b.payroll_id is not null  " +
                            "and b.action is null " +
                            
                            "and d.Balance is not null and a.Emp_id=" + empidd + " and b.payroll_id=" + payid + "";
            System.Data.DataTable dtloan = dbhelper.getdata(loan);
            
            //string netytd = "select SUM(a.NetIncome) netYTD from TPayrollLine a " +
            //                "left join TPayroll b on a.PayrollId=b.id " +
            //                "where b.status is null and a.EmployeeId=" + empidd + " and b.id<=" + payid + " ";
            //System.Data.DataTable dttest = dbhelper.getdata(netytd);
            
            //YearToDateComputation
            string pyear = "select year(b.PayrollDate)PayrollYr from TPayrollLine a left join TPayroll b on a.PayrollId=b.id" +
                " where b.status is null and a.EmployeeId=" + empidd + " and b.Id = " + payid + " ";
            System.Data.DataTable dtpyear = dbhelper.getdata(pyear);
            
            
            string netytd = "select SUM(a.NetIncome) netYTD from TPayrollLine a left join TPayroll b on a.PayrollId=b.id "+
                "where b.status is null and a.EmployeeId=" + empidd + " and b.id<=" + payid + " and year(b.PayrollDate) = '" + dtpyear.Rows[0]["PayrollYr"] + "' group by year(b.PayrollDate)";
            System.Data.DataTable dtnetytd = dbhelper.getdata(netytd);
            lbl_tn_ytd.Text = Math.Round(decimal.Parse(dtnetytd.Rows[0]["netYTD"].ToString()), 2).ToString("#,###.00");

            string grossytd = "select SUM(a.grossincome) grossYTD from TPayrollLine a " +
                           "left join TPayroll b on a.PayrollId=b.id " +
                           "where b.status is null and a.EmployeeId=" + empidd + " and b.id<=" + payid + " and year(b.PayrollDate) = '" + dtpyear.Rows[0]["PayrollYr"] + "' group by year(b.PayrollDate) ";

            System.Data.DataTable dtgrossytd = dbhelper.getdata(grossytd);
            lbl_gross_ytd.Text = Math.Round(decimal.Parse(dtgrossytd.Rows[0]["grossYTD"].ToString()), 2).ToString("#,###.00");
            
             string tax = "select SUM(a.tax) Tax from TPayrollLine a " +
                           "left join TPayroll b on a.PayrollId=b.id " +
                           "where b.status is null and a.EmployeeId=" + empidd + " and b.id<=" + payid + " and year(b.PayrollDate) = '" + dtpyear.Rows[0]["PayrollYr"] + "' group by year(b.PayrollDate) ";

            System.Data.DataTable dttax = dbhelper.getdata(tax);
             lbl_tax_ytd.Text = Math.Round(decimal.Parse(dttax.Rows[0]["Tax"].ToString()), 2).ToString("#,###.00");
%>

        <div class="slip">
        <div class="navbar nav_title text-center">
        
               <img class="mns" src="dist/img/Human.png" alt="CLI" style="width:130px; height:55px; margin-top:-1px"/> 
            </div>
        <p  style=" color:Red; text-align:center; font-weight:bold;">CONFIDENTIAL DATA: DO NOT LOOK AT THIS SCREEN IF THIS IS NOT YOUR RECORD</p>
        <table>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>Company</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_idnumber" runat="server" Text="Id Number"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Employee Name</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_empname" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Department</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_deptpartment" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Position</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_position" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                       
                    </table>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>Level</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_level" runat="server" Text="Test Level"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Account #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_account_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Monthly Rate</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_rate" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Daily Rate</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_dr" runat="server" Text="Test Daily Rate"></asp:Label></td>
                        </tr>
                        
                       
                    </table>
                </td>
                <td  colspan="2">
                    <table>
                    <tr>
                            <td>TIN #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_tin_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                           <tr>
                            <td>SSS #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_sss_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                           <tr>
                            <td>PHIC. #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_phic_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>HDMF. #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_hdmf_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                    
                    
                    
                       
                        
                        
                        <tr style="display:none;">
                            <td>Basic</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_bp_amt" runat="server"></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td class="style1">DATE END</td>
                            <td class="style1">:</td>
                            <td class="style1"><asp:Label ID="lbl_end" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                       

                       
                    </table>
                </td>
                   <td>
                    <table>
                     <tr >
                            <td>Payroll Frequency</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_payrolltype" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr >
                            <td>DTR Period</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_start" runat="server" Text=""></asp:Label></td>
                        </tr>
                    <tr>
                        <td>Payroll Date</td>
                        <td>:</td>
                        <td><asp:Label ID="lbl_pd" runat="server" ></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Payroll Period</td>
                        <td>:</td>
                        <td><asp:Label ID="lbl_pp" runat="server" ></asp:Label></td>
                    </tr>
                    
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6" style=" border:1px solid #eee; padding:10px; margin:10px;">
                     <asp:GridView ID="grid_breakdown" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="false" CssClass="Grid" >
                        <Columns>
                            <asp:BoundField DataField="brekdown" HeaderText="Break Down"/>
                            <asp:BoundField DataField="reghrs" HeaderText="Regular Hrs"/>
                            <asp:BoundField DataField="regamt" HeaderText="Regular Amt."/>
                            <asp:BoundField DataField="hdpremium" HeaderText="Holiday Premium"/>
                            <asp:BoundField DataField="nighthrs" HeaderText="Night HRS."/>
                            <asp:BoundField DataField="nightpremium" HeaderText="Regular Night Diff."/>
                            <asp:BoundField DataField="regothrs" HeaderText="Regular OT HRS."/>
                            <asp:BoundField DataField="regotamt" HeaderText="Regular OT Amt."/>
                            <asp:BoundField DataField="nightothrs" HeaderText="Night OT HRS."/>
                            <asp:BoundField DataField="nightotamt" HeaderText="Night OT Amt."/>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                            <th>Absent / Tardiness</th>
                            <th></th>
                        </tr>
                        <tr>
                            <td>Late<asp:Label ID="lbl_lhrs" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_late" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Undertime<asp:Label ID="lbl_uhrs" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_undertime" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Absent<asp:Label ID="lbl_absentdays" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_absent" runat="server" ></asp:Label></td>
                        </tr >
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td>Leave w/ Pay</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_lpay" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                     <table>
                       <tr>
                            <th>Other Income(T)</th>
                            <th></th>
                       </tr>
                     
                       <% decimal totalti = 0;
                          foreach (DataRow dr_ti in dtoit.Rows)
                          {%>
                            <tr>
                                <td><%=string.Format("{0:n2}", dr_ti["OtherIncome"].ToString()) %></td>
                                <td>:</td>
                                <td><%=string.Format("{0:n2}", decimal.Parse(dr_ti["Amount"].ToString()))%></td>
                            </tr>
                               <% totalti = totalti + decimal.Parse(dr_ti["Amount"].ToString());
                          } %>
               
                        <tr>
                            <th>Other Income(NT)</th>
                            <th></th>
                        </tr>
                        
                        <%
                        decimal totalnti = 0;
                        foreach (DataRow dr_nti in dtointi.Rows)
                        {%>
                            <tr>
                            <td><%=string.Format("{0:n2}", dr_nti["OtherIncome"].ToString())%></td>
                            <td>:</td>
                            <td><%=string.Format("{0:n2}", decimal.Parse(dr_nti["Amount"].ToString()))%></td>
                            </tr>
                        <% totalnti = totalnti + decimal.Parse(dr_nti["Amount"].ToString());
                        } 
                        
                        decimal totalotm = 0;
                       
                         totalotm = decimal.Parse(dtotperemp.Rows[0]["otmeal"].ToString());
                       
                        if(decimal.Parse(dtotperemp.Rows[0]["otmeal"].ToString())>0)
                        {
                            %>
                            <tr>
                                <td>OT Meal</td>
                                <td>:</td>
                                <td><%=string.Format("{0:n2}", decimal.Parse(dtotperemp.Rows[0]["otmeal"].ToString()))%></td>
                            </tr>
                        <%     }
                    %>
                       
                        
                        
                    </table>
                     <asp:Label ID="lbl_oit" runat="server" style=" display:none"></asp:Label>
                     <asp:Label ID="lbl_sp" runat="server" style=" display:none;"></asp:Label>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <td style=" font-weight:bold;">Gross Pay</td>
                                    <td><asp:Label ID="lbl_gp" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>-</td>
                                    <td>-</td>
                                </tr>
                                <tr>
                                    <th>DEDUCTIONS</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td>SSS Premium</td>
                                    <td><asp:Label ID="lbl_deduction_sss" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>SSS MPF</td>
                                    <td><asp:Label ID="lbl_mpf" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>PHIL. Premium</td>
                                    <td><asp:Label ID="lbl_phil" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>HDMF Premium</td>
                                    <td><asp:Label ID="lbl_hdmf" runat="server" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Withholding Tax</td>
                                    <td><asp:Label ID="lbl_whtt" runat="server" ></asp:Label></td>
                                </tr>
                               
                       </table>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <th>Other Deduction</th>
                                    <th></th>
                                </tr>
                               
                                <%foreach (DataRow drdod in dtotherdeductions.Rows)
                                {%>
                                <tr>
                                    <td><%=string.Format("{0:n2}", drdod["OtherDeduction"].ToString())%></td>
                                    <td><%=string.Format("{0:n2}", drdod["loanamt"].ToString())%></td>
                                </tr>
                            <%} %>
                       </table>
                </td>
                <td colspan="2" style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <th>Loan Balance</th>
                                    <th></th>
                                </tr>
                               
                                <%foreach (DataRow drloan in dtloan.Rows)
                                {%>
                                <tr>
                                    <td><%=string.Format("{0:n2}", drloan["Otherdeduction"].ToString())%></td>
                                    <td><%=string.Format("{0:n2}", drloan["bal_per_payroll"].ToString())%></td>
                                </tr>
                            <%} %>
                       </table>
                </td>
            </tr>
            <tr>
                <td  style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                            <td style=" font-size:8px; font-weight:bold">Total Gross Income (A): <%= Math.Round(decimal.Parse(dthdreg.Rows[0]["regamt"].ToString()) + decimal.Parse(dttotalbreakdown.Rows[0]["total_labor_inc"].ToString()) + decimal.Parse(lbl_lpay.Text) - (decimal.Parse(dr["TotalAbsentAmount"].ToString()) + decimal.Parse(dr["totalundertimeamt"].ToString()) + decimal.Parse(dr["totallateamt"].ToString())),2)%></td>
                        </tr>
                        <tr>
                            <td style=" font-size:8px; font-weight:bold">Total Gross (YTD): <asp:Label ID="lbl_gross_ytd" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td  style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                            <td style=" font-size:8px; font-weight:bold">Total Other Income (B): <%=string.Format("{0:n2}", totalnti + totalti + totalotm)    %></td>
                        </tr>
                    </table>
                </td>
                <td  style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                             <td style=" font-size:8px; font-weight:bold">Total Deduction (C): <asp:Label ID="lbl_td" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td  style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                            <td style=" font-size:8px; font-weight:bold">Net Pay (A + B - C): <asp:Label ID="lbl_np" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td colspan="2"  style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                    <tr>
                            <td style=" font-size:8px; font-weight:bold">Total Tax Witheld (YTD): <asp:Label ID="lbl_tax_ytd" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td style=" font-size:8px; font-weight:bold">Total Net (YTD): <asp:Label ID="lbl_tn_ytd" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2">
                    <table>
                        <tr style=" display:none;">
                            <td>Non Taxable Night Shift Differential</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_ntnsd" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td>Non Taxable Allowance</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_nta" runat="server" ></asp:Label></td>
                        </tr>
                         <tr style=" display:none;">
                            <td>Non Taxable Acting Capacity Allowance</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_ntaca" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td>Other Income (Non Taxable)</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_oint" runat="server" ></asp:Label></td>
                        </tr>
                      
                    </table>
                </td>
            </tr>
        </table>
        <table  style=" display:none;">
            <tr>
                <td>Prepared By:<asp:Label ID="lbl_pb" runat="server" style=" border-bottom:1px solid gray;" ></asp:Label></td>
            </tr>
        </table>
        <br />
        <div style=" text-align:center; ">
             <p>This is an electronically generated report, hence does not require a signature.</p>
             <asp:Label ID="Label1" Text="Recieved By:"  runat="server" style=" display:none;"></asp:Label><asp:Label ID="txt_" runat="server" style=" border:NONE; border-bottom:1px solid gray; display:none;"></asp:Label>
        </div>
        </div>
           <% 
                decimal llll = loop / 2;
                if (!llll.ToString().Contains('.'))
                { %>
                    <h1 style='page-break-before: always; visibility:hidden;'>Movenpick</h1>
                <%}
                    loop = loop + 1;
            } %>
     

  
    </div>



    </form>
</body>
</html>
