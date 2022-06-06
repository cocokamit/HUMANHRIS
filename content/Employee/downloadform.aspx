<%@ Page Language="C#" AutoEventWireup="true" CodeFile="downloadform.aspx.cs" Inherits="content_Employee_downloadform" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="conten_leaveList">
<section class="content-header">
    <h1>Forms</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Forms</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive no-pad-top">
              <asp:GridView ID="grid_det" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="filename" HeaderText="File Name"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                         <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                         <%-- <asp:LinkButton ID="lnk_can" runat="server" OnClientClick="Confirm()" OnClick="candetails"  CommandName='<%# Eval("id") %>' Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
--%>                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>


 
<asp:HiddenField ID="id" runat="server" />
</asp:Content>
