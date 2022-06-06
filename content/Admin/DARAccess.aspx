<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DARAccess.aspx.cs" Inherits="content_Admin_DARAccess"
    MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

    </script>
    <style type="text/css">
        textarea
        {
            resize: none;
        }
    </style>
</asp:Content>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="content">
    <section class="content-header">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>DAR Application Access</h3>
        </div>
    </div>
    </section>
    <section class="content">
       <div class="x_panel">
   <div class="row">
            <div class="col-md-12">
               <asp:LinkButton ID="btn_compose" runat="server" OnClick="editor" Text="Add +" CssClass="btn btn-primary pull-right" AutoPostBack="True"></asp:LinkButton>

            </div>
              </div> 
              <br />
                <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                 <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" ClientIDMode="AutoID">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>                     
                        <asp:BoundField DataField="mcaddress" HeaderText="MAC Address"/>
                        <asp:BoundField DataField="location" HeaderText="Location"/>
                          <asp:TemplateField  HeaderText="Status">
                            <ItemTemplate>
                                     <asp:Label ID="lb_approved" runat="server" CssClass="label label-success" Text="allowed" Visible='<%# Eval("status").ToString()=="allow"? true:false%>'> </asp:Label>
                                     <asp:Label ID="Label1" runat="server" CssClass="label label-danger" Text="Disabled" Visible='<%# Eval("status").ToString()!="allow"? true:false%>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderText="Action">
                            <ItemTemplate>
                                   <asp:LinkButton ID="viewbtn" runat="server" OnClick="editor" CssClass="fa fa-pencil" AutoPostBack="True"></asp:LinkButton>
                                   <asp:LinkButton ID="delbtn" runat="server" OnClick="editor" CssClass="fa fa-trash" AutoPostBack="True" ClientIDMode="Static" OnClientClick="return confirm('Are you sure you want to remove this device?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
       </div>
     </section>
    <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 500px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Add Device</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                <label>Device MAC Address</label>
                                    <asp:TextBox ID="txtmc" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                
                                <label>Set Location</label>
                                    <asp:TextBox ID="txtlocation" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:CheckBox ID="chkallow" runat="server" Text="Allow Access" CssClass="checkbox checkbox-inline" ClientIDMode="Static" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success pull-right" Text="Save" ClientIDMode="Static" OnClick="save_device" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>


        <div id="viewForm" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 500px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Edit Device</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel2" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                <label>Device MAC Address</label>
                                    <asp:TextBox ID="txt_mc" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                
                                <label>Set Location</label>
                                    <asp:TextBox ID="txt_loc" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:CheckBox ID="chk_allow" runat="server" Text="Allow Access" CssClass="checkbox checkbox-inline" ClientIDMode="Static" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btn_edit" runat="server" CssClass="btn btn-success pull-right" Text="Edit" ClientIDMode="Static" OnClick="edit_device" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
</asp:Content>
