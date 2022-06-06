using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verify_undertime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);


        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();

    }
    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.id,b.id as emp_id,LEFT(CONVERT(varchar,a.date_filed,101),10)date_filed, " +
                "a.time,a.reason, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
                ",case when a.dtunder IS null then LEFT(CONVERT(varchar,a.date_filed,101),10)+' '+a.time else a.dtunder  end  timeout " +
                "from Tundertime a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "where a.status like '%Verification%' order by a.date_filed desc ";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;


    }

    protected void search(object sender, EventArgs e)
    {
       

        string query = "select a.id,b.id as emp_id,LEFT(CONVERT(varchar,a.date_filed,101),10)date_filed, " +
               "a.time,a.reason, " +
               "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname,b.Id as emp_id,b.BranchId,b.DepartmentId " +
               ",case when a.dtunder IS null then LEFT(CONVERT(varchar,a.date_filed,101),10)+' '+a.time else a.dtunder  end  timeout " +
               "from Tundertime a " +
               "left join MEmployee b on a.emp_id=b.Id " +
               "where a.status like '%Verification%' " +
               "and LEFT(CONVERT(varchar,a.date_filed,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
               "order by a.date_filed desc ";
     
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)grid_view.HeaderRow.FindControl("chkboxSelectAll");
        int i = 0;
        foreach (GridViewRow row in grid_view.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkEmp");

            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
                i++;
            }
            else
            {

                ChkBoxRows.Checked = false;
                if (i > 0)
                {
                    i--;
                }
            }

        }
        lbl_del_notify.Text = i + " rows".ToString();
    }

    protected void delete_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");
            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = "0";
                a.sc = "UT";
                a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "";
                bol.system_logs(a);
                query += "update Tundertime set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where id=" + grid_view.Rows[i].Cells[0].Text + " ";
            }
        }

        if (query.Replace(" ", "").Length > 0)
        {
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully'); window.location='vu'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select a row'); window.location='vu'", true);

    }

    protected void approve_all(object sender, EventArgs e)
    {
        string query = "";
        for (int i = 0; i <= grid_view.Rows.Count - 1; i++)
        {
            function.ReadNotification(grid_view.Rows[i].Cells[0].Text, 0);
            CheckBox chkEmp = (CheckBox)grid_view.Rows[i].FindControl("chkEmp");

            if (chkEmp.Checked == true)
            {
                stateclass a = new stateclass();
                a.sa = grid_view.Rows[i].Cells[0].Text;
                a.sb = "0";
                a.sc = "UT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                dbhelper.getdata("update Tundertime set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + grid_view.Rows[i].Cells[0].Text + "");
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vu'", true);
            }

        }
    }

    protected void delete2(object sender, EventArgs e)
    {
       
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                //Read Notification
                function.ReadNotification(row.Cells[0].Text, 0);
                if (TextBox1.Text == "Yes")
                {
                    dbhelper.getdata("update Tundertime set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");
                    stateclass a = new stateclass();
                    a.sa = row.Cells[0].Text;
                    a.sb = Session["emp_id"].ToString();
                    a.sc = "UT";
                    a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                    a.se = "ok";
                    bol.system_logs(a);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='vu'", true);
                }
        }

    }
    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "UT";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update Tundertime set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + idd.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='vu'", true);
    }
    protected void view(object sender, EventArgs e)
    {
        idd.Value = grid_view.Rows[0].Cells[0].Text;
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("vu", false);
    }
}