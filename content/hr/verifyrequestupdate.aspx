<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verifyrequestupdate.aspx.cs" Inherits="content_hr_verifyrequestupdate" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3{  width:300px;left:60%;}
    .close{ margin:-10px;float:right;}
    .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 { padding:10px;   border-radius:4px; position: absolute; z-index: 1002;  width: 600px; top: 28%;left: 50%; margin: -150px 0 0 -300px; background-color: #fff; }
    .input-form {}
    .input-form-span { margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>

<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
    function Confirmapp() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure you want to Approved this Application?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
</script>

<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>   
<script type="text/javascript" src="script/myJScript.js"></script>
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
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
    <style type="text/css">
        .radio { margin-left: 20px}
    </style>

<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
    function CheckBoxCount() {
        var gv = document.getElementById("<%= grid_view.ClientID %>");
        var lbl_del_notify = document.getElementById("<%= lbl_del_notify.ClientID %>");
        var lbl_del_notify_val = document.getElementById('lbl_del_notify');
        var inputList = gv.getElementsByTagName("input");
        var numChecked = 0;

        for (var i = 0; i < inputList.length - 1; i++) {
            if (inputList[i].type == "checkbox" && inputList[i].checked) {
                numChecked = numChecked + 1;
            }

        }
        lbl_del_notify.textContent = numChecked + " rows";
    }
</script>
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
        <div class="title_left">
        <h3>Request Update</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee form-control" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
            <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee form-control" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
            <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
            <div id="alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <div class="pull-right">
                <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
             </div>
            <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-30px; font-size:11px; "></asp:Label>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>
                    <asp:BoundField DataField="RequestDate" HeaderText="Date Request" HeaderStyle-Width="100px"/>
                    <asp:BoundField DataField="FullName" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="Failed" HeaderText="Field"/>
                    <asp:BoundField DataField="Request" HeaderText="Request"/>
                     <asp:BoundField DataField="Code" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="100px"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_approve" OnClientClick="Confirmapp()"  OnClick="approve" ToolTip="Approved" CssClass="glyphicon glyphicon-ok-sign disabled"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnk_view"  OnClientClick="Confirm()" OnClick="cancel" ToolTip="Cancel"><i class="glyphicon glyphicon-remove-sign"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="empid" runat="server" />
<asp:HiddenField ID="adjid" runat="server" />
</asp:Content>