using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_report_Control_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
            if (grid_view.Rows.Count > 0)
                exp_report.Visible = true;
            else
                exp_report.Visible = false;
        }
    }
    protected void loadable()
    {
        string query = "select id, LEFT(CONVERT(varchar,datestart,101),10)datestart,LEFT(CONVERT(varchar,DateEnd,101),10)dateend,payroll_id from tdtr where payroll_id is not null";
        DataTable dt = dbhelper.getdata(query);
        ddl_range.Items.Clear();
        ddl_range.Items.Add(new ListItem("Select","0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_range.Items.Add(new ListItem(dr["datestart"].ToString() + "-" + dr["dateend"].ToString(), dr["payroll_id"].ToString()));
        }

    }
    protected void select_range(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {
        string query = "CREATE TABLE pvt (BasicPay decimal(18,2), " +
        "LeaveWithPay decimal(18,2), " +
        "Overtime decimal(18,2), " +
        "NightPremium decimal(18,2),  " +
        "Holliday decimal(18,2), " +
        "Restday decimal(18,2), " +
       // "gross decimal(18,2), " +
        "OtherIncomeTaxable decimal(18,2), " +
        "OtherIncomeNonTaxable decimal(18,2), " +
        "SSS decimal(18,2), " +
        "PHIC decimal(18,2), " +
        "HDMF decimal(18,2),WHT decimal(18,2)); " +
        "declare @Basic_Pay decimal(18,2) " +
        "declare @Leave_with_pay decimal(18,2)  " +
        "declare @Overtime decimal(18,2)  " +
        "declare @Night_Premium decimal(18,2)  " +
        "declare @Holliday decimal(18,2) " +
        "declare @Restday decimal(18,2) " +
        "declare @gross decimal(18,2) " +
        "declare @TI decimal(18,2) " +
        "declare @NTI decimal(18,2) " +
        "declare @SSS decimal(18,2) " +
        "declare @PHIC decimal(18,2)  " +
        "declare @HDMF decimal(18,2)  " +
        "declare @wht decimal(18,2)  " +
        "select @Basic_Pay=SUM(t_reg_amt),@Leave_with_pay=SUM(totalleavepay),@Overtime=SUM(ot_amt),@Night_Premium=SUM(night_amt),@Holliday=SUM(hd_amt),@Restday=SUM(rd_amt),@NTI=SUM(NTI),@SSS=SUM(SSS_CONTRIBUTION),@PHIC=SUM(PHIC_CONTRIBUTION),@HDMF=SUM(HDMF_CONTRIBUTION),@wht=SUM(WHT),@TI=SUM(OTHER_INCOME_TAXABLE) " +
        "from  " +
        "( " +
        "select   " +
        "(case when a.PayrollTypeId=1 then case when (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id)>0 then  case when a.payrollrate-(a.totaltardyamount+a.totalabsentamount+a.totalleavepay)>0 then a.payrollrate-(a.totaltardyamount+a.totalabsentamount+a.totalleavepay)else 0 end else 0 end else (select SUM(regamt) from TPayrollLineBreakDown where payrolllineid=a.Id) end) t_reg_amt,  " +
        "a.totalleavepay, " +
        "(select SUM(regotamt+nightotamt) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_amt,  " +
        "(select SUM(nightpremium) from TPayrollLineBreakDown where payrolllineid=a.Id)night_amt, " +
        "(select SUM(hdpremium) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20)hd_amt, " +
        "(select SUM(regamt+hdpremium) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20)rd_amt, " +

        "a.grossincome gross, " +

        "a.TotalOtherIncomeNonTaxable NTI, " +
        "a.TotalOtherIncomeTaxable OTHER_INCOME_TAXABLE, " +
        "a.SSSContribution SSS_CONTRIBUTION, " +
        "a.PHICContribution PHIC_CONTRIBUTION, " +
        "a.HDMFContribution HDMF_CONTRIBUTION, " +
        "a.Tax WHT " +
        "from TPayrollLine a left join memployee b on a.EmployeeId=b.Id left join mdepartment c on b.departmentid=c.Id " +
        "where a.PayrollId="+ddl_range.SelectedValue+"   " +
        ") control_report " +
        "insert into pvt " +
        "values (@Basic_Pay,@Leave_with_pay,@Overtime,@Night_Premium,@Holliday,@Restday,@TI,@NTI,@SSS,@PHIC,@HDMF,@wht) " +
        "SELECT  Description,Income " +
        "FROM  " +
        "( " +
        "SELECT  BasicPay, LeaveWithPay,Overtime, NightPremium, Holliday,Restday,SSS,PHIC,HDMF,WHT,OtherIncomeNonTaxable,OtherIncomeTaxable FROM pvt " +
        ") p " +
        "UNPIVOT " +
        "(Income FOR Description IN  " +
        "(BasicPay, LeaveWithPay,Overtime, NightPremium, Holliday,Restday,SSS,PHIC,HDMF,WHT,OtherIncomeNonTaxable,OtherIncomeTaxable) " +
        ")AS unpvt " +
        "drop table pvt";

        DataTable dt = dbhelper.getdata(query);
        //int dtoi = dt.Rows.Count;
        //string query1 = "select OtherIncome_id,Otherincome,SUM(incomeamt)incomeamt from (select b.OtherIncome_id, b.emp_id, " + 
        //"case when b.OtherIncome_id = 0 then 'Additional income' else (select OtherIncome from motherincome where ID=b.OtherIncome_id )end Otherincome, " + 
        //" b.amount incomeamt from TPayrollOtherIncome a " + 
        //" left join TPayrollOtherIncomeLine b on a.id=b.PayOtherIncome_id  " + 
        //" left join tpayroll c on a.id=c.PayrollOtherIncomeId  " +
        //" where a.action is null and c.id=" + ddl_range.SelectedValue + ") oootherincome group by OtherIncome_id,Otherincome ";

        //DataTable dtoincome = dbhelper.getdata(query1);
        //DataRow driof;
        //foreach (DataRow dr in dtoincome.Rows)
        //{
        //    driof = dt.NewRow();
        //    driof["description"] = dr["Otherincome"];
        //    dt.Rows.Add(driof);
        //    dt.Rows[dtoi]["Income"] = dr["incomeamt"].ToString();
        //    dtoi++;
        //}
        int dtcnr=dt.Rows.Count;
        dt.Columns.Add(new DataColumn("Deduction", typeof(string)));
        DataTable dtod = dbhelper.getdata("select a.Id,a.OtherDeduction  from MOtherDeduction a ");
        string hhh = "select OtherDeduction_id,OtherDeduction,SUM(loanamt)loanamt from (select b.otherdeduction_id, b.emp_id, " +
                    "case when b.otherdeduction_id = 0 then 'Additional Deduction' else (select otherdeduction from motherdeduction where ID=b.otherdeduction_id )end OtherDeduction, " +
                    "b.amount loanamt " +
                    "from TPayrollOtherDeduction a " +
                    "left join TPayrollOtherDeductionline b on a.id=b.payotherdeduction_id " +
                    "left join tpayroll c on a.id=c.payrollotherdeductionid " +
                    "where a.action is null and c.id=" + ddl_range.SelectedValue + ")ooootherdeduction group by OtherDeduction_id,OtherDeduction ";  //and b.emp_id=" + dr["EmployeeId"] + "
        System.Data.DataTable dtotherdeductions = dbhelper.getdata(hhh);
        DataRow drf;
        foreach (DataRow dr in dtod.Rows)
        {
            drf = dt.NewRow();
            drf["description"] = dr["OtherDeduction"];
            dt.Rows.Add(drf);
            DataRow[] dtdr = dtotherdeductions.Select("otherdeduction_id=" + dr["id"] + "");
            if (dtdr.Count() > 0)
                dt.Rows[dtcnr]["Deduction"] = dtdr[0]["loanamt"].ToString();
            else
                dt.Rows[dtcnr]["Deduction"] = "0.00";
        dtcnr++;
        }

        //clone specific row to other table
        DataTable dtnormalgovt = dt.Clone();
        int cnttt = 0;
        for (int tt = 6; tt <= 9; tt++)
        {
           
            DataRow drnormalgovt = dtnormalgovt.NewRow();
            drnormalgovt.ItemArray = dt.Rows[tt].ItemArray;
            dtnormalgovt.Rows.Add(drnormalgovt);
            dtnormalgovt.Rows[cnttt]["Deduction"] = dtnormalgovt.Rows[cnttt]["Income"];
            dtnormalgovt.Rows[cnttt]["Income"] = "0.00";
            cnttt++;
        }

        //eleiminate govt mandatory deduction
        dt.Rows.Remove(dt.Rows[6]);
        dt.Rows.Remove(dt.Rows[6]);
        dt.Rows.Remove(dt.Rows[6]);
        dt.Rows.Remove(dt.Rows[6]);

        //merge dtnormalgovt to dt
        dt.Merge(dtnormalgovt);

        int jj=0;
        foreach (DataRow fdr in dt.Rows)
        {
            dt.Rows[jj]["Income"] = dt.Rows[jj]["Income"].ToString().Length > 0 ? dt.Rows[jj]["Income"].ToString() : "0.00";
            dt.Rows[jj]["Deduction"] = dt.Rows[jj]["Deduction"].ToString().Length>0?dt.Rows[jj]["Deduction"].ToString():"0.00";
                jj++;
        }

        grid_view.DataSource = dt;
        grid_view.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "SSS_Contribution_" + ddl_range.SelectedItem.Text;
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            //grid_view.AllowPaging = false;
            //this.disp();
            // this.DataBind();
            //grid_view.DataBind();
            //this.BindGrid();

            grid_view.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in grid_view.HeaderRow.Cells)
            {
                cell.BackColor = grid_view.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grid_view.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grid_view.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grid_view.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grid_view.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }
    protected void gridrowbound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[1].Text = e.Row.Cells[1].Text.Length == 0 ? "0.00" : e.Row.Cells[1].Text;
        //}
    }
}