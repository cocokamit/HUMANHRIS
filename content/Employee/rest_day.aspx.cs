using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_rest_day : System.Web.UI.Page
{
    //public static string user_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
    }



    protected void click_save_ot(object sender, EventArgs e)
    {
        if (checker())
        {
            DataTable dt = dbhelper.getdata("select * from TRestdaylogs where EmployeeId='" + Session["emp_id"].ToString() + "' and left(convert(varchar,Date,101),10)='" + txt_otd.Text.Trim() + "' and status like '%Approved%'");
            if (dt.Rows.Count == 0)
            {

                //DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
                DataTable dtinserthdrd = dbhelper.getdata("insert into TRestdaylogs values (" + Session["emp_id"].ToString() + ",NULL,'" + txt_otd.Text.Trim() + "','" + txt_lineremarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,GETDATE(),'" + approver_id.Rows[0]["under_id"].ToString() + "') select scope_identity() id");
                function.AddNotification("RD/HD Approval", "rd", approver_id.Rows[0]["under_userid"].ToString(), dtinserthdrd.Rows[0]["id"].ToString());
            }
            else
                Response.Write("<script>alert('already exist!')</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_Restday'", true);
        }
    }

    protected bool checker()
    {
        bool oi = true;


        if (txt_otd.Text.Length < 10)
        {
            oi = false;
            lbl_date.Text = "Invalid date";
        }
        else
            lbl_date.Text = "";

        if (txt_lineremarks.Text == "")
        {
            oi = false;
            lbl_reason.Text = "*";
        }
        else
            lbl_reason.Text = "";

        return oi;

    }
}