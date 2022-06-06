using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_underTime : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
              //  getdata();
        }
    }
    protected void getdata()
    {
       string query = "select * from Tundertime where emp_id='" + Session["emp_id"].ToString() + "' order by Id desc";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

    }
    protected void addunder(object sender, EventArgs e)
    {
        if (checker())
        {

           // DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " and a.under_id<>" + Session["emp_id"].ToString() + " and a.herarchy<>0 ");
            
            
            string tout = (txt_date11.Text + " " + txt_time.Text);
            stateclass a = new stateclass();
            a.sa = Session["emp_id"].ToString();
            a.sb = txt_date.Text;
            a.sc = tout;
            a.sd = txt_reason.Text.Replace("'", "");
            a.se = approver_id.Rows.Count == 0 ? "verification" : "For Approval";
            a.sf = approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_id"].ToString();
            a.sg = rbl_class.SelectedItem.Text;
           string x= bol.undertime(a);
           if (x == "1")
           {
               DataTable dt = dbhelper.getdata("select top 1(id) from Tundertime order by ID desc");
               function.Notification("ut", "for approval", dt.Rows[0]["id"].ToString(), approver_id.Rows[0]["under_id"].ToString());
               function.AddNotification("Undertime Approval", "aut", approver_id.Rows.Count == 0 ? "0" : approver_id.Rows[0]["under_userid"].ToString(), dt.Rows[0]["id"].ToString());
               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KIOSK_undertime'", true);
           }
           else
               lbl_err.Text = "Exist already!";
        }
    }
    protected void cancel(object sender, EventArgs e)
    {
        string query = "update Tundertime set status='" + "deleted-" + DateTime.Now.ToShortDateString().ToString() + "',reason='" + txt_re.Text.Replace("'", "") + "' where id=" + id.Value + " ";
        dbhelper.getdata(query);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='ut'", true);
    }
    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
        }
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("KIOSK_undertime", false);
    }

    protected bool checker()
    {
        bool oi = true;

        if (txt_date.Text.Length < 10)
        {
            oi = false;
            lbl_err.Text = "Invalid Date";
        }
        else
            lbl_err.Text = "";

        if (txt_time.Text.Length == 0)
        {
            oi = false;
            lbl_err.Text = "Invalid Input Time!";
        }
        else
            lbl_err.Text = "";

        if (txt_reason.Text.Length == 0)
        {
            oi = false;
            lbl_err.Text = "Invalid Input Reason!";
        }
        else
            lbl_err.Text = "";

        if (rbl_class.Text.Length == 0)
        {
            oi = false;
            lbl_err.Text = "Invalid Input Personal / Authorized Undertime!";
        }
        else
            lbl_err.Text = "";



        return oi;

    }
}