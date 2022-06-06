<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verifyovertime.aspx.cs" Inherits="content_hr_verifyovertime" MasterPageFile="~/content/MasterPageNew.master" %>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
    .PopUpPanel { position:absolute;background-color: #fff;   top:10%;left:50%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
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
        <h3>Overtime Verification</h3>
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
                <asp:LinkButton ID="Button2" runat="server" OnClick="click_add" CssClass="right add none"><i class="fa fa-plus-circle"></i></asp:LinkButton>
            </div>
            <div id="alert" runat="server" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date" HeaderText="Date Filed" />
                    <asp:BoundField DataField="date_ot" HeaderText="Date Overtime"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="time_in" HeaderText="Time In"/>
                    <asp:BoundField DataField="time_out" HeaderText="Time Out"/>
                    <asp:BoundField DataField="OvertimeHours" HeaderText="OT Regular Hours"/>
                    <asp:BoundField DataField="OvertimeNightHours" HeaderText="OT Night Hours"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>  
                     <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_view" runat="server" OnClick="view" Text="Verify" ><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                             <asp:LinkButton runat="server" ID="LinkButton1" runat="server" OnClick="click_delete" Text="Verify" ><i class="fa fa-thumbs-o-down"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="73"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div id="Div1" runat="server" visible="false" class="Overlay2"></div>
<div id="pAdd" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li>Date :</li>
        <li>
            <asp:TextBox ID="txt_date" CssClass="datee" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="Label1" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Start OT :</li>
        <li>
            <asp:TextBox ID="txt_start" CssClass="nobel" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_start" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>End OT :</li>
        <li>
            <asp:TextBox ID="txt_end" CssClass="nobel" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_end" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Reason</li>
        <li>
            <asp:TextBox ID="txt_remarks" TextMode="MultiLine" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_remarks" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li><hr /></li>
        <li><asp:Button ID="btn_add" runat="server" OnClick="addOT" Text="ADD" CssClass="btn btn-primary"/></li>
    </ul>
</div>
<div id="Div4" runat="server" visible="false" class="PopUpPanel nobel-modal">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li style=" font-weight:bold">Employee Name</li>
        <li class="input-form-span"><asp:Label ID="lbl_name1" runat="server"></asp:Label></li>
        <li style=" font-weight:bold">Date Overtime</li>
        <li class="input-form-span"><asp:Label ID="lbl_date1" runat="server" ></asp:Label></li>
        <li style=" font-weight:bold">OT Time In</li>
        <li class="input-form-span"><asp:Label ID="lbl_oti" runat="server" ></asp:Label></li>
        <li style=" font-weight:bold">OT Time Out</li>
        <li class="input-form-span"><asp:Label ID="lbl_oto" runat="server" ></asp:Label></li>
    </ul>
    <ul class="input-form">
        <li>Reason</li>
        <li>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine"  style=" resize:none; width:100%" runat="server"></asp:TextBox>
        </li>
        <li><br></li>
        <li><asp:Button ID="Button3" runat="server" Text="Save" OnClick="click_can"  CssClass="btn btn-primary"/></li>
    </ul>
</div>
<div id="Div2" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="lbl_name" runat="server"></asp:Label></h4>
    </div>
    <div class="modal-body">
        <ul class="input-form">
            <li style=" font-weight:bold">Date Overtime</li>
            <li class="input-form-span"><asp:Label ID="lbl_date" runat="server" ></asp:Label></li>
            <li style=" font-weight:bold">OT Time In</li>
            <li class="input-form-span"><asp:Label ID="lbl_in" runat="server" ></asp:Label></li>
            <li style=" font-weight:bold">OT Time Out</li>
            <li class="input-form-span"><asp:Label ID="lbl_out" runat="server" ></asp:Label></li>
            <li style=" font-weight:bold">OT Reg Hours Approved<asp:Label ID="lbl_erorha" runat="server" ></asp:Label></li>
            <li><asp:textbox ID="lbl_orha" onkeyup="decimalinput(this)" runat="server" ></asp:textbox></li>
            <li style=" font-weight:bold">OT Night Hours Approved<asp:Label ID="lbl_eronha" runat="server" ></asp:Label></li>
            <li><asp:textbox ID="lbl_onha" runat="server" onkeyup="decimalinput(this)" ></asp:textbox></li>
            <li style=" font-weight:bold">Reason</li>
            <li class="input-form-span"><asp:Label ID="lbl_reason" runat="server" ></asp:Label></li>
        </ul>
    </div>
    <div class="modal-footer">
         <asp:Button ID="btn_app" runat="server" OnClick="click_approved"  Text="Approve" CssClass="btn btn-primary" />
    </div>
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
