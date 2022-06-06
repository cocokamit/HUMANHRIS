using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verify_offset : System.Web.UI.Page
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

        string query = "select a.id,b.id as emp_id ,b.IdNumber,b.LastName+', '+b.FirstName+' '+b.MiddleName as fullname,left(convert(varchar,a.date,101),10)date,left(convert(varchar,a.appliedfrom,101),10)appliedfrom, left(convert(varchar,a.appliedto,101),10)appliedto,a.offsethrs,a.status " +
                        "from toffset a " +
                        "left join memployee b on a.empid=b.Id " +
                        "where " +
                        "a.status like'%verification%' order by a.date desc";
        DataTable dt = new DataTable();
        dt = dbhelper.getdata(query);

        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void search(object sender, EventArgs e)
    {

        string query = "select a.id,b.id as emp_id ,b.IdNumber,b.LastName+', '+b.FirstName+' '+b.MiddleName as fullname,left(convert(varchar,a.date,101),10)date,left(convert(varchar,a.appliedfrom,101),10)appliedfrom, left(convert(varchar,a.appliedto,101),10)appliedto,a.offsethrs,a.status " +
                      "from toffset a " +
                      "left join memployee b on a.empid=b.Id " +
                      "where " +
                      "a.status like'%verification%' " +
                      "and left(convert(varchar,a.appliedfrom,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                      "order by a.date desc ";

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
                a.sb = "0";
                a.sc = "OF";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);

                query += "update toffset set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='vo'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='vo'", true);

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
                a.sc = "OF";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);

                dbhelper.getdata("update toffset set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
                //string date = grid_view.Rows[i].Cells[5].Text;
                //string dayname = Convert.ToDateTime(date).DayOfWeek.ToString();
                //string empid = grid_view.Rows[i].Cells[1].Text;
                //string noh = grid_view.Rows[i].Cells[6].Text;

                //DataTable dtgetday = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + getshiftcode.shiftcode(grid_view.Rows[i].Cells[5].Text, empid) + " and Day='" + dayname + "' ");
                //DataTable dtgetdaytype = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(varchar,a.date,101),10)='" + date + "' and a.status is null ");
                //DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + empid + " ");

                //string daymultiplier;
                //string nightmultiplier;
                //if (dtgetdaytype.Rows.Count > 0)
                //{
                //    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                //    {
                //        daymultiplier = dtgetdaytype.Rows[0]["WorkingDays"].ToString();
                //        nightmultiplier = dtgetdaytype.Rows[0]["NightWorkingDays"].ToString();
                //    }
                //    else
                //    {
                //        daymultiplier = dtgetdaytype.Rows[0]["RestdayDays"].ToString();
                //        nightmultiplier = dtgetdaytype.Rows[0]["NightRestdayDays"].ToString();
                //    }
                //}
                //else
                //{
                //    DataTable dtgetworkingday = dbhelper.getdata("select * from MDayType where id=1");
                //    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                //    {
                //        daymultiplier = dtgetworkingday.Rows[0]["WorkingDays"].ToString();
                //        nightmultiplier = dtgetworkingday.Rows[0]["NightWorkingDays"].ToString();
                //    }
                //    else
                //    {
                //        daymultiplier = dtgetworkingday.Rows[0]["RestdayDays"].ToString();
                //        nightmultiplier = dtgetworkingday.Rows[0]["NightRestdayDays"].ToString();
                //    }
                //}
                //decimal regamt = (decimal.Parse(dtgetemp.Rows[0]["HourlyRate"].ToString()) * decimal.Parse(daymultiplier)) * decimal.Parse(noh);
                //string ggg = "select * from TPayroll a " +
                //                "left join tdtr b on a.DTRId=b.Id " +
                //                "where a.status is null and CONVERT(date,b.dateend)>='" + date + "' and b.status is null";
                //DataTable dtchkpayroll = dbhelper.getdata(ggg);
                //if (dtchkpayroll.Rows.Count > 0)
                //{
                //    DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(grid_view.Rows[i].Cells[0].Text, empid, "Offset", daymultiplier, nightmultiplier, dtgetemp.Rows[0]["HourlyRate"].ToString(), noh.ToString(), "0", regamt.ToString(), "0"));
                //}
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vo'", true);
            }

        }
    }

    protected void approved_offset(object sender, EventArgs e)
    {
        
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                //Read Notification
                function.ReadNotification(row.Cells[0].Text, 0);
                if (TextBox1.Text == "Yes")
                {
               
                dbhelper.getdata("update toffset set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");
               
                //string date = row.Cells[5].Text;
                //string dayname = Convert.ToDateTime(date).DayOfWeek.ToString();
                //string empid = row.Cells[1].Text;
                //string noh = row.Cells[6].Text;

                //DataTable dtgetday = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + getshiftcode.shiftcode(row.Cells[5].Text, empid) + " and Day='" + dayname + "' ");
                //DataTable dtgetdaytype = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(varchar,a.date,101),10)='" + date + "' and a.status is null ");
                //DataTable dtgetemp = dbhelper.getdata("select * from memployee where id=" + empid + " ");

                //string daymultiplier;
                //string nightmultiplier;
                //if (dtgetdaytype.Rows.Count > 0)
                //{
                //    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                //    {
                //        daymultiplier = dtgetdaytype.Rows[0]["WorkingDays"].ToString();
                //        nightmultiplier = dtgetdaytype.Rows[0]["NightWorkingDays"].ToString();
                //    }
                //    else
                //    {
                //        daymultiplier = dtgetdaytype.Rows[0]["RestdayDays"].ToString();
                //        nightmultiplier = dtgetdaytype.Rows[0]["NightRestdayDays"].ToString();
                //    }
                //}
                //else
                //{
                //    DataTable dtgetworkingday = dbhelper.getdata("select * from MDayType where id=1");
                //    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                //    {
                //        daymultiplier = dtgetworkingday.Rows[0]["WorkingDays"].ToString();
                //        nightmultiplier = dtgetworkingday.Rows[0]["NightWorkingDays"].ToString();
                //    }
                //    else
                //    {
                //        daymultiplier = dtgetworkingday.Rows[0]["RestdayDays"].ToString();
                //        nightmultiplier = dtgetworkingday.Rows[0]["NightRestdayDays"].ToString();
                //    }
                //}
                //decimal regamt = (decimal.Parse(dtgetemp.Rows[0]["HourlyRate"].ToString()) * decimal.Parse(daymultiplier)) * decimal.Parse(noh);
                //DataTable dtchkpayroll = dbhelper.getdata("select * from TPayroll a where a.status is null and LEFT(CONVERT(varchar,a.payrolldate,101),10)>='" + date + "'");
                //if (dtchkpayroll.Rows.Count > 0)
                //{
                //    DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(row.Cells[0].Text, empid, "Offset", daymultiplier, nightmultiplier, dtgetemp.Rows[0]["HourlyRate"].ToString(), noh.ToString(), "0", regamt.ToString(), "0"));
                //}
                    stateclass a = new stateclass();
                    a.sa = row.Cells[0].Text;
                    a.sb = Session["emp_id"].ToString();
                    a.sc = "OF";
                    a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                    a.se = "ok";
                    bol.system_logs(a);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vo'", true);
                }
                else
                { }
            }
        
    }
    protected void delete2(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                if (row.Cells[10].Text == "0")
                    dbhelper.getdata("update TOverTimeLine set status='" + "For Verification-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");
                else
                    dbhelper.getdata("update TOverTimeLine set approver_id='" + row.Cells[10].Text + "' where Id=" + row.Cells[0].Text + "");
                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='ao'", true);
            }
        }

    }

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        //Read Notification
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "OF";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update toffset set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='vo'", true);
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
        Response.Redirect("ao", false);
    }
}