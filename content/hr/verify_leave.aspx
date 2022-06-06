<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_leave.aspx.cs" Inherits="content_hr_verify_leave" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" ID="head_approve_leave" runat="server">

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
    <style type="text/css">
        .radio { margin-left: 20px}
        .aspNetDisabled { color:#CCC}
    </style>


</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_approve_leave">
<div class="page-title">
    <div class="title_left">
        <h3>Leave Verification</h3>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x-head hide">
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee" autocomplete="off" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee" autocomplete="off" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
                
                    <div class="pull-right none">
                        <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                        <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                        <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-60px; font-size:11px; "></asp:Label>
                    </div>
                </div>

                <div id="alert" runat="server" class="alert alert-empty" visible="false">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <asp:GridView ID="grid_view" runat="server" OnRowDataBound="leavemonitoring" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-input no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="empID" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField ItemStyle-Width="40px" Visible="false">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sysdate" HeaderText="Date Filed"/>
                        <asp:BoundField DataField="leavefrom" HeaderText="Leave From"/>
                        <asp:BoundField DataField="leaveto" HeaderText="Leave To"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave Type"/>
                        <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                        <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk_approve" OnClientClick="Confirmapp()"  OnClick="approve" ToolTip="Approved" CssClass="glyphicon glyphicon-ok-sign disabled"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnk_view"  OnClientClick="Confirm()" OnClick="view" ToolTip="Cancel"><i class="glyphicon glyphicon-remove-sign"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="LinkButton1" OnClick="view_img" ToolTip="Attachment"><i class="glyphicon glyphicon-paperclip"></i></asp:LinkButton>
                                    <asp:LinkButton ID="linkviewleave" OnClick="Viewleavestatus" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="150px"/>
                        </asp:TemplateField>
                         <asp:TemplateField Visible="false" HeaderStyle-CssClass="dnone" ItemStyle-CssClass="dnone">
                            <ItemTemplate>
                                <asp:LinkButton ID="itemstat" runat="server" Text='<%# bind("status")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div id="modal" runat="server" class="modal fade in" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Leave Details(<asp:Label ID="lbl_lt" runat="server" style=" font-weight:bolder; font-size:medium; color:Green;"></asp:Label>)</h4>
        </div>
        <div class="modal-body">
           <asp:GridView ID="grid_pay" Visible="false" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowboundgrid_pay" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="date" HeaderText="Date Leave" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="WithPay" HeaderText="With Pay"/>
                    <asp:BoundField DataField="nod" HeaderText="No. of Days"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="view" ToolTip="Cancel" CssClass="glyphicon glyphicon-remove-sign" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="10px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="l_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="request_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
            <div class="modal-header">
                <h4 class="modal-title">Approver History</h4>
            </div>
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="sysdate" HeaderText="Date Processed" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="approver" HeaderText="Approver" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_approver" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered ">
                <Columns>
                    <asp:BoundField DataField="date" HeaderText="Date Approve" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="approver" HeaderText="Approver Name"/>
                 <%--   <asp:BoundField DataField="remarks" HeaderText="Remarks"/>--%>
                    <asp:BoundField DataField="status"  HeaderText="Status"/>
                </Columns>
            </asp:GridView>
 
        </div>
    </div>
    </div>
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
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" CssClass="hide" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
<asp:DropDownList ID="drop_type" Visible="false" runat="server"> </asp:DropDownList>
</asp:Content>

