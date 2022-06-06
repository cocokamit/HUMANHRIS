using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class temp_ReadNotification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.QueryString["key"].ToString();
        function.ReadNotification(id);
        DataTable dt = dbhelper.getdata("select convert(varchar,[date],101) [sent], * from sys_notification where id=" + id);

        string url = string.Empty;
        switch (dt.Rows[0]["subject"].ToString())
        {
            case "Schedule Approval":
                url = dt.Rows[0]["url"].ToString() + "?key=" + dt.Rows[0]["key"].ToString();
                break;
            default:
                url = dt.Rows[0]["url"].ToString();
                break;
        }

        Response.Redirect(url);
    }
}