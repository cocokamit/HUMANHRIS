<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_leave.aspx.cs" Inherits="content_Manager_approve_leave" MasterPageFile="~/content/site.master" %>


<asp:Content ContentPlaceHolderID="head" ID="head_approve_leave" runat="server">

    <script type="text/javascript">
        function ConfirmRem() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to approve this application?"))
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
        function Confirm() 
        {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to cancel this application?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
        function Confirmapp() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to approve this Leave Application request?"))
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

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_approve_leave">
<asp:ScriptManager ID="myScriptManager" runat="server"></asp:ScriptManager>
<section class="content-header">
    <h1>Leave Approval</h1>
    <ol class="breadcrumb">
        <li><a href="dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
        <li class="active">Leave Approval</li>
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
                <div id="div_action" runat="server" class="input-group pull-right none">
                    <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" style="margin-right:5px; float:left;"  ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" style=" float:left;"  ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-25px; font-size:11px; "></asp:Label>
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                 </div>
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField ItemStyle-Width="40px" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" >
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sysdate" HeaderText="Filed"/>
                        <asp:BoundField DataField="leavefrom" HeaderText="From"/>
                        <asp:BoundField DataField="leaveto" HeaderText="To"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="delegate" HeaderText="Delegate" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                        <asp:BoundField DataField="nxt_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                     <asp:LinkButton runat="server" ID="lnk_info" OnClick="viewinfo" ToolTip="Abouts"  CssClass="border-right"> <i class="fa fa-info-circle"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnk_approve" OnClientClick="Confirmapp()"  OnClick="approve" ToolTip="Approved"  CssClass="border-right"> <i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                                     <asp:LinkButton runat="server" ID="lnk_view"  OnClientClick="Confirm()" OnClick="view" ToolTip="Cancel" CssClass="border-right"><i class="fa fa-thumbs-o-down"></i></asp:LinkButton>
                                      <asp:LinkButton runat="server" ID="LinkButton1" OnClick="view_img" ToolTip="Attachment" CssClass="border-right"><i class="fa fa-paperclip"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle  Width="150px"/>
                        </asp:TemplateField>
                        <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>


<div id="modal" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="lbl_lt" runat="server"></asp:Label></h4>
        </div>
        <div class="modal-body">
           <asp:GridView ID="grid_pay" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowboundgrid_pay" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="date" HeaderText="Date Leave" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="WithPay" HeaderText="With Pay"/>
                    <asp:BoundField DataField="nod" HeaderText="No. of Days"/>
                    <asp:BoundField DataField="designation" HeaderText="Designation"/>
                    <asp:BoundField DataField="status" HeaderText="Status"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Action"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  >
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="view" ToolTip="Cancel" CssClass="glyphicon glyphicon-remove-sign" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="10px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="l_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="request_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlApproverHistory" runat="server">
            <div class="modal-header">
            </div>
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="sysdate" HeaderText="Date Processed" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="approver" HeaderText="Approver" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_approver" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered ">
                <Columns>
                    <asp:BoundField DataField="date" HeaderText="Date Approved" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="approver" HeaderText="Approver"/>
                 <%--   <asp:BoundField DataField="remarks" HeaderText="Remarks"/>--%>
                    <asp:BoundField DataField="status"  HeaderText="Status"/>
                </Columns>
            </asp:GridView>
            </asp:Panel>
        </div>
    </div>
    </div>
</div>

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

<div id="Div3" runat="server" visible="false" class="Overlay"></div>
<div id="Div4" runat="server" visible="false" class="PopUpPanel">
      <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <div class="form-group">
        <label> <asp:Label ID="lbl_msg" ForeColor="Red" runat="server" Text="Label"></asp:Label></label>
        <asp:GridView ID="grid_img" runat="server" AutoGenerateColumns ="False"  EmptyDataText="No record found"  CssClass="table table-bordered table-hover dataTable" >
            <Columns>
                <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <img width="150px" src='files/leave/<%# Eval("awts") %>' alt="Alternate Text" style=" margin:5px" />
                    </ItemTemplate>
                </asp:TemplateField>       
            </Columns>
        </asp:GridView>
    </div>
</div>

<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" CssClass="hide" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="hfEmployee" runat="server" />
</asp:Content>

