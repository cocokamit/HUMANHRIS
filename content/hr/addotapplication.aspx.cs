using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_addotapplication : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            pg.Value = function.Decrypt(Request.QueryString["pg"].ToString(), true);
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
    protected void click_save_ot(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from TOverTimeLine where EmployeeId='" + ddl_employee.SelectedValue + "' and left(convert(varchar,Date,101),10)='" + txt_otd.Text.Trim() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into TOverTimeLine values (NULL," + ddl_employee.SelectedValue + ",'" + txt_otd.Text.Trim() + "','" + txt_OT.Text + "'," + txt_night_ot.Text + ",'0','" + txt_lineremarks.Text + "','" + "Approved-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate())");
            }
            else
                Response.Write("<script>alert('already exist!')</script>");
     
    }
}