using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_addshiftcodelist : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
            disp();
            testt();
            grid_view.DataBind();

        }
    }
    protected void lodable()
    {
        string query = "select * from MPayrollGroup where payrollgroup<>'Resigned' order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_payroll.Items.Clear();
        ddl_payroll.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select * from MShiftCode where status is null order by id desc";
        dt = dbhelper.getdata(query);

        ddl_shiftcode.Items.Clear();
        ddl_shiftcode.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_shiftcode.Items.Add(new ListItem(dr["shiftcode"].ToString(), dr["id"].ToString()));
        }
    }
   

    protected void testt()
    {
        DataTable dtp = new DataTable();
        DataRow drp = null;

        dtp.Columns.Add(new DataColumn("emp_id", typeof(string)));
        dtp.Columns.Add(new DataColumn("employee", typeof(string)));
        dtp.Columns.Add(new DataColumn("DATE", typeof(string)));
    
        //dtp.Columns.Add(new DataColumn("remarks", typeof(string)));
        dtp.Columns.Add(new DataColumn("shiftcode", typeof(string)));
        dtp.Columns.Add(new DataColumn("shiftcode_id", typeof(string)));
       // dtp.Columns.Add(new DataColumn("DATEto", typeof(string)));

        drp = dtp.NewRow();

        //drp["emp_id"] = string.Empty;
        //drp["employee"] = string.Empty;
        //drp["DATE"] = string.Empty;
        //drp["OTH"] = string.Empty;
        //drp["OTNH"] = string.Empty;
        //drp["OTLH"] = string.Empty;
        //drp["remarks"] = string.Empty;


        dtp.Rows.Add(drp);
        ViewState["Item_List1_deb"] = dtp;

    }
    protected void click_pg(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {
        string query = "select b.Id,b.lastname+', '+b.firstname+' '+ b.middlename+' '+b.extensionname as Fullname from Approver a " +
                      "left join MEmployee b on a.emp_id=b.id " +
                      "where a.status is null and  a.under_id=" + Session["emp_id"] + " and b.payrollgroupid<>'9'";
                          

        DataTable dt = dbhelper.getdata(query);
        //ddl_payroll.SelectedValue = ddl_payroll.SelectedValue;
        ddl_employee.Items.Clear();
        ddl_employee.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_employee.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }
    }
    protected void add_datatable(object sender, EventArgs e)
    {
        if (chk())
        {
            DataTable dtCurrentTablep = (DataTable)ViewState["Item_List1_deb"];
            DataRow drp = null;
            DateTime datef = Convert.ToDateTime(txt_csd.Text);
            TimeSpan nod = DateTime.Parse(txt_to.Text) - DateTime.Parse(txt_csd.Text);
            string nodformat = nod.TotalDays.ToString();
            for (int i = 0; i <= int.Parse(nodformat); i++)
            {
                string[] f_datef = datef.AddDays(i).ToString().Trim().Split(' ');
                string[] getdate = f_datef[0].Trim().Split('/');

               string month = getdate[0].Length > 1 ? getdate[0] : "0" + getdate[0];
               string day = getdate[1].Length > 1 ? getdate[1] : "0" + getdate[1];

               string dayformat = month + "/" + day + "/" + getdate[2];

                DataRow[] chk_dtrow = dtCurrentTablep.Select("emp_id=" + ddl_employee.SelectedValue + " and date='" + dayformat + "'");
                if (chk_dtrow.Count() == 0)
                {
                    DataTable check = dbhelper.getdata("select * from TChangeShiftLine where left(convert(varchar,date,101),10)='" + dayformat + "' and employeeid=" + ddl_employee.SelectedValue + " and status='Approved'");
                    if (check.Rows.Count == 0)
                    {
                        if (dtCurrentTablep.Rows[0][0].ToString() == string.Empty)
                        {
                            dtCurrentTablep.Rows.Remove(dtCurrentTablep.Rows[0]);
                        }
                        drp = dtCurrentTablep.NewRow();

                        drp["emp_id"] = ddl_employee.SelectedValue;
                        drp["employee"] = ddl_employee.SelectedItem;
                        drp["DATE"] = dayformat;
                        drp["shiftcode"] = ddl_shiftcode.SelectedItem;
                        drp["shiftcode_id"] = ddl_shiftcode.SelectedValue;
                        dtCurrentTablep.Rows.Add(drp);

                    }
                }
                ViewState["Item_List1_deb"] = dtCurrentTablep;
                grid_view.DataSource = dtCurrentTablep;
                grid_view.DataBind();

                if (grid_view.Rows.Count > 0)
                    Button2.Visible = true;
                }
        }
    }
    protected void delete_row_datatable(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            DataTable dt = (DataTable)ViewState["Item_List1_deb"];
            if (grid_view.Rows.Count == 1)
            {
                dt.Rows.Remove(dt.Rows[row.RowIndex]);
                testt();
            }
            else
            {
                dt.Rows.Remove(dt.Rows[row.RowIndex]);
            }
            grid_view.DataSource = dt;
            grid_view.DataBind();
      
        }


    }
    protected void click_save_changeshift(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from mperiod where period='" + DateTime.Now.Year + "'");
        DataTable dtcnt = dbhelper.getdata("select * from TChangeShift");
        DataTable dtinsert = dbhelper.getdata("insert into TChangeShift (PeriodId,csnumber,csdate,Remarks,EntryUserId,EntryDateTime,UpdateUserId,UpdateDateTime,status) values (" + dt.Rows[0]["id"].ToString() + "," + dtcnt.Rows.Count + 50000 + ",getdate(),'" + txt_remarks.Text + "'," + Session["emp_id"].ToString() + ",GETDATE()," + Session["emp_id"].ToString() + ",GETDATE(),'Approved') select scope_identity() id ");
        foreach (GridViewRow gr in grid_view.Rows)
        {
            string test = gr.Cells[2].Text;
            dbhelper.getdata("insert into TChangeShiftLine(ChangeShiftId,EmployeeId,Date,ShiftCodeId,status) " +
                             "values " +
                             "(" + dtinsert.Rows[0]["id"].ToString() + "," + gr.Cells[0].Text + ",'" + gr.Cells[2].Text + "','" + gr.Cells[4].Text + "','Approved')");
        }
        Response.Redirect("addchangeshift&pg="+function.Encrypt(ddl_payroll.SelectedValue,true)+"", false);
    }
    protected bool chk()
    {
        bool err = true;
        DateTime from=Convert.ToDateTime(txt_csd.Text);
        DateTime to=Convert.ToDateTime(txt_to.Text);
        //if (ddl_payroll.SelectedValue == "0")
        //{
        //    Response.Write("<script>alert('Invalid Payroll Group!')</script>");
        //    err = false;
        //}
        //else 
        if (ddl_employee.SelectedValue == " ")
        {
            Response.Write("<script>alert('Invalid Employee!')</script>");
            err = false;
        }
        else if (txt_csd.Text.Length == 0)
        {
            Response.Write("<script>alert('Invalid Date!')</script>");
            err = false;
        }
        else if (ddl_shiftcode.SelectedValue == "0")
        {
            Response.Write("<script>alert('Invalid Shift Code!')</script>");
            err = false;
        }
        else if (from > to)
        {
            Response.Write("<script>alert('Invalid Date!')</script>");
            err = false;
        }
        //else if (txt_lineremarks.Text.Length == 0)
        //{
        //    Response.Write("<script>alert('Invalid Remarks!')</script>");
        //    err = false;
        //}
       
        return err;
    }
    
    
}