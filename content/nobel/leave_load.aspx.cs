using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

public partial class content_nobel_leave_load : System.Web.UI.Page
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
        string query = string.Empty;
        DataTable employees = dbhelper.getdata("select * from memployee");

        string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
        FileUpload1.SaveAs(path);

        string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
        DataTable dt = new DataTable();
        OleDbDataAdapter MyCommand;
        OleDbConnection MyConnection = new OleDbConnection();
        MyConnection.ConnectionString = excelConnectionString;
        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
        MyCommand.Fill(dt);
        MyConnection.Close();

        DataTable columns = new DataTable();
        columns.Columns.Add("id");
        columns.Columns.Add("field");
        foreach (DataColumn column in dt.Columns)
        {
            if (column.Ordinal > 0)
            {
            Nobel:
                DataTable leave = dbhelper.getdata("select * from mleave where leave='" + column.ColumnName + "'");
                if (leave.Rows.Count == 0)
                {
                    leave = dbhelper.getdata("insert into Mleave (LeaveType,Leave) values ('" + GetInitial(column.ColumnName) + "','" + column.ColumnName + "')");
                    goto Nobel;
                }
                columns.Rows.Add(leave.Rows[0]["id"], leave.Rows[0]["leave"]);
            }
        }

        foreach (DataRow dr in dt.Rows)
        {
            DataRow[] employee = employees.Select("idnumber='" + dr[0] + "'");
            if (employee.Length > 0)
            {
                foreach (DataRow column in columns.Rows)
                {
                    string credit = dr[column["field"].ToString()].ToString().Length == 0 ? "0" : dr[column["field"].ToString()].ToString();
                    query += "insert into leave_credits values (getdate(),0," + employee[0]["id"] + "," + column["id"] + "," + credit  + ",'yes','Yearly',NULL,year(getdate()));";
                    //dbhelper.getdata(query);
                    
                }
            }
        }

        dbhelper.getdata(query);
        pnl_process.Visible = false;
        callout.Visible = true;
    }

    protected string GetInitial(string x)
    {
        string hold = string.Empty;
        string[] split = x.Split(' ');
        foreach (string str in split)
        {
            hold += str.Substring(0, 1);
        }

        return hold;
    }
}