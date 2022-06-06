using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_Admin_employee_position : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
        {
            load();
            BindData("0");
        }

        loadable();
    }

    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("select a.id,a.LastName+', '+a.firstname+' '+a.MiddleName+' ' fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id left join nobel_user c on c.emp_id = a.Id where c.name like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["fullname"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void click_search(object sender, EventArgs e)
    {
        BindData("c.id=" + lbl_bals.Value + "");
        //BindData("a.name like '%" + tb_search.Text.Trim() + "%'");
    }
    protected void load()
    {
        DataTable dt = dbhelper.getdata("select * from nobel_account where status=0 order by name desc");
        foreach (DataRow dr in dt.Rows)
        {
            ddlRole.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
            ddlRole2.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadable()
    {
        DataTable dt = dbhelper.getdata("select *,(select COUNT(name) from nobel_user where acc_id=nobel_account.id) hcount from nobel_account where status=0 order by hcount desc");
        foreach (DataRow dr in dt.Rows)
        {
            //populate heads list-------------------------------------------------------------

            HtmlGenericControl div2 = new HtmlGenericControl("div");
            HtmlGenericControl div3 = new HtmlGenericControl("div");
            HtmlGenericControl div5 = new HtmlGenericControl("div");
            HtmlGenericControl label = new HtmlGenericControl("label");
            LinkButton searchIcon = new LinkButton();
            HtmlGenericControl p = new HtmlGenericControl("p");

            searchIcon.Attributes.Add("Class", "fa fa-search Float right");
            searchIcon.Attributes.Add("Style", "margin-right:10px; background-color:white; padding:3px; border-radius:50px;");
            searchIcon.Attributes.Add("runat", "server");
            searchIcon.Click += (sender, EventArgs) => { RoleSearch_Click(sender, EventArgs, dr["id"].ToString()); }; ;

            div2.Attributes.Add("Class", "col-md-2");
            div3.Attributes.Add("Class", "tile-stats bg-aquaa");
            div3.Attributes.Add("Style", "padding:0; border-radius:20px;");
            label.InnerText = dr["hcount"].ToString();
            div5.Attributes.Add("Class", "count");
            div5.Attributes.Add("Style", "Font-size:15px;");
            div5.Controls.Add(label);
            div5.Controls.Add(searchIcon);
            p.InnerText = dr["name"].ToString();
            div3.Controls.Add(p);
            div3.Controls.Add(div5);
            div2.Controls.Add(div3);

            Countss.Controls.Add(div2);

        }
    }
    private void RoleSearch_Click(object sender, EventArgs e, string id)
    {
        int idd = Convert.ToInt32(id);
        BindData("acc_id=" + idd);

    }
    protected void BindData(string condition)
    {
        condition = condition == "0" ? "" : "and " + condition;
        DataTable dt = dbhelper.getdata("select  case when a.[status] = 1 then 'Enable' else 'Disable' end [status], a.id, a.acc_id,b.name acc_name, a.emp_id, a.name, a.username, a.[password],a.[status],(Select COUNT(emp_id) from Approver where under_id= a.emp_id) [underM] " +
        "from nobel_user a  " +
        "left join nobel_account b on a.acc_id = b.id " +
        "left join memployee c on a.emp_id = c.id " +
        "where acc_id > 1 " + condition + " order by underM desc");
        grid_view.DataSource = ViewState["sys_user"] = dt;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");
        replaceModal.Style.Add("display", "none");
        modalM.Style.Add("display", "none");
        accessModal.Style.Add("display", "none");
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

    [WebMethod]
    public static string getrolevalue(string i)
    {
        string val = i;
        DataTable dt = dbhelper.getdata("select case when a.[status] = 1 then 'Enable' else 'Disable' end [status], a.id, a.acc_id,b.name acc_name,a.emp_id, a.name, a.username, a.[password],a.[status],(Select COUNT(emp_id) from Approver where under_id= a.emp_id)[underM] from nobel_user a  left join nobel_account b on a.acc_id = b.id left join memployee c on a.emp_id = c.id where a.emp_id = " + i + "");
        return dt.Rows[0]["acc_id"].ToString();
    }

    protected void click_saveRep(object sender, EventArgs e)
    {
        string a = ddlRole2.SelectedValue;
        string b = hf_account.Value;
        string ee;
        DataTable dtttts = dbhelper.getdata("select under_id from Approver where emp_id = " + hf_employee.Value + "");
        if (dtttts.Rows.Count > 0)
        {
            ee = dtttts.Rows[0]["under_id"].ToString();
            string query = "Update Approver set under_id = " + ddl_employee.SelectedValue + " where emp_id = " + hf_employee.Value + ""
                + "Update nobel_user set acc_id = " + b + " where emp_id = " + hf_employee.Value + ""
                + "Update nobel_user set acc_id = " + a + " where emp_id = " + ddl_employee.SelectedValue + ""
                + "Update tmanuallogline set approver_id = " + ddl_employee.SelectedValue + ",status = 'for approval' where EmployeeId = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update dtr_period  set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where emp_id = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update TOverTimeLine set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where EmployeeId = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update TRestdaylogs set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where EmployeeId = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update Ttravel set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where emp_id = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update Tundertime set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where emp_id = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update temp_shiftcode set status = 'for approval',approver_id = " + ddl_employee.SelectedValue + " where emp_id = " + hf_employee.Value + " and status = 'for approval' and approver_id = " + ee + ""
                + "Update toffset set status = 'for approval',aproverid=" + ddl_employee.SelectedValue + " where empid = " + hf_employee.Value + " and status = 'for approval' and aproverid = " + ee + ""
                + "BEGIN TRANSACTION update a set a.status = 'for approval' from TLeaveApplicationLine a inner join Tleave b on b.id = a.l_id where b.approver_id=" + hf_employee.Value + ""
                + "update b set b.approver_id = " + ddl_employee.SelectedValue + " from Tleave b inner join TLeaveApplicationLine a on b.id = a.l_id where b.approver_id=" + ee + " and status = 'for approval' COMMIT "
                + "Update texitclearance set status = 'for approval',nextapproverid = " + ddl_employee.SelectedValue + " where empid = " + hf_employee.Value + " and status = 'for approval' and nextapproverid = " + ee + ""
                + "Update tfinalexit set status = 'for approval',nextapproverid = " + ddl_employee.SelectedValue + " where empid = " + hf_employee.Value + " and status = 'for approval' and nextapproverid = " + ee + "";

            DataTable dt = dbhelper.getdata(query);
            Response.Redirect(Request.RawUrl.ToString());
        }
        else
        {
        }
    }

    protected void click_save(object sender, EventArgs e)
    {
        if (check())
        {
            DataTable dt = dbhelper.getdata("Update nobel_user set acc_id=" + ddlRole.SelectedValue + ",username='" + tb_username.Text + "',password='" + tb_password.Text + "' where emp_id=" + hf_employee.Value + "");
            if (ddlRole.SelectedItem.ToString() == "Approver")
            {
                dbhelper.getdata("update MEmployee set level = 1 where Id = " + hf_employee.Value + "");
            }
            if (ddlRole.SelectedItem.ToString() == "Member")
            {
                dbhelper.getdata("update MEmployee set level = 3 where Id = " + hf_employee.Value + "");
            }
            Response.Redirect(Request.RawUrl.ToString());
        }
    }


    protected void click_saveM(object sender, EventArgs e)
    {
        DataTable dt;
        DataTable dt2 = dbhelper.getdata("Select herarchy,emp_id from Approver where under_id=" + hf_employee.Value + "");
        List<string> ctns = hf_cbvalues.Value.Split(',').ToList();
        string sqlquery = "";
        int counter = 0;
        if (dt2.Rows.Count > 0)
        {
            for (int i = 0; i < chkMemberList.Items.Count; i++)
            {
                int d = Convert.ToInt32(chkMemberList.Items[i].Value);
                if (chkMemberList.Items[i].Selected)
                {
                    if (!ctns.Contains(chkMemberList.Items[i].Value))
                    {
                        sqlquery += saver(d);
                    }
                    else
                    {
                        DataTable dsd = dbhelper.getdata("Select herarchy,emp_id from Approver where emp_id=" + d + " and under_id=" + hf_employee.Value + "");
                        string find = "herarchy = 1";
                        DataRow[] foundRows = dsd.Select(find);
                        if (dsd.Rows.Count < 2)
                        {
                            if (hfchk.Value == "YES")
                            {
                                if (foundRows.Count() == 0)
                                {
                                    sqlquery += saver(d);
                                }
                            }
                            else { sqlquery += saver(d); }
                        }
                    }
                }

                
            }

        }
        else
        {
            for (int i = 0; i < chkMemberList.Items.Count; i++)
            {
                int d = Convert.ToInt32(chkMemberList.Items[i].Value);
                if (chkMemberList.Items[i].Selected)
                {
                    if (!ctns.Contains(chkMemberList.Items[i].Value))
                    {
                        sqlquery += saver(d);
                    }
                }
            }
        }
        for (int i = 0; i < chksched.Items.Count; i++)
        {
            int d = Convert.ToInt32(chksched.Items[i].Value);
            if (!chksched.Items[i].Selected)
            {
                if (ctns.Contains(chksched.Items[i].Value))
                {
                    sqlquery += "Delete from Approver where under_id=" + hf_employee.Value + " and emp_id=" + d + "";
                }
            }
        }

        for (int i = 0; i < chkappr.Items.Count; i++)
        {
            int d = Convert.ToInt32(chkappr.Items[i].Value);
            if (!chkappr.Items[i].Selected)
            {
                if (ctns.Contains(chkappr.Items[i].Value))
                {
                    sqlquery += "Delete from Approver where under_id=" + hf_employee.Value + " and emp_id=" + d + "";
                }
            }
        }

        if (sqlquery != "")
        {
            dt = dbhelper.getdata(sqlquery);
        }

        Response.Redirect(Request.RawUrl.ToString());
    }

    protected string saver(int d)
    {
        string sqlquery = "";
        if (ViewState["acc_name"].ToString() == "Scheduler")
        {
            sqlquery += "Delete from Approver where emp_id=" + d + " and herarchy=0 " +
                       "Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + ",0)";
        }
        else if (ViewState["acc_name"].ToString() == "Approver and Scheduler")
        {
            if (hfchk.Value == "YES")
            {
                //DataTable dtt = dbhelper.getdata("Select COUNT(id) [herarchy] from Approver where emp_id=" + d + " and herarchy>0");
                //if (dtt.Rows.Count > 0)
                //{
                //    sqlquery += "Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + "," + (Convert.ToInt32(dtt.Rows[0]["herarchy"].ToString()) + 1) + ")";
                //}
                DataTable dtt = dbhelper.getdata("Select top 1 herarchy from Approver where emp_id=" + d + " and herarchy>0 order by herarchy desc");
                string c = "1";
                if (dtt.Rows.Count > 0)
                {
                    c = (Convert.ToInt32(dtt.Rows[0]["herarchy"].ToString()) + 1).ToString();
                }
                sqlquery += "Delete from Approver where emp_id=" + d + " and herarchy<>0 " +
                            " Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + "," + c + ")";
                sqlquery += updateapproversM(hf_employee.Value, d.ToString());
            }
            else
            {
                sqlquery += "Delete from Approver where emp_id=" + d + " and herarchy=0 " +
                          "Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + ",0)";
            }
        }
        else
        {
            //DataTable dtt = dbhelper.getdata("Select COUNT(id) [herarchy] from Approver where emp_id=" + d + " and herarchy>0");
            //if (dtt.Rows.Count > 0)
            //{
            //    sqlquery += "Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + "," + (Convert.ToInt32(dtt.Rows[0]["herarchy"].ToString()) + 1) + ")";
            //}

            //########################################################################################################################
            DataTable dtt = dbhelper.getdata("Select top 1 herarchy,under_id from Approver where emp_id=" + d + " and herarchy>0 order by herarchy desc");
            string c = "1";
            if (dtt.Rows.Count > 0)
            {
                c = (Convert.ToInt32(dtt.Rows[0]["herarchy"].ToString()) + 1).ToString();
            }
            sqlquery += "Delete from Approver where emp_id=" + d + " and herarchy<>0" +
                        " Insert into Approver(emp_id,under_id,herarchy) Values(" + d + "," + hf_employee.Value + "," + c + ")";
            sqlquery += updateapproversM(hf_employee.Value, d.ToString());
        }

        return sqlquery;
    }

    protected string updateapproversM(string NewAppr, string empid)
    {

        //string query = " Update tmanuallogline set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and status like '%For Approval%'" +
        //                " Update dtr_period  set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
        //                " Update TOverTimeLine set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and status like '%For Approval%'" +
        //                " Update TRestdaylogs set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and status like '%For Approval%'" +
        //                " Update Ttravel set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
        //                " Update Tundertime set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
        //                " Update temp_shiftcode set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
        //                " Update toffset set aproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'" +
        //                " Update b set b.approver_id = " + NewAppr + " from Tleave b inner join TLeaveApplicationLine a on b.id = a.l_id where a.EmployeeId=" + empid + " and status like '%For Approval%' " +
        //                " Update texitclearance set nextapproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'" +
        //                " Update tfinalexit set nextapproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'";
        //return query;

        string query = " Update tmanuallogline set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and dtr_id is null and status like '%For Approval%'" +
                        " Update dtr_period  set approver_id=" + NewAppr + " where emp_id=" + empid + " and dtr_id is null and status like '%For Approval%'" +
                        " Update TMealHours set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and dtr_id is null and status like '%For Approval%'" +
                        " Update TOverTimeLine set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and dtr_id is null and status like '%For Approval%'" +
                        " Update TRestdaylogs set approver_id=" + NewAppr + " where EmployeeId=" + empid + " and dtr_id is null and status like '%For Approval%'" +
                        " Update Ttravel set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
                        " Update Tundertime set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
                        " Update temp_shiftcode set approver_id=" + NewAppr + " where emp_id=" + empid + " and status like '%For Approval%'" +
                        " Update toffset set aproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'" +
                        " Update b set b.approver_id = " + NewAppr + " from Tleave b inner join TLeaveApplicationLine a on b.id = a.l_id where a.EmployeeId=" + empid + " and dtr_id is not null and status like '%For Approval%' " +
                        " Update texitclearance set nextapproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'" +
                        " Update tfinalexit set nextapproverid=" + NewAppr + " where empid=" + empid + " and status like '%For Approval%'";
        return query;
    }

    protected void grid_view_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_view.PageIndex = e.NewPageIndex;
        BindData("0");
    }

    protected void click_repledit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            hf_action.Value = "1";
            hf_id.Value = row.Cells[0].Text;
            hf_account.Value = row.Cells[1].Text;
            hf_employee.Value = row.Cells[2].Text;

            BindEmployee();
            //BindAccountRoutes(row.Cells[0].Text);

            LinkButton lb = (LinkButton)sender;
            if (lb.ID == "lnk_edit")
            {
                l_employee.Text = lb.CommandName.Trim();
                ddlRole.SelectedValue = row.Cells[1].Text;
                ddl_user.Text = row.Cells[2].Text;
                tb_username.Text = row.Cells[6].Text;
                tb_password.Text = row.Cells[7].Text;

                ddl_user.CssClass = "";
                ddl_user.Enabled = false;
                modal.Style.Add("display", "block");
            }
            else if (lb.ID == "lnk_members")
            {
                BindDataM("where a.name!='admin'");
                hfaccname.Value = row.Cells[3].Text;
                ViewState["acc_name"] = row.Cells[4].Text;
                hfrole.Value = row.Cells[4].Text;
                modalM.Style.Add("display", "block");
            }
            else if (lb.ID == "link_admin")
            {
                bind(row.Cells[2].Text);
                ViewState["idd"] = row.Cells[2].Text;
                accessModal.Style.Add("display", "block");
            }
            else
            {
                l_employee2.Text = lb.CommandName.Trim();
                ddlRole2.SelectedValue = row.Cells[1].Text;
                l_role.Text = row.Cells[4].Text;
                DataTable dt = dbhelper.getdata("select  case when a.[status] = 1 then 'Enable' else 'Disable' end [status], a.id, a.acc_id,b.name acc_name,a.emp_id, a.name, a.username, a.[password],a.[status],(Select COUNT(emp_id) from Approver where under_id= a.emp_id)[underM] from nobel_user a  left join nobel_account b on a.acc_id = b.id left join memployee c on a.emp_id = c.id where b.name not like '%Admin%'");
                dt.DefaultView.Sort = "name asc";
                dt = dt.DefaultView.ToTable();
                foreach (DataRow rowe in dt.Rows)
                {
                    ddl_employee.Items.Add(new ListItem(rowe["name"].ToString(), rowe["emp_id"].ToString()));
                }
                replaceModal.Style.Add("display", "block");
                ViewState["acc_name"] = row.Cells[4].Text;

            }
        }

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


    protected void refresh(object sender, EventArgs e)
    {
        BindData("0");
    }
    protected void refresh2(object sender, EventArgs e)
    {
        foreach (ListItem item in this.chkMemberList.Items)
        {
            item.Attributes.CssStyle["display"] = "inline";
        }

        this.chkMemberList.RepeatColumns = 2;
    }
    protected void click_searchM(object sender, EventArgs e)
    {
        //BindDataM("where a.name like '%"+tb_searchM.Text+"%'");
        foreach (ListItem item in this.chkMemberList.Items)
        {
            if (item.Text.ToUpper().Contains(tb_searchM.Text.ToUpper()))
            {
                item.Attributes.CssStyle["display"] = "inline";
            }
            else
            {
                item.Attributes.CssStyle["display"] = "none";
            }
        }

        this.chkMemberList.RepeatColumns = 1;
    }

    protected void BindDataM(string condition)
    {
        chkMemberList.Items.Clear();
        chkappr.Items.Clear();
        chksched.Items.Clear();
        lblsched.Text = "0";
        lblapp.Text = "0";
        condition = condition == "0" ? "" : condition;
        DataTable dt = dbhelper.getdata("select case when b.under_id=" + hf_employee.Value + " then 1 else 0 end unders,b.herarchy, a.name,a.emp_id from nobel_user a left join Approver b on a.emp_id=b.emp_id " + condition + " order by unders desc, a.name asc");

        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        foreach (DataRow drow in dt.Rows)
        {
            if (hTable.Contains(drow["emp_id"]))
            {

                if (drow["unders"].ToString() == "1")
                {

                }
                else
                {
                    duplicateList.Add(drow);
                }

            }
            else
            {
                hTable.Add(drow["emp_id"], string.Empty);
            }
        }

        foreach (DataRow dRow in duplicateList)
        {
            dt.Rows.Remove(dRow);
        }
        hftemplist.Value = "";
        hf_cbvalues.Value = "";

        string temp = "1";
        foreach (DataRow row in dt.Rows)
        {
            if (chkMemberList.Items.Contains(new ListItem(row["name"].ToString(), row["emp_id"].ToString())))
            {

            }
            else
            {
                chkMemberList.Items.Add(new ListItem(row["name"].ToString(), row["emp_id"].ToString()));
            }

            if (row["unders"].ToString() != "0")
            {
                ListItem li = new ListItem(row["name"].ToString(), row["emp_id"].ToString());
                li.Selected = true;

                hf_cbvalues.Value += row["emp_id"].ToString() + ",";
                if (row["herarchy"].ToString() == "0")
                {
                    chksched.Items.Add(li);
                    lblsched.Text = (Convert.ToInt32(lblsched.Text) + 1).ToString();
                }
                else
                {
                    chkappr.Items.Add(li);
                    lblapp.Text = (Convert.ToInt32(lblapp.Text) + 1).ToString();

                }
            }

            hftemplist.Value = hf_cbvalues.Value;
        }

        foreach (DataRow row in dt.Rows)
        {
            if (chksched.Items.Contains(new ListItem(row["name"].ToString(), row["emp_id"].ToString())) && chkappr.Items.Contains(new ListItem(row["name"].ToString(), row["emp_id"].ToString())))
            {
                chkMemberList.Items.Remove(new ListItem(row["name"].ToString(), row["emp_id"].ToString()));
            }
        }

    }

    //if (temp == row["herarchy"].ToString())
    //{
    //    chkappr.Items.Add(li);
    //}
    //else
    //{
    //    HtmlGenericControl div1 = new HtmlGenericControl("div");
    //    HtmlGenericControl div2 = new HtmlGenericControl("div");
    //    HtmlGenericControl labelers1 = new HtmlGenericControl("label");
    //    CheckBoxList chkbox = new CheckBoxList();
    //    div1.Attributes.Add("class", "row");
    //    div2.Attributes.Add("class", "col-md-12");
    //    chkbox.Attributes.Add("runat", "server");
    //    chkbox.Attributes.Add("RepeatLayout", "table");
    //    chkbox.Attributes.Add("RepeatDirection", "vertical");
    //    chkbox.Attributes.Add("CssClass", "checkbox checkbox-inline");
    //    chkbox.Attributes.Add("Style", "max-height:500px; overflow-y:scroll;");
    //    labelers1.InnerText = "Approver " + row["herarchy"].ToString();
    //    chkbox.ID = "Approver" + row["herarchy"].ToString();

    //    foreach()
    //    {   
    //        ListItem li = new ListItem(row["name"].ToString(), row["emp_id"].ToString());
    //        li.Selected = true;

    //    }
    //    div2.Controls.Add(labelers1);
    //    div2.Controls.Add(chkbox);
    //    div1.Controls.Add(div2);
    //    approvers.Controls.Add(div1);
    //    temp = row["herarchy"].ToString();
    //}

    protected void click_checkallnot(object sender, EventArgs e)
    {
        if (checkallnot.Checked)
        {
            foreach (ListItem item in chkMemberList.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            chkMemberList.ClearSelection();
        }

    }
    protected void gv_orderBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckRow = (CheckBox)e.Row.FindControl("CheckBox1");
            CheckBox read = (CheckBox)e.Row.FindControl("read");
            CheckBox update = (CheckBox)e.Row.FindControl("update");

            CheckRow.Checked = e.Row.Cells[1].Text == "1" ? true : false;
            read.Checked = e.Row.Cells[5].Text == "1" ? true : false;
            update.Checked = e.Row.Cells[6].Text == "1" ? true : false;

            if (CheckRow.Checked)
            {
                e.Row.Cells[2].Attributes.Add("Class", "bg-info");
            }

            GridView gvOrders = ((Control)sender).Parent.Parent.FindControl("gvOrders") as GridView;
            GridViewRow row = ((Control)sender).Parent.Parent as GridViewRow;
            if (row.Cells[3].Text != "Master File")
            {
                gvOrders.HeaderRow.Cells[3].Visible = false;
                gvOrders.HeaderRow.Cells[4].Visible = false;
                gvOrders.HeaderRow.Cells[2].Visible = false;

                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
            }
        }
    }
    protected void gv_routeBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckRow = (CheckBox)e.Row.FindControl("cb");
            CheckRow.Checked = e.Row.Cells[1].Text == "1" ? true : false;
            if (CheckRow.Checked)
            {
                if (CheckRow.Visible)
                {
                    e.Row.Cells[2].Attributes.Add("Class", "bg-info");
                }
            }

            string id = ViewState["idd"].ToString();
            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
            if (e.Row.Cells[3].Text == "Master File")
            {

                DataTable d = new DataTable();
                d.Columns.Add("id", typeof(System.Int32));
                d.Columns.Add("name", typeof(System.String));
                d.Columns.Add("cnt", typeof(System.String));
                d.Columns.Add("description", typeof(System.String));
                d.Columns.Add("default", typeof(System.Int32));
                d.Columns.Add("read", typeof(System.Int32));
                d.Columns.Add("update", typeof(System.Int32));

                int j = 0;
                string[] names = new string[] { "MasterAdd", "Profile", "Payroll", "Requirements", "Descipline Management", "Asset", "Report to", "Leave Credit", "Appraisal", "Audit Trail", "Status", "Note", "Compensation", "Change Payroll Rate", "Monitoring" };
                for (int i = 1900; i <= 1912; i++)
                {
                    DataTable dt = dbhelper.getdata("Select Count(*) [cnt]from nobel_userRight where route_id=" + i + " and user_id=" + id);
                    DataTable dt2 = dbhelper.getdata("Select [read],[update]from nobel_userRight where route_id=" + i + " and user_id=" + id);
                    if (dt2.Rows.Count > 0)
                    {
                        d.Rows.Add(i, names[j], Convert.ToInt32(dt.Rows[0]["cnt"]), "-", 2, Convert.ToInt32(dt2.Rows[0]["read"]), Convert.ToInt32(dt2.Rows[0]["update"]));
                    }
                    else
                    {
                        d.Rows.Add(i, names[j], Convert.ToInt32(dt.Rows[0]["cnt"]), "-", 2, 0, 0);
                    }
                    j++;
                }
                gvOrders.DataSource = d;
                gvOrders.DataBind();
            }
            else
            {
                gvOrders.DataSource = dbhelper.getdata("Select *,case when (Select COUNT(user_id) from nobel_userRight where route_Id=a.id and user_id=" + id + ")>0 then 1 else 0 end [cnt],[default][read],[default][update] from nobel_route a where [system]=2 and [default]=" + e.Row.Cells[0].Text + "");
                gvOrders.DataBind();
            }

        }
    }
    protected void bind(string id)
    {
        ViewState["idd"] = id;
        gv_route.DataSource = dbhelper.getdata("Select *,case when (Select COUNT(user_id) from nobel_userRight where route_Id=a.id and user_id=" + id + ")>0 then 1 else 0 end [cnt] from nobel_route a where [system]=2 and [default]=0 order by name asc");
        gv_route.DataBind();
    }


    protected void click_saveAccess(object sender, EventArgs e)
    {
        string id = ViewState["idd"].ToString();
        DataTable ddt = dbhelper.getdata("Select case when Count(seqs)>0 then MAX(seqs)else 0 end [max] from nobel_userRight a Left join nobel_route b on a.route_id=b.id where b.system=2 and user_id=" + id);

        int count = Convert.ToInt32(ddt.Rows[0]["max"].ToString()) + 1;
        string query = "";
        foreach (GridViewRow row in gv_route.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[1].FindControl("cb") as CheckBox);
                bool chk = chkRow.Checked;
                DataTable t = dbhelper.getdata("Select COUNT(*)[cnt] from nobel_userRight where [user_id]=" + id + " and route_id=" + row.Cells[0].Text + "");

                if (chk = chkRow.Checked)
                {
                    if (Convert.ToInt32(t.Rows[0]["cnt"].ToString()) == 0)
                    {
                        query += "Insert into nobel_userRight([user_id],[route_id],[update],[read],[create],[delete],[seqs]) values(" + id + "," + row.Cells[0].Text + ",1,1,1,1," + count + ")";
                        count++;
                    }
                }
                else
                {
                    query += "Delete from nobel_userRight where user_id=" + id + " and route_id=" + row.Cells[0].Text + "";
                }

                GridView gvorder = (row.Cells[1].FindControl("gvOrders") as GridView);
                if (gvorder != null)
                {
                    int cnt = 0;
                    int cnt2 = 0;
                    foreach (GridViewRow row2 in gvorder.Rows)
                    {
                        chkRow = (row2.Cells[1].FindControl("CheckBox1") as CheckBox);
                        string updatechk, readchk;
                        updatechk = (row2.Cells[4].FindControl("update") as CheckBox).Checked ? "1" : "0";
                        readchk = (row2.Cells[3].FindControl("read") as CheckBox).Checked ? "1" : "0";

                        chk = chkRow.Checked;
                        DataTable t2 = dbhelper.getdata("Select COUNT(*)[cnt] from nobel_userRight where [user_id]=" + id + " and route_id=" + row2.Cells[0].Text + "");

                        if (chk = chkRow.Checked)
                        {
                            if ((updatechk == "0" && readchk == "0") && row.Cells[3].Text == "Master File")
                            {
                                readchk = "1";
                            }

                            cnt2 = 1;
                            if (Convert.ToInt32(t2.Rows[0]["cnt"].ToString()) == 0)
                            {
                                query += "Insert into nobel_userRight([user_id],[route_id],[update],[read],[create],[delete],[seqs]) values(" + id + "," + row2.Cells[0].Text + "," + updatechk + "," + readchk + ",1,1," + count + ")";
                                count++;
                                cnt = 1;
                            }
                        }
                        else
                        {
                            query += "Delete from nobel_userRight where user_id=" + id + " and route_id=" + row2.Cells[0].Text + "";
                        }
                        if (cnt2 == 0)
                        {
                            query += "Delete from nobel_userRight where user_id=" + id + " and route_id=" + row.Cells[0].Text + "";
                        }
                    }
                    if (cnt == 1)
                    {
                        if (Convert.ToInt32(t.Rows[0]["cnt"].ToString()) == 0)
                        {
                            query += "Insert into nobel_userRight([user_id],[route_id],[update],[read],[create],[delete],[seqs]) values(" + id + "," + row.Cells[0].Text + ",1,1,1,1," + count + ")";
                            count++;
                        }
                    }
                }

            }

        }

        DataTable dt = dbhelper.getdata(query);
        Response.Redirect(Request.RawUrl.ToString());
    }


}