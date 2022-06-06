<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2316.aspx.cs" Inherits="_2316" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat ="server">
	<title>My Invoice</title>
    <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../style/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <link href="../style/css/custom.min.css" rel="stylesheet">
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet">
	<style type="text/css">
	   .invoice-container {
      	max-width: 800px;
      	margin: auto;
      	padding: 30px;
      	border: 1px solid #eee;
      	box-shadow: 0 0 10px rgba(0, 0, 0, .15);
      	font-size: 12px;
      	line-height: 24px;
      	font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
      	color: #555;
      	background-image:url('compressed.tracemonkey-pldi-09-1.png');
      	height:100%;
      	
      }
      .invoice-container img { width:100%;}
      .invoice-container table {
      	width: 100%;
      	line-height: inherit;
      	text-align: left;
      }
      .invoice-container table td {
      	padding: 1px;

      }
      .invoice-container table tr td:nth-child(2) {
      	text-align: left;
      }
      .invoice-container table tr.top table td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.top table td.title {
      	font-size: 20px;
      	line-height: 20px;
      	color: #333;
      }
      .invoice-container table tr.information table td {
      	padding-bottom: 0px;
      }
      .invoice-container table tr.heading td {
      	background: #eee;
      	border-bottom: 1px solid #ddd;
      	font-weight: bold;
      }
      .invoice-container table tr.details td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.item td {
      	border-bottom: 1px solid #eee;
      }
      .invoice-container table tr.item.last td {
      	border-bottom: none;
      }
      .invoice-container table tr.total td:nth-child(2) {
      	border-top: 2px solid #eee;
      	font-weight: bold;
      }
      @media only print {
      	.invoice-container table tr.top table td {
      		width: 100%;
      		display: block;
      		text-align: center;
      	}
      	.invoice-container table tr.information table td {
      		width: 100%;
      		display: block;
      		text-align: center;
      	}
      }
      .flo tr{ float:left;}
        .hideGridColumn
        {
            display: none;
        }
        .table {border:0 !important;}
        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td { border:0 !important;}
	    .table  tr td:nth-child(2) {text-align: right;}
	    .ssst{ float:right !important;}
	</style>
</head>
<body>
	<div class="invoice-container">
		<%--<table cellpadding="0" cellspacing="0">
			<tr class="top">
				<td colspan="2">
					<table>
						<tr>
                            <td><img class="mns" src="dist/img/gfs.png" alt="GFS" width="100px" style=" display:none;  margin:0px 0 0 0px" /></td>
							<td class="title">EMPLOYEE PERFORMANCE APPRAISAL FORM</td>
							<td style=" font-size:8px;">Form No. HRF-OD-003<br />
							Version No: 01-03.01.12<br />
						    Confidential</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr class="information">
				<td colspan="2">
					<table>
						<tr>
							<td>
                            Name of Employee: <asp:Label ID="lbl_noe" runat="server" Text="Nobel Caquelala"></asp:Label><br />
                            Position Title: <asp:Label ID="lbl_pt" runat="server" Text="Tig kiss sa mga gwapa"></asp:Label><br />
                            Period Evaluated: <asp:Label ID="lbl_pe" runat="server" Text="09/09/2018"></asp:Label><br />
                            </td>
                            <td class="ssst">
                            Id Number: <asp:Label ID="lbl_idnumber" runat="server" Text="100010"></asp:Label><br />
                            Department: <asp:Label ID="lbl_dept" runat="server" Text="sa mga gwapo"></asp:Label><br />
                            Appraiser's Name : <asp:Label ID="lbl_app" runat="server" Text="Orley Limpangog"></asp:Label><br />
                            </td>
						</tr>
					</table>
				</td>
			</tr>
            <tr>
                <td><a style=" font-weight:bold;">PURPOSE OF APPRAISAL:</a><br /><asp:RadioButtonList ID="rdiopurpose" Enabled="false" CssClass="flo" runat="server"> </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                   <a style=" font-weight:bold;">DIRECTION:</a> <br />
                    <asp:Label ID="lbl_direction" runat="server" Text="Below are the scale value and their descriptive definition. Multiply the scale of each item to the percentage assigned to each job factor. Get the average if the job factor has more than one item. Then, finally, summate the scores to get the final rating:"></asp:Label><br />
                </td>
            </tr>
             <tr>
                <td>
                    <div id="div_ratings"  style=" font-weight:bold; font-size:x-small; padding:10px;" runat="server">

                    </div>
                </td>
            </tr>
            <tr>
                <td>
         
                 
                </td>
            </tr>
		   <tr>
                <td>
                    <a style=" font-weight:bold;">DESCRIPTION EVALUATION</a> <br />
                 
                </td>
           </tr>
           <tr>
           <td>
              </td>
           </tr>
		</table>--%>
     
	</div>
    <div>

    </div>
</body>
</html>
