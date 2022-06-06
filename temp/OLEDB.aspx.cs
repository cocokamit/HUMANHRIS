using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class temp_OLEDB : System.Web.UI.Page
{
    public static string CltConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Administrator\Desktop\Movenpick\attMovenpick.mdb;Uid=Admin;Pwd=xx;Persist Security Info=False";
       
    protected void Page_Load(object sender, EventArgs e)
    {
        //UPDATE [CHECKINOUT] SET [export]=''
    }

    public static DataTable OleDataTable(string query)
    {
        DataTable dt = new DataTable();
        OleDbConnection con = new OleDbConnection(CltConnection);
        OleDbCommand cmd = new OleDbCommand(query, con);
        OleDbDataAdapter da = new OleDbDataAdapter();
        cmd.CommandType = CommandType.Text;
        con.Open();
        da.SelectCommand = cmd;
        da.Fill(dt);
        con.Close();
        return dt;
    }

    protected void click_execute(object sender, EventArgs e)
    {
      DataTable dt = OleDataTable(tb_query.Text.Trim());

      if (dt.Rows.Count > 0)
      {
          grid.DataSource = dt;
          grid.DataBind();
      }
    }
}