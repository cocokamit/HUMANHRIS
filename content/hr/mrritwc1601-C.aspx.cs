using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_hr_mrritwc1601_C : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
            load();
        }
    }
    protected void load()
    {
        DataTable dt;
        string query = "";

        query = "select * from MMonth order by Id";
        dt = dbhelper.getdata(query);
        ddlmonth.Items.Clear();
        foreach (DataRow drc in dt.Rows)
        {
            ddlmonth.Items.Add(new ListItem(drc["Month"].ToString(), drc["Id"].ToString()));
            ddl_montly.Items.Add(new ListItem(drc["Month"].ToString(), drc["Id"].ToString()));
        }

        query = "select * from MYear order by Id";
        dt = dbhelper.getdata(query);
        ddlyear.Items.Clear();
        ddl_year_search.Items.Clear();
        foreach (DataRow drc in dt.Rows)
        {
            ddlyear.Items.Add(new ListItem(drc["Year"].ToString(), drc["Id"].ToString()));
            ddl_year_search.Items.Add(new ListItem(drc["Year"].ToString(), drc["Id"].ToString()));
        }

        query = "select * from mcompany";
        dt = dbhelper.getdata(query);
        foreach (DataRow drc in dt.Rows)
        {
            ddl_company.Items.Add(new ListItem(drc["company"].ToString(), drc["id"].ToString()));
        }
        ViewState["company"] = dt;
    }
    protected void search(object sender, EventArgs e)
    {
        string query = "select a.id,(select Month from MMonth where Id = a.id)month,a.yyyy,a.companyname from alphalist1601C a where action is null and a.yyyy = " + ddl_year_search.SelectedItem.ToString() + " and a.mm = " + ddl_montly.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        grid_alpha_trn.DataSource = dt;
        grid_alpha_trn.DataBind();
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id,(select Month from MMonth where Id = a.id)month,a.yyyy,a.companyname from alphalist1601C a where action is null");
        grid_alpha_trn.DataSource = dt;
        grid_alpha_trn.DataBind();
    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("mrritw");
    }

    protected void ttos(bool oi)
    {
        ttospo.Visible = oi;
        ttosppp.Visible = oi;
    }
    protected void processannualization(object sender, EventArgs e)
    {
        ttosppp.Style.Add("width", "1150px");
        ttosppp.Style.Add("left", "250px");
        ttosppp.Style.Add("top", "5%");
        ttos(true);
    }
    protected void processsss(object sender, EventArgs e)
    {
        string test = "";
        string query = "select sum(OTMEAL)OTMEAL,sum(CONVERT(decimal,replace(mr,',','')))mr,sum(RATE)RATE,sum(totallateamt)totallateamt,sum(totalundertimeamt)totalundertimeamt,sum(TotalAbsentAmount)TotalAbsentAmount,sum(t_reg_amt)t_reg_amt,sum(totalleavepay)totalleavepay, sum(ot_amt)ot_amt,sum(night_amt)night_amt,sum(hd_amt)hd_amt,sum(rd_amt)rd_amt,sum(case when NET_INCOME >0 then OIT else 0 end)OIT,sum(GROSS_INCOME)GROSS_INCOME,sum(SSS_CONTRIBUTION)SSS_CONTRIBUTION,(case when (sum(SSS_MPF))is null then '0.00' else SUM(SSS_MPF)end)SSS_MPF,sum(SSStotal)SSSTotal,sum(PHIC_CONTRIBUTION)PHIC_CONTRIBUTION,sum(HDMF_CONTRIBUTION)HDMF_CONTRIBUTION,sum(WHT)WHT,sum(case when NET_INCOME >0 then OD else 0 end)OD,sum(case when NET_INCOME >0 then OINT else 0 end)OINT,sum(NET_INCOME)NET_INCOME,sum(TotalGross)TotalGross from(select ROW_NUMBER()OVER(Order by a.Id)AS RowNumber,b.LastName+', '+b.FirstName+' '+b.MiddleName EMPLOYEE,c.department DEPARTMENT,case when(select SUM(otmeal)from TDTRLine where EmployeeId = b.Id and DTRId = e.DTRId)is null then '0.00' else(select SUM(otmeal)from TDTRLine where EmployeeId = b.Id and DTRId = e.DTRId)end OTMEAL,b.monthlyrate mr,case when a.PayrollTypeId='1'then a.payrollrate else a.dailyrate end RATE,a.totallateamt,a.totalundertimeamt,a.TotalAbsentAmount,(case when a.PayrollTypeId=1 then case when (select SUM(reghrs)from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid='1')>0 then  a.payrollrate-(a.totaltardyamount + a.totalabsentamount + totalleavepay)else 0 end else(select SUM(regamt)from TPayrollLineBreakDown where payrolllineid=a.Id)end)t_reg_amt,a.totalleavepay,(select SUM(regotamt+nightotamt) from TPayrollLineBreakDown where payrolllineid=a.Id)ot_amt,(select SUM(nightpremium)from TPayrollLineBreakDown where payrolllineid=a.Id)night_amt,(select SUM(hdpremium)from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid>1 and daytypeid<20)hd_amt,(select SUM(hdpremium)from TPayrollLineBreakDown where payrolllineid=a.Id and daytypeid=20)rd_amt,(select case when sum(y.taxable_amt)is null then 0 else sum(y.taxable_amt)end from TPayrollOtherIncome z left join TPayrollOtherIncomeLine y on z.id=y.payotherincome_id where z.action is null and z.payroll_id=a.PayrollId and y.Emp_id=a.EmployeeId)OIT,a.GrossIncome GROSS_INCOME,a.SSSContribution SSS_CONTRIBUTION,(case when(a.mpf_ee)is null then '0.00' else a.mpf_ee end)SSS_MPF,a.SSSContribution+(case when(a.mpf_ee)is null then '0.00' else a.mpf_ee end)SSSTotal,a.PHICContribution PHIC_CONTRIBUTION,a.HDMFContribution HDMF_CONTRIBUTION,a.Tax WHT,(select case when SUM(y.amount)is null then 0 else SUM(y.amount)end OD from TPayrollOtherDeduction z left join TPayrollOtherDeductionline y on z.id=y.payotherdeduction_id where z.action is null and y.Emp_id=a.EmployeeId and z.payroll_id=a.PayrollId )OD,(select case when sum(y.nontaxable_amt)is null then 0 else sum(y.nontaxable_amt)end from TPayrollOtherIncome z left join TPayrollOtherIncomeLine y on z.id=y.payotherincome_id where z.action is null and z.payroll_id=a.PayrollId and y.Emp_id=a.EmployeeId)OINT,a.NetIncome NET_INCOME,(select case when sum(y.nontaxable_amt)is null then 0 else sum(y.nontaxable_amt)end from TPayrollOtherIncome z left join TPayrollOtherIncomeLine y on z.id=y.payotherincome_id where z.action is null and z.payroll_id=a.PayrollId and y.Emp_id=a.EmployeeId)+a.GrossIncome TotalGross from TPayrollLine a left join memployee b on a.EmployeeId=b.Id left join mdepartment c on b.departmentid=c.Id left join mstore d on b.store_id=d.Id left join TPayroll e on a.payrollid=e.id where e.MonthId=" + ddlmonth.SelectedValue + " and e.status is null and e.PayrollNumber like '%" + ddlyear.SelectedItem + "%')tt group by grouping sets((RowNumber,EMPLOYEE,DEPARTMENT,SSSTotal,TotalGross,OTMEAL,RATE,totallateamt,totalundertimeamt,TotalAbsentAmount,t_reg_amt,totalleavepay,ot_amt,night_amt,hd_amt,rd_amt,case when NET_INCOME >0 then OIT else 0 end,GROSS_INCOME,SSS_CONTRIBUTION,PHIC_CONTRIBUTION,HDMF_CONTRIBUTION,WHT,case when NET_INCOME >0 then OD else 0 end ,case when NET_INCOME >0 then OINT else 0 end,NET_INCOME),())";
        DataTable dt = dbhelper.getdata(query);
        DataTable newdt = new DataTable();
        if (dt.Rows.Count > 0)
        {
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            string[] kuyaw=new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++) {
                newdt.Columns.Add(dt.Columns[i].ColumnName);
                kuyaw[i] = dt.Compute("Sum("+dt.Columns[i].ColumnName+")", string.Empty).ToString();
                if (newdt.Rows.Count == 0)
                {
                    newdt.Rows.Add();
                }
                newdt.Rows[0][dt.Columns[i].ColumnName] = (kuyaw[i]);
            }

            if (newdt.Rows.Count > 0)
            {
                Button1.Visible = false;
                Button3.Visible = true;
            }
            ViewState["newdt"] = newdt;
            GridView1.DataSource = newdt;
            GridView1.DataBind();
        }
    }

    protected void processSave(object sender, EventArgs e)
    {
        DataTable dt = ViewState["newdt"] as DataTable;
        DataTable dtt = ViewState["company"] as DataTable;

        string query = "insert into alphalist1601C(date,userid,yyyy,mm,amendedreturn,taxedwithheld,rdocode,categoryofwithholdagent,internaltaxtreaty,companyid,companyname,companyaddress,companycode,companytin,companynumber)values(GETDATE(),'" + Session["user_id"].ToString() + "','" + ddlyear.SelectedItem.ToString() + "','" + ddlmonth.SelectedValue + "','False','True','123','Private','False','" + dtt.Rows[0]["Id"] + "','" + dtt.Rows[0]["Company"] + "','" + dtt.Rows[0]["Address"] + "','" + dtt.Rows[0]["zipcode"] + "','" + dtt.Rows[0]["TIN"] + "','00000')select scope_identity()transid";
        DataTable dttrans = dbhelper.getdata(query);
        query = "insert into alphalist1601C_summary(date,transid,totaltaxable,totalntaxable,totalgross,totaltax,hdmfee,phicee,sssee,action)values(GETDATE(),'" + dttrans.Rows[0]["transid"] + "','" + dt.Rows[0]["GROSS_INCOME"] + "','" + dt.Rows[0]["OINT"] + "','" + dt.Rows[0]["TotalGross"] + "','" + dt.Rows[0]["WHT"] + "','" + dt.Rows[0]["HDMF_CONTRIBUTION"] + "','" + dt.Rows[0]["PHIC_CONTRIBUTION"] + "','" + dt.Rows[0]["SSSTotal"] + "',NULL)";
        dbhelper.getdata(query);
        Response.Redirect("mrritw");
    }

    protected void PDF(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?BIR1601C?key=" + function.Encrypt(row.Cells[0].Text, true) + "'", true);
        }
    }
}