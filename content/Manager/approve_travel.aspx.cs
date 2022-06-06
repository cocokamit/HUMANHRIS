using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_approve_travel : System.Web.UI.Page
{
    //public static string query, id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, " +

                "d.under_id,d.herarchy,a.approver_id, " +
                "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id " +


                "from Ttravel a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "left join approver d on a.emp_id=d.emp_id " +
                "where a.status like '%for approval%' and a.approver_id=" + approver + " and d.under_id=" + approver + " and d.herarchy <> 0 order by a.date_input desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_action.Visible = dt.Rows.Count == 0 ? false : true;
    }

    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
    

                string query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, " +

                "d.under_id,d.herarchy,a.approver_id, " +
                "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id " +

                "from Ttravel a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "left join approver d on a.emp_id=d.emp_id " +
                "where a.status like '%for approval%' and a.approver_id=" + Session["emp_id"] + " and d.under_id=" + Session["emp_id"] + " " +
                "and LEFT(CONVERT(varchar,a.travel_start,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                "order by a.date_input desc";

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
                a.sc = "OBT";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update Ttravel set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='at'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='at'", true);

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
                a.sc = "OBT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                if (grid_view.Rows[i].Cells[7].Text == "0")
                {   
                    DataTable xx = dbhelper.getdata("select * from allow_admin where id='5' and allow='no'");
                    if (xx.Rows.Count == 0)
                    {
                        query += "update Ttravel set status='verification' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                        function.AddNotification("Travel Verification", "Travel", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                    }
                    else
                        query += "update Ttravel set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                }
                else
                {
                    query += "update Ttravel set approver_id='" + grid_view.Rows[i].Cells[7].Text + "' where id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("Travel Approval", "at", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='at'", true);
            }
            //else
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='al'", true);
        }

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

    protected void approve(object sender, EventArgs e)
    {
       
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                function.ReadNotification(row.Cells[0].Text, int.Parse(Session["user_id"].ToString()));

                DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[7].Text + "");
                if (row.Cells[7].Text == "0")
                {
                     DataTable xx = dbhelper.getdata("select * from allow_admin where id='5' and allow='no'");
                     if (xx.Rows.Count == 0)
                     {
                         dbhelper.getdata("update Ttravel set status='Verification' where Id=" + row.Cells[0].Text + "");
                         function.AddNotification("Travel Verification", "Travel", row.Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                     }
                     else
                         dbhelper.getdata("update Ttravel set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");
                         
                }
                else
                {
                    dbhelper.getdata("update Ttravel set approver_id='" + row.Cells[7].Text + "' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("Travel Approval", "at", row.Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }

                function.Notification("obt", "approved", row.Cells[0].Text, row.Cells[7].Text);

                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OBT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='at'", true);
            }
        }
    }

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, int.Parse(Session["user_id"].ToString()));
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "OBT";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update Ttravel set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + idd.Value + "");
        function.Notification("obt", "declined", idd.Value, idd.Value);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='at'", true);
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("at?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        //stateclass a = new stateclass();

        //if (btn_save.Text == "Update")
        //{
        //    query = "update Ttravel_budget set meals='"+txt_meals.Text+"',transportation='"+txt_trans.Text+"',accommodation='"+txt_acom.Text+"',otherexpense='"+txt_other.Text+"',totalcashapproved='"+txt_cash.Text+"' where travel_id="+id+"  ";
        //    dbhelper.getdata(query);
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='at?user_id=" + function.Encrypt(key.Value, true) + "'", true);
        //}
        //else
        //{
        //    a.sa = id;
        //    a.sb = txt_meals.Text;
        //    a.sc = txt_trans.Text;
        //    a.sd = txt_acom.Text;
        //    a.se = txt_other.Text;
        //    a.sf = txt_cash.Text;
        //    bol.travel_budget(a);

        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='at?user_id=" + function.Encrypt(key.Value, true) + "';window.open('','_new').location.href='travel_form?&id=" + function.Encrypt(id, true) + "'", true);
        //}
    }
    protected void viewobt(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string query = "select a.id,b.id nxtid,(select convert(varchar,a.tDate,101))tvlDate,(select convert(varchar,b.date_input,101))fileDate,(select convert(varchar,a.obIn,22))tIn,(select convert(varchar,a.obOut,22))tOut from obtline a left join Ttravel b on a.tID = b.id where tID = " + row.Cells[0].Text + " and a.status = 'for approval'";
            DataTable dtall = dbhelper.getdata(query);
            gridobt.DataSource = dtall;
            gridobt.DataBind();
            pinfra.Visible = true;
            pinfra2.Visible = true;

        }
    }
    protected void closepopup(object sender, EventArgs e)
    {
        pinfra.Visible = false;
        pinfra2.Visible = false;
    }
    protected void cancelobt(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string query = "select a.id,b.id nxtid,(select convert(varchar,a.tDate,101))tvlDate,(select convert(varchar,b.date_input,101))fileDate,(select convert(varchar,a.obIn,22))tIn,(select convert(varchar,a.obOut,22))tOut from obtline a left join Ttravel b on a.tID = b.id where tID = " + row.Cells[1].Text + " and a.status = 'for approval'";
                DataTable dtall = dbhelper.getdata(query);
                if (dtall.Rows.Count > 1)
                {
                    dbhelper.getdata("update obtline set status = 'Cancelled' where id = " + row.Cells[0].Text + "");
                }
                else
                {
                    dbhelper.getdata("update obtline set status = 'Cancelled' where id = " + row.Cells[0].Text + "");
                    dbhelper.getdata("update Ttravel set status = 'Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id = " + row.Cells[1].Text + "");
                }
            }
        }
        Response.Redirect("at");
    }
}