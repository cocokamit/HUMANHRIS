<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_schedule.aspx.cs" Inherits="content_Manager_approve_schedule" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
    </style>

<script type="text/javascript">


    bkLib.onDomLoaded(function () { nicEditors.allTextAreas() });


    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this schedule?"))
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
            $(".datee").datepicker({ changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true
            });
        });
    })(jQuery);
    </script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>Schedule Approval</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Schedule Approval</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
        <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm">
                     
                </div>
            </div>
            <div class="box-body">
                <div id="grid_alert" runat="server" class="alert alert-empty no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No Record Found</span>
                </div>
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="dtr_id" HeaderText="dtr_id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" /> 
                        <asp:BoundField DataField="nxt_id" HeaderText="nxt_id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton2" OnClick="click_schedule" Text='<%# Eval("period") %>' CssClass="no-border no-padding" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton1" OnClick="back" Visible="false" ToolTip="Back to Supervisor Tab" CssClass="pull-right" style="padding:2px; font-size:17px" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
</section>
 
<div class="row hidden">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">

           <div class="x-head with-border">
                <asp:TextBox ID="txt_f" cssclass="datee" placeholder="From" runat="server"></asp:TextBox>
                <asp:TextBox ID="txt_t" cssclass="datee"  placeholder="To" runat="server"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server" OnClick="btn_search" Text="Search" CssClass="btn btn-primary" />
            </div>
             <div class="x_content">
                

            </div>
        </div>
       
    </div>
</div>

<div id="modal_delete" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close_delete" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Declined Schedule</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                <label>Reason</label>
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="box-footer pad">
            <asp:Button ID="btn_save" runat="server" OnClick="back_save" Text="Save" CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>

<asp:TextBox ID="TextBox1" CssClass=" hidden" runat="server"></asp:TextBox>
<asp:HiddenField ID="hf_did" runat="server" />
<asp:HiddenField ID="hf_start" runat="server" />
<asp:HiddenField ID="hf_end" runat="server" />
</asp:Content>



