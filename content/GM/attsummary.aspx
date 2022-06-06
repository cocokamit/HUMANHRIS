<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="attsummary.aspx.cs" Inherits="content_GM_attsummary" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
<!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .att tr td:first-child {white-space: nowrap}
        .att tr td , .att tr th{ padding:5px !important; font-size:11px; cursor: pointer}
        .modal { position:fixed !important}
    </style>
    
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <h1>Attendance</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Attendance</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <asp:DropDownList ID="ddl_period" ClientIDMode="Static" CssClass="select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeFilter" style="float:left !important; width:180px" >
                </asp:DropDownList>
                <asp:LinkButton ID="lb_export" runat="server" OnClick="ExportToExcel" CssClass="fa fa-download pull-right" style="padding:5px"></asp:LinkButton>
            </div>
            <div class="box-body table-responsive" style="padding-top:0 !important">
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                </div>
                <asp:GridView ID="gv_schedule" runat="server" AutoGenerateColumns="true" OnRowDataBound="rowbound" CssClass="table table-bordered no-margin att">
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>

<div class="modal fade in" id="modal-default">
<div class="modal-dialog">
<div class="modal-content">
    <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
        <span aria-hidden="true">×</span></button>
    <h4 class="modal-title">Default Modal</h4>
    </div>
    <div class="modal-body">
        <table id="gvAttlogs" class="table table-bordered">
            <tr>
                <th>Logs</th>
            </tr>
        </table>
    </div>
</div>
<!-- /.modal-content -->
</div>
<!-- /.modal-dialog -->
</div>

</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);


    $("[data-toggle='modal']").click(function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "content/Manager/attendance.aspx/attlogs",
            data: "{'id':'" + $(this).attr("data-emp") + "', 'date':'" + $(this).attr("data-date") + "'}",
            dataType: "json",
            success: function (data) {
                $('#gvAttlogs').find('tbody').empty();
                for (var i = 0; i < data.d.length; i++) {
                    $("#gvAttlogs").append("<tr><td>" + data.d[i].date + "</td></tr>");
                }
            },
            error: function (result) {
                alert(result.responseText);
            }
        });
    });

</script>

 
</asp:Content>


