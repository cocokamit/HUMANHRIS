using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_thirteenmp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
        {
            Response.Redirect("quit");
        }
        if (!IsPostBack)
            disp();
    }


    protected void click_range(object sender, EventArgs e)
    {
        if (btn_ver.Text == "Verify")
        {
            if (ddl_range_thirteen.SelectedValue == "1")
                dectomay(ddl_emp.SelectedValue);
            else if (ddl_range_thirteen.SelectedValue == "2")
                junetonov(ddl_emp.SelectedValue);
            else if (ddl_range_thirteen.SelectedValue == "3")
                wholeyear(ddl_emp.SelectedValue);

            if (grid_item.Rows.Count > 0)
                btn_save.Enabled = true;

            DataTable dt = ViewState["bunos"] as DataTable;

            DataTable dtfinaltosave = new DataTable();
            dtfinaltosave = dt.Copy();
            dtfinaltosave.Clear();
            DataRow dr_final;
            foreach (DataRow dr in dt.Rows) 
            {
                 DataTable dtchk = dbhelper.getdata("select * from thirteenmonth_trn_details a " +
                                       "left join thirteenmonth_transaction b on a.tmtrnid=b.tmtrnid " +
                                       "where b.year=" + DateTime.Now.Year + " and b.action is null and b.[desc]='" + ddl_range_thirteen.SelectedItem.Text + "' and a.empid=" + dr["ID"] + "");
                 if (dtchk.Rows.Count == 0)
                 {
                     dr_final = dtfinaltosave.NewRow();
                     dr_final.ItemArray = dr.ItemArray;
                     dtfinaltosave.Rows.Add(dr_final);
                 }
                 else
                 {
                 
                     lbl_err.Text =  lbl_err.Text + "<br/>" + dr["employee"].ToString() +" is not Processed.  <br/>";
                 }
            }


            grid_item.DataSource = dtfinaltosave;
            grid_item.DataBind();

            if (dtfinaltosave.Rows.Count>0)
                btn_save.Enabled = true;

            ddl_emp.Enabled = false  ;
            ddl_range_thirteen.Enabled = false;
            btn_ver.Text = "Change";
            txt_remarks.Enabled = false;



        }
        else if (btn_ver.Text == "Change")
        {
            ddl_emp.Enabled = true;
            ddl_range_thirteen.Enabled = true;
            btn_ver.Text = "Verify";
            txt_remarks.Enabled = true;
            btn_save.Enabled = false;
            grid_item.DataBind();
            lbl_err.Text = "";
        }


 
     

    }
    protected void dectomay(string empid)
    {
        string prevyear = (DateTime.Now.Year - 1).ToString();
        string newcon = int.Parse(empid) > 0 ? " and z.id=" + empid + "" : "";
        string query = "select ID,employee,Payroll_Type, " +
                        "cast(case when January is null then '0' else January end as decimal(18,2)) January , " +
                        "cast(case when Febuary is null then '0' else Febuary end as decimal(18,2)) Febuary , " +
                        "cast(case when March is null then '0' else March end as decimal(18,2)) March , " +
                        "cast(case when April is null then '0' else April end as decimal(18,2)) April , " +
                        "cast(case when May is null then '0' else May end as decimal(18,2)) May , " +
                        "'0' June , " +
                        "'0' July , " +
                        "'0' August , " +
                        "'0' September , " +
                        "'0' October , " +
                        "'0' November , " +
                        "cast(case when December is null then '0' else December end as decimal(18,2)) December , " +
                        "cast(round((case when January is null then '0' else January end +  " +
                        "case when Febuary is null then '0' else Febuary end + " +
                        "case when March is null then '0' else March end + " +
                        "case when April is null then '0' else April end + " +
                        "case when May is null then '0' else May end + " +
                        "case when December is null then '0' else December end )/12,2)as decimal(18,2)) Thirteen_Month " +
        "from  " +
        "( " +
        "select z.id,z.lastname+', '+z.firstname+' '+z.middlename employee,case when z.PayrollTypeId=1 then 'Fixed' else 'Daily' end Payroll_Type, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='01' and a.employeeid=z.id) January, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='02' and a.employeeid=z.id) Febuary, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='03' and a.employeeid=z.id) March, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='04' and a.employeeid=z.id) April, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='05' and a.employeeid=z.id) May, " +





        "(select  " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + prevyear + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='12' and a.employeeid=z.id) December " +

        "from memployee z " +
        "where z.payrollgroupid!=4 " + newcon + "  " +
        ") thirteenmonthpay";

        DataTable dt = dbhelper.getdata(query);
        ViewState["bunos"] = dt;


       
    }
    protected void junetonov(string empid)
    {
        string prevyear = (DateTime.Now.Year - 1).ToString();
        string newcon = int.Parse(empid) > 0 ? " and z.id=" + empid + "" : "";
        string query = "select ID,employee,Payroll_Type, " +

                        "'0' December , " +
                        "'0' January , " +
                        "'0' Febuary , " +
                        "'0' March , " +
                        "'0' April , " +
                        "'0' May , " +
                        "cast(case when June is null then '0' else June end as decimal(18,2)) June , " +
                        "cast(case when July is null then '0' else July end as decimal(18,2)) July , " +
                        "cast(case when August is null then '0' else August end as decimal(18,2)) August , " +
                        "cast(case when September is null then '0' else September end as decimal(18,2)) September , " +
                        "cast(case when October is null then '0' else October end as decimal(18,2)) October , " +
                        "cast(case when November is null then '0' else November end as decimal(18,2)) November , " +


                        "cast(round((case when June is null then '0' else June end + " +
                        "case when July is null then '0' else July end + " +
                        "case when August is null then '0' else August end + " +
                        "case when September is null then '0' else September end + " +
                        "case when October is null then '0' else October end + " +
                        "case when November is null then '0' else November end " +
                        ")/12,2)as decimal(18,2)) Thirteen_Month " +
        "from  " +
        "( " +
        "select z.id,z.lastname+', '+z.firstname+' '+z.middlename employee,case when z.PayrollTypeId=1 then 'Fixed' else 'Daily' end Payroll_Type, " +

      
        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='06' and a.employeeid=z.id) June, " +

        "(select  " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='07' and a.employeeid=z.id) July, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='08' and a.employeeid=z.id) August, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='09' and a.employeeid=z.id) September, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='10' and a.employeeid=z.id) October, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='11' and a.employeeid=z.id) November " +


    
        "from memployee z " +
        "where z.payrollgroupid!=4  " + newcon + " " +
        ") thirteenmonthpay";

        DataTable dt = dbhelper.getdata(query);
        ViewState["bunos"] = dt;

    }
    protected void wholeyear(string empid)
    {
        string prevyear = (DateTime.Now.Year - 1).ToString();
        string newcon = int.Parse(empid) > 0 ? " and z.id=" + empid + "" : "";
        string query = "select ID,employee,Payroll_Type, " +
                        "cast(case when January is null then '0' else January end as decimal(18,2)) January , " +
                        "cast(case when Febuary is null then '0' else Febuary end as decimal(18,2)) Febuary , " +
                        "cast(case when March is null then '0' else March end as decimal(18,2)) March , " +
                        "cast(case when April is null then '0' else April end as decimal(18,2)) April , " +
                        "cast(case when May is null then '0' else May end as decimal(18,2)) May , " +
                        "cast(case when June is null then '0' else June end as decimal(18,2)) June , " +
                        "cast(case when July is null then '0' else July end as decimal(18,2)) July , " +
                        "cast(case when August is null then '0' else August end as decimal(18,2)) August , " +
                        "cast(case when September is null then '0' else September end as decimal(18,2)) September , " +
                        "cast(case when October is null then '0' else October end as decimal(18,2)) October , " +
                        "cast(case when November is null then '0' else November end as decimal(18,2)) November , " +
                        "cast(case when December is null then '0' else December end as decimal(18,2)) December , " +
                        "cast(round((case when January is null then '0' else January end +  " +
                        "case when Febuary is null then '0' else Febuary end + " +
                        "case when March is null then '0' else March end + " +
                        "case when April is null then '0' else April end + " +
                        "case when May is null then '0' else May end + " +
                        "case when June is null then '0' else June end + " +
                        "case when July is null then '0' else July end + " +
                        "case when August is null then '0' else August end + " +
                        "case when September is null then '0' else September end + " +
                        "case when October is null then '0' else October end + " +
                        "case when November is null then '0' else November end + " +
                        "case when December is null then '0' else December end )/12,2)as decimal(18,2)) Thirteen_Month " +
        "from  " +
        "( " +
        "select z.id,z.lastname+', '+z.firstname+' '+z.middlename employee,case when z.PayrollTypeId=1 then 'Fixed' else 'Daily' end Payroll_Type, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='01' and a.employeeid=z.id) January, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='02' and a.employeeid=z.id) Febuary, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='03' and a.employeeid=z.id) March, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='04' and a.employeeid=z.id) April, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='05' and a.employeeid=z.id) May, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='06' and a.employeeid=z.id) June, " +

        "(select  " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='07' and a.employeeid=z.id) July, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='08' and a.employeeid=z.id) August, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='09' and a.employeeid=z.id) September, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='10' and a.employeeid=z.id) October, " +

        "(select " +
        "sum(a.netincome) " +
        "from TPayrollLine a " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + DateTime.Now.Year + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='11' and a.employeeid=z.id) November, " +


        "(select  " +
        "sum(a.netincome) " +
        "from TPayrollLine a  " +
        "left join TPayroll b on a.PayrollId=b.Id " +
        "where b.status is null and year((select datestart from tdtr where ID=b.dtrid))='" + prevyear + "' and MONTH((select datestart from tdtr where ID=b.dtrid))='12' and a.employeeid=z.id) December " +

        "from memployee z " +
        "where z.payrollgroupid!=4 " + newcon + " " +
        ") thirteenmonthpay";

        DataTable dt = dbhelper.getdata(query);
        ViewState["bunos"] = dt;


       

    }
    protected void newdtrlogs(object sender, EventArgs e)
    {
        ppop(true);
        panelPopUpPanel.Style.Add("width","90%");
        panelPopUpPanel.Style.Add("left", "300px");
        panelPopUpPanel.Style.Add("top", "5%");
        grid_item.DataBind();
        DataTable dtemp = dbhelper.getdata("select lastname +', '+firstname+','+middlename e_name,id from memployee");
        ddl_emp.Items.Clear();
        ddl_emp.Items.Add(new ListItem("Select Employee", "0"));
        foreach (DataRow dr in dtemp.Rows)
        {
            ddl_emp.Items.Add(new ListItem(dr["e_name"].ToString(), dr["id"].ToString()));
        }
    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("tmp");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void select_year(object sender, EventArgs e)
    { 

    }
    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='3' and  Amount<='" + e.Row.Cells[15].Text + "' order by Amount desc";
            DataTable gettax = dbhelper.getdata(query);
            decimal getpercent = 0;
            decimal amounttax = 0;
            decimal amountcolumn = 0;
            if (gettax.Rows.Count > 0)
            {
                getpercent = decimal.Parse(gettax.Rows[0]["percentage"].ToString());
                amounttax = decimal.Parse(gettax.Rows[0]["tax"].ToString());
                amountcolumn = decimal.Parse(gettax.Rows[0]["amount"].ToString());
            }
            else
            {
                getpercent = 0;
                amounttax = 0;
                amountcolumn = 0;
            }
            getpercent = getpercent.ToString().Length == 1 ? decimal.Parse("0.0" + getpercent) : decimal.Parse("0." + getpercent);
            decimal getwht = getpercent * (decimal.Parse(e.Row.Cells[15].Text) - amountcolumn) + amounttax;
            decimal net = decimal.Parse(e.Row.Cells[15].Text) - getwht;

            e.Row.Cells[16].Text = string.Format("{0:n2}", getwht);
            e.Row.Cells[17].Text = string.Format("{0:n2}", net);
        }
    }
    protected void savethirteen(object sender, EventArgs e)
    {
        string query = "insert into thirteenmonth_transaction (date,userid,useridlastupdate,datelastupdate,year,[desc],notes)values(getdate()," + Session["user_id"] + "," + Session["user_id"] + ",getdate()," + DateTime.Now.Year + ",'" + ddl_range_thirteen.SelectedItem.Text + "','" + txt_remarks.Text + "') select scope_identity() id";
        DataTable dttrn = dbhelper.getdata(query);
        foreach (GridViewRow gr in grid_item.Rows)
        {
            query = "insert into thirteenmonth_trn_details(tmtrnid,empid,grossamt,wht,net,dec,jan,feb,mar,apr,may,jun,jul,aug,sep,oct,nov)values " +
                    "(" + dttrn.Rows[0]["id"].ToString() + "," + gr.Cells[0].Text + ",'" + gr.Cells[15].Text.Replace(",", "") + "','" + gr.Cells[16].Text.Replace(",", "") + "','" + gr.Cells[17].Text.Replace(",", "") + "','" + gr.Cells[3].Text.Replace(",", "") + "','" + gr.Cells[4].Text.Replace(",", "") + "','" + gr.Cells[5].Text.Replace(",", "") + "','" + gr.Cells[6].Text.Replace(",", "") + "','" + gr.Cells[7].Text.Replace(",", "") + "','" + gr.Cells[8].Text.Replace(",", "") + "','" + gr.Cells[9].Text.Replace(",", "") + "','" + gr.Cells[10].Text.Replace(",", "") + "','" + gr.Cells[11].Text.Replace(",", "") + "','" + gr.Cells[12].Text.Replace(",", "") + "','" + gr.Cells[13].Text.Replace(",", "") + "','" + gr.Cells[14].Text.Replace(",", "") + "')";
            dbhelper.getdata(query);
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='tmp'", true);
    }
    protected void searchtrn(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {
        string query = "select tmtrnid,left(convert(varchar,date,101),10)date,year,[desc] description,notes from thirteenmonth_transaction where action is null ";
        DataTable dt = dbhelper.getdata(query);
        grid_sc_trans.DataSource = dt;
        grid_sc_trans.DataBind();
    }
    protected void generatetexfile(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            // string ggg = "select c.ATMAccountNumber Account_No,a.NetIncome Net_Income " +
            //"from TPayrollLine a " +
            //"left join TPayroll b on a.PayrollId=b.Id " +
            //"left join MEmployee c on a.EmployeeId=c.Id " +
            //"where a.PayrollId=" + row.Cells[0].Text + " and len(c.ATMAccountNumber)>0  and a.NetIncome>0 order by b.PayrollDate desc ";
            // DataTable dt = dbhelper.getdata(ggg);

            string query = "select  b.ATMAccountNumber Account_No,a.net Net_Income from thirteenmonth_trn_details a left join memployee b on a.empid=b.Id where len(b.ATMAccountNumber)>0 and  a.tmtrnid=" + row.Cells[0].Text + "";
            DataTable dt = dbhelper.getdata(query);
            //grid_item.DataSource = dt;
            //grid_item.DataBind();
            //Build the Text file data.
            string txt = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                txt += column.ColumnName + "\t";
            }
            txt += "\r\n";
            foreach (DataRow rows in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    txt += rows[column.ColumnName].ToString() + "\t";
                }
                txt += "\r\n";
            }
            string filename = row.Cells[2].Text + "_13th_month.txt";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + "");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(txt);
            Response.Flush();
            Response.End();
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                LinkButton lnkcan = (LinkButton)row.FindControl("lnkcan");
                dbhelper.getdata("update thirteenmonth_transaction set action='Cancel',useridlastupdate=" + Session["user_id"] + ",datelastupdate=getdate() where tmtrnid=" + lnkcan.CommandName + " ");
                Response.Redirect("tmp", false);
            }
            else
            {
            }
        }
    }
}