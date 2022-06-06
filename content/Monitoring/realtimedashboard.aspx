<%@ Page Language="C#" AutoEventWireup="true" CodeFile="realtimedashboard.aspx.cs" Inherits="content_Monitoring_realtimedashboard" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .x_title { border-bottom:2px solid #E6E9ED}
        .hide{ display:none;}
    </style>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/jquery/2.0.3/jquery.min.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.2/raphael-min.js"></script>
    <script src="vendors/morris.js/morris.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/prettify/r224/prettify.min.js"></script>
    <script src="vendors/morris.js/examples/lib/example.js"></script>
    <link rel="stylesheet" href="http://cdnjs.cloudflare.com/ajax/libs/prettify/r224/prettify.min.css">
    <link rel="stylesheet" href="vendors/morris.js/morris.css">
    
</asp:Content>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<span>Time Sheet Monitoring</span>
<br />
<asp:Label ID="lbl_tlate" runat="server" Text="Label"></asp:Label><asp:Label ID="lbl_punchheadcount" runat="server" Text="Label"></asp:Label>
<asp:Label ID="lbl_tabsent" runat="server" Text="Label"></asp:Label>
 <asp:ScriptManager ID="myScriptManager" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="update" runat="server">
                <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="100" ontick="Timer1_Tick"></asp:Timer>
<asp:GridView ID="grid_timesheet" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
    <Columns>
        <asp:TemplateField>
        <ItemTemplate>
        <%# Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="empid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
        <asp:BoundField DataField="idnumber" HeaderText="ID No."/>
        <asp:BoundField DataField="empname" HeaderText="Employee Name"/>
        <asp:BoundField DataField="Department" HeaderText="Department"/>
       <%-- <asp:BoundField DataField="daytype" HeaderText="Day Type"/>--%>
        <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
         <asp:BoundField DataField="date" HeaderText="Date"/>
        <asp:BoundField DataField="in1" HeaderText="Time In 1"/>
        <asp:BoundField DataField="out1" HeaderText="Time Out 1"/>
        <asp:BoundField DataField="in2" HeaderText="Time In 2"/>
        <asp:BoundField DataField="out2" HeaderText="Time Out 2"/>
     <%--   <asp:BoundField DataField="latehrs" HeaderText="Late Hrs."/>
        <asp:BoundField DataField="othrs" HeaderText="OT Hrs."/>--%>
    </Columns>
</asp:GridView>
    </ContentTemplate>
        </asp:UpdatePanel>
<asp:Label ID="lb_gender" runat="server" CssClass="none"></asp:Label>
<asp:Label ID="lb_hpd" runat="server" CssClass="none"></asp:Label>
<asp:Label ID="lb_payroll" runat="server" CssClass="none"></asp:Label>
</asp:Content>
 
