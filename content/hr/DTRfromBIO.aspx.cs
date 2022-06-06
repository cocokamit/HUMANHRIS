using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using System.Drawing;

public partial class content_hr_DTRfromBIO : System.Web.UI.Page
{
    void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["role"].ToString() != "Admin")
            this.MasterPageFile = "~/content/site.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            lodable();
            disp();
        }
    }
    protected void lodable()
    {
        string query = "select * from MPayrollGroup order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        query = "select * from Mstore order by id desc";
        dt = dbhelper.getdata(query);
        ddl_store.Items.Clear();
        ddl_store.Items.Add(new ListItem(" ", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_store.Items.Add(new ListItem(dr["store"].ToString(), dr["id"].ToString()));
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select a.id,left(convert(varchar,a.sysdate,101),10)sysdate, " +
                                        "c.store " +
                                        "from tdtrperpayrol a " +
                                        "left join MPayrollGroup b on a.PayrollGroupId=b.Id " +
                                        "left join Mstore c on a.store_id=c.id " +
                                        "where a.status like '%Approved%' order by a.id desc");
        griddtrlist.DataSource = dt;
        griddtrlist.DataBind();
    }

    protected void search(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select a.id,left(convert(varchar,a.sysdate,101),10)sysdate from tdtrperpayrol a left join MPayrollGroup b on a.PayrollGroupId=b.Id  where a.status like '%Approved%' and left(convert(varchar,a.datestart,101),10)='" + txt_from.Text + "' and left(convert(varchar,a.dateend,101),10)='" + txt_to.Text + "' ");
        griddtrlist.DataSource = dt;
        griddtrlist.DataBind();
    }

    protected void newdtrlogs(object sender, EventArgs e)
    {
        ppop(true);
    }

    protected void close(object sender,EventArgs e)
    {
        Response.Redirect("adddtrlogs");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
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
    protected bool chk()
    {
        bool err = true;
        if (!FileUpload1.HasFile)
        {
            lbl_ef.Text = "*";
            err = false;
        }
        else
        {
            lbl_ef.Text = "";
        }
        return err;
    }
    protected void click_file(object sender, EventArgs e)
    {
        try
        {
            if (chk())
            {

                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
                FileUpload1.SaveAs(path);
                string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                OleDbConnection MyConnection = new OleDbConnection();
                MyConnection.ConnectionString = excelConnectionString;
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
                MyCommand.TableMappings.Add("Table", "TestTable");
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet);
                MyConnection.Close();
                DataTable ins = dbhelper.getdata("insert into tdtrperpayrol (sysdate,status,store_id)values(getdate(),'Approved','" + ddl_store.SelectedValue + "') select scope_identity() id");
                try
                {
                    foreach (DataRow gr in DtSet.Tables[0].Rows)
                    {
                        DataTable getdtemp = dbhelper.getdata("select * from Memployee where idnumber='" + gr["Idnumber"].ToString() + "'");
                        string checktype = "NULL";
                        //if (gr["Status"].ToString() == "C/In")
                        //    checktype = "I";
                        //else if (gr["Status"].ToString() == "Out")
                        //    checktype = "0";
                        //else if (gr["Status"].ToString() == "Out Back")
                        //    checktype = "1";
                        //else if (gr["Status"].ToString() == "C/Out")
                        //    checktype = "O";
                        //string query = "select * from tdtrperpayrolperline where empid=" + getdtemp.Rows[0]["id"].ToString() + " and Biotime=convert(datetime,'" + gr["Date/Time"] + "')";
                        //DataTable dtchkdtrline = dbhelper.getdata(query);
                        //if (dtchkdtrline.Rows.Count == 0)
                        //{
                        if (getdtemp.Rows.Count > 0)
                        {
                            dbhelper.getdata("insert into tdtrperpayrolperline (dtrperpayrol_id,empid,idnumber,Biotime,deviceid,checktype)values(" + ins.Rows[0]["id"].ToString() + "," + getdtemp.Rows[0]["id"].ToString() + "," + gr["Idnumber"].ToString() + ",'" + gr["Date/Time"].ToString() + "',0,NULL)");
                        }
                        //}
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='adddtrlogs'", true);
                }
                catch
                {
                    dbhelper.getdata("update tdtrperpayrol set status='Cancel' where id=" + ins.Rows[0]["id"].ToString() + "");
                    lbl_error.Text = "ERROR! Pls. contact Administrator.";
                }
            }
        }
        catch
        { }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkcan = (LinkButton)e.Row.FindControl("lnkcan");
            DataTable dt = dbhelper.getdata("select *,case when dtr_id is null then '0' else '1' end dtrid from tdtrperpayrol where id=" + lnkcan.CommandName + "");
            if (dt.Rows[0]["dtrid"].ToString() == "1")
            {
                lnkcan.Enabled = false;
                lnkcan.OnClientClick = "";
            }
        }
    }
    protected void click_cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                LinkButton lnkcan = (LinkButton)row.FindControl("lnkcan");
                dbhelper.getdata("update tdtrperpayrol set status='Cancel' where Id=" + lnkcan.CommandName + " ");
                //dbhelper.getdata("delete from tdtrperpayrolperline where dtrperpayrol_id=" + lnkcan.CommandName + " ");
                Response.Redirect("adddtrlogs?user_id=" + function.Encrypt(key.Value, true) + "", false);
            }
            else
            {
            }
        }
    }

    protected void click_view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)sender;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='viewtrlogs?&logsid=" + function.Encrypt(lb.CommandName, true) + "'", true);
        }
    }
}
