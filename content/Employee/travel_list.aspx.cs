using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_travel_list : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            getdata();
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbDelete = (LinkButton)e.Row.FindControl("lbDelete");
            string[] stat = e.Row.Cells[5].Text.Split('-');

            switch (stat[0])
            {
                case "Approved":
                    lbDelete.Enabled = false;
                    lbDelete.OnClientClick = null;
                    break;
                case "For Approval":
                     
                    break;
                case "Cancel":
                    
                    break;
                case "verification":
 
                    break;
                default:
                    break;
            }
        }
    }

    protected void deleteOB(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            modal_delete.Style.Add("display", "block");
        }
    }

    protected void DeleteTransaction(object sender, EventArgs e)
    {
        string query = "update Ttravel set status='Deleted-" + DateTime.Now.ToShortDateString().ToString()+"', notes='"+tbCancelRemarks.Text.Replace("'", "") + "' where id=" + id.Value+";";
        query += "update obtline set status='Cancelled' where tID=" + id.Value;
        dbhelper.getdata(query);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='"+Request.RawUrl+"'", true);
    }
    
    protected void getdata()
    {
        string query = "select notes, id,date_input,purpose,travel_start,travel_end,case when status is null then 'For Approval' else status end as stat  from Ttravel where emp_id='" + Session["emp_id"].ToString() + "' and status not like '%Delete%' order by date_input";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        div_msg.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void addtravel(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_addtravel");
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;

            string[] status = row.Cells[5].Text.Split('-') ;
            pnlAlert.Visible = status[0] == "Declined" ? true : false;
            lblNotes.Text = row.Cells[7].Text;

            string  query = "select a.id,a.expected_budget,a.actual_budget,a.travel_description,a.notes,a.status, " +
                   "b.place,b.travel_mode,b.arrange_type " +
                   "from Ttravel a " +
                   "left join Ttravelline b on a.id=b.travel_id " +
                   "where a.id=" + id.Value + "";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);

            //txt_reason.Text = dt.Rows[0]["travel_description"].ToString();


            query = "select * from Ttravelline where travel_id=" + id.Value + " ";
            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_destinations.DataSource = ds;
            grid_destinations.DataBind();

            query = "select * from Ttravel_budget where travel_id=" + id.Value + " ";
            DataTable dtt = new DataTable();
            dtt = dbhelper.getdata(query);

            if (dtt.Rows.Count == 0)
            {
                meals.Visible = false;
                accommodation.Visible = false;
                other.Visible = false;
                transportation.Visible = false;
                total.Visible = false;
            }
            else
            {
                if (dt.Rows[0]["status"].ToString() == "")
                {
                    meals.Visible = false;
                    accommodation.Visible = false;
                    other.Visible = false;
                    transportation.Visible = false;
                    total.Visible = false;
                }
                else
                {
                lbl_meals.Text = dtt.Rows[0]["meals"].ToString();
                lbl_accom.Text = dtt.Rows[0]["accommodation"].ToString();
                lbl_trans.Text = dtt.Rows[0]["transportation"].ToString();
                lbl_other.Text = dtt.Rows[0]["otherexpense"].ToString();
                lbl_total.Text = dtt.Rows[0]["totalcashapproved"].ToString();
                }
            }

            Div1.Visible = true;
            Div2.Visible = true;


            dt = dbhelper.getdata("select * from obtline where tID=" + row.Cells[0].Text);
            gvDates.DataSource = dt;
            gvDates.DataBind();
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_travel", false);
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}