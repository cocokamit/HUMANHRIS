<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OLEDB.aspx.cs" Inherits="temp_OLEDB"  MasterPageFile="~/content/site.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        textarea{ width:100%}
    </style>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>OleDB</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">OleDB</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-primary">
                <div class="box-body table-responsive">
                    <div class="form-group">
                        <asp:TextBox ID="tb_query" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btn_execute" runat="server" Text="Submit" OnClick="click_execute" CssClass="btn btn-primary" />
                    <asp:GridView ID="grid" runat="server" CssClass="table table-bordered table-hover dataTable"></asp:GridView>
                </div>
            </div>
        </div>
    </div>
</section>
 
<asp:HiddenField ID="hf_id" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
</asp:Content>
