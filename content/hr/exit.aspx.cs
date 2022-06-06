using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_exit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (!IsPostBack)
            loadable();

    }

    protected void loadable()
    {
        string query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);

        //ddl_dtrfile.Items.Add(" ");
        foreach (DataRow dr in dtt.Rows)
        {
            drop_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }

        //DataTable dt = dbhelper.getdata("select * from MPayrollGroup order by id asc");
        //ddl_payroll_group.Items.Clear();
        //foreach (DataRow dr in dt.Rows)
        //{
        //    ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        //}

        //DataTable dt1 = dbhelper.getdata("select * from MBranch order by id asc");
        //drop_branch.Items.Clear();
        //foreach (DataRow dr in dt1.Rows)
        //{
        //    drop_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["Id"].ToString()));
        //}



    }

    protected void save_resignation(object sender, EventArgs e)
    {
        stateclass a = new stateclass();

        a.sa = drop_emp.SelectedValue;
        a.sb = txt_date.Text;
        a.sc = drop_type.Text;
        a.sd = dro_con.Text;
        a.se = txt_description.Text;
        a.sf = txt_notes.Text;
        bol.exit(a);

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='exit?user_id=" + function.Encrypt(key.Value, true) + "'", true);
    }
}