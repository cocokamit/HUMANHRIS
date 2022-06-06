using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_payroll_sc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
        {
            Response.Redirect("quit");
        }
        if (!IsPostBack)
        {
            loadable();
            pay_range();
            disp();
        }
      

    }
    protected void pay_range()
    {
        string query = "select a.id payid,left(convert(varchar,b.DateStart,101),10)start,left(convert(varchar,b.Dateend,101),10)eend from tpayroll a " +
                        "left join tdtr b on a.dtrid=b.id " +
                        "left join sc_transaction c on a.id=c.payid " +
                        "where a.status is null and (select COUNT(*) from sc_transaction where payid=a.id and action is null)=0 " +
                        "order by b.DateStart desc";
        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
        ddl_pay_range.Items.Add(new ListItem("Select Payroll Range", "0"));
        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["start"].ToString() + "-" + dr["eend"].ToString(), dr["payid"].ToString()));
        }
    }
    protected void searchtrn(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {
        string query = "select payid, sctrnid,left(convert(varchar,date,101),10)date,left(convert(varchar,dfrom,101),10)dfrom,left(convert(varchar,dtoo,101),10)dtoo,sc_amt from sc_transaction where action is null ";
       
        if (txt_from.Text.Length > 0 || txt_to.Text.Length > 0)
            query += "and CONVERT(date,dfrom)>=CONVERT(date,'" + txt_from.Text + "') and CONVERT(date,dfrom)<=CONVERT(date,'" + txt_to.Text + "')";
        DataTable dt = dbhelper.getdata(query);
        grid_sc_trans.DataSource = dt;
        grid_sc_trans.DataBind();
    }
    protected void loadable()
    {
        //MASTER DATA
        DataTable master = new DataTable();
        master.Columns.Add(new DataColumn("rownumber", typeof(string)));
        master.Columns.Add(new DataColumn("empid", typeof(string)));
        master.Columns.Add(new DataColumn("employee", typeof(string)));
        master.Columns.Add(new DataColumn("actualsc", typeof(string)));
        master.Columns.Add(new DataColumn("sccompanypercentage", typeof(string)));
        master.Columns.Add(new DataColumn("scbreakages", typeof(string)));
        master.Columns.Add(new DataColumn("netsc", typeof(string)));
        master.Columns.Add(new DataColumn("scmultiplier", typeof(string)));
        master.Columns.Add(new DataColumn("tnod", typeof(string)));
        master.Columns.Add(new DataColumn("tdpe", typeof(string)));
        master.Columns.Add(new DataColumn("empsc", typeof(string)));
        ViewState["master"] = master;
        grid_item.DataSource = master;
        grid_item.DataBind();
        
    }
    protected void verifysc(object sender,EventArgs e)
    {
        DataTable master = ViewState["master"] as DataTable;
        if (txt_sc_amt.Text.Replace(",","").Length == 0)
            Response.Write("<script>alert('Invalid Input Service Charge Amount!')</script>");
        else
        {
            if (Button1.Text == "Verify")
            {
                   decimal sccompanypercentage = decimal.Parse(txt_sc_percentage.Text) / 100;
                   decimal actsc = decimal.Parse(txt_sc_amt.Text);
                   decimal empsc = 0;
                   decimal netsc = 0;
                   decimal scbreakages = 0;
                   decimal sctnod = 0;
                   decimal scmultiplier = 0;

                   //string queryall = " select ID,e_name, case when total_days_per_emp is null then 0 else total_days_per_emp end total_days_per_emp,case when total_days_whole_cutoff is null then 0 else total_days_whole_cutoff end total_days_whole_cutoff from  " +
                   //                     "(  " +
                   //                         "select  a.id,a.lastname+', '+a.firstname+' '+a.middlename e_name, " +
                   //                         "( " +
                   //                             "select SUM( case when  z.RegularHours + z.NightHours + z.totaloffsethrs + case when HalfLeave ='True' then 1 else 0 end + case when OnLeave ='True' then 1 else 0 end> 0 then 1 else 0 end)  " +
                   //                             "from tdtrline z  " +
                   //                             "left join tdtr y on z.DTRId=y.Id " +
                   //                             "left join memployee x on z.EmployeeId=x.Id " +
                   //                             "where  (z.RegularHours + z.NightHours + z.totaloffsethrs + (case when z.HalfLeave ='True' then 1 else 0 end) + (case when z.OnLeave ='True' then 1 else 0 end))>0 and y.status is null  " +
                   //                             "and CONVERT(DATE,z.[DATE])>=CONVERT(DATE,'" + txt_f.Text + "') AND CONVERT(DATE,z.[DATE])<=CONVERT(DATE,'" + txt_t.Text + "') " +
                   //                             "and z.EmployeeId=a.Id " +
                   //                             ") total_days_per_emp, " +
                   //                             "( " + 
                   //                             "select SUM( case when  z.RegularHours + z.NightHours + z.totaloffsethrs + case when HalfLeave ='True' then 1 else 0 end + case when OnLeave ='True' then 1 else 0 end> 0 then 1 else 0 end) " +
                   //                             "from tdtrline z  " +
                   //                             "left join tdtr y on z.DTRId=y.Id " +
                   //                             "left join memployee x on z.EmployeeId=x.Id " +
                   //                             "where  (z.RegularHours + z.NightHours + z.totaloffsethrs + (case when z.HalfLeave ='True' then 1 else 0 end) + (case when z.OnLeave ='True' then 1 else 0 end))>0 and y.status is null " +
                   //                             "and CONVERT(DATE,z.[DATE])>=CONVERT(DATE,'" + txt_f.Text + "') AND CONVERT(DATE,z.[DATE])<=CONVERT(DATE,'" + txt_t.Text + "') " +
                   //                             ") total_days_whole_cutoff " +
                   //                         "from memployee a " +
                   //                     ") tt ";
                   string queryall = "select  " +
                                "a.id,a.lastname+', '+a.firstname+' '+a.middlename e_name,  " +
                                "(select case when ( 13 - SUM(case when stat='True' then 1   else 0 end)) is null then 0 else ( 13 - SUM(case when stat='True' then 1   else 0 end)) end from  " +
                                "( " +
                                "	select  case when Absent='True' or lwopabsent='1' then 'Absent' else 'False' end stat from " +
                                "	( " +
                                "		select EmployeeId, date,[Absent], " +
                                "		(select COUNT(*) from TLeaveApplicationLine where EmployeeId=z.employeeid and CONVERT(date,date)=CONVERT(date,z.date) and WithPay='False')lwopabsent  " +
                                "		from TDTRLine z  left join tdtr y on z.DTRId=y.Id where z.EmployeeId=a.id and y.status is null  " +
                                                            "and CONVERT(DATE,z.[DATE])>=CONVERT(DATE,'" + txt_f.Text + "') AND CONVERT(DATE,z.[DATE])<=CONVERT(DATE,'" + txt_t.Text + "') " + 
                                "	)riesol  " +
                                ")riesol_1 " +
                                ") as total_days_per_emp " +
                                //"(select 13 - SUM(case when stat='True' then 1   else 0 end) total_absent from  " +
                                //"( " +
                                //"	select  case when Absent='True' or lwopabsent='1' then 'Absent' else 'False' end stat from " +
                                //"	( " +
                                //"		select EmployeeId, date,[Absent], " +
                                //"		(select COUNT(*) from TLeaveApplicationLine where EmployeeId=z.employeeid and CONVERT(date,date)=CONVERT(date,z.date) and WithPay='False')lwopabsent " +
                                //"		from TDTRLine z  where z.EmployeeId=a.id " +
                                //"	)riesol " +
                                //")riesol_1 " +
                                //") as total_days_whole_cutoff " +
                                "from memployee a";
                    DataTable dtall = dbhelper.getdata(queryall);
                    decimal total_render_days = 0;
                    total_render_days = decimal.Parse(dtall.Compute("sum(total_days_per_emp)", string.Empty).ToString());
                    
                    DataRow mdr;
                    int i=1;
                    foreach (DataRow dr in dtall.Rows)
                    {
                        scbreakages = actsc * sccompanypercentage;
                        netsc = actsc - scbreakages;
                        sctnod = total_render_days; //decimal.Parse(dr["total_days_whole_cutoff"].ToString());
                        scmultiplier = netsc / sctnod;
                        empsc = decimal.Parse(dr["total_days_per_emp"].ToString()) * scmultiplier;
                        if (empsc > 0)
                        {
                            mdr = master.NewRow();
                            mdr["rownumber"] = i;
                            mdr["empid"] = dr["id"];
                            mdr["employee"] = dr["e_name"];
                            mdr["actualsc"] = actsc;
                            mdr["sccompanypercentage"] = sccompanypercentage;
                            mdr["scbreakages"] = scbreakages;
                            mdr["netsc"] = netsc;
                            mdr["scmultiplier"] = scmultiplier;
                            mdr["tnod"] = sctnod;
                            mdr["tdpe"] = decimal.Parse(dr["total_days_per_emp"].ToString());
                            mdr["empsc"] = string.Format("{0:n2}", empsc);
                            master.Rows.Add(mdr);
                        }
                        i++;
                    }

                    ViewState["master"] = master;
                    grid_item.DataSource = master;
                    grid_item.DataBind();
                    Button1.Text = "Re-type";
                    txt_sc_amt.Enabled = false;
                    txt_sc_percentage.Enabled = false;
                    txt_f.Enabled = false;
                    txt_t.Enabled = false;
                }
                else
                {
                    Button1.Text = "Verify";
                    txt_sc_amt.Enabled = true;
                    txt_sc_percentage.Enabled = true;
                    txt_f.Enabled = true;
                    txt_t.Enabled = true;
                    loadable();
                    grid_item.DataSource = null;
                    grid_item.DataBind();
                }
        }
        if (grid_item.Rows.Count > 0)
        {
            btn_save.Visible = true;
        }
        else
            btn_save.Visible = false;
    }
    protected void Save(object sender, EventArgs e)
    {
            DataTable dtmaster=ViewState["master"] as DataTable;
    
       
            DataTable dtchk = dbhelper.getdata("select * from sc_transaction where convert(date,dtoo)>=convert(date,'" + txt_f.Text + "') and action is null");
            if (dtchk.Rows.Count == 0 && Convert.ToDateTime(txt_t.Text) >= Convert.ToDateTime(txt_f.Text))
            {
                string query = "INSERT INTO [sc_transaction]([date],[userid],[useridlastupdate],[datelastupdate],[dfrom],[dtoo],[sc_amt],[percentage],[payid]) " +
                                "VALUES " +
                                "(getdate()," + Session["user_id"] + "," + Session["user_id"] + ",getdate(),'" + txt_f.Text + "','" + txt_t.Text + "','" + txt_sc_amt.Text.Replace(",", "") + "','" + txt_sc_percentage.Text + "',0) Select scope_identity() id";
                DataTable dt = dbhelper.getdata(query);
                foreach (DataRow dr in dtmaster.Rows)
                {
                    query = "INSERT INTO [sc_trn_details]([sctrnid],[empid],[manhr],[grossamt],[wht],[net]) " +
                            "VALUES(" + dt.Rows[0]["id"].ToString() + "," + dr["empid"].ToString() + ",'" + dr["tdpe"].ToString() + "','" + dr["empsc"].ToString().Replace(",", "") + "',0,0)";
                    dbhelper.getdata(query);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='sc'", true);
            }
            else
            {
                Response.Write("<script>alert('Invalid Date Range Input!')</script>");
            }
        

    }
    protected void generatetexfile(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
           // string ggg = "select c.ATMAccountNumber Account_No,a.NetIncome Net_Income " +
           //"from TPayrollLine a " +
           //"left join TPayroll b on a.PayrollId=b.Id " +
           //"left join MEmployee c on a.EmployeeId=c.Id " +
           //"where a.PayrollId=" + row.Cells[0].Text + " and len(c.ATMAccountNumber)>0  and a.NetIncome>0 order by b.PayrollDate desc ";
           // DataTable dt = dbhelper.getdata(ggg);

            string query = "select  b.ATMAccountNumber Account_No,a.net Net_Income from sc_trn_details a left join memployee b on a.empid=b.Id where len(b.ATMAccountNumber)>0 and  a.sctrnid=" + row.Cells[0].Text + "";
            DataTable dt = dbhelper.getdata(query);
            //grid_item.DataSource = dt;
            //grid_item.DataBind();
            //Build the Text file data.
            string txt = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                txt += column.ColumnName + "\t";
            }
            txt += "\r\n";
            foreach (DataRow rows in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    txt += rows[column.ColumnName].ToString() + "\t";
                }
                txt += "\r\n";
            }
            string filename = row.Cells[2].Text.Replace("/", "") + "to" + row.Cells[3].Text.Replace("/", "") + "_Service_Charge.txt";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + "");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(txt);
            Response.Flush();
            Response.End();
        }
    }
    protected void newdtrlogs(object sender, EventArgs e)
    {
        ppop(true);
        panelPopUpPanel.Style.Add("top","5%");
        panelPopUpPanel.Style.Add("width", "900px");
        panelPopUpPanel.Style.Add("left", "450px");
    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("sc");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                LinkButton lnkcan = (LinkButton)row.FindControl("lnkcan");
                dbhelper.getdata("update sc_transaction set action='Cancel',useridlastupdate=" + Session["user_id"] + ",datelastupdate=getdate() where sctrnid=" + row.Cells[0].Text + " ");
                //dbhelper.getdata("delete from tdtrperpayrolperline where dtrperpayrol_id=" + lnkcan.CommandName + " ");
                Response.Redirect("sc", false);
            }
            else
            {
            }
        }
    }
    protected void rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnkcan");
            if (lnkcan.CommandName == "0")
                lnkcan.Visible = true;
            else
                lnkcan.Visible = false;
        }
    }
    
}