<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payslipviewing.aspx.cs" Inherits="content_payroll_payslipviewing" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .box {width:49%}
    </style>
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
        $(function () {
            $(".datee").datepicker({ changeMonth: true,
                yearRange: "-100:+0",
                changeYear: true
            });
        });
    })(jQuery);
    </script>
<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_payslip">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Payslip</h3>
    </div>  
     <div class="x-head">
                 <asp:DropDownList ID="ddl_pg" runat="server" ></asp:DropDownList>
                <asp:TextBox ID="txt_search" runat="server"  Placeholder="Search" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_from" runat="server" CssClass="datee" Placeholder="FROM" AutoComplete="off"></asp:TextBox>
                <asp:TextBox ID="txt_to" CssClass="datee" runat="server" ClientIDMode="Static" AutoComplete="off" Placeholder="TO"></asp:TextBox>
                <asp:Button ID="Button1"  runat="server"  Text="Search" CssClass="btn btn-primary"/>
            </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
    <div>
    <%
        string query = "select b.id payid, a.payrolltypeid,a.id as payrolllineid,a.EmployeeId,a.NetIncome,a.TotalTardyAmount,a.TotalAbsentAmount,a.TotalRegularOvertimeAmount,a.ssscontribution,a.phiccontribution,a.hdmfcontribution,a.tax,    " +
        "b.id,left(convert(varchar,b.PayrollDate,101),10)PayrollDate, " +
        "c.IdNumber,c.FirstName+' '+c.MiddleName+' '+c.LastName as c_name,  " +
        "d.PayrollType,case when a.payrolltypeid=1 then c.MonthlyRate else c.DailyRate end rate, " +
        "e.TaxCode, " +
        "left(convert(varchar,f.DateStart,101),10)DateStart,left(convert(varchar,f.DateEnd,101),10)DateEnd , " +
        "g.Department, " +
        "h.Position, " +
        "(select SUM(case when [Absent]='True'  then 1 else 0 end + case when [HalfdayAbsent]='True'  then 0.5 else 0 end) from TDTRLine where employeeid=a.EmployeeId and dtrid=b.dtrid) as absentdays," +
        "a.TotalOtherIncomeTaxable,a.TotalOtherIncomeNonTaxable,a.taxtable, " +
        "a.totalregularpay " +
        ",a.totallateamt " +
        ",a.totalundertimeamt " +
        ",a.totalot " +
        ",a.totalrdpay " +
        ",a.totalleavepay " +
        ",a.totalhdpay " +
        ",a.TotalAbsentAmount,a.TotalRegularWorkingHours,a.TotalTardyLateHours,a.TotalTardyUndertimeHours " +
        ",a.TotalRegularOvertimeHours " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id  " +
        "left join MEmployee c on a.EmployeeId=c.Id  " +
        "left join MPayrollType d on a.PayrollTypeId=d.Id  " +
        "left join MTaxCode e on a.TaxCodeId=e.Id  " +
        "left join TDTR f on b.DTRId=f.Id " +
        "left join MDepartment g on c.DepartmentId=g.Id " +
        "left join MPosition h on c.PositionId=h.Id where b.id is not null and b.status is null ";
       
        if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
        {
            query += "and LEFT(CONVERT(varchar,f.DateStart,101),10)>='" + txt_from.Text + "' and  LEFT(CONVERT(varchar,f.DateStart,101),10)<='" + txt_to.Text + "'";
        }
        if (int.Parse(ddl_pg.SelectedValue) > 0)
        {
            query += " and c.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        }
        if (txt_search.Text.Length > 0)
        {
            query += " and c.LastName + '' + c.FirstName + '' + c.MiddleName+''+c.IdNumber like '%" + txt_search.Text + "%'";
        }                

        System.Data.DataTable dt = dbhelper.getdata(query);
        if (dt.Rows.Count > 0)
        {
            int cnt = 0;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                lbl_idnumber.Text = dr["IdNumber"].ToString();
                lbl_empname.Text = dr["c_name"].ToString();
                lbl_payrolltype.Text = dr["taxtable"].ToString();
                lbl_rate.Text = decimal.Parse(dr["rate"].ToString()).ToString("#,###.##");
                lbl_start.Text = dr["DateStart"].ToString();
                lbl_end.Text = dr["dateend"].ToString();
                lbl_deptpartment.Text = dr["Department"].ToString();
                lbl_position.Text = dr["Position"].ToString();

                lbl_late.Text = Math.Round(decimal.Parse(dr["totallateamt"].ToString()), 2).ToString();
                lbl_lhrs.Text = Math.Round(decimal.Parse(dr["TotalTardyLateHours"].ToString()), 2).ToString() + " Hours";

                lbl_undertime.Text = Math.Round(decimal.Parse(dr["totalundertimeamt"].ToString()), 2).ToString();
                lbl_uhrs.Text = Math.Round(decimal.Parse(dr["TotalTardyUndertimeHours"].ToString()), 2).ToString() + " Hours";

                lbl_absent.Text = Math.Round(decimal.Parse(dr["TotalAbsentAmount"].ToString()), 2).ToString();
                lbl_absentdays.Text = Math.Round(decimal.Parse(dr["absentdays"].ToString()), 2).ToString() + " Days";

                lbl_reg_amt.Text = Math.Round(decimal.Parse(dr["totalregularpay"].ToString()), 2).ToString();
                lbl_rahrs.Text = Math.Round(decimal.Parse(dr["TotalRegularWorkingHours"].ToString()), 2).ToString() + " Hours";

                lbl_ot.Text = Math.Round(decimal.Parse(dr["totalot"].ToString()), 2).ToString();
                lbl_othrs.Text = Math.Round(decimal.Parse(dr["TotalRegularOvertimeHours"].ToString()), 2).ToString() + " Hours";

                lbl_lpay.Text = Math.Round(decimal.Parse(dr["totalleavepay"].ToString()), 2).ToString();
                lbl_rdpay.Text = Math.Round(decimal.Parse(dr["totalrdpay"].ToString()), 2).ToString();
                lbl_hp.Text = Math.Round(decimal.Parse(dr["totalhdpay"].ToString()), 2).ToString();
                lbl_oit.Text = Math.Round(decimal.Parse(dr["TotalOtherIncomeTaxable"].ToString()), 2).ToString();
                lbl_gp.Text = Math.Round(decimal.Parse(lbl_reg_amt.Text) + decimal.Parse(lbl_ot.Text) + decimal.Parse(lbl_lpay.Text) + decimal.Parse(lbl_rdpay.Text) + decimal.Parse(lbl_hp.Text) + decimal.Parse(lbl_oit.Text), 2).ToString();

                lbl_payrate.Text = dr["PayrollType"].ToString() == "Fixed" ? "Monthly Rate" : "Daily Rate";

                //deductions
                lbl_deduction_sss.Text = Math.Round(decimal.Parse(dr["ssscontribution"].ToString()), 2).ToString();
                lbl_phil.Text = Math.Round(decimal.Parse(dr["phiccontribution"].ToString()), 2).ToString();
                lbl_hdmf.Text = Math.Round(decimal.Parse(dr["hdmfcontribution"].ToString()), 2).ToString();
                lbl_whtt.Text = Math.Round(decimal.Parse(dr["tax"].ToString()), 2).ToString();

              decimal  totaldeductions = decimal.Parse(lbl_deduction_sss.Text) + decimal.Parse(lbl_phil.Text) + decimal.Parse(lbl_hdmf.Text) + decimal.Parse(lbl_whtt.Text);
                System.Data.DataTable dtotherdeductions = dbhelper.getdata("select  " +
                                                "a.OtherDeduction, " +
                                                "(select  case when SUM(c.Amount) is null then '0.00' else SUM(c.Amount) end  from TPayrollOtherDeduction b " +
                                                "left join  TPayrollOtherDeductionLine c on b.id=c.PayOtherDeduction_id " +
                                                "where c.Emp_id=" + dr["EmployeeId"] + " and b.payroll_id=" + dr["payid"] + " and c.OtherDeduction_id=a.id)loanamt " +
                                                "from MOtherDeduction a ");
                grid_view.DataSource = dtotherdeductions;
                grid_view.DataBind();

                foreach (GridViewRow gr in grid_view.Rows)
                {
                    totaldeductions = totaldeductions + decimal.Parse(gr.Cells[1].Text);
                }
                lbl_td.Text = Math.Round(totaldeductions, 2).ToString();
                lbl_oint.Text = Math.Round(decimal.Parse(dr["TotalOtherIncomeNonTaxable"].ToString()), 2).ToString();
                lbl_np.Text = Math.Round(decimal.Parse(lbl_gp.Text) - decimal.Parse(lbl_td.Text) + decimal.Parse(lbl_oint.Text), 2).ToString("#,###.##");

%>
 
<table class="box left">
    <tr>
        <td>ID NUMBER</td>
        <td>:</td>
        <td><asp:Label ID="lbl_idnumber" runat="server" Text="Id Number"></asp:Label></td>
    </tr>
    <tr>
        <td>EMPLOYEE NAME</td>
        <td>:</td>
        <td><asp:Label ID="lbl_empname" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
    <tr>
        <td>TAX TABLE</td>
        <td>:</td>
        <td><asp:Label ID="lbl_payrolltype" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
        <tr>
        <td>
            <asp:Label ID="lbl_payrate" runat="server" Text="Label"></asp:Label></td>
        <td>:</td>
        <td><asp:Label ID="lbl_rate" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
</table>
<table class="box right">
    <tr>
        <td>DATE START</td>
        <td>:</td>
        <td><asp:Label ID="lbl_start" runat="server" Text="Id Number"></asp:Label></td>
    </tr>
    <tr>
        <td>DATE END</td>
        <td>:</td>
        <td><asp:Label ID="lbl_end" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
    <tr>
        <td>DEPARTMENT</td>
        <td>:</td>
        <td><asp:Label ID="lbl_deptpartment" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
        <tr>
        <td>POSITION</td>
        <td>:</td>
        <td><asp:Label ID="lbl_position" runat="server" Text="EMPLOYEE NAME"></asp:Label></td>
    </tr>
</table>
<div class="clearfix"></div>
<table class="box left">
    <tr>
        <th>EARNINGS</th>
        <th>AMOUNT</th>
    </tr>
    <tr>
        <td>Late(<asp:Label ID="lbl_lhrs" runat="server" Text="Label"></asp:Label>)</td>
        <td><asp:Label ID="lbl_late" runat="server" Text="Label"></asp:Label></td>
    </tr>
     <tr>
        <td>Undertime(<asp:Label ID="lbl_uhrs" runat="server" Text="Label"></asp:Label>)</td>
        <td><asp:Label ID="lbl_undertime" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>Absent(<asp:Label ID="lbl_absentdays" runat="server" Text="Label"></asp:Label>)</td>
        <td><asp:Label ID="lbl_absent" runat="server" Text="Label"></asp:Label></td>
    </tr>
        <tr>
        <td>Regular Amount(<asp:Label ID="lbl_rahrs" runat="server" Text="Label"></asp:Label>)</td>
        <td><asp:Label ID="lbl_reg_amt" runat="server" Text="Label"></asp:Label></td>
    </tr>
        <tr>
        <td>OVERTIME(<asp:Label ID="lbl_othrs" runat="server" Text="Label"></asp:Label>)</td>
        <td><asp:Label ID="lbl_ot" runat="server" Text="Label"></asp:Label></td>
    </tr>
        <tr>
        <td>Leave w/ Pay</td>
        <td><asp:Label ID="lbl_lpay" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>Restday Pay</td>
        <td><asp:Label ID="lbl_rdpay" runat="server" Text="Label"></asp:Label></td>
    </tr>
        <tr>
        <td>Holliday Pay</td>
        <td><asp:Label ID="lbl_hp" runat="server" Text="Label"></asp:Label></td>
    </tr>
                         
        <tr>
        <td>Other Income (Taxable)</td>
        <td><asp:Label ID="lbl_oit" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>Gross Pay</td>
        <td><asp:Label ID="lbl_gp" runat="server" Text="Label"></asp:Label></td>
    </tr>
</table>
<table class="box right">
    <tr>
        <th>Deductions</th>
        <th>AMOUNT</th>
    </tr>
    <tr>
        <td>SSS Premium</td>
        <td><asp:Label ID="lbl_deduction_sss" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>PHIL. Premium</td>
        <td><asp:Label ID="lbl_phil" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>HDMF Premium</td>
        <td><asp:Label ID="lbl_hdmf" runat="server" Text="Label"></asp:Label></td>
    </tr>
        <tr>
        <td>Withholding Tax</td>
        <td><asp:Label ID="lbl_whtt" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="grid_view" runat="server" style=" width:100%;" AutoGenerateColumns="false" CssClass="Grid">
                <Columns>
                        <asp:BoundField DataField="OtherDeduction" HeaderText="Other Deductions"/>
                        <asp:BoundField DataField="loanamt" HeaderText="Amount"/>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<div class="clearfix"></div>
<table style=" font-weight:bold">
    <tr>
        <td style=" width:200px">Total Deductions</td>
        <td style=" width:20px">:</td>
        <td><asp:Label ID="lbl_td" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>Other Income (Non Taxable)</td>
        <td>:</td>
        <td><asp:Label ID="lbl_oint" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
        <td>Net Pay</td>
            <td>:</td>
            <td><asp:Label ID="lbl_np" runat="server" Text="Label"></asp:Label></td>
    </tr>
</table>
<br />
<%
    if (cnt < dt.Rows.Count - 1)
    { 
        %>
         <hr />
        <%
    }
%>
<br />
 <% 
     cnt++;
            }
        }
        else
        { 
     %>
        <div class="alert alert-empty">
            <i class="fa fa-info-circle"></i>
            <span>Payroll not yet process</span>
        </div>
        
     <%    }
     %>
 </div>
        </div>
    </div>
</div>







</asp:Content>
