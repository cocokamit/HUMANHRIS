<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editdaytype.aspx.cs" Inherits="content_hr_editdaytype" MasterPageFile="~/content/MasterPageNew.master" %>

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
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
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
 <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function($) {
            $(function() {
                $(".datee").datepicker({ changeMonth: true,
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>

<div class="page-title">
    <div class="title_left">
        <h3>Create Holiday</h3>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <ul class="input-form">
                <li>Company <asp:Label ID="lbl_pg" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="ddl_company" AutoComplete="off" runat="server" OnTextChanged="click_company" AutoPostBack="true"   ></asp:DropDownList></li>
                <li>Branch <asp:Label ID="lbl_emp" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:DropDownList ID="ddl_branch" AutoComplete="off" runat="server"></asp:DropDownList></li>
                <li>Date<asp:Label ID="lbl_date" style="color:Red;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_date_holiday" CssClass="datee"   runat="server"></asp:TextBox></li>
                <li>Remarks <asp:Label ID="lbl_remarks2" runat="server" CssClass="text-danger"></asp:Label></li>
                <li><asp:TextBox ID="txt_remarks" runat="server" TextMode="MultiLine"></asp:TextBox></li>
            </ul>
                <hr />
                <asp:Button ID="btn_add" runat="server" CssClass="btn btn-primary"  OnClick="Update" Text="Save" />
                <asp:Button ID="btn_save_daytype" runat="server" Visible="false"  CssClass="btn btn-primary"  Text="Save" />
            </div>
        </div>
     </div>
</div>


<div class="div">

<hr />
<asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="false" CssClass="Grid">
    <Columns>
    
        <asp:BoundField DataField="line_id"/>
        <asp:BoundField DataField="branch" HeaderText="Branch"/>
        <asp:BoundField DataField="date" HeaderText="Date"/>
     <%--   <asp:BoundField DataField="DateBefore" HeaderText="Working Date Before" />
        <asp:BoundField DataField="DateAfter" HeaderText="Working Date After"/>
        <asp:BoundField DataField="ExcludedInFixed" HeaderText="Excluded in Fix"/>
        <asp:BoundField DataField="WithAbsentInFixed" HeaderText="With Absent" />--%>
        <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="cancel_line" OnClientClick="Confirm()" Text="can" style=" margin:3px;" ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />

</div>
    <div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
<table style=" visibility:hidden;">
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
        <td><asp:TextBox ID="txt_daytype"  runat="server"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_mow"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_mor"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_owm"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_orm"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_nwm"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_nrm"  runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
    </tr>
</table>
<asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="appid" runat="server" />
</asp:Content>
