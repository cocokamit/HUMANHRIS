<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payslip.aspx.cs" Inherits="content_Employee_payslip" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
     <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .table {border:0 !important}
        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td { border-top: 1px solid #fff !important;}
    .hiddencol{ display:none;}
    </style>
</asp:Content>
    
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_payslip">
<section class="content-header">
    <h1>Payroll Transaction</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Payslip</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
    <div class="box box-primary">
        <div class="box-header">
            <asp:DropDownList ID="ddl_month" runat="server" OnTextChanged="search" AutoPostBack="true" CssClass="select2" Width="150">
                <asp:ListItem Value="01">January</asp:ListItem>
                <asp:ListItem Value="02">Febuary</asp:ListItem>
                <asp:ListItem Value="03">March</asp:ListItem>
                <asp:ListItem Value="04">April</asp:ListItem>
                <asp:ListItem Value="05">May</asp:ListItem>
                <asp:ListItem Value="06">June</asp:ListItem>
                <asp:ListItem Value="07">July</asp:ListItem>
                <asp:ListItem Value="08">August</asp:ListItem>
                <asp:ListItem Value="09">September</asp:ListItem>
                <asp:ListItem Value="10">October</asp:ListItem>
                <asp:ListItem Value="11">November</asp:ListItem>
                <asp:ListItem Value="12">December</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddl_year" runat="server" OnTextChanged="search" AutoPostBack="true" CssClass="select2" Width="80">
            </asp:DropDownList>
        </div>
        <div class="box-body no-pad-top">
            <div id="div_msg" runat="server" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="EmployeeId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="remss" HeaderText="Payroll Date"/>
                <asp:BoundField DataField="datedtrrange" HeaderText="DTR Range"/>
                <%-- <asp:BoundField DataField="IdNumber" HeaderText="IdNumber"/>
                <asp:BoundField DataField="c_name" HeaderText="Employee Name"/>
                <asp:BoundField DataField="payrolltype" HeaderText="Payroll Type"/>
                <asp:BoundField DataField="taxcode" HeaderText="Tax Code"/>--%>
                <asp:BoundField DataField="netincome" HeaderText="Net Income"/>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_posted" runat="server" CssClass="remove_href" style=" font-size:10px; border:1px; border-radius:5px; padding:3px;" Text='<%#Eval("status_1")%>' ></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Click to view payslip."  Text="view" OnClick="viewpayslip"  style=" margin:3px;" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
            <asp:HiddenField ID="payid" runat="server" />
        </div>
    </div>
    </div>
</div>
</section>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>
</asp:Content>


