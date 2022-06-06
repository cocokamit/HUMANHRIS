using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_verify_meal : System.Web.UI.Page
{
    public static string query, id;
    public DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        query = "select Id,BranchId,DepartmentId,FirstName from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
        dt = dbhelper.getdata(query);

        query = "select a.id, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot, " +
              "a.remarks,a.status, " +
              "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
              "from TMealHours a " +
              "left join MEmployee b on a.EmployeeId=b.Id " +
              "where a.status like '%Verification%' order by a.date desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;

    }

    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        query = "select a.id, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot, " +
        "a.remarks,a.status, " +
        "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
        "from TMealHours a " +
        "left join MEmployee b on a.EmployeeId=b.Id " +
        "where a.status like '%Verification%' " +
        "and left(convert(varchar,a.date,101),10) between '" + txt_f.Text + "' and '" + txt_t.Text + "' " +
        "order by a.date desc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }

    protected bool checker()
    {
        bool oi = true;
        return oi;
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
    protected void delete2(object sender, EventArgs e)
    {
        string test = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            if (chkEmp.Checked == true)
            {
                test = "naa";
                dbhelper.getdata("update TOverTimeLine set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
            }
        }
        if (test.Trim().Length > 0)
        {
            //Credentials?user_id=" + function.Encrypt(user_id, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='al'", true);
        }
    }
    protected void delete3(object sender, EventArgs e)
    {
        string test = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            if (chkEmp.Checked == true)
            {
                test = "naa";
                //dbhelper.getdata("update TOverTimeLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
            }
        }
        if (test.Trim().Length > 0)
        {

            //Credentials?user_id=" + function.Encrypt(user_id, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully!'); window.location='ao'", true);
        }
    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("vmla", false);
    }
    protected void click_approved(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            stateclass a = new stateclass();
            view_id.Value = row.Cells[0].Text;
            function.ReadNotification(view_id.Value, 0);
            a.sa = view_id.Value;
            a.sb = Session["emp_id"].ToString();
            a.sc = "MA";
            a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
            a.se = "ok";
            bol.system_logs(a);
            dbhelper.getdata("update TMealHours set status = '" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + view_id.Value + "");

            function.Notification("mealOT", "approved", view_id.Value, row.Cells[7].Text);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vmla'", true);
        }
    }

    protected void click_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            view_id.Value = row.Cells[0].Text;
            hdn_empid.Value = row.Cells[7].Text;
            Div4.Visible = true;
        }
    }
    protected void click_can(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        //  function.ReadNotification(view_id.Value, 0);
        a.sa = view_id.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "MA";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update TMealHours set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + view_id.Value + "");
        function.Notification("mealOT", "declined", view_id.Value, hdn_empid.Value);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully!'); window.location='vmla'", true);

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
                a.sb = "0";
                a.sc = "MA";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update TMealHours set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
                function.Notification("mealOT", "declined", grid_view.Rows[i].Cells[0].Text, grid_view.Rows[i].Cells[7].Text);
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='vmla'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select row!'); window.location='vmla'", true);
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
                a.sb = "0";
                a.sc = "MA";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                dbhelper.getdata("update TMealHours set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
                function.Notification("mealOT", "approved", grid_view.Rows[i].Cells[0].Text, grid_view.Rows[i].Cells[7].Text);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vmla'", true);
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select row!'); window.location='vmla'", true);
    }
}