<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editloan.aspx.cs" Inherits="content_hr_editloan" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_editloan">

<table>
    <tr>
        <td>Employee :</td>
        <td> <asp:TextBox ID="txt_emp" runat="server"></asp:TextBox></td>
    </tr>
     <tr>
        <td>Loan :</td>
        <td><asp:DropDownList ID="dll_deduction"  runat="server"></asp:DropDownList></td>
    </tr>

    <tr>
        <td>Loan No. :</td>
        <td>
            <asp:DropDownList ID="dll_no"  runat="server">
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
        <td><asp:TextBox ID="txt_amount" runat="server" ontextchanged="txt_no_TextChanged" AutoPostBack="true"></asp:TextBox></td>
    </tr>

    <tr>
        <td>No. of Payments :</td>
        <td><asp:TextBox ID="txt_no" runat="server" ontextchanged="txt_no_TextChanged" AutoPostBack="true"></asp:TextBox></td>
    </tr>

    <tr>
        <td>Amortization :</td>
        <td><asp:TextBox ID="txt_amortization" runat="server"></asp:TextBox></td>
    </tr>

    <tr>
        <td>Memo :</td>
        <td><asp:TextBox ID="txt_memo" TextMode="MultiLine" runat="server"></asp:TextBox></td>
    </tr>

    <tr>
        <td>Ispaid :</td>
        <td>
            <asp:CheckBox ID="check_paid" runat="server" />
        </td>
    </tr>

</table>
    <asp:Button ID="btn_save" runat="server" Text="Update" onclick="btn_save_Click" />

</asp:Content>


