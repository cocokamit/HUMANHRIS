<%@ Page Language="C#" AutoEventWireup="true" CodeFile="oicAdjustment.aspx.cs" Inherits="content_Admin_oicAdjustment"
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
            <h3>OIC Adjustment</h3>
        </div>
    </div>
    </section>
    <section class="content"><%--
       <div class="row">
            <div class="col-md-12">
               <asp:LinkButton ID="btn_compose" runat="server" OnClick="editor" Text="Compose +" CssClass="btn btn-primary pull-right" AutoPostBack="True"></asp:LinkButton>

            </div>
              </div> 
              <br />--%>
       <div class="x_panel">

              <br />
                <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                 <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" ClientIDMode="AutoID">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>                     
                        <asp:BoundField DataField="tdate" HeaderText="Date Start"/>
                        <asp:BoundField DataField="tDateEnd" HeaderText="Date End"/>
                        <asp:BoundField DataField="appfullname" HeaderText="Approver"/>
                        <asp:BoundField DataField="oicfullname" HeaderText="OIC"/>
                        
                        <asp:BoundField DataField="status" HeaderText="Status"/>
 
                        <asp:TemplateField  HeaderText="Action">
                            <ItemTemplate>
                                     <asp:LinkButton ID="viewbtn" runat="server" OnClick="editor" CssClass="fa fa-pencil" AutoPostBack="True"></asp:LinkButton>
                       
                                 
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
                        Edit</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                   <label>Current OIC</label>
                                   <asp:Label ID="lb_current" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <label>New OIC</label>
                                    <asp:DropDownList ID="ddl_new" ClientIDMode="Static" AutoComplete="off" runat="server" CssClass="select2" style="width:100%">
                                   </asp:DropDownList>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                            <asp:Button ID="btnSave" runat="server" class="btn btn-success btn-rounded mb-4 pull-right"
                                                Text="Save" OnClick="saveform" OnClientClick="return confirm('Are you sure you want to save?')"
                                                ClientIDMode="Static" />
                                        </div>
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

<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $('.select2').select2()
        })(jQuery);
   
    </script>
</asp:Content>
