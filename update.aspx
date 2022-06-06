<%@ Page Language="C#" AutoEventWireup="true" CodeFile="update.aspx.cs" Inherits="update" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="style/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <link href="style/css/custom.min.css" rel="stylesheet">
    <link href="vendors/nprogress/nprogress.css" rel="stylesheet">
<head id="Head1" runat="server">
    <title>Orley Gwapo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txt_remarks" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:FileUpload ID="FileUpload2" runat="server" accept=".xlsx, .xls, .csv" CssClass="btn btn-primary"/><br />
        <asp:Button ID="Button3" OnClick="loadupdate"  runat="server" Text="Load File Selected" CssClass="btn btn-primary"/>
        <asp:Button ID="Button1" Visible="false" OnClick="updatetable" runat="server" Text="Update Records" CssClass="btn btn-primary"/>
        <asp:GridView ID="GridView2" runat="server"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="IdNumber" HeaderText="IDNumber" />
                <asp:BoundField DataField="OldRate" HeaderText="OldRate" />
            </Columns>
          </asp:GridView>
    </div>
    </form>
</body>
</html>
