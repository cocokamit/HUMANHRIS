using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;



public partial class content_payroll_payrollsummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {

            dtrid.Value = function.Decrypt(Request.QueryString["dtrid"].ToString(), true);
            pgid.Value = function.Decrypt(Request.QueryString["pg"].ToString(), true);
            ds.Value = function.Decrypt(Request.QueryString["ds"].ToString(), true);
            de.Value = function.Decrypt(Request.QueryString["de"].ToString(), true);
            hdn_selected_id.Value = Request.QueryString["tar"].ToString();
            this.disp();
           
            //Response.Redirect(Request.RawUrl);
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
       // string uri = Page.Request.Url.ToString();
       
    }
    protected void disp()
    {
        DataTable dtshiftcode = dbhelper.getdata("select * from mshiftcode");
        ViewState["mastershift"] = dtshiftcode;
       

        string query = "Select *,'"+function.Encrypt(dtrid.Value,true)+"' dtrid,'"+function.Encrypt(ds.Value,true)+"'ds,'"+function.Encrypt(de.Value,true)+"'de from (" +
        "select  ROW_NUMBER() OVER(ORDER BY a.Id ASC) AS Row," +
        "a.departmentid, a.IdNumber,a.LastName+', '+ a.FirstName+' '+ a.MiddleName e_name, a.id empid, " +
        "cast(round((select SUM( case when [Absent] = 'True'  then 1  else 0 end + case when HalfdayAbsent = 'True'  then 0.5  else 0 end ) from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id),18)as numeric(18,2)) Absent_Day, " +
        "cast(round((select case when SUM(TardyLateHours)is null then 0 else SUM(TardyLateHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id),18)as numeric(18,2)) Late_Hours, " +
        "cast(round((select case when SUM(TardyUndertimeHours)is null then 0 else SUM(TardyUndertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id),18) as numeric(18,2)) Undertime_Hours, " +
        "cast(round((select case when SUM(NumberOfHours) is null then 0 else SUM(NumberOfHours * setupbasichrs) end  from TLeaveApplicationLine where CONVERT(date,[date])>=CONVERT(date,'" + ds.Value + "')  and  CONVERT(date,[date])<=CONVERT(date,'" + de.Value + "') and WithPay='False' and status like'%Approved%' and EmployeeId=a.id),18) as numeric(18,2)) LWOP_Hours, " +

         "cast(round((select case when SUM(RegularHours+totaloffsethrs)is null then 0 else SUM(RegularHours+totaloffsethrs) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id),18) as numeric(18,2)) total_reg_render, " +
        //--offset hrs
        "cast(round((select case when SUM(totaloffsethrs)is null then 0 else SUM(totaloffsethrs) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and totaloffsethrs>0),18) as numeric(18,2)) offsetHRS, " +
        // "--Regular Working Day id 1 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=1),18) as numeric(18,2)) Regular_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=1),18) as numeric(18,2)) Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=1),18) as numeric(18,2)) Regular_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=1),18) as numeric(18,2)) Night_OT_HRS, " +

        // "--Regular Restday id 1 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=1),18) as numeric(18,2)) RD_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=1),18) as numeric(18,2)) RD_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=1),18) as numeric(18,2)) RD_Regular_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=1),18) as numeric(18,2)) RD_Night_OT_HRS, " +

        // " --Special Holiday id 3 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=3),18) as numeric(18,2)) SH_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=3),18) as numeric(18,2)) SH_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=3),18) as numeric(18,2)) SH_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=3),18) as numeric(18,2)) SH_Night_OT_HRS, " +

        //" --Special Restday id 3 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=3),18) as numeric(18,2)) SH_RD_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=3),18) as numeric(18,2)) SH_RD_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=3),18) as numeric(18,2)) SH_RD_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=3),18) as numeric(18,2)) SH_RD_Night_OT_HRS, " +


        //" --Legal Holiday id 4 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=4),18) as numeric(18,2)) Legal_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=4),18) as numeric(18,2)) Legal_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=4),18) as numeric(18,2)) Legal_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=4),18) as numeric(18,2)) Legal_Night_OT_HRS, " +

        //" --Legal Restday id 4 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=4),18) as numeric(18,2)) Legal_RD_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=4),18) as numeric(18,2)) Legal_RD_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=4),18) as numeric(18,2)) Legal_RD_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=4),18) as numeric(18,2)) Legal_RD_Night_OT_HRS, " +

        //" --Double Holiday id 5 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=5),18) as numeric(18,2)) Double_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=5),18) as numeric(18,2)) Double_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=5),18) as numeric(18,2)) Double_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='False' and DayTypeId=5),18) as numeric(18,2)) Double_Night_OT_HRS, " +

        //" --Double Restday id 5 " +
        "cast(round((select case when SUM(RegularHours)is null then 0 else SUM(RegularHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=4),18) as numeric(18,2)) Double_RD_HRS, " +
        "cast(round((select case when SUM(NightHours)is null then 0 else SUM(NightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=5),18) as numeric(18,2)) Double_RD_Night_HRS, " +
        "cast(round((select case when SUM(OvertimeHours)is null then 0 else SUM(OvertimeHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=5),18) as numeric(18,2)) Double_RD_OT_HRS, " +
        "cast(round((select case when SUM(OvertimeNightHours)is null then 0 else SUM(OvertimeNightHours) end from TDTRLine where DTRId=" + dtrid.Value + " and EmployeeId=a.id and RestDay='True' and DayTypeId=5),18) as numeric(18,2)) Double_RD_Night_OT_HRS, " +


        "cast(round((select case when SUM(NumberOfHours) is null then 0 else SUM(NumberOfHours * setupbasichrs) end  from TLeaveApplicationLine where CONVERT(date,[date])>=CONVERT(date,'" + ds.Value + "')  and  CONVERT(date,[date])<=CONVERT(date,'" + de.Value + "') and WithPay='True' and status like'%Approved%' and EmployeeId=a.id),18) as numeric(18,2)) LWP_Hours " +

        "from memployee a " +
       // "left join Mstore b on a.branchid=b.id " +
        "where a.payrollgroupid=" + pgid.Value + " " +
        ") tt  order by Absent_Day desc "; //Late_Hours+Undertime_Hours+LWOP_Hours+
        DataTable dt = dbhelper.getdata(query);
        DataTable dtdept = dbhelper.getdata("Select * from mdepartment");
        DataTable final = new DataTable();
        DataTable dttv = new DataTable();
        DataRow dr_final;
        final = dt.Copy();
        final.Rows.Clear();


        foreach (DataRow dr in dtdept.Rows)
        {
            DataView dv = new DataView();
            DataRow[] drtransfer = dt.Select(" departmentid=" + dr["id"].ToString() + "");
            if (drtransfer.Count() > 0)
            {
                dr_final = final.NewRow();
                final.Rows.Add(dr_final);
                final.Rows[final.Rows.Count - 1][3] = dr["Department"].ToString();
                DataTable dummy = drtransfer.CopyToDataTable();
                dv = dummy.AsDataView();
                dv.Sort = " e_name asc";
                dttv = dv.ToTable();

                decimal reg_hrs = 0; decimal night_hrs = 0; decimal reg_ot_hrs = 0; decimal night_ot_hrs = 0; decimal lwp_hrs = 0;
                decimal absent_hrs = 0; decimal late_hrs = 0; decimal undertime_hrs = 0; decimal lwop_hrs = 0;
              
                foreach (DataRow drrr1 in dttv.Rows)
                {
                    dr_final = final.NewRow();
                    dr_final.ItemArray = drrr1.ItemArray;
                    final.Rows.Add(dr_final);
                }
            }
        }
        ViewState["testing"] = final;
        grid_view.DataSource = final;
        grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        string query = "select b.IdNumber,b.IdNumber+' - '+b.LastName+', '+ b.FirstName+' '+ b.MiddleName e_name,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)date, " +
        "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
        "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
        "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
        "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
        "a.OnLeave,a.HalfLeave,a.Absent,a.HalfdayAbsent,a.RegularHours,a.NightHours,a.OvertimeHours,a.OvertimeNightHours, " +
        "a.TardyLateHours,a.TardyUndertimeHours,a.RegularAmount,a.NightAmount,a.OvertimeAmount,a.OvertimeNightAmount,a.LAmount,a.UAmount,a.RDAmount,a.HDAmount,a.AbsentAmount,a.LeaveAmount,a.NetAmount,a.totaloffsethrs,' '  " +
        "from TDTRLine a " +
        "left join MEmployee b on a.EmployeeId=b.Id " +
        "left join MShiftCode c on a.ShiftCodeId=c.Id " +
        "where a.DTRId=" + dtrid.Value + " and (b.FirstName+''+ b.MiddleName+''+ b.LastName+''+b.IdNumber) like'%" + txt_search.Text + "%' order by  b.IdNumber, a.DATE asc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
      

    }
    protected void OnRowDataBoundgrid1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
     

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();


            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Basic Information";
            cell.BackColor = System.Drawing.Color.Gray;
            row.Cells.Add(cell);

            //row.Cells[4].BackColor = System.Drawing.Color.Yellow;

            //cell = new TableHeaderCell();
            //cell.ColumnSpan = 1;
            //cell.HorizontalAlign = HorizontalAlign.Center;
            //cell.Text = "test";
            //cell.BackColor = System.Drawing.Color.LightSteelBlue;
            //row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Worked/Absent/Tardiness/Undertime/LWOP/LWP";
            cell.BackColor = System.Drawing.Color.LightSteelBlue;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Regular Working Day";
            cell.BackColor = System.Drawing.Color.Orange;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Regular (RD)";
            cell.BackColor = System.Drawing.Color.Orange;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Special Holiday";
            cell.BackColor = System.Drawing.Color.LightGreen;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Special Holiday (RD)";
            cell.BackColor = System.Drawing.Color.LightGreen;
            row.Cells.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Legal Holiday";
            cell.BackColor = System.Drawing.Color.SkyBlue;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Legal Holiday (RD)";
            cell.BackColor = System.Drawing.Color.SkyBlue;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Double Holiday";
            cell.BackColor = System.Drawing.Color.PeachPuff;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Double Holiday (RD)";
            cell.BackColor = System.Drawing.Color.PeachPuff;
            row.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Offsetting";
            cell.BackColor = System.Drawing.Color.LightGreen;
            row.Cells.Add(cell);

            grid_view.Controls[0].Controls.AddAt(0, row);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[1].Text.Contains("&nbsp;"))
            {
                e.Row.Cells[2].BackColor = System.Drawing.Color.Yellow;
                e.Row.Cells[2].Style.Add("font-weight", "bold");
                e.Row.Cells[2].Style.Add("color", "black");
        
                e.Row.Cells[0].Visible=false;
            }
            //if (e.Row.Cells[3].Text.Contains("Total"))
            //{
            //    e.Row.BackColor = System.Drawing.Color.Yellow;
            //    e.Row.Style.Add("font-weight", "bold");
            //    e.Row.Style.Add("font-variant", "small-caps");
            //    e.Row.Style.Add("color", "black");
            //    e.Row.Cells[0].Style.Add("display", "none");
            //}
        }
        
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select 'DTR-Summary-'+convert(varchar,id)+'-'+convert(varchar,CONVERT(date,DateStart))+'-'+convert(varchar,CONVERT(date,DateEnd))filename from tdtr where id=" + dtrid.Value + "");
        if (dt.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["filename"].ToString() + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                this.disp();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grid_view.Style.Add("text-transform", "uppercase");
                grid_view.RenderControl(hw);
                string style = @"<style> .textmode { text-transform:uppercase } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
            Response.Write("<script>alert('No Data to be downloaded!')</script>");
    }
}