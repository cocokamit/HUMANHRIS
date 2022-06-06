using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

using System.Data.SqlClient;

public partial class content_Admin_resetpass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            // hf_id.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            view();
        }
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id,c.name from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id left join nobel_user c on c.emp_id = a.Id where c.name like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["name"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void view()
    {
        DataTable dt = dbhelper.getdata("select value from Sys_viewconfig where id=1");
        hf_view.Value = dt.Rows[0]["value"].ToString();

        if (dt.Rows[0]["value"].ToString() == "List")
        {
            disp();
            div_list.Visible = true;
        }
    }

    protected void lodable()
    {
        DataTable dt = dbhelper.getdata("select * from MPayrollGroup where status = '1' order by id asc");
        ddl_payroll_group.Items.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
        ddl_payroll_group.Items.Add(new ListItem(" ", "0"));
    }

    protected void disp()
    {
        string query = "select case when (select top 1 empid from file_details where empid=a.id order by id desc) is null then 0 else a.id end profile,a.id, a.IdNumber,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname,case when a.PositionId = '0' then 'No Designated Position Assign' else b.Position end Position, case when a.DepartmentId = '0' then 'No Designated Department Assign' else c.Department end Department from MEmployee a left join MPosition b on a.PositionId=b.Id left join MDepartment c on a.DepartmentId = c.Id left join nobel_user d on d.emp_id=a.Id where PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and d.name like '%" + txt_search.Text + "%' order by a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void tawesie(object seder, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[4].Text == "No Designated Department Assign")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[3].Text == "No Designated Position Assign")
            {
                e.Row.ForeColor = System.Drawing.Color.Green;
                e.Row.Font.Bold = true;
            }
        }
    }

    protected void reset(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Value == "Yes")
            {
                dbhelper.getdata("insert into resetpassword values (getdate()," + Session["user_id"].ToString() + "," + lb.CommandName + ")");
                dbhelper.getdata("update nobel_user set password='" + row.Cells[1].Text + "' where emp_id=" + lb.CommandName + "");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Successfully');window.location='reset'", true);
               // Response.Redirect("addemployee?user_id=" + function.Encrypt(hf_id.Value, true) + "&app_id=" + lb.CommandName + "&tp=ed", false);
            }
        }
    }

    protected void search(object sender, EventArgs e)
    {
        if (hf_view.Value == "List")
        {
            disp();
        }
    }
}