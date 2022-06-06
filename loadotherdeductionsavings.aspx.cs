﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

public partial class loadotherdeductionsavings : System.Web.UI.Page
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
            DataTable dt = dbhelper.getdata("select * from MOtherDeduction where OtherDeduction='" + dr["Other_deduction"].ToString() + "' ");
            DataTable dtemp = dbhelper.getdata("select * from memployee where idnumber='" + dr["ID_NUMBER"].ToString() + "' ");
            string otherdeductid = dt.Rows.Count > 0 ? dt.Rows[0]["id"].ToString() : "0";
            string empid = dtemp.Rows[0]["id"].ToString();


            string query = "INSERT INTO other_deduction_saving  " +
                        "([date],[other_deduction_id],[empid],[userid],[amount]) " +
                        "VALUES " +
                        "(GETDATE()," + otherdeductid + "," + empid + ",0,'" + dr["Amount"].ToString() + "')";

            dbhelper.getdata(query);

        }
    }
}