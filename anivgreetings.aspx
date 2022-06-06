<%@ Page Language="C#" AutoEventWireup="true" CodeFile="anivgreetings.aspx.cs" Inherits="anivgreetings" %>

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
    <div class="row">
        <div class="col-md-12" style=" text-align:center;">
            <img src="style/img/EOYCebuLandmasters100617.png" height="100px" width="350px" style="margin-top:40px;" class="login-logo" />
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-4" style=" text-align:center;">
                    <img src="style/img/cliclipsmileys.png" height="380px" width="310px"
                        class="login-logo" style="margin-top:20px; margin-left:40px;" />
                </div>
                <div class="col-md-8" style="padding:auto;">
                    <div class="x_title" style="margin-top: 40px; margin-left: 50px; font-size:24px; word-spacing:10px;">
                        <p>
                            Hi <a href="homepage2"><%= ViewState["names"]%></a>, you turned <%= ViewState["year"]%> <%= ViewState["ones"]%> TODAY!</p>
                        <p>
                            Congratulations on your <strong style="font-size:40px;">work anniversary!</strong></p>
                        <p>
                            We appreciate your energy, your resilience and</p>
                        <p>
                            all the <strong style="font-size:40px;">work</strong> you do, but most of all, we just</p>
                        <p>
                            appreciate you!</p>
                        <br />
                        <br />
                        <p>
                            From your CLI Family</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
