 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Drawing;

public partial class content_Employee_leave : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            bind();
            grid_leave.DataBind();
            disp();
            loadable();
        }
    }
    protected void bind()
    {
        DataTable final_disp = new DataTable();
        final_disp.Columns.Add(new DataColumn("cnt", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Leaveid", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Leave", typeof(string)));
        final_disp.Columns.Add(new DataColumn("date", typeof(string)));
        final_disp.Columns.Add(new DataColumn("noh", typeof(string)));
        final_disp.Columns.Add(new DataColumn("pay", typeof(string)));
        final_disp.Columns.Add(new DataColumn("expectedin", typeof(string)));
        ViewState["bind"] = final_disp;

    }
    protected void loadable()
    {
        string query = "select * from MLeave where action is null order by Leave asc";
        DataTable dt = dbhelper.getdata(query);
        ddl_leave.Items.Clear();
        ddl_leave.Items.Add(new ListItem("", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_leave.Items.Add(new ListItem(dr["Leave"].ToString(), dr["id"].ToString()));
        }
    }
    protected static DataTable chk_leave_credits(string empid, string leaveid, string yearer)
    {
        DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

        if (dtt.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(empid, yearer, " and a.leaveid=" + leaveid + "").Split(new string[] { "UNION" }, StringSplitOptions.None);

            DataTable dpres = dbhelper.getdata(twos[0]);
            DataTable dprev = dbhelper.getdata(twos[1]);

            for (int i = 0; i < dpres.Rows.Count; i++)
            {
                double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                double diff = asd <= 0 ? 0 : asd;
                if (diff > 5)
                {
                    diff = 5;
                }
                for (int j = 0; j < dprev.Rows.Count; j++)
                {
                    if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                    {
                        dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";
                          
                    }
                }
            }

            dpres.Merge(dprev);

            return dpres;
        }
        else
        {
            string query = SetUp.leavetype(empid, yearer, " and a.leaveid=" + leaveid + "");
            DataTable dt = dbhelper.getdata(query);
            return dt;
        }

    }
    protected void disp()
    {
        DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

        if (dtt.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(Session["emp_id"].ToString(), DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

            DataTable dpres = dbhelper.getdata(twos[0]);
            DataTable dprev = dbhelper.getdata(twos[1]);

            for (int i = 0; i < dpres.Rows.Count; i++)
            {
                double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                double diff = asd <= 0 ? 0 : asd;
                if (diff > 5)
                {
                    diff = 5;
                }
                for (int j = 0; j < dprev.Rows.Count; j++)
                {
                    if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                    {
                        dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";
                          
                    }
                }
            }

            dpres.Merge(dprev);

            grid_view.DataSource = dpres;
            grid_view.DataBind();
        }
        else
        {
            query.Value = SetUp.leavetype(Session["emp_id"].ToString(), DateTime.Now.Year.ToString(), "");
            DataSet ds = new DataSet();
            ds = bol.display(query.Value);
            grid_view.DataSource = ds.Tables["table"];
            grid_view.DataBind();
        }


    }
    protected void click_emp(object sender, EventArgs e)
    {
                DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

                if (dtt.Rows[0]["LeaveType"].ToString() == "4")
                {
                    string[] twos = SetUp.leavetype(Session["emp_id"].ToString(), DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

                    DataTable dpres = dbhelper.getdata(twos[0]);
                    DataTable dprev = dbhelper.getdata(twos[1]);

                    for (int i = 0; i < dpres.Rows.Count; i++)
                    {
                        double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                        double diff = asd <= 0 ? 0 : asd;
                        if (diff > 5)
                        {
                            diff = 5;
                        }
                        for (int j = 0; j < dprev.Rows.Count; j++)
                        {
                            if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                            {
                                dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";
                          
                            }
                        }
                    }

                    dpres.Merge(dprev);
                    grid_view.DataSource = dpres;
                    grid_view.DataBind();
                }
                else
                {
                    string query = SetUp.leavetype(Session["emp_id"].ToString(), DateTime.Now.Year.ToString(), "");
                    DataTable dt = dbhelper.getdata(query);
                    grid_view.DataSource = dt;
                    grid_view.DataBind();
                }
    }
    protected void click_veirfy(object sender, EventArgs e)
    {

        if (ddl_leave.SelectedValue == "13")
        {
            DataTable dt = dbhelper.getdata("select * from memployee where emp_status='2' and id='" + Session["emp_id"].ToString() + "' ");

            if (dt.Rows.Count == 0)
                lbl_error_msg.Text = "You can use your Paid Leave after 6 months";
            else
                loslos();
        }
        else
            loslos();
    }

    protected void loslos()
    {
        if (checker())
        {
            DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

            string month;
            string day;
            string[] f_datef;
            DateTime datef = Convert.ToDateTime(this.txt_from.Text);
            DataTable final_disp = ViewState["bind"] as DataTable;
            DataRow final_dr;
            string noh = rb_range.SelectedValue;
            decimal get_t_noh = 0;
            decimal bal = 0, balprev = 0, balsec = 0;
            decimal balnext = 0;
            decimal draftbal = 0;
            DataTable dtprev = chk_leave_credits(Session["emp_id"].ToString(), ddl_leave.SelectedValue, DateTime.Now.AddYears(-1).Year.ToString());
            DataTable dt = chk_leave_credits(Session["emp_id"].ToString(), ddl_leave.SelectedValue, DateTime.Now.Year.ToString());
            DataTable dtnext = chk_leave_credits(Session["emp_id"].ToString(), ddl_leave.SelectedValue, DateTime.Now.AddYears(1).Year.ToString());
            balprev = dtprev.Rows.Count > 0 ? decimal.Parse(dtprev.Rows[0]["balance"].ToString()) : 0;
            bal = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[0]["balance"].ToString()) : 0;
            if (dt.Rows.Count > 1)
            {
                balsec = dt.Rows.Count > 0 ? decimal.Parse(dt.Rows[1]["balance"].ToString()) : 0;
            }

            balnext = dtnext.Rows.Count > 0 ? decimal.Parse(dtnext.Rows[0]["balance"].ToString()) : 0;

            TimeSpan nod = DateTime.Parse(txt_from.Text) - DateTime.Parse(txt_to.Text);
            string nodformat = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0}", nod.Days, nod.Hours, nod.Minutes, nod.Seconds).Replace("-", "");
            for (int i = 0; i <= int.Parse(nodformat); i++)
            {
                string[] getval = txt_from.Text.Trim().Split('/');
                f_datef = datef.AddDays(i).ToString().Replace(" 12:00:00 AM", "").Split('/');
                month = f_datef[0].Length > 1 ? f_datef[0] : "0" + f_datef[0];
                day = f_datef[1].Length > 1 ? f_datef[1] : "0" + f_datef[1];
                string pay = dt.Rows.Count == 0 ? "No" : bal > 0 ? bal >= decimal.Parse(noh) ? "Yes" : "No" : "No"; // 1st   //try1

                if (dtt.Rows[0]["LeaveType"].ToString() == "4")
                {
                    pay = dt.Rows.Count == 0 ? "No" : bal > 0 ? bal >= decimal.Parse(noh) ? "Yes" : "No" : "No";//try2
                    if (pay == "No")
                    {
                        pay = dt.Rows.Count == 0 ? "No" : balsec > 0 ? balsec >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                    }
                }
                else
                {
                    if (f_datef[2] == DateTime.Now.Year.ToString())
                    {
                        pay = dt.Rows.Count == 0 ? "False" : bal > 0 ? bal >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                    }
                    else if (f_datef[2] == DateTime.Now.AddYears(1).Year.ToString())
                    {
                        if (dtnext.Rows.Count > 0)
                        {
                            pay = dtnext.Rows.Count == 0 ? "False" : balnext > 0 ? balnext >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                        }
                    }
                    else if (f_datef[2] == DateTime.Now.AddYears(-1).Year.ToString())
                    {
                        if (dtprev.Rows.Count > 0)
                        {
                            pay = dtprev.Rows.Count == 0 ? "False" : balprev > 0 ? balprev >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                        }
                    }
                }
                if (chekingleave(Session["emp_id"].ToString(), Session["emp_id"].ToString(), month + "/" + day + "/" + f_datef[2], radiobtnlist.SelectedValue, noh))
                {
                    string expin = rb_range.SelectedValue == "0.5" ? radiobtnlist.SelectedValue : "0";
                    bal = bal - decimal.Parse(noh); //2nd
                    if (bal <= 0)
                    {
                        balsec = balsec - decimal.Parse(noh);
                    }
                    balnext = balnext - decimal.Parse(noh);
                    balprev = balprev - decimal.Parse(noh);
                    DataRow[] verified = final_disp.Select(" date='" + month + "/" + day + "/" + f_datef[2] + "'");
                    if (verified.Count() == 0)
                    {
                        final_dr = final_disp.NewRow();
                        final_dr["cnt"] = final_disp.Rows.Count + 1;
                        final_dr["Leaveid"] = ddl_leave.SelectedValue;
                        final_dr["Leave"] = ddl_leave.SelectedItem.Text;
                        final_dr["date"] = month + "/" + day + "/" + f_datef[2];
                        final_dr["noh"] = noh;
                        final_dr["pay"] = pay.ToString();
                        final_dr["expectedin"] = expin;
                        final_disp.Rows.Add(final_dr);
                        get_t_noh = get_t_noh + decimal.Parse(noh);
                    }
                    Response.Write("<script>alert('To continue this process please click Save thank you!')</script>");
                }
            }
            ViewState["bind"] = final_disp;
            ViewState["leavedash"] = final_disp;
            grid_leave.DataSource = final_disp;
            grid_leave.DataBind();
            bind();
        }
    }
    protected void tawesie(object seder, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[4].Text == "No")
            {
                e.Row.Cells[4].ForeColor = Color.Red;
            }
        }
    }
    protected void delete_tran(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent)
        {
            DataTable dt = ViewState["leavedash"] as DataTable;
            dt.Rows[row.RowIndex].Delete();
            ViewState["bind"] = dt;
            ViewState["leavedash"] = dt;
            grid_leave.DataSource = dt;
            grid_leave.DataBind();

            //dbhelper.getdata("update Tleave set action='deleted' where id='" + row.Cells[0].Text + "'");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='KOISK_LEAVE'", true);
        }
    }

    protected void datatbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (decimal.Parse(e.Row.Cells[4].Text) >0)
                e.Row.Cells[5].Text = "True";
            else
                e.Row.Cells[5].Text = "False";
        }
    }

    protected void click_save(object sender, EventArgs e)
    {
        /**BETA 110519
           * ADD EMAIL NOTIFICATION
           * **/
        if (grid_leave.Rows.Count > 0)
        {
            string iswithpay = "";
            stateclass a = new stateclass();
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
            a.sa = txt_delegate.Text.Replace("'", "");
            a.sb = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
            string x = bol.leave(a);
            string newName1 = null;
            string filepath = Server.MapPath("files/leave/" + (int.Parse(x)) + "/");
            DirectoryInfo di = Directory.CreateDirectory(filepath);
            HttpFileCollection uploadedFiles = Request.Files;
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];
                if (userPostedFile.ContentLength > 0)
                {
                    string extention = Path.GetExtension(file_img.FileName).Substring(1);
                    newName1 = filepath + "\\" + Path.GetFileName(x);
                    userPostedFile.SaveAs(filepath + "\\" + Path.GetFileName(int.Parse(x) + "." + extention));
                    dbhelper.getdata("insert into leave_image values (" + x + ",'" + int.Parse(x) + "." + extention + "',NULL)");
                }
            }
            DataTable dt = ViewState["leavedash"] as DataTable;
            string useridd = Session["user_id"].ToString();
            DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
            foreach (DataRow gr in dt.Rows)
            {
                string aa = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
                if (gr["pay"].ToString() == "No")
                {
                    iswithpay = "False";
                }
                else
                {
                    iswithpay = "True";
                }

                decimal amt_to_paid = 0;
                amt_to_paid = gr["pay"].ToString() == "True" ? decimal.Parse(gr["noh"].ToString()) * decimal.Parse(dtemp.Rows[0]["FixNumberOfHours"].ToString()) * decimal.Parse(dtemp.Rows[0]["HourlyRate"].ToString()) : 0;
                query.Value = "insert into TLeaveApplicationLine " +
                        "(EmployeeId,LeaveId,Date,NumberOfHours,WithPay,Remarks,status,sysdate,l_id,setupbasichrs,dailyrate,amount_to_be_paid,inoutduringhalfdayleave) " +
                        "values " +
                        "(" + Session["emp_id"].ToString() + "," + gr["Leaveid"] + ",'" + gr["date"] + "','" + gr["noh"] + "','" + iswithpay + "','" + txt_lineremarks.Text.Replace("'", "") + "','" + aa + "',getdate()," + x + ",'" + dtemp.Rows[0]["FixNumberOfHours"].ToString() + "','" + dtemp.Rows[0]["DailyRate"].ToString() + "','" + amt_to_paid + "'," + gr["expectedin"] + ")";
                dbhelper.getdata(query.Value);
            }

            //DataTable dtapprover = dbhelper.getdata("select b.EmailAddress from approver a left join memployee b on a.under_id=b.Id where a.emp_id=" + Session["emp_id"].ToString() + " and a.status is null");
            //foreach (DataRow dr in dtapprover.Rows)
            //{
            //    string msg = message.newmsg(getdata.fitchdata(x.ToString()), x.ToString());
            //    sendmail.emailsender(msg, getdata.fitchdata(x.ToString()), "'" + dr["EmailAddress"] + "'", "Leave Application", "");
            //}
            //sendmail.emailsender(msg, getdata.fitchdata(x.ToString()), "meshnetwork101@gmail.com", "Leave Application", "marlon.salaw@gmail.com,mymeshnetworks@gmail.com");
            //function.AddNotification("Leave Approval", "al", approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_userid"].ToString(), x);

            string approver = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
            function.Notification("leave", "for approval", x, approver);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='KOISK_LEAVE';window.open('','_new').location.href='leaveform?key=" + function.Encrypt(x, true) + "'", true);
            Response.Redirect("KOISK_LEAVE");
        }
        else
        {
            Response.Write("<script>alert('No Data Found Please Verify First')</script>");
        }
    }
    protected bool chekingleave(string emp_id, string leave_id, string date_leave, string designation, string noh)
    {
        bool err = true;
        DataTable dtemp = dbhelper.getdata("select * from memployee where Id='" + emp_id + "'");

        if (rb_range.SelectedValue == "1")
        {
            DataTable dt = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "' and (NumberOfHours = '0.50' or NumberOfHours = '1.00') and (status like'%For Approval%' or status like'%Approved%' or status like'%verification%' )");
            if (dt.Rows.Count > 0)
            {
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " is already exist! <br/>";
                err = false;
            }
        }
        else
        {
            string query = "select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "' and NumberOfHours = '1.00' and (status like'%For Approval%' or status like'%Approved%' or status like'%verification%' )";
            DataTable dt = dbhelper.getdata(query);
            if (dt.Rows.Count > 0)
            {
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " is already exist! <br/>";
                err = false;
            }

            DataTable dtdaytype = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "' and NumberOfHours='" + noh + "' and inoutduringhalfdayleave = '" + designation + "' and (status like'%For Approval%' or status like'%Approved%' or status like'%verification%' )");
            if (dtdaytype.Rows.Count > 0)
            {
                string daytype = designation == "1" ? "AM" : "PM";
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " " + daytype + " is already exist! <br/>";
                err = false;
            }
        }

        DataTable dtrest = dbhelper.getdata("select * from MShiftCodeDay where shiftcodeid='" + dtemp.Rows[0]["shiftcodeid"].ToString() + "' and restday='True' and status is null");
        foreach (DataRow drest in dtrest.Rows)
        {
            string shift = Convert.ToDateTime(date_leave).DayOfWeek.ToString();
            string hdd = dtrest.Rows.Count > 0 ? drest["Day"].ToString() : "empty";
            if (shift == hdd)
            {
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " is a Restday! <br/>";
                err = false;
            }
        }
        DataTable dthol = dbhelper.getdata("select * from MDayTypeDay where branchid='" + dtemp.Rows[0]["branchid"].ToString() + "' and date='" + date_leave + "' and status is null");
        //DataTable dtdtrprev = dbhelper.getdata("select * from TDTRLine a left join tdtr b on a.DTRId=b.Id where b.status is null and EmployeeId=" + Session["emp_id"].ToString() + " and CONVERT(date,a.Date)=CONVERT(date,'" + month + "/" + day + "/" + f_datef[2] + "')  and a.Absent='True'");
        DataTable dtdtrprev = dbhelper.getdata("select * from TDTRLine a left join tdtr b on a.DTRId=b.Id where b.status is null and EmployeeId=" + Session["emp_id"].ToString() + " and CONVERT(date,a.Date)=CONVERT(date,'" + date_leave + "') and b.PayrollGroupId=" + dtemp.Rows[0]["payrollgroupid"].ToString() + " and b.payroll_id is null");
        
        if (dthol.Rows.Count > 0)
        {
            lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " is a Holliday! <br/>";
            err = false;
        }

       
        

        //else if (dtdtrprev.Rows.Count > 0)
        //{
        //    lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + "Date " + date_leave + " is not allowed for leave filling! <br/>";
        //    err = false;
        //}
        //else
        //{
        //    lbl_error_msg.Text = "";
        //}
        return err;
    }
    protected bool checker()
    {
        bool oi = true;

        if (rb_range.SelectedValue.Length == 0)
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Input Number of Hours";
            goto exit;
        }
        else
        {
            if (rb_range.SelectedValue == "0.5")
            {
                if (radiobtnlist.SelectedValue.Length == 0)
                {
                    oi = false;
                    lbl_error_msg.Text = "Invalid Input for Expected Time In";
                    goto exit;
                }
            }
        }

        if (ddl_leave.SelectedItem.ToString() == "")
        {
            oi = false;
            lbl_error_msg.Text = "Please select leave type";
            goto exit;
        }
        if (txt_lineremarks.Text == "")
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Remarks";
            goto exit;
        }
        else if (txt_from.Text.Length < 10)
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Date From";
            goto exit;
        }
        else if (txt_to.Text.Length < 10)
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Date To";
            goto exit;
        }
        else
            lbl_error_msg.Text = "";

        exit:
        return oi;

    }
    protected void select_nod(object sender, EventArgs e)
    {
        radiobtnlist.SelectedValue = null;
        if (rb_range.SelectedValue == "0.5")
            startin.Style.Add("display","block");
        else
            startin.Style.Add("display", "none");
    }
}