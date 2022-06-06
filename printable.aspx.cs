using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class printable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        disp();
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
            lbl_rate.ID=dr["id"].ToString();
            lbl_rate.Text = dr["val"].ToString() + "-" + dr["descc"].ToString()+"  ";
            div_ratings.Controls.Add(lbl_rate);
        }

        DataTable jobfactors = dbhelper.getdata("select a.id, '* '+a.header+'('+convert(varchar,a.percentage)+'%)<br/>'+a.descc header from app_setup_jobfactors a");
        grid_jfd.DataSource = jobfactors;
        grid_jfd.DataBind();


        DataTable desceval = dbhelper.getdata("select a.descc+'<br/>' descc  from app_setup_desceval a");
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
            RadioButtonList rdiorate = (RadioButtonList)e.Row.FindControl("rdiorate");

            DataTable dtratins = dbhelper.getdata("select * from app_setup_ratings where action is null");
            rdiorate.Items.Clear();
            foreach (DataRow dr in dtratins.Rows)
            {
                rdiorate.Items.Add(new ListItem(dr["val"].ToString(), dr["id"].ToString()));
                rdiorate.Enabled = true;
            }

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
        DataTable dtinsappform = dbhelper.getdata("insert into app_form_trn (date,userid,lastupdateuserid,lastupdatedate,empid,purposeid) values (GETDATE()," + Session["user_id"] + "," + Session["user_id"] + ",GETDATE()," + empid + "," + rdiopurpose.SelectedValue + ") select scope_identity() trnid ");
        foreach (DataRow dr in grid_jfd.Rows)
        { 
            GridView gr=(GridView)grid_jfd.Rows[i].Cells[1].FindControl("grid_details");
            Label lbl_jfdid = (Label)grid_jfd.Rows[i].Cells[1].FindControl("lbl_iddd");
            int ii = 0;
            foreach (DataRow drr in gr.Rows)
            {
                Label lbl_idddd = (Label)gr.Rows[ii].Cells[2].FindControl("lbl_idddd");
                RadioButtonList rdiorate = (RadioButtonList)gr.Rows[ii].Cells[2].FindControl("rdiorate");

                using (SqlConnection con = new SqlConnection(dbconnection.conn))
                {
                    using (SqlCommand cmd = new SqlCommand("app_form_rate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@trnid", SqlDbType.VarChar, 5000).Value = dtinsappform.Rows[0]["trnid"].ToString();
                        cmd.Parameters.Add("@jfid", SqlDbType.VarChar, 5000).Value = lbl_jfdid.Text;
                        cmd.Parameters.Add("@jfdetid", SqlDbType.VarChar, 5000).Value = lbl_idddd.Text;
                        cmd.Parameters.Add("@rateansid", SqlDbType.VarChar, 50).Value = rdiorate.SelectedValue;
             
                      
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
        }
        int iii = 0;
        foreach (DataRow drdesceavl in grid_desceval.Rows)
        {

            Label lbl_desceval = (Label)grid_desceval.Rows[iii].Cells[1].FindControl("lbl_desceval");
            TextBox txt_ans = (TextBox)grid_desceval.Rows[iii].Cells[1].FindControl("txt_ans");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("app_form_desc_eval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@trnid", SqlDbType.VarChar, 5000).Value = dtinsappform.Rows[0]["trnid"].ToString();
                    cmd.Parameters.Add("@desceavalid", SqlDbType.VarChar, 50).Value = lbl_desceval.Text;
                    cmd.Parameters.Add("@ans", SqlDbType.VarChar, 5000).Value = txt_ans.Text;

                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
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