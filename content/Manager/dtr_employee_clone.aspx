<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtr_employee_clone.aspx.cs" Inherits="content_Manager_dtr_employee_clone" MasterPageFile="~/content/site.master"%>


<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_dtr">
    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".txt_f").datepicker();
                $(".txt_t").datepicker();
            });
        })(jQuery);
    </script>

<%--     <script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
     
     <script type="text/javascript" src="script/freezeheadergv/index.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
            freaze("grid_item", "container");
        });
    </script>--%>

    <style type="text/css">
        input[type="date"].form-control,input[type="time"].form-control { line-height:normal !Important}
        .select2 { min-width:200px}
        .table th{font-size:12px !important}
        .table { font-size:13x !important}
        .table a  { font-size:12px !important}
        .modal { position:fixed !important}
    </style>

</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr">
<section class="content-header">
    <h1>DTR</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">DTR</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                 <div class="pull-right">
                    <asp:LinkButton ID="btn_addRequest" runat="server" OnClick="addRequest" Text="Add" class="btn btn-primary"></asp:LinkButton>
			<asp:Button ID="Button6add" runat="server" OnClick="sanctiontab" Text="GOsanction" CssClass="btn btn-primary"/>
                </div>
                <div class="input-group">
                    <asp:DropDownList ID="ddl_pay_range" AutoPostBack="true" OnSelectedIndexChanged="ddlchange" CssClass="select2" runat="server"></asp:DropDownList>
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="txt_f form-control" style="float:left; width:150px; margin-right:5px; display:none; " ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="txt_f form-control" style="float:left; width:150px;margin-right:5px;  display:none; "  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" OnClick="btn_go" Visible="false" Text="GO" CssClass="btn btn-primary"/>
                </div>
            </div>
            <div class="box-body" style="padding-top:0 !important">
             <asp:GridView ID="grid_preotrequest" runat="server" AutoGenerateColumns="false" BorderColor="Turquoise" CssClass="table table-striped table-bordered" >
                <Columns>
                    <asp:BoundField DataField="Id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="EmpsName" HeaderText="Name" />
                    <asp:BoundField DataField="DF" HeaderText="From Date" />
                    <asp:BoundField DataField="DT" HeaderText="To Date" />
                    <asp:BoundField DataField="NumberofHours" HeaderText="Number of Hours" />
                    <asp:BoundField DataField="TotalHours" HeaderText="Total Hours" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnview" runat="server" OnClick="displaylogforpreOT" CommandName='<%# Eval("Id") %>' ToolTip="View" CssClass="glyphicon glyphicon-info-sign border-right" ></asp:LinkButton>
                            <asp:LinkButton ID="lnkbtncan" runat="server" OnClick="cancellogforpreOT" CommandName='<%# Eval("Id") %>' ToolTip="Cancel" CssClass="glyphicon glyphicon-remove-sign" ></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="80" />
                    </asp:TemplateField>
                </Columns>
             </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>

<div class="modal fade in" id="modal_preOTLogs" runat="server">
    <div class="modal-dialog modal-lg">
    <div class="modal-content">
        <div class="modal-header">
            <div align="right">
                <asp:LinkButton ID="LinkButton7" OnClick="closingremarks" runat="server"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
            </div>
            <h4 class="modal-title">Rendered Logs</h4>
        </div>
        <div class="modal-body">
         <asp:GridView ID="grid_preotrequestLogs" runat="server" ClientIDMode="Static" BorderColor="Turquoise" OnRowDataBound="ordb" AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"> 
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="employee" HeaderText="Employee" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                      <asp:TemplateField HeaderText="SHIFT CODE" >
                            <ItemTemplate> 
                                <asp:LinkButton ID="lnk_shift" runat="server" Text='<%#Eval("ShiftCode")%>' style=" display:block; padding:0 !important" ></asp:LinkButton>
                                <asp:Label ID="lbl_sr" runat="server" ForeColor="Black"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                        <ItemStyle />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Date" HeaderText="DATE" HeaderStyle-Width="10px"/>
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein1" HeaderText="LOG IN" />
                    <asp:BoundField DataField="timeout1" HeaderText="BREAK IN"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"  />
                    <asp:BoundField DataField="timein2" HeaderText="BREAK OUT"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="timeout2" HeaderText="LOG OUT" />
                    <asp:BoundField DataField="olw" HeaderText="LEAVE" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:TemplateField HeaderText="ABS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                            <ItemTemplate> 
                                  <asp:Label ID="lbl_aw" runat="server" Text='<%#bind("aw")%>'></asp:Label>
                                  <asp:LinkButton ID="lnk_offset_absent" ForeColor="Red" Visible="false"   ToolTip="OT Offset" OnClick="add_offset"  runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
                            </ItemTemplate>
                        <ItemStyle  Width="50px"/>
                        </asp:TemplateField>
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="reg_hr" HeaderText="REG."  />
                    <asp:BoundField DataField="night" HeaderText="NIGHT" />
                    <asp:BoundField DataField="offsethrs" HeaderText="OS" Visible="false"/>
                    <asp:BoundField DataField="ot" HeaderText="OT" />
                    <asp:BoundField DataField="otn" HeaderText="OTN" />
                    <asp:BoundField DataField="totalhrs" HeaderText="Total" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="late" HeaderText="LATE" />
                    <asp:TemplateField HeaderText="UT" >
                            <ItemTemplate> 
                                  <asp:Label ID="lbl_uthrs" runat="server" Text='<%#bind("ut")%>'></asp:Label>
                                  <asp:LinkButton ID="lnk_offset" ForeColor="Red" Visible="false"  ToolTip="OT Offset" OnClick="add_offset"   runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
                            </ItemTemplate>
                        <ItemStyle  Width="50px"/>
                        </asp:TemplateField>
                    <asp:BoundField DataField="nethours" HeaderText="Net HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="shiftcodeid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="empid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:TemplateField HeaderText="STATUS" >
                        <ItemTemplate > 
                            <asp:LinkButton ID="lnk_status" Text='<%#bind("status")%>' OnClick="clickrd"  style=" font-size:9px;"   runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_offset_stat" Width="60px" BackColor="Orange"  style=" color:Gray; font-size:10px; border:1px; border-radius:5px; display:none;" Visible="false"   runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_offsethdrd" Width="60px"  BackColor="Orange" style=" color:Gray; font-size:10px; border:1px; border-radius:5px; display:none;" Visible="false" OnClick="clickrdhdrdoffset" ToolTip="Holiday / Restday Offset" Text="Offset" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle  Width="20px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="irregularity" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
             <%--        <asp:BoundField DataField="status" HeaderText="Status"/>--%>
                    <asp:TemplateField HeaderText="ACTION" >
                        <ItemTemplate> 
                            <a href="javascript:void(0)"  data-toggle="modal" data-target="#modal-default<%# Container.DataItemIndex + 1 %>" class="fa fa-list-ul border-right text-sm"></a>
                            <asp:LinkButton ID="lnk_ot" ToolTip="Overtime Application" OnClick="add_ot" Enabled="false" ForeColor="Gray"  runat="server" CssClass="fa fa-history no-padding-right"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_tadj" ToolTip="Time Adjustment Application" OnClick="click_adj" CssClass="fa fa-adjust border-right none" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_cws" ToolTip="Change Shift" OnClick="click_cws"  runat="server" CssClass="fa fa-share no-padding-right none"></asp:LinkButton>
                            <%--<asp:LinkButton ID="lnk_offset" ForeColor="Red" Visible="false"  ToolTip="Offset Hours" OnClick="add_offset"  runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
      --%>                  
                        <div class="modal fade in" id="modal-default<%# Container.DataItemIndex + 1 %>">
                            <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">×</span></button>
                                <h4 class="modal-title">Attendance Logs</h4>
                                </div>
                                <div class="modal-body">
                                <asp:GridView ID="gv_attlog" runat="server" ShowHeader="false" EmptyDataText="No record found" CssClass="table table-bordered no-margin" ></asp:GridView>
                                </div>
                            </div>
                            </div>
                        </div>
      
      
                        </ItemTemplate>
                    <ItemStyle  Width="70px"/>
                    </asp:TemplateField>
                     <asp:BoundField DataField="setupfinalout" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                </Columns>
                </asp:GridView>
        </div>
        <div class="box-footer pad">
            <asp:Button ID="Button6" runat="server" Text="Complete Request" OnClick="save_ot_request"  CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>

<div class="modal" id="modalRequest" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" CssClass="close" OnClick="modalClose"><span aria-hidden="true">&times;</span></asp:LinkButton>
         
        <h4 class="modal-title">Request Overtime</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Employee</label>
                <asp:DropDownList ID="drop_emp" ClientIDMode="Static" AutoPostBack="true" CssClass="select form-control" runat="server" >
                </asp:DropDownList>
            </div>
            <div class="row">
                <div class="col-lg-6">
                    <div class="form-group">
                        <label>From Date</label>
                        <asp:Label ID="lbl_from" runat="server" CssClass="text-red"></asp:Label>
                        <asp:TextBox ID="txtpreotdatefrom" runat="server" CssClass="form-control" type="Date"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label>To Date</label>
                        <asp:Label ID="lbl_to" runat="server" CssClass="text-red"></asp:Label>
                        <asp:TextBox ID="txtpreotdateto" runat="server" CssClass="form-control" type="Date"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label>Number of Hours</label>
                 <asp:Label ID="lbl_hrs" runat="server" CssClass="text-red"></asp:Label>
                <asp:TextBox ID="txtpreothourstotal" AutoPostBack="true" OnTextChanged="calculatehours" runat="server" CssClass="form-control number"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Total Hours</label>
                <asp:Label ID="lbl_thrs" runat="server" CssClass="text-red"></asp:Label>
                <asp:TextBox ID="txtpreottotalhours" runat="server" CssClass="form-control datee"></asp:TextBox>
            </div>
            <div class="form-group no-margin">
                <label>Purpose/Remarks</label>
                <asp:Label ID="lbl_purpose" runat="server" CssClass="text-red"></asp:Label>
                <asp:TextBox ID="txtpreotremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:TextBox ID="txthandler" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnaddpreot" runat="server" OnClick="addpreotrequest" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div id="modal_ta" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Time Adjustment</h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Date</label>
                    <asp:Label ID="lbl_date" CssClass="form-control" runat="server" ></asp:Label>
                </div>

                <div class="form-group">
                    <label>Log In</label>
                    <div class="row">
                        <div class="col-lg-6">
                             <asp:TextBox ID="txt_in1_date"  runat="server"   type="Date" CssClass="form-control"></asp:TextBox> 
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_in1_time" runat="server" Text="18:30" type="Time" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label>Time Out 2</label>
                    <div class="row">
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_out2_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_out2_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                
                <div id="brek" runat="server">
                    <div class="form-group">
                        <label>Time Out 1</label>
                        <asp:TextBox ID="txt_out1_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="txt_out1_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Time In 2</label>
                        <asp:TextBox ID="txt_in2_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="txt_in2_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label>Reason</label>
                    <asp:DropDownList ID="txt_reason" runat="server" DataSourceID="sql_income" DataTextField="manual_type" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true" >
                    <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sql_income" runat="server"  
                    ProviderName="System.Data.SqlClient"  SelectCommand="select * from time_adjustment">
                    </asp:SqlDataSource>
                </div>
                <div class="form-group no-margin">
                    <label>Notes</label>
                    <asp:TextBox ID="txt_remarks" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Label ID="lbl_remarks" style=" color:Red;" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="btn_add" runat="server" Text="ADD" OnClick="save_time_adjustment" CssClass="btn btn-primary"/>
                <asp:Label ID="lbl_errmsg" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdn_scid" runat="server" />
    <asp:HiddenField ID="nightshift" runat="server" />
     <asp:HiddenField ID="hdn_rd" runat="server" />
</div>
<div id="modal_ot" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Overtime Request</h4>
            </div>
            <div class="modal-body">
              <div class="form-group">
                     <label>OT Reg. Hours</label>
                     <asp:Label ID="hdn_reg_othrs" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                  
                </div>
                  <div class="form-group">
                  <label>OT Night Hours</label>
                    <asp:Label ID="hdn_night_othrs" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                  </div>
                <div class="form-group">
                    <label>Date :</label>
                    <asp:Label ID="lbl_ddate" style=" color:Red;" runat="server" CssClass="form-control"></asp:Label>
                </div>
                <div class="form-group">
                    <label>Time Out</label>
                    <div class="row">
                        <div class="col-lg-6">
                            <asp:TextBox ID="TextBox1"  runat="server" Enabled="false" type="Date" CssClass="form-control"></asp:TextBox><%--Text="2018-06-19"--%>
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="TextBox2" runat="server" Enabled="false" type="Time" CssClass="form-control"></asp:TextBox><%--Text="18:30" --%>
                        </div>
                    </div>
                </div>
                <div class="form-group no-margin">
                    <label>Note</label>
                     <asp:TextBox ID="txt_otremarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button2" runat="server" Text="Process" OnClick="save_ot_request"  CssClass="btn btn-primary"/>
                <asp:Label ID="lbl_err_ot" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdn_setupout" runat="server" />
    <asp:HiddenField ID="hdn_actout" runat="server" />
</div>
<div id="modal_os" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Offset Request<asp:label ID="hdn_alocated_bal" style=" display:none;" runat="server"/></asp:label></h4>
            </div>
            <div class="modal-body">
                <asp:GridView ID="grid_temp_ot" AutoGenerateColumns="false"   CssClass="table table-striped table-bordered no-margin" runat="server">
                    <Columns>
                        <asp:BoundField DataField="date" HeaderText="Date"/>
                        <asp:BoundField DataField="excess" HeaderText="Excess Hours"/>
                       <%-- <asp:BoundField DataField="excess" HeaderText="Balance"/>--%>
                     
                        <asp:TemplateField>
                            <ItemTemplate> 
                                <asp:CheckBox ID="chk_" runat="server" OnCheckedChanged="click_checked" AutoPostBack="true" />                  
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="form-group">
                     <label>Total Excess Hours</label>
                    <asp:Label ID="lbl_teh" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
                <div class="form-group">
                    <label>Date</label>
                    <asp:TextBox ID="txt_off_date" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Time Out</label>
                    <asp:TextBox ID="txt_off_out" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Offset Hrs</label>
                    <asp:TextBox ID="txt_off_hrs" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group" style=" display:none">
                    <label>Date Offset</label>
                    <asp:DropDownList ID="ddl_doffset" runat="server"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Reason</label><asp:Label ID="lbl_off_reason" style=" color:Red;" runat="server"></asp:Label>
                    <asp:TextBox ID="txt_off_reason" TextMode="MultiLine" style=" resize:none;" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_off_err" runat="server" style=" display:none;" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button3" runat="server"  Text="Proccess" OnClick="save_off_set" Visible="false"  CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="hdn_total_tempot" runat="server" />
      <asp:HiddenField ID="hdn_max_offset" runat="server"/>
</div>
<div id="modal_cws" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton4" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Change Shift</h4>
            </div>
            <div class="modal-body">
             <div class="form-group">
                <label>Date</label>
                <asp:TextBox ID="txt_date" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Shift Code</label>
                <asp:DropDownList ID="ddl_shiftcode" runat="server" CssClass="form-control" >
                </asp:DropDownList>
            </div>
            <div class="form-group no-margin">
                <label>Remarks</label>
                <asp:TextBox ID="txt_r" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
   
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button4" runat="server"  Text="Save" OnClick="click_save_changeshift"  CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="dd" runat="server" />
</div>
<div id="WV" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton5" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Work Verification</h4>
            </div>
            <div class="modal-body">
            <div class="form-group">
               <asp:RadioButtonList ID="rbl_class" Enabled="false" runat="server" OnSelectedIndexChanged="select" AutoPostBack="true">
               <asp:ListItem Value="RD">Rest Day</asp:ListItem>
               <asp:ListItem Value="HD">Holliday</asp:ListItem>
               </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <label>Date</label>
                <asp:Label ID="Label1" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_otd" CssClass="txt_f form-control" Enabled="false" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            <div id="sc" runat="server" class="form-group" style=" display:none">
                <label>Shift Code</label>
                <asp:Label ID="lbl_sc" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:dropdownlist ID="ddl_sc" CssClass="datee form-control" AutoComplete="False" runat="server"></asp:dropdownlist>
            </div>
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_lineremarks" TextMode="MultiLine" CssClass="nobel form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            </div>
            <div class="box-footer pad">
                 <asp:Button ID="btn_save" runat="server" OnClick="click_save_wv" Text="Save" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>






    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="HiddenField4" runat="server" />
    <asp:HiddenField ID="HiddenField5" runat="server" />
</div>
<div id="hdrd_offset" runat="server"  class="modal fade in">
     <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton6" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">RD/HD Offsetting</h4>
            </div>
            <div class="modal-body">
            <div class="form-group">
               <asp:RadioButtonList ID="rbl_rdhd" Enabled="false" runat="server">
               <asp:ListItem Value="RD">Rest Day</asp:ListItem>
               <asp:ListItem Value="HD">Holliday</asp:ListItem>
               </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <label>Date From</label>
                <asp:Label ID="Label2" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_d_from" CssClass="txt_f form-control" Enabled="false" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date To</label>
                <asp:Label ID="Label5" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_d_to" CssClass="txt_f form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Number of HRS offset</label>
                <asp:Label ID="Label3" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_noho" CssClass="form-control" AutoComplete="False" runat="server" Enabled="false"></asp:TextBox>
            </div>
           
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="Label4" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_reason_offsethdrd" TextMode="MultiLine" CssClass="nobel form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            </div>
            <div class="box-footer pad">
                 <asp:Button ID="Button5" runat="server" OnClick="click_save_offset_hdrd"  Text="Save" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField6" runat="server" />
    <asp:HiddenField ID="HiddenField7" runat="server" />
</div>
<asp:HiddenField ID="payrange" runat="server" />
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" runat="server">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>
</asp:Content>
