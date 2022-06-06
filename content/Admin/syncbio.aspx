<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="syncbio.aspx.cs" Inherits="content_Admin_syncbio" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<div class="page-title">
    <div class="title_left">
        <h3>Sync Bio</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="masterfile"><i class="fa fa-users"></i> Master File</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li><asp:Label ID="l_page" runat="server" ></asp:Label></li>
       </ul>
    </div>
</div>
</asp:Content>

