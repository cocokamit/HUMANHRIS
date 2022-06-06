<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="crop_image_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/cropper.min.css" rel="stylesheet">
    <link href="css/main.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
     <div class="jumbotron docs-jumbotron">
        <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
     </div>

  <div>
        <div class="img-container"><img src="img/picture-1.jpg"></div>
        <div class="docs-toolbar">
          <div class="btn-group">
             <label class="btn btn-primary" for="inputImage" title="Upload image file">
              <input class="hide" id="inputImage" name="file" type="file" accept="image/*">
              <span class="docs-tooltip" data-toggle="tooltip" title="Import image with Blob URLs">
                <span class="glyphicon glyphicon-upload"></span>
              </span>
            </label>
          </div>
        </div>
   
 
  
      
     

          <div class="row">
            
            <div class="col-sm-6 col-md-6">
              <div class="input-group">
                <span class="input-group-btn">
                  <button class="btn btn-primary" id="setAspectRatio" type="button">Set Aspect Ratio</button>
                </span>
                <input class="form-control" id="setAspectRatioWith" type="text" value="1" placeholder="Input aspect ratio">
              </div>
            </div>
          </div>
 
          <div class="row docs-data-url">
            <div class="col-sm-12">
              <button class="btn btn-primary" id="getDataURL2" data-toggle="tooltip" type="button" title="$().cropper(&quot;getDataURL&quot;, &quot;image/jpeg&quot;)">Get Data URL (JPG)</button>
            </div>
            <div class="col-sm-6">
              <textarea class="form-control" id="dataURLInto" runat="server" rows="8"></textarea>
            </div>
            <div class="col-sm-6">
              <div id="dataURLView"></div>
            </div>
          </div>
    
    
  

  </div>

  <script src="js/jquery-1.12.4.min.js"></script>
  <script src="js/cropper.min.js"></script>
  <script src="js/main.js"></script>
    </form>
</body>
</html>
