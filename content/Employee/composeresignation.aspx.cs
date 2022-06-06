using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class content_Employee_composeresignation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
    protected void Save_Resignation(object sender, EventArgs e)
    {
        string ret = "";
        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " order by herarchy asc ");
        if (chkinput())
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("Tspexit", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empid", SqlDbType.Int).Value = Session["emp_id"].ToString();
                    cmd.Parameters.Add("@exittype", SqlDbType.VarChar, 5000).Value = "Resignation";
                    cmd.Parameters.Add("@content", SqlDbType.VarChar, 500000).Value = composetextarea.Text;
                    cmd.Parameters.Add("@nextapproverid", SqlDbType.Int).Value = approver_id.Rows.Count > 0 ? approver_id.Rows[0]["under_id"].ToString() : "0";
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();
                    if (ret == "Failed")
                        lbl_errmsg.Text = "Invalid Double Entry";
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='kiosk_res'", true);

                }
            }
        }
    }
    protected bool chkinput()
    {
        bool ret = true;
        if (composetextarea.Text.Length == 0)
        {
            lbl_errmsg.Text = "Invalid Input";
            ret = false;
        }
        else
            lbl_errmsg.Text = "";

        return ret;
    }
}