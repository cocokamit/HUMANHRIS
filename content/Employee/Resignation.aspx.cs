using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;

public partial class content_Employee_Resignation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
    [WebMethod]
    public static string bindRecordToEdit(int id,string type)
    {
        string data = string.Empty;
        DataTable dt;
        try
        {
            switch (type)
            { 
                case "Deleted":
                     dt = dbhelper.getdata("update Tfinalexit set action='Deleted' where id=" + id + "");
                    data = "success";
                    data = data.Length.ToString();
                    break;
                case "View":
                     dt = dbhelper.getdata("select * from Tfinalexit where id=" + id + "");
                     data = dt.Rows[0]["content"].ToString();
                    break;
            }
            return data;     
        }
        catch { return data; }
    }
    protected void compose(object sender, EventArgs e)
    {
       
        Response.Redirect("kiosk_comp_res");
    }
    protected void cancel(object sender, EventArgs e)
    {
        Button lnkcan = sender as Button;
        string njnj = lnkcan.CommandName;
    }
}