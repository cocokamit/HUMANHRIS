<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_undertime.aspx.cs" Inherits="content_hr_verify_undertime" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" ID="head_approve_leave" runat="server">

<script type="text/javascript">
    bkLib.onDomLoaded(function () { nicEditors.allTextAreas() });

    function Confirm(elem, msg) {
        if (elem.getAttribute("title") != 'approved') {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to " + msg + " this job?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
    }

    function Confirm_a() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to approved this undertime?"))
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

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_undertime">
<div class="page-title">
    <div class="title_left">
        <h3>Undertime Verification</h3>
    </div>
    <div class="clearfix"></div>
    <div class="row">
            
        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x-head">
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
                    
                    <div class="pull-right">
                        <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                        <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app"  CssClass="btn btn-default" ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                        <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-60px; font-size:11px; "></asp:Label>
                    </div>
                </div>

                <div id="alert" runat="server" class="alert alert-empty">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>

            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                     </asp:TemplateField>

                    <asp:BoundField DataField="date_filed" HeaderText="Undertime"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Employee"/>
                    <asp:BoundField DataField="timeout" HeaderText="Time"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:TemplateField HeaderText="Action">
                         
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_app" runat="server" OnClick="delete2" OnClientClick="Confirm_a()" ToolTip="approved"> <i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_cancelled" runat="server" OnClick="view" ToolTip="cancel"><i class="fa fa-trash-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="70"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
     </div>
</div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Delete Transaction</h4>
    </div>
    <div class="modal-body">
         <ul class="input-form">
            <li><asp:TextBox ID="txt_reason" TextMode="MultiLine" runat="server" placeholder="Remarks"></asp:TextBox></li>
        </ul>
        <asp:Button ID="btn_save" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" />
    </div>
</div>

<asp:TextBox ID="TextBox1" runat="server" CssClass="hide"></asp:TextBox>   
<asp:HiddenField ID="idd" runat="server" />   
</asp:Content>
