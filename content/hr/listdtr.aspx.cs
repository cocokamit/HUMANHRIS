using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_payroll_listdtr : System.Web.UI.Page
{
    void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["role"].ToString() != "Admin")
            this.MasterPageFile = "~/content/site.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
            disp();
        }
    }

    protected void lodable()
    {
        string query = "select * from MPayrollGroup where status = '1' order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }

    protected void click_add_dtr(object sender, EventArgs e)
    {
         Response.Redirect("addDTR", false);
    }

    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id, left(convert(varchar,a.DTRDate,101),10)DTRDate,a.DTRNumber,left(convert(varchar,a.DateStart,101),10)DateStart,left(convert(varchar,a.DateEnd,101),10)DateEnd,a.Remarks,b.PayrollGroup,b.id pg from TDTR a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.status is null ORDER BY a.id desc ");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void search(object sender, EventArgs e)
    {
        String query = "select a.id, left(convert(varchar,a.DTRDate,101),10)DTRDate,a.DTRNumber,left(convert(varchar,a.DateStart,101),10)DateStart,left(convert(varchar,a.DateEnd,101),10)DateEnd,a.Remarks,b.PayrollGroup,b.id pg from TDTR a left join MPayrollGroup b on a.PayrollGroupId=b.Id " +
        "where a.status is null ";

        if (int.Parse(ddl_pg.SelectedValue) > 0)
            query += " and a.payrollgroupid=" + ddl_pg.SelectedValue + " ";
        if (txt_f.Text.Length > 0 && txt_t.Text.Length > 0)
            query += " and left(convert(varchar,a.DTRDate,101),10)>='" + txt_f.Text + "' and left(convert(varchar,a.DTRDate,101),10) <='" + txt_t.Text + "'";
        query += " order by a.id desc ";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='dtrdetails?&dtrid=" + function.Encrypt(row.Cells[0].Text, true) + "'", true);
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                DataTable dt=dbhelper.getdata("select * from tdtr where payroll_id is null and id="+row.Cells[0].Text+"");

                if (dt.Rows.Count>0)
                {
                    string query = "select b.PayrollGroup,convert(varchar,CONVERT(date,a.DateStart))+' - '+convert(varchar,CONVERT(date,a.DateEnd))payrange from tdtr a left join MPayrollGroup b on a.PayrollGroupId = b.Id where a.Id = " + row.Cells[0].Text + "";
                    DataTable dtt = dbhelper.getdata(query);
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("audittrail_master_payroll", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "deleteadjustedrecord";
                            cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "" + dtt.Rows[0]["PayrollGroup"].ToString() + "";
                            cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = "Delete Adjusted Record";
                            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "" + dtt.Rows[0]["payrange"].ToString() + "";
                            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                  //dbhelper.getdata("update TPayroll set  status='" + "Cancelled-" + DateTime.Now.ToShortDateString() + "' where id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update tdtr set status='" + "Cancelled-" + DateTime.Now.ToShortDateString() + "' where id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update TChangeShift set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "'");
                    dbhelper.getdata("update TLeaveApplicationLine set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "'");
                    dbhelper.getdata("update TOverTimeLine set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "' ");
                    dbhelper.getdata("update TChangeShiftLine set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "' ");
                    dbhelper.getdata("update Tmanuallogline set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "' ");
                    dbhelper.getdata("update TRestdaylogs set dtr_id=NULL where dtr_id='" + row.Cells[0].Text + "' ");
                    dbhelper.getdata("update tdtrperpayrol set dtr_id=NULL where dtr_id=" + row.Cells[0].Text + "");
                    Response.Redirect("Mdtrlist", false);
                }
                else
                {
                    Response.Write("<script>alert('Cancellation Denied!')</script>");
                }
            }
            else
            {
            }
        }
    }

}