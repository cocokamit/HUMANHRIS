using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class content_hr_DTRfromBIOline : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            logsid.Value = function.Decrypt(Request.QueryString["logsid"].ToString(), true);
            disp();
        }
    }

    protected void disp()
    {
        string query = "SELECT a.idnumber,b.lastname+', '+b.firstname+' '+b.middlename e_name,a.biotime Date_Time FROM tdtrperpayrolperline a " +
                        "left join memployee b on a.empid=b.id " +
                        "where dtrperpayrol_id=" + logsid.Value + " " +
                        "order by a.idnumber,a.biotime asc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

    }
    protected void search(object sender, EventArgs e)
    {
        string query = "SELECT a.idnumber,b.lastname+', '+b.firstname+' '+b.middlename e_name,a.biotime Date_Time FROM tdtrperpayrolperline a " +
                                 "left join memployee b on a.empid=b.id " +
                               "where dtrperpayrol_id=" + logsid.Value + " and (b.firstname+''+b.middlename+''+b.lastname+''+ convert(varchar,a.idnumber)) like '%" + txt_search.Text + "%' " +
                               "order by a.idnumber,a.biotime asc";

        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }

}