<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>
 

 <!DOCTYPE html> 
<html> 
<head> 
<meta name="viewport" content="initial-scale=1.0, user-scalable=no"/> 
<meta http-equiv="content-type" content="text/html; charset=UTF-8"/> 
<title>Reverse Geocoding</title> 

<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script> 
<script type="text/javascript">
    var geocoder;
    $.getJSON("https://api.bigdatacloud.net/data/reverse-geocode-client?latitude=10.342762&longitude=123.919051&localityLanguage=en")
        .success(function (data) {
            console.log(data);
        })
        .error(function (error) {
            $("#output").html("An error occurred.");
        })
</script> 
</head> 
<body onload="initialize()"> 

</body> 
</html> 