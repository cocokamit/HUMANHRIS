<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print_leadger.aspx.cs" Inherits="content_report_print_leadger" MasterPageFile="~/content/report/Report.master" %>

<asp:Content ID="sales_daily_head" runat="server" ContentPlaceHolderID="head">

<script src="Script/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="Script/jquery-ui.js" type="text/javascript"></script>
<link href="Script/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">    $(function () { $("#txt_Fdate").datepicker(); });</script>
<script type="text/javascript">    $(function () { $("#TextBox1").datepicker(); });</script>
<style>
    *{font-family: 'Source Sans Pro', sans-serif;}
    input[type=text]{ border:none; }
    .with-border{border-bottom:1px solid #f4f4f4}
    .marlon li small{ font-weight:bold; }
    .marlon li { text-transform:uppercase}
</style>
</asp:Content>

<asp:Content ID="sales_daily_content" runat="server" ContentPlaceHolderID="content">
    <div class="row">  
        <div class="col-lg-12">
            <div class="header with-border">
               <h1>LOAN BALANCE</h1>
            </div>
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style=" width:150px">ID Number</td>
                    <td style=" width:30px">:</td>
                    <td><asp:Label ID="lbl_id" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>:</td>
                    <td><asp:Label ID="lbl_name" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Loan Number</td>
                    <td>:</td>
                    <td><asp:Label ID="lbl_no" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Loan Type</td>
                    <td>:</td>
                    <td><asp:Label ID="lbl_type" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Loan Balances</td>
                    <td>:</td>
                    <td><asp:Label ID="lbl_bal" runat="server" Text="Label"></asp:Label></td>
                </tr>
            </table>
        </div>
    </div>
    
    <br />
<asp:GridView ID="grid_released" runat="server" AutoGenerateColumns="False" style=" width:100%" CssClass="table table-striped table-bordered">
    <Columns>
        
        <asp:BoundField DataField="DateStart" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />       
        <asp:BoundField DataField="LoanAmount" HeaderText="Debit" DataFormatString="{0:N4}"/>
        <asp:BoundField DataField="credit" HeaderText="Credit"  DataFormatString="{0:N4}"/>
        <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:N4}"/>

    </Columns>
</asp:GridView>

 



<asp:Label ID="lbl_err" runat="server" Text=""></asp:Label>

 


<asp:HiddenField ID="hf_id" runat="server" />

<!-- jQuery 3 -->
    <script src="vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- SlimScroll -->
    <script src="vendors/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="vendors/fastclick/lib/fastclick.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <script src="dist/js/active.js"></script>

</asp:Content>