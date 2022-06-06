<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rest_day.aspx.cs" Inherits="content_Employee_rest_day" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_ot_list">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript">
       jQuery.noConflict();
       (function($) {
           $(function() {
               $(".datee").datepicker({ changeMonth: true,
                   yearRange: "-100:+0",
                   changeYear: true
               });
           });
       })(jQuery);
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_rd" runat="server">


<section class="content-header">
    <h1>Work Verification Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Work Verification Application</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            
            <div class="box-body table-responsive no-pad-top">
          
            <div class="form-group">
                <label>Date</label>
                <asp:Label ID="lbl_date" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_otd" CssClass="datee form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_lineremarks" TextMode="MultiLine" CssClass="nobel form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>

            </div>
            <div class="box-footer pad">
                <asp:Button ID="btn_save" runat="server" OnClick="click_save_ot" Text="Save" CssClass="btn btn-primary"/>
            </div>
          </div>
        </div>
    </div>
</section>
</asp:Content>

