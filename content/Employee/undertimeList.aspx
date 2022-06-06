<%@ Page Language="C#" AutoEventWireup="true" CodeFile="undertimeList.aspx.cs" Inherits="content_Employee_undertimeList" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_undertime">
    <style type="text/css">
        .radio-box{ margin-top:-12px; float:right}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_undertime">
<section class="content-header">
    <h1>Undertime</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Undertime</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header with-border">
                <div class="input-group input-group-sm" style="width: 300px;">
                     <asp:Button ID="btn_add" runat="server" OnClick="addUnder" Text="ADD" CssClass="btn btn-primary" />
                </div>
                <div class="box-tools radio-box pull-right none">
                    <asp:RadioButtonList ID="rbl_class" OnSelectedIndexChanged="change" AutoPostBack="true" CssClass="radio" runat="server">
                        <asp:ListItem Selected>Personal Undertime</asp:ListItem>
                        <asp:ListItem>Authorized Undertime</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="box-body table-responsive no-pad-top">
                
            <asp:GridView ID="grid_view" OnPageIndexChanging="gridview_PageIndexChanging" AllowPaging="true" PageSize="10" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date_filed" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="timeout" HeaderText="Time Out" />
                    <asp:BoundField DataField="reason" HeaderText="Remarks"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
              
                     <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can"  OnClick="view" Tooltip="cancel" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div id="div_msg" runat="server" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            </div>
          </div>
        </div>
    </div>
</section>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
        <div class="box-body table-responsive no-pad-top">
        <div class="form-group">
            <label>Reason</label>
            <asp:Label ID="lbl_re" style=" color:Red;" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <asp:Button ID="btn_save" runat="server" OnClick="cancel" Text="Save" CssClass="btn btn-primary"/>
</div>
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="hdn_datefiled" runat="server" />
</asp:Content>

