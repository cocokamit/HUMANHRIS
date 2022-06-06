<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addDTR.aspx.cs" Inherits="content_payroll_DTR" MasterPageFile="~/content/MasterPageNew.master" %>
 <asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
     <style type="text/css">
        .left { margin-top:2px}
        .x-head th{ text-align:left}
        .remarks{ float:left; margin-top:10px}
        .remarks h4 { color:Red}
        .remarks textarea{ border:1px solid gray; width:800px; font-size:12px; padding:5px}
        .Grid { background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; font-family:Calibri; color: #474747; width: 100%; font-size: 12px; text-align: center;}
        .Grid td {padding: 2px; border: solid 1px #655858; }
        .Grid th {padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
        .Grid .alt {background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }
        .Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }
        .Grid .pgr table { margin: 3px 0; }
        .Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  
        .Grid .pgr a { color: Gray; text-decoration: none; }
        .Grid .pgr a:hover{ color: #000; text-decoration: none; }
        .Grid select {border:none; background: #fff;  -webkit-appearance: none;-moz-appearance: none;text-indent: 1px;text-overflow: '';}
        table th{padding : 4px 2px;color:gray; border:none; font-size:10px;text-align: center;text-transform: uppercase;font-weight: bold;}
        .pre li { margin-left:-40px}
        .pre select { width:300px}
        textarea{ width:50%}
        textarea:focus,textarea:active{ border:none}
        .nobel {position: fixed; overflow:hidden;top: 42%;left: 50%;margin-left:-70px;z-index: 2001;padding: 20px;  }
        .GridViewStyle{font-family:Verdana;font-size:11px; background-color: White; }
        .GridViewHeaderStyle{font-family:Verdana;font-size:xx-small;height:30px; text-align:center; padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
    </style>
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
        <script language="javascript" type="text/javascript">

            function CreateGridHeader(DataDiv, content_grid_item, HeaderDiv) {

                var DataDivObj = document.getElementById(DataDiv);
                var DataGridObj = document.getElementById(content_grid_item);
                var HeaderDivObj = document.getElementById(HeaderDiv);

                //********* Creating new table which contains the header row ***********
                var HeadertableObj = HeaderDivObj.appendChild(document.createElement('table'));

                DataDivObj.style.paddingTop = '0px';
                var DataDivWidth = DataDivObj.clientWidth;
                DataDivObj.style.width = '5000px';

                //********** Setting the style of Header Div as per the Data Div ************
                HeaderDivObj.className = DataDivObj.className;
                HeaderDivObj.style.cssText = DataDivObj.style.cssText;
                //**** Making the Header Div scrollable. *****
                HeaderDivObj.style.overflow = 'auto';
                //*** Hiding the horizontal scroll bar of Header Div ****
                HeaderDivObj.style.overflowX = 'hidden';
                //**** Hiding the vertical scroll bar of Header Div **** 
                HeaderDivObj.style.overflowY = 'hidden';
                HeaderDivObj.style.height = '30px'; //DataGridObj.rows[0].clientHeight + 'px';

                //**** Removing any border between Header Div and Data Div ****
                HeaderDivObj.style.borderBottomWidth = '0px';

                //********** Setting the style of Header Table as per the GridView ************
                HeadertableObj.className = DataGridObj.className;
                //**** Setting the Headertable css text as per the GridView css text 
                HeadertableObj.style.cssText = DataGridObj.style.cssText;
                HeadertableObj.border = '1px';
                HeadertableObj.rules = 'all';
                HeadertableObj.cellPadding = DataGridObj.cellPadding;
                HeadertableObj.cellSpacing = DataGridObj.cellSpacing;

                //********** Creating the new header row **********
                var Row = HeadertableObj.insertRow(0);
                Row.className = DataGridObj.rows[0].className;
                Row.style.cssText = DataGridObj.rows[0].style.cssText;
                Row.style.fontWeight = '8px';

                //******** This loop will create each header cell *********
                for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
                    var spanTag = Row.appendChild(document.createElement('td'));
                    spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;
                    var width = 0;
                    //****** Setting the width of Header Cell **********
                    if (spanTag.clientWidth > DataGridObj.rows[1].cells[iCntr].clientWidth) {
                        width = spanTag.clientWidth;
                    }
                    else {
                        width = DataGridObj.rows[1].cells[iCntr].clientWidth;
                    }
                    if (iCntr <= DataGridObj.rows[0].cells.length - 2) {
                        spanTag.style.width = width + 'px';
                    }
                    else {
                        spanTag.style.width = width + 20 + 'px';
                    }
                    DataGridObj.rows[1].cells[iCntr].style.width = width + 'px';
                }
                var tableWidth = DataGridObj.clientWidth;
                //********* Hidding the original header of GridView *******
                DataGridObj.rows[0].style.display = 'none';
                //********* Setting the same width of all the componets **********
                HeaderDivObj.style.width = DataDivWidth + 'px';
                DataDivObj.style.width = DataDivWidth + 'px';
                DataGridObj.style.width = tableWidth + 'px';
                HeadertableObj.style.width = tableWidth + 20 + 'px';
                return false;
            }

            function Onscrollfnction() {
                var div = document.getElementById('DataDiv');
                var div2 = document.getElementById('HeaderDiv');
                //****** Scrolling HeaderDiv along with DataDiv ******
                div2.scrollLeft = div.scrollLeft;
                return false;
            }
    
    </script>
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
     <%--<script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
     
     <script type="text/javascript" src="script/freezeheadergv/index.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
            freaze("grid_item", "container");
        });
    </script>--%>
</asp:Content>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left">
        <h3>Process DTR</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-bug"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li><a href="Mdtrlist?user_id=<% Response.Write(Session["user_id"]); %>">Adjusted Record</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Process DTR</li>
       </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div id="xp" class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" AutoPostBack="true"  OnSelectedIndexChanged="selectpg" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddl_pay_range" runat="server" ></asp:DropDownList>
              <%--<asp:TextBox ID="txt_f" runat="server" Enabled="false"  CssClass="datee" autocomplete="off" placeholder='From'></asp:TextBox>
                  <asp:TextBox ID="txt_t" runat="server" Enabled="false" CssClass="datee" autocomplete="off" Placeholder='To'></asp:TextBox>--%>
                <asp:Button  ID="btn_go" runat="server" OnClick="payroll_verify" Text="GO" CssClass="btn btn-primary load-nobel" />
                
                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                <hr />
            </div>
            <ul id="tbl1" runat="server" visible="false" class="pre">
                <div style="display:none;>
                    <li>DTR</li>
                    <li><asp:DropDownList ID="ddl_dtrfile" runat="server" ></asp:DropDownList></li>
                </div>
                <li>Remarks</li>
                <li><asp:TextBox ID="txt_remarks" ClientIDMode="Static" runat="server" style="resize:none; width:100%" TextMode="MultiLine"></asp:TextBox></li>
                <li><asp:Button ID="procdtr" Visible="false" runat="server" OnClick="click_save_DTR" Text="Process DTR"  CssClass="btn btn-primary load-nobel" /></li>
            </ul>
           <%--  <div id="container" style="overflow: scroll; overflow-x: hidden;">
            </div>--%>
            <div>
             <div id="HeaderDiv"></div>
             <div id="DataDiv" style="overflow: auto; border: 0px solid #286090; width: 100%; height: 300px; margin-top:1px; " onscroll="Onscrollfnction();">
             <asp:GridView ID="grid_item" runat="server"  AutoGenerateColumns="False" CssClass="GridViewStyle">
                <HeaderStyle  CssClass="GridViewHeaderStyle" />
                <Columns>
                    <asp:BoundField DataField="employee" HeaderText="Employee"  />
                    <asp:BoundField DataField="ShiftCode" HeaderText="Shift Code" />
                    <asp:BoundField DataField="Date" HeaderText="Date" />
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" />
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" />
                    <asp:BoundField DataField="timein1" HeaderText="Time In 1" />
                    <asp:BoundField DataField="timeout1" HeaderText="Time Out 1" />
                    <asp:BoundField DataField="timein2" HeaderText="Time In 2" />
                    <asp:BoundField DataField="timeout2" HeaderText="Time Out 2" />
                    <asp:BoundField DataField="olw" HeaderText="On leave Wholeday" />
                    <asp:BoundField DataField="aw" HeaderText="Absent Wholeday" />
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" />
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday" />
                    <asp:BoundField DataField="reg_hr" HeaderText="Regular Working Hours" />
                    <asp:BoundField DataField="offsethrs" HeaderText="Offset Hrs" />
                    <asp:BoundField DataField="night" HeaderText="Night Working Hours" />
                    <asp:BoundField DataField="ot" HeaderText="OT Hours"  />
                    <asp:BoundField DataField="otn" HeaderText="OTN Hours"  />
                    <asp:BoundField DataField="totalhrs" HeaderText="Total Hours" />
                    <asp:BoundField DataField="late" HeaderText="Late Hours" />
                    <asp:BoundField DataField="ut"  HeaderText="Undertime Hours" />
                    <asp:BoundField DataField="nethours"  DataFormatString="{0:N2}" HeaderText="Net Hours"   />
                <%--    <asp:BoundField DataField="nethours" HeaderText="Price" DataFormatString="{0:C1}" HtmlEncode="false" /> --%>
                
                  <%--<asp:BoundField DataField="reg_amount" HeaderText="Regular Amount" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="night_amount" HeaderText="Night Amount"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="ot_amount" HeaderText="OT Ampunt"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="otn_amount" HeaderText="OTN Amount"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="tardyamt" HeaderText="Tardy Amount"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="absentamt" HeaderText="Absent Amount" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="restdayamt" HeaderText="Restday Amount"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="holidayamt" HeaderText="Holiday Amount"  HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    <asp:BoundField DataField="leaveamt" HeaderText="Leave Amount" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="netamt" HeaderText="Net Amount" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="hrrate" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none" />
--%>                </Columns>
            </asp:GridView>
            </div>
            </div>
            <script type="text/javascript">
                //alert(document.getElementById("xp").offsetWidth);
                var a = document.getElementById("xp").offsetWidth;
                var ele = document.getElementsByClassName('top_nav');
                var eleb = document.getElementsByClassName('page-title');
                //alert(ele.length ? ele[0].offsetWidth : 'no elements with the class');
                var tbl = document.getElementById("grid_item").rows.length;
                var b = document.getElementById("grid_item").offsetWidth;
                //alert(b);
                //alert(a);
                if (b > a) {
                    if (tbl > 0) {
                        ele[0].style.width = "calc(100% - 40px)";
                        eleb[0].style.width = (b + 35) + "px";
                    }
                    document.getElementById("xp").style.width = "auto";
                }
            </script>
        </div>
    </div>
</div>


<div id="modalrd" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        </div>
        <div class="modal-body">
           <asp:GridView ID="gridchker" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Employee">
                      <ItemTemplate>
                        <asp:LinkButton ID="lbirlistfiles" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("employee") %>' OnClick="landingpage"></asp:LinkButton>
                      </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>

<div id="modalsetup" runat="server" class="modal fade in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        </div>
        <div class="modal-body">
           <asp:GridView ID="gridsetup" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Employee">
                      <ItemTemplate>
                        <asp:LinkButton ID="lbirlistfiles" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("employee") %>' OnClick="landingpage"></asp:LinkButton>
                      </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PayrollTypeId" HeaderText="Payroll Type"/>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                    <asp:BoundField DataField="MonthlyRate" HeaderText="Monthly Rate"/>
                    <asp:BoundField DataField="DailyRate" HeaderText="Daily Rate"/>
                    <asp:BoundField DataField="HourlyRate" HeaderText="Hourly Rate"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>

<div id="modalinfinate" runat="server" class="modal fade in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
            <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Please Update Monthly Rate to correct Hourly Rate in SysConfig(Change Payroll Rate)."></asp:Label>
        </div>
        <div class="modal-body">
           <asp:GridView ID="gridinfinity" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Employee">
                      <ItemTemplate>
                        <asp:LinkButton ID="lbirlistfiles" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("employee") %>' OnClick="landingpage"></asp:LinkButton>
                      </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PayrollTypeId" HeaderText="Payroll Type"/>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                    <asp:BoundField DataField="MonthlyRate" HeaderText="Monthly Rate"/>
                    <asp:BoundField DataField="DailyRate" HeaderText="Daily Rate"/>
                    <asp:BoundField DataField="HourlyRate" HeaderText="Hourly Rate"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </div>
</div>

<asp:HiddenField ID="user_id" runat="server" />
  
<asp:HiddenField ID="hfmastercount" runat="server" ClientIDMode="Static" Value="0" />

</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script src="vendors/jquery.inputmask/dist/min/jquery.inputmask.bundle.min.js"></script>
<script type='text/javascript' src="script/inputmasking/4lenghtshiftdosjquery-1.11.0.js"></script>
<script type='text/javascript'>
    $.noConflict();
    $(window).load(function () {
        $(":input").inputmask();
    });
    $(".load-nobel").click(function () {
        $("#load").removeClass("hide");
        $("#loader").removeClass("hide");
    });

  
</script>
    <div id="load" class="Overlay hide"></div>
    <div id="loader"  class="nobel hide">
        <img src="style/images/loading.gif" alt="loading" />
    </div>
</asp:Content>