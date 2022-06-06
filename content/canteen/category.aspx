<%@ Page Language="C#" AutoEventWireup="true" CodeFile="category.aspx.cs" Inherits="content_canteen_category" MasterPageFile="~/content/MasterPageNew.master"%>
 
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
 

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

<!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Schedule Verification</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head with-border">
                <asp:DropDownList ID="ddlDepartment" runat="server" OnSelectedIndexChanged="btn_search" AutoPostBack="true" CssClass="select2"></asp:DropDownList>
                <asp:DropDownList ID="ddl_period" runat="server" OnSelectedIndexChanged="btn_search" AutoPostBack="true" CssClass="minimal"></asp:DropDownList>
            </div>
             <div class="x_content">
                <div id="grid_alert" runat="server" class="alert alert-empty no-margin">
                    <i class="fa fa-info-circle"></i>
                    <span>No Record Found</span>
                </div>
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin ">
                    <Columns>
                        <asp:BoundField DataField="dtr_id" HeaderText="dtr_id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="status" HeaderText="status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                        <asp:BoundField DataField="date_start" HeaderText="date_start" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:BoundField DataField="date_end" HeaderText="date_end" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:BoundField DataField="emp_id"  HeaderText="emp_id" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:BoundField DataField="id"  HeaderText="TChangeShiftID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"/>
                        <asp:BoundField DataField="employee"  HeaderText="Scheduler" />
                        <asp:BoundField DataField="approverName"  HeaderText="Approver" />
                        <asp:TemplateField HeaderText="Period">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton2" OnClick="click_schedule" Text='<%# Eval("period") %>' CssClass="no-border no-padding" Font-Size="Small" runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" OnClick="back" ToolTip="Back to Supervisor Tab" CssClass="glyphicon glyphicon-remove-sign text-danger pull-right" runat="server" style="margin:5px 0 0 0px; font-size:16px "></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" Text='<%# Eval("status") %>' CssClass="label label-primary pull-right" style="padding:5px; font-size:10px" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2();
    })(jQuery);
</script>

</asp:Content>
