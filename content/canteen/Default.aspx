<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="content_canteen_Default" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<!--FREEZE GRID-->
<link href="script/FreezeGrid/web.css" rel="stylesheet" type="text/css" />

<style type="text/css">
    #gv_schedule_Header { margin-top:10px}
    .GridViewScrollItemFreeze td { line-height:normal}
        
</style>
</asp:Content>

 <asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Schedule</h3>
    </div> 
    <a href="schedule" class="pull-right btn none"><i class="fa fa-arrow-circle-left"> Back</i></a>
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
                <h4 class="no-margin"><asp:LinkButton ID="lb_period" runat="server"></asp:LinkButton></h4>
                <small><asp:Label ID="lb_department" runat="server"></asp:Label></small>
                 <div class="clearfix">
                 </div>
            </div>
            <div class="x_content overflow">
                <asp:GridView ID="gv_schedule" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" OnRowDataBound="rowbound" HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem" >
                </asp:GridView>
            </div>
            <asp:Panel ID="p_footer" runat="server">
                <div class="clearfix">
                </div>
                <br />
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
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="box-footer" style="padding: 0 15px 15px">
            <asp:Button ID="btn_save" runat="server" OnClick="click_decline" Text="Save" CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>


<asp:HiddenField ID="hf_id" runat="server"/>
<asp:HiddenField ID="hf_schedulerid" runat="server"/>
<asp:HiddenField ID="hf_TChangeShiftID" runat="server"/>
</asp:Content>


<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!--FREEZE GRID-->
<script type="text/javascript" src="script/FreezeGrid/gridviewscroll.js"></script>
<script type="text/javascript">
    var gridViewScroll = null;
    window.onload = function () {
        gridViewScroll = new GridViewScroll({
            elementID: "gv_schedule",
            width: $(".x_panel").width(),
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
</asp:Content>