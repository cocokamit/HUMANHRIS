using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using HiQPdf;

public partial class content_supervisor_appraisalform : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select * from app_setup_purposeofappraisal where action is null");
        rdiopurpose.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            rdiopurpose.Items.Add(new ListItem(dr["descc"].ToString(), dr["id"].ToString()));
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

        DataTable desceval = dbhelper.getdata("select a.descc+'<br/>' descc, a.id  from app_setup_desceval a");
        grid_desceval.DataSource = desceval;
        grid_desceval.DataBind();

        DataTable recommendation = dbhelper.getdata("select * from app_setup_reccomendation ");
        ddl_recommendation.Items.Clear();
        foreach (DataRow dr in recommendation.Rows)
        {
            ddl_recommendation.Items.Add(new ListItem(dr["desc"].ToString(),dr["id"].ToString()));
        }
        string query = "select * from memployee where id="+function.Decrypt(Request.QueryString["key"].ToString(),true)+"";
        DataTable dttemp = dbhelper.getdata(query);

        lbl_noe.Text = dttemp.Rows[0]["Lastname"].ToString() + ", " + dttemp.Rows[0]["Firstname"].ToString() + " " + dttemp.Rows[0]["Middlename"].ToString();
        lbl_idnumber.Text=dttemp.Rows[0]["Idnumber"].ToString();

    }
    protected void databoundnd(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("lbl_iddd");
            GridView gr = (GridView)e.Row.FindControl("grid_details");
            DataTable dtdet = dbhelper.getdata("select * from app_setup_jobfactors_details where jfid=" + lbl.Text + "");
            gr.DataSource = dtdet;
            gr.DataBind();

        }
    }
    protected void databoundnd1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl = (Label)e.Row.FindControl("lbl_idddd");
            Panel Panel1 = (Panel)e.Row.FindControl("Panel1");

            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rb_rate");

            //string mm = rdiorate.SelectedValue;
            //RadioButtonList rbl = new RadioButtonList();
            //rbl.ID = "desc_" + lbl.Text;
            DataTable dtratins = dbhelper.getdata("select * from app_setup_ratings where action is null");
            rbl.Items.Clear();
            foreach (DataRow dr in dtratins.Rows)
            {
                rbl.Items.Add(new ListItem(dr["val"].ToString(), dr["id"].ToString()));
            }

            //Panel1.Controls.Add(rbl);

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
        string query = "";
        int i = 0;
        int empid = 0;
        string ret = "";
        lbl_poa.Text = "";
        DataTable dt_rate = new DataTable();
        dt_rate.Columns.Add(new DataColumn("trnid", typeof(string)));
        dt_rate.Columns.Add(new DataColumn("jfid", typeof(string)));
        dt_rate.Columns.Add(new DataColumn("jfdetid", typeof(string)));
        dt_rate.Columns.Add(new DataColumn("rateansid", typeof(string)));
        dt_rate.Columns.Add(new DataColumn("rateansval", typeof(string)));
        DataRow dt_rate_dr;
        DataTable dt_desceval = new DataTable();
        dt_desceval.Columns.Add(new DataColumn("trnid", typeof(string)));
        dt_desceval.Columns.Add(new DataColumn("desceavalid", typeof(string)));
        dt_desceval.Columns.Add(new DataColumn("ans", typeof(string)));
        DataRow dt_desceval_dr;
        if (rdiopurpose.SelectedValue.Length > 0)
        {
            string prpid = rdiopurpose.SelectedValue.Length == 0 ? "0" : rdiopurpose.SelectedValue;
            hdn_purposeid.Value = prpid;
            decimal average = 0;
            foreach (GridViewRow dr in grid_jfd.Rows)
            {

                GridView gr = (GridView)grid_jfd.Rows[i].Cells[0].FindControl("grid_details");
                Label lbl_jfdid = (Label)grid_jfd.Rows[i].Cells[0].FindControl("lbl_iddd");
                Label lbl_percent = (Label)grid_jfd.Rows[i].Cells[0].FindControl("lbl_percent");
                int ii = 0;
                decimal hrmatrix=0;
                foreach (GridViewRow drr in gr.Rows)
                {
                    Label lbl_idddd = (Label)gr.Rows[ii].Cells[2].FindControl("lbl_idddd");

                    Panel Panel1 = (Panel)gr.Rows[ii].Cells[2].FindControl("Panel1");
                    RadioButtonList rdiorate = (RadioButtonList)gr.Rows[ii].Cells[2].FindControl("rb_rate");
                   
                    string value = rdiorate.SelectedValue.Length == 0 ? "0" : rdiorate.SelectedItem.Text;
                    dt_rate_dr = dt_rate.NewRow();
                    dt_rate_dr["jfid"] = lbl_jfdid.Text;
                    dt_rate_dr["jfdetid"] = lbl_idddd.Text;
                    dt_rate_dr["rateansid"] = rdiorate.SelectedValue.Length == 0 ? "0" : rdiorate.SelectedValue;
                    dt_rate_dr["rateansval"] = value;
                    dt_rate.Rows.Add(dt_rate_dr);

                    hrmatrix = hrmatrix + decimal.Parse(value);
                   
                    ii++;
                }
                dt_rate_dr = dt_rate.NewRow();
                dt_rate_dr["jfid"] = 0;
                dt_rate_dr["jfdetid"] = 0;
                dt_rate_dr["rateansid"] =0;
                dt_rate_dr["rateansval"] = "Average-" + Math.Round((hrmatrix / gr.Rows.Count) * decimal.Parse(lbl_percent.Text), 2);
                dt_rate.Rows.Add(dt_rate_dr);

                average = Math.Round(average + (hrmatrix / gr.Rows.Count) * decimal.Parse(lbl_percent.Text),2);
                i++;
            }
            dt_rate_dr = dt_rate.NewRow();
            dt_rate_dr["jfid"] = 0;
            dt_rate_dr["jfdetid"] = 0;
            dt_rate_dr["rateansid"] = 0;
            dt_rate_dr["rateansval"] = average;
            dt_rate.Rows.Add(dt_rate_dr);

            int iii = 0;
            foreach (GridViewRow drdesceavl in grid_desceval.Rows)
            {

                Label lbl_iddddd = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_iddddd");
                Label lbl_desceval = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_desceval");
                TextBox txt_ans = (TextBox)grid_desceval.Rows[iii].Cells[1].FindControl("txt_ans");
                dt_desceval_dr = dt_desceval.NewRow();
                dt_desceval_dr["desceavalid"] = lbl_iddddd.Text;
                dt_desceval_dr["ans"] = txt_ans.Text;
                dt_desceval.Rows.Add(dt_desceval_dr);
                
                iii++;
            }

            ViewState["ansrating"] = dt_rate;
            ViewState["desceval"] = dt_desceval;

          //  DataRow[] gettotal = dt_rate.Select("rateansval like'%total%");
            Label2.Text ="Total Rating: "+ dt_rate.Rows[dt_rate.Rows.Count -1]["rateansval"].ToString();    // dt_rate.Rows[int.Parse(dt_rate.Rows.Count)]["rateansval"].ToString();
             string recommendid="0";
             if (decimal.Parse(dt_rate.Rows[dt_rate.Rows.Count - 1]["rateansval"].ToString()) < 3)


                 recommendid = "7";
             else
                 recommendid = "6";
             ddl_recommendation.SelectedValue = recommendid;
            
            ppop(true);
        }
        else
            lbl_poa.Text = "*";
            //Response.Write("<script>alert('Pls. Specify Purpose of Appraisal!')</script>");
    }
    protected void click_save(object sender, EventArgs e)
    {
        DataTable ansrating = ViewState["ansrating"] as DataTable;
        DataTable desceval = ViewState["desceval"] as DataTable;
        string ret = "";
        string query1 = "insert into app_form_trn (date,userid,lastupdateuserid,lastupdatedate,empid,purposeid,recommendid,totalratings) values " +
            "(GETDATE()," + Session["user_id"] + "," + Session["user_id"] + ",GETDATE()," + function.Decrypt(Request.QueryString["key"].ToString(), true) + "," + hdn_purposeid.Value + "," + ddl_recommendation.SelectedValue + ",'" + ansrating.Rows[ansrating.Rows.Count - 1]["rateansval"].ToString() + "') select scope_identity() trnid ";
        DataTable dtinsappform = dbhelper.getdata(query1);
        foreach (DataRow drrating in ansrating.Rows)
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("app_form_rate", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@trnid", SqlDbType.Int).Value = dtinsappform.Rows[0]["trnid"].ToString();
                    cmd.Parameters.Add("@jfid", SqlDbType.Int).Value = drrating["jfid"];
                    cmd.Parameters.Add("@jfdetid", SqlDbType.Int).Value = drrating["jfdetid"];
                    cmd.Parameters.Add("@rateansid", SqlDbType.Int).Value = drrating["rateansid"];
                    cmd.Parameters.Add("@rateansval", SqlDbType.VarChar, 5000).Value = drrating["rateansval"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }
        }
        foreach (DataRow drrrr in desceval.Rows)
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("app_form_desc_eval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@trnid", SqlDbType.Int).Value = dtinsappform.Rows[0]["trnid"].ToString();
                    cmd.Parameters.Add("@desceavalid", SqlDbType.Int).Value = drrrr["desceavalid"];
                    cmd.Parameters.Add("@ans", SqlDbType.VarChar, 500000).Value = drrrr["ans"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50000);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "window.open('','_new').location.href='pdf?prntapp?key=" + function.Encrypt(dtinsappform.Rows[0]["trnid"].ToString(), true) + "'", true);
    }
    protected void close(object sender, EventArgs e)
    {
        ppop(false);
    }
    protected void ppop(bool oi)
    {
        panelOverlay.Visible = oi;
        panelPopUpPanel.Visible = oi;
    }
}