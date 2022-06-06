using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading;
using System.DirectoryServices;
using System.Runtime.InteropServices;


public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        //    if (!IsPostBack)
        //    {
        //        string[] uri = Request.Url.AbsoluteUri.ToString().Split('/');
        //        if (uri[0] == "http:")
        //            Response.Redirect("https://mycli.cebulandmasters.com/");

        //    }
        }
        //decimal hh = decimal.Parse("508.3890487000") / (decimal.Parse("1.30") *100 )* 100 - decimal.Parse("508.3890487000");
        //Response.Write(hh);
      
    }

    protected bool Login(string userName)
    {
        System.Collections.Generic.List<string> d = Application["UsersLoggedIn"]as System.Collections.Generic.List<string>;
        if (d != null)
        {
            lock (d)
            {
                if (d.Contains(userName))
                {
                    // User is already logged in!!!
                    return false;
                }
                d.Add(userName);
            }
        }
        Session["UserLoggedIn"] = userName;
        return true;
    }

    protected void click_login(object sender, EventArgs e)
    {
        stateclass sc = new stateclass();
        sc.sa = txt_user.Text;
        sc.sb = txt_pass.Text;
        string x = bol.procedures(sc, "auth");

        if (x == "0")
            lbl_msg.Text = "Incorrect login account";
        else
        {
            DataTable dt = getdata.access(x);
            Session["user_id"] = x;
            Session["ngalan"] = dt.Rows[0]["name"].ToString();
            Session["role"] = dt.Rows[0]["acc_id"].ToString();
            Session["emp_id"] = dt.Rows[0]["emp_id"].ToString();
            Response.Redirect(dt.Rows[0]["url"].ToString());
        }
    }

    protected void click_go(object sender, EventArgs e)
    {
        string ip = HttpContext.Current.Request.UserHostAddress.ToString();
        stateclass sc = new stateclass();
        sc.sa = txt_user.Text;
        sc.sb = txt_pass.Text;
        DataTable dtu = dbhelper.getdata("select Password from MUser where Id=1");
        DataTable dteu = dbhelper.getdata("select TOP(1) case when a.password='superadmin' then '2' else b.statusid end statusid,"
            + "case when a.emp_id=0 then 'True' when c.allowAccess is NULL then 'False' else c.allowAccess end allowAccess from nobel_user a "
            + "left join memployeestatus b on a.emp_id = b.empid left join MEmployee c on c.Id = a.emp_id where "
            + "a.username = '" + txt_user.Text + "' order by empstatid desc");

        if (txt_pass.Text == dtu.Rows[0]["Password"].ToString() && txt_user.Text == "Administrator")
        {

            string val = bol.authentication(sc);

            if (val == "0")
                lbl_msg.Text = "*Incorrect login account";
            else
            {
                Session["user_id"] = val.Substring(0, val.IndexOf("@"));
                Session["ngalan"] = val.Substring(val.IndexOf('@') + 1).Replace("" + val.Substring(val.IndexOf('*')) + "", "");
                Session["role"] = val.Substring(val.IndexOf('*') + 1).Replace("" + val.Substring(val.IndexOf('-')) + "", "");
                Session["emp_id"] = val.Substring(val.IndexOf('-') + 1);
                Session["profile"] = "0";

                string user_id = function.Encrypt(Session["user_id"].ToString(), true);
                if (Session["role"].ToString() == "Admin")
                {
                    //DataTable dt = dbhelper.getdata("select id,IdNumber,LastName+' '+FirstName as fullname,DATEDIFF(DAY, getdate(), health_card)as dayss, " +
                    //                              "left(convert(varchar,health_card,101),10)health_card,healthcard_status " +
                    //                              "from Memployee where health_card is not null and DATEDIFF(DAY, getdate(), health_card) < 10");

                    //for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    //{
                    //    if (dt.Rows[i]["healthcard_status"].ToString() == "")
                    //    {
                    //        function.AddNotification("Health Card expiration", "addemployee?app_id=" + dt.Rows[i]["id"].ToString() + "&tp=ed", "0", dt.Rows[i]["id"].ToString());
                    //        dbhelper.getdata("update Memployee set healthcard_status='1' where id=" + dt.Rows[i]["id"].ToString() + " ");
                    //    }
                    //}

                    Response.Redirect("homepage?user_id=" + user_id + "");
                }
                else
                    Response.Redirect("homepage2?user_id=" + user_id + "");
            }
        }
        else if (dteu.Rows.Count <= 0)
        {
            lbl_msg.Text = "Incorrect login account";
        }
        else if (dteu.Rows[0]["allowAccess"].ToString() == "True")
        {
            if (txt_user.Text == "CLI436" || dteu.Rows[0]["statusid"].ToString() != "4" && dteu.Rows[0]["statusid"].ToString() != "5" && dteu.Rows[0]["statusid"].ToString() != "6")
            {
                string x = bol.procedures(sc, "auth");

                if (x == "0")
                    lbl_msg.Text = "Incorrect login account";
                else
                {
                    DataTable dt = getdata.access(x);
                    string oi = dt.Rows[0]["url"].ToString();
                    Session["user_id"] = x;
                    Session["ngalan"] = dt.Rows[0]["name"].ToString();
                    Session["role"] = dt.Rows[0]["acc_id"].ToString();
                    Session["emp_id"] = dt.Rows[0]["emp_id"].ToString();
                    Session["profile"] = dt.Rows[0]["profile"].ToString();
                    Session["department"] = dt.Rows[0]["departmentid"].ToString();

                    string aniv = "select case when (month(DateHired)=MONTH(GETDATE()) and DAY(DateHired)=DAY(getdate()))" +
                        "then 'True' else 'False' end aniversary,datediff(YEAR,DateHired,GETDATE())yearage,case when " +
                        "datediff(YEAR,DateHired,GETDATE()) = 1 then 'year' else 'years' end yearname,* from memployee" +
                        " where id=" + dt.Rows[0]["emp_id"].ToString() + " and(month(DateHired)=MONTH(GETDATE()) and" +
                        " DAY(DateHired)=DAY(getdate()))";

                    DataTable dtaniv = dbhelper.getdata(aniv);

                    string query = "select " +
                    "case when (month(DateHired)=MONTH(GETDATE()) and DAY(DateHired)=DAY(getdate())) then 'True' else 'False' end ifanniversary, " +
                    "case when (month(DateOfBirth)=MONTH(GETDATE()) and DAY(DateOfBirth)=DAY(getdate())) then 'True' else 'False' end ifbirthday, " +
                    "* from memployee where id=" + dt.Rows[0]["emp_id"].ToString() + " and " +
                    "( " +
                    "(month(DateOfBirth)=MONTH(GETDATE()) and DAY(DateOfBirth)=DAY(getdate())) " +
                    "or " +
                    "(month(DateHired)=MONTH(GETDATE()) and DAY(DateHired)=DAY(getdate())))";
                    DataTable dtempbd = dbhelper.getdata(query);

                    if (dtaniv.Rows.Count > 0)
                    {
                        Response.Redirect("anniversary?aniv=" + dtaniv.Rows[0]["aniversary"].ToString() + "&name=" + dtaniv.Rows[0]["FirstName"].ToString() + "&a=" + dtaniv.Rows[0]["yearage"].ToString() + "&b=" + dtaniv.Rows[0]["yearname"].ToString() + "");
                    }

                    else if (dtempbd.Rows.Count > 0)
                        Response.Redirect("bd?bd=" + dtempbd.Rows[0]["ifbirthday"].ToString() + "&ann=" + dtempbd.Rows[0]["ifanniversary"].ToString() + "");
                    else
                    {
                        DataTable user = dbhelper.getdata("select * from nobel_user where id=" + x);
                        if (user.Rows[0]["emp_id"].ToString() != "0" && user.Rows[0]["acc_id"].ToString() == "1")
                        {
                            Session["role"] = "Admin";
                            Response.Redirect("homepage?user_id=" + function.Encrypt(Session["user_id"].ToString(), true) + "");
                        }
                        else
                            Response.Redirect("homepage2?");
                    }
                }
            }
            else
                Response.Write("<script>alert('Thank you for serving CebuLandMasters Inc.!')</script>");
        }
        else
            Response.Write("<script>alert('Thank you for serving CebuLandMasters Inc.!')</script>");
    }
    protected void click_convert(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?test.aspx'", true);
       
    }
   
}