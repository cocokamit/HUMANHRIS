using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_editdaytype : System.Web.UI.Page
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count==0)
            Response.Redirect("quit");

        if (!IsPostBack)
        {
            lodable();
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            appid.Value = function.Decrypt(Request.QueryString["app_id"].ToString(), true);
            disp();
        }
    }
    protected void lodable()
    {
    
        DataTable dt;

        string query = "select * from MCompany order by id desc";
        dt = dbhelper.getdata(query);

        ddl_company.Items.Clear();
        ddl_company.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_company.Items.Add(new ListItem(dr["company"].ToString(), dr["id"].ToString()));
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.DayType,a.WorkingDays,a.RestdayDays,a.OTWorkingDays,a.OTRestdayDays,a.NightWorkingDays,a.NightRestdayDays from MDayType a " +
            "left join MDayTypeDay b on a.id=b.DayTypeId " +
            "where a.id=" + appid.Value + "");
        if (dt.Rows.Count > 0)
        {

            txt_daytype.Text = dt.Rows[0]["DayType"].ToString();
            txt_mow.Text = dt.Rows[0]["WorkingDays"].ToString();
            txt_mor.Text = dt.Rows[0]["RestdayDays"].ToString();

            txt_owm.Text = dt.Rows[0]["OTWorkingDays"].ToString();
            txt_orm.Text = dt.Rows[0]["OTRestdayDays"].ToString();

            txt_nwm.Text = dt.Rows[0]["NightWorkingDays"].ToString();
            txt_nrm.Text = dt.Rows[0]["NightRestdayDays"].ToString();
            DataTable dts = dbhelper.getdata("select a.id as line_id,b.branch,left(convert(varchar,a.Date,101),10)Date,a.remarks from MDayTypeDay a left join MBranch b on a.branchid=b.id where DayTypeId=" + appid.Value + " and a.status is null");
            if (dts.Rows.Count > 0)
            {
                grid_view.DataSource = dts;
                grid_view.DataBind();
            }
            else
            {
                grid_view.Enabled = false;
            }
        }
        else
        {
            grid_view.Enabled = false;
        }
        
    }
    protected void click_company(object sender, EventArgs e)
    {
        string query = "select * from MBranch where CompanyId=" + ddl_company.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        ddl_branch.Items.Clear();
        ddl_branch.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }
    }
    protected void Update(object sender, EventArgs e)
    {

        //string eif = chk_eif.Checked ? "True" : "False";
        //string wa = chk_wa.Checked ? "True" : "False";
        DataTable dtcheck = dbhelper.getdata("select * from MDayType where DayType='"+txt_daytype.Text.Trim()+"' and id<>"+appid.Value+" and status is null");
        if (dtcheck.Rows.Count == 0)
        {
            if (txt_date_holiday.Text.Trim().Length == 0)
            {
                dbhelper.getdata("update MDayType set  DayType='" + txt_daytype.Text + "' , WorkingDays=" + txt_mow.Text + ",RestdayDays=" + txt_mor.Text + ",UpdateUserId=" + function.Decrypt(Request.QueryString["user_id"].ToString(), true) + ",UpdateDateTime=getdate() where id =" + appid.Value + "");
                Response.Redirect("editdaytype?app_id="+function.Encrypt(appid.Value, true)+"", false);
            }
            else
            {//txt_wdb.Text.Trim().Length == 0 || txt_wda.Text.Trim().Length == 0 ||
                if ( txt_remarks.Text.Trim().Length == 0)
                {
                    Response.Write("<script>alert('Empty feild for adding new line!')</script>");
                }
                else if(ddl_branch.SelectedValue=="0")
                {
                    DataTable dt = dbhelper.getdata("select * from MBranch");
                    foreach (DataRow dr in dt.Rows)
                    {
                        dbhelper.getdata("insert into MDayTypeDay values (" + appid.Value + "," + dr["Id"] + ",'" + txt_date_holiday.Text + "','0:00:00','0:00:00',NULL,'" + txt_remarks.Text + "',NULL,NULL)");
                    }
                    dbhelper.getdata("update MDayType set  DayType='" + txt_daytype.Text + "' , WorkingDays=" + txt_mow.Text + ",RestdayDays=" + txt_mor.Text + ",UpdateUserId='" + Session["emp_id"].ToString() + "',UpdateDateTime=getdate() where id =" + appid.Value + "");
                    Response.Redirect("editdaytype?app_id=" + function.Encrypt(appid.Value, true) + "", false);
                }
                else
                {
                    dbhelper.getdata("update MDayType set  DayType='" + txt_daytype.Text + "' , WorkingDays=" + txt_mow.Text + ",RestdayDays=" + txt_mor.Text + ",UpdateUserId='"+Session["emp_id"].ToString()+"',UpdateDateTime=getdate() where id =" + appid.Value + "");
                    dbhelper.getdata("insert into MDayTypeDay values (" +appid.Value + "," + ddl_branch.SelectedValue + ",'" + txt_date_holiday.Text + "','0:00:00','0:00:00',NULL,'" + txt_remarks.Text + "',NULL,NULL)");
                    Response.Redirect("editdaytype?app_id="+function.Encrypt(appid.Value, true)+"", false);
                }
            }
        }
        else
        {
            Response.Write("<script>alert('Day Type is already exist!')</script>");
        }
    }
    protected void cancel_line(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MDayTypeDay set status='cancel-" + Session["emp_id"].ToString() + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("editdaytype?app_id="+function.Encrypt(appid.Value, true)+"", false);
            }
            else
            { 
            }
        }
    }
}