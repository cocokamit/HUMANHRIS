using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class content_Employee_RequestUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();    
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select Id,Failed,Request,Status,left(convert(varchar,DateRequest,101),10)DateRequest,ActionResponse from RequestUpdate201 where IdNumber = " + Session["emp_id"].ToString() + " order by DateRequest desc");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void RowEventUpdate(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("employee-profile?user_id=" + Session["emp_id"].ToString() + "", false);
        }
    }
}