<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rest_day_list.aspx.cs" Inherits="content_Employee_rest_day_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_rdl">
<section class="content-header">
    <h1>Work Verification</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Work Verification</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
    <div class="box box-primary">
        <div class="box-header" >
            <div class="input-group input-group-sm" style="width: 300px; display:none;">
                <asp:Button ID="btn_add" runat="server" OnClick="addOT" Text="ADD" CssClass="btn btn-primary" />
            </div>
            <div class="box-tools box-tool-add">
            </div>
        </div>
        <div class="box-body table-responsive no-pad-top">
            <div id="div_msg" runat="server" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" AutoGenerateColumns="false" CssClass="table table-bordered table-hover no-margin">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                <asp:BoundField DataField="sysdate" HeaderText="File Date" DataFormatString="{0:MM/dd/yyyy}"/>
                <asp:BoundField DataField="date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}"/>
                <asp:BoundField DataField="class" HeaderText="Type"/>
                <asp:BoundField DataField="reason" HeaderText="Reason"/>
                <asp:BoundField DataField="status" HeaderText="Status"/>
                <asp:TemplateField>
                  <ItemTemplate>
                    <asp:LinkButton ID="lnk_can" OnClick="  cancel" Text="cancel" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
                  </ItemTemplate>
                  <ItemStyle Width="40" />
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>
</section>
<div class="clearfix"></div>
</asp:Content>

