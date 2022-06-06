<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sc.aspx.cs" Inherits="content_payroll_sc" MasterPageFile="~/content/MasterPageNew.master" %>
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
        .hiddencol { display: none; }
   </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left">
        <h3>Service Charge</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="sc"><i class="fa fa-clipboard"></i> Others</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Service Charge</li>
       </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
        
                <asp:TextBox ID="txt_from" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_to" cssclass="datee" runat="server"  placeholder="To"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" OnClick="searchtrn" Text="Search"  CssClass="btn btn-primary" />
                <asp:LinkButton ID="Button3" runat="server"  OnClick="newdtrlogs" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
              </div>
             <asp:GridView ID="grid_sc_trans" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="sctrnid" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="date" HeaderText="Trn. Date"/>
                    <asp:BoundField DataField="dfrom" HeaderText="Date From"/>
                    <asp:BoundField DataField="dtoo" HeaderText="Date To"/>
                    <asp:BoundField DataField="sc_amt" HeaderText="Total Amount"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                          <a href="scdets?&payid=<%# function.Encrypt(Eval("sctrnid").ToString(), true)%>" title="Details" target="_new" style=" font-size:14px;" ><i class="fa fa-list"></i></a>
                            <a href="scslip?&payid=<%# function.Encrypt(Eval("sctrnid").ToString(), true)%>&b=batch&cat=sc" target="_new" title="Print Slip" style="font-size:15px;display:none;"><i class="fa fa-print"></i></a>
                             <a href="scdet?&sctrnid=<%# function.Encrypt(Eval("sctrnid").ToString(), true)%>" title="Service Charge Registered" target="_new" style=" font-size:14px;display:none;" ><i class="fa fa-line-chart"></i></a>
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Bank Text File" style="display:none;" OnClick="generatetexfile"  Font-Size="14px"><i class="fa fa-file-text-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnkcan" runat="server" ToolTip="Cancel Payroll"  CommandName='<%#Eval("payid") %>'  Text="can" OnClick="click_cancel" OnClientClick="Confirm()" Font-Size="16px" ><i class="fa fa-trash" style="padding-right:0 "></i></asp:LinkButton>

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
<b>Process Service Charge Range</b>
<hr />
<asp:DropDownList ID="ddl_pay_range" runat="server" style=" display:none;"></asp:DropDownList>

            <div class="form-group">
              <div class="col-lg-6">
                <label>Date Start</label>
                <asp:TextBox ID="txt_f" runat="server" CssClass="form-control datee" autocomplete="off" placeholder='From' ></asp:TextBox>   
              </div>
              <div class="col-lg-6">
                <label>Date End</label>
                <asp:TextBox ID="txt_t" runat="server" CssClass="form-control datee" autocomplete="off" Placeholder='To'  ></asp:TextBox>
              </div>
            </div>  
            <div class="form-group">
               <div class="col-lg-6">
               <label>Actual Service Charge</label>
                <asp:TextBox ID="txt_sc_amt" runat="server"  CssClass="form-control" autocomplete="off" onkeyup="decimalinput(this)"  ></asp:TextBox>
              </div>
              <div class="col-lg-6">
                 <label>Less : % Allow. For breakages</label>
                <asp:TextBox ID="txt_sc_percentage" runat="server" CssClass="form-control"  autocomplete="off" onkeyup="decimalinput(this)"  ></asp:TextBox>
              </div>
            </div>
            <div class="form-group">
            <div class="col-lg-12">
            <label>Employee List</label>
                      <div style="height:250px;">
                <asp:GridView ID="grid_item" runat="server" EmptyDataText="No Data Found"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="empid" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="employee" HeaderText="Employee"/>
                        <asp:BoundField DataField="tdpe" HeaderText="No. of Days"/>
                        <asp:BoundField DataField="empsc" HeaderText="Service Charge"/>
                    </Columns>
                </asp:GridView>
            </div>
            </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lbl_err" runat="server"  ForeColor="Red"></asp:Label>
            </div>
       
     
            <asp:Button ID="Button1" runat="server" OnClick="verifysc" Text="Verify" CssClass="btn btn-primary" />
            <asp:Button ID="btn_save" runat="server" Visible="false"   Text="Submit" OnClick="Save" CssClass="btn btn-primary" />
     


</div>


<div class="modal fade in" id="modal-newsc" >
  
</div>
<asp:TextBox ID="TextBox1" runat="server"  class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="month" runat="server" />
<asp:HiddenField ID="day" runat="server" />
</asp:Content>
