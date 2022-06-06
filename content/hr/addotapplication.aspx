<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addotapplication.aspx.cs" Inherits="content_hr_addotapplication" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
 <script type="text/javascript">
     jQuery.noConflict();
     (function($) {
         $(function() {
             $(".datee").datepicker({ changeMonth: true,
                 yearRange: "-100:+0",
                 changeYear: true
             });
         });
     })(jQuery);
    </script>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>OT Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Motapplication?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-bug"></i> OT Summary</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>OT Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <ul class="input-form">
                <li>Employee</li>
                <li><asp:DropDownList ID="ddl_employee" runat="server"></asp:DropDownList></li>
                <li>Date</li>
                <li><asp:TextBox ID="txt_otd" runat="server" AutoComplete="off" CssClass="datee" ></asp:TextBox></li>
                <li>OT HRS</li>
                <li><asp:TextBox ID="txt_OT" runat="server" onkeyup="intinput(this)" MaxLength="1"></asp:TextBox></li>
                <li>OT Night HRS</li>
                <li><asp:TextBox ID="txt_night_ot" runat="server" onkeyup="intinput(this)" MaxLength="1"></asp:TextBox></li>
                <li>Remarks</li>
                <li><asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine"></asp:TextBox></li>
            </ul>
            <hr />
            <asp:Button ID="Button2" runat="server" OnClick="click_save_ot" Text="SAVE" CssClass="btn btn-primary"/>
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