using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_Manager_approve_rest : System.Web.UI.Page
{
    //public static string user_id, query;
    //public DataTable dt;
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
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        //query = "select Id,BranchId,DepartmentId,FirstName from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
        //dt = dbhelper.getdata(query);

        //query = "select a.id,a.date,a.sysdate,a.reason,a.status, " +
        //      "b.lastname+' '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
        //      //"c.Leave " +
        //      "from TRestdaylogs a " +
        //      "left join MEmployee b on a.EmployeeId=b.Id " +
        //      //"left join MLeave c on a.LeaveId=c.Id " +
        //      "where a.status like '%for approval%' and b.reportto=" + Session["emp_id"] + "   order by a.date desc";
        ////and b.BranchId='" + dt.Rows[0]["BranchId"].ToString() + "' and b.DepartmentId='" + dt.Rows[0]["DepartmentId"].ToString() + "'
        //DataSet ds = new DataSet();
        //ds = bol.display(query);
        //grid_view.DataSource = ds;
        //grid_view.DataBind();

        //div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;

        string query = "select a.id,a.date,a.sysdate,a.reason,a.status,a.approver_id, " +
              "b.lastname+' '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId, " +

              "d.under_id,d.herarchy,a.approver_id, " +
              "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id " +


              "from TRestdaylogs a " +
              "left join MEmployee b on a.EmployeeId=b.Id " +
              "left join approver d on a.EmployeeId=d.emp_id " +
              "where a.status like '%for approval%'  and a.approver_id=" + approver + " and d.under_id=" + approver + " and d.herarchy <> 0 order by a.date desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        div_action.Visible = dt.Rows.Count == 0 ? false : true;

    }

    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

        string query = "select a.id,a.date,a.sysdate,a.reason,a.status,a.approver_id, " +
             "b.lastname+' '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId, " +
             "d.under_id,d.herarchy,a.approver_id, " +
             "case when (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=d.emp_id and status is null and herarchy =  d.herarchy + 1) end as nxt_id " +
             "from TRestdaylogs a " +
             "left join MEmployee b on a.EmployeeId=b.Id " +
             "left join approver d on a.EmployeeId=d.emp_id " +
             "where a.status like '%for approval%'  and a.approver_id=" + Session["emp_id"] + " and d.under_id=" + Session["emp_id"] + " " +
             "and LEFT(CONVERT(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
             "order by a.date desc";
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
                a.sc = "RD";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update TRestdaylogs set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='rd'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='rd'", true);

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
                a.sc = "RD";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                if (grid_view.Rows[i].Cells[7].Text == "0")
                {
                    DataTable xx = dbhelper.getdata("select * from allow_admin where id='4' and allow='no'");
                    if (xx.Rows.Count == 0)
                    {
                        query += "update TRestdaylogs set status='Verification' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                        function.AddNotification("RD/HD Verification", "Restdayverification", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                    }
                    else
                    {
                        query += "update TRestdaylogs set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "";

                        DataTable dtrdlogs = dbhelper.getdata("select * from TRestdaylogs where id=" + grid_view.Rows[i].Cells[0].Text + "");
                        DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + grid_view.Rows[i].Cells[8].Text + "");
                        DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(grid_view.Rows[i].Cells[3].Text, dtemp.Rows[0]["PayrollGroupId"].ToString()));
                        if (dtchkpayroll.Rows.Count == 1)
                        {
                            DataTable dtr = Core.DTRF(grid_view.Rows[i].Cells[3].Text, grid_view.Rows[i].Cells[3].Text, "KIOSK_" + grid_view.Rows[i].Cells[8].Text);
                            decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) : 0 : 0;
                            if (amttt > 0)
                            {
                                DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(grid_view.Rows[i].Cells[3].Text, grid_view.Rows[i].Cells[8].Text));
                                if (decimal.Parse(chkifnaa.Rows[0]["netamount"].ToString()) == 0)
                                {
                                    DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(grid_view.Rows[i].Cells[3].Text, grid_view.Rows[i].Cells[0].Text, grid_view.Rows[i].Cells[8].Text, "" + dtrdlogs.Rows[0]["class"].ToString() + "", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                                }
                            }
                        }
                    }
                }
                else
                {
                    query += "update TRestdaylogs set approver_id='" + grid_view.Rows[i].Cells[7].Text + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    function.AddNotification("RD/HD Approval", "rd", grid_view.Rows[i].Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), grid_view.Rows[i].Cells[0].Text);
                }
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='rd'", true);
            }
            //else
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='al'", true);
        }

    }

    protected void delete2(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                function.ReadNotification(row.Cells[0].Text, int.Parse(Session["user_id"].ToString()));

                DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[7].Text + "");
                if (row.Cells[7].Text == "0")
                {
                     DataTable xx = dbhelper.getdata("select * from allow_admin where id='4' and allow='no'");
                     if (xx.Rows.Count == 0)
                     {
                         dbhelper.getdata("update TRestdaylogs set status='Verification' where Id=" + row.Cells[0].Text + "");
                         function.AddNotification("RD/HD Verification", "Restdayverification", row.Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                     }
                     else
                     {
                         dbhelper.getdata("update TRestdaylogs set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");

                         DataTable dtrdlogs = dbhelper.getdata("select * from TRestdaylogs where id=" + row.Cells[0].Text + "");
                         DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + row.Cells[8].Text + "");
                         DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(row.Cells[3].Text, dtemp.Rows[0]["PayrollGroupId"].ToString()));
                         if (dtchkpayroll.Rows.Count == 1)
                         {
                             DataTable dtr = Core.DTRF(row.Cells[3].Text, row.Cells[3].Text, "KIOSK_" + row.Cells[8].Text);
                             decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) : 0 : 0;
                             if (amttt > 0)
                             {
                                 DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(row.Cells[3].Text, row.Cells[8].Text));
                                 if (decimal.Parse(chkifnaa.Rows[0]["netamount"].ToString()) == 0)
                                 {
                                     DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(row.Cells[3].Text, row.Cells[0].Text, row.Cells[8].Text, "" + dtrdlogs.Rows[0]["class"].ToString() + "", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                                 }
                             }
                         }
                     }
                }
                else
                {
                    dbhelper.getdata("update TRestdaylogs set approver_id='" + row.Cells[7].Text + "' where Id=" + row.Cells[0].Text + "");
                    function.AddNotification("RD/HD Approval", "rd", row.Cells[7].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                }
               
               

                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "RD";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='rd'", true);
            }
        }

    }

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, int.Parse(Session["user_id"].ToString()));
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "RD";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update TRestdaylogs set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',reason='" + txt_reason.Text.Replace("'", "") + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='rd'", true);
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
        Response.Redirect("rd", false);
    }
}