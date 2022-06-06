<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loan.aspx.cs" Inherits="content_hr_loan" EnableEventValidation="false" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script src="script/auto/myJScript.js" type="text/javascript"></script>

<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>


 <script type="text/javascript">
     $(document).ready(function () {
         $.noConflict();
         $(".auto").autocomplete({
             source: function (request, response) {
                 $.ajax({
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     url: "content/hr/addloan.aspx/GetEmployee",
                     data: "{'term':'" + $(".auto").val() + "'}",
                     dataType: "json",
                     success: function (data) {
                         response($.map(data.d, function (item) {
                             return {
                                 label: item.split('-')[1],
                                 val: item.split('-')[0]
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

<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datee").datepicker({ changeMonth: true,
                yearRange: "-100:+2",
                changeYear: true
            });
        });
    })(jQuery);
    </script>
    <style type="text/css">
      .hide{ display:none;}
        label {
            display: inline-block;
            max-width: 100%;
            margin-bottom: 0px;
            font-weight: 100;
        }
    </style>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>
<asp:Content runat="server" ID="loan_content" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Deduction</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-9">
        <div class="x_panel">
            <div class="x-head">
                <asp:TextBox ID="txt_search" runat="server"  Placeholder="Search" AutoComplete="off"></asp:TextBox>
                <asp:Button ID="Button1" OnClick="search" runat="server" Text="Search" CssClass="btn btn-primary"/>
                <asp:LinkButton ID="Button2" runat="server" style=" display:none;" OnClick="click_add_employee" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" OnClick="generatereport" CssClass="right add"></asp:LinkButton>
            </div>
            <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="IdNumber" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="loandate" HeaderText="LoanDate" DataFormatString="{0:MM/dd/yyyy}"/>
                 
                    <asp:BoundField DataField="FullName" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="DateStart" HeaderText="Start" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Dateend" HeaderText="End" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="OtherDeduction" HeaderText="Loan Type" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" />
                    <asp:BoundField DataField="MonthlyAmortization" HeaderText="Amortization" />
                    <asp:BoundField DataField="schedule" HeaderText="Schedule" />
                    <asp:BoundField DataField="IsPaid" HeaderText="Paid" />
                    <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="interest" HeaderText="Interest Amount" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:TemplateField HeaderText="Action" >
                        <ItemTemplate>
                          <%--  <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_approve" Text="approve"><i class="fa fa-location-arrow"></i></asp:LinkButton>--%>
                            <asp:LinkButton ID="lnk_delete" runat="server" OnClick="delete"  Text="cancel"><i class="fa fa-eye"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
                <div class="form-group">
                    <label>Payroll Group<asp:Label ID="lbl_pg" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:DropDownList ID="ddl_payroll_group"  runat="server"  CssClass="form-control minimal"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Employee<asp:Label ID="lbl_emp" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="ddl_emp" placeholder="Employee Name"  CssClass="form-control auto" runat="server"></asp:TextBox>
                </div>
                
                
                <div class="form-group">
                    <label>Loan Date<asp:Label ID="lbl_loandate" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_loandate" CssClass="form-control datee" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Start<asp:Label ID="lbl_datestart" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_loanstart" CssClass="form-control datee" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>End<asp:Label ID="lbl_dateend" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_loanend" CssClass="form-control datee" runat="server"></asp:TextBox>
                </div>
                
                
                
                <div class="form-group">
                    <label>Loan Type<asp:Label ID="lbl_loan" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:DropDownList ID="dll_deduction" runat="server" CssClass="form-control minimal"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Reference</label>
                    <asp:TextBox ID="dll_no" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Loan Amount<asp:Label ID="lbl_la" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_amount" runat="server" CssClass="form-control"  AutoComplete="off" onkeyup="decimalinput(this)" ></asp:TextBox>
                </div>
                <div class="form-group hidden">
                    <label>No. of Terms (Month/s)<asp:Label ID="lbl_not" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_no" runat="server" CssClass="form-control" Enabled="false" ontextchanged="txt_no_TextChanged" AutoComplete="off" onkeyup="decimalinput(this)" AutoPostBack="true"></asp:TextBox>
                </div>
                <div class="form-group hidden">
                    <label>Interest<asp:Label ID="lbl_li" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="lbl_interest" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Amortization<asp:Label ID="lbl_amort" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_amortization" CssClass="form-control" runat="server" onkeyup="decimalinput(this)"></asp:TextBox>
                </div>
                 <div class="form-group">
                    <label>Schedule Type</label>
                    <asp:DropDownList ID="ddl_schedule_type" OnTextChanged="click_select_scheduletype" AutoPostBack="true" CssClass="form-control minimal" runat="server">
                    <asp:ListItem Value="">Select</asp:ListItem>
                        <asp:ListItem Value="Per Month">Per Month</asp:ListItem>
                        <asp:ListItem Value="Per Payroll">Per Payroll</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:RadioButtonList ID="rbl_schedule" Visible="false" runat="server">
                    </asp:RadioButtonList>
                </div>
                <div class="form-group">
                    <label>Memo<asp:Label ID="lbl_memo" style="color:Red;" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txt_memo" TextMode="MultiLine"  style=" resize:none; height:50px" CssClass="form-control" runat="server"></asp:TextBox>
                    <hr/>
                </div>
                    <div class="mailbox-controls">
                    <div class="btn-group">
                    <asp:FileUpload ID="FileUpload2" runat="server" Width="250px" Height="30px"   />
                        <asp:Button ID="Button3" runat="server" OnClick="btn_save_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                     </div>
                <!-- Check all button -->
               <%-- <button type="button" class="btn btn-default btn-sm checkbox-toggle"><i class="fa fa-square-o"></i>
                </button>
                <div class="btn-group">
                  <button type="button" class="btn btn-default btn-sm"><i class="fa fa-trash-o"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fa fa-reply"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fa fa-share"></i></button>
                </div>
                <!-- /.btn-group -->
                <button type="button" class="btn btn-default btn-sm"><i class="fa fa-refresh"></i></button>--%>
              
                <!-- /.pull-right -->
              </div>
                    
               
               <%-- <div class="form-group">
                    <label>Batch Upload</label>
                    <asp:FileUpload ID="file_data" runat="server" />
                </div>--%>
        </div>
     </div>
</div>
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="l_name" runat="server"></asp:Label></h4>
    </div>
    <div class="modal-body">
        <ul class="input-form">
            <li>Remarks</li> 
            <li><asp:TextBox ID="txt_remarks" TextMode="MultiLine" style=" resize:none; height:300px;" runat="server"></asp:TextBox></li> 
        </ul>
    </div>
    <div class="modal-footer">
    <asp:Label ID="Label1" ForeColor="Red" runat="server" Text="*Note: If you click submit button the transaction will be cancelled." CssClass="pull-left"></asp:Label>
       <%--
       <asp:LinkButton ID="lnk_button" runat="server" CssClass="btn pull-left">Cancel</asp:LinkButton>
       <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="btn btn-primary"/>
       --%>
       <asp:Button ID="btn_save" runat="server" OnClick="click_delete_employee" Text="Delete" CssClass="btn btn-primary"/>
       <asp:Button ID="Button4" runat="server" OnClick="click_paid_loan" Text="Re-Loan" CssClass="btn btn-primary"/>
    </div>
</div>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="pg1" runat="server" />
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />


<asp:HiddenField ID="hdn_trnid" runat="server" />
</asp:Content>
