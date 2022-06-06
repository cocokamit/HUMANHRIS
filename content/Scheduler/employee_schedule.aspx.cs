using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Scheduler_employee_schedule : System.Web.UI.Page
{
    public static string user_id, query;
    protected void Page_Load(object sender, EventArgs e)
    {
       // user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            getdata();
    }

    protected void getdata()
    {
        //query = "select a.id,a.status,a.shiftcode_id, " +
        ////"c.ShiftCode As shiftcodeFrom,c.ShiftCode as aa, " +

        //"case when LEFT(CONVERT(varchar,a.date_change,101),10) is null then LEFT(CONVERT(varchar,b.date,101),10) else LEFT(CONVERT(varchar,a.date_change,101),10) end as date, " +
        //"case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom,case when e.ShiftCode is null then h.ShiftCode else e.ShiftCode end as aa, " +
        //"d.ShiftCode As shiftcodeTo,d.ShiftCode as bb  " +
        //"from temp_shiftcode a " +
        //"left join TChangeShiftLine b on a.changeshift_id=b.Id " +
        //"left join MShiftCode c on a.changeshift_id=c.Id " +
        //"left join MShiftCode d on  a.shiftcode_id=d.Id " +
        //"left join MShiftCode e on b.ShiftCodeId=e.Id " +
        //"left join MShiftCode h on a.changeshift_id=h.Id " +
        //"where a.emp_id='" + Session["emp_id"].ToString() + "' and a.status not like '%delete%' order by a.Id desc";

        query = "select a.id,a.status,a.shiftcode_id, " +
                "case when LEFT(CONVERT(varchar,a.date_change,101),10) is null " +
                "then LEFT(CONVERT(varchar,b.date,101),10) else LEFT(CONVERT(varchar,a.date_change,101),10) end as date, " +

                "case when a.old_shiftcodeid is null then h.ShiftCode else e.ShiftCode end As shiftcodeFrom, " +
                "case when a.old_shiftcodeid is null then h.ShiftCode else e.ShiftCode end as aa, " +

                "d.ShiftCode As shiftcodeTo,d.ShiftCode as bb " +

                "from temp_shiftcode a " +
                "left join TChangeShiftLine b on a.changeshift_id=b.Id " +
                "left join MShiftCode d on  a.shiftcode_id=d.Id " +
                "left join MShiftCode e on a.old_shiftcodeid=e.Id " +
                "left join MShiftCode h on b.ShiftCodeId=h.Id " +
                "where a.emp_id='" + Session["emp_id"].ToString() + "' and a.status not like '%delete%' order by a.Id desc";

        DataSet ds = new DataSet();
        ds = bol.display(query);
        grid_view.DataSource = ds;
        grid_view.DataBind();
        alert.Visible = grid_view.Rows.Count == 0 ? true : false;
    }

    protected void loadable()
    {

        //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
        DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where status is null");

        foreach (DataRow dr in sc.Rows)
        {
            ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
            item.Attributes.Add("title", dr["remarks"].ToString());

            //li.Attributes["title"] = li.Text;
            ddl_shiftcode.Items.Add(item);

        }
    }

    protected void addOT(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_addRestday?user_id=" + function.Encrypt(user_id, true));
    }

    protected void cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            query = "update temp_shiftcode set status='" + "deleted-" + user_id + "-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + " ";
            dbhelper.getdata(query);
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='esl'", true);
    }

    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnkcan");
            LinkButton lnkedit = (LinkButton)e.Row.FindControl("lnk_edit");
            string[] stat = e.Row.Cells[4].Text.Split('-');

            if (stat[0] == "Approved")
            {
                lnkcan.Enabled = false;
                lnkedit.Enabled = false;
            }

            if (stat[0].Contains("Cancel"))
            {
                //lnkcan.Enabled = false;
                lnkedit.Enabled = false;
            }

        }
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //  id = row.Cells[0].Text;
            query = "select a.status,a.remarks,a.date, " +
            "case when a.emp_id =0 " +
            "then 'HR Admin' " +
            "else b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname end Fullname " +
            "from sys_applog a " +
            "left join MEmployee  b on a.emp_id=b.Id " +
            "where type='CS' and app_id=" + row.Cells[0].Text + " order by a.id ";


            DataSet ds = new DataSet();
            ds = bol.display(query);
            grid_approver.DataSource = ds;
            grid_approver.DataBind();

            Div1.Visible = false;
            Div1.Visible = false;
            Div3.Visible = true;
            Div4.Visible = true;
        }
    }


    protected void view1(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            loadable();
            Label aw = (Label)row.Cells[3].FindControl("Label2");

            key.Value = row.Cells[0].Text;
            txt_date.Text = row.Cells[1].Text;
            ddl_shiftcode.SelectedValue = row.Cells[5].Text;

            Div1.Visible = true;
            Div2.Visible = true;

            Div3.Visible = false;
            Div4.Visible = false;
        }
    }

    protected void click_save_changeshift(object sender, EventArgs e)
    {
        dbhelper.getdata("update temp_shiftcode set shiftcode_id='" + ddl_shiftcode.SelectedValue + "',remarks='" + txt_remarks.Text + "' where id=" + key.Value + " ");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Shift Updated Sucessfully'); window.location='esl'", true);
    }

    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("esl", false);
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        getdata();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}