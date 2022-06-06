<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotificationList.aspx.cs" Inherits="content_hr_NotificationList" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style>
        .marlon { width:99%}
        .marlon i {color:#649fec;float:left;margin:3px 10px 0 0}
        .sub-marlon { float:left; width:calc(100% - 30px)}
        .sub-marlon span { display:block}
        .ann-date { float:right; margin-top:11px; color:#444; font-size:10px}
        .tbl { width:100%}
        .tbl,  .tbl tr,  .tbl th , .tbl td {border:none}
        .tbl tr {border-bottom:1px solid #eee;}
        .tbl tr:last-child {border:none}
        .tbl td {padding:10px 0}
        .tbl td span { line-height:20px}
        .editor {border:none}
        .no-border, .no-border td,.no-border th, .no-border tr { border:none}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="notificationlist">
<div class="page-title">
<div class="title_left hd-tl">
    <h3>Notifications</h3>
</div>
</div>
<div class="title_right hd-tl">
     <asp:Button ID="btnmark" OnClick="markallasread" runat="server" Text="Mark All As Read" />
     <asp:Button ID="btnsee" OnClick="seelatestnoti" Visible="false" runat="server" Text="View Current Notification" />
</div>
   
<div class="clearfix"></div>
        <asp:GridView ID="gridNotificationlist" runat="server" AutoGenerateColumns="false" CssClass="table no-border" >
          <Columns>
            <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="url" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="date" HeaderText="Date"/>
            <asp:BoundField DataField="subject" HeaderText="Subject"/>
            <asp:TemplateField HeaderText="Content">
            <ItemTemplate>
                <asp:LinkButton ID="lnkbtncontent" runat="server" CommandName='<%# Eval("id") %>' ToolTip="List" Text='<%# Eval("content") %>' OnClick="notificationcontent"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
        <asp:GridView ID="gridcurrent" runat="server" AutoGenerateColumns="false" CssClass="table no-border">
            <Columns>
            <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="url" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="date" HeaderText="Date"/>
            <asp:BoundField DataField="subject" HeaderText="Subject"/>
            <asp:TemplateField HeaderText="Content">
            <ItemTemplate>
                <asp:LinkButton ID="lnkbtncontent" runat="server" CommandName='<%# Eval("id") %>' ToolTip="List" Text='<%# Eval("content") %>' OnClick="hapitda"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
<asp:HiddenField ID="trn_det_id" runat="server" />
</asp:Content>