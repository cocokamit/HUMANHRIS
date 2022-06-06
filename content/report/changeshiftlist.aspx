<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changeshiftlist.aspx.cs" Inherits="content_report_changeshiftlist" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        hr{margin:10px 0}
        .vp{margin:0 20px}
        .x_panel, .x_title {margin-bottom:0}
       .panel_toolbox {min-width:0}
       .dropdown-menu{min-width:10px}
       .vh{ visibility:hidden}
       .input-group-btn input[type=submit] { border:none;border-left:none; border-top-right-radius:40%; border-bottom-right-radius:40%}
       .hiddencol { display: none; }
    </style>
        <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
     <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
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
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Change Shift History</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"></asp:DropDownList>
                <asp:TextBox ID="txt_search" runat="server"  Placeholder="Search" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_from" runat="server" CssClass="datee" Placeholder="FROM" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_to" CssClass="datee" runat="server" ClientIDMode="Static" AutoComplete="off" Placeholder="TO"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server" OnClick="click_search"  Text="Search" CssClass="btn btn-primary"/> 
            </div>
 <div id="div_list" runat="server">
                                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                                <asp:GridView ID="grid_display" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="IdNumber" HeaderText="ID Number"/>
                                        <asp:BoundField DataField="ename" HeaderText="Employee Name"/>
                                        <asp:BoundField DataField="PayrollGroup" HeaderText="PayrollGroup" />
                                        <asp:BoundField DataField="changedate" HeaderText="Date" />
                                        <asp:BoundField DataField="ShiftCode" HeaderText="ShiftCode" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                      
                                     
                                    </Columns>
                                </asp:GridView>
                            </div>
                            </div>
    </div>
</div>

<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>
