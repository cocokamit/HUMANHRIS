using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

using System.Security.Cryptography;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_report_leave_credit_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
            if (grid_view.Rows.Count > 0)
                exp_report.Visible = true;
            else
                exp_report.Visible = false;
        }

      

    }
    protected void loadable()
    {
        ddl_yyyy.Items.Clear();
        ddl_yyyy.Items.Add(new ListItem("Select", "0"));
        for (int i = 2017; i <= DateTime.Now.Year + 1; i++)
        ddl_yyyy.Items.Add(new ListItem(i.ToString(), i.ToString()));

        //string query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee";
        //DataTable dt = dbhelper.getdata(query);

        //ddl_employee.Items.Clear();
        //ddl_employee.Items.Add(new ListItem("None", "0"));
        //foreach (DataRow dr in dt.Rows)
        //{
        //    ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        //}

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

                retCategory.Add(string.Format("{0}-{1}", "0", "All"));
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

   

    protected void btn_search(object sender, EventArgs e)
    {
        if (lbl_bals.Value == "0")
        {
            string query = "Select Id,Lastname from Memployee where (PayrollGroupId=1 or PayrollGroupId=10) order by Lastname asc";
            DataTable dt = dbhelper.getdata(query);
            DataTable drs = new DataTable();
            foreach (DataRow row in dt.Rows)
            {
                string[] twos = SetUp.leavetype(row["Id"].ToString(), ddl_yyyy.SelectedValue, "").Split(new string[] { "UNION" }, StringSplitOptions.None);

                DataTable dpres = dbhelper.getdata(twos[0]);
                DataTable dprev = dbhelper.getdata(twos[1]);

                for (int i = 0; i < dpres.Rows.Count; i++)
                {
                    double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                    double diff = asd <= 0 ? 0 : asd;
                    if (diff > 5)
                    {
                        diff = 5;
                    }
                    for (int j = 0; j < dprev.Rows.Count; j++)
                    {
                        if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                        {
                            dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";

                        }
                    }
                }

                dpres.Merge(dprev);
                drs.Merge(dpres);
            }

            grid_view.DataSource = drs;
            grid_view.DataBind();
        }
        else
        {

            string[] twos = SetUp.leavetype(lbl_bals.Value, ddl_yyyy.SelectedValue, "").Split(new string[] { "UNION" }, StringSplitOptions.None);

            DataTable dpres = dbhelper.getdata(twos[0]);
            DataTable dprev = dbhelper.getdata(twos[1]);

            for (int i = 0; i < dpres.Rows.Count; i++)
            {
                double asd = Convert.ToDouble(dpres.Rows[i]["Credit"].ToString()) - Convert.ToDouble(dpres.Rows[i]["Balance2"].ToString());
                double diff = asd <= 0 ? 0 : asd;
                if (diff > 5)
                {
                    diff = 5;
                }
                for (int j = 0; j < dprev.Rows.Count; j++)
                {
                    if (diff >= 0 && dpres.Rows[i]["Leave"].ToString() == dprev.Rows[j]["Leave"].ToString())
                    {
                        dprev.Rows[j]["Balance"] = Math.Round((Convert.ToDouble(dprev.Rows[j]["Balance"]) + diff), 2).ToString("#.00") + "";

                    }
                }
            }

            dpres.Merge(dprev);
            //DataTable dt = dbhelper.getdata(dprev);
            grid_view.DataSource = dpres;
            grid_view.DataBind();
        }

        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
        if (grid_view.Rows.Count > 0)
            exp_report.Visible = true;
        else
            exp_report.Visible = false;
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal.Style.Add("display", "block");

            string query = "select c.LastName+', '+c.FirstName+' '+c.MiddleName Employee,d.Department,left(CONVERT(varchar,a.date,101),10)Date,left(CONVERT(varchar,a.sysdate,101),10)Sysdate,e.Leave,a.NumberOfHours NoDAYS,a.WithPay,a.Remarks,case when inoutduringhalfdayleave=0 then 'AM-PM'when inoutduringhalfdayleave=1 then 'AM' else 'PM' end designation from TLeaveApplicationLine a " +
                                "left join Tleave b on a.LeaveId=b.id " +
                                "left join memployee c on a.EmployeeId=c.Id " +
                                "left join MDepartment d on c.DepartmentId=d.Id " +
                                "left join MLeave e on a.LeaveId=e.Id " +
                                "where a.status like'%Approved%' and a.EmployeeId='" + row.Cells[0].Text + "' and a.LeaveId='" + row.Cells[1].Text + "' and a.Date like '%" + ddl_yyyy.SelectedItem + "%'";
            DataTable dt = dbhelper.getdata(query);

            query = "select ID,Employee,SUM(NumberOfHours)Total,LType from(select a.NumberOfHours NumberOfHours,b.FullName Employee,b.IdNumber ID,(select Leave from MLeave where Id =a.LeaveId)LType "
                + "from TLeaveApplicationLine a left join MEmployee b on a.EmployeeId=b.Id "
                + "where EmployeeId = " + row.Cells[0].Text + " and LeaveId = " + row.Cells[1].Text + " and Date like '%" + ddl_yyyy.SelectedItem + "%' and status like '%Approved%')test "
                + "group by GROUPING sets((Employee,ID,LType),())";
            DataTable counter = dbhelper.getdata(query);
            lblcounter.Text = "Total Leave: " + counter.Rows[0]["Total"].ToString() + "     ";
            lbltype.Text = counter.Rows[0]["LType"].ToString() + "     ";
            grid_history.DataSource = dt;
            grid_history.DataBind();
        }
    }

    protected void ExportToExcelreport(object sender, EventArgs e)
    {
        string huhu = grid_history.Rows.Count.ToString();
        string filename = "Leave_Credits_" + ddl_emp.Text;
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

            grid_history.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in grid_history.HeaderRow.Cells)
            {
                cell.BackColor = grid_history.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grid_history.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grid_history.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grid_history.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grid_history.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    protected void cpop(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "Leave_Credits_" + ddl_yyyy.SelectedItem.Text;
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
    protected void rowboundgrid_pay(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_can = (LinkButton)e.Row.FindControl("lnk_can");
            string stat = e.Row.Cells[4].Text;
            if (stat == "Approved" || stat == "Cancelled")
                lnk_can.Visible = false;

            if (e.Row.Cells[2].Text == "No")
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
            else
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
        }
    }
}