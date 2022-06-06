using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

public partial class content_MasterPageNew : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            logout();
        
        if (!IsPostBack)
        {
            CheckBio();
            key.Value = Session["user_id"].ToString();
            disp();
            //pobre();
            //haha();
        }
    }
    protected void haha()
    {
        DataTable dtemps = dbhelper.getdata("select Id from MEmployee");
        foreach (DataRow dr in dtemps.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from leave_credits where leaveid = 18 and empid= " + dr["Id"] + "");
            if (dt.Rows.Count < 1)
            {
                dbhelper.getdata("insert into leave_credits values('2020-02-08 18:05:18.890','0','" + dr["Id"] + "','18','200','no','Yearly',NULL,'2020','2')");
            }
        }
    }

    protected void pobre()
    {
        DataTable ron = dbhelper.getdata("select a.Id, b.Position, left(convert(varchar,a.DateHired,101),10)DateHired,"
            + " a.LastName+', '+a.FirstName+' '+a.MiddleName as e_name, datediff(day,a.DateHired,GETDATE())daterange, "
            + "case when datediff(MONTH,a.DateHired,GETDATE()) = 2 and DATEDIFF(DAY,a.DateHired,GETDATE()) = 74 then"
            + " 'Reached 2nd Month and 15th Days' else '-' end eventss from MEmployee a left join MPosition b on b.Id = a.PositionId"
            + " where case when datediff(MONTH,a.DateHired,GETDATE()) = 2 and DATEDIFF(DAY,a.DateHired,GETDATE()) = 74 then "
            + "'Reached 2nd Month and 15th Days' else '-' end <> '-' and a.PositionId = '1' or a.PositionId = '2'"
            + " order by DATEDIFF(DAY,a.DateHired,GETDATE())");
        if (ron.Rows.Count != 0)
        {
            foreach (DataRow dar in ron.Rows)
            {
                DataTable datadar = dbhelper.getdata("select * from sys_notification where content like '%" + dar["e_name"] + "%'");
                if (datadar.Rows.Count == 0)
                {
                    string direct = "addemployee?user_id=TIdS9+05Aas=&app_id=" + dar["Id"] + "&tp=ed";
                    if (ron.Rows[0]["eventss"].ToString() != "-")
                    {
                        dbhelper.getdata("insert into sys_notification values (GETDATE(),'FTE', '0', '1', '" + direct + "', '0', '0', '" + dar["e_name"].ToString() + " " + dar["eventss"].ToString() + " Since Start Working." + "')");
                    }
                }
            }
        }

        DataTable dt = dbhelper.getdata("select a.Id, b.Department, left(convert(varchar,a.DateHired,101),10)DateHired,"
            + "a.LastName+', '+a.FirstName+' '+a.MiddleName as e_name, datediff(day,a.DateHired,GETDATE()), case when "
            + "datediff(day,a.DateHired,GETDATE())= 21 then '3rd Week' when datediff(MONTH,a.DateHired,GETDATE()) = 3 and "
            + "DAY(a.DateHired)=DAY(GETDATE()) then '3rd Month' when datediff(MONTH,a.DateHired,GETDATE()) = 6  then '5th Month' "
            + "else '-'end eventss from MEmployee a left join MDepartment b on b.Id = a.DepartmentId where case when "
            + "datediff(day,a.DateHired,GETDATE())= 21 then '3rd Week' when datediff(MONTH,a.DateHired,GETDATE()) = 3 and"
            + " DAY(a.DateHired)=DAY(GETDATE()) then '3rd Month' when datediff(MONTH,a.DateHired,GETDATE()) = 6  then '5th Month'"
            + " else '-'end <> '-' order by datediff(day,a.DateHired,GETDATE())");
        if (dt.Rows.Count != 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                DataTable data = dbhelper.getdata("select * from sys_notification where content like '%" + dr["e_name"] + "%'");
                if (data.Rows.Count == 0)
                {
                    string direct = "addemployee?user_id=TIdS9+05Aas=&app_id=" + dr["Id"] + "&tp=ed";
                    dbhelper.getdata("insert into sys_notification values (GETDATE(),'Provisionary Contract', '0', '1', '" + direct + "', '0', '0', '" + dr["e_name"].ToString() + " is in " + dr["eventss"].ToString() + " Since Start Working." + "')");
                }
            }
        }
        DataTable dtceleb = dbhelper.getdata("select a.Id, d.department, replace(LEFT(CONVERT(varchar,a.DateHired,101),10),year(a.DateHired),"
            + "YEAR(getdate()))eventdate, a.lastname+', '+a.firstname+' '+a.middlename e_name,case when day(a.DateHired)= day(GETDATE())"
            + "then 'Happy Anniversary' else 'Upcoming Anniversary' end eventss from memployee a left join mdepartment d on "
            + "a.departmentid=d.id where datediff(month,a.DateHired,GETDATE())=12  and month(a.DateHired)=MONTH(GETDATE()) and"
            + " day(a.DateHired)>=day(GETDATE())and a.payrollgroupid<>4 union select a.Id, d.department, replace(LEFT(CONVERT(varchar,a.DateOfBirth,101),10)"
            + ",YEAR(a.DateOfBirth),YEAR(getdate()))eventdate, a.lastname+', '+a.firstname+' '+a.middlename e_name,case when "
            + "day(a.DateOfBirth)= day(GETDATE()) then 'Happy Birthday' else 'Upcoming Birthday' end eventss from memployee a "
            + "left join mdepartment d on a.departmentid=d.id where month(a.DateOfBirth)=MONTH(GETDATE()) and day(a.DateOfBirth)>=day(GETDATE())"
            + " and a.payrollgroupid <> 4 order by eventss,eventdate desc");
        if (dtceleb.Rows.Count != 0)
        {
            foreach (DataRow dr in dtceleb.Rows)
            {
                string direct = "addemployee?user_id=TIdS9+05Aas=&app_id=" + dr["Id"] + "&tp=ed";
                if (dr["eventss"].ToString() == "Happy Birthday")
                {
                    DataTable dtt = dbhelper.getdata(" select * from sys_notification where content like '%" + dr["e_name"] + "%'");
                    if (dtt.Rows.Count == 0)
                    {
                        dbhelper.getdata("insert into sys_notification values (GETDATE(),'Birthday Celebrant', '0', '1', '" + direct + "', '0', '0', '" + dr["e_name"].ToString() + "')");
                    }
                }
                else if (dr["eventss"].ToString() == "Happy Anniversary")
                {
                    DataTable dtt = dbhelper.getdata(" select * from sys_notification where content like '%" + dr["e_name"] + "%'");
                    if (dtt.Rows.Count == 0)
                    {
                        dbhelper.getdata("insert into sys_notification values (GETDATE(),'Anniversary Celebrant', '0', '1', '" + direct + "', '0', '0', '" + dr["e_name"].ToString() + "')");
                    }
                }
            }
        }
    }


    protected void CheckBio()
    {
        Ping ping = new Ping();
        PingReply pingReply = ping.Send("192.168.36.35");
        string status = pingReply.Status == IPStatus.Success ? "Online" : "Disconnected";
        pnl_notify.Visible = status == "Disconnected" ? true : false;
        if (status == "Disconnected")
        {
            dbhelper.getdata("insert into sys_RemoteDTR values('1',GETDATE(),'-','" + status + "')");
        }
    }

    protected void logout()
    {
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("login");
    }

    protected void click_out(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }

    protected void disp()
    {
        avatar.Value = File.Exists(Server.MapPath("~/files/profile/" + Session["user_id"].ToString() + ".png")) ? Session["user_id"].ToString() : "0";

        string query = "select b.formname,b.remarks, b.icon, b.cnt, b.formtype from MUserForm a " +
        "left join SysForm b on a.FormId=b.Id " +
        "where a.UserId='" + key.Value + "' and a.CanAllow='True' " +
        "union ALL " +
        "select formname,remarks,icon,cnt,formtype FROM SysForm where standard<>'EMPLOYEE' or standard is null ";
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
    protected void close(object sender, EventArgs e)
    {
        panelOverlay.Visible = false;
        panelchangeprofile.Visible = false;

    }
    protected void clickp(object sender, EventArgs e)
    {
        Response.Redirect("profile");
        DataTable dt = dbhelper.getdata("select * from file_details where status='Active' and empid=" + Session["user_id"].ToString() + " and classid=1");
        if (dt.Rows.Count > 0)
        {
            lbl_sign.Text = "Change Profile";
            btn_savep.Text = "Update";
            profid.Value = dt.Rows[0]["id"].ToString();
        }
        else
        {
            lbl_sign.Text = "Add Profile";
            btn_savep.Text = "Add";
            profid.Value = "0";
        }


        panelOverlay.Visible = true;
        panelchangeprofile.Visible = true;
    }
    protected void clicksavep(object sender, EventArgs e)
    {
        string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        string contenttype = FileUpload1.PostedFile.ContentType;
        if (contenttype.Contains("image"))
        {
            if (btn_savep.Text == "Update")
                dbhelper.getdata("update file_details set status='cancel' where id=" + profid.Value + "");

            DataTable dt = dbhelper.getdata("insert into file_details(date,userid,empid,classid,location,filename,description,status,contenttype) " +
                                   "values (getdate()," + Session["user_id"].ToString() + "," + key.Value + ",1,'files/profile','" + fileExtension + "','profile','Active','" + contenttype + "') select scope_identity() id");
            string input = Server.MapPath("~/files/profile/") + dt.Rows[0]["id"].ToString() + fileExtension;
            FileUpload1.SaveAs(input);
            panelOverlay.Visible = false;
            panelchangeprofile.Visible = false;
        }
        else
            Response.Write("<script>alert('Incorrect File Type!')</script>");
    }
}
