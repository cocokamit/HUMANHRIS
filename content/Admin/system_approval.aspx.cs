using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_Admin_system_approval : System.Web.UI.Page
{
    protected void transition()
    {
        grid_adjustment.Visible = false;
        grid_preot.Visible = false;
        grid_overtime.Visible = false;
        grid_CS.Visible = false;
        grid_hd.Visible = false;
        grid_travel.Visible = false;
        grid_leave.Visible = false;
        grid_undertime.Visible = false;
        btngen_ot.Visible = false;
        btngen_ta.Visible = false;
        btngen_preot.Visible = false;
        btngen_cws.Visible = false;
        btngen_wv.Visible = false;
        btngen_obt.Visible = false;
        btngen_leave.Visible = false;
        btngen_undertime.Visible = false;
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void choice()
    {
        switch (dl_choice.Text)
        {
            case "Time Adjustment":
                time_adjustment();
                grid_adjustment.Visible = true;
                btngen_ta.Visible = true;
                break;
            case "Pre-Overtime":
                Pre_Overtime();
                grid_preot.Visible = true;
                btngen_preot.Visible = true;
                break;
            case "Overtime":
                Overtime_disp();
                grid_overtime.Visible = true;
                btngen_ot.Visible = true;
                break;
            case "CWS":
                changeShift_disp();
                grid_CS.Visible = true;
                btngen_cws.Visible = true;
                break;
            case "Work Verification":
                workverification();
                grid_hd.Visible = true;
                btngen_wv.Visible = true;
                break;
            case "OBT":
                obt();
                grid_travel.Visible = true;
                btngen_obt.Visible = true;
                break;
            case "Leave":
                leave();
                grid_leave.Visible = true;
                btngen_leave.Visible = true;
                break;
            case "Under Time":
                undertime();
                grid_undertime.Visible = true;
                btngen_undertime.Visible = true;
                break;
            case "Offset":
                offset();
                grid_offset.Visible = true;
                break;
        }
    }
    protected void change_choice(object sender, EventArgs e)
    {
        transition();
        choice();
    }
    protected void time_adjustment()
    {
        string query = "select a.EmployeeId,a.id,b.IdNumber,left(convert(varchar,a.sysdate,101),10)sysdate,a.note,b.idnumber, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date_log,convert(datetime,a.time_in)time_in,convert(datetime,a.time_out)time_out,a.time_in1,a.time_out1,c.manual_type as reason " +
                        "from tmanuallogline a " +
                        "left join Memployee b on a.employeeid=b.id " +
                        "left join time_adjustment c on a.reason=c.id " +
                        "where a.status like '%for approval%' " ;

                if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                    query += "order by a.date desc ";
                else
                {

                    if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                        query += "and a.EmployeeId='" + lbl_bals.Value + "' and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                    else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "and a.EmployeeId='" + lbl_bals.Value + "' ";
                    else
                        query += "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";


                    query += "order by a.date desc";
                }

        DataTable dt = dbhelper.getdata(query);
        grid_adjustment.DataSource = dt;
        grid_adjustment.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void Pre_Overtime()
    {
        string query = "select a.Id,b.IdNumber,left(convert(varchar,a.Sysdate,101),10)sysdate,a.status,left(convert(varchar,a.OTDate,101),10)OTDate,"
            + "b.LastName +', '+ b.FirstName +' '+ b.MiddleName as FullName,a.OvertimeHours,a.Remarks,c.Department from PreOTRequest a "
            + "left join MEmployee b on a.EmpId=b.Id left join MDepartment c on c.Id=b.DepartmentId "
            + "where a.Status like '%For Approval%' ";

        if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
            query += "order by a.date desc ";
        else
        {
            if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                query += "and a.EmpId='" + lbl_bals.Value + "' and left(convert(varchar,a.Sysdate,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
            else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                query += "and a.EmpId='" + lbl_bals.Value + "' ";
            else
                query += "and left(convert(varchar,a.Sysdate,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
            query += "order by a.Sysdate desc ";
        }
        DataTable dt = dbhelper.getdata(query);
        grid_preot.DataSource = dt;
        grid_preot.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void Overtime_disp()
    {
        string query = "select a.id,b.IdNumber, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot,a.OvertimeHours, " +
         "a.OvertimeNightHours,a.remarks,a.status,a.time_in,a.time_out, " +
         "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
         "from TOverTimeLine a " +
         "left join MEmployee b on a.EmployeeId=b.Id " +
         "where a.status like '%for approval%' ";

            if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                query += "order by a.date desc ";
            else
            {


                if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                    query += "and a.EmployeeId='" + lbl_bals.Value + "' and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                    query += "and a.EmployeeId='" + lbl_bals.Value + "' ";
                else
                    query += "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                query += "order by a.date desc ";
            }

        DataTable dt = dbhelper.getdata(query);
        grid_overtime.DataSource = dt;
        grid_overtime.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }
    protected void changeShift_disp()
    {
        string query = "select distinct(a.emp_id), " +
                    "b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, " +
                    "c.id,c.changeshift_id,b.IdNumber, " +
                     "LEFT(CONVERT(varchar,c.date_change,101),10) as date_filed, " +

                    "c.remarks,c.shiftcode_id as shiftidTo, " +
                    "d.ShiftCode +' ('+ d.Remarks + ')' As shiftcodeTo,d.remarks as bb, " +
                    "case when e.ShiftCode is null then h.ShiftCode +' ('+ h.Remarks + ')' else e.ShiftCode +' ('+ e.Remarks + ')' end As shiftcodeFrom,e.remarks as aa, " +

                    "f.ShiftCodeId as shiftidFrom " +
                    "from " +
                    "approver a " +
                    "left join memployee b on a.emp_id=b.id " +
                    "left join temp_shiftcode c on b.id = c.emp_id " +
                    "left join MShiftCode d on c.shiftcode_id=d.Id " +
                    "left join TChangeShiftLine f on c.changeshift_id=f.Id " +
                    "left join MShiftCode e on f.ShiftCodeId=e.Id " +
                    "left join MShiftCode h on c.changeshift_id=h.Id " +
                    "where c.status like '%for approval%' " +
                    "and c.emp_id='"+lbl_bals.Value+"' and LEFT(CONVERT(varchar,c.date_change,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                    "order by c.id desc";

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_CS.DataSource = ds;
        grid_CS.DataBind();

        alert.Visible = grid_CS.Rows.Count == 0 ? true : false;
    }
    protected void workverification()
    {
        string query = " SELECT left(convert(varchar,a.sysdate,101),10)sysdate,a.id,b.idnumber,a.employeeid, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date,a.reason FROM TRestdaylogs a " +
                   "left join Memployee b on a.employeeid=b.id " +
                   "where a.status like '%for approval%' ";

                    if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "order by a.id desc ";
                    else
                    {

                        if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                            query += "and a.employeeid='" + lbl_bals.Value + "' and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                            query += "and a.employeeid='" + lbl_bals.Value + "' ";
                        else
                            query += "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        query += "order by a.id desc ";
                    }

        DataTable dt = dbhelper.getdata(query);
        grid_hd.DataSource = dt;
        grid_hd.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void obt()
    {
        string query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname, b.IdNumber " +
                "from Ttravel a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "where a.status like '%for approval%'  ";


                if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                    query += "order by a.id desc ";
                else
                {

                    if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                        query += "and a.emp_id='" + lbl_bals.Value + "' and left(convert(varchar,a.travel_start,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                    else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "and a.emp_id='" + lbl_bals.Value + "' ";
                    else
                        query += "and left(convert(varchar,a.travel_start,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                    query += "order by a.id desc ";
                }



        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_travel.DataSource = ds;
        grid_travel.DataBind();

        alert.Visible = grid_travel.Rows.Count == 0 ? true : false;
    }
    protected void leave()
    {
        string query = "select b.EmployeeId request_id, b.l_id, b.id,a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status,left(convert(varchar,b.Date,101),10)Date_leave, " +
                    "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname,  " +
                    "d.Leave,c.IdNumber,  " +
                    "e.Department,b.remarks  " +
                    "from TLeaveApplicationLine b  " +
                    "left join TLeave a on b.l_id=a.id  " +
                    "left join MEmployee c on b.EmployeeId=c.id  " +
                    "left join MLeave d on b.LeaveId=d.Id  " +
                    "left join MDepartment e on c.DepartmentId=e.id  " +
                    "where b.status like '%for approval%' and b.dtr_id is null ";

                    
                    if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "order by b.Date desc ";
                    else
                    {

                        if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                            query += "and b.EmployeeId='" + lbl_bals.Value + "' and left(convert(varchar,b.Date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                            query += "and b.EmployeeId='" + lbl_bals.Value + "' ";
                        else
                            query += "and left(convert(varchar,b.Date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        query += "order by b.Date desc ";
                    }


        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_leave.DataSource = ds;
        grid_leave.DataBind();

        alert.Visible = grid_leave.Rows.Count == 0 ? true : false;
    }
    protected void undertime()
    {
        string query = "select a.id,b.id as emp_id,LEFT(CONVERT(varchar,a.date_filed,101),10)date_filed, " +
               "a.time,a.reason,b.IdNumber, " +
               "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
               ",case when a.dtunder IS null then LEFT(CONVERT(varchar,a.date_filed,101),10)+' '+a.time else a.dtunder  end  timeout " +
               "from Tundertime a " +
               "left join MEmployee b on a.emp_id=b.Id " +
               "where a.status like '%for approval%' ";

                if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                    query += "order by a.date_filed desc ";
                else
                {
                    if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                        query += "and a.emp_id='" + lbl_bals.Value + "' and left(convert(varchar,a.date_filed,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                    else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "and a.emp_id='" + lbl_bals.Value + "' ";
                    else
                        query += "and left(convert(varchar,a.date_filed,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                    query += "order by a.date_filed desc ";
                }

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_undertime.DataSource = ds;
        grid_undertime.DataBind();

        alert.Visible = grid_undertime.Rows.Count == 0 ? true : false;
    }
    protected void offset()
    {
        string query = "select a.id,b.id as emp_id ,b.IdNumber,b.LastName+', '+b.FirstName+' '+b.MiddleName as fullname,left(convert(varchar,a.date,101),10)date,left(convert(varchar,a.appliedfrom,101),10)appliedfrom, left(convert(varchar,a.appliedto,101),10)appliedto,a.offsethrs,a.status " +
                      "from toffset a " +
                      "left join memployee b on a.empid=b.Id " +
                      "where " +
                      "a.status like'%for approval%' ";

                    if (ddl_emp.Text == "" && txt_from.Text == "" && txt_to.Text == "")
                        query += "order by a.appliedfrom desc ";
                    else
                    {
                        if (ddl_emp.Text != "" && txt_from.Text != "" && txt_to.Text != "")
                            query += "and a.empid='" + lbl_bals.Value + "' and left(convert(varchar,a.appliedfrom,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        else if (ddl_emp.Text != "" && txt_from.Text == "" && txt_to.Text == "")
                            query += "and a.empid='" + lbl_bals.Value + "' ";
                        else
                            query += "and left(convert(varchar,a.appliedfrom,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' ";
                        query += "order by a.appliedfrom desc ";
                    }
                      

                     
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_offset.DataSource = ds;
        grid_offset.DataBind();
        alert.Visible = grid_offset.Rows.Count == 0 ? true : false;
    }

    protected void time_adjustment_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[3].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);

                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update tmanuallogline set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',note='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);
            }
        }
    }
    protected void preovertime_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[2].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update PreOTRequest set Status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);
            }
        }
    }
    protected void overtime_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[2].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update tovertimeline set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);
            }
        }
    }
    protected void cws_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
               "left join tdtr b on a.DTRId=b.Id " +
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[4].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update temp_shiftcode set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);


            }
        }
    }
    protected void hd_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
                "left join tdtr b on a.DTRId=b.Id " +
                "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[4].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update Trestdaylogs set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',reason='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);


            }
        }
    }
    protected void obt_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
               "left join tdtr b on a.DTRId=b.Id " +
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[4].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update Ttravel set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',notes='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);



            }
        }
    }
    protected void leave_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
               "left join tdtr b on a.DTRId=b.Id " +
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[4].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    stateclass a = new stateclass();
                    a.sa = row.Cells[1].Text; //leaveid
                    a.sb = "0";//emp_id
                    a.sc = "L";
                    a.sd = "Cancelled - " + DateTime.Now.ToShortDateString().ToString();
                    a.se = "Cancelled by Admin from Approved to Cancelled Date Leave (" + row.Cells[4].Text + ")";
                    a.sf = row.Cells[2].Text;//request_id
                    bol.system_logs(a);
                    dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);



            }
        }
    }
    protected void undertime_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
               "left join tdtr b on a.DTRId=b.Id " +
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[2].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update Tundertime set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',reason='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);



            }
        }
    }
    protected void offset_delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                string ggg = "select * from TPayroll a " +
              "left join tdtr b on a.DTRId=b.Id " +
              "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[4].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update toffset set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',remarks='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    transition();
                    choice();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='system_approval'", true);



            }
        }
    }



    protected void approve_manual(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update Tmanuallogline set status='Approved' where Id=" + row.Cells[0].Text + "");


                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "ML";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='am'", true);
            }
        }
    }
    protected void preapprove_ot(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update PreOTRequest set Status = 'Approved' where Id = " + row.Cells[0].Text + "");

                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "Pre-OT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='am'", true);
            }
        }
    }
    protected void approve_ot(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update TOverTimeLine set status='Approved' where Id=" + row.Cells[0].Text + "");


                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='am'", true);
            }
        }
    }

    protected void approve_cws(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update temp_shiftcode set status='Approved' where Id=" + row.Cells[0].Text + "");
                dbhelper.getdata("update TChangeShiftLine set ShiftCodeId='" + row.Cells[2].Text + "' where id='" + row.Cells[3].Text + "' ");

                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "CS";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='am'", true);
            }
        }
    }

    protected void approve_rd(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update TRestdaylogs set status='Approved' where Id=" + row.Cells[0].Text + "");

                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "RD";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='am'", true);
            }
        }
    }

    protected void approve_obt(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update Ttravel set status='Approved' where Id=" + row.Cells[0].Text + "");

                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OBT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
            }
        }
    }

    protected void approve_leave(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update TLeaveApplicationLine set status='Approved' where id=" + row.Cells[0].Text + "");

                stateclass a = new stateclass();
                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "L";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
            }
        }
    }

    protected void approve_ut(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update Tundertime set status='Approved' where Id=" + row.Cells[0].Text + "");

                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "UT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
            }
        }
    }

    protected void approve_off(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update toffset set status='Approved' where Id=" + row.Cells[0].Text + "");

                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OF";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "Approved by Admin";
                bol.system_logs(a);

                transition();
                choice();
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_undertime.PageIndex = e.NewPageIndex;
        grid_leave.PageIndex = e.NewPageIndex;
        grid_travel.PageIndex = e.NewPageIndex;
        grid_hd.PageIndex = e.NewPageIndex;
        grid_CS.PageIndex = e.NewPageIndex;
        grid_preot.PageIndex = e.NewPageIndex;
        grid_overtime.PageIndex = e.NewPageIndex;
        grid_adjustment.PageIndex = e.NewPageIndex;
    }
    protected void generatereport_undertime(object sender, EventArgs e)
    {
        if (grid_undertime.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=UnderTime.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_undertime.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_undertime.HeaderRow.Cells)
                {
                    cell.BackColor = grid_undertime.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_undertime.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_undertime.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_undertime.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_undertime.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_leave(object sender, EventArgs e)
    {
        if (grid_leave.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Leave.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_leave.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_leave.HeaderRow.Cells)
                {
                    cell.BackColor = grid_leave.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_leave.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_leave.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_leave.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_leave.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_obt(object sender, EventArgs e)
    {
        if (grid_travel.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OfficialBusinessTrip.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_travel.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_travel.HeaderRow.Cells)
                {
                    cell.BackColor = grid_travel.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_travel.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_travel.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_travel.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_travel.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_wv(object sender, EventArgs e)
    {
        if (grid_hd.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Work_Verification.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_hd.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_hd.HeaderRow.Cells)
                {
                    cell.BackColor = grid_hd.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_hd.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_hd.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_hd.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_hd.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_cws(object sender, EventArgs e)
    {
        if (grid_CS.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Change_Shift.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_CS.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_CS.HeaderRow.Cells)
                {
                    cell.BackColor = grid_CS.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_CS.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_CS.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_CS.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_CS.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_preot(object sender, EventArgs e)
    {
        if (grid_preot.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Pre_Overtime.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_preot.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_preot.HeaderRow.Cells)
                {
                    cell.BackColor = grid_preot.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_preot.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_preot.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_preot.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_preot.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_ot(object sender, EventArgs e)
    {
        if (grid_overtime.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Overtime.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_overtime.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_overtime.HeaderRow.Cells)
                {
                    cell.BackColor = grid_overtime.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_overtime.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_overtime.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_overtime.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_overtime.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
    protected void generatereport_ta(object sender, EventArgs e)
    {
        if (grid_adjustment.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=TimeAdjustment.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages

                grid_adjustment.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grid_adjustment.HeaderRow.Cells)
                {
                    cell.BackColor = grid_adjustment.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_adjustment.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_adjustment.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_adjustment.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grid_adjustment.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        Response.Write("<script>alert('Empty File!')</script>");
    }
}