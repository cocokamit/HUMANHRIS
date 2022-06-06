using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_payroll_proccesspayroll : System.Web.UI.Page
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session.Count == 0)
                Response.Redirect("quit?key=out");
            lodable();
            // user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disppayroll();
           
        }
    }

    protected void lodable()
    {
        string query = "select * from MPayrollGroup where status <> 0 order by id desc";
        DataTable dt = dbhelper.getdata(query);
        ddl_pg.Items.Clear();
        ddl_pg_tmp.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        ddl_pg_tmp.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
            ddl_pg_tmp.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        ddl_yyyy_tmp.Items.Clear();
        ddl_yyyy_tmp.Items.Add(new ListItem("None", "0"));
        int i = 2018;
        for (; i <= DateTime.Now.Year + 1; i++)
        {
            ddl_yyyy_tmp.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }
    protected void click_add_dtr(object sender, EventArgs e)
    {
        //if (ddl_pg.SelectedValue =="0")
        //    Response.Write("<script>alert('Please select PayrollGroup!')</script>");
        //else
          Response.Redirect("addproccesspayroll", false);
    }
    protected void disppayroll()
    {
        DataTable dt = dbhelper.getdata("select a.id,LEFT(CONVERT(varchar,a.PayrollDate,101),10)PayrollDate,(select DATENAME(month,a.PayrollDate))+' '+(case when(select DAY(LEFT(CONVERT(varchar,b.DateStart,101),10)))=11 then '10th' else '25th' end)+' '+(select CONVERT(varchar(10),(select year(a.PayrollDate))))remss, " +
                                     "left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10) datedtrrange,c.PayrollGroup pg,case when a.status_1 is null then 'Pending' else a.status_1 end status_1   " +
                                     "from TPayroll a " +
                                     "left join TDTR b on a.DTRId=b.Id  " +
                                     "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                                     "where a.status is null  order by a.PayrollDate desc");
        grid_paylist.DataSource = dt;
        grid_paylist.DataBind();
    
    }
    protected void click_search(object sender, EventArgs e)
    {
        string query="select a.id,LEFT(CONVERT(varchar,a.PayrollDate,101),10)PayrollDate, " +
                                     "left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10) datedtrrange,c.PayrollGroup pg,case when a.status_1 is null then 'Pending' else a.status_1 end status_1   " +
                                     "from TPayroll a " +
                                     "left join TDTR b on a.DTRId=b.Id  " +
                                     "left join MPayrollGroup c on a.PayrollGroupId=c.Id " +
                                    "where a.status is null ";
                                    if (int.Parse(ddl_pg.SelectedValue) > 0)
                                        query += " and a.payrollgroupid=" + ddl_pg.SelectedValue + " ";
                                    if (txt_f.Text.Length > 0 && txt_t.Text.Length > 0)
                                        query += " and left(convert(varchar,a.PayrollDate,101),10)>='" + txt_f.Text + "' and left(convert(varchar,a.PayrollDate,101),10) <='" + txt_t.Text + "'";
                                    query += "order by a.id desc";
                        
        DataTable dt = dbhelper.getdata(query);
        grid_paylist.DataSource = dt;
        grid_paylist.DataBind();
    }

    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='payrolldetails?&payid=" + function.Encrypt(row.Cells[0].Text, true) + "'", true);
        }
    }

    protected void click_printbatch(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?key=" + function.Encrypt(dtinsappform.Rows[0]["trnid"].ToString(), true) + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='printablepayslip?payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=batch'", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?printablepayslip?empid=" + function.Encrypt(row.Cells[1].Text, true) + "&payid=" + function.Encrypt(row.Cells[0].Text, true) + "&b=single'", true);
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_posted = (LinkButton)row.FindControl("lnk_posted");
            if (TextBox1.Text == "Yes")
            {

                DateTime sd = Convert.ToDateTime(DateTime.Now.ToShortDateString());//DateTime.Now.ToShortDateString()
                DateTime gg = Convert.ToDateTime(row.Cells[1].Text.ToString());
                TimeSpan result = sd - gg;
                if (lnk_posted.Text == "Pending" || lnk_posted.Text == "Draft")
                {
                    string query = "select a.id,LEFT(CONVERT(varchar,a.PayrollDate,101),10)PayrollDate,case when(select DAY(LEFT(CONVERT(varchar,b.DateStart,101),10)))=11 then '10th Payroll' else '25th Payroll' end remrequest,(select DATENAME(month,a.PayrollDate))+' '+(SELECT CONVERT(VARCHAR(10),(SELECT DAY(a.PayrollDate))) + CASE WHEN (SELECT DAY(a.PayrollDate)) % 100 IN (11, 12, 13) THEN 'th' ELSE CASE (SELECT DAY(a.PayrollDate)) % 10 WHEN 1 THEN 'st' WHEN 2 THEN 'nd' WHEN 3 THEN 'rd' ELSE 'th' END END)+' '+(select CONVERT(varchar(10),(select year(a.PayrollDate))))PDateREMS,(select DATENAME(month,a.PayrollDate))+' '+(case when(select DAY(LEFT(CONVERT(varchar,b.DateStart,101),10)))=11 then '10th' else '25th' end)+' '+(select CONVERT(varchar(10),(select year(a.PayrollDate))))remss, left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10) datedtrrange,c.PayrollGroup pg,case when a.status_1 is null then 'Pending' else a.status_1 end status_1 from TPayroll a left join TDTR b on a.DTRId=b.Id left join MPayrollGroup c on a.PayrollGroupId=c.Id where a.status is null and a.Id = " + row.Cells[0].Text + "";
                    DataTable dt = dbhelper.getdata(query);
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("audittrail_master_payroll", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "deletepayroll";
                            cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "" + dt.Rows[0]["pg"].ToString() + "";
                            cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = "Delete Payroll";
                            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "" + dt.Rows[0]["datedtrrange"].ToString() + "";
                            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    dbhelper.getdata("update TPayrollOtherDeduction set payroll_id=NULL where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update TPayrollOtherIncome set payroll_id=NULL where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update tdtr set payroll_id=NULL  where payroll_id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update TPayroll set  status='" + "Cancelled-" + DateTime.Now.ToShortDateString() + "' where id=" + row.Cells[0].Text + "");
                    dbhelper.getdata("update govt_deduction_schedule set  status='cancel' where payrollid=" + row.Cells[0].Text + "");
                    Response.Redirect("procpay", false);
                }
                else
                {
                    Response.Write("<script>alert('Cancellation Denied!')</script>");
                }
            }
            else
            { 
            }
        }
    }
    protected void generatetexfile(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string ggg = "select c.ATMAccountNumber Account_No,a.NetIncome Net_Income " +
           "from TPayrollLine a " +
           "left join TPayroll b on a.PayrollId=b.Id " + 
           "left join MEmployee c on a.EmployeeId=c.Id " +
           "where a.PayrollId=" + row.Cells[0].Text + " and len(c.ATMAccountNumber)>0  and a.NetIncome>0 order by b.PayrollDate desc ";
            DataTable dt = dbhelper.getdata(ggg);

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
            string filename =  row.Cells[4].Text.Replace("/", "").Replace(" - ","to")+".txt";
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
    protected void click_export(object sender, EventArgs e)
    {
        //Class2.Export("test.xls", grid_paylist);
        
    }

    //protected void click_post(object sender, EventArgs e)
    //{
    //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
    //    {

    //        if (TextBox1.Text == "Yes")
    //        {
    //            dbhelper.getdata("update tpayroll set status_1='Posted' where id=" + row.Cells[0].Text + "");
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Successfuly Posted!", "location.href='procpay'", true);
    //        }
    //        else { }

    //    }
    //}
    protected void grid_row(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnk_posted = (LinkButton)e.Row.FindControl("lnk_posted");
            if (lnk_posted.Text == "Pending")
                lnk_posted.CssClass = "bg-red";
            else
                //lnk_posted.Attributes.Remove("HREF");
                lnk_posted.CssClass = "bg-blue";
        }

    }

    [WebMethod]
    public static List<Dictionary<string, object>> compute(int pg, int yyyy,decimal per,string clss)
    {
        decimal percentage = per / 100;
        int day = DateTime.DaysInMonth(yyyy, 12);
        string end = "12/"+day+"/"+yyyy;
        string thirteen = "select ID,idnumber,Name,DateHired,monthlyrate,DOY,TD, ";

        if (clss == "Bonus")
            thirteen += "(cast(round(case when convert(decimal,TD) >=convert(decimal,DOY) then monthlyrate else (convert(decimal,TD)/convert(decimal,DOY))* convert(decimal,monthlyrate) end,2)as numeric(18,2))) * " + percentage + " total  ";
        else
            thirteen += "cast(round(case when convert(decimal,TD) >=convert(decimal,DOY) then monthlyrate else (convert(decimal,TD)/convert(decimal,DOY))* convert(decimal,monthlyrate) end,2)as numeric(18,2))  total  ";
       
        
            thirteen += "from(select id, idnumber,lastname+', '+firstname+' '+middlename Name,left(convert(varchar,datehired,101),10)DateHired, monthlyrate, " +
                        "datediff(day,'01/01/" + yyyy + "','" + end + "')DOY, datediff(day,left(convert(varchar,datehired,101),10),'" + end + "')TD " +
                        "from memployee where payrollgroupid=" + pg + ") hh order by Name  ";

        DataTable dt = dbhelper.getdata(thirteen);
        HttpContext.Current.Session["thirteen"] = dt;
        HttpContext.Current.Session["Fourteen"] = dt;
        HttpContext.Current.Session["Bonus"] = dt;
       
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach(DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return rows.ToList();
    }
    [WebMethod]
    public static string save_data_thirteen(int pg, int yyyy, string clasification)
    {
        DataTable cieling_exemption=dbhelper.getdata("select * from Tax_Exemption");
 
            DataTable dt = HttpContext.Current.Session["thirteen"] as DataTable;
            var checking = from a in datacontext.Context.thirteen_mp_trns.Where(x => x.pgid == pg && x.yyyy == yyyy && x.status != "Cancelled" && x.classification == clasification) select a;
            if (checking.Count() == 0)
            {
                thirteen_mp_trn thirteen = new thirteen_mp_trn();
                thirteen.pgid = pg;
                thirteen.yyyy = yyyy;
                thirteen.userid = int.Parse(HttpContext.Current.Session["user_id"].ToString());
                thirteen.sysdate = DateTime.Now;
                thirteen.status = "Pending";
                thirteen.classification = clasification;
                datacontext.Context.thirteen_mp_trns.InsertOnSubmit(thirteen);
                datacontext.Context.SubmitChanges();
                foreach (DataRow dr in dt.Rows)
                {

                    DataTable thirteenlist = dbhelper.getdata("select IsNull(sum(a.total_thirteen),0)total_thirteen from thirteen_mp_trn_child a left join thirteen_mp_trn b on a.trnid=b.trnid where a.status='Active' and b.status!='Cancelled' and b.yyyy=" + yyyy + " and a.empid=" + int.Parse(dr["id"].ToString()) + " ");
                    decimal tax = 0;
                    decimal taxable = 0;
                    decimal nontaxable = 0;
                    decimal total_NTI = thirteenlist.Rows.Count>0?decimal.Parse(thirteenlist.Rows[0]["total_thirteen"].ToString()):0;
                    decimal total_ceiling = cieling_exemption.Rows.Count > 0 ? decimal.Parse(cieling_exemption.Rows[0]["minimum_ceiling"].ToString()) : 0;
                    
                    decimal current_amount = decimal.Parse(dr["total"].ToString().Replace(",", ""));
                    if (total_NTI + current_amount > total_ceiling)
                    {
                        string testing = "";
                    }
                    //check if total Other benifits  is greater than minimum tax exemption will be taxable for those are excess
                    decimal currentceiling= total_ceiling>total_NTI?total_ceiling-total_NTI:0;
                    if (current_amount > currentceiling)
                    {
                        taxable = current_amount - currentceiling;
                        nontaxable = current_amount - taxable;
                    }
                    else {
                        taxable = 0;
                        nontaxable = current_amount;
                    }

                    //decimal divisor = total_NTI + current_amount > total_ceiling ? (total_NTI + current_amount) - total_ceiling : 0;
                    //check if total Other benifits  is greater than minimum tax exemption will be taxable for those are excess
                    //taxable = current_amount > currentceiling ? current_amount - total_ceiling : 0;
                    //nontaxable = currentceiling > current_amount ? currentceiling - current_amount: 0;
                    
                    
                    /////bool iswithheld = double.Parse(cieling_exemption.Rows[0]["minimum_ceiling"].ToString()) > double.Parse(thirteenlist.Rows[0]["total_thirteen"].ToString())?true:false;
                    //decimal tax=0;
                    //if (divisor > 0 )
                    //{
                    //    string query = "select top 1 id,amount,tax,replace(percentage,'.00','')percentage from tax_train where Taxtable='5' and  Amount<='" + divisor + "' order by Amount desc";
                    //    DataTable gettax = dbhelper.getdata(query);

                    //    decimal getpercent = 0;
                    //    decimal amounttax = 0;
                    //    decimal amountcolumn = 0;
                    //    if (gettax.Rows.Count > 0)
                    //    {
                    //        getpercent = decimal.Parse(gettax.Rows[0]["percentage"].ToString());
                    //        amounttax = decimal.Parse(gettax.Rows[0]["tax"].ToString());
                    //        amountcolumn = decimal.Parse(gettax.Rows[0]["amount"].ToString());
                    //    }
                    //    else
                    //    {
                    //        getpercent = 0;
                    //        amounttax = 0;
                    //        amountcolumn = 0;
                    //    }
                    //    getpercent = getpercent.ToString().Length == 1 ? decimal.Parse("0.0" + getpercent) : decimal.Parse("0." + getpercent);
                    //    tax = getpercent * (divisor - amountcolumn) + amounttax;
                    //}
                    
                    thirteen_mp_trn_child thirteen_child = new thirteen_mp_trn_child();
                    thirteen_child.trnid = thirteen.trnid;
                    thirteen_child.empid = int.Parse(dr["id"].ToString());
                    thirteen_child.datehired = DateTime.Parse(dr["DateHired"].ToString());
                    thirteen_child.monthlyrate = decimal.Parse(dr["monthlyrate"].ToString().Replace(",", ""));
                    thirteen_child.total_thirteen = decimal.Parse(dr["total"].ToString().Replace(",", ""));
                    thirteen_child.status = "Active";
                    thirteen_child.tax = tax;
                    thirteen_child.net = decimal.Parse(dr["total"].ToString().Replace(",", "")) - tax;
                    thirteen_child.taxable = taxable;
                    thirteen_child.nontaxable = nontaxable;
                    datacontext.Context.thirteen_mp_trn_childs.InsertOnSubmit(thirteen_child);
                    datacontext.Context.SubmitChanges();
                }
                return "Success";
            }
            else
                return "Existing";
      

    }
    [WebMethod]
    public static List<Dictionary<string, object>> view_thirteen_trn()
    {
        var trnlist = from a in datacontext.Context.thirteen_mp_trns
                      join b in datacontext.Context.MPayrollGroups on a.pgid equals b.Id
                      where a.status != "Cancelled" && a.classification == "Thirteen"
                      select new
                      {
                          trnid = a.trnid,
                          sysdate = a.sysdate.Value.ToLongDateString(),
                          pg = b.PayrollGroup,
                          yyyy = a.yyyy,
                          status = a.status,
                          classs="13"
                      };
        DataTable dt = LINQResultToDataTable(trnlist);
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return rows.ToList();
    }
    [WebMethod]
    public static IEnumerable<object> view_fourteen_trn()
    {

        

        var trnlist = from a in datacontext.Context.thirteen_mp_trns
                      join b in datacontext.Context.MPayrollGroups on a.pgid equals b.Id
                      where a.status != "Cancelled" && a.classification == "Fourteen"

                      select new
                      {
                          trnid = a.trnid,
                          sysdate = a.sysdate.Value.ToLongDateString(),
                          pg = b.PayrollGroup,
                          yyyy = a.yyyy,
                          status = a.status,
                          classs = "14"
                      };
        
        return trnlist;
    }
    [WebMethod]
    public static IEnumerable<object> view_bonus_trn()
    {

        
        var trnlist = from a in datacontext.Context.thirteen_mp_trns
                      join b in datacontext.Context.MPayrollGroups on a.pgid equals b.Id
                      where a.status != "Cancelled" && a.classification == "Bonus"

                      select new
                      {
                          trnid = a.trnid,
                          sysdate = a.sysdate.Value.ToLongDateString(),
                          pg = b.PayrollGroup,
                          yyyy = a.yyyy,
                          status = a.status,
                          classs = "bonus"
                      };
        return trnlist;
    }
    public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
    {
        DataTable dt = new DataTable();
        PropertyInfo[] columns = null;
        if (Linqlist == null) return dt;
        foreach (T Record in Linqlist)
        {
            if (columns == null)
            {
                columns = ((Type)Record.GetType()).GetProperties();
                foreach (PropertyInfo GetProperty in columns)
                {
                    Type colType = GetProperty.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                }
            }

            DataRow dr = dt.NewRow();

            foreach (PropertyInfo pinfo in columns)
            {
                dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                (Record, null);
            }

            dt.Rows.Add(dr);
        }
        return dt;
    }
    [WebMethod]
    public static IEnumerable<object> view_thirteen_trn_details(int trnid)
    {
        var thirteen_det = from a in datacontext.Context.thirteen_mp_trn_childs
                           join b in datacontext.Context.MEmployees on a.empid equals b.Id
                           where a.status == "Active" && a.trnid == trnid
        select new
        {
            rowid=a.id,
            trnid = a.trnid,
            idnumber= b.IdNumber,
            fullname=b.LastName+", "+b.FirstName+" "+b.MiddleName,
            datehired = a.datehired.Value.Month+"/"+a.datehired.Value.Day+"/"+a.datehired.Value.Year,
            monthly = a.monthlyrate,
            total = a.total_thirteen < 0 ? 0 : a.total_thirteen,
            taxable=a.taxable,
            nontaxable=a.nontaxable,
            status = a.status
        };
        //DataTable dt = LINQResultToDataTable(thirteen_det);
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //Dictionary<string, object> row;
        //foreach (DataRow dr in dt.Rows)
        //{
        //    row = new Dictionary<string, object>();
        //    foreach (DataColumn col in dt.Columns)
        //    {
        //        row.Add(col.ColumnName, dr[col]);
        //    }
        //    rows.Add(row);
        //}
        return thirteen_det;
    }
    [WebMethod]
    public static string cancel_tmp_trn(int trnid)
    {
        var trn = datacontext.Context.thirteen_mp_trns.Single(x=>x.trnid==trnid);
        trn.status = "Cancelled";
        datacontext.Context.SubmitChanges();
        return "Success";
    }
    [WebMethod]
    public static string Post_tmp(int trnid)
    {
        var trn = datacontext.Context.thirteen_mp_trns.Single(x => x.trnid == trnid);
        trn.status = "Posted";
        datacontext.Context.SubmitChanges();
        return "Success";
    }
    protected void selection(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from tpayroll where id = " + seriesid.Value + "");

            p_status.Visible = true;
            p_status2.Visible = true;
        }
    }
    protected void closepopup(object sender, EventArgs e)
    {
        p_status.Visible = false;
        p_status2.Visible = false;
    }
    protected void click_post(object sender, EventArgs e)
    {
        if (TextBox1.Text == "Yes")
        {
            dbhelper.getdata("update tpayroll set status_1 = '" + rb_range.SelectedValue + "' where id=" + seriesid.Value + "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Successfuly Posted!", "location.href='procpay'", true);
        }
    }
}