using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class update : System.Web.UI.Page
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
    protected void loadupdate(object sender, EventArgs e)
    {
        if (FileUpload2.HasFile)
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload2.FileName));
            FileUpload2.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();

            ViewState["test"] = DtSet.Tables[0];

            GridView2.DataSource = DtSet.Tables[0];
            GridView2.DataBind();

            DataTable dt = DtSet.Tables[0];

            DataRow[] dr = dt.Select();
            Button1.Visible = true;
        }
        else
        {
            Response.Write("Please Load Template!!!");
        }
    }
    protected void updatetable(object sender, EventArgs e)
    {
        DataTable dtttt = ViewState["test"] as DataTable;
        foreach (DataRow dr in dtttt.Rows)
        {
            stateclass a = new stateclass();

            a._idnumber = dr["IdNumber"].ToString().Length > 0 ? dr["IdNumber"].ToString() : "0";
            a._hourlyrate = dr["OldRate"].ToString().Length > 0 ? dr["OldRate"].ToString() : "0.00";

            string query = "select b.id id from app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id "
                + "left join MPayrollType c on a.paytypeid=c.Id left join MEmployee d on d.Id=b.empid "
                + "where d.IdNumber = '" + a._idnumber + "'";

            DataTable dt = dbhelper.getdata(query);

            dbhelper.getdata("update app_trn_salaryinc set lastsal='" + a._hourlyrate + "' where app_trn_id ='" + dt.Rows[0]["id"] + "'");
        }
        Response.Write("<script>alert('File Upload Successful!')</script>");
    }
}