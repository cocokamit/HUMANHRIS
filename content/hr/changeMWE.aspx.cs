using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class content_hr_changeMWE : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        { }
    }
   
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            string query = string.Format("select a.id, a.firstname+', '+a.lastname+' '+a.Middlename fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.idnumber+' '+a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}~{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }
    public void data_interaction(int empid, string remarks, int paytypeid,decimal mr,decimal pr,decimal dr,decimal hr,DateTime effective)
    {
        app_trn apptrn = new app_trn();
        apptrn.userid = int.Parse(Session["user_id"].ToString());
        apptrn.lastupdateuserid = int.Parse(Session["user_id"].ToString());
        apptrn.date = DateTime.Now;
        apptrn.lastupdatedate = DateTime.Now;
        apptrn.empid = empid;
        apptrn.remarks = remarks;
        datacontext.Context.app_trns.InsertOnSubmit(apptrn);
        datacontext.Context.SubmitChanges();

        app_trn_salaryinc apptrnsalinc = new app_trn_salaryinc();
        apptrnsalinc.app_trn_id = apptrn.id;
        apptrnsalinc.paytypeid = paytypeid;
        apptrnsalinc.fnod = 313;
        apptrnsalinc.fnoh = 8;
        apptrnsalinc.mr = mr;
        apptrnsalinc.pr = pr;
        apptrnsalinc.dr = dr;
        apptrnsalinc.hr = hr;
        apptrnsalinc.effective_date = effective;
        datacontext.Context.app_trn_salaryincs.InsertOnSubmit(apptrnsalinc);
        datacontext.Context.SubmitChanges();

        MEmployee emp = new MEmployee();
        emp = datacontext.Context.MEmployees.Single(x => x.Id == empid);
        emp.PayrollTypeId = paytypeid;
        emp.FixNumberOfDays = "313";
        emp.FixNumberOfHours = "8";
        emp.MonthlyRate = mr.ToString();
        emp.PayrollRate = pr.ToString();
        emp.DailyRate = dr.ToString();
        emp.AbsentDailyRate = dr.ToString();
        emp.HourlyRate = hr.ToString();
        datacontext.Context.SubmitChanges();
    }
    protected void save_cpr(object sender, EventArgs e)
    {
        decimal mr = 0;
        decimal pr = 0;
        decimal dr = 0;
        decimal hr = 0;
        if (dll_type_rate.SelectedValue != "0")
        {
            if (FileUpload.HasFile)
            {
                getexcel();
                var gg = ViewState["excel_data"] as DataTable;
                foreach (DataRow data_list in gg.Rows)
                {
                    if (dll_type_rate.SelectedValue == "1")
                    {
                        mr = decimal.Parse(data_list["New Rate"].ToString().Replace(",", ""));
                        pr = mr / 2;
                        dr = (mr * 12) / 313;
                        hr = dr / 8;
                    }
                    if (dll_type_rate.SelectedValue == "2")
                    {
                        mr = 0;
                        pr = 0;
                        dr = decimal.Parse(data_list["New Rate"].ToString().Replace(",", ""));
                        hr = dr / 8;
                    }
                    string idno = data_list["IDNUMBER"].ToString();
                    var emplist = from emp in datacontext.Context.GetTable<MEmployee>() where emp.IdNumber == idno select new { empid = emp.Id };
                    data_interaction(emplist.SingleOrDefault().empid, data_list["Remarks"].ToString(),int.Parse(dll_type_rate.SelectedValue),mr, pr, dr, hr, DateTime.Parse(data_list["Effectivity"].ToString()));
                }
            }
            else
            {
                if (dll_type_rate.SelectedValue == "1")
                {
                    mr = decimal.Parse(txt_rate.Text.Replace(",", ""));
                    pr = mr / 2;
                    dr = (mr * 12) / 313;
                    hr = dr / 8;
                }
                if (dll_type_rate.SelectedValue == "2")
                {
                    mr = 0;
                    pr = 0;
                    dr = decimal.Parse(txt_rate.Text.Replace(",", ""));
                    hr = dr / 8;
                }
                data_interaction(int.Parse(lbl_bals.Value), txt_remarks.Text, int.Parse(dll_type_rate.SelectedValue), mr, pr, dr, hr, DateTime.Parse(txt_effectivity.Text));
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='changerate'", true);
        }
        else
            Response.Write("<script>alert('Pls. Select Payroll Type!')</script>");
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
    private void getexcel()
    {
        if (dll_type_rate.SelectedValue == "0")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Other Income Type');", true);
        }
        else
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload.FileName));
            FileUpload.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "] where len(trim(IDNUMBER))>0 ", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();
            ViewState["excel_data"] = DtSet.Tables[0];
        }
    }
    protected void cancel(object sender, EventArgs e)
    {
        int id = 0;
        app_trn apptrn = new app_trn();
        apptrn = datacontext.Context.app_trns.Single(x => x.id == id);
        apptrn.action = "Cancelled";
        datacontext.Context.SubmitChanges();
    }
    [WebMethod]
    public static string[] cancel(int id,int empid)
    {
        app_trn apptrn = new app_trn();
        apptrn = datacontext.Context.app_trns.Single(x => x.id == id);
        apptrn.action = "Cancelled";
        datacontext.Context.SubmitChanges();


        var trn_list = from a in datacontext.Context.GetTable<app_trn>()
        join b in datacontext.Context.GetTable<app_trn_salaryinc>() on a.id equals b.app_trn_id
        where a.action == null && a.empid == empid
        orderby a.id descending
        select new
        {
            paytypeid = b.paytypeid,
            mr = b.mr,
            pr = b.pr,
            dr = b.dr,
            hr = b.hr,
            emp = a.empid
        };

        
        MEmployee empp = new MEmployee();
        empp = datacontext.Context.MEmployees.Single(x => x.Id == empid);
        if (trn_list.Count() > 0)
        {
        empp.PayrollTypeId = trn_list.SingleOrDefault().paytypeid;
        empp.FixNumberOfDays = "313";
        empp.FixNumberOfHours = "8";
        }
        empp.MonthlyRate =trn_list.Count()>0?trn_list.SingleOrDefault().mr.ToString():"0";
        empp.PayrollRate = trn_list.Count()>0?trn_list.SingleOrDefault().pr.ToString():"0";
        empp.DailyRate = trn_list.Count()>0?trn_list.SingleOrDefault().dr.ToString():"0";
        empp.AbsentDailyRate = trn_list.Count()>0?trn_list.SingleOrDefault().dr.ToString():"0";
        empp.HourlyRate = trn_list.Count()>0?trn_list.SingleOrDefault().hr.ToString():"0";
        datacontext.Context.SubmitChanges();
             
        var apptrn_list = from a in datacontext.Context.GetTable<app_trn>() where a.action == null select a;
        string[] test = { "Successfully Save", apptrn_list.Count().ToString() };
        return test;
    }
}