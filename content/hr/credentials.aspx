<%@ Page Language="C#" AutoEventWireup="true" CodeFile="credentials.aspx.cs" Inherits="content_hr_credentials" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
<div class="page-title">
    <div class="title_left">
        <h3>Credentials</h3>
    </div>
    <div class="title_right">
        <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
            <div class="input-group">
                <asp:TextBox ID="txt_search" class="form-control" placeholder="Search for..." runat="server"></asp:TextBox>
                <span class="input-group-btn">
                    <asp:Button ID="Button1" runat="server" class="btn btn-default input-group-btn-submit"  Text="Go!" />
                </span>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="false"  CssClass="table table-striped table-bordered table-input">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="fullname" HeaderText="Name"/>
                        <asp:BoundField DataField="branch" HeaderText="Branch" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_view" runat="server" OnClick="click_view"   Text="view"><i class="fa fa-sliders"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

<div class="div">
    <table width="100%" align="center"   >
        <tr>
            <td>
               
                
            </td>
        </tr>
    </table>
</div>
<%--<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server"  visible="false" class="PopUpPanel">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
<ul>
<li>Period<asp:Label ID="lbl_leaveid" ForeColor="White" runat="server" ></asp:Label></li>
<li><asp:TextBox ID="txt_period" runat="server"></asp:TextBox></li>
<li>Ot Doc Date</li>
<li><asp:TextBox ID="txt_ldocdate" runat="server"></asp:TextBox></li>
<li>Payroll Group</li>
<li><asp:DropDownList ID="ddl_viewpg" runat="server" Enabled="false" DataValueField="Id"></asp:DropDownList></li>
<li>Remarks</li>
<li><asp:TextBox ID="txt_viewremarks" runat="server"></asp:TextBox></li>
</ul>
<table>
<tr>
<th>Employee</th>
<th>Date</th>
<th>Remarks</th>
<th>Shift Code</th>
</tr>
<tr>
    <td><asp:DropDownList ID="ddl_employee" runat="server"></asp:DropDownList></td>
    <td><asp:TextBox ID="txt_csd" runat="server"></asp:TextBox></td>
    <td><asp:TextBox ID="txt_lineremarks" runat="server"></asp:TextBox></td>
     <td><asp:DropDownList ID="ddl_shiftcode" runat="server" DataSourceID="sql_shiftcode" DataTextField="ShiftCode" DataValueField="Id"></asp:DropDownList></td>
    <td><asp:Button ID="Button3" runat="server" OnClick="click_addperline" Text="ADD" /></td>
</tr>
</table>
 <asp:GridView ID="grid_linedetails" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                    <Columns>
                        <asp:BoundField DataField="id"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
                        <asp:BoundField DataField="Date" HeaderText="Date"/>
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                        <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server"  Text="can" OnClientClick="Confirm()" OnClick="click_can_perline"  style=" margin:3px;" ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
</div>--%>
 <div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
  <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="pg" runat="server" />
</asp:Content>



