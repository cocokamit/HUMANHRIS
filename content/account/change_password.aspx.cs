using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_account_change_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

    }
    protected void save(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from nobel_user where emp_id = '" + Session["emp_id"] + "'");
        if (dt.Rows[0]["password"].ToString() == txt_cur.Text)
        {
            stateclass a = new stateclass();

            a.sa = txt_cur.Text.Replace(" ", "");
            a.sb = txt_new.Text.Replace(" ", "");
            a.sd = txt_confirm.Text.Replace(" ", "");
            a.sc = Session["emp_id"].ToString();

            hf_id.Value = Session["emp_id"].ToString();

            if (txt_new.Text == "" && txt_confirm.Text == "" && txt_confirm.Text == "")
                Response.Write("<script>alert('Please Fill In Data!')</script>");
            else
            {
                if (a.sb == a.sd)
                {
                    string x = bol.change_pass(a);

                    if (x == "1")
                    {

                        Response.Write("<script>alert('Successfully Save!')</script>");
                    }
                    else
                        Response.Write("<script>alert('Account Does Not Regestered!')</script>");
                }
                else
                    Response.Write("<script>alert('Password Does Not Match!')</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('Forgot Password!')</script>");
        }
    }
}