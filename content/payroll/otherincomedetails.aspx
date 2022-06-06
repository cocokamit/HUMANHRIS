<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otherincomedetails.aspx.cs" EnableEventValidation ="false" Inherits="content_payroll_otherincomedetails" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
<style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
.hiddencol { display: none; }
.box-primary { border-top-color: white;}
</style>

<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="../../script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="../../script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
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
<link rel="stylesheet" href="../../vendors/bootstrap/dist/css/bootstrap.min.css"/>
<link rel="stylesheet" href="../../vendors/font-awesome/css/font-awesome.min.css">
<link rel="stylesheet" href="../../dist/css/base.css">
<link rel="stylesheet" href="../../dist/css/custom.css" />
<link rel="stylesheet" href="../../dist/css/skins/_all-skins.min.css">
<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
    <link href="../../style/fixheadergrid/css/web.css" rel="stylesheet" />
    <script type="text/javascript" src="style/fixheadergrid/js/gridviewscroll.js"></script>
    <script type="text/javascript">
        var gridViewScroll = null;
        window.onload = function () {
            gridViewScroll = new GridViewScroll({
                elementID: "grid_view",
                width: window.innerWidth,
                height: 500,
                freezeColumn: true,
                // freezeFooter: true,
                freezeColumnCssClass: "GridViewScrollItemFreeze",
                freezeFooterCssClass: "GridViewScrollFooterFreeze",
                freezeHeaderRowCount: 1,
                freezeColumnCount: 3,
                onscroll: function (scrollTop, scrollLeft) {
                    console.log(scrollTop + " - " + scrollLeft);
                }
            });
            gridViewScroll.enhance();

            //redirect exact row
            var id = document.getElementById("hdn_selected_id").value;
            var fid = '#' + id;
            $('.test').attr('href', fid);
            window.location.href = $('.test').attr('href');
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
</script>
</head>
<body>
    <form id="form2" runat="server">
     <section class="content">
      <div class="row">
        <div class="col-md-3">
           <div class="form-group">
                    <label>Search Employee</label>
                    <asp:TextBox ID="txt_searchemp"  runat="server"  ClientIDMode="Static"  Placeholder="Search Employee" class="form-control has-feedback-left auto"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Amount</label>
                    <asp:TextBox ID="txt_addinc_amt"  runat="server" AutoComplete="off" class="form-control has-feedback-left" placeholder="Amount"  ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Type</label>
                    <asp:DropDownList ID="ddl_type" runat="server" class="form-control"> </asp:DropDownList>                                
                </div>
                 <div class="form-group">
                    <asp:Button ID="btn_submit" runat="server" OnClick="addincome" Visible="false" Text="SUBMIT" CssClass="btn btn-primary"    />                       
                </div>
        </div>
        <!-- /.col -->
        <div class="col-md-9">
      
            <div class="box-body no-padding">
               <asp:GridView ID="grid_view" runat="server" ClientIDMode="Static"   AutoGenerateColumns="false" >
                    <Columns>
                        <asp:BoundField DataField="e_name" HeaderText="Employee"/>
                        <asp:BoundField DataField="Otherincome" HeaderText="Other Income"/>
                        <asp:TemplateField HeaderText="Non-Taxable">
                            <ItemTemplate>
                                <asp:Textbox ID="txt_nontax_amt" Enabled="false"  OnClick="click_set" Text='<%# Eval("nontaxable_amt") %>' ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Taxable">
                            <ItemTemplate>
                                <asp:Textbox ID="txt_taxt_amt" Enabled="false"  OnClick="click_set" Text='<%# Eval("taxable_amt") %>' ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:BoundField DataField="worked_hrs" Visible="false"  ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                             <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_can" OnClick="click_delete" OnClientClick="Confirm()" runat="server"><i class="fa fa-trash-o"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

         
            </div>
            <hr />
            <asp:Button ID="Button2" runat="server" Text="Download" OnClick="ExportToExcel" CssClass="btn btn-primary"    />
            <!-- /.box-body -->
            </div>

        <!-- /.col -->
      </div>
      <!-- /.row -->
    </section>
    <!-- /.content -->




<%--     <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="updateTPayrollotherincomeline"  />--%>
  
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
     <asp:HiddenField ID="TextBox1" runat="server" />
    
    </form>
</body>
</html>





