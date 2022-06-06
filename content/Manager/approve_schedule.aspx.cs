using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_Manager_approve_schedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        if (!IsPostBack)
            BindData();


    }

    protected void click_schedule(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Session["start_orley"] = row.Cells[1].Text;
            //Session["end_orley"] = row.Cells[2].Text;
            //Session["idd"] = row.Cells[3].Text;

            /**BTK 
             * 111119 OIC**/
            string approver = Request.QueryString["oic"] == null ? "" : "&oic=" +Request.QueryString["key"].ToString();

            Response.Redirect("view-schedule?key=" + row.Cells[0].Text + "&id=" + row.Cells[1].Text + approver);
        }
    }

    //protected void loadable()
    //{
    //    DataTable dt = dbhelper.getdata("select id ,left(convert(varchar,dfrom,101),10)+'-'+left(convert(varchar,dtoo,101),10) dtrrange from payroll_range where action is null order by dfrom desc");

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        ddl_period.Items.Add(new ListItem(row["dtrrange"].ToString(), row["id"].ToString()));
    //    }

    //}

    protected void BindData()
    {
        /**BTK 
         * 111119 OIC
         * **/
        string approver = Request.QueryString["oic"] == null ? Session["emp_id"].ToString() : Request.QueryString["key"].ToString();

        string query = "select a.id dtr_id, b.id,c.id emp_id,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
        "where [periodid]=a.id) cnt, " +
        "f.under_id,f.herarchy, " +
        "case when " +
        "(select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) is null " +
        "then 0 else " +
        "(select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) end as nxt_id, " +
        // "g.Department + ' ' + h.sec_desc + ' 
        "'(' +  DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) + ')' period ,*  " +
        "from dtr_period a " +
        "left join [TChangeShift] b on a.id=b.periodid " +
        "left join nobel_user d on b.EntryUserId=d.id " +
        "left join memployee c on d.emp_id=c.id " +
        "left join approver f on a.emp_id=f.emp_id " +
        "left join MDepartment g on c.DepartmentId=g.Id " +
        "left join msection h on c.sectionid = h.sectionid " +
        "where a.[status] like '%for approval%' and a.approver_id=" + approver + " and f.under_id=" + approver + " " +
        "order by a.Id desc";
        DataSet ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void btn_search(object sender, EventArgs e)
    {
        string query = "select a.id dtr_id, b.id,c.id emp_id,a.status,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
       "where [periodid]=a.id) cnt,  " +
       "DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,*  " +
       "from dtr_period a  " +
       "left join [TChangeShift] b on a.id=b.periodid " +
       "left join nobel_user d on b.EntryUserId=d.id " +
       "left join memployee c on d.emp_id=c.id " +
       "where a.status not like '%created%' and a.status not like '%new%' and a.status not like '%decline%' and left(convert(varchar,a.date_input,101),10) between '" + txt_f.Text + "' and '" + txt_t.Text + "' " +
       "order by a.Id desc";

        DataSet ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='approve_schedule'", true);

    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton2");
            switch (e.Row.Cells[1].Text)
            {
                case "approved":
                    lb.CssClass = "glyphicon glyphicon-ok-sign text-success pull-right";
                    lb.Style.Add("font-size", "15px");
                    lb.Text = "";
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
        Response.Redirect("approve_schedule", false);
    }
}