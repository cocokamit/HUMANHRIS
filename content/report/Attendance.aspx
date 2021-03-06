<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="content_report_Attendance" MasterPageFile="~/content/MasterPageNew.master"%>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
.GridViewStyle{font-family:Verdana;font-size:8px;background-color: White; text-align:center;}
.GridViewHeaderStyle{font-family:Verdana;font-size:8px; text-align:center; padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }

.rdl tr { float:left; padding:4px;}
.textmode { mso-number-format:\@; font-family:Verdana;font-size:8px;color:#666666; }  .ColorGridCabe{font-family:Verdana;font-size:8px;background-color:#449FC9;color:White;width:200px}
</style>
<style>
        .marlon { width:99%}
        .marlon i {color:#649fec;float:left;margin:3px 10px 0 0}
        .sub-marlon { float:left; width:calc(100% - 30px)}
        .sub-marlon span { display:block}
        .ann-date { float:right; margin-top:11px; color:#444; font-size:10px}
        .tbl { width:100%}
        .tbl,  .tbl tr,  .tbl th , .tbl td {border:none}
        .tbl tr {border-bottom:1px solid #eee;}
        .tbl tr:last-child {border:none}
        .tbl td {padding:10px 0}
        .tbl td span { line-height:20px}
        .editor {border:none}
        .no-border, .no-border td,.no-border th, .no-border tr { border:none}
    </style>

              <script language="javascript" type="text/javascript">

                  function CreateGridHeader(DataDiv, content_grid_view, HeaderDiv) {

                      var DataDivObj = document.getElementById(DataDiv);
                      var DataGridObj = document.getElementById(content_grid_view);
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
                      HeaderDivObj.style.margintop = '30px';

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
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_attReport">
<div class="page-title">
    <div class="title_left">
        <h3>Attendance Report</h3>
    </div>
    </div>
    <div class="title_right">
        <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
            <div class="input-group">
                <div class="x-head">
                    
                </div>
            </div>
        </div>
    </div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12  col-sm-12 col-xs-12">
        <div class="x_panel">
        <div id="HeaderDiv"></div>

        <asp:TextBox ID="txt_inout" Height="33px" type="Date" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddl_deptsearch" runat="server" OnSelectedIndexChanged="selectdepartment" AutoPostBack="true">
        </asp:DropDownList>
        <asp:Button ID="btnsearch" runat="server" Text="Search" OnClick="selectsearch" Height="36px"/><br /><br />
        <asp:GridView ID="grid_inout" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin" BorderColor="Turquoise">
            <Columns>
                <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="Department" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="ShiftCode" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="FullName" HeaderText="Employee Name"/>
                <asp:BoundField DataField="Date" HeaderText="Date"/>
                <asp:BoundField DataField="TimeIn1" HeaderText="Time In"/>
                <asp:BoundField DataField="TimeOut1" HeaderText="Time Out"/>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="dtrid" runat="server" />
        </div>
    </div>
</div>
</asp:Content>
