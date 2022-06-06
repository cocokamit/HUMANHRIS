using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class content_hr_addCredentials : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }
    
    }


    [WebMethod]
    public static int CheckCustomerData(string name)
    {
        List<string> data = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select a.Id,b.Department,c.Branch from MEmployee a left join MDepartment b on a.DepartmentId=b.Id left join MBranch c on a.BranchId=c.Id where a.FirstName = '{0}' ", name);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(string.Format("{0}-{1}-{2}", reader["Id"], reader["Department"], reader["Branch"]));
                }
            }
            con.Close();
        }
        return data.ToArray().Length;
    }

    [WebMethod]
    public static string[] GetCustomerData(string term)
    {
        List<string> data = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            //string query = string.Format("select Id from MEmployee where FirstName +' '+LastName +' '+MiddleName like '%{0}%' ", term);
            //using (SqlCommand cmd = new SqlCommand(query, con))
            //{
            //    con.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        data.Add(string.Format("{0}", reader["Id"]));
            //    }
            //}

            string query = string.Format("select a.Id,b.Department,c.Branch from MEmployee a left join MDepartment b on a.DepartmentId=b.Id left join MBranch c on a.BranchId=c.Id where a.FirstName +' '+a.LastName +' '+a.MiddleName like '%{0}%' ", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(string.Format("{0}-{1}-{2}", reader["Id"], reader["Department"], reader["Branch"]));
                }
            }
            con.Close();
        }
        return data.ToArray();
    }



    [WebMethod]
    public static string[] GetCustomer(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
        {
            string query = string.Format("select distinct (FirstName +' '+ LastName) as name from MEmployee where FirstName +' '+LastName +' '+MiddleName like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retCategory.Add(reader.GetString(0));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }


    protected void disp()
    {

        DataTable dt = dbhelper.getdata("select * from muser where id=" + key.Value + "");
        txt_user.Text=dt.Rows[0]["username"].ToString();
        txt_pass.Text=dt.Rows[0]["password"].ToString();
        txt_allias.Text=dt.Rows[0]["fullname"].ToString();
        lbl_des.Text = dt.Rows[0]["designation"].ToString();
       
        lbl_pass.Text = dt.Rows[0]["password"].ToString();
        lbl_user.Text = dt.Rows[0]["username"].ToString();

       // where standard is null or standard='Manager' or standard='Payroll' or standard='report'
        dt = dbhelper.getdata("select * from SysForm order by id,standard desc");
        grid_item.DataSource = dt;
        grid_item.DataBind();


        int i=0;
        foreach (GridViewRow gr in grid_item.Rows)
        {
            CheckBox chk=(CheckBox)grid_item.Rows[i].Cells[2].FindControl("chk_allow");
            string jk = gr.Cells[0].Text;
            DataTable checkdt = dbhelper.getdata("select * from MUserForm where userid=" + key.Value + " and formid=" + gr.Cells[0].Text + " and CanAllow='1' ");
            if (checkdt.Rows.Count == 1)
                chk.Checked = true;
            else
            {
                chk.Checked = false;
               
                DataTable dtcheckulit = dbhelper.getdata("select * from SysForm where id=" + gr.Cells[0].Text + "");
                
                DataTable dtgg = dbhelper.getdata("select * from MUserForm a " +
                                                "left join MUser f on a.UserId=f.Id " +
                                                "where a.CanAllow='True' and f.designation<>'Admin' and a.FormId=" + gr.Cells[0].Text + "");

                if (dtgg.Rows.Count > 1)
                {
                    if (dtcheckulit.Rows[0]["nou"].ToString() == "s")
                        chk.Enabled = false;
                }
            }
            i++;
        }
      
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DataTable dt = dbhelper.getdata("select * from MUserForm a " +
            //                                "where a.CanAllow='True' and a.FormId=" + e.Row.Cells[0].Text + "");
            //CheckBox chk = (CheckBox)e.Row.Cells[2].FindControl("chk_allow");
            //string hhhh = chk.ID.ToString();
            ////grid_item.Rows[e.Row.RowIndex].Cells[2].FindControl("chk_allow");
            //DataTable dtcheckulit = dbhelper.getdata("select * from SysForm where id=" + e.Row.Cells[0].Text + "");
         

            //    if (dt.Rows.Count > 0)
            //    {
            //        if (lbl_des.Text != "Admin")
            //        {
            //            if (dtcheckulit.Rows[0]["nou"].ToString() == "s")
            //            {
            //                if (key.Value != dt.Rows[0]["userid"].ToString()) // && dtcheckulit.Rows[0]["nou"].ToString() == "s"
            //                    chk.Enabled = false;
            //                else
            //                    chk.Enabled = true;
            //            }
            //            else
            //            {
            //                chk.Enabled = true;
            //            }
            //        }
            //    }
            //    else
            //        chk.Enabled = true;
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string pass = txt_pass.Text.Length == 0 ? lbl_pass.Text : txt_pass.Text;
        DataTable ifexists = dbhelper.getdata("select * from MUser where UserName='" + txt_user.Text + "' and Password='" + pass + "' ");
        if (ifexists.Rows.Count == 0)
            dbhelper.getdata("update MUser set UserName='" + txt_user.Text + "',Password='" + pass + "',FullName='" + txt_allias.Text + "',UpdateUserId=" + key.Value + ",UpdateDateTime=getdate() where id=" + key.Value + "");
      
        //string pass = txt_pass.Text.Length == 0 ? lbl_pass.Text : txt_pass.Text;
        //dbhelper.getdata("update MUser set UserName='" + txt_user.Text + "',Password='" + pass + "',FullName='" + txt_allias.Text + "',UpdateUserId=" + key.Value + ",UpdateDateTime=getdate() where id=" + key.Value + "");
        int i = 0;
        foreach (GridViewRow gr in grid_item.Rows)
        {
            CheckBox chk = (CheckBox)grid_item.Rows[i].Cells[2].FindControl("chk_allow");
            string jj = chk.Checked == true ? "1" : "0";
            DataTable checkdt = dbhelper.getdata("select * from MUserForm where userid=" + key.Value + " and formid=" + gr.Cells[0].Text + " ");
            if (checkdt.Rows.Count >0)
                dbhelper.getdata("update MUserForm set [CanAllow]=" + jj + " where UserId=" + key.Value + " and FormId=" + gr.Cells[0].Text + "  ");
            else
                dbhelper.getdata("insert into MUserForm ([UserId],[FormId],[CanAdd],[CanEdit],[CanDelete],[CanLock],[CanUnlock],[CanPreview],[CanPrint],[CanView],[CanAllow])values(" + key.Value + "," + gr.Cells[0].Text + ",'0','0','0','0','0','0','0','0'," + jj + ")");
            i++;
        }
       
    }
}