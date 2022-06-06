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

        if (!IsPostBack)
            Loadable();

        schedule();
    }

    protected void Loadable()
    {
        /**BTK 
      * 111119 OIC
      * **/
        ViewState["approver"] = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["oic"].ToString();
        ViewState["url"] = Request.QueryString["oic"] == null ? "schedule-approval" : "schedule-approval?oic=true&key=" + Request.QueryString["oic"].ToString();
    }

    protected void click_approve(object sender, EventArgs e)
    {
        /**BUILD**/

        /**BTK 
        * 111119 OIC
        * **/
        string awts = Request.QueryString["id"].ToString();
        string id = Request.QueryString["key"].ToString();
        DataTable dt = (DataTable)ViewState["PERIOD"];
        DataTable collection = (DataTable)ViewState["data"];


        if (awts == "0")
        {
            DataTable xx = dbhelper.getdata("select * from allow_admin where id='9' and allow='no'");

            if (xx.Rows.Count != 0)
            {
                string query = "update dtr_period set [status] ='approved' where id=" + id;
                query += " update [TChangeShift] set [status] ='approved' where periodid=" + id + ";";

                foreach (DataRow item in collection.Rows)
                {
                    query += "update [TChangeShiftLine] set [status] ='approved' where employeeid='" + item["idd"] + "' and ChangeShiftId=" + dt.Rows[0]["TChangeShiftID"].ToString() + ";";
                }
                dbhelper.getdata(query);
            }
            else
                dbhelper.getdata("update dtr_period set [status]='verification',approver_id=" + awts + " where id=" + id);


        }
        else
            dbhelper.getdata("update dtr_period set approver_id=" + awts + " where id=" + id + "");

        //SETLOG
        stateclass a = new stateclass();
        a.sa = id;
        a.sb = ViewState["approver"].ToString();
        a.sc = "S";
        a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
        a.se = "";
        bol.system_logs(a);

        //OIC
        OIC(id, "Approved");

        function.Notification("scheduling", "approved", id, dt.Rows[0]["emp_id"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully approved'); window.location='" + ViewState["url"].ToString() + "'", true);
    }

    protected void OIC(string id, string action)
    {
        /**BTK 
       * 111119 OIC
       * **/
        DataTable oic = getdata.OIC(Session["emp_id"].ToString());
        if (oic.Rows.Count > 0)
        {
            dbhelper.getdata("insert into approval_OIC_transaction values (" + oic.Rows[0]["id"].ToString() + "," + id + ",'schedule','" + action + "')");
        }
    }

    protected void click_decline(object sender, EventArgs e)
    {
        /**BTK 
       * 111119 OIC
       * **/
        string id = Request.QueryString["key"].ToString();
        DataTable dtr = (DataTable)ViewState["PERIOD"];

        string query = "insert into declined_list (date_input,period_id,notes) values(getdate(),'" + id + "','" + txt_reason.Text.Replace("'", "") + "');" +
        "update dtr_period set [status]='decline' where id=" + id + ";" +
        "update TChangeShift set status='decline' where id=" + dtr.Rows[0]["TChangeShiftID"].ToString() + ";" +
        "update TChangeShiftLine set status='decline' where ChangeShiftId=" + dtr.Rows[0]["TChangeShiftID"].ToString() + ";";
        dbhelper.getdata(query);

        //OIC
        OIC(id, "Cancelled");

        function.Notification("scheduling", "declined", dtr.Rows[0]["id"].ToString(), dtr.Rows[0]["emp_id"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='" + ViewState["url"].ToString() + "'", true);
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

        DataTable sc = getdata.getShifts();
        ViewState["sc"] = sc;
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
                foreach (DataRow drothers in sc.Rows)
                {
                    if (drothers["ShiftCode"].ToString() == shiftcode)
                    {
                        gv_schedule.Rows[row].Cells[col].Attributes.Add("title", drothers["remarks"].ToString());
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