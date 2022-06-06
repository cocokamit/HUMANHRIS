<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>HRIS</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="stylesheet" href="vendors/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="vendors/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="dist/css/content.css" />
    <link rel="stylesheet" href="dist/css/login.css" />
    <script type="text/javascript">
        window.history.forward();
        function noBack() {
            window.history.forward();
       }
    </script>
  
</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="" background-repeat:no-repeat;background-attachment:fixed; background-position:center" >
    <form id="form1" runat="server">
        <div class="login-box">
          <div class="login-box-body">
           <img src="dist/img/Human.png" class="login-logo" />
              <div class="form-group has-feedback">
                <asp:TextBox ID="txt_user" runat="server" CssClass="form-control" placeholder="Username" required></asp:TextBox>
                <span class="glyphicon glyphicon-user form-control-feedback"></span>
              </div>
              <div class="form-group has-feedback">
                <asp:TextBox ID="txt_pass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" required></asp:TextBox>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
              </div>
              <div class="row">
                <div class="col-xs-12 text-center">
                 <asp:Button ID="btn_login" runat="server" OnClick="click_go" class="btn btn-primary btn-block btn-flat" Text="Sign In"/>
              
                 <asp:Button ID="Button1" runat="server" OnClick="click_convert" class="btn btn-primary btn-block btn-flat hidden" Text="Convert to pdf"/>
                  <asp:Button ID="Button2" runat="server" OnClick="click_login" class="btn btn-primary btn-block btn-flat hidden" Text="New Sign In"/>
                 <asp:Label ID="lbl_msg" runat="server" Visible="false" ForeColor="Red" CssClass="label" style=" font-style:italic; display:block; padding-top:15px"></asp:Label>
                <div class="poweredby">
                 <asp:Label ID="Label1" runat="server" ForeColor="Gray" CssClass="label" Text="powered by " style=" font-style:italic; display:block; padding-top:15px"></asp:Label>
                 </div>
                </div>
              </div>
          </div>
            <div class="powered">
                <a href="http://meshnetworksinc.com/"><img src="style/img/hris.PNG" class="login-logo" /></a>
            </div>
        </div>
    </form>
</body>

</html>
