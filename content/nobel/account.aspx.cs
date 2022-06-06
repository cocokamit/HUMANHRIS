using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class content_su_account : System.Web.UI.Page
{
    //naa pay bug inig add nko sa routes para hr admin
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (Session.Count == 0)
            Response.Redirect("login");

        if(!IsPostBack)
            bind();
    }

    protected void gvRightBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lb = (LinkButton)e.Row.FindControl("lb_tree");
            if (e.Row.Cells[1].Text == "0")
            {
                lb.ForeColor = Color.Silver;
                lb.Enabled = false;
                lb.Visible = false;
            }
        }
    }

    protected void bind()
    {
        grid_view.DataSource = getdata.sys_accounts();
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
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
        dbhelper.getdata("update nobel_accountrole set seqs =" + preference + " where id=" + locationId);
    }

    protected void BindRoutesCollection(string account)
    {
        DataTable dt = dbhelper.getdata("select (select COUNT(*) from nobel_route where [default]=(select TOP(1)id from nobel_route order by id desc)) cnt, " +
        "case when (select [status] from nobel_accountrole where route_id=(select TOP(1)id from nobel_route order by id desc) and [acc_id]=" + account + ") is null then 0 else (select [status] from nobel_accountrole where route_id=(select TOP(1)id from nobel_route order by id desc) and [acc_id]=" + account + ") end pick," +
        "case when (select [status] from nobel_accountrole where route_id=(select TOP(1)id from nobel_route order by id desc) and [acc_id]=" + account + ") is null then 0 else 1 end exist," +
        "case when a.url ='-' then 1 else 0 end treeview," +
        "a.id, a.name,a.[description] " +
        "from nobel_route a " +
        "where a.[system] = 1 " +
        "and a.[default] = 0 order by a.name");

        ViewState["sys_routes"] = dt;
        gv_route.DataSource = dt;
        gv_route.DataBind();
    }

    protected void click_addroutes(object sender, EventArgs e)
    {
        p_routes.Visible = true;
        p_edit.Visible = false;
        p_collection.Visible = false;
        lb_treeview.Visible = true;
        
        BindRoutesCollection(hf_id.Value);
    }

    protected void click_addback(object sender, EventArgs e)
    {
        p_routes.Visible = false;
        p_collection.Visible = true;
        p_edit.Visible = true;
        lb_treeview.Visible = true;
    }
    
    protected void dataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label l = (Label)e.Row.FindControl("l_status");
            if (int.Parse(e.Row.Cells[1].Text) == 0)
                l.Visible = true;
            else
                l.Text = e.Row.Cells[1].Text;
        }
    }

    protected void gv_routeBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckRow = (CheckBox)e.Row.FindControl("cb");
            CheckRow.Checked = e.Row.Cells[2].Text == "1" ? true : false;
        }
    }

    protected void click_add(object sender, EventArgs e)
    {

        BindRoutesCollection("1");

        hf_action.Value = "0";
        tb_account.Text = "";
        

        p_edit.Visible = true;
        p_collection.Visible = false;
        lb_back.Visible = false;
        p_routes.Visible = true;

        modal.Style.Add("display", "block");
    }

    protected void DeleteAccount(object sender, EventArgs e)
    {
        if (hf_action.Value == "2")
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                dbhelper.getdata("update nobel_account set status='1' where id=" + row.Cells[0].Text);
                bind();
            }
        }
    }

    protected void click_save(object sender, EventArgs e)
    {
        int j = 0;

        if (check())
        {
            string query = hf_action.Value == "0" ? "insert into nobel_account values ('" + tb_account.Text.Trim() + "',0); select scope_identity() id;" : "update nobel_account set name='" + tb_account.Text.Trim() + "' where id=" + hf_id.Value + ";";
            DataTable dt = dbhelper.getdata(query);
            hf_id.Value = dt.Rows.Count > 0 ? dt.Rows[0]["id"].ToString() : hf_id.Value;

            if (p_routes.Visible)
            {
                foreach (GridViewRow row in gv_route.Rows)
                {
                    string hold = row.Cells[4].Text;
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckRow = (row.Cells[0].FindControl("cb") as CheckBox);
                        if (row.Cells[2].Text == "0" && row.Cells[3].Text == "0" && CheckRow.Checked)
                        {
                            query += "insert into nobel_accountrole values (" + hf_id.Value + "," + row.Cells[0].Text + ",0,1,1,1,1," + j + ",1);";
                            if (row.Cells[1].Text == "1")
                            {
                                int i = 0;
                                dt = dbhelper.getdata("select * from nobel_route where [default]=" + row.Cells[0].Text);
                                foreach (DataRow r in dt.Rows)
                                {
                                    query += "insert into nobel_accountrole values (" + hf_id.Value + "," + r[0].ToString() + "," + row.Cells[0].Text + ",1,1,1,1," + i + ",1);";
                                    i++;
                                }
                            }
                            j++;
                        }

                        if (row.Cells[2].Text == "1" && row.Cells[3].Text == "1" && !CheckRow.Checked)
                        {
                            query += "update nobel_accountrole set status=0 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text + ";";
                            if (row.Cells[1].Text == "1")
                            {
                                int i = 0;
                                dt = dbhelper.getdata("select * from nobel_route where [default]=" + row.Cells[0].Text);
                                foreach (DataRow r in dt.Rows)
                                {
                                    query += "update nobel_accountrole set status=0 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text + ";";
                                    i++;
                                }
                            }
                            j++;
                        }

                        if (row.Cells[2].Text == "0" && row.Cells[3].Text == "1" && CheckRow.Checked)
                        {
                            query += "update nobel_accountrole set status=1 where acc_id=" + hf_id.Value + " and route_id=" + row.Cells[0].Text + ";";
                            if (row.Cells[1].Text == "1")
                            {
                                int i = 0;
                                dbhelper.getdata("delete nobel_accountrole where acc_id=" + hf_id.Value + " and class = " + row.Cells[0].Text);
                                
                                dt = dbhelper.getdata("select * from nobel_route where [default]=" + row.Cells[0].Text);
                                foreach (DataRow r in dt.Rows)
                                {
                                    query += "insert into nobel_accountrole values (" + hf_id.Value + "," + r[0].ToString() + "," + row.Cells[0].Text + ",1,1,1,1," + i + ",1);";
                                    i++;
                                }
                            }
                            j++;
                        }
                    }
                }
            }

            dbhelper.getdata(query);

            p_routes.Visible = false;
            p_collection.Visible = true;
            p_edit.Visible = true;
            BindAccountRoutes(hf_id.Value);
        }
    }

    protected bool check()
    {
        bool val = true;
        
        l_name.Text = tb_account.Text.Replace(" ", "").Length == 0 ? "*" : "";
        
        if ((l_name.Text).Contains("*"))
            val = false;

        return val;
    }

    protected void click_edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hf_action.Value = "1";
            hf_id.Value = row.Cells[0].Text;
            tb_account.Text = row.Cells[2].Text;
            modal.Style.Add("display", "block");
          

            BindAccountRoutes(row.Cells[0].Text);
            BindRoutesCollection(hf_id.Value);

            if (gv_right.Rows.Count > 0)
            {
                LinkButton lnkUp = (gv_right.Rows[0].FindControl("lnkUp") as LinkButton);
                LinkButton lnkDown = (gv_right.Rows[gv_right.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
                lnkUp.Enabled = false;
                lnkDown.Enabled = false;
            }
        }
    }

    protected void BindAccountRoutes(string x)
    {
        gv_right.DataSource = getdata.sys_routes(x,0);
        gv_right.DataBind();
    }

    protected void deleterights(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update nobel_accountrole set status=0 where id=" + row.Cells[0].Text);
            BindAccountRoutes(hf_id.Value);
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
        modal.Style.Add("display", "none");
        p_edit.Visible = true;
        p_routes.Visible = false;
    }
}