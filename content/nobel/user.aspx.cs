using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_su_user : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
            bind();
    }

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_admin.Checked)
        {
            tb_Adpass.Enabled = true;
            divider.Visible = true;
            tb_Adpass.Focus();
        }
        else
        {
            tb_Adpass.Enabled = false;
            divider.Visible = false;
            
        }
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
        gv_right.DataSource = getdata.access(x,1);
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
        string oi = "select case when b.url ='-' then 1 else 0 end treeview, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") end pick, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else 1 end exist, " +
        "b.id, b.name,b.[description]  " +
        "from nobel_accountrole a " +
        "left join nobel_route b on a.route_id=b.id " +
        "where a.acc_id=3 order by b.name";

        DataTable dt = dbhelper.getdata("select case when b.url ='-' then 1 else 0 end treeview, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") end pick, " +
        "case when (select [status] from nobel_userRight where route_id=a.route_id and [user_id]=" + user + ") is null then 0 else 1 end exist, " +
        "b.id, b.name,b.[description]  " +
        "from nobel_accountrole a " +
        "left join nobel_route b on a.route_id=b.id " +
        "where a.acc_id=3 order by b.name");

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

            if( e.Row.Cells[1].Text != "0" )
            {
                GridView gv = (GridView)e.Row.Cells[2].FindControl("gv_treechild");
                string query = "select a.id,a.[user_id],a.route_id, a.status,b.name from nobel_userright a " +
                "left join nobel_route b on a.route_id=b.id " +
                "where a.[user_id]=" + hf_id.Value + " " +
                "and a.[default]=" + e.Row.Cells[2].Text;
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
            lb.CssClass = e.Row.Cells[1].Text == "0" ? "button fa fa-check-circle pull-right" : "button fa fa-times-circle text-red pull-right";
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

    protected void load()
    {
        BindAccount();
    }

    protected void BindAccount()
    {
        DataTable dt = getdata.sys_accounts();
        foreach (DataRow row in dt.Rows)
        {
            ddl_account.Items.Add(new ListItem(row["name"].ToString(), row["id"].ToString()));
        }
    }

    protected void  BindEmployee()
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

    protected void bind()
    {
        load();

        DataTable dt = dbhelper.getdata("select c.idnumber,  case when a.[status] = 1 then 'Enable' else 'Disable' end [status], a.id, a.acc_id,b.name acc_name, a.emp_id, a.name, a.username, a.[password],a.[status] " +
        "from nobel_user a  " +
        "left join nobel_account b on a.acc_id = b.id " +
        "left join memployee c on a.emp_id = c.id " +
        "where acc_id > 1 order by a.name");
        ViewState["sys_user"] = dt;
        //grid_view.DataBind();
        //alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void click_add(object sender, EventArgs e)
    {
        hf_action.Value = "0";
        p_routes.Visible = false;
        p_sys_routes.Visible = false;
        ddl_user.CssClass = "select2";
        ddl_user.Enabled = true;
        ddl_account.Enabled = true;
        modal.Style.Add("display", "block");

        BindEmployee();
    }

    protected void click_save(object sender, EventArgs e)
    {
        if (check())
        {
            string query = hf_action.Value == "0" ?
            "declare @id int insert into nobel_user values (" + ddl_account.SelectedValue + "," + ddl_user.SelectedValue + ",'" + ddl_user.SelectedItem + "','" + tb_username.Text.Trim() + "','" + tb_password.Text.Trim() + "',1) set @id = scope_identity() " :
            "update nobel_user set acc_id=" + ddl_account.SelectedValue + ", emp_id=" + ddl_user.SelectedValue + ", name='" + ddl_user.SelectedItem + "',username='" + tb_username.Text.Trim() + "',password='" + tb_password.Text.Trim() + "' where id=" + hf_id.Value+";";


            if (cb_admin.Checked)
            {
                query += "if exists (select * from nobel_user where acc_id=1 and emp_id=" + ddl_user.SelectedValue + ") " +
                "    update nobel_user set password='"+tb_Adpass.Text.Replace(" ","")+"', status=1 where acc_id=1 and emp_id=" + ddl_user.SelectedValue + " " +
                "else " +
                " begin " +
                "    insert into nobel_user (acc_id,emp_id,name,username,password,status) " +
                "    select 1,emp_id,name,username,'" + tb_Adpass.Text.Replace(" ", "") + "',status " +
                "    from nobel_user where emp_id=" + ddl_user.SelectedValue + " " +
                " end";
            }
            else
                query += "update nobel_user set status=0 where acc_id=1 and emp_id=" + ddl_user.SelectedValue + ";";

            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record successfully save');location = self['location'].href", true);
 
        }
    }

    protected bool check()
    {
        bool val = true;

        l_account.Text = ddl_account.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_user.Text = ddl_user.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_username.Text = tb_username.Text.Replace(" ", "").Length == 0 ? "*" : "";
        l_password.Text = tb_password.Text.Replace(" ", "").Length == 0 ? "*" : "";

        if ((l_account.Text + l_user.Text + l_username.Text + l_password.Text).Contains("*"))
            val = false;

        return val;
    }

    protected void click_edit(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["sys_user"];
        DataRow[] row = dt.Select("id=" + hf_id.Value);

        hf_action.Value = "1";

        hf_account.Value = row[0]["acc_id"].ToString();
        hf_employee.Value = row[0]["emp_id"].ToString();

        BindEmployee();
        //BindAccountRoutes(row.Cells[0].Text);

        ddl_account.Text = row[0]["acc_id"].ToString();
        ddl_user.Text = row[0]["emp_id"].ToString();
        tb_username.Text = row[0]["username"].ToString();
        tb_password.Text = row[0]["password"].ToString();

        //admin prevelendge

        dt = dbhelper.getdata("select * from nobel_user where acc_id=1 and emp_id=" + row[0]["emp_id"].ToString());
        if (dt.Rows.Count == 0)
        {
            cb_admin.Checked = false;
            tb_Adpass.Enabled = false;
            divider.Visible = false;
            tb_Adpass.Text = string.Empty;
        }
        else
        {
            if (dt.Rows[0]["status"].ToString() == "0")
            {
                tb_Adpass.Enabled = false;
                divider.Visible = false;
                tb_Adpass.Text = string.Empty;

                cb_admin.Checked = false;
            }
            else
            {
                cb_admin.Checked = true;
                tb_Adpass.Enabled = true;
                divider.Visible = true;
                tb_Adpass.Text = dt.Rows[0]["password"].ToString();
            }
        }


        ddl_user.CssClass = "";
        ddl_user.Enabled = false;
        //ddl_account.Enabled = false;
        modal.Style.Add("display", "block");

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