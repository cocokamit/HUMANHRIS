<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuditTrail.aspx.cs" EnableEventValidation="false" Inherits="content_report_AuditTrail" MasterPageFile="~/content/MasterPageNew.master"%>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    .x_title { border-bottom:2px solid #E6E9ED}
</style>
<%--<script src="http://cdnjs.cloudflare.com/ajax/libs/jquery/2.0.3/jquery.min.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.2/raphael-min.js"></script>--%>
    <script src="script/createdjs/jquery.js"></script>
    <script src="script/createdjs/raphael.js"></script>
    <script src="vendors/morris.js/morris.js"></script>
     <%--<script src="http://cdnjs.cloudflare.com/ajax/libs/prettify/r224/prettify.min.js"></script>--%>
     <script src="script/createdjs/prettify.js"></script>
    <script src="vendors/morris.js/examples/lib/example.js"></script>
    <%-- <link rel="stylesheet" href="http://cdnjs.cloudflare.com/ajax/libs/prettify/r224/prettify.min.css">--%>
    <link rel="stylesheet" href="script/createdjs/prettifyr224.js">
    <link rel="stylesheet" href="vendors/morris.js/morris.css">

<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>

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

<asp:Content ContentPlaceHolderID="content" runat="server" ID="audit_trail">
    <div class="page-title">
    <div class="title_left">
        <h3>Audit Trail</h3>
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

        <div class="col-md-8  col-sm-6 col-xs-6">
            <div class="x_panel">
                <div class="x_title">
                <h2>Statistics</h2>
                <div class="clearfix"></div>
                </div>
                <div class="x_content">
                     <div id="trailcnt"></div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
        <div class="x_panel">
            <div class="x_title">
            <h2>Count</h2>
            <div class="clearfix"></div>
            </div>
            <div class="x_content">
                 <div id="counter"></div>
            </div>
        </div>
    </div>

        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x_title">
                <h2>Data</h2>
                <div class="clearfix"></div>
                </div>
                <asp:DropDownList ID="ddl_range" runat="server" AutoPostBack="true" OnTextChanged="filterby">
                    <asp:ListItem Value="0">Select Filter</asp:ListItem>
                    <asp:ListItem Value="1">Daily</asp:ListItem>
                    <asp:ListItem Value="2">Monthly</asp:ListItem>
                    <asp:ListItem Value="3">Yearly</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtbydate"  Height="33px" type="Date" runat="server"></asp:TextBox>
                <asp:DropDownList ID="ddlmonth" Visible="false" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlyeaer" Visible="false" runat="server">
                </asp:DropDownList>

                <asp:Button ID="btnsearch" Height="36px" Visible="false" runat="server" OnClick="clckserch" Text="Go" />
                <asp:Button ID="btnviewall" Height="36px" Visible="false" runat="server" OnClick="viewall" Text="View all" />
                <div class="x-head">  
                     <div id="exp_report" runat="server">
                     <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Export to Excel"  OnClick = "generatereport"  CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                     <asp:LinkButton ID="LinkButton2" runat="server" Visible="false" ToolTip="Export to Excel"  OnClick = "generatereportall"  CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                     <asp:Label ID="lbl_filter_info" runat="server" style=" font-size:9px; font-weight:bold;"></asp:Label>
                    </div>
                </div>
                <div class="title_right">
                    <asp:TextBox ID="txt_search" Height="36px" placeholder="Search Subject..." CssClass="auto" runat="server"></asp:TextBox>
                    <asp:Button ID="btnname" Height="38px" runat="server" OnClick="namesearch" Text="Go" />
                    <asp:Button ID="btnallname" Height="36px" Visible="false" runat="server" OnClick="namesearchall" Text="View all" />
                </div>
                <div id="alert" visible="false" runat="server" class="alert alert-empty">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <asp:GridView ID="gridaudit" AllowPaging="true" OnPageIndexChanging="changeonpage" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
                    <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="UserId" HeaderText="By" />
                    <asp:BoundField DataField="FullName" HeaderText="Subject" />
                    <asp:BoundField DataField="Action" HeaderText="Action" />
                    <asp:BoundField DataField="Transact" HeaderText="Transaction" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject Action" />
                    <asp:BoundField DataField="Particular" HeaderText="Particular" />
                    <asp:BoundField DataField="AlterF" HeaderText="From" />
                    <asp:BoundField DataField="AlterT" HeaderText="To" />
                    <asp:BoundField DataField="Sysdate" HeaderText="Date" />
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gridallview" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
                    <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="UserId" HeaderText="By" />
                    <asp:BoundField DataField="FullName" HeaderText="Subject" />
                    <asp:BoundField DataField="Action" HeaderText="Action" />
                    <asp:BoundField DataField="Transact" HeaderText="Transaction" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject Action" />
                    <asp:BoundField DataField="Particular" HeaderText="Particular" />
                    <asp:BoundField DataField="AlterF" HeaderText="From" />
                    <asp:BoundField DataField="AlterT" HeaderText="To" />
                    <asp:BoundField DataField="Sysdate" HeaderText="Date" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>

    <pre id="code" class="prettyprint linenums  hidden">
    Morris.Bar({
      element: 'trailcnt',
      data: [<% Response.Write(lbtrailcnt.Text); %>],
      xkey: 'y',
      ykeys: ['a'],
      labels: ['Volume'],
      horizontal: true,
      stacked: true
    });
    Morris.Donut({
      element: 'counter',
      data: [<% Response.Write(lbtrailcnt2.Text); %>],
        colors: [
       '#006bb3 ', <%--blue--%>
        '#006600 ', <%--Green--%>
        '#cc0000', <%--Red--%>
        '#ff704d ', <%--Orange--%>
        '#f56953 ' <%--Darkorange--%>
      ],
      formatter: function (x) { return x + ""}
    }).on('click', function(i, row){
      console.log(i, row);
    });
   <%-- Morris.Line({
     element: 'linegraph',
     data:[<%Response.Write(lblinegraph.Text); %>],
     color:['#cc0000'],

    });--%>
    </pre>

    <asp:Label ID="lbtrailcnt" runat="server" CssClass="none"></asp:Label>
    <asp:Label ID="lbtrailcnt2" runat="server" CssClass="none"></asp:Label>

<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<script type="text/javascript">
    $(document).ready(function() {
        $.noConflict();
        $(".auto").autocomplete({
            source: function(request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/report/AuditTrail.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function(data) {
                        console.log("test");
                        response($.map(data.d, function(item) {
                            return {
                                label: item.split('-')[1],
                                val: item.split('-')[0]
                            }
                        }))
                    },
                    error: function(result) {
                        alert(result.responseText);
                    }
                });
            },
            select: function(e, i) {
                index = $(".auto").parent().parent().index();
                $("#lbl_bals").val(i.item.val);
            }
        });
    });
</script>
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!-- DataTables -->
<script src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script>
    $(function () {
        $('#example2').DataTable()
        $('#example1').DataTable({
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            'paging': true,
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            'autoWidth': false
        })
    })
</script>

</asp:Content>