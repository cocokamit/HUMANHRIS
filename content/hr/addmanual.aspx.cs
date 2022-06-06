using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_addmanual : System.Web.UI.Page
{

 
    protected void Page_Load(object sender, EventArgs e)
    {
        key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        pg.Value = function.Decrypt(Request.QueryString["pg"].ToString(), true);
        if (!IsPostBack)
        {
            fload();
            disp();
        }
    }

    protected void disp()
    {
       string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                      "left join MPosition b on a.PositionId=b.Id WHERE payrollgroupid=" + pg.Value + " ";
        DataTable dt = dbhelper.getdata(query);
        ddl_employee.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
    }


    protected void fload()
    {
        for (int i = 0; i < 13; i++)
        {
            if (i.ToString().Length == 1)
            {
                ddl_hrs_in.Items.Add("0" + i.ToString());
                ddl_hrs_out.Items.Add("0" + i.ToString());
            }
            else
            {
                ddl_hrs_in.Items.Add(i.ToString());
                ddl_hrs_out.Items.Add(i.ToString());
            }
        }

        for (int i = 0; i < 61; i++)
        {
            if (i.ToString().Length == 1)
            {
                ddl_minute_in.Items.Add("0" + i.ToString());
                ddl_minute_out.Items.Add("0" + i.ToString());
            }
            else
            {
                ddl_minute_in.Items.Add(i.ToString());
                ddl_minute_out.Items.Add(i.ToString());
            }
        }
    }

    protected void click_save_ot(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from Tmanuallogline where EmployeeId='" + ddl_employee.SelectedValue + "' and left(convert(varchar,Date,101),10)='" + txt_otd.Text.Trim() + "'");
        if (dt.Rows.Count == 0)
        {
            dbhelper.getdata("insert into Tmanuallogline values (NULL," + ddl_employee.SelectedValue + ",'" + txt_otd.Text.Trim() + "','" + ddl_hrs_in.Text + ":" + ddl_minute_in.Text + " " + ddl_am_pm_in.Text + "','" + ddl_hrs_out.Text + ":" + ddl_minute_out.Text + " " + ddl_am_pm_out.Text + "','" + txt_lineremarks.Text + "',NULL,'" + "Approved-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,getdate())");
        }
        else
            Response.Write("<script>alert('already exist!')</script>");

    }
}