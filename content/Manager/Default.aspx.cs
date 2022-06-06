using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class content_Manager_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static string trigger(string id, string current_appid, string type, string nxtherarchy)
    {
        string data = string.Empty;
        try
        {
            string query = ";with nxtapprover as " +
                                   "( " +
                                       "select * from  " +
                                       "( " +
                                           "select  ROW_NUMBER() over( order by herarchy asc )rowcnt,emp_id,under_id,herarchy from  " +
                                           "( " +
                                           "select emp_id,under_id,herarchy from approver where emp_id=" + current_appid + " and status is null  " +
                                           "union " +
                                           "select '0',approver_id,cnt from msetup_clearance_hierarchy where action is null  " +
                                           ") tt  " +
                                       ")yy " +
                                   ") " +
                                   "select a.rowcnt,a.emp_id,a.under_id,case when (select under_id from nxtapprover where rowcnt=a.rowcnt+1 ) is null then 0 else (select under_id from nxtapprover where rowcnt=a.rowcnt+1 ) end nxtapproverid,a.rowcnt+1 nextheirarchy from nxtapprover a " +
                                   "where under_id=" + current_appid + " and a.rowcnt=" + nxtherarchy + "";
            DataTable nxt = dbhelper.getdata(query);

            DataTable dtselect = dbhelper.getdata("select * from texitclearance where id=" + id + " ");
            stateclass a = new stateclass();
            a.sa = id;
            a.sb = dtselect.Rows[0]["empid"].ToString();
            a.sc = "Clearance";
            a.sd = type + "-" + DateTime.Now.ToShortDateString().ToString();
            a.se = "ok";
            DataTable dt;
            switch (type)
            {
                case "Approved":
                    //bol.system_logs(a);
                    //if (int.Parse(nxtappid) > 0)
                    //    dbhelper.getdata("update texitclearance set nextapproverid=" + nxtappid + ",nexthierarchy="++" where id=" + id + "");
                    //else
                    //    dbhelper.getdata("update texitclearance set nextapproverid=" + nxtappid + ",nexthierarchy="++",status='Verification' where id=" + id + "");

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
}