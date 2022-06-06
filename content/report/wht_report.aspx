<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wht_report.aspx.cs" Inherits="content_report_wht_report" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
.GridViewStyle{font-family:Verdana;font-size:12px;background-color: White; text-align:center;}
.GridViewHeaderStyle{font-family:Verdana;font-size:12px; text-align:center; padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
.rdl tr { float:left; padding:4px;}
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
        <h3>WTX. Report</h3>
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
              <asp:DropDownList ID="ddl_month" runat="server" OnTextChanged="search" AutoPostBack="true">
                        <asp:ListItem Value="0">Select Month</asp:ListItem>
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">Febuary</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
               </asp:DropDownList>
                <asp:DropDownList ID="ddl_year" runat="server" OnTextChanged="search" AutoPostBack="true">
               </asp:DropDownList>
                <div id="exp_report" runat="server" visible="false">
                    <asp:LinkButton ID="Button3" runat="server" ToolTip="EXCEL" OnClick="ExportToExcel" CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                   <%-- <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="PDF"  OnClick = "ExportGridToword"  CssClass="right add"><i class="fa fa-file-pdf-o"></i></asp:LinkButton>--%>
              
                    <asp:Label ID="lbl_filter_info" runat="server" style=" font-size:9px; font-weight:bold;"></asp:Label>
                </div>
  <div id="HeaderDiv"></div>
  <div id="DataDiv" style="overflow: auto; border: 0px solid #286090; width: 100%; height: 500px; margin-top:1px; " onscroll="Onscrollfnction();">
  <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="False" CssClass="GridViewStyle">
           <HeaderStyle  CssClass="GridViewHeaderStyle" />
                    <Columns>
                            <asp:BoundField DataField="Employee" HeaderText="Employee"/>
                              <asp:BoundField DataField="department" HeaderText="Department"/>
                            <asp:BoundField DataField="tin" HeaderText="TIN Number"/>
                            <asp:BoundField DataField="netincome" HeaderText="Net Income" DataFormatString="{0:n2}"/>
                   
                            <asp:BoundField DataField="Total" HeaderText="Total WHT" DataFormatString="{0:n2}"/>
                    </Columns>
                </asp:GridView>
                </div>
        <asp:HiddenField ID="dtrid" runat="server" />

  
          </div>
        </div>
     </div>




</asp:Content>