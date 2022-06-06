<%@ Page Language="C#" AutoEventWireup="true" CodeFile="quitclaimcomputetation.aspx.cs" Inherits="content_payroll_quitclaimcomputetation"  MasterPageFile="~/content/MasterPageNew.master" %>

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
    </style>
      <script src="script/auto/myJScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_other_income">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Quit Claim Request</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
             <div class="x-head"></div>
                <ul class="input-form">
                    <li>Other Deduction</li>
                    <li><asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found" OnRowDataBound="deductionbound"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="IdNumber" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="DateStart" HeaderText="DATE" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField DataField="FullName" HeaderText="NAME"/>
                                <asp:BoundField DataField="OtherDeduction" HeaderText="LOAN" />
                                <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" />
                                <asp:BoundField DataField="interest" HeaderText="Interest Amount" />
                                <asp:BoundField DataField="Balance" HeaderText="BALANCE" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1"  runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView></li>
                        <li>Service Incentive Leave</li>
                        <li><asp:GridView ID="grid_sil" runat="server" EmptyDataText="No record found" OnRowDataBound="silrowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                                <asp:BoundField DataField="LeaveType" HeaderText="LeaveType"/>
                                <asp:BoundField DataField="yearlytotal" HeaderText="Yearly Total"/>
                                <asp:BoundField DataField="leave_bal" HeaderText="Balance"/>
                                <asp:BoundField DataField="leavestatus" HeaderText="Leave Status"/>
                            </Columns>
                        </asp:GridView></li>
                        <li>Computation</li>
                         <li><asp:GridView ID="grid_compute" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                                <asp:BoundField DataField="unclaimsalary" HeaderText="Unclaim Salary"/>
                                <asp:BoundField DataField="13monthpay" HeaderText="13th Month Pay" />
                                <asp:BoundField DataField="saving" HeaderText="Internal Savings" />
                                <asp:BoundField DataField="taxrefund" HeaderText="Tax Refund" />
                                <asp:BoundField DataField="serviceincentiveleave" HeaderText="Service Incentive Leave" />
                                  <asp:TemplateField HeaderText="Separation Pay">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_sp" runat="server" ClientIDMode="Static" onkeyup="decimalinput(this);" Text='<%#bind("separationpay") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="retirementpay" HeaderText="Retirement Pay" />
                                <asp:BoundField DataField="taxpayable" HeaderText="Tax Payable" />
                                <asp:BoundField DataField="deduction" HeaderText="Internal Deduction" />
                               <%-- <asp:BoundField DataField="totalamt" HeaderText="Total Amount" />--%>
                            </Columns>
                        </asp:GridView></li>
                </ul>
                 <asp:Button ID="Button2" runat="server" Text="Process" OnClick="proccess"  CssClass="btn btn-primary" />
     </div>
    </div>
</div>
   <asp:HiddenField ID="TextBox1" runat="server" />
   <asp:HiddenField ID="hdn_total_sil" runat="server" />
   <asp:HiddenField ID="hdn_total_deduction" runat="server" />
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="id" runat="server" />
   <asp:HiddenField ID="idd" runat="server" />
</asp:Content>

