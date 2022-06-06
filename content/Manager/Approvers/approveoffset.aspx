<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approveoffset.aspx.cs" Inherits="content_Manager_approveoffset" MasterPageFile="~/content/site.master" %>
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

<script type="text/javascript">

    bkLib.onDomLoaded(function () { nicEditors.allTextAreas() });

    function Confirm(elem, msg) {
        if (elem.getAttribute("title") != 'approved') {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to " + msg + " this job?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
    }

    function CheckBoxCount() {
        var gv = document.getElementById("<%= grid_view.ClientID %>");
        var lbl_del_notify = document.getElementById("<%= lbl_del_notify.ClientID %>");
        var lbl_del_notify_val = document.getElementById('lbl_del_notify');
        var inputList = gv.getElementsByTagName("input");
        var numChecked = 0;

        for (var i = 0; i < inputList.length; i++) {
            if (inputList[i].type == "checkbox" && inputList[i].checked) {
                numChecked = numChecked + 1;
            }
        }

        if (numChecked > 0) {
            if (numChecked == 1)
                lbl_del_notify.textContent = numChecked + " row selected";
            else
                lbl_del_notify.textContent = numChecked + " rows selected";
            document.getElementById("<%= ib_del.ClientID %>").style.display = "block";
            document.getElementById("<%= ib_can.ClientID %>").style.display = "block";
        }
        else {
            lbl_del_notify.textContent = "";
            document.getElementById("<%= ib_del.ClientID %>").style.display = "none";
            document.getElementById("<%= ib_can.ClientID %>").style.display = "none";
        }
    }


    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to approve this Offset?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }

    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_approve_ot" runat="server">
<section class="content-header">
    <h1>Offset Approval</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Offset Approval</li>
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
                <div  id="div_action" runat="server" class="input-group pull-right">
                    <asp:LinkButton runat="server" OnClick="delete_all" ID="LinkButton1" CssClass="btn btn-default" style="margin-right:5px; float:left;"  ToolTip="Deleted"><i class="fa fa-trash border-right"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" OnClick="approve_all" ID="ib_app" CssClass="btn btn-default" style="float:left;"  ToolTip="Approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                </div>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <asp:Label ID="Label1" runat="server" style=" position:absolute; right:0; margin-top:-25px; font-size:11px; "></asp:Label>
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
                        <asp:BoundField DataField="date" HeaderText="Date Filed" />
                        <asp:BoundField DataField="date_offset" HeaderText="Date Offset" />
                        <asp:BoundField DataField="Fullname" HeaderText="Employee" />
                        <asp:BoundField DataField="offsethrs" HeaderText="Offset Hrs."/>
                        <asp:BoundField DataField="status" HeaderText="Status"/>
                        <asp:BoundField DataField="nxt_id" HeaderText="" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField ItemStyle-Width="70px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_app" runat="server" OnClick="approved_offset" OnClientClick="Confirm()" Tooltip="approved"><i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnk_cancelled" runat="server" OnClick="view" Tooltip="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
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
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" runat="server"/>
  
       <div class="box-body table-responsive no-pad-top">
        <div class="form-group">
            <label>Reason</label>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
     <asp:Button ID="btn_save" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" />
</div>


<div class="hide"> 
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>   
</div>

<asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin:15px; font-size:11px;display:none; " ></asp:Label>
<asp:HiddenField ID="idd" runat="server" />
<asp:LinkButton ID="ib_can" runat="server"  CssClass="btn btn-danger" Text="cancel" style="margin-top:-2px; margin-left:5px; float:left; display:none; float:right;" ></asp:LinkButton>
        <asp:LinkButton ID="ib_del" runat="server"  CssClass="btn btn-primary" Text="approve" style="margin-top:-2px;  float:left; display:none; float:right;" ></asp:LinkButton>
    
</asp:Content>
