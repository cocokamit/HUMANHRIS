using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

public partial class content_GM_attsummary : System.Web.UI.Page
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
        gv_schedule.Visible = true;
        alert.Visible = false;
        string[] period = ddl_period.SelectedItem.ToString().Split('-');

        string tt = "where CONVERT(date,b.date) between CONVERT(date,'" + period[0] + "') and CONVERT(date,'" + period[1] + "') " +
        "and g.level like '%hod%' " +
        "order by b.[date]";
        DataTable hh = getdata.schedule(tt);
        ViewState["ttt"] = hh;

        //HOD & AHOD
        string query = "select a.id idd, IdNumber ID,Firstname +' '+LastName Employees, PayrollGroupId, " +
        "case when c.Department =  d.seccode " +
        "then c.Department  " +
        "else c.Department +' ' + d.seccode end Department " +
        "from MEmployee a " +
        "left join MLevel b on a.level=b.Id " +
        "left join MDepartment c on a.DepartmentId= c.Id  " +
        "left join msection d on a.sectionid = d.sectionid " +
        "where a.PayrollGroupId<>6 and b.level like '%hod%' order by  c.Department,  Firstname +' '+LastName";

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

        DataTable leaves = getdata.Leave("b.level like '%hod%'");
        DataTable holidays = getdata.Holiday(period[0], period[1]);

        int crtl = 5;
        for (int row = 0; row < dt.Rows.Count; row++)
        {
            for (int col = crtl; col < dt.Columns.Count; col++)
            {
                if (ddt.Rows[col - crtl][0].ToString() == "08/06/2019" && dt.Rows[row][0].ToString() == "56")
                {

                }

                DataRow[] dr = hh.Select("employeeID='" + dt.Rows[row][0].ToString() + "' and date='" + ddt.Rows[col - crtl][0].ToString() + "'");

                // && dr[0]["status"].ToString() == "approved"
                if (dr.Count() > 0)
                {
                    int[] now = ddt.Rows[col - crtl][0].ToString().Split('/').Select(Int32.Parse).ToArray();
                    DataRow[] leave = leaves.Select("EmployeeId = '" + dt.Rows[row][0].ToString() + "' and date='" + ddt.Rows[col - crtl][0].ToString() + "'");
                    DataRow[] holiday = holidays.Select("date='" + ddt.Rows[col - crtl][0].ToString() + "'");

                    string shift = dr[0]["shiftcode"].ToString();
                    gv_schedule.Rows[row].Cells[col].Text = shift;
                    gv_schedule.Rows[row].Cells[col].Attributes.Add("data-toggle", "modal");
                    gv_schedule.Rows[row].Cells[col].Attributes.Add("data-target", "#modal-default");
                    gv_schedule.Rows[row].Cells[col].Attributes.Add("data-emp", dt.Rows[row][0].ToString());
                    gv_schedule.Rows[row].Cells[col].Attributes.Add("data-date", ddt.Rows[col - crtl][0].ToString());

                    if (leave.Count() > 0)
                    {
                        gv_schedule.Rows[row].Cells[col].Text = leave[0]["LeaveType"].ToString();
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "leave");
                    }

                    if (holiday.Count() > 0)
                    {
                        gv_schedule.Rows[row].Cells[col].Text = shift == "PH" ? "PH" : shift;
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "holiday");
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
}