using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verifyrequestupdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.Id,(select FullName from MUser where emp_id = a.IdNumber)FullName,a.Failed,a.[Status],left(convert(varchar,a.DateRequest,101),10)RequestDate,a.Request,a.Code from RequestUpdate201 a where [Status] = 'For Approval' Order by Id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);
            if (TextBox1.Text == "Yes")
            {
                DataTable getinfo = dbhelper.getdata("select * from RequestUpdate201 where Id = " + row.Cells[0].Text + "");
                dbhelper.getdata("update RequestUpdate201 set [Status] = 'Cancel' where Id = " + row.Cells[0].Text + "");
                dbhelper.getdata("insert into sys_notification([date],[subject],[to],[key],url,[status],[type],content)values"
                    + "(GETDATE(),'MASTERFILE REQUEST'," + getinfo.Rows[0]["IdNumber"] + ",'3','KIOSK_Masterfile','0','0','" + getinfo.Rows[0]["Failed"].ToString() + " was Cancelled. " + "')");
            }
        }
        string query = "select a.Id,(select FullName from MUser where emp_id = a.IdNumber)FullName,a.Failed,a.[Status],left(convert(varchar,a.DateRequest,101),10)RequestDate,a.Request,a.Code from RequestUpdate201 a where [Status] = 'For Approval' Order by Id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void approve(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);
            if (TextBox1.Text == "Yes")
            {
                DataTable getinfo = dbhelper.getdata("select * from RequestUpdate201 where Id = " + row.Cells[0].Text + "");
                dbhelper.getdata("insert into sys_notification([date],[subject],[to],[key],url,[status],[type],content)values"
                    + "(GETDATE(),'MASTERFILE REQUEST'," + getinfo.Rows[0]["IdNumber"] + ",'3','KIOSK_Masterfile','0','0','" + getinfo.Rows[0]["Failed"].ToString() + " was Approved. " + "')");
                dbhelper.getdata("update RequestUpdate201 set [Status] = 'Approved',ActionResponse = GETDATE() where Id = " + row.Cells[0].Text + "");
                DataTable dtcolumn = dbhelper.getdata("select Failed from RequestUpdate201 where Id = " + row.Cells[0].Text + "");
                dbhelper.getdata("update MEmployee set " + dtcolumn.Rows[0]["Failed"] + " = (select Code from RequestUpdate201 where Id = " + row.Cells[0].Text + ") where Id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                DataTable dtemp = dbhelper.getdata("select LastName+', '+FirstName+' '+MiddleName+' '+ExtensionName as empname from MEmployee where Id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                if (dtcolumn.Rows[0]["Failed"].ToString() == "FirstName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "LastName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "MiddleName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "ExtensionName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + row.Cells[0].Text + ")");
                }
            }
        }
        string query = "select a.Id,(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id =a.IdNumber)FullName,a.Failed,a.[Status],left(convert(varchar,a.DateRequest,101),10)RequestDate,a.Request,a.Code from RequestUpdate201 a where [Status] = 'For Approval'";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.Id,(select LastName+', '+FirstName+' '+MiddleName from MEmployee where Id =a.IdNumber)FullName,a.Failed,a.[Status],left(convert(varchar,a.DateRequest,101),10)RequestDate,a.Request,a.Code from RequestUpdate201 a where a.Status != 'Cancel' and LEFT(CONVERT(varchar,a.DateRequest,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("request", false);
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
                a.sc = "Masterfile Update";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update RequestUpdate201 set Status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='request'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Row.!'); window.location='request'", true);
    }

    protected void approve_all(object sender, EventArgs e)
    {
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, 0);
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");

            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();

                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "Masterfile Update";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                a.sf = grid_view.Rows[i].Cells[1].Text;
                bol.system_logs(a);

                DataTable getinfo = dbhelper.getdata("select * from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + "");
                dbhelper.getdata("insert into sys_notification([date],[subject],[to],[key],url,[status],[type],content)values"
                    + "(GETDATE(),'MASTERFILE REQUEST'," + getinfo.Rows[0]["IdNumber"] + ",'3','KIOSK_Masterfile','0','0','" + getinfo.Rows[0]["Failed"].ToString() + " was Approved. " + "')");
                dbhelper.getdata("update RequestUpdate201 set [Status] = 'Approved',ActionResponse = GETDATE() where Id = " + grid_view.Rows[i].Cells[0].Text + "");
                DataTable dtcolumn = dbhelper.getdata("select Failed from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + "");
                dbhelper.getdata("update MEmployee set " + dtcolumn.Rows[0]["Failed"] + " = (select Code from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ") where Id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");

                DataTable dtemp = dbhelper.getdata("select LastName+', '+FirstName+' '+MiddleName+' '+ExtensionName as empname from MEmployee where Id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                if (dtcolumn.Rows[0]["Failed"].ToString() == "FirstName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "LastName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "MiddleName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                }
                else if (dtcolumn.Rows[0]["Failed"].ToString() == "ExtensionName")
                {
                    dbhelper.getdata("update MUser set FullName = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                    dbhelper.getdata("update nobel_user set name = '" + dtemp.Rows[0]["empname"] + "' where emp_id = (select IdNumber from RequestUpdate201 where Id = " + grid_view.Rows[i].Cells[0].Text + ")");
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='request'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Row.!'); window.location='request'", true);
            }
        }
    }
}