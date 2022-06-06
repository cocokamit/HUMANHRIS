using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Admin_oicAdjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {

        }
        loader();

        modal.Style.Add("display", "none");
    }

    protected void loader()
    {
        DataTable dt = dbhelper.getdata("Select *,(Select lastname+', '+firstname from MEmployee where id=a.oic) oicfullname,(Select lastname+', '+firstname from MEmployee where id=a.realApprover) appfullname from approval_OIC a order by id desc");
        grid_forms.DataSource = dt;
        grid_forms.DataBind();



        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;
    }

    protected void editor(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        if (lb.ID == "viewbtn")
        { using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                DataTable dt = dbhelper.getdata("Select *,(Select lastname+', '+firstname from MEmployee where id=a.oic) fullname from approval_OIC a where id=" + row.Cells[0].Text + "");
                if (dt.Rows.Count > 0)
                {
                    lb_current.Text = dt.Rows[0]["fullname"].ToString();

                    dt = dbhelper.getdata("select id, lastname+', '+firstname fullname,(Select Department from MDepartment where id=a.DepartmentID) [department] from MEmployee a where a.level!=3 order by a.lastname asc ");
                    ddl_new.Items.Clear();
                    ddl_new.Items.Add(new ListItem(" ", "0"));
                    ViewState["rowid"] = row.Cells[0].Text;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ddl_new.Items.Add(new ListItem(dr["fullname"].ToString(), dr["id"].ToString()));
                    }
                }

            modal.Style.Add("display", "block");
        }
        }

    }

    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
    }

    protected void saveform(object sender, EventArgs e)
    {
        string id = ViewState["rowid"].ToString();
        DataTable dt = dbhelper.getdata("Update approval_OIC set oic=" + ddl_new.SelectedValue+ " where id="+id+"");

        loader();

    }

}