using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_workrdhd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session.Count == 0)
                Response.Redirect("quit?key=out");
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();

        }
    }
   
    protected void disp()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = " SELECT left(convert(varchar,a.sysdate,101),10)sysdate,a.id,b.idnumber,a.employeeid, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date,a.reason FROM TRestdaylogs a " +
                       "left join Memployee b on a.employeeid=b.id " +
                       "where a.status like '%verification%' order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }
    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = " SELECT left(convert(varchar,a.sysdate,101),10)sysdate,a.id,b.idnumber,a.employeeid, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date,a.reason FROM TRestdaylogs a " +//Case when a.class='RD' then 'Rest Day' else 'Holiday' end class
                       "left join Memployee b on a.employeeid=b.id " +
                       "where a.status like '%verification%' " +
                       "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                       "order by a.id desc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

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
                a.sb = "0";
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='Restdayverification'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='Restdayverification'", true);

    }
    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, 0);
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            string trnid = grid_view.Rows[i].Cells[0].Text;
            string date = grid_view.Rows[i].Cells[5].Text;
            string empid = grid_view.Rows[i].Cells[7].Text;
            if (chkEmp.Checked == true)
            {
               
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = "0";
                a.sc = "RD";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                dbhelper.getdata("update TRestdaylogs set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
                DataTable dtrdlogs = dbhelper.getdata("select * from TRestdaylogs where id=" + trnid + "");
                DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + empid + "");
                DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(date, dtemp.Rows[0]["PayrollGroupId"].ToString()));
                if (dtchkpayroll.Rows.Count == 1)
                {
                    DataTable dtr = Core.DTRF(date, date, "KIOSK_" + empid);
                    decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) : 0 : 0;
                    if (amttt > 0)
                    {
                        DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(date, empid));
                        if (decimal.Parse(chkifnaa.Rows[0]["netamount"].ToString()) == 0)
                        {
                            DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(date, trnid, empid, "" + dtrdlogs.Rows[0]["class"].ToString() + "", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                        }
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='Restdayverification'", true);
            }

        }
    }
    protected void click_add_whd(object sender, EventArgs e)
    {
        //  KOISK_addMANUAL? user_id = AzHhEjH7Q + U =
        Response.Redirect("KOISK_addRestday", false);
    }
    protected void delete2(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                function.ReadNotification(row.Cells[0].Text, 0);
                string trnid = row.Cells[0].Text;
                string date = row.Cells[5].Text;
                string empid = row.Cells[7].Text;
                dbhelper.getdata("update TRestdaylogs set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + trnid + "");
                DataTable dtrdlogs = dbhelper.getdata("select * from TRestdaylogs where id=" + trnid + "");
                DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + empid + "");
                DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(date, dtemp.Rows[0]["PayrollGroupId"].ToString()));
                if (dtchkpayroll.Rows.Count == 1)
                {
                    DataTable dtr = Core.DTRF(date, date, "KIOSK_" + empid);
                    decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) : 0 : 0;
                    if (amttt > 0)
                    {
                        DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(date, empid));
                        if (decimal.Parse(chkifnaa.Rows[0]["netamount"].ToString()) == 0)
                        {
                            DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(date, trnid, empid, "" + dtrdlogs.Rows[0]["class"].ToString() + "", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                        }
                    }
                }
                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = "0";
                a.sc = "RD";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='Restdayverification'", true);
            }
        }

    }

    protected void delete3(object sender, EventArgs e)
    {
        dbhelper.getdata("update TRestdaylogs set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + idd.Value + "");
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = "0";
        a.sc = "RD";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancelled Successfully'); window.location='Restdayverification'", true);


    }

    protected void view(object sender, EventArgs e)
    {
        idd.Value = grid_view.Rows[0].Cells[0].Text;
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("Restdayverification", false);
    }
}