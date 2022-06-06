<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ttospdf.aspx.cs" Inherits="content_printable_ttospdf" %>
<%@ Import Namespace="System.Data"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2316</title>
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
            .wrapper{ width:100%    }
        </style>
</head>
	<body> 
    <% DataTable dtalphalist = getdata.alphalist(" and a.id=" + function.Decrypt(Request.QueryString["key"].ToString(), true) + "");  %>

		<div class="wrapper">
            <%--1--%>
            <div style="margin:115px 174px; letter-spacing:7px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["yr"].ToString()); %></div>
            <%--2--%>
            <div style="margin:115px 535px; letter-spacing:6px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["frommmdd"].ToString()); %></div>
            <div style="margin:115px 684px; letter-spacing:6px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["tommdd"].ToString()); %></div>
            <%--3--%>
            <div style="margin:156px 174px; letter-spacing:7.5px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["emp_TIN"].ToString()); %></div>
      
            <%--4--%>
            <div style="margin:190px 69px; letter-spacing:0px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["emp_name"].ToString()); %></div>
            <%--7--%>
            <div style="margin:328px 68px; letter-spacing:9px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["dateofbirth"].ToString()); %></div>
            <%--9--%>
            <%if (dtalphalist.Rows[0]["civil_status"].ToString() == "Single")
              {%>
            <div style="margin:359px 131px; letter-spacing:0px; position:absolute;">x</div>
            <%}
              else
              { %>
            <div style="margin:359px 242px; letter-spacing:0px; position:absolute;">x</div>
            <%} %>
            <%--9a--%>
          <%--  <div style="margin:382px 131px; letter-spacing:0px; position:absolute;">x</div>
            <div style="margin:381px 242px; letter-spacing:0px; position:absolute;">x</div>--%>

             <%--15--%>
            <div style="margin:563px 174px; letter-spacing:7.5px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["compnay_TIN"].ToString()); %></div>

            <%--<div style="margin:563px 232px; letter-spacing:6px; position:absolute;">020</div>
            <div style="margin:563px 291px; letter-spacing:6px; position:absolute;">010</div>
            <div style="margin:563px 351px; letter-spacing:6px; position:absolute;">020</div>--%>

            <%--16--%>
            <div style="margin:596px 76px; letter-spacing:0px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["company_name"].ToString()); %></div>
            <%--17--%>
            <div style="margin:637px 66px; letter-spacing:0px; font-size:10px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["company_address"].ToString()); %></div>
            <%--18--%>
            <div style="margin:680px 174px; letter-spacing:7.5px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["prevtin"].ToString()); %></div>

            <%--19--%>
            <div style="margin:715px 76px; letter-spacing:0px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["prevemployersname"].ToString()); %></div>
            <%--20--%>
            <div style="margin:755px 66px; letter-spacing:0px; font-size:10px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["regaddress"].ToString()); %></div>
            <%--20 A--%>
            <div style="margin:750px 345px; letter-spacing:8px; position:absolute;"><% Response.Write(dtalphalist.Rows[0]["zipcode"].ToString()); %></div>

              
             <%--21--%>
            <div style="margin:785px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["present_gci"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--22--%>
            <div style="margin:805px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["total_nt"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--23--%>
            <div style="margin:827px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["total_present_tci"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--24--%>
            <div style="margin:848px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["total_prev_tci"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--25--%>
            <div style="margin:868px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["gross_tci"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--26--%>
             <div style="margin:889px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["less_total_exemption"].ToString()).ToString("#,###,##0.00")); %></div>
               <%--27--%>
             <div style="margin:909px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["less_premiumpaidonhealth"].ToString()).ToString("#,###,##0.00")); %></div>
               <%--28--%>
             <div style="margin:931px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["net_tci"].ToString()).ToString("#,###,##0.00")); %></div>
                <%--29--%>
             <div style="margin:952px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["taxdue"].ToString()).ToString("#,###,##0.00")); %></div>
                  <%--30a--%>
             <div style="margin:980px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["present_tax_withheld"].ToString()).ToString("#,###,##0.00")); %></div>
              <%--30b--%>
             <div style="margin:1001px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["prev_tax_withheld"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--31--%>
             <div style="margin:1022px 244px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["amt_taxwithheld_as_adjusted"].ToString()).ToString("#,###,##0.00")); %></div>
              <%--non taxable--%>
               <%--32--%>
             <div style="margin:194px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--33--%>
             <div style="margin:235px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--34--%>
             <div style="margin:263px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
              <%--35--%>
             <div style="margin:291px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
             <%--36--%>
             <div style="margin:320px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
                <%--37--%>
             <div style="margin:347px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); %></div>
               <%--38--%>
             <div style="margin:380px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["denimis"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--39--%>
             <div style="margin:419px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["govtdeductionanduniondues"].ToString()).ToString("#,###,##0.00")); %></div>
              <%--40--%>
             <div style="margin:468px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); %></div>
              <%--41--%>
             <div style="margin:505px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["total_nt"].ToString()).ToString("#,###,##0.00")); %></div>
             <%--42--%>
             <div style="margin:573px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["t_basic_salary"].ToString()).ToString("#,###,##0.00")); %></div>
               <%--43--%>
             <div style="margin:601px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--44--%>
             <div style="margin:628px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
                <%--45--%>
             <div style="margin:655px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["t_salariesandotherformofcompensation"].ToString()).ToString("#,###,##0.00")); %></div>
              <%--46--%>
             <div style="margin:682px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
              <%--47 a--%>
               <div style="margin:722px 430px; letter-spacing:0px; font-size:10px; position:absolute; width:120px;"></div>
              <%--47 a value--%>
             <div style="margin:717px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
              <%--47 b--%>
              <div style="margin:745px 430px; letter-spacing:0px; font-size:10px; position:absolute; width:120px;"></div>
              <%--47 b value--%>
              <div style="margin:743px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
              <%--48--%>
              <div style="margin:779px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
                <%--49--%>
              <div style="margin:808px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--50--%>
              <div style="margin:839px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--51--%>
              <div style="margin:868px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["t_thirteenmonthpay"].ToString()).ToString("#,###,##0.00")); %></div>
               <%--52--%>
              <div style="margin:900px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
               <%--53--%>
              <div style="margin:930px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
                <%--54 a--%>
               <div style="margin:976px 430px; letter-spacing:0px; font-size:10px; position:absolute; width:120px;"></div>
              <%--54 a value--%>
             <div style="margin:972px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
              <%--54 b--%>
              <div style="margin:999px 430px; letter-spacing:0px; font-size:10px; position:absolute; width:160px; "></div>
              <%--54 b value--%>
               <div style="margin:994px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"></div>
                <%--55--%>
                <div style="margin:1020px 577px; letter-spacing:0px; position:absolute; text-align:right; width:160px;"><% Response.Write(decimal.Parse(dtalphalist.Rows[0]["total_t"].ToString()).ToString("#,###,##0.00")); %></div>
                 <%--56--%>
                <div style="margin:1056px 107px; letter-spacing:0px; position:absolute; text-align:center;  width:280px;"><% Response.Write(dtalphalist.Rows[0]["autorized_agent"].ToString()); %></div>
                 <%--57--%>
                <div style="margin:1085px 107px; letter-spacing:0px; position:absolute; text-align:center;  width:280px;"><% Response.Write(dtalphalist.Rows[0]["emp_name"].ToString()); %></div>
                 <%--58--%>
                <div style="margin:1174px 88px; letter-spacing:0px; position:absolute; text-align:center;  width:280px;"><% Response.Write(dtalphalist.Rows[0]["autorized_agent"].ToString()); %></div>
                 <%--59--%>
                <div style="margin:1204px 457px; letter-spacing:0px; position:absolute; text-align:center;  width:225px;"><% Response.Write(dtalphalist.Rows[0]["emp_name"].ToString()); %></div>
            <img src="compressed.tracemonkey-pldi-09-1.png" alt="Alternate Text" />
        </div>
	</body>

</html>
