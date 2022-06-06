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
            DataTable dtselect = dbhelper.getdata("select * from Tfinalexit where id=" + id + " ");
            string query11 = "select top 1 ROW_NUMBER() over( order by herarchy asc ) rowcnt,emp_id,under_id,herarchy from  " +
                                  "( " +
                                  "select emp_id,under_id,herarchy from approver where emp_id=" + dtselect.Rows[0]["empid"].ToString() +" and status is null  " +
                                  "union " +
                                  "select '0',approver_id,cnt from msetup_clearance_hierarchy where action is null  " +
                                  ") tt ";
            DataTable dttt = dbhelper.getdata(query11);
            stateclass a = new stateclass();
            a.sa = id;
            a.sb = "0";
            a.sc = "Resignation";
            a.sd = type;
            a.se = "ok";
            DataTable dt;
            switch (type)
            {
                case "Approved":
                    bol.system_logs(a);
                    dbhelper.getdata("update Tfinalexit set status='" + type + "' where id=" + id + "   insert into Texitclearance (date,empid,nextapproverid,nextheirarchy,status,resignid)values(getdate()," + dtselect.Rows[0]["empid"].ToString() + "," + dttt.Rows[0]["under_id"].ToString() + "," + dttt.Rows[0]["rowcnt"].ToString() + ",'Pending'," + id + ")");
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