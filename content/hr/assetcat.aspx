<%@ Page Language="C#" AutoEventWireup="true" CodeFile="assetcat.aspx.cs" Inherits="content_hr_assetcat" MasterPageFile="~/content/MasterPageNew.master" %>

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
        <h3>Asset Category</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="addcategory" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            <asp:GridView ID="grid_category" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="description" HeaderText="Category"/>
                     <asp:BoundField DataField="um" HeaderText="Unit Measure"/>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" OnClick="updatearea" runat="server" Text="edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="overlay" runat="server" visible="false" class="Overlay"></div>
<div id="catPanel" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="closepopup" runat="server"/>
    <ul class="input-form">
        <li><b>New Category</b></li>
        <li> <asp:TextBox ID="txt_category" runat="server"></asp:TextBox></li>
          <li>Unit Measure<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:DropDownList ID="ddl_um"   CssClass="minimal"   runat="server"><asp:ListItem>Set</asp:ListItem><asp:ListItem>Pieces</asp:ListItem></asp:DropDownList></li>
        <hr />
        <li><asp:CheckBox ID="chk_srlized" runat="server" /> Serialized </li>
        <hr />
        <li><asp:Button ID="Button3"  runat="server" OnClick="savecategory" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>
<div id="overlay2" runat="server" visible="false" class="Overlay"></div>
<div id="catPanel2" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="closepopup" runat="server"/>
    <ul class="input-form">
        <li><b>Update Category</b></li>
        <li> <asp:TextBox ID="txt_desc" runat="server"></asp:TextBox></li>
        <li>Unit Measure<asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:DropDownList ID="ddl_unitm"   CssClass="minimal"   runat="server"><asp:ListItem>Set</asp:ListItem><asp:ListItem>Pieces</asp:ListItem></asp:DropDownList></li>
        <hr />
        <li><asp:CheckBox ID="chk_serial" runat="server" /> Serialized </li>
        <hr />
        <li><asp:Button ID="Button1"  runat="server" OnClick="updatecategory" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>
<div class="hide"> 
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
<asp:HiddenField ID="seriesid" runat="server" />
</div>
</asp:Content>