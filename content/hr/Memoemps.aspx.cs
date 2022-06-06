using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Mail;

public partial class content_hr_Memoemps : System.Web.UI.Page
{
    public string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            loadable();
        if (Request.QueryString["empid"] != null)
            dispcontent();
    }
    protected void loadable()
    {
        query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);

        foreach (DataRow dr in dtt.Rows)
        {
            ddl_dtrfile.Items.Add(new ListItem(dr["Fullname"].ToString(),dr["id"].ToString()));
        }

        DataTable dt = dbhelper.getdata("select * from MPayrollGroup order by id asc");
        ddl_payroll_group.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        DataTable dt1 = dbhelper.getdata("select * from MBranch order by id asc");
        drop_branch.Items.Clear();
        foreach (DataRow dr in dt1.Rows)
        {
            drop_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["Id"].ToString()));
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (txt_date.Text.Length > 0)
        {
            string filename = null;
            stateclass a = new stateclass();

            a.sa = txt_memo_from.Text;
            a.sb = txt_subject.Text;
            a.sc = txt_date.Text;
            a.sd = HttpUtility.HtmlDecode(txt_description.Text);
            a.se = txt_notes.Text;
            string x = bol.memo(a);

            if (all.Visible == true)
            {
                a.sa = x;
                a.sb = ddl_dtrfile.SelectedValue;
                bol.memo_line(a);
            }
            if (payrollgroup.Visible == true)
            {
                DataTable dt = dbhelper.getdata("select Id,* from MEmployee where PayrollGroupId=" + ddl_payroll_group.SelectedValue + "");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    a.sa = x;
                    a.sb = dt.Rows[i]["Id"].ToString();
                    bol.memo_line(a);
                }
            }
            if (branch.Visible == true)
            {
                DataTable dt = dbhelper.getdata("select Id,* from MEmployee where BranchId=" + drop_branch.SelectedValue + " ");
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    a.sa = x;
                    a.sb = dt.Rows[i]["Id"].ToString();
                    bol.memo_line(a);
                }
            }

            ////images / videos
            if (file_upload.HasFile)
            {
                string filepath = Server.MapPath("files/memo/" + x + "/");
                DirectoryInfo di = Directory.CreateDirectory(filepath);
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];

                    if (userPostedFile.ContentLength > 0)
                    {
                        string extention = Path.GetExtension(userPostedFile.FileName);

                        filename = filepath + "\\" + Path.GetFileName(x);
                        userPostedFile.SaveAs(filepath + "\\" + Path.GetFileName(txt_subject.Text.Replace("'", "") + "-" + i + extention));

                        a.sa = x;
                        a.sb = txt_subject.Text.Replace("'", "") + "-" + i + extention;
                        if (extention != ".jpg" &&
                            extention != ".jpeg" &&
                            extention != ".pjpeg" &&
                            extention != ".gif" &&
                            extention != ".png" &&
                            extention != ".tif")
                        {
                            a.sc = "docs";
                        }
                        else
                        {
                            a.sc = "image";
                        }
                        bol.memoAttachments(a);
                    }
                }
            }
            if (Request.QueryString["empid"] != null)
            {
                dbhelper.getdata("update sanctionentry set status = 'For Clearing' where id = " + Request.QueryString["entid"].ToString() + " ");
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='sanction'", true);
        }
        else
            Response.Write("<script>alert('Date must be required!')</script>");
    }


    protected void click_filter(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }

    protected void cpop(object sender, EventArgs e)
    {
        Div1.Visible = false;
        Div2.Visible = false;
    }

    protected void click_save(object sender, EventArgs e)
    {

        if (memo_to.SelectedValue == "1")
        {
            payrollgroup.Visible = true;
            employee.Visible = false;
            all.Visible = false;
            branch.Visible = false;
        }
        else
            payrollgroup.Visible = false;

        if (memo_to.SelectedValue == "2")
        {
            employee.Visible = false;
            payrollgroup.Visible = false;
            all.Visible = true;
            branch.Visible = false;

        }
        else
            employee.Visible = false;

        if (memo_to.SelectedValue == "3")
        {
            branch.Visible = true;
            all.Visible = false;
            payrollgroup.Visible = false;
            employee.Visible = false;
        }
        else
            branch.Visible = false;
        Div1.Visible = false;
        Div2.Visible = false;
    }
    protected void dispcontent()
    {
        DataTable dt = dbhelper.getdata("select id, empid, LEFT(convert(varchar, dateapply,101),10) as dateapply, rootcause, (select LastName+', '+FirstName+' '+MiddleName from MEmployee where id = a.empid) as Name, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status, (select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays, (select sanction from sanctioncodes where id =a.sanctioncode )as sanid from sanctionentry a where id = " + Request.QueryString["entid"].ToString() + "");
        if (dt.Rows.Count > 0)
        {
            txt_date.Text = dt.Rows[0]["dateapply"].ToString();
            txt_subject.Text = dt.Rows[0]["rootcause"].ToString();
            txt_memo_from.Text = "HR Department";
            ddl_dtrfile.SelectedValue = dt.Rows[0]["empid"].ToString();
            txt_description.Text = "You" + " " + dt.Rows[0]["Name"].ToString() + " is Subject for " + dt.Rows[0]["sanid"].ToString() + " due to " + dt.Rows[0]["rootcause"].ToString() + " happened last " + dt.Rows[0]["incidentdate"].ToString() + ", Number Days of Suspension: " + dt.Rows[0]["nodays"].ToString() + " " + "Day/s";
        }
    }
}