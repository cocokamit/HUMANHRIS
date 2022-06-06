using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_setup_leavesetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            this.disp();
    }
    protected void disp()
    {
        string query = "select * from MLeave where action is null";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

    }
    protected void close(object sender, EventArgs e)
    {
        Div1.Visible = false;
        pAdd.Visible = false;
    }
    protected void add(object sender, EventArgs e)
    {
        Div1.Visible = true;
        pAdd.Visible = true;
        pAdd.Style.Add("width", "450px");
    }
    protected void savedata(object sender, EventArgs e)
    {
        string ret="0";
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("save_leave", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LeaveType", SqlDbType.VarChar, 50).Value = txt_lt.Text;
                cmd.Parameters.Add("@Leave", SqlDbType.VarChar,50).Value =txt_desc.Text;
                cmd.Parameters.Add("@yearlytotal", SqlDbType.VarChar, 50).Value = txt_yt.Text;
                cmd.Parameters.Add("@converttocash", SqlDbType.VarChar, 50).Value = chk_convertable.Checked==true?"yes":"no";
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                ret = cmd.Parameters["@out"].Value.ToString();
                con.Close();
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='leavesetup?user_id=" + Request.QueryString["user_id"].ToString() + "'", true);
    }
    protected bool chk()
    {
        bool ret = true;
        if (txt_lt.Text.Length > 0)
        {
            lbl_lt.Text = "*";
            ret = false;
        }
        else if (txt_desc.Text.Length > 0)
        {
            lbl_desc.Text = "*";
            ret = false;
        }
        else if (txt_yt.Text.Length > 0)
        {
            lbl_yt.Text = "*";
            ret = false;

        }
        return ret;
    }
}