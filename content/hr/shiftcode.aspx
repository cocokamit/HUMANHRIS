<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shiftcode.aspx.cs" Inherits="content_hr_shiftcode" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Shift Code</h3>
    </div>  
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Shift Code</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="title_right">
                <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="click_add_shiftcode" CssClass="btn btn-primary" />
            </div> 
             <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" runat="server" OnClick="click_viewshiftcode" Text="view" ToolTip="View"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_can" OnClientClick="Confirm()" Text="can" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="72px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
</asp:Content>
