<%@ Page Language="C#" AutoEventWireup="true" CodeFile="terminalcomputation.aspx.cs" Inherits="content_printable_terminalcomputation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <div class="slip">
        <table>
            <tr>
                <td>
                <table>
                <tr>
                    <td>Name</td>
                     <td>:</td>
                     <td><asp:Label ID="lbl_name" runat="server" Text="NOBEL BARRO CAQUELALA"></asp:Label></td>
                </tr>
                  <tr>
                    <td>Position</td>
                     <td>:</td>
                     <td><asp:Label ID="lbl_pos" runat="server" Text="IT"></asp:Label></td>
                </tr>
              <%--    <tr>
                    <td>Reason</td>
                     <td>:</td>
                     <td><asp:Label ID="lbl_reason" runat="server" Text="GWAPO TALAGA AKO! WALANG SINONG MAKAPAG PIGIL SA KAGWAPOHAN KO!"></asp:Label></td>
                </tr>--%>
             </table>
                </td>
                <td>
                    <table style=" width:40%; float:left;" >
                <tr>
                    <td>Date</td>
                     <td>:</td>
                     <td><asp:Label ID="lbl_date_released" runat="server" Text="5/20/2018"></asp:Label></td>
                </tr>
                <tr>
                    <td>Date Resigned</td>
                     <td>:</td>
                     <td><asp:Label ID="lbl_date_resigned" runat="server" Text="3/20/2018"></asp:Label></td>
                </tr>
                
             </table>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                     <table>
                            <tr>
                                <td colspan="3">I. Salaries and Wages</td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <table>
                                        <tr>
                                            <td >a.Unclaimed Salary</td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_us" runat="server" Text="5000000"></asp:Label></td>
                                        </tr>
                                         <tr>
                                            <td>b.13TH MONTH </td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_13monthpay" runat="server" Text="5000000"></asp:Label></td>
                                        </tr>
                                         <tr>
                                            <td>c.Service Incentive Leave </td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_sil" runat="server" Text="5000000"></asp:Label></td>
                                        </tr>
                                         <tr>
                                            <td>Total </td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_total" runat="server" Text="15000000"></asp:Label></td>
                                        </tr>
                                    </table>
                                  </td>
                            </tr>
                            <tr>
                                <td colspan="3">II. DEDUCTIONS</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>a.Tax Payable</td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_tp" runat="server" Text="0.00"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>b.Other </td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_other" runat="server" Text="0.00"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Total Deduction</td>
                                            <td>:</td>
                                            <td><asp:Label ID="lbl_total_deduction" runat="server" Text="0.00"></asp:Label></td>
                                        </tr>
                                    </table>
                                  </td>
                            </tr>
                            <tr>
                                <td>III. GROSS TOTAL</td>
                                 <td>:</td>
                                 <td><asp:Label ID="lbl_gt" runat="server" Text="0.00" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Tax Refund</td>
                                <td>:</td>
                                <td><asp:Label ID="lbl_tr" runat="server" Text="0.00" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Internal Savings</td>
                                <td>:</td>
                                <td><asp:Label ID="lbl_is" runat="server" Text="0.00" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Net Pay</td>
                                <td>:</td>
                                <td><asp:Label ID="lbl_netpay" runat="server" Text="15000000"></asp:Label></td>
                            </tr>
                      </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>Prepared By:<asp:Label ID="lbl_pb" runat="server" style=" border-bottom:1px solid gray;" ></asp:Label></td>
            </tr>
             <tr>
                <td>Recieved By:<asp:Label ID="txt_rb" runat="server" style=" border-bottom:1px solid gray;"></asp:Label></td>
            </tr>
        </table>
           
        </div>
    </div>
    </form>
</body>
</html>
