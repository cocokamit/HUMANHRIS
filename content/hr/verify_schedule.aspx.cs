using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verify_schedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            DataBind();


   
    }   
    protected void click_submit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Response.Redirect("schedule-viewer?key=" + id.Value + "");
        }
    }
    protected void click_back(object sender, EventArgs e)
    {
        Response.Redirect("vs");
       
    }
    protected void DataBind()
    {
        DataSet ds = bol.display(bind("status='for approval' "));
        grid_view.DataSource = ds;
        grid_view.DataBind();
    }

    protected string bind(string condition)
    {
        return "select a.status, (select count(*) from [TChangeShift] where [periodid]=a.id) cnt, DATENAME(MM, date_start) +' '+  DATENAME(DD, date_start) + ' to ' + DATENAME(MM, date_start) + RIGHT(CONVERT(VARCHAR(12), date_end, 107), 9) period ,* " +
        "from dtr_period a where " + condition + " order by Id desc";
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("vs", false);
    }

  

  
}