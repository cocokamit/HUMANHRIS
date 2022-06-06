using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_verifyTravel : System.Web.UI.Page
{
    public static string query, id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname " +
                "from Ttravel a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "where a.status like '%Verification%' order by a.id desc ";
        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void search(object sender, EventArgs e)
    {


        query = "select a.id,a.date_input,a.purpose,a.travel_start,a.travel_end, " +
                "b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname " +
                "from Ttravel a " +
                "left join MEmployee b on a.emp_id=b.Id " +
                "where a.status like '%Verification%'  " +
                "and LEFT(CONVERT(varchar,a.travel_start,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                "order by a.id desc ";
               

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();

        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
            function.ReadNotification(row.Cells[0].Text, 0);

            idd.Value = row.Cells[0].Text;
            l_name.Text = row.Cells[2].Text;

                query = "select a.id,a.expected_budget,a.actual_budget,a.travel_description,a.notes, " +
                   "b.place,b.travel_mode,b.arrange_type " +
                   "from Ttravel a " +
                   "left join Ttravelline b on a.id=b.travel_id " +
                   "where a.id=" + idd.Value + "";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);
          
            txt_reason.Text = dt.Rows[0]["travel_description"].ToString();
            lbl_ex.Text = dt.Rows[0]["expected_budget"].ToString();
            lbl_act.Text = dt.Rows[0]["actual_budget"].ToString();

            query = "select * from Ttravelline where travel_id=" + idd.Value + " ";
            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_destinations.DataSource = ds;
            grid_destinations.DataBind();

            query = "select * from Ttravel_budget where travel_id=" + idd.Value + " ";
            DataTable dtt = new DataTable();
            dtt = dbhelper.getdata(query);

            if (dtt.Rows.Count == 0)
            {
                txt_meals.Text = "0.00";
                txt_acom.Text = "0.00";
                txt_trans.Text = "0.00";
                txt_other.Text = "0.00";
                txt_cash.Text = "0.00";
            }
            else
            {
                txt_meals.Text = dtt.Rows[0]["meals"].ToString();
                txt_acom.Text = dtt.Rows[0]["accommodation"].ToString();
                txt_trans.Text = dtt.Rows[0]["transportation"].ToString();
                txt_other.Text = dtt.Rows[0]["otherexpense"].ToString();
                txt_cash.Text = dtt.Rows[0]["totalcashapproved"].ToString();
                btn_save.Text = "Update";
                lnk_button.Visible = true;


                
               

            }

            //idd.Value = grid_view.Rows[0].Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;

            Div3.Visible = false;
            Div4.Visible = false;
        }
    }

    protected void GridView_RowDataBound(Object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnk_approve = (LinkButton)e.Row.FindControl("lnk_approve");

            idd.Value = e.Row.Cells[0].Text;
            query = "select * from Ttravel_budget where travel_id=" + idd.Value + " ";
            DataTable dtt = new DataTable();
            dtt = dbhelper.getdata(query);

            if (dtt.Rows.Count == 0)
                lnk_approve.Visible = false;
            else
                lnk_approve.Visible = true;
        }

    }

    protected void view1(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification
          
            idd.Value = row.Cells[0].Text;
            Div1.Visible = false;
            Div2.Visible = false;

            Div3.Visible = true;
            Div4.Visible = true;
        }
    }


    protected void approve(object sender, EventArgs e)
    {

        
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                //Read Notification
                function.ReadNotification(row.Cells[0].Text, 0);
                if (TextBox1.Text == "Yes")
                {
                //if (row.Cells[6].Text == "0")
                //{
                    dbhelper.getdata("update Ttravel set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + "");
                    /**BTK 111119
                      * OBT**/
                    dbhelper.getdata("update OBTLine set status='Approved' where OBTId=" + row.Cells[0].Text);
                    
                    //}
                //else
                //{
                //    dbhelper.getdata("update Ttravel set approver_id='" + row.Cells[6].Text + "' where Id=" + row.Cells[0].Text + "");
                //}
                stateclass a = new stateclass();

                a.sa = row.Cells[0].Text;
                a.sb = Session["emp_id"].ToString();
                a.sc = "OBT";
                a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
                a.se = "ok";
                bol.system_logs(a);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='Travel'", true);
            }
        }
    }

    protected void delete3(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, 0);
        a.sa = idd.Value;
        a.sb = Session["emp_id"].ToString();
        a.sc = "OBT";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason1.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update Ttravel set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + idd.Value + "");
        /**BTK 111119
                  * OBT**/
        dbhelper.getdata("update OBTLine set status='Cancel' where OBTId=" + idd.Value);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='Travel'", true);
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("Travel?user_id=" + function.Encrypt(key.Value, true) + "", false);
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(idd.Value, 0);
        if (btn_save.Text == "Update")         
        {
            query = "update Ttravel_budget set meals='" + txt_meals.Text + "',transportation='" + txt_trans.Text + "',accommodation='" + txt_acom.Text + "',otherexpense='" + txt_other.Text + "',totalcashapproved='" + txt_cash.Text + "' where travel_id=" + idd.Value + "  ";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Travel'", true);
        }
        else
        {
          
            a.sa = idd.Value;
            a.sb = txt_meals.Text;
            a.sc = txt_trans.Text;
            a.sd = txt_acom.Text;
            a.se = txt_other.Text;
            a.sf = txt_cash.Text;
            bol.travel_budget(a);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='Travel';window.open('travel_form?&key=" + function.Encrypt(idd.Value, true) + "','_new')", true);
        }
    }
    protected void lnk_button_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.location='Travel';window.open('travel_form?&key=" + function.Encrypt(idd.Value, true) + "','_new')", true);
    }
}