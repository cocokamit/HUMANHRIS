<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailsperdtr.aspx.cs" Inherits="content_payroll_detailsperdtr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}

.none { display:none}
</style>


        <link rel="stylesheet" type="text/css" href="../../style/fixedheader/normalize.css"  /> <%--href="style/fixedheader/normalize.css"--%>
        <link rel="stylesheet" type="text/css" href="../../style/fixedheader/demo.css" />
        <link rel="stylesheet" type="text/css" href="../../style/fixedheader/component.css" />

        <style type="text/css">
        .sticky-wrap .sticky-intersect th { background-color:#437cab}
        .sticky-intersect th , .sticky-col tbody tr th {border:1px solid #fff !important} 
        </style>
</head>
<body>
    <form id="form1" runat="server">

    <div>
    <h2>Daily Time Record</h2>
<hr />
  <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
  <asp:Button ID="Button1" runat="server" OnClick="search" Text="Search" /><%--OnClick="search" --%>
    <asp:Button ID="btnExport" runat="server" Text="Export To Excel"  CssClass="btn btn-primary" OnClick = "ExportToExcel"  />
  <asp:GridView ID="grid_view" runat="server" ClientIDMode="Static"  OnRowDataBound="OnRowDataBound" Class="overflow-y"   AutoGenerateColumns="false" >
                    <Columns>
                            <asp:BoundField DataField="IdNumber" HeaderText="Id Number" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="e_name" HeaderText="Employee"/>
                            <asp:BoundField DataField="ShiftCode" HeaderText="Shiftcode"/>
                            <asp:BoundField DataField="RestDay" HeaderText="Rest Day"/>
                            <asp:BoundField DataField="DayMultiplier" HeaderText="Day Multiplier"/>
                            <asp:BoundField DataField="date" HeaderText="Date"/>
                            <asp:BoundField DataField="TimeIn1" HeaderText="Time IN 1"/>
                            <asp:BoundField DataField="TimeOut1" HeaderText="Time OUT 1"/>
                            <asp:BoundField DataField="TimeIn2" HeaderText="Time IN 2"/>
                            <asp:BoundField DataField="TimeOut2" HeaderText="Time OUT 2"/>
                            <asp:BoundField DataField="OnLeave" HeaderText="Wholeday Leave"/>
                            <asp:BoundField DataField="HalfLeave" HeaderText="Halfday Leave"/>
                            <asp:BoundField DataField="Absent"  HeaderText="Wholeday Absent"/>
                            <asp:BoundField DataField="HalfdayAbsent" HeaderText="Halfday Absent"/>
                            <asp:BoundField DataField="RegularHours" HeaderText="Regular HRS." DataFormatString="{0:n2}" />
                            <asp:BoundField DataField="totaloffsethrs" HeaderText="Offset HRS." DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="NightHours" HeaderText="Night HRS." DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="OvertimeHours" HeaderText="OT HRS." DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="OvertimeNightHours" HeaderText="OTN HRS." DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="TardyLateHours" HeaderText="Late HRS." DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="TardyUndertimeHours" HeaderText="Undertime HRS" DataFormatString="{0:n2}"/>
                            <asp:BoundField DataField="RegularAmount" HeaderText="Regular Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="NightAmount" HeaderText="Night Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="OvertimeAmount" HeaderText="OT Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="OvertimeNightAmount" HeaderText="OT Night Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="LAmount" HeaderText="Late Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="UAmount" HeaderText="Undertime Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="RDAmount" HeaderText="RD Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="HDAmount" HeaderText="Holliday Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="AbsentAmount" HeaderText="Absent Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="LeaveAmount" HeaderText="Leave Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                            <asp:BoundField DataField="NetAmount" HeaderText="Net Amt." HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none"/>
                    </Columns>
                </asp:GridView>
        <asp:HiddenField ID="dtrid" runat="server" />

    <script src="style/js/jquery.min.js"></script>
    <script src="style/js/JScript.js"></script>
	<script src="style/js/jquery.stickyheader.js"></script>

   <%-- <script type="text/javascript">
        $('tbody tr td:nth-child(1)').each(function () {
            var $td = $(this);
            //$td.html('<a href="#">' + $td.text() + '</a>');
            $td.replaceWith("<th>" + $td.text() + "</th>")
        });

    </script>--%>
    </form>
</body>
</html>
