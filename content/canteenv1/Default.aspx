<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="content_canteen_Default" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .table th{ font-size:10px}
        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th { padding:5px}
        .minimala {-webkit-appearance: none;-moz-appearance: none; font-size:10px; background:#fff; border:none !important}
    </style>
</asp:Content>

 <asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Schedule</h3>
    </div> 
    <a href="schedule" class="pull-right btn"><i class="fa fa-arrow-circle-left"> Back</i></a>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
       
        <div class="x_panel">
             <div class="x-head">
                <div class="tools text-right pull-right">
                    <asp:LinkButton ID="lb_scheduler" runat="server"></asp:LinkButton><br />
                    <asp:LinkButton ID="lb_created" runat="server"></asp:LinkButton>
                 </div>
                <h3> <asp:LinkButton ID="lb_period" runat="server"></asp:LinkButton></h3>
                 <div class="clearfix">
                 </div>
            </div>
             <div class="x_content overflow">
                <asp:GridView ID="gv_schedule" runat="server" AutoGenerateColumns="true" OnRowDataBound="rowbound" CssClass="table table-bordered no-margin">
                </asp:GridView>
            </div>
            <asp:Panel ID="p_footer" runat="server">
                <asp:LinkButton ID="lb_approve" runat="server" OnClick="click_approve" CssClass="btn btn-primary" Text="Approve"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="view" CssClass="btn btn-danger pull-right" Text="Decline"></asp:LinkButton>
            </asp:Panel>
        </div>
    </div>
</div>


<div id="modal_delete" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close_delete" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Declined Schedule</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                <label>Reason</label>
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="box-footer pad">
            <asp:Button ID="btn_save" runat="server" OnClick="click_decline" Text="Save" CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>


<asp:HiddenField ID="hf_id" runat="server"/>
<asp:HiddenField ID="hf_schedulerid" runat="server"/>
<asp:HiddenField ID="hf_TChangeShiftID" runat="server"/>
</asp:Content>
