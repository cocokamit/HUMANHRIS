<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_manual.aspx.cs" Inherits="content_Manager_approve_manual" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" ID="head_approve_ot" runat="server">

    <script type="text/javascript">
        function ConfirmRem() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to approve this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
        function CheckBoxCount() {
            var gv = document.getElementById("<%= grid_view.ClientID %>");
            var lbl_del_notify = document.getElementById("<%= lbl_del_notify.ClientID %>");
            var lbl_del_notify_val = document.getElementById('lbl_del_notify');
            var inputList = gv.getElementsByTagName("input");
            var numChecked = 0;

            for (var i = 0; i < inputList.length - 1; i++) {
                if (inputList[i].type == "checkbox" && inputList[i].checked) {
                    numChecked = numChecked + 1;
                }

            }
            lbl_del_notify.textContent = numChecked + " rows";
        }
  </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to cancel this Application?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
        function Confirmapp() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to Approved this Application?"))
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
<%--<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>--%>



</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_app_manual">
<section class="content-header">
    <h1>Adjustment Approval</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Adjustment Approval</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
        <div class="box box-primary">

            <div class="box-header">
                <div class="input-group pull-left">
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee form-control" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee form-control" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
                </div>
                <div id="div_action" runat="server" class="input-group pull-right">
                    <asp:LinkButton runat="server" OnClick="delete_all" ID="LinkButton1" CssClass="btn btn-default" style="margin-right:5px; float:left;"  ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" style="float:left;"  ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                </div>
            </div>

            <div class="box-body table-responsive no-pad-top">
                <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-25px; font-size:11px; "></asp:Label>
                <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField ItemStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="date" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="date_manual" HeaderText="Date Manual" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="time_in" HeaderText="Time In"/>
                        <asp:BoundField DataField="time_out" HeaderText="Time Out"/>
                        <asp:BoundField DataField="reason" HeaderText="Reason"/>
                            <asp:BoundField DataField="note" HeaderText="Remarks"/>
                        <asp:BoundField DataField="status" HeaderText="Status"/>  
                         <asp:BoundField DataField="nxt_id" HeaderText="" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>  
                          <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>  
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_app" runat="server" OnClick="delete2" Tooltip="approved" CssClass="border-right"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnk_can" runat="server" OnClick="view" Tooltip="declined"><i class="fa fa-thumbs-o-down"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
</section>

 
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
        <div class="box-body table-responsive no-pad-top">
        <div class="form-group">
            <label>Reason</label>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <asp:Button ID="btn_save" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" />
</div>



<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox>   
<asp:HiddenField ID="idd" runat="server" />
<asp:LinkButton ID="ib_can" runat="server" OnClick="view" CssClass="btn btn-danger" Text="cancel" style="margin-top:-2px; margin-left:5px; float:left; display:none; float:right;" ></asp:LinkButton>
        <asp:LinkButton ID="ib_del" runat="server" OnClick="delete2" CssClass="btn btn-primary" Text="approve" style="margin-top:-2px;  float:left; display:none; float:right;" ></asp:LinkButton>
  
</asp:Content>

