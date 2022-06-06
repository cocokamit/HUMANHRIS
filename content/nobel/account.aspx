<%@ Page Language="C#" AutoEventWireup="true" CodeFile="account.aspx.cs" Inherits="content_su_account" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .checkbox input[type=checkbox] {margin-left:-19px !important}
        .table-bordered td a {padding:5px !important}
    </style>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>Account</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">Account</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm" style="width: 300px;">
                     <asp:TextBox ID="TextBox5" runat="server" class="form-control pull-right" AutoPostBack="true" placeholder="Search"></asp:TextBox>
                      <div class="input-group-btn">
                        <asp:LinkButton ID="lb_search" runat="server" class="btn btn-default"><i class="fa fa-search"></i></asp:LinkButton>
                      </div>
                </div>
                <div class="box-tools box-tool-add">
                    <asp:LinkButton ID="lb_add" runat="server" OnClick="click_add"  CssClass="btn btn-block btn-primary">Add</asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive padding no-pad-top">
                <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:120px; margin:15px; font-size:11px; " ></asp:Label>
                 <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <h5><i class="icon fa fa-check-circle"></i> No record found</h5>
                 </div>
                 <asp:GridView ID="grid_view" CssClass="table table-bordered table-hover dataTable" runat="server" OnRowDataBound="dataBound" AutoGenerateColumns="false" ShowHeader="false">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                        <asp:BoundField DataField="cnt" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                        <asp:BoundField DataField="name" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                        <asp:TemplateField HeaderText="Account">
                            <ItemTemplate>
                                <asp:Label ID="l_name" runat="server" Text ='<%# Eval("name") %>'></asp:Label>
                                <asp:Label ID="l_status" runat="server" Visible="false" CssClass="label label-danger pull-right">incomplete</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" CssClass="fa fa-pencil"></asp:LinkButton>
                                <asp:LinkButton ID="lnk_delete" OnClientClick="Confirm(this,'Delete Account')" OnClick="DeleteAccount" runat="server" CssClass="fa fa-times-circle no-pad-right"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" CssClass="action" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
      </div>
</section>

<div id="modal" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Create Account</h4>
            </div>
            <div class="modal-body">
                <asp:Panel ID="p_edit" runat="server">
                <div class="form-group">
                    <label>Name</label> <asp:Label ID="l_name" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_account" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                </asp:Panel>

                <asp:Panel ID="p_collection" runat="server">
                <div class="form-group no-margin">
                    <label>Routes</label> <asp:Label ID="Label2" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_treeview" runat="server" OnClick="click_addroutes" CssClass="fa fa-pencil pull-right"></asp:LinkButton>
                     <asp:GridView ID="gv_right" CssClass="table table-bordered table-hover dataTable no-margin"  OnRowDataBound="gvRightBound"  EmptyDataText="No route found" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="tree" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="seqs" HeaderStyle-CssClass="" ItemStyle-CssClass="" ItemStyle-Width="30px"/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                    <asp:LinkButton ID="lb_tree" runat="server" CssClass="button fa fa-sitemap pull-right"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUp" CssClass="fa fa-chevron-circle-up" CommandArgument = "up"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDown" CssClass="button fa fa-chevron-circle-down" CommandArgument = "down"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lb_delete" runat="server" OnClick="deleterights" CssClass="button fa fa-times-circle"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="92"/>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </asp:Panel>


                <asp:Panel ID="p_routes" runat="server" Visible="false">
                    <div class="form-group no-margin">
                    <label>Routes Collection</label> <asp:Label ID="Label3" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_back" runat="server" OnClick="click_addback" CssClass="fa fa-arrow-circle-left pull-right"> <span class="lable">Back</span></asp:LinkButton>
                    <asp:GridView ID="gv_route" CssClass="table table-bordered table-hover dataTable no-margin" OnRowDataBound="gv_routeBound" EmptyDataText="No route available" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="treeview" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="pick" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="exist" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <div class="checkbox no-margin" >
                                        <label style=" width:100%;">
                                            <asp:CheckBox ID="cb" runat="server" />
                                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' CssClass="text-muted pull-right"></asp:Label>
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
<asp:HiddenField ID="hf_action" runat="server" />
<script type="text/javascript">
    function Confirm(elem,msg) {
        var confirm_value = document.getElementById("<%= hf_action.ClientID %>")
        if (confirm(msg + "?"))
            confirm_value.value = "2";
        else
            confirm_value.value = "";
    }
</script>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
</asp:Content>