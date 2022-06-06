using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Reflection;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Web.UI.HtmlControls;
using Spire.Xls;

public partial class content_hr_dtr_period : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            DataBind();
            pay_range();
        }

        if (Request.QueryString["key"] != null)
            read(Request.QueryString["key"].ToString());

        schedule();

    }

    protected void pay_range()
    {
        //and month(dfrom) = month(getdate())
        DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto from payroll_range where action is null  order by dfrom desc";

        DataTable dt_range = dbhelper.getdata(query);

        ViewState["pak"] = dt_range;

        ddl_pay_range.Items.Clear();
        //ddl_pay_range.Items.Add(new ListItem("Payrol Period","0"));
        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["id"].ToString()));
        }
    }

    protected void read(string key)
    {
        function.ReadNotification(Request.QueryString["nt"].ToString());
        DataTable dt = getdata.GetSchedulePeriod("a.id = " + key);

        id.Value = key;
        hf_TChangeShiftID.Value = dt.Rows[0]["TChangeShiftID"].ToString();
        hf_shiftline.Value = dt.Rows[0]["cnt"].ToString();
        hf_status.Value = dt.Rows[0]["status"].ToString();
        l_range.Text = dt.Rows[0]["period"].ToString();
        hf_start.Value = dt.Rows[0]["date_start"].ToString();
        hf_end.Value = dt.Rows[0]["date_end"].ToString();

        main.Visible = false;
        p_scheduling.Visible = true;
        lb_shiftline.Visible = hf_shiftline.Value == "0" ? true : false;


        switch (dt.Rows[0]["status"].ToString())
        {
            case "for approval":
            case "approved":
            case "verification":
                p_footer.Visible = false;
                btn_upload.Visible = false;
                break;
            default:
                p_footer.Visible = true;
                break;
        }
    }

    protected void schedule_rowbound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
    }

    protected void SetLogs(string action)
    {
        string q = "insert into dtr_period_logs values (getdate(),'" + action + "'," + id.Value + ")";
        dbhelper.getdata(q);
    }

    protected void click_submit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            hf_approver.Value = row.Cells[8].Text;
            hf_start.Value = row.Cells[5].Text;
            hf_end.Value = row.Cells[6].Text;
            modal_submit.Style.Add("display", "block");
        }
    }

    protected void click_submitschedule(object sender, EventArgs e)
    {
        /**BUILD 1.1.0**/
        SetLogs("submit");

        DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
        string aw = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

        string query = "update dtr_period set status='for approval',approver_id='" + aw + "' where id=" + id.Value + ";";
        DataTable settings = dbhelper.getdata("select status,* from TChangeShift  where PeriodId=" + id.Value);
        DataTable collection = (DataTable)ViewState["data"];
        foreach (DataRow item in collection.Rows)
        {
            query += "update [TChangeShiftLine] set  ChangeShiftId=" + settings.Rows[0]["id"] + ", [status] ='for approval' where employeeid='" + item["idd"] + "' and date between CONVERT(datetime,'" + hf_start.Value + "') and CONVERT(datetime,'" + hf_end.Value + "');";
        }

        dbhelper.getdata(query);
        function.Notification("scheduling", "for approval", id.Value, hf_approver.Value);
        Response.Redirect(Request.RawUrl);
    }

    protected void click_DeletePeriod(object sender, EventArgs e)
    {
        SetLogs("delete");
        dbhelper.getdata("update dtr_period set status='deleted ' + convert(varchar,getdate()) where id=" + id.Value);
        Response.Redirect(Request.RawUrl);
    }

    protected void click_SaveSchedule(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        DataTable dt = (DataTable)ViewState["data"];

        if (hf_shiftline.Value == "0")
        {
            a.sa = id.Value; //period;
            a.sb = "1";//dt.Rows[3]["payrollgroupid"].ToString();
            a.sc = Session["user_id"].ToString();
            idd.Value = bol.loadchangeshift1st(a);
        }
         
        int ii = 0;
        foreach (DataRow dr in dt.Rows)
        {
            int j = 2;
            DataTable range = (DataTable)ViewState["range"];
            DataTable schedules = (DataTable)ViewState["ttt"];

            string transaction = hf_shiftline.Value == "0" ? idd.Value : hf_TChangeShiftID.Value; 

            foreach (DataRow date in range.Rows)
            {
                DataRow[] schedule = schedules.Select("employeeID=" + dr["idd"].ToString() + " and date='" + date["ddate"].ToString() + "'");
                string ddl_id = dr["idnumber"] + "_" + date["ddate"].ToString().Replace("/", "_") + date["mdate"].ToString();
                DropDownList ddl = (DropDownList)gv_schedule.Rows[ii].Cells[j].FindControl(ddl_id);

                if (schedule.Count() == 0)
                {
                    a.sa = dr["idd"].ToString();
                    a.sb = date["ddate"].ToString();
                    a.sc = ddl.SelectedValue;
                    a.sd = transaction;
                    bol.loadchangeshift(a);
                }
                else
                {
                    if (ddl.Enabled)
                    {
                        DataTable hekhek = dbhelper.getdata("select top 1 (id) from TChangeShift where periodid=" + id.Value + "");
                        dbhelper.getdata("update TChangeShiftLine set ShiftCodeId=" + ddl.SelectedValue + ",ChangeShiftId='" + hekhek.Rows[0]["id"].ToString() + "' where EmployeeId='" + dr[2].ToString() + "' and left(convert(varchar,date,101),10)='" + date["ddate"].ToString() + "' ");
                    }
                }
                j++;
            }
            ii++;
        }

        string query = "update dtr_period set status='created' where id=" + id.Value;
        dbhelper.getdata(query);
        SetLogs("create");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='" + Request.RawUrl + "'", true);
    }

    protected void click_back(object sender, EventArgs e)
    {
        Response.Redirect(hf_role.Value == "HOD" ? "dtrp" : "scheduler");
        main.Visible = true;
        p_scheduling.Visible = false;
    }

    protected void click_set(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            hf_shiftline.Value = row.Cells[1].Text;
            hf_status.Value = row.Cells[4].Text;
            l_range.Text = row.Cells[3].Text;
            hf_start.Value = row.Cells[5].Text;
            hf_end.Value = row.Cells[6].Text;
            hf_TChangeShiftID.Value = row.Cells[9].Text;

            main.Visible = false;
            p_scheduling.Visible = true;
            lb_shiftline.Visible = hf_shiftline.Value == "0" ? true : false;

            switch ( row.Cells[4].Text)
            {
                case "for approval" :
                case "approved":
                case "verification":
                    p_footer.Visible = false;
                    btn_upload.Visible = false;
                    break;
                default:
                    p_footer.Visible = true;
                    break;
            }

            schedule();
        }
    }

    protected void clickSaveSetting(object sender, EventArgs e)
    {
        string x = cbAHOD.Checked ? "AHOD" : "0";
        dbhelper.getdata("update TChangeShift set class='" + x + "' where PeriodId=" + id.Value);
        modal_setting.Style.Add("display","none");
    }

    protected void schedule()
    {
        if (id.Value.Length > 0)
        {
            //SCHEDULE
            string tt = "where CONVERT(date,b.date) between CONVERT(date,'" + hf_start.Value + "') and CONVERT(date,'" + hf_end.Value + "') and ";

            if (Session["role"].ToString() == "10")
                tt += "e.under_id=" + Session["emp_id"].ToString();
            else
                tt += "g.level like '%hod%' and d.departmentid = " + Session["department"] + "  ";

            tt += " and f.status<>0 order by b.[date]";

            DataTable hh = getdata.schedule(tt);
            ViewState["ttt"] = hh;

            /**EMPLOYEE PayrollGroupId=4 [RESIGN]**/
            if (hf_action.Value.Length == 0)
            {
                DataTable settings = dbhelper.getdata("select status,* from TChangeShift  where PeriodId=" + id.Value);
                if (settings.Rows.Count > 0)
                    cbAHOD.Checked = settings.Rows[0]["class"].ToString() == "AHOD" ? true : false;
            }

            string query = string.Empty;
            string odi = Session["role"].ToString();
            switch (Session["role"].ToString())
            {
                case "8":
                    //GM
                    query = "b.id=" + Session["emp_id"].ToString();
                    break;
                case "10":
                    //Scheduler
                    query = "a.under_id='" + Session["emp_id"].ToString() + "' and a.herarchy=0 and c.status<>0 order by  d.seccode, b.FirstName +' '+b.LastName";
                    break;
                default:
                    string lvl = cbAHOD.Checked ? "e.level like '%hod%'" : "e.level = 'HOD'";
                    query = lvl + " and b.departmentid = " + Session["department"] + " and a.herarchy=0 and c.status<>0  order by  d.seccode, b.FirstName +' '+ b.LastName";
                    break;
            }
            
          
            DataTable dt = getdata.Scheduler(query);
            DataTable dtDup = getdata.Scheduler_Duplicate(query);
            ViewState["data"] = dt;

            /*PAYROLL DATE RANGE COLLECTION*/
            DataTable ddt = getdata.GetDateCollection(id.Value);
            ViewState["range"] = ddt;

            for (int aa = 0; aa < ddt.Rows.Count; aa++)
            {
                dt.Columns.Add(new DataColumn(ddt.Rows[aa]["ddate"].ToString() + " (" + ddt.Rows[aa]["mdate"].ToString() + ")", typeof(string)));
            }

            gv_dtrtemplate.DataSource = dt;
            gv_dtrtemplate.DataBind();

            gv_schedule.DataSource = dt;
            gv_schedule.DataBind();

            DataTable sc = getdata.getShifts();
            DataTable holidays = getdata.Holiday(hf_start.Value, hf_end.Value);

            int alt = 0;
            int fn = 0;
            int ctrl = 5;

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                /**BTK 1112319 
                 * REFLECT APPROVED LEAVE **/
                DataTable leaves = getdata.Leave("b.id=" + dt.Rows[row]["idd"].ToString() + " and a.date between convert(date,'" + hf_start.Value + "') and convert(date,'" + hf_end.Value + "')");

                for (int col = ctrl; col < dt.Columns.Count; col++)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.Style.Add("Width", "100%");
                    string oi = dt.Rows[row]["idnumber"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_");
                    ddl.ID = dt.Rows[row]["idnumber"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_").Replace(" ", "").Replace("(", "").Replace(")", "");// grid_view.Rows[row].Cells[0].Text + "_" + dt.Columns[col].ToString();
                    ddl.Items.Clear();
                    ddl.CssClass = "select2";
                    //ddl.Items.Add(new ListItem("", "0"));
                    foreach (DataRow dr in sc.Rows)
                    {
                        ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
                        item.Attributes.Add("title", dr["remarks"].ToString());
                        ddl.Items.Add(item);

                        if (item.ToString().Contains("RD"))
                            item.Attributes.Add("style", "color:red");
                        else if (item.ToString().Contains("SP(RD)"))
                            item.Attributes.Add("style", "color:red");
                        else if (item.ToString().Contains("SP"))
                            item.Attributes.Add("style", "color:green");
                        else
                            item.Attributes.Add("style", "color:blue");


                    }

                   

                    //HOLIDAY
                    DataRow[] holiday = holidays.Select("date='" + ddt.Rows[col - ctrl][0].ToString() + "'");
                    if (holiday.Count() > 0)
                    {
                        gv_schedule.HeaderRow.Cells[col].ForeColor = System.Drawing.Color.Red;
                        ListItem item = new ListItem("PH", "65");
                        item.Attributes.Add("title", "Public Holiday");
                        ddl.Items.Add(item);
                    }



                    if (hh.Rows.Count > 0)
                    {
                        string hold = null;
                        string[] colname = dt.Columns[col].ToString().Split(' ');
                        DataRow[] dr = hh.Select("employeeID=" + dt.Rows[row][2].ToString() + " and date='" + colname[0] + "'");

                        //if (dt.Rows[row][2].ToString() == "4")
                        //{ }

                        if (dr.Count() > 0)
                        {
                            hold = dr[0][3].ToString();
                            ddl.SelectedValue = dr[0][3].ToString();

                            if (dr[0]["status"].ToString().Contains("appr"))
                            {
                                fn++;
                                ddl.Enabled = true;
                            }

                        }

                    }
                    else
                    {
                        string defaultSchedule = dtDup.Rows[row]["ShiftCodeId"].ToString();
                        ddl.SelectedValue = defaultSchedule;
                    }

                    gv_schedule.Rows[row].Cells[col].Controls.Add(ddl);


                    /**BTK 1112319 
                    * REFLECT APPROVED LEAVE **/
                    if (hf_status.Value == "approved")
                    {
                        if (leaves.Rows.Count > 0)
                        {
                            DataRow[] leave = leaves.Select("date='" + ddt.Rows[col - ctrl][0].ToString() + "'");
                            if (leave.Length > 0)
                            {
                                ddl.Items.Clear();
                                gv_schedule.HeaderRow.Cells[col].ForeColor = System.Drawing.Color.Green;
                                ListItem item = new ListItem(leave[0]["leave"].ToString(), "0");
                                item.Attributes.Add("title", leave[0]["leave"].ToString());
                                ddl.Items.Add(item);
                            }
                        }
                    }


                    if (hf_status.Value == "created" || hf_status.Value == "new")
                    {
                        if (col == ctrl)
                        {
                            HtmlGenericControl icon = new HtmlGenericControl("i");
                            icon.Attributes.Add("class", "fa fa-clone clone");
                            //icon.Attributes.Add("style", "display:none");
                            gv_schedule.Rows[row].Cells[col].Controls.Add(icon);
                        }
                    }

                }
                if (fn > 0)
                    alt++;

            }
            if (alt == dt.Rows.Count)
            {
                btn_upload.Visible = false;
                Button1.Visible = true;
            }
        }
    }

    protected void click_modalsave(object sender, EventArgs e)
    {
        if (checkfield())
        {
            if (hf_action.Value == "0")
            {
                /*NEW DTR PERIOD*/
                //string[] period = ddl_pay_range.SelectedItem.ToString().Split('-');
                string[] period = (txt_from.Text+"-"+txt_to.Text).Split('-');
                string query = "SELECT  * FROM dtr_period " +
                "WHERE emp_id='" + Session["emp_id"].ToString() + "' and [type]='" + hf_role.Value + "' and status not like '%delete%' and CONVERT(varchar,date_start,101) = '" + period[0] + "' and CONVERT(varchar,date_end,101) = '" + period[1] + "' ";
                
                DataTable dt = dbhelper.getdata(query);
                if (dt.Rows.Count > 0)
                    setAlert(true, "danger", "Period is invalid");
                else
                {
                    setAlert(false);

                    DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
                    string aw = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();

                    dbhelper.getdata("insert into dtr_period (date_input,date_start,date_end,remarks,status,emp_id,[type],approver_id) values(getdate(),'" + period[0] + "','" + period[1] + "','" + txt_remarks.Text.Trim() + "','new','" + Session["emp_id"].ToString() + "','" + hf_role.Value + "',"+aw+")");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved'); window.location='"+Request.RawUrl+"'", true);
                }
                
            }
            else
            {
                /*DELETE DTR PERIOD*/
                string query = "update dtr_period set status='" + "deleted-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id + " ";
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='dtrp'", true);
            }
        }
    }

    protected void setAlert(bool toggle, string action, string msg) 
    {
        alert_modal.Visible = toggle;
        alert_modal.CssClass = "alert alert-" + action;
        l_modal.Text = msg;
    }

    protected void setAlert(bool toggle, string action)
    {
        alert_modal.Visible = toggle;
        alert_modal.CssClass += action;
    }

    protected void setAlert(bool toggle)
    {
        alert_modal.Visible = toggle;
    }

    protected string bind(string condition)
    {
        return "select a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt, DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,* " +
        "from dtr_period a where " + condition + " order by DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9)";
    }

    protected void DataBind()
    {
        hf_role.Value = Session["role"].ToString() == "10" ? "SCHEDULER" : "HOD";
        lb_setting.Visible = Session["role"].ToString() == "10" ? false : true;

        //string query = bind("status not like '%delete%' and emp_id='" + Session["emp_id"].ToString() + "' and [type]='" + hf_role.Value + "' ");
        //DataSet ds = bol.display(query);
        //grid_view.DataSource = ds;

        DataTable dt = getdata.scheduleRange("a.emp_id='" + Session["emp_id"] + "' and [type]='" + hf_role.Value + "'");
        grid_view.DataSource = dt;
        grid_view.DataBind();
        grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void addUnder(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_addUndertime");
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hf_action.Value = "1";
            id.Value = row.Cells[0].Text;
            modal.Style.Add("display", "block");
            p_create.Visible = false;
            p_delete.Visible = true;
        }
    }

    protected void clickSetting(object sender, EventArgs e)
    {
        modal_setting.Style.Add("display", "block");
        hf_action.Value = "HOD&AHOD";
    }

    protected void add(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        modal.Style.Add("display", "block");
        p_create.Visible = true;
        p_delete.Visible = false;
    }
 
    protected void cpop(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
        modal_upload.Style.Add("display","none");
        modal_setting.Style.Add("display", "none");
        modal_delete.Style.Add("display", "none");
        modal_upload.Style.Add("display", "none");
        modal_submit.Style.Add("display", "none");

        hf_action.Value = "";

        //Response.Redirect("dtrp", false);
    }
 
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            LinkButton lnk_dec = (LinkButton)e.Row.FindControl("lnk_details");
            string[] stat = e.Row.Cells[4].Text.Split('-');
            if (stat[0] == "Approved")
                lnkcan.Visible = false;

            LinkButton lb_status = (LinkButton)e.Row.FindControl("lb_status");
            if (e.Row.Cells[4].Text == "new")
            {
                lb_status.Enabled = false;
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        lb_status.Text = "Incomplete";
                        lb_status.CssClass = "label label-danger pull-right"; 
                        break;
                }
            }
            else
            {
                switch (e.Row.Cells[4].Text)
                {
                    case "created":
                        lb_status.Text = "Submit";
                        lb_status.CssClass = "label label-success pull-right";
                        break;
                    case "for approval":
                        lb_status.Enabled = false;
                        lb_status.Text = "For approval";
                        lb_status.CssClass = "label label-primary pull-right";
                        break;
                    case "approved":
                        lb_status.Enabled = false;
                        lb_status.Text = "";
                        lb_status.CssClass = "fa fa-check-circle-o text-success pull-right";
                        break;
                    case "decline":
                        lnk_dec.Visible = true;
                        lb_status.Enabled = false;
                        lb_status.Text = "Declined";
                        lb_status.Visible = false;
                        lb_status.CssClass = "label label-primary pull-right";
                        break;
                    case "verification":
                        lb_status.Text = "Verification";
                        lb_status.Enabled = false;
                        lb_status.CssClass = "label label-warning pull-right";
                        break;

                }
            }
        }
    }

    protected bool checkfield()
    {
        bool oi = true;

        l_start.Text = txt_from.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_end.Text = txt_to.Text.Replace(" ", "").Length == 0 ? "*" : "";

        if ((l_start.Text + l_end.Text).Contains("*"))
            oi = false;

        return oi;
    }

    protected void viewdec(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           
            string query = "select * from declined_list where period_id='" + row.Cells[0].Text + "' order by date_input desc ";
            DataSet ds = new DataSet();
            ds = bol.display(query);

            grid_dec.DataSource = ds;
            grid_dec.DataBind();
            modal_delete.Style.Add("display", "block");
        }

    }

    protected void upload(object sender, EventArgs e)
    {
        modal_upload.Style.Add("display", "block");
    }

    protected void upload_save(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        DataTable dt = (DataTable)ViewState["data"];

        if (hf_shiftline.Value == "0")
        {
            a.sa = id.Value; //period;
            a.sb = "1";//dt.Rows[3]["payrollgroupid"].ToString();
            a.sc = Session["user_id"].ToString();
            idd.Value = bol.loadchangeshift1st(a);
        }

        //dtper = dbhelper.getdata("select * from dtr_period where id=" + id.Value + "");
        int ii = 0;
        foreach (DataRow dr in dt.Rows)
        {
            int j = 2;
            DataTable range = (DataTable)ViewState["range"];
            foreach (DataRow date in range.Rows)
            {
                string ddl_id = dr["idnumber"] + "_" + date["ddate"].ToString().Replace("/", "_") + date["mdate"].ToString();
                DropDownList ddl = (DropDownList)gv_schedule.Rows[ii].Cells[j].FindControl(ddl_id);

                if (hf_shiftline.Value == "0")
                {
                    a.sa = dr["idd"].ToString();
                    a.sb = date["ddate"].ToString();
                    a.sc = ddl.SelectedValue;
                    a.sd = idd.Value;
                    bol.loadchangeshift(a);
                }
                else
                {
                    //string x = dr[0].ToString();
                    //string b = date["ddate"].ToString();
                    //dbhelper.getdata("update TChangeShiftLine set ShiftCodeId=" + ddl.SelectedValue + " where EmployeeId='" + dr[2].ToString() + "' and left(convert(varchar,date,101),10)='" + date["ddate"].ToString() + "' ");

                    DataTable yy = (DataTable)ViewState["ttt"];
                    string x = dr[0].ToString();
                    string b = date["ddate"].ToString();

                    string gg = "select * from TChangeShiftLine where EmployeeId='" + dr[2].ToString() + "' and left(convert(varchar,date,101),10)='" + date["ddate"].ToString() + "' ";
                    DataTable ff = dbhelper.getdata(gg);

                    if (ff.Rows.Count == 0)
                    {
                        a.sa = dr["idd"].ToString();
                        a.sb = date["ddate"].ToString();
                        a.sc = ddl.SelectedValue;
                        a.sd = yy.Rows[0]["tid"].ToString();
                        bol.loadchangeshift(a);
                    }
                    else
                    {
                        dbhelper.getdata("update TChangeShiftLine set ShiftCodeId=" + ddl.SelectedValue + " where EmployeeId='" + dr[2].ToString() + "' and left(convert(varchar,date,101),10)='" + date["ddate"].ToString() + "' ");
                    }
                }
                j++;
            }
            ii++;
        }

    
    }

    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();

           
            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    //protected void process(object sender, EventArgs e)
    //{
    //    //try
    //    //{
    //        System.Data.DataSet DtSet;
    //        System.Data.OleDb.OleDbDataAdapter MyCommand;
    //        string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
    //        FileUpload1.SaveAs(path);
    //        string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", path);
    //        OleDbConnection MyConnection = new OleDbConnection();
    //        MyConnection.ConnectionString = excelConnectionString;
    //        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
    //        MyCommand.TableMappings.Add("Table", "TestTable");
    //        DtSet = new System.Data.DataSet();
    //        MyCommand.Fill(DtSet);
    //        MyConnection.Close();
    //        string hold = null;
    //        DataTable dt = DtSet.Tables[0];
    //        stateclass a = new stateclass();
    //        if (hf_shiftline.Value == "0")
    //        {
    //            a.sa = id.Value; //period;
    //            a.sb = "1";//dt.Rows[3]["payrollgroupid"].ToString();
    //            a.sc = Session["user_id"].ToString();
    //            idd.Value = bol.loadchangeshift1st(a);
    //        }
    //        foreach (DataRow row in dt.Rows)
    //        {
    //            for (int i = 1; i < dt.Columns.Count; i++)
    //            {
    //                string[] rto = dt.Columns[i].ToString().Split(' ');
    //                int shiftcode = 0;
    //                string ss = "select id from MShiftCode where ShiftCode='" + row[dt.Columns[i].ToString()] + "'";
    //                DataTable qq = dbhelper.getdata(ss);
    //                if (qq.Rows.Count == 0)
    //                    shiftcode = 1;
    //                else
    //                    shiftcode = int.Parse(qq.Rows[0]["id"].ToString());
    //                string idno = "select id from memployee where Idnumber='" + row["IdNumber"] + "'";
    //                DataTable gg = dbhelper.getdata(idno);
    //                string query = "select * from TChangeShiftLine where EmployeeId='" + gg.Rows[0]["id"].ToString() + "' and left(convert(varchar,date,101),10)='" + rto[0] + "' ";
    //                DataTable ff = dbhelper.getdata(query);
    //                if (ff.Rows.Count == 0)
    //                {
    //                    if (gg.Rows.Count > 0)
    //                    {
    //                        hold = "insert into TChangeShiftLine (ChangeshiftId,EmployeeId,Date,ShiftCodeId,Remarks,status) values(" + idd.Value + "," + gg.Rows[0]["id"].ToString() + ",'" + rto[0] + "','" + shiftcode + "','scheduler','for approval')";
    //                        dbhelper.getdata(hold);
    //                    }
    //                }
    //                else
    //                {
    //                    dbhelper.getdata("update TChangeShiftLine set shiftCodeId='" + shiftcode + "' where EmployeeId='" + gg.Rows[0]["id"].ToString() + "' and left(convert(varchar,date,101),10)='" + rto[0] + "' ");
    //                }
    //            }
    //        }

    //        string query1 = "update dtr_period set status='created' where id=" + id.Value;
    //        dbhelper.getdata(query1);
    //        SetLogs("create");
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='dtrp'", true);
    //    //}
    //    //catch (Exception ee)
    //    //{
    //    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('empty file or re-save your excel file'); window.location='dtrp'", true);
    //    //}

    //}

    protected void process(object sender, EventArgs e)
    {
        try
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
            FileUpload1.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();

            string hold = null;
            DataTable dt = DtSet.Tables[0];

            stateclass a = new stateclass();


            if (hf_shiftline.Value == "0")
            {
                a.sa = id.Value; //period;
                a.sb = "1";//dt.Rows[3]["payrollgroupid"].ToString();
                a.sc = Session["user_id"].ToString();
                idd.Value = bol.loadchangeshift1st(a);
            }

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 3; i < dt.Columns.Count; i++)
                {
                    string[] rto = dt.Columns[i].ToString().Split(' ');

                    int shiftcode = 0;
                    string ss = "select id from MShiftCode where ShiftCode='" + row[dt.Columns[i].ToString()] + "'";
                    DataTable qq = dbhelper.getdata(ss);

                    if (qq.Rows.Count == 0)
                        shiftcode = 1;
                    else
                        shiftcode = int.Parse(qq.Rows[0]["id"].ToString());

                    string idno = "select id from memployee where Idnumber='" + row["IdNumber"] + "'";
                    DataTable gg = dbhelper.getdata(idno);

                    string query = "select * from TChangeShiftLine where EmployeeId='" + gg.Rows[0]["id"].ToString() + "' and left(convert(varchar,date,101),10)='" + rto[0] + "' ";
                    DataTable ff = dbhelper.getdata(query);

                    string hh = "select * from TChangeShift where periodId='" + id.Value + "'";
                    DataTable pp = dbhelper.getdata(hh);

                    if (ff.Rows.Count == 0)
                    {
                        if (gg.Rows.Count > 0)
                        {
                            dbhelper.getdata("insert into TChangeShiftLine (ChangeshiftId,EmployeeId,Date,ShiftCodeId,Remarks,status) values(" + pp.Rows[0]["id"].ToString() + "," + gg.Rows[0]["id"].ToString() + ",'" + rto[0] + "','" + shiftcode + "','scheduler','for approval')");

                        }
                    }
                    else
                    {
                        dbhelper.getdata("update TChangeShiftLine set shiftCodeId='" + shiftcode + "' where EmployeeId='" + gg.Rows[0]["id"].ToString() + "' and left(convert(varchar,date,101),10)='" + rto[0] + "' ");
                    }
                }
            }

            string query1 = "update dtr_period set status='created' where id=" + id.Value;
            dbhelper.getdata(query1);
            SetLogs("create");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='dtrp'", true);
        }
        catch (Exception ee)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('empty file or re-save your excel file'); window.location='dtrp'", true);
        }

    }

    public class ListtoDataTableConverter
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values.ToString());

            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

    private void ExporttoExcel(DataTable table)
    {
        //GridView gv = new GridView();
        //gv.AllowPaging = false;
        //gv.DataSource = table;
        //gv.DataBind();

        //Workbook wb = new Workbook();
        //Worksheet sheet = wb.Worksheets[0];
        //sheet.InsertDataTable(table, true, 1, 1);  



        ////sheet.Range["B2"].Text = "Department:";
        ////sheet.Range["B2"].Style.Font.IsBold = true;
        ////sheet.Range["B2"].Style.KnownColor = ExcelColors.Turquoise;

        ////sheet.Range["C2"].DataValidation.Values = new string[] { "Sales", "HR", "R&D", "Finance" };
        ////sheet.Range["C2"].DataValidation.IsSuppressDropDownArrow = false;
        ////sheet.Range["C2"].Style.KnownColor = ExcelColors.LightGreen1;

        ////sheet.Range["C2"].DataValidation.AlertStyle = AlertStyleType.Warning;
        ////sheet.Range["C2"].DataValidation.ShowError = true;
        ////sheet.Range["C2"].DataValidation.ErrorTitle = "Error001";
        ////sheet.Range["C2"].DataValidation.ErrorMessage = "Please select an item from list!";

        ////sheet.Range["B9"].Text = "Input Number:";
        ////sheet.Range["B9"].Style.Font.IsBold = true;
        ////sheet.Range["B9"].Style.KnownColor = ExcelColors.Turquoise;

        ////sheet.Range["C9"].DataValidation.CompareOperator = ValidationComparisonOperator.Between;
        ////sheet.Range["C9"].DataValidation.Formula1 = "1";
        ////sheet.Range["C9"].DataValidation.Formula2 = "10";
        ////sheet.Range["C9"].DataValidation.AllowType = CellDataType.Decimal;
        ////sheet.Range["C9"].Style.KnownColor = ExcelColors.LightGreen1;
        ////sheet.Range["C9"].DataValidation.InputMessage = "Type a number between 1-10 in this cell.";

        ////sheet.SetColumnWidth(2, 18);
    
       // string sFileName = Server.MapPath("~/Exel/Data.xlsx");
       // //wb.SaveToFile(sFileName, ExcelVersion.Version2013);



       //// string FFileName = Server.MapPath("~/Exel/Data_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + Session["emp_id"].ToString() + ".xlsx");
       // string FFileName = Server.MapPath("~/Exel/result.xlsx");
       // Workbook workbook = new Workbook();
       // workbook.LoadFromFile(sFileName);
       // workbook.Worksheets.Remove("Evaluation Warning");
       // workbook.Worksheets[2].Remove();
       // workbook.SaveToFile(FFileName, ExcelVersion.Version2013);
       //// System.Diagnostics.Process.Start(FFileName);




        string sFileName = "Data_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + Session["emp_id"].ToString();

        GridView gv = new GridView();
        gv.AllowPaging = false;
        gv.DataSource = table;
        gv.DataBind();

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(table, "Customers");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {

                gv.HeaderRow.Style.Add("color", "#FFFFFF");

                //Applying stlye to gridview header cells
                for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
                {
                    //Change to Blue
                    gv.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");

                }
                int j = 1;

                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in gv.Rows)
                {
                    gvrow.BackColor = Color.White;

                    if (j <= gv.Rows.Count)
                    {
                        for (int k = 0; k < gvrow.Cells.Count; k++)
                        {
                            gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                            gvrow.Cells[k].Attributes.Add("style", "mso-number-format:\\@");

                        }
                    }
                    j++;
                }
                //gv.RenderControl(htw);


                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }


       // Response.Clear();
       // Response.ClearContent();
       // Response.ClearHeaders();
       // Response.Buffer = true;
       //// Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", sFileName));
       // Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xls");
       // Response.Charset = "";
       // Response.ContentType = "application/vnd.ms-excel";
       // //Response.ContentType = "application/vnd.openxmlformats";
       // StringWriter sw = new StringWriter();
       // HtmlTextWriter htw = new HtmlTextWriter(sw);
       // //Change the Header Row Foreground to white color
       // gv.HeaderRow.Style.Add("color", "#FFFFFF");

       // //Applying stlye to gridview header cells
       // for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
       // {
       //     //Change to Blue
       //     gv.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");

       // }
       // int j = 1;

       // //This loop is used to apply stlye to cells based on particular row
       // foreach (GridViewRow gvrow in gv.Rows)
       // {
       //     gvrow.BackColor = Color.White;

       //     if (j <= gv.Rows.Count)
       //     {
       //         //if (j % 2 != 0)
       //         //{
       //         for (int k = 0; k < gvrow.Cells.Count; k++)
       //         {
       //             gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
       //             gvrow.Cells[k].Attributes.Add("style", "mso-number-format:\\@");

       //         }
       //         //}
       //     }
       //     j++;
       // }
       // gv.RenderControl(htw);
       // Response.Write(sw.ToString());
       // Response.Flush();
       // Response.End();


        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.ClearContent();
        //HttpContext.Current.Response.ClearHeaders();
        //HttpContext.Current.Response.Buffer = true;
        //HttpContext.Current.Response.ContentType = "application/ms-excel";
        //HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

        //HttpContext.Current.Response.Charset = "utf-8";
        //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        //sets font
        //HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        //HttpContext.Current.Response.Write("<BR><BR><BR>");
        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        //HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
        //  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
        //  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
        //am getting my grid's column headers
        //int columnscount = table.Columns.Count;

        //for (int j = 0; j < columnscount; j++)
        //{      //write in new column
        //    HttpContext.Current.Response.Write("<Td>");
        //    Get column headers  and make it as bold in excel columns
        //    HttpContext.Current.Response.Write("<B>");
        //    HttpContext.Current.Response.Write(table.Columns[j].ToString());

        //    HttpContext.Current.Response.Write("</B>");
        //    HttpContext.Current.Response.Write("</Td>");
        //}
        //HttpContext.Current.Response.Write("</TR>");
        //foreach (DataRow row in table.Rows)
        //{//write in new row
        //    HttpContext.Current.Response.Write("<TR>");
        //    for (int i = 0; i < table.Columns.Count; i++)
        //    {
        //        HttpContext.Current.Response.Write("<Td mso-number-format:\\@>");
        //        HttpContext.Current.Response.Write(row[i].ToString());
        //        HttpContext.Current.Response.Write("</Td>");
        //    }

        //    HttpContext.Current.Response.Write("</TR>");
        //}
        //HttpContext.Current.Response.Write("</Table>");
        //HttpContext.Current.Response.Write("</font>");
        //HttpContext.Current.Response.Flush();
        //HttpContext.Current.Response.End();

    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["data"];
        dt.Columns.Remove("idd");
        //dt.Columns.Remove("name");
        dt.Columns.Remove("payrollgroupid");
        
        ExporttoExcel(dt);

           
       
    }

}