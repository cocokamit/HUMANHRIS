using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

public partial class content_hr_assetassign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dispaly();
        }
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id, a.lastname+', '+a.firstname+' '+a.MiddleName fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //retCategory.Add(string.Format("{0}-{1}", "0", "All"));
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void search(object sender, EventArgs e)
    {
        string query = "select a.id assassign,a.empid,d.Description Category,a.action,d.um,e.LastName+' '+e.FirstName+' '+e.MiddleName Name,case when g.Department is null then 'Not Assigned' else g.Department end Department,h.Branch,c.Description invname,a.qty,c.serial,c.propertycode,c.vendor,c.brand,c.price,LEFT(convert(varchar,a.[date],101),10)as date,a.status,case when(select top 1 bb.status from memployeestatus aa left join mempstatus_setup bb on aa.statusid=bb.id where aa.empid=e.Id order by aa.empstatid desc)is null then(select status from mempstatus_setup where id = e.emp_status)else(select top 1 bb.status from memployeestatus aa left join mempstatus_setup bb on aa.statusid=bb.id where aa.empid=e.Id order by aa.empstatid desc) end empstatus from asset_assign a left join asset_details b on a.id = b.inventid left join asset_inventory c on c.id = a.invid left join asset_cat d on d.id = a.categoryid left join MEmployee e on e.Id = a.empid left join mempstatus_setup f on e.emp_status = f.id left join MDepartment g on g.Id = e.DepartmentId left join MBranch h on h.Id = e.BranchId where a.empid = " + lbl_bals.Value + " and a.action is NULL order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_assign.DataSource = dt;
        grid_assign.DataBind();
    }
    protected void dispaly()
    {
        string query = "select a.id assassign,a.empid,d.Description Category,a.action,d.um,e.LastName+' '+e.FirstName+' '+e.MiddleName Name,case when g.Department is null then 'Not Assigned' else g.Department end Department,h.Branch,c.Description invname,a.qty,c.serial,c.propertycode,c.vendor,c.brand,c.price,LEFT(convert(varchar,a.[date],101),10)as date,a.status,case when(select top 1 bb.status from memployeestatus aa left join mempstatus_setup bb on aa.statusid=bb.id where aa.empid=e.Id order by aa.empstatid desc)is null then(select status from mempstatus_setup where id = e.emp_status)else(select top 1 bb.status from memployeestatus aa left join mempstatus_setup bb on aa.statusid=bb.id where aa.empid=e.Id order by aa.empstatid desc) end empstatus from asset_assign a left join asset_details b on a.id = b.inventid left join asset_inventory c on c.id = a.invid left join asset_cat d on d.id = a.categoryid left join MEmployee e on e.Id = a.empid left join mempstatus_setup f on e.emp_status = f.id left join MDepartment g on g.Id = e.DepartmentId left join MBranch h on h.Id = e.BranchId where a.action is NULL order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_assign.DataSource = dt;
        grid_assign.DataBind();
    }
    protected void grid_datarow(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[12].Text.Contains("Resigned") && e.Row.Cells[11].Text.Contains("Deployed"))
            {
                e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[4].Text.Contains("Not Assigned"))
            {
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
            }
        }
    }
    protected void emp_redirect(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string query = "select a.empid,b.Id from asset_assign a left join MEmployee b on a.empid = b.Id where b.id = " + row.Cells[1].Text + "";
            DataTable dt = dbhelper.getdata(query);
            Response.Redirect("employee?user_id=" + function.Encrypt("" + dt.Rows[0]["empid"] + "", true) + "=&app_id=" + dt.Rows[0]["Id"] + "&tp=ed");
        }
    }
    protected void gridview_paging(object sender, GridViewPageEventArgs e)
    {
        grid_assign.PageIndex = e.NewPageIndex;
        dispaly();
    }
}