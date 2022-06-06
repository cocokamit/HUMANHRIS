<%@ Page Language="C#" AutoEventWireup="true" CodeFile="transotherincome.aspx.cs" Inherits="content_payroll_transotherincome" MasterPageFile="~/content/MasterPageNew.master" %>

<%@ Import Namespace="System.Data" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="content_head_income">
<script type="text/javascript" src="script/gridviewpane/gridpane.js"></script>

    <script type="text/javascript">
        $(function () {
            $("[id*=imgOrdersShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgProductsShow]").each(function () {
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
    <style type="text/css">
        .table-bordered tbody > tr > td{ vertical-align:top}
         .hiddencol { display: none; }
         .aspNetDisabled {color: Silver}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_other_income">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Other Income</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
<%
    
    string id = Session["role"].ToString() == "Admin" ? "0" : Session["emp_id"].ToString();
    DataTable dt = dbhelper.getdata("Select * from nobel_userRight a left join nobel_route b on a.route_id=b.id  where a.[read]='1' and a.[update]='1' and a.user_id=" + id);
    string url = HttpContext.Current.Request.Url.AbsolutePath.Replace("/", "");
    DataRow[] dr = dt.Select(" url='" + url + "'");
%>
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
             <div class="x-head">
                <asp:DropDownList ID="ddl_payroll_group" runat="server" CssClass="minimal" AutoPostBack="true" OnTextChanged="ChangeGroup"></asp:DropDownList>
                <asp:TextBox ID="txt_f"  type="date" runat="server" placeholder="From" CssClass="none"></asp:TextBox>
                <asp:TextBox ID="txt_t" type="date" runat="server" placeholder="To" CssClass="none"></asp:TextBox>
                <asp:Button ID="Button2"  runat="server" onclick="btn_search_Click"  Text="Search" CssClass="btn btn-primary none"/>
               
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_add_other" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
                 
             </div>


              <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
        <asp:GridView ID="grid_view1" runat="server" OnRowDataBound ="OnRowDataBound"  AutoGenerateColumns="false" EmptyDataText="No Data Found"  CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="payroll_id" HeaderText="payroll_id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="action" HeaderText="action" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="entrydate" HeaderText="Entry Date"/>
                 <asp:BoundField DataField="period" HeaderText="Cutoff"/>
                <asp:BoundField DataField="pg" HeaderText="Payroll Group" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" CssClass="fa "></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton3" runat="server"  ToolTip="Click to view transaction." OnClick="view"  ><i class="fa fa-clipboard sm"></i></asp:LinkButton><%--OnClick="cancel_tran"OnClick="view" --%>
                   <% 
                            string id = Session["role"].ToString() == "Admin" ? "0" : Session["emp_id"].ToString();
                            DataTable dt = dbhelper.getdata("Select * from nobel_userRight a left join nobel_route b on a.route_id=b.id  where a.[read]='1' and a.[update]='1' and a.user_id=" + id);
                            string url = HttpContext.Current.Request.Url.AbsolutePath.Replace("/", "");
                            DataRow[] dr = dt.Select(" url='" + url + "'");
                            if (Session["emp_id"].ToString() == "0")
                        { %>
                        <asp:LinkButton ID="LinkButton2" runat="server"  ToolTip="Click to cancel transaction." OnClick="click_cancel" OnClientClick="Confirm()"><i class="fa fa-trash-o"></i></asp:LinkButton><%-- OnClientClick="Confirm()"--%>
                        <%} %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
      </div>
    </div>
   <%-- <ul id="nav1">
    <li><a style=" border:none; color:Black"><asp:DropDownList ID="ddl_payroll_group"  runat="server"></asp:DropDownList></a></li>
    <li><asp:TextBox ID="txt_search" placeholder="Employee Name"  runat="server"></asp:TextBox></li>
    <li class="btn" style=" float:right;"><a style=" border:none;  color:Black"><asp:Button ID="Button2" runat="server" Text="ADD" OnClick="click_add_other"/></a></li>
    </ul>--%>

   <%-- <asp:Button ID="btn_search" runat="server" Text="Search" onclick="btn_search_Click" />
        <asp:GridView ID="grid_view1" runat="server" AutoGenerateColumns="false" CssClass="Grid">
            <Columns>
             <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="imgOrdersShow" runat="server" OnClick="Show_Hide_OrdersGrid" ImageUrl="~/style/img/add.png" ToolTip="Click view the details!" CommandArgument="Show" />
                    <asp:Panel ID="pnlOrders" runat="server" Visible="false" Style="position: relative">
                    <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                        <Columns>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                            <asp:BoundField DataField="idd" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                            <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                            <asp:BoundField DataField="OtherIncome" HeaderText="Income"/>
                            <asp:BoundField DataField="Amount" HeaderText="Amount"/>
                            <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                <asp:BoundField DataField="entrydate" HeaderText="Entry Date"/>
                <asp:BoundField DataField="pg" HeaderText="Payroll Group"/>
                <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
            </Columns>
        </asp:GridView>--%>


   <asp:HiddenField ID="TextBox1" runat="server" />
   <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="id" runat="server" />
   <asp:HiddenField ID="idd" runat="server" />
</asp:Content>

