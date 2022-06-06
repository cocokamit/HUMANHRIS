<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="leave_load.aspx.cs" Inherits="content_nobel_leave_load" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <h1>Import Leave</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-bug"></i> Home</a></li>
    <li><a href="sys-leave"> Leave Management</a></li>
    <li class="active">Import Leave</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <div id="callout" runat="server" visible="false" class="callout callout-success">
                    <h4>Success!</h4>
                    Excel was successfully imported.
                </div>
                <asp:Panel ID="pnl_process" runat="server">
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="pull-left" />
                    <asp:Button ID="Button1" runat="server" Text="Import" OnClick="process" CssClass="btn btn-primary pull-right" />
                </asp:Panel>
            </div>
          </div>
        </div>
      </div>
</section>
</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

