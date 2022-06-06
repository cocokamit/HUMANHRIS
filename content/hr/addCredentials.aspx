<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addCredentials.aspx.cs" Inherits="content_hr_addCredentials" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" ID="head_add_creadentials" runat="server">
 
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_add_credentials">
<asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server"> </asp:ScriptManager>
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Credentials Details</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Credentials?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-group (alias)"></i> Credentials</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Credentials Details</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <ul class="input-form">
                <li>USERNAME</li>
                <li><asp:TextBox ID="txt_user" runat="server"></asp:TextBox></li>
                <li>PASSWORD</li>
                <li><asp:TextBox ID="txt_pass" TextMode="Password"  placeholder="●●●●●●●●●●●●●" runat="server"></asp:TextBox></li>
                <li>ALLIAS</li>
                <li><asp:TextBox ID="txt_allias"  runat="server"></asp:TextBox></li>
            </ul>
            <asp:GridView ID="grid_item" runat="server" ShowFooter="false"  OnRowDataBound="OnRowDataBound"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField  HeaderText="Form">
                        <ItemTemplate>
                            <asp:Label ID="lbl_formname" runat="server" Text='<%# Bind("remarks") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allow">
                        <ItemTemplate> 
                            <asp:CheckBox ID="chk_allow"  runat="server" />          
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" CssClass="btn btn-primary"/>
        </div>
    </div>
</div>
  

 
    <div style=" visibility:hidden;">
    <asp:label ID="lbl_userid" runat="server"></asp:label>
    <asp:label ID="lbl_des" runat="server"></asp:label>
    <asp:label ID="lbl_nou" runat="server"></asp:label>
        <asp:label ID="lbl_pass" runat="server"></asp:label>
            <asp:label ID="lbl_user" runat="server"></asp:label>
    </div>
       <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
</asp:Content>

