<%@ Page Language="C#" AutoEventWireup="true" CodeFile="listdtr.aspx.cs" Inherits="content_payroll_listdtr" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
     hr{margin:5px 0 10px}
     input[type=text]{padding:6.5px; float:left;margin-right:5px}
</style>
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
<section class="content-header">
<div class="page-title">
    <div class="title_left pull-left">
        <h3>Adjusted Record</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Adjusted Record</li>
       </ul>
    </div>
</div>
</section>
<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"  style="padding:7.5px"></asp:DropDownList>
                <asp:TextBox ID="txt_f" CssClass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_t" CssClass="datee" runat="server" placeholder="To"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server" OnClick="search"  Text="Search" CssClass="btn btn-primary" />
                <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_dtr" CssClass="right add pull-right"><i class="fa fa-plus-circle"></i></asp:LinkButton>
             </div>
            <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="DTRDate" HeaderText="Date"/>
                    <asp:BoundField DataField="DTRNumber" HeaderText="DTR Number"/>
                    <asp:BoundField DataField="PayrollGroup" HeaderText="Payroll Group"/>
                    <asp:BoundField DataField="DateStart" HeaderText="Date Start"/>
                    <asp:BoundField DataField="DateEnd" HeaderText="Date End"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <a href="dtrdetails?&dtrid=<%# function.Encrypt(Eval("id").ToString(), true)%>" target="_new"  class="border-right" ><i class="fa fa-clipboard sm"></i></a>
                            <a class="none" href="payrollsum?&dtrid=<%# function.Encrypt(Eval("id").ToString(), true)%>&pg=<%# function.Encrypt(Eval("pg").ToString(), true)%>&ds=<%# function.Encrypt(Eval("DateStart").ToString(), true)%>&de=<%# function.Encrypt(Eval("DateEnd").ToString(), true)%>" target="_new"  class="border-right" ><i class="fa fa-file-text" title="DTR Summary"></i></a>
                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Click to cancel transaction."  Text="can" OnClick="click_cancel" OnClientClick="Confirm()" ><i class="fa fa-trash-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                                
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
 </section>
 <div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
</asp:Content>
