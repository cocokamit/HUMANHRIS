using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Manager_approve_leave : System.Web.UI.Page
{
    //public static string query,user_id,id;
    //public static DataTable dt;
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
        string query = "select distinct(a.id),a.delegate,left(convert(varchar,a.sysdate,101),10) sysdate,b.status, " +
                "b.remarks,(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date asc) as leavefrom, " +
                "(select top 1 left(convert(varchar,Date,101),10) from TLeaveApplicationLine where l_id=a.id order by Date desc) as leaveto, " +
                "c.LastName +', ' + c.FirstName + ' ' + c.MiddleName as fullname, " +
                "d.Leave, " +
                "e.Department, " +

                "f.under_id,f.herarchy, " +
                "case when (select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) is null then 0 else (select under_id from approver where emp_id=f.emp_id and status is null and herarchy =  f.herarchy + 1) end as nxt_id " +
  
                "from Tleave a " +
                "left join TLeaveApplicationLine b on a.id=b.l_id " +
                "left join MEmployee c on b.EmployeeId=c.id " +
                "left join MLeave d on b.LeaveId=d.Id " +
                "left join MDepartment e on c.DepartmentId=e.id " +
                "left join approver f on b.EmployeeId=f.emp_id " +

                "where b.status like '%for approval%' and a.approver_id=" + Session["emp_id"] + " and f.under_id=" + Session["emp_id"] + " " +
                "order by a.id desc ";
    

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
             if (TextBox1.Text == "Yes")
                {
                    idd.Value = grid_view.Rows[0].Cells[0].Text;
                    Div1.Visible = true;
                    Div2.Visible = true;
                    Div3.Visible = false;
                    Div4.Visible = false;
                }
                else{}
        }
    }
    protected void approve(object sender, EventArgs e)
    {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                if (TextBox1.Text == "Yes")
                {
                    function.ReadNotification(row.Cells[0].Text, int.Parse(Session["user_id"].ToString()));

                    DataTable dtapproveruserid = dbhelper.getdata("select id from nobel_user where emp_id=" + row.Cells[8].Text + "");

                    if (row.Cells[8].Text == "0")
                    {
                        dbhelper.getdata("update TLeaveApplicationLine set status='verification' where l_Id=" + row.Cells[0].Text + "");
                        function.AddNotification("Leave Verification", "vl", row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                    }
                    else
                    {
                        dbhelper.getdata("update Tleave set approver_id='" + row.Cells[8].Text + "' where id=" + row.Cells[0].Text + "");
                        function.AddNotification("Leave Approval", "al", row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);
                    }

               


                    //string approveruserid=row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString();
                    //string param = row.Cells[8].Text == "0" ? "Leave Verification,vl," + approveruserid + "," + row.Cells[0].Text + "" : "Leave Approval, al," + approveruserid + "," + row.Cells[0].Text + "";
                    //function.AddNotification(param.ToString());
                    //function.AddNotification("Leave Approval", "al", row.Cells[8].Text == "0" ? "0" : dtapproveruserid.Rows[0]["id"].ToString(), row.Cells[0].Text);



                    stateclass a = new stateclass();
                    a.sa = row.Cells[0].Text;
                    a.sb = Session["emp_id"].ToString();
                    a.sc = "L";
                    a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                    a.se = "ok";
                    bol.system_logs(a);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='al'", true);
                }
                else
                { }
            }
        
    }
    protected void delete3(object sender, EventArgs e)
    {
            function.ReadNotification(idd.Value, int.Parse(Session["user_id"].ToString()));
            stateclass a = new stateclass();
            a.sa = idd.Value;
            a.sb = Session["emp_id"].ToString();
            a.sc = "L";
            a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
            a.se = txt_reason.Text.Replace("'", "");
            bol.system_logs(a);
            dbhelper.getdata("update TLeaveApplicationLine set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where l_id=" + idd.Value + "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='al'", true);
    }
    protected void view_img(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Response.Write("<script>alert('a')</script>");

            idd.Value = row.Cells[0].Text;

            string query = "select id,CONVERT(varchar, leave_id)+'\\'+CONVERT(varchar, file_img) as awts from leave_image where leave_id=" + idd.Value + " ";
            DataSet ds = bol.display(query);
            grid_img.DataSource = ds.Tables["table"];
            grid_img.DataBind();

            if (grid_img.Rows.Count == 0)
                lbl_msg.Text = "No Attachment File";
            else
                lbl_msg.Text = "";



            Div1.Visible = false;
            Div2.Visible = false;

            Div3.Visible = true;
            Div4.Visible = true;

        }
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("al", false);
    }
    
}