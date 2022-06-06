<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSSreport.aspx.cs" Inherits="content_report_SSSreport" MasterPageFile="~/content/MasterPageNew.master"  %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
   <title>Cebu LandMasters SSS</title>
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
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_attReport">
<div class="page-title">
  <div class="title_left">
    <h3>SSS Report</h3>
  </div>
</div>
    <div class="clearfix"></div>
    <div class="row">
         <div class="x_panel">
            <asp:DropDownList ID="ddl_month" runat="server" OnTextChanged="search" AutoPostBack="true">
                <asp:ListItem Value="0">Select Month</asp:ListItem>
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
            <asp:DropDownList ID="ddl_year" runat="server" OnTextChanged="search" AutoPostBack="true">
            </asp:DropDownList>
            <div id="alert" runat="server" class="alert alert-default no-margin">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div> 

            <% if(ViewState["data"] != null) {%>
        <table id="tbl" class="table table-striped table-bordered nowrap ">
            <thead>
                <tr>
                    <% DataTable dtColumns = (DataTable)ViewState["data"]; %>
                    <% foreach (DataColumn row in dtColumns.Columns) { %>
                    <td><%=row%></td>
                    <% } %>
                </tr>
                </thead>
                <tbody>
                 <% DataTable dtRows = (DataTable)ViewState["data"]; %>
                 <% foreach (DataRow row in dtRows.Rows){%>  
                 <tr>
                 <% foreach (DataColumn col in dtColumns.Columns) { %>
                      <td><%=row[col]%></td>
                  <% } %>
                  </tr>
                  <% } %>
            </tbody>
        </table>
        <% } %>


            <div id="exp_report" runat="server" visible="false">
                <asp:LinkButton ID="Button3" runat="server" ToolTip="EXCEL" OnClick="ExportToExcel" CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                <%-- <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="PDF"  OnClick = "ExportGridToword"  CssClass="right add"><i class="fa fa-file-pdf-o"></i></asp:LinkButton>--%>
                <asp:Label ID="lbl_filter_info" runat="server" style=" font-size:9px; font-weight:bold;"></asp:Label>
            </div>
            <hr />
            <asp:GridView ID="grid_view" runat="server" OnPageIndexChanging="OnPageIndexChanging" AutoGenerateColumns="False" CssClass="GridViewStyle">
              <HeaderStyle  CssClass="GridViewHeaderStyle" />
               <Columns>
                <asp:BoundField DataField="IdNumber" HeaderText="ID Number"/>
                <asp:BoundField DataField="Employee" HeaderText="Employee"/>
                <asp:BoundField DataField="department" HeaderText="Department"/>
                <asp:BoundField DataField="sssnumber" HeaderText="SSS Number"/>
                <%--<asp:BoundField DataField="payrollnumber" HeaderText="Payroll Number"/>--%>
                <asp:BoundField DataField="netincome" HeaderText="Net Income" DataFormatString="{0:n2}"/>
                <asp:BoundField DataField="ee_share" HeaderText="EE Share" DataFormatString="{0:n5}"/>
                <asp:BoundField DataField="er_share" HeaderText="ER Share" DataFormatString="{0:n5}"/>
                <asp:BoundField DataField="ee_er" HeaderText="Total EE/ER" DataFormatString="{0:n2}"/>
                <asp:BoundField DataField="ec" HeaderText="EC" DataFormatString="{0:n2}"/>
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:n2}"/>
               </Columns>
            </asp:GridView>
        </div>
     </div>
<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!-- DataTables -->
<script type="text/javascript" src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script type="text/javascript"src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/fixedcolumns/3.2.6/js/dataTables.fixedColumns.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('#tbl').DataTable({
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            dom: 'Bfrtip',
            buttons: ['print'], //'copy', 'csv', 'excel', 'pdf', 'print'
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