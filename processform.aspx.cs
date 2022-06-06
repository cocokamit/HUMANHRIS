using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class processform : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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

        DataTable jobfactors = dbhelper.getdata("select a.id, '* '+a.header+'('+convert(varchar,a.percentage)+'%)<br/>'+a.descc header from app_setup_jobfactors a");
        grid_jfd.DataSource = jobfactors;
        grid_jfd.DataBind();


        DataTable desceval = dbhelper.getdata("select a.descc+'<br/>' descc, a.id  from app_setup_desceval a");
        grid_desceval.DataSource = desceval;
        grid_desceval.DataBind();
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
        string query1 = "insert into app_form_trn (date,userid,lastupdateuserid,lastupdatedate,empid,purposeid) values (GETDATE(),1,1,GETDATE()," + empid + ",1) select scope_identity() trnid ";
        DataTable dtinsappform = dbhelper.getdata(query1);
        foreach (GridViewRow dr in grid_jfd.Rows)
        {
          
            GridView gr = (GridView)grid_jfd.Rows[i].Cells[0].FindControl("grid_details");
            Label lbl_jfdid = (Label)grid_jfd.Rows[i].Cells[0].FindControl("lbl_iddd");
            int ii = 0;
            foreach (GridViewRow drr in gr.Rows)
            {
                Label lbl_idddd = (Label)gr.Rows[ii].Cells[2].FindControl("lbl_idddd");

                Panel Panel1 = (Panel)gr.Rows[ii].Cells[2].FindControl("Panel1");
                RadioButtonList rdiorate = (RadioButtonList)gr.Rows[ii].Cells[2].FindControl("rb_rate");
                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("app_form_rate", con))
                    {
                        
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@trnid", SqlDbType.Int).Value = dtinsappform.Rows[0]["trnid"].ToString();
                            cmd.Parameters.Add("@jfid", SqlDbType.Int).Value = lbl_jfdid.Text;
                            cmd.Parameters.Add("@jfdetid", SqlDbType.Int).Value = lbl_idddd.Text;
                            cmd.Parameters.Add("@rateansid", SqlDbType.Int).Value = rdiorate.SelectedValue.Length == 0 ? "0" : rdiorate.SelectedValue;
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ret = cmd.Parameters["@out"].Value.ToString();
                            con.Close();
                        
                    }
                }
                ii++;
            }
            i++;
        }
        int iii = 0;
        foreach (GridViewRow drdesceavl in grid_desceval.Rows)
        {

            Label lbl_iddddd = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_iddddd");
            Label lbl_desceval = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_desceval");
            TextBox txt_ans = (TextBox)grid_desceval.Rows[iii].Cells[1].FindControl("txt_ans");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("app_form_desc_eval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@trnid", SqlDbType.Int).Value = dtinsappform.Rows[0]["trnid"].ToString();
                    cmd.Parameters.Add("@desceavalid", SqlDbType.Int).Value = lbl_iddddd.Text;
                    cmd.Parameters.Add("@ans", SqlDbType.VarChar, 500000).Value = txt_ans.Text;
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 50000);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ret = cmd.Parameters["@out"].Value.ToString();
                    con.Close();

                }
            }
            iii++;
        }
    }
}