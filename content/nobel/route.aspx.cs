using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_su_route : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Lacking: assigning tree child
        //Current Manual

        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
            bind();
    }

    protected void click_TransferSave(object sender, EventArgs e)
    {
        string query = "update nobel_route set [default]=" + ddlRoutes.SelectedValue + " where id=" + hf_id.Value +";";
        query += "update nobel_accountrole set [status]=1, class=" + ddlRoutes.SelectedValue + " where route_id=" + hf_id.Value;

        dbhelper.getdata(query);
        Response.Redirect(Request.RawUrl);
    }

    protected void click_treeview(object sender, EventArgs e)
    {
        treeset();
    }

    protected void treeset()
    {
        string action = lb_treeview.CssClass.ToString();
        if (action.Contains("off"))
        {
            p_action.Visible = false;
            lb_treeview.CssClass = "fa fa-toggle-on";
        }
        else
        {
            p_action.Visible = true;
            lb_treeview.CssClass = "fa fa-toggle-off";
        }
    }

    protected bool checkdirectory(string filter)
    {
        int i = 0;
        bool ret = false;
        string[] allfiles = System.IO.Directory.GetFiles(Server.MapPath("~/content"), "*.aspx", System.IO.SearchOption.AllDirectories);
        string test = "";
        foreach (string item in allfiles)
        {
            if (filter.Contains("."))
            {
                string hold = item.Replace("\\", ".");
                ret = hold.Contains(filter.Replace(" ", "")) ? true : false;

            }

            if (ret)
            {
                if (hf_action.Value == "1")
                {
                    ret = true;
                    break;
                }
                else
                    break;
            }
        }

        string val = allfiles[0];
        return ret;
    }

    protected void bind()
    {
        DataTable dt = getdata.sys_routes();
        grid_view.DataSource = dt;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;

        DataRow[] results = dt.Select("url = '-'");
        ddlRoutes.Items.Clear();
        foreach (DataRow result in results)
        {
            ddlRoutes.Items.Add(new ListItem(result["name"].ToString(), result["id"].ToString()));
        }
       
    }

    protected void click_add(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        modal.Style.Add("display", "block");
        lb_treeview.Visible = true;
        p_action.Visible = true;
    }

    protected void clickAddMultiRout(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal_transfer.Style.Add("display", "block");
            hf_id.Value = row.Cells[0].Text;
        }
    }

    protected void clickRedoMultiRoute(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string query = "update nobel_route set [default]=0 where id=" + row.Cells[0].Text + ";";
            query += "update nobel_accountrole set [status]=0 where route_id=" + row.Cells[0].Text + " and class="+ hf_id.Value +";";
            dbhelper.getdata(query);
            MultiRoutes();
           
        }
    }

    protected void clickMultiRoutes(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            hf_action.Value = "3";
            hf_id.Value = row.Cells[0].Text;
            modal_subs.Style.Add("display", "block");
            MultiRoutes();
            
        }
    }

    protected void MultiRoutes()
    {
        DataTable dt = getdata.sys_routes(int.Parse(hf_id.Value));
        gv_Subs.DataSource = dt;
        gv_Subs.DataBind();
    }

    protected void click_save(object sender, EventArgs e)
    {
        if (check())
        {
            string query = hf_action.Value == "0" ?
                "insert into nobel_route values ('" + tb_name.Text.Trim() + "','" + tb_url.Text.Replace(" ", "") + "','" + tb_phy.Text.Replace(" ", "") + "','" + tb_icon.Text.Trim() + "','" + tb_description.Text.Trim() + "',0,1)" :
                "update nobel_route set name='" + tb_name.Text.Trim() + "', url='" + tb_url.Text.Trim() + "', path='" + tb_phy.Text.Trim() + "', icon='" + tb_icon.Text.Trim() + "', description='" + tb_description.Text.Trim() + "' where id=" + hf_id.Value;
            dbhelper.getdata(query);

            switch (hf_action.Value)
            {
                case "3":
                    modal.Style.Add("display", "none");
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record successfully save');location = self['location'].href", true);
                    break;
            }

        }
    }

    protected bool check()
    {
        bool val = true;

        l_name.Text = tb_name.Text.Replace(" ", "").Length == 0 ? "*" : "";

        l_con.Text = tb_icon.Text.Replace(" ", "").Length == 0 ? "*" : "";

        if (p_action.Visible)
        {
            //l_url.Text = tb_url.Text.Replace(" ", "").Length == 0 ? "*" : "";
            //if (l_url.Text.Length == 0)
            //{
            //    string condition = hf_action.Value == "1" || hf_action.Value == "3" ? "and id !=" + hf_id.Value : "";
            //    DataTable dt = dbhelper.getdata("select * from nobel_route where url='" + tb_url.Text.Replace(" ", "") + "' " + condition);
            //    l_url.Text = dt.Rows.Count == 0 ? "" : "* existing";
            //}

          
            
            l_phy.Text = tb_phy.Text.Replace(" ", "").Length == 0 ? "*" : "";
            if (l_phy.Text.Length == 0)
                l_phy.Text = checkdirectory(tb_phy.Text.Replace(" ", "")) ? "" : "* not existing";
        }
        else
        {
            tb_url.Text = "-";
            tb_phy.Text = "-";
        }

        l_description.Text = tb_description.Text.Replace(" ", "").Length == 0 ? "*" : "";

        if ((l_name.Text + l_url.Text + l_con.Text + l_description.Text + l_phy.Text + l_con.Text).Contains("*"))
            val = false;

        return val;
    }

    protected void click_edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            
            hf_id.Value = row.Cells[0].Text;
            tb_name.Text = row.Cells[1].Text;
            tb_url.Text = row.Cells[2].Text;
            tb_icon.Text = row.Cells[4].Text;
            tb_phy.Text = row.Cells[3].Text;
            tb_description.Text = row.Cells[5].Text;
            modal.Style.Add("display", "block");


            p_action.Visible = row.Cells[2].Text == "-" ? false : true;
            lb_treeview.Visible = false;

            switch (hf_action.Value)
            {
                case "3":
                    //modal_subs.Style.Add("display", "none");
                    break;
                default:
                    hf_action.Value = "1";
                    break;
                
            }
            

            
        }
    }

    protected void click_ThreeChild(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hf_id.Value = row.Cells[0].Text;
            tb_name.Text = row.Cells[1].Text;
            tb_url.Text = row.Cells[2].Text;
            tb_icon.Text = row.Cells[4].Text;
            tb_phy.Text = row.Cells[3].Text;
            tb_description.Text = row.Cells[5].Text;
            modal.Style.Add("display", "block");
             

            InitModal();
        }
    }

    protected void InitModal()
    {
        p_action.Visible = true;
        lb_treeview.CssClass = "fa fa-toggle-off";
    }

    protected void click_delete(object sender, EventArgs e)
    {
        if (hf_action.Value == "2")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string query = "update nobel_account set status=1 where id=" + row.Cells[0].Text;
                dbhelper.getdata(query);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully');location = self['location'].href", true);
            }
        }
    }

    protected void click_close(object sender, EventArgs e)
    {
        tb_name.Text = "";
        tb_url.Text = "";
        tb_icon.Text = "";

        switch (hf_action.Value)
        {
            case "3":
                modal.Style.Add("display", "none");
                modal_transfer.Style.Add("display", "none");
                break;
            default:
                modal.Style.Add("display", "none");
                modal_transfer.Style.Add("display", "none");
                modal_subs.Style.Add("display", "none");
                break;
        }
    }

    protected void click_CloseMulti(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    protected void gvRouteBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lb = (LinkButton)e.Row.FindControl("lb_cnt");
            LinkButton transfer = (LinkButton)e.Row.FindControl("lb_transfer");
            if (e.Row.Cells[2].Text == "-")
            {
                lb.CssClass = "label label-danger";
                transfer.Enabled = false;
                transfer.CssClass = transfer.CssClass + " disabled";
            }
            else
            {
                lb.CssClass = "";
                lb.Text = "-";
            }

        }
    }
}