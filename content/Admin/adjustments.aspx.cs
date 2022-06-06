using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Admin_adjustments : System.Web.UI.Page
{
    protected void transition()
    {
        grid_adjustment.Visible = false;
        grid_overtime.Visible = false;
        grid_CS.Visible = false;
        grid_hd.Visible = false;
        grid_travel.Visible = false;
        grid_leave.Visible = false;
        grid_undertime.Visible = false;
    }

    protected void choice()
    {
        switch (dl_choice.Text)
        {

            case "Time Adjustment":
                time_adjustment();
                grid_adjustment.Visible = true;
                break;
            case "Overtime":
                Overtime_disp();
                grid_overtime.Visible = true;
                break;
            case "CWS":
                changeShift_disp();
                grid_CS.Visible = true;
                break;
            case "Work Verification":
                workverification();
                grid_hd.Visible = true;
                break;
            case "OBT":
                obt();
                grid_travel.Visible = true;
                break;
            case "Leave":
                leave();
                grid_leave.Visible = true;
                break;
            case "Under Time":
                undertime();
                grid_undertime.Visible = true;
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
        string query = "select a.EmployeeId,a.id,left(convert(varchar,a.sysdate,101),10)sysdate,a.note,b.idnumber, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date_log,convert(datetime,a.time_in)time_in,convert(datetime,a.time_out)time_out,a.time_in1,a.time_out1,c.manual_type as reason " +
                        "from tmanuallogline a " +
                        "left join Memployee b on a.employeeid=b.id " +
                        "left join time_adjustment c on a.reason=c.id " +
                        "where a.status like '%approved%' " +
                        "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                        "order by a.date desc";
            DataTable dt = dbhelper.getdata(query);
            grid_adjustment.DataSource = dt;
            grid_adjustment.DataBind();

            alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void Overtime_disp()
    {
                       string query = "select a.id, LEFT(CONVERT(varchar,a.sysdate,101),10)date,LEFT(CONVERT(varchar,a.date,101),10)date_ot,a.OvertimeHours, " +
                        "a.OvertimeNightHours,a.remarks,a.status,a.time_in,a.time_out, " +
                        "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
                        "from TOverTimeLine a " +
                        "left join MEmployee b on a.EmployeeId=b.Id " +
                        "where a.status like '%approved%' " +
                        "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                        "order by a.date desc ";
        DataTable dt = dbhelper.getdata(query);
        grid_overtime.DataSource = dt;
        grid_overtime.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;

    }
    protected void changeShift_disp()
    {
        string query = "select distinct(a.emp_id), b.lastname+', '+b.firstname+' '+b.middlename+' '+b.extensionname as Employee_Name, "
            + "c.id,f.Id TCSID,c.changeshift_id, case when LEFT(CONVERT(varchar,f.date,101),10) is null then LEFT(CONVERT(varchar,c.date_change,101),10) "
            +"else LEFT(CONVERT(varchar,f.date,101),10) end as date_filed, c.remarks,c.shiftcode_id as shiftidTo, d.ShiftCode As shiftcodeTo,"
            +"d.ShiftCode as bb,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null"
            +" then h.ShiftCode else e.ShiftCode end as aa, f.ShiftCodeId as shiftidFrom from approver a left join memployee b on a.emp_id=b.id "
            +"left join temp_shiftcode c on b.id = c.emp_id left join MShiftCode d on c.shiftcode_id=d.Id left join TChangeShiftLine f on "
            + "c.date_change=f.Date and c.emp_id=f.EmployeeId and c.shiftcode_id=f.ShiftCodeId left join MShiftCode e on f.ShiftCodeId=e.Id left join MShiftCode h on c.changeshift_id=h.Id where "
            +"c.status like '%Approved%' and f.Date between '" + txt_from.Text + "' and '" + txt_to.Text + "' order by c.id desc";

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
                   "where a.status like '%Approved%' " +
                   "and left(convert(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                   "order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_hd.DataSource = dt;
        grid_hd.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void obt()
    {
       string query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
               "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname " +
               "from Ttravel a " +
               "left join MEmployee b on a.emp_id=b.Id " +
               "where a.status like '%Approved%'  " +
               "and LEFT(CONVERT(varchar,a.travel_start,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
               "order by a.id desc ";


        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_travel.DataSource = ds;
        grid_travel.DataBind();

        alert.Visible = grid_travel.Rows.Count == 0 ? true : false;
    }
    protected void leave()
    {
       string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
               "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
               "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
               "c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
               "d.Leave, " +
               "e.Department " +
               "from Tleave a " +
               "left join TLeaveApplicationLine b on a.id=b.l_id " +
               "left join MEmployee c on b.EmployeeId=c.id " +
               "left join MLeave d on b.LeaveId=d.Id " +
               "left join MDepartment e on c.DepartmentId=e.id " +
               "left join approver f on b.EmployeeId=f.emp_id " +
               "where b.status like '%Approved%' " +
               "and (select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
               "order by a.id desc ";
        
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_leave.DataSource = ds;
        grid_leave.DataBind();

        alert.Visible = grid_leave.Rows.Count == 0 ? true : false;
    }
    protected void undertime()
    {
        string query = "select a.id,b.id as emp_id,LEFT(CONVERT(varchar,a.date_filed,101),10)date_filed, " +
               "a.time,a.reason, " +
               "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
               ",case when a.dtunder IS null then LEFT(CONVERT(varchar,a.date_filed,101),10)+' '+a.time else a.dtunder  end  timeout " +
               "from Tundertime a " +
               "left join MEmployee b on a.emp_id=b.Id " +
               "where a.status like '%Approved%' " +
               "and LEFT(CONVERT(varchar,a.date_filed,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
               "order by a.date_filed desc ";

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
                      "a.status like'%Approved%' " +
                      "and left(convert(varchar,a.appliedfrom,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                      "order by a.date desc ";

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
                "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[6].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);

                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update tmanuallogline set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',note='cancel by admin' where id=" + row.Cells[0].Text + " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);
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
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[5].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {
                    dbhelper.getdata("update temp_shiftcode set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='Cancelled by Administrator' where id=" + row.Cells[0].Text + " ");
                    dbhelper.getdata("update TChangeShiftLine set status = '" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks= 'Cancelled by Administrator' where Id = " + row.Cells[1].Text + "");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);

              
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
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
               "where a.status is null and CONVERT(date,b.dateend)>='" + row.Cells[2].Text + "' and b.status is null";
                DataTable dtchkpayroll = dbhelper.getdata(ggg);
                if (dtchkpayroll.Rows.Count == 0)
                {

                    dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='cancel by admin' where l_id=" + row.Cells[0].Text + " ");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);



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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);



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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancel Successfully'); window.location='adjustments'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Payroll Already Process!'); window.location='adjustments'", true);


               
            }
        }
    }

}