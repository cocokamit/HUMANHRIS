using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_verify_leave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            getdata();
            loadable();
        }

    }

    protected void loadable()
    {
        string query = "";

        DataTable dt;

        query = "select * from Mleave where action is null order by id desc";
        dt = dbhelper.getdata(query);

        drop_type.Items.Clear();
        drop_type.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            drop_type.Items.Add(new ListItem(dr["Leave"].ToString(), dr["id"].ToString()));
        }
    }

    protected void search(object sender, EventArgs e)
    {
        string query = "";

        //if (txt_to.Text != "" && txt_from.Text != "")
        //{
            query += "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
               "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
               "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
               "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
               "d.Leave, " +
               "e.Department " +
               "from Tleave a " +
               "left join TLeaveApplicationLine b on a.id=b.l_id " +
               "left join MEmployee c on b.EmployeeId=c.id " +
               "left join MLeave d on b.LeaveId=d.Id " +
               "left join MDepartment e on c.DepartmentId=e.id " +
               "left join approver f on b.EmployeeId=f.emp_id " +
               "where b.status like '%verification%' " +
               "and (select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
               "order by a.id desc ";
        //}
        //if(drop_type.Text != "")
        //{
        //    query += "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
        //       "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
        //       "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
        //       "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
        //       "d.Leave, " +
        //       "e.Department " +
        //       "from Tleave a " +
        //       "left join TLeaveApplicationLine b on a.id=b.l_id " +
        //       "left join MEmployee c on b.EmployeeId=c.id " +
        //       "left join MLeave d on b.LeaveId=d.Id " +
        //       "left join MDepartment e on c.DepartmentId=e.id " +
        //       "left join approver f on b.EmployeeId=f.emp_id " +
        //       "where b.leaveId='" + drop_type.SelectedValue + "' and b.status like '%verification%' " +
        //       "order by a.id desc ";
        //}
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
                "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                "c.id empID, c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                "d.Leave, " +
                "e.Department " +
                "from Tleave a " +
                "left join TLeaveApplicationLine b on a.id=b.l_id " +
                "left join MEmployee c on b.EmployeeId=c.id " +
                "left join MLeave d on b.LeaveId=d.Id " +
                "left join MDepartment e on c.DepartmentId=e.id " +
                "left join approver f on b.EmployeeId=f.emp_id " +
                "where b.status like '%verification%' " +
                //"where b.status not like '%approved%' and b.status not like '%cancel%' and b.status not like '%delete%' " +
                "order by a.id desc ";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }
    protected void leavemonitoring(object seder, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton hide = (LinkButton)e.Row.FindControl("itemstat");
            LinkButton thumbup = (LinkButton)e.Row.FindControl("lnk_approve");
            LinkButton trash = (LinkButton)e.Row.FindControl("lnk_view");
            LinkButton paperclip = (LinkButton)e.Row.FindControl("LinkButton1");
            LinkButton details = (LinkButton)e.Row.FindControl("linkviewleave");
            if (hide.Text != "verification")
            {
                thumbup.OnClientClick ="";
                trash.OnClientClick = "";
                thumbup.Enabled = false;
                trash.Enabled = false;
                paperclip.Enabled = false;
            }
            //else if (hide.Text == "verification")
            //{
            //    details.Enabled = false;
            //}
        }
    }

    protected void Viewleavestatus(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = dbhelper.getdata("select Id from MEmployee where id=" + row.Cells[1].Text);
            id.Value = row.Cells[0].Text;
            lbl_lt.Text = row.Cells[3].Text;

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
             "where a.emp_id=" + dt.Rows[0]["Id"].ToString() + " " +
             "union " +
             "select " + dt.Rows[0]["Id"].ToString() + ",0, " +
             "(select COUNT(*) from Approver a where emp_id=" + dt.Rows[0]["Id"].ToString() + "),'Admin', " +
             "case when (select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
             "then (select CONVERT(varchar,date,101) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
             "else '--/--/--' end date," +
             "case when " +
             "(select COUNT(*) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) = 1 " +
             "then (select SUBSTRING(status,0,CHARINDEX('-',status)) from sys_applog where app_id=" + row.Cells[0].Text + " and type='L' and emp_id=0) " +
             "else 'pending' end " +
             "order by herarchy";


            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_approver.DataSource = ds;
            grid_approver.DataBind();

            nadarang();
            modal.Style.Add("display", "block");
        }
    }
    protected void nadarang()
    {
        string query = "select employeeid request_id,id,l_id,DATE,WithPay,case when numberofhours<1 then 'Halftday' else 'Wholeday' end nod, " +
        "case when LEN( " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end)=0 then status else  " +
        "case when SUBSTRING(status,0, CHARINDEX('-',status))='cancel' or SUBSTRING(status,0, CHARINDEX('-',status))='deleted' then 'Cancelled' else SUBSTRING(status,0, CHARINDEX('-',status)) end end " +
        "status," +
        "remarks from TLeaveApplicationLine where l_id=" + id.Value + "";
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
        }
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='vl'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='vl'", true);

    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                idd.Value = grid_view.Rows[0].Cells[0].Text;
                Div1.Visible = true;
                Div2.Visible = true;
                Div3.Visible = false;
                Div4.Visible = false;
            }
            else { }
        }
    }
    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, 0);
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");

            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = "0";
                a.sc = "L";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                function.Notification(grid_view.Rows[i].Cells[0].Text, "leave", "0");
                query = "update TLeaveApplicationLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where l_Id=" + grid_view.Rows[i].Cells[0].Text + "";
                dbhelper.getdata(query);
                DataTable getleavedtails = dbhelper.getdata("select *,left(convert(varchar,date,101),10)dateleave from TLeaveApplicationLine where l_Id=" + grid_view.Rows[i].Cells[0].Text + " and withpay='True' ");
                foreach (DataRow dr in getleavedtails.Rows)
                {
                    DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + dr["EmployeeId"].ToString() + " ");
                    DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(dr["dateleave"].ToString(), dtgetemp.Rows[0]["PayrollGroupId"].ToString()));
                    if (dtchkpayroll.Rows.Count == 1)
                    {
                        DataTable dtr = Core.DTRF(dr["dateleave"].ToString(), dr["dateleave"].ToString(), "KIOSK_" + dr["EmployeeId"].ToString());
                        decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["leaveamount"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["leaveamount"].ToString()) : 0 : 0;
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

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vl'", true);
            }

        }
    }
    protected void approve(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);
            if (TextBox1.Text == "Yes")
            {
               dbhelper.getdata("update TLeaveApplicationLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where l_Id=" + row.Cells[0].Text + "");
            
               DataTable getleavedtails = dbhelper.getdata("select *,left(convert(varchar,date,101),10)dateleave from TLeaveApplicationLine where l_Id=" + row.Cells[0].Text + " and withpay='True' ");
               foreach (DataRow dr in getleavedtails.Rows)
               {
                   DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + dr["EmployeeId"].ToString() + " ");
                   DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(dr["dateleave"].ToString(), dtgetemp.Rows[0]["PayrollGroupId"].ToString()));
                   if (dtchkpayroll.Rows.Count == 1)
                   {
                       DataTable dtr = Core.DTRF(dr["dateleave"].ToString(), dr["dateleave"].ToString(),"KIOSK_" + dr["EmployeeId"].ToString());
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
                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "L";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                function.Notification(row.Cells[0].Text, "leave", "0");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vl'", true);
            }
            else
            { }
        }

    }
    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        //Read Notification
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "L";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);
        dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + idd.Value + "");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='vl'", true);
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
        Response.Redirect("vl", false);
    }
}