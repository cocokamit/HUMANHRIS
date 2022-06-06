using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_Manager_range_h : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit");

        if (!IsPostBack)
            getdata();


    }

    protected void click_schedule(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Session["start_orley"] = row.Cells[1].Text;
            Session["end_orley"] = row.Cells[2].Text;
            Session["idd"] = row.Cells[3].Text;

            Response.Redirect("h_schedule?key=" + row.Cells[0].Text);


        }
    }

    protected void getdata()
    {
        //string query = "select a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt, DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_start) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,* " +
        //"from dtr_period a where status not like '%delete%' order by Id desc";

        //string query = "select a.id dtr_id, b.id,c.id emp_id,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
        //"where [periodid]=a.id) cnt,  " +
        //"DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,*  " +
        //"from dtr_period a  " +
        //"left join [TChangeShift] b on a.id=b.periodid " +
        //"left join nobel_user d on b.EntryUserId=d.id " +
        //"left join memployee c on d.emp_id=c.id " +
        //"where a.[status] like '%for approval%'  order by a.Id desc";

        string query = "select id as dtr_id,DATENAME(MM, dfrom) +' '+  DATENAME(DD, dfrom) + ' to ' + DATENAME(MM, dtoo) + RIGHT(CONVERT(VARCHAR(12), dtoo, 107), 9) period,* " +
                        "from payroll_range where dtr_id is null and action is null order by id desc";
        DataSet ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;
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
        grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Schedule successfully declined'); window.location='h_schedule'", true);

    }




    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton3");
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
        Response.Redirect("h_schedule", false);
    }
}