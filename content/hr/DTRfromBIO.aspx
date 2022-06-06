<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DTRfromBIO.aspx.cs" Inherits="content_hr_DTRfromBIO" MasterPageFile="~/content/MasterPageNew.master" %>
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
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
   <script type="text/javascript">
       function Confirm() {
           var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
           if (confirm("Are you sure to cancel this transaction?"))
           { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
       } 
   </script>
   <style type="text/css">
        hr{margin:5px 0 10px}
        .x_panel input[type=text]{padding:6.5px; float:left;margin-right:5px}
   </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
<div class="page-title">
    <div class="title_left pull-left">
        <h3>Import File</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Raw File</li>
       </ul>
    </div>
</div>
</section>
<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:TextBox ID="txt_from" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_to" cssclass="datee" runat="server"  placeholder="To"></asp:TextBox>
                <asp:Button ID="Button4" runat="server" OnClick="search" Text="Search" CssClass="btn btn-primary"  />
                <asp:LinkButton ID="Button3" runat="server" OnClick="newdtrlogs" CssClass="right add pull-right"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <asp:GridView ID="griddtrlist" runat="server" OnRowDataBound="OnRowDataBound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="sysdate" HeaderText="Transaction Date"/>
                    <asp:BoundField DataField="store" HeaderText="Store Name"/>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <a href="viewtrlogs?&logsid=<%# function.Encrypt(Eval("id").ToString(), true)%>" target="_new" class="border-right" ><i class="fa fa-file-text-o sm border-right"></i></a>
                            <asp:LinkButton ID="lnkcan" runat="server"  CommandName='<%# Eval("id") %>' CssClass="no-padding-right"  OnClientClick="Confirm()" OnClick="click_cancel"><i class="fa fa-trash-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</section>

<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a" style="top:5%">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <ul class="input-form">
        <div style=" display:none;">
        <li>DTR Range <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></li>
        <li>
            <asp:TextBox ID="txt_f" runat="server" CSSCLASS="datee" autocomplete="off" placeholder='From' Width="49.5%"></asp:TextBox>
            <asp:TextBox ID="txt_t" runat="server" cssclass="datee" autocomplete="off" Placeholder='To' Width="49.3%"></asp:TextBox>
        </li>
        <li>Payroll Group<asp:Label ID="lbl_pg" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:DropDownList ID="ddl_pg" runat="server" Width="99.8%"></asp:DropDownList></li>
        </div>
        <li><asp:Label ID="lbl_error" runat="server"  ForeColor="Red"></asp:Label></li>
        <li>Excel File<asp:Label ID="lbl_ef" runat="server" ForeColor="Red"></asp:Label></li>
        <li><asp:FileUpload ID="FileUpload1" runat="server" /></li>
        <li>Store Name</li>
        <li><asp:dropdownlist ID="ddl_store" runat="server" AutoComplete="off" CssClass="form-control"></asp:dropdownlist></li>
        <li><hr /></li>
        <li><asp:Button ID="Button1" runat="server" OnClick="click_file" Text="Convert" CssClass="btn btn-primary" /></li>
    </ul>
</div>
<asp:TextBox ID="TextBox1" runat="server"  class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="month" runat="server" />
<asp:HiddenField ID="day" runat="server" />
</asp:Content>
