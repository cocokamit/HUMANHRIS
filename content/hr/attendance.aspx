<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="attendance.aspx.cs" Inherits="content_hr_attendance" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
        .select { font-size:10px}
        .table-responsive::-webkit-scrollbar {width: 1em;}
        .table-responsive::-webkit-scrollbar-track {-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);}
        .table-responsive::-webkit-scrollbar-thumb {background-color: darkgrey;  outline: 1px solid slategrey;}
        
        .irregular .select2-selection__rendered { color: red !important;}
        .schedule {font-family: 'Source Sans Pro', 'Helvetica Neue', Helvetica, Arial, sans-serif !important}
        .schedule .select2-container--default .select2-selection--single { border:none !important; float:left !important}
        .schedule .select2-container--default .select2-selection--single .select2-selection__arrow { right;0 !important}
        .schedule .select2-container {margin-right:0 !important}
        .schedule th { font-size:11px !important; padding:3px 5px !important}
        .schedule tr { padding:0 !important; white-space: nowrap} 
        .schedule tr td {padding:0 5px !important}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr_period">
<section class="content-header">
    <div class="page-title">
        <div class="title_left pull-left">
            <h3>Attendance</h3>
        </div>   
        <div class="title_right">
           <ul>
            <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Attendance</li>
           </ul>
        </div>
    </div>
</section>

<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div id="main" runat="server" class="table-responsive">
                <div class="x-head">
                    <asp:DropDownList ID="ddl_payrollgroup" ClientIDMode="Static" CssClass="select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnChangeFilter" style="float:left !important" >
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_period" ClientIDMode="Static" CssClass="select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeFilter" style="float:left !important" >
                    </asp:DropDownList>
                </div>
                <div class="clearfix"></div>
                <div id="grid_alert" runat="server" class="alert alert-empty no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <asp:GridView ID="gv_schedule" runat="server" AutoGenerateColumns="true" OnRowDataBound="schedule_rowbound" CssClass="table table-bordered no-margin schedule">
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
</section>

<section class="content">



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
<asp:HiddenField ID="hf_start" runat="server" />
<asp:HiddenField ID="hf_end" runat="server" />
<asp:HiddenField ID="hf_dtrid" runat="server" />
    
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>
</asp:Content>
