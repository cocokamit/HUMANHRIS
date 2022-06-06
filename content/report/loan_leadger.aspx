<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loan_leadger.aspx.cs" Inherits="content_report_loan_leadger" MasterPageFile="~/content/MasterPageNew.master" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
   <title>Movenpick Loan</title>
    <!-- DataTables -->
    <link href="vendors/datatables.net-bs/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="vendors/datatables.net-fixedColumns/css/fixedColumns.bootstrap.min.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        hr{margin:0px 0 8px 0 !important}
        .alert {margin-top:0 !important;  margin-bottom:0 !important}
        .x-head {margin-bottom:8px} 
        .filter .table { margin-bottom:0 !important}
        .filter .table,.filter .table td{border:none}
        .filter .table td label {margin-left:5px}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_leader">
<div class="page-title">
  <div class="title_left">
    <h3>Loan Ledger</h3>
  </div>
</div>
<div class="clearfix"></div>
<div class="row">
<div class="x_panel">
    <asp:DropDownList ID="ddl_employee" runat="server"></asp:DropDownList>
    <asp:DropDownList ID="ddl_loantype" runat="server" ></asp:DropDownList>
    <asp:Button ID="btn_searcha" OnClick="btn_search" runat="server" Text="Search" Height="35px"/>
    <asp:GridView ID="grid_view" EmptyDataText="No Data Found!" runat="server" style=" width:100%;" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
      <Columns>
        <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
        <asp:BoundField DataField="fullname" HeaderText="Employee Name"/>
        <asp:BoundField DataField="DateStart" HeaderText="Date Input" DataFormatString="{0:MM/dd/yyyy}"/>
        <asp:BoundField DataField="LoanNumber" HeaderText="Loan No."/>
        <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" DataFormatString="{0:N4}"/>
        <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
        <asp:TemplateField HeaderText="Details">
          <ItemTemplate>
            <asp:LinkButton ID="lnk_viewe" runat="server" OnClick="view" Text="View"></asp:LinkButton>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
</div>
</div>
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>

