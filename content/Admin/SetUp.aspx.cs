using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;

public partial class content_Admin_SetUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("login");

        if (!IsPostBack)
        {
            disp();

            loader();
        }


    }
    protected void onSetupSave(object sender, EventArgs e)
    {
        string les = "";
        foreach (ListItem li in liststatus.Items)
        {
            if (li.Selected)
            {
                les += li.Value + ",";
            }
        }

        string elt = "";
        foreach (ListItem li in listlt.Items)
        {
            if (li.Selected)
            {
                elt += li.Value + ",";
            }
        }
        string ldn = "";
        foreach (ListItem li in listdivision.Items)
        {
            if (li.Selected)
            {
                ldn += li.Value + ",";
            }
        }

        DataTable dt = dbhelper.getdata("Select * from SetUpTable");

        string type = ddlLeave.SelectedValue == "1" ? ddlYearly.SelectedValue : ddlLeave.SelectedValue;
        txt_leaveRes.Text = ddlLeave.SelectedValue != "1" ? "" : txt_leaveRes.Text;
        txt_leaveext.Text = ddlLeave.SelectedValue != "1" ? "" : txt_leaveext.Text;

        string typename = ddlLeave.SelectedValue == "1" ? ddlYearly.SelectedItem.Text : ddlLeave.SelectedItem.Text;


        if (dt.Rows.Count > 0)
        {
            dbhelper.getdata("Update SetUpTable set LeaveType=" + type + ",LeaveResetDate='" + txt_leaveRes.Text + "',LeaveExtension='" + txt_leaveext.Text + "',LeaveTypeName='" + typename + "', LeaveMonthlyAdd='" + hfleavear.Value + "',LeaveWithin='" + ddlconsump.SelectedValue + "',LeaveExtDefault=" + txt_dce.Text + ",LeaveEmpStatus='" + les + "',LeaveExtLt='" + elt + "',LeaveDivision='"+ldn+"'  where id=" + dt.Rows[0]["id"].ToString() + "");
        }
        else
        {
            dbhelper.getdata("Insert into SetUpTable values(" + type + ",'" + txt_leaveRes.Text + "','" + txt_leaveext.Text + "','" + typename + "','" + hfleavear.Value + "','" + ddlconsump.SelectedValue + "'," + txt_dce.Text + ",'" + les + "','" + elt + "','"+ldn+"')");
        }
        disp();
    }

    [WebMethod]
    public static string getLeaveAdd()
    {
        DataTable dt = dbhelper.getdata("Select * from SetUpTable");
        string[] additionals = dt.Rows[0]["LeaveMonthlyAdd"].ToString().Split(',');
        return dt.Rows[0]["LeaveMonthlyAdd"].ToString();
    }



    protected void disp()
    {
        liststatus.Items.Clear();
        listlt.Items.Clear();
        listdivision.Items.Clear();
        DataTable dt = dbhelper.getdata("Select * from SetUpTable");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["LeaveType"].ToString() != "2")
            {
                ddlLeave.Items.FindByValue("1").Selected = true;
                ddlYearly.Items.FindByValue(dt.Rows[0]["LeaveType"].ToString()).Selected = true;

                lcrd.Style.Add("display", "block");
                ytype.Style.Add("display", "block");
                leex.Style.Add("display", "block");

                if (ddlYearly.SelectedValue == "4")
                {
                    vlv.Style.Add("display", "block");
                }
                else
                {
                    vlv.Style.Add("display", "none");
                }

                if (dt.Rows[0]["LeaveWithin"].ToString() != "")
                {
                    ddlconsump.Items.FindByValue(dt.Rows[0]["LeaveWithin"].ToString()).Selected = true;
                }

                DataTable dtt = dbhelper.getdata("Select * from mempstatus_setup");

                string[] ddd = dt.Rows[0]["LeaveEmpStatus"].ToString().Split(',');
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    ListItem li = new ListItem(dtt.Rows[i]["status"].ToString(), dtt.Rows[i]["Id"].ToString());
                    if (ddd.Contains(dtt.Rows[i]["Id"].ToString()))
                    {
                        li.Selected = true;
                        liststatus.Items.Add(li);
                    }
                    else
                    {
                        li.Selected = false;
                        liststatus.Items.Add(li);
                    }
                }

                dtt = dbhelper.getdata("Select * from MDivision2");

                ddd = dt.Rows[0]["LeaveDivision"].ToString().Split(',');
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    ListItem li = new ListItem(dtt.Rows[i]["Division2"].ToString(), dtt.Rows[i]["Id"].ToString());
                    if (ddd.Contains(dtt.Rows[i]["Id"].ToString()))
                    {
                        li.Selected = true;
                        listdivision.Items.Add(li);
                    }
                    else
                    {
                        li.Selected = false;
                        listdivision.Items.Add(li);
                    }
                }

                dtt = dbhelper.getdata("Select * from MLeave where [action] is null");

                ddd = dt.Rows[0]["LeaveExtLt"].ToString().Split(',');
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    ListItem li = new ListItem(dtt.Rows[i]["Leave"].ToString() + "   ctc-" + dtt.Rows[i]["converttocash"].ToString(), dtt.Rows[i]["LeaveType"].ToString());
                    if (ddd.Contains(dtt.Rows[i]["LeaveType"].ToString()))
                    {
                        li.Selected = true;
                        listlt.Items.Add(li);
                    }
                    else
                    {
                        li.Selected = false;
                        listlt.Items.Add(li);
                    }
                }

            }
            else
            {
                ddlLeave.Items.FindByValue("2").Selected = true;
                lcrd.Style.Add("display", "none");
                ytype.Style.Add("display", "none");
                leex.Style.Add("display", "none");
            }

            txt_leaveRes.Text = dt.Rows[0]["LeaveResetDate"].ToString();
            txt_leaveext.Text = dt.Rows[0]["LeaveExtension"].ToString();
            txt_dce.Text = dt.Rows[0]["LeaveExtDefault"].ToString();

        }
        else
        {
            DataTable dtt = dbhelper.getdata("Select * from mempstatus_setup");
            foreach (DataRow dr in dtt.Rows)
            {
                liststatus.Items.Add(new ListItem(dr["status"].ToString(), dr["Id"].ToString()));
            }

            dtt = dbhelper.getdata("Select * from MDivision2");
            foreach (DataRow dr in dtt.Rows)
            {
                listdivision.Items.Add(new ListItem(dr["Division2"].ToString(), dr["Id"].ToString()));
            }

            dtt = dbhelper.getdata("Select * from MLeave where [action] is null");
            foreach (DataRow dr in dtt.Rows)
            {
                listlt.Items.Add(new ListItem(dr["Leave"].ToString() + "   ctc-" + dr["converttocash"].ToString(), dr["LeaveType"].ToString()));
            }
            txt_leaveext.Text = DateTime.Now.ToString("");
            txt_leaveRes.Text = DateTime.Now.ToString("01 January");
            txt_dce.Text = "";
        }
    }

    [WebMethod]
    public static string[] Getleavetypes(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            string query = string.Format("Select * from MLeave where [action] is null and LeaveType like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["LeaveType"], reader["LeaveType"]));
                }
            }

            con.Close();
        }
        return retCategory.ToArray();
    }


    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            ListBox lbsc = (e.Row.FindControl("txt_cond2") as ListBox);
            lbsc.DataSource = dbhelper.getdata("Select * from mempstatus_setup");
            lbsc.DataTextField = "status";
            lbsc.DataValueField = "Id";
            lbsc.DataBind();

            string[] ddd = e.Row.Cells[5].Text.Split(',');
            if (ddd.Count() > 1)
            {
                foreach (string d in ddd)
                {
                    if (d != "")
                    {
                        lbsc.Items.FindByValue(d).Selected = true;
                    }
                }
            }
        }
    }


    protected void loader()
    {
        DataTable dt = dbhelper.getdata("Select a.*,b.* from MLeave a Left Join LeaveTypeSetUp b on a.Id=b.LeaveTypeId where [action] is null");
        grid_forms.DataSource = dt;
        grid_forms.DataBind();

        alert.Visible = grid_forms.Rows.Count == 0 ? true : false;
    }

    [WebMethod]
    public static string saveauto(string statusreqid, string leaveid)
    {
        DataTable dt = dbhelper.getdata("Select * from LeaveTypeSetUp where LeaveTypeId=" + leaveid + "");

        if (dt.Rows.Count > 0)
        {
            dbhelper.getdata("Update LeaveTypeSetUp set StatusReq='" + statusreqid + "' where LeaveTypeId=" + leaveid + "");
        }
        else
        {
            dbhelper.getdata("Insert into LeaveTypeSetUp values(" + leaveid + ",'" + statusreqid + "',NULL)");
        }
        return "aaa";
    }

}