<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestUpdate.aspx.cs" Inherits="content_Employee_RequestUpdate" MasterPageFile="~/content/site.master"%>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="conten_manual_login" runat="server">
<section class="content-header">
    <h1>Master File Request</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Masterfile Update</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="Failed" HeaderText="Field"/>
                        <asp:BoundField DataField="Request" HeaderText="Request"/>
                        <asp:BoundField DataField="DateRequest" HeaderText="Date Requested"/>
                        <asp:TemplateField HeaderText="Status">
                          <ItemTemplate>
                            <asp:LinkButton ID="lnkbtncontent" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("Status") %>' OnClick="RowEventUpdate"></asp:LinkButton>
                          </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>
</asp:Content>

