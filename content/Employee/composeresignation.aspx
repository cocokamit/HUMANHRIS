<%@ Page Language="C#" AutoEventWireup="true" CodeFile="composeresignation.aspx.cs" Inherits="content_Employee_composeresignation" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
    <link rel="stylesheet" href="vendors/bootstrap-wysihtml5/bootstrap3-wysihtml5.css">
    <style type="text/css">
        a[data-wysihtml5-command=insertImage] { display:none}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="conten_leaveList">
    <section class="content">
      <div class="row">
        <div class="col-md-12">
          <div class="box box-primary">
            <div class="box-header with-border">
              <h3 class="box-title">Compose Resignation</h3>
            </div>
            <div class="box-body">
              <div class="form-group">
                <asp:textbox runat="server" ID="composetextarea" TextMode="MultiLine" ClientIDMode="Static" class="form-control" style=" height: 300px" />
              </div>
              <div class="form-group">
              <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="Save_Resignation" CssClass="btn btn-primary" />
              <asp:Label ID="lbl_errmsg" runat="server"  ForeColor="Red"></asp:Label>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script src="vendors/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.js"></script>
<script>
    $(function () {
        $('#composetextarea').wysihtml5()
    })
</script>
</asp:Content>
