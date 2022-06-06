using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;


public partial class content_Manager_resignationlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
    [WebMethod]
    public static string bindRecordToEdit(string id, string empid, string type, string nxtappid)
    {
        string data = string.Empty;
        try
        {
            DataTable dtselect = dbhelper.getdata("select * from Tfinalexit where id="+id+" ");
            stateclass a = new stateclass();
            a.sa = id;
            a.sb = empid; //dtselect.Rows[0]["empid"].ToString();
            a.sc = "Resignation";
            a.sd = type;
            a.se = "ok";
            DataTable dt;
            switch (type)
            {
                case "Approved":
                        bol.system_logs(a);
                        if (int.Parse(nxtappid) > 0)
                            dbhelper.getdata("update Tfinalexit set nextapproverid=" + nxtappid + " where id=" + id + "");
                        else
                            dbhelper.getdata("update Tfinalexit set nextapproverid=" + nxtappid + ",status='Verification' where id=" + id + "");

                        data = "success";
                        data = data.Length.ToString();
                    break;
                case "Deleted":
                        bol.system_logs(a);
                        dbhelper.getdata("update Tfinalexit set action='Deleted' where id=" + id + "");
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