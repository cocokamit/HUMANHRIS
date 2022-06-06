<%@ Page Language="C#" AutoEventWireup="true" CodeFile="display_schedule.aspx.cs" Inherits="content_Manager_display_schedule" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <!--FREEZE GRID-->
    <link href="script/FreezeGrid/web.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GridViewScrollItemFreeze td { line-height:normal}
    </style>
</asp:Content>

 <asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
 <section class="content-header">
    <h1>Schedule Approval</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Schedule Approval</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
        <div class="box box-primary">
            <div class="box-header with-border">
              <h3 class="box-title"><asp:Label ID="period" runat="server"></asp:Label></h3>
              <div class="box-tools pull-right">
                <% string backURL = Request.QueryString["oic"] == null ? "" : "?oic=true&key=" + Request.QueryString["oic"]; %>
                <a href="schedule-approval<%= backURL %>" class="fa fa-arrow-circle-left"></a>
              </div>
            </div>
            <div class="box-body" style="overflow:hidden">
                <asp:GridView ID="gv_schedule" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" OnRowDataBound="rowbound" HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem" CssClass="no-margin">
                </asp:GridView>
            </div>
            <div class="box-footer">
               <asp:Panel ID="p_footer" runat="server" style="padding: 0 10px 10px" >
                    <asp:LinkButton ID="lb_approve" runat="server" OnClick="click_approve" CssClass="btn btn-primary" Text="Approve"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="view" CssClass="btn btn-danger pull-right" Text="Decline"></asp:LinkButton>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>
</section>

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

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!--FREEZE GRID-->
<script type="text/javascript" src="script/FreezeGrid/gridviewscroll.js"></script>
<script type="text/javascript">
    var gridViewScroll = null;
    window.onload = function () {
        gridViewScroll = new GridViewScroll({
            elementID: "gv_schedule",
            width: $(".content").width() - 20,
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