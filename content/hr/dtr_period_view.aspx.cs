using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_dtr_period_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        schedule();
    }

    protected void click_back(object sender, EventArgs e)
    {
        Response.Redirect("dtrp");
    }

    protected void schedule()
    {
        //read notification
        //need encription
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        //need encription
        string id = Request.QueryString["key"].ToString();


        string tt = "select (select LastName + ', ' + FirstName from memployee where id=a.entryuserid) scheduler, a.entryuserid, left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
         "c.shiftcode, " +
         "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
         "d.IdNumber as ID, c.shiftcode " +
         "from TChangeShift a " +
         "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
         "left join MShiftCode c on b.ShiftCodeId=c.id " +
         "left join memployee d on b.EmployeeId=d.Id " +
         "where a.PeriodId=" + id;

        DataTable hh = dbhelper.getdata(tt);
        //hf_schedulerid.Value = hh.Rows[0]["entryuserid"].ToString();
        //lb_scheduler.Text = hh.Rows[0]["scheduler"].ToString();

        DataTable dtr = getdata.sys_dtr("id=" + id);
        l_range.Text = dtr.Rows[0]["period"].ToString();
        //lb_created.Text = dtr.Rows[0]["date_input"].ToString();




        string query = "select b.IdNumber as ID,b.LastName+', '+b.FirstName as Employee,  b.id as idd, b.payrollgroupid " +
                       "from Approver a " +
                       "left join MEmployee b on a.emp_id=b.id " +
                       "where a.under_id='" + hh.Rows[0]["entryuserid"].ToString() + "'";
        DataTable dt = dbhelper.getdata(query);
        ViewState["data"] = dt;

        string awts = "create table #dummy(ddate varchar(50), mdate varchar(50)) " +
        "declare @start datetime declare @end datetime  " +
        "select @start=date_start,@end=date_end from dtr_period  where ID='" + id + "'  " +
        "insert into #dummy(ddate,mdate) values (left(convert(varchar,@start,101),10),DATENAME(weekday,  @start))  " +
        "while(@start<@end) begin set @start=DATEADD(DAY,1,@start)  " +
        "insert into #dummy(ddate,mdate)values(left(convert(varchar,@start,101),10),DATENAME(weekday,  @start)) end  " +
        "select * from #dummy  " +
        "drop table #dummy";

        DataTable ddt = dbhelper.getdata(awts);
        ViewState["range"] = ddt;

        //int k=4;
        for (int aa = 0; aa < ddt.Rows.Count; aa++)
        {
            dt.Columns.Add(new DataColumn(ddt.Rows[aa]["ddate"].ToString() + " (" + ddt.Rows[aa]["mdate"].ToString() + ")", typeof(string)));
            //foreach (DataRow row in dt.Rows)
            //{
            //    string dfd = row["ID"].ToString();
            //    string rdate = ddt.Rows[aa]["ddate"].ToString();
            //    DataRow[] r = hh.Select("employeeid=" + row["ID"] + " and date='" + ddt.Rows[aa]["ddate"].ToString() + "'");
            //    string rid = r[0][0].ToString();
            //    string shftcode = r[0][9].ToString();
            //    dt.Rows[aa][k] = r[0][9];
            //}

            //k++;
        }

        gv_schedule.DataSource = dt;
        gv_schedule.DataBind();

        //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
        DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where status is null");

        for (int row = 0; row < dt.Rows.Count; row++)
        {
            for (int col = 4; col < dt.Columns.Count; col++)
            {
                DropDownList ddl = new DropDownList();

                string oi = dt.Rows[row]["ID"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_");
                ddl.ID = dt.Rows[row]["ID"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_").Replace(" ", "").Replace("(", "").Replace(")", "");// grid_view.Rows[row].Cells[0].Text + "_" + dt.Columns[col].ToString();
                ddl.Items.Clear();
                ddl.CssClass = "minimala";
                ddl.Enabled = false;
                //ddl.Items.Add(new ListItem("", "0"));
                foreach (DataRow dr in sc.Rows)
                {
                    ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
                    item.Attributes.Add("title", dr["remarks"].ToString());
                    ddl.Items.Add(item);

                }

                if (hh.Rows.Count > 0)
                {
                    string hold = null;
                    string[] colname = dt.Columns[col].ToString().Split(' ');
                    string x = dt.Rows[row][3].ToString();
                    string y = colname[0];
                    DataRow[] dr = hh.Select("employeeID=" + dt.Rows[row][2].ToString() + " and date='" + colname[0] + "'");
                    // foreach (DataRow r in dr)
                    //{
                    //    hold = r[3].ToString();
                    //}
                    hold = dr[0][3].ToString();
                    ddl.SelectedValue = dr[0][3].ToString();
                }

                gv_schedule.Rows[row].Cells[col].Controls.Add(ddl);
            }
        }
    }
}