using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_ot_list : System.Web.UI.Page
{
    

    //public static DataTable dt;
    //public static string user_id, query, id;
    protected void Page_Load(object sender, EventArgs e)
    {
      //  user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            Bind();
            disp();
        }
    }

    protected void disp()
    {
        ViewState["role"] = Session["role"].ToString() == "4" ? "HOD" : "Heartist";
        if (ViewState["role"].ToString() == "HOD")
        {
            btnotadd.Visible = false;
        }
        else
        {
            btnotadd.Visible = true;
        }
    }
    protected void click_add(object sender, EventArgs e)
    {
        Div1.Visible = true;
        pAdd.Visible = true;
        Div2.Visible = false;
    }

    protected void Bind()
    {
        if (Request.QueryString["nt"] != null)
            function.ReadNotification(Request.QueryString["nt"].ToString());

       DataTable dt= dbhelper.getdata("select * from TOverTimeLine where EmployeeId='" + Session["emp_id"].ToString() + "' and status not like '%deleted%' order by Id desc");
       alert.Visible = dt.Rows.Count == 0 ? true : false;
       grid_view.DataSource = dt;
       grid_view.DataBind();
    }

    protected void addOT(object sender, EventArgs e)
    {
        if (checker())
        {
            string a = DateTime.Now.ToString("MM/dd/yyyy");
           string query = "select top 1 *,CONVERT(varchar,[Date],101) as datee from TChangeShiftLine where EmployeeId='" + Session["emp_id"].ToString() + "' order by id desc ";
            DataTable wtf = new DataTable();
            wtf = dbhelper.getdata(query);


            if (wtf.Rows.Count <= 0)
            {
                query = "select a.ShiftCodeId, " +
                        "b.[Day],b.TimeOut2 " +
                        "from MEmployee a " +
                        "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                        "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
            }
            else
            {
                if (DateTime.Parse(wtf.Rows[0]["datee"].ToString()) < DateTime.Parse(txt_date.Text))
                {
                    query = "select top 1 a.ShiftCodeId, " +
                            "b.[Day],b.TimeOut2 " +
                            "from TChangeShiftLine a " +
                            "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                            "left join MEmployee c on a.EmployeeId=c.Id " +
                            "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                    //  if (DateTime.Parse(wtf.Rows[0]["datee"].ToString()) < DateTime.Parse(txt_date.Text))
                    query += " order by a.Date desc ";
                    //else
                    // query += " order by Date asc";
                }
                else
                {
                    if (wtf.Rows.Count < 0)
                    {

                        query = "select a.ShiftCodeId, " +
                           "b.[Day],b.TimeOut2 " +
                           "from MEmployee a " +
                           "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                           "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.Id='" + Session["emp_id"].ToString() + "' ";
                    }
                    else
                    {
                        query = "select top 1 a.ShiftCodeId, " +
                          "b.[Day],b.TimeOut2 " +
                          "from TChangeShiftLine a " +
                          "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
                          "left join MEmployee c on a.EmployeeId=c.Id " +
                          "where  b.[Day] in (select DATENAME(WEEKDAY,'" + txt_date.Text + "')) and a.EmployeeId='" + Session["emp_id"].ToString() + "' ";
                        query += " order by Date asc";
                    }
                }


            }
            DataTable dt = dbhelper.getdata(query);
            DataTable tdcheck = dbhelper.getdata("select * from TOverTimeLine where employeeid=" + Session["emp_id"].ToString() + " and left(convert(varchar,date,101),10)='" + txt_date.Text + "' and (SUBSTRING(status,0, CHARINDEX('-',status))='For Approval' or  SUBSTRING(status,0, CHARINDEX('-',status))='approved' or SUBSTRING(status,0, CHARINDEX('-',status))='for verification' )");
            if (tdcheck.Rows.Count == 0)
            {
                DateTime getshiftout = Convert.ToDateTime(txt_date.Text + " " + dt.Rows[0]["TimeOut2"].ToString());
                DateTime getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text);
                DateTime otin = Convert.ToDateTime(txt_date.Text + " " + txt_start.Text);
                if (getshiftout > otin)
                    otin = getshiftout;
                if (otin.ToString().Contains("PM") && getotout.ToString().Contains("AM"))
                    getotout = Convert.ToDateTime(txt_date.Text + " " + txt_end.Text).AddDays(1);
                if (getotout > getshiftout)
                {
                    query = "select CONVERT(float, datediff(minute, '" + otin + "','" + getotout.ToString() + "' )) / 60 as time_diff ";
                    DataTable dtt = new DataTable();
                    dtt = dbhelper.getdata(query);

                    string nighthrs = getnightdif.getnight(getshiftout.ToString(), getotout.ToString(), "dtr");// (getotout - getshiftout).TotalHours.ToString();
                    decimal totalreghours = decimal.Parse(dtt.Rows[0]["time_diff"].ToString()) - decimal.Parse(nighthrs);

                    DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");

                    dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + txt_date.Text + "','" + totalreghours + "','" + nighthrs + "','0','" + txt_remarks.Text.Replace("'", "") + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + otin + "','" + getotout + "',0,0,'" + approver_id.Rows[0]["under_id"].ToString() + "' )");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_OT'", true);
                }
                else
                    Response.Write("<script>alert('Incorect Time Input!')</script>");
            }
            else
                Response.Write("<script>alert('Input Data is already Exist!')</script>");

        }
    }



    protected void cancel(object sender, EventArgs e)
    {
        if (checkerCan())
        {
            string query = "update TOverTimeLine set status='" + "deleted-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where Id=" + id.Value + " ";
            dbhelper.getdata(query);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='KOISK_OT'", true);

        }
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
            string[] stat = e.Row.Cells[8].Text.Split('-');
            if (stat[0] == "Approved")
                lnkcan.Visible = false;
           // else
           //     lnkcan.CssClass += "fa-trash";

        }
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
        Response.Redirect("KOISK_OT", false);
    }

    protected bool checker()
    {
        bool oi = true;

        //if (txt_date.Text == "")
        //{
        //    oi = false;
        //    lbl_date.Text = "*";
        //}
        //else
        //    lbl_date.Text = "";

        if (txt_start.Text == "")
        {
            oi = false;
            lbl_start.Text = "*";
        }
        else
            lbl_start.Text = "";

        if (txt_end.Text == "")
        {
            oi = false;
            lbl_end.Text = "*";
        }
        else
            lbl_end.Text = "";


        if (txt_remarks.Text == "")
        {
            oi = false;
            lbl_remarks.Text = "*";
        }
        else
            lbl_remarks.Text = "";

        if (txt_date.Text.Length < 10)
        {
            oi = false;
            lbl_date.Text = "Invalid Date";
        }
        else
            lbl_date.Text = "";

        return oi;
    }

    protected bool checkerCan()
    {
        bool oi = true;
        if (txt_reason.Text == "")
        {
            oi = false;
            lbl_reason.Text = "*";
        }
        else
            lbl_reason.Text = "";
        return oi;
    }

    protected void addpreot(object sender, EventArgs e)
    {
        Response.Redirect("KOISK_preOT");
    }
    protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Bind();
        grid_view.PageIndex = e.NewPageIndex;
        grid_view.DataBind();
    }
}