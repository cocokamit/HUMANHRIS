using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Text;

public partial class content_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
        {
            logout();
        }

        if(!IsPostBack)
        {
            lbl_userid.Text = Session["user_id"].ToString();
            disp();
        }
    }

    protected void logout()
    {
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }

    protected void click_out(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }

    protected void disp()
    {
        string query = "select b.formname,b.remarks from MUserForm a " +
                       "left join SysForm b on a.FormId=b.Id " +
                       "where a.UserId='" + lbl_userid.Text + "' and a.CanAllow='True' ";
                           //"union ALL " +
                           //"select formname,remarks FROM SysForm where standard='EMPLOYEE' " ;
            DataTable dt = dbhelper.getdata(query);
            grid_view.DataSource = dt;
            grid_view.DataBind();

            for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
            {
                string tt = grid_view.Rows[i].Cells[1].Text;
            }

            if (dt.Rows.Count == 0)
            {
                Session.RemoveAll();
                Session.Abandon();
                Response.Redirect("quit?key=out");
            }
    }
}
