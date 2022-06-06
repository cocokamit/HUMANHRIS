<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manuallog.aspx.cs" Inherits="content_hr_manuallog" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">



<style type="text/css">
    .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3{  width:300px;left:60%;}
    .close{ margin:-10px;float:right;}
    .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 { padding:10px;   border-radius:4px; position: absolute; z-index: 1002;  width: 600px; top: 28%;left: 50%; margin: -150px 0 0 -300px; background-color: #fff; }
    .input-form {}
    .input-form-span { margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>

<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>

<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>   
<script type="text/javascript" src="script/myJScript.js"></script>
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
    <style type="text/css">
        .radio { margin-left: 20px}
    </style>



<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datee").datepicker();
        });
    })(jQuery);
</script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
        <div class="title_left">
        <h3>Adjustment Verification</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee form-control" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
            <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee form-control" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
            <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
            <div id="alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="EmployeeId" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="sysdate" HeaderText="Filed"/>
                    <asp:BoundField DataField="idnumber" HeaderText="ID No."/>
                    <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="date_log" HeaderText="Date"/>
                    <asp:BoundField DataField="time_in" HeaderText="Time In 1" HeaderStyle-Width="90px"/>
                    <asp:BoundField DataField="time_out1" HeaderText="Time Out 1" HeaderStyle-Width="90px"/>
                    <asp:BoundField DataField="time_in1" HeaderText="Time In 2" HeaderStyle-Width="90px"/>
                    <asp:BoundField DataField="time_out" HeaderText="Time Out 2" HeaderStyle-Width="90px"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:BoundField DataField="note" HeaderText="Remarks" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_view" runat="server" OnClick="view" Text="Verify" ><i class="fa fa-sliders"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="Div1" runat="server" visible="false" class="Overlay2"></div>
<div id="Div2" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="lbl_name" runat="server" ></asp:Label></h4>
    </div>
    <div class="modal-body">
    <ul class="input-form">
        <li style=" font-weight:bold">Date Manual</li>
        <li class="input-form-span"><asp:Label ID="lbl_date" runat="server" ></asp:Label></li>
        <li>
            <div class="row">
                <div class="col-md-6">
                     <ul>
                        <li class="text-bold">Time In 1</li>
                        <li class="input-form-span"><asp:Label ID="lbl_in" runat="server" ></asp:Label></li>
                    </ul>
                </div>
                <div class="col-md-6">
                     <ul>
                        <li class="text-bold">Time Out 1</li>
                        <li class="input-form-span"><asp:Label ID="lbl_out1" runat="server" ></asp:Label></li>
                    </ul>
                </div>
            </div>
        </li>
        <li>
            <div class="row">
                <div class="col-md-6">
                     <ul>
                        <li class="text-bold">Time In 2</li>
                        <li class="input-form-span"><asp:Label ID="lbl_in1" runat="server" ></asp:Label></li>
                    </ul>
                </div>
                <div class="col-md-6">
                     <ul>
                        <li class="text-bold">Time Out 2</li>
                        <li class="input-form-span"><asp:Label ID="lbl_out" runat="server" ></asp:Label></li>
                    </ul>
                </div>
            </div>
        </li>
        <li style=" font-weight:bold">Reason</li>
        <li class="input-form-span"><asp:Label ID="lbl_reason" runat="server"></asp:Label></li>
        <li style=" font-weight:bold">Remarks</li>
        <li class="input-form-span"><asp:Label ID="lbl_note" runat="server" Text="Label"></asp:Label></li>
        <li><asp:Label ID="lbl_err" CssClass="text-danger text-uppercase" runat="server" Text="Label"></asp:Label></li>
    </ul>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="Biotime" HeaderText="Time record"/>
        </Columns>
    </asp:GridView>
    <ul class="input-form">
        <li class="text-bold">Note</li>
        <li><asp:TextBox ID="txt_reason" TextMode="MultiLine" runat="server"></asp:TextBox></li>
    </ul>
    <div class="clearfix"></div>
    </div>
    <div class="modal-footer">
        <asp:Button ID="btn_app" runat="server" OnClick="Approve" Text="Approve" CssClass="btn btn-primary pull-left" />
        <asp:Button ID="btn_can" runat="server" OnClick="Cancel" Text="Cancel" CssClass="btn btn-danger" />
    </div>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="empid" runat="server" />
<asp:HiddenField ID="adjid" runat="server" />
</asp:Content>
