<%@ Page Language="C#" AutoEventWireup="true" CodeFile="empList.aspx.cs" Inherits="content_supervisor_empList" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="conten_leaveList">
<%--<section class="content-header">
    <h1>Employee list</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Employee list</li>
    </ol>
</section>--%>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive no-pad-top">
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false"  EmptyDataText="No record found" CssClass="table table-bordered no-margin">
                <Columns>
                 <%--   <asp:BoundField DataField="empp" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />--%>
                    <asp:BoundField DataField="fullname" HeaderText="Employee"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>

                         <a href="apprform?&key=<%# function.Encrypt(Eval("empp").ToString(), true)%>" target="_new" title="Appraise"  class="border-right" ><i class="glyphicon glyphicon-info-sign"></i></a>
                            <asp:LinkButton ID="lnk_appdet" CssClass="fa fa-clipboard sm" CommandName='<%# Eval("empp") %>' OnClick="viewdetails"  ToolTip="Appraisal Details" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="95px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>
<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
<div style=" height:350px; overflow:auto">
    <b>Appraisal Details</b>
      <asp:GridView ID="grid_det" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found!" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="date" HeaderText="Date"/>
                    <asp:BoundField DataField="purpose" HeaderText="Purpose of Appraisal"/>
                    <asp:BoundField DataField="recommend" HeaderText="Recommendation"/>
                    <asp:BoundField DataField="totalratings" HeaderText="Score"/>
                                
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                         <asp:LinkButton ID="lnk_print" CssClass="fa fa-clipboard sm" CommandName='<%# Eval("id") %>' OnClick="click_print"  ToolTip="Print" runat="server"><i class="fa fa-print"></i></asp:LinkButton>

                        <%--<a href="prntapp?&key=<%# Eval("id").ToString()%>" target="_new" title="Appraise"  class="border-right" ><i class="fa fa-print"></i></a>--%>
                        <%-- Response.Redirect("prntapp?key=" + dtinsappform.Rows[0]["trnid"].ToString() + "");--%>
                        <%--  <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' ><i class="fa fa-print"></i></asp:LinkButton>--%>
                        <%--<asp:LinkButton ID="lnk_can" runat="server" OnClientClick="Confirm()" OnClick="candetails"  CommandName='<%# Eval("id") %>' Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>--%>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
         </div>   
</div>
<asp:HiddenField ID="id" runat="server" />
</asp:Content>