<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addchangeshift.aspx.cs" Inherits="content_hr_addshiftcodelist" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
    jQuery.noConflict();
        (function($) {
            $(function() {
                $(".datee").datepicker();
            });
        })(jQuery);
 
    </script>
<style type="text/css">
    .x-head th {font-size:10px; line-height:25px; text-transform:uppercase; color: #444}
    .x-head td { padding:2px}
    .x_panel textarea {border:1px solid #eee;width:100%}
</style>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Change Shift Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Mchangeshiftlist?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-bug"></i> Change Shift</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Change Shift Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
     <div class="col-md-9">
        <div class="x_panel">
             <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="emp_id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="employee" HeaderText="EMPLOYEE"/>
                    <asp:BoundField DataField="Date" HeaderText="DATE"/>
                  <%--  <asp:BoundField DataField="remarks" HeaderText="Remarks" />--%>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shiftcode"/>
                    <asp:BoundField DataField="shiftcode_id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="delete_row_datatable"   Text="del" ><i class="fa fa-close"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="10px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <ul class="input-form"> 
                  <li> <asp:TextBox ID="txt_remarks" runat="server" TextMode="MultiLine" placeholder="remarks"></asp:TextBox></li>
            </ul>
            
            <hr />
            <asp:Button ID="Button2" runat="server" Visible="false" OnClick="click_save_changeshift" Text="SAVE" CssClass="btn btn-primary" />
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <ul class="input-form">
            <li style=" display:none;">Payroll Group</li>
            <li style=" display:none;"><asp:DropDownList ID="ddl_payroll" runat="server" CssClass="minimal" OnSelectedIndexChanged="click_pg" AutoPostBack="true" ></asp:DropDownList></li>
            <li>Employee</li>
            <li><asp:DropDownList ID="ddl_employee" runat="server" CssClass="minimal"></asp:DropDownList></li>
            <li>Date From</li>
            <li><asp:TextBox ID="txt_csd" runat="server" cssclass="datee" ClientIDMode="Static"></asp:TextBox></li>
            <li>Date To</li>
            <li><asp:TextBox ID="txt_to" runat="server" cssclass="datee" ClientIDMode="Static"></asp:TextBox></li>
            <li>Shift Code</li>
            <li><asp:DropDownList ID="ddl_shiftcode" runat="server" CssClass="minimal"></asp:DropDownList></li>
           <%-- <li>Remarks</li>
            <li><asp:TextBox ID="txt_lineremarks" runat="server"></asp:TextBox></li>--%>
           
            </ul>
            <asp:Button ID="Button1" runat="server" OnClick="add_datatable" Text="ADD" CssClass="btn btn-primary"/>
            <hr />
        </div>
    </div>

</div>
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
</asp:Content>
