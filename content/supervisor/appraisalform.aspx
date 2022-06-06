<%@ Page Language="C#" AutoEventWireup="true" CodeFile="appraisalform.aspx.cs" Inherits="content_supervisor_appraisalform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  <link rel="stylesheet" href="../../vendors/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../vendors/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="../../dist/css/base.css">
    <link rel="stylesheet" href="../../dist/css/custom.css" />
    <link rel="stylesheet" href="../../dist/css/skins/_all-skins.min.css">
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
      }
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
	.nobel {position: fixed; overflow:hidden; top: 42%;left: 50%;margin-left:-70px; z-index: 2001; padding: 20px;}

    .emp-identity{padding:0;float:left; width:100%;}
    .emp-identity input[type=text]{padding:8px; width:100%; margin-bottom:10px}
    .emp-identity select {padding:7px; width:100%;margin-bottom:10px}
    .emp-identity input[type=checkbox] {margin-right:5px !important;}
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div class="invoice-container">
		<table cellpadding="0" cellspacing="0">
			<tr class="top" style=" display:none;" >
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
							<td style=" font-size:18px; font-weight:bold;">
                            Name of Employee: <asp:Label ID="lbl_noe" runat="server" Text="Nobel Caquelala"></asp:Label><br />
<%--                            Position Title: <asp:Label ID="lbl_pt" runat="server" Text="Tig kiss sa mga gwapa"></asp:Label><br />
                            Period Evaluated: <asp:Label ID="lbl_pe" runat="server" Text="09/09/2018"></asp:Label><br />--%>
                            </td>
                            <td style=" font-size:18px; font-weight:bold;" class="ssst">
                            Id Number: <asp:Label ID="lbl_idnumber" runat="server" Text="100010"></asp:Label><br />
                           <%-- Department: <asp:Label ID="lbl_dept" runat="server" Text="sa mga gwapo"></asp:Label><br />
                            Appraiser's Name : <asp:Label ID="lbl_app" runat="server" Text="Orley Limpangog"></asp:Label><br />--%>
                            </td>
						</tr>
					</table>
				</td>
			</tr>
            <tr>
            <td></td>
            </tr>
            <tr>
                <td><a style=" font-weight:bold;">
                    <asp:Label ID="lbl_poa" runat="server" ForeColor="Red"></asp:Label>PURPOSE OF APPRAISAL:</a><br /><asp:RadioButtonList ID="rdiopurpose" CssClass="flo" runat="server"> </asp:RadioButtonList></td>
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
         
                    <asp:GridView ID="grid_jfd" runat="server"  ClientIDMode="Static" OnRowDataBound="databoundnd"  HeaderStyle-CssClass="hideGridColumn" CssClass="table table-bordered no-margin"  AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="Job Factors">
                            <ItemTemplate>
                                    <asp:Label ID="lbl_percent" runat="server" Visible="false" Text='<%#Eval("percenttt")%>'></asp:Label>
                                    <asp:Label ID="lbl_iddd" runat="server" Visible="false" Text='<%#Eval("id")%>'></asp:Label>
                                    <asp:Label ID="lbl_jfheader" runat="server" style=" font-weight:bold;" Text='<%#Eval("header")%>'></asp:Label>
                                    <asp:GridView ID="grid_details" runat="server" OnRowDataBound="databoundnd1" HeaderStyle-CssClass="hideGridColumn"  ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-bordered no-margin">
                                    <Columns>
                                        <asp:TemplateField>   
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.   
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="descc" ItemStyle-Font-Size="11px" HeaderText="Details" />
                                        <asp:TemplateField HeaderText="Ratings" ItemStyle-Font-Size="11px" ItemStyle-CssClass="ssst"  ><%--ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn"--%>
                                            <ItemTemplate >
                                                <asp:Label ID="lbl_idddd" runat="server" Visible="false" Text='<%#Eval("id")%>'></asp:Label>
                                                  <asp:Label ID="Label1" runat="server" ></asp:Label>
                                                    <asp:RadioButtonList ID="rb_rate" CssClass="flo" runat="server">
                                                    </asp:RadioButtonList>
                                                 </td>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                          </ItemTemplate>
                         </asp:TemplateField>
                       </Columns>
                    </asp:GridView>
                 
                </td>
            </tr>
		   <tr>
                <td>
                    <a style=" font-weight:bold;">DESCRIPTION EVALUATION</a> <br />
                    <asp:GridView ID="grid_desceval" runat="server"  ClientIDMode="Static"   HeaderStyle-CssClass="hideGridColumn" CssClass="table table-bordered no-margin"  AutoGenerateColumns="false" >
                    <Columns>
                        <asp:TemplateField>   
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>.   
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <ItemTemplate>
                                <asp:Label ID="lbl_iddddd" runat="server" Visible="false" Text='<%#bind("id")%>'></asp:Label>
                                <asp:Label ID="lbl_desceval" runat="server" Text='<%#bind("descc")%>'></asp:Label>
                                <asp:textbox ID="txt_ans" runat="server" TextMode="MultiLine" Text="" style=" resize:none; width:700px; padding:10px; border:0px; border-bottom:1px solid gray;"></asp:textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                 
                    </Columns>
                    </asp:GridView>
                </td>
           </tr>
           <tr>
           <td>
              </td>
           </tr>
		</table>
     
	</div>
    <div style=" text-align:center; margin:10px;">
     <asp:Button ID="Button1" runat="server" OnClick="save_app_form" Text="Submit" CssClass="btn btn-primary" />
    </div>
    <asp:HiddenField ID="hdn_purposeid" runat="server" />
       <div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
        <div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
            <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
            <div style=" overflow:auto">
                <b>Appraisal Recommendation</b>
                <br />
                <ul class="emp-identity bei">
                <li><asp:Label ID="Label2" runat="server" Text=""></asp:Label></li>
                <li>Recommendation</li>
                <li> <asp:DropDownList ID="ddl_recommendation" runat="server"></asp:DropDownList></li>
                <li><asp:Button ID="Button2" runat="server" Text="Save" OnClick="click_save" CssClass="btn btn-primary" /></li>
               
                </ul>
            </div>   
        </div>

    </div>
    <script type='text/javascript'>
        //    $.noConflict();
        $(window).load(function () {
            $(":input").inputmask();
        });
        $(".load-nobel").click(function () {
            $("#load").removeClass("hide");
            $("#loader").removeClass("hide");
        });
</script>
    </form>
</body>
</html>
