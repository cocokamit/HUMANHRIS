<%@ Page Language="C#" AutoEventWireup="true" CodeFile="timeAdjustments.aspx.cs" Inherits="content_hr_timeAdjustments" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="timeAdjustments">
    <style type="text/css">
        .PopUpPanel { width:400px; margin-left:-205px}
        .add-box { margin:0 5px 10px 0; width:50%}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_timeAdjustments">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Adjustment Classification</h3>
    </div> 
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Adjustment Classification</li>
        </ul>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
      <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:TextBox ID="txt_aa" runat="server" CssClass="left add-box"></asp:TextBox>
                <asp:Button ID="btn_save" runat="server" OnClick="add_adjustment" Text="save" CssClass="btn btn-primary" />
                <asp:LinkButton ID="Button2" runat="server"  OnClick="click_add_adj" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="manual_type" HeaderText="Type"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" runat="server" Text="Edit" OnClick="view"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="lb_delete" runat="server" Text="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ib_close" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul>
        <li><asp:TextBox ID="txt_type" runat="server"></asp:TextBox></li>
        <li><hr /></li>
        <li><asp:Button ID="Button1" runat="server" OnClick="update" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id" runat="server" />
</asp:Content>

