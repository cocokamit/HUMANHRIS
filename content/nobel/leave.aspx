<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="leave.aspx.cs" Inherits="content_nobel_leave" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <h1>Leave Management</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">Leave Management</li>
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
                    <asp:LinkButton ID="lb_add" runat="server" OnClick="CreateLeave" CssClass="btn btn-block btn-primary">Add</asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive padding no-pad-top">
                <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:120px; margin:15px; font-size:11px; " ></asp:Label>
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <h5><i class="icon fa fa-check-circle"></i> No record found</h5>
                </div>
                <asp:GridView ID="gv_data" runat="server" AutoGenerateColumns="false" OnRowDataBound="dataBound" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="LeaveType" HeaderText="Leave Type"/>
                        <asp:BoundField DataField="Leave" HeaderText="Description"/>
                        <asp:BoundField DataField="yearlytotal" HeaderText="Yearly Total"/>
                        <asp:BoundField DataField="converttocash" HeaderText="Convert To Cash"/>
                        <asp:BoundField DataField="action" HeaderText="Status" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                 <asp:Label ID="l_status" runat="server" CssClass="label label-danger text-capitalize" Text='<%# Eval("action") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" Tooltip="Edit" CssClass="fa fa-pencil"></asp:LinkButton>
                                <asp:LinkButton ID="lb_action" OnClick="action" runat="server" ></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="80" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <a href="sys-importleave"><i class="fa fa-cloud-upload" ></i> Import file</a>
            </div>
          </div>
        </div>
      </div>
</section>

<div class="modal fade in" id="modal" runat="server">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="lb_close" runat="server" OnClick="CloseModal" CssClass="close"><span aria-hidden="true">×</span></asp:LinkButton>
                <h4 id="modal_title" runat="server" class="modal-title"></h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Type</label>
                    <asp:Label ID="l_type" runat="server" CssClass="text-red"></asp:Label>
                    <asp:TextBox ID="txt_leavetype" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Description</label>
                    <asp:TextBox ID="txt_leavedescription" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Credit</label>
                    <asp:TextBox ID="txt_leavetotal" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:CheckBox ID="check_leave" runat="server" Text="Convert to Cash" CssClass="checkbox" />
                </div>

            </div>
            <div class="modal-footer">
                <asp:Button ID="btn_save" OnClick="Submit" runat="server" Text="Submit" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hf_id" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

