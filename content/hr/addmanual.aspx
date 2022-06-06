<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addmanual.aspx.cs" Inherits="content_hr_addmanual" MasterPageFile="~/content/MasterPageNew.master" %>


<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_ot_list">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript">
          jQuery.noConflict();
          (function ($) {
              $(function () {
                  $(".datee").datepicker({ changeMonth: true,
                      yearRange: "-100:+0",
                      changeYear: true
                  });
              });
          })(jQuery);
    </script>

<asp:SqlDataSource ID="sql_payroll_group" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MPayrollGroup order by id desc">
</asp:SqlDataSource>

<asp:SqlDataSource ID="sql_period" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from Mperiod order by id desc">
</asp:SqlDataSource>

<asp:SqlDataSource ID="sql_leave" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MLeave order by id desc">
</asp:SqlDataSource>

</asp:Content>


<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_addmanual">

<ul>

<li>Employee</li>
<li><asp:DropDownList ID="ddl_employee" runat="server"></asp:DropDownList></li>

<li>Date</li>
<li><asp:TextBox ID="txt_otd" runat="server" AutoComplete="off" CssClass="datee" ></asp:TextBox></li>

<li>Time In</li>
<li>
     
      <asp:DropDownList ID="ddl_hrs_in" runat="server"  Width="48px" style=" margin-left:2px">
            <asp:ListItem value="" disabled selected>HR</asp:ListItem>
      </asp:DropDownList>

      <asp:DropDownList ID="ddl_minute_in" runat="server" Width="48px" style=" margin-left:2px">
        <asp:ListItem value="" disabled selected>MM</asp:ListItem>
      </asp:DropDownList> 

    <asp:DropDownList ID="ddl_am_pm_in" runat="server" Width="49px" style=" margin-left:2px">
        <asp:ListItem Value="AM">AM</asp:ListItem>
        <asp:ListItem Value="PM">PM</asp:ListItem>
    </asp:DropDownList>
</li>

<li>Time Out</li>
<li>
     
     <asp:DropDownList ID="ddl_hrs_out" runat="server"  Width="48px" style=" margin-left:2px">
            <asp:ListItem value="" disabled selected>HR</asp:ListItem>
     </asp:DropDownList>

      <asp:DropDownList ID="ddl_minute_out" runat="server" Width="48px" style=" margin-left:2px">
        <asp:ListItem value="" disabled selected>MM</asp:ListItem>
      </asp:DropDownList> 

    <asp:DropDownList ID="ddl_am_pm_out" runat="server" Width="49px" style=" margin-left:2px">
        <asp:ListItem Value="AM">AM</asp:ListItem>
        <asp:ListItem Value="PM">PM</asp:ListItem>
    </asp:DropDownList>
  
    
</li>
<li>Reason</li>
<li><asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine"></asp:TextBox></li>
<li><asp:Button ID="Button2" runat="server" OnClick="click_save_ot" Text="SAVE" /></li>
</ul>
   <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
</asp:Content>

