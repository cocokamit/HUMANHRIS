<%@ Page Language="C#" AutoEventWireup="true" CodeFile="create_schedule.aspx.cs" Inherits="content_Scheduler_create_schedule" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .table select {border:none;-webkit-appearance: none;-moz-appearance: none;text-indent: 1px;text-overflow: '';}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_schedule">
<section class="content-header">
    <h1>Scheduler</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Scheduler</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm">
                    <asp:DropDownList ID="ddl_dtr" OnSelectedIndexChanged="pak" CssClass="selecta" AutoPostBack="true" runat="server" ></asp:DropDownList> 
                </div>
            </div>
             <div class="box-body table-responsive no-pad-top">
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="true" OnRowDataBound="grid_hide" CssClass="table table-bordered no-margin">
                </asp:GridView>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button1" runat="server" Text="Save" onclick="Button1_Click" CssClass="btn btn-primary" />
            </div>
          </div>
        </div>
    </div>
</section>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
</asp:Content>

