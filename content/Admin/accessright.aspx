<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="accessright.aspx.cs" Inherits="content_Admin_accessright" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
         .checkbox input[type="checkbox"], .checkbox-inline input[type="checkbox"], .radio input[type="radio"], .radio-inline input[type="radio"] { margin-left:-20px !important}
        .checkbox { margin:0 !important}
    .nobel {content: "\f0c9"; padding:9px 12px !important}
    .form-control { box-shadow:none; border-radius:3px}
    .table-bordered td a { font-size:11px}
    
    
    .modal {background: rgba(0,0,0,0.3); overflow:auto}
    .modal .modal-content {border-radius:0}
    
    
    .pagination-ys {
    /*display: inline-block;*/
    padding-left: 0;
    margin: 20px 0;
    border-radius: 4px;
}

.pagination-ys table > tbody > tr > td {
    display: inline;
}

.pagination-ys table > tbody > tr > td > a,
.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;
    color: #3c8dbc;
    background-color: #ffffff;
    border: 1px solid #dddddd;
    margin-left: -1px;
}

.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;    
    margin-left: -1px;
    z-index: 2;
    color: #aea79f;
    background-color: #f5f5f5;
    border-color: #dddddd;
    cursor: default;
}

.pagination-ys table > tbody > tr > td:first-child > a,
.pagination-ys table > tbody > tr > td:first-child > span {
    margin-left: 0;
    border-bottom-left-radius: 4px;
    border-top-left-radius: 4px;
}

.pagination-ys table > tbody > tr > td:last-child > a,
.pagination-ys table > tbody > tr > td:last-child > span {
    border-bottom-right-radius: 4px;
    border-top-right-radius: 4px;
}

.pagination-ys table > tbody > tr > td > a:hover,
.pagination-ys table > tbody > tr > td > span:hover,
.pagination-ys table > tbody > tr > td > a:focus,
.pagination-ys table > tbody > tr > td > span:focus {
    color: #97310e;
    background-color: #eeeeee;
    border-color: #dddddd;
}
.pagination-ys table td a:last-child { padding-right:12px !important}
    </style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Acces Rights</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <a href="access" class="pull-right" style="padding:8px 8px 0"><i class="fa fa-refresh"></i></a>
                <div class="input-group" style=" width:300px">
                    <asp:TextBox ID="tb_search" runat="server" class="form-control" AutoPostBack="true" placeholder="Search" OnTextChanged="click_search"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="b_search" runat="server" CssClass="btn btn-primary" placeholder="Search" Text="GO" OnClick="click_search" />
                    </span>
                </div>
                
            </div>
            <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                <h5><i class="icon fa fa-info-circle"></i> No record found</h5>
            </div>
            <asp:GridView ID="grid_view" AllowPaging="True"  PageSize="10" OnPageIndexChanging="grid_view_PageIndexChanging" CssClass="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="false">
                <PagerStyle CssClass="pagination-ys" />   
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="acc_id" HeaderText="acc_id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" HeaderText="emp_id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                     <asp:TemplateField HeaderText="Employee">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" Text='<%# Eval("name") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="acc_name" HeaderText="Account"/>
                    <asp:BoundField DataField="username" HeaderText="Username"/>
                    <asp:BoundField DataField="password" HeaderText="Password"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<div id="modal" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Employee Rights</h4>
            </div>
            <div class="modal-body">
              <asp:Panel ID="p_details" runat="server">
                <div class="form-group">
                    <label>Employee</label> <asp:Label ID="l_user" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:Label ID="l_employee" runat="server" CssClass="form-control" autocomplete="off"></asp:Label>
                    <asp:DropDownList ID="ddl_user" CssClass="select2" runat="server" Visible="false">
                        <asp:ListItem Value="" disabled selected hidden></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Role </label> <asp:Label ID="lRoute" ForeColor="Red" runat="server"></asp:Label>
                     <asp:DropDownList ID="ddlRole" CssClass="select2" runat="server" >
                        <asp:ListItem Value="3">Member</asp:ListItem>
                        <asp:ListItem Value="10">Scheduler</asp:ListItem>
                        <asp:ListItem Value="5">Approver and Scheduler</asp:ListItem>
                    </asp:DropDownList>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="form-group">
                    <label>Username</label> <asp:Label ID="l_username" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_username" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                </div>
                <div class="form-group no-margin">
                    <label>Password</label> <asp:Label ID="l_password" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_password" runat="server" CssClass="form-control"  autocomplete="off"></asp:TextBox>
                </div>
              </asp:Panel>
              <asp:Panel ID="p_routes" runat="server" Visible="false" style=" margin-top:15px">
                <div class="form-group no-margin">
                    <label>Access Rights</label> <asp:Label ID="Label2" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_addroute" runat="server" OnClick="click_addroutes" CssClass="fa fa-pencil pull-right"></asp:LinkButton>
                    <asp:GridView ID="gv_right" CssClass="table table-bordered table-hover dataTable no-margin table-align-top" EmptyDataText="No route found" OnRowDataBound="gv_rightsBound" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="route_id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="cnt" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="nobelroute_id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <asp:Label ID="l_lable" runat="server" Text='<%# Eval("route") %>' ></asp:Label>
                                    <small style=" padding-left:5px">(<asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' ></asp:Label>)</small>

                            <asp:Panel ID="p_tree" runat="server">
                             <asp:GridView ID="gv_treechild" runat="server" OnRowDataBound="gv_treechildBound" CssClass="table table-bordered table-hover dataTable no-margin" ShowHeader="false" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                                    <asp:BoundField DataField="status" HeaderText="status" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                            <asp:LinkButton ID="lb_delete" runat="server" OnClick="click_TreeChild" style=" padding:8px"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                              </asp:GridView>
                            </asp:Panel>


                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding:7px 0 0">
                                        <asp:LinkButton ID="lnkUp" CssClass="fa fa-chevron-circle-up" CommandArgument = "up"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDown" CssClass="fa fa-chevron-circle-down" CommandArgument = "down"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                        <asp:LinkButton ID="lb_delete" runat="server" OnClick="deleterights" CssClass="fa fa-times-circle"></asp:LinkButton>
                                     </div>
                                </ItemTemplate>
                                <ItemStyle Width="95"/>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </asp:Panel>
                <asp:Panel ID="p_sys_routes" runat="server" Visible="false" style=" margin-top:0px">
                    <div class="form-group no-margin">
                    <label>Routes Collection</label> <asp:Label ID="Label3" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_back" runat="server" OnClick="click_addback" CssClass="fa fa-arrow-circle-left pull-right"> <span class="lable">Back</span></asp:LinkButton>
                    <asp:GridView ID="gv_route" CssClass="table table-bordered table-hover dataTable no-margin" OnRowDataBound="gv_routeBound" EmptyDataText="No route available" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-CssClass="nones" ItemStyle-CssClass="nones"/>
                            <asp:BoundField DataField="treeview" HeaderText="treeview" HeaderStyle-CssClass="nones" ItemStyle-CssClass="nones"/>
                            <asp:BoundField DataField="pick" HeaderText="pick" HeaderStyle-CssClass="nones" ItemStyle-CssClass="nones"/>
                            <asp:BoundField DataField="exist"  HeaderText="exist" HeaderStyle-CssClass="nones" ItemStyle-CssClass="nones"/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="cb" runat="server" />
                                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                            (<asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' ></asp:Label>)
                                        </label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div class="modal-footer">
                <asp:Button ID="Button3" runat="server" Text="Save" OnClick="click_save" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hf_id" runat="server" />
<asp:HiddenField ID="hf_employee" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
<asp:HiddenField ID="hf_account" runat="server" />
<script type="text/javascript">
    function Confirm(msg) {
        var confirm_value = document.getElementById("<%= hf_action.ClientID %>")
        if (confirm("Are you sure to delete examination?"))
            confirm_value.value = "2";
        else
            confirm_value.value = "";
    }
</script>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
    <script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $('.select2').select2()
        })(jQuery);
    </script>

</asp:Content>
 

