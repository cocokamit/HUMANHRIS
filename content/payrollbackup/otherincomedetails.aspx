<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otherincomedetails.aspx.cs" Inherits="content_payroll_otherincomedetails" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
<style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
.hiddencol { display: none; }

</style>
   <script src="script/auto/myJScript.js" type="text/javascript"></script>
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
</head>
<body>
    <form id="form2" runat="server">
    <h4>Other Income Transaction</h4>
    <hr />
    <asp:GridView ID="grid_view" runat="server" ClientIDMode="Static"  HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem"  style=" width:100%"  AutoGenerateColumns="false" >
        <Columns>
            <asp:BoundField DataField="e_name" HeaderText="Employee"/>
            <asp:BoundField DataField="Otherincome" HeaderText="Other Income"/>
            <asp:TemplateField HeaderText="Non-Taxable">
                <ItemTemplate>
                    <asp:Textbox ID="txt_nontax_amt"  OnClick="click_set" Text='<%# Eval("nontaxable_amt") %>' ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:Textbox>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Taxable">
                <ItemTemplate>
                    <asp:Textbox ID="txt_taxt_amt"  OnClick="click_set" Text='<%# Eval("taxable_amt") %>' ClientIDMode="Static" onkeyup="decimalinput(this);" runat="server"></asp:Textbox>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:BoundField DataField="worked_hrs" HeaderText="Worked Hours"/>
            <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
        </Columns>
    </asp:GridView>
    <hr />
    <%  System.Data.DataTable dtdt = dbhelper.getdata("select * from TPayrollOtherIncome where id=" + key.Value + " and payroll_id is not null");
        if (dtdt.Rows.Count == 0)
        {%>
    <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="updateTPayrollotherincomeline"  />
    <%} %>
    <asp:HiddenField ID="key" runat="server" />

    </form>
</body>
</html>





