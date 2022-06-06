<%@ Page Language="C#" AutoEventWireup="true" CodeFile="proccesspayroll.aspx.cs" Inherits="content_payroll_proccesspayroll" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function($) {
        $(function() {
            $(".datee").datepicker({ changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true
            });
        });
    })(jQuery);
    </script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Payroll</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"></asp:DropDownList>
                <asp:TextBox ID="txt_f" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_t" cssclass="datee" runat="server" placeholder="To"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server"  OnClick="click_search"  Text="Search" CssClass="btn btn-primary"/>
                <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_dtr" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            <asp:GridView ID="grid_paylist" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="PayrollDate" HeaderText="Payroll Date"/>
                    <asp:BoundField DataField="pg" HeaderText="Payroll Group"/>
                    <asp:BoundField DataField="datedtrrange" HeaderText="Payroll Range"/>
                    <asp:TemplateField HeaderText="Action" >
                        <ItemTemplate>
                            <a href="payrolldetails?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Details" target="_new" style=" font-size:14px" ><i class="fa fa-list"></i></a>
                            <a href="printablepayslip?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>&b=batch" target="_new" title="Print Slip" style="font-size:15px"><i class="fa fa-print"></i></a>
                            <a href="payworksheet?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Payroll Registered" target="_new" style=" font-size:14px" ><i class="fa fa-line-chart"></i></a>
                            <%--<a href="requestbudget?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Request Budget" target="_new" style=" font-size:14px" ><i class="fa fa-line-chart"></i></a>--%>
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Bank Text File" OnClick="generatetexfile" Font-Size="14px"><i class="fa fa-file-text-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Cancel Payroll"  Text="can" OnClick="click_cancel" OnClientClick="Confirm()" Font-Size="16px" ><i class="fa fa-trash" style="padding-right:0 "></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="180px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
</asp:Content>