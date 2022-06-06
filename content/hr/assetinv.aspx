<%@ Page Language="C#" AutoEventWireup="true" CodeFile="assetinv.aspx.cs" Inherits="content_hr_assetinv" MasterPageFile="~/content/MasterPageNew.master" %>

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

    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
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
        <h3>Asset Inventory</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
             <div class="x-head title_right">
                <asp:TextBox ID="txt_search" placeholder="Desciption..." CssClass="auto" runat="server"></asp:TextBox>
                <asp:Button ID="btnname" runat="server" Text="Go" CssClass="btn btn-primary"/>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_inventory" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
             </div>
            <asp:GridView ID="grid_inv" AllowPaging="true" PageSize="20" runat="server" OnPageIndexChanging="gridview_paging" OnRowDataBound="rowboundasset" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="itemid" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                    <asp:BoundField DataField="Category" HeaderText="Category"/>
                    <asp:BoundField DataField="Namejud" HeaderText="Description"/>
                    <asp:BoundField DataField="brand" HeaderText="Brand Name"/>
                    <asp:BoundField DataField="serial" HeaderText="Serial"/>
                    <asp:BoundField DataField="propertycode" HeaderText="Property Code"/>
                    <asp:BoundField DataField="vendor" HeaderText="Vendor"/>
                    <asp:BoundField DataField="date" HeaderText="Shelf Life" ItemStyle-Width="90px"/>
                    <asp:BoundField DataField="price" HeaderText="Price"/>
                    <asp:BoundField DataField="podate" HeaderText="PO Date" ItemStyle-Width="90px"/>
                    <asp:BoundField DataField="qty" HeaderText="QTY"/>
                    <asp:BoundField DataField="um" HeaderText="Unit Measure"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" runat="server" ToolTip="Details" OnClick="showdetails" Text="Details"><i class="fa fa-eye"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_edit" runat="server" ToolTip = "Edit" OnClick="editdetails" Text="edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="col-md-12">
        <asp:LinkButton ID="lb_cate" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="click_category"><i class="fa fa-plus"></i>Category</asp:LinkButton>
        <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
        <asp:GridView ID="grid_category" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="description" HeaderText="Description"/>
                <asp:BoundField DataField="um" HeaderText="Unit Measure"/>
                <asp:TemplateField HeaderText="Action">
                  <ItemTemplate>
                    <asp:LinkButton ID="lnk_view" runat="server" Text="edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                  </ItemTemplate>
                  <ItemStyle Width="73px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>

<div id="overlay" runat="server" visible="false" class="Overlay"></div>
<div id="catPanel" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="click_close" runat="server"/>
    <ul class="input-form">
        <li><b>New Category</b></li>
        <li> <asp:TextBox ID="txt_category" runat="server"></asp:TextBox></li>
        <li>Unit Measure<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:DropDownList ID="ddl_um" CssClass="minimal" runat="server">
            <asp:ListItem>Set</asp:ListItem>
            <asp:ListItem>Pieces</asp:ListItem>
            </asp:DropDownList>
        </li>
        <li><asp:CheckBox ID="chk_srlized" runat="server" /> Serialized </li>
        <li><asp:Button ID="Button3"  runat="server" OnClick="Clicksavecategory" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>
<div id="invetPanel" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="click_close" runat="server"/>
    <ul class="input-form">
        <li>Category <asp:Label ID="lbl_cat" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:DropDownList ID="ddl_cat" runat="server" AutoPostBack="true" OnTextChanged="select_category" CssClass="minimal"></asp:DropDownList></li>
        <li>Serial<asp:Label ID="lbl_serial" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_serial" runat="server"></asp:TextBox></li>
        <li>Property Code<asp:Label ID="lbl_property" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_propertycode" runat="server"></asp:TextBox></li>
        <li>Vendor<asp:Label ID="lbl_vendor" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_vendor" runat="server"></asp:TextBox></li>
        <li>Brand Name<asp:Label ID="lbl_brandname" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_brandname" runat="server"></asp:TextBox></li>
        <li>Item Description<asp:Label ID="lbl_desc" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_desc" runat="server"></asp:TextBox></li>
        <li style=" display:none;">Location<asp:Label ID="lbl_loc" runat="server" ForeColor="Red"></asp:Label></li>
        <li style=" display:none;"><asp:TextBox ID="txt_location" runat="server"></asp:TextBox></li>
        <li>Original Price<asp:Label ID="lbl_price" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_price" ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:TextBox></li>
        <li>Purchase Date<asp:Label ID="lbl_podate" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_podate" CssClass="datee" runat="server" autocomplete="off"></asp:TextBox></li>
        <li>Quantity<asp:Label ID="lbl_nqty" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:TextBox ID="txt_nqty" ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:TextBox></li>
        <li><hr /></li>
        <li><asp:Button ID="Button4" runat="server" OnClick="clicksaveinventory" Text="Save" CssClass="btn btn-primary" /></li>
        <li><asp:Button ID="Button5" runat="server" OnClick="cancel" Text="Cancel" Visible="false" /></li>
    </ul>
</div>

<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="qty" runat="server" />
<asp:HiddenField ID="hdn_empid" runat="server" />
<div class="hide"> 
  <asp:HiddenField ID="seriesid" runat="server" />
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/assetinv.aspx/GetAssetInv",
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

<div id="modal_editasset" runat="server" class="modal fade in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">
        <div class="modal-header">
          <asp:LinkButton ID="LinkButton2" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
          <h4 class="modal-title"><asp:Label ID="lbldetails" ForeColor="Blue" runat="server"></asp:Label></h4>
        </div>
        <div class="modal-body">
          <div class="col-lg-3">
            <div class="form-group">
                <label>Serial</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_s" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="col-lg-3">
            <div class="form-group">
                <label>Property Code</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_pc" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="col-lg-3">
            <div class="form-group">
                <label>Vendor</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_v" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="col-lg-3">
            <div class="form-group">
                <label>Brand Name</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_vn" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="col-lg-3">
            <div class="form-group">
                <label>Item Description</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_id" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="col-lg-3">
            <div class="form-group">
                <label>Location</label>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="form-group">
                <asp:TextBox ID="txt_l" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button1" runat="server" OnClick="saveedit" Text="Save" CssClass="btn btn-primary pull-right" />
        </div>
    </div>
    </div>
</div>

<div id="modal_assetdetails" runat="server" class="modal fade in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">
        <div class="modal-header">
          <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
          <h4 class="modal-title"><asp:Label ID="lbl_detail" ForeColor="Blue" runat="server"></asp:Label></h4>
        </div>
        <div class="modal-body">
          <asp:GridView ID="gridasset" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
            <Columns>
                 <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                 <asp:BoundField DataField="FullName" HeaderText="Employee" />
                 <asp:BoundField DataField="dept" HeaderText="Department" />
                 <asp:BoundField DataField="Namejud" HeaderText="Description" />
                 <asp:BoundField DataField="cat" HeaderText="Category"/>
                 <asp:BoundField DataField="propertycode" HeaderText="Property Code"/>
                 <asp:BoundField DataField="serial" HeaderText="Serial"/>
                 <asp:BoundField DataField="qty" HeaderText="Quantity"/>
                 <asp:BoundField DataField="um" HeaderText="Measure"/>
                 <asp:BoundField DataField="price" HeaderText="Amount" />
                 <asp:BoundField DataField="status" HeaderText="Status" />
            </Columns>
          </asp:GridView>
        </div>
    </div>
    </div>
</div>
</asp:Content>