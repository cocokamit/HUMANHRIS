using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_otapplicationlist : System.Web.UI.Page
{
    public static string user_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }
    }

    protected void lodable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_add_ot(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addOT?user_id=" + function.Encrypt(key.Value, true) + "", false);

    }

    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id, left(convert(varchar,a.date,101),10)date,a.OvertimeHours,a.OvertimeNightHours,a.OvertimeLimitHours,a.remarks,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from TOverTimeLine a " +
                                    "left join MEmployee b on a.EmployeeId=b.Id  " +
                                    "where   a.status like'%Approved%'");

        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_search(object sender, EventArgs e)
    {
        string query = "select a.id, left(convert(varchar,a.date,101),10)date,a.OvertimeHours,a.OvertimeNightHours,a.OvertimeLimitHours,a.remarks,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from TOverTimeLine a " +
                                    "left join MEmployee b on a.EmployeeId=b.Id  " +
                                    "where  a.status like'%Approved%' ";
        if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
        {
            query += "and LEFT(CONVERT(varchar,a.Date,101),10)>='" + txt_from.Text + "' and  LEFT(CONVERT(varchar,a.Date,101),10)<='" + txt_to.Text + "'";
        }
        if (int.Parse(ddl_pg.SelectedValue) > 0)
        {
            query += " and b.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        }
        if (txt_search.Text.Length > 0)
        {
            query += " and b.LastName + '' + b.FirstName + '' + b.MiddleName+''+b.IdNumber like '%" + txt_search.Text + "%'";
        }

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
}