<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BIR1601-C.aspx.cs" Inherits="content_printable_BIR1601_C" %>
<%@ Import Namespace="System.Data"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>1601-C</title>
    <style type="text/css">
          span{ color:#000; letter-spacing:5px; position:absolute}
            .wrapper{width:797px; 	font-family: Calibri;}
            .wrapper img { width:100%;}
            .row { position:absolute; width:100%;}
            .a {margin:145px 0}
            .year {margin: 0 226px;}
            .periodfrom {margin-left:673px}
        </style>
        <style type="text/css" media="print">
            .wrapper{ width:100%  }
        </style>
</head>
<body>
    <% DataTable dtalphalist = getdata.alphalist1601C("a.id=1");  %>
    <%
        string[] datas = Session["alldata"] as string[];
     %>
		<div class="wrapper">

            <%--1--%>
            <%if (dtalphalist.Rows[0]["mm"].ToString().Length == 1)
              {%>
              <div style="margin:136px 162px; letter-spacing:11px; position:absolute; font-size:14px;"><% Response.Write("0"); %></div>
              <div style="margin:136px 181px; letter-spacing:11px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["mm"].ToString()); %></div>
            <%}
              else
              { %>
              <div style="margin:136px 162px; letter-spacing:11px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["mm"].ToString()); %></div>
            <%} %>

            <div style="margin:136px 205px; letter-spacing:11px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["yyyy"].ToString()); %></div>
            <%--2--%>

            <%if (dtalphalist.Rows[0]["amendedreturn"].ToString() == "False")
              {%>
              <div style="margin:136px 401px; letter-spacing:6px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%}
              else
              { %>
              <div style="margin:136px 340px; letter-spacing:6px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%} %>

            <%--4--%>
            <%if (dtalphalist.Rows[0]["taxedwithheld"].ToString() == "False")
              {%>
              <div style="margin:136px 713px; letter-spacing:6px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%}
              else
              { %>
              <div style="margin:136px 653px; letter-spacing:6px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%} %>

            <%--5--%>
            <div style="margin:175px 75px; letter-spacing:11px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["companytin"].ToString()+"-000"); %></div>
            <%--6--%>
            <div style="margin:175px 444px; letter-spacing:6px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["rdocode"].ToString()); %></div>
            <%--8--%>
            <div style="margin:210px 69px; letter-spacing:0px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["companyname"].ToString()); %></div>
            <%--9--%>
            <div style="margin:210px 657px; letter-spacing:9px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["companynumber"].ToString()); %></div>
            <%--10--%>
            <div style="margin:246px 69px; letter-spacing:0px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["companyaddress"].ToString()); %></div>
            <%--11--%>
            <div style="margin:246px 690px; letter-spacing:12px; position:absolute; font-size:14px;"><% Response.Write(dtalphalist.Rows[0]["companycode"].ToString()); %></div>
            <%--12--%>
            <%if (dtalphalist.Rows[0]["categoryofwithholdagent"].ToString() == "Private")
              {%>
              <div style="margin:281px 70px; letter-spacing:9px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%}
              else
              { %>
              <div style="margin:281px 132px; letter-spacing:9px; position:absolute; font-size:14px;"><% Response.Write("X"); %></div>
            <%} %>

            <%--13--%>
            <%if (dtalphalist.Rows[0]["internaltaxtreaty"].ToString() == "False")
              {%>
              <div style="margin:284px 312px; letter-spacing:9px; position:absolute; font-size:14px;"><% Response.Write("x"); %></div>
            <%}
              else
              { %>
              <div style="margin:284px 248px; letter-spacing:9px; position:absolute; font-size:14px;"><% Response.Write("x"); %></div>
            <%} %>
            
            <%--15--%>
            <div style="margin:334px 367px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totalgross"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--16C--%>
            <div style="margin:407px 367px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totalntaxable"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--17--%>
            <div style="margin:430px 367px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totaltaxable"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--18--%>
            <div style="margin:448px 600px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totaltax"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--20--%>
            <div style="margin:487px 600px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totaltax"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--25--%>
            <div style="margin:633px 600px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totaltax"].ToString()).ToString("#,###,##0.00")); %></div>
            <%--30D--%>
            <div style="margin:999px 475px; letter-spacing:0px; position:absolute; text-align:right; width:160px; font-size:12px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["totaltax"].ToString()).ToString("#,###,##0.00")); %></div>
              
            <img src="BIR-Form1601-C.jpg" alt="Alternate Text" />
        </div>
</body>
</html>
