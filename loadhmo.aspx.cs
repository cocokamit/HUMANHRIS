using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

public partial class loadhmo : System.Web.UI.Page
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
           
            DataTable dtchkemp = dbhelper.getdata("select * from memployee where idnumber='" + dr["ID_NUMBER"] + "'");
            string prinid = dtchkemp.Rows.Count > 0 ? dtchkemp.Rows[0]["id"].ToString() : "0";
            string userid = "0";

            string query="insert into Mhmo 	(date,userid,prinid,membertype,fname,mname,lname,reltoprincipal,roomcategory,contractno,coverage_date_from,coverage_date_to,total_limit_amt,total_limit_premium,remarks,payable,amortization,insurancename 	)  " +
                        "values " +
                        "( " +
                        "GETDATE()," + userid + "," + prinid + ",'" + dr["Membertype"].ToString() + "','" + dr["Fname"].ToString() + "','" + dr["Mname"].ToString() + "','" + dr["Lname"].ToString() + "','" + dr["Relationshiptoprincipal"].ToString() + "','" + dr["RoomCategory"].ToString() + "','" + dr["InsuranceNumber"].ToString() + "','" + dr["DateFrom"].ToString() + "','" + dr["DateTo"].ToString() + "','" + dr["Coverage_Amt"].ToString() + "','" + dr["Premium"].ToString() + "','" + dr["remarks"].ToString() + "','" + dr["Payable"].ToString() + "','" + dr["Amortization"].ToString() + "','" + dr["InsuranceName"].ToString() + "'  " +
                        ")";
            dbhelper.getdata(query);

        }
    }
}