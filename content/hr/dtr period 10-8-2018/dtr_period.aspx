<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtr_period.aspx.cs" Inherits="content_hr_dtr_period" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
        .select { font-size:10px}
        .table-responsive::-webkit-scrollbar {width: 1em;}
        .table-responsive::-webkit-scrollbar-track {-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);}
        .table-responsive::-webkit-scrollbar-thumb {background-color: darkgrey;  outline: 1px solid slategrey;}
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
            <div class="box-body table-responsive padding no-pad-top">
                <div id="grid_alert" runat="server" class="alert alert-default no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                 <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="cnt" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="date_input"  ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="period" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:BoundField DataField="status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_can"  OnClick="click_set" Text='<%# Eval("period") %>' runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="lb_status" Text="Incomplete" OnClick="click_submit" CssClass="label label-danger pull-right" style=" padding:5px" runat="server"></asp:LinkButton> 
                                <asp:LinkButton ID="lnk_details" Text="view" Visible="false" OnClick="viewdec" CssClass="label label-danger pull-right" style=" padding:5px" runat="server"></asp:LinkButton>
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
            <div class="box-body table-responsive">
                <asp:GridView ID="gv_schedule" runat="server" AutoGenerateColumns="true" OnRowDataBound="schedule_rowbound" CssClass="table table-bordered no-margin schedule">
                </asp:GridView>
            </div>
            <div id="p_footer" runat="server" class="box-footer pad">
                <asp:Button ID="Button1" runat="server" Text="Save" onclick="click_SaveSchedule" CssClass="btn btn-primary"/>
                <button id="elem_delete" runat="server" type="button" class="btn btn-default pull-right" data-toggle="modal" data-target="#modal-default">Delete</button>
            </div>
        </div>
    </div>
</asp:Panel>
</section>

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
                <asp:Panel ID="alert_modal" runat="server" Visible="false">
                    <i class="fa fa-info-circle"></i>
                    <span><asp:Label ID="l_modal" runat="server" Text="No record found"></asp:Label></span>
                </asp:Panel>
                <asp:Panel ID="p_create" runat="server">
                    <div class="form-group">
                        <label>Start</label>
                        <asp:Label ID="l_start" runat="server" CssClass="text-danger"></asp:Label>
                        <asp:TextBox ID="txt_from" cssclass="datee form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>End</label>
                        <asp:Label ID="l_end" runat="server" CssClass="text-danger"></asp:Label>
                        <asp:TextBox ID="txt_to" cssclass="datee form-control" runat="server"></asp:TextBox>
                    </div>
                     <div class="form-group no-margin">
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
 
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
<asp:HiddenField ID="hf_shiftline" runat="server" />
<asp:HiddenField ID="hf_status" runat="server" />
    
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script type="text/javascript">
   

    
</script>
</asp:Content>

