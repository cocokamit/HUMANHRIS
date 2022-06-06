<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtr_period_view.aspx.cs" Inherits="content_hr_dtr_period_view" MasterPageFile="~/content/site.master"%>
 
 <asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>Scheduler</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Scheduler</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <div class="input-group input-group-sm" style="width: 300px;">
                         <h3 class="no-margin"><asp:Label ID="l_range" runat="server"></asp:Label></h3>
                    </div>
                    <div class="box-tools box-tool-add">
                        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="click_back" CssClass="glyphicon glyphicon-circle-arrow-left" Font-Size="20px" style=" margin:3px"></asp:LinkButton>
                    </div>
                </div>
                <div class="box-body table-responsive">
                    <asp:GridView ID="gv_schedule" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered no-margin schedule">
                    </asp:GridView>
                </div>
                
            </div>
        </div>
     </div>
</section>
</asp:Content>
