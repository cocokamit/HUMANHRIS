using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class content_Employee_change_shift : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");

        if (!IsPostBack)
        {
           
            testt();
            grid_view.DataBind();
            yow();
          
        }
       
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

    protected void testt()
    {
        DataTable dtp = new DataTable();
        DataRow drp = null;

        dtp.Columns.Add(new DataColumn("id", typeof(string)));
        dtp.Columns.Add(new DataColumn("emp_id", typeof(string)));
        dtp.Columns.Add(new DataColumn("employee", typeof(string)));
        dtp.Columns.Add(new DataColumn("DATE", typeof(string)));
        dtp.Columns.Add(new DataColumn("shiftcode", typeof(string)));
        dtp.Columns.Add(new DataColumn("shiftcode_id", typeof(string)));

        drp = dtp.NewRow();
        
        dtp.Rows.Add(drp);
        ViewState["Item_List1_deb"] = dtp;

    }

    protected void yow()
    {
        string query1 = "select id,left(convert(varchar,date_start,101),10)datestart,left(convert(varchar,date_end,101),10)dateend from dtr_period where id='" + Request.QueryString["key"].ToString() + "'";
        DataTable dtover1 = dbhelper.getdata(query1);


        DataTable dtCurrentTablep = (DataTable)ViewState["Item_List1_deb"];

        DataRow drp = null;
        DateTime datef = Convert.ToDateTime(dtover1.Rows[0]["datestart"].ToString());
        TimeSpan nod = DateTime.Parse(dtover1.Rows[0]["dateend"].ToString()) - DateTime.Parse(dtover1.Rows[0]["datestart"].ToString());
        string nodformat = nod.TotalDays.ToString();
        for (int i = 0; i <= int.Parse(nodformat); i++)
        {
            string[] f_datef = datef.AddDays(i).ToString().Trim().Split(' ');
            string[] getdate = f_datef[0].Trim().Split('/');

            string month = getdate[0].Length > 1 ? getdate[0] : "0" + getdate[0];
            string day = getdate[1].Length > 1 ? getdate[1] : "0" + getdate[1];

            string dayformat = month + "/" + day + "/" + getdate[2];

            DataRow[] chk_dtrow = dtCurrentTablep.Select("emp_id=" + Session["emp_id"].ToString() + " and date='" + dayformat + "'");
            if (chk_dtrow.Count() == 0)
            {
            
                //DataTable check = dbhelper.getdata("select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                //                                "SUBSTRING(c.ShiftCode,0,CHARINDEX('(',c.ShiftCode)) As shiftcode, " +
                //                                "b.id,b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId " +
                //                                "from TChangeShift a " +
                //                                "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                //                                "left join MShiftCode c on b.ShiftCodeId=c.id " +
                //                                "left join memployee d on b.EmployeeId=d.Id " +
                //                                "where left(convert(varchar,b.date,101),10)='" + dayformat + "' and b.employeeid='" + Session["emp_id"].ToString() + "'");

                DataTable check = dbhelper.getdata("select left(convert(varchar,b.date,101),10) date, d.IdNumber , " +
                                               "c.ShiftCode As shiftcode, " +
                                               "b.id,b.ShiftCodeId,b.EmployeeId,b.date,a.EntryUserId " +
                                               "from TChangeShift a " +
                                               "left join TChangeShiftLine b on a.Id=b.ChangeShiftId " +
                                               "left join MShiftCode c on b.ShiftCodeId=c.id " +
                                               "left join memployee d on b.EmployeeId=d.Id " +
                                               "where left(convert(varchar,b.date,101),10)='" + dayformat + "' and b.employeeid='" + Session["emp_id"].ToString() + "'");


                if (dtCurrentTablep.Rows[0][0].ToString() == string.Empty)
                {
                    dtCurrentTablep.Rows.Remove(dtCurrentTablep.Rows[0]);
                }
                drp = dtCurrentTablep.NewRow();
                if (check.Rows.Count != 0)
                {

                    drp["id"] = check.Rows[0]["id"].ToString();
                    drp["emp_id"] = Session["emp_id"].ToString();
                    drp["employee"] ="test";
                    drp["DATE"] = dayformat;
                    drp["shiftcode"] = check.Rows[0]["shiftcode"].ToString(); //ddl_shiftcode.SelectedItem;
                    drp["shiftcode_id"] = check.Rows[0]["ShiftCodeId"].ToString();  //ddl_shiftcode.SelectedValue;
                    dtCurrentTablep.Rows.Add(drp);
                }
            }

        }
        ViewState["Item_List1_deb"] = dtCurrentTablep;
        grid_view.DataSource = dtCurrentTablep;
        grid_view.DataBind();


      
    }

    //protected void delete_row_datatable(object sender, EventArgs e)
    //{
    //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
    //    {
    //        DataTable dt = (DataTable)ViewState["Item_List1_deb"];
    //        if (grid_view.Rows.Count == 1)
    //        {
    //            dt.Rows.Remove(dt.Rows[row.RowIndex]);
    //            testt();
    //        }
    //        else
    //        {
    //            dt.Rows.Remove(dt.Rows[row.RowIndex]);
    //        }
    //        grid_view.DataSource = dt;
    //        grid_view.DataBind();

    //    }


    //}
    protected void click_save_changeshift(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * from temp_shiftcode where changeshift_id=" + key.Value + " and status not like '%Cancel%' order by date_change ");

        if (dt.Rows.Count == 0)
        {
           // DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");
            DataTable dtinsertcs=  dbhelper.getdata("insert into temp_shiftcode (date_change,emp_id,changeshift_id,shiftcode_id,status,remarks,approver_id) values (getdate(),'" + Session["emp_id"].ToString() + "'," + key.Value + "," + ddl_shiftcode.SelectedValue + ",'For Approval','" + txt_remarks.Text.Replace("'","") + "','" + approver_id.Rows[0]["under_id"].ToString() + "') select scope_identity() id ");
            function.AddNotification("Change Shift Approval", "acs", approver_id.Rows[0]["under_userid"].ToString(), dtinsertcs.Rows[0]["id"].ToString());
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='cs?key=" + Request.QueryString["key"].ToString() + "'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Shift Code Already Change'); window.location='cs?key=" + Request.QueryString["key"].ToString() + "'", true);
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            loadable();

            key.Value = row.Cells[0].Text;
            txt_date.Text = row.Cells[2].Text;
            ddl_shiftcode.SelectedValue = row.Cells[4].Text;
          

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
        Response.Redirect("cs?key=" + Request.QueryString["key"].ToString() + "", false);
    }
    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();



            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    
}