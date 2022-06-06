<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otherdeductiondetails.aspx.cs" Inherits="content_payroll_otherdeductiondetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
.hiddencol { display: none; }
</style>
  <link rel="stylesheet" type="text/css" href="../../style/fixedheader/normalize.css"  /> <%--href="style/fixedheader/normalize.css"--%>
        <link rel="stylesheet" type="text/css" href="../../style/fixedheader/demo.css" />
        <link rel="stylesheet" type="text/css" href="../../style/fixedheader/component.css" />
        <style type="text/css">
        .sticky-wrap .sticky-intersect th { background-color:#437cab}
        .sticky-intersect th , .sticky-col tbody tr th {border:1px solid #fff !important} 
        
        .Overlay { position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
        .PopUpPanel {top:5% !important; width:1000px; position:absolute;background-color: #fff; margin-left:-500px;  top:10%;left:50%;z-index:2001; padding:20px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;}
        .PopUpPanel input[type=image]{ margin:-30px;float:right;}
        
        </style>


<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="../../script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="jquery-1.10.2.min.js" type="text/javascript"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.7.7/xlsx.core.min.js"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xls/0.7.4-a/xls.core.min.js"></script>

 
     <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="../../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../../style/font-awesome/css/font-awesome.min.css" rel="stylesheet">
        <link href="../../style/css/custom.min.css" rel="stylesheet">
    <style type="text/css">
        .table { width:100%}
        .wrapper{padding:10px} 
        .sticky-wrap.overflow-y {max-height: 60vh;}
        .overflow-y tr th{padding:5px 10px !important}
        .overflow-y tr {border:none !important}
        .modal-header {padding:5px 0 !important}
        .modal-body { padding:5px 0 !important}
        .input-form input[type=text]{ height:35px}
        .PopUpPanel .btn {padding:5px 8px}
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <h2>Other Deduction Transaction</h2>
        <asp:GridView ID="grid_view" runat="server" ClientIDMode="Static"  Class="overflow-y"   AutoGenerateColumns="false" >
            <Columns>
                    <%--<asp:BoundField DataField="IdNumber" HeaderText="Id Number"/>--%>
                    <asp:BoundField DataField="e_name" HeaderText="Employee"/>
                    <asp:BoundField DataField="otherdeduction" HeaderText="Other Deduction"/>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Textbox ID="txt_amt"  OnClick="click_set" Text='<%# Eval("Amount") %>' ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:Textbox>
                        </ItemTemplate>
                    </asp:TemplateField>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                    <asp:BoundField DataField="balance" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                    <asp:BoundField DataField="loan_id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                    <asp:BoundField DataField="PayOtherDeduction_id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
            </Columns>
        </asp:GridView>
        
<asp:Button ID="btn_add" OnClick="view" runat="server" Text="ADD" CssClass="pull-left btn btn-default" />
<asp:Button ID="Button1" runat="server" Text="Update" OnClick="updateTPayrollotherdeductionline"  CssClass="btn btn-primary pull-right"/>
        
    </div>
   

 
<asp:HiddenField ID="key" runat="server" />

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
        <div class="box-body no-pad-top">
             <div class="modal-header">
                <b>Additional Deduction</b>
              </div>
              <div class="modal-body">
                  <div class="row">
                      <div class="col-lg-4">
                   
                        <ul class="input-form">
                            <li><asp:Label ID="lbl_err_msg" runat="server" ForeColor="Red"></asp:Label></li>
                            <li><asp:FileUpload ID="file_data" runat="server"/></li>
                            <li>Search</li>
                            <li><asp:TextBox ID="txt_searchemp"  runat="server"  ClientIDMode="Static"  CssClass="auto"></asp:TextBox></li>
                            <li>Deduction Type</li>
                            <li><asp:DropDownList ID="ddl_dt"  runat="server"></asp:DropDownList></li>
                            <li>Amount</li>
                            <li><asp:TextBox ID="txt_addinc_amt"  runat="server" AutoComplete="off"  ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                            <li>Notes</li>
                            <li><asp:TextBox ID="txt_addinc_remarks" TextMode="MultiLine" style=" resize:none;" runat="server"></asp:TextBox></li>
                        </ul>
                         <asp:Button ID="Button2" runat="server" Text="Add" OnClick="insertadditionaldeduct" CssClass="btn btn-default" style="margin-top:8px" />
                      </div>
                      <div class="col-lg-8">
                        <asp:GridView ID="grid_add_deduct" runat="server" EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" CssClass="table" >
                            <Columns>
                                <asp:BoundField DataField="empid" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                <asp:BoundField DataField="deductionid" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                <asp:BoundField DataField="empname" HeaderText="Employee Name"/>
                                <asp:BoundField DataField="deductionname" HeaderText="Deduction Type"/>
                                <asp:BoundField DataField="amount" HeaderText="Total Amt"/>
                                <asp:BoundField DataField="notes" HeaderText="Description"/>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lb_delete" runat="server" OnClick="delete"><i class="fa fa-minus-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                         <asp:Button ID="btn_save" runat="server"  Text="Save" OnClick="btn_save_Click" CssClass="btn btn-primary pull-right" />
                      </div>
                  </div>
              </div>
    </div>
</div>

    <script src="style/js/jquery.min.js"></script>
    <script src="style/js/JScript.js"></script>
	<script src="style/js/jquery.stickyheader.js"></script>

     <asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />

     <script type="text/javascript">
         $(document).ready(function () {
             $.noConflict();
             $(".auto").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         url: "content/payroll/otherdeductiondetails.aspx/GetEmployee",
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
    </form>
</body>
</html>

