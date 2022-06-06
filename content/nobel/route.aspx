<%@ Page Language="C#" AutoEventWireup="true" CodeFile="route.aspx.cs" Inherits="content_su_route" MasterPageFile="~/content/site.master"%>
 
 <asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
     <style type="text/css">
       .disabled {   color: gray !important}
     </style>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>Route</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">Route</li>
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
                <asp:GridView ID="grid_view" CssClass="table table-bordered table-hover dataTable" runat="server" OnRowDataBound="gvRouteBound" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                        <asp:BoundField DataField="name" HeaderText="Route"/>
                        <asp:BoundField DataField="url" HeaderText="URL"/>
                        <asp:BoundField DataField="path" HeaderText="Path"/>
                        <asp:BoundField DataField="icon" HeaderText="Icon"/>
                        <asp:BoundField DataField="description" HeaderText="Description"/>
                        <asp:TemplateField HeaderText="Treeview">
                            <ItemTemplate>
                                <asp:LinkButton ID="lb_cnt" runat="server" Text='<%# Eval("cnt") %>' OnClick="clickMultiRoutes"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" CssClass="fa fa-edit border-right"></asp:LinkButton>
                                 <asp:LinkButton ID="lb_transfer" runat="server" OnClick="clickAddMultiRout" CssClass="fa fa-share border-right"></asp:LinkButton>
                                <asp:LinkButton ID="lnk_delete" OnClientClick="Confirm(this,'delete')" runat="server" CssClass="fa fa-trash no-border no-border no-pad-right"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="120px" CssClass="action" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
</section>

<div id="modal_subs" runat="server" class="modal fade in">
    <div class="modal-dialog" style=" width:1000px !important">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_CloseMulti" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Multilevel Route</h4>
            </div>
            <div class="modal-body">
                  <asp:GridView ID="gv_Subs" CssClass="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                            <asp:BoundField DataField="name" HeaderText="Route"/>
                            <asp:BoundField DataField="url" HeaderText="URL"/>
                            <asp:BoundField DataField="path" HeaderText="Path"/>
                            <asp:BoundField DataField="icon" HeaderText="Icon"/>
                            <asp:BoundField DataField="description" HeaderText="Description"/>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" CssClass="fa fa-edit border-right"></asp:LinkButton>
                                     <asp:LinkButton ID="lb_transfer" runat="server" OnClick="clickRedoMultiRoute" CssClass="fa fa-mail-reply (alias)"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="90px" CssClass="action" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </div>
        </div>
    </div>
</div>

<div id="modal_transfer" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Multilevel Route</h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Route Collection</label> <asp:Label ID="Label1" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:DropDownList ID="ddlRoutes" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="Button1" runat="server" Text="Save" OnClick="click_TransferSave" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</div>  

<div id="modal" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Create Route</h4>
            </div>
            <div class="modal-body">
                <div class="form-group pull-right">
                    <asp:LinkButton ID="lb_treeview" runat="server" OnClick="click_treeview" CssClass="fa fa-toggle-off"> <span class="lable">Tree View</span></asp:LinkButton>
                    
                </div>
                <div class="form-group">
                    <label>Name</label> <asp:Label ID="l_name" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_name" runat="server" autocomplete="off" CssClass="form-control"></asp:TextBox>
                </div>
                 <div class="form-group">
                    <label>Icon</label> <asp:Label ID="l_con" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_icon" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Panel ID="p_action" runat="server">
                    <div class="form-group">
                        <label>URL</label> <asp:Label ID="l_url" ForeColor="Red" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="tb_url" runat="server" autocomplete="off" CssClass="form-control"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label>Physical Path</label> <asp:Label ID="l_phy" ForeColor="Red" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="tb_phy" runat="server" autocomplete="off" CssClass="form-control"></asp:TextBox>
                    </div>
                </asp:Panel>
                <div class="form-group no-margin">
                    <label>Description</label> <asp:Label ID="l_description" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_description" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
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
    function Confirm(msg) {
        var confirm_value = document.getElementById("<%= hf_action.ClientID %>")
        if (confirm("Are you sure to delete examination?"))
            confirm_value.value = "2";
        else
            confirm_value.value = "";
    }
</script>
</asp:Content>