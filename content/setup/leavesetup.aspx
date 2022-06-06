<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leavesetup.aspx.cs" Inherits="content_setup_leavesetup" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3{  width:300px;left:60%;}
    .close{ margin:-10px;float:right;}
    .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 {   border-radius:4px; position: fixed; z-index: 1002;  width: 600px; top: 28%;left: 50%; margin: -180px 0 0 -300px; background-color: #fff; }
    .input-form-span { margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<script src="script/auto/myJScript.js" type="text/javascript"></script>
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
        <h3>Leave Set Up</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x-head">
                 <asp:LinkButton ID="Button2" runat="server" OnClick="add"   CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="LeaveType" HeaderText="Leave Type"/>
                    <asp:BoundField DataField="Leave" HeaderText="Leave Description"/>
                    <asp:BoundField DataField="yearlytotal" HeaderText="Total Credits"/>
                    <asp:BoundField DataField="converttocash" HeaderText="Convertable to Cash"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="Div1" runat="server" visible="false" class="Overlay2"></div>
<div id="pAdd" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
    <ul class="input-form">
        <li>Leave Type</li>
        <li>
            <asp:TextBox ID="txt_lt" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_lt" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Description</li>
        <li>
            <asp:TextBox ID="txt_desc" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_desc" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Yearly Credits</li>
        <li>
            <asp:TextBox ID="txt_yt" AutoComplete="off" ClientIDMode="Static" onkeyup="intinput(this);"  runat="server"></asp:TextBox>
            <asp:Label ID="lbl_yt" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li><asp:CheckBox ID="chk_convertable" runat="server" Text="Convertable to Cash!" /></li>
        <li><hr /></li>
        <li><asp:Button ID="btn_add" runat="server" OnClick="savedata"  Text="ADD" CssClass="btn btn-primary"/></li>
    </ul>
</div>
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="view_id" runat="server" />
<script src="script/jtimepicker/timepicki.js" type="text/javascript"></script>
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $('.nobel').timepicki();
        });
    })(jQuery);
</script>
<script src="script/jtimepicker/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
