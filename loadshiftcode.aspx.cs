using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

public partial class loadshiftcode : System.Web.UI.Page
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
        string ret;
        foreach (DataRow dr in DtSet.Tables[0].Rows)
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("load_shiftcode", con))
                {
                    DateTime timein = Convert.ToDateTime(dr["TimeIn"].ToString());
                    //DateTime Outforbreak = Convert.ToDateTime(dr["Outforbreak"].ToString());
                    //DateTime Infrombreak = Convert.ToDateTime(dr["Infrombreak"].ToString());
                    DateTime TimeOut = Convert.ToDateTime(dr["TimeOut"].ToString());
                    decimal noh;
                    TimeSpan gethrs = new TimeSpan();
                    TimeSpan gethrs1 = new TimeSpan();
                    TimeSpan gethrs2 = new TimeSpan();
                    TimeSpan br = new TimeSpan();

                    string sth = "0";
                    string ndh = "0";
                    string mid = "0";
                    string tn = "0";
                    string nightdeduct = "0";
                    int fnight=0;
                    if (dr["PaidBreak"].ToString() == "p")
                    {
                        gethrs = TimeOut - timein;
                        noh = decimal.Parse(gethrs.TotalHours.ToString());
                        mid = getnightdif.getnight(timein.ToString(), TimeOut.ToString(), "shiftcode");
                        tn = mid;
                    }
                    else
                    {

                       // br = Infrombreak - Outforbreak;
                        gethrs = TimeOut - timein;

                        mid = "1";//decimal.Parse(br.TotalHours.ToString()).ToString();
                        noh = decimal.Parse(gethrs.TotalHours.ToString()) - decimal.Parse(mid);
                        //gethrs1 = Outforbreak - timein;
                        //gethrs2 = TimeOut - Infrombreak;
                        //noh = decimal.Parse(gethrs1.TotalHours.ToString()) + decimal.Parse(gethrs2.TotalHours.ToString());

                        //sth = getnightdif.getnight(timein.ToString(), Outforbreak.ToString(), "shiftcode");
                        //ndh = getnightdif.getnight(Infrombreak.ToString(), TimeOut.ToString(), "shiftcode");
                        //tn = sth + ndh;
                    }
                    //if (decimal.Parse(tn) > 0)
                    //    fnight = 1;

                    //nightdeduct = getnightdif.getnight(Outforbreak.ToString(), Infrombreak.ToString(), "shiftcode");
             

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ShiftCode", SqlDbType.VarChar, 5000).Value = dr["ShiftCode"].ToString();
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 5000).Value = dr["ShiftCode"].ToString();
                    cmd.Parameters.Add("@EntryUserId", SqlDbType.Int).Value = "1";
                    cmd.Parameters.Add("@TimeIn1", SqlDbType.VarChar, 5000).Value = timein.ToString("hh:mm tt");
                    cmd.Parameters.Add("@TimeOut1", SqlDbType.VarChar, 5000).Value = "0:00:00";
                    cmd.Parameters.Add("@TimeIn2", SqlDbType.VarChar, 5000).Value = "0:00:00";
                    cmd.Parameters.Add("@TimeOut2", SqlDbType.VarChar, 5000).Value = TimeOut.ToString("hh:mm tt");
                    cmd.Parameters.Add("@NumberOfHours", SqlDbType.VarChar, 5000).Value = "8";
                    cmd.Parameters.Add("@LateFlexibility", SqlDbType.VarChar, 5000).Value = "0";
                    cmd.Parameters.Add("@LateGraceMinute", SqlDbType.VarChar, 5000).Value = "0";
                    cmd.Parameters.Add("@NightHours", SqlDbType.VarChar, 5000).Value = tn;
                    cmd.Parameters.Add("@nighshift", SqlDbType.VarChar, 5000).Value = fnight;
                    cmd.Parameters.Add("@nightbreakhours", SqlDbType.VarChar, 50).Value = mid;
                    string bwp;
                    if (dr["PaidBreak"].ToString() == "p")
                        bwp = "True";
                    else
                        bwp = "False";
                    cmd.Parameters.Add("@breakwithpay", SqlDbType.VarChar, 50).Value = "False";
                    cmd.Parameters.Add("@mandatorytopunch", SqlDbType.VarChar, 5000).Value = "True";
                    cmd.Parameters.Add("@flexbreak", SqlDbType.VarChar, 5000).Value = "True";
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }
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