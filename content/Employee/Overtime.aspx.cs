using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_Overtime : System.Web.UI.Page
{
    public static string user_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
    }
    protected void addOT(object sender, EventArgs e)
    {
        if (checker())
        {

            DataTable checkleave = dbhelper.getdata("select * from TLeaveApplicationLine where Date = '" + txt_date.Text + "' and EmployeeId = '" + Session["emp_id"] + "' and status not like '%Cancel%'");
            if (checkleave.Rows.Count == 0)
            {
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");

                string a = DateTime.Now.ToString("MM/dd/yyyy");
                string query = "select top 1 *,CONVERT(varchar,[Date],101) as datee from TChangeShiftLine where EmployeeId='" + Session["emp_id"].ToString() + "' order by id desc ";
                DataTable wtf = new DataTable();
                wtf = dbhelper.getdata(query);


                if (wtf.Rows.Count <= 0)
                {
                    query = "select a.ShiftCodeId, b.[Day],b.TimeOut2 from MEmployee a left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
                }
                else
                {
                    if (DateTime.Parse(wtf.Rows[0]["datee"].ToString()) < DateTime.Parse(txt_date.Text))
                    {
                        query = "select top 1 a.ShiftCodeId,b.[Day],b.TimeOut2 from TChangeShiftLine a left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId left join MEmployee c on a.EmployeeId=c.Id where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                        query += " order by a.Date desc ";
                    }
                    else
                    {
                        if (wtf.Rows.Count < 0)
                        {

                            query = "select a.ShiftCodeId,b.[Day],b.TimeOut2 from MEmployee a left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
                        }
                        else
                        {
                            query = "select top 1 a.ShiftCodeId,b.[Day],b.TimeOut2 from TChangeShiftLine a left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId left join MEmployee c on a.EmployeeId=c.Id where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                            query += " order by Date asc";
                        }
                    }
                }
                DataTable dt = dbhelper.getdata(query);

                //makuha iya time out

                DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
                DataTable daytype = dbhelper.getdata("select *, a.id,a.DayType,a.workingdays,a.RestdayDays,case when b.BranchId is null then '0' else b.BranchId end BranchId ,case when left(convert(varchar,b.Date,101),10) is null then '0' else left(convert(varchar,b.Date,101),10) end datte from MDayType a left join MDayTypeDay b on a.Id=b.DayTypeId where b.status is null order by a.id asc ");
                DataRow[] dtgetallDayType = daytype.Select("datte='" + txt_date.Text + "' and BranchId='" + dtemp.Rows[0]["branchid"].ToString() + "'");

                DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + txt_date.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification')");
                if (tdcheck.Rows.Count == 0)
                {
                    DateTime getshiftout = Convert.ToDateTime(txt_date.Text + " " + dt.Rows[0]["TimeOut2"].ToString());
                    DateTime getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text);
                    DateTime otin = Convert.ToDateTime(txt_date.Text + " " + txt_start.Text);

                    if (getshiftout > otin)
                        otin = getshiftout;
                    if (otin.ToString().Contains("PM") && getotout.ToString().Contains("AM"))
                        getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text).AddDays(1);
                    if (getotout > getshiftout)
                    {
                        query = "select CONVERT(float, datediff(minute, '" + otin + "','" + getotout.ToString() + "' )) / 60 as time_diff ";
                        DataTable dtt = new DataTable();
                        dtt = dbhelper.getdata(query);

                        string nighthrs = getnightdif.getnight(getshiftout.ToString(), getotout.ToString(), "dtr");
                        decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString()) - decimal.Parse(nighthrs);
                        decimal otdaymultiplier = 0;
                        decimal otnightmultiplier = 0;

                        string hdn_rd = "Restday";
                        if (hdn_rd == "Restday")
                        {
                            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTrestdaydays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTrestdaydays"].ToString());
                            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightRestdayDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightRestdayDays"].ToString());
                        }
                        else
                        {
                            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTworkingdays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTworkingdays"].ToString());
                            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(daytype.Rows[0]["OTNightWorkingDays"].ToString());
                        }

                        string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
                        string bb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

                        DataTable dtpreots = dbhelper.getdata("insert into TOverTimeLine([OverTimeId],[EmployeeId],[Date],[OvertimeHours],[OvertimeNightHours],[OvertimeLimitHours],[Remarks],[status],[ifdone],[dtr_id],[sysdate],[time_in],[time_out],[overtimehoursapp],[overtimenighthoursapp],[approver_id],regmultiplier,nightmultiplier,hourlyrate)values(NULL," + Session["emp_id"].ToString() + ",'" + txt_date.Text + "','" + totalreghours + "','" + nighthrs + "','0.00','" + txt_remarks.Text.Replace("'", "") + "','" + aa + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "','" + dtt.Rows[0]["time_diff"] + "','0.00','" + bb + "','" + otdaymultiplier + "','" + otnightmultiplier + "','" + decimal.Parse(dtemp.Rows[0]["hourlyrate"].ToString()) + "' ) select scope_identity() id");

                        function.AddNotification("Pre-Overtime Approval", "preot", approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_userid"].ToString(), dtpreots.Rows[0]["id"].ToString());
                        //error pa dere

                        function.Notification("ot", "alert", dtpreots.Rows[0]["id"].ToString(), bb);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_OT'", true);

                    }
                    else
                        Response.Write("<script>alert('Incorect Time Input!')</script>");

                }
                else
                    Response.Write("<script>alert('Input data is already existed!')</script>");

            }
            else
            {
                Response.Write("<script>alert('You are scheduled for a Leave!')</script>");
            }
        }
    }

    protected bool checker()
    {
        bool oi = true;

        if (txt_start.Text == "")
        {
            oi = false;
        }
        else
            lbl_start.Text = "";

        if (txt_end.Text == "")
        {
            oi = false;
        }
        else
            lbl_end.Text = "";


        if (txt_remarks.Text == "")
        {
            oi = false;
        }
        else
            lbl_remarks.Text = "";

        if (txt_date.Text.Length < 10)
        {
            oi = false;
        }
        else
            lbl_date.Text = "";

        return oi;
    }
}