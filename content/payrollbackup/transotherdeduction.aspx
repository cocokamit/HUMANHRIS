<%@ Page Language="C#" AutoEventWireup="true" CodeFile="transotherdeduction.aspx.cs" Inherits="content_payroll_transotherdeduction" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="content_head_income">
<script type="text/javascript" src="script/gridviewpane/gridpane.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=imgOrdersShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>
    <style type="text/css">
        .table-bordered tbody > tr > td{ vertical-align:top}
         .hiddencol { display: none; }
    </style>

    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-100:+0",
                    changeYear: true
                });
            });
        })(jQuery);

    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_other_income">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Other Deduction</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
         <div class="x-head">
            <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"></asp:DropDownList>
            <asp:TextBox ID="txt_f" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
            <asp:TextBox ID="txt_t" cssclass="datee" runat="server" placeholder="To"></asp:TextBox>
            <asp:Button ID="Button2"  runat="server" onclick="btn_search_Click"  Text="Search" CssClass="btn btn-primary"/>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_add_other" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
         </div>
        <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
        <asp:GridView ID="grid_view1" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found"  CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="entrydate"  HeaderText="Entry Date"/>
                <asp:BoundField DataField="pg" HeaderText="Payroll Group"/>
                <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton3" runat="server"  ToolTip="Click to view transaction." OnClick="view"  ><i class="fa fa-clipboard sm"></i></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server"  ToolTip="Click to cancel transaction." OnClick="cancel_tran" OnClientClick="Confirm()"><i class="fa fa-trash-o"></i></asp:LinkButton><%--OnClick="click_cancel" OnClientClick="Confirm()"--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
    </div>
</div>
   <asp:HiddenField ID="TextBox1" runat="server" />
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="id" runat="server" />
   <asp:HiddenField ID="idd" runat="server" />
</asp:Content>