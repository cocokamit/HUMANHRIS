using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;

public partial class content_report_demographic : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            LoadData();
        }
    }

    protected void LoadData()
    {
        DataTable dt = getdata.ReportFields(1);
        ViewState["fields"] = dt;
        gvFields.DataSource = dt;
        gvFields.DataBind();

        int row = gvFields.Rows.Count;
        for (int i = 0; i < row; i++)
        {
            string field = gvFields.Rows[i].Cells[1].Text;
            string description = gvFields.Rows[i].Cells[2].Text;
            CheckBox cb = (CheckBox)gvFields.Rows[i].FindControl("cbField");
            if (cb.Text == "Department")
            {
                cb.Checked = true;
            }
            if (cb.Text == "Status")
            {
                cb.Checked = true;
            }
        }
        dt = getdata.PayrollGroup("1");
        ddlPayrollGroup.Items.Clear();
        ddlPayrollGroup.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddlPayrollGroup.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }

    protected void checkallfields(object sender, EventArgs e)
    {
        int i = 0;
        foreach (GridViewRow row in gvFields.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("cbField");
            if (cball.Checked == true)
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
    }

    protected void ClickGo(object sender, EventArgs e)
    {
        BindData();
    }

    protected void BindData()
    {
        int row = gvFields.Rows.Count;
        string sql = string.Empty;
        for (int i = 0; i < row; i++)
        {
            string field = gvFields.Rows[i].Cells[1].Text;
            string description = gvFields.Rows[i].Cells[2].Text;
            CheckBox cb = (CheckBox)gvFields.Rows[i].FindControl("cbField");
            if (cb.Checked)
            {
                sql += field + " " + description.Replace(" ","_") + ",";
            }
        }

        string query = "select a.idnumber ID_Number,a.firstname First_Name, a.lastname Last_Name ,a.middlename Middle_Name," + sql +

        "case when j.position is null then 'Not Defined' else j.position end Position," +
        "(select a.LastName+', '+a.FirstName+' '+a.MiddleName as approver from MEmployee a where a.Id=aa.under_id)approver " +

        "from memployee a " +
        "left join mzipcode b on a.zipcodeid=b.id " +
        "left join mcitizenship c on a.citizenshipid=c.id " +
        "left join MReligion d on a.religionid=d.id " +
        "left join Mtaxcode e on a.taxcodeid=e.id " +
        "left join mcompany f on a.companyid=f.id " +
        "left join mposition j on a.positionid=j.id " +
        "left join mbranch h on a.branchid=h.id " +
        "left join mdivision i on a.divisionid=i.id " +
        "left join mdepartment g on a.departmentid=g.id " +
        "left join mpayrollgroup k on a.payrollgroupid=k.id " +
        "left join maccount l on a.accountid=l.id " +
        "left join mpayrolltype m on a.payrolltypeid=m.id " +
        "left join mshiftcode n on a.shiftcodeid=n.id " +
        "LEFT join mbloodtype o on a.bloodtype=o.id " +
        "left join msection p on a.sectionid = p.sectionid " +
        "left join mempstatus_setup q on a.emp_status = q.id " +
        "left join mlevel r on a.level = r.id " +
        "left join MDivision2 s on a.DivisionId2 = s.Id " +
        "left join minternalorder t on a.internalorderid = t.id " +
        "left join Approver aa on a.Id = aa.emp_id " +

        "left join MCivilStatus u on a.CivilStatus=u.id ";
        if (ddlPayrollGroup.SelectedIndex > 0)
            query += "where a.PayrollGroupId= " + ddlPayrollGroup.SelectedValue + " and aa.herarchy= 1 ";
        else
            query += "where aa.herarchy= 1 order by a.firstname +' '+ a.middlename +' ' + a.lastname";

        //"where a.idnumber+''+a.lastname+''+a.firstname+''+a.middlename+''+a.extensionname+''+a.address+''+a.permanentaddress+''+ " +
        //"a.phonenumber+''+a.cellphonenumber+''+a.emailaddress+''+a.placeofbirth+''+b.zipcode+''+a.datehired+''+a.sex+''+a.civilstatus+''+c.citizenship+''+ " +
        //"d.religion+''+a.height+''+a.weight+''+a.gsisnumber+''+a.sssnumber+''+a.hdmfnumber+''+a.phicnumber+''+a.tin+''+e.taxcode+''+f.company+''+ " +
        //"g.department+''+i.division+''+case when j.position is null then 'Not Defined' else j.position end+''+k.payrollgroup+''+l.account+''+m.payrolltype+''+n.shiftcode+''+a.fixnumberofdays+''+a.fixnumberofhours+''+a.monthlyrate+''+a.payrollrate+''+a.dailyrate+''+a.absentdailyrate+''+a.hourlyrate+''+o.bloodtype like '%" + txt_search.Text + "%' ";

        DataTable dt = dbhelper.getdata(query);
        ViewState["data"] = dt;
        alert.Visible = dt.Rows.Count == 0 ? true : false;

        //gvDemographics.DataSource =dt;
        //gvDemographics.DataBind();

       
        //grid_view.DataSource = dbhelper.getdata(query);
        //grid_view.DataBind();
        //ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_view', 'HeaderDiv');</script>");
    }

    protected void Rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("_", " ");
            }
        }
    }

    protected void search(object sender, EventArgs e)
    {
        BindData();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_view.PageIndex = e.NewPageIndex;
        this.BindData();
        //grid_view.DataBind();
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        string huhu = grid_view.Rows.Count.ToString();
        string filename = "Demographics";
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}