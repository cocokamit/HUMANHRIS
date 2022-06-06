<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Approval.aspx.cs" Inherits="content_hr_Approval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HUMAN | Leave Approval</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="../../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../../style/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    
    <style type="text/css">
        hr { margin:10px 0}
        table { width:100%; margin-top:0px}
        table, tr, td, th {border:none}
        table td { line-height:30px}
        textarea{ width:100%; border:1px dashed #bfbcbc; padding:10px;}
        .row {margin-right:0; margin-left:0}
        .box {border:1px solid #eee; width:95%; margin:20px auto; border-radius:3px; padding:10px 20px }
        .form-control {border-radius:0;-webkit-box-shadow:none; box-shadow:none;-o-transition:none; transition:none}
    </style>
     <script type="text/javascript">
         function Confirm() {
             var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
             if (confirm("Are you sure you want to disapproved this application?"))
             { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
         } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="box">
        <div class="row">
            <h3><i class="fa fa-calendar"></i> LEAVE APPROVAL</h3>    
            <hr />
        </div>
<asp:Label ID="lbl_del_notify" runat="server" style=" position:absolute; right:0; margin:15px; font-size:11px;display:none;" ></asp:Label>

<asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" ShowHeader="false">
    <Columns>
        <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
        <asp:TemplateField>
            <ItemTemplate>
                 <asp:Label ID="l_df" runat="server" CssClass="pull-right" Text='<%# Eval("sysdate") %>' style=" font-weight:bold; margin-top:0px" />   
                <div class="form-group">
	                <label>Leave Type</label>
                    <span class="form-control"><%# Eval("Leave") %></span>
                </div>
                <div class="form-group">
	                <label>Employee Name</label>
	                <span class="form-control"><%# Eval("fullname")%></span>
                </div>
                <div class="form-group">
	                <label>Date Leave</label>
	                <span class="form-control"><%# Eval("leavefrom")%> - <%# Eval("leaveto")%></span>
                </div>
                <div class="form-group">
	                <label>Reason</label>
	                <span class="form-control"><%# Eval("remarks")%></span>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:TextBox ID="txt_remarks" TextMode="MultiLine" runat="server" placeholder="Remarks..." Rows="5"></asp:TextBox>
<hr />
<asp:Button ID="Button1" OnClick="approved" runat="server" Text="Approve" CssClass="btn btn-primary"/>
<asp:Button ID="Button2" OnClick="delete3" runat="server"  OnClientClick="Confirm()" Text="Cancel" CssClass="btn btn-danger"/>

 <asp:HiddenField ID="TextBox1" runat="server" />
        <asp:HiddenField ID="id" runat="server" />
    </div>
    </form>
</body>
</html>
