using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_Manager_approve_leave : System.Web.UI.Page
{
    //public static string query,user_id,id;
    //public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            BindData();

    }


    protected void viewinfo(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            lbl_lt.Text = row.Cells[6].Text;
            string employee = Session["emp_id"].ToString();

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
            else
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
        }
    }

    protected void BindData()
    {
        /**BTK 111119**/
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();

        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
        "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
        "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
        "c.LastName +', ' + c.FirstName + ' ' + c.MiddleName as fullname, c.id emp_id, " +
        "d.Leave, " +
        "e.Department, " +

        "f.under_id,f.herarchy, " +
        "case when (select under_id from approver where emp_id=f.emp_id and status is null and herarchy = f.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=f.emp_id and status is null and herarchy = f.herarchy + 1) end as nxt_id " +

        "from Tleave a " +
        "left join TLeaveApplicationLine b on a.id=b.l_id " +
        "left join MEmployee c on b.EmployeeId=c.id " +
        "left join MLeave d on b.LeaveId=d.Id " +
        "left join MDepartment e on c.DepartmentId=e.id " +
        "left join approver f on b.EmployeeId=f.emp_id " +

        "where b.status like '%for approval%' and a.approver_id=" + approver + " and f.under_id=" + approver + " and f.herarchy <> 0 " +
        "order by a.id desc ";


        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        alert.Visible = dt.Rows.Count == 0 ? true : false;
        div_action.Visible = dt.Rows.Count == 0 ? false : true;
    }

    protected void cbsel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((CheckBox)sender).Parent.Parent)
        {
            CheckBox chkEmp = (CheckBox)row.FindControl("chkEmp");
            if (chkEmp.Checked)
                ib_del.Visible = true;
            else
                ib_del.Visible = false;
        }
    }

    protected void search(object sender, EventArgs e)
    {
        /**BTK 111119**/
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();

        if (Request.QueryString["nt"] != null)
            

        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
                "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                "c.LastName +', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                "d.Leave, " +
                "e.Department, " +

                "f.under_id,f.herarchy, " +
                "case when (select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) end as nxt_id " +

                "from Tleave a " +
                "left join TLeaveApplicationLine b on a.id=b.l_id " +
                "left join MEmployee c on b.EmployeeId=c.id " +
                "left join MLeave d on b.LeaveId=d.Id " +
                "left join MDepartment e on c.DepartmentId=e.id " +
                "left join approver f on b.EmployeeId=f.emp_id " +
                "where b.status like '%for approval%' and a.approver_id=" + approver + " and f.under_id=" + approver + " " +
                "and (select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                "order by a.id desc ";


        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grid_view.HeaderRow.FindControl("chkboxSelectAll");
        int i = 0;
        foreach (GridViewRow row in grid_view.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkEmp");

            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
                i++;
            }
            else
            {

                ChkBoxRows.Checked = false;
                if (i > 0)
                {
                    i--;
                }
            }

        }
        lbl_del_notify.Text = i + " rows".ToString();
    }

    protected void delete_all(object sender, EventArgs e)
    {
             string query = "";
             for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
             {
                 CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
                 if (chkEmp.Checked == true)
                 {
                     stateclass a = new stateclass();
                     a.sa = grid_view.Rows[i].Cells[0].Text;
                     a.sb = Session["emp_id"].ToString();
                     a.sc = "L";
                     a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                     a.se = "";
                     bol.system_logs(a);
                     query += "update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + grid_view.Rows[i].Cells[0].Text + " ";
                 }
             }

            if (query.Replace(" ", "").Length > 0)
            {
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='al'", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='al'", true);
 
    }

    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
             
            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, int.Parse(Session["user_id"].ToString()));
            DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + grid_view.Rows[i].Cells[9].Text + "");
            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "L";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                if (grid_view.Rows[i].Cells[9].Text == "0")
                {
                    DataTable xx = dbhelper.getdata("select * from allow_admin where id='6' and allow='no'");

                    if (xx.Rows.Count == 0)
                        query += "update TLeaveApplicationLine set status='verification' where l_Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    else
                    {
                        query += "update TLeaveApplicationLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where l_Id=" + grid_view.Rows[i].Cells[0].Text + "";

                        DataTable getleavedtails = dbhelper.getdata("select *,left(convert(varchar,date,101),10)dateleave from TLeaveApplicationLine where l_Id=" + grid_view.Rows[i].Cells[0].Text + " and withpay='True' ");
                        foreach (DataRow dr in getleavedtails.Rows)
                        {
                            DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + dr["EmployeeId"].ToString() + " ");
                            DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(dr["dateleave"].ToString(), dtgetemp.Rows[0]["PayrollGroupId"].ToString()));
                            if (dtchkpayroll.Rows.Count == 1)
                            {
                                
                                DataTable dtr = Core.DTRF(dr["dateleave"].ToString(), dr["dateleave"].ToString(), "KIOSK_" + dr["EmployeeId"].ToString());
                                decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["leaveamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["leaveamt"].ToString()) : 0 : 0;
                                if (amttt > 0)
                                {
                                    DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(dr["dateleave"].ToString(), dr["EmployeeId"].ToString()));
                                    if (decimal.Parse(chkifnaa.Rows[0]["leaveamount"].ToString()) == 0)
                                    {
                                        DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(dr["dateleave"].ToString(), dr["id"].ToString(), dr["EmployeeId"].ToString(), "Leave", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                                    }
                                }
                            }
                        }
                    }
                   // function.AddNotification("Leave Verification", "vl", grid_view.Rows[i].Cells[9].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                else
                {
                    query += "update Tleave set approver_id='" + grid_view.Rows[i].Cells[9].Text + "' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                   // function.AddNotification("Leave Approval", "al", grid_view.Rows[i].Cells[9].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                dbhelper.getdata(query);
                function.Notification(grid_view.Rows[i].Cells[0].Text, "leave", grid_view.Rows[i].Cells[11].Text);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='al'", true);
            }
            //else
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='al'", true);
        }

   
    }

    protected void view(object sender, EventArgs e)
    {
        /**BETA 110519
         * ADD EMAIL NOTIFICATION
         * -NEW ELEMANT [hfEmployee.Value = row.Cells[11].Text;]
         * **/
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
             if (TextBox1.Text == "Yes")
                {
                    idd.Value = row.Cells[0].Text;
                    hfEmployee.Value = row.Cells[11].Text;
                    Div1.Visible = true;
                    Div2.Visible = true;
                    Div3.Visible = false;
                    Div4.Visible = false;
                }
                else{}
        }
    }

    protected void approve(object sender, EventArgs e)
    {
        /**BETA 
         * 110519
         * ADD EMAIL NOTIFICATION
         * 
         * 111119
         * OIC
         * **/
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();

                function.ReadNotification(row.Cells[0].Text, int.Parse(approver));
                DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[9].Text + "");

                if (row.Cells[9].Text == "0")
                {
                    DataTable xx = dbhelper.getdata("select * from allow_admin where id='6' and allow='no'");

                    if (xx.Rows.Count == 0)
                        dbhelper.getdata("update TLeaveApplicationLine set status='verification' where l_Id=" + row.Cells[0].Text + "");
                    else
                    {
                        dbhelper.getdata("update TLeaveApplicationLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where l_Id=" + row.Cells[0].Text + "");
                        //function.AddNotification("Leave Verification", "vl", row.Cells[9].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);

                        DataTable getleavedtails = dbhelper.getdata("select *,left(convert(varchar,date,101),10)dateleave from TLeaveApplicationLine where l_Id=" + row.Cells[0].Text + " and withpay='True' ");
                        foreach (DataRow dr in getleavedtails.Rows)
                        {
                            DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + dr["EmployeeId"].ToString() + " ");
                            DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(dr["dateleave"].ToString(), dtgetemp.Rows[0]["PayrollGroupId"].ToString()));
                            if (dtchkpayroll.Rows.Count == 1)
                            {

                                DataTable dtr = Core.DTRF(dr["dateleave"].ToString(), dr["dateleave"].ToString(), "KIOSK_" + dr["EmployeeId"].ToString());
                                decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["leaveamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["leaveamt"].ToString()) : 0 : 0;
                                if (amttt > 0)
                                {
                                    DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(dr["dateleave"].ToString(), dr["EmployeeId"].ToString()));
                                    if (decimal.Parse(chkifnaa.Rows[0]["leaveamount"].ToString()) == 0)
                                    {
                                        DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(dr["dateleave"].ToString(), dr["id"].ToString(), dr["EmployeeId"].ToString(), "Leave", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    dbhelper.getdata("update Tleave set approver_id='" + row.Cells[9].Text + "' where id=" + row.Cells[0].Text + "");
                    //function.AddNotification("Leave Approval", "al", row.Cells[9].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }


                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = approver;
                a.sc = "L";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                //NOTIFICATION
                function.Notification("leave", "approved", row.Cells[0].Text, row.Cells[11].Text);

                //OIC
                OIC(row.Cells[0].Text, "Approved");

               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='" + Request.RawUrl + "'", true);
            }
        }
    }

    protected void OIC(string id, string action)
    {
        /**BTK 
         * 110519
         * OIC**/
        DataTable oic = getdata.OIC(Session["emp_id"].ToString());
        if (oic.Rows.Count > 0)
        {
            dbhelper.getdata("insert into approval_OIC_transaction values (" + oic.Rows[0]["id"].ToString() + "," + id + ",'leave','" + action + "')");
        }
    }

   protected void delete3(object sender, EventArgs e)
   {
       /**BTK 
        * 110519
        * -ADD EMAIL NOTIFICATION
        * 
        * 111119
        * -OIC
        * **/
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();

       stateclass a = new stateclass();
        a.sa = idd.Value;
        a.sb = approver;
        a.sc = "L";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);
        dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + idd.Value + "");
        
        //OIC
        OIC(idd.Value, "Cancelled");

        //NOTIFICATION
        function.Notification("leave", "declined", idd.Value, hfEmployee.Value);

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='" + Request.RawUrl + "'", true);
    }

    protected void view_img(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Response.Write("<script>alert('a')</script>");

            idd.Value = row.Cells[0].Text;

            string query = "select id,CONVERT(varchar, leave_id)+'\\'+CONVERT(varchar, file_img) as awts from leave_image where leave_id=" + idd.Value + " ";
            DataSet ds = bol.display(query);
            grid_img.DataSource = ds.Tables["table"];
            grid_img.DataBind();

            if (grid_img.Rows.Count == 0)
                lbl_msg.Text = "No Attachment File";
            else
                lbl_msg.Text = "";



            Div1.Visible = false;
            Div2.Visible = false;

            Div3.Visible = true;
            Div4.Visible = true;

        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("al", false);
    }
    
}