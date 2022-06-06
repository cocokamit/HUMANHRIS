<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leavelist.aspx.cs" EnableEventValidation="false" Inherits="content_report_monitoring_leavelist" MasterPageFile="~/content/MasterPageNew.master"%>

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
                    url: "content/report/monitoring/leavelist.aspx/GetEmployee",
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
        <h3>Leave Monitoring</h3>
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
            <asp:GridView ID="gridtrans" runat="server"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
                        <Columns>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                            <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                            <asp:BoundField DataField="employee" HeaderText="Employee"/>
                            <asp:BoundField DataField="date" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="Leave" HeaderText="Type"/>
                            <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                            <asp:BoundField DataField="status" HeaderText="Status" />
                            <asp:TemplateField ItemStyle-CssClass="none" HeaderStyle-CssClass="none">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" CssClass="label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_view" OnClick="view1" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnk_reprint" style=" display:none;" ToolTip="Print" CssClass="glyphicon glyphicon-print" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnk_can" Visible="false" ToolTip="Cancel" OnClientClick="Confirm()" CssClass="glyphicon glyphicon-remove-sign no-padding-right" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle  Width="75px"/>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </div>
        </div>
    </div>
 </div>

<div id="modaltrans" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="close" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="lbl_lt" runat="server"></asp:Label></h4>
        </div>
        <div class="modal-body">
           <asp:GridView ID="grid_pay" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowboundgrid_pay" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="date" HeaderText="Date Leave" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="WithPay" HeaderText="With Pay"/>
                    <asp:BoundField DataField="nod" HeaderText="No. of Days"/>
                    <asp:BoundField DataField="designation" HeaderText="Designation"/>
                    <asp:BoundField DataField="status" HeaderText="Status"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Action"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  >
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="view" ToolTip="Cancel" CssClass="glyphicon glyphicon-remove-sign" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="10px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="l_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="request_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlApproverHistory" runat="server">
            <div class="modal-header">
                <h5 class="modal-title">Approver History</h5>
            </div>
            <asp:GridView ID="gridhistory" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="sysdate" HeaderText="Date Processed" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="approver" HeaderText="Approver" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_approver" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered ">
                <Columns>
                    <asp:BoundField DataField="date" HeaderText="Date Approved" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="approver" HeaderText="Approver"/>
                 <%--   <asp:BoundField DataField="remarks" HeaderText="Remarks"/>--%>
                    <asp:BoundField DataField="status"  HeaderText="Status"/>
                </Columns>
            </asp:GridView>
            </asp:Panel>
        </div>
    </div>
    </div>
</div>

<div id="modal" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        </div>
        <div class="modal-body">
          
            <div class="modal-header">
                <h4 class="modal-title">Leave History</h4>
            </div>
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Date" HeaderText="Date Leave" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="Leave" HeaderText="Leave Type" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="withpay" HeaderText="With Pay" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>
<asp:HiddenField ID="id" runat="server" />
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
