using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class content_Employee_change_shift_list : System.Web.UI.Page
{
    //public static string query, user_id, id;
    public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
            loadable();
            //han();
        }

        BindData();
         
    }


    protected void loadable()
    {
    
        DataTable aw = dbhelper.getdata("select under_id from Approver where emp_id='"+Session["emp_id"].ToString()+"' and herarchy=0");

        DataTable dt = getdata.sys_dtr("status='approved' and emp_id='" + aw.Rows[0]["under_id"].ToString() + "' ");
        if (dt.Rows.Count > 0)
        {
            ddl_dtr.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ddl_dtr.Items.Add(new ListItem(dr["period"].ToString(), dr["id"].ToString()));
            }
        }
    }

    protected void grid_hide(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
    }

    protected void pak(object sender, EventArgs e)
    {
        //string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + ddl_dtr.SelectedValue + "'";
        //DataTable dtover1 = dbhelper.getdata(query1);
        //if (DateTime.Parse(dtover1.Rows[0]["datestart"].ToString()) <= DateTime.Now)
        //    Button1.Visible = false;
        //else
        //    Button1.Visible = true;

        //string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
        //            "SUBSTRING(c.ShiftCode,0,CHARINDEX('(',c.ShiftCode)) As shiftcode, " +
        //            "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
        //            "d.IdNumber as ID " +
        //            "from TChangeShift a " +
        //            "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
        //            "left join MShiftCode c on b.ShiftCodeId=c.id " +
        //            "left join memployee d on b.EmployeeId=d.Id " +
        //            "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and b.EmployeeId='" + Session["emp_id"].ToString() + "' ";

        string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                   "c.ShiftCode As shiftcode, " +
                   "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
                   "d.IdNumber as ID " +
                   "from TChangeShift a " +
                   "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                   "left join MShiftCode c on b.ShiftCodeId=c.id " +
                   "left join memployee d on b.EmployeeId=d.Id " +
                   "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and b.EmployeeId='" + Session["emp_id"].ToString() + "' ";

        DataTable hh = dbhelper.getdata(tt);



        string query = "select distinct(b.FirstName+' '+b.LastName) as NAME,b.IdNumber as ID,b.id as idd " +
                        "from Approver a " +
                        "left join MEmployee b on a.emp_id=b.id " +
                        "where a.emp_id='" + Session["emp_id"].ToString() + "' ";

        dt = dbhelper.getdata(query);
        ViewState["data"] = dt;

        string awts = "create table #dummy(ddate varchar(50)) " +
                       "declare @start datetime " +
                       "declare @end datetime " +
                       "select @start=date_start,@end=date_end from dtr_period  where ID='" + ddl_dtr.SelectedValue + "' " +
                       "insert into #dummy(ddate) values (left(convert(varchar,@start,101),10)) " +
                       "while(@start<@end) " +
                       "begin " +
                       "set @start=DATEADD(DAY,1,@start) " +
                       "insert into #dummy(ddate)values(left(convert(varchar,@start,101),10)) " +
                       "end " +
                       "select * from #dummy " +
                       "drop table #dummy ";
        DataTable ddt = dbhelper.getdata(awts);
        ViewState["range"] = ddt;


        for (int aa = 0; aa < ddt.Rows.Count; aa++)
        {
            dt.Columns.Add(new DataColumn(ddt.Rows[aa]["ddate"].ToString(), typeof(string)));
        }
        grid_view.DataSource = dt;
        grid_view.DataBind();

        //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
        DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where status is null");

        for (int row = 0; row < dt.Rows.Count; row++)
        {
            for (int col = 2; col < dt.Columns.Count; col++)
            {
                DropDownList ddl = new DropDownList();
                ddl.ID = dt.Rows[row]["ID"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_");
                ddl.Items.Clear();
                //ddl.Items.Add(new ListItem("", "0"));
                foreach (DataRow dr in sc.Rows)
                {
                    ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
                    item.Attributes.Add("title", dr["remarks"].ToString());
                    ddl.Items.Add(item);
                }

                if (hh.Rows.Count > 0)
                {
                    string hold = null;
                    string colname = dt.Columns[col].ToString();
                    DataRow[] dr = hh.Select("ID=" + dt.Rows[row][1].ToString() + " and date='" + colname + "'");
                    foreach (DataRow r in dr)
                    {
                        hold = r[3].ToString();
                    }
                    ddl.SelectedValue = hold;
                    //Button1.Text = "Update";
                }
                //else
                   //Button1.Text = "Save";
                grid_view.Rows[row].Cells[col].Controls.Add(ddl);
            }
        }



    }

    protected void BindData()
    {

        //string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + ddl_dtr.SelectedValue + "'";
        //DataTable dtover1 = dbhelper.getdata(query1);
        //if (DateTime.Parse(dtover1.Rows[0]["datestart"].ToString()) <= DateTime.Now)
        //    Button1.Visible = false;
        //else
        //    Button1.Visible = true;


        //string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
        //            "SUBSTRING(c.ShiftCode,0,CHARINDEX('(',c.ShiftCode)) As shiftcode, " +
        //            "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
        //            "d.IdNumber as ID " +
        //            "from TChangeShift a " +
        //            "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
        //            "left join MShiftCode c on b.ShiftCodeId=c.id " +
        //            "left join memployee d on b.EmployeeId=d.Id " +
        //            "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and b.EmployeeId='" + Session["emp_id"].ToString() + "' ";

        string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                    "c.ShiftCode As shiftcode, " +
                    "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
                    "d.IdNumber as ID " +
                    "from TChangeShift a " +
                    "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                    "left join MShiftCode c on b.ShiftCodeId=c.id " +
                    "left join memployee d on b.EmployeeId=d.Id " +
                    "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and b.EmployeeId='" + Session["emp_id"].ToString() + "' ";


        DataTable hh = dbhelper.getdata(tt);


        if (hh.Rows.Count > 0)
        {



            string query = "select distinct(b.FirstName+' '+b.LastName) as NAME,b.IdNumber as ID,b.id as idd " +
                            "from Approver a " +
                            "left join MEmployee b on a.emp_id=b.id " +
                            "where a.emp_id='" + Session["emp_id"].ToString() + "' ";


            dt = dbhelper.getdata(query);
            ViewState["data"] = dt;


            string awts = "create table #dummy(ddate varchar(50)) " +
                           "declare @start datetime " +
                           "declare @end datetime " +
                           "select @start=date_start,@end=date_end from dtr_period  where ID='" + ddl_dtr.SelectedValue + "' " +
                           "insert into #dummy(ddate) values (left(convert(varchar,@start,101),10)) " +
                           "while(@start<@end) " +
                           "begin " +
                           "set @start=DATEADD(DAY,1,@start) " +
                           "insert into #dummy(ddate)values(left(convert(varchar,@start,101),10)) " +
                           "end " +
                           "select * from #dummy " +
                           "drop table #dummy ";
            DataTable ddt = dbhelper.getdata(awts);
            ViewState["range"] = ddt;

            for (int aa = 0; aa < ddt.Rows.Count; aa++)
            {
                dt.Columns.Add(new DataColumn(ddt.Rows[aa]["ddate"].ToString(), typeof(string)));
            }
            //dt.Columns.RemoveAt(0);
            //dt.Columns.RemoveAt(1);
            //dt.Columns.RemoveAt(0);
            grid_view.DataSource = dt;
            grid_view.DataBind();



            //DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");
            DataTable sc = dbhelper.getdata("select shiftcode,id,remarks from  MShiftCode where status is null");


            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 2; col < dt.Columns.Count; col++)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = dt.Rows[row]["ID"].ToString() + "_" + dt.Columns[col].ToString().Replace("/", "_");// grid_view.Rows[row].Cells[0].Text + "_" + dt.Columns[col].ToString();
                    ddl.Items.Clear();
                    //ddl.Items.Add(new ListItem("", "0"));
                    foreach (DataRow dr in sc.Rows)
                    {
                        ListItem item = new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString());
                        item.Attributes.Add("title", dr["remarks"].ToString());
                        ddl.Items.Add(item);

                    }
                    if (hh.Rows.Count > 0)
                    {
                        string hold = null;
                        string colname = dt.Columns[col].ToString();
                        DataRow[] dr = hh.Select("ID=" + dt.Rows[row][1].ToString() + " and date='" + colname + "'");
                        foreach (DataRow r in dr)
                        {
                            hold = r[3].ToString();
                        }
                        ddl.SelectedValue = hold;
                        // Button1.Text = "Update";
                    }
                    //else
                    //Button1.Text = "Save";
                    grid_view.Rows[row].Cells[col].Controls.Add(ddl);
                }
            }
        }
        else
        {
           alert.Visible = true;
           Button1.Enabled = false;
        }
    }

    //protected void getdata()
    //{

    //     query = "select a.id,a.[Date],a.EmployeeID,a.status, " +
    //            "b.remarks, " +
    //            "c.shiftcode, " +
    //            "d.FirstName +' '+d.LastName as name " +
    //            "from TChangeShiftLine a " +
    //            "left join TChangeShift b on a.changeshiftid=b.id " +
    //            "left join Mshiftcode c on a.ShiftCodeId=c.id " +
    //            "left join Memployee d on a.EmployeeId=d.id " +
    //            "where a.employeeId ='" + Session["emp_id"].ToString() + "' ";
    //      DataSet ds = new DataSet();
    //    ds = bol.display(query);
    //    grid_view.DataSource = ds;
    //    grid_view.DataBind();
    //}

    //protected void view(object sender, EventArgs e)
    //{
    //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
    //    {
    //        id = row.Cells[0].Text;


    //        Div1.Visible = true;
    //        Div2.Visible = true;

    //        Div3.Visible = false;
    //        Div4.Visible = false;
    //    }
    //}
    //protected void view1(object sender, EventArgs e)
    //{
    //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
    //    {
    //        id = row.Cells[0].Text;
    //        query = "select a.status,a.remarks,a.date, " +
    //                "b.lastname+' '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname " +
    //                "from sys_applog a " +
    //                "left join MEmployee  b on a.emp_id=b.Id " +
    //                "where type='CS' and app_id=" + id + " order by a.id ";


    //        DataSet ds = new DataSet();
    //        ds = bol.display(query);
    //        grid_approver.DataSource = ds;
    //        grid_approver.DataBind();

    //        //nadarang();
    //        Div1.Visible = false;
    //        Div2.Visible = false;

    //        Div3.Visible = true;
    //        Div4.Visible = true;
    //    }
    //}

    //protected void cancel(object sender, EventArgs e)
    //{

    //    query = "update TChangeShiftLine set status='" + "deleted-" + DateTime.Now.ToShortDateString().ToString() + "',Remarks='" + txt_reason.Text.Replace("'", "") + "' where id=" + id + " ";
    //    dbhelper.getdata(query);

    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cancelled Successfully'); window.location='csr?user_id=" + function.Encrypt(user_id, true) + "'", true);
    //}


    protected void adders(object sender, EventArgs e)
    {
        //string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + ddl_dtr.SelectedValue + "'";
        //DataTable dtover1 = dbhelper.getdata(query1);

        //if (DateTime.Parse(dtover1.Rows[0]["datestart"].ToString()) < DateTime.Now && DateTime.Parse(dtover1.Rows[0]["dateend"].ToString()) > DateTime.Now)
            Response.Redirect("cs?key=" + ddl_dtr.SelectedValue);
        //else
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lapas na sa Date Period or Wala pa sa Date Period'); window.location='csr'", true);
    }
    //protected void cpop(object sender, EventArgs e)
    //{
    //    Response.Redirect("csr?user_id=" + function.Encrypt(user_id, true) + "", false);
    //}
    //protected void rowbound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnk_can");
    //        string[] stat = e.Row.Cells[4].Text.Split('-');
    //        if (stat[0] == "Approved")
    //            lnkcan.Visible = false;


    //    }
    //}
}