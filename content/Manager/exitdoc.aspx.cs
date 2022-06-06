using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class content_Manager_exitdoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
   // [WebMethod]
    [WebMethod]
    public static string bindRecordToEdit(string id, string current_appid, string type, string nxtherarchy)
    {
        string data = string.Empty;
        try
        {
            DataTable dtselect = dbhelper.getdata("select * from texitclearance where id=" + id + " ");
            string query = ";with nxtapprover as " +
                                   "( " +
                                       "select * from  " +
                                       "( " +
                                           "select  ROW_NUMBER() over( order by herarchy asc )rowcnt,emp_id,under_id,herarchy from  " +
                                           "( " +
                                           "select emp_id,under_id,herarchy from approver where emp_id=" + dtselect.Rows[0]["empid"].ToString() + " and status is null  " +
                                           "union " +
                                           "select '0',approver_id,cnt from msetup_clearance_hierarchy where action is null  " +
                                           ") tt  " +
                                       ")yy " +
                                   ") " +
                                   "select a.rowcnt,a.emp_id,a.under_id,case when (select under_id from nxtapprover where rowcnt=a.rowcnt+1 ) is null then 0 else (select under_id from nxtapprover where rowcnt=a.rowcnt+1 ) end nxtapproverid,a.rowcnt+1 nextheirarchy from nxtapprover a " +
                                   "where under_id=" + current_appid + " and a.rowcnt=" + nxtherarchy + "";
            DataTable nxt = dbhelper.getdata(query);

            stateclass a = new stateclass();
            a.sa = id;
            a.sb = current_appid;
            a.sc = "Clearance";
            a.sd = type;
            a.se = "ok";
            DataTable dt;
            switch (type)
            {
                case "Approved":
                    bol.system_logs(a);
                    if (int.Parse(nxt.Rows[0]["nxtapproverid"].ToString()) > 0)
                        dbhelper.getdata("update texitclearance set nextapproverid=" + nxt.Rows[0]["nxtapproverid"].ToString() + ",nextheirarchy=" + nxt.Rows[0]["nextheirarchy"].ToString() + " where id=" + id + "");
                    else
                        dbhelper.getdata("update texitclearance set nextapproverid=" + nxt.Rows[0]["nxtapproverid"].ToString() + ",nextheirarchy=" + nxt.Rows[0]["nextheirarchy"].ToString() + ",status='Verification' where id=" + id + "");

                    data = "success";
                    data = data.Length.ToString();
                    break;
                case "Deleted":
                    bol.system_logs(a);
                    dbhelper.getdata("update texitclearance set action='Deleted' where id=" + id + "");
                    data = "success";
                    data = data.Length.ToString();
                    break;
                case "View":
                    dt = dbhelper.getdata("select * from texitclearance where id=" + id + "");
                    data = dt.Rows[0]["content"].ToString();
                    break;
            }

            return data;
        }
        catch { return data; }
    }
}