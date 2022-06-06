<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leave_list.aspx.cs" Inherits="content_Employee_leave_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
<!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure you want to cancel this transaction?"))
        { confirm_value.value = "YES"; } else { confirm_value.value = "No"; }
    } 
</script>
    <style type="text/css">
        .aspNetDisabled  { color: #8a8a8a !important}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="conten_leaveList">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<section class="content-header">
    <h1>Leave</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Leave</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-md-4">
          <div class="box box-primary">
            <div class="box-header" style="padding-bottom:0">
                <div id="pnlManager" runat="server"  style="padding-bottom:8px"  >
                    <asp:Button ID="Button1" runat="server" Text="ADD" OnClick="addempsleave" CssClass="btn btn-primary pull-right" />
                    <asp:DropDownList ID="ddl_empsleave" runat="server" CssClass="select2" AutoPostBack="true" OnSelectedIndexChanged="leaveapplication" ></asp:DropDownList>
                </div>
            </div>
            <div class="box-body no-pad-top ">
                <asp:GridView ID="grid_leave_credits" runat="server" AutoGenerateColumns="false" EmptyDataText="No Leave Credits"  CssClass="table table-bordered no-margin"><%--OnRowDataBound="datatbound"--%>
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="Credit" HeaderText="Credits"/>
                        <asp:BoundField DataField="Balance" HeaderText="Balance"/>
                        <asp:BoundField DataField="yyyyear" HeaderText="Year"/>

                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
        <div class="col-md-8">
          <div class="box box-primary">
            <div id="pnlMember" runat="server" class="box-header" style="padding-bottom:0">
                <asp:Button ID="btn_add" runat="server" Text="ADD" OnClick="addcredentials" CssClass="btn btn-primary pull-left" />
            </div>
	        
            <div class="box-body">
                <asp:GridView ID="gvLeave" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" OnRowDataBound="rowboundLeave" EmptyDataText="No record found" CssClass="table table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="date" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Leave" HeaderText="Type"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:BoundField DataField="status" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" CssClass="label"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" OnClick="view1" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_reprint" OnClick="reprint" style=" display:none;" ToolTip="Print" CssClass="glyphicon glyphicon-print" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_can" OnClick="cancelleave" ToolTip="Cancel" OnClientClick="Confirm()" CssClass="glyphicon glyphicon-remove-sign no-padding-right" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="75px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false"  OnRowDataBound="rowbound" EmptyDataText="No record found" CssClass="table table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="trnid"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="trn_date" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Leave" HeaderText="Type"/>
                    <asp:BoundField DataField="delegate" HeaderText="Delegate"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" OnClick="view1" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                         <%--   <asp:LinkButton ID="LinkButton1"  CssClass="fa fa-history" runat="server"></asp:LinkButton>--%>
                            <asp:LinkButton ID="lnk_reprint" OnClick="reprint" style=" display:none;" ToolTip="Print" CssClass="glyphicon glyphicon-print" runat="server"></asp:LinkButton>
                            <%--<ajaxToolkit:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="LinkButton1" PopupControlID="PopupMenu1"
                            PopupPosition="left" OffsetX="0" OffsetY="0"   PopDelay="50" HoverCssClass="popupHover"></ajaxToolkit:HoverMenuExtender>
                            <asp:Panel ID="PopupMenu1" runat="server" style=" overflow-y:auto; overflow-x: auto; height:70px; width:80%"> 
                            <asp:GridView ID="PopupMenu" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="Gray" EmptyDataText="No History"  CssClass="table table-bordered no-margin bg-teal-gradient">
                                <Columns>
                                    <asp:BoundField DataField="sysdate" HeaderText="Date Processed" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="approver" HeaderText="Approver" ItemStyle-Width="50px"/>
                                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="50px"/>
                                </Columns>
                            </asp:GridView>
                            </asp:Panel>--%>
                            <asp:LinkButton ID="lnk_can" OnClick="cancelall" ToolTip="Cancel" OnClientClick="Confirm()" CssClass="glyphicon glyphicon-remove-sign no-padding-right" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="75px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>

<div id="modal" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
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
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin hidden">
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
<div id="modal_delete" runat="server" class="modal fade in">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close_delete" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Cancellation Request</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                <label>Reason</label>
                <asp:HiddenField ID="hdn_l_id" runat="server" />
                  <asp:HiddenField ID="hdnLID" runat="server" />
                 <asp:HiddenField ID="hdn_date_leave" runat="server" />
                 <asp:HiddenField ID="hdn_request_id" runat="server" />
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="box-footer pad">
            <asp:Button ID="btn_save" runat="server" OnClick="cancel" Text="Save" CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>

<asp:HiddenField ID="id" runat="server" />
<div style=" visibility:hidden;">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    
</div>
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
