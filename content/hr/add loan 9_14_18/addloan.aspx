<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addloan.aspx.cs" Inherits="content_hr_addloan" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_addloan">
 <script src="script/auto/myJScript.js" type="text/javascript"></script>
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Loan Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Mloan"><i class="fa fa-ticket"></i> Loan</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Loan Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <ul class="input-form">
                <li>Payroll Group<asp:Label ID="lbl_pg" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="ddl_pg"  runat="server" OnSelectedIndexChanged="click_pg" CssClass="minimal" AutoPostBack="true"></asp:DropDownList></li>
                <li>Employee<asp:Label ID="lbl_emp" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="ddl_emp"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                <li>Loan<asp:Label ID="lbl_loan" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="dll_deduction" OnSelectedIndexChanged="txt_ew" Enabled="false" AutoPostBack="true"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                <li style=" display:none;">Loan No</li>
                <li style=" display:none;">
                    <asp:DropDownList ID="dll_no"  runat="server" CssClass="minimal">
                        <asp:ListItem>0</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>Loan Amount<asp:Label ID="lbl_la" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_amount" runat="server" Enabled="false" ontextchanged="txt_aw" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></li>
                <li>No. of Terms<asp:Label ID="lbl_not" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_no" runat="server" Enabled="false" ontextchanged="txt_no_TextChanged" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></li>
                <li>Loan Interest<asp:Label ID="lbl_li" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="lbl_interest" runat="server" Enabled="false"></asp:TextBox></li>
                <li>Amortization<asp:Label ID="lbl_amort" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_amortization" runat="server" onkeyup="decimalinput(this)"></asp:TextBox></li>
                <li>Memo<asp:Label ID="lbl_memo" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_memo" TextMode="MultiLine" runat="server"></asp:TextBox></li>
            </ul>
            <hr />
            <asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" CssClass="btn btn-primary"/>
        </div>
    </div>
</div>  
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="pg1" runat="server" />

</asp:Content>

