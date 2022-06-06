<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2316.aspx.cs" Inherits="content_payroll_2316" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="content_head_income">
    <script type="text/javascript" src="script/gridviewpane/gridpane.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=imgOrdersShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>
    <style type="text/css">
        .table-bordered tbody > tr > td{ vertical-align:top}
         .hiddencol { display: none; }
         body {
                color: black;
                font-family:Arial;
                font-size: 12px;
         }
   
    </style>
      <script src="script/auto/myJScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_other_income">
    <div class="title_left">
        <h4>Process 2316</h4>
    </div>  
    <div class="title_right">
       <ul>
     <%--//  2316process?user_id="+function.Encrypt(Session["user_id"].ToString(),true)--%>
        <li><a href="quitclaim?user_id=<% Response.Write(function.Encrypt(Session["user_id"].ToString(), true)); %>"><i class="fa fa fa-dashboard"></i>Process Quit Claim</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Dashboard</li>
       </ul>
    </div>  
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-6">
        <div class="x_panel">
             <div class="x-head">
             </div>
             <table>
             <tr>
              <th colspan="2">1 For the period</th>
             </tr>
                 <tr>
                 <td>Year (YYYY)</td>
                     <td><asp:TextBox ID="txt_fortheyear" runat="server"></asp:TextBox></td>
                 </tr>
                 <tr>
                     <th colspan="2">Part I Employee Information</th>
                    
                 </tr>
                 <tr>
                     <td>3 Taxpayer Identification No.</td>
                     <td><asp:TextBox ID="txt_emptin" runat="server" ></asp:TextBox></td>
                 </tr>
                <tr>
                     <th>4 Employee's Name(Last Name,First Name, Middle Name)</th>
                     <th>5 RDO Code</th>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txt_empname" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_rdocode" runat="server"></asp:TextBox></td>
                </tr>
              
                <tr>
                    <th colspan="2">9 Exemption Status</th>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txt_es_single" Width="20px" Enabled="false" runat="server"></asp:TextBox>Single</td>
                    <td><asp:TextBox ID="txt_es_married" runat="server" Enabled="false" Width="20px"></asp:TextBox>Married</td>
                </tr>
                <tr>
                    <th colspan="2">Part II Employer Information (Present)</th>
                </tr>
                <tr>
                     <td>15 Taxpayer Identification No.</td>
                     <td><asp:TextBox ID="txt_tin_employer_present" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                     <th colspan="2">16 Employer's Name</th>
                </tr>
                <tr>
                    <td colspan="2"><asp:TextBox ID="txt_employername" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                     <th>17 Registered Address</th>
                     <th>17A Zip Code</th>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txt_empregadd" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_empregaddzipcode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2">Part III Employer Information (Previous)</th>
                </tr>
                <tr>
                     <td>18 Taxpayer Identification No.</td>
                     <td><asp:TextBox ID="txt_tin_employer_previous" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2">19 Employer's Name</th>
                </tr>
                <tr>
                    <td colspan="2"><asp:TextBox ID="txt_employer_previous" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                   <th>20 Registered Address</th>
                   <th>20A Zip Code</th>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txt_regadd_employer_previous" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_regzipcode_employer_previous" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2">Part IV-A Summary</th>
                </tr>
                <tr>
                    <td>21 Gross Compensation Income from Present Employer(item 41 plus item 55)</td>
                    <td><asp:TextBox ID="txt_gci_present_employer" onfocus="this.blur()" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>22 Less: Total Non - Taxable / Exempt(item 41)</td>
                   <td><asp:TextBox ID="txt_ltnt" runat="server" ClientIDMode="Static" onfocus="this.blur()"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>23 Taxable Compensation Income from Present Employer(item 55)</td>
                    <td><asp:TextBox ID="txt_tci_present_employer" onfocus="this.blur()" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                <td>24 Add: Taxable Compensation Income from Previous Employer</td>
                 <td><asp:TextBox ID="txt_tci_previous_employer" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"  runat="server"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>25 Gross Income Compensation Income</td>
                 <td><asp:TextBox ID="txt_gici" runat="server"  ClientIDMode="Static" onfocus="this.blur()"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>26 Less: Total Exemptions</td>
                 <td><asp:TextBox ID="txt_total_exemptions"  AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();" runat="server"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>27 Less: Premium Paid on Health</td>
                 <td><asp:TextBox ID="txt_ppoh" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>28 Net Taxable Compensation Income</td>
                 <td><asp:TextBox ID="txt_ntci" runat="server" ClientIDMode="Static" onfocus="this.blur()"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>29 Tax Due <asp:Button ID="Button1" runat="server" OnClick="click_process_gettaxdue" Text="Compute" /></td>
                 <td><asp:TextBox ID="txt_taxdue" runat="server" onfocus="this.blur()"></asp:TextBox></td>
                </tr>
                <tr><th colspan="2">30 Amount of Taxes Witheld</th></tr>
                <tr>
                <td>30A Present Employer</td>
                <td><asp:TextBox ID="txt_aotw_present_employer" onfocus="this.blur()" runat="server"></asp:TextBox></td>
                </tr>
                 <tr>
                <td>30B Previous Employer</td>
                 <td><asp:TextBox ID="txt_aotw_previous_employer" runat="server" AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></td>
                </tr>
                 <tr>
                 <td>31 Total amount of taxes Withheld As Udjusted</td>
                 <td><asp:TextBox ID="txt_taotwau" onfocus="this.blur()" runat="server"></asp:TextBox></td>
                </tr>

                 <tr>
                 <td>Present Employer/ Authorized Agent Signature Over Printed Name</td>
                 <td><asp:TextBox ID="txt_peaasopn" style=" text-transform:uppercase;"  runat="server"></asp:TextBox></td>
                </tr>
             </table>
          
        </div>
    </div>
     <div class="col-md-6">
        <div class="x_panel">
             <div class="x-head">
             </div>
             <table>
                <tr>
                    <th colspan="2">2 For the period</th>
                </tr>
                <tr>
                    <td>From (MM/DD)</td>
                    <td>To (MM/DD)</td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txt_period_from_mmdd" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_period_to_mmdd" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2">Part IV-B   Details of Compensation Income and Tax Withheld from Present Employer</th>
                </tr>
                <tr>
                    <th colspan="2"><BR />A. NON-TAXABLE/EXEMPT COMPENSATION INCOME</th>
                </tr>
                <tr>
                    <td>32 Basic Salary/Statutory Minimum Wage Minimum Wage Earner(MWE)</td>
                    <td><asp:TextBox ID="txt_basic_salary_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>33 Holiday Pay(MWE)</td>
                    <td><asp:TextBox ID="txt_holiday_pay_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"  ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>34 Overtime Pay(MWE)</td>
                    <td><asp:TextBox ID="txt_overtime_pay_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>35 Night Shift Differntial(MWE)</td>
                    <td><asp:TextBox ID="txt_nightshiftdif_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>36 Hazard Pay(MWE)</td>
                    <td><asp:TextBox ID="txt_hazard_pay_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>37 13th Month Pay(MWE)</td>
                    <td><asp:TextBox ID="txt_thirtenmonth_pay_mwe" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>38 De Minimis Benefits</td>
                    <td><asp:TextBox ID="txt_dmb" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>39 SSS,GSIS,PHIC & Pag-ibig Contributions, & Union Dues(Employee share only)</td>
                    <td><asp:TextBox ID="txt_goverment_deduction" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                    <td>40 Salaries & Other Forms of Compensation</td>
                    <td><asp:TextBox ID="txt_sofoc" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>41 Total Non-Taxable/Exempt Compensation Income</td>
                    <td><asp:TextBox ID="txt_non_tax" runat="server" onfocus="this.blur()" ClientIDMode="Static"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2"><BR />B. NON-TAXABLE/EXEMPT COMPENSATION INCOME</th>
                </tr>
                <tr>
                <th colspan="2">Regular</th>
                </tr>
                <tr>
                    <td>42 Basic Pay</td>
                    <td><asp:TextBox ID="txt_basic_pay_taxable" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>43 Representation</td>
                    <td><asp:TextBox ID="txt_repasentation_taxable" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>44 Transportation</td>
                    <td><asp:TextBox ID="txt_transportation_taxable" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>45 Cost of Living Allowance</td>
                    <td><asp:TextBox ID="txt_cola" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>46 Fixed Housing Allowance</td>
                    <td><asp:TextBox ID="txt_fha" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                    <th colspan="2">47 Others(Specify)</th>
                </tr>
                 <tr>
                    <td>47A <asp:TextBox ID="txt_label_others_a_regular" runat="server" ></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_value_others_a_regular" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>47B <asp:TextBox ID="txt_label_others_b_regular" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_value_others_b_regular" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                 <tr>
                    <th colspan="2">Suplementary</th>
                </tr>
                <tr>
                     <td>48 Commission</td>
                     <td><asp:TextBox ID="txt_commission" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <td>49 Profit Sharing</td>
                     <td><asp:TextBox ID="txt_pf" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <td>50 Fees Including Director's Fees</td>
                     <td><asp:TextBox ID="txt_fidf" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <td>51 Taxable 13 Month Pay and Other Benefits</td>
                     <td><asp:TextBox ID="txt_taxable_13monthpay_others" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <td>52 Hazard Pay</td>
                     <td><asp:TextBox ID="txt_taxable_hazard_pay" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                   <tr>
                     <td>53 Overtime Pay</td>
                     <td><asp:TextBox ID="txt_taxable_overtime_pay" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <th colspan="2">54 Others(specify)</th>
                </tr>
                 <tr>
                    <td>54A <asp:TextBox ID="txt_label_others_a_supplemen" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_value_others_a_supplemen" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
               
                </tr>
                <tr>
                    <td>54B <asp:TextBox ID="txt_label_others_b_supplemen" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txt_value_others_b_supplemen" runat="server" AutoComplete="off" ClientIDMode="Static" onfocus="javascript:this.select();" onkeyup="ifepty(this);decimalinput(this);changeitramt();"></asp:TextBox></td>
                </tr>
                <tr>
                     <td>55 Total Taxable Compensation Income</td>
                     <td><asp:TextBox ID="txt_total_taxable_compensation_income" onfocus="this.blur()" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                </tr>
             </table>
        </div>
         <asp:Button ID="Button2" runat="server" Text="Save" OnClick="process_itr" CssClass="btn btn-primary" />
    </div>
</div>
<asp:HiddenField ID="TextBox1" runat="server" />
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="ntci" runat="server" />
<asp:HiddenField ID="hdn_btn_com_tax" runat="server" />
</asp:Content>
