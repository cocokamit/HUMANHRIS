<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editshiftcode.aspx.cs" Inherits="content_hr_editshiftcode" MasterPageFile="~/content/MasterPageNew.master" %>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Modify Shift</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i> System Configuration</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li><a href="Mshiftcode"> Shift Code</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Modify Shift</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <ul class="input-form"> 
                <li>Shift Code <asp:Label ID="lbl_shiftcode" runat="server" ></asp:Label></li>
                <li><asp:TextBox ID="txt_shiftcode" CssClass="input" runat="server"></asp:TextBox></li>
                <li>Remarks <asp:Label ID="lbl_remarks" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_remarks"  runat="server"></asp:TextBox></li>
            </ul>
            <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered"> 
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="Day" HeaderText="Day"/>
                    <asp:BoundField DataField="RestDay" HeaderText="RD"/>
                    <asp:BoundField DataField="TimeIn1" HeaderText="Time In 1"/>
                    <asp:BoundField DataField="TimeOut1" HeaderText="Time Out 1"/>
                    <asp:BoundField DataField="TimeIn2" HeaderText="Time In 2"/>
                    <asp:BoundField DataField="TimeOut2" HeaderText="Time Out 2"/>
                    <asp:BoundField DataField="NumberOfHours" HeaderText="No. of HRS." />
                    <asp:BoundField DataField="LateFlexibility" HeaderText="Flex HRS." />
                    <asp:BoundField DataField="LateGraceMinute" HeaderText="Grace MIN."/>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Button ID="btn_save" runat="server" OnClick="Update"  Text="Update"  CssClass="btn btn-primary" />
        </div>
    </div>
</div>  
<table style=" visibility:hidden;">
    <tr>
        <th>Day</th>
        <th>Rest Day</th>
       <th>Time In 1</th>
       <%--  <th>Time Out 1</th>
        <th>Time In 2</th>--%>
        <th>Time Out 2</th>
        <th>No. of Hrs.</th>
  <%--      <th>Flex Hrs.</th>--%>
        <th>Grace Min</th> 
      <%--  <th>Night Hrs.</th>--%>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddl_day" runat="server">
                <asp:ListItem>Monday</asp:ListItem>
                <asp:ListItem>Tuesday</asp:ListItem>
                <asp:ListItem>Wednesday</asp:ListItem>
                <asp:ListItem>Thursday</asp:ListItem>
                <asp:ListItem>Friday</asp:ListItem>
                <asp:ListItem>Saturday</asp:ListItem>
                <asp:ListItem>Sunday</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td><asp:CheckBox ID="chk_rd" runat="server" /></td>
        <td><asp:TextBox ID="txt_timein1" runat="server" data-inputmask="'mask': '9{1,2}:99 aa'"></asp:TextBox>  </td>
        <td><asp:TextBox ID="txt_timeout2" runat="server" data-inputmask="'mask': '9{1,2}:99 aa'"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_noh" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></td>
        <td><asp:TextBox ID="txt_gm" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></td>
    </tr>

</table>
<div style=" visibility:hidden;">
<table>
<tr>
<td><asp:TextBox ID="txt_fh" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></td>
<td><asp:TextBox ID="txt_timein2" runat="server"></asp:TextBox></td>
<td><asp:TextBox ID="txt_timeout1" runat="server"></asp:TextBox></td>
<td><asp:TextBox ID="txt_nh" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></td>
</tr>
</table>
</div>
<asp:TextBox ID="TextBox1" runat="server"  class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
</asp:Content>
