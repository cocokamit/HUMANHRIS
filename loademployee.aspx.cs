using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class loademployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
    protected void process(object sender, EventArgs e)
    {
        System.Data.DataSet DtSet;
        System.Data.OleDb.OleDbDataAdapter MyCommand;
        string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
        FileUpload1.SaveAs(path);
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
            stateclass a = new stateclass();
            a._idnumber = dr["Idnumber"].ToString();
            a._lastname = dr["Lastname"].ToString();
            a._firstname = dr["Firstname"].ToString();
            a._middlename = dr["Middlename"].ToString();
            a._extensionname = dr["Extensionname"].ToString();
            a._presentaddress = dr["Presentaddress"].ToString();
            a._permanentaddress = dr["Permanentaddress"].ToString();
            a._zipcode = dr["Zipcode"].ToString().Length>0?dr["Zipcode"].ToString():"0";
            a._phonenumber = dr["Phonenumber"].ToString();
            a._cellphonenumber = dr["Cellphonenumber"].ToString();
            a._emailaddress = dr["Emailaddress"].ToString();
            a._dateofbirth = dr["Dateofbirth"].ToString();
            a._placeofbirth = dr["Placeofbirth"].ToString();
            a._birthzipcode = dr["Birthzipcode"].ToString().Length > 0 ? dr["Birthzipcode"].ToString() : "0";
            a._datehired = dr["Datehired"].ToString();
            a._dateresigned = dr["Dateresigend"].ToString();
            a._sex = dr["Sex"].ToString();
            a._civilstatus = dr["Civilstatus"].ToString();
            a._citizenship = dr["Citizenship"].ToString().Length > 0 ? dr["Citizenship"].ToString() : "0";
            a._religion = dr["Relegion"].ToString().Length > 0 ? dr["Relegion"].ToString() : "0";
            a._height = dr["Hieght"].ToString().Length > 0 ? dr["Hieght"].ToString() : "0";
            a._weight = dr["Weight"].ToString().Length > 0 ? dr["Weight"].ToString() : "0";
            a._gsisnumber = dr["GSISnumber"].ToString();
            a._sssnumber = dr["SSSnumber"].ToString();
            a._hdmfnumber = dr["HDMFnumber"].ToString();
            a._phicnumber = dr["PHICnumber"].ToString();
            a._tin = dr["TINnumber"].ToString();
            DataTable dt_txcode = dbhelper.getdata("select * from MTaxCode where TaxCode='" + dr["Taxcode"] + "'");
            a._taxcode =dt_txcode.Rows.Count > 0 ? dt_txcode.Rows[0]["id"].ToString() : "0";
            a._atmaccountnumber = dr["Accountnumber"].ToString();
            DataTable dt_company = dbhelper.getdata("select * from Mcompany where company='" + dr["Company"] + "'");
            a._company = dt_company.Rows.Count > 0 ? dt_company.Rows[0]["Id"].ToString() : "0";
            DataTable dt_branch = dbhelper.getdata("select * from Mbranch where Branch='" + dr["branch"] + "'");
            a._branch = dt_branch.Rows.Count>0?dt_branch.Rows[0]["id"].ToString():"0";
            DataTable dt_DEPT = dbhelper.getdata("select * from MDepartment where Department='" + dr["Department"] + "'");
            a._department = dt_DEPT.Rows.Count > 0 ? dt_DEPT.Rows[0]["id"].ToString() : "0";
            DataTable dt_div = dbhelper.getdata("select * from MDivision where Division='" + dr["Divission"] + "'");
            a._division = dt_div.Rows.Count > 0 ? dt_div.Rows[0]["Id"].ToString() : "0"; ;
            DataTable dt_pos = dbhelper.getdata("select * from MPosition where position='" + dr["Position"] + "'");
            a._position = dt_pos.Rows.Count > 0 ? dt_pos.Rows[0]["id"].ToString() : "0";
            DataTable dt_pg = dbhelper.getdata("select * from MPayrollgroup where payrollgroup='" + dr["Payrollgroup"] + "'");
            a._payrollgroup = dt_pg.Rows.Count>0?dt_pg.Rows[0]["id"].ToString():"0";//dr["Branch"].ToString();
             DataTable dt_acc = dbhelper.getdata("select * from Maccount where account='" + dr["coa"] + "'");
            a._glaccount =dt_acc.Rows.Count>0?dt_acc.Rows[0]["id"].ToString():"0";// "2";
            DataTable dt_payrolltype = dbhelper.getdata("select * from MPayrollType where PayrollType='" + dr["Payrolltype"] + "'");
            a._payrolltype = dt_payrolltype.Rows.Count > 0 ? dt_payrolltype.Rows[0]["id"].ToString() : "0";
            DataTable dt_shift = dbhelper.getdata("select * from MShiftCode where  SUBSTRING(ShiftCode,0, CHARINDEX('(',ShiftCode)) ='" + dr["Shiftcode"] + "'");
            a._shiftcode = dt_shift.Rows.Count>0?dt_shift.Rows[0]["id"].ToString():"0";
            a._fixnoofdays = dr["FixNoofDays"].ToString().Length > 0 ? dr["FixNoofDays"].ToString() : "0";
            a._fixnoofhours = dr["FixNoofHours"].ToString().Length > 0 ? dr["FixNoofHours"].ToString() : "0";
            a._monthlyrate = dr["MonthlyRate"].ToString().Length > 0 ? dr["MonthlyRate"].ToString() : "0";
            a._payrollrate = dr["PayrollRate"].ToString().Length > 0 ? dr["PayrollRate"].ToString() : "0";
            a._dailyrate = dr["DailyRate"].ToString().Length > 0 ? dr["DailyRate"].ToString() : "0";
            a._absentdailyrate = dr["DailyRate"].ToString().Length > 0 ? dr["DailyRate"].ToString() : "0";
            a._hourlyrate = dr["HourlyRate"].ToString().Length > 0 ? dr["HourlyRate"].ToString() : "0";
            a._nighthourlyrate = "0";
            a._overtimehourlyrate = "0";
            a._overtimenighthourlyrate = "0";
            a._tardyhourlyrate = "0";
            a.userid = "1";
            a._taxtable = dr["Tax-table"].ToString().Length > 0 ? dr["Tax-table"].ToString() : "0";
            a._hdmfaddon = "0";
            a._sssaddon = "0";
            a._hdmftype = "Value";
            a._sssgrosssalary = "true";
            a._isminimum = "false";
            a._bloodtype = "0";
            string pl_stat = "";
            if (dr["Paidleave"].ToString().Length > 0)
            {
                if (dr["Paidleave"].ToString() == "FALSE")
                    pl_stat = "False";
                else
                    pl_stat = "True";
            }
            else
                pl_stat = "False";

            a.saa = pl_stat;
            string pl_es="";
            if(dr["Employeestatus"].ToString()=="Probationary")
            pl_es="1";
            if(dr["Employeestatus"].ToString()=="Regular")
            pl_es="2";
            if(dr["Employeestatus"].ToString()=="Contractual")
            pl_es="3";
            if(dr["Employeestatus"].ToString()=="Resigned")
            pl_es="4";
            if(dr["Employeestatus"].ToString()=="AWOL")
            pl_es="5";
            if(dr["Employeestatus"].ToString()=="Terminated")
            pl_es="6";
            a.sbb = pl_es;
            DataTable dtrptto = dbhelper.getdata("select * from memployee where id='" + dr["Immediatesuperior"].ToString() + "'");
            a.scc = dtrptto.Rows.Count > 0 ? dtrptto.Rows[0]["id"].ToString() : "0";//reportto
            DataTable sectionid = dbhelper.getdata("select * from msection where seccode='" + dr["Sectioncode"].ToString() + "'");
            a.sdd = sectionid.Rows.Count > 0 ? sectionid.Rows[0]["sectionid"].ToString() : "0";//sectionid
            string val = bol.newemployee(a);

            dbhelper.getdata("insert into Approver (emp_id,under_id,herarchy)values(" + val + "," + a.scc + ",0)");
       }






        //string value;
        //int cnt_column = 1;
        //    foreach (DataColumn dc in DtSet.Tables[0].Columns)
        //    {
        //        for (int i = 0; i <= DtSet.Tables[0].Rows.Count; i++)
        //        {

        //            if (dc.ColumnName.Contains("table"))
        //            {
        //                string[] arr_cn = dc.ColumnName.Split('-');
        //                DataTable dt = dbhelper.getdata("select * from " + arr_cn[0].ToString() + " where " + arr_cn[0].ToString().Replace("M", "") + "=" + DtSet.Tables[0].Rows[1].ToString() + "  ");
        //            }
        //             cnt_column++;
        //        }
        //    }

        //DataTable ndt = new DataTable();
        //ndt.Columns.Add(new DataColumn("Idnumber", typeof(string)));
        //ndt.Columns.Add(new DataColumn("employee", typeof(string)));
        //ndt.Columns.Add(new DataColumn("date", typeof(string)));
        //DataTable dt = DtSet.Tables[0];
    }
}