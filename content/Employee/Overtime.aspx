<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Overtime.aspx.cs" Inherits="content_Employee_Overtime" MasterPageFile="~/content/site.master"%>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
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
                   yearRange: "-100:+1",
                   minDate: 0,
                   changeYear: true
               });
           });
       })(jQuery);
    </script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
    <style type="text/css">
    @media only screen and (max-width: 1200px) {
		input[type=time] {
				margin-top:13px !important
		}
    }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_leave" runat="server">

<section class="content-header">
    <h1>Pre-Overtime Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Pre-Overtime Application</li>
    </ol>
</section>
<section class="content">
  <div class="box box-primary">
   <div class="box-body">
        <ul class="input-form">
            <div class="form-group">
                <label>Date</label>
                <asp:TextBox ID="txt_date" CssClass="datee form-control" AutoComplete="off" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_date" style=" color:Red;" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <label>Overtime Start</label>
                <asp:TextBox ID="txt_start" CssClass="nobel form-control" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_start" style=" color:Red;" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <label>Overtime End</label>
                <asp:TextBox ID="txt_end" CssClass="nobel form-control" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_end" style=" color:Red;" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <label>Reason</label>
                <asp:TextBox ID="txt_remarks" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_remarks" style=" color:Red;" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group">
                <asp:Button ID="btn_add" runat="server" OnClick="addOT" Text="ADD" CssClass="btn btn-primary"/>
            </div>
        </ul>
    </div>
  </div>
</section>

<script src="script/jtimepicker/bootstrap.min.js" type="text/javascript"></script>
<script src="script/jtimepicker/timepicki.js" type="text/javascript"></script>
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $('.nobel').timepicki();
        });
    })(jQuery);
</script>



</asp:Content>
