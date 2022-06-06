using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Human;

public partial class content_hr_manuallog : System.Web.UI.Page
{

 //   public static string user_id, query, id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
           // lodable();
           // key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();

        }
    }
    protected void lodable()
    {
        //string query = "select * from MPayrollGroup order by id desc";
        //DataTable dt = dbhelper.getdata(query);

        //ddl_pg.Items.Clear();
        //ddl_pg.Items.Add(new ListItem("None", "0"));
        //foreach (DataRow dr in dt.Rows)
        //{
        //    ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        //}
    }

    protected void search(object sender, EventArgs e)
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.EmployeeId,a.id,left(convert(varchar,a.sysdate,101),10)sysdate,a.note,b.idnumber, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date_log,convert(datetime,a.time_in)time_in,convert(datetime,a.time_out)time_out,a.time_in1,a.time_out1,c.manual_type as reason " +
                        "from tmanuallogline a " +
                        "left join Memployee b on a.employeeid=b.id " +
                        "left join time_adjustment c on a.reason=c.id " +
                        "where a.status like '%verification%' " +
                         "and LEFT(CONVERT(varchar,a.date,101),10) between '" + txt_from.Text + "' and '" + txt_to.Text + "' " +
                        "order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
    }
    protected void disp()
    {
        if (Request.QueryString["nt"] != null)
        function.ReadNotification(Request.QueryString["nt"].ToString());
        string query = "select a.EmployeeId,a.id,left(convert(varchar,a.sysdate,101),10)sysdate,a.note,b.idnumber, b.lastname+', '+b.firstname+' '+middlename e_name, left(convert(varchar,a.date,101),10)date_log,convert(datetime,a.time_in)time_in,convert(datetime,a.time_out)time_out,a.time_in1,a.time_out1,c.manual_type as reason " +
                        "from tmanuallogline a " +
                        "left join Memployee b on a.employeeid=b.id " +
                        "left join time_adjustment c on a.reason=c.id " +
                        "where a.status like '%verification%' and dtr_id is null order by a.id desc";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();

        alert.Visible = dt.Rows.Count == 0 ? true : false;
            
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //Read Notification            

            adjid.Value = row.Cells[0].Text;
            lbl_name.Text = row.Cells[4].Text;
            lbl_date.Text =  row.Cells[5].Text;
            lbl_in.Text = row.Cells[6].Text;
            lbl_in1.Text = row.Cells[8].Text;
            lbl_out1.Text = row.Cells[7].Text;
            lbl_out.Text = row.Cells[9].Text;
            lbl_reason.Text = row.Cells[10].Text;
            lbl_note.Text = row.Cells[11].Text;
            empid.Value = row.Cells[1].Text;

            //string query = "select * from tdtrperpayrolperline where LEFT(CONVERT(varchar,BioTime,101),10)='" + row.Cells[5].Text + "' and empid='" + row.Cells[1].Text + "'";
            string query = "select Biotime from tdtrperpayrolperline a " +
                            "left join tdtrperpayrol b on a.dtrperpayrol_id=b.id " +
                            "where LEFT(CONVERT(varchar,a.BioTime,101),10)='" + lbl_date.Text + "' and a.empid='" + empid.Value + "' ";//and b.status ='Approved' " +
                            //"union " +
                            //"select Biotime from tdtrperpayrolperline a where  a.dtrperpayrol_id=0  and empid='" + empid.Value + "'";
            DataTable dt = dbhelper.getdata(query);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Div1.Visible = true;
            Div2.Visible = true;
            if (GridView1.Rows.Count > 0)
            {
                lbl_err.Text = "";
            }
            else
            {
                lbl_err.Text = "No DTR FILE";
            }
        }
    }

    protected void Cancel(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        function.ReadNotification(adjid.Value, 0);
        a.sa = adjid.Value;
        a.sb = empid.Value;
        a.sc = "ML";
        a.sd = "Cancelled-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);

        dbhelper.getdata("update Tmanuallogline set status='" + "Cancel-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + adjid.Value + "");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='Manuallogs'", true);
    }

    protected void Approve(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        //function.ReadNotification(adjid.Value, 0);
        a.sa = adjid.Value;
        a.sb = empid.Value;
        a.sc = "ML";
        a.sd = "Approved-" + DateTime.Now.ToShortDateString().ToString();
        a.se = txt_reason.Text.Replace("'", "");
        bol.system_logs(a);
        dbhelper.getdata("update Tmanuallogline set status='" + "Approved-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + adjid.Value + "");


        string query = " select ((hourlyrate * nightmultiplier)*nighthrs) + ((hourlyrate * regmultiplier)*hrs) amt_to_be_paid,employeeid from " +
                        "( " +
                        "select  " +
                        "case when regmultiplier IS null then 0 else regmultiplier end regmultiplier, " +
                        "case when nightmultiplier IS null then 0 else nightmultiplier end nightmultiplier, " +
                        "case when hourlyrate IS null then 0 else hourlyrate end hourlyrate,  " +
                        "case when hrs IS null then 0 else hrs end hrs, " +
                        "case when nighthrs IS null then 0 else nighthrs end nighthrs,employeeid " +
                        "from tmanuallogline where ID=" + adjid.Value + " " +
                        ")t";
        
        DataTable dtgetamt = dbhelper.getdata(query);
        DataTable dtemp = dbhelper.getdata(adjustdtrformat.allemployee() + " where id=" + empid.Value + "");
        DataTable dtchkpayroll = dbhelper.getdata(checking.chekonepayrollaway(lbl_date.Text, dtemp.Rows[0]["PayrollGroupId"].ToString()));
        if (dtchkpayroll.Rows.Count == 1)
        {
            DataTable dtr = Core.DTRF(lbl_date.Text, lbl_date.Text, "KIOSK_" + empid.Value);
            decimal amttt = dtr.Rows.Count > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) > 0 ? decimal.Parse(dtr.Rows[0]["netamt"].ToString()) : 0 : 0;
            if (amttt > 0)
            {
                DataTable chkifnaa = dbhelper.getdata(checking.chekifnetnaaamount(lbl_date.Text, empid.Value));
                if (decimal.Parse(chkifnaa.Rows[0]["netamount"].ToString())  == 0)
                {
                    DataTable dtinsertpayrolladjustment = dbhelper.getdata(adjustdtrformat.payrolladjustment(lbl_date.Text,adjid.Value, empid.Value, "TimeAdjustment", "0", "0", "0", "0", "0", amttt.ToString(), "0"));
                }
            }
        }
        function.Notification(adjid.Value, "ml", "0");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Approved Successfully'); window.location='Manuallogs'", true);
    }
    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("Manuallogs", false);
    }
    protected void click_add_adj(object sender, EventArgs e)
    {
      //  KOISK_addMANUAL? user_id = AzHhEjH7Q + U =
        Response.Redirect("KOISK_addMANUAL", false);
    }
}