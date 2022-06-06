using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Services;
using System.Configuration;
using System.Data.OleDb;

public partial class content_payroll_otherdeductiondetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["trid"].ToString(), true);
            disp();
            loadable();
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

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select a.id, a.firstname+', '+a.lastname+' '+a.Middlename fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.idnumber+' '+a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}~{1}", reader["id"], reader["fullname"]));
                    //retCategory.Add(reader.GetString(0));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
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

                        DataTable dtt = dbhelper.getdata("select emp_id,OtherDeduction_id from TPayrollOtherDeductionLine where emp_id='" + lbl_bals.Value + "' and OtherDeduction_id='" + ddl_dt.SelectedValue + "' ");

                            if (dtt.Rows.Count == 0)
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
                                lbl_err_msg.Text = "already exist!";
                    }
                    else
                        lbl_err_msg.Text = "exist!";
                }
                else
                    lbl_err_msg.Text = "Not yet registered!";
            }
        }

    }


    protected void loadable()
    {
        string query = "select * from MPayrollGroup where id<>'9' order by id asc";
        DataTable dt = dbhelper.getdata(query);

        //ddl_payroll_group.Items.Clear();
        //ddl_payroll_group.Items.Add(new ListItem(" ", "0"));
        //foreach (DataRow dr in dt.Rows)
        //{
        //    ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        //}


        ////get emplooyee
        //query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,c.PayrollGroup from MEmployee a " +
        //                "left join MPosition b on a.PositionId=b.Id " +
        //                "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
        //                "where a.PayrollGroupId=" + ddl_payroll_group.SelectedValue + "";
        //dt = dbhelper.getdata(query);
        //if (grid_item.Rows.Count > 0)
        //{
        //    DropDownList ddl_emp = (DropDownList)grid_item.Rows[0].Cells[1].FindControl("ddl_emp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        ddl_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));

        //    }
        //}
        dt = dbhelper.getdata("select a.Id,a.OtherDeduction  from MOtherDeduction a where action is null and status='4'");
        ddl_dt.Items.Clear();
        ddl_dt.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow otd in dt.Rows)
        {
            ddl_dt.Items.Add(new ListItem(otd["OtherDeduction"].ToString(), otd["id"].ToString()));
        }



    }

    protected void disp()
    {
        string query = "select a.id , c.IdNumber+ ' - '+ c.LastName+', '+c.FirstName e_name,case when b.OtherDeduction IS null then 'Additional' else b.OtherDeduction end OtherDeduction,a.Amount,a.balance,a.loan_id,a.PayOtherDeduction_id from  TPayrollOtherDeductionLine a " +
                        "left join MOtherDeduction b on a.OtherDeduction_id=b.Id " +
                        "left join MEmployee c on a.Emp_id=c.Id " +
                        "where a.PayOtherDeduction_id=" + key.Value + "";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        grid_view.HeaderRow.TableSection = TableRowSection.TableHeader;
        grid_view.UseAccessibleHeader = true;
    }
    protected void updateTPayrollotherdeductionline(object sender, EventArgs e)
    {
        int ii = 0;
        foreach (GridViewRow gr in grid_view.Rows)
        {
            TextBox txt_amt = (TextBox)grid_view.Rows[ii].Cells[2].FindControl("txt_amt");
            string query = "select d.Id as loan_id,d.EmployeeId,d.OtherDeductionId,d.MonthlyAmortization, " +
                "a.Id,a.lastname+', '+a.firstname+' '+ a.middlename as Fullname, " +
                "c.PayrollGroup,d.otherdeductionid,e.otherdeduction, " +
                "(case when ( " +
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
                "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id where aa.Emp_id=a.Id and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null and aa.loan_id=d.Id)end) as Balance " +
                "from MEmployee a " +
                "left join MPosition b on a.PositionId=b.Id " +
                "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                "left join MEmployeeLoan d on a.Id=d.EmployeeID " +
                "left join motherdeduction e on d.otherdeductionid=e.id " +
                "where d.id=" + gr.Cells[5].Text + " and d.status like '%Approve%' and e.action is null  " +
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
                "convert(float,d.Balance) -( select SUM (convert(float,(aa.Amount))) from TPayrollOtherDeductionLine aa " +
                "left join TPayrollOtherDeduction bb on aa.PayOtherDeduction_id=bb.id where aa.Emp_id=a.Id and aa.OtherDeduction_id=d.OtherDeductionId and bb.payroll_id is not null and aa.loan_id=d.Id)end) > 0 " +
                "order by d.Id desc ";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);


            decimal ball=dt.Rows.Count>0?decimal.Parse(dt.Rows[0]["Balance"].ToString()):0;
            

            dbhelper.getdata("update TPayrollOtherDeductionLine set Amount='" + txt_amt.Text + "',balance=" + (ball - decimal.Parse(txt_amt.Text)) + " where id=" + gr.Cells[3].Text + "");

            //dbhelper.getdata("update TPayrollOtherDeductionLine set Amount='" + txt_amt.Text + "' where id=" + gr.Cells[3].Text + "");
            ii++;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='viewdetailsotherdeduction?trid=" + Request.QueryString["trid"].ToString() + "'", true);
    }


    protected void view(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }

    protected void cpop(object sender, EventArgs e)
    {
        Div1.Visible = false;
        Div2.Visible = false;

    }

    protected void delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = ViewState["additionalamt"] as DataTable;
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["additionalamt"] = dt;
            grid_add_deduct.DataSource = dt;
            grid_add_deduct.DataBind();
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        //if (errchk())
        //{

            stateclass a = new stateclass();

            foreach (GridViewRow grr in grid_add_deduct.Rows)
            {

                DataTable dt = dbhelper.getdata("select emp_id,OtherDeduction_id from TPayrollOtherDeductionLine where emp_id='" + grr.Cells[0].Text + "' and OtherDeduction_id='" + grr.Cells[1].Text + "' ");

                if (dt.Rows.Count == 0)
                {
                    a.sa = key.Value;
                    a.sb = grr.Cells[0].Text;//empid
                    a.sc = grr.Cells[1].Text; //deductionid
                    a.sd = grr.Cells[4].Text;//amount
                    a.se = "0";
                    a.sf = "0";
                    bol.payotherdeductionline(a);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='viewdetailsotherdeduction?trid=" + Request.QueryString["trid"].ToString() + "'", true);
                }


            }

            
       // }

    }

    //protected bool errchk()
    //{
    //    bool err = true;
    //    if (txt_remarks.Text.Length == 0)
    //    {
    //        lbl_errsave.Text = "Remarks must be required!";
    //        err = false;
    //    }
    //    else
    //        lbl_errsave.Text = "";


    //    return err;
    //}

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
        else if (ddl_dt.SelectedValue == "0")
        {
            lbl_err_msg.Text = "Invalid Input Deduction Type";
            err = false;
        }
        else
            lbl_err_msg.Text = "";

        return err;
    }
}