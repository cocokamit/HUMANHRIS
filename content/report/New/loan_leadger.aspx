<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loan_leadger.aspx.cs" EnableEventValidation="false" Inherits="content_report_New_loan_leadger" MasterPageFile="~/content/MasterPageNew.master"%>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
   <title>HRIS Loan</title>
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
    <asp:DropDownList ID="ddl_employee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selectchange" ></asp:DropDownList>
    <asp:DropDownList ID="ddl_loantype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selectchange" ></asp:DropDownList>
    <asp:LinkButton ID="Button2" Text="Export" runat="server" OnClick="generatereport" CssClass="right add"></asp:LinkButton>
    <asp:GridView ID="grid_view" EmptyDataText="No Data Found!" runat="server" style=" width:100%;" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
      <Columns>
        <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
        <asp:BoundField DataField="IdNumber" HeaderText="Employee ID"/>
        <asp:BoundField DataField="Employee" HeaderText="Employee Name"/>
        <asp:BoundField DataField="DateStart" HeaderText="Date Input" DataFormatString="{0:MM/dd/yyyy}"/>
        <asp:BoundField DataField="OtherDeduction" HeaderText="LOAN TYPE"/>
        <asp:BoundField DataField="LoanNumber" HeaderText="LOAN ACCOUNT"/>
        <asp:BoundField DataField="balance" HeaderText="OUTSTANDING BALANCE" DataFormatString="{0:N4}"/>
        <asp:BoundField DataField="Amount" HeaderText="TOTAL AMOUNT DUE" DataFormatString="{0:N4}"/>
        <asp:BoundField DataField="LoanAmount" HeaderText="AMOUNT TO BE PAID" DataFormatString="{0:N4}"/>
        <asp:TemplateField HeaderText="Details">
          <ItemTemplate>
            <asp:LinkButton ID="lnk_viewe" runat="server" OnClick="view" Text="View"></asp:LinkButton>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
    <asp:GridView ID="gridsss" EmptyDataText="No Data Found!" runat="server" style=" width:100%;" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
      <Columns>
        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
        <asp:BoundField DataField="IdNumber" HeaderText="Employee Number"/>
        <asp:BoundField DataField="SSSNumber" HeaderText="SSS Number"/>
        <asp:BoundField DataField="Employee" HeaderText="Name of Borrower"/>
        <asp:BoundField DataField="OtherDeduction" HeaderText="LOAN TYPE"/>
        <asp:BoundField DataField="LoanNumber" HeaderText="LOAN ACCOUNT"/>
        <asp:BoundField DataField="loandate" HeaderText="LOAN DATE" DataFormatString="{0:MM/dd/yyyy}"/>
        <asp:BoundField DataField="balance" HeaderText="OUTSTANDING BALANCE" DataFormatString="{0:N2}"/>
        <asp:BoundField DataField="Amount" HeaderText="TOTAL AMOUNT DUE" DataFormatString="{0:N2}"/>
        <asp:BoundField DataField="LoanAmount" HeaderText="AMOUNT TO BE PAID" DataFormatString="{0:N2}"/>
        <asp:TemplateField HeaderText="Details" Visible="false">
          <ItemTemplate>
            <asp:LinkButton ID="lnk_viewe" runat="server" OnClick="view" Text="View"></asp:LinkButton>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>

    <asp:GridView ID="gridhdmf" EmptyDataText="No Data Found!" runat="server" style=" width:100%;" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
      <Columns>
        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
        <asp:BoundField DataField="HDMFNumber" HeaderText="Pag-IBIG ID"/>
        <asp:BoundField DataField="IdNumber" HeaderText="Employee ID"/>
        <asp:BoundField DataField="LastName" HeaderText="LastName"/>
        <asp:BoundField DataField="FirstName" HeaderText="FirstName"/>
        <asp:BoundField DataField="MiddleName" HeaderText="MiddleName"/>
        <asp:BoundField DataField="HDMFContribution" HeaderText="Employee Contribution"/>
        <asp:BoundField DataField="HDMFContribution" HeaderText="Employer Contribution"/>
        <asp:BoundField DataField="TIN" HeaderText="TIN" />
        <asp:BoundField DataField="Birthdate" HeaderText="Birth Date" />
      </Columns>
    </asp:GridView>

</div>
</div>
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!-- DataTables -->
<script type="text/javascript" src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script type="text/javascript"src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/fixedcolumns/3.2.6/js/dataTables.fixedColumns.min.js"></script>

<!-- DataTables -->
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.flash.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.print.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('#grid_view').DataTable({
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            dom: 'Bfrtip',
            buttons: ['excel', 'print'], //'copy', 'csv', 'excel', 'pdf', 'print'
            scrollY: $(window).height() - 240,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            fixedColumns: {
                leftColumns: 2
            }
        })
    })
</script>
</asp:Content>


