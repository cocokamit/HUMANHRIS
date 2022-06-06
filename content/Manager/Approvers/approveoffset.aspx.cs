using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_approveoffset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.id, b.IdNumber,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,left(convert(varchar,a.date,101),10)date,left(convert(varchar,a.appliedfrom,101),10)appliedfrom, left(convert(varchar,a.appliedto,101),10)appliedto,a.offsethrs,a.status, " + 
                        "d.under_id,d.herarchy,a.aproverid, " +
                        "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id, " +
                        "case when appliedto is null then left(convert(varchar,appliedfrom,101),10) else left(convert(varchar,appliedto,101),10) end date_offset " +
                        "from toffset a " +
                        "left join memployee b on a.empid=b.Id " +
                        "left join approver d on a.empid=d.emp_id " +
                        "where " +
                        "a.status like'%For Approval%' and a.aproverid=" + Session["emp_id"] + " and d.under_id=" + Session["emp_id"] + " order by a.date desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_action.Visible = dt.Rows.Count == 0 ? false : true;
    }
    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.id,LEFT(CONVERT(varchar,a.date_filed,101),10)date_filed, " +
            "a.time,a.reason, " +
            "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId, " +
            "d.under_id,d.herarchy,a.approver_id, " +
            "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id, " +
            "case when appliedto is null then left(convert(varchar,appliedfrom,101),10) else left(convert(varchar,appliedto,101),10) end date_offset, " +
            ",case when a.dtunder IS null then LEFT(CONVERT(varchar,a.date_filed,101),10)+' '+a.time else a.dtunder  end  timeout " +
            "from Tundertime a " +
            "left join MEmployee b on a.emp_id=b.Id " +
            "left join approver d on a.emp_id=d.emp_id " +
            "where a.status like '%for approval%'  and a.approver_id=" + Session["emp_id"] + " and d.under_id=" + Session["emp_id"] + "  " +
            "and LEFT(CONVERT(varchar,a.date_filed,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
            "order by a.date_filed desc ";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
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
                a.sc = "UT";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update toffset set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='offset'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='offset'", true);

    }

    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");

            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, int.Parse(Session["user_id"].ToString()));
            DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + grid_view.Rows[i].Cells[7].Text + "");
            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OF";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                if (grid_view.Rows[i].Cells[7].Text == "0")
                {
                    query += "update toffset set status='verification' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("Offset Verification", "vo", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                else
                {
                    query += "update toffset set approver_id='" + grid_view.Rows[i].Cells[7].Text + "' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("Offset Approval", "offset", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='offset'", true);
            }
            //else
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='al'", true);
        }

    }
    protected void approved_offset(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                function.ReadNotification(row.Cells[0].Text, int.Parse(Session["user_id"].ToString()));
                DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[7].Text + "");
                if (row.Cells[7].Text == "0")
                {
                    dbhelper.getdata("update toffset set status='Verification' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("Offset Verification", "vo", row.Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }
                else
                {
                    dbhelper.getdata("update toffset set aproverid='" + row.Cells[7].Text + "' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("Offset Approval", "offset", row.Cells[7].Text == "0" ?"0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }
                
                

                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OF";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='offset'", true);
            }
        }
        else
        { }
    }
   

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();

        function.ReadNotification(idd.Value, int.Parse(Session["user_id"].ToString()));

        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "OF";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update toffset set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='offset'", true);
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            idd.Value = row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("ao", false);
    }
}