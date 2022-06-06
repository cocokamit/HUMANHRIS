<%@ Page Language="C#" AutoEventWireup="true" CodeFile="assetassign.aspx.cs" Inherits="content_hr_assetassign" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .PopUpPanel hr{ margin:10px 0}
        .btn .fa { margin-right:10px}
    </style>
    <script type="text/javascript">
        function Confirm(elem, msg) {
            if (msg == "fixed") {
                if (elem.text == "Damage") {
                    var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
                    if (confirm("Are you sure that this item has already fixed?"))
                    { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
                }
            }
            else {
                var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
                if (confirm("Are you sure to cancel this transaction?"))
                { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
            }
        }

    </script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />

    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker();
            });
        })(jQuery);
    </script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Asset Assign</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
         <div class="x_panel">
             <div class="x-head title_right">
                <asp:TextBox ID="txt_search" placeholder="Employee..." CssClass="auto" runat="server"></asp:TextBox>
                <asp:Button ID="btnname" Height="35px" runat="server" Text="Go" CssClass="btn btn-primary" OnClick="search"/>
                <asp:LinkButton ID="lnkbuttnadd" runat="server" Visible="false" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
             </div>
            <asp:GridView ID="grid_assign" AllowPaging="true" PageSize="20" OnPageIndexChanging="gridview_paging" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" OnRowDataBound="grid_datarow">
                <Columns>
                    <asp:BoundField DataField="assassign" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Category" HeaderText="Category"/>
                    <asp:TemplateField HeaderText="Employee">
                        <ItemTemplate>
                            <asp:LinkButton ID="itemstatus" runat="server" OnClick="emp_redirect" Text='<%# bind("Name")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Department" HeaderText="Department"/>
                    <asp:BoundField DataField="Branch" HeaderText="Branch"/>
                    <asp:BoundField DataField="invname" HeaderText="Description"/>
                    <asp:BoundField DataField="qty" HeaderText="QTY"/>
                    <asp:BoundField DataField="serial" HeaderText="Serial No."/>
                    <asp:BoundField DataField="propertycode" HeaderText="Property Code"/>
                    <asp:BoundField DataField="price" HeaderText="Amount"/>
                    <asp:BoundField DataField="date" HeaderText="Date of Issuance"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                    <asp:BoundField DataField="empstatus" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                     <asp:TemplateField HeaderText="Action" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" Visible="false" runat="server" Text="edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div class="hide"> 
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</div>

<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/assetassign.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        console.log("test");
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

</asp:Content>