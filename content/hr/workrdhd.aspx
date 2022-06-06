<%@ Page Language="C#" AutoEventWireup="true" CodeFile="workrdhd.aspx.cs" Inherits="content_hr_workrdhd" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to approved this transaction?"))
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

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Work Verification</h3>
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

            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                   <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                   <asp:TemplateField ItemStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;" OnCheckedChanged="chkboxSelectAll_CheckedChanged" AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox ID="chkEmp" runat="server" onclick="CheckBoxCount();" ></asp:CheckBox>
                                </ItemTemplate>
                        </asp:TemplateField>
                    <asp:BoundField DataField="sysdate" HeaderText="System Date"/>
                    <asp:BoundField DataField="idnumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="date" HeaderText="Date"/>
                    <asp:BoundField DataField="reason" HeaderText="Reason"/>
                    <asp:BoundField DataField="employeeid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                 <%--   <asp:BoundField DataField="class" HeaderText="Class" />--%>
                    <asp:TemplateField ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_app" runat="server" OnClick="delete2" OnClientClick="Confirm()" Tooltip="approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_cancelled" runat="server" OnClick="view" Tooltip="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div id="alert" runat="server" class="alert alert-empty" visible="false">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
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


<div class="hide"> 
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>   
</div>


<asp:HiddenField ID="idd" runat="server" />

<asp:HiddenField ID="key" runat="server" />
</asp:Content>