<%@ Page Language="C#" AutoEventWireup="true" CodeFile="processform.aspx.cs" Inherits="processform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="invoice-container">
		<table cellpadding="0" cellspacing="0">
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
         
                    <asp:GridView ID="grid_jfd" runat="server"  ClientIDMode="Static" OnRowDataBound="databoundnd"  HeaderStyle-CssClass="hideGridColumn" CssClass="table table-striped table-bordered no-margin"  AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="Job Factors">
                            <ItemTemplate>
                                    <asp:Label ID="lbl_iddd" runat="server" Visible="false" Text='<%#Eval("id")%>'></asp:Label>
                                    <asp:Label ID="lbl_jfheader" runat="server" style=" font-weight:bold;" Text='<%#Eval("header")%>'></asp:Label>
                                    <asp:GridView ID="grid_details" runat="server" OnRowDataBound="databoundnd1" HeaderStyle-CssClass="hideGridColumn"  ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
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
                                                    <asp:RadioButtonList ID="rb_rate" runat="server">
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
                    <asp:GridView ID="grid_desceval" runat="server"  ClientIDMode="Static"   HeaderStyle-CssClass="hideGridColumn" CssClass="table table-striped table-bordered no-margin"  AutoGenerateColumns="false" >
                    <Columns>
                        <asp:TemplateField>   
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>.   
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <ItemTemplate>
                                <asp:Label ID="lbl_iddddd" runat="server" Text='<%#bind("id")%>'></asp:Label>
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
    <div>
     <asp:Button ID="Button1" runat="server" OnClick="save_app_form" Text="Submit" />
    </div>
    </div>
    </form>
</body>
</html>
