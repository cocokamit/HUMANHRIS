<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ot_list.aspx.cs" Inherits="content_Employee_ot_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>   
    <script type="text/javascript" src="script/myJScript.js"></script>
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
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_ot_list" runat="server">
<section class="content-header">
    <h1>Overtime</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Overtime</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <div class="input-group input-group-sm" style="width: 300px;">
                    <asp:Button ID="btnotadd" Visible="false" OnClick="addpreot" runat="server" Text="ADD" CssClass="btn btn-primary" />
                </div><br />
                 <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                 </div>
               <asp:GridView ID="grid_view" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" runat="server" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                    <asp:BoundField DataField="sysdate" HeaderText="File Date" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="date" HeaderText="OT Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="time_in" HeaderText="Overtime Start"/>
                    <asp:BoundField DataField="time_out" HeaderText="Overtime End"/>
                    <asp:BoundField DataField="OvertimeHours" HeaderText="OT Hours"/>
                    <asp:BoundField DataField="OvertimeNightHours" HeaderText="OTN Hours"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Reason"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>  
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="view" runat="server" CssClass="fa fa-trash-o"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>  
            </div>
          </div>
          <div class="box-header with-border">
               <div class="box-tools radio-box pull-right none">
                    <asp:RadioButtonList ID="rbl_class" CssClass="radio" runat="server">
                        <asp:ListItem Selected>Personal Undertime</asp:ListItem>
                        <asp:ListItem>Authorized Undertime</asp:ListItem>
                    </asp:RadioButtonList>
                </div> 
            </div>
        </div>
    </div>
</section>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="pAdd" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
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
        <li><asp:Button ID="btn_add" runat="server" OnClick="addOT" Text="ADD" CssClass="btn btn-primary"/></li>
    </ul>
</div>

<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    </ul>
       <div class="box-body table-responsive no-pad-top">
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    <asp:Button ID="btn_save" runat="server" OnClick="cancel" Text="Save" CssClass="btn btn-primary"/>
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

