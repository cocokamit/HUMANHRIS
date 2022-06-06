<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adjustments.aspx.cs" Inherits="content_Admin_adjustments" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">

<style type="text/css">
    .x-head input[type=text]{ padding:8.1px;}
    .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3{  width:300px;left:60%;}
    .close{ margin:-10px;float:right;}
    .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 {   border-radius:4px; position: fixed; z-index: 1002;  width: 600px; top: 28%;left: 50%; margin: -180px 0 0 -300px; background-color: #fff; }
    .input-form-span { margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>

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

</asp:Content>

<asp:Content ID="mandatorytables" runat="server" ContentPlaceHolderID="content">
 <div class="page-title">
    <div class="title_left hd-tl">
        <h3>System Adjustment</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li>>System Adjustment</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:TextBox ID="txt_from" cssclass="datee" placeholder="From" runat="server"></asp:TextBox>
                <asp:TextBox ID="txt_to" cssclass="datee"  placeholder="To" runat="server"></asp:TextBox>
                <asp:DropDownList ID="dl_choice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="change_choice" CssClass="minimal">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>Time Adjustment</asp:ListItem>
                    <asp:ListItem>Overtime</asp:ListItem>
                    <asp:ListItem>CWS</asp:ListItem>
                    <asp:ListItem>Work Verification</asp:ListItem>
                    <asp:ListItem>OBT</asp:ListItem>
                    <asp:ListItem>Leave</asp:ListItem>
                    <asp:ListItem>Under Time</asp:ListItem>
                    <asp:ListItem>Offset</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div id="alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>

             <asp:GridView ID="grid_adjustment" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="EmployeeId" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="sysdate" HeaderText="System Date"/>
                    <asp:BoundField DataField="idnumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="date_log" HeaderText="Date"/>
                    <asp:BoundField DataField="time_in" HeaderText="Time In 1"/>
                    <asp:BoundField DataField="time_out1" HeaderText="Time Out 1"/>
                    <asp:BoundField DataField="time_in1" HeaderText="Time In 2"/>
                    <asp:BoundField DataField="time_out" HeaderText="Time Out"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:BoundField DataField="note" HeaderText="Remarks" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_del" OnClick="time_adjustment_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             <asp:GridView ID="grid_overtime" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date" HeaderText="Date Filed" />
                    <asp:BoundField DataField="date_ot" HeaderText="Date Overtime"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="time_in" HeaderText="Time In"/>
                    <asp:BoundField DataField="time_out" HeaderText="Time Out"/>
                    <asp:BoundField DataField="OvertimeHours" HeaderText="OT Regular Hours"/>
                    <asp:BoundField DataField="OvertimeNightHours" HeaderText="OT Night Hours"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:TemplateField ItemStyle-Width="40px">
                        <ItemTemplate>
                              <asp:LinkButton runat="server" ID="lnk_del" OnClick="overtime_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             <asp:GridView ID="grid_CS" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="shiftidTo" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                     <asp:BoundField DataField="changeshift_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date_filed" HeaderText="Date Filed"/>
                    <asp:BoundField DataField="Employee_Name" HeaderText="Employee Name"/>
                    <asp:TemplateField HeaderText="Shift From">
                        <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("shiftcodeFrom")%>' tooltip='<%# Eval("aa") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Shift To">
                                <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("shiftcodeTo")%>' tooltip='<%# Eval("bb") %>' ></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                    <asp:TemplateField ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnk_del" OnClick="cws_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             <asp:GridView ID="grid_hd" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                   <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="sysdate" HeaderText="System Date"/>
                    <asp:BoundField DataField="idnumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="date" HeaderText="Date"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:BoundField DataField="employeeid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:TemplateField ItemStyle-Width="40px">
                        <ItemTemplate>
                             <asp:LinkButton runat="server" ID="lnk_del" OnClick="hd_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             <asp:GridView ID="grid_travel" runat="server" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date_input" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="purpose" HeaderText="Purpose"/>
                        <asp:BoundField DataField="travel_start" HeaderText="Travel Start"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="travel_end" HeaderText="Travel End"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lnk_del" OnClick="obt_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>

             <asp:GridView ID="grid_leave" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="sysdate" HeaderText="Date Filed"/>
                        <asp:BoundField DataField="leavefrom" HeaderText="Leave From"/>
                        <asp:BoundField DataField="leaveto" HeaderText="Leave To"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave Type"/>
                        <asp:BoundField DataField="delegate" HeaderText="Delegate"/>
                        <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                        <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk_del" OnClick="leave_delete"  OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="40px"/>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

             <asp:GridView ID="grid_undertime" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date_filed" HeaderText="Undertime"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
                    <asp:BoundField DataField="timeout" HeaderText="Time"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_del" OnClick="undertime_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_offset" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date" HeaderText="Date Filed" />
                        <asp:BoundField DataField="fullname" HeaderText="Employee Name" />
                        <asp:BoundField DataField="appliedfrom" HeaderText="Date From"/>
                        <asp:BoundField DataField="offsethrs" HeaderText="Offset Hrs."/>
                        <asp:BoundField DataField="status" HeaderText="Status"/>
                        <asp:TemplateField ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnk_del" OnClick="offset_delete" OnClientClick="Confirm()" ><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </div>
    </div>
</div>
    <asp:TextBox ID="TextBox1" style="display:none;" runat="server"></asp:TextBox>
</asp:Content>

