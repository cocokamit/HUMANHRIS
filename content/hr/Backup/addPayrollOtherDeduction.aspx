<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addPayrollOtherDeduction.aspx.cs" Inherits="content_hr_addPayrollOtherDeduction" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server"  ContentPlaceHolderID="head">
    <style type="text/css">
        .table-input input[type=text] {border:none; background: transparent}
        .table-input select {border:none; background: transparent; -moz-appearance: none; -webkit-appearance:none;}
    </style>
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
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_addpayroll">
<asp:ScriptManager ID="ScriptManager" runat="server" />
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Other Deduction Application</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="Mpayotherdeduction"><i class="fa fa-credit-card"></i> Other Deduction</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Other Deduction Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-9">
        <div class="x_panel">
           <%-- <asp:Label ID="lbl_err1" runat="server" ForeColor="Red"></asp:Label>--%>
            <b>Loan</b>
            <%--<asp:GridView ID="grid_item" runat="server" EmptyDataText="No Data Found!" EmptyDataRowStyle-ForeColor="Red" onrowdeleting="grid_item_RowDeleting" OnRowDataBound="OnRowDataBound" AutoGenerateColumns="False"  CssClass="table table-striped table-bordered table-input">
                <Columns>
               
                    <asp:TemplateField ItemStyle-Width="15px"> 
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate >             
                            <asp:ImageButton ID="btn" runat="server" Visible="false" ImageUrl="~/style/img/add.png" OnClick="ButtonAdd_Click"  />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddl_emp" runat="server" Enabled="false" DataSourceID="sql_emp" DataTextField="Fullname" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbl_emp" runat="server" CssClass="na" Text=""></asp:Label>
                            <asp:Label ID="lbl_emp_desp"  runat="server" Text=""></asp:Label> 
                            <asp:SqlDataSource ID="sql_emp" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select a.Id,a.lastname+' '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,c.PayrollGroup from MEmployee a left join MPosition b on a.PositionId=b.Id left join MPayrollGroup c on a.PayrollGroupId=c.Id where a.PayrollGroupId=?">
                            <selectparameters>
                            <asp:controlparameter name="emp_group" controlid="ddl_payroll_group" propertyname="SelectedValue"/>
                            </selectparameters>
                            </asp:SqlDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other Deduction">
                        <ItemTemplate>
                            <asp:DropDownList ID="dll_income" runat="server" Enabled="false" DataSourceID="sql_income" DataTextField="OtherDeduction" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true" >
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbl_other" runat="server" CssClass="na" Text=""></asp:Label>
                            <asp:Label ID="lbl_other_desp"  runat="server" Text=""></asp:Label> 
                            <asp:SqlDataSource ID="sql_income" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from MOtherDeduction">
                            </asp:SqlDataSource>
                            <asp:HiddenField ID="loan_id" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lbl_descripton" runat="server" CssClass="na" Text=""></asp:Label>
                            <asp:Label ID="lbl_descripton_desp"   runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txt_description" Width="320px"  autocomplete="off" onkeyup="decimalinput(this)"  runat="server"></asp:TextBox> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                       <asp:TemplateField HeaderText="Balance">
                        <ItemTemplate>
                            <asp:Label ID="lbl_balance" runat="server" CssClass="na" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField  >
                        <ItemTemplate>
                            <asp:ImageButton ID="can" runat="server" CausesValidation="false"  CommandName="Delete" ImageUrl="~/style/img/delete.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>            
            </asp:GridView>--%>
            <asp:GridView ID="grid_item" runat="server" EmptyDataText="No Data Found!" EmptyDataRowStyle-ForeColor="Red"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered table-input">
                <Columns>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="loanid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="deductionid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="empname" HeaderText="Employee"/>
                    <asp:BoundField DataField="otherdeduction" HeaderText="Other Deduction"/>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_amt" runat="server" AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this);" Text='<%#bind ("Amortization")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
              <%--      <asp:BoundField DataField="Amortization" HeaderText="Amount"/>--%>
                    <asp:BoundField DataField="balance" HeaderText="Balance"/>
                <%--    <asp:TemplateField  >
                        <ItemTemplate>
                            <asp:ImageButton ID="can" runat="server" CausesValidation="false"  CommandName="Delete" ImageUrl="~/style/img/delete.png" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>            
            </asp:GridView>
             <b>Others</b>
            <asp:GridView ID="grid_contribution" runat="server" EmptyDataText="No Data Found!" EmptyDataRowStyle-ForeColor="Red"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-input">
                <Columns>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="deductionid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="empname" HeaderText="Employee"/>
                    <asp:BoundField DataField="otherdeduction" HeaderText="Other Deduction"/>
                    <asp:BoundField DataField="amount" HeaderText="Amount"/>
                 </Columns>
            </asp:GridView>
            <b>Additional Deduction</b>
             <asp:GridView ID="grid_add_deduct" runat="server" EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="deductionid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="empname" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="deductionname" HeaderText="Deduction Type"/>
                    
                    <asp:BoundField DataField="amount" HeaderText="Total Amt"/>
                    <asp:BoundField DataField="notes" HeaderText="Description"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="can" runat="server" CausesValidation="false" OnClick="delete" ImageUrl="~/style/img/delete.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
                <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="btn btn-primary" /> 
            <br />
            <asp:Label ID="lbl_errsave" ForeColor="Red" runat="server" ></asp:Label> 
         
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <ul class="input-form">
                <li>Payroll Group</li>
                <li>
                    <asp:DropDownList ID="ddl_payroll_group" OnTextChanged="selectpg" runat="server"  AutoPostBack="true">
                    </asp:DropDownList>
                </li>
                <li>Remarks<asp:Label ID="lbl_remarks" runat="server"  ForeColor="Red"></asp:Label></li>
                <li> <asp:TextBox ID="txt_remarks" TextMode="MultiLine" style=" resize:none;" runat="server"></asp:TextBox></li>
            </ul>
         
        </div>
        <div class="x_panel">
           <b>Additional Deduction </b>
            <ul class="input-form">
                
                <li></li>
                <li><asp:FileUpload ID="file_data" runat="server" /></li>
                
                <li>Search</li>
                <li><asp:TextBox ID="txt_searchemp"  runat="server"  ClientIDMode="Static"  CssClass="auto"></asp:TextBox></li>
                <li>Deduction Type</li>
                <li><asp:DropDownList ID="ddl_dt"  runat="server"></asp:DropDownList></li>
                <li>Amount</li>
                <li>
                <asp:TextBox ID="txt_addinc_amt"  runat="server" AutoComplete="off"  ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox>
                </li>
                <li>Notes</li>
                <li><asp:TextBox ID="txt_addinc_remarks" TextMode="MultiLine" style=" resize:none;" runat="server"></asp:TextBox></li>
                <li><asp:Label ID="lbl_err_msg" runat="server" ForeColor="Red"></asp:Label></li>
            </ul>
            <asp:Button ID="Button2" runat="server" Text="Add" OnClick="insertadditionaldeduct" CssClass="btn btn-primary" />
        </div>

    </div>
</div>

<div class="odd">
 



</div>
  <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
       <asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
</asp:Content>