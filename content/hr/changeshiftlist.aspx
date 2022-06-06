<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changeshiftlist.aspx.cs" Inherits="content_hr_changeshiftlist" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datee").datepicker();
        });
    })(jQuery);
</script>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<style type="text/css">
    .PopUpPanel {margin-left:-400px; width:800px}
    .none { display:none}
</style>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Change Shift</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" runat="server" style=" display:none;" CssClass="minimal"></asp:DropDownList>
                 <asp:TextBox ID="txt_search" runat="server" Placeholder="Search" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_from" runat="server" cssclass="datee" Placeholder="from" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_to" runat="server" cssclass="datee" AutoComplete="off" Placeholder="to"></asp:TextBox>
                <asp:Button ID="Button1" OnClick="click_search" runat="server" Text="Search" CssClass="btn btn-primary"/>
                <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_changeshift" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
           

            <asp:GridView ID="grid_view" runat="server"  EmptyDataText="No record found" OnRowDataBound="datarow" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="CSDate" HeaderText="Date"/>
               <%-- <asp:BoundField DataField="payrollgroup" HeaderText="Payroll Group"/>--%>
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_edit" ToolTip="Edit Remarks!"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_add" runat="server" ToolTip="Can Add or Delete employee!" OnClick="click_view" Text="view" ><i class="fa fa-clipboard sm"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_can" runat="server" ToolTip="Cancel Transaction!" OnClick="cancel_changeshift"   OnClientClick="Confirm()" Font-Size="16px" ><i class="fa fa-trash" style="padding-right:0 "></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3 style=" margin:10px;">Monthly Monitoring</h3>
        </div>   
    </div>
     <div class="col-md-12">
        <div class="x_panel">
            <asp:GridView ID="grid_monthlymonitoring" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="IdNumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="ename" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="changedate" HeaderText="Date" />
                    <asp:BoundField DataField="ShiftCode" HeaderText="ShiftCode" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                </Columns>
            </asp:GridView>
        </div>
        </div>
</div>

<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server"  visible="false" class="PopUpPanel">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>

<ul class="input-form">
   <li style=" display:none;">Payroll Group</li>
    <li style=" display:none;"><asp:DropDownList ID="ddl_viewpg" runat="server" Enabled="false" DataValueField="Id"></asp:DropDownList></li>
    <li style=" display:none;">Remarks</li>
    <li style=" display:none;"><asp:TextBox ID="txt_viewremarks" runat="server"></asp:TextBox></li>
    <li>Employee <asp:Label ID="lbl_emp" runat="server" ForeColor="Red"></asp:Label></li>
    <li><asp:DropDownList ID="ddl_employee" runat="server" ></asp:DropDownList></li>
    <li>Date<asp:Label ID="lbl_date" runat="server" ForeColor="Red"></asp:Label></li>
    <li><asp:TextBox ID="txt_csd" runat="server" cssclass="datee"></asp:TextBox></li>
    <li>Remarks<asp:Label ID="lbl_remarks" runat="server" ForeColor="Red"></asp:Label></li>
    <li><asp:TextBox ID="txt_lineremarks" runat="server"></asp:TextBox></li>
    <li>Shift Code<asp:Label ID="lbl_sc" runat="server" ForeColor="Red"></asp:Label></li>
    <li>
        <asp:DropDownList ID="ddl_shiftcode" runat="server" DataSourceID="sql_shiftcode" DataTextField="ShiftCode" DataValueField="Id" style=" width:91%"></asp:DropDownList>
        <asp:Button ID="Button3" runat="server" OnClick="click_addperline" Text="ADD" CssClass="btn btn-primary right"  />
    </li>
    <li></li>
</ul>
 
<asp:GridView ID="grid_linedetails" runat="server" AutoGenerateColumns="false" CssClass="Grid">
        <Columns>
            <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
             <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
            <asp:BoundField DataField="Date" HeaderText="Date"/>
            <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
            <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server"  Text="can" OnClientClick="Confirm()" OnClick="click_can_perline"  style=" margin:3px;" ><i class="fa fa-close"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<div id="Div1" runat="server"  visible="false" class="PopUpPanel">
   <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
   <span>EDIT REMARKS</span>
   <hr />
   <ul class="input-form">
       <li>Remarks</li>
       <li><asp:TextBox ID="txt_remarkstoedit" runat="server"></asp:TextBox></li>
   </ul>
   <hr />
       <asp:Button ID="Button4" runat="server" Text="edit" OnClick="click_save_change" CssClass="btn btn-primary left"/>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id_to_edit" runat="server" />
<asp:HiddenField ID="lbl_leaveid" runat="server" />
<asp:SqlDataSource ID="sql_shiftcode" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MShiftCode order by id desc"></asp:SqlDataSource>
</asp:Content>

