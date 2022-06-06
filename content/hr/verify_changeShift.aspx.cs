using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verify_changeShift : System.Web.UI.Page
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
        //string query = "select distinct(a.emp_id), " +
        //    //"(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //               "b.lastname+' '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //               "c.id,c.changeshift_id,LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
        //               "c.remarks,c.shiftcode_id as shiftidTo, " +
        //               "SUBSTRING(d.ShiftCode,0,CHARINDEX('(',d.ShiftCode)) As shiftcodeTo,d.ShiftCode as bb, " +
        //               "SUBSTRING(e.ShiftCode,0,CHARINDEX('(',e.ShiftCode)) As shiftcodeFrom,e.ShiftCode as aa, " +
        //               "f.ShiftCodeId as shiftidFrom " +
        //               "from " +
        //               "approver a " +
        //               "left join memployee b on a.emp_id=b.id " +
        //               "left join temp_shiftcode c on b.id = c.emp_id " +
        //               "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //               "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //               "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //               "where c.status like '%verification%' ";
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        //string query = "select distinct(a.emp_id), " +
        //    //"(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //              "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //              "c.id,c.changeshift_id,LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
        //              "c.remarks,c.shiftcode_id as shiftidTo, " +
        //              "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //              "e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //              "f.ShiftCodeId as shiftidFrom " +
        //              "from " +
        //              "approver a " +
        //              "left join memployee b on a.emp_id=b.id " +
        //              "left join temp_shiftcode c on b.id = c.emp_id " +
        //              "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //              "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //              "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //              "where c.status like '%Verification%' order by c.id desc";

                       //"where c.id is not null and c.status like '%for approval%' " +
                       //"and (select COUNT(*) from sys_applog where app_id=c.id and request_id=b.id and [TYPE]='CS' and status like '%Approve%') >= 1";

        //string query = "select distinct(a.emp_id), " +
        //    //"(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //             "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //             "c.id,c.changeshift_id, " +
        //              "case when LEFT(CONVERT(varchar,c.date_change,101),10) is null then LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,c.date_change,101),10) end as date_filed, " +
        //             "c.remarks,c.shiftcode_id as shiftidTo, " +
        //             "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //    //"e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //             "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
        //             "f.ShiftCodeId as shiftidFrom " +
        //             "from " +
        //             "approver a " +
        //             "left join memployee b on a.emp_id=b.id " +
        //             "left join temp_shiftcode c on b.id = c.emp_id " +
        //             "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //             "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //             "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //               "left join MShiftCode h on c.changeshift_id=h.Id " +
        //             "where c.status like '%Verification%' order by c.id desc";

        string query = "select distinct(c.emp_id),b.Id as empidd, " +
                        "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
                        "a.id,a.changeshift_id, " +
                        "case when LEFT(CONVERT(varchar,a.date_change,101),10) is null then " +
                        "LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,a.date_change,101),10) end as date_filed, " +
                        "a.remarks,a.shiftcode_id as shiftidTo, " +
                        "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
                        "case when e.ShiftCode is null then h.ShiftCode " +
                        "else e.ShiftCode end As shiftcodeFrom, " +
                        "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
                        "f.ShiftCodeId as shiftidFrom " +
                        "from " +
                        "temp_shiftcode a " +
                        "left join memployee b on a.emp_id=b.id " +
                        "left join approver c on a.emp_id = c.emp_id " +
                        "left join MShiftCode d on a.shiftcode_id=d.Id " +
                        "left join TChangeShiftLine f on a.changeshift_id=f.Id " +
                        "left join MShiftCode e on f.ShiftCodeId=e.Id " +
                        "left join MShiftCode h on a.changeshift_id=h.Id " +
                        "where a.status like '%verification%' " +
                        "order by a.id desc";

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

       
            
    }

    protected void search(object sender, EventArgs e)
    {
        //string query = "select distinct(a.emp_id), " +
        //              "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //              "c.id,c.changeshift_id,LEFT(CONVERT(varchar,c.date_change,101),10)date_filed, " +
        //              "c.remarks,c.shiftcode_id as shiftidTo, " +
        //              "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //              "e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //              "f.ShiftCodeId as shiftidFrom " +
        //              "from " +
        //              "approver a " +
        //              "left join memployee b on a.emp_id=b.id " +
        //              "left join temp_shiftcode c on b.id = c.emp_id " +
        //              "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //              "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //              "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //              "where c.status like '%Verification%' " +
        //              "and LEFT(CONVERT(varchar,c.date_change,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
        //              "order by c.id desc";

        //string query = "select distinct(a.emp_id), " +
        //    //"(select COUNT(*) from sys_applog where app_id=c.id and emp_id=a.under_id and request_id=b.id and [TYPE]='CS'), " +
        //              "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
        //              "c.id,c.changeshift_id, " +
        //               "case when LEFT(CONVERT(varchar,c.date_change,101),10) is null then LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,c.date_change,101),10) end as date_filed, " +
        //              "c.remarks,c.shiftcode_id as shiftidTo, " +
        //              "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
        //    //"e.ShiftCode As shiftcodeFrom,e.ShiftCode as aa, " +
        //              "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
        //              "f.ShiftCodeId as shiftidFrom " +
        //              "from " +
        //              "approver a " +
        //              "left join memployee b on a.emp_id=b.id " +
        //              "left join temp_shiftcode c on b.id = c.emp_id " +
        //              "left join MShiftCode d on c.shiftcode_id=d.Id " +
        //              "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
        //              "left join MShiftCode e on f.ShiftCodeId=e.Id " +
        //                "left join MShiftCode h on c.changeshift_id=h.Id " +
        //              "where c.status like '%Verification%' " +
        //              "and  case when LEFT(CONVERT(varchar,c.date_change,101),10) is null then LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,c.date_change,101),10) end between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
        //              "order by c.id desc";

        string query = "select distinct(c.emp_id),b.Id as empidd, " +
                        "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
                        "a.id,a.changeshift_id, " +
                        "case when LEFT(CONVERT(varchar,a.date_change,101),10) is null then " +
                        "LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,a.date_change,101),10) end as date_filed, " +
                        "a.remarks,a.shiftcode_id as shiftidTo, " +
                        "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb, " +
                        "case when e.ShiftCode is null then h.ShiftCode " +
                        "else e.ShiftCode end As shiftcodeFrom, " +
                        "case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
                        "f.ShiftCodeId as shiftidFrom " +
                        "from " +
                        "temp_shiftcode a " +
                        "left join memployee b on a.emp_id=b.id " +
                        "left join approver c on a.emp_id = c.emp_id " +
                        "left join MShiftCode d on a.shiftcode_id=d.Id " +
                        "left join TChangeShiftLine f on a.changeshift_id=f.Id " +
                        "left join MShiftCode e on f.ShiftCodeId=e.Id " +
                        "left join MShiftCode h on a.changeshift_id=h.Id " +
                        "where a.status like '%verification%' " +
                        "and  case when LEFT(CONVERT(varchar,a.date_change,101),10) is null then LEFT(CONVERT(varchar,f.date,101),10) else LEFT(CONVERT(varchar,a.date_change,101),10) end between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                        "order by a.id desc";
        
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='vcs'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='vcs'", true);

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
                a.sb = Session["emp_id"].ToString();
                a.sc = "CS";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                a.sf = grid_view.Rows[i].Cells[1].Text;
                bol.system_logs(a);

                string aa = "select * from TchangeshiftLine where [date]=left(convert(varchar,'" + grid_view.Rows[i].Cells[5].Text + "',101),10) and employeeId='" + grid_view.Rows[i].Cells[1].Text + "' "; //and ShiftCodeId='" + grid_view.Rows[i].Cells[2].Text + "'
                DataTable hh = new DataTable();
                hh = dbhelper.getdata(aa);

                if (hh.Rows.Count == 0)
                {
                    query += "insert into TChangeShiftLine (ChangeShiftId,EmployeeId,Date,ShiftCodeId,Remarks,status,dtr_id) values (0," + grid_view.Rows[i].Cells[1].Text + ",'" + grid_view.Rows[i].Cells[5].Text + "','" + grid_view.Rows[i].Cells[2].Text + "','scheduler','approved',NULL) ";
                    query += "update temp_shiftcode set status='Approved' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    dbhelper.getdata(query);
                }
                else
                {

                    query += "update TChangeShiftLine set ShiftCodeId='" + grid_view.Rows[i].Cells[2].Text + "' where Id=" + grid_view.Rows[i].Cells[3].Text + "";
                    query += "update temp_shiftcode set status='Approved' where Id=" + grid_view.Rows[i].Cells[0].Text + "";
                    dbhelper.getdata(query);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vcs'", true);
            }

        }
    } 

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);

            idd.Value = grid_view.Rows[0].Cells[0].Text;
            req_id.Value = grid_view.Rows[0].Cells[1].Text;
            Div1.Visible = true;
            Div2.Visible = true;

        }
    }

    protected void delete2(object sender, EventArgs e)
    {

        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);
            if (TextBox1.Text == "Yes")
            {
                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "CS";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                a.sf = row.Cells[1].Text;
                bol.system_logs(a);

                string aa = "select * from TchangeshiftLine where [date]=left(convert(varchar,'" + row.Cells[5].Text + "',101),10) and employeeId='" + row.Cells[1].Text + "'";// and ShiftCodeId='" + row.Cells[2].Text + "'
                DataTable hh = new DataTable();
                hh = dbhelper.getdata(aa);

                if (hh.Rows.Count == 0)
                {
                    string query = "insert into TChangeShiftLine (ChangeShiftId,EmployeeId,Date,ShiftCodeId,Remarks,status,dtr_id) values (0," + row.Cells[1].Text + ",'" + row.Cells[5].Text + "','" + row.Cells[2].Text + "','scheduler','approved',NULL) ";
                    query += "update temp_shiftcode set status='Approved' where Id=" + row.Cells[0].Text + "";
                    dbhelper.getdata(query);
                }
                else
                {
                    string query = "update TChangeShiftLine set ShiftCodeId='" + row.Cells[2].Text + "' where Id=" + row.Cells[3].Text + "";
                    query += "update temp_shiftcode set status='Approved' where Id=" + row.Cells[0].Text + "";
                    dbhelper.getdata(query);
                }

                // dbhelper.getdata("update temp_shiftcode set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[2].Text + "");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vcs'", true);
            }
        }

    }

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "CS";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text;
        a.sf = req_id.Value;
        bol.system_logs(a);

        dbhelper.getdata("update temp_shiftcode set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancelled Successfully'); window.location='vcs'", true);


    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("vcs", false);
    }
}