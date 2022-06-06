<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailsperpayroll.aspx.cs" Inherits="content_payroll_detailsperpayroll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
#nav,#nav1 
{
 background: gray url(style/images/nav_bkg.jpg) repeat-x 0% 0%; float: left; list-style: none; margin: 0; padding: 0; width: 100%; 
}
#nav,#nav1 li 
{
 float: left; font-size: 11px; margin: 0; padding: 1; text-transform: uppercase; 
}
#nav1 li a 
{
 border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 0px 8px; text-decoration: none; 
}
#nav li a {
 border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 10px 16px; text-decoration: none; 
}
#nav li a:hover {
 background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; 
}
.click{background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; }
.PopUpPanel input[type=text]
{
    width: 200px;
    padding: 6px 20px;
    margin: 8px 0;
    display: inline-block;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
}
select {

    width: 150px;
    padding: 6px 20px;
    margin: 8px 0;
    display: inline-block;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
}
.btn input[type=submit] 
{
    width: 100%;
    background-color: #4CAF50;
    color: white;
    padding: 9px 30px;
    margin: 8px 0;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}
.btn input[type=submit]:hover 
{
    background-color: #45a049;
}
.Grid { background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; font-family:Calibri; color: #474747; width: 100%; font-size: 12px; text-align: center;}
.Grid td {padding: 2px; border: solid 1px #c1c1c1; }
.Grid th {padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
.Grid .alt {background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }
.Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }
.Grid .pgr table { margin: 3px 0; }
.Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  
.Grid .pgr a { color: Gray; text-decoration: none; }
.Grid .pgr a:hover{ color: #000; text-decoration: none; }
.hiddencol { display: none; }
.PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:5%;z-index:2001; padding:20px;min-width:1200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
</style>
<script type="text/javascript" src="script/freezeheadergv/freazehdgv.js" ></script>
     
     <script type="text/javascript" src="script/freezeheadergv/index.js" ></script>
    <script type="text/javascript">
        $(document).ready(function () {
            freaze("grid_view", "container");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <table>
    <tr>
        <td>
             <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
        </td>
        <td>
             <asp:Button ID="Button1" runat="server" OnClick="search" Text="Search" />
        </td>
    </tr>
    </table>

    <div>
       <div id="container" style="overflow: scroll; overflow-x: hidden;">
        </div>
        <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="EmployeeId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                        <asp:BoundField DataField="PayrollDate" HeaderText="Payroll Date"/>
                        <asp:BoundField DataField="datedtrrange" HeaderText="DTR Range"/>
                        <asp:BoundField DataField="IdNumber" HeaderText="IdNumber"/>
                        <asp:BoundField DataField="c_name" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="payrolltype" HeaderText="Payroll Type"/>
                        <asp:BoundField DataField="taxcode" HeaderText="Tax Code"/>
                        <asp:BoundField DataField="netincome" HeaderText="Net Income"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Click to view payslip."  Text="view" OnClick="viewpayslip"  style=" margin:3px;" ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        <asp:HiddenField ID="payid" runat="server" />
    </div>
    </form>
</body>
</html>
