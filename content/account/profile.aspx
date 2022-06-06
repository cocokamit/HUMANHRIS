<%@ Page Language="C#" AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="content_account_profile" MasterPageFile="~/content/MasterPageNew.master" %>
 
 <asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
 <script type="text/javascript">
     $(window).load(function () {
         setTimeout(function () { $('.alert').fadeOut('slow') }, 5000);

         base64 = $("#dataURLInto").val();
         if (base64 != "")
             $("#dataURLView").html('<img class="img-responsive avatar-view"  src="' + base64 + '" title="Profile" style=" width:100%">');
     });
</script>
 </asp:Content>
 <asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
 <div class="page-title">
    <div class="title_left hd-tl">
        <h3>Account Profile</h3>
    </div>  
    <div class="title_right">
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div id="pgridview" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridview1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ximage" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <ul class="input-form">
            <li><label>Current Password</label></li>
            <li><asp:TextBox ID="txtoldpass" TextMode="Password" AutoComplete="off" runat="server"></asp:TextBox></li>
            <li><label>New Password</label></li>
            <li><asp:TextBox ID="txtnewpass" TextMode="Password" runat="server"></asp:TextBox></li>
            <li><label>Confirm Password</label></li>
            <li><asp:TextBox ID="txtconpass" TextMode="Password" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnupdate" runat="server" OnClick="updatepassword" Text="Update" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
<div class="col-md-12">
    <div class="x_panel">
    <div class="container cropper">
        <div class="col-md-5">
          <div class="profile_img img-hover center">
            <div id="dataURLView">
                <img class="img-responsive avatar-view" src="files/adminprofile/<%= profile.Value %>.png" title="Profile" style=" width:100%" />
            </div>
            <div class="middle" style="position:absolute; margin-top:-20px; margin-left:5px">
                <div class="docs-toolbar">
                    <label for="inputImage" title="Upload image file">
                        <input class="hide" id="inputImage" name="file" type="file" accept="image/*">
                        <i class="fa fa-camera text-white" style="color:#fff !important"></i>
                    </label>
                </div>
            </div>
          </div>
        </div>
    </div>
      <div class="x_panel">
        <asp:Button ID="btn_profile" runat="server" OnClick="changeprofile" CssClass="btn btn-primary" ClientIDMode="Static" Text="Change Profile" />
        <asp:Button ID="btnview" runat="server" OnClick="viewupdate" CssClass="btn btn-primary" ClientIDMode="Static" Text="Change Password" />
      </div>
    </div>
</div>
</div>


<div class="modal fade in" id="modal-profile" >
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button id="close-modalprofile" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Change Profile</h4>
            </div>
            <div class="modal-body">
                <div class="img-container"><img src="vendors/crop image/img/picture-1.jpg"></div>
                <div class="row docs-data-url none">
                <textarea class="form-control" id="dataURLInto" runat="server" rows="8" clientidmode="Static"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-primary" id="getDataURL2" data-toggle="tooltip" type="button" title="$().cropper(&quot;getDataURL&quot;, &quot;image/jpeg&quot;)">Change Profile</button>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="profile" runat="server" Value="0"/>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!-- Cropper -->
<script src="vendors/crop image/js/jquery-1.12.4.min.js"></script>
<script src="vendors/crop image/js/cropper.min.js"></script>
<script src="vendors/crop image/js/main.js"></script>
<link href="vendors/crop image/css/cropper.min.css" rel="stylesheet">
<link href="vendors/crop image/css/main.css" rel="stylesheet">
</asp:Content>
