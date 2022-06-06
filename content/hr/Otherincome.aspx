<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Otherincome.aspx.cs" Inherits="content_hr_Otherincome" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        label{ margin:0 5px 0 10px}
    </style>
         <script type="text/javascript">
             function Confirm() {
                 var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
                 if (confirm("Are you sure you want to cancel this transaction?"))
                 { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
             } 
    </script>
 <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
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
        <h3>Other Income Set Up</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Other Income Set Up</li>
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
                    <asp:BoundField DataField="type" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="OtherIncome" HeaderText="Name"/>
                    <asp:BoundField DataField="frequency" HeaderText="Frequency"/>
                    <asp:BoundField DataField="variability" HeaderText="Variability"/>
                    <asp:BoundField DataField="taxrule" HeaderText="Tax Rule"/>
                    <asp:BoundField DataField="incometype" HeaderText="Income Type"/>
                    <asp:BoundField DataField="schedule" HeaderText="Schedule"/>
                    <asp:BoundField DataField="notes" HeaderText="Note"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" runat="server" ToolTip="View Transaction" OnClick="click_view" ><i class="fa fa-eye-slash"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_can" runat="server" ToolTip="Cancel Transaction" OnClick="click_can" OnClientClick="Confirm()" ><i class="fa fa-trash-o"></i></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="View" OnClick="view" ><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <ul class="input-form">
                <li>Description <asp:Label ID="lbl_ioerr" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_oi" runat="server"></asp:TextBox></li>
                <li>Frequency <asp:Label ID="Label3" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li>
                    <asp:DropDownList ID="ddl_frequency" OnSelectedIndexChanged="click_frequency" AutoPostBack="true" runat="server"></asp:DropDownList>
                </li>
                <li>Variability <asp:Label ID="Label1" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li>
                    <asp:DropDownList ID="ddl_type" runat="server"></asp:DropDownList>
                </li>
                <li>Tax Rule <asp:Label ID="Label2" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li>
                    <asp:DropDownList ID="ddl_tax_rule" OnTextChanged="clicktaxrule" AutoPostBack="true" runat="server"></asp:DropDownList>
                </li>
                <li style=" display: none; ">Tax Ceiling <asp:Label ID="lbl_taxceilingerr" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li style=" display: none; "><asp:TextBox ID="txt_ceiling" runat="server" AutoComplete="off" Text="0" onkeyup="decimalinput(this)"></asp:TextBox></li>
                <li style=" display: none; ">Amount <asp:Label ID="lbl_amterr" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li style=" display: none; "><asp:TextBox ID="txt_amount" runat="server" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></li>
                 <asp:CheckBox ID="chk_mandatory" Visible="false" runat="server" Text="Mandatory" />
                <li>Income Type <asp:Label ID="lbl_income_type" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
                <li>
                    <asp:DropDownList ID="ddl_income_type" runat="server"></asp:DropDownList>
                </li>

                <li><asp:Label ID="lbl_schedule" Text='Schedule' Visible="false" runat="server"></asp:Label><asp:Label  ID="lbl_sechedule" runat="server" ForeColor="Red" style=" font-size:large;"></asp:Label></li>
                <li>
                    <asp:RadioButtonList ID="rbl_range" runat="server"></asp:RadioButtonList>
                </li>
                <li style=" display:none; font-size:10px; color:Red;">*Note: If taxable checkbox was checked! Withholding Tax will be deducted every payroll.</li>
                <li style="display:none;"><asp:CheckBox ID="chk_taxable" runat="server" OnCheckedChanged="checkistaxable" AutoPostBack="true" Text="Taxable"/></li>
                <li><hr/></li>
                <li><asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="add_income" Text="ADD"/></li>
                <li><asp:Label ID="lbl_error" ForeColor="Red" style=" font-size:large;" runat="server"></asp:Label></li>
            </ul>
        </div>
    </div>
    <div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
    <div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel skills">
        <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/><%--OnClick="close"--%>
        <h4>Allow Employee</h4>
        <asp:TextBox ID="txt_search" runat="server" placeholder="Search" style=" padding:10px"></asp:TextBox>&nbsp<asp:Button ID="Button2" runat="server"  Text="Search" OnClick="search" CssClass="btn btn-primary"/>
        <hr />
        <asp:GridView ID="grid_emp" runat="server"   AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" />
                <asp:BoundField DataField="emp_name" HeaderText="Employee Name"/>
                <asp:BoundField DataField="company" HeaderText="Companay"/>
                <asp:BoundField DataField="branch" HeaderText="Branch" />
                <asp:BoundField DataField="department" HeaderText="Department"/>
                <asp:TemplateField ItemStyle-Width="40px">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkboxSelectAll" runat="server" style=" font-size:9px; font-weight:normal;"  OnCheckedChanged="chkboxSelectAll_CheckedChanged"  AutoPostBack="true"  /><%--OnCheckedChanged="chkboxSelectAll_CheckedChanged"--%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkEmp" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <hr />
        <asp:Button ID="Button3" runat="server" OnClick="process" Text="ADD" CssClass="btn btn-primary"/>
    </div>
    <div id="div_view_edit" runat="server" visible="false" class="PopUpPanel skills">
        <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/><%--OnClick="close"--%>
        <h4>Details</h4>
        <hr />

        <div class="col-md-3">
        <ul class="input-form">
            <li>Search Name</li>
            <li><asp:TextBox ID="txt_searchemp" runat="server" ClientIDMode="Static"  CssClass="auto" Placeholder="Search"></asp:TextBox></li>
            <li>Amount</li>
            <li><asp:TextBox ID="txt_income_amt" runat="server" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
            <li><asp:FileUpload ID="file_data" runat="server"/></li>
            <li><asp:Button ID="Button5" runat="server" Text="Add" OnClick="process"  CssClass="btn btn-primary"/></li>
            <li><asp:Label ID="lbl_allowederr" runat="server" ForeColor="Red"></asp:Label></li>
        </ul>
        </div>
        <div class="col-md-9">
        <li><asp:Button ID="btn_deleteall" runat="server" Text="Delete All Entries" OnClientClick="Confirm()" OnClick="deleteall" CssClass="btn btn-primary right"/></li>
        <b>Allowed Employee</b>
        <asp:GridView ID="grid_list" runat="server"   AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                <asp:BoundField DataField="emp_name" HeaderText="Employee Name"/>
                <asp:BoundField DataField="department" HeaderText="Department"/>
                <asp:BoundField DataField="amount" HeaderText="Amount"/>
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_can" runat="server" ToolTip="Cancel Transaction"  OnClientClick="Confirm()" OnClick="click_cancel_det" ><i class="fa fa-trash-o"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>
        </div>
    </div>

<div class="modal fade in" id="modalview" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <div align="right">
                <asp:LinkButton ID="lnkbtnclose" OnClick="close" runat="server"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
            </div>
            <h4 class="modal-title"><asp:Label ID="lbloiname" Text="Allowed Income" runat="server" ForeColor="Blue"></asp:Label></h4>
        </div>
        <div class="modal-body">
          <asp:GridView ID="viewdetails" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="employee" HeaderText="Employee Name"/>
                <asp:BoundField DataField="amount" HeaderText="Amount"/>
            </Columns>
        </asp:GridView>
        </div>
    </div>
    </div>
</div>


<asp:HiddenField ID="otherincomeid" runat="server" />
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 

</asp:Content>
