<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchabledropdownlist.aspx.cs" Inherits="searchabledropdownlist" %>

<html>
<head>
    <script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
    <script src="style/js/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable({
                maxListSize: 200, // if list size are less than maxListSize, show them all
                maxMultiMatch: 300, // how many matching entries should be displayed
                exactMatch: false, // Exact matching on search
                wildcards: true, // Support for wildcard characters (*, ?)
                ignoreCase: true, // Ignore case sensitivity
                latency: 200, // how many millis to wait until starting search
                warnMultiMatch: 'top {0} matches ...',
                warnNoMatch: 'no matches ...',
                zIndex: 'auto'
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:DropDownList ID="myselect" runat="server">
            <asp:ListItem>Select</asp:ListItem>
            <asp:ListItem>venki</asp:ListItem>
            <asp:ListItem>venu</asp:ListItem>
            <asp:ListItem>charles ven</asp:ListItem>
            <asp:ListItem>venuzila</asp:ListItem>
            <asp:ListItem>veron philender</asp:ListItem>
            <asp:ListItem>india</asp:ListItem>
            <asp:ListItem>indianven</asp:ListItem>
            <asp:ListItem>vesta</asp:ListItem>
        </asp:DropDownList>
    </form>
</body>
</html>
