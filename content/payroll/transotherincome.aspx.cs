using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Data.SqlClient;

public partial class content_payroll_transotherincome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Timeout = 68;
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!Page.IsPostBack)
        {
            loadable();
            key.Value = Session["user_id"].ToString();
        }
    }

    protected void loadable()
    {
        string query = "select * from MOtherIncome where action is null and frequencyid=3";
        DataTable dt = dbhelper.getdata(query);

        ddl_type.Items.Clear();
        ddl_type.Items.Add(new ListItem("Select", "0"));
        ddl_income_type.Items.Clear();
        ddl_income_type.Items.Add(new ListItem("Select", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_type.Items.Add(new ListItem(dr["OtherIncome"].ToString(), dr["id"].ToString()));
            ddl_income_type.Items.Add(new ListItem(dr["OtherIncome"].ToString(), dr["id"].ToString()));
        }
        query = "select * from MPayrollGroup where status='1' order by id asc";
        DataTable dtt = dbhelper.getdata(query);
        ddl_payroll_group.Items.Clear();
        ddl_payroll_group.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    protected void click_pgpg(object sender, EventArgs e)
    {
        dtr_range();
    }
    protected void dtr_range()
    {
        string query = "select id,left(convert(varchar,datestart,101),10)datestart,left(convert(varchar,dateend,101),10)dateend from TDTR  where status is null and PayrollGroupId=" + ddl_payroll_group.SelectedValue + " and payroll_id is null order by Id desc";
        DataTable dtover = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dtover.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }

    }
    [WebMethod]
    public static string[] GetEmployee(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            string query = string.Format("select a.id, a.firstname+', '+a.lastname+' '+a.Middlename fullname from MEmployee a left join MPayrollGroup b on a.PayrollGroupId=b.Id where a.idnumber+' '+a.firstname+' '+a.lastname like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}~{1}", reader["id"], reader["fullname"]));
                    //retCategory.Add(reader.GetString(0));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }
    [WebMethod]
    public static List<ListItem> getrange(int pgid)
    {
        var tdtr_list = from a in datacontext.Context.GetTable<TDTR>()
                        where a.status == null && a.PayrollGroupId == pgid && a.payroll_id == null
                        select new
                        {
                            datestart = a.DateStart.Value.Month + "/" + a.DateStart.Value.Day + "/" + a.DateStart.Value.Year,
                            dateend = a.DateEnd.Value.Month + "/" + a.DateEnd.Value.Day + "/" + a.DateEnd.Value.Year,
                            id = a.Id
                        };
        List<ListItem> bb = new List<ListItem>();
        bb.Add(new ListItem("Select", "0"));
        foreach (var data_list in tdtr_list.ToList())
        {
            bb.Add(new ListItem(data_list.datestart + "-" + data_list.dateend, data_list.id.ToString()));
        }
        return bb;
    }
    [WebMethod]
    public static List<Dictionary<string, object>> verifydata(int pgid, int prid, string prtext)
    {
        DataTable dt = getdata.queryotherincome(pgid.ToString(), prid.ToString(), prtext);
        HttpContext.Current.Session["load"] = dt;
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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
    public static string[] computedata(int empid, int incomeid, decimal wh)
    {
        var employee = from a in datacontext.Context.GetTable<MEmployee>() where a.Id == empid select a;
        var income = from a in datacontext.Context.GetTable<MOtherIncome>()
                     join b in datacontext.Context.GetTable<MOtherIncomeTaxRule>() on a.taxruleid equals b.id
                     where a.Id == incomeid
                     select new { incomeid = a.Id, istaxable = b.istaxable };

        var hr = employee.SingleOrDefault().HourlyRate;

        var total = Math.Round(wh * decimal.Parse(hr), 2);

        string nt = income.SingleOrDefault().istaxable == "False" ? total.ToString() : "0.00";
        string t = income.SingleOrDefault().istaxable == "True" ? total.ToString() : "0.00";

        string[] trn = { nt, t };

        return trn;
    }

    public static string count = "0";
    [WebMethod]
    public static string savedata_trans(int pg, int periodid, string remarks)
    {
        DataTable dt = dbhelper.getdata("Select * from TPayrollOtherIncome where PeriodId=" + periodid + " and PayrollGroupId=" + pg + " and action=null ");

        int trnid = 0;
        if (dt.Rows.Count == 0)
        {

            DataTable dtt = dbhelper.getdata("Insert into TPayrollOtherIncome(EntryDate,EntryUserId,PeriodId,PayrollGroupId,remarks,status1)"
                                            + " values(GetDate()," + int.Parse(HttpContext.Current.Session["user_id"].ToString()) + "," + periodid + "," + pg + ",'" + remarks + "','Draft') select scope_identity() id");

            trnid = Convert.ToInt32(dtt.Rows[0]["Id"].ToString());
        }
        count = "0";
        return trnid.ToString();
    }
    [WebMethod]
    public static string save_data(int empid, int incomeid, string Amount, string wh, string nt, string taxable, int transid)
    {
        DataTable dttt = HttpContext.Current.Session["load"] as DataTable;

        TPayrollOtherIncomeLine OIL = new TPayrollOtherIncomeLine();
        OIL.PayOtherIncome_id = transid;
        OIL.Emp_id = empid;
        OIL.OtherIncome_id = incomeid;
        OIL.nontaxable_amt = decimal.Parse(nt.Trim().Replace(",", ""));
        OIL.taxable_amt = decimal.Parse(taxable.Trim().Replace(",", ""));
        OIL.worked_hrs = decimal.Parse(wh.Trim().Replace(",", ""));
        OIL.allowed_amt = 0;

        datacontext.Context.TPayrollOtherIncomeLines.InsertOnSubmit(OIL);
        datacontext.Context.SubmitChanges();
        
        count = (Convert.ToInt32(count) + 1).ToString();
        return count;
    }
    [WebMethod]
    public static string approve_trans(int trnid)
    {
        TPayrollOtherIncome TOI = new TPayrollOtherIncome();
        TOI = datacontext.Context.TPayrollOtherIncomes.Single(x => x.id == trnid);
        TOI.status1 = "Approved";
        TOI.status = "True";
        datacontext.Context.SubmitChanges();
        return "test";
    }
    [WebMethod]
    public static List<Dictionary<string, object>> view_det(int trnid)
    {
        var OITRN = from a in datacontext.Context.GetTable<TPayrollOtherIncome>()
                    join b in datacontext.Context.GetTable<TPayrollOtherIncomeLine>() on a.id equals b.PayOtherIncome_id
                    join c in datacontext.Context.GetTable<MEmployee>() on b.Emp_id equals c.Id
                    join d in datacontext.Context.GetTable<MOtherIncome>() on b.OtherIncome_id equals d.Id
                    join e in datacontext.Context.GetTable<MPayrollGroup>() on a.PayrollGroupId equals e.Id
                    join f in datacontext.Context.GetTable<TDTR>() on a.PeriodId equals f.Id
                    where b.PayOtherIncome_id == trnid && b.status == null
                    select new
                    {
                        pgname = e.PayrollGroup,
                        range = f.DateStart.Value.Month + "/" + f.DateStart.Value.Day + "/" + f.DateStart.Value.Year + " - " + f.DateEnd.Value.Month + "/" + f.DateEnd.Value.Day + "/" + f.DateEnd.Value.Year,
                        remarks = a.remarks,
                        dateentry = a.EntryDate.Value.Month + "/" + a.EntryDate.Value.Day + "/" + a.EntryDate.Value.Year,
                        trn_id_line = b.id,
                        Emp_id = b.Emp_id,
                        emp_name = c.LastName + ", " + c.FirstName + " " + c.MiddleName,
                        OtherIncome_id = b.OtherIncome_id,
                        Amount = b.allowed_amt,
                        OtherIncome = d.OtherIncome,
                        thw = b.worked_hrs,
                        Amt_to_be_tax = b.taxable_amt,
                        amt_to_bepaid = b.nontaxable_amt
                    };

        DataTable dt = LINQResultToDataTable(OITRN);
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
    public static string delete_perline(int rowid)
    {
        TPayrollOtherIncomeLine TOIL = new TPayrollOtherIncomeLine();
        TOIL = datacontext.Context.TPayrollOtherIncomeLines.Single(x => x.id == rowid);
        TOIL.status = "Cancelled";
        datacontext.Context.SubmitChanges();
        return "Success";
    }

    //with json serialiser and deserialized
    [WebMethod]
    public static List<Dictionary<string, object>> loadexcel(string arr, int incomtype, int transid)
    {

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;


        var income = from a in datacontext.Context.GetTable<MOtherIncome>()
                     join b in datacontext.Context.GetTable<MOtherIncomeTaxRule>() on a.taxruleid equals b.id
                     where a.Id == incomtype
                     select new { incomeid = a.Id, istaxable = b.istaxable, OINAME = a.OtherIncome };

        var serializer = new JavaScriptSerializer();


        var s = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<myObject> obj = s.Deserialize<List<myObject>>(arr);
        var oistatus = from a in datacontext.Context.GetTable<TPayrollOtherIncome>() where a.id == transid select a;

        if (incomtype > 0 && oistatus.SingleOrDefault().status1 == "Draft")
        {
            foreach (var dr in obj.ToList())
            {
                row = new Dictionary<string, object>();
                var employee = from a in datacontext.Context.GetTable<MEmployee>() where a.IdNumber == dr.IDNumber select a;
                var idnumebr = dr.IDNumber;
                var wh = dr.WorkedHrs;
                var amount = dr.Amount;
                string nt = income.Count() > 0 ? income.SingleOrDefault().istaxable == "False" ? amount.ToString() : "0.00" : "0.00";
                string t = income.Count() > 0 ? income.SingleOrDefault().istaxable == "True" ? amount.ToString() : "0.00" : "0.00";
                try
                {
                    TPayrollOtherIncomeLine OIL = new TPayrollOtherIncomeLine();
                    OIL.PayOtherIncome_id = transid;
                    OIL.Emp_id = employee.SingleOrDefault().Id;
                    OIL.OtherIncome_id = incomtype;
                    OIL.nontaxable_amt = decimal.Parse(nt);
                    OIL.taxable_amt = decimal.Parse(t);
                    OIL.worked_hrs = wh;
                    OIL.allowed_amt = 0;
                    OIL.timailhankunggiload = "loaded";

                    datacontext.Context.TPayrollOtherIncomeLines.InsertOnSubmit(OIL);
                    datacontext.Context.SubmitChanges();

                    row.Add("Emp_id", employee.SingleOrDefault().Id);
                    row.Add("PayOtherIncome_id", incomtype);
                    row.Add("Emp_Name", employee.SingleOrDefault().LastName + ", " + employee.SingleOrDefault().FirstName + " " + employee.SingleOrDefault().ExtensionName);
                    row.Add("incometypename", income.SingleOrDefault().OINAME);
                    row.Add("nontaxable_amt", nt);
                    row.Add("taxable_amt", t);
                    row.Add("worked_hrs", wh);
                    row.Add("allowed_amt", 0);
                    row.Add("trn_id_line", transid);
                    rows.Add(row);

                }
                catch (Exception)
                { }
            }

        }
        else
        {
            row = new Dictionary<string, object>();
            row.Add("error", "err");
            rows.Add(row);
        }
        return rows.ToList();
    }
    public class myObject
    {
        public string IDNumber { get; set; }
        public decimal WorkedHrs { get; set; }
        public decimal Amount { get; set; }
    }

    [WebMethod]
    public static string backtodraft(int trnid)
    {
        TPayrollOtherIncome TOIL = new TPayrollOtherIncome();
        TOIL = datacontext.Context.TPayrollOtherIncomes.Single(x => x.id == trnid);
        TOIL.status1 = "Draft";
        TOIL.status = null;
        datacontext.Context.SubmitChanges();
        return "Success";
    }
    [WebMethod]
    public static string cancelled_trn(int trnid)
    {
        //Cancelled-0-2/27/2020
        string nn = "Cancelled-" + HttpContext.Current.Session["user_id"] + "-" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
        TPayrollOtherIncome TOIL = new TPayrollOtherIncome();
        TOIL = datacontext.Context.TPayrollOtherIncomes.Single(x => x.id == trnid);
        TOIL.action = nn;
        datacontext.Context.SubmitChanges();
        return "Success";
    }

}