<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sevenone.aspx.cs" Inherits="content_printable_alphalist" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        table { font-size:10px;}
    </style>
   <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
   <script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("[id*=btnExport]").click(function () {
            $("[id*=hfGridHtml]").val($("#Grid").html());
        });
    });
</script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnExport" runat="server" OnClick="click_convert" Text="CONVERT TO EXCEL" />
       
    <div id="Grid" runat="server">
 <% DataTable dtcompany = dbhelper.getdata("select * from mcompany where id="+function.Decrypt(Request.QueryString["kc"].ToString(),true)+"");
       DataTable dtalphalist = getdata.alphalist(" and a.yr=" + function.Decrypt(Request.QueryString["kyy"].ToString(), true) + " and a.class=" + function.Decrypt(Request.QueryString["kclass"].ToString(), true) + " and a.companyid="+function.Decrypt(Request.QueryString["kc"].ToString(),true)+"");
       string classs = function.Decrypt(Request.QueryString["kclass"].ToString(), true);
       int rcnt = 1;
       decimal gci = 0; decimal nt_thirteenmonth = 0; decimal nt_denimis = 0; decimal nt_givtdeduction = 0; decimal nt_otherform = 0; decimal total_nt = 0;
       decimal t_bs = 0; decimal t_thirteenmonth = 0; decimal t_otherform = 0; decimal total_t = 0;
       decimal s_total_taxable_prev = 0; decimal s_wh_prev = 0; decimal s_exempt_amt = 0; decimal s_premiumhealth = 0; decimal s_net_tci = 0; decimal s_taxdue = 0; decimal s_wh_jan_nov = 0; decimal s_wh_dec = 0; decimal s_over_wh = 0; decimal s_adjusted_wh = 0;
       decimal present_twh_december = 0;
       string msg = classs == "1" ? "WITH NO PREVIOUS EMPLOYER" : "WITH PREVIOUS EMPLOYER/S";
       string schedule = classs == "1" ? "7.3" : "7.4";
 
        %>
        <br />
         <table   bordercolor='#000'  
                cellspacing='0' cellpadding='0' style='font-size: 10; text-align:left;'>

            <tr>
                <th colspan="23" style=" text-align:left !important;">
                    BIR FORM 1604CF - SCHEDULE <%Response.Write(schedule); %>																						
                </th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;">
                    ALPHALIST OF EMPLOYEES AS OF DECEMBER 31 <% Response.Write(msg); %>  WITHIN THE YEAR
                </th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;">
                    AS OF DECEMBER <%Response.Write(function.Decrypt(Request.QueryString["kyy"].ToString(),true)); %>
                </th>
            </tr>
            <tr>
                 <th colspan="23" style=" text-align:left !important;"></th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;">T.I.N:<% Response.Write(dtcompany.Rows[0]["tin"].ToString()); %></th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;"></th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;">WITHHOLDING AGENT'S NAME: CARLOS A. GOTHONG LINES INCORPORATED:<% Response.Write(dtcompany.Rows[0]["company"].ToString()); %></th>
            </tr>
          <%--  <tr>
                <th colspan="23" style=" text-align:left !important;"></th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;"></th>
            </tr>
            <tr>
                <th colspan="23" style=" text-align:left !important;"></th>
            </tr>--%>

         </table>
        

        <% if (classs == "1")
           {%>
        <table border='1' bordercolor='#000'
                cellspacing='0' cellpadding='0' style='font-size: 10; text-align:left;'>
        <thead>
           
             <tr>
                <th colspan="3" style=" text-align:left !important;"></th>
                <th colspan="10" style=" text-align:left !important;">(4) G R O S S   C O M P E N S A T I O N   I N C O M E </th>
                <th colspan="10" style=" text-align:left !important;"></th>
            </tr>
            
              <tr>
                <th colspan="3" style=" text-align:left !important;"></th>
                <th style=" text-align:left !important;"></th>
                <th colspan="5" style=" text-align:left !important;">N O N - T A X A B L E</th>
                <th colspan="4" style=" text-align:left !important;">T A X A B L E</th>
                <th colspan="2" style=" text-align:left !important;">E X E M P T I O N</th>
                <th colspan="2" style=" text-align:left !important;"></th>
                <th style=" text-align:left !important;">TAX DUE</th>
                <th style=" text-align:left !important;">TAX WITHHELD</th>
                <th  colspan="2" style=" text-align:left !important;">YEAR-END ADJUSTMENT (10a or 10b)</th>
                <th style=" text-align:left !important;"></th>
                <th style=" text-align:left !important;">SUBSTITUTED FILING?</th>
            </tr>
        </thead>
            <thead>
                <tr>
                    <th style=" text-align:left !important;">SEQ NO</th>
                    <th style=" text-align:left !important;">TAXPAYER IDENTIFICATION NUMBER</th>
                    <th style=" text-align:left !important;">NAME OF EMPLOYEES (Last Name, First Name, Middle Name)</th>
                    <th style=" text-align:left !important;">GROSS COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">13th MONTH PAY & OTHER BENEFITS</th>
                    <th style=" text-align:left !important;">DE MINIMIS BENEFITS</th>
                    <th style=" text-align:left !important;">SSS, GSIS, PHIC & PAG-IBIG CONTRIBUTIONS AND UNION DUES</th>
                    <th style=" text-align:left !important;">SALARIES & OTHER FORMS OF COMPENSATION</th>
                    <th style=" text-align:left !important;">TOTAL NON-TAXABLE COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">BASIC SALARY</th>
                    <th style=" text-align:left !important;">13th MONTH PAY & OTHER BENEFITS</th>
                    <th style=" text-align:left !important;">SALARIES & OTHER FORMS OF COMPENSATION</th>
                    <th style=" text-align:left !important;">TOTAL TAXABLE COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">Code</th>
                    <th style=" text-align:left !important;">Amount</th>
                    <th style=" text-align:left !important;">PREMIUM PAID ON HEALTH AND/OR HOSPITAL INSURANCE</th>
                    <th style=" text-align:left !important;">NET TAXABLE COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">(Jan. - Dec.)</th>
                    <th style=" text-align:left !important;">(Jan. - Nov.)</th>
                    <th style=" text-align:left !important;">AMT WITHHELD & PAID FOR IN DECEMBER</th>
                    <th style=" text-align:left !important;">OVER WITHHELD TAX EMPLOYEE</th>
                    <th style=" text-align:left !important;">AMOUNT OF TAX WITHHELD AS ADJUSTED</th>
                    <th style=" text-align:left !important;">YES/NO</th>
                </tr>
                <tr>
                    <th style=" text-align:left !important;">(1a)</th>
                    <th style=" text-align:left !important;">(2a)</th>
                    <th style=" text-align:left !important;">(3a)</th>
                    <th style=" text-align:left !important;">(4a)</th>
                    <th style=" text-align:left !important;">(4b)</th>
                    <th style=" text-align:left !important;">(4c)</th>
                    <th style=" text-align:left !important;">(4d)</th>
                    <th style=" text-align:left !important;">(4e)</th>
                    <th style=" text-align:left !important;">(4f)</th>
                    <th style=" text-align:left !important;">(4g)</th>
                    <th style=" text-align:left !important;">(4h)</th>
                    <th style=" text-align:left !important;">(4i)</th>
                    <th style=" text-align:left !important;">(4j)</th>
                    <th style=" text-align:left !important;">(5a)</th>
                    <th style=" text-align:left !important;">(5b)</th>
                    <th style=" text-align:left !important;">(6a)</th>
                    <th style=" text-align:left !important;">(7a)</th>
                    <th style=" text-align:left !important;">(8a)</th>
                    <th style=" text-align:left !important;">(9a)</th>
                    <th style=" text-align:left !important;">((10a)=(8)-(9))</th>
                    <th style=" text-align:left !important;">(10b)=(9)-(8)</th>
                    <th style=" text-align:left !important;">(11a)=(9+10a)or(9-10b)</th>
                    <th style=" text-align:left !important;">(12a)</th>
                </tr>
            </thead>
           
            <tbody>
            <%
               
                
            foreach (DataRow drdetails in dtalphalist.Rows)
            {
                present_twh_december = decimal.Parse(drdetails["present_tax_withheld"].ToString()) - decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString());
                    %>
                <tr>
                    <td style=" text-align:left !important;"><%Response.Write(rcnt); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["emp_TIN"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["emp_name"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["present_gci"].ToString()).ToString("#,###,##0.00")); gci = gci + decimal.Parse(drdetails["present_gci"].ToString()); %></td>
                    <%--non taxable--%>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); nt_thirteenmonth = nt_thirteenmonth + decimal.Parse(drdetails["thirteenmonthpay"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["denimis"].ToString()).ToString("#,###,##0.00")); nt_denimis = nt_denimis + decimal.Parse(drdetails["denimis"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["govtdeductionanduniondues"].ToString()).ToString("#,###,##0.00")); nt_givtdeduction = nt_givtdeduction + decimal.Parse(drdetails["govtdeductionanduniondues"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); nt_otherform = nt_otherform + decimal.Parse(drdetails["salariesandotherformofcompensation"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["total_nt"].ToString()).ToString("#,###,##0.00")); total_nt = total_nt + decimal.Parse(drdetails["total_nt"].ToString()); %></td>
                    <%--taxable--%>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_basic_salary"].ToString()).ToString("#,###,##0.00")); t_bs = t_bs + decimal.Parse(drdetails["t_basic_salary"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); t_thirteenmonth = t_thirteenmonth + decimal.Parse(drdetails["t_thirteenmonthpay"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); t_otherform = t_otherform + decimal.Parse(drdetails["t_salariesandotherformofcompensation"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["total_t"].ToString()).ToString("#,###,##0.00")); total_t = total_t + decimal.Parse(drdetails["total_t"].ToString()); %></td>
                    <%--summary--%>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["tax_code_desc"].ToString());  %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["less_total_exemption"].ToString()).ToString("#,###,##0.00")); s_exempt_amt = s_exempt_amt + decimal.Parse(drdetails["less_total_exemption"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["less_premiumpaidonhealth"].ToString()).ToString("#,###,##0.00")); s_premiumhealth = s_premiumhealth + decimal.Parse(drdetails["less_premiumpaidonhealth"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["net_tci"].ToString()).ToString("#,###,##0.00")); s_net_tci = s_net_tci + decimal.Parse(drdetails["net_tci"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["taxdue"].ToString()).ToString("#,###,##0.00")); s_taxdue = s_taxdue + decimal.Parse(drdetails["taxdue"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString()).ToString("#,###,##0.00")); s_wh_jan_nov = s_wh_jan_nov + decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(present_twh_december.ToString("#,###,##0.00")); s_wh_dec = s_wh_dec + present_twh_december; %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["over_tax_withheld"].ToString()).ToString("#,###,##0.00")); s_over_wh = s_over_wh + decimal.Parse(drdetails["over_tax_withheld"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["amt_taxwithheld_as_adjusted"].ToString()).ToString("#,###,##0.00")); s_adjusted_wh = s_adjusted_wh + decimal.Parse(drdetails["amt_taxwithheld_as_adjusted"].ToString()); %></td>
                    <td style=" text-align:left !important;">y</td>
                   
                </tr>
               <% rcnt++;
            } %>
                <tfoot>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td><%Response.Write(gci.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_thirteenmonth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_denimis.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_givtdeduction.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_otherform.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(total_nt.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_bs.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_thirteenmonth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_otherform.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(total_t.ToString("#,###,##0.00")); %></td>
                    <td></td>
                    <td><%Response.Write(s_exempt_amt.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_premiumhealth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_net_tci.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_taxdue.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_wh_jan_nov.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_wh_dec.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_over_wh.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_adjusted_wh.ToString("#,###,##0.00")); %></td>
                    <td></td>
                </tr>
            </tfoot>
            </tbody>

        </table>
        <%}
           else
           { %>
          <table border='1' bordercolor='#000'
                cellspacing='0' cellpadding='0' style='font-size: 10; text-align:left;'>
        <thead>
           
             <tr>
                <th colspan="3" style=" text-align:left !important;"></th>
                <th colspan="10" style=" text-align:left !important;">(4) G R O S S   C O M P E N S A T I O N   I N C O M E </th>
                <th colspan="10" style=" text-align:left !important;"></th>
            </tr>
            
              <tr>
                <th colspan="3" style=" text-align:left !important;"></th>
                <th style=" text-align:left !important;"></th>
                <th colspan="5" style=" text-align:left !important;">N O N - T A X A B L E</th>
                <th colspan="4" style=" text-align:left !important;">T A X A B L E</th>
                <th></th>
                <th colspan="2" style=" text-align:left !important;">E X E M P T I O N</th>
                <th colspan="2" style=" text-align:left !important;"></th>
                <th style=" text-align:left !important;">TAX DUE</th>
                <th colspan="2" style=" text-align:left !important;">TAX WITHHELD</th>
                <th  colspan="2" style=" text-align:left !important;">YEAR-END ADJUSTMENT (10a or 10b)</th>
                <th style=" text-align:left !important;"></th>
          
            </tr>
        </thead>
            <thead>
                <tr>
                    <th style=" text-align:left !important;">SEQ NO</th>
                    <th style=" text-align:left !important;">TAXPAYER IDENTIFICATION NUMBER</th>
                    <th style=" text-align:left !important;">NAME OF EMPLOYEES (Last Name, First Name, Middle Name)</th>
                    <th style=" text-align:left !important;">GROSS COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">13th MONTH PAY & OTHER BENEFITS</th>
                    <th style=" text-align:left !important;">DE MINIMIS BENEFITS</th>
                    <th style=" text-align:left !important;">SSS, GSIS, PHIC & PAG-IBIG CONTRIBUTIONS AND UNION DUES</th>
                    <th style=" text-align:left !important;">SALARIES & OTHER FORMS OF COMPENSATION</th>
                    <th style=" text-align:left !important;">TOTAL NON-TAXABLE COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">BASIC SALARY</th>
                    <th style=" text-align:left !important;">13th MONTH PAY & OTHER BENEFITS</th>
                    <th style=" text-align:left !important;">SALARIES & OTHER FORMS OF COMPENSATION</th>
                    <th style=" text-align:left !important;">TOTAL TAXABLE COMPENSATION INCOME</th>
                   
                    <th style=" text-align:left !important;">TOTAL TAXABLE (PREVIOUS EMPLOYER)</th>


                    <th style=" text-align:left !important;">Code</th>
                    <th style=" text-align:left !important;">Amount</th>
                    <th style=" text-align:left !important;">PREMIUM PAID ON HEALTH AND/OR HOSPITAL INSURANCE</th>
                    <th style=" text-align:left !important;">NET TAXABLE COMPENSATION INCOME</th>
                    <th style=" text-align:left !important;">(Jan. - Dec.)</th>
                    <th style=" text-align:left !important;">(Jan. - Nov.) PRESENT EMPLOYER</th>
                    <th style=" text-align:left !important;"> PREVIOUS EMPLOYER</th>
                    <th style=" text-align:left !important;">AMT WITHHELD & PAID FOR IN DECEMBER</th>
                    <th style=" text-align:left !important;">OVER WITHHELD TAX EMPLOYEE</th>
                    <th style=" text-align:left !important;">AMOUNT OF TAX WITHHELD AS ADJUSTED</th>
                
                </tr>
                <tr>
                    <th style=" text-align:left !important;">(1a)</th>
                    <th style=" text-align:left !important;">(2a)</th>
                    <th style=" text-align:left !important;">(3a)</th>
                    <th style=" text-align:left !important;">(4a)</th>
                    <th style=" text-align:left !important;">(4b)</th>
                    <th style=" text-align:left !important;">(4c)</th>
                    <th style=" text-align:left !important;">(4d)</th>
                    <th style=" text-align:left !important;">(4e)</th>
                    <th style=" text-align:left !important;">(4f)</th>
                    <th style=" text-align:left !important;">(4g)</th>
                    <th style=" text-align:left !important;">(4h)</th>
                    <th style=" text-align:left !important;">(4i)</th>
                    <th style=" text-align:left !important;">(4j)</th>
                     <th style=" text-align:left !important;">(5a)</th>
                    <th style=" text-align:left !important;">(6a)</th>
                    <th style=" text-align:left !important;">(6b)</th>
                    <th style=" text-align:left !important;">(7a)</th>
                    <th style=" text-align:left !important;">(8a)</th>
                    <th style=" text-align:left !important;">(9a)</th>
                    <th style=" text-align:left !important;">(10a)</th>
                    <th style=" text-align:left !important;">(10b)</th>
                    <th style=" text-align:left !important;">(11a)</th>
                    <th style=" text-align:left !important;">(11b)</th>
                    <th style=" text-align:left !important;">(12a)</th>
              
                </tr>
            </thead>
           
            <tbody>
            <%
               
                
            foreach (DataRow drdetails in dtalphalist.Rows)
            {
                present_twh_december = decimal.Parse(drdetails["present_tax_withheld"].ToString()) - decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString());
                    %>
                <tr>
                    <td style=" text-align:left !important;"><%Response.Write(rcnt); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["emp_TIN"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["emp_name"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["present_gci"].ToString()).ToString("#,###,##0.00")); gci = gci + decimal.Parse(drdetails["present_gci"].ToString()); %></td>
                    <%--non taxable--%>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); nt_thirteenmonth = nt_thirteenmonth + decimal.Parse(drdetails["thirteenmonthpay"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["denimis"].ToString()).ToString("#,###,##0.00")); nt_denimis = nt_denimis + decimal.Parse(drdetails["denimis"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["govtdeductionanduniondues"].ToString()).ToString("#,###,##0.00")); nt_givtdeduction = nt_givtdeduction + decimal.Parse(drdetails["govtdeductionanduniondues"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); nt_otherform = nt_otherform + decimal.Parse(drdetails["salariesandotherformofcompensation"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["total_nt"].ToString()).ToString("#,###,##0.00")); total_nt = total_nt + decimal.Parse(drdetails["total_nt"].ToString()); %></td>
                    <%--taxable--%>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_basic_salary"].ToString()).ToString("#,###,##0.00")); t_bs = t_bs + decimal.Parse(drdetails["t_basic_salary"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); t_thirteenmonth = t_thirteenmonth + decimal.Parse(drdetails["t_thirteenmonthpay"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["t_salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); t_otherform = t_otherform + decimal.Parse(drdetails["t_salariesandotherformofcompensation"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["total_t"].ToString()).ToString("#,###,##0.00")); total_t = total_t + decimal.Parse(drdetails["total_t"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["total_prev_tci"].ToString()).ToString("#,###,##0.00")); s_total_taxable_prev = s_total_taxable_prev + decimal.Parse(drdetails["total_prev_tci"].ToString()); %></td>
                    <%--summary--%>
                    <td style=" text-align:left !important;"><%Response.Write(drdetails["tax_code_desc"].ToString());%></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["less_total_exemption"].ToString()).ToString("#,###,##0.00")); s_exempt_amt = s_exempt_amt + decimal.Parse(drdetails["less_total_exemption"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["less_premiumpaidonhealth"].ToString()).ToString("#,###,##0.00")); s_premiumhealth = s_premiumhealth + decimal.Parse(drdetails["less_premiumpaidonhealth"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["net_tci"].ToString()).ToString("#,###,##0.00")); s_net_tci = s_net_tci + decimal.Parse(drdetails["net_tci"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["taxdue"].ToString()).ToString("#,###,##0.00")); s_taxdue = s_taxdue + decimal.Parse(drdetails["taxdue"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString()).ToString("#,###,##0.00")); s_wh_jan_nov = s_wh_jan_nov + decimal.Parse(drdetails["present_tax_withheld_jan_nov"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["prev_tax_withheld"].ToString()).ToString("#,###,##0.00")); s_wh_prev = s_wh_prev + decimal.Parse(drdetails["prev_tax_withheld"].ToString()); %></td>
                    <td style=" text-align:left !important;"><%Response.Write(present_twh_december.ToString("#,###,##0.00")); s_wh_dec = s_wh_dec + present_twh_december; %></td>
                    <td style=" text-align:left !important;"><%Response.Write((decimal.Parse(drdetails["over_tax_withheld"].ToString())>0?decimal.Parse(drdetails["over_tax_withheld"].ToString()):0).ToString("#,###,##0.00")); s_over_wh = s_over_wh + decimal.Parse(drdetails["over_tax_withheld"].ToString())>0?decimal.Parse(drdetails["over_tax_withheld"].ToString()):0; %></td>
                    <td style=" text-align:left !important;"><%Response.Write(decimal.Parse(drdetails["amt_taxwithheld_as_adjusted"].ToString()).ToString("#,###,##0.00")); s_adjusted_wh = s_adjusted_wh + decimal.Parse(drdetails["amt_taxwithheld_as_adjusted"].ToString()); %></td>
                </tr>
               <% rcnt++;
            } %>
                <tfoot>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td><%Response.Write(gci.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_thirteenmonth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_denimis.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_givtdeduction.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(nt_otherform.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(total_nt.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_bs.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_thirteenmonth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(t_otherform.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(total_t.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_total_taxable_prev.ToString("#,###,##0.00")); %></td>
                    <td></td>
                    <td><%Response.Write(s_exempt_amt.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_premiumhealth.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_net_tci.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_taxdue.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_wh_jan_nov.ToString("#,###,##0.00")); %></td>
                     <td><%Response.Write(s_wh_prev.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_wh_dec.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_over_wh.ToString("#,###,##0.00")); %></td>
                    <td><%Response.Write(s_adjusted_wh.ToString("#,###,##0.00")); %></td>
                    <td></td>
                </tr>
            </tfoot>
            </tbody>

        </table>
        <%} %>



    </div>
    </div>
     <asp:HiddenField ID="hfGridHtml" runat="server" />
    </form>
</body>
</html>
