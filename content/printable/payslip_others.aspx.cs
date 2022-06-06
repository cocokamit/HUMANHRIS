using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class content_printable_payslip_others : System.Web.UI.Page
{
    public static string empid, payid;
    public static decimal totaldeductions = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["b"].ToString() == "single")
            {
                empid = Request.QueryString["key"].ToString();
            }
            payid = Request.QueryString["trn"].ToString();

            //disp();
        }
    }
}