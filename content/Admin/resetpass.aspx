<%@ Page Language="C#" AutoEventWireup="true" CodeFile="resetpass.aspx.cs" Inherits="content_Admin_resetpass" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .vp{margin:0 20px}
        .x_panel, .x_title {margin-bottom:0}
       .panel_toolbox {min-width:0}
       .dropdown-menu{min-width:10px}
       .vh{ visibility:hidden}
       .input-group-btn input[type=submit] {border:1px solid #efefef;border-left:none;  border-top-right-radius:40%; border-bottom-right-radius:40%}
       .table-bordered {margin-top:0}
    </style>
        <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
    <script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
     
     <script type="text/javascript" src="script/freezeheadergv/index.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
            freaze("grid_view", "container");
        });
    </script>
     <script type="text/javascript">
         function Confirm() {
             var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
             if (confirm("Are you sure to reset the password?"))
             { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
         } 
    </script>
    
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function() {
        $.noConflict();
        $(".auto").autocomplete({
            source: function(request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/Admin/resetpass.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                label: item.split('-')[1],
                                val: item.split('-')[0]
                            }
                        }))
                    },
                    error: function(result) {
                        alert(result.responseText);
                    }
                });
            },
            select: function(e, i) {
                index = $(".auto").parent().parent().index();
                $("#lbl_bals").val(i.item.val);
            }
        });
    });
</script>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>MASTER LIST</h3>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-md-12">
                <div class="x_panel">
                    <asp:DropDownList ID="ddl_payroll_group" runat="server"  CssClass="minimal" AutoPostBack="true" OnSelectedIndexChanged="search"></asp:DropDownList>
                    <div class="title_right" style="margin:0">
                        <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
                            <div class="input-group">
                                <asp:TextBox ID="txt_search" placeholder="Search for..." runat="server" CssClass="form-control auto"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="Button1" runat="server" OnClick="search" class="btn btn-default"  Text="Go!" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="x_content">
                        <div class="row" style="margin:0 -15px">
                          <div class="col-md-12 col-sm-12 col-xs-12">
                            </div>
                            <div class="clearfix"></div>
                            <div id="div_list" runat="server" style="margin:-25px 10px 0">
                                <div id="container" style="overflow: scroll; overflow-x: hidden;">
        </div>
                                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="tawesie" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Profile">
                                            <ItemTemplate>
                                                <img src="files/profile/<%# Eval("profile") %>.png" alt="" class="img-circle" width="30px">
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IdNumber" HeaderText="ID No."/>
                                        <asp:BoundField DataField="FullName" HeaderText="Name"/>
                                        <asp:BoundField DataField="position" HeaderText="Position" />
                                        <asp:BoundField DataField="Department" HeaderText="Department" />
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk_view" runat="server"  CommandName='<%# Eval("id") %>' Text="edit" OnClientClick="Confirm()" OnClick="reset"  ToolTip="Reset Password"><i class="fa fa-retweet"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
        </div>  
    </div>
</div>
<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
 <asp:HiddenField ID="TextBox1" runat="server" />
</asp:Content>
