using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HiQPdf;

public partial class content_supervisor_empList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {

        string query = ("select distinct(a.emp_id), a.emp_id empp, " +
         //"case when c.id is null then 0 else c.id end [profile], " +
         "b.IdNumber,b.LastName +', '+b.FirstName+' '+b.middlename as fullname, b.payrollgroupid, " +
         "case when (select COUNT(*) from TLeaveApplicationLine where employeeid=a.emp_id and CONVERT(date,[date])=CONVERT(date,GETDATE()) and status like'%Approved%')=0 " +
         "then " +
         "case when (select COUNT(*) from tdtrperpayrolperline where empid=a.emp_id and  CONVERT(date,biotime)=CONVERT(date,GETDATE()))=0 then '0' else '1' end " +
         "else 2 end [status] " +
         "from approver a " +
         "left join memployee b on a.emp_id=b.id " +
         //"left join file_details c on a.emp_id=c.empid " +
         "where a.under_id=" + Session["emp_id"] + " and a.herarchy=0");
        DataTable dt = dbhelper.getdata(query);
      grid_view.DataSource = dt;
      grid_view.DataBind();
    }
    protected void close(object sender, EventArgs e)
    {
        Response.Redirect("appr");
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
    protected void viewdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_appdet=(LinkButton)row.Cells[1].FindControl("lnk_appdet");

            string query = "select a.id, a.purposeid,b.lastname+', '+b.firstname+' '+b.middlename as fullname,b.idnumber,c.department,d.position,LEFT(CONVERT(varchar,a.date,101),10)date,a.recommendid,a.totalratings,(select name from nobel_user where ID=a.userid)appraiser,e.[desc] recommend,f.descc purpose from app_form_trn a " +
                            "left join memployee b on a.empid=b.id " +
                            "left join mdepartment c on b.departmentid=c.id " +
                            "left join mposition d on b.positionid=d.id " +
                            "left join app_setup_reccomendation e on a.recommendid=e.id " +
                            "left join app_setup_purposeofappraisal f on a.purposeid=f.id " +
                            "where a.empid=" + lnk_appdet.CommandName + " and a.purposeid>0 and b.payrollgroupid<>4";

            //string query = "select a.id, LEFT(CONVERT(varchar,a.date,101),10)date,b.descc purpose, from app_form_trn a " +
            //                "left join app_setup_purposeofappraisal b on a.purposeid=b.id " +
            //                "left join memployee c on a.empid=c.Id where empid=" + lnk_appdet.CommandName + " and a.purposeid>0  ";
            grid_det.DataSource = dbhelper.getdata(query);
            grid_det.DataBind();
            ppop(true);
        }
    }
    protected void click_print(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_print = (LinkButton)row.Cells[5].FindControl("lnk_print");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?prntapp?key=" + function.Encrypt(lnk_print.CommandName, true) + "'", true);
            
            //Response.Redirect("pdf?key="+ function.Encrypt(lnk_print.CommandName,true)+"");
        }
    }
}