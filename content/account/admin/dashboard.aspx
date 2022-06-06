<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="content_account_admin_dashboard" MasterPageFile="~/content/MasterPageNew.master" %>

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
</asp:Content>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left">
        <h3>Dashboard</h3>
    </div>  
    <div class="title_right">
       <ul>
        <li><a href="MEmployee"><i class="fa fa fa-dashboard"></i> Home</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Dashboard</li>
       </ul>
    </div>       
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
        <div class="tile-stats bg-aquaa">
            <div class="icon"><i class="fa fa-user-plus"></i></div>
            <div class="count"><asp:Label ID="lb_hc" runat="server"></asp:Label></div>
            <p>HEAD COUNTS</p>
        </div>
    </div>
    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
        <div class="tile-stats bg-greene">
            <div class="icon"><i class="fa fa-check-circle-o"></i></div>
            <div class="count"><asp:Label ID="lb_ra" runat="server"></asp:Label></div>
            <p>REQUEST APPROVAL</p>
        </div>
    </div>
    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
        <div class="tile-stats bg-yellow">
            <div class="icon"><i class="fa fa-suitcase"></i></div>
            <div class="count"><asp:Label ID="lb_po" runat="server"></asp:Label></div>
            <p>OPEN POSITION</p>
        </div>
    </div>

    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
        <div class="tile-stats bg-redd">
            <div class="icon"><i class="fa fa-sort-amount-desc"></i></div>
            
            <i><small><asp:LinkButton ID="lb_upcoming" runat="server" ForeColor="white" ></asp:LinkButton></small></i>
            <br />
            <strong><asp:Label ID="l_holname" runat="server" style="white-space: nowrap; width: 150px;overflow: hidden;text-overflow: ellipsis; " CssClass=""></asp:Label></strong>
            <br />
            <asp:Label ID="l_holiday" runat="server" CssClass=""></asp:Label>
            <p><a href="Mdaytype" style=" color:#fff">HOLIDAY</a></p>
        </div>
    </div>

    <%--<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
        <div class="tile-stats bg-redd">
            <div class="icon"><i class="fa fa-sort-amount-desc"></i></div>
            <asp:Label ID="l_holiday" runat="server" CssClass="count"></asp:Label>
            <p><a href="Mdaytype" style=" color:#fff">HOLIDAY</a></p>
            <asp:LinkButton ID="lb_upcoming" runat="server" Text="Upcoming" ></asp:LinkButton>
        </div>
    </div>--%>
</div>

<div class="row">
    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
        <div class="tile-stats bg-orange">
            <div class="icon"><i class="fa fa-exclamation-triangle"></i></div>
            <div class="count"><asp:Label ID="lbl_tlate" runat="server" Text="0"></asp:Label></div>
            <p>TOTAL LATE</p>
        </div>
    </div>
    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
        <div class="tile-stats bg-redd">
            <div class="icon"><i class="fa fa-times-circle-o"></i></div>
            <div class="count"><asp:Label ID="lbl_tabsent" runat="server" Text="0"></asp:Label></div>
            <p>TOTAL ABSENT</p>
        </div>
    </div>
    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
        <div class="tile-stats bg-greene">
            <div class="icon"><i class="fa fa-check-square-o"></i></div>
            <div class="count"><asp:Label ID="lbl_punchheadcount" runat="server" Text="0"></asp:Label></div>
            <p>ON DUTY</p>
        </div>
    </div>
</div>

<div class="row">
     <div class="col-md-4">
        <div class="x_panel">
            <div class="x_title">
            <h2>Gender</h2>
            <div class="clearfix"></div>
            </div>
            <div class="x_content">
                 <div id="gender"></div>
            </div>
        </div>
    </div>
    <div class="col-md-8">
        <div class="x_panel">
            <div class="x_title">
            <h2>Headcount per department</h2>
            <div class="clearfix"></div>
            </div>
            <div class="x_content">
                <div id="hpd"></div>
            </div>
        </div>
    </div>
</div> 

<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>Total Salary</h2>
                <asp:DropDownList ID="dl_year" runat="server" OnTextChanged="onchangeyear" AutoPostBack="true" style=" float:right;">
                </asp:DropDownList>
                <div class="clearfix"></div>
            </div>
            <div class="x_content2">
                <asp:Label ID="lb_tamount" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px">Php 8,005,00.00</asp:Label>
                <div id="graph"></div>
            </div>
        </div>
    </div>
</div>

<pre id="code" class="prettyprint linenums  hidden">
Morris.Bar({
  element: 'hpd',
  data: [<% Response.Write(lb_hpd.Text); %>],
  xkey: 'y',
  ykeys: ['a'],
  labels: ['Headcount'],
  horizontal: true,
  stacked: true
});

Morris.Bar({
  element: 'graph',
   xLabelMargin: 15,
   xLabelAngle: 70,
  data:  [<% Response.Write(lb_payroll.Text); %>],
  xkey: 'x',
  ykeys: ['Php'],
  labels: ['Php']
}).on('click', function(i, row){
  console.log(i, row);
});

Morris.Donut({
  element: 'gender',
  data: [<% Response.Write(lb_gender.Text); %>],
    colors: [
    '#3c8dbc',
    '#f56954 '
  ],
  formatter: function (x) { return x + ""}
}).on('click', function(i, row){
  console.log(i, row);
});
</pre> 

<div class="row" style="display:none;">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x_title">
                <h2>Time Sheet Monitoring</h2>
                <div class="clearfix"></div>
            </div>
            <div class="x_content2">
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
        <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
         <asp:BoundField DataField="date" HeaderText="Date"/>
        <asp:BoundField DataField="in1" HeaderText="Time In 1"/>
        <asp:BoundField DataField="out1" HeaderText="Time Out 1"/>
        <asp:BoundField DataField="in2" HeaderText="Time In 2"/>
        <asp:BoundField DataField="out2" HeaderText="Time Out 2"/>
        </Columns>
        </asp:GridView>
        </div>
        </div>
    </div>
</div>
<asp:Label ID="lb_gender" runat="server" CssClass="none"></asp:Label>
<asp:Label ID="lb_hpd" runat="server" CssClass="none"></asp:Label>
<asp:Label ID="lb_payroll" runat="server" CssClass="none"></asp:Label>
</asp:Content>
 