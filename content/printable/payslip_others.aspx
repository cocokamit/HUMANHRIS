<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payslip_others.aspx.cs" Inherits="content_printable_payslip_others" %>

<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payslip</title>
    <style type="text/css">
        *{margin:0; padding:0; font-family:"Lucida Sans Unicode", "Lucida Grande", sans-serif;}
        .content { min-width:1200px; width:70%; margin:0 auto; font-size:9px }
        .slip {border:1px solid #eee; padding:40px; margin:20px;}
        table { text-align:left; width:100%; margin:10px 0 0; border:none;}
        table th { text-transform:uppercase; font-size:8px;}
        table tr { vertical-align:top;}
        .Grid td,.Grid th {padding:2px 4px; border:none;}
        .dnone{ display:none;}
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
        string empidd = empid;
        string cls=Request.QueryString["cls"].ToString();
        string classs = cls == "13" ? "13th Month Pay" : cls=="14"?"Fourteen Month Pay":"Bonus";
        string tbl = cls == "13" ? "thirteen_mp_trn" : cls == "14" ? "fourteen_mp_trn" : "bonus_trn";
        string tbl_child = cls == "13" ? "thirteen_mp_trn_child" : cls == "14" ? "fourteen_mp_trn_child" : "bonus_trn_child";
        string field = cls == "13" ? "total_thirteen" : cls == "14" ? "total_fourteen" : "total_bonus";
        
        string query = "select i.Company, c.FirstName+' '+c.MiddleName+' '+c.LastName as c_name, " +
                    "g.Department,h.Position,k.Division2,c.atmaccountnumber, " +
                    "cast(round(case when c.payrolltypeid=1 then c.MonthlyRate else c.DailyRate end,12)as numeric(12,2)) MonthlyRate, " +
                    "cast(round(c.dailyrate,12)as numeric(12,2)) dailyrate, " +
                    "c.tin,c.sssnumber,c.phicnumber,c.hdmfnumber,LEFT(CONVERT(varchar,sysdate,101),10)Payroll_Date, " +
                    "a.yyyy Period,b.monthlyrate,b.total_thirteen as total,b.taxable,b.nontaxable " +
                    "from thirteen_mp_trn a " +
                    "left join thirteen_mp_trn_child b on a.trnid=b.trnid " +
                    "left join memployee c on b.empid=c.Id " +
                    "left join MDepartment g on c.DepartmentId=g.Id  " +
                    "left join MPosition h on c.PositionId=h.Id  " +
                    "left join MCompany i on c.companyId=i.Id " +
                    "left join MDivision2 k on c.DivisionId2=k.Id where a.status !='Cancelled' and ";
                    if (Request.QueryString["b"].ToString() == "single")
                        query += "c.IdNumber='" + empidd + "' and a.trnid=" + payid + " ";
                    else
                        query += "a.trnid=" + payid + " ";

        System.Data.DataTable dt = dbhelper.getdata(query);
        decimal loop = 1;
        foreach (System.Data.DataRow dr in dt.Rows)
        {
            System.Data.DataTable dtbreakdown = dbhelper.getdata("select  " +
                                              "case when brekdown ='Working  Day' then 'Regular Working Day' else brekdown end brekdown , " +
                                              " '0.00' reghrs, " +
                                              " '0.00' regamt, " +
                                              "hdpremium, " +
                                              "regothrs,regotamt, " +
                                              "nighthrs,nightpremium, " +
                                              "nightothrs,nightotamt from TPayrollLineBreakDown " +
                                              "where payrolllineid=1");
            grid_breakdown.DataSource = dtbreakdown;
            grid_breakdown.DataBind();
           
            
            lbl_idnumber.Text = dr["Company"].ToString();
            lbl_empname.Text = dr["c_name"].ToString();

            lbl_rate.Text = decimal.Parse(dr["MonthlyRate"].ToString()).ToString("0#,###.00");
          
            lbl_deptpartment.Text = dr["Department"].ToString();
            lbl_position.Text = dr["Position"].ToString();
            lbl_account_no.Text = dr["atmaccountnumber"].ToString();
            lbl_tin_no.Text = dr["tin"].ToString();
            lbl_sss_no.Text = dr["sssnumber"].ToString();
            lbl_phic_no.Text = dr["phicnumber"].ToString();
            lbl_hdmf_no.Text = dr["hdmfnumber"].ToString();

            lbl_level.Text = dr["Division2"].ToString();
            lbl_dr.Text = dr["dailyrate"].ToString();
            lbl_pd.Text = dr["Payroll_Date"].ToString();
            lbl_pp.Text = dr["Period"].ToString();
            lbl_desc_NT_AMT.Text=dr["nontaxable"].ToString();
            lbl_desc_T_AMT.Text=dr["taxable"].ToString();
         
%>
         
        <div class="slip">
        <div class="navbar nav_title text-center">
               <img class="mns" src="style/img/EOYCebuLandmasters100617.png" alt="CLI" style="width:130px; height:55px; margin-top:-1px"/> 
            </div>
        <p  style=" color:Red; text-align:center; font-weight:bold;">CONFIDENTIAL DATA: DO NOT LOOK AT THIS SCREEN IF THIS IS NOT YOUR RECORD</p>
        <table>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>Company</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_idnumber" runat="server" Text="Id Number"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>Employee Name</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_empname" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Department</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_deptpartment" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Position</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_position" runat="server" Text="Position"></asp:Label></td>
                        </tr>
                       
                        
                    </table>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>Level</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_level" runat="server" Text="Level"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Account #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_account_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Monthly Rate</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_rate" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>Daily Rate</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_dr" runat="server" Text="Daily Rate"></asp:Label></td>
                        </tr>
                        

                       
                    </table>
                </td>
                <td  colspan="2">
                    <table>
                    <tr>
                            <td>TIN #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_tin_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                           <tr>
                            <td>SSS #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_sss_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                           <tr>
                            <td>PHIC. #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_phic_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>HDMF. #</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_hdmf_no" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
                        </tr>


                        
                    </table>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>Payroll Date</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_pd" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Payroll Period</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_pp" runat="server" ></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6" style=" border:1px solid #eee; padding:10px; margin:10px;">
                     <asp:GridView ID="grid_breakdown" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="false" CssClass="Grid" >
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
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
                <tr>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                        <tr>
                            <th>Absent / Tardiness</th>
                            <th></th>
                        </tr>
                        <tr>
                            <td>Late<asp:Label ID="lbl_lhrs" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_late" runat="server" Text="0.00" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Undertime<asp:Label ID="lbl_uhrs" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_undertime" runat="server" Text="0.00" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Absent<asp:Label ID="lbl_absentdays" runat="server" ></asp:Label></td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_absent" runat="server" Text="0.00"></asp:Label></td>
                        </tr >
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td>Leave w/ Pay</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_lpay" runat="server" Text="0.00"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                     <table>
                       <tr style=" display:none;">
                            <th>Other Income(T)</th>
                            <th></th>
                       </tr>
                       <tr>
                           <td><asp:Label ID="lbl_desc_T" runat="server" Text="Taxable"></asp:Label></td>
                            <td><asp:Label ID="lbl_desc_T_AMT" runat="server" Text="Label"></asp:Label></td>
                       </tr>
                     
                      
               
                        <tr style=" display:none;">
                            <th>Other Income(NT)</th>
                            <th></th>
                        </tr>
                        <tr>
                            <td><asp:Label ID="lbl_desc_NT" runat="server" Text="Non-Taxable"></asp:Label></td>
                            <td><asp:Label ID="lbl_desc_NT_AMT" runat="server" Text="Label"></asp:Label></td>
                       </tr>
                   
                       
                    </table>
                     <asp:Label ID="lbl_oit" runat="server" style=" display:none"></asp:Label>
                     <asp:Label ID="lbl_sp" runat="server" style=" display:none;"></asp:Label>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <td style=" font-weight:bold;"><%=classs%></td>
                                    <td><%=dr["total"].ToString()%></td>
                                </tr>
                                <tr>
                                    <td style=" font-weight:bold;">Gross Pay</td>
                                    <td><%=dr["total"].ToString()%></td>
                                </tr>
                                <tr>
                                    <td>-</td>
                                    <td>-</td>
                                </tr>
                                <tr>
                                    <th>DEDUCTIONS</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td>SSS Premium</td>
                                    <td><asp:Label ID="lbl_deduction_sss" runat="server" Text="0.00"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>PHIL. Premium</td>
                                    <td><asp:Label ID="lbl_phil" runat="server" Text="0.00"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>HDMF Premium</td>
                                    <td><asp:Label ID="lbl_hdmf" runat="server" Text="0.00"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Withholding Tax</td>
                                    <td><asp:Label ID="lbl_whtt" runat="server" Text="0.00"></asp:Label></td>
                                </tr>
                               
                       </table>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <th>Other Deduction</th>
                                    <th></th>
                                </tr>
                               
                               
                       </table>
                </td>
                <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <th>Loan Balance</th>
                                    <th></th>
                                </tr>
                               
                       </table>
                </td>
                 <td style=" border:1px solid #eee; padding:10px; margin:10px;">
                    <table>
                                <tr>
                                    <td style=" font-size:8px; font-weight:bold">Net Pay: <%=dr["total"].ToString()%></td>
                                </tr>
                               
                       </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2">
                    <table>
                        <tr style=" display:none;">
                            <td>Non Taxable Night Shift Differential</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_ntnsd" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td>Non Taxable Allowance</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_nta" runat="server" ></asp:Label></td>
                        </tr>
                         <tr style=" display:none;">
                            <td>Non Taxable Acting Capacity Allowance</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_ntaca" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style=" display:none;">
                            <td>Other Income (Non Taxable)</td>
                            <td>:</td>
                            <td><asp:Label ID="lbl_oint" runat="server" ></asp:Label></td>
                        </tr>
                      
                    </table>
                </td>
            </tr>
        </table>
        <table style="display:none;">
            <tr>
                <td>Prepared By:<asp:Label ID="lbl_pb" runat="server" style=" border-bottom:1px solid gray;" ></asp:Label></td>
            </tr>
        </table>
        <br />
        <div style=" text-align:center; ">
             <p>  This is an electronically generated report, hence does not require a signature.</p>
             <asp:Label ID="Label1" Text="Recieved By:"  runat="server" style=" display:none;"></asp:Label><asp:Label ID="txt_" runat="server" style=" border:NONE; border-bottom:1px solid gray; display:none;"></asp:Label>
          </div>
        </div>
           <% } %>
     

  
    </div>



    </form>
</body>
</html>

