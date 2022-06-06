<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Otherdeduction.aspx.cs" Inherits="content_hr_Otherdeduction" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
 <script type="text/javascript">
     function Confirm() {
         var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
         if (confirm("Are you sure to cancel this transaction?"))
         { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
     } 
    </script>
   <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
      <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="jquery-1.10.2.min.js" type="text/javascript"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.7.7/xlsx.core.min.js"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xls/0.7.4-a/xls.core.min.js"></script> 
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/addPayrollOtherIncome.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0]
                            }
                        }))
                    },
                    error: function (result) {
                        alert(result.responseText);
                    }
                });
            },
            select: function (e, i) {
                index = $(".auto").parent().parent().index();
                $("#lbl_bals").val(i.item.val);
            }
        });
    });
</script>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
 <div class="page-title">
    <div class="title_left hd-tl">
        <h3> <h3>Deduction Set Up</h3></h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Mshiftcode"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Deduction Setup</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-9">
        <div class="x_panel">
            <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" OnRowDataBound="ordb" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="OtherDeduction" HeaderText="Description"/>
                    <asp:BoundField DataField="LoanType" HeaderText="Loan"/>
                    <asp:BoundField DataField="gl" HeaderText="GL Account"/>
                    <asp:BoundField DataField="status" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                           <%-- <asp:LinkButton ID="lnk_view" runat="server"  ToolTip="View" OnClick="view"><i class="fa fa-pencil"></i></asp:LinkButton>--%>
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Cancel" OnClientClick="Confirm()" OnClick="click_can"  ><i class="fa fa-trash-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="View" OnClick="allow" ><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <ul class="input-form">
                <li>Description</li>
                <li><asp:TextBox ID="txt_od" runat="server"></asp:TextBox></li>
                <li>Type</li>
                <li>
                    <asp:DropDownList ID="ddl_type" runat="server" OnTextChanged="change" AutoPostBack="true">
                    <asp:ListItem Value="0">N/A</asp:ListItem>
                    <asp:ListItem Value="static">Government Loan</asp:ListItem>
                    <asp:ListItem Value="2">Internal Loan</asp:ListItem>
                    <asp:ListItem Value="3">Savings(Every Payroll)</asp:ListItem>
                    <asp:ListItem Value="4">Others(One time deduction)</asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li style=" display:none;">Amount</li>
                <li style=" display:none;"><asp:TextBox ID="txt_amount" runat="server" AutoComplete="off" Enabled="false" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                <li>Interest (% per month)</li>
                <li><asp:TextBox ID="txt_interest" runat="server" AutoComplete="off" Enabled="false" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                <li>Loan</li>
                <li><asp:CheckBox ID="chk_loan" runat="server" Enabled="false" /></li>
                <li><hr /></li>
                <li><asp:Button ID="Button1" runat="server" OnClick="add_deduction" Text="ADD" CssClass="btn btn-primary" /></li>
                <li><asp:Label ID="lbl_errmsg" runat="server" ForeColor="Red" ></asp:Label></li>
            </ul>
        </div>
    </div>
</div>

 

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel" style=" width:500px; margin-left:-250px">
    <asp:ImageButton ID="ib_close" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li>Loan Type</li>
        <li><asp:TextBox ID="txt_type" runat="server"></asp:TextBox></li>
        <%--<li>Loan Amount</li>
        <li><asp:TextBox ID="txt_lamount" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></li>--%>
        <li><asp:Label ID="lbl_interest" runat="server" Text="Loan Interest"></asp:Label></li>
        <li><asp:TextBox ID="txt_interestt" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></li>
         
        <li><hr /></li>
        <li><asp:Button ID="btn_save" runat="server" OnClick="update" Text="Save" CssClass="btn btn-primary"/></li>
    </ul>
</div>
<div id="div_view_edit" runat="server" visible="false" class="PopUpPanel skills">
            <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop"  runat="server"/><%--OnClick="close"--%>
            <h4>Details</h4>
            <hr />

        <div class="col-md-3">
        <ul class="input-form">
              <li>Search Name</li>
              <li><asp:TextBox ID="txt_searchemp" runat="server" ClientIDMode="Static"  CssClass="auto" Placeholder="Search"></asp:TextBox></li>
              <li>Amount</li>
              <li><asp:TextBox ID="txt_deduct_amt" runat="server" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
               <li><asp:FileUpload ID="file_data" runat="server"/></li>
              <li><asp:Button ID="Button5" runat="server" Text="Add" OnClick="allowemp"  CssClass="btn btn-primary" /></li>
              <li>
                  <asp:Label ID="lbl_allowederr" runat="server" ForeColor="Red" ></asp:Label></li>
              </ul>
          </div>
           <div class="col-md-9">
           <b>Allowed Employee</b>
            <asp:GridView ID="grid_list" runat="server"  AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="emp_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="department" HeaderText="Department"/>
                    <asp:BoundField DataField="amount" HeaderText="Amount"/>
                    <asp:TemplateField ItemStyle-Width="40px">
                        <ItemTemplate>
                           <asp:LinkButton ID="lnk_can" runat="server" ToolTip="Cancel Transaction"  OnClientClick="Confirm()" OnClick="click_cancel_det"  ><i class="fa fa-trash-o"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
         
        </div>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="hdn_deductionid" runat="server" />
 <asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
</asp:Content>
