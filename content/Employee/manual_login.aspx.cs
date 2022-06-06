using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_manual_login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            loadable();
    }


 
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (check())
        {

            DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");

            stateclass a = new stateclass();
            a.sa = Session["emp_id"].ToString();
            a.sb = txt_date_in1.Text;
            a.sc = (txt_date_in1.Text + " " + txt_time_in1.Text).Replace(" ","").Length>0?txt_date_in1.Text + " " + txt_time_in1.Text:"0"; //time in 1
            a.si = (txt_date_out1.Text + " " + txt_time_out1.Text).Replace(" ", "").Length > 0 ? txt_date_out1.Text + " " + txt_time_out1.Text : "0"; // time out 1
            a.sh = (txt_date_in2.Text + " " + txt_time_in2.Text).Replace(" ", "").Length > 0 ? txt_date_in2.Text + " " + txt_time_in2.Text : "0";//time in 2
            a.sd = (txt_date_out2.Text + " " + txt_time_out2.Text).Replace(" ", "").Length > 0 ? txt_date_out2.Text + " " + txt_time_out2.Text : "0"; //time out 2
            a.se = txt_reason.SelectedValue;
            a.sf = "For Approval-" + Session["emp_id"] + "-" + DateTime.Now.ToShortDateString().ToString();
            a.sg = txt_remarks.Text.Replace("'", "");
            a.sj = approver_id.Rows[0]["under_id"].ToString();
            string x=bol.manual_logs(a);
            if (x == "0")
                Response.Write("<script>alert('Data is already exist!')</script>");
            else
            {
                //TimeSpan datefile = ;
                string dayname = Convert.ToDateTime(txt_date_in1.Text).DayOfWeek.ToString();
                DataTable dtgetday = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + getshiftcode.shiftcode(txt_date_in1.Text, Session["emp_id"].ToString()) + " and Day='"+dayname+"' ");
                DataTable dtgetdaytype = dbhelper.getdata("select * from MDayTypeDay a left join MDayType b on a.daytypeid=b.id where LEFT(CONVERT(varchar,a.date,101),10)='" + txt_date_in1.Text + "' and a.status is null ");
                DataTable dtgetemp = dbhelper.getdata("select * from memployee where id="+Session["emp_id"].ToString()+" ");
                string daymultiplier;
                string nightmultiplier;
                if (dtgetdaytype.Rows.Count > 0)
                {
                    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                    {
                        daymultiplier = dtgetdaytype.Rows[0]["WorkingDays"].ToString();
                        nightmultiplier = dtgetdaytype.Rows[0]["NightWorkingDays"].ToString();
                    }
                    else
                    {
                        daymultiplier = dtgetdaytype.Rows[0]["RestdayDays"].ToString();
                        nightmultiplier = dtgetdaytype.Rows[0]["NightRestdayDays"].ToString();
                    }
                }
                else
                {
                    DataTable dtgetworkingday = dbhelper.getdata("select * from MDayType where id=1");
                    if (dtgetday.Rows[0]["RestDay"].ToString() == "False")
                    {
                        daymultiplier = dtgetworkingday.Rows[0]["WorkingDays"].ToString();
                        nightmultiplier = dtgetworkingday.Rows[0]["NightWorkingDays"].ToString();
                    }
                    else
                    {
                        daymultiplier = dtgetworkingday.Rows[0]["RestdayDays"].ToString();
                        nightmultiplier = dtgetworkingday.Rows[0]["NightRestdayDays"].ToString();
                    }
                }
                //string getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd) = getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd).ToString();
                DateTime getin = Convert.ToDateTime(getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[0].ToString());
                DateTime get1out = Convert.ToDateTime(getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[1].ToString());
                DateTime get2in = Convert.ToDateTime(getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[2].ToString());
                DateTime getout = Convert.ToDateTime(getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[3].ToString());
                TimeSpan stregh = new TimeSpan();
                TimeSpan ndregh = new TimeSpan();
                //shift set up 
                DateTime setgetin = Convert.ToDateTime(dtgetday.Rows[0]["TimeIn1"].ToString());
                DateTime setget1out = Convert.ToDateTime(dtgetday.Rows[0]["TimeOut1"].ToString());
                DateTime setget2in = Convert.ToDateTime(dtgetday.Rows[0]["TimeIn2"].ToString());
                DateTime setgetout = Convert.ToDateTime(dtgetday.Rows[0]["TimeOut2"].ToString());
                decimal reghours = 0;
                decimal night = 0;
                decimal difbet1out2in = 0;
                decimal difbet1out2innight = 0;
                TimeSpan difbet1out2in1 = setget2in - setget1out;
                difbet1out2in = decimal.Parse(difbet1out2in1.TotalHours.ToString());
                difbet1out2innight = decimal.Parse(getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[7]);
                if (getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[4].ToString() == "True")
                {
                    if (a.sb.Length > 1 && a.si.Length > 1)
                        stregh = get1out - getin;
                    if (a.sh.Length > 1 && a.sd.Length > 1)
                        ndregh = getout - get2in;
                    reghours = decimal.Parse(stregh.TotalHours.ToString()) + decimal.Parse(ndregh.TotalHours.ToString());
                    if (a.sb.Length > 1 && a.si.Length > 1)
                        night = decimal.Parse((getnightdif.getnight(getin.ToString(), get1out.ToString(), "dtr")).ToString());
                    if (a.sh.Length > 1 && a.sd.Length > 1)
                        night += decimal.Parse((getnightdif.getnight(get2in.ToString(), getout.ToString(), "dtr")).ToString());
                }
                else
                {
                    if (getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[4].ToString() == "False" && getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[5].ToString() != "0:00:00" && getshiftcode.getexactpunchdata(Session["emp_id"].ToString(), a.sb, a.sc, a.si, a.sh, a.sd)[6].ToString() != "0:00:00")
                    {
                        if (a.sc.Length > 1 && a.sd.Length > 1)
                            ndregh = getout - getin;
                        reghours = decimal.Parse(ndregh.TotalHours.ToString());
                        if (a.sd.Length > 1)
                        {
                            if (getout >= get2in)
                                reghours = reghours - difbet1out2in;
                        }
                        if (a.sc.Length > 1 && a.sd.Length > 1)
                            night = decimal.Parse((getnightdif.getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                        night = night > 0 ? night - difbet1out2innight : 0;
                    }
                    else
                    {
                        if (a.sc.Length > 1 && a.sd.Length > 1)
                            ndregh = getout - getin;
                        if (a.sc.Length > 1 && a.sd.Length > 1)
                            night = decimal.Parse((getnightdif.getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                        reghours = decimal.Parse(ndregh.TotalHours.ToString());
                    }
                }
                decimal reghrss = (reghours >= night?reghours - night: reghours) ;
                if (reghours > decimal.Parse(dtgetemp.Rows[0]["FixNumberOfHours"].ToString()))
                    reghrss = decimal.Parse(dtgetemp.Rows[0]["FixNumberOfHours"].ToString());
               
                decimal nightamt = (decimal.Parse(dtgetemp.Rows[0]["HourlyRate"].ToString()) * decimal.Parse(nightmultiplier)) * night;
                decimal regamt =  (decimal.Parse(dtgetemp.Rows[0]["HourlyRate"].ToString()) * decimal.Parse(daymultiplier)) * reghrss ;

                dbhelper.getdata("insert into pay_adjustment_details(trans_id,date,emp_id,class,daymultiplier,nightmultiplier,hourlyrate,noofreghrs,noofnighthrs,regamt,nightamt) " +
                                 "values("+x+",GETDATE()," + Session["emp_id"].ToString() + ",'timeadjustment','" + daymultiplier + "','"+nightmultiplier+"','" + dtgetemp.Rows[0]["HourlyRate"].ToString() + "','" + reghrss + "','" + night + "','" + regamt + "','" + nightamt + "')");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KOISK_MANUAL'", true);
            }

        }
    }
    protected void loadable()
    {
       string query = "select PayrollGroupId from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
       DataTable dtt = new DataTable();
       dtt = dbhelper.getdata(query);
       query = "select id,LEFT(CONVERT(varchar,[datestart],101),10)[datestart],LEFT(CONVERT(varchar,[dateend],101),10)[dateend]  from tdtrperpayrol where status like'%Approved%' and payrollgroupid='" + dtt.Rows[0]["PayrollGroupId"].ToString() + "' order by id desc ";
        DataTable getfrombio = dbhelper.getdata(query);
        ddl_dtrfile.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in getfrombio.Rows)
        {
            ddl_dtrfile.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }
    }
    protected void verify_date(object sender, EventArgs e)
    {
       DataTable dt_getemployee = dbhelper.getdata("select case when (select shiftcodeid from TChangeShiftLine where status='Approved' and EmployeeId=a.id and LEFT(CONVERT(varchar,date,101),10)='" + txt_date_in1.Text + "') is null then a.ShiftCodeId else " +
                                                    "(select shiftcodeid from TChangeShiftLine where status='Approved' and EmployeeId=a.id and LEFT(CONVERT(varchar,date,101),10)='" + txt_date_in1.Text + "' ) end scid from MEmployee a where Id='" + Session["emp_id"].ToString() + "'");
       if (dt_getemployee.Rows.Count > 0)
       {
           DataTable getshiftday = dbhelper.getdata("select * from MShiftCode where id=" + dt_getemployee.Rows[0]["scid"].ToString() + " and broke='1'");
           if (getshiftday.Rows.Count > 0)
               div_1.Visible = true;
           else
               div_1.Visible = false;
       }




    }
    protected void dtr_go(object sender, EventArgs e)
    {
       string query = "select  IdNumber from MEmployee where Id='" + Session["emp_id"].ToString() + "'";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);


        DataTable dt = dbhelper.getdata("SELECT b.empid emp_id,'0'employee,CONVERT(datetime,b.Biotime)Biotime from tdtrperpayrol a left join tdtrperpayrolperline b on a.id=b.dtrperpayrol_id where a.id=" + ddl_dtrfile.SelectedValue + " and b.empid='" + Session["emp_id"].ToString() + "' and b.status is null order by Biotime asc  ");
        DataTable ndt = new DataTable();
        ndt.Columns.Add(new DataColumn("emp_id", typeof(string)));
        ndt.Columns.Add(new DataColumn("employee", typeof(string)));
        ndt.Columns.Add(new DataColumn("date", typeof(string)));
        ndt.Columns.Add(new DataColumn("time", typeof(string)));
        ndt.Columns.Add(new DataColumn("apm", typeof(string)));
        //DataTable dt = DtSet.Tables[0];
        DataRow dr;
        foreach (DataRow row in dt.Rows)
        {
            dr = ndt.NewRow();
            dr["emp_id"] = row[0].ToString().Length == 1 ? "0" + row[0] : row[0];
            dr["employee"] = row[1];
            dr["date"] = row[2];
            string sam = row[2].ToString();
            string[] date = row[2].ToString().Split(' ');
            string[] specdate = date[0].ToString().Split('/');
            string  month = specdate[0].ToString().Length > 1 ? specdate[0].ToString() : "0" + specdate[0].ToString();
            string day = specdate[1].ToString().Length > 1 ? specdate[1].ToString() : "0" + specdate[1].ToString();
            dr["date"] = month + "/" + day + "/" + specdate[2].ToString();
            dr["time"] = date[1];
            dr["apm"] = date[2];
            ndt.Rows.Add(dr);
        }

        DataView view = new DataView(ndt);
        DataTable dis_temp = view.ToTable(true, "emp_id");
        DataTable dis_date = view.ToTable(true, "date");

        DataTable final_disp = new DataTable();
        final_disp.Columns.Add(new DataColumn("emp_id", typeof(string)));
        final_disp.Columns.Add(new DataColumn("employee", typeof(string)));
        final_disp.Columns.Add(new DataColumn("date", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_In", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_Out", typeof(string)));

        DataRow final_dr;
        foreach (DataRow row in dis_temp.Rows)
        {
            //DataTable get_per_emp = new DataTable();
            foreach (DataRow ro in dis_date.Rows)
            {
                string rowdd = row["emp_id"].ToString();
                string rodd = ro["date"].ToString();
                DataRow[] get_per_emp = ndt.Select("emp_id='" + row["emp_id"] + "' and date='" + ro["date"] + "' ");
                if (get_per_emp.Count() > 0)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        if (i == 0)
                        {
                            //insert datatable
                            final_dr = final_disp.NewRow();
                            final_dr["emp_id"] = get_per_emp[0].ItemArray[0] as string;
                            final_dr["employee"] = get_per_emp[0].ItemArray[1] as string;
                            final_dr["date"] = get_per_emp[0].ItemArray[2] as string;
                            final_dr["Date_Time_In"] = get_per_emp[0].ItemArray[2] + " " + get_per_emp[0].ItemArray[3] + " " + get_per_emp[0].ItemArray[4];
                            final_disp.Rows.Add(final_dr);
                        }
                        if (i == 1)
                        {
                            //update datatablerow
                            if (get_per_emp.Count() > 1)
                                final_disp.Rows[final_disp.Rows.Count - 1]["Date_Time_Out"] = get_per_emp[get_per_emp.Count() - 1].ItemArray[2] + " " + get_per_emp[get_per_emp.Count() - 1].ItemArray[3] + " " + get_per_emp[get_per_emp.Count() - 1].ItemArray[4];
                            else
                                final_disp.Rows[final_disp.Rows.Count - 1]["Date_Time_Out"] = "No Log out";
                        }
                    }
                }
                else
                {
                    final_dr = final_disp.NewRow();
                    final_dr["emp_id"] = "0";
                    final_dr["employee"] = "0";
                    final_dr["date"] = "0";
                    final_dr["Date_Time_In"] = "0";
                    final_dr["Date_Time_Out"] = "0";
                    final_disp.Rows.Add(final_dr);
                }
            }
        }
        grid_view.DataSource = final_disp;
        grid_view.DataBind();
    }
    public bool check()
    {
        bool oi = true;
        if (txt_date_in1.Text == "")
        {
            lbl_date_in.Text = "*";
            oi = false;
        }
        else
            lbl_date_in.Text = "";
        if (txt_time_out2.Text == "")
        {
            lbl_time_out2.Text = "*";
            oi = false;
        }
        else
            lbl_time_out2.Text = "";
        
        if (div_1.Visible == true)
        {
            if (txt_date_out1.Text == "")
            {
                lbl_date_out.Text = "*";
                oi = false;
            }
            else
                lbl_date_out.Text = "";

            if (txt_time_in2.Text == "")
            {
                lbl_time_in2.Text = "*";
                oi = false;
            }
            else
                lbl_time_in2.Text = "";
        }
        

        if (txt_reason.Text == "")
        {
            lbl_reason.Text = "*";
            oi = false;
        }
        else
            lbl_reason.Text = "";

        if (txt_remarks.Text == "")
        {
            lbl_remarks.Text = "*";
            oi = false;
        }
        else
            lbl_remarks.Text = "";

        return oi;
    }
}