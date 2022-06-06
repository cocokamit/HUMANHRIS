using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

public partial class content_printable_printappraisal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        disp();
    }
    protected void disp()
    {

        string query = "select a.purposeid,b.lastname+', '+b.firstname+' '+b.middlename as fullname,b.idnumber,c.department,d.position,LEFT(CONVERT(varchar,a.date,101),10)datee,a.recommendid,a.totalratings,(select name from nobel_user where ID=a.userid)appraiser from app_form_trn a " +
                        "left join memployee b on a.empid=b.id " +
                        "left join mdepartment c on b.departmentid=c.id " +
                        "left join mposition d on b.positionid=d.id " +
                        "where a.id=" + function.Decrypt(Request.QueryString["key"].ToString(),true) + " ";

        DataTable dttt = dbhelper.getdata(query);
        lbl_dept.Text = dttt.Rows[0]["department"].ToString();
        lbl_noe.Text = dttt.Rows[0]["fullname"].ToString();
        lbl_pt.Text = dttt.Rows[0]["position"].ToString();
        lbl_pe.Text = dttt.Rows[0]["datee"].ToString();
        lbl_idnumber.Text = dttt.Rows[0]["idnumber"].ToString();
        lbl_app.Text = dttt.Rows[0]["appraiser"].ToString();
        lbl_evalby.Text = dttt.Rows[0]["appraiser"].ToString();
        total_ratings.Value = dttt.Rows[0]["totalratings"].ToString();

        DataTable dt = dbhelper.getdata("select * from app_setup_purposeofappraisal where action is null");
        rdiopurpose.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            rdiopurpose.Items.Add(new System.Web.UI.WebControls.ListItem(dr["descc"].ToString(), dr["id"].ToString()));
        }
        DataTable dtratins = dbhelper.getdata("select * from app_setup_ratings where action is null");
        foreach (DataRow dr in dtratins.Rows)
        {
            Label lbl_rate = new Label();
            lbl_rate.ID = dr["id"].ToString();
            lbl_rate.Text = dr["val"].ToString() + "-" + dr["descc"].ToString() + "  ";
            div_ratings.Controls.Add(lbl_rate);
        }

        DataTable jobfactors = dbhelper.getdata("select a.id, '* '+a.header+'('+convert(varchar,a.percentage)+'%)<br/>'+a.descc header,(a.percentage/100)percenttt from app_setup_jobfactors a");
        grid_jfd.DataSource = jobfactors;
        grid_jfd.DataBind();


        DataTable desceval = dbhelper.getdata("select a.id, a.descc+'<br/>' descc  from app_setup_desceval a");
        grid_desceval.DataSource = desceval;
        grid_desceval.DataBind();

        rdiopurpose.SelectedValue = dttt.Rows[0]["purposeid"].ToString();
    }

    protected void databounddescval(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("lbl_descvalid");
             TextBox txt_ans = (TextBox)e.Row.FindControl("txt_ans");
             string query = "select b.descc, a.desceavalid,a.ans from app_form_trn_formtwo a left join app_setup_desceval b on a.desceavalid=b.id where a.app_form_id=" + function.Decrypt(Request.QueryString["key"].ToString(), true) + " and a.desceavalid=" + lbl.Text + "";
            DataTable dtdt = dbhelper.getdata(query);

            txt_ans.Text = dtdt.Rows[0]["ans"].ToString();
            txt_ans.Enabled = false;
            //Label lbl = (Label)e.Row.FindControl("lbl_iddd");
            //GridView gr = (GridView)e.Row.FindControl("grid_details");

            //string query = "select a.jfid, b.descc,a.rateansid,a.rateansval from app_form_trn_formone a left join app_setup_jobfactors_details b on a.jfdetid=b.id where a.app_form_id=" + Request.QueryString["key"].ToString() + " and a.jfid=" + lbl.Text + "";
            //DataTable dtdt = dbhelper.getdata(query);
           // DataTable dtdet = dbhelper.getdata("select * from app_setup_jobfactors_details where jfid=" + lbl.Text + "");
            //gr.DataSource = dtdt;
            //gr.DataBind();

        }
    }
    protected void databoundnd(object sender, GridViewRowEventArgs e)
    {
        DataTable dtt = new DataTable();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_percent = (Label)e.Row.FindControl("lbl_percent");
            Label lbl = (Label)e.Row.FindControl("lbl_iddd");
            GridView gr = (GridView)e.Row.FindControl("grid_details");
            hdn_percent.Value = lbl_percent.Text;
            string query = "select a.jfid, b.descc,a.rateansid,a.rateansval from app_form_trn_formone a left join app_setup_jobfactors_details b on a.jfdetid=b.id where a.app_form_id=" + function.Decrypt(Request.QueryString["key"].ToString(), true) + " and a.jfid=" + lbl.Text + "";
            dtt = dbhelper.getdata(query);
            gr.DataSource = dtt;
            gr.DataBind();
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbl_total_avg = (Label)e.Row.FindControl("lbl_total_avg");
            lbl_total_avg.Text ="Total: "+ total_ratings.Value;
        }
    }
    protected void databoundnd1(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_idddd = (Label)e.Row.FindControl("lbl_idddd");
            Label lbl_rateansid = (Label)e.Row.FindControl("lbl_rateansid");
            Label lbl_rateansval = (Label)e.Row.FindControl("lbl_rateansval");
            RadioButtonList rdiorate = (RadioButtonList)e.Row.FindControl("rdiorate");
            DataTable dtratins = dbhelper.getdata("select * from app_setup_ratings where action is null");
            rdiorate.Items.Clear();
            foreach (DataRow dr in dtratins.Rows)
            {
                rdiorate.Items.Add(new System.Web.UI.WebControls.ListItem(dr["val"].ToString(), dr["id"].ToString()));
                rdiorate.Enabled = true;
            }
            if (decimal.Parse(lbl_rateansid.Text) > 0)
               rdiorate.SelectedValue = lbl_rateansid.Text;

            rdiorate.Enabled = false;
            decimal t_avg=lbl_hdntotalverage.Value.Length==0?0:decimal.Parse(lbl_hdntotalverage.Value);
            lbl_hdntotalverage.Value = (t_avg + decimal.Parse(lbl_rateansval.Text)).ToString();
            total_no.Value = lbl_idddd.Text;
           

           
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            string query = "select a.jfid, b.descc,a.rateansid,a.rateansval from app_form_trn_formone a left join app_setup_jobfactors_details b on a.jfdetid=b.id where a.app_form_id=" + function.Decrypt(Request.QueryString["key"].ToString(), true) + " and a.jfid=" + total_no.Value + "";
            DataTable dtt = dbhelper.getdata(query);
            Label lbl_hrmetrix = (Label)e.Row.FindControl("lbl_hrmetrix");
            lbl_hrmetrix.Text = "Average(base on HR METRIX): " + Math.Round((decimal.Parse(lbl_hdntotalverage.Value) / dtt.Rows.Count) * decimal.Parse(hdn_percent.Value),2);
            total_no.Value = "0";
            lbl_hdntotalverage.Value = "0";
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
      server control at run time. */
    }
    protected void select_rate(object sender, GridViewRowEventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((RadioButtonList)sender).Parent.Parent)
        {


        }

    }
    protected void save_app_form(object sender, EventArgs e)
    {
        //string query = "";
        //int i = 0;
        //int empid = 0;
        //string ret = "";
        //DataTable dtinsappform = dbhelper.getdata("insert into app_form_trn (date,userid,lastupdateuserid,lastupdatedate,empid,purposeid) values (GETDATE()," + Session["user_id"] + "," + Session["user_id"] + ",GETDATE()," + empid + "," + rdiopurpose.SelectedValue + ") select scope_identity() trnid ");
        //foreach (DataRow dr in grid_jfd.Rows)
        //{
        //    GridView gr = (GridView)grid_jfd.Rows[i].Cells[1].FindControl("grid_details");
        //    Label lbl_jfdid = (Label)grid_jfd.Rows[i].Cells[1].FindControl("lbl_iddd");
        //    int ii = 0;
        //    foreach (DataRow drr in gr.Rows)
        //    {
        //        Label lbl_idddd = (Label)gr.Rows[ii].Cells[2].FindControl("lbl_idddd");
        //        RadioButtonList rdiorate = (RadioButtonList)gr.Rows[ii].Cells[2].FindControl("rdiorate");

        //        //using (SqlConnection con = new SqlConnection(dbconnection.conn))
        //        //{
        //        //    using (SqlCommand cmd = new SqlCommand("app_form_rate", con))
        //        //    {
        //        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //        cmd.Parameters.Add("@trnid", SqlDbType.VarChar, 5000).Value = dtinsappform.Rows[0]["trnid"].ToString();
        //        //        cmd.Parameters.Add("@jfid", SqlDbType.VarChar, 5000).Value = lbl_jfdid.Text;
        //        //        cmd.Parameters.Add("@jfdetid", SqlDbType.VarChar, 5000).Value = lbl_idddd.Text;
        //        //        cmd.Parameters.Add("@rateansid", SqlDbType.VarChar, 50).Value = rdiorate.SelectedValue;


        //        //        cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        //        //        cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        //        //        con.Open();
        //        //        cmd.ExecuteNonQuery();
        //        //        ret = cmd.Parameters["@out"].Value.ToString();
        //        //        con.Close();

        //        //    }
        //        //}
        //        ii++;
        //    }
        //}
        //int iii = 0;
        ////foreach (DataRow drdesceavl in grid_desceval.Rows)
        ////{

        ////    Label lbl_desceval = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_desceval");
        ////    TextBox txt_ans = (TextBox)grid_desceval.Rows[iii].Cells[1].FindControl("txt_ans");
        ////    using (SqlConnection con = new SqlConnection(dbconnection.conn))
        ////    {
        ////        using (SqlCommand cmd = new SqlCommand("app_form_desc_eval", con))
        ////        {
        ////            cmd.CommandType = CommandType.StoredProcedure;
        ////            cmd.Parameters.Add("@trnid", SqlDbType.VarChar, 5000).Value = dtinsappform.Rows[0]["trnid"].ToString();
        ////            cmd.Parameters.Add("@desceavalid", SqlDbType.VarChar, 50).Value = lbl_desceval.Text;
        ////            cmd.Parameters.Add("@ans", SqlDbType.VarChar, 5000).Value = txt_ans.Text;

        ////            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
        ////            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
        ////            con.Open();
        ////            cmd.ExecuteNonQuery();
        ////            ret = cmd.Parameters["@out"].Value.ToString();
        ////            con.Close();

        ////        }
        ////    }
        ////    iii++;
        ////}
    }
    protected void ExportToPDF(object sender, EventArgs e)
    {
        StringReader sr = new StringReader(Request.Form[hfGridHtml.UniqueID]);
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        pdfDoc.Close();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=HTML.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(pdfDoc);
        Response.End();
    }
}