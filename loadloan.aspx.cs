using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class loadloan : System.Web.UI.Page
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

            //String[] excelSheets = new String[dt.Rows.Count];
            //int i = 0;

            //// Add the sheet name to the string array.
            //foreach (DataRow row in dt.Rows)
            //{
            //    excelSheets[i] = row["TABLE_NAME"].ToString();
            //    i++;
            //}

            //// Loop through all of the sheets if you want too...
            //for (int j = 0; j < excelSheets.Length; j++)
            //{
            //    // Query each excel sheet.
            //}

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
            string idno = dr["idnumber"].ToString();
            string loan = dr["Loantype"].ToString();
            string amount = dr["LoanAmount"].ToString().Replace(",", "");
            string amort = dr["Semi-MonthlyAmortization"].ToString().Replace(",", "");
            double number = double.Parse(dr["Numberofmonths"].ToString());
            string date = dr["Datestart"].ToString();
            string remarks = dr["Remarks"].ToString().Replace("'", "");

            DataTable dt = dbhelper.getdata("select id,* from Memployee where IdNumber='" + idno + "'");
            DataTable loan_dt = dbhelper.getdata("select id,interest,* from MOtherDeduction where OtherDeduction='" + loan + "'");



            dbhelper.getdata("insert into MEmployeeLoan (EmployeeId,OtherDeductionId,LoanNumber,LoanAmount,MonthlyAmortization,NumberofMonths,DateStart,TotalPayment,Balance,IsPaid,EntryUserId,EntryDatetime,UpdateUserId,UpdateDateTime,Status,Remarks,interest) " +
            " values(" + dt.Rows[0]["id"] + "," + loan_dt.Rows[0]["id"].ToString() + ",0,'" + amount + "','" + amort + "','" + number + "','" + date + "',0,'" + amount + "','False',0,'" + date + "',0,'"+date+"','Approved','" + remarks + "','" + loan_dt.Rows[0]["interest"].ToString() + "')");
         

        }

    }
}