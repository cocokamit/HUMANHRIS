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
using System.Net.Mime;

public partial class content_hr_memo : System.Web.UI.Page
{
    public string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (!IsPostBack)
            loadable();
        if (Request.QueryString["empid"] != null)
            dispcontent();


        modal.Style.Add("display", "none");
    }

    protected void loadable()
    {
        query = "select Id,lastname+', '+firstname+' '+ middlename+' '+ extensionname as Fullname from MEmployee order by lastname,firstname asc";
        DataTable dtt = new DataTable();
        dtt = dbhelper.getdata(query);

      //ddl_dtrfile.Items.Add(" ");
        foreach (DataRow dr in dtt.Rows)
        {
          ddl_dtrfile.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }

        DataTable dt = dbhelper.getdata("select * from MPayrollGroup where status <> 0 order by PayrollGroup asc");
        ddl_payroll_group.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            ddl_payroll_group.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }

        DataTable dt1 = dbhelper.getdata("select * from MBranch order by Branch asc");
        drop_branch.Items.Clear();
        foreach (DataRow dr in dt1.Rows)
        {
            drop_branch.Items.Add(new ListItem(dr["Branch"].ToString(), dr["Id"].ToString()));
        }


      
    }

    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected bool checkField()
    {
        lblRDate.Text = txt_date.Text.Replace(" ", "").Length == 0 ? "*" : string.Empty;

        string required = lblRDate.Text;

        return required.Contains("*");

    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        lbox_notsent.Items.Clear();
        lbox_sent.Items.Clear();
        string filename = null;
        if ( !checkField())
        {
          
            stateclass a = new stateclass();

            a.sa = txt_memo_from.Text;
            a.sb = txt_subject.Text;
            a.sc = txt_date.Text;
            a.sd = HttpUtility.HtmlDecode(txt_description.Text);
            a.se = txt_notes.Text;
            a.sf = "";
            string x = bol.memo(a);
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

            if (all.Visible == true)
            {
                a.sa = x;
                a.sb = ddl_dtrfile.SelectedValue;
                bol.memo_line(a);
                DataTable dt = dbhelper.getdata("select Id,* from MEmployee where id=" + ddl_dtrfile.SelectedValue + "");
                if (chkmailer.Checked)
                {
                    if (IsValidEmail(dt.Rows[0]["EmailAddress"].ToString()))
                    {
                      Emailer(dt.Rows[0]["EmailAddress"].ToString(), dt.Rows[0]["lastname"].ToString() + ", " + dt.Rows[0]["firstname"].ToString(), txt_subject.Text, HttpUtility.HtmlDecode(txt_description.Text), txt_date.Text, txt_memo_from.Text);
                        lbox_sent.Items.Add(dt.Rows[0]["lastname"].ToString() + ", " + dt.Rows[0]["firstname"].ToString());
                    }
                    else
                    {
                        lbox_notsent.Items.Add(dt.Rows[0]["lastname"].ToString() + ", " + dt.Rows[0]["firstname"].ToString());
                    }
                }
            }
            if (payrollgroup.Visible == true)
            {
                DataTable dt = dbhelper.getdata("select Id,* from MEmployee where PayrollGroupId=" + ddl_payroll_group.SelectedValue + "");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    a.sa = x;
                    a.sb = dt.Rows[i]["Id"].ToString();
                    bol.memo_line(a);
                    if (chkmailer.Checked)
                    {
                        if (IsValidEmail(dt.Rows[i]["EmailAddress"].ToString()))
                        {
                            Emailer(dt.Rows[i]["EmailAddress"].ToString(), dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString(), txt_subject.Text, HttpUtility.HtmlDecode(txt_description.Text), txt_date.Text, txt_memo_from.Text);
                            lbox_sent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                        else
                        {
                            lbox_notsent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                    }
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
                    if (chkmailer.Checked)
                    {
                        if (IsValidEmail(dt.Rows[i]["EmailAddress"].ToString()))
                        {
                            Emailer(dt.Rows[i]["EmailAddress"].ToString(), dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString(), txt_subject.Text, HttpUtility.HtmlDecode(txt_description.Text), txt_date.Text, txt_memo_from.Text);
                            lbox_sent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                        else
                        {
                            lbox_notsent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                    }
                }
            }

            if (allemployee.Visible == true)
            {
                DataTable dt = dbhelper.getdata("select Id,* from MEmployee ");
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    a.sa = x;
                    a.sb = dt.Rows[i]["Id"].ToString();
                    bol.memo_line(a);
                    if (chkmailer.Checked)
                    {
                        if (IsValidEmail(dt.Rows[i]["EmailAddress"].ToString()))
                        {
                            Emailer(dt.Rows[i]["EmailAddress"].ToString(), dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString(), txt_subject.Text, HttpUtility.HtmlDecode(txt_description.Text), txt_date.Text, txt_memo_from.Text);
                            lbox_sent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                        else
                        {
                            lbox_notsent.Items.Add(dt.Rows[i]["lastname"].ToString() + ", " + dt.Rows[i]["firstname"].ToString());
                        }
                    }
                }
            }

            if (Request.QueryString["empid"] != null)
            {
                dbhelper.getdata("update sanctionentry set status = 'For Clearing' where id = " + Request.QueryString["entid"].ToString() + " ");
            }
            if (lbox_notsent.Items.Count > 0 || lbox_sent.Items.Count > 0)
            {
                modal.Style.Add("display", "block");
                alert1.Visible = lbox_sent.Items.Count == 0 ? true : false;
                alert2.Visible = lbox_notsent.Items.Count == 0 ? true : false;

                lbox_sent.Visible =lbox_sent.Items.Count==0?false:true;
                
                lbox_notsent.Visible =lbox_notsent.Items.Count==0?false:true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully'); window.location='MemoList'", true);
            }
        }
        else
            Response.Write("<script>alert('Date is required!')</script>");

        //Emailer("cocokamit@gmail.com", "cocokster", "test subject", "test description", "11/28/2019", "cocokstersx", uploadedFiles);

    }

    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", " none");
        txt_date.Text = "";
        txt_subject.Text = "";
        txt_memo_from.Text = "";
        txt_description.Text = "";
        file_upload.Dispose();
     }
    protected void emailenabled()
    {
        
    }



    protected void Emailer(string emailadd,string name,string subject,string body,string datee,string from)
    {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("human.hris@gmail.com", "Infra183");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress("human.hris@gmail.com", "HUMAN HRIS");
                mail.To.Add(new MailAddress("human.hris@gmail.com"));
                mail.CC.Add(new MailAddress(emailadd));
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "<html xmlns='http://www.w3.org/1999/xhtml'><head><title></title></head><body><br /><br /><div style='border-top: 3px solid #22BCE5'>&nbsp;</div><span style='font-family: Arial; font-size: 10pt'>Hello <b>" + name + "</b>,<br /><br />Date: " + datee + "<br /><br /><strong><h3><label>" + subject + "</label></h3></strong><br />" + body + "<br /><br />Thanks,<br />" + from + " </span></body></html>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                DataTable dt = dbhelper.getdata("Select *,CONVERT(varchar, a.memo_id)+ '\\' +a.filename as fck from memo_attachments a left join Tmemo b on b.id=a.memo_id where b.id=(Select MAX(id)[id] from Tmemo)");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rows in dt.Rows)
                    {
                        Attachment data = new Attachment(Server.MapPath("~/files/memo/" + rows["fck"].ToString() + ""), MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }
                }
                            
              
                mail.Priority = MailPriority.High;
                smtpClient.Send(mail);
     
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
       // check_date.Checked = false;
        //check_loc.Checked = false;
    }

    protected void click_save(object sender, EventArgs e)
    {
        //if (cb_emp.Checked)
        //{
        //    employee.Visible = true;
           
        //}
        //else
        //    employee.Visible = false;

        //if (cb_pg.Checked)
        //{
        //    payrollgroup.Visible = true;
           
        //}
        //else
        //    payrollgroup.Visible = false;

        //if (cb_all.Checked)
        //{
        //    all.Visible = true;
          
        //}
        //else
        //    all.Visible = false;


        if(memo_to.SelectedValue == "1")
        {
            payrollgroup.Visible = true;
            employee.Visible = false;
            all.Visible = false;
            branch.Visible = false;
            allemployee.Visible = false;
        }
        else
            payrollgroup.Visible = false;

        if(memo_to.SelectedValue == "2")
        {
            employee.Visible = false;
            payrollgroup.Visible = false;
            all.Visible = true;
            branch.Visible = false;
            allemployee.Visible = false;
        }
        else
            employee.Visible = false;

        if (memo_to.SelectedValue == "3")
        {
            branch.Visible = true;
            all.Visible = false;
            payrollgroup.Visible = false;
            employee.Visible = false;
            allemployee.Visible = false;
        }
        else
            branch.Visible = false;

        if (memo_to.SelectedValue == "0")
        {
            branch.Visible = false;
            all.Visible = false;
            payrollgroup.Visible = false;
            employee.Visible = false;
            allemployee.Visible = true;
        }
        else
            allemployee.Visible = false;


      

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
            //txt_description.Text = "You" + " " + dt.Rows[0]["Name"].ToString() + " is Subject for " + dt.Rows[0]["sanid"].ToString() + " due to " + dt.Rows[0]["rootcause"].ToString() + " happened last " + dt.Rows[0]["incidentdate"].ToString() + ", Number Days of Suspension: " + dt.Rows[0]["nodays"].ToString() + " " + "Day/s";
        }
    }
}