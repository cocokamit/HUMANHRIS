<%@ Page Language="C#" AutoEventWireup="true" CodeFile="appraisallist.aspx.cs" Inherits="content_hr_appraisallist" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">

<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
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
        .emp-identity{padding:0;float:left; width:100%;}
        .emp-identity input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .emp-identity select {padding:7px; width:100%;margin-bottom:10px}
        .emp-identity input[type=checkbox] {margin-right:5px !important;}
        .disabled
        {
            pointer-events: none;
            opacity: 0.5;
        }
         .enabled
        {
            opacity: 1;
        }
   </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
<div class="page-title">
    <div class="title_left pull-left">
        <h3>Appraisal</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Appraisal</li>
       </ul>
    </div>
</div>
</section>
<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
              <asp:GridView ID="grid_det" runat="server" AutoGenerateColumns="false" OnRowDataBound="robound" EmptyDataText="No Data Found!" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="purposeid" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                    <asp:BoundField DataField="date" HeaderText="Date"/>
                    <asp:BoundField DataField="fullname" HeaderText="Employee"/>
                    <asp:BoundField DataField="purpose" HeaderText="Purpose of Appraisal"/>
                    <asp:BoundField DataField="recommend" HeaderText="Recommendation"/>
                    <asp:BoundField DataField="totalratings" HeaderText="Rate"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_print" runat="server" OnClick="print" CommandName='<%# Eval("id") %>' ToolTip="Print" ><i class="fa fa-print"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_download" runat="server" OnClick="process" CommandName='<%# Eval("id") %>' ToolTip="Process" ><i class="fa fa-edit"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_cancel" OnClientClick="Confirm()" CommandName='<%# Eval("id") %>' ToolTip="DisAproved" ><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="150px" />
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
<div id="pay_det" class="disabled" runat="server">
  <h4 class="page-header">Payroll Adjustment</h4>
                            <div class="row">
                                <div class="col-lg-3 none">
                                    <div class="form-group">
                                      <label>Payroll Type <span class="text-red"></span></label>
                                      <asp:dropdownlist ID="ddl_payrolltype" runat="server" AutoPostBack="true" ClientIDMode="Static"  OnTextChanged="click_paytype"   AutoComplete="off"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-3 none">
                                    <div class="form-group">
                                        <label>Fix No of Days( in a year)</label>
                                        <asp:Dropdownlist ID="txt_fnod" runat="server" AutoComplete="off"  ClientIDMode="Static" >
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="365">365</asp:ListItem>
                                        <asp:ListItem Value="313">313</asp:ListItem>
                                        </asp:Dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-3 none">
                                    <div class="form-group">
                                        <label>Fix No of Hours</label>
                                        <asp:TextBox ID="txt_fnoh" runat="server" AutoComplete="off" class="form-control"  ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Monthly Rate</label>
                                        <asp:TextBox ID="txt_mr" runat="server" AutoComplete="off" class="form-control"  ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="col-lg-3">
                                    <div class="form-group">
                                      <label>SEMI - MONTHLY INCOME<span class="text-red"></span></label>
                                      <asp:TextBox ID="txt_pr" runat="server"   class="form-control"  AutoComplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                             <div class="row">
                              
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Daily Rate</label>
                                       <asp:TextBox ID="txt_dr" runat="server"  AutoComplete="off" class="form-control" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Hourly Rate</label>
                                        <asp:TextBox ID="txt_hr" runat="server" class="form-control"  AutoComplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            </div>
<div id="div_promotion" class="disabled" runat="server">
  <h4 class="page-header">Promotion</h4>
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group">
                <label>Position</label>
                <asp:DropDownList ID="ddl_position"  runat="server"></asp:DropDownList>    
            </div>
        </div>
    </div>
    </div>
    <div id="div_regularization" class="disabled" runat="server">
    <h4 class="page-header">Regularization</h4>
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group">
                <label>Status</label>
                <asp:DropDownList ID="ddl_status"  runat="server"></asp:DropDownList>   
            </div>
        </div>
    </div>
    </div>
    <h4 class="page-header">Attached Files</h4>
    <div class="row">
    <div class="col-lg-12">

        </div>
        <div class="col-lg-12">
            <div class="form-group">
                 <asp:FileUpload ID="FileUpload2" runat="server" /> 
            </div>
        </div>
    </div>
   <div id="pay_info" class="tab-pane"  runat="server">
      
                        <ul class="emp-identity bei" style=" display:none;" >
                         <li style=" display:none;"><asp:dropdownlist ID="ddl_taxtable" AutoComplete="off"  ClientIDMode="Static" onchange="addcompute()"  runat="server">
                            <asp:ListItem>Semi-Monthly</asp:ListItem>
                            </asp:dropdownlist>
                            </li>
                        
                            <li>Meal Allowance</li>
                            <li><asp:TextBox ID="txt_meal_allow" runat="server"   AutoComplete="off"  ClientIDMode="Static"  ></asp:TextBox></li> <%--onkeyup="decimalinput(this);addcompute();"  onclick="this.select();"--%>
                          <li>Non Tax Income</li>
                          <li><asp:TextBox ID="txt_nti" runat="server"   AutoComplete="off" ClientIDMode="Static"   ></asp:TextBox></li><%-- onclick="this.select();" onkeyup="decimalinput(this);"--%>

                        </ul>
    </div>
                <div class="form-group">
                <label>Effective Date</label>
                <asp:TextBox ID="txt_effective_date" class="datee" runat="server"></asp:TextBox>
            </div>
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="savetrn"  Text="Save" CssClass="btn btn-primary" />
    <asp:HiddenField ID="hdn_recomendation" runat="server" />
</div>

<asp:TextBox ID="TextBox1" runat="server"  class="hide"></asp:TextBox> 
<asp:HiddenField ID="hdn_purposeid" runat="server" />
<asp:HiddenField ID="hnd_fromid" runat="server" />
<asp:HiddenField ID="hnd_empid" runat="server" />
<asp:HiddenField ID="hnd_prev_stat" runat="server" />
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="month" runat="server" />
<asp:HiddenField ID="day" runat="server" />


<asp:HiddenField ID="hdn_payrolltype" runat="server" />
<asp:HiddenField ID="hdn_fnod" runat="server" />
<asp:HiddenField ID="hdn_fnoh" runat="server" />
<asp:HiddenField ID="hdn_mr" runat="server" />
<asp:HiddenField ID="hdn_pr" runat="server" />
<asp:HiddenField ID="hdn_dr" runat="server" />
<asp:HiddenField ID="hdn_hr" runat="server" />
<asp:HiddenField ID="hdn_prev_position" runat="server" />
</asp:Content>
