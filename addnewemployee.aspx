<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addnewemployee.aspx.cs" Inherits="addnewemployee" %>
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
    <title>HRIS</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
        <asp:FileUpload ID="FileUpload1" Visible="false" runat="server" accept=".xlsx, .xls, .csv" CssClass="btn btn-primary"/><br />
        <asp:Button ID="Button1" Visible="false" OnClick="loademployee"  runat="server" Text="Go" CssClass="btn btn-primary"/>
        <asp:Button ID="btnback" Visible="false" OnClick="back" runat="server" Text="Back" CssClass="btn btn-primary"/>
        <asp:GridView ID="GridView1" Visible="false" runat="server"  AutoGenerateColumns="false" OnRowEditing="rowediting" OnRowDeleting="rowdeleterow" CssClass="table table-striped table-bordered" OnRowDataBound="kanakanaas">
            <Columns>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" Text="Edit" runat="server" CommandName="Edit"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton2" Text="Update" runat="server" OnClick="rowupdate"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton3" Text="Cancel" runat="server" OnClick="rowcancel"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IdNumber" HeaderText="IDNumber" />
                <asp:BoundField DataField="SAPNumber" HeaderText="SAP Number" />
                <asp:BoundField DataField="Biometric" HeaderText="BiometricIdNumber" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" />
                <asp:BoundField DataField="ExtensionName" HeaderText="ExtensionName" />
                <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" />
                <asp:BoundField DataField="BirthPlace" HeaderText="Birthplace" />
                <asp:BoundField DataField="ZipCode" HeaderText="ZipCode" />
                <asp:BoundField DataField="BloodType" HeaderText="Blood_Type" />
                <asp:BoundField DataField="CellphoneNumber" HeaderText="CellphoneNumber" />
                <asp:BoundField DataField="Citizenship" HeaderText="Citizenship" />
                <asp:BoundField DataField="Civilstatus" HeaderText="CivilStatus" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" />
                <asp:BoundField DataField="SSSNumber" HeaderText="SSSNumber" />
                <asp:BoundField DataField="HDMFNumber" HeaderText="HDMFNumber" />
                <asp:BoundField DataField="PHICNumber" HeaderText="PHICNumber" />
                <asp:BoundField DataField="TINNumber" HeaderText="TINNumber" />
                <asp:BoundField DataField="TelephoneNumber" HeaderText="TelephoneNumber" />
                <asp:BoundField DataField="PresentAddress" HeaderText="Address" />
                <asp:BoundField DataField="PermanentAddress" HeaderText="PermanentAddress" />
                <asp:BoundField DataField="Religion" HeaderText="Relegion" />
                <asp:BoundField DataField="Height" HeaderText="Height" />
                <asp:BoundField DataField="Weight" HeaderText="Weight" />
                <asp:BoundField DataField="Branch" HeaderText="Branch" />
                <asp:BoundField DataField="HireDate" HeaderText="DareHired" />
                <asp:BoundField DataField="Department" HeaderText="Department" />
                <asp:BoundField DataField="Taxcode" HeaderText="Taxcode" />
                <asp:BoundField DataField="BandLevel" HeaderText="BandLevel" />
                <asp:BoundField DataField="Level" HeaderText="Level" />
                <asp:BoundField DataField="Status" HeaderText="EmployeeStatus" />
                <asp:BoundField DataField="Position" HeaderText="Position" />
                <asp:BoundField DataField="Account" HeaderText="ChartOfAccount" />
                <asp:BoundField DataField="Company" HeaderText="Company" />
                <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" />
                <asp:BoundField DataField="InternalOrder" HeaderText="InternalOrder" />
                <asp:BoundField DataField="MonthlyRate" HeaderText="MonthlyRate" />
                <asp:BoundField DataField="PayrollRate" HeaderText="PayrollRate" />
                <asp:BoundField DataField="DailyRate" HeaderText="DailyRate" />
                <asp:BoundField DataField="HourlyRate" HeaderText="HourlyRate" />
                <asp:BoundField DataField="Payrollgroup" HeaderText="Payrollgroup" />
                <asp:BoundField DataField="Payrolltype" HeaderText="Payrolltype" />
                <asp:BoundField DataField="Role" HeaderText="Role" />
                <asp:BoundField DataField="Section" HeaderText="Section" />
                <asp:BoundField DataField="Shiftcode" HeaderText="ShiftCode" />
                <asp:TemplateField ItemStyle-CssClass="none" HeaderStyle-CssClass="none">
                 <ItemTemplate>
                       <asp:LinkButton ID="lbnremoverows" Onclick="rowdeleting" Text="Delete" runat="server"><i class="fa fa-trash"></i></asp:LinkButton>
                 </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:GridView ID="gridcompanydistinct" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="Company" HeaderText="Company" />
          </Columns>
        </asp:GridView>
        <asp:GridView ID="gridggroup" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="PayrollGroup" HeaderText="Payrollgroup" />
          </Columns>
        </asp:GridView>
        <asp:GridView ID="gridbranch" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="Branch" HeaderText="Branch" />
          </Columns>
        </asp:GridView>
        <asp:GridView ID="griddepartment" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="Department" HeaderText="Department" />
          </Columns>
        </asp:GridView>
        <asp:GridView ID="gridposition" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="Position" HeaderText="Position" />
          </Columns>
        </asp:GridView>
       <%-- <asp:GridView ID="gridshiftcode" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
          <Columns>
            <asp:BoundField DataField="ShiftCode" HeaderText="ShiftCode" />
          </Columns>
        </asp:GridView>--%>
        <asp:GridView ID="gridsction" Visible="false" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
         <Columns>
            <asp:BoundField DataField="Sectioncode" HeaderText="Section Code"/>
         </Columns>
        </asp:GridView>
        <br />
        <asp:Button ID="Button2" Visible="false" OnClick="loadtemplate" CssClass="btn btn-primary" runat="server" Text="Load Template" />
        <asp:Button ID="Button4" runat="server" Visible="false" OnClick="saveselectdistinct" CssClass="btn btn-primary" Text="Save Selected Distinct" />
    </div>
     <asp:HiddenField ID="hdn_empid" runat="server" />
    </form>
</body>
</html>
