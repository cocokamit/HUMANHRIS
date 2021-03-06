<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otapplicationlist.aspx.cs" Inherits="content_hr_otapplicationlist" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3 id="nobel"></h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                 <asp:DropDownList ID="ddl_pg" runat="server" ></asp:DropDownList>
                <asp:TextBox ID="txt_search" runat="server"  Placeholder="Search" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_from" runat="server" CssClass="datee" Placeholder="FROM" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_to" CssClass="datee" runat="server" ClientIDMode="Static" AutoComplete="off" Placeholder="TO"></asp:TextBox>
                <asp:Button ID="Button1" OnClick="click_search" runat="server"  Text="Search" CssClass="btn btn-primary"/>
                <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_ot" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
                    <asp:BoundField DataField="date" HeaderText="Date OT"/>
                    <asp:BoundField DataField="OvertimeHours" HeaderText="OT"/>
                    <asp:BoundField DataField="OvertimeNightHours" HeaderText="OT Night Hours"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server"/>
</asp:Content>

 