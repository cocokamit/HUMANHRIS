using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_detailsperdtr : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            dtrid.Value = function.Decrypt(Request.QueryString["dtrid"].ToString(), true);
            disp();
        }
    }
    protected void disp()
    {
        string query = "select b.IdNumber,b.IdNumber+' - '+ b.LastName+', '+ b.FirstName+' '+ b.MiddleName e_name,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)date, " +
            "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
            "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
            "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
            "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
            "a.OnLeave,a.HalfLeave,a.Absent,a.HalfdayAbsent,a.RegularHours,a.NightHours,a.OvertimeHours,a.OvertimeNightHours, " +
            "a.TardyLateHours,a.TardyUndertimeHours,a.RegularAmount,a.NightAmount,a.OvertimeAmount,a.OvertimeNightAmount,a.LAmount,a.UAmount,a.RDAmount,a.HDAmount,a.AbsentAmount,a.LeaveAmount,a.NetAmount,a.totaloffsethrs  " +
            "from TDTRLine a " +
            "left join MEmployee b on a.EmployeeId=b.Id " +
            "left join MShiftCode c on a.ShiftCodeId=c.Id " +
            "where a.DTRId=" + dtrid.Value + " order by  b.IdNumber, a.DATE asc";
            DataTable dt = dbhelper.getdata(query);
            grid_view.DataSource = dt;
            grid_view.DataBind();
            grid_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            grid_view.UseAccessibleHeader = true;

    }
    protected void search(object sender,EventArgs e)
    {
        string query = "select b.IdNumber,b.IdNumber+' - '+b.LastName+', '+ b.FirstName+' '+ b.MiddleName e_name,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)date, " +
       "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
            "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
            "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
            "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
        "a.OnLeave,a.HalfLeave,a.Absent,a.HalfdayAbsent,a.RegularHours,a.NightHours,a.OvertimeHours,a.OvertimeNightHours, " +
        "a.TardyLateHours,a.TardyUndertimeHours,a.RegularAmount,a.NightAmount,a.OvertimeAmount,a.OvertimeNightAmount,a.LAmount,a.UAmount,a.RDAmount,a.HDAmount,a.AbsentAmount,a.LeaveAmount,a.NetAmount,a.totaloffsethrs  " +
        "from TDTRLine a " +
        "left join MEmployee b on a.EmployeeId=b.Id " +
        "left join MShiftCode c on a.ShiftCodeId=c.Id " +
        "where a.DTRId=" + dtrid.Value + " and (b.FirstName+''+ b.MiddleName+''+ b.LastName+''+b.IdNumber) like'%" + txt_search.Text + "%' order by  b.IdNumber, a.DATE asc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        grid_view.HeaderRow.TableSection = TableRowSection.TableHeader;
        grid_view.UseAccessibleHeader = true;

    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[3].Text == "True")
            {
                e.Row.Cells[3].Text = "✓";
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[3].Text = "--";

            //if (e.Row.Cells[8].Text == "True")
            //{
            //    e.Row.Cells[8].Text = "✓";
            //    e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
            //}
            //else
            //    e.Row.Cells[8].Text = "--";

            //if (e.Row.Cells[9].Text == "True")
            //{
            //    e.Row.Cells[9].Text = "✓";
            //    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
               
            //}
            //else
            //    e.Row.Cells[9].Text = "--";

            if (e.Row.Cells[10].Text == "True")
            {
                e.Row.Cells[10].Text = "✓";
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[10].Text = "--";

            if (e.Row.Cells[11].Text == "True")
            {
                e.Row.Cells[11].Text = "✓";
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[11].Text = "--";

            if (e.Row.Cells[12].Text == "True")
            {
                e.Row.Cells[12].Text = "✓";
                e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[12].Text = "--";

            if (e.Row.Cells[13].Text == "True")
            {
                e.Row.Cells[13].Text = "✓";
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[13].Text = "--";





            //if (decimal.Parse(e.Row.Cells[12].Text) >0)
            //    e.Row.Cells[12].ForeColor = System.Drawing.Color.Blue;
            //if (decimal.Parse(e.Row.Cells[13].Text) > 0)
            //    e.Row.Cells[13].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[14].Text) > 0)
                e.Row.Cells[14].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[15].Text) > 0)
                e.Row.Cells[15].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[16].Text) > 0)
                e.Row.Cells[16].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[17].Text) > 0)
                e.Row.Cells[17].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[18].Text) > 0)
                e.Row.Cells[18].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[19].Text) > 0)
                e.Row.Cells[19].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[20].Text) > 0)
                e.Row.Cells[20].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[21].Text) > 0)
                e.Row.Cells[21].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[22].Text) > 0)
                e.Row.Cells[22].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[23].Text) > 0)
                e.Row.Cells[23].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[24].Text) > 0)
                e.Row.Cells[24].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[25].Text) > 0)
                e.Row.Cells[25].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[26].Text) > 0)
                e.Row.Cells[26].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[27].Text) > 0)
                e.Row.Cells[27].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[28].Text) > 0)
                e.Row.Cells[28].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[29].Text) > 0)
                e.Row.Cells[29].ForeColor = System.Drawing.Color.Blue;
            if (decimal.Parse(e.Row.Cells[30].Text) > 0)
                e.Row.Cells[30].ForeColor = System.Drawing.Color.Blue;
        }
    }
}