<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="content_Manager_Default" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="TextBox5" runat="server" class="form-control" AutoPostBack="true" placeholder="Search"></asp:TextBox>
                    <div class="input-group-btn">
                        <asp:LinkButton ID="lb_search" runat="server" class="btn btn-default"><i class="fa fa-search"></i></asp:LinkButton>
                    </div>
                </div>
                <div class="box-tools box-tool-add">
                    <asp:LinkButton ID="lb_add" runat="server" CssClass="btn btn-block btn-primary">Add</asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive padding">
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                <i class="icon fa fa-check-circle"></i>
                <span>No record found</span>
                </div>
            </div>
        </div>
    </div>
</div>
</section>
</asp:Content>

