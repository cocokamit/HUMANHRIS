<%@ Page Language="C#" AutoEventWireup="true" CodeFile="scdetails.aspx.cs" Inherits="content_payroll_scdetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
         *{ font-size:12px}
        .hide{ display: none;}
        </style>

<%--<script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
           // freaze("grid_view", "container");



            var width = new Array();
            var table = $("table[id*=grid_view]"); //Pass your gridview id here.
            table.find("th").each(function (i) {
                width[i] = $(this).width();
            });
            headerRow = table.find("tr:first");
            headerRow.find("th").each(function (i) {
                $(this).width(width[i]);
            });
            firstRow = table.find("tr:first").next();
            firstRow.find("td").each(function (i) {
                $(this).width(width[i]);
            });
            var header = table.clone();
            header.empty();
            header.append(headerRow);
            //            header.append(firstRow);
            // header.css("width", width);
            header.width(table.width() + 20);
            $("#container").before(header);
            table.find("tr:first td").each(function (i) {
                $(this).width(width[i]);
            });
            //  $("#" + y + "").css("width", "100%");
            $("#container").height(300);
//            $("#container").css("width", "1000");
           $("#container").width(table.width() + 20);
            $("#container").append(table);
        });
    </script>--%>

    

    <link rel="stylesheet" type="text/css" href="../../style/fixedheader/normalize.css"  /> <%--href="style/fixedheader/normalize.css"--%>
		<link rel="stylesheet" type="text/css" href="../../style/fixedheader/demo.css" />
		<link rel="stylesheet" type="text/css" href="../../style/fixedheader/component.css" />

    <style type="text/css">
  .sticky-wrap .sticky-intersect th { background-color:#437cab}
  .sticky-intersect th , .sticky-col tbody tr th {border:1px solid #fff !important} 
    </style>


</head>
<body >
    <form id="form1" runat="server">
    <div>
    <h2>Service Charge</h2>
<hr />
<asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
<asp:Button ID="Button1" runat="server" OnClick="clicksearch"   Text="Search" />
<asp:Button ID="btnExport" runat="server" OnClick="ExportToExcel" Text="Export To Excel"  />
<div style=" overflow:auto;  width:100%; height:500px; margin:5px 5px 100px 5px;">
<asp:GridView ID="grid_item" runat="server"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
    <Columns>
        <asp:BoundField DataField="employee" HeaderText="Employee"/>
        <asp:BoundField DataField="department" HeaderText="Department"/>
        <asp:BoundField DataField="position" HeaderText="Position"/>
        <asp:BoundField DataField="manhr" HeaderText=" Actual Man Hour"/>
        <asp:BoundField DataField="grossamt" HeaderText="Service Charge"/>
        <asp:BoundField DataField="wht" HeaderText="WHT"/>
        <asp:BoundField DataField="net" HeaderText="Net Pay"/>
    </Columns>
</asp:GridView>
</div>
   
    </div>
    <script src="style/js/jquery.min.js"></script>
    <script src="style/js/JScript.js"></script>
	<script src="style/js/jquery.stickyheader.js"></script>

    <script type="text/javascript">

        //        $('tbody tr td:nth-child(1)').each(function () {
        //            var $td = $(this);
        //            //$td.html('<a href="#">' + $td.text() + '</a>');
        //            $td.replaceWith("<th>" + $td.text() + "</th>")
        //        });

    </script>
    </form>
</body>
</html>
