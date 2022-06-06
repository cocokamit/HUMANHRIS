using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_canteen_category : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        if (!IsPostBack)
        {
            loadable();
            getdata();
        }
    }

    protected void loadable()
    {
        DataTable dt = dbhelper.getdata("select id ,left(convert(varchar,date_start,101),10)+'-'+left(convert(varchar,date_end,101),10) dtrrange from dtr_period where dtr_id is null order by date_input desc");

        //DataTable dt = dbhelper.getdata("select id ,left(convert(varchar,dfrom,101),10)+'-'+left(convert(varchar,dtoo,101),10) dtrrange from payroll_range where action is null order by dfrom desc");


        foreach (DataRow row in dt.Rows)
        {
            ddl_period.Items.Add(new ListItem(row["dtrrange"].ToString(), row["id"].ToString()));
        }

        /**BTK 111419**/
        DataTable dept = dbhelper.getdata("select * from MDepartment order by Department");
        ddlDepartment.Items.Add(new ListItem("All", "0"));
        foreach (DataRow row in dept.Rows)
        {
            ddlDepartment.Items.Add(new ListItem(row["Department"].ToString(), row["id"].ToString()));
        }
        
    }

    protected void click_schedule(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Session["start_orley"] = row.Cells[2].Text;
            Session["end_orley"] = row.Cells[3].Text;
            Session["idd"] = row.Cells[4].Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Redirect", "window.open('schedule-viewer?key="+ row.Cells[0].Text+"', '_blank');", true);
        }
    }

    protected void getdata()
    {
       // //string query = "select a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt, DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_start) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,* " +
       // //"from dtr_period a where status not like '%delete%' order by Id desc";
       // string query = "select  f.sec_desc,e.Department,a.id dtr_id, b.id,c.id emp_id,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
       // "where [periodid]=a.id) cnt,  " +
       // "e.Department + ' (' + f.sec_desc + ' - ' + DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')' period ,*  " +
       //"from dtr_period a  " +
       // "left join [TChangeShift] b on a.id=b.periodid " +
       // "left join nobel_user d on b.EntryUserId=d.id " +
       // "left join memployee c on d.emp_id=c.id " +
       // "left join MDepartment e on c.DepartmentId=e.Id " +
       // "left join msection f on c.sectionid = f.sectionid " +
       // "where a.[status] like '%for approval%'  order by a.Id desc";
       // DataSet ds = bol.display(query);
       // grid_view.DataSource = ds;
       // grid_view.DataBind();
       // grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;

        //a.status not like '%created%' and a.status not like '%new%' and a.status not like '%decline%'
        string[] period = ddl_period.SelectedItem.ToString().Split('-');
         string dept = ddlDepartment.SelectedValue == "0" ? "" : "e.id= " + ddlDepartment.SelectedValue + " and ";

         string query = "select g.FirstName + ' '+ g.LastName approverName, f.sec_desc, e.Department,a.id dtr_id, b.id,c.id emp_id,a.status,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
         "where [periodid]=a.id) cnt,  " +
         "e.Department + ' (' + f.sec_desc + ' - ' +  DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')' period ,*  " +
         "from dtr_period a  " +
         "left join [TChangeShift] b on a.id=b.periodid " +
         "left join nobel_user d on b.EntryUserId=d.id " +
         "left join memployee c on d.emp_id=c.id " +
         "left join MDepartment e on c.DepartmentId=e.Id " +
         "left join msection f on c.sectionid = f.sectionid " +
         "left join memployee g on a.approver_id=g.id " +
         "where " + dept + " a.status not  like '%created%' and a.status not like '%delete%' and a.status not like '%new%' ";
        //and a.status not like '%approved%'  ";
        //remove temporaty


        //+
        //"and left(convert(varchar,a.date_start,101),10) = '" + period[0] + "' " +
        //"and left(convert(varchar,a.date_end,101),10) = '" + period[1] + "' " +
        //"order by e.Department + ' (' + f.sec_desc + ' - ' +  DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')'";

        DataSet ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void btn_search(object sender, EventArgs e)
    {
        string query = "";
        string[] period = ddl_period.SelectedItem.ToString().Split('-');
        string dept = ddlDepartment.SelectedItem.ToString();
        if (ddlDepartment.SelectedItem.ToString() == "All")
        {
            query = "select g.FirstName + ' '+ g.LastName approverName, f.sec_desc, e.Department,a.id dtr_id, b.id,c.id emp_id,a.status,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt,  e.Department + ' (' + f.sec_desc + ' - ' +  DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')' period ,*  from dtr_period a  left join [TChangeShift] b on a.id=b.periodid left join nobel_user d on b.EntryUserId=d.id left join memployee c on d.emp_id=c.id left join MDepartment e on c.DepartmentId=e.Id left join msection f on c.sectionid = f.sectionid left join memployee g on a.approver_id=g.id where  a.status not  like '%created%' and a.status not like '%delete%' and a.status not like '%new%' and (select convert(varchar,a.date_start,101))='" + period[0] + "' and (select convert(varchar,a.date_end,101))= '" + period[1] + "'";
        }
        else
        {
            query = "select g.FirstName + ' '+ g.LastName approverName, f.sec_desc, e.Department,a.id dtr_id, b.id,c.id emp_id,a.status,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt,  e.Department + ' (' + f.sec_desc + ' - ' +  DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')' period ,*  from dtr_period a  left join [TChangeShift] b on a.id=b.periodid left join nobel_user d on b.EntryUserId=d.id left join memployee c on d.emp_id=c.id left join MDepartment e on c.DepartmentId=e.Id left join msection f on c.sectionid = f.sectionid left join memployee g on a.approver_id=g.id where  a.status not  like '%created%' and a.status not like '%delete%' and a.status not like '%new%' and (select convert(varchar,a.date_start,101))='" + period[0] + "' and (select convert(varchar,a.date_end,101))= '" + period[1] + "' and e.Department = '" + dept + "'";
        }
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

    protected void back(object sender, EventArgs e)
    {

        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hf_did.Value = row.Cells[0].Text;
            modal_delete.Style.Add("display", "block");
        }
    }

    protected void back_save(object sender, EventArgs e)
    {
           // string id = Request.QueryString["key"].ToString();
        //function.ReadNotification(hf_did.Value, 0);
            dbhelper.getdata("insert into declined_list (date_input,period_id,notes) values(getdate(),'" + hf_did.Value + "','" + txt_reason.Text.Replace("'", "") + "') ");

            string query = "update dtr_period set [status]='decline' where id=" + hf_did.Value;
            dbhelper.getdata(query);
           // function.AddNotification("Decline Schedule", "dtrp", "0", hf_did.Value);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='schedule'", true);
        
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton delete = (LinkButton)e.Row.FindControl("LinkButton1");
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton3");
            switch (e.Row.Cells[1].Text)
            {
                case "verification":
                    delete.Visible = false;
                    lb.Visible = false;
                    break;
                case "approved":
                    lb.CssClass = "glyphicon glyphicon-ok-sign text-success pull-right";
                    lb.Style.Add("font-size", "16px");
                    lb.Text = "";
                    break;
                case "for approval":
                    //delete.Visible = false;
                    break;
                default:
                    if (e.Row.Cells[1].Text.Contains("delete"))
                    {
                        lb.CssClass = "label label-danger pull-right";
                        delete.Visible = false;
                    }
                    else if (e.Row.Cells[1].Text.Contains("decline"))
                    {
                        lb.CssClass = "label label-danger pull-right";
                        delete.Visible = false;
                        lb.Text = "declined";
                    }
                    break;
            }
        }
    }

    protected void click_set(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //id.Value = row.Cells[0].Text;
            //hf_shiftline.Value = row.Cells[1].Text;
            //l_range.Text = row.Cells[3].Text;

            //main.Visible = false;
            //p_scheduling.Visible = true;
            //lb_shiftline.Visible = hf_shiftline.Value == "0" ? true : false;
            
            //schedule();
        }
    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("schedule", false);
    }
}