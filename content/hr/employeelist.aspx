<%@ Page Language="C#" AutoEventWireup="true" CodeFile="employeelist.aspx.cs" Inherits="content_hrms_hr_emp_list" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
       .vp{margin:0 20px}
       .x_panel, .x_title {margin-bottom:0}
       .panel_toolbox {min-width:0}
       .dropdown-menu{min-width:10px}
       .vh{ visibility:hidden}
       .input-group-btn input[type=submit] {border:1px solid #efefef;border-left:none;  border-top-right-radius:40%; border-bottom-right-radius:40%}
       .table-bordered {margin-top:0}
       .overflow{  }
    </style>
    <script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
    <script type="text/javascript" src="script/freezeheadergv/index.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
            freaze("grid_view", "container");
        });
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>

    <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">

</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>Master File</h3>
        </div>
        <div class="title_right">
            <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="click_add_employee" CssClass="btn btn-primary" />
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-md-12">
                <div class="x_panel">
                    <div class="x_content">
                        <asp:DropDownList ID="ddl_payroll_group" runat="server"  CssClass="minimal" AutoPostBack="true" OnSelectedIndexChanged="search"></asp:DropDownList>
                    <div class="title_right" style="margin:0">
                        <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
                            <div class="input-group">
                                <asp:TextBox ID="txt_search" class="form-control" AutoPostBack="true" OnTextChanged="search" placeholder="Search for..." runat="server"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="Button1" runat="server" OnClick="search" class="btn btn-default"  Text="Go!" />
                                </span>
                            </div>
                        </div>
                    </div>
                            <div id="div_list" runat="server" style="border:1px solid transparent; overflow:hidden; width:100.5%">
                               <div id="container" class="overflow" style="margin-top:-1px"></div>
                                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" style="margin:0 !important">
                                    <Columns>
                                        <asp:BoundField DataField="setup" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <img src="files/profile/<%# Eval("profile") %>.png" alt="" class="img-circle" width="30px">
                                                <asp:LinkButton ID="lnk_view" runat="server" OnClick="click_edit_employee" CommandName='<%# Eval("id") %>' Text='<%# Eval("IdNumber") %>' Font-Size="13px" CssClass="no-border" ></asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" Font-Size="13px" CssClass="fa fa-warning text-red pull-right"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FullName" HeaderText="Name"/>
                                        <asp:BoundField DataField="position" HeaderText="Position" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                    </div>
                </div>
                <ul class="nav panel_toolbox left">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-gear"></i> <small style="font-size:11px; position:absolute; margin-left:5px; margin-top:2px">VIEW</small></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><asp:LinkButton ID="lb_list" runat="server" Text="List" OnClick="click_grid"></asp:LinkButton></li>
                            <li><asp:LinkButton ID="lb_grid" runat="server" Text="Grid" OnClick="click_grid"></asp:LinkButton></li>
                        </ul>
                    </li>                  
                </ul>
            </div>
        </div>
    </div>  
<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
<div style=" height:350px; overflow:auto">
    <b>Requirements</b>
      <asp:GridView ID="grid_det" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found!" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="filename" HeaderText="File Name"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                         <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                          <asp:LinkButton ID="lnk_can" runat="server" OnClientClick="Confirm()" OnClick="candetails"  CommandName='<%# Eval("id") %>' Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <b>Job Offer</b>
            <asp:GridView ID="grid_jobpost" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found!" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="filename" HeaderText="File Name"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                         <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="downloadjobpost"><i class="fa fa-download"></i></asp:LinkButton>
                        <%--  <asp:LinkButton ID="lnk_can" runat="server" OnClientClick="Confirm()" OnClick="candetails"  CommandName='<%# Eval("id") %>' Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
--%>                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
         </div>   
</div>

<asp:HiddenField ID="TextBox1" runat="server" />
<asp:HiddenField ID="trn_det_id" runat="server" />
<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>
