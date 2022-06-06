<%@ Page Language="C#" AutoEventWireup="true" CodeFile="category.aspx.cs" Inherits="content_canteen_category" MasterPageFile="~/content/MasterPageNew.master"%>
 
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
    </style>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Schedule Verification</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
            </div>
             <div class="x_content">
                <div id="grid_alert" runat="server" class="alert alert-empty no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                       <asp:BoundField DataField="dtr_id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                       <asp:BoundField DataField="status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                       <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                 <asp:LinkButton ID="LinkButton2" OnClick="click_schedule" Text='<%# Eval("period") %>' CssClass="no-border no-padding" runat="server"></asp:LinkButton>
                                 <small><i>(<%# Eval("employee")%>)</i></small>
                                 <asp:LinkButton ID="LinkButton3" Text='<%# Eval("status") %>' CssClass="label label-primary pull-right" style="padding:5px; font-size:10px" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
       
    </div>
</div>
</asp:Content>
