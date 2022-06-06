<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addot.aspx.cs" Inherits="content_Employee_addot" MasterPageFile="~/content/site.master" %>

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
    <li class="active">Pre Overtime Application</li>
    </ol>
</section>
<section class="content">
   <div class="row">
        <div class="col-xs-12">
  <div class="box box-primary">
   <div class="box-body">
   <div class="box-header with-border">
        <div class="input-group input-group-sm" style="width: 300px;">
            <asp:Button ID="Button1" runat="server" OnClick="addpreot" Text="ADD" CssClass="btn btn-primary" />
        </div>
        </div>
        <div class="box-body table-responsive no-pad-top">
            <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Sysdate" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="OTDate" HeaderText="Pre-Overtime Date" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="OTStart" HeaderText="Overtime Start" />
                    <asp:BoundField DataField="OTEnd" HeaderText="Overtime End" />
                    <asp:BoundField DataField="OvertimeHours" HeaderText="Number of Hours" />
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:BoundField DataField="Status" HeaderText="Status"/>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="cancelApplication" Tooltip="cancel" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div id="div_msg" runat="server" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
        </div>
    </div>
  </div>
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
