<%@ Page Language="C#" AutoEventWireup="true" CodeFile="quitclaimrequest.aspx.cs" Inherits="content_payroll_quitclaimrequest" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="content_head_income">
<script type="text/javascript" src="script/gridviewpane/gridpane.js"></script>
<script type="text/javascript">
    function ConfirmRem() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to approve this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
    function CheckBoxCount() {
        var gv = document.getElementById("<%= grid_quitclaimrequest.ClientID %>");
        var lbl_del_notify = document.getElementById("<%= lbl_del_notify.ClientID %>");
        var lbl_del_notify_val = document.getElementById('lbl_del_notify');
        var inputList = gv.getElementsByTagName("input");
       

        var numChecked = 0;

        for (var i = 1; i <= inputList.length - 1; i++)
        {
            if (inputList[i].checked)
                numChecked = numChecked + 1;
        }
        lbl_del_notify.textContent = numChecked + " rows";
    }
  </script>
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
            var confirm_value = document.getElementById("<%= TextBox2.ClientID %>")
            if (confirm("Are you sure you want to re-compute 2316?")) {
                confirm_value.value = "Yes"; 
             }
            else { confirm_value.value = "No"; }

            if (confirm_value.value == "Yes") {
                var clickButton = document.getElementById("<%= Button1.ClientID %>");
                clickButton.click();
            }
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
         <div class="x-head">
          <div class="pull-right none">
                        <asp:LinkButton runat="server"  ID="ib_del" CssClass="btn btn-default" ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                        <asp:LinkButton runat="server"  ID="ib_app" CssClass="btn btn-default" ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                        <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-60px; font-size:11px; "></asp:Label>
                    </div>
         </div>
         <br />
         <br />
        <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
        <asp:GridView ID="grid_quitclaimrequest" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found"  CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="empid" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="date"  HeaderText="Entry Date"/>
                <asp:BoundField DataField="empname" HeaderText="Employee Name"/>
                <asp:BoundField DataField="notes" HeaderText="Remarks"/>
                <asp:TemplateField ItemStyle-Width="40px" HeaderText="Action">
                  <ItemTemplate>
                    <asp:LinkButton ID="lnkcan" runat="server" ToolTip="Process"  CssClass="no-padding-right"><i class="fa fa-refresh"></i></asp:LinkButton>
                  </ItemTemplate>
                </asp:TemplateField>
               <%--<asp:TemplateField HeaderText="Attachment">
                    <ItemTemplate>
                       <asp:LinkButton ID="LinkButton3" runat="server" OnClick="view_files" ToolTip="View Clearance File"><i class="fa fa-file-o"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server"  ToolTip="Compute 2316"   OnClick="compute_quit_2316"  ><i class="fa fa-building-o"></i></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server"  ToolTip="Compute Quit Claim" OnClick="compute_quit_claim"  ><i class="fa fa-calculator"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                 <%-- <asp:TemplateField ItemStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  />
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                   </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        </div>
    </div>
</div>
<div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <span>Clearance File</span>
    <hr />
    <div id="img_id" runat="server">
    </div>
   <%-- <asp:Image ID="Image1" runat="server" Height = "500" Width = "500" />--%>
</div>
<div id="div_compute_quit" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Compute Quit Claim</h2>
    <hr />
    <ul class="input-form">
        <li>Unclaimed Salary</li>
        <li><asp:Label ID="lbl_lp" runat="server"></asp:Label></li>
        <li>13 Month Pay</li>
        <li><asp:Label ID="lbl_13monthpay" runat="server"></asp:Label></li>
        <li>Savings</li>
        <li><asp:Label ID="lbl_internalsavings" runat="server"></asp:Label></li>
        <li>Tax Refund</li>
        <li><asp:Label ID="lbl_titr" runat="server"></asp:Label></li>
        <li>Service Incentive Leave</li>
        <li><asp:Label ID="lbl_sil" runat="server"></asp:Label></li>
        <li>Separation Pay</li>
        <li> <asp:TextBox ID="txt_separationfee" AutoComplete="off"  ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:TextBox></li>
        <li>Retirement Pay</li>
        <li><asp:Label ID="lbl_rf" runat="server"></asp:Label></li>
        <li>Total Amount</li>
        <li><asp:Label ID="lbl_total_amt" runat="server"></asp:Label></li>
        <li>Deduction</li>
        <li> <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="IdNumber" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="DateStart" HeaderText="DATE" DataFormatString="{0:MM/dd/yyyy}"  />
                    <asp:BoundField DataField="FullName" HeaderText="NAME"/>
                    <asp:BoundField DataField="OtherDeduction" HeaderText="LOAN" />
                    <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" />
                    <asp:BoundField DataField="interest" HeaderText="Interest Amount" />
                    <asp:BoundField DataField="Balance" HeaderText="BALANCE" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView> </li>
            <li>Tax</li>
            <li>  <asp:Label ID="lbl_tax" runat="server"></asp:Label></li>
    </ul>
    <hr />
    <asp:Button ID="Button7" runat="server" Text="Proccess" CssClass="btn btn-primary" />
</div>
   <asp:TextBox ID="TextBox2" style=" visibility:hidden;"  runat="server"></asp:TextBox>
   <asp:Button ID="Button1" style=" visibility:hidden;" runat="server"  OnClick="textchange"  />
   <asp:HiddenField ID="TextBox1" runat="server" />
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="id" runat="server" />
   <asp:HiddenField ID="idd" runat="server" />
    <asp:HiddenField ID="slect_empid" runat="server" />
    
</asp:Content>