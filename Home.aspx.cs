using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Data.SqlClient;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");


        hf_empid.Value = Session["emp_id"].ToString();
        if (!Page.IsPostBack)
        {

        }
        loader();
       
    }

    protected void loader()
    {
        DataTable dt = dbhelper.getdata("Select * from HomeUpload where type='banner'");

        int counter = 1;

        if (dt.Rows.Count > 0)
        {

            homebanner.ImageUrl = "~/files/HomeImages/" + dt.Rows[0]["imageurl"].ToString();
        }

        dt = dbhelper.getdata("Select * from HomeUpload where type!='banner' order by id desc");

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    HtmlGenericControl imager = new HtmlGenericControl("img");
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("href", rows["url"].ToString());
                    anchor.Attributes.Add("target", "_blank");
                    anchor.Attributes.Add("style", "curser:pointer");

                    div1.Attributes.Add("class", "columnr");
                    div1.Attributes.Add("style", " -ms-flex: 50%; flex: 50%; max-width: 50%;");
                    imager.Attributes.Add("style", "width:100%;");
                    imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                    anchor.Controls.Add(imager);
                    div1.Controls.Add(anchor);

                    divannouncement.Controls.Add(div1);
                }
            }
            else if (dt.Rows.Count == 2)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    HtmlGenericControl imager = new HtmlGenericControl("img");

                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("href", rows["url"].ToString());
                    anchor.Attributes.Add("target", "_blank");
                    anchor.Attributes.Add("style", "curser:pointer");

                    div1.Attributes.Add("class", "columnr");
                    div1.Attributes.Add("style", " -ms-flex: 50%; flex: 50%; max-width: 50%;");
                    imager.Attributes.Add("style", "width:100%;");
                    imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                    anchor.Controls.Add(imager);
                    div1.Controls.Add(anchor);

                    divannouncement.Controls.Add(div1);
                }
            }
            else
            {
                HtmlGenericControl div1 = new HtmlGenericControl("div");
                HtmlGenericControl div2 = new HtmlGenericControl("div");
                HtmlGenericControl div3 = new HtmlGenericControl("div");

                div1.Attributes.Add("class", "columnr");
                div2.Attributes.Add("class", "columnr");
                div3.Attributes.Add("class", "columnr");

                foreach (DataRow rows in dt.Rows)
                {
                    HtmlGenericControl imager = new HtmlGenericControl("img");
                    HtmlGenericControl anchor = new HtmlGenericControl("a");

                    anchor.Attributes.Add("target", "_blank");
                    anchor.Attributes.Add("style", "curser:pointer;");
                    if (counter == 1)
                    {
                        anchor.Attributes.Add("href", rows["url"].ToString());

                        imager.Attributes.Add("style", "width:100%;");
                        imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                        anchor.Controls.Add(imager);

                        div1.Controls.Add(anchor);
                        counter = 2;
                    }
                    else if (counter == 2)
                    {
                        anchor.Attributes.Add("href", rows["url"].ToString());

                        imager.Attributes.Add("style", "width:100%;");
                        imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                        anchor.Controls.Add(imager);

                        div2.Controls.Add(anchor);
                        counter = 3;
                    }
                    else if (counter == 3)
                    {
                        anchor.Attributes.Add("href", rows["url"].ToString());

                        imager.Attributes.Add("style", "width:100%;");
                        imager.Attributes.Add("src", "files/HomeImages/" + rows["imageurl"].ToString());

                        anchor.Controls.Add(imager);

                        div3.Controls.Add(anchor);
                        counter = 1;
                    }
                }

                divannouncement.Controls.Add(div1);
                divannouncement.Controls.Add(div2);
                divannouncement.Controls.Add(div3);
            }

        }
    }
    protected void click_close(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static void getLeaveUpdate(string id)
    {
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            string datetime = DateTime.Now.ToString();
            string empid = id;
            string query = " Select DateHired,case when DATEDIFF(Year, DateHired, GETDATE()) !=0 then DATEDIFF(Year, DateHired, GETDATE()) else (case when DATEDIFF(MONTH, DateHired, GETDATE())>=6 then 0.5 else 0 end) end empspan from MEmployee where Id=" + id + "";

            using (SqlCommand clm = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = clm.ExecuteReader();
                if (reader.Read())
                {
                    DataTable dttt = dbhelper.getdata("Select * from SetUpTable");

                    if (dttt.Rows.Count > 0)
                    {
                        if (dttt.Rows[0]["LeaveType"].ToString() == "5")
                        {
                            List<DataRow> tenure = dbhelper.getdata("Select * from TenureValue").AsEnumerable().ToList();

                            string nani = "";
                            DataTable dtt = dbhelper.getdata("Select * from leave_credits where empid=" + id + " and action is NULL and (leaveid=(Select Id from MLeave where LeaveType='BL' and [action] is null) or leaveid=(Select Id from MLeave where LeaveType='SL' and [action] is null) or leaveid=(Select Id from MLeave where LeaveType='VL' and [action] is null)) and mark=1");
                            if (dtt.Rows.Count > 0)
                            {
                                for (int i = 0; i < tenure.Count(); i++)
                                {
                                    if (Convert.ToDouble(reader["empspan"].ToString()) >= Convert.ToDouble(tenure[i]["YearofService"].ToString()))
                                    {
                                        nani = " Update leave_credits set credit=" + Convert.ToDouble(tenure[i]["SickLeave"].ToString()) + ",yyyyear=year(GETDATE()) where leaveid=(Select Id from MLeave where LeaveType='SL' and [action] is null) and mark=1 and empid=" + id + ""
                                        + " Update leave_credits set credit=" + Convert.ToDouble(tenure[i]["VacationLeave"].ToString()) + ",yyyyear=year(GETDATE()) where leaveid=(Select Id from MLeave where LeaveType='VL' and [action] is null) and mark=1 and empid=" + id + "";
                                    }
                                }
                            }
                            else
                            {

                                for (int i = 0; i < tenure.Count(); i++)
                                {
                                    if (Convert.ToDouble(reader["empspan"].ToString()) >= Convert.ToDouble(tenure[i]["YearofService"].ToString()))
                                    {
                                        nani = " insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + id + ",1,(Select Id from MLeave where LeaveType='BL' and [action] is null),1,'yes','Yearly',year(GETDATE()),1)"
                                              + " insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + id + ",1,(Select Id from MLeave where LeaveType='PSL' and [action] is null)," + Convert.ToDouble(tenure[i]["SickLeave"].ToString()) + ",'no','Yearly',year(GETDATE()),1)"
                                              + " insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + id + ",1,(Select Id from MLeave where LeaveType='PVL' and [action] is null)," + Convert.ToDouble(tenure[i]["VacationLeave"].ToString()) + ",'yes','Yearly',year(GETDATE()),1)";
                                    }
                                }
                            }

                            if (nani != "")
                            {
                                dbhelper.getdata(nani);
                            }
                        }
                        else if (dttt.Rows[0]["LeaveType"].ToString() == "4")
                        {
                            string additionals = "";
                            string[] lma = dttt.Rows[0]["LeaveMonthlyAdd"].ToString().Split(',');
                            for (int i = 0; i < lma.Count() - 1; i++)
                            {
                                additionals += "leaveid=(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null) or ";
                                i += 2;
                            }

                            string nani = "";
                           
                            string dfrom = Convert.ToDateTime(Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("2000-MM-dd")) > Convert.ToDateTime(DateTime.Now.ToString("2000-MM-dd")) ? DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("MM-dd") : DateTime.Now.ToString("yyyy") + "-" + Convert.ToDateTime(dttt.Rows[0]["LeaveResetDate"].ToString()).ToString("MM-dd");
                            string dto = Convert.ToDateTime(dfrom).AddYears(1).ToString("yyyy-MM-dd");

                            DateTime from = Convert.ToDateTime(dfrom);
                            DateTime to = Convert.ToDateTime(dto);

                            string[] diff = Enumerable.Range(0, Int32.MaxValue)
                                     .Select(e => from.AddMonths(e))
                                     .TakeWhile(e => e <= to)
                                     .Select(e => e.ToString("MMMM-yyyy")).ToArray();

                            for (int i = 0; i < lma.Count() - 1; i++)
                            {
                                for (int j = 0; j < diff.Count(); j++)
                                {
                                    if (Convert.ToDateTime(diff[j]).Month == Convert.ToDateTime(lma[i]).Month && DateTime.Now >= Convert.ToDateTime(lma[i] + " " + Convert.ToDateTime(diff[j]).ToString("yyyy")))
                                    {
                                        DataTable dtt = dbhelper.getdata("Select a.*,(Select LeaveType from Mleave where id=a.leaveid) acrl from leave_credits a where empid=" + id + " and action is NULL and (" + additionals.Substring(0, additionals.Length - 3) + ") and mark=2");

                                        if (dtt.Select("acrl='" + lma[i + 2] + "'").Count() > 0)
                                        {
                                            nani += " Update leave_credits set credit=" + lma[i + 1] + ",yyyyear=" + from.Year.ToString() + " where leaveid=(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null) and mark=2 and empid=" + id + "";
                                        }
                                        else
                                        {
                                            dbhelper.getdata(" insert into leave_credits (sysdate,empid,userid,leaveid,credit,convertocash,renew,yyyyear,mark)values(getdate()," + id + ",1,(Select Id from MLeave where LeaveType='" + lma[i + 2] + "' and [action] is null)," + lma[i + 1] + ",'no','Yearly'," + from.Year.ToString() + ",2)");
                                        }
                                    }
                                }

                                i += 2;
                            }


                            if (nani != "")
                            {
                                dbhelper.getdata(nani);
                            }

                        }
                    }
                }
                con.Close();
            }
        }
    }


    [WebMethod]
    public static string deviceinformer(string emp)
    {
        string info = "";

        DataTable dt = dbhelper.getdata("Select * from nobel_user where emp_id=" + emp);
        HttpBrowserCapabilities capability = HttpContext.Current.Request.Browser;
        string BrowserName = capability.Browser.ToString();
        string version = capability.Version.ToString();
        string platform = capability.Platform.ToString();

        string devices = "";

        if (capability.IsMobileDevice)
        {
            devices = "Mobile";
        }
        else
        {
            devices = "Laptop/Desktop";
        }

        info += BrowserName + "~";
        info += version + "~";
        info += platform + "~";
        info += devices + "~";
        info += "cladvance" + "~";
        info += dt.Rows[0]["password"].ToString() + "~";
        //if (password == dt.Rows[0]["password"].ToString())
        //{

        //}
        //else
        //{
        //    info = "Incorrect Password.";
        //}

        return info;
    }


    [WebMethod]
    public static void savelogger(string emp, string time, string browser, string device, string latitude, string longitude,string city)
    {
        DataTable dt = dbhelper.getdata("Select * from Memployee where Id=" + emp);
        if (dt.Rows.Count > 0)
        {
            DataTable dtt = dbhelper.getdata("Insert into tdtrperpayrolperline(dtrperpayrol_id,empid,idnumber,biotime) values(0,'" + emp + "','" + dt.Rows[0]["IdNumber"] + "','" + time + "') select scope_identity() id");
            dbhelper.getdata("Insert into tdtrperpayrol_deviceinfo values('" + browser + "','" + device + "','" + latitude + "','" + longitude + "','" + time + "'," + dtt.Rows[0]["id"].ToString() + ",'"+city+"')");
        }

    }

    [WebMethod]
    public static daters[] getlogs(string emp)
    {
        string dtime = DateTime.Now.ToShortDateString();

        string query = "SELECT a.biotime Date_Time " +
                 "FROM tdtrperpayrolperline a " +
                 "left join memployee b on a.empid=b.id " +
                 "where convert(varchar,a.biotime, 101)  =  convert(datetime,'" + dtime + "') and b.id=" + emp + " order by a.biotime";


        List<daters> Detail = new List<daters>();

        DataTable dt = dbhelper.getdata(query);

        foreach (DataRow dr in dt.Rows)
        {
            daters DataObj = new daters();

            DataObj.dates = dr["Date_Time"].ToString();

            Detail.Add(DataObj);
        }
        return Detail.ToArray();
    }

    public class daters
    {
        public string dates { get; set; }

    }

}