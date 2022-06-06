using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class content_hr_verify_clearance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
    [WebMethod]
    public static string bindRecordToEdit(string id,string type)
    {
        string data = string.Empty;
        try
        {
            DataTable dtselect = dbhelper.getdata("select * from texitclearance where id=" + id + " ");
            stateclass a = new stateclass();
            a.sa = id;
            a.sb = "0";
            a.sc = "Clearance";
            a.sd = type;
            a.se = "ok";
            DataTable dt;
            switch (type)
            {
                case "Approved":
                    bol.system_logs(a);
                    dbhelper.getdata("update texitclearance set status='" + type + "',date_accomplished=getdate() where id=" + id + " insert into quit_claim_request (date,userid,empid,notes,status,clearanceid)values(getdate(),'1'," + dtselect.Rows[0]["empid"].ToString() + ",'Request for Quit Claim','Pending',"+id+")");
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
    protected void compose(object sender, EventArgs e)
    {
    }
    protected void cancel(object sender, EventArgs e)
    {
    }
}