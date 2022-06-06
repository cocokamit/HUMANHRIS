<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="DTR_att.aspx.cs" Inherits="content_hr_DTR_att" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <!--Datepicker-->
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

    <style type="text/css">
        .table {margin-top:40px !important}
         .datetimepicker { position:absolute; z-index:9999}
         .dataTables_filter i 
         {
             border:1px solid red;
    margin-left: -25px;
    color: #d2d6de;
    margin-top: -25px;
    position: absolute;
}
        .alert {padding:10px; margin-top:45px !important}
    </style>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<section class="content-header">
    <div class="page-title">
        <div class="title_left pull-left">
            <h3>Attendance</h3>
        </div>   
        <div class="title_right">
           <ul>
            <li><a href="dashboard?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-dashboard"></i> Dashboard</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"> DTR</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Attendance</li>
           </ul>
        </div>
    </div>
</section>

<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <div class="datetimepicker">
                    <asp:DropDownList ID="drop_emp" ClientIDMode="Static" CssClass="select2" runat="server" style="float:left !important" ></asp:DropDownList>
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="txt_f form-control" autocomplete="off" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="txt_f form-control" autocomplete="off"  style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" OnClick="btn_go"  Text="GO" CssClass="btn btn-primary"/>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="pnl_alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_item" runat="server" ClientIDMode="Static" BorderColor="Turquoise" OnRowDataBound="ordb" AutoGenerateColumns="False" CssClass="table table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"> 
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="employee" HeaderText="Employee" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:TemplateField HeaderText="SHIFT CODE" >
                        <ItemTemplate> 
                            <asp:LinkButton ID="lnk_shift" runat="server" Text='<%#Eval("ShiftCode")%>' style=" font-size:11px; color:Black" ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Date" HeaderText="DATE" />
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein1" HeaderText="IN" />
                    <asp:BoundField DataField="timeout1" HeaderText="Out 1" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein2" HeaderText="In 2" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timeout2" HeaderText="OUT" />
                    <asp:BoundField DataField="olw" HeaderText="LEAVE"/>
                    <asp:BoundField DataField="aw" HeaderText="ABSENT" />
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="reg_hr" HeaderText="REG. HRS" />
                    <asp:BoundField DataField="night" HeaderText="NIGHT HRS" />
                    <asp:BoundField DataField="offsethrs" HeaderText="OFFSET HRS"/>
                    <asp:BoundField DataField="ot" HeaderText="OT HRS" />
                    <asp:BoundField DataField="otn" HeaderText="OTN HRS" />
                    <asp:BoundField DataField="totalhrs" HeaderText="Total HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="late" HeaderText="LATE HRS"/>
                    <asp:BoundField DataField="ut" HeaderText="UT HRS"/>
                    <asp:BoundField DataField="nethours" HeaderText="Net HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="shiftcodeid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="empid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:TemplateField HeaderText="STATUS" >
                        <ItemTemplate> 
                            <asp:LinkButton ID="lnk_status"  Text='<%#bind("status")%>' style=" font-size:10px;"   runat="server"></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle  Width="50px"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" Visible="false" >
                        <ItemTemplate> 
                            <asp:LinkButton ID="lnk_tadj" ForeColor="Green"  ToolTip="Time Adjustment Application" OnClick="click_adj" runat="server"><i class="fa fa-adjust"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_ot" ForeColor="Blue" Visible="false" ToolTip="Overtime Application" OnClick="add_ot"  runat="server"><i class="fa fa-clock-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_offset" ForeColor="Red" Visible="false"  ToolTip="Offset Hours" OnClick="add_offset"  runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle  Width="50px"/>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
           
        </div>
    </div>
</div>
</section>
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>
</asp:Content>

