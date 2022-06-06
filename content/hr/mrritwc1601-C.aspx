<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mrritwc1601-C.aspx.cs" Inherits="content_hr_mrritwc1601_C" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .vp{margin:0 20px}
        .x_panel, .x_title {margin-bottom:0}
       .panel_toolbox {min-width:0}
       .dropdown-menu{min-width:10px}
       .vh{ visibility:hidden}
       .input-group-btn input[type=submit] {border:1px solid #efefef;border-left:none;  border-top-right-radius:40%; border-bottom-right-radius:40%}
       .table-bordered {margin-top:0}
    </style>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script src="style/js/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script type="text/javascript">
    var $jj = jQuery.noConflict();
    $jj(document).ready(function () {
        $jj("select").searchable({
            maxListSize: 10000, // if list size are less than maxListSize, show them all
            maxMultiMatch: 10000, // how many matching entries should be displayed
            exactMatch: false, // Exact matching on search
            wildcards: true, // Support for wildcard characters (*, ?)
            ignoreCase: true, // Ignore case sensitivity
            latency: 200, // how many millis to wait until starting search
            warnMultiMatch: 'top {0} matches ...',
            warnNoMatch: 'no matches ...',
            zIndex: 'auto'
        });
    });
    </script>
    <script type="text/javascript">

        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>

    <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>Annualization 1601C</h3>
        </div>
        <div class="title_right">
            <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="processannualization"  CssClass="btn btn-primary" />
        </div>
     </div>  
    <div class="clearfix"></div>
    <div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
            <table>
                <tr>
                    <td style=" border:none;  padding:5px;">
                        <asp:DropDownList ID="ddl_montly" runat="server"  Height="20px"></asp:DropDownList>
                    </td>
                    <td style=" border:none; padding:5px;">
                        <asp:DropDownList ID="ddl_year_search" runat="server"  Height="20px" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="search" Height="32px" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
            </div>
            <asp:GridView ID="grid_alpha_trn" runat="server" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="companyname" HeaderText="Company"/>
                    <asp:BoundField DataField="month" HeaderText="Month"/>
                    <asp:BoundField DataField="yyyy" HeaderText="Year"/>
                        <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                                <asp:LinkButton ID="pdf" runat="server" CausesValidation="false" OnClick="PDF" Text="PDF"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</div>
<div id="ttospo" visible="false" runat="server" class="Overlay"></div>
<div id="ttosppp" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
    <div class="row">
    <div class="col-md-3">
       <div class="x_panel">
       <ul class="input-form">
            <li>For The Month Of</li>
            <li><asp:DropDownList ID="ddlmonth" runat="server" Width="200px" Height="20px"></asp:DropDownList></li>
            <li>For The Year Of</li>
            <li><asp:DropDownList ID="ddlyear" runat="server" Width="200px" Height="20px"></asp:DropDownList></li>
            <li>Company</li>
            <li><asp:DropDownList ID="ddl_company" runat="server" Width="200px" Height="20px"></asp:DropDownList></li>
            <li><br /></li>
            <li><asp:Button ID="Button1" runat="server" Text="Process" OnClick="processsss" CssClass="btn btn-primary"/>
            <asp:Button ID="Button3" runat="server" Text="Save" OnClick="processSave" Visible="false" CssClass="btn btn-primary"/>
            </li>
            <li><asp:Label ID="lbl_error_msg" CssClass="text-danger" ForeColor="Red" runat="server"></asp:Label></li>
        </ul>     
        </div>
    </div>
    <div class="col-md-9"  >
         <div class="x_panel" style=" overflow:scroll;">
             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true"  CssClass="table table-striped table-bordered">
                <Columns>
                </Columns>
            </asp:GridView>
         </div>
    </div>
    </div>
</div>

<asp:HiddenField ID="TextBox1" runat="server" />
</asp:Content>