using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class temp_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void click_back(object sender, EventArgs e)
    {
        switch (Session["role"].ToString())
        {
            case "EMPLOYEE":
                Response.Redirect("employee-dashboard?user_id=" + Session["_user"].ToString());
                break;
            default:
                break;
        }    
    }
}