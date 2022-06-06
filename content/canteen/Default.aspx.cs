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
        DataTable collection = (DataTable)ViewState["data"];

        DataTable dt = (DataTable)ViewState["PERIOD"]; //dbhelper.getdata("select top 1 (id) from TChangeShift where periodid=" + id + "");
        string query = "update dtr_period set [status]='approved' where id=" + id;
        query += " update [TChangeShift] set [status] ='approved' where periodid=" + id +";";

        foreach (DataRow item in collection.Rows)
        {
            query += "update [TChangeShiftLine] set [status] ='approved' where employeeid='" + item["idd"] + "' and ChangeShiftId=" + dt.Rows[0]["TChangeShiftID"].ToString() + ";";
        }

        dbhelper.getdata(query);

        function.Notification("scheduling", "approved", id, dt.Rows[0]["emp_id"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully approved'); window.location='schedule'", true);
    }

    protected void click_decline(object sender, EventArgs e)
    {
        string id = Request.QueryString["key"].ToString();
        function.ReadNotification(id, 0);
        dbhelper.getdata("insert into declined_list (date_input,period_id,notes) values(getdate(),'" + id + "','" + txt_reason.Text.Replace("'","") + "') ");

        string query = "update dtr_period set [status]='decline' where id=" + id +";";
        DataTable collection = (DataTable)ViewState["data"];
        foreach (DataRow item in collection.Rows)
        {
            query += "update [TChangeShiftLine] set [status] ='decline' where employeeid='" + item["idd"] + "' and ChangeShiftId=" + id + ";";
        }
        dbhelper.getdata(query);

        //function.AddNotification("Decline Schedule", "dtrp", hf_schedulerid.Value, Request.QueryString["key"].ToString());
        //function.Notification("scheduling", "declined", id, ViewState["SID"].ToString());
        
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='schedule'", true);
    }

    protected void schedule()
    {
        /*READ NOTIFICATION*/
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        string id = Request.QueryString["key"].ToString();

        /*PERIOD*/
        DataTable dtr = getdata.GetSchedulePeriod("a.id=" + id);
        string TChangeShiftID = dtr.Rows[0]["TChangeShiftID"].ToString();
        string from = dtr.Rows[0]["date_start"].ToString();
        string to = dtr.Rows[0]["date_end"].ToString();
        lb_period.Text = dtr.Rows[0]["period"].ToString();
        lb_created.Text = dtr.Rows[0]["date_input"].ToString();
        lb_scheduler.Text = dtr.Rows[0]["fullname"].ToString();
        lb_department.Text = dtr.Rows[0]["department"].ToString() + " (" + dtr.Rows[0]["seccode"].ToString() + ")";
        ViewState["PERIOD"] = dtr;

        if (dtr.Rows[0]["status"].ToString() == "approved")
            p_footer.Visible = false;



        /*EMPLOYEE*/
        DataTable dt = getdata.FinalScheduleMember(TChangeShiftID);
        ViewState["data"] = dt;

        /*SCHEDULES*/
        DataTable hh = getdata.FinalShedule(TChangeShiftID, from, to);

        /*HOLIDAYS*/
        DataTable holidays = getdata.Holiday(from, to);

        /*BIND FINAL TABLE*/
        DataTable ddt = getdata.GetDateCollection(id);
        ViewState["range"] = ddt;
        for (int aa = 0; aa < ddt.Rows.Count; aa++)
        {
            dt.Columns.Add(new DataColumn(ddt.Rows[aa]["ddate"].ToString() + " (" + ddt.Rows[aa]["mdate"].ToString() + ")", typeof(string)));
        }
        gv_schedule.DataSource = dt;
        gv_schedule.DataBind();

        /*SET DATA*/
        int ctrl = 5;
        for (int row = 0; row < dt.Rows.Count; row++)
        {
            DataTable leaves = getdata.Leave("b.id=" + dt.Rows[row]["idd"].ToString() + " and a.date between convert(date,'" + from + "') and convert(date,'" + to + "')");

            for (int col = ctrl; col < dt.Columns.Count; col++)
            {
                string condition = "employeeID='" + dt.Rows[row]["idd"].ToString() + "' and date='" + ddt.Rows[col - ctrl][0].ToString() + "'";
                DataRow[] dr = hh.Select(condition);
                string shiftcode = dr[0]["shiftcode"].ToString();
                gv_schedule.Rows[row].Cells[col].Text = shiftcode;

                DataRow[] holiday = holidays.Select("date='" + ddt.Rows[col - ctrl][0].ToString() + "'");
                if (holiday.Count() > 0 && shiftcode.Length == 0)
                {
                    gv_schedule.Rows[row].Cells[col].Text = "PH";
                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "holiday");
                }

                //LEAVE
                if (dtr.Rows[0]["status"].ToString() == "approved")
                {
                    if (leaves.Rows.Count > 0)
                    {
                        if (leaves.Rows[0]["date"].ToString() == ddt.Rows[col - ctrl][0].ToString())
                        {
                            gv_schedule.Rows[row].Cells[col].Text = leaves.Rows[0]["Leave"].ToString();
                            gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "holiday");
                        }
                    }
                }
            }
        }
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
           // e.Row.Cells[4].Visible = false;
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