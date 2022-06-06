<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adddaytype.aspx.cs" Inherits="content_hr_adddaytype" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
#nav,#nav1 
{
 background: gray url(style/images/nav_bkg.jpg) repeat-x 0% 0%; float: left; list-style: none; margin: 0; padding: 0; width: 100%; 
}
#nav,#nav1 li 
{
 float: left; font-size: 11px; margin: 0; padding: 1; text-transform: uppercase; 
}
#nav1 li a 
{
 border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 0px 8px; text-decoration: none; 
}
#nav li a {
 border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 10px 16px; text-decoration: none; 
}
#nav li a:hover {
 background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; 
}
.click{background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; }
input[type=text], select 
{
    width: 100%;
    padding: 6px 20px;
    margin: 8px 0;
    display: inline-block;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
}
.btn input[type=submit] 
{
    width: 100%;
    background-color: #4CAF50;
    color: white;
    padding: 9px 30px;
    margin: 8px 0;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}
.btn input[type=submit]:hover 
{
    background-color: #45a049;
}
.Grid { background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; font-family:Calibri; color: #474747; width: 100%; font-size: 12px; text-align: center;}
.Grid td {padding: 2px; border: solid 1px #c1c1c1; }
.Grid th {padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
.Grid .alt {background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }
.Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }
.Grid .pgr table { margin: 3px 0; }
.Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  
.Grid .pgr a { color: Gray; text-decoration: none; }
.Grid .pgr a:hover{ color: #000; text-decoration: none; }
</style>
      <%-- <script src="script/jquery.js" type="text/javascript"></script>--%>
<script src="script/jquery.maskinput.js" type="text/javascript"></script>
  <script src="script/fiddle.js" type="text/javascript"></script>

<script type="text/javascript">
    function ValidateDate(sender, args) {
        var dateString = document.getElementById(sender.controltovalidate).value;
        var regex = /(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$/;
        if (regex.test(dateString)) {
            var parts = dateString.split("/");
            var dt = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
            args.IsValid = (dt.getDate() == parts[0] && dt.getMonth() + 1 == parts[1] && dt.getFullYear() == parts[2]);
        } else {
            args.IsValid = false;
        }
    }


   
</script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        $("#txt_date_holiday").datepicker();
        $("#txt_wdb").datepicker();
        $("#txt_wda").datepicker();
    });
    </script>
<asp:SqlDataSource ID="sql_emp" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MCompany">
</asp:SqlDataSource>
<div class="div">
<table>
    <tr>
        <th><asp:Label ID="lbl_dt" runat="server" ></asp:Label>Day Type</th>
        <th><asp:Label ID="lbl_mow" runat="server"></asp:Label>Workday Multiplier</th>
        <th><asp:Label ID="lbl_mor" runat="server"></asp:Label>Restday Multiplier</th>
        <th>OT Workday Multiplier</th>
        <th>OT Restday Multiplier</th>
        <th>Night Workday Multiplier</th>
        <th>Night Restday Multiplier</th>
    </tr>
    <tr>
        <td><asp:TextBox ID="txt_daytype" runat="server"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_mow" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_mor" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_owm" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_orm" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_nwm" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_nrm" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
    </tr>
</table>
<hr />
<asp:Button ID="btn_save_daytype" runat="server"  OnClick="click_save_daytype" Text="Save" />

</div>
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="pg" runat="server" />
</asp:Content>


