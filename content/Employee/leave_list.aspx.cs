using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class content_Employee_leave_list : System.Web.UI.Page
{
    //public static string query,user_id,id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
            BindData();
        }
    }

    protected void disp()
    {


        //BUTYOK
        ViewState["role"] = Session["role"].ToString() == "4" ? "HOD" : "Heartist";
        if (ViewState["role"].ToString() == "HOD")
        {
            DataTable dtdept = getdata.ApproverUnder(Session["emp_id"].ToString()); 
            ddl_empsleave.Items.Clear();
            ddl_empsleave.Items.Add(new ListItem(Session["ngalan"].ToString(), Session["emp_id"].ToString()));

            foreach (DataRow dr in dtdept.Rows)
            {
                ddl_empsleave.Items.Add(new ListItem(dr["underName"].ToString(), dr["underID"].ToString()));
            }

            pnlMember.Visible = false;
            pnlManager.Visible = true;
            ddl_empsleave.SelectedValue = Session["emp_id"].ToString();
        }
        else
        {
            pnlManager.Visible = false;
            pnlMember.Visible = true;
        }
    }

    protected void addempsleave(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from MEmployee where Id = '" + ddl_empsleave.SelectedValue + "'");
        try
        {
            Response.Redirect("KOISK_addempsLEAVE?emp_id=" + function.Encrypt(dt.Rows[0]["Id"].ToString(), true) + "", false);
        }
        catch { }
    }

    protected void getLeaves()
    {
        string employee = ViewState["role"].ToString() == "HOD" ? ddl_empsleave.SelectedValue : Session["emp_id"].ToString();
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

            grid_leave_credits.DataSource = dpres;
            grid_leave_credits.DataBind();
        }
        else
        {
            string query = SetUp.leavetype(employee, DateTime.Now.Year.ToString(), "");
            DataTable dt = dbhelper.getdata(query);
            grid_leave_credits.DataSource = dt;
            grid_leave_credits.DataBind();
        }
    }

    protected void leaveapplication(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        getLeaves();

        string query = "create table #dummy(row_cnt int identity(1,1),trnid int,sysdate datetime,approverid int,empid int,delegate varchar(max),remarks varchar(max)) " +
       "create table #dummy1(trnid int,sysdate datetime,approverid int,empid int,descc varchar(max),leaveid int,delegate varchar(max),remarks varchar(max) ) " +
       "create table #dummy2(row_cnt int,date datetime,leaveid int) " +
       "declare @trnid int " +
       "declare @sysdate datetime " +
       "declare @approverid int " +
       "declare @empid int " +
       "declare @descc varchar(max) " +
       "declare @leaveid int " +
       "declare @delegate varchar(max) " +
       "declare @remarks varchar(max) " +
       "insert into #dummy (trnid,sysdate,approverid,empid,delegate,remarks) " +
       "select distinct (a.id),a.sysdate,a.approver_id,b.employeeid,a.delegate,b.remarks " +
       "from Tleave a " +
       "left join TLeaveApplicationLine b on a.id= b.l_id  " +
       "where b.EmployeeId='" + ddl_empsleave.SelectedValue + "' ORDER BY a.id desc " +
       "declare @cnt int " +
       "declare @rowcnt int " +
       "set @rowcnt=1 " +
       "set @cnt=(SELECT COUNT(*) from #dummy)  " +
       "print @cnt " +
       "while(@rowcnt<=@cnt) " +
       "begin " +
       "SELECT @trnid=trnid,@sysdate=sysdate,@approverid=approverid,@empid=empid,@delegate=delegate,@remarks=remarks from #dummy where row_cnt=@rowcnt " +
       "declare @cntTAL int " +
       "declare @rowcntTAL int " +
       "declare @descccc varchar(max) " +
       "set @rowcntTAL=1 " +
       "set @cntTAL=(select COUNT(*) from TLeaveApplicationLine where l_id=@trnid) " +
       "insert into #dummy2(row_cnt, date,leaveid) " +
       "select ROW_NUMBER() OVER(ORDER BY id ASC), DATE,leaveid from TLeaveApplicationLine where l_id=@trnid  order by id asc " +
       "set @descccc=''; " +
       "while(@rowcntTAL <= @cntTAL) " +
       "begin " +
       "select @descc= LEFT(CONVERT(varchar,Date,101),10),@leaveid=leaveid from #dummy2 where  row_cnt=@rowcntTAL  " +
       "if(@rowcntTAL=1) " +
       "begin " +
       "set @descccc=@descc " +
       "end " +
       "else if(@rowcntTAL>1 and @rowcntTAL < @cntTAL) " +
       "begin " +
       "set @descccc=@descccc+', '+@descc " +
       "end " +
       "else " +
       "begin " +
       "set @descccc=@descccc+', '+@descc " +
       "end " +
       "set @rowcntTAL = @rowcntTAL + 1; " +
       "end " +

       "insert into #dummy1(trnid,sysdate,approverid,empid,descc,leaveid,delegate,remarks) " +
                    "values(@trnid,@sysdate,@approverid,@empid,@descccc,@leaveid,@delegate,@remarks) " +
                    "print @descccc; " +
       "delete from #dummy2 " +
       "set @rowcnt=@rowcnt+1; " +
       "end  " +
       "select a.trnid,LEFT(CONVERT(varchar,a.sysdate,101),10)trn_date,b.leave,a.descc,a.delegate,a.remarks from #dummy1 a " +
       "left join mleave b on a.leaveid=b.id " +
       "drop table #dummy " +
       "drop table #dummy1 " +
       "drop table #dummy2";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();



        DataTable dtt = dbhelper.getdata("Select * from SetUpTable");

        if (dtt.Rows[0]["LeaveType"].ToString() == "4")
        {
            string[] twos = SetUp.leavetype(ddl_empsleave.SelectedValue, DateTime.Now.Year.ToString(), "").Split(new string[] { "UNION" }, StringSplitOptions.None);

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

            grid_leave_credits.DataSource = dpres;
            grid_leave_credits.DataBind();
        }
        else
        {

            query = SetUp.leavetype(ddl_empsleave.SelectedValue, DateTime.Now.Year.ToString(), "");
            ds = bol.display(query);
            grid_leave_credits.DataSource = ds;
            grid_leave_credits.DataBind();
        }
    }

    //BUTYOK
    protected void BindData()
    {
      
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        getLeaves();

        //string sql = "select b.id, convert(varchar,a.sysdate,101) date,c.Leave,b.delegate, a.Remarks,a.status "+
        //"from TLeaveApplicationLine a  "+
        //"left join Tleave b on a.l_id=b.id  "+
        //"left join MLeave c on a.LeaveId=c.Id "+
        //"where a.status not like '%cancelEMP%' " +
        //"group by b.id, convert(varchar,a.sysdate,101),c.Leave,b.delegate, a.Remarks,a.status " +
        //"order by date desc";
        //DataTable leaves = dbhelper.getdata(sql);
        //gvLeave.DataSource = leaves;
        //gvLeave.DataBind();

        //string query = "select distinct(a.id),a.delegate,a.sysdate,a.admin_remarks,b.status, " +
        //     "b.remarks,(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
        //     "(select top 1 Date from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
        //     "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
        //     "d.Leave, " +
        //     "e.Department  " +
        //     "from Tleave a " +
        //     "left join TLeaveApplicationLine b on a.id=b.l_id " +
        //     "left join MEmployee c on b.EmployeeId=c.id " +
        //     "left join MLeave d on b.LeaveId=d.Id " +
        //     "left join MDepartment e on c.DepartmentId=e.id " +
        //     "where b.EmployeeId='" + Session["emp_id"].ToString() + "' and SUBSTRING(status,0, CHARINDEX('-',b.status))<>'deleted'  order by a.id desc";


        //DataSet ds = new DataSet();
        //ds = bol.display(query);
        //grid_view.DataSource = ds;
        //grid_view.DataBind();
        string query = "create table #dummy(row_cnt int identity(1,1),trnid int,sysdate datetime,approverid int,empid int,delegate varchar(max),remarks varchar(max)) " +
       "create table #dummy1(trnid int,sysdate datetime,approverid int,empid int,descc varchar(max),leaveid int,delegate varchar(max),remarks varchar(max) ) " +
       "create table #dummy2(row_cnt int,date datetime,leaveid int) " +
       "declare @trnid int " +
       "declare @sysdate datetime " +
       "declare @approverid int " +
       "declare @empid int " +
       "declare @descc varchar(max) " +
       "declare @leaveid int " +
       "declare @delegate varchar(max) " +
       "declare @remarks varchar(max) " +
       "insert into #dummy (trnid,sysdate,approverid,empid,delegate,remarks) " +
       "select distinct (a.id),a.sysdate,a.approver_id,b.employeeid,a.delegate,b.remarks " +
       "from Tleave a " +
       "left join TLeaveApplicationLine b on a.id= b.l_id  " +
       "where b.EmployeeId='" + Session["emp_id"].ToString() + "' ORDER BY a.id desc " +
       "declare @cnt int " +
       "declare @rowcnt int " +
       "set @rowcnt=1 " +
       "set @cnt=(SELECT COUNT(*) from #dummy)  " +
       "print @cnt " +
       "while(@rowcnt<=@cnt) " +
       "begin " +
       "SELECT @trnid=trnid,@sysdate=sysdate,@approverid=approverid,@empid=empid,@delegate=delegate,@remarks=remarks from #dummy where row_cnt=@rowcnt " +
       "declare @cntTAL int " +
       "declare @rowcntTAL int " +
       "declare @descccc varchar(max) " +
       "set @rowcntTAL=1 " +
       "set @cntTAL=(select COUNT(*) from TLeaveApplicationLine where l_id=@trnid) " +
       "insert into #dummy2(row_cnt, date,leaveid) " +
       "select ROW_NUMBER() OVER(ORDER BY id ASC), DATE,leaveid from TLeaveApplicationLine where l_id=@trnid  order by id asc " +
       "set @descccc=''; " +
       "while(@rowcntTAL <= @cntTAL) " +
       "begin " +
       "select @descc= LEFT(CONVERT(varchar,Date,101),10),@leaveid=leaveid from #dummy2 where  row_cnt=@rowcntTAL  " +
       "if(@rowcntTAL=1) " +
       "begin " +
       "set @descccc=@descc " +
       "end " +
       "else if(@rowcntTAL>1 and @rowcntTAL < @cntTAL) " +
       "begin " +
       "set @descccc=@descccc+', '+@descc " +
       "end " +
       "else " +
       "begin " +
       "set @descccc=@descccc+', '+@descc " +
       "end " +
       "set @rowcntTAL = @rowcntTAL + 1; " +
       "end " +

       "insert into #dummy1(trnid,sysdate,approverid,empid,descc,leaveid,delegate,remarks) " +
                    "values(@trnid,@sysdate,@approverid,@empid,@descccc,@leaveid,@delegate,@remarks) " +
                    "print @descccc; " +
       "delete from #dummy2 " +
       "set @rowcnt=@rowcnt+1; " +
       "end  " +
       "select a.trnid,LEFT(CONVERT(varchar,a.sysdate,101),10)trn_date,b.leave,a.descc,a.delegate,a.remarks from #dummy1 a " +
       "left join mleave b on a.leaveid=b.id " +
       "drop table #dummy " +
       "drop table #dummy1 " +
       "drop table #dummy2";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();


    

    }

    protected void addcredentials(object sender, EventArgs e)
    {
        //DataTable checkcredits = dbhelper.getdata("select DATEDIFF(MONTH, DateHired, GETDATE()) AS dh,DateHired,* from memployee where id='" + Session["emp_id"].ToString() + "' ");

        //if (int.Parse(checkcredits.Rows[0]["dh"].ToString()) <= 6)
        //{
        //    DataTable dt = dbhelper.getdata("select * from leave_credits where leaveid='13' and empid='" + Session["emp_id"].ToString() + "' and yyyyear='" + DateTime.Now.Year + "' ");
        //    if (dt.Rows.Count != 0)
        //    {
        //        dbhelper.getdata("update leave_credits set credit='" + checkcredits.Rows[0]["dh"].ToString() + "' where id='" + dt.Rows[0]["id"].ToString() + "' ");
        //    }
        //    else
        //    {
        //        dbhelper.getdata("insert into leave_credits (sysdate,userid,empid,leaveid,credit,convertocash,renew,action,yyyyear) " +
        //                        "values (getdate(),0,'" + Session["emp_id"].ToString() + "'),13,'" + checkcredits.Rows[0]["dh"].ToString() + "','no','Yearly',NULL,'" + DateTime.Now.Year + "' ");
        //    }

        //}
        //else
        //{
        //    DataTable dt = dbhelper.getdata("select * from leave_credits where leaveid='13' and empid='" + Session["emp_id"].ToString() + "' and yyyyear=" + DateTime.Now.Year + " ");
        //    if (dt.Rows.Count != 0)
        //    {
        //        dbhelper.getdata("update leave_credits set credit=month(GETDATE()) where id='" + dt.Rows[0]["id"].ToString() + "' ");
        //    }
        //    else
        //    {
        //        dbhelper.getdata("insert into leave_credits (sysdate,userid,empid,leaveid,credit,convertocash,renew,action,yyyyear) " +
        //                        "values (getdate(),0,'" + Session["emp_id"].ToString() + "',13,month(GETDATE()),'no','Yearly',NULL," + DateTime.Now.Year + ") ");
        //    }
        //}

        Response.Redirect("KOISK_addLEAVE");
    }

    protected void cancel(object sender, EventArgs e)
    {
        if (TextBox1.Text == "YES")
        {
            string query = "update TLeaveApplicationLine set status='" + "deleted-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where id=" + hdnLID.Value + " ";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='KOISK_LEAVE'", true);
        }
    }
    protected void cancelall(object sender, EventArgs e)
    {

    }

    protected void cancelleave(object sender, EventArgs e)
    {
        if (TextBox1.Text == "YES")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text; //leaveid
                a.sb = Session["emp_id"].ToString();//emp_id
                a.sc = "L";
                a.sd = "Cancelled - " + DateTime.Now.ToShortDateString().ToString();
                a.se = "Cancelled Leave Requested";
                a.sf = Session["emp_id"].ToString();//request_id
                bol.system_logs(a);
                string query = "update TLeaveApplicationLine set status='" + "cancelEMP-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + row.Cells[0].Text + "";
                dbhelper.getdata(query);
                DataTable dt = dbhelper.getdata("select (select Leave from MLeave where Id = LeaveId)Leave, Remarks,a.EmployeeId, * from TLeaveApplicationLine a left join Tleave b on b.id=a.l_id where b.id = " + row.Cells[0].Text + "");
                DataTable getid = dbhelper.getdata("select id from nobel_user where emp_id = '" + dt.Rows[0]["EmployeeId"].ToString() + "'");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "Inbehalfcancelingleave";
                        cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "Cancel Leave";
                        cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Leave"].ToString();
                        cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = dt.Rows[0]["Remarks"].ToString();
                        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = dt.Rows[0]["EmployeeId"].ToString();
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"].ToString();
                        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='KOISK_LEAVE'", true);
            }
        }
        else
        { }
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hdnLID.Value = row.Cells[0].Text;
            modal_delete.Style.Add("display", "block");
        }
    }

    protected void view1(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            lbl_lt.Text = row.Cells[2].Text;
            string employee = ViewState["role"].ToString() == "HOD" ? ddl_empsleave.SelectedValue : Session["emp_id"].ToString();
 
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

    protected void reprint(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='KOISK_LEAVE';window.open('','_new').location.href='leaveform?key=" + function.Encrypt(id.Value, true) + "'", true);
        }
    }

    protected void opop(object sender, EventArgs e)
    {

    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_LEAVE", false);
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
                    lnkcan.OnClientClick =null;
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
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getLeaves();
        gvLeave.PageIndex = e.NewPageIndex;
        gvLeave.DataBind();
    }
}