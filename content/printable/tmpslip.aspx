<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tmpslip.aspx.cs" Inherits="content_printable_tmpslip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payslip</title>
    <style type="text/css">
        *{margin:0; padding:0; font-family:"Lucida Sans Unicode", "Lucida Grande", sans-serif;}
        .content { min-width:1200px; width:70%; margin:0 auto; font-size:12px }
        .slip {border:1px solid #eee; padding:40px; margin:20px;}
        table { text-align:left; width:100%; margin:10px 0 0; border:none;}
        table th { text-transform:uppercase; font-size:9px;}
        table tr { vertical-align:top}
        .Grid td,.Grid th {padding:2px 4px; border:none;}
        .style1
        {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
    <%
        string query = "select  b.LastName+', '+b.FirstName+' '+b.MiddleName c_name,a.grossamt,a.wht,a.net , c.year ,    " +
                       "case when b.payrolltypeid=1 then b.MonthlyRate else b.DailyRate end rate,i.company,b.taxtable, " +
                        "e.TaxCode, " +
                        "g.Department, " +
                        "h.Position " +
                        "from thirteenmonth_trn_details a left join memployee b on a.empid=b.Id " +
                        "left join thirteenmonth_transaction c on a.tmtrnid=c.tmtrnid " +
                        "left join MTaxCode e on b.TaxCodeId=e.Id  " +
                        "left join MDepartment g on b.DepartmentId=g.Id " +
                        "left join MPosition h on b.PositionId=h.Id " +
                        "left join MCompany i on b.companyId=i.Id " +
                        "where ";
                        if (Request.QueryString["b"].ToString() == "single")
                            query += "a.empid=" + empid + " and a.tmtrnid=" + payid + " ";
                        else
                            query += "a.tmtrnid=" + payid + " ";

        System.Data.DataTable dt = dbhelper.getdata(query);
        foreach (System.Data.DataRow dr in dt.Rows)
        {
            lbl_idnumber.Text = dr["Company"].ToString();
            lbl_empname.Text = dr["c_name"].ToString();
            lbl_payrolltype.Text = dr["taxtable"].ToString();
            lbl_rate.Text = decimal.Parse(dr["rate"].ToString()).ToString("0#,###.00");
            lbl_start.Text = dr["year"].ToString();
            //lbl_end.Text = dr["dateend"].ToString();
            lbl_deptpartment.Text = dr["Department"].ToString();
            lbl_position.Text = dr["Position"].ToString();

           // lbl_paytype.Text = " ("+dr["payrolltype"].ToString()+")";
            lbl_bp_amt.Text = decimal.Parse(dr["rate"].ToString()).ToString("#,###.00");
           
            lbl_late.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_lhrs.Text = " (" + Math.Round(decimal.Parse("0.00"), 2).ToString() + " Hour)";

            lbl_undertime.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_uhrs.Text = " (" + Math.Round(decimal.Parse("0.00"), 2).ToString() + " Hour)";

            lbl_absent.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_absentdays.Text = " (" + Math.Round((decimal.Parse("0.00") * decimal.Parse("0.00")), 2) + " Hour)";



            lbl_lpay.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_oit.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_gp.Text = decimal.Parse("0.00").ToString("#,###.00");

           

            //deductions
            lbl_deduction_sss.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_phil.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_hdmf.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_whtt.Text = Math.Round(decimal.Parse(dr["wht"].ToString()), 2).ToString("#,###.00");

            totaldeductions = decimal.Parse("0.00");
            foreach (GridViewRow gr in grid_view.Rows)
            {
                totaldeductions = totaldeductions + decimal.Parse(gr.Cells[1].Text);
            }
            lbl_td.Text ="0.00";
            lbl_oint.Text = Math.Round(decimal.Parse("0.00"), 2).ToString("#,###.00");
            lbl_np.Text = Math.Round(decimal.Parse(dr["net"].ToString()), 2).ToString("#,###.00");
            txt_.Text=dr["c_name"].ToString();
            lbl_sc.Text = Math.Round(decimal.Parse(dr["grossamt"].ToString()), 2).ToString("#,###.00");
%>
        <div class="slip">
         <p  style=" color:Red; text-align:center; font-weight:bold;">CONFIDENTIAL DATA: DO NOT LOOK AT THIS SCREEN IF THIS IS NOT YOUR RECORD</p>
        <table>
        <tr>
         
        </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>Company</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_idnumber" runat="server" Text="Id Number"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Department</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_deptpartment" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Employee Name</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_empname" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                      
                         <tr style=" visibility:hidden;">
                            <td><asp:Label ID="lbl_payrate" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_rate" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>
                       <tr>
                            <td>Payroll Type</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_payrolltype" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Cut-Off</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_start" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Basic</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_bp_amt" runat="server"></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td class="style1">DATE END</td>
                            <td class="style1">:</td>
                            <td class="style1"><asp:Label ID="lbl_end" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td>DEPARTMENT</td>
                            <td>:</td>
                        </tr>
                        <tr style=" display:none;">
                            <td>POSITION</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_position" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
           <asp:GridView ID="grid_breakdown" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="false" CssClass="Grid">
                    <Columns>
                        <asp:BoundField DataField="brekdown" HeaderText="Break Down"/>
                        <asp:BoundField DataField="reghrs" HeaderText="Regular Hrs"/>
                        <asp:BoundField DataField="regamt" HeaderText="Regular Amt."/>
                        <asp:BoundField DataField="hdpremium" HeaderText="Holiday Premium"/>
                        <asp:BoundField DataField="nighthrs" HeaderText="Night HRS."/>
                        <asp:BoundField DataField="nightpremium" HeaderText="Regular Night Diff."/>
                        <asp:BoundField DataField="regothrs" HeaderText="Regular OT HRS."/>
                        <asp:BoundField DataField="regotamt" HeaderText="Regular OT Amt."/>
                        <asp:BoundField DataField="nightothrs" HeaderText="Night OT HRS."/>
                        <asp:BoundField DataField="nightotamt" HeaderText="Night OT Amt."/>
                         <asp:BoundField DataField="totalamt" HeaderText="Total"/>
                    </Columns>
                    </asp:GridView>
            <table>
            <tr>
                <td>
                     <table>
                        <tr>
                                <th>Additional Income</th>
                                <th></th>
                        </tr>
                        <tr>
                            <td>Leave w/ Pay</td>
                            <td><asp:Label ID="lbl_lpay" runat="server" ></asp:Label></td>
                        </tr>
                         <tr>
                            <td>13th Month Pay</td>
                            <td><asp:Label ID="lbl_sc" runat="server" ></asp:Label></td>
                        </tr>

                        <tr>
                            <td>Other Income (Taxable)</td>
                            <td><asp:Label ID="lbl_oit" runat="server" ></asp:Label></td>
                        </tr>
                      </table>
                </td>
                <td>
                <table>
                <tr>
                                 <th>Deduction</th>
                                 <th></th>
                            </tr>
                              <tr>
                            <td>Late<asp:Label ID="lbl_lhrs" runat="server" ></asp:Label></td>
                            <td><asp:Label ID="lbl_late" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Undertime<asp:Label ID="lbl_uhrs" runat="server" ></asp:Label></td>
                            <td><asp:Label ID="lbl_undertime" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Absent<asp:Label ID="lbl_absentdays" runat="server" ></asp:Label></td>
                            <td><asp:Label ID="lbl_absent" runat="server" ></asp:Label></td>
                        </tr >
                        <tr style=" font-weight:bold">
                             <td>Gross Pay</td>
                            <td><asp:Label ID="lbl_gp" runat="server" ></asp:Label></td>
                        </tr>
                </table>
                </td>
                <td>
                      <table >
                            <tr>
                                 <th>Gov't. DEDUCTIONS</th>
                                 <th></th>
                            </tr>
                            <tr>
                                <td>SSS Premium</td>
                                <td><asp:Label ID="lbl_deduction_sss" runat="server" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>PHIL. Premium</td>
                                <td><asp:Label ID="lbl_phil" runat="server" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>HDMF Premium</td>
                                <td><asp:Label ID="lbl_hdmf" runat="server" ></asp:Label></td>
                            </tr>
                             <tr>
                                <td>Withholding Tax</td>
                                <td><asp:Label ID="lbl_whtt" runat="server" ></asp:Label></td>
                            </tr>
                          
                            <tr>
                               
                            </tr>
                      </table>
                </td>
                <td>

                                    <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                                        <Columns>
                                                <asp:BoundField DataField="OtherDeduction" HeaderText="Other Deductions"/>
                                                <asp:BoundField DataField="loanamt"  DataFormatString="{0:#,###.00}" HeaderStyle-Width="100px"/>
                                        </Columns>
                                    </asp:GridView>

                </td>

            </tr>
            <tr>
                <td colspan="2">
                    <table >
                        <tr>
                             <td>Total Deductions</td>
                             <td>:</td>
                             <td><asp:Label ID="lbl_td" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Other Income (Non Taxable)</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_oint" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style=" font-weight:bold">
                            <td>Net Pay</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_np" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
          <table>
        <tr>
       
       
            <td>Prepared By:<asp:Label ID="lbl_pb" runat="server" style=" border-bottom:1px solid gray;" ></asp:Label></td>
          
        </tr>
        </table>
        <br />
      <div style=" text-align:center;">
      <p>  I acknowledge to have received the amount stated here within  with no further claim for services rendered.</p>
<%-- <asp:Label ID="lbl_signature"  runat="server"></asp:Label><br />--%>
             <br />
             <asp:Label ID="Label1" Text="Recieved By:"  runat="server"></asp:Label><asp:Label ID="txt_" runat="server" style=" border:NONE; border-bottom:1px solid gray;"></asp:Label>
             </div>
        </div>

       <% }
     %>

  
    </div>



    </form>
</body>
</html>
