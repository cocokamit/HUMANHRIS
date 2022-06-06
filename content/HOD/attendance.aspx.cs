using Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.IO;

public partial class content_HOD_attendance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            Loadable();
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    private void ExportGridToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = ddl_period.SelectedItem + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        gv_schedule.GridLines = GridLines.Both;
        gv_schedule.HeaderStyle.Font.Bold = true;
        gv_schedule.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    [WebMethod]
    public static Attlogs[] attlogs(string id, string date)
    {
        List<Attlogs> attlogs = new List<Attlogs>();

        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = "SELECT a.biotime " +
            "FROM tdtrperpayrolperline a " +
            "left join memployee b on a.empid=b.id " +
            "where convert(varchar,a.biotime, 101)  =  convert(datetime,'" + date + "') and b.id=" + id + " order by a.biotime";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Attlogs log = new Attlogs();
                    log.date = reader["biotime"].ToString();
                    attlogs.Add(log);

                }
            }
            con.Close();
        }

        return attlogs.ToArray();
    }

    protected void Loadable()
    {
        string query = "select id ,left(convert(varchar,dfrom,101),10)+' - '+left(convert(varchar,dtoo,101),10) dtrrange from payroll_range where action is null order by dfrom desc";
        DataTable dt = dbhelper.getdata(query);
        foreach (DataRow dr in dt.Rows)
        {
            ddl_period.Items.Add(new ListItem(dr["dtrrange"].ToString(), dr["id"].ToString()));
        }

        query = "select * from msection where dept_id=" + Session["department"] + " order by seccode";
        dt = dbhelper.getdata(query);
        ddl_section.Items.Add(new ListItem("All","0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
        }

        ddl_section.SelectedIndex = 1;
        ddl_section.Visible = dt.Rows.Count == 1 ? false : true;

        Schedule();

    }

    protected void OnChangeFilter(object sender, EventArgs e)
    {
        Schedule();
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
    }

    public class Attlogs
    {
        public string date { get; set; }
    }

    protected void Schedule()
    {
        try
        {
            gv_schedule.Visible = true;
            alert.Visible = false;
            string[] period = ddl_period.SelectedItem.ToString().Split('-');
            DataTable dtr = Core.DTRF(period[0], period[1], "attlogs_" + Session["department"] + ",section_" + ddl_section.SelectedValue);
                
            string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , c.ShiftCode, b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId,a.id as tid, d.IdNumber as ID, b.status " +
            "from TChangeShift a " +
            "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
            "left join MShiftCode c on b.ShiftCodeId=c.id " +
            "left join memployee d on b.EmployeeId=d.Id " +
            "left join Approver e on  d.Id= e.emp_id " +
            "where " +
            "CONVERT(date,b.date) between CONVERT(date,'" + period[0] + "') and CONVERT(date,'" + period[1] + "')" +
            "and d.departmentid = " + Session["department"] + " " +
            "order by b.[date]";

            DataTable hh = dbhelper.getdata(tt);
            ViewState["ttt"] = hh;

            //DEPARTMENT
            string section = ddl_section.SelectedValue == "0" ? "" : "and sectionid=" + ddl_section.SelectedValue;
            string query = "select id idd, IdNumber ID,Firstname +' '+LastName Employees, PayrollGroupId " +
            "from MEmployee " +
            "where PayrollGroupId<>" + Config.inactive() + " and departmentid = " + Session["department"] + " " + section + " order by level,Firstname +' '+LastName";

            DataTable dt = dbhelper.getdata(query);
            ViewState["data"] = dt;

            string awts = "create table #dummy(ddate varchar(50), mdate varchar(50)) " +
            "declare @start datetime declare @end datetime  " +
            "select @start=dfrom,@end=dtoo from payroll_range where ID='" + ddl_period.SelectedValue + "'  " +
            "insert into #dummy(ddate,mdate) values (left(convert(varchar,@start,101),10),DATENAME(weekday,  @start))  " +
            "while(@start<@end) begin set @start=DATEADD(DAY,1,@start)  " +
            "insert into #dummy(ddate,mdate)values(left(convert(varchar,@start,101),10),DATENAME(weekday,  @start)) end  " +
            "select * from #dummy  " +
            "drop table #dummy";

            DataTable ddt = dbhelper.getdata(awts);
            ViewState["range"] = ddt;

            for (int aa = 0; aa < ddt.Rows.Count; aa++)
            {
                string[] x = ddt.Rows[aa]["ddate"].ToString().Split('/');
                dt.Columns.Add(new DataColumn(x[1] + " " + ddt.Rows[aa]["mdate"].ToString().Substring(0, 3), typeof(string)));
            }

            gv_schedule.DataSource = dt;
            gv_schedule.DataBind();

            ViewState["data"] = dt;



            DataTable leaves = getdata.Leave("b.departmentid=" +Session["department"].ToString());
            DataTable holidays = getdata.Holiday(period[0], period[1]);

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 4; col < dt.Columns.Count; col++)
                {
                    if (ddt.Rows[col - 4][0].ToString() == "08/19/2019")
                    {
                    }

                    DataRow[] dr = hh.Select("employeeID='" + dt.Rows[row][0].ToString() + "' and date='" + ddt.Rows[col - 4][0].ToString() + "'");

                    // && dr[0]["status"].ToString() == "approved"
                    if (dr.Count() > 0)
                    {
                        int[] now = ddt.Rows[col - 4][0].ToString().Split('/').Select(Int32.Parse).ToArray();
                        DataRow[] result = dtr.Select("empid = '" + dt.Rows[row][0].ToString() + "' and date like '%" + string.Join("/", now) + "%'");
                        DataRow[] leave = leaves.Select("EmployeeId = '" + dt.Rows[row][0].ToString() + "' and date='" + ddt.Rows[col - 4][0].ToString() + "'");
                        DataRow[] holiday = holidays.Select("date='" + ddt.Rows[col - 4][0].ToString() + "'");

                        gv_schedule.Rows[row].Cells[col].Text = dr[0]["shiftcode"].ToString();
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("data-toggle", "modal");
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("data-target", "#modal-default");
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("data-emp", dt.Rows[row][0].ToString());
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("data-date", ddt.Rows[col - 4][0].ToString());

                        if (leave.Count() > 0)
                        {
                            gv_schedule.Rows[row].Cells[col].Text = leave[0]["LeaveType"].ToString();
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "leave");
                        }

                        if (holiday.Count() > 0 & result[0]["timein1"].ToString() == "--")
                        {
                            string shiftcode = dr[0]["shiftcode"].ToString();

                            if (dr[0]["shiftcode"].ToString() == "PH")
                                gv_schedule.Rows[row].Cells[col].Text = "PH";

                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "holiday");
                        }

                        if (result.Count() > 0 && leave.Count() == 0)
                        {
                            DataTable fitchDTR = result.CopyToDataTable();
                            float reg_hr = float.Parse(fitchDTR.Rows[0]["reg_hr"].ToString());
                            float night = float.Parse(fitchDTR.Rows[0]["night"].ToString());
                            float offsethrs = float.Parse(fitchDTR.Rows[0]["offsethrs"].ToString());
                            float totalhrs = reg_hr + night + offsethrs;
                            float fixhour = float.Parse(fitchDTR.Rows[0]["FixNumberOfHours"].ToString());

                            if (totalhrs == 0)
                            {
                                gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "danger");

                                if (dr[0]["shiftcode"].ToString() == "DO")
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "restday");

                                if (result[0]["timein1"].ToString() != "--")
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "orange");

                                if (holiday.Count() > 0)
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "holiday");
                            }
                            else
                            {
                                if (totalhrs < fixhour)
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "orange");

                                if (dr[0]["shiftcode"].ToString() == "DO")
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "restday");
                            }
                        }

                        if (dr[0]["shiftcode"].ToString() == "DO")
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "restday");

                        if (dr[0]["shiftcode"].ToString() == "OIL")
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "oil");

                        if (dr[0]["shiftcode"].ToString() == "OIM")
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "oim");

                        if (dr[0]["shiftcode"].ToString() == "OPH")
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "oph");

                    }
                    else
                        gv_schedule.Rows[row].Cells[col].Text = "-";

                }
            }

            alert.Visible = gv_schedule.Rows.Count == 0 ? true : false;
        }
        catch
        {
            gv_schedule.Visible = false;
            alert.Visible = true;
        }
    }
}