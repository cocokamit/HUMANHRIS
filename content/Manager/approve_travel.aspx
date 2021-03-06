<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_travel.aspx.cs" Inherits="content_Manager_approve_travel" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_travel_list">
 <script type="text/javascript">
     function ConfirmRem() {
         var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
         if (confirm("Are you sure you want to approve this transaction?"))
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
    bkLib.onDomLoaded(function () { nicEditors.allTextAreas() });

       function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure you want to approve this travel request?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
        function Confirmcan() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this travel application?"))
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

</asp:Content>


<asp:Content ContentPlaceHolderID="content" ID="content_approveTravel" runat="server">

<section class="content-header">
    <h1>OBT Approval</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">OBT Approval</li>
    </ol>
</section>

<section class="content">
    <div class="row">
        <div id="pinfra" visible="false" runat="server" class="Overlay"></div>
        <div id="pinfra2" visible="false" runat="server" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton12" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
            <div class="modal-header">
                <h4 class="modal-title">Official Business Trip</h4>
            </div>
            <div class="modal-body">
                <asp:GridView ID="gridobt" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
                  <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="nxtid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="fileDate" HeaderText="File Date"/>
                    <asp:BoundField DataField="tvlDate" HeaderText="Travel Date"/>
                    <asp:BoundField DataField="tIn" HeaderText="Travel In"/>
                    <asp:BoundField DataField="tOut" HeaderText="Travel Out"/>
                    <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn" runat="server" ToolTip="Cancel" CommandName='<%# Eval("id") %>' Text="Cancel" OnClick="cancelobt" OnClientClick="Confirmcan()"><i class="fa fa-trash"></i></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="75px" />
                    </asp:TemplateField>
                  </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="col-xs-12">
          <div class="box box-primary">
           <div class="box-header">
                <div class="input-group pull-left">
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee form-control" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee form-control" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
                </div>
                <div id="div_action" runat="server" class="input-group pull-right">
                    <asp:LinkButton runat="server" OnClick="delete_all" ID="ib_del" CssClass="btn btn-default" style="margin-right:5px; float:left;"  ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" style="float:left;"  ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin-top:-25px; font-size:11px; "></asp:Label>
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found"  CssClass="table table-striped table-bordered no-margin">
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
                        <asp:BoundField DataField="date_input" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                         <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="purpose" HeaderText="Purpose"/>
                        <asp:BoundField DataField="travel_start" HeaderText="Travel Start"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="travel_end" HeaderText="Travel End"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="nxt_id" HeaderText="" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkbyone" ToolTip="view" OnClick="viewobt" ><i class="fa fa-info-circle"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnk_approve" ForeColor="Red" OnClientClick="Confirm()" OnClick="approve" ToolTip="approve"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                                 <asp:LinkButton runat="server" ID="lnk_view" OnClick="view" ToolTip="cancel" OnClientClick="Confirmcan()"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                              <ItemStyle  Width="130px"/>
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


<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" style="visibility:hidden;" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
</asp:Content>

