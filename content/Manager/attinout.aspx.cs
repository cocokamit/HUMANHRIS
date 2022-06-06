using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_attinout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            pay_range();
            loadable();
            loadsc();
            BindData();
        }
    }


   
    protected void btn_go(object sender, EventArgs e)
    {
        BindData();
    }

    protected void pay_range()
    {
        DataTable dt = dbhelper.getdata("select * from memployee where id=" + Session["emp_id"].ToString() + "");
        string query = "select id ,left(convert(varchar,dfrom,101),10)ffrom,left(convert(varchar,dtoo,101),10) tto from payroll_range where action is null order by dfrom desc";

         
    }

    protected void BindData()
    {
        string section = pnl_grid.Visible ? " and b.sectionid=" + ddl_section.SelectedValue.ToString() : "";

        string query = "where a.biotime between convert(datetime,'" + tbFrom.Text.Replace(" ", "") + "') and convert(datetime,'" + tbTo.Text.Replace(" ", "") + "') + 1 " +
        "and b.DepartmentId= " + Session["department"] + " " + section + " " +
        "order by a.biotime desc";

        DataTable dt = getdata.GetEmpRawLogs(query);
        ViewState["data"] = dt;
        pnl_grid.Visible = dt.Rows.Count == 0 ? false : true;
        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }

    protected void loadable()
    {

        sql_income.ConnectionString = Config.connection();
        //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
        DataTable sc = dbhelper.getdata("select shiftcode +' ('+ Remarks + ')' as shiftcode,id,remarks from  MShiftCode where status is null order by shiftcode ");

        foreach (DataRow dr in sc.Rows)
        {
            ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
            item.Attributes.Add("title", dr["remarks"].ToString());

            //li.Attributes["title"] = li.Text;
            ddl_shiftcode.Items.Add(item);

        }

        string query = "select * from msection where dept_id=" + Session["department"] + " order by seccode";
        DataTable dt = dbhelper.getdata(query);
        ddl_section.Items.Add(new ListItem("All", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_section.Items.Add(new ListItem(dr["seccode"].ToString(), dr["sectionid"].ToString()));
        }

        ddl_section.SelectedIndex = 1;
        pnlSection.Visible = dt.Rows.Count == 1 ? false : true;
    }

    protected void loadsc()
    {
        DataTable sc = dbhelper.getdata("select shiftcode +' ('+ Remarks + ')' as shiftcode,id,remarks from  MShiftCode where status is null order by shiftcode ");

        foreach (DataRow dr in sc.Rows)
        {
            ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());

            item.Attributes.Add("title", dr["remarks"].ToString());

            //li.Attributes["title"] = li.Text;
            ddl_sc.Items.Add(item);

        }
    }
}