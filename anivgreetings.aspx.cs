using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class anivgreetings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            display();
        }
    }
    protected void display()
    {
        string name = Request.QueryString["name"].ToString();
        ViewState["names"] = name;
        string age = Request.QueryString["a"].ToString();
        ViewState["year"] = age;
        string withs = Request.QueryString["b"].ToString();
        ViewState["ones"] = withs;
    }
}