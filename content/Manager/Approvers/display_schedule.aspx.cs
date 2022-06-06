using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_display_schedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        schedule();
    }

    protected void click_approve(object sender, EventArgs e)
    {
        string awts = Request.QueryString["id"].ToString();
        string id = Request.QueryString["key"].ToString();
        DataTable dt = (DataTable)ViewState["PERIOD"];

        //SETLOG
        stateclass a = new stateclass();
        a.sa = id;
        a.sb = Session["emp_id"].ToString();
        a.sc = "S";
        a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
        a.se = "";
        bol.system_logs(a);

       // DataTable dt = dbhelper.getdata("select top 1 (id) from TChangeShift where periodid=" + id + "");
        string query = "update dtr_period set [status]='verification',approver_id=" + awts + " where id=" + id;
        //query += " update [TChangeShiftLine] set [status] ='approved' where [type]='1' and ChangeShiftId=" + dt.Rows[0]["id"].ToString();

        dbhelper.getdata(query);




        //function.Notification("scheduling", "approved", id, dt.Rows[0]["emp_id"].ToString());
        //function.AddNotification("Approved Schedule", "dtrp", hf_schedulerid.Value, Request.QueryString["key"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully approved'); window.location='schedule-approval'", true);
    }

    protected void click_decline(object sender, EventArgs e)
    {
        string id = Request.QueryString["key"].ToString();
        DataTable dtr = (DataTable)ViewState["PERIOD"];

        string query = "insert into declined_list (date_input,period_id,notes) values(getdate(),'" + id + "','" + txt_reason.Text.Replace("'", "") + "');" +
        "update dtr_period set [status]='decline' where id=" + id + ";" +
        "update TChangeShift set status='decline' where id=" + dtr.Rows[0]["TChangeShiftID"].ToString() + ";" +
        "update TChangeShiftLine set status='decline' where ChangeShiftId=" + dtr.Rows[0]["TChangeShiftID"].ToString() + ";";
        dbhelper.getdata(query);

        function.Notification("scheduling", "declined", dtr.Rows[0]["id"].ToString(), dtr.Rows[0]["emp_id"].ToString());

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='schedule-approval'", true);
    }

    protected void schedule()
    {
        //need encription
        //read notification
        //need encription

        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        string id = Request.QueryString["key"].ToString();

        /*PERIOD*/
        DataTable dtr = getdata.GetSchedulePeriod("a.id=" + id);
        string TChangeShiftID = dtr.Rows[0]["TChangeShiftID"].ToString();
        string from = dtr.Rows[0]["date_start"].ToString();
        string to = dtr.Rows[0]["date_end"].ToString();
        period.Text = dtr.Rows[0]["period"].ToString();
        ViewState["PERIOD"] = dtr;

        if (dtr.Rows[0]["status"].ToString() == "approved")
            p_footer.Visible = false;


        /*EMPLOYEE*/
        DataTable dt = getdata.FinalScheduleMember(TChangeShiftID);
        ViewState["data"] = dt;


        /*SCHEDULE*/
        DataTable hh = getdata.FinalShedule(TChangeShiftID, from, to);
        hf_schedulerid.Value = hh.Rows[0]["entryuserid"].ToString();
        hf_TChangeShiftID.Value = hh.Rows[0]["TChangeShiftID"].ToString();

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
        Response.Redirect("h_schedule", false);
    }
}