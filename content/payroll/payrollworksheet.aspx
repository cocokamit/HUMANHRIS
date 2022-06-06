<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payrollworksheet.aspx.cs" Inherits="content_payroll_payrollworksheet" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
         *{ font-size:12px}
        .hide{ display: none !important;}
        </style>

<%--<script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
           // freaze("grid_view", "container");



            var width = new Array();
            var table = $("table[id*=grid_view]"); //Pass your gridview id here.
            table.find("th").each(function (i) {
                width[i] = $(this).width();
            });
            headerRow = table.find("tr:first");
            headerRow.find("th").each(function (i) {
                $(this).width(width[i]);
            });
            firstRow = table.find("tr:first").next();
            firstRow.find("td").each(function (i) {
                $(this).width(width[i]);
            });
            var header = table.clone();
            header.empty();
            header.append(headerRow);
            //            header.append(firstRow);
            // header.css("width", width);
            header.width(table.width() + 20);
            $("#container").before(header);
            table.find("tr:first td").each(function (i) {
                $(this).width(width[i]);
            });
            //  $("#" + y + "").css("width", "100%");
            $("#container").height(300);
//            $("#container").css("width", "1000");
           $("#container").width(table.width() + 20);
            $("#container").append(table);
        });
    </script>--%>

    

    <link rel="stylesheet" type="text/css" href="../../style/fixedheader/normalize.css"  /> <%--href="style/fixedheader/normalize.css"--%>
		<link rel="stylesheet" type="text/css" href="../../style/fixedheader/demo.css" />
		<link rel="stylesheet" type="text/css" href="../../style/fixedheader/component.css" />

    <style type="text/css">
  .sticky-wrap .sticky-intersect th { background-color:#437cab}
  .sticky-intersect th , .sticky-col tbody tr th {border:1px solid #fff !important} 
    </style>


</head>
<body >
    <form id="form1" runat="server">
    <div>
    <h2>Payroll Summary</h2>
<hr />
<asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
<asp:Button ID="Button1" runat="server" OnClick="search"  Text="Search" />
<asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick = "ExportToExcel"  />
<div style=" overflow:auto;  width:100%; height:500px; margin:5px 5px 100px 5px;">
    
<asp:GridView ID="GridView1" runat="server" ClientIDMode="Static" OnRowDataBound="OnRowDataBoundgrid1" style=" text-transform:uppercase !important" ShowFooter="true" Class="overflow-y" AutoGenerateColumns="true" >
    <Columns>
    </Columns>
    </asp:GridView>
<%--<asp:GridView ID="GridView1" runat="server" ClientIDMode="Static" OnRowDataBound="OnRowDataBoundgrid1" ShowFooter="true" Class="overflow-y" AutoGenerateColumns="false" >
    <Columns>
        <asp:BoundField DataField="Employee" HeaderText="EMPLOYEE" />
        <asp:BoundField DataField="department" HeaderText="Department"/>
           <asp:TemplateField HeaderText="Rate">
            <ItemTemplate>
                <asp:Label ID="lbl_rate" runat="server" Text='<%#Eval("Rate","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_rate" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
  
        <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_reg_hrs" runat="server" Text='<%#Eval("t_reg_hrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_reg_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_reg_amt" runat="server" Text='<%#Eval("t_reg_amt","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_reg_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
  
         <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_leave_hrs" runat="server" Text='<%#Eval("totalleavehrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_leave_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_leave_amt" runat="server" Text='<%#Eval("totalleavepay","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_leave_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_ot_hrs" runat="server" Text='<%#Eval("ot_hrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_ot_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_ot_amt" runat="server" Text='<%#Eval("ot_amt","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_ot_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_night_hrs" runat="server" Text='<%#Eval("night_hrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_night_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_night_amt" runat="server" Text='<%#Eval("night_amt","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_night_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_hd_hrs" runat="server" Text='<%#Eval("tholidayhrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_hd_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_hd_amt" runat="server" Text='<%#Eval("hd_amt","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_hd_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>

       <asp:TemplateField HeaderText="Hours">
            <ItemTemplate>
                <asp:Label ID="lbl_rd_hrs" runat="server" Text='<%#Eval("rd_hrs","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_rd_hrs" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="lbl_rd_amt" runat="server" Text='<%#Eval("rd_amt","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_rd_amt" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>



<asp:TemplateField HeaderText="OTHER INCOME(TAXABLE)">
            <ItemTemplate>
                <asp:Label ID="lbl_oit" runat="server" Text='<%#Eval("TotalOtherIncomeTaxable","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_oit" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
<asp:TemplateField HeaderText="Gross Pay">
            <ItemTemplate>
                <asp:Label ID="lbl_gp" runat="server" Text='<%#Eval("GrossIncome","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_gp" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
    <asp:TemplateField HeaderText="SSS">
            <ItemTemplate>
                <asp:Label ID="lbl_SSS" runat="server" Text='<%#Eval("SSSContribution","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_SSS" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="PHIC">
            <ItemTemplate>
                <asp:Label ID="lbl_PHIC" runat="server" Text='<%#Eval("PHICContribution","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_PHIC" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
    <asp:TemplateField HeaderText="HDMF">
            <ItemTemplate>
                <asp:Label ID="lbl_HDMF" runat="server" Text='<%#Eval("HDMFContribution","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_HDMF" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
    <asp:TemplateField HeaderText="TAX">
            <ItemTemplate>
                <asp:Label ID="lbl_tax" runat="server" Text='<%#Eval("Tax","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_tax" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
  
       <asp:TemplateField HeaderText="NTI">
            <ItemTemplate>
                <asp:Label ID="lbl_nti" runat="server" Text='<%#Eval("TotalOtherIncomeNonTaxable","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_nti" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Net Pay">
            <ItemTemplate>
                <asp:Label ID="lbl_np" runat="server" Text='<%#Eval("NetIncome","{0:n}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbl_f_np" ForeColor="Red" runat="server"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>


    </Columns>
</asp:GridView>--%>
</div>
<%--<asp:GridView ID="grid_view" runat="server" style=" display:none;" ClientIDMode="Static"  OnRowDataBound="OnRowDataBound" Class="overflow-y" ShowFooter="true"    AutoGenerateColumns="false" >
                    <Columns>
                    
                            <asp:BoundField DataField="e_name" HeaderText="EMPLOYEE" />
                            <asp:BoundField DataField="Department" HeaderText="DEPT."/>
                            <asp:BoundField DataField="PayrollType" HeaderText="PAY TYPE"/>
                            <asp:BoundField DataField="Basic" HeaderText="BASIC PAY" />
                            <asp:TemplateField HeaderText="TARDY" >
                                <ItemTemplate>
                                        <asp:Label ID="lbl_tta" runat="server" Text='   <%#Eval("TotalTardyAmount","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_tta" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ABSENT" >
                                <ItemTemplate>
                                        <asp:Label ID="lbl_taa" runat="server" Text='<%#Eval("TotalAbsentAmount","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_taa" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WORKING HRS">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_twh" runat="server" Text='<%#Eval("t_working_hrs","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_twh" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="REG.  PAY">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_tg" runat="server" Text='<%#Eval("totalregularpay","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_tg" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="OT PAY">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_oa" runat="server" Text='<%#Eval("totalot","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_oa" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                       <asp:TemplateField HeaderText="RD PAY">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_ra" runat="server" Text='<%#Eval("totalrdpay","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_ra" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="LEAVE W/ Pay">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_lp" runat="server" Text='<%#Eval("totalleavepay","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_lp" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
               
                    <asp:TemplateField HeaderText="HD PAY">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_hp" runat="server" Text='<%#Eval("totalhdpay","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_hp" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NIGHT PAY">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_na" runat="server" Text='<%#Eval("TotalRegularNightAmount","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_na" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Taxable Income">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_ti" runat="server" Text='<%#Eval("TotalOtherIncomeTaxable","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_ti" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Pay">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_gp" runat="server" Text='<%#Eval("gross_pay","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_gp" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="SSS">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_sc" runat="server" Text='<%#Eval("ssscontribution","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_sc" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                    
                            <asp:TemplateField HeaderText="PHIC.">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_pc" runat="server" Text='<%#Eval("phiccontribution","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_pc" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="HDMF">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_hc" runat="server" Text='<%#Eval("hdmfcontribution","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_hc" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="W/Tax">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_wt" runat="server" Text='<%#Eval("tax","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_wt" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Other Deduction">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_od" runat="server" Text='<%#Eval("other_deduct","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_od" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Non-Tax Income">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_nti" runat="server" Text='<%#Eval("TotalOtherIncomeNonTaxable","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_nti" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Pay">
                                <ItemTemplate>
                                        <asp:Label ID="lbl_netincome" runat="server" Text='<%#Eval("NetIncome","{0:n}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbl_f_netincome" ForeColor="Red" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
   
    </div>
    <script src="style/js/jquery.min.js"></script>
    <script src="style/js/JScript.js"></script>
	<script src="style/js/jquery.stickyheader.js"></script>

    <script type="text/javascript">

//        $('tbody tr td:nth-child(1)').each(function () {
//            var $td = $(this);
//            //$td.html('<a href="#">' + $td.text() + '</a>');
//            $td.replaceWith("<th>" + $td.text() + "</th>")
//        });

    </script>
    </form>
</body>
</html>
