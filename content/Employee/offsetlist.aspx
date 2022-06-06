<%@ Page Language="C#" AutoEventWireup="true" CodeFile="offsetlist.aspx.cs" Inherits="content_Employee_offsetlist" MasterPageFile="~/content/site.master" %>

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
                    yearRange: "-100:+0",
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_ot_list" runat="server">
<section class="content-header">
    <h1>Offset List</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Offset List</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                 <div id="alert" runat="server" style=" display:none;" class="alert alert-default alert-dismissible">
                    <h5><i class="icon fa fa-check-circle"></i> No record found</h5>
                 </div>
             <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                    <asp:BoundField DataField="Date_Input" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="status" HeaderText="Type" />
                    <asp:BoundField DataField="DateFrom" HeaderText="Date From" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="DateTo" HeaderText="Date To" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:BoundField DataField="transtatus" HeaderText="Status"/>
                </Columns>
            </asp:GridView> 
            </div>
          </div>
        </div>
      </div>
</section>
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="pAdd" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png"  runat="server"/>
    <ul class="input-form">
        <li>Date :</li>
        <li>
            <asp:TextBox ID="txt_date" CssClass="datee" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_date" style=" color:Red;" runat="server" Text=""></asp:Label>
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
        <li><asp:Button ID="btn_add" runat="server"  Text="ADD" CssClass="btn btn-primary"/></li>
    </ul>
</div>

<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" runat="server"/>

       <div class="box-body table-responsive no-pad-top">
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    <asp:Button ID="btn_save" runat="server"  Text="Save" CssClass="btn btn-primary"/>
</div>
    <asp:HiddenField ID="id" runat="server" />
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

