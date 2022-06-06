using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class content_report_att_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
            grid_view.DataBind();
            if (grid_view.Rows.Count > 0)
                exp_report.Visible = true;
            else
                exp_report.Visible = false;
        }
    }
    protected void loadable()
    {
        ddl_year.Items.Clear();
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("Select Year", "0"));
        for (int i = 2017; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_year.Items.Add(new  System.Web.UI.WebControls.ListItem(i.ToString(),i.ToString()));
        }

        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto from payroll_range where action is null order by dfrom desc";
        DataTable dt_range = dbhelper.getdata(query);
        ddl_range.Items.Clear();
        ddl_range.Items.Add(new System.Web.UI.WebControls.ListItem("Select Payroll Range", "0"));
        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_range.Items.Add(new System.Web.UI.WebControls.ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }

        query = "select * from MDepartment order by id desc";
       DataTable dt = dbhelper.getdata(query);
        ddl_department.Items.Clear();
        ddl_department.Items.Add(new System.Web.UI.WebControls.ListItem("Select Department", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_department.Items.Add(new System.Web.UI.WebControls.ListItem(dr["department"].ToString(), dr["id"].ToString()));
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
            "left join MShiftCode c on a.ShiftCodeId=c.Id ";
            //"where a.DTRId=" + dtrid.Value + " order by  b.IdNumber, a.DATE asc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");

    }
    protected void search(object sender, EventArgs e)
    {
        string query = "";
        //string query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
        //"case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
        //"case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
        //"case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
        //"case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
        //"a.OnLeave,a.HalfLeave,a.Absent,a.HalfdayAbsent,a.RegularHours,a.NightHours,a.OvertimeHours,a.OvertimeNightHours, " +
        //"a.TardyLateHours,a.TardyUndertimeHours,a.RegularAmount,a.NightAmount,a.OvertimeAmount,a.OvertimeNightAmount,a.LAmount,a.UAmount,a.RDAmount,a.HDAmount,a.AbsentAmount,a.LeaveAmount,a.NetAmount,a.totaloffsethrs  " +
        //"from TDTRLine a " +
        //"left join MEmployee b on a.EmployeeId=b.Id " +
        //"left join MShiftCode c on a.ShiftCodeId=c.Id " +
        //"left join tdtr d on a.DTRId=d.Id " +
        //"left join mdepartment e on b.DepartmentId=e.id ";           
            string[] cutt = ddl_range.SelectedValue.ToString().Split('-');
             switch (rbl_filter.SelectedValue)
             { 
                 case "1":

                     query = "select " +
                            "b.IdNumber, b.LastName+', '+b.FirstName+' '+b.MiddleName EMPLOYEE,c.department DEPARTMENT, " +
                            "(select SUM(regothrs+nightothrs) from TPayrollLineBreakDown where payrolllineid=a.Id)OTHRS, " +
                            "(select SUM(regotamt+nightotamt) from TPayrollLineBreakDown where payrolllineid=a.Id)OTAMOUNT " +
                            "from TPayrollLine a  " +
                            "left join memployee b on a.EmployeeId=b.Id  " +
                            "left join mdepartment c on b.departmentid=c.Id  " +
                            "left join tdtr d on a.PayrollId=d.payroll_id ";
                     query += "where (select SUM(regothrs+nightothrs) from TPayrollLineBreakDown where payrolllineid=a.Id) > 0 ";
                   if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";


                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                  
                     break;
                 case "2":
                    query= "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                         "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
                         "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
                         "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
                         "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
                         "a.totaloffsethrs OffsetHRS  " +
                         "from TDTRLine a " +
                         "left join MEmployee b on a.EmployeeId=b.Id " +
                         "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                         "left join tdtr d on a.DTRId=d.Id " +
                         "left join mdepartment e on b.DepartmentId=e.id ";       

                     query += "where a.totaloffsethrs > 0 ";  
                      if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                     break;
                 case "3":
                     query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                         "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
                         "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
                         "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
                         "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
                         "(case when a.overbreak is null then 0 else a.overbreak end) OverbreakHrs  " +
                         "from TDTRLine a " +
                         "left join MEmployee b on a.EmployeeId=b.Id " +
                         "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                         "left join tdtr d on a.DTRId=d.Id " +
                         "left join mdepartment e on b.DepartmentId=e.id ";    
                     query += "where (case when a.overbreak is null then 0 else a.overbreak end) > 0 ";   
                      if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                     break;
                 case "4":
                     query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                         "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
                         "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
                         "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
                         "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
                         "a.TardyLateHours LateHRS " +
                         "from TDTRLine a " +
                         "left join MEmployee b on a.EmployeeId=b.Id " +
                         "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                         "left join tdtr d on a.DTRId=d.Id " +
                         "left join mdepartment e on b.DepartmentId=e.id ";      
                     query += "where a.TardyLateHours > 0 ";    
                      if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                     break;
                 case "5":
                     query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                         "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
                         "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
                         "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
                         "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
                         "a.TardyUndertimeHours UndertimeHRS " +
                         "from TDTRLine a " +
                         "left join MEmployee b on a.EmployeeId=b.Id " +
                         "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                         "left join tdtr d on a.DTRId=d.Id " +
                         "left join mdepartment e on b.DepartmentId=e.id ";    
                     query += "where a.TardyUndertimeHours > 0 ";  
                      if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                     break;
                 case "6":
                     //query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,left(convert(varchar,a.Date,101),10)Date, " +
                     //   " (case when a.OnLeave='True' then 8 else 0 end + case when a.HalfLeave='True' then 8 else 0 end) / 8 LeaveDays " +
                     //    "from TDTRLine a " +
                     //    "left join MEmployee b on a.EmployeeId=b.Id " +
                     //    "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                     //    "left join tdtr d on a.DTRId=d.Id " +
                     //    "left join mdepartment e on b.DepartmentId=e.id left join mleave f on a. ";      
                     query = "select c.LastName+', '+c.FirstName+' '+c.MiddleName Employee,d.Department,left(CONVERT(varchar,a.date,101),10)Date,e.Leave,a.NumberOfHours NoDAYS,a.WithPay,a.Remarks from TLeaveApplicationLine a " +
                            "left join Tleave b on a.LeaveId=b.id " +
                            "left join memployee c on a.EmployeeId=c.Id " +
                            "left join MDepartment d on c.DepartmentId=d.Id " +
                            "left join MLeave e on a.LeaveId=e.Id " +
                            "where a.status like'%Approved%' ";
                   
                      if (ddl_year.SelectedValue != "0")
                          query += " and YEAR(a.date)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += " and MONTH(a.date)=" + ddl_month.SelectedValue + " ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and convert(date,a.Date) between convert(date,'" + cutt[0] + "') and convert(date,'" + cutt[1] + "') ) ";
                      //   query += " and ('" + cutt[0] + "' between a.date and a.date or '" + cutt[1] + "' between a.date and a.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += " and c.IdNumber+''+c.IdNumber+''+c.LastName+''+ c.FirstName+''+ c.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " order by Employee asc";
                     break;
                 case "7":
                     query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                     " (case when a.Absent='True' then 8 else 0 end + case when a.HalfdayAbsent='True' then 8 else 0 end) / 8 AbsentDays " +
                      "from TDTRLine a " +
                      "left join MEmployee b on a.EmployeeId=b.Id " +
                      "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                      "left join tdtr d on a.DTRId=d.Id " +
                      "left join mdepartment e on b.DepartmentId=e.id ";      
                     query += "where (a.Absent ='True' or a.HalfdayAbsent ='True') ";   
                      if (ddl_year.SelectedValue != "0")
                         query += "and YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     if (ddl_month.SelectedValue != "0")
                         query += "and MONTH(d.DateStart)="+ddl_month.SelectedValue+" ";
                     if(ddl_range.SelectedValue!="0")
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     if (txt_search.Text.Length > 0)
                         query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%"+txt_search.Text+"%' ";

                     query += " and d.status is null and d.payroll_id is not null  order by Employee asc";
                     break;
                 case "8":
                     string condition = "";
                     string and = "";
                     query = "select b.IdNumber,b.LastName+', '+ b.FirstName+' '+ b.MiddleName Employee,e.department Department,c.ShiftCode ,a.RestDay,a.DayMultiplier,left(convert(varchar,a.Date,101),10)Date, " +
                            "case when a.TimeIn1 ='0' then '--'  else a.TimeIn1 end TimeIn1 , " +
                            "case when a.TimeOut1 ='0' then '--'  else a.TimeOut1 end TimeOut1 , " +
                            "case when a.TimeIn2 ='0' then '--'  else a.TimeIn2 end TimeIn2 , " +
                            "case when a.TimeOut2 ='0' then '--'  else a.TimeOut2 end TimeOut2 , " +
                            "a.OnLeave,a.HalfLeave,a.Absent,a.HalfdayAbsent,a.RegularHours,a.NightHours,a.OvertimeHours,a.OvertimeNightHours, " +
                            "a.TardyLateHours,a.TardyUndertimeHours,a.totaloffsethrs OffsetHRS  " +
                            "from TDTRLine a " +
                            "left join MEmployee b on a.EmployeeId=b.Id " +
                            "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                            "left join tdtr d on a.DTRId=d.Id " +
                            "left join mdepartment e on b.DepartmentId=e.id " +
                            "where d.status is null and d.payroll_id is not null ";

       
                     if (ddl_year.SelectedValue != "0")
                     {
                         query += " and  YEAR(d.DateStart)=" + ddl_year.SelectedValue + " ";
                     }
                     if (ddl_month.SelectedValue != "0")
                     {
                         query += " and MONTH(d.DateStart)=" + ddl_month.SelectedValue + " ";
                     }
                     if (ddl_range.SelectedValue != "0")
                     {
                         query += " and ('" + cutt[0] + "' between d.DateStart and d.DateEnd or '" + cutt[1] + "' between d.DateStart and d.DateEnd ) ";
                     }
                     if (txt_search.Text.Length > 0)
                     {
                         query += " and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%" + txt_search.Text + "%' ";
                     }
                    
                     query += " order by Employee asc";
                     break;
             }
            
            
          // query=" (b.FirstName+''+ b.MiddleName+''+ b.LastName+''+b.IdNumber) like'%" + txt_search.Text + "%' order by  b.IdNumber, a.DATE asc";

             DataTable dt = dbhelper.getdata(query);
             grid_view.DataSource = dt;
             grid_view.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
        ppop(false);
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;


        string year = int.Parse(ddl_year.SelectedValue) > 0 ? ddl_year.SelectedValue+">" : "";
        string month = int.Parse(ddl_month.SelectedValue) > 0 ? ddl_month.SelectedValue + ">" : "";
        string range = ddl_range.SelectedValue !="0" ? ddl_range.SelectedValue + ">" : "";
        string dept = int.Parse(ddl_department.SelectedValue) > 0 ? ddl_department.SelectedValue + ">" : "";
        string ser=txt_search.Text.Length >0 ? txt_search.Text : "";
        lbl_filter_info.Text = "Filter > " + year  + month + range +  dept +ser;

    }
    protected void getfilters(string query,string ffrom,string tto)
    {
        if (ddl_year.SelectedValue != "0")
            query += "and YEAR(DateStart)=" + ddl_year.SelectedValue + " ";
        if (ddl_month.SelectedValue != "0")
            query += "and MONTH(d.DateStart)=" + ddl_month.SelectedValue + " ";
        if (ddl_range.SelectedValue != "0")
            query += " and ('" + ffrom + "' between d.DateStart and d.DateEnd or '" + ffrom + "' between d.DateStart and d.DateEnd ) ";
        if (txt_search.Text.Length > 0)
            query += "and b.IdNumber+''+b.IdNumber+''+b.LastName+''+ b.FirstName+''+ b.MiddleName like '%'" + txt_search.Text + "'%' ";
        
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
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
            
           
        }
        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "Attendance_Report";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                //grid_view.AllowPaging = false;
                //this.disp();
                // this.DataBind();
                //grid_view.DataBind();
                //this.BindGrid();

                grid_view.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in grid_view.HeaderRow.Cells)
                {
                    cell.BackColor = grid_view.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid_view.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid_view.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid_view.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid_view.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        
    }
   

    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("att_report");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void filter(object sender, EventArgs e)
    {
        ppop(true);
    }
    protected void ExportGridToword(object sender, EventArgs e)
    {
      
        //DataTable dt = (DataTable)ViewState["hhh"] as DataTable; 

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Locations.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document document = new Document();

            PdfWriter.GetInstance(document, Response.OutputStream);

            document.Open();

            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.COURIER , 8);

            PdfPTable table = new PdfPTable(grid_view.Columns.Count);
            PdfPRow row = null;
            float[] widths = new float[] { 6f, 6f, 2f, 4f, 2f };

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            int iCol = 0;
            string colname = "";
            PdfPCell cell = new PdfPCell(new Phrase("Locations"));

            cell.Colspan = grid_view.Columns.Count;

            foreach (DataColumn c in grid_view.Columns)
            {

                table.AddCell(new Phrase(c.ColumnName, font5));
            }
            int i = 0;
            foreach (DataRow r in grid_view.Rows)
            {
                if (grid_view.Rows.Count > 0)
                {
                    table.AddCell(new Phrase(r[i].ToString(), font5));
                   
                }
                i++;
            }
            document.Add(table);
            document.Close();

            Response.Write(document);
            Response.End();
        } 
    //Response.ContentType = "application/pdf";
         //   Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
         //   Response.Cache.SetCacheability(HttpCacheability.NoCache);
         //   /*style Para Convertir numerico a String*/
         //   string style = @"<style> .textmode { mso-number-format:\@; font-family:Verdana;font-size:5px;color:#666666; color:red; }  .ColorGridCabe{font-family:Verdana;font-size:5px;background-color:#449FC9;color:White;width:200px} </style>";
         //   Response.Write(style);

         //   using (StringWriter sw = new StringWriter())
         //   {
         //       HtmlTextWriter hw = new HtmlTextWriter(sw);

         //       foreach (TableCell cell in grid_view.HeaderRow.Cells)
         //       {
         //           //grillaFinal.HeaderRow.CssClass = "ColorGridCabe";
         //           cell.CssClass = "ColorGridCabe";
                   
                   

         //       }
         //       foreach (GridViewRow row in grid_view.Rows)
         //       {
         //           row.BackColor = System.Drawing.Color.White;

         //           foreach (TableCell cell in row.Cells)
         //           {

         //               if (row.RowIndex % 2 == 0)
         //               {
         //                   cell.BackColor = grid_view.AlternatingRowStyle.BackColor;
         //                   //cell.BackColor = Color.Black;
         //               }
         //               else
         //               {
         //                   cell.BackColor = grid_view.RowStyle.BackColor;
         //                   //cell.BackColor = Color.Red;
         //               }
         //               cell.CssClass = "textmode";
                      
         //           }


         //       }

         //       grid_view.RenderControl(hw);
              

         //       StringReader sr = new StringReader(sw.ToString());
         //       iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 100f, 0f);
         //       iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
         //       iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
         //       pdfDoc.Open();
         //       htmlparser.Parse(sr);
         //       pdfDoc.Close();
         //       Response.Write(pdfDoc);
         //       Response.End();
         //   }
       
                
                //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=" + rbl_filter.SelectedItem + "_AR.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //grid_view.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
        //grid_view.AllowPaging = true;
        //grid_view.DataBind();
    }
