using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bdaygreetings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {
        if (Request.QueryString["bd"].ToString() == "True" && Request.QueryString["ann"].ToString() == "True")
        {
            lbl_gretmsg.Text = "Happy Birthday & Anniversary";
        }
        else
        {
            if (Request.QueryString["bd"].ToString() == "True")
                lbl_gretmsg.Text = "Happy Birthday";
            if (Request.QueryString["ann"].ToString() == "True")
                lbl_gretmsg.Text = "Happy Anniversary";
        }
    }
}