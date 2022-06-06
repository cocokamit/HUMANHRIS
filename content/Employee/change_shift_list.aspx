<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_shift_list.aspx.cs" Inherits="content_Employee_change_shift_list"  MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
     <style type="text/css">
        .table select {border:none;-webkit-appearance: none;-moz-appearance: none;text-indent: 1px;text-overflow: '';}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_csrl">
<section class="content-header">
    <h1>Schedule</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Schedule</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm" style="width: 180px;">
                      <asp:DropDownList ID="ddl_dtr" OnSelectedIndexChanged="pak" AutoPostBack="true" runat="server" CssClass="select">
                        <asp:ListItem Value="0">-</asp:ListItem>
                      </asp:DropDownList>
                </div>
                <div class="box-tools box-tool-add">
                     <asp:Button ID="Button1" runat="server" Text="Change Shift" OnClick="adders"  CssClass="btn btn-primary" />
                </div>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <div id="alert" runat="server" visible="false" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="true" OnRowDataBound="grid_hide" CssClass="table table-bordered no-margin table-schedule" >
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>
</asp:Content>



