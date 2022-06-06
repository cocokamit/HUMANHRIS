<%@ Page Language="C#" AutoEventWireup="true" CodeFile="thirteenmp.aspx.cs" Inherits="content_payroll_thirteenmp" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/auto/myJScript.js" type="text/javascript"></script>
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
<div class="page-title">
    <div class="title_left">
        <h3>13TH MONTH PAY TRANSACTION</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="sc"><i class="fa fa-clipboard"></i>Others</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>13TH MONTH PAY TRANSACTION</li>
       </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
            <%--    <asp:TextBox ID="txt_from" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_to" cssclass="datee" runat="server"  placeholder="To"></asp:TextBox>
                <asp:Button ID="Button2" runat="server"  Text="Search"  CssClass="btn btn-primary" />--%>
                <asp:LinkButton ID="Button3" runat="server" OnClick="newdtrlogs" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
             <asp:GridView ID="grid_sc_trans" runat="server"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="tmtrnid" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="date" HeaderText="Trn. Date"/>
                    <asp:BoundField DataField="year" HeaderText="Year"/>
                    <asp:BoundField DataField="description" HeaderText="Description"/>
                     <asp:BoundField DataField="notes" HeaderText="Remarks"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                          <a href="tmpdet?&payid=<%# function.Encrypt(Eval("tmtrnid").ToString(), true)%>" title="Details" target="_new" style=" font-size:14px" ><i class="fa fa-list"></i></a>
                            <a href="tmpslip?&payid=<%# function.Encrypt(Eval("tmtrnid").ToString(), true)%>&b=batch&cat=sc" target="_new" title="Print Slip" style="font-size:15px"><i class="fa fa-print"></i></a>
                        <%--     <a href="scdet?&sctrnid=<%# function.Encrypt(Eval("tmtrnid").ToString(), true)%>" title="Service Charge Registered" target="_new" style=" font-size:14px" ><i class="fa fa-line-chart"></i></a>
--%>                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Bank Text File" OnClick="generatetexfile"  Font-Size="14px"><i class="fa fa-file-text-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkcan" runat="server" CommandName='<%#Eval("tmtrnid") %>' ToolTip="Cancel Payroll"  Text="can" OnClick="click_cancel" OnClientClick="Confirm()" Font-Size="16px" ><i class="fa fa-trash" style="padding-right:0 "></i></asp:LinkButton>

                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>

<div class="row">
<div class="col-lg-3">
<div class="form-group">
        <asp:DropDownList ID="ddl_emp" runat="server" >
            <asp:ListItem Value="0">All</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:DropDownList ID="ddl_range_thirteen" runat="server">
            <asp:ListItem Value="0">Select Period</asp:ListItem>
            <asp:ListItem Value="1">December - May</asp:ListItem>
            <asp:ListItem Value="2">June - November</asp:ListItem>
            <asp:ListItem Value="3">Whole Year</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
       
    </div>
    <div class="form-group">
            <asp:TextBox ID="txt_remarks" TextMode="MultiLine" placeholder="Remarks" runat="server"></asp:TextBox>
    </div>
    <div class="form-group">
     <asp:Button ID="btn_ver" runat="server" Text="Verify" OnClick="click_range" CssClass="btn btn-primary" />
     <asp:Button ID="btn_save" runat="server" OnClick="savethirteen"  Enabled="false"   Text="Save"  CssClass="btn btn-primary" />
    </div>
    <div class="form-group">
        <asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label>
    </div>
</div>
<div class="col-lg-9">
    <div class="form-group">
        <div style=" overflow:auto; height:450px;">
            <asp:GridView ID="grid_item" runat="server" style=" font-size:9px;" AutoGenerateColumns="false" EmptyDataText="No Data Found" OnRowDataBound="ordb" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="employee" HeaderText="Employee"/>
                    <asp:BoundField DataField="Payroll_Type" HeaderText="Payroll Type"/>
                    <asp:BoundField DataField="December" HeaderText="DEC"/>
                    <asp:BoundField DataField="January" HeaderText="JAN"/>
                    <asp:BoundField DataField="Febuary" HeaderText="FEB"/>
                    <asp:BoundField DataField="March" HeaderText="MAR"/>
                    <asp:BoundField DataField="April" HeaderText="APR"/>
                    <asp:BoundField DataField="May" HeaderText="MAY"/>
                    <asp:BoundField DataField="June" HeaderText="JUN"/>
                    <asp:BoundField DataField="July" HeaderText="JUL"/>
                    <asp:BoundField DataField="August" HeaderText="AUG"/>
                    <asp:BoundField DataField="September" HeaderText="SEPT"/>
                    <asp:BoundField DataField="October" HeaderText="OCT"/>
                    <asp:BoundField DataField="November" HeaderText="NOV"/>
                    <asp:BoundField DataField="Thirteen_Month" HeaderText="GROSS"/>
                    <asp:BoundField DataField="Thirteen_Month" HeaderText="WHT"/>
                    <asp:BoundField DataField="Thirteen_Month" HeaderText="NET"/>
                </Columns>
             </asp:GridView>
        </div>
    </div>
</div>
</div>
</div>

<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="month" runat="server" />
<asp:HiddenField ID="day" runat="server" />
</asp:Content>
