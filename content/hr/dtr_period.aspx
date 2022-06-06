﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtr_period.aspx.cs" Inherits="content_hr_dtr_period" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<!--FREEZE GRID-->
<link href="script/FreezeGrid/web.css" rel="stylesheet" type="text/css" />

<!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

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
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
    <style type="text/css">
        .label { font-size:10px !important}
        .select { font-size:12px; height:30px !important}
        .select2-container--default.select2-container--disabled .select2-selection--single { background-color:#fff}
        .select2-container--default.select2-container--disabled .select2-selection--single .select2-selection__arrow { display: none !Important} 
        .irregular .select2-selection__rendered { color: red !important;}
        .clone { background: #fff;  cursor:copy;  z-index:200000; position:relative; margin-left:-19px; }
        tr:nth-child(even) {background-color: #f2f2f2;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr_period">
<section class="content-header">
    <h1>Scheduler</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Scheduler</li>
    </ol>
</section>
<section class="content">
<div id="main" runat="server" class="row">
    <div class="col-xs-12">
        <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm">
                    <asp:LinkButton ID="lb_add" runat="server" OnClick="add" CssClass="btn btn-primary">Add</asp:LinkButton>
                </div>
            </div>
            <div class="box-body no-pad-top">
                <div id="grid_alert" runat="server" class="alert alert-default no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div> 
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="cnt" HeaderText="cnt" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="date_input" HeaderText="date_input" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="period" HeaderText="period" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="status" HeaderText="status" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date_start" HeaderText="date_start" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date_end" HeaderText="date_end" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="rc" HeaderText="rc" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="approver_id" HeaderText="approver" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="TChangeShiftID" HeaderText="TChangeShiftID" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_can"  OnClick="click_set" Text='<%# Eval("period") %>' runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="lb_status" Text="Incomplete" OnClick="click_submit" CssClass="label label-danger pull-right" style=" padding:5px" runat="server"></asp:LinkButton> 
                                <asp:LinkButton ID="lnk_details" Text="Declined" Visible="false" OnClick="viewdec" CssClass=" small label label-danger pull-right" style=" padding:5px" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

<asp:Panel ID="p_scheduling" runat="server" Visible="false" CssClass="row">
    <div class="col-xs-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <div class="input-group input-group-sm" style="width: 300px;">
                     <h3 class="no-margin"><asp:Label ID="l_range" runat="server"></asp:Label> <asp:LinkButton ID="lb_shiftline" runat="server" Enabled><small>(Unsaved)</small></asp:LinkButton></h3>
                </div>
                <div class="box-tools box-tool-add">
                     <asp:LinkButton ID="LinkButton3" runat="server" OnClick="click_back" CssClass="glyphicon glyphicon-circle-arrow-left" Font-Size="20px" style=" margin:3px"></asp:LinkButton>
                </div>
            </div>
            <div class="box-body">
                <div style="overflow:hidden">
                    <asp:GridView ID="gv_schedule" runat="server" ClientIDMode="Static" AutoGenerateColumns="true" CssClass="GridSchedule" HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem" OnRowDataBound="schedule_rowbound" >
                    </asp:GridView>
                </div>
            </div>
            <div id="p_footer" runat="server" class="box-footer pad">
                <div class="btn-group">
                    <button id="elem_delete" runat="server" type="button" title="Delete" class="btn btn-default" data-toggle="modal" data-target="#modal-default"><i class="glyphicon glyphicon-remove"></i></button>
                    <asp:LinkButton ID="lb_setting" runat="server" ToolTip="Setting"  OnClick="clickSetting"  CssClass="btn btn-default" ><i class="fa fa-gear"></i></asp:LinkButton>
                    <asp:LinkButton ID="btn_upload" runat="server" ToolTip="Import"  OnClick="upload"  CssClass="btn btn-default" ><i class="glyphicon glyphicon-import"></i></asp:LinkButton>
                </div>
                <asp:Button ID="Button1" runat="server" Text="Save" onclick="click_SaveSchedule" CssClass="btn btn-primary pull-right"/>
            </div>
        </div>
    </div>
</asp:Panel>

</section>

<div class="modal fade in" id="modal_setting" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="LinkButton8" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h4 class="modal-title">Setting</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <div class="checkbox">
                    <label>
                      <asp:CheckBox ID="cbHOD" runat="server" Text="HOD" Checked Enabled="false" />
                    </label>
                  </div>
                  <div class="checkbox">
                    <label>
                      <asp:CheckBox ID="cbAHOD" runat="server" Text="AHOD"/>
                    </label>
                  </div>
                </div>
        </div>
         <div class="modal-footer">
            <asp:Button ID="Button3" runat="server" OnClick="clickSaveSetting" Text="Save" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-default" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-body">
          <button type="button" class="btn btn-default  pull-right" data-dismiss="modal">No</button>
          <asp:LinkButton ID="lb_close" runat="server" OnClick="click_DeletePeriod" CssClass="btn btn-danger pull-right" style="margin-right:5px">Yes</asp:LinkButton>
          <h4 class="modal-title">Delete period?</h4>
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal_submit" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-body">
          <asp:LinkButton ID="LinkButton4" runat="server" OnClick="cpop" CssClass="btn btn-default pull-right">No</asp:LinkButton>
          <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_submitschedule" CssClass="btn btn-success pull-right" style="margin-right:5px">Yes</asp:LinkButton>
          <h4 class="modal-title">Submit schedule?</h4>
        </div>
    </div>
    </div>
</div>

<div id="modal" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title"><%= hf_action.Value == "0" ? "Create Schedule" : "Delete Schedule"%></h4>
            </div>
            <div class="modal-body">
                <asp:Panel ID="alert_modal" runat="server" Visible="false" style="padding:10px; margin-bottom:10px">
                    <i class="fa fa-info-circle"></i>
                    <span><asp:Label ID="l_modal" runat="server" Text="No record found"></asp:Label></span>
                </asp:Panel>
                <asp:Panel ID="p_create" runat="server">
                    
		            <div class="form-group" style="display:none;">
                        <label>Date Period</label><asp:Label ID="Label1" runat="server" CssClass="text-danger"></asp:Label>
                        <asp:DropDownList ID="ddl_pay_range" CssClass="form-control"  runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group" >
                        <label>Start</label>
                        <asp:Label ID="l_start" runat="server" CssClass="text-danger"></asp:Label>
                        <asp:TextBox ID="txt_from" cssclass="datee form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group" >
                        <label>End</label>
                        <asp:Label ID="l_end" runat="server" CssClass="text-danger"></asp:Label>
                        <asp:TextBox ID="txt_to" cssclass="datee form-control" runat="server"></asp:TextBox>
                    </div>
                     <div class="form-group no-margin" style="display:none;">
                        <label>Note</label>
                        <asp:TextBox ID="txt_remarks" TextMode="MultiLine" cssclass="datee form-control" Rows="5" runat="server"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel ID="p_delete" runat="server">
                    <div class="form-group no-margin">
                        <label>Reason</label>
                         <asp:Label ID="lbl_re" CssClass="text-danger" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="txt_reason" TextMode="MultiLine" cssclass="datee form-control" runat="server"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btn_save" runat="server" OnClick="click_modalsave" Text="Save" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</div>

<div id="modal_delete" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close_delete" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Remarks</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                 <asp:GridView ID="grid_dec" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="date_input"  HeaderText="Date Declined" />
                        <asp:BoundField DataField="notes"  HeaderText="Remarks" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="box-footer pad">
         
        </div>
    </div>
    </div>
</div>

<div id="modal_upload" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="LinkButton5" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
            <h4 class="modal-title">Upload Schedule</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                <asp:FileUpload ID="FileUpload1" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" runat="server" />
            </div>
        </div>
        <div class="box-footer pad">
            <asp:LinkButton ID="lnk_excel" runat="server" ToolTip="EXCEL"  OnClick="ExportToExcel" ><i class="fa fa-file-text-o" style="padding-top:3px"></i> Template</asp:LinkButton>
            <asp:Button ID="Button2" runat="server" OnClick="process" Text="Upload" CssClass="btn btn-primary pull-right"/>
        </div>
    </div>
    </div>
</div>
 
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
<asp:HiddenField ID="hf_shiftline" runat="server" />
<asp:HiddenField ID="hf_status" runat="server" />
<asp:HiddenField ID="hf_start" runat="server" />
<asp:HiddenField ID="hf_end" runat="server" />
<asp:HiddenField ID="hf_role" runat="server" />
<asp:HiddenField ID="hf_approver" runat="server" />
<asp:HiddenField ID="hf_TChangeShiftID" runat="server" />

<asp:GridView ID="gv_dtrtemplate" runat="server" AutoGenerateColumns="true" ShowHeader="true" CssClass="none"></asp:GridView>

<asp:HiddenField ID="hf_a" runat="server" Value="0" ClientIDMode="Static" />
<asp:HiddenField ID="hf_b" runat="server"  Value="0"  ClientIDMode="Static" />
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!--GRID SCROLL-->
<script type="text/javascript" src="script/FreezeGrid/gridviewscroll.js"></script>
<script type="text/javascript">
    var gridViewScroll = null;
    window.onload = function () {
        gridViewScroll = new GridViewScroll({
            elementID: "gv_schedule",
            width: $(".content").width(),
            height: $(window).height() - 260,
            freezeColumn: true,
            freezeFooter: false,
            freezeColumnCssClass: "GridViewScrollItemFreeze",
            freezeFooterCssClass: "GridViewScrollFooterFreeze",
            freezeHeaderRowCount: 1,
            freezeColumnCount: 2,
            onscroll: function (scrollTop, scrollLeft) {
                console.log(scrollTop + " - " + scrollLeft);
            }
        });
        gridViewScroll.enhance();
    }
    function getScrollPosition() {
        var position = gridViewScroll.scrollPosition;
        alert("scrollTop: " + position.scrollTop + ", scrollLeft: " + position.scrollLeft);
    }
    function setScrollPosition() {
        var scrollPosition = { scrollTop: 50, scrollLeft: 50 };

        gridViewScroll.scrollPosition = scrollPosition;
    }
</script>

<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);

    $(document).ready(function () {

        $(".clone").click(function () {

            var col_index = $(this).parent().parent().index();
            var row_index = $(this).closest("tr").index();
            var elem = $(this).parent().find("select option:selected").val();

            console.log(col_index);
            console.log(row_index);

            i = 0;
            $("#gv_schedule tr").each(function () {
                $(this).find("td").each(function () {
                    if (i == row_index) {
                        var select = $(this).find(".select2");
                        select.val([1, elem]).trigger('change');
                    }
                });

                i++;
            });

        });

        //$(".select2").hover(function () {

            //var a = $(this).parent().parent().index();
            //var b = $(this).closest("td").index();
           // var c = $(this).parent().index();

           // var elem = $(this).parent().find("i")
             
             //   $(elem).toggle();


       // });
  
    });
</script>
</asp:Content>

