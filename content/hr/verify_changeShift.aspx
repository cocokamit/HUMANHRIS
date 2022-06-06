<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_changeShift.aspx.cs" Inherits="content_hr_verify_changeShift" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" ID="head_approve_leave" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to this job?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
    </script>

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
    </style>


</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_changeShift">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Verify Change Shift</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
          <div class="x-head with-border">
             <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
             <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
             <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
             <div class="pull-right">
                <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
             </div>
             <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-30px; font-size:11px; "></asp:Label>
           </div>
        <div id="alert" runat="server" class="alert alert-empty" visible="false">
            <i class="fa fa-info-circle"></i>
            <span>No record found</span>
        </div>
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="empidd" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="shiftidTo" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                     <asp:BoundField DataField="changeshift_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                     <asp:TemplateField ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>

                    <asp:BoundField DataField="date_filed" HeaderText="Date Filed"/>
                      <asp:BoundField DataField="Employee_Name" HeaderText="Employee Name"/>

                    <%--<asp:BoundField DataField="shiftcodeFrom" HeaderText="Shift From"/>--%>

                    
                    <asp:TemplateField HeaderText="Shift From">
                                <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("shiftcodeFrom")%>'
                                tooltip='<%# Eval("aa") %>' ></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Shift To">
                                <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("shiftcodeTo")%>'
                                tooltip='<%# Eval("bb") %>' ></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnk_approve" OnClientClick="Confirm()" OnClick="delete2" ToolTip="approve"><i class="fa fa-thumbs-o-up"></i> </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnk_view" OnClick="view" ToolTip="cancel" style="margin-left:8px"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="80px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li>Reason</li>
        <li><asp:TextBox ID="txt_reason" TextMode="MultiLine" runat="server"></asp:TextBox></li>
        <li><hr /></li>
        <li><asp:Button ID="btn_save" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>




<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" style="visibility:hidden;" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
<asp:HiddenField ID="req_id" runat="server" />
</asp:Content>

