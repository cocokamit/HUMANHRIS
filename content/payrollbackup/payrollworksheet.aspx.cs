using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;

public partial class content_payroll_payrollworksheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            this.disp();
    }

    protected void disp()
    {
        string qq = "select b.id empid, " +
        "b.LastName+', '+b.FirstName+' '+b.MiddleName EMPLOYEE,c.department DEPARTMENT,case when len(d.store)>0 then d.store else '--' end Outlet, case when LEN(ATMAccountNumber) > 2 then convert(varchar,ATMAccountNumber,100) else '-' end atm, case when a.PayrollTypeId='1' then a.payrollrate else a.dailyrate end RATE, " +
        "(select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1')t_reg_hrs,(case when a.PayrollTypeId=1 then case when (select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1')>0 then  a.payrollrate-(a.totaltardyamount + a.totalabsentamount) else 0 end else (select SUM(regamt) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1') end) t_reg_amt, " + //(select SUM(regamt) from TPayrollLineBreakDown where payrolllineid=a.Id)
        "case when a.totalleavehrs IS null then 0 else a.totalleavehrs end totalleavehrs ,a.totalleavepay, " +
        "(select SUM(regothrs+nightothrs) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_hrs,(select SUM(regotamt+nightotamt) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_amt, " +
        "(select SUM(nighthrs) from TPayrollLineBreakDown where payrolllineid=a.Id)night_hrs,(select SUM(nightpremium) from TPayrollLineBreakDown where payrolllineid=a.Id)night_amt, " +
        "(select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20)tholidayhrs,(select SUM(regamt+hdpremium) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20)hd_amt, " +
        "(select SUM(reghrs) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20 )rd_hrs,(select SUM(regamt+hdpremium) from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20)rd_amt, " +
        "a.GrossIncome GROSS_INCOME,a.SSSContribution SSS_CONTRIBUTION,a.PHICContribution PHIC_CONTRIBUTION,a.HDMFContribution HDMF_CONTRIBUTION,a.Tax WHT, " +
        "a.NetIncome NET_INCOME,(ssseccontributionemployer + ssscontributionemployer)Employer_SSS_Contribution,phiccontributionemployer Employer_PHIC_Contribution,hdmfcontributionemployer Employer_HDMF_Contribution " +
        "from TPayrollLine a " +
        "left join memployee b on a.EmployeeId=b.Id " +
        "left join mdepartment c on b.departmentid=c.Id " +
        "left join mstore d on b.store_id=d.Id " +
        "where a.PayrollId=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "  " +
        "and b.FirstName+''+b.MiddleName+''+b.LastName+''+b.IdNumber like'%" + txt_search.Text + "%' " +
        "order by b.LastName asc";
        DataTable dt = dbhelper.getdata(qq);

        DataTable dtOIT = dbhelper.getdata("select  a.Id,a.otherincome from MOtherIncome a  left join MOtherIncomeTaxRule b on a.taxruleid=b.id  where a.action is null  and istaxable='True' or (b.Ceilingg>0 and istaxable='False')");
        ViewState["dtOIT"] = dtOIT;
        string queryOI = "select b.otherincome_id, b.emp_id, " +
                      "case when b.otherincome_id = 0 then 'Additional Income' else (select otherincome from motherincome where ID=b.otherincome_id )end OtherIncome, " +
                      "b.taxable_amt , b.nontaxable_amt " +
                      "from TPayrollOtherIncome a " +
                      "left join TPayrollOtherIncomeLine b on a.id=b.payotherincome_id " +
                      "left join tpayroll c on a.id=c.payrollotherincomeid " +
                      "where a.action is null and c.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "  ";//and b.emp_id=" + dr["EmployeeId"] + "

        //string queryOI = "select a.emp_id, b.otherincome,a.amount,b.id otherincome_id from TPayrollOtherIncomeLine a left join MOtherIncome b on a.OtherIncome_id=b.Id left join tpayroll c on a.id=c.payrollotherincomeid where a.action is null and c.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + " ";
        DataTable dtotherincome = dbhelper.getdata(queryOI);
        foreach (DataRow dr in dtOIT.Rows)
        {
            if (!dt.Columns.Contains("" + dr["otherincome"].ToString() + ""))
            {
                dt.Columns.Add(new DataColumn(dr["otherincome"].ToString(), typeof(string)));
            }
            int iii = 0;
            foreach (DataRow dtr in dt.Rows)
            {
                DataRow[] dtdr = dtotherincome.Select("emp_id=" + dtr["empid"] + " and otherincome_id=" + dr["id"] + "");
                if (dtdr.Count() > 0)
                {
                    decimal total_income = 0;
                    for (int ik = 0; ik < dtdr.Count(); ik++)
                    {
                        decimal hjhj = decimal.Parse(dtdr[ik]["taxable_amt"].ToString().Length > 0 ? dtdr[ik]["taxable_amt"].ToString() : "0");
                        total_income = total_income + hjhj;
                    }
                    dt.Rows[iii]["" + dr["otherincome"].ToString() + ""] = total_income;// total_deduction;
                }
                else
                    dt.Rows[iii]["" + dr["otherincome"].ToString() + ""] = "0.00";
                iii++;
            }

        }
        Session["DTOITCHECKING"] = 0;
        if (dtOIT.Rows.Count > 0)
        {
            Session["DTOITCHECKING"] = 1;
            for (int oit = 0; oit <= dtOIT.Rows.Count; oit++) // DataRow dr in dtOIT.Rows)
            {
                dt.Columns[18].SetOrdinal(dt.Columns.Count-1);
            }
        }

        DataTable dtod = dbhelper.getdata("select  a.Id,a.OtherDeduction  from MOtherDeduction a ");
        string hhh = "select b.otherdeduction_id, b.emp_id, " +
                     "case when b.otherdeduction_id = 0 then 'Additional Deduction' else (select otherdeduction from motherdeduction where ID=b.otherdeduction_id )end OtherDeduction, " +
                     "b.amount loanamt " +
                     "from TPayrollOtherDeduction a " +
                     "left join TPayrollOtherDeductionline b on a.id=b.payotherdeduction_id " +
                     "left join tpayroll c on a.id=c.payrollotherdeductionid " +
                     "where a.action is null and c.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "";
        //and b.emp_id=" + dr["EmployeeId"] + "
        System.Data.DataTable dtotherdeductions = dbhelper.getdata(hhh);
        foreach (DataRow dr in dtod.Rows)
        {
            if (!dt.Columns.Contains("" + dr["OtherDeduction"].ToString() + ""))
            {
                dt.Columns.Add(new DataColumn(dr["OtherDeduction"].ToString(), typeof(string)));
            }
            int iii = 0;
            foreach (DataRow dtr in dt.Rows)
            {
                DataRow[] dtdr = dtotherdeductions.Select("emp_id=" + dtr["empid"] + " and otherdeduction_id=" + dr["id"] + "");
                if (dtdr.Count() > 0)
                {
                    decimal total_deduction = 0;
                    for (int ik = 0; ik < dtdr.Count(); ik++)
                    {
                        decimal hjhj = decimal.Parse(dtdr[ik]["loanamt"].ToString());
                        total_deduction = total_deduction + hjhj;
                    }
                    dt.Rows[iii]["" + dr["OtherDeduction"].ToString() + ""] = total_deduction;
                }
                else
                    dt.Rows[iii]["" + dr["OtherDeduction"].ToString() + ""] = "0.00";
                iii++;
            }
        }
        Session["dtodchecking"] = 0;
        if (dtod.Rows.Count > 0)
        {
            Session["dtodchecking"] = 1;
            for (int od = 0; od <= (dtod.Rows.Count + 7); od++)// DataRow dr in dtOIT.Rows)
            {
                dt.Columns[18 + dtOIT.Rows.Count + 5].SetOrdinal(dt.Columns.Count - 1);
            }
        }
        DataTable dtOINT = dbhelper.getdata("select  a.Id,a.otherincome from MOtherIncome a  left join MOtherIncomeTaxRule b on a.taxruleid=b.id where a.action is null  and istaxable='False' and b.Ceilingg=0");
        ViewState["dtOINT"] = dtOINT;
        foreach (DataRow dr in dtOINT.Rows)
        {
            if (!dt.Columns.Contains("" + dr["otherincome"].ToString() + ""))
            {
                dt.Columns.Add(new DataColumn(dr["otherincome"].ToString(), typeof(string)));
            }
            int iii = 0;
            foreach (DataRow dtr in dt.Rows)
            {
                DataRow[] dtdr = dtotherincome.Select("emp_id=" + dtr["empid"] + " and otherincome_id=" + dr["id"] + "");
                if (dtdr.Count() > 0)
                {
                    decimal total_income = 0;
                    for (int ik = 0; ik < dtdr.Count(); ik++)
                    {
                        decimal hjhj = decimal.Parse(dtdr[ik]["nontaxable_amt"].ToString().Length > 0 ? dtdr[ik]["nontaxable_amt"].ToString() : "0");
                        total_income = total_income + hjhj;
                    }
                    dt.Rows[iii]["" + dr["otherincome"].ToString() + ""] = total_income;// total_deduction;
                }
                else
                    dt.Rows[iii]["" + dr["otherincome"].ToString() + ""] = "0.00";
                iii++;
            }
        }
        Session["dtOINTchecking"] = 0;
        if (dtOINT.Rows.Count > 0)
        {
            Session["dtOINTchecking"] = 1;
            for (int oint = 0; oint <= dtOINT.Rows.Count + 7; oint++)// DataRow dr in dtOIT.Rows)
            {
                dt.Columns[18 + dtOIT.Rows.Count + 5 + dtod.Rows.Count].SetOrdinal(dt.Columns.Count - 1);
            }
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        if (GridView1.Columns.Count > 0)
            GridView1.Columns[0].Visible = false;
        else
        {
            GridView1.HeaderRow.Cells[0].Visible = false;
            foreach (GridViewRow grow in GridView1.Rows)
            {
                grow.Cells[0].Visible = false;
            }
        }

        //GridView1.Columns[0].ItemStyle.CssClass = "hide";
        //GridView1.Columns[0].HeaderStyle.CssClass = "hide";

    }
    protected void search(object sender, EventArgs e)
    {
        this.disp();
    }
    decimal netinc = 0;decimal nti = 0;    decimal od = 0;    decimal wt = 0;    decimal hc = 0;    decimal pc = 0;    decimal sc = 0;    decimal gp = 0;    decimal ti = 0;
    decimal na = 0;    decimal hp = 0;    decimal lp = 0;    decimal ra = 0;    decimal oa = 0;    decimal tg = 0;    decimal twh = 0;    decimal taa = 0;    decimal tta = 0;
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            Label lbl_NetIncome = (Label)e.Row.FindControl("lbl_NetIncome");
            Label lbl_nti = (Label)e.Row.FindControl("lbl_nti");
            Label lbl_od = (Label)e.Row.FindControl("lbl_od");
            Label lbl_wt = (Label)e.Row.FindControl("lbl_wt");
            Label lbl_hc = (Label)e.Row.FindControl("lbl_hc");
            Label lbl_pc = (Label)e.Row.FindControl("lbl_pc");
            Label lbl_sc = (Label)e.Row.FindControl("lbl_sc");
            Label lbl_gp = (Label)e.Row.FindControl("lbl_gp");
            Label lbl_ti = (Label)e.Row.FindControl("lbl_ti");
            Label lbl_na = (Label)e.Row.FindControl("lbl_na");
            Label lbl_hp = (Label)e.Row.FindControl("lbl_hp");
            Label lbl_lp = (Label)e.Row.FindControl("lbl_lp");
            Label lbl_ra = (Label)e.Row.FindControl("lbl_ra");
            Label lbl_oa = (Label)e.Row.FindControl("lbl_oa");
            Label lbl_tg = (Label)e.Row.FindControl("lbl_tg");
            Label lbl_twh = (Label)e.Row.FindControl("lbl_twh");
            Label lbl_taa = (Label)e.Row.FindControl("lbl_taa");
            Label lbl_tta = (Label)e.Row.FindControl("lbl_tta");

            netinc = netinc + decimal.Parse(lbl_NetIncome.Text);
            nti = nti + decimal.Parse(lbl_nti.Text);
            od = od + decimal.Parse(lbl_od.Text);
            wt = wt + decimal.Parse(lbl_wt.Text);
            hc = hc + decimal.Parse(lbl_hc.Text);
            pc = pc + decimal.Parse(lbl_pc.Text);
            sc = sc + decimal.Parse(lbl_sc.Text);
            gp = gp + decimal.Parse(lbl_gp.Text);
            ti = ti + decimal.Parse(lbl_ti.Text);
            na = na + decimal.Parse(lbl_na.Text);
            hp = hp + decimal.Parse(lbl_hp.Text);
            lp = lp + decimal.Parse(lbl_lp.Text);
            ra = ra + decimal.Parse(lbl_ra.Text);
            oa = oa + decimal.Parse(lbl_oa.Text);
            tg = tg + decimal.Parse(lbl_tg.Text);
            twh = twh + decimal.Parse(lbl_twh.Text);
            taa = taa + decimal.Parse(lbl_taa.Text);
            tta = tta + decimal.Parse(lbl_tta.Text);

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbl_f_netincome = (Label)e.Row.FindControl("lbl_f_netincome");
            Label lbl_f_nti = (Label)e.Row.FindControl("lbl_f_nti");
            Label lbl_f_od = (Label)e.Row.FindControl("lbl_f_od");
            Label lbl_f_wt = (Label)e.Row.FindControl("lbl_f_wt");
            Label lbl_f_hc = (Label)e.Row.FindControl("lbl_f_hc");
            Label lbl_f_pc = (Label)e.Row.FindControl("lbl_f_pc");
            Label lbl_f_sc = (Label)e.Row.FindControl("lbl_f_sc");
            Label lbl_f_gp = (Label)e.Row.FindControl("lbl_f_gp");
            Label lbl_f_ti = (Label)e.Row.FindControl("lbl_f_ti");
            Label lbl_f_na = (Label)e.Row.FindControl("lbl_f_na");
            Label lbl_f_hp = (Label)e.Row.FindControl("lbl_f_hp");
            Label lbl_f_lp = (Label)e.Row.FindControl("lbl_f_lp");
            Label lbl_f_ra = (Label)e.Row.FindControl("lbl_f_ra");
            Label lbl_f_oa = (Label)e.Row.FindControl("lbl_f_oa");
            Label lbl_f_tg = (Label)e.Row.FindControl("lbl_f_tg");
            Label lbl_f_twh = (Label)e.Row.FindControl("lbl_f_twh");
            Label lbl_f_taa = (Label)e.Row.FindControl("lbl_f_taa");
            Label lbl_f_tta = (Label)e.Row.FindControl("lbl_f_tta");

          
            lbl_f_netincome.Text = string.Format("{0:n2}", netinc);
            lbl_f_nti.Text = nti.ToString();
            lbl_f_od.Text = od.ToString();
            lbl_f_wt.Text = wt.ToString();
            lbl_f_hc.Text = hc.ToString();
            lbl_f_pc.Text = pc.ToString();
            lbl_f_sc.Text = sc.ToString();
            lbl_f_gp.Text = gp.ToString();
            lbl_f_ti.Text = ti.ToString();
            lbl_f_na.Text = na.ToString();
            lbl_f_hp.Text = hp.ToString();
            lbl_f_lp.Text = lp.ToString();
            lbl_f_ra.Text = ra.ToString();
            lbl_f_oa.Text = oa.ToString();
            lbl_f_tg.Text = tg.ToString();
            lbl_f_twh.Text = twh.ToString();
            lbl_f_taa.Text = taa.ToString();
            lbl_f_tta.Text = tta.ToString();
        }
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select 'T-'+convert(varchar,a.id)+'-'+convert(varchar,CONVERT(date,b.DateStart))+'-'+convert(varchar,CONVERT(date,b.DateEnd))filename from tpayroll a left join tdtr b on a.DTRId=b.Id where a.id=" + function.Decrypt(Request.QueryString["payid"].ToString(), true) + "");
        if (dt.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["filename"].ToString() + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                this.disp();
                GridView1.Style.Add("text-transform", "uppercase");
                GridView1.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView1.HeaderRow.Cells)
                {
                    cell.BackColor = GridView1.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GridView1.RenderControl(hw);
                string style = @"<style> .textmode { text-transform:uppercase } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
            Response.Write("<script>alert('No Data to be downloaded!')</script>");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void OnRowDataBoundgrid1(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gvr = e.Row;
        if (gvr.RowType == DataControlRowType.Header)
        {



            e.Row.Cells[6].Text = "HRS.";
            e.Row.Cells[8].Text = "HRS.";
            e.Row.Cells[10].Text = "HRS.";
            e.Row.Cells[12].Text = "HRS.";
            e.Row.Cells[14].Text = "HRS.";
            e.Row.Cells[16].Text = "HRS.";


            e.Row.Cells[7].Text = "AMT.";
            e.Row.Cells[9].Text = "AMT.";
            e.Row.Cells[11].Text = "AMT.";
            e.Row.Cells[13].Text = "AMT.";
            e.Row.Cells[15].Text = "AMT.";
            e.Row.Cells[17].Text = "AMT.";



            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();

            cell.ColumnSpan = 5;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = " ";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "REGULAR";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "LEAVE";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "OVERTIME";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "NSD";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "HOLLIDAY";
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "RESTDAY";
            row.Cells.Add(cell);
            if (Session["DTOITCHECKING"].ToString() == "1")
            {
                DataTable dtOIT = ViewState["dtOIT"] as DataTable;
                cell = new TableHeaderCell();
                cell.ColumnSpan = dtOIT.Rows.Count;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "Taxable Income";
                row.Cells.Add(cell);
            }
            cell = new TableHeaderCell();
            cell.ColumnSpan = Session["DTOITCHECKING"].ToString() == "1"?1:2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = " ";
            row.Cells.Add(cell);
            

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Gov't Mandated";
            row.Cells.Add(cell);

            DataTable dtod = dbhelper.getdata("select  a.Id,a.OtherDeduction  from MOtherDeduction a ");
            if (dtod.Rows.Count > 0)
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = dtod.Rows.Count;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "Other Deduction";
                row.Cells.Add(cell);
            }

            DataTable dtOINT = ViewState["dtOINT"] as DataTable;
            if (dtOINT.Rows.Count > 0)
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = dtOINT.Rows.Count;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "Non-Taxable Income";
                row.Cells.Add(cell);
                GridView1.Controls[0].Controls.AddAt(0, row);
            }

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = " ";
            row.Cells.Add(cell);
            GridView1.Controls[0].Controls.AddAt(0, row);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Employer";
            row.Cells.Add(cell);
            GridView1.Controls[0].Controls.AddAt(0, row);
        }
    }
}