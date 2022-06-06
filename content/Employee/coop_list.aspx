<%@ Page Language="C#" AutoEventWireup="true" CodeFile="coop_list.aspx.cs" Inherits="content_Employee_coop_list"  MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_coop_list">
<section class="content-header">
    <h1>Loan</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Loan</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
    <div class="box box-primary">
    <div class="box-body table-responsive">
        <div id="div_msg" runat="server" class="alert alert-default">
            <i class="fa fa-info-circle"></i>
            <span>No record found</span>
        </div>
        <asp:GridView ID="grid_view" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered no-margin">
          <Columns>
            <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="OtherDeduction" HeaderText="LOAN Type" />
            <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" />
            <asp:BoundField DataField="interest" HeaderText="Interest Amount" />
            <asp:BoundField DataField="MonthlyAmortization" HeaderText="MonthlyAmortization" />
            <asp:BoundField DataField="status" HeaderText="Status" />
            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
            <asp:TemplateField>
              <ItemTemplate>
                <asp:LinkButton ID="lnk_view" runat="server"  Text="view"  OnClick="view"><i class="fa fa-sliders"></i></asp:LinkButton>
              </ItemTemplate>
              <ItemStyle Width="40px" />
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
    </div>
    </div>
    </div>
</div>
</section>

<asp:Button ID="btn_add" style="border:1px solid silver; padding:5px 15PX; float:left; margin:5px 0 0 -10px; font-size:12px" runat="server" Visible="False"  OnClick="addOT" Text="ADD" />
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="LinkButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <div class="overflow">
    <asp:GridView ID="grid_bal" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
      <Columns>
        <%--<asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>--%>
        <asp:BoundField DataField="PayrollDate" HeaderText="Date Pay" DataFormatString="{0:MM/dd/yyyy}"  />
        <asp:BoundField DataField="Amount" HeaderText="Credit" />
        <asp:BoundField DataField="balance" HeaderText="Balance" />
      </Columns>
    </asp:GridView>
    </div>
</div>
</asp:Content>

