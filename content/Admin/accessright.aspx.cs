using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Admin_accessright : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
        {
            BindData("0");
        }
    }

    protected void grid_view_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_view.PageIndex = e.NewPageIndex;
        BindData("0");
    }

    protected void click_search(object sender, EventArgs e)
    {
        BindData("a.name like '%" + tb_search.Text.Trim() + "%'");
    }

    protected void ChangePreference(object sender, EventArgs e)
    {
        string commandArgument = (sender as LinkButton).CommandArgument;

        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        int locationId = Convert.ToInt32(gv_right.Rows[rowIndex].Cells[0].Text);
        int preference = Convert.ToInt32(rowIndex);
        preference = commandArgument == "up" ? preference - 1 : preference + 1;
        this.UpdatePreference(locationId, preference);

        rowIndex = commandArgument == "up" ? rowIndex - 1 : rowIndex + 1;
        locationId = Convert.ToInt32(gv_right.Rows[rowIndex].Cells[0].Text);
        preference = Convert.ToInt32(rowIndex);
        preference = commandArgument == "up" ? preference + 1 : preference - 1;
        this.UpdatePreference(locationId, preference);

        BindAccountRoutes(hf_id.Value);
    }

    private void UpdatePreference(int locationId, int preference)
    {
        dbhelper.getdata("update nobel_userright set seqs =" + preference + " where id=" + locationId);
    }

    protected void BindAccountRoutes(string x)
    {
        DataTable dt = getdata.userAccess("a.id = " + x + " and class=0 order by b.seqs");
        ViewState["RouteCollection"] = dt;
        gv_right.DataSource = dt ;
        gv_right.DataBind();
    }

    protected void deleterights(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update nobel_userright set status=0 where user_id=" + hf_id.Value + " and id=" + row.Cells[0].Text);
            BindAccountRoutes(hf_id.Value);
        }
    }

    protected void click_addroutes(object sender, EventArgs e)
    {
        p_sys_routes.Visible = true;
        p_routes.Visible = false;
        p_details.Visible = false;
        BindRoutesCollection(hf_id.Value);
    }

    protected void BindRoutesCollection(string user)
    {
        DataTable dt = dbhelper.getdata("select case when b.url ='-' then 1 else 0 end treeview, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") end pick, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else 1 end exist, " +
        "b.id, b.name,b.[description]  " +
        "from nobel_accountrole a " +
        "left join nobel_route b on a.route_id=b.id " +
        "where a.acc_id=" + hf_account.Value + " and a.class=0 order by a.seqs");
         
        gv_route.DataSource = dt;
        gv_route.DataBind();
    }

    protected void gv_routeBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckRow = (CheckBox)e.Row.FindControl("cb");
            CheckRow.Checked = e.Row.Cells[2].Text == "1" ? true : false;
        }
    }

    protected void gv_rightsBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Panel drop = (Panel)e.Row.FindControl("p_tree");
            drop.Visible = e.Row.Cells[1].Text == "0" ? false : true;

            if (e.Row.Cells[1].Text != "0")
            {
                GridView gv = (GridView)e.Row.Cells[2].FindControl("gv_treechild");
                string query = "select a.id,a.[user_id],a.route_id, a.status,b.name from nobel_userright a " +
                "left join nobel_route b on a.route_id=b.id " +
                "where a.[user_id]=" + hf_id.Value + " " +
                "and a.[default]=" + e.Row.Cells[2].Text + " order by name";
                DataTable dt = dbhelper.getdata(query);

                gv.DataSource = dt;
                gv.DataBind();

            }
        }
    }

    protected void gv_treechildBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("lb_delete");
            lb.CssClass = e.Row.Cells[1].Text == "0" ? "fa fa-check-circle pull-right" : "fa fa-times-circle text-danger pull-right";
        }
    }

    protected void click_TreeChild(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)row.FindControl("lb_delete");
            string x = lb.CssClass.ToString().Contains("check") ? "1" : "0";

            dbhelper.getdata("update nobel_userright set status=" + x + " where id=" + row.Cells[0].Text);
            BindAccountRoutes(hf_id.Value);
        }
    }

    protected void click_addback(object sender, EventArgs e)
    {
        p_sys_routes.Visible = false;
        p_routes.Visible = true;
        p_details.Visible = true;
    }

    protected void BindEmployee()
    {
        ddl_user.Items.Clear();

        ListItem d = new ListItem { Text = "", Value = "" };
        d.Attributes.Add("selected", "");
        d.Attributes.Add("hidden", "");
        d.Attributes.Add("disabled", "");
        ddl_user.Items.Add(d);

        DataTable dt = dbhelper.getdata("select a.id, a.firstname,a.lastname, " +
       "case when b.id is null then 0 else 1 end naa " +
       "from MEmployee a " +
       "left join nobel_user b on a.id = b.emp_id order by firstname +' '+ lastname");
        foreach (DataRow row in dt.Rows)
        {
            if (hf_action.Value == "0")
            {
                if (row["naa"].ToString() == "0")
                    ddl_user.Items.Add(new ListItem(row["firstname"].ToString() + " " + row["lastname"].ToString(), row["id"].ToString()));
            }
            else
            {
                if (row["naa"].ToString() == "1")
                    ddl_user.Items.Add(new ListItem(row["firstname"].ToString() + " " + row["lastname"].ToString(), row["id"].ToString()));
            }
        }
    }

    protected void BindData(string condition)
    {
        condition = condition == "0" ? "" : "and " + condition;
        DataTable dt = dbhelper.getdata("select  case when a.[status] = 1 then 'Enable' else 'Disable' end [status], a.id, a.acc_id,b.name acc_name, a.emp_id, a.name, a.username, a.[password],a.[status] " +
        "from nobel_user a  " +
        "left join nobel_account b on a.acc_id = b.id " +
        "left join memployee c on a.emp_id = c.id " +
        "where acc_id > 1 " + condition + " order by a.name");
        grid_view.DataSource = ViewState["sys_user"] = dt;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
        
    }

    protected void click_add(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        p_routes.Visible = false;
        p_sys_routes.Visible = false;
        ddl_user.CssClass = "select2";
        ddl_user.Enabled = true;
        modal.Style.Add("display", "block");

        BindEmployee();
    }

    protected void click_save(object sender, EventArgs e)
    {
        if (check())
        {
            string query = hf_action.Value == "0" ?
            "declare @id int insert into nobel_user values (" + ddlRole.SelectedValue + "," + ddl_user.SelectedValue + ",'" + ddl_user.SelectedItem + "','" + tb_username.Text.Trim() + "','" + tb_password.Text.Trim() + "',1) set @id = scope_identity() " :
            "update nobel_user set acc_id=" + ddlRole.SelectedValue + ", emp_id=" + ddl_user.SelectedValue + ", name='" + ddl_user.SelectedItem + "',username='" + tb_username.Text.Trim() + "',password='" + tb_password.Text.Trim() + "' where id=" + hf_id.Value;

            if (p_sys_routes.Visible)
            //{
            //    DataTable routes = getdata.sys_routes(ddl_account.SelectedValue);
            //    foreach (DataRow route in routes.Rows)
            //    {
            //        if (route["class"].ToString() == "0")
            //        {
            //            query += "insert into nobel_userRight values (@id," + route["route_id"] + ",1,1,1,1," + route["seqs"] + ",0,1,0)";
            //            if (route["url"].ToString() == "-")
            //            {
            //                string oi = "acc_id=" + ddl_account.SelectedValue + " and class=" + route["route_id"];
            //                DataRow[] found = routes.Select("acc_id=" + ddl_account.SelectedValue + " and class=" + route["route_id"]);
            //                foreach (DataRow item in found)
            //                {
            //                    query += "insert into nobel_userRight values (@id," + item["route_id"] + ",1,1,1,1," + item["seqs"] + "," + route["route_id"] + ",1,0)";
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            {
                int j = 0;
                foreach (GridViewRow row in gv_route.Rows)
                {
                    Label l_lable = (row.Cells[4].FindControl("l_lable") as Label);

                    string log = l_lable.Text;
                    if (log.Contains("Approval"))
                    {

                    }

                    CheckBox CheckRow = (row.Cells[4].FindControl("cb") as CheckBox);
                    if (row.Cells[2].Text == "0" && row.Cells[3].Text == "0" && CheckRow.Checked)
                    {
                        query += "insert into nobel_userRight values (" + hf_id.Value + "," + row.Cells[0].Text + ",1,1,1,1," + j + ",0,1,0)";
                        if (row.Cells[1].Text == "1")
                        {
                            DataTable treeview = getdata.sys_routes(ddlRole.SelectedValue, int.Parse(row.Cells[0].Text));
                            foreach (DataRow item in treeview.Rows)
                            {
                                query += "insert into nobel_userRight values (" + hf_id.Value + "," + item["route_id"] + ",1,1,1,1," + item["seqs"] + "," + row.Cells[0].Text + ",1,0)";
                            }
                        }
                        j++;
                    }

                    if (row.Cells[2].Text == "1" && row.Cells[3].Text == "1" && !CheckRow.Checked)
                    {
                        query += "update nobel_userRight set status=0 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text;
                        if (row.Cells[1].Text == "1")
                        {
                            int i = 0;
                            DataTable dt = dbhelper.getdata("select * from nobel_userRight where [default]=" + row.Cells[0].Text);
                            foreach (DataRow r in dt.Rows)
                            {
                                query += "update nobel_accountrole set status=0 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text;
                                i++;
                            }
                        }
                    }

                    if (row.Cells[2].Text == "0" && row.Cells[3].Text == "1" && CheckRow.Checked)
                    {
                        query += "update nobel_userRight set status=1 where [user_id]=" + hf_id.Value + " and route_id=" + row.Cells[0].Text;
                        query += "update nobel_accountrole set status=1, seqs=" + j + " where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text;
                        if (row.Cells[1].Text == "1")
                        {
                            int i = 0;
                            DataTable dt = dbhelper.getdata("select * from nobel_route where [default]=" + row.Cells[0].Text);
                            foreach (DataRow r in dt.Rows)
                            {
                                query += " if exists(select * from nobel_userRight where [default]=" + row.Cells[0].Text + " and [user_id]=" + hf_id.Value + " and route_id=" + r["id"] + ") " +
                                "update nobel_userRight set status=1 where [user_id]=" + hf_id.Value + " and route_id= " + r["id"] + "  " +
                                "else  " +
                                "insert into nobel_userRight values (" + hf_id.Value + "," + r["id"] + ",1,1,1,1," + i + "," + row.Cells[0].Text + ",1,0) ";
                                //query += "update nobel_accountrole set status=1 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text;
                                i++;
                            }
                        }
                        j++;
                    }
                }
            }

            dbhelper.getdata(query);

            if (p_sys_routes.Visible == true)
            {
                BindEmployee();
                ddl_user.Text = hf_employee.Value;
                BindAccountRoutes(hf_id.Value);

                p_sys_routes.Visible = false;
                p_routes.Visible = true;
                p_details.Visible = true;
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record successfully save');location = self['location'].href", true);
        }
    }

    protected bool check()
    {
        bool val = true;

        lRoute.Text = ddlRole.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_user.Text = ddl_user.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_username.Text = tb_username.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_password.Text = tb_password.Text.Replace(" ", "").Length == 0 ? "*" : "";

        if ((lRoute.Text + l_user.Text + l_username.Text + l_password.Text).Contains("*"))
            val = false;

        return val;
    }

    protected void click_edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           
            hf_action.Value = "1";
            hf_id.Value = row.Cells[0].Text;
            hf_account.Value = row.Cells[1].Text;
            hf_employee.Value = row.Cells[2].Text;

            BindEmployee();
            //BindAccountRoutes(row.Cells[0].Text);

            LinkButton lb = (LinkButton)row.Cells[4].FindControl("lnk_edit");
            l_employee.Text = lb.Text.Trim();
             
            ddlRole.SelectedValue = row.Cells[1].Text;
            ddl_user.Text = row.Cells[2].Text;
            tb_username.Text = row.Cells[5].Text;
            tb_password.Text = row.Cells[6].Text;
           

            ddl_user.CssClass = "";
            ddl_user.Enabled = false;
            modal.Style.Add("display", "block");
            p_routes.Visible = false;
        }
    }

    protected void click_breakdown(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //LinkButton drop = (LinkButton)row.FindControl("lb_break");
            //drop.CssClass = "fa fa-angle-down pull-right";
        }
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
        modal.Style.Add("display", "none");
        //tb_name.Text = "";
        //tb_url.Text = "";
        //tb_icon.Text = "";
    }
}