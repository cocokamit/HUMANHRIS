using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Scheduler_create_schedule : System.Web.UI.Page
{
    public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (!IsPostBack)
        {
            loadable();
            //han();
        }

        getdata();

    }


    protected void loadable()
    {

        string query = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where status is null order by id desc";
        DataTable dtover = dbhelper.getdata(query);
        ddl_dtr.Items.Clear();
       //ddl_dtr.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_dtr.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }
    }

    protected void grid_hide(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
    }

    protected void pak(object sender,EventArgs e)
    {
        //string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + ddl_dtr.SelectedValue + "'";
        //DataTable dtover1 = dbhelper.getdata(query1);
        //if (DateTime.Parse(dtover1.Rows[0]["datestart"].ToString()) <= DateTime.Now)
        //    Button1.Visible = false;
        //else
        //    Button1.Visible = true;

        string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                    "SUBSTRING(c.ShiftCode,0,CHARINDEX('(',c.ShiftCode)) As shiftcode, " +
                    "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
                    "d.IdNumber as ID " +
                    "from TChangeShift a " +
                    "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                    "left join MShiftCode c on b.ShiftCodeId=c.id " +
                    "left join memployee d on b.EmployeeId=d.Id " +
                   // "where a.EntryUserId='" + Session["emp_id"].ToString() + "' ";
                    "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and a.EntryUserId='" + Session["emp_id"].ToString() + "' ";
        DataTable hh = dbhelper.getdata(tt);


        string query = "select b.LastName+', '+b.FirstName as NAME,b.IdNumber as ID,b.id as idd " +
                        "from Approver a " +
                        "left join MEmployee b on a.emp_id=b.id " +
                        "where a.under_id='" + Session["emp_id"].ToString() + "' and a.herarchy=0 ";

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

        DataTable sc = dbhelper.getdata("select SUBSTRING(shiftcode,0,CHARINDEX('(',shiftcode)) As shiftcode,id,remarks from  MShiftCode where status is null");

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
                    Button1.Text = "Update";
                }
                else
                    Button1.Text = "Save";
                grid_view.Rows[row].Cells[col].Controls.Add(ddl);
            }
        }



    }

    protected void getdata()
    {

        //string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + ddl_dtr.SelectedValue + "'";
        //DataTable dtover1 = dbhelper.getdata(query1);
        //if (DateTime.Parse(dtover1.Rows[0]["datestart"].ToString()) <= DateTime.Now)
        //    Button1.Visible = false;
        //else
        //    Button1.Visible = true;


        string tt = "select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                    "SUBSTRING(c.ShiftCode,0,CHARINDEX('(',c.ShiftCode)) As shiftcode, " +
                    "b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId, " +
                    "d.IdNumber as ID " +
                    "from TChangeShift a " +
                    "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                    "left join MShiftCode c on b.ShiftCodeId=c.id " +
                    "left join memployee d on b.EmployeeId=d.Id " +
                    "where a.PeriodId='" + ddl_dtr.SelectedValue + "' and a.EntryUserId='" + Session["emp_id"].ToString() + "' ";
        DataTable hh = dbhelper.getdata(tt);




        string query = "select b.LastName+' '+b.FirstName as NAME,b.IdNumber as ID,b.id as idd " +
                       "from Approver a " +
                       "left join MEmployee b on a.emp_id=b.id " +
                       "where a.under_id='" + Session["emp_id"].ToString() + "' and a.herarchy=0 ";
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
                    Button1.Text = "Update";
                }
                else
                    Button1.Text = "Save";
                grid_view.Rows[row].Cells[col].Controls.Add(ddl);
            }
        }
    }

  
    protected void Button1_Click(object sender, EventArgs e)
    {
        stateclass a = new stateclass();
        DataTable dt = (DataTable)ViewState["data"];
        DataTable dtemp; DataTable dtper; DataTable dtsc;

        dtper = dbhelper.getdata("select * from dtr_period where id=" + ddl_dtr.SelectedValue + "");
        int ii = 0;
        foreach (DataRow dr in dt.Rows)
        {
            string JJ = dr[1].ToString();
            dtemp = dbhelper.getdata("select * from memployee where idnumber=" + dr[1] + "");
            if (dtemp.Rows.Count > 0)
            {
                if (dt.Rows[ii]["id"].ToString().Length > 0)
                {
                    string period = dtper.Rows.Count > 0 ? dtper.Rows[0]["id"].ToString() : "0";
                    string pg = dtemp.Rows[0]["payrollgroupid"].ToString();
                    string empid = dtemp.Rows[0]["id"].ToString();

                    if (Button1.Text != "Update")
                    {
                        a.sa = period;
                        a.sb = pg;
                        a.sc = Session["emp_id"].ToString();
                        idd.Value = bol.loadchangeshift1st(a);

                        int j = 2;
                        DataTable range = (DataTable)ViewState["range"];
                        foreach (DataRow date in range.Rows)
                        {
                            DropDownList ddl = (DropDownList)grid_view.Rows[ii].Cells[j].FindControl(dr["ID"] + "_" + date["ddate"].ToString().Replace("/", "_"));
                            dtsc = dbhelper.getdata("select * from mshiftcode where ID='" + ddl.Text + "'");
                            string codeid = dtsc.Rows.Count > 0 ? dtsc.Rows[0]["id"].ToString() : "0";

                            a.sa = empid;
                            a.sb = date["ddate"].ToString();
                            a.sc = codeid;
                            a.sd = idd.Value;
                            bol.loadchangeshift(a);
                            j++;
                        }
                    }
                    else
                    {
                        int ee = 2;
                        DataTable range = (DataTable)ViewState["range"];
                        foreach (DataRow date in range.Rows)
                        {
                            DropDownList ddl = (DropDownList)grid_view.Rows[ii].Cells[ee].FindControl(dr["ID"] + "_" + date["ddate"].ToString().Replace("/", "_"));
                            dtsc = dbhelper.getdata("select * from mshiftcode where ID='" + ddl.Text + "'");
                            string codeid = dtsc.Rows.Count > 0 ? dtsc.Rows[0]["id"].ToString() : "0";
                            dbhelper.getdata("update TChangeShiftLine set ShiftCodeId=" + codeid + " where EmployeeId='" + dr[2].ToString() + "' and left(convert(varchar,date,101),10)='" + date["ddate"].ToString() + "' ");
                            ee++;
                        }
                    }
                }
            }
            ii++;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='ccs'", true);

    }
}