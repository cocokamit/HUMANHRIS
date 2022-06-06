<%@ Page Language="C#" AutoEventWireup="true" CodeFile="workverification.aspx.cs" EnableEventValidation="false" Inherits="content_report_monitoring_workverification" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    hr{margin:10px 0}
    .vp{margin:0 20px}
    .x_panel, .x_title {margin-bottom:0}
    .panel_toolbox {min-width:0}
    .dropdown-menu{min-width:10px}
    .vh{ visibility:hidden}
    .input-group-btn input[type=submit] { border:none;border-left:none; border-top-right-radius:40%; border-bottom-right-radius:40%}
    .hiddencol { display: none; }
</style>
    <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="script/autocomplete/jquery.min.js" type="text/javascript"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/report/monitoring/undertimelist.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[1],
                                val: item.split('-')[0]
                            }
                        }))
                    },
                    error: function (result) {
                        alert(result.responseText);
                    }
                });
            },
            select: function (e, i) {
                index = $(".auto").parent().parent().index();
                $("#lbl_bals").val(i.item.val);
            }
        });
    });
</script>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Work Verification Monitoring</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:TextBox ID="txtemp" Placeholder="Employee Name" CssClass="auto" runat="server" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txtfrom" runat="server" CssClass="datee" Placeholder="From" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txtto" runat="server" CssClass="datee" ClientIDMode="Static" AutoComplete="off" Placeholder="To"></asp:TextBox>
                <asp:Button ID="btnsearch" OnClick="click_search" runat="server"  Text="Search" CssClass="btn btn-primary"/>
            </div>
            <div id="exp_report" runat="server" visible="false">
                <asp:LinkButton ID="btnexport" runat="server" ToolTip="Export Excel" OnClick="ExportToExcel"  CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                <asp:Label ID="lblfilter" runat="server" style=" font-size:9px; font-weight:bold;"></asp:Label>
            </div>
            <div id="div_list" runat="server">
            <div id="alert" runat="server" class="alert alert-empty" visible="false">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                    <asp:BoundField DataField="employee" HeaderText="Employee" />
                    <asp:BoundField DataField="sysdate" HeaderText="Filed Date" />
                    <asp:BoundField DataField="Date" HeaderText="Date" />
                    <asp:BoundField DataField="class" HeaderText="Type" />
                    <asp:BoundField DataField="reason" HeaderText="Reason" />
                    <asp:BoundField DataField="status" HeaderText="Status" />
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
 </div>

<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" runat="server">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datee").datepicker({ changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true
            });
        });
    })(jQuery);
</script>
</asp:Content>