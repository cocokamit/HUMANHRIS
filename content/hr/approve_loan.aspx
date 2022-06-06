<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_loan.aspx.cs" Inherits="content_hr_approve_loan" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_addloan">

<h1>Add Loan</h1>


<table>
    <tr>
        <td>Employee :</td>
        <td><asp:TextBox ID="txt_name" runat="server"></asp:TextBox></td>
    </tr>


    <tr>
        <td>Loan :</td>
        <td><asp:DropDownList ID="dll_deduction" OnSelectedIndexChanged="txt_aw" AutoPostBack="true" runat="server"></asp:DropDownList></td>
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
        <td><asp:TextBox ID="txt_amount" runat="server" ontextchanged="txt_aw" AutoPostBack="true"></asp:TextBox></td>
    </tr>
   
    <tr>
        <td>No. of Terms :</td>
        <td><asp:TextBox ID="txt_no" runat="server" Enabled="false" ontextchanged="txt_no_TextChanged" AutoPostBack="true"></asp:TextBox></td>
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

    <tr>
        <td>Ispaid :</td>
        <td>
            <asp:CheckBox ID="check_paid" runat="server" />
        </td>
    </tr>

</table>
   <asp:Button ID="btn_save" runat="server" Text="approve" onclick="btn_save_Click" />
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="ap" runat="server" />
</asp:Content>

