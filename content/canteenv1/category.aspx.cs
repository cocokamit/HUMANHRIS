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
            getdata();

 
    }

    protected void click_schedule(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("schedule-viewer?key=" + row.Cells[0].Text);
        }
    }

    protected void getdata()
    {
        //string query = "select a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt, DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_start) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,* " +
        //"from dtr_period a where status not like '%delete%' order by Id desc";
        string query = "select a.id dtr_id, b.id,c.FirstName + ' '+ c.LastName employee, a.status, (select count(*) from [TChangeShift] " +
        "where [periodid]=a.id) cnt,  " +
        "DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_end) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,*  " +
        "from dtr_period a  " +
        "left join [TChangeShift] b on a.id=b.periodid " +
        "left join nobel_user d on b.EntryUserId=d.id " +
        "left join memployee c on d.emp_id=c.id " +
        "where a.[status] like '%appro%'  order by a.Id desc";
        DataSet ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        grid_alert.Visible = grid_view.Rows.Count == 0 ? true : false;
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
}