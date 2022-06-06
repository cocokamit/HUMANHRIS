<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addloan.aspx.cs" Inherits="content_hr_addloan" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_addloan">
 <script src="script/auto/myJScript.js" type="text/javascript"></script>

 <script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>


 <script type="text/javascript">
     $(document).ready(function () {
         $.noConflict();
         $(".auto").autocomplete({
             source: function (request, response) {
                 $.ajax({
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     url: "content/hr/addloan.aspx/GetEmployee",
                     data: "{'term':'" + $(".auto").val() + "'}",
                     dataType: "json",
                     success: function (data) {
                         response($.map(data.d, function (item) {
                             return {
                                 label: item.split('-')[1],
                                 val: item.split('-')[0]
                             }
                         }))
                     },
                     error: function (result) {
                         alert(result.responseText);
                     }
                 });
             },
             select: function (e, i) {
                 index = $(".auto").parent().parent().index();
                 $("#lbl_bals").val(i.item.val);
             }
         });
     });

    
</script>

<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Deduction Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Mloan"><i class="fa fa-ticket"></i> Deduction</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Deduction application</li>
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
                <li><asp:TextBox ID="ddl_emp" placeholder="Employee Name" CssClass="auto" runat="server"></asp:TextBox></li>
                <li>Type<asp:Label ID="lbl_loan" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="dll_deduction" OnSelectedIndexChanged="txt_ew" Enabled="false" AutoPostBack="true"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                <li>Reference</li>
                <li><asp:TextBox ID="dll_no" runat="server"></asp:TextBox></li>
                <li>Amount<asp:Label ID="lbl_la" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_amount" runat="server" Enabled="false" ontextchanged="txt_aw" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></li>
                <li>No. of Terms (Month/s)<asp:Label ID="lbl_not" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_no" runat="server" Enabled="false" ontextchanged="txt_no_TextChanged" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox></li>
                <li>Interest<asp:Label ID="lbl_li" style="color:Red;" runat="server"></asp:Label></li>
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
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
</asp:Content>

