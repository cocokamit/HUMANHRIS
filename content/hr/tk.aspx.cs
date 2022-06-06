using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_tk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            loadable();
            //disp();
        }
    }
    protected void loadable()
    {
        DataTable dt = dbhelper.getdata("select * from mDepartment");
        ddl_dept.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_dept.Items.Add(new ListItem(dr["Department"].ToString(), dr["id"].ToString()));
        }
    }

    protected void btn_go(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {
        string day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        string month = DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
        DataTable dtperiod = adjustdtrformat.payrollperiod("all", "1");
        string from = txt_from.Text.Length > 0 ? txt_from.Text : month + "/01/" + DateTime.Now.Year;
        string to = txt_to.Text.Length > 0 ? txt_to.Text : month + "/" + day + "/" + DateTime.Now.Year;
        if (txt_to.Text.Length == 0 && txt_to.Text.Length == 0)
        {
            if (dtperiod.Rows.Count > 0)
            {
                from = dtperiod.Rows[0]["ffrom"].ToString();
                to = dtperiod.Rows[0]["tto"].ToString();
            }
        }
        DataTable dtr = Core.DTRF(from, to, "dept_" + ddl_dept.SelectedValue);
        grid_item.DataSource = dtr;
        grid_item.DataBind();
    }
    protected void ordb(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk_ot = (LinkButton)e.Row.FindControl("lnk_ot");
            LinkButton lnk_tadj = (LinkButton)e.Row.FindControl("lnk_tadj");
            LinkButton lnk_offset = (LinkButton)e.Row.FindControl("lnk_offset");
            string scid = e.Row.Cells[23].Text;
            string[] datenname = e.Row.Cells[3].Text.Split('-');
            string[] getdatefromout = e.Row.Cells[9].Text.Split(' ');
            DataTable chkdtsc = dbhelper.getdata("select * from MShiftCodeDay a where a.status is null and a.shiftcodeid=" + scid + " and a.day='" + datenname[1] + "'");
            DataTable dthd = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(date,a.date,101),10)=convert(date,'" + datenname[0] + "')");
            DataTable dtotpolicy = dbhelper.getdata("select c.id otrolesid,c.policyid otpolicyid,c.considered,c.roles from memployee a left join MCompany b on a.CompanyId=b.Id left join sys_policy_roles c on b.ot_roles=c.id where a.id=" + Session["emp_id"].ToString() + "");
            DataTable dtchkot = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where  (status like '%Approved%' or status like '%For Approval%') and LEFT(CONVERT(date,date,101),10)=convert(date,'" + datenname[0] + "') and EmployeeId=" + Session["emp_id"] + "");
            DataTable dtchkmanual = dbhelper.getdata("select *  from Tmanuallogline where  status like '%Approved%' and LEFT(CONVERT(date,date,101),10)=convert(date,'" + datenname[0].ToString() + "') and EmployeeId=" + Session["emp_id"] + "");
            DataTable dtoffset = dbhelper.getdata("select *  from toffset where  status like '%Approved%' and LEFT(CONVERT(date,appliedfrom,101),10)=convert(date,'" + datenname[0].ToString() + "') and empid=" + Session["emp_id"] + "");

            decimal consideredhrs = 0;
            DataRow[] getrd = chkdtsc.Select("restday='True'");

            if (e.Row.Cells[10].Text == "True")
            {
                e.Row.Cells[10].Text = "✓";
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[10].Text = "--";

            if (e.Row.Cells[11].Text == "True")
            {
                e.Row.Cells[11].Text = "✓";
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[11].Text = "--";

            if (e.Row.Cells[12].Text == "True")
            {
                e.Row.Cells[12].Text = "✓";
                e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[12].Text = "--";

            if (e.Row.Cells[13].Text == "True")
            {
                e.Row.Cells[13].Text = "✓";
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[13].Text = "--";

            DataTable chkwv = dbhelper.getdata("select * from TRestdaylogs a where a.status like '%Approved%' and  left(convert(varchar,a.date,101),10)='" + datenname[0] + "'");
            if (dtchkmanual.Rows.Count > 0)
                lnk_tadj.Visible = false;

            if (e.Row.Cells[9].Text == "--")
            {
                lnk_ot.Visible = false;
            }
            else
            {
                DateTime actout = Convert.ToDateTime(e.Row.Cells[9].Text);
                DateTime setupout = Convert.ToDateTime(datenname[0] + " " + chkdtsc.Rows[0]["timeout2"].ToString());
                if (actout > setupout)
                {
                    lnk_offset.Visible = true;
                    // if (actout >= setupout.AddMinutes(30))
                    if (actout >= setupout.AddHours(1))
                        lnk_ot.Visible = true;
                    if (dtchkot.Rows.Count > 0 || dtoffset.Rows.Count > 0)
                    {
                        lnk_ot.Visible = false;
                        lnk_offset.Visible = false;
                    }
                }
            }




        }
    }
}