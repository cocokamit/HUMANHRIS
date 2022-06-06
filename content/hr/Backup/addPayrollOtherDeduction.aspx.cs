using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class content_hr_addPayrollOtherDeduction : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            loadable();

            grid_item.DataBind();
     
            grid_contribution.DataBind();
            manaualdatatable();
            grid_add_deduct.DataBind();
        }
    }
   
    protected void manaualdatatable()
    {
        DataTable additionalamt = new DataTable();
        additionalamt.Columns.Add(new DataColumn("row_no", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("empid", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("deductionid", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("deductionname", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("empname", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("amount", typeof(string)));
        additionalamt.Columns.Add(new DataColumn("notes", typeof(string)));
        ViewState["additionalamt"] = additionalamt;
    }
    protected void selectpg(object sender, EventArgs e)
    {
        if (int.Parse(ddl_payroll_group.SelectedValue) > 0)
        {
            gridviewrow();
            othercontributionrow();
        }
        else
        {
            grid_item.DataSource = null;
            grid_item.DataBind();
            grid_contribution.DataSource = null;
            grid_contribution.DataBind();
            Response.Write("<script>alert('Invalid Payroll Group!')</script>");
        }
    }
    protected void loadable()
    {
        string query = "select * from MPayrollGroup where id<>'9' and status = '1' order by id asc";
        DataTable dt = dbhelper.getdata(query);

        ddl_payroll_group.Items.Clear();
        ddl_payroll_group.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }


        //get emplooyee
        query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,c.PayrollGroup from MEmployee a " +
                        "left join MPosition b on a.PositionId=b.Id " +
                        "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                        "where a.PayrollGroupId=" + ddl_payroll_group.SelectedValue + "";
        dt = dbhelper.getdata(query);
        if (grid_item.Rows.Count > 0)
        {
            DropDownList ddl_emp = (DropDownList)grid_item.Rows[0].Cells[1].FindControl("ddl_emp");
            foreach (DataRow dr in dt.Rows)
            {
                ddl_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));

            }
        }
        dt = dbhelper.getdata("select a.Id,a.OtherDeduction  from MOtherDeduction a where action is null and status='4'");
        ddl_dt.Items.Clear();
        ddl_dt.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow otd in dt.Rows)
        {
            ddl_dt.Items.Add(new ListItem(otd["OtherDeduction"].ToString(), otd["id"].ToString()));
        }
       

      
    }
    private void gridviewrow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("empid", typeof(string)));
        dt.Columns.Add(new DataColumn("loanid", typeof(string)));
        dt.Columns.Add(new DataColumn("deductionid", typeof(string)));
        dt.Columns.Add(new DataColumn("empname", typeof(string)));
        dt.Columns.Add(new DataColumn("otherdeduction", typeof(string)));
        dt.Columns.Add(new DataColumn("Amortization", typeof(string)));
        dt.Columns.Add(new DataColumn("Balance", typeof(string)));





        string query = "select d.Id as loan_id,d.EmployeeId,d.OtherDeductionId,d.MonthlyAmortization, " +
                    "a.Id,a.lastname+', '+a.firstname+' '+ a.middlename as Fullname, " +
                    "c.PayrollGroup,d.otherdeductionid,e.otherdeduction, " +
                    "round((case when ( " +
                    "select SUM (convert(float,(aa.amount))) " +
                    "from TPayrollOtherDeductionLine aa " +
                    "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id " +
                    "where aa.Emp_id=a.Id " +
                    "and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null " +
                    "and aa.loan_id=d.Id " +
                    ") IS NULL " +
                    "then convert(float,d.Balance) " +
                    "else " +
                    "convert(float,d.Balance)-( select SUM (convert(float,(aa.Amount))) from TPayrollOtherDeductionLine aa " +
                    "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id where aa.Emp_id=a.Id and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null and aa.loan_id=d.Id)end),2) as Balance " +
                    "from MEmployee a " +
                    "left join MPosition b on a.PositionId=b.Id " +
                    "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                    "left join MEmployeeLoan d on a.Id=d.EmployeeID " +
                    "left join motherdeduction e on d.otherdeductionid=e.id " +
                    "where a.PayrollGroupId='" + ddl_payroll_group.SelectedValue + "' and d.status like '%Approve%' and e.action is null  " +
                    "and " +
                    "(case when ( " +
                    "select SUM (convert(float,(aa.Amount))) " +
                    "from TPayrollOtherDeductionLine aa " +
                    "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id " +
                    "where aa.Emp_id=a.Id " +
                    "and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null " +
                    "and aa.loan_id=d.Id " +
                    ") IS NULL " +
                    "then convert(float,d.Balance) " +
                    "else " +
                    "round(convert(float,d.Balance) - (select SUM (convert(float,(aa.Amount))) from TPayrollOtherDeductionLine aa " +
                    "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id where aa.Emp_id=a.Id and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null and aa.loan_id=d.Id),2)end) > 0 " +
                    "order by d.Id desc ";

        // and d.status like '%Approve%' ";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);

        if (dtt.Rows.Count > 0)
        {
            for(int i = 0 ; i < dtt.Rows.Count ; i ++)
            {
                dr = dt.NewRow();
                dr["empid"] = dtt.Rows[i]["EmployeeId"].ToString();
                dr["loanid"] = dtt.Rows[i]["loan_id"].ToString();
                dr["deductionid"] = dtt.Rows[i]["otherdeductionid"].ToString();
                dr["empname"] = dtt.Rows[i]["Fullname"].ToString();
                dr["otherdeduction"] = dtt.Rows[i]["otherdeduction"].ToString();
                dr["Amortization"] = decimal.Parse(dtt.Rows[i]["MonthlyAmortization"].ToString()) >= decimal.Parse(dtt.Rows[i]["balance"].ToString()) ? decimal.Parse(dtt.Rows[i]["balance"].ToString()) : decimal.Parse(dtt.Rows[i]["MonthlyAmortization"].ToString());
                dr["Balance"] = dtt.Rows[i]["balance"].ToString();
                dt.Rows.Add(dr);
            }
            ViewState["Item_List"] = dt;
            grid_item.DataSource = dt;
            grid_item.DataBind();
           
        }
    }
   


    //other contribution
    private void othercontributionrow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("empid", typeof(string)));
        dt.Columns.Add(new DataColumn("deductionid", typeof(string)));
        dt.Columns.Add(new DataColumn("empname", typeof(string)));
        dt.Columns.Add(new DataColumn("otherdeduction", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));

        //string getdatas = "select * from motherdeduction where loantype='False' and action is null";
        //DataTable dtgetotherdedudctions = dbhelper.getdata(getdatas);

        //int i = 0;
        //foreach (DataRow drr in dtgetotherdedudctions.Rows)
        //{
           // getdatas = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename as Fullname from memployee a where a.payrollgroupid='" + ddl_payroll_group.SelectedValue + "'";
          string  getdatas = "select b.Id empid,c.Id otherdedudctionid,b.LastName+', '+b.FirstName+' '+b.MiddleName fullname,c.OtherDeduction,a.amount from other_deduction_saving a " +
            "left join memployee b on a.empid=b.Id " +
            "left join MOtherDeduction c on a.other_deduction_id=c.Id " +
            "where a.action is null and c.action is null";
            
            DataTable dte=dbhelper.getdata(getdatas);
            foreach (DataRow dre in dte.Rows)
            {
                dr = dt.NewRow();
                dr["empid"] = dre["empid"].ToString();
                dr["deductionid"] = dre["otherdedudctionid"].ToString();
                dr["empname"] = dre["fullname"].ToString();
                dr["otherdeduction"] = dre["OtherDeduction"].ToString();
                dr["Amount"] = dre["amount"].ToString();
                dt.Rows.Add(dr);
                //i++;
            }
       // }


            ViewState["Item_Listcontribution"] = dt;
            grid_contribution.DataSource = dt;
            grid_contribution.DataBind();
            //setPreviousDatacontribution();

    }
   


    protected bool errchk()
    {
        bool err = true;
        if (txt_remarks.Text.Length == 0)
        {
            lbl_errsave.Text = "Remarks must be required!";
            err = false;
        }
        else
             lbl_errsave.Text = "";
     

        return err;
    }


    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (errchk())
        {
           
            stateclass a = new stateclass();

            DataTable table = ViewState["Item_List"] as DataTable;
            DataTable table1 = ViewState["Item_Listcontribution"] as DataTable;


            a.sa = DateTime.Now.Year.ToString();
            a.sb = ddl_payroll_group.SelectedValue;
            a.sc = Session["user_id"].ToString();
            a.sd = txt_remarks.Text;
            string x = bol.payotherdeduction(a);

            if (table != null)
            {
                int rowcount = 0;
                foreach (GridViewRow row in grid_item.Rows)
                {
                    TextBox txt_amt = (TextBox)grid_item.Rows[rowcount].Cells[5].FindControl("txt_amt");
                    a.sa = x;
                    a.sb = row.Cells[0].Text;//empid;
                    a.sc = row.Cells[2].Text; //deductionid;
                    a.sd = txt_amt.Text.Length>0?txt_amt.Text:"0";//amortization;
                    a.se = row.Cells[1].Text;//loanid;
                    a.sf = (decimal.Parse(row.Cells[6].Text) - decimal.Parse(a.sd)).ToString();//balance
                    bol.payotherdeductionline(a);
                    rowcount++;
         
                }


            }
            int ii = 0;
            foreach (GridViewRow grr in grid_contribution.Rows)
            {
             

                a.sa = x;
                a.sb =grr.Cells[0].Text;//empid
                a.sc = grr.Cells[1].Text; //deductionid
                a.sd = grr.Cells[4].Text;//amount
                a.se = "0";
                a.sf = "0";
                bol.payotherdeductionline(a);

                ii++;
            }
            foreach (GridViewRow grr in grid_add_deduct.Rows)
            {


                a.sa = x;
                a.sb = grr.Cells[0].Text;//empid
                a.sc = grr.Cells[1].Text; //deductionid
                a.sd = grr.Cells[4].Text;//amount
                a.se = "0";
                a.sf = "0";
                bol.payotherdeductionline(a);

                
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='transotherdeduction'", true);
        }
       
    }
    protected bool chkadditional()
    {
        bool err = true;
        if (lbl_bals.Value.Length == 0)
        {
            lbl_err_msg.Text = "Not Registered!";
            err = false;
        }
        else if (txt_addinc_amt.Text.Length == 0)
        {
            lbl_err_msg.Text = "Invalid Input Amount";
            err = false;
        }
        else if (ddl_dt.SelectedValue=="0")
        {
            lbl_err_msg.Text = "Invalid Input Deduction Type";
            err = false;
        }
        else
            lbl_err_msg.Text = "";

        return err;
    }
    protected void ddl_payroll_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        rowcount();
    }

    protected void rowcount()
    {
        DataTable dt = (DataTable)ViewState["Item_List"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            grid_item.Rows[i].Cells[0].Text = Convert.ToString(i + 1);

        }
    }

    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();
            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }

    protected void insertadditionaldeduct(object sender, EventArgs e)
    {
        DataTable dt = ViewState["additionalamt"] as DataTable;

        if (file_data.HasFile)
        {
            if (ddl_dt.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Deduction Type');", true);
            }
            else
            {

                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                string path = string.Concat(Server.MapPath("~/Excel/" + file_data.FileName));
                file_data.SaveAs(path);
                string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                OleDbConnection MyConnection = new OleDbConnection();
                MyConnection.ConnectionString = excelConnectionString;
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
                MyCommand.TableMappings.Add("Table", "TestTable");
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet);
                MyConnection.Close();

                foreach (DataRow dr in DtSet.Tables[0].Rows)
                {
                    string idno = dr["idnumber"].ToString();
                    string amount = dr["amount"].ToString().Replace(",", "");
                    DataTable dtt = dbhelper.getdata("select id,LastName+ +FirstName as name,* from Memployee where IdNumber='" + idno + "'");

                    if (dtt.Rows.Count != 0)
                    {

                        DataRow[] drchk = dt.Select("empid=" + dtt.Rows[0]["id"].ToString() + " and deductionid=" + ddl_dt.SelectedValue + " ");
                        DataRow drr;
                        if (drchk.Count() == 0)
                        {

                            drr = dt.NewRow();
                            drr["row_no"] = dt.Rows.Count + 1;
                            drr["empid"] = dtt.Rows[0]["id"].ToString();
                            drr["deductionid"] = ddl_dt.SelectedValue;
                            drr["empname"] = dtt.Rows[0]["name"].ToString();
                            drr["deductionname"] = ddl_dt.SelectedItem.Text;
                            drr["amount"] = amount;
                            drr["notes"] = txt_addinc_remarks.Text;

                            dt.Rows.Add(drr);
                            ViewState["additionalamt"] = dt;

                            DataRow[] finaldr = dt.Select("", "row_no desc");
                            DataTable finaldt = finaldr.CopyToDataTable();

                            grid_add_deduct.DataSource = finaldt;
                            grid_add_deduct.DataBind();
                            txt_searchemp.Text = "";
                            lbl_err_msg.Text = "";
                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('idnumber does not register');", true);

                }
            }

        }
        else
        {
            if (chkadditional())
            {

                if (lbl_bals.Value.Length > 0)
                {
                    DataRow[] drchk = dt.Select("empid=" + lbl_bals.Value + " and deductionid=" + ddl_dt.SelectedValue + " ");
                    DataRow dr;
                    if (drchk.Count() == 0)
                    {
                        dr = dt.NewRow();
                        dr["row_no"] = dt.Rows.Count + 1;
                        dr["empid"] = lbl_bals.Value;
                        dr["deductionid"] = ddl_dt.SelectedValue;
                        dr["empname"] = txt_searchemp.Text;
                        dr["deductionname"] = ddl_dt.SelectedItem.Text;
                        dr["amount"] = txt_addinc_amt.Text.Replace(",", "");
                        dr["notes"] = txt_addinc_remarks.Text;
                        dt.Rows.Add(dr);
                        ViewState["additionalamt"] = dt;

                        DataRow[] finaldr = dt.Select("", "row_no desc");
                        DataTable finaldt = finaldr.CopyToDataTable();

                        grid_add_deduct.DataSource = finaldt;
                        grid_add_deduct.DataBind();
                        txt_searchemp.Text = "";
                        lbl_err_msg.Text = "";
                    }
                    else
                        lbl_err_msg.Text = "exist!";
                }
                else
                    lbl_err_msg.Text = "Not yet registered!";
            }
        }

    }

    protected void delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            DataTable dt = ViewState["additionalamt"] as DataTable;
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["additionalamt"] = dt;
            grid_add_deduct.DataSource = dt;
            grid_add_deduct.DataBind();
        }
    }
}