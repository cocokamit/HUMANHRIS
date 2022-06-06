<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addleaveapplication.aspx.cs" Inherits="content_hr_addleaveapplication" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $("#txt_to").datepicker();
            $("#txt_from").datepicker();
            $("#txt_date").datepicker();
        });
    })(jQuery);
    function checkOnlyOne(b) {
        var x = document.getElementsByClassName('checks');
        var i;
        for (i = 0; i < x.length; i++) {
            if (x[i].value != b) x[i].checked = false;
        }
    }
</script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
<style type="text/css">
    .x-head label { margin:0 5px}
    .pre {margin-top:10px;width:320px; float:left; margin-right:15px}
    .pre select { width:100%}
    .pre textarea{ width:100%}
    .pre li { margin-left:-40px; line-height:25px}
    .gs { padding-left:15px; float:left; width: calc(100% - 350px); margin:32px 0 0; border-left:1px solid #eee}
</style>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Leave Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="procpay?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-bug"></i> Leave Summary</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Leave Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <div style="float:left">
                    <asp:RadioButton ID="rb_range" GroupName="t" ClientIDMode="Static" onclick="get()" runat="server" Text=" Whole Day" Checked/>
                    <asp:RadioButton ID="rb_half" GroupName="t" onclick="get()" runat="server" ClientIDMode="Static"   Text=" Half Day"/>
                </div>
                <div class="clearfix"></div>
                <ul class="pre">
                    <li>Employee</li>
                    <li><asp:DropDownList ID="ddl_employee" runat="server" OnSelectedIndexChanged="click_emp" AutoPostBack="true"></asp:DropDownList></li>
                    <li>Leave Type</li>
                    <li><asp:DropDownList ID="ddl_leave" runat="server" DataSourceID="sql_leave" DataTextField="Leave" DataValueField="Id"></asp:DropDownList></li>
                     <li>Date Range</li>
                    <li>
                        <asp:TextBox ID="txt_from" Placeholder="From"   runat="server"  AutoComplete="off" ClientIDMode="Static"></asp:TextBox>
                        <asp:TextBox ID="txt_to" runat="server" Placeholder="To"  AutoComplete="off" ClientIDMode="Static"></asp:TextBox>
                    </li>
                    <li>Remarks</li>
                    <li><asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine"></asp:TextBox></li>
                </ul>
                <div class="gs">
                    <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                        <Columns>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                            <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                            <asp:BoundField DataField="LeaveType" HeaderText="LeaveType"/>
                            <asp:BoundField DataField="yearlytotal" HeaderText="Yearly Total"/>
                            <asp:BoundField DataField="leave_bal" HeaderText="Balance"/>
                             <asp:BoundField DataField="leavestatus" HeaderText="Leave Status"/>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="grid_leave" runat="server" ClientIDMode="Static" AutoGenerateColumns="false" CssClass="Grid">
                        <Columns>
                            <asp:BoundField DataField="date" HeaderText="Date"/>
                            <asp:BoundField DataField="noh" HeaderText="No of Days"/>
                            <asp:BoundField DataField="pay" HeaderText="Is with pay"/>
                        </Columns>
                </asp:GridView>
                </div>
                <div class="clearfix"></div>
                <hr />
                <asp:Button ID="Button2" runat="server" OnClick="click_veirfy" Text="Verify"  CssClass="btn btn-warning" />
                    <asp:Button ID="Button1" runat="server" OnClick="click_save" Text="Save" CssClass="btn btn-primary right" />
            </div>
        </div>
    </div>
</div>
 





       <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />


<asp:SqlDataSource ID="sql_payroll_group" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MPayrollGroup order by id desc">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_period" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from Mperiod order by id desc">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_leave" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MLeave order by id desc">
</asp:SqlDataSource>
</asp:Content>