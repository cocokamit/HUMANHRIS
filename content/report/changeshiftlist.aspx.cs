using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_report_changeshiftlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadable();
            disp();
        }
    }
    protected void loadable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void disp()
    {
        string query = "select top 100 a.id, left(convert(varchar,a.date,101),10)changedate,a.Remarks,b.LastName +', '+b.FirstName ename,b.IdNumber,c.ShiftCode,d.PayrollGroup from TChangeShiftLine a  " +
                "left join MEmployee b on a.EmployeeId=b.Id " +
                "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                "left join MPayrollGroup d on b.PayrollGroupId=d.Id " +
                "where a.dtr_id is not null and a.status='Approved'";
        query += " order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_display.DataSource = dt;
        grid_display.DataBind();

     
    }
    protected void click_search(object sender, EventArgs e)
    {
        string query = "select a.id, left(convert(varchar,a.date,101),10)changedate,a.Remarks,b.LastName +', '+b.FirstName ename,b.IdNumber,c.ShiftCode,d.PayrollGroup from TChangeShiftLine a  " +
                        "left join MEmployee b on a.EmployeeId=b.Id " +
                        "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                        "left join MPayrollGroup d on b.PayrollGroupId=d.Id " +
                        "where a.dtr_id is not null and a.status='Approved'";
        if (int.Parse(ddl_pg.SelectedValue) > 0)
            query += " and d.id="+ddl_pg.SelectedValue+"";
        if(txt_search.Text.Length>0)
            query += " and a.Remarks+''+b.FirstName +''+b.LastName like '%"+txt_search.Text+"%'";
        if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
            query += "and left(convert(varchar,a.date,101),10)>=" + txt_from.Text + " and left(convert(varchar,a.date,101),10)<="+txt_to.Text+" ";
 


        DataTable dt = dbhelper.getdata(query);
        grid_display.DataSource = dt;
        grid_display.DataBind();
        
    }
}