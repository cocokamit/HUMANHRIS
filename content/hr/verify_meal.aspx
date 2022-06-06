<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_meal.aspx.cs" Inherits="content_hr_verify_meal" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    .PopUpPanel {position:absolute;background-color: #fff; top:10%;left:50%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 {width:300px;left:60%;}
    .close {margin:-10px;float:right;}
    .Overlay2 {position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 {position:absolute;background-color: #fff; top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 {border-radius:4px; position: fixed; z-index: 1002; width: 600px; top: 28%;left: 50%; margin: -180px 0 0 -300px; background-color: #fff; }
    .input-form-span {margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure you want to cancel this application?"))
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
        <h3>Meal Allowance Verification</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x-head with-border">
                <asp:TextBox ID="txt_f" cssclass="datee" placeholder="From" runat="server"></asp:TextBox>
                <asp:TextBox ID="txt_t" cssclass="datee"  placeholder="To" runat="server"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server" OnClick="search" Text="Search" CssClass="btn btn-primary" />
                <div class="pull-right">
                    <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" ToolTip="Delete"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app"  CssClass="btn btn-default" ToolTip="Approve"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                    <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-30px; font-size:11px; "></asp:Label>
                </div>
            </div>
            <div id="alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                     </asp:TemplateField>
                    <asp:BoundField DataField="date" HeaderText="Date Filed" ItemStyle-Width="35px"/>
                    <asp:BoundField DataField="date_ot" HeaderText="Date Overtime" ItemStyle-Width="120px"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>  
                     <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_view" runat="server" OnClick="click_approved" ToolTip="Approve" Text="Verify" ><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                             <asp:LinkButton runat="server" ID="LinkButton1" runat="server" OnClick="click_delete" Text="Verify" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<div id="Div4" runat="server" visible="false" class="PopUpPanel nobel-modal">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li>Reason</li>
        <li><asp:TextBox ID="txt_reason" TextMode="MultiLine"  style=" resize:none; width:100%" runat="server"></asp:TextBox></li>
        <li><br></li>
        <li><asp:Button ID="Button3" runat="server" Text="Save" OnClick="click_can"  CssClass="btn btn-primary"/></li>
    </ul>
</div>

<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="view_id" runat="server" />
<asp:HiddenField ID="hdn_empid" runat="server" />
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

