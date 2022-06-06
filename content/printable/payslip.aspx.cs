using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;



public partial class printable_payslip : System.Web.UI.Page
{
    public static string empid, payid;
    public static decimal totaldeductions=0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["b"].ToString() == "single")
            {
                empid = function.Decrypt(Request.QueryString["empid"].ToString(), true);
            }
            payid = function.Decrypt(Request.QueryString["payid"].ToString(), true);
            
            //disp();
        }
    }

}