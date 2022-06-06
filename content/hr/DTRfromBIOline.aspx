<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DTRfromBIOline.aspx.cs" Inherits="content_hr_DTRfromBIOline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DTR</title>
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
input[type=text]{     width: 500px;
padding: 8px;
margin: 8px 0;
display: inline-block;
border: 1px solid #ccc;
border-radius: 4px;
box-sizing: border-box;
}
 
input[type=submit] 
{
    background-color: #4CAF50;
    color: white;
    padding: 9px 20px;
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
.Grid th {padding : 10px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
.Grid .alt {background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }
.Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }
.Grid .pgr table { margin: 3px 0; }
.Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  
.Grid .pgr a { color: Gray; text-decoration: none; }
.Grid .pgr a:hover{ color: #000; text-decoration: none; }
.hiddencol { display: none; }
.PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:5%;z-index:2001; padding:20px;min-width:1200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="search" Text="Search" CssClass="btn" />
    <div>
        <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                    <Columns>
                        <asp:BoundField DataField="idnumber" HeaderText="ID Number"/>
                        <asp:BoundField DataField="e_name" HeaderText="Employee"/>
                        <asp:BoundField DataField="Date_Time" HeaderText="DateTime"/>
                    </Columns>
                </asp:GridView>
    </div>
     <asp:HiddenField ID="key" runat="server" />
   <asp:HiddenField ID="pg" runat="server" />
    <asp:HiddenField ID="logsid" runat="server" />
    </form>
</body>
</html>
