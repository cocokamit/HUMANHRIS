using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_canteen_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        schedule();
    }

    protected void click_approve(object sender, EventArgs e)
    {
        string id = Request.QueryString["key"].ToString();
        function.ReadNotification(id,0);
        string query = "update dtr_period set [status]='approved' where id=" + id;
        query += " update [TChangeShift] set [status] ='approved' where periodid=" + id;
        query += " update [TChangeShiftLine] set [status] ='approved' where ChangeShiftId=" + hf_TChangeShiftID.Value;
        dbhelper.getdata(query);
        function.AddNotification("Approved Schedule", "dtrp", hf_schedulerid.Value, Request.QueryString["key"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully approved'); window.location='schedule'", true);
    }

    protected void click_decline(object sender, EventArgs e)
    {
        string id = Request.QueryString["key"].ToString();
        function.ReadNotification(id, 0);
        dbhelper.getdata("insert into declined_list (date_input,period_id,notes) values(getdate(),'" + id + "','" + txt_reason.Text.Replace("'","") + "') ");

        string query = "update dtr_period set [status]='decline' where id=" + id;
        dbhelper.getdata(query);
        function.AddNotification("Decline Schedule", "dtrp", hf_schedulerid.Value, Request.QueryString["key"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='schedule'", true);
    }

    protected void schedule()
    {
        //read notification
        //need encription
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        //need encription
       string id = Request.QueryString["key"].ToString();


       string tt = "select d.id emp_id, d.firstname +' ' + d.lastname  scheduler, a.entryuserid, left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
        "c.shiftcode, " +
        "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
        "d.IdNumber as ID, c.shiftcode, a.id TChangeShiftID " +
        "from TChangeShift a " +
        "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
        "left join MShiftCode c on b.ShiftCodeId=c.id " +
        "left join nobel_user e on a.EntryUserId=e.id " +
        "left join memployee d on e.emp_id=d.id " +
        "where a.PeriodId=" + id;

        DataTable hh = dbhelper.getdata(tt);
        hf_schedulerid.Value = hh.Rows[0]["entryuserid"].ToString();
        lb_scheduler.Text = hh.Rows[0]["scheduler"].ToString();
        hf_TChangeShiftID.Value = hh.Rows[0]["TChangeShiftID"].ToString();

        DataTable dtr = getdata.sys_dtr("id=" + id);
        lb_period.Text = dtr.Rows[0]["period"].ToString();
        lb_created.Text = dtr.Rows[0]["date_input"].ToString();

        if (dtr.Rows[0]["status"].ToString() == "approved")
            p_footer.Visible = false;
       

        string query = "select b.IdNumber as ID,b.FirstName+' '+b.LastName as Employee,b.id as idd, b.payrollgroupid " +
                       "from Approver a " +
                       "left join MEmployee b on a.emp_id=b.id " +
                       "where a.under_id='" + hh.Rows[0]["emp_id"].ToString() + "' and a.herarchy=0 and b.departmentID <> '3' and b.PayrollGroupId<>'4' ";

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
                    if (dr.Count() > 0)
                    {
                        hold = dr[0][6].ToString();
                        ddl.SelectedValue = dr[0][6].ToString();
                    }
                }

                gv_schedule.Rows[row].Cells[col].Controls.Add(ddl);
            }
        }
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
    }

    protected void view(object sender, EventArgs e)
    {
         modal_delete.Style.Add("display", "block");
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("schedule", false);
    }

     
}