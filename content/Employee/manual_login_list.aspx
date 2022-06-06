<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manual_login_list.aspx.cs" Inherits="content_Employee_manual_login_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="conten_manual_login" runat="server">
<section class="content-header">
    <h1>Time Adjustment</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Time Adjustment</li>
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
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" AutoGenerateColumns="false" CssClass="table table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="sysdate" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="date" HeaderText="Manual" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="timeinf" HeaderText="Log In" DataFormatString="{0:MM/dd/yyyy hh:mm: tt}"/>
                        <asp:BoundField DataField="timeOUT1f" HeaderText="Time Out 1" DataFormatString="{0:MM/dd/yyyy hh:mm: tt}" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="timein1f" HeaderText="Time In 2" DataFormatString="{0:MM/dd/yyyy hh:mm: tt}" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="timeoutf" HeaderText="Log Out" DataFormatString="{0:MM/dd/yyyy hh:mm: tt}"/>
                        <asp:BoundField DataField="manual_type" HeaderText="Reason"/>
                        <asp:BoundField DataField="note" HeaderText="Notes"/>
                        <asp:BoundField DataField="Remarks" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" HeaderText="Notes Admin"/>
                        <asp:BoundField DataField="status" HeaderText="Status"/>  
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_can" OnClick="view" Text="cancel" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
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

 
 <asp:Button ID="btn_add" runat="server" OnClick="addManual" Text="ADD" Visible="false" CssClass="btn btn-primary" />

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
     <div class="box-body table-responsive no-pad-top">
        <div class="form-group">
            <label>Reason</label>
            <asp:Label ID="lbl_re" style=" color:Red;" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <asp:Button ID="btn_save" runat="server" OnClick="cancel" Text="Save" CssClass="btn btn-primary" />
</div>
    <asp:HiddenField ID="id" runat="server" />
</asp:Content>
