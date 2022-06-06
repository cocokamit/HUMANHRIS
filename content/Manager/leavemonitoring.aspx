<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="leavemonitoring.aspx.cs" Inherits="content_Manager_leavemonitoring" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <h1>Leave Monitoring</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li><a href="javascript:void(0)"> Report</a></li>
    <li class="active">Leave Monitoring</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <asp:DropDownList ID="ddl_yyyy" runat="server" CssClass="select" AutoPostBack="true" OnTextChanged="btn_search" Width="70">
                </asp:DropDownList>
            </div>
            <div class="box-body table-responsive" style="padding-top:0 !important">
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                 </div>
                 <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="False" OnRowDataBound="rowbound" ShowHeader="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="IdNumber" HeaderText="Id Number" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="FullName" HeaderText="Employee" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:TemplateField >
                            <ItemTemplate> 
                                 <%# Eval("FullName") %> 
                                 <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" EmptyDataText="No data found." CssClass="table table-striped table-bordered no-margin" style="margin-top:5px !important">
                                    <Columns>
                                        <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField DataField="leave" HeaderText="Leave Type"/>
                                        <asp:BoundField DataField="credit" HeaderText="Allowed Credit"  DataFormatString="{0:n2}"/>
                                        <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:n2}"/>
                                    </Columns>
                                 </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                   
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>

<div id="modal" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        </div>
        <div class="modal-body">
          
            <div class="modal-header">
                <h4 class="modal-title">Leave History</h4>
            </div>
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Date" HeaderText="Date Leave" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="Leave" HeaderText="Leave Type" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="withpay" HeaderText="With Pay" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>

</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

