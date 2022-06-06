﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_approve_shift : System.Web.UI.Page
{
    //public static string query, user_id, id;
    //public static DataTable dt;
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
        //string query = "select a.emp_id, " +
        //              "(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //              "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //              "c.id,c.changeshift_id,LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
        //              "c.shiftcode_id as shiftidTo,c.remarks, " +
        //              "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //              "e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //              "f.ShiftCodeId as shiftidFrom,LEFT(CONVERT(varchar,f.date,101),10)date, " +

        //              "a.under_id,a.herarchy,c.approver_id, " +
        //              "case when (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) end as nxt_id " +


        //              "from " +
        //              "approver a " +
        //              "left join memployee b on a.emp_id=b.id " +
        //              "left join temp_shiftcode c on b.id = c.emp_id " +
        //              "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //              "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //              "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //               "where c.status like '%for approval%'  and c.approver_id=" + Session["emp_id"].ToString() + " and a.under_id=" + Session["emp_id"].ToString() + "  order by c.date_change desc";


                       //"where a.under_id='" + Session["emp_id"].ToString() + "' " +
                       //"and c.id is not null " +
                       //"and (select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS') = 0  order by c.date_change desc ";

        string query = "select a.emp_id, " +
                     "(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
                     "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
                     "c.id,c.changeshift_id, " +
                     "LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
                     "c.shiftcode_id as shiftidTo,c.remarks, " +
                     "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
            //"e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
                     "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
                     "case when LEFT(CONVERT(varchar,f.date,101),10) is null then LEFT(CONVERT(varchar,c.date_change,101),10) else LEFT(CONVERT(varchar,f.date,101),10) end as date, " +

                     "f.ShiftCodeId as shiftidFrom, " +
                     "a.under_id,a.herarchy,c.approver_id, " +
                     "case when (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) end as nxt_id " +


                     "from " +
                     "approver a " +
                     "left join memployee b on a.emp_id=b.id " +
                     "left join temp_shiftcode c on b.id = c.emp_id " +
                     "left join MShiftCode d on c.shiftcode_id=d.Id " +
                     "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
                     "left join MShiftCode e on f.ShiftCodeId=e.Id " +
                      "left join MShiftCode h on c.changeshift_id=h.Id " +
                      "where c.status like '%for approval%'  and c.approver_id=" + Session["emp_id"].ToString() + " and a.under_id=" + Session["emp_id"].ToString() + "  order by c.date_change desc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_action.Visible = dt.Rows.Count == 0 ? false : true;

      

    }


    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        //string query = "select a.emp_id, " +
        //             "(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //             "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //             "c.id,c.changeshift_id,LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
        //             "c.shiftcode_id as shiftidTo,c.remarks, " +
        //             "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //             "e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //             "f.ShiftCodeId as shiftidFrom,LEFT(CONVERT(varchar,f.date,101),10)date, " +

        //             "a.under_id,a.herarchy,c.approver_id, " +
        //             "case when (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) end as nxt_id " +


        //             "from " +
        //             "approver a " +
        //             "left join memployee b on a.emp_id=b.id " +
        //             "left join temp_shiftcode c on b.id = c.emp_id " +
        //             "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //             "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //             "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //             "where c.status like '%for approval%'  and c.approver_id=" + Session["emp_id"].ToString() + " and a.under_id=" + Session["emp_id"].ToString() + "  " +
        //             "and LEFT(CONVERT(varchar,f.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
        //              //"and convert(datetime,c.date_change) between convert(datetime,'" + txt_from.Text + "') and  convert(datetime,'" + txt_to.Text + "') " +
        //             "order by c.date_change desc";

        string query = "select a.emp_id, " +
                     "(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
                     "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
                     "c.id,c.changeshift_id, " +
                     "LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
                     "c.shiftcode_id as shiftidTo,c.remarks, " +
                     "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +

                     "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
                     "case when LEFT(CONVERT(varchar,f.date,101),10) is null then LEFT(CONVERT(varchar,c.date_change,101),10) else LEFT(CONVERT(varchar,f.date,101),10) end as date, " +

                     "f.ShiftCodeId as shiftidFrom, " +
                     "a.under_id,a.herarchy,c.approver_id, " +
                     "case when (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=a.emp_id and status is null and herarchy =  a.herarchy + 1) end as nxt_id " +


                     "from " +
                     "approver a " +
                     "left join memployee b on a.emp_id=b.id " +
                     "left join temp_shiftcode c on b.id = c.emp_id " +
                     "left join MShiftCode d on c.shiftcode_id=d.Id " +
                     "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
                     "left join MShiftCode e on f.ShiftCodeId=e.Id " +
                      "left join MShiftCode h on c.changeshift_id=h.Id " +
                    "where c.status like '%for approval%'  and c.approver_id=" + Session["emp_id"].ToString() + " and a.under_id=" + Session["emp_id"].ToString() + "  " +
                    "and case when LEFT(CONVERT(varchar,f.date,101),10) is null then LEFT(CONVERT(varchar,c.date_change,101),10) else LEFT(CONVERT(varchar,f.date,101),10)end between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                    "order by c.date_change desc";


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
                a.sc = "CS";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update temp_shiftcode set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='acs'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='acs'", true);

    }

    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");

            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, int.Parse(Session["user_id"].ToString()));
            DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + grid_view.Rows[i].Cells[8].Text + "");
            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "CS";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                if (grid_view.Rows[i].Cells[8].Text == "0")
                {
                    query +="update temp_shiftcode set status='Verification' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("Change Shift Verification", "vcs", grid_view.Rows[i].Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                else
                {
                    query +="update temp_shiftcode set approver_id='" + grid_view.Rows[i].Cells[8].Text + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("Change Shift Approval", "acs", grid_view.Rows[i].Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='acs'", true);
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
            req_id.Value = row.Cells[1].Text;
            Div1.Visible = true;
            Div2.Visible = true;

            Div3.Visible = false;
            Div4.Visible = false;
        }
    }

    protected void approve(object sender, EventArgs e)
    {

        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                function.ReadNotification(row.Cells[0].Text, int.Parse(Session["user_id"].ToString()));

                DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[8].Text + "");
                if (row.Cells[8].Text == "0")
                {
                    dbhelper.getdata("update temp_shiftcode set status='Verification' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("Change Shift Verification", "vcs", row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }
                else
                {
                    dbhelper.getdata("update temp_shiftcode set approver_id='" + row.Cells[8].Text + "' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("Change Shift Approval", "acs", row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }
                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "CS";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                a.sf = row.Cells[1].Text;
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='acs'", true);
            }
        }


        //if (TextBox1.Text == "Yes")
        //{
        //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        //    {
        //        idd.Value = grid_view.Rows[0].Cells[0].Text;

        //        stateclass a = new stateclass();
        //        a.sa = row.Cells[0].Text;
        //        a.sb = Session["emp_id"].ToString();
        //        a.sc = "CS";
        //        a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
        //        a.se = "ok";
        //        a.sf = row.Cells[1].Text;
        //        bol.system_logs(a);
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='acs?user_id=" + function.Encrypt(key.Value, true) + "'", true);
        //    }
        //}
    }

    protected void view_img(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            idd.Value = row.Cells[0].Text;
            
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

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();

        function.ReadNotification(idd.Value, int.Parse(Session["user_id"].ToString()));

        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "CS";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        a.sf = req_id.Value;
        bol.system_logs(a);
        

        dbhelper.getdata("update temp_shiftcode set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='acs'", true);
    
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("acs", false);
    }
}