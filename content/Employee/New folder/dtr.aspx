<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtr.aspx.cs" Inherits="content_Employee_dtr"  MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_dtr">
    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function($) {
            $(function() {
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
                <div class="input-group input-group-sm">
                <asp:DropDownList ID="ddl_pay_range" CssClass="select2" OnTextChanged="btn_go" AutoPostBack="true" runat="server"></asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" OnClick="btn_go" style=" display:none;" Text="search" CssClass="btn btn-primary"/>
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="txt_f form-control" style="float:left; width:150px; margin-right:5px; display:none; " ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="txt_f form-control" style="float:left; width:150px;margin-right:5px;  display:none; "  placeholder="To" runat="server" ></asp:TextBox>
                </div>
            </div>
            <div class="box-body" style="padding-top:0 !important">
                <div class="table-responsive">
            <asp:GridView ID="grid_item" runat="server" ClientIDMode="Static" BorderColor="Turquoise" OnRowDataBound="ordb" AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"> 
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="employee" HeaderText="Employee" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="ShiftCode" HeaderText="SHIFT CODE"/>
                    <asp:BoundField DataField="Date" HeaderText="DATE" />
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein1" HeaderText="LOG IN" />
                    <asp:BoundField DataField="timeout1" HeaderText="Out 1" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein2" HeaderText="In 2" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timeout2" HeaderText="LOG OUT" />
                    <asp:BoundField DataField="olw" HeaderText="LEAVE"/>
                    <asp:BoundField DataField="aw" HeaderText="ABSENT"/>
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="reg_hr" HeaderText="REG. HRS"  />
                    <asp:BoundField DataField="night" HeaderText="NIGHT HRS" />
                    <asp:BoundField DataField="offsethrs" HeaderText="OFFSET HRS"/>
                    <asp:BoundField DataField="ot" HeaderText="OT HRS" />
                    <asp:BoundField DataField="otn" HeaderText="OTN HRS" />
                    <asp:BoundField DataField="totalhrs" HeaderText="Total HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="late" HeaderText="LATE HRS" />
                    <asp:BoundField DataField="ut" HeaderText="UT HRS" />
                    <asp:BoundField DataField="nethours" HeaderText="Net HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="shiftcodeid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="empid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:TemplateField HeaderText="Status" >
                        <ItemTemplate> 
                            <asp:LinkButton ID="lnk_status"  Text='<%#bind("status")%>' OnClick="clickrd"  style=" font-size:10px;"   runat="server"></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle  Width="50px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="irregularity" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
             <%--        <asp:BoundField DataField="status" HeaderText="Status"/>--%>
                    <asp:TemplateField HeaderText="Action" >
                        <ItemTemplate> 
                            <asp:LinkButton ID="lnk_ot" ToolTip="Overtime Application" OnClick="add_ot" CssClass="fa fa-history border-right"  runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_tadj" ToolTip="Time Adjustment Application" OnClick="click_adj" CssClass="fa fa-adjust border-right" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_cws" ToolTip="Change Shift" OnClick="click_cws"  runat="server" CssClass="fa fa-share no-padding-right"></asp:LinkButton>
                            
                            <asp:LinkButton ID="lnk_offset" ForeColor="Red" Visible="false"  ToolTip="Offset Hours" OnClick="add_offset"  runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle  Width="105px"/>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
                </div>
            </div>
          </div>
        </div>
    </div>
</section>

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
                    <asp:SqlDataSource ID="sql_income" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from time_adjustment">
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
                     <label>Total Exist Hours</label>
                    <asp:Label ID="lbl_teh_ot" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
                <div class="form-group">
                    <label>Date</label>
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
                    <label>Notes</label>
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
                <h4 class="modal-title">Offset Request</h4>
            </div>
            <div class="modal-body">
             <div class="form-group">
                     <label>Total Exist Hours</label>
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
                    
                    <asp:Label ID="lbl_off_err" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button3" runat="server"  Text="Proccess" OnClick="save_off_set"   CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="hdn_total_tempot" runat="server" />
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
