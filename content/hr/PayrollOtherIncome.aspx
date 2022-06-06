<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayrollOtherIncome.aspx.cs" Inherits="content_hr_PayrollOtherIncome" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="head">
    <style type="text/css">
        .PopUpPanel { margin-left:-250px; width:500px}
           .hiddencol { display: none; }
    </style>
    <script type="text/javascript" src="script/gridviewpane/gridpane.js"></script>
    <script type="text/javascript">
        $(function() {
            $("[id*=imgOrdersShow]").each(function() {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgProductsShow]").each(function() {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });

    </script>
       <script type="text/javascript">
           function Confirm() {
               var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
               if (confirm("Are you sure to cancel this transaction?"))
               { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
           } 
    </script>
</asp:Content>
 
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_other_income">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Other Income</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
              <%--  <asp:DropDownList ID="ddl_payroll_group"  runat="server" CssClass="minimal"></asp:DropDownList>--%>
                <asp:TextBox ID="txt_search" placeholder="search" runat="server"></asp:TextBox>
                <asp:Button ID="btn_search" runat="server" Text="Search" onclick="search" CssClass="btn btn-primary"/>
              <%--  <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_other" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>--%>
            </div>
           <asp:GridView ID="grid_oi" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" OnRowDataBound="rowbound"  CssClass="table table-striped table-bordered"><%--OnRowDataBound="rowbound"--%>
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                 <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lbl_oi" runat="server"  style=" text-transform:uppercase; font-weight:bold;" Text='<%# bind("otherincome") %>'></asp:Label>
                            <asp:GridView ID="grid_oilist" runat="server" EmptyDataText="No Data Found!" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" CssClass="Grid">
                                    <Columns>
                                       <%--<asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="idd" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>--%>
                                        <asp:BoundField DataField="payrolldate" HeaderText="Transaction Date"/>
                                        <asp:BoundField DataField="range" HeaderText="Range"/>
                                        <asp:BoundField DataField="ename" HeaderText="Name"/>
                                       <%-- <asp:BoundField DataField="otherdeduction" HeaderText="Deduction"/>--%>
                                        <asp:BoundField DataField="amount" HeaderText="Amount"/>
                                    </Columns>
                                </asp:GridView>
                        </ItemTemplate>
                    
                    </asp:TemplateField>
            </Columns>
        </asp:GridView>
  
        </div>
    </div>
</div>
 



<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="idd" runat="server" />
<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
</asp:Content>


