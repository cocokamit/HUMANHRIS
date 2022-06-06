<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travel_form.aspx.cs" Inherits="content_printable_travel_form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>REQUEST TRAVEL FORM</h3><br />

        Name: <asp:Label ID="txt_name" runat="server"></asp:Label>
        <br />

        Date:<asp:Label ID="txt_date" runat="server"></asp:Label>
        <br />

        Travel place: 
          <asp:GridView ID="grid_destinations" runat="server" AutoGenerateColumns="false" CssClass="table">
                            <Columns>
                            <asp:BoundField DataField="id"/>
                            <asp:BoundField DataField="place" HeaderText="Place"/>
                            <asp:BoundField DataField="travel_mode" HeaderText="Travel Mode"/>
                            <asp:BoundField DataField="arrange_type" HeaderText="Arrangement Type"/>
                            </Columns>
                    </asp:GridView>
        <br />

        Purpose: <asp:Label ID="txt_purpose" runat="server"></asp:Label>
        <br />

        <br />
        Meals: <asp:Label ID="txt_meals" runat="server"></asp:Label>
        <br />
        Transportation: <asp:Label ID="txt_transpo" runat="server"></asp:Label>
        <br />
        Accommodation: <asp:Label ID="txt_accom" runat="server"></asp:Label>
        <br />
        Other Expense: <asp:Label ID="txt_other" runat="server"></asp:Label>
        <br />
        Total cash approved: <asp:Label ID="txt_total" runat="server"></asp:Label>
        <br />

        <br />
        <br />

    
    </div>
    </form>
</body>
</html>
