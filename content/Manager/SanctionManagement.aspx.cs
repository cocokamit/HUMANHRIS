using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class content_Manager_SanctionManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadable();
        }
    }
    protected void loadable()
    {
        DataTable dt;
        string query = "select distinct(a.emp_id),b.IdNumber,b.FirstName+' '+b.LastName as fullname from approver a "
            + "left join memployee b on a.emp_id=b.id where a.under_id=" + Session["emp_id"].ToString() + ""
            + " and a.emp_id != " + Session["emp_id"].ToString() + "";
        dt = dbhelper.getdata(query);
        foreach (DataRow dr in dt.Rows)
        {
            drop_emp.Items.Add(new ListItem(dr["fullname"].ToString(), dr["emp_id"].ToString()));
        }
        dt = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + drop_emp.SelectedValue + " and action = 'Active' order by id desc");
        grid_sanctions.DataSource = dt;
        grid_sanctions.DataBind();
    }
    protected void load()
    {
        DataTable dt = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + drop_emp.SelectedValue + " and action = 'Active' order by id desc");
        grid_sanctions.DataSource = dt;
        grid_sanctions.DataBind();
    }
    protected void changedisplay(object sender, EventArgs e)
    {
        load();
    }
    protected void ddlvalue_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsanctioncodes.SelectedValue == "1") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "2") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "3") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
        else if (ddlsanctioncodes.SelectedValue == "0") { pnl_SacnOption.Visible = false; btnsusdate.Enabled = false; }
    }
    protected void addsusdate(object sender, EventArgs e)
    {
        if (txtsuspend.Text.Length > 0)
        {
            lblsusdate.Text = string.Empty;
            DataTable dtsunctionsetup = dbhelper.getdata("select * from sanctioncodes where id =" + ddlsanctioncodes.SelectedValue + "");
            DataRow dtrdr;
            DataTable dtdr = ViewState["dtdraft"] as DataTable;
            dtrdr = dtdr.NewRow();
            dtrdr["suspendate"] = txtsuspend.Text;
            dtdr.Rows.Add(dtrdr);
            ViewState["dtdraft"] = dtdr;
            susdate.DataSource = dtdr;
            susdate.DataBind();

            if (dtdr.Rows.Count >= decimal.Parse(dtsunctionsetup.Rows[0]["suspensiondays"].ToString()))
            {
                btnsusdate.Enabled = false;
                suspendpanel.Visible = false;
            }
            else
                btnsusdate.Visible = true;
        }
        else
            lblsusdate.Text = "*";
    }
    protected void closeupload(object sender, EventArgs e)
    {
        suspendpanel.Visible = false;
    }
    protected void RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable sourceData = (DataTable)ViewState["dtdraft"];
        sourceData.Rows[e.RowIndex].Delete();
        susdate.DataSource = sourceData;
        susdate.DataBind();
    }
    protected void addsanctions(object sender, EventArgs e)
    {
        DataTable dtaddsanctions = new DataTable();
        if (ddlsanctioncodes.SelectedItem.Text == "Sanctions/s") l_msg.Text = "Sanctions must be supplied!";
        else if (txt_incidentdate.Text.Length == 0) l_msg.Text = "Incident Date must be supplied!";
        else if (txt_incidentabout.Text.Length == 0) l_msg.Text = "Incident About must be supplied!";
        else if (txt_incidentremarks.Text.Length == 0) l_msg.Text = "Remarks must be supplied!";
        else
        {
            if (drop_emp.SelectedValue.Length > 0)
            {
                DataTable dtid = dbhelper.getdata("insert into sanctionentry (dateapply, sanctioncode, empid, userid, rootcause, action, incidentdate, status, stat, remarks, cancelledate) values ('" + DateTime.Now.ToString() + "','" + ddlsanctioncodes.SelectedValue + "'," + drop_emp.SelectedValue + ",'" + Session["user_id"].ToString() + "','" + txt_incidentabout.Text.Replace("'", "") + "','Active','" + txt_incidentdate.Text + "','Draft',NULL,'" + txt_incidentremarks.Text.Replace("'", "") + "',NULL) select scope_identity() as santionid ");
                Response.Redirect("sendmemo?empid=" + drop_emp.SelectedValue.ToString() + "&entid=" + dtid.Rows[0]["santionid"].ToString() + "", false);
            }
            grid_sanctions.DataSource = dtaddsanctions;
            grid_sanctions.DataBind();
        }
    }
    protected void incidentreports(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lb = (LinkButton)grid_sanctions.Rows[row.RowIndex].Cells[3].FindControl("lbirlistfiles");
            trn_det_id.Value = lb.CommandName;
            DataTable dt = dbhelper.getdata("select * from irfiles where said = '" + trn_det_id.Value + "' and empid = '" + drop_emp.SelectedValue + "' and [action] = 'Active'");
            incidentfiles.DataSource = dt;
            incidentfiles.DataBind();
            irsanctionlist.Style.Add("display", "block");
        }
    }
    protected void uploadirfiles(object sender, EventArgs e)
    {
        if (fuincidentreport.HasFile)
        {
            string filename = Path.GetFileNameWithoutExtension(fuincidentreport.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuincidentreport.PostedFile.FileName);
            string contenttype = fuincidentreport.PostedFile.ContentType;

            string nameofiles = drop_emp.SelectedValue;
            DirectoryInfo nw = Directory.CreateDirectory(Server.MapPath("~/files/incidentreportfiles/") + nameofiles);
            DataTable dtinsertirfile = dbhelper.getdata("insert into irfiles (empid, userid, extension, status, contenttype, filename, said, action, date, canceldate) values ('" + drop_emp.SelectedValue + "','" + Session["user_id"].ToString() + "','" + fileExtension + "','Active','" + contenttype + "','" + filename + "','" + sanctionid.Value + "','Active','" + DateTime.Now.ToString() + "',Null) select scope_identity() idd");
            dbhelper.getdata("update sanctionentry set status = 'Verified', stat = '1' where id =" + sanctionid.Value + "");

            DataTable dtselect = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + drop_emp.SelectedValue + " and action = 'Active' order by id desc");
            fuincidentreport.SaveAs(Server.MapPath("~/files/incidentreportfiles/") + nameofiles + "/" + dtinsertirfile.Rows[0]["idd"].ToString() + fileExtension);

            grid_sanctions.DataSource = dtselect;
            grid_sanctions.DataBind();

            modal_SancAttachment.Style.Add("display", "none");
        }
    }
    protected void downlaodirfilesbypiece(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string folderdir = drop_emp.SelectedValue;
            LinkButton lnkbtn = (LinkButton)incidentfiles.Rows[row.RowIndex].Cells[1].FindControl("lnk_downloadirpiece");
            DataTable dt = dbhelper.getdata("select * from irfiles where id = " + lnkbtn.CommandName + "");
            string input = Server.MapPath("~/files/incidentreportfiles/" + folderdir + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = dt.Rows[0]["contenttype"].ToString();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(input));
            Response.WriteFile(input);
            Response.Flush();
            Response.End();
        }
    }
    protected void downloadirfiles(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string folderdir = drop_emp.SelectedValue;
            LinkButton laodirfile = (LinkButton)grid_sanctions.Rows[row.RowIndex].Cells[7].FindControl("lbdonwloadir");
            DataTable dt = dbhelper.getdata("select top 1 * from irfiles where said = " + laodirfile.CommandName + " order by id desc");
            string fileout = Server.MapPath("~/files/incidentreportfiles/" + folderdir + "/") + dt.Rows[0]["id"].ToString() + dt.Rows[0]["extension"].ToString();
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileout));
            Response.WriteFile(fileout);
            Response.Flush();
            Response.End();
        }
    }
    protected void deletesanctions(object sender, EventArgs e)
    {
        DataTable dtdeltesanctioins = new DataTable();
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            dbhelper.getdata("update irfiles set action = 'Cancel', canceldate = '" + DateTime.Now.ToString() + "' where said = '" + sanctionid.Value + "'");
            dbhelper.getdata("update sanctionentry set action = 'Cancel', cancelledate = '" + DateTime.Now.ToString() + "' where id=" + row.Cells[0].Text + "");
            dbhelper.getdata("update suspension set action = 'Cancel', canceldate = '" + DateTime.Now.ToShortDateString() + "' where id = " + row.Cells[0].Text + "");
            dtdeltesanctioins = dbhelper.getdata("select id,(select name from nobel_user where Id = userid)userid, rootcause,(select sanction from sanctioncodes where id = sanctioncode)sanctioncode, LEFT(convert(varchar, incidentdate,101),10) as incidentdate, remarks, status,(select suspensiondays from sanctioncodes where id=a.sanctioncode)as nodays from sanctionentry a where empid = " + drop_emp.SelectedValue + " and action = 'Active' order by id desc");
        }
        grid_sanctions.DataSource = dtdeltesanctioins;
        grid_sanctions.DataBind();
    }
    protected void chngestatus(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            string id = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from sanctionentry where id = '" + id + "'");
            Response.Redirect("sendmemo?empid=" + drop_emp.SelectedValue.ToString() + "&entid=" + id + "", false);
        }
    }
    protected void irfilesupload(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            sanctionid.Value = row.Cells[0].Text;
        }
        modal_SancAttachment.Style.Add("display", "block");
    }
    protected void close_sacnAttachment(object sender, EventArgs e)
    {
        modal_SancAttachment.Style.Add("display", "none");
    }
    protected void tawesie(object seder, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton draft = (LinkButton)e.Row.FindControl("lbupload");
            LinkButton stat = (LinkButton)e.Row.FindControl("itemstatus");
            if (stat.Text == "Verified" || stat.Text == "For Clearing")
            {
                stat.Enabled = false;
            }
            else if (stat.Text == "Draft")
                draft.Visible = false;
            LinkButton lbshowdownload = (LinkButton)e.Row.FindControl("lbdonwloadir");
            DataTable dt = dbhelper.getdata("select * from irfiles where said=" + lbshowdownload.CommandName + "");
            if (dt.Rows.Count > 0)
                lbshowdownload.Visible = true;
            else
                lbshowdownload.Visible = false;
            LinkButton lb = (LinkButton)e.Row.FindControl("trash");
            if (stat.Text == "Verified")
            {
                lb.Visible = false;
                draft.ToolTip = "Re Verified";
            }
            else if (stat.Text == "Draft")
                lb.ToolTip = "Cancel Draft";
            else
                lb.Enabled = true;
        }
    }
    protected void closingremarks(object sender, EventArgs e)
    {
        irsanctionlist.Style.Add("display", "none");
    }
}