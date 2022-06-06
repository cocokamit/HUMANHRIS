<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_password.aspx.cs" Inherits="content_account_change_password" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="content" ID="content_change" runat="server">

 <script type="text/javascript">
     var specialKeys = new Array();
     specialKeys.push(8); //Backspace
     specialKeys.push(9); //Tab
     specialKeys.push(46); //Delete
     specialKeys.push(36); //Home
     specialKeys.push(35); //End
     specialKeys.push(37); //Left
     specialKeys.push(39); //Right
     function IsAlphaNumeric(e) {
         var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
         var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
         document.getElementById("error").style.display = ret ? "none" : "inline";
         return ret;
     }
    </script>

<section class="content-header">
    <h1>Change Password</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Change Password</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
               <div class="box-body">
                <div class="form-group" runat="server" id="meals">
                    <label>Current Password</label>
                     <asp:TextBox ID="txt_cur" TextMode="Password" CssClass="nobel form-control" runat="server"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="Div1">
                    <label>New Password</label><span class="text-red"> (Maximum of 6 Character only)</span>
                    <asp:TextBox ID="txt_new" TextMode="Password" CssClass="nobel form-control" runat="server"></asp:TextBox>
                    <%--<asp:TextBox ID="txt_new" MaxLength="6" onkeypress="return IsAlphaNumeric(event);" TextMode="Password" CssClass="nobel form-control" runat="server"></asp:TextBox>--%>
                </div>
                <div class="form-group no-margin" runat="server" id="Div2">
                    <label>Confirm New Password</label>
                      <asp:TextBox ID="txt_confirm" TextMode="Password" CssClass="nobel form-control" runat="server"></asp:TextBox>
                      <%--<asp:TextBox ID="txt_confirm" MaxLength="6" onkeypress="return IsAlphaNumeric(event);" TextMode="Password" CssClass="nobel form-control" runat="server"></asp:TextBox>
                      <span id="error" style="color: Red; display: none">* Special Characters Are Not Allowed!</span>--%>
                </div>
             </div>
              <div class="box-footer pad">
                <asp:Button ID="btn_save" runat="server" Text="Save" onclick="save" CssClass="btn btn-primary"/>
                <asp:Label ID="Label1" style=" color:Red; font-size:12px;" runat="server" Text=""></asp:Label></td>
              </div>
          </div>
        </div>
    </div>
</section>


<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>