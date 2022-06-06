<%@ Page Language="C#" AutoEventWireup="true" CodeFile="allow_admin.aspx.cs" Inherits="content_hr_allow_admin" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="timeAdjustments">
    <style type="text/css">
        .PopUpPanel { width:400px; margin-left:-205px}
        .add-box { margin:0 5px 10px 0; width:50%}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_timeAdjustments">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Approval Setup</h3>
    </div> 
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>>Approval Setup</li>
        </ul>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
      <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
             
            
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowdatabound" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="name" HeaderText="Transaction"/>
                    <asp:TemplateField HeaderText="Allow">
                        <ItemTemplate>
                            <asp:CheckBox ID="check_a" ClientIDMode="Static" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
          
        </div>
        
    </div>
</div>
  <asp:Button ID="btn_update" runat="server" OnClick="update" Text="UPDATE" CssClass="btn btn-primary" />

<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id" runat="server" />
</asp:Content>