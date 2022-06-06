using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Human;

public partial class content_hr_attendance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            Loadable();
         
    }

    protected void Loadable()
    {
        string query = "select * from MPayrollGroup order by PayrollGroup";
        DataTable dt = dbhelper.getdata(query);
        ddl_payrollgroup.Items.Add(new ListItem("-", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payrollgroup.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select id ,left(convert(varchar,dfrom,101),10)+' - '+left(convert(varchar,dtoo,101),10) dtrrange from payroll_range where action is null order by dfrom desc";
        dt = dbhelper.getdata(query);
        ddl_period.Items.Add(new ListItem("-", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_period.Items.Add(new ListItem(dr["dtrrange"].ToString(), dr["id"].ToString()));
        }
    }

    protected void read(string key)
    {
        function.ReadNotification(Request.QueryString["nt"].ToString());

        DataTable dt = dbhelper.getdata(bind("id=" + key));
        id.Value = key;
        hf_shiftline.Value = dt.Rows[0]["cnt"].ToString();
        hf_status.Value = dt.Rows[0]["status"].ToString();

        main.Visible = false;


        //switch (dt.Rows[0]["status"].ToString())
        //{
        //    case "approved":
        //        p_footer.Visible = false;
        //        break;
        //    default:
        //        p_footer.Visible = true;
        //        break;
        //}



        Schedule();
    }

    protected void schedule_rowbound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            
            e.Row.Cells[0].Width = Unit.Pixel(500); //DATE
            e.Row.Cells[1].Width = Unit.Pixel(100); //CUSTOMER
            e.Row.Cells[2].Width = Unit.Pixel(100); //DESTINATION


        }


        e.Row.Cells[0].Visible = false;

        //e.Row.Cells[2].Visible = false;
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
            modal_submit.Style.Add("display", "block");
        }
    }

    protected void click_submitschedule(object sender, EventArgs e)
    {
        SetLogs("submit");
        function.AddNotification("Schedule Approval", "schedule-viewer", "0", id.Value);//0 basta admin | change ang url sa sakto
        dbhelper.getdata("update dtr_period set status='for approval' where id=" + id.Value);
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

        //dtper = dbhelper.getdata("select * from dtr_period where id=" + id.Value + "");
        int ii = 0;
        foreach (DataRow dr in dt.Rows)
        {
            int j = 2;
            DataTable range = (DataTable)ViewState["range"];
            foreach (DataRow date in range.Rows)
            {
                string ddl_id = dr["ID"] + "_" + date["ddate"].ToString().Replace("/", "_") + date["mdate"].ToString();
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

        string query = "update dtr_period set status='approved' where id=" + id.Value;
        dbhelper.getdata(query);
        SetLogs("create");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='dtrp'", true);
    }

    protected void click_back(object sender, EventArgs e)
    {
        Response.Redirect("attendance");
        main.Visible = true;
    }

    protected void OnChangeFilter(object sender, EventArgs e)
    {
        Schedule();
    }

    protected void click_set(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnkstat = (LinkButton)row.FindControl("lb_status");

            id.Value = row.Cells[0].Text;
            hf_shiftline.Value = row.Cells[1].Text;
            hf_status.Value = row.Cells[4].Text;
            hf_start.Value = row.Cells[5].Text;
            hf_end.Value = row.Cells[6].Text;
            hf_dtrid.Value = row.Cells[7].Text;

            main.Visible = false;
 
            

            Schedule();
        }
    }

    protected void Schedule()
    {
        if (ddl_payrollgroup.SelectedIndex > 0 && ddl_period.SelectedIndex > 0)
        {
            string[] period = ddl_period.SelectedItem.ToString().Split('-');
            DataTable dtr = Core.DTRF(period[0], period[1], ddl_payrollgroup.SelectedValue);

            string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                        "c.ShiftCode, " +
                        "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId,a.id as tid, " +
                        "d.IdNumber as ID " +
                        "from TChangeShift a " +
                        "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                        "left join MShiftCode c on b.ShiftCodeId=c.id " +
                        "left join memployee d on b.EmployeeId=d.Id " +
                        "where a.PeriodId='" + id.Value + "' and a.EntryUserId='" + Session["user_id"].ToString() + "' and d.departmentid<>'3' and d.PayrollGroupId<>'4' ";



            tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , c.ShiftCode, b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId,a.id as tid, d.IdNumber as ID " +
            "from TChangeShift a " +
            "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
            "left join MShiftCode c on b.ShiftCodeId=c.id " +
            "left join memployee d on b.EmployeeId=d.Id " +
            "left join Approver e on  d.Id= e.emp_id " +
            "where " +
            "CONVERT(date,b.date) between CONVERT(date,'" + period[0] + "') and CONVERT(date,'" + period[1] + "')" +
            "and d.PayrollGroupId = "+ ddl_payrollgroup.SelectedValue  +" " +
            "order by b.[date]";

            DataTable hh = dbhelper.getdata(tt);
            ViewState["ttt"] = hh;

            string query = "select id idd, IdNumber ID,FirstName +' '+ LastName Name, PayrollGroupId " +
            "from MEmployee " +
            "where PayrollGroupId = '" + ddl_payrollgroup.SelectedValue + "'";
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

                    if (hh.Rows.Count > 0)
                    {
                         if (dt.Rows[row][0].ToString() == "238")
                         { }
                        string hold = null;
                        string[] colname = dt.Columns[col].ToString().Split(' ');

                        DataRow[] dr = hh.Select("employeeID='" + dt.Rows[row][0].ToString() + "' and date='" + ddt.Rows[col - 4][0].ToString() + "'");

                        if (dr.Count() > 0)
                        {
                            string y = ddt.Rows[col - 4][0].ToString().Substring(0, 1) == "0" ? ddt.Rows[col - 4][0].ToString().Substring(1) : ddt.Rows[col - 4][0].ToString();
                            DataRow[] result = dtr.Select("empid = " + dt.Rows[row][0].ToString() + " and date like '%" + y + "%'");

                            hold = dr[0][3].ToString();
                            ddl.SelectedValue = dr[0][3].ToString();

                         
                            //if (hf_status.Value == "approved")
                            //    ddl.Enabled = false;

                            if (hf_dtrid.Value.Replace("&nbsp;", "") != "")
                                ddl.Enabled = false;
                            else
                                ddl.Enabled = true;

                            if (result.Count() > 0)
                            {
                                DataTable fitchDTR = result.CopyToDataTable();
                                float reg_hr = float.Parse(fitchDTR.Rows[0]["reg_hr"].ToString());
                                float night = float.Parse(fitchDTR.Rows[0]["night"].ToString());
                                float offsethrs = float.Parse(fitchDTR.Rows[0]["offsethrs"].ToString());
                                float totalhrs = reg_hr + night + offsethrs;
                                float fixhour = float.Parse(fitchDTR.Rows[0]["FixNumberOfHours"].ToString());

                                if(totalhrs < fixhour)
                                    gv_schedule.Rows[row].Cells[col].Attributes.Add("class", "irregular");
                            }
                        }

                    }
                
                    gv_schedule.Rows[row].Cells[col].Controls.Add(ddl);


                }
            }


            grid_alert.Visible = gv_schedule.Rows.Count == 0 ? true : false;

        }
    }

    protected void click_modalsave(object sender, EventArgs e)
    {
        if (checkfield())
        {
            if (hf_action.Value == "0")
            {

                //DataTable dt = dbhelper.getdata("select top 1 * from dtr_period where status is null and left(convert(varchar(30),'" + txt_from.Text + "',120),10) between left(convert(varchar(30),'" + txt_to.Text + "',120),10) and date_end");
                // DataTable dt = dbhelper.getdata("SELECT * FROM dtr_period WHERE (date_start <='" + txt_from.Text + "' and date_end >='" + txt_to.Text + "')");

                if (DateTime.Parse(txt_to.Text) <= DateTime.Parse(txt_from.Text))
                    setAlert(true, "danger", "Period is invalid");
                else
                {
                    DataTable dt = dbhelper.getdata("SELECT  * " +
                                                    "FROM dtr_period " +
                                                    "WHERE emp_id='" + Session["emp_id"].ToString() + "' and date_start <= '" + txt_to.Text + "' AND status not like '%delete%' and (date_end >= '" + txt_from.Text + "' OR (date_end IS NULL AND GETDATE() >= '" + txt_from.Text + "'))");
                    if (dt.Rows.Count > 0)
                        setAlert(true, "danger", "Period is invalid");
                    else
                    {
                        setAlert(false);
                        dbhelper.getdata("insert into dtr_period (date_input,date_start,date_end,remarks,status,emp_id) values(getdate(),'" + txt_from.Text.Trim() + "','" + txt_to.Text.Trim() + "','" + txt_remarks.Text.Trim() + "','new','" + Session["emp_id"].ToString() + "')");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved'); window.location='dtrp'", true);
                    }
                }
            }
            else
            {
                string query = "update dtr_period set status='" + "deleted-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id + " ";
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='dtrp'", true);
            }
        }
    }

    //protected void click_modalsave(object sender, EventArgs e)
    //{
    //    if (checkfield())
    //    {
    //        if (hf_action.Value == "0")
    //        {

    //            DataTable dt = dbhelper.getdata("select top 1 * from dtr_period where status is null and CONVERT (datetime,'" + txt_from.Text + "') between  CONVERT(datetime,'2017-12-30 00:00:00.000') and date_end");
    //            if (dt.Rows.Count > 0)
    //                setAlert(true, "danger", "Period is invalid");
    //            else
    //            {
    //                setAlert(false);
    //                dbhelper.getdata("insert into dtr_period (date_input,date_start,date_end,remarks,status,emp_id) values(getdate(),'" + txt_from.Text.Trim() + "','" + txt_to.Text.Trim() + "','" + txt_remarks.Text.Trim() + "','new','" + Session["emp_id"].ToString() + "')");
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved'); window.location='dtrp'", true);
    //            }

    //        }
    //        else
    //        {
    //            string query = "update dtr_period set status='" + "deleted-" + Session["emp_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id + " ";
    //            dbhelper.getdata(query);
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='dtrp'", true);
    //        }
    //    }
    //}

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
        "from dtr_period a where " + condition + " order by Id desc";
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

    protected void add(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        modal.Style.Add("display", "block");
        p_create.Visible = true;
        p_delete.Visible = false;
    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("dtrp", false);
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
                        lb_status.CssClass = "label label-primary pull-right";
                        break;

                }
            }

            LinkButton lnkstat = (LinkButton)e.Row.FindControl("lb_status");
            hf_dtrid.Value = e.Row.Cells[7].Text;
            if (hf_dtrid.Value.Replace("&nbsp;", "") != "")
            {
                lnkstat.Visible = true;
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
}