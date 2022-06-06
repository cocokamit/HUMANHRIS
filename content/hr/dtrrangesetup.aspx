<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtrrangesetup.aspx.cs" Inherits="content_hr_dtrrangesetup" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript">
       jQuery.noConflict();
       (function ($) {
           $(function () {
               $(".datee").datepicker({ changeMonth: true,
                   yearRange: "-100:+1",
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
        .alert {padding:10px}
        .x_panel input[type=text]{padding:6.5px; float:left;margin-right:5px}
        .none{ display: none;}
        .no-margin {margin:0 !important}
        .add {margin:0px 2px 7px}
   </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
<div class="page-title">
    <div class="title_left pull-left">
        <h3>Payroll Period</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Payroll Period</li>
       </ul>
    </div>
</div>
</section>
<section class="content"> 
 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:LinkButton ID="Button3" runat="server" OnClick="newdtrlogs" CssClass="right add pull-right"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <div id="alert" runat="server" class="alert alert-empty no-margin">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="griddtrlist" runat="server"  AutoGenerateColumns="false" OnRowDataBound="rowbound" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="dtrrange" HeaderText="DTR Range"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                        <asp:LinkButton ID="lnkcan" runat="server"  CommandName='<%# Eval("id") %>'  OnClientClick="Confirm()" OnClick="click_cancel"><i class="fa fa-trash-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="30px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</section>

<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
    <ul class="input-form">
      <li><asp:ScriptManager ID="ScriptManager1" runat="server">
             </asp:ScriptManager>
             <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="10000">
             </asp:Timer>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="l_msg" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel></li>
        <li>Range <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></li>
        <li>
            <asp:TextBox ID="txt_f" runat="server" CSSCLASS="datee" autocomplete="off" placeholder='From' Width="49.5%"></asp:TextBox>
            <asp:TextBox ID="txt_t" runat="server" cssclass="datee" autocomplete="off" Placeholder='To' Width="49.3%"></asp:TextBox>
        </li>
        <li><hr /></li>
        <li><asp:Button ID="Button1" runat="server" OnClick="saverange"  Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>


<asp:TextBox ID="TextBox1" runat="server"  class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="month" runat="server" />
<asp:HiddenField ID="day" runat="server" />
</asp:Content>
