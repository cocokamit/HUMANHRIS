<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_schedule.aspx.cs" Inherits="content_hr_verify_schedule" MasterPageFile="~/content/MasterPageNew.master" %>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
    <style type="text/css">
        .select { font-size:10px}
        .table-responsive::-webkit-scrollbar {width: 1em;}
        .table-responsive::-webkit-scrollbar-track {-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);}
        .table-responsive::-webkit-scrollbar-thumb {background-color: darkgrey;  outline: 1px solid slategrey;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr_period">
<section class="content-header">
    <h1>Verification Scheduler</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Verification Scheduler</li>
    </ol>
</section>
<section class="content">

<asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
<Columns>
    <asp:BoundField DataField="id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"  />
    <asp:BoundField DataField="cnt" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
    <asp:BoundField DataField="date_input"  ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
    <asp:BoundField DataField="period" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
    <asp:BoundField DataField="status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
    <asp:TemplateField HeaderText="Period">
        <ItemTemplate>
            <asp:Label ID="lnk_can" Text='<%# Eval("period") %>' runat="server"></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="">
        <ItemTemplate>
            <asp:LinkButton ID="LinkButton1" Text="view" OnClick="click_submit" CssClass="label label-danger" style=" padding:5px" runat="server"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>

</section>

 
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
<asp:HiddenField ID="hf_shiftline" runat="server" />
<asp:HiddenField ID="hf_status" runat="server" />
    
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script type="text/javascript">
   

    
</script>
</asp:Content>