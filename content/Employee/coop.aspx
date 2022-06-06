<%@ Page Language="C#" AutoEventWireup="true" CodeFile="coop.aspx.cs" Inherits="content_Employee_coop"  MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="content" ID="content_coop" runat="server">

<h3>Coop</h3>

<table>


 <tr>
        <td>Loan :</td>
        <td><asp:DropDownList ID="dll_deduction" OnSelectedIndexChanged="txt_ew" AutoPostBack="true"  runat="server"></asp:DropDownList></td>
    </tr>

    <tr>
        
        <td>
            <asp:DropDownList ID="dll_no" Visible="false"  runat="server">
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
            </asp:DropDownList>
        
        </td>
    </tr>

     <tr>
        <td>Loan Amount :</td>
        <td><asp:TextBox ID="txt_amount" runat="server" Enabled="false" ontextchanged="txt_aw" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></td>
    </tr>
   
    <tr>
        <td>No. of Terms :</td>
        <td><asp:TextBox ID="txt_no" runat="server" Enabled="false" ontextchanged="txt_no_TextChanged" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></td>
    </tr>

     <tr>
        <td>Loan Interest :</td>
        <td><asp:Label ID="lbl_interest" runat="server" Text="Label"></asp:Label></td>
    </tr>


    <tr>
        <td>Amortization :</td>
        <td><asp:TextBox ID="txt_amortization" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>

    <tr>
        <td>Memo :</td>
        <td><asp:TextBox ID="txt_memo" TextMode="MultiLine" runat="server"></asp:TextBox></td>
    </tr>





</table>
<asp:Button ID="btn_save" runat="server" onclick="click_save_coop" Text="SAVE" />

</asp:Content>
