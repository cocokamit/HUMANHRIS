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
using System.Data.SqlClient;
using System.Web.Services;

public partial class content_Employee_Adminsideleave : System.Web.UI.Page
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
            getLeaves();
        }
        loaddelegate();
        modal.Style.Add("display", "none");
        user_id.Value = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);
    }
    protected void loaddelegate()
    {
        hfdelegates.Value = "";
        DataTable dt = dbhelper.getdata("Select *,(Select lastname+','+firstname from Memployee where id= a.realApprover) fullname from approval_OIC a where [status]=0 and oic=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + " or realApprover=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + "");

        foreach (DataRow row in dt.Rows)
        {
            foreach (DateTime day in EachDay(Convert.ToDateTime(row["tDateStart"].ToString()), Convert.ToDateTime(row["tDateEnd"].ToString())))
            {
                hfdelegates.Value += hfdelegates.Value + day.ToString("d-M-yyyy") + ",";
            }
        }
        DataRow[] dr = dt.Select("oic=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + "");
        if (dr.Count() > 0)
        {
            grid_forms.DataSource = dt;
            grid_forms.DataBind();
        }
        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;
        btndelagates.Visible = grid_forms.Rows.Count == 0 ? false : true;

    }
    public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }

    protected void modalclick(object sender, EventArgs e)
    {
        modal.Style.Add("display", "block");
    }

    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
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
        foreach (DataRow dr in dt.Rows)
        {
            ddl_leave.Items.Add(new ListItem(dr["Leave"].ToString(), dr["id"].ToString()));
        }

        string empID = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);
        dt = getdata.ActiveEmployee("a.level <> 3");
        DataRow[] results = dt.Select("empID=" + empID);

        if (results.Length > 0)
        {
            ddlOIC.Visible = true;
            ddlOIC.Items.Clear();
            ddlOIC.Items.Add(new ListItem("", "0"));

            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["empID"].ToString() != empID)
                {
                    ddlOIC.Items.Add(new ListItem(dr["employee"].ToString(), dr["empID"].ToString()));
                    DropDownList1.Items.Add(new ListItem(dr["employee"].ToString(), dr["empID"].ToString()));
                }
            }
        }
        else
            txt_delegate.Visible = true;
    }

    [WebMethod]
    public static string GetInfo(string from, string to, string id)
    {
        string info = "";
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id empID, a.FirstName + ' ' + a.LastName employee, * from  approval_OIC b Left Join memployee a on a.Id=b.realApprover where PayrollGroupId <> 4 and a.level <> 3 and b.status=0 and ((('" + from + "' between b.tDateStart and tDateEnd) and ('" + to + "' between b.tDateStart and tDateEnd)) or ((b.tDateStart between '" + from + "' and '" + to + "') or (b.tDateEnd between '" + from + "' and '" + to + "'))) order by a.FirstName ");
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    info += reader["oic"].ToString() + ",";
                }
            }
            con.Close();
        }
        return info;
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
            string[] twos = SetUp.leavetype(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

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
            query.Value = SetUp.leavetype(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), DateTime.Now.Year.ToString(), "");
            DataSet ds = new DataSet();
            ds = bol.display(query.Value);
            grid_view.DataSource = ds.Tables["table"];
            grid_view.DataBind();
        }

        DataTable dtemp = dbhelper.getdata("select * from MEmployee where Id = '" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + "'");
        lblempname.Text = " " + (dtemp.Rows[0]["LastName"].ToString() + ", " + dtemp.Rows[0]["FirstName"].ToString());
        lblempname.ForeColor = System.Drawing.Color.Blue;

        pnlsverifying.Visible = false;
    }
    protected void click_emp(object sender, EventArgs e)
    {
        DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

        if (dtt.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

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
            string query = SetUp.leavetype(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), DateTime.Now.Year.ToString(), "");
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
            DataTable dtprev = chk_leave_credits(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), ddl_leave.SelectedValue, DateTime.Now.AddYears(-1).Year.ToString());
            DataTable dt = chk_leave_credits(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), ddl_leave.SelectedValue, DateTime.Now.Year.ToString());
            DataTable dtnext = chk_leave_credits(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), ddl_leave.SelectedValue, DateTime.Now.AddYears(1).Year.ToString());
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
                string pay = dt.Rows.Count == 0 ? "No" : bal > 0 ? bal >= decimal.Parse(noh) ? "Yes" : "No" : "No";

                if (dtt.Rows[0]["LeaveType"].ToString() == "4")
                {
                    pay = dt.Rows.Count == 0 ? "No" : bal > 0 ? bal >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                    if (pay == "No")
                    {
                        pay = dt.Rows.Count == 0 ? "No" : balsec > 0 ? balsec >= decimal.Parse(noh) ? "Yes" : "No" : "No";
                    }
                }
                else
                {
                    if (f_datef[2] == DateTime.Now.Year.ToString())
                    {
                        pay = dt.Rows.Count == 0 ? "False" : bal > 0 ? bal >= decimal.Parse(noh) ? "True" : "False" : "False";
                    }
                    else if (f_datef[2] == DateTime.Now.AddYears(1).Year.ToString())
                    {
                        if (dtnext.Rows.Count > 0)
                        {
                            pay = dtnext.Rows.Count == 0 ? "False" : balnext > 0 ? balnext >= decimal.Parse(noh) ? "True" : "False" : "False";
                        }
                    }
                    else if (f_datef[2] == DateTime.Now.AddYears(-1).Year.ToString())
                    {
                        if (dtprev.Rows.Count > 0)
                        {
                            pay = dtprev.Rows.Count == 0 ? "False" : balprev > 0 ? balprev >= decimal.Parse(noh) ? "True" : "False" : "False";
                        }
                    }
                }
                if (chekingleave(function.Decrypt(Request.QueryString["emp_id"].ToString(), true), function.Decrypt(Request.QueryString["emp_id"].ToString(), true), month + "/" + day + "/" + f_datef[2], radiobtnlist.SelectedValue, noh))
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

            pnlstransaction.Visible = false;
            pnlsverifying.Visible = true;
        }
    }

    protected void delete_tran(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = ViewState["leavedash"] as DataTable;
            dt.Rows[row.RowIndex].Delete();
            ViewState["bind"] = dt;
            ViewState["leavedash"] = dt;
            grid_leave.DataSource = dt;
            grid_leave.DataBind();
        }
    }

    protected void datatbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (decimal.Parse(e.Row.Cells[4].Text) > 0)
                e.Row.Cells[5].Text = "True";
            else
                e.Row.Cells[5].Text = "False";
        }
    }

    protected void click_save(object sender, EventArgs e)
    {
        if (grid_leave.Rows.Count > 0)
        {
            string iswithpay = "";
            stateclass a = new stateclass();
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + " and a.under_id <> " + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + " and a.herarchy<>0 ");
            a.sa = ddlOIC.Visible == true ? ddlOIC.SelectedItem.ToString() : txt_delegate.Text.Replace("'", "");
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
            DataTable dtemp = dbhelper.getdata("select * from memployee where id=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + "");
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
                DataTable todate = dbhelper.getdata("select convert(varchar, getdate(), 101)todate");
                query.Value = "insert into TLeaveApplicationLine " +
                        "(EmployeeId,LeaveId,Date,NumberOfHours,WithPay,Remarks,status,sysdate,l_id,setupbasichrs,dailyrate,amount_to_be_paid,inoutduringhalfdayleave) " +
                        "values " +
                        "(" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + "," + gr["Leaveid"] + ",'" + gr["date"] + "','" + gr["noh"] + "','" + iswithpay + "','" + txt_lineremarks.Text.Replace("'", "") + "','Approved-" + todate.Rows[0]["todate"] + "',getdate()," + x + ",'" + dtemp.Rows[0]["FixNumberOfHours"].ToString() + "','" + dtemp.Rows[0]["DailyRate"].ToString() + "','" + amt_to_paid + "'," + gr["expectedin"] + ")";
                dbhelper.getdata(query.Value);
            }

            string approver = "0";
            function.Adminonly("leave", "adminapproved", x, approver);
            DataTable dtgetdate1 = dbhelper.getdata("select TOP(1) (select convert(varchar,a.Date,101))Date from TLeaveApplicationLine a left join Tleave b on a.l_id = b.id where a.l_id = " + x + " order by a.Id asc");
            DataTable dtgetdate2 = dbhelper.getdata("select TOP(1) (select convert(varchar,a.Date,101))Date from TLeaveApplicationLine a left join Tleave b on a.l_id = b.id where a.l_id = " + x + " order by a.Id desc");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "adminaddLeave";
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = dtgetdate1.Rows[0]["Date"].ToString();
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = dtgetdate2.Rows[0]["Date"].ToString();
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Add Leave Credits";
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Response.Redirect("KOISK_addLEAVEadminside?emp_id=" + Request.QueryString["emp_id"].ToString() + "&app_id=" + "" + Request.QueryString["emp_id"].ToString() + "" + "&tp=ed");

            if (ddlOIC.Visible)
            {
                string empID = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);
                dbhelper.getdata("insert into approval_OIC (tdate,tDateEnd,realApprover,oic) values (getdate(),'" + txt_to.Text + "'," + empID + "," + ddlOIC.SelectedValue + ")");
            }
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
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " is already exist! <br/>";
                err = false;
            }
        }
        else
        {
            string query = "select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "' and NumberOfHours = '1.00' and (status like'%For Approval%' or status like'%Approved%' or status like'%verification%' )";
            DataTable dt = dbhelper.getdata(query);
            if (dt.Rows.Count > 0)
            {
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " is already exist! <br/>";
                err = false;
            }

            DataTable dtdaytype = dbhelper.getdata("select * from TLeaveApplicationLine where EmployeeId='" + emp_id + "' and left(convert(varchar,Date,101),10)='" + date_leave + "' and NumberOfHours='" + noh + "' and inoutduringhalfdayleave = '" + designation + "' and (status like'%For Approval%' or status like'%Approved%' or status like'%verification%' )");
            if (dtdaytype.Rows.Count > 0)
            {
                string daytype = designation == "1" ? "AM" : "PM";
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " " + daytype + " is already exist! <br/>";
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
                lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " is a Restday! <br/>";
                err = false;
            }
        }

        DataTable dthol = dbhelper.getdata("select * from MDayTypeDay where branchid='" + dtemp.Rows[0]["branchid"].ToString() + "' and date='" + date_leave + "' and status is null");
        DataTable dtdtrprev = dbhelper.getdata("select * from TDTRLine a left join tdtr b on a.DTRId=b.Id where b.status is null and EmployeeId=" + function.Decrypt(Request.QueryString["emp_id"].ToString(), true) + " and CONVERT(date,a.Date)=CONVERT(date,'" + date_leave + "') and b.PayrollGroupId=" + dtemp.Rows[0]["payrollgroupid"].ToString() + " and b.payroll_id is null");

        if (dthol.Rows.Count > 0)
        {
            lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " is a Holliday! <br/>";
            err = false;
        }
        else if (dtdtrprev.Rows.Count > 0)
        {
            lbl_error_msg.Text = lbl_error_msg.Text + "<br/>" + " Date " + date_leave + " is not allowed for leave filling! <br/>";
            err = false;
        }
        return err;
    }
    protected bool checker()
    {
        bool oi = true;

        if (rb_range.SelectedValue.Length == 0)
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Day Type Designation";
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

        if (txt_from.Text.Length < 10)
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
        else if (txt_lineremarks.Text == "")
        {
            oi = false;
            lbl_error_msg.Text = "Invalid Remarks";
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
            startin.Style.Add("display", "block");
        else
            startin.Style.Add("display", "none");
    }
    protected void getLeaves()
    {
        string employee = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);
        string sql = "select b.id, convert(varchar,a.sysdate,101) date,c.Leave,b.delegate, a.Remarks,a.status " +
        "from TLeaveApplicationLine a  " +
        "left join Tleave b on a.l_id=b.id  " +
        "left join MLeave c on a.LeaveId=c.Id " +
        "where a.status not like '%cancelEMP%' " +
        "and a.EmployeeId=" + employee + " " +
        "group by b.id, convert(varchar,a.sysdate,101),c.Leave,b.delegate, a.Remarks,a.status " +
        "order by b.id desc";
        DataTable leaves = dbhelper.getdata(sql);
        gvLeave.DataSource = leaves;
        gvLeave.DataBind();

        DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

        if (dtt.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(employee, DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

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
            alert.Visible = dpres.Rows.Count == 0 ? true : false;
            grid_view.DataSource = dpres;
            grid_view.DataBind();
        }
        else
        {
            string query = SetUp.leavetype(employee, DateTime.Now.Year.ToString(), "");
            DataTable dt = dbhelper.getdata(query);
            grid_view.DataSource = dt;
            grid_view.DataBind();
        }
    }
    protected void view1(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            lbl_lt.Text = row.Cells[2].Text;
            string employee = function.Decrypt(Request.QueryString["emp_id"].ToString(), true);

            string query = "select a.emp_id,a.under_id,a.herarchy, " +
             "b.FirstName +' '+b.LastName approver, " +
             "case when (select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) = 1 " +
             "then (select convert(varchar,date,101) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) else '--/--/--'  end date, " +
             "case when " +
             "(select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) = 1 " +
             "then (select SUBSTRING(status,0,CHARINDEX('-',status)) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=a.under_id) " +
             "else 'pending'  end [status] " +
             "from Approver a " +
             "left join memployee b on a.under_id = b.Id " +
             "where a.emp_id=" + employee + " and a.under_id<> " + employee + " ";

            DataTable tt = dbhelper.getdata("select * from allow_admin where id='6' and allow='no'");
            if (tt.Rows.Count == 0)
            {

                query += "union " +
                  "select " + employee + ",0, " +
                  "(select COUNT(*) from Approver a where emp_id=" + employee + "),'Admins', " +
                  "case when (select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
                  "then (select CONVERT(varchar,date,101) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
                  "else '--/--/--' end date," +
                  "case when " +
                  "(select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
                  "then (select SUBSTRING(status,0,CHARINDEX('-',status)) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
                  "else 'pending' end " +
                  "order by herarchy";
            }


            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_approver.DataSource = ds;
            grid_approver.DataBind();

            pnlApproverHistory.Visible = ds.Tables[0].Rows.Count == 0 ? false : true;

            nadarang();
            modal.Style.Add("display", "block");
        }
    }
    protected void nadarang()
    {
        string query = "select employeeid request_id,id,l_id,DATE,case when WithPay='True' then 'Yes'else'No' end WithPay,case when numberofhours<1 then 'Halfday' else 'Wholeday' end nod, " +
        "case when LEN( " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end)=0 then status else  " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end end " +
        "status," +
        "remarks,case when inoutduringhalfdayleave=0 then 'AM-PM'when inoutduringhalfdayleave=1 then 'AM' else 'PM' end designation from TLeaveApplicationLine where l_id=" + id.Value + "";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_pay.DataSource = ds;
        grid_pay.DataBind();

        query = "select LEFT(CONVERT(varchar,date,101),10)sysdate,case when a.emp_id=0 then 'HR Admin' else  (select lastname+', '+firstname from memployee where id=a.emp_id) end approver,a.remarks,SUBSTRING(a.status,0, CHARINDEX('-',status)) status from sys_applog a where app_id=" + id.Value + " and a.type='L' order by a.id asc";
        DataTable dt = dbhelper.getdata(query);
        grid_history.DataSource = dt;
        grid_history.DataBind();
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            Label lbl_status = (Label)e.Row.FindControl("lbl_status");
            string[] stat = e.Row.Cells[1].Text.Split('-');
            if (stat[0] == "Approved" || stat[0] == "Cancel")
            {
                lnkcan.Enabled = false;
                lnkcan.CssClass += " text-gray";
                lbl_status.Text = stat[0] + " (" + stat[1] + ")";
            }
        }
    }
    protected void rowboundLeave(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            Label lbl_status = (Label)e.Row.FindControl("lbl_status");
            string[] stat = e.Row.Cells[4].Text.Split('-');
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");

            switch (stat[0])
            {
                case "Approved":
                    lblStatus.CssClass += " label-success";
                    lblStatus.Text = "Approved (" + stat[1] + ")";
                    lnkcan.Enabled = false;
                    lnkcan.OnClientClick = null;
                    break;
                case "For Approval":
                    lblStatus.CssClass += " label-warning";
                    lblStatus.Text = "For Approval";
                    break;
                case "Cancel":
                    lblStatus.CssClass += " label-danger";
                    lblStatus.Text = "Disapproved (" + stat[1] + ")";
                    break;
                case "verification":
                    lblStatus.CssClass += " label-primary";
                    lblStatus.Text = "Verification";
                    break;
                default:
                    break;
            }
        }
    }
    protected void rowboundgrid_pay(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_can = (LinkButton)e.Row.FindControl("lnk_can");
            string stat = e.Row.Cells[4].Text;
            if (stat == "Approved" || stat == "Cancelled")
                lnk_can.Visible = false;

            if (e.Row.Cells[2].Text == "No")
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void reprint(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Write("<script>alert('Selection Failed!')</script>");
        }
    }
    protected void cancelall(object sender, EventArgs e)
    {
        Response.Write("<script>alert('Selection Failed!')</script>");
    }
    protected void cancelleave(object sender, EventArgs e)
    {
        Response.Write("<script>alert('Selection Failed!')</script>");
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addLEAVEadminside?emp_id=" + Request.QueryString["emp_id"].ToString() + "&app_id=" + "" + Request.QueryString["emp_id"].ToString() + "" + "&tp=ed");
    }
}