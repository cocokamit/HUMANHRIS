using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_report_absentlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            disp();
        }
    }
    protected void lodable()
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
        DataTable dt = dbhelper.getdata("select top 20 a.id,c.idnumber,c.lastname+', '+c.firstname+' '+ c.middlename ename,e.company,f.branch,left(convert(varchar,a.date,101),10)date, " +
                                        "case when g.position is null then 'Not Defined' else g.position end position, " +
                                        "d.shiftcode, " +
                                        "case when a.absent='True' then 1 else 0 end + case when a.halfdayabsent='True' then 0.5 else 0 end total " +
                                        "from TDTRLine a " +
                                        "left join TDTR b on a.dtrid=b.id " +
                                        "left join Memployee c on a.employeeid=c.id " +
                                        "left join Mshiftcode d on a.shiftcodeid=d.id " +
                                        "left join mcompany e on c.companyid=e.id " +
                                        "left join mbranch f on c.branchid=f.id " +
                                        "left join mposition g on c.positionid=g.id " +
                                        "where b.id is not null and case when a.absent='True' then 1 else 0 end + case when a.halfdayabsent='True' then 1 else 0 end>0 and b.payroll_id is not null " +
                                        "order by e.company,f.branch,a.date asc");
        grid_absent.DataSource = dt;
        grid_absent.DataBind();
    }
    protected void click_search(object sender, EventArgs e)
    {
        string query = "select top 20 a.id,c.idnumber,c.lastname+', '+c.firstname+' '+ c.middlename ename,e.company,f.branch,left(convert(varchar,a.date,101),10)date, " +
                                         "case when g.position is null then 'Not Defined' else g.position end position, " +
                                         "d.shiftcode, " +
                                         "case when a.absent='True' then 1 else 0 end + case when a.halfdayabsent='True' then 0.5 else 0 end total " +
                                         "from TDTRLine a " +
                                         "left join TDTR b on a.dtrid=b.id " +
                                         "left join Memployee c on a.employeeid=c.id " +
                                         "left join Mshiftcode d on a.shiftcodeid=d.id " +
                                         "left join mcompany e on c.companyid=e.id " +
                                         "left join mbranch f on c.branchid=f.id " +
                                         "left join mposition g on c.positionid=g.id " +
                                         "where b.id is not null and case when a.absent='True' then 1 else 0 end + case when a.halfdayabsent='True' then 1 else 0 end>0 and b.payroll_id is not null ";
                                        if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
                                        {
                                        query += "and LEFT(CONVERT(varchar,a.date,101),10)>='" + txt_from.Text + "' and  LEFT(CONVERT(varchar,a.date,101),10)<='" + txt_to.Text + "'";
                                        }
                                        if (int.Parse(ddl_pg.SelectedValue) > 0)
                                        {
                                        query += " and c.PayrollGroupId=" + ddl_pg.SelectedValue + "";
                                        }
                                        if (txt_search.Text.Length > 0)
                                        {
                                        query += " and c.LastName + '' + c.FirstName + '' + c.MiddleName+''+c.IdNumber like '%" + txt_search.Text + "%'";
                                        }    
                                        query+="order by e.company,f.branch,a.date asc";
        DataTable dt = dbhelper.getdata(query);
        grid_absent.DataSource = dt;
        grid_absent.DataBind();
    }
}