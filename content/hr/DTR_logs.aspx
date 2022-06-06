<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="DTR_logs.aspx.cs" Inherits="content_hr_DTR_logs" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
<title>Movenpick In & Out</title>
<!--DATETIMEPICKER-->
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datepicker").datepicker({ changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true
            });
        });
    })(jQuery);
    </script>

    <style type="text/css">
        .datetimepicker { position:absolute; z-index:9999}
        .alert {padding:10px; margin-top:0px !important}
        .page-title {margin-bottom:39px}
        .dataTables_filter {width:100% !important}
        .dataTables_filter input {border-color:#d2d6de !imortant; width:300px !important; border-radius:3px; padding:17px 10px !important; box-shadow:none !important;}
        .dataTables_filter i {margin-left:-25px; color:#d2d6de; margin-top:-25px; position:absolute}
        @-moz-document url-prefix() {
                .dataTables_filter i { margin-top:11px !important}
            }
        .table-bordered {margin-top:5px !important}
        .table-bordered > thead > tr > td, .table-bordered > thead > tr > th {border-bottom-width:1px !important}
        .dataTables_paginate {margin-top:-20px}
        .dataTables_paginate a {background: #fff !important;}
        .dataTables_paginate .active a { padding:6.5px 10px !important; background-color: #337ab7 !important;border-color: #337ab7; !important}
        .dt-buttons { position:absolute;}
         a.buttons-print:before { display:inline-block; padding:6px; font-family: FontAwesome;
    content: '\f02f';
    margin-right:2px
      
 }
    </style>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <div class="page-title">
        <div class="title_left pull-left">
            <h3>In & Out</h3>
        </div>   
        <div class="title_right">
           <ul>
            <li><a href="dashboard?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-dashboard"></i> Dashboard</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"> DTR</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Attendance</li>
           </ul>
        </div>
    </div>
</section>
<section class="content"> 
<div class="row">
    <div class="col-md-3">
        <div class="x_panel">
             <div class="form-group">
                <label>Date From</label>
                <asp:TextBox ID="txt_from" runat="server" CssClass="datepicker form-control"  autocomplete="off"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date To</label>
                <asp:TextBox ID="txt_to" runat="server" CssClass="datepicker form-control" autocomplete="off"  ></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Department</label>
                <asp:DropDownList ID="ddl_dept" ClientIDMode="Static" CssClass="form-control select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeDept" >
                </asp:DropDownList>
                <div class="clearfix"></div>
            </div>
            <div id="pnlSection" runat="server" class="form-group">
                <label>Section</label>
                <asp:DropDownList ID="ddl_section" ClientIDMode="Static" CssClass="form-control select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeSection">
                </asp:DropDownList>
                <div class="clearfix"></div>
            </div>
            <hr style=" margin:10px 0" />
            <asp:Button ID="Button4" runat="server" OnClick="Filter" Text="Search" CssClass="btn btn-primary "/>
        </div>
    </div>
    <div class="col-md-9">
        <div class="x_panel">
            <div id="pnl_alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            
            <asp:Panel ID="pnl_grid" runat="server">
           
            <table id="example1" class="table table-bordered table-hover dataTable no-margin">
                <thead>
                    <tr>
                      <th>ID Number</th>
                      <th>Name</th>
                      <th>Department</th>
                      <th>Section</th>
                      <th>Date</th>
                      <th>Check Type</th>
                    </tr>
                </thead>
              <tbody>
              <% DataTable dt = (DataTable)ViewState["data"]; %>
              <% foreach (DataRow row in dt.Rows) { %>
              <tr>
                  <td><%=row["idnumber"]%></td>
                  <td><%=row["e_name"]%></td>
                  <td><%=row["Department"]%></td>
                  <td><%=row["seccode"]%></td>
                  <td><%=row["Date_Time"]%></td>
                  <td><%=row["checktype"]%></td>
              </tr>
              <% } %>
               
                </tbody>
              </table>
            </asp:Panel>
        </div>
    </div>
</div>
</section>
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!-- DataTables -->
<script type="text/javascript" src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script type="text/javascript"src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>


<%--
https://datatables.net/extensions/buttons/examples/initialisation/export.html
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.print.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>--%>

 

<script>
    $(function () {
        $('#example1').DataTable({
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            'paging': true,
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            'autoWidth': false,
            dom: 'Bfrtip',
            buttons: ['print'] //'copy', 'csv', 'excel', 'pdf', 'print'
        })

    })
 
</script>
</asp:Content>

