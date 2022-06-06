using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class content_hr_changeshiftlist : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lodable();
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }
    }
    protected void lodable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        ddl_viewpg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
            ddl_viewpg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_add_changeshift(object sender, EventArgs e)
    {
        Response.Redirect("addchangeshift?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select  a.id, LEFT(CONVERT(varchar,a.csdate,101),10)csdate,b.payrollgroup,a.remarks from TChangeShift a " +
                                        "left join MPayrollGroup b on a.payrollgroupid=b.id  where a.status like '%Approved%' and a.entryuserid="+Session["user_id"]+" ");
        grid_view.DataSource = dt;
        grid_view.DataBind();

        string query = "select a.id, left(convert(varchar,a.date,101),10)changedate,a.Remarks,b.LastName +', '+b.FirstName ename,b.IdNumber,c.ShiftCode,d.PayrollGroup from TChangeShiftLine a  " +
             "left join MEmployee b on a.EmployeeId=b.Id " +
             "left join MShiftCode c on a.ShiftCodeId=c.Id " +
             "left join MPayrollGroup d on b.PayrollGroupId=d.Id " +
             "where a.status='Approved' and month(a.date)=" + DateTime.Now.Month + "";
        query += " order by a.id desc";
        dt = dbhelper.getdata(query);
        grid_monthlymonitoring.DataSource = dt;
        grid_monthlymonitoring.DataBind();

    }
    protected void click_view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ppop(true);
            lbl_leaveid.Value = row.Cells[0].Text;
            DataTable dtotapp = dbhelper.getdata("select a.PayrollGroupId,left( convert(varchar,a.csDate,101),10) csDate,a.Remarks, b.Period from TChangeShift a " +
                                                 "left join MPeriod b on a.PeriodId=b.Id where a.id=" + row.Cells[0].Text + "");
            txt_viewremarks.Text = dtotapp.Rows[0]["Remarks"].ToString();
            employee(Session["emp_id"].ToString());
            DataTable dt = dbhelper.getdata("select a.id, left( convert(varchar,a.date,101),10)date,a.remarks,c.ShiftCode,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from TChangeShiftLine a " +
                                            "left join MEmployee b on a.EmployeeId=b.Id  " +
                                            "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                                            "where a.ChangeShiftId=" + lbl_leaveid.Value + " and a.status like '%Approved%'");

            grid_linedetails.DataSource = dt;
            grid_linedetails.DataBind();
             dt = dbhelper.getdata("select * from TChangeShift where id=" + row.Cells[0].Text + " and dtr_id is not null");
             if (dt.Rows.Count > 0)
             {
                 Button3.Enabled = false;
                 grid_linedetails.Enabled = false;
             }
        }

    }
    protected void click_popup(object sender, EventArgs e)
    {
        DataTable dtotapp = dbhelper.getdata("select a.PayrollGroupId,left( convert(varchar,a.csDate,101),10) csDate,a.Remarks, b.Period from TChangeShift a " +
                                                   "left join MPeriod b on a.PeriodId=b.Id where a.id=" + lbl_leaveid.Value + "");
        txt_viewremarks.Text = dtotapp.Rows[0]["Remarks"].ToString();
       // ddl_viewpg.SelectedValue = dtotapp.Rows[0]["PayrollGroupId"].ToString();
        employee(Session["emp_id"].ToString());


        DataTable dt = dbhelper.getdata("select a.id, a.date,a.remarks,c.ShiftCode,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from TChangeShiftLine a " +
                                            "left join MEmployee b on a.EmployeeId=b.Id  " +
                                            "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                                            "where a.ChangeShiftId=" + lbl_leaveid.Value + " and a.status like '%Approved%'");

        grid_linedetails.DataSource = dt;
        grid_linedetails.DataBind();
        ppop(true);

    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("Mchangeshiftlist?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void click_can_perline(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update TChangeShiftLine set status='cancel-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + row.Cells[0].Text + "");
                click_popup(null, null);
            }
            else
            { }
        }

    }
    protected void click_search(object sender, EventArgs e)
    {
         string query=("select a.id, LEFT(CONVERT(varchar,a.csdate,101),10)csdate,b.payrollgroup,a.remarks from TChangeShift a " +
                                      "left join MPayrollGroup b on a.payrollgroupid=b.id  where a.status like '%Approved%' and a.EntryUserId="+Session["user_id"]+" and a.dtr_id is null");
         if (txt_from.Text.Length > 0 && txt_to.Text.Length > 0)
         {
             query += " and LEFT(CONVERT(varchar,a.csdate,101),10)>='" + txt_from.Text + "' and LEFT(CONVERT(varchar,a.csdate,101),10) <='"+txt_to.Text+"'";
         }
         if (txt_search.Text.Length > 0)
         {
             query += "and a.remarks LIKE '%"+txt_search.Text+"%'";
         }
         query += "order by a.id desc;";

         DataTable dt = dbhelper.getdata(query);

        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void employee(string emp_id)
    {
        string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                     "left join MPosition b on a.PositionId=b.Id where a.reportto=" + emp_id + " and a.payrollgroupid<>'9'";
        DataTable dt = dbhelper.getdata(query);
        ddl_employee.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
    }
    protected bool chk()
    {
        bool err = true;
        if (ddl_employee.SelectedValue.Trim().Length == 0)
        {
            lbl_emp.Text = "*";
            err = false;
        }
        else
            lbl_emp.Text = "";

        if (txt_csd.Text.Trim().Length == 0)
        {
            lbl_date.Text = "*";
            err = false;
        }
        else
            lbl_date.Text = "";


        if (txt_lineremarks.Text.Trim().Length == 0)
        {
            lbl_remarks.Text = "*";
            err = false;
        }
        else
            lbl_remarks.Text = "";
        return err;
    }
    protected void click_addperline(object sender, EventArgs e)
    {
        if (chk())
        {
            dbhelper.getdata("insert into TChangeShiftLine values (" + lbl_leaveid.Value + "," + ddl_employee.SelectedValue + ",'" + txt_csd.Text + "'," + ddl_shiftcode.SelectedValue + ",'" + txt_lineremarks.Text + "','Approved',NULL)");
            DataTable dt = dbhelper.getdata("select a.id, a.date,a.remarks,c.ShiftCode,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from TChangeShiftLine a " +
                                           "left join MEmployee b on a.EmployeeId=b.Id  " +
                                           "left join MShiftCode c on a.ShiftCodeId=c.Id " +
                                           "where a.ChangeShiftId=" + lbl_leaveid.Value + " and a.status like '%Approved%'");

            grid_linedetails.DataSource = dt;
            grid_linedetails.DataBind();
            //Response.Redirect("Mchangeshiftlist?user_id=" + function.Encrypt(key.Value, true) + "");
            //click_popup(null, null);
        }
    }
    protected void datarow(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = dbhelper.getdata("select * from TChangeShift where id="+e.Row.Cells[0].Text+" and dtr_id is not null");
            LinkButton lnk_edit = (LinkButton)e.Row.FindControl("lnk_edit");
            LinkButton lnk_can = (LinkButton)e.Row.FindControl("lnk_can");
            if (dt.Rows.Count > 0)
            {
                lnk_edit.Visible = false;
                lnk_can.Visible = false;
            }
        }
    }
    protected void cancel_changeshift(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update TChangeShift set status='cancel-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + row.Cells[0].Text + "");
                dbhelper.getdata("update TChangeShiftLine set status='cancel-" + key.Value + "-" + DateTime.Now.ToShortDateString().ToString() + "' where changeshiftid=" + row.Cells[0].Text + "");
                Response.Redirect("Mchangeshiftlist?user_id=" + function.Encrypt(key.Value, true) + "", false);
            }
            else
            { }
        }
    }
    protected void click_edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id_to_edit.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from TChangeShift where id=" + row.Cells[0].Text + " ");
            txt_remarkstoedit.Text=dt.Rows.Count>0?dt.Rows[0]["Remarks"].ToString():"";
            panelOverlay.Visible = true;
                Div1.Visible = true;
        }
    }
    protected void click_save_change(object sender, EventArgs e)
    {
        dbhelper.getdata("update TChangeShift set Remarks='" + txt_remarkstoedit.Text + "' where id=" + id_to_edit.Value + " ");
        Response.Redirect("Mchangeshiftlist?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
}