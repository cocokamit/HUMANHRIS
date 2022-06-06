<%@ Page Language="C#" AutoEventWireup="true" CodeFile="request4budget.aspx.cs" Inherits="content_printable_request4budget" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget</title>
    <style type="text/css">
        *{margin:0; padding:0; font-family:"Lucida Sans Unicode", "Lucida Grande", sans-serif;}
        .content { min-width:1200px; width:70%; margin:0 auto; font-size:14px }
        .slip {border:1px solid #eee; padding:40px; margin:20px;}
        table { text-align:left; width:100%; margin:10px 0 0}
        table th { text-transform:uppercase}
        table tr { vertical-align:top}
        .Grid td,.Grid th {padding:2px 4px}
        .style1
        {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">

    <table>
        <tr>
            <td>Company</td>
            <td>:</td>
            <td><asp:Label ID="lbl_comapany" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Company Address</td>
            <td>:</td>
            <td><asp:Label ID="lbl_compadress" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Period</td>
            <td>:</td>
            <td><asp:Label ID="lbl_period" runat="server" ></asp:Label></td>
        </tr>
    </table>
    <asp:GridView ID="grid_item" runat="server"  ShowFooter="True"  OnRowDataBound="rowdatabound" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                <Columns>
                  
                     <asp:BoundField DataField="IdNumber" HeaderText="Id Number" />
                     <asp:BoundField DataField="c_name" HeaderText="Employee Name" />
                     <asp:BoundField DataField="Company" HeaderText="Company" />
                     <asp:BoundField DataField="Department" HeaderText="Department" />
                     <asp:BoundField DataField="Position" HeaderText="Position" />
                      <asp:TemplateField HeaderText="Overtime Pay">
                        <ItemTemplate>
                            <asp:Label ID="lbl_totalot" runat="server" Text='<%# bind("totalot") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_footertotalot" ForeColor="Red" runat="server"></asp:Label>
                        </FooterTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Restday Pay">
                        <ItemTemplate>
                            <asp:Label ID="lbl_totalrdpay" runat="server" Text='<%# bind("totalrdpay") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_footertotalrdpay" ForeColor="Red" runat="server"></asp:Label>
                        </FooterTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Leave w/ Pay">
                        <ItemTemplate>
                            <asp:Label ID="lbl_totalleavepay" runat="server" Text='<%# bind("totalleavepay") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_footertotalleavepay" ForeColor="Red" runat="server"></asp:Label>
                        </FooterTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Holiday Pay">
                        <ItemTemplate>
                            <asp:Label ID="lbl_totalhdpay" runat="server" Text='<%# bind("totalhdpay") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_footertotalhdpay" ForeColor="Red" runat="server"></asp:Label>
                        </FooterTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Net Income">
                        <ItemTemplate>
                            <asp:Label ID="lbl_NetIncome" runat="server" Text='<%# bind("NetIncome") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_footerNetIncome" ForeColor="Red" runat="server"></asp:Label>
                        </FooterTemplate>
                      </asp:TemplateField>
                  </Columns>
            </asp:GridView>
  
    </div>
    </form>
</body>
</html>
