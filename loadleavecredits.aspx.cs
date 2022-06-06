using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

public partial class loadleavecredits : System.Web.UI.Page
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
                //DataTable dtchk = dbhelper.getdata("select * from mleave where leave='" + dr["Leave_type"] + "'");
                DataTable dtchkemp = dbhelper.getdata("select * from memployee where idnumber='" + dr["Id_number"] + "'");
                //string empid = dtchkemp.Rows[0]["Id"].ToString();
                string empid = dtchkemp.Rows.Count > 0 ? dtchkemp.Rows[0]["id"].ToString() : "0";
                //string leaveid = dtchk.Rows.Count > 0 ? dtchk.Rows[0]["id"].ToString() : "0";
                if (dr["Id_number"].ToString().Length > 0)
                {
                    //query = "insert into leave_credits values (getdate(),0," + empid + "," + leaveid + "," + dr["credit"] + ",'" + dr["Convert_to_cash"] + "','Yearly',NULL)";
                    //dbhelper.getdata(query);
                    string query="";
                    //if (dr["Service Incentive Leave"].ToString().Length > 0)
                    //{
                    //    query = "insert into leave_credits values (getdate(),0," + empid + ",9," + dr["Service Incentive Leave"].ToString() + ",'no','Yearly',NULL)";
                    //    dbhelper.getdata(query);
                    //}
                    //if (dr["Birthday Leave"].ToString().Length > 0)
                    //{
                    //    query = "insert into leave_credits values (getdate(),0," + empid + ",10," + dr["Birthday Leave"].ToString() + ",'no','Yearly',NULL,'2019')";
                    //    dbhelper.getdata(query);
                    //}
                    if (dr["Vacation Leave"].ToString().Length > 0)
                    {
                        query = "insert into leave_credits values (getdate(),0," + empid + ",2," + dr["Vacation Leave"].ToString() + ",'no','Yearly',NULL,'2019')";
                        dbhelper.getdata(query);
                    }
                    //if (dr["Sick Leave"].ToString().Length > 0)
                    //{
                    //    query = "insert into leave_credits values (getdate(),0," + empid + ",1," + dr["Sick Leave"].ToString() + ",'no','Yearly',NULL,'2019')";
                    //    dbhelper.getdata(query);
                    //}
                    
                    //if (dr["Emergency Leave"].ToString().Length > 0)
                    //{
                    //    query = "insert into leave_credits values (getdate(),0," + empid + ",15," + dr["Emergency Leave"].ToString() + ",'no','Yearly',NULL,'2019')";
                    //    dbhelper.getdata(query);
                    //}
                    //if (dr["Bereavement  Leave"].ToString().Length > 0)
                    //{
                    //    query = "insert into leave_credits values (getdate(),0," + empid + ",14," + dr["Bereavement  Leave"].ToString() + ",'no','Yearly',NULL,'2019')";
                    //    dbhelper.getdata(query);
                    //}
                   
                }
            }
    }
}