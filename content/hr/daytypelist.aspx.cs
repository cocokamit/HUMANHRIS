using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;

public partial class content_hr_daytypelist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);

            modal.Style.Add("display", "none");
            modalcreate.Style.Add("display", "none");
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select * from MDayType where status is null ");
        grid_view.DataSource = dt;
        grid_view.DataBind();

        dt = dbhelper.getdata("Select id,startdate,enddate,name,DATENAME(dw,startdate) [dow],(Select DayType from MDayType where Id=a.Type)[type] from MDayTypeHoliday a where year(startdate)=YEAR(GETDATE()) order by startdate asc");
        grid_holidays.DataSource=dt;
        grid_holidays.DataBind();

        string query = "select * from Mbranch order by id desc";
        dt = dbhelper.getdata(query);

        lstFruits.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            lstFruits.Items.Add(new ListItem(dr["Branch"].ToString(), dr["id"].ToString()));
        }

    }
    protected void click_add_daytype(object sender, EventArgs e)
    {
        Response.Redirect("adddaytype", false);

    }
    protected void click_edit_daytype(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("editdaytype?app_id=" + function.Encrypt(row.Cells[0].Text, true) + "", false);
        }
    }
    protected void cancel_daytype(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MDayType set status='cancel-" + function.Decrypt(Request.QueryString["user_id"].ToString(), true) + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("Mdaytype", false);
            }
            else
            {
            }
        }
    }
    protected void gg_ta_ani_brad(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    LinkButton lnk_view = e.Row.FindControl("lnk_view") as LinkButton;
        //    if (e.Row.Cells[0].Text == "1")
        //    {
        //        lnk_view.Visible = false;
        //    }
            
        //}
    }

    protected void editHoliday(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            LinkButton lnk = (sender) as LinkButton;

            if (lnk.ID == "lnk_holiday")
            {
                DataTable dt = dbhelper.getdata("Select name,type,CONVERT(date,startdate)[startdates],CONVERT(date,enddate)[enddates] from MDayTypeHoliday where id=" + row.Cells[0].Text.ToString() + "");
          
                lb_holidate.Text = dt.Rows[0]["name"].ToString();
                txt_startdate.Text =Convert.ToDateTime(dt.Rows[0]["startdates"].ToString()).ToString("MM/dd/yyyy");
                txt_enddate.Text = Convert.ToDateTime(dt.Rows[0]["enddates"].ToString()).ToString("MM/dd/yyyy");
                ddl_datetype.ClearSelection();

                ddl_datetype.Items.FindByValue(dt.Rows[0]["type"].ToString()).Selected = true;
                ViewState["holid"] = row.Cells[0].Text.ToString();
                
                string query = "select * from Mbranch order by id desc";
                dt = dbhelper.getdata(query);
             
                listbranch.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                     dt = dbhelper.getdata("Select * from MDayTypeDay where HolidayId="+row.Cells[0].Text.ToString()+" and BranchId="+dr["Id"].ToString()+"");
                     ListItem li = new ListItem(dr["Branch"].ToString(), dr["id"].ToString());
                    if (dt.Rows.Count > 0)
                     {
                         li.Selected = true;
                         listbranch.Items.Add(li);
                     }
                     else
                    {
                         li.Selected = false;
                         listbranch.Items.Add(li);
                     }
                }

                modal.Style.Add("display", "block");
            }
            else
            {
                DataTable dt = dbhelper.getdata("Delete from MDayTypeHoliday where id="+row.Cells[0].Text.ToString()+"");
                disp();
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        string datetime = DateTime.Now.ToString();
        string holid=ViewState["holid"].ToString();

        string apostrophe = lb_holidate.Text.Replace("'", "''");
        DataTable dt = dbhelper.getdata("Update MDayTypeHoliday set name='" + apostrophe + "',startdate='" + txt_startdate.Text + "',enddate='" + txt_enddate.Text + "',type=" + ddl_datetype.SelectedValue + " where id=" + holid + "");
    
        foreach (ListItem item in listbranch.Items)
        {
            dt = dbhelper.getdata("Select * from MDayTypeDay where HolidayId=" + holid + " and BranchId=" + item.Value + "");

            if (item.Selected)
            {
              
                if (dt.Rows.Count == 0)
                {
                    //change format
                    dt = dbhelper.getdata("insert into MDayTypeDay values (" + ddl_datetype.SelectedValue + "," + item.Value + ",'" + txt_startdate.Text + "','" + txt_startdate.Text + "','" + txt_enddate.Text + "',NULL,'" + apostrophe + "',NULL," + holid + ",NULL)");
                }
                if (dt.Rows.Count > 0)
                {
                    //change format
                    dt = dbhelper.getdata("Update MDayTypeDay set DayTypeId=" + ddl_datetype.SelectedValue + ",BranchId=" + item.Value + ",Date='" + txt_startdate.Text + "',DateAfter='" + txt_startdate.Text + "',DateBefore='" + txt_enddate.Text + "',ExcludedInFixed='NULL',Remarks='" + apostrophe + "',WithAbsentInFixed='NULL',HolidayId=" + holid + ",status='NULL' where HolidayId="+holid+" ");
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    dt = dbhelper.getdata("Delete from MDayTypeDay where HolidayId="+holid+" and BranchId="+item.Value+"");
                }
            }
        }
        disp();

        modal.Style.Add("display", "none");
    }
    protected void Create_Click(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("Select * from MDayTypeHoliday where name='" + txt_holname.Text + "' and startdate='" + txt_holsdate.Text + "'");

        if (dt.Rows.Count == 0)
        {
            string datetime = DateTime.Now.ToString();
            string apostrophe = txt_holname.Text.Replace("'", "''");
            dt = dbhelper.getdata("Insert into MDayTypeHoliday(startdate,enddate,name,dateupdated,type) OUTPUT Inserted.ID Values('" + txt_holsdate.Text + "','" + txt_holedate.Text + "','" + apostrophe + "','" + datetime + "'," + ddl_holtype.SelectedValue + ")");
       
            if (dt.Rows.Count > 0)
            {
                foreach (ListItem item in lstFruits.Items)
                {
                    if (item.Selected)
                    {
                        dbhelper.getdata("insert into MDayTypeDay values (" + ddl_holtype.SelectedValue + "," + item.Value + ",'" + txt_holsdate.Text + "','" + txt_holsdate.Text + "','" + txt_holedate.Text + "',NULL,'" + apostrophe + "',NULL," + dt.Rows[0]["ID"].ToString() + ",NULL)");
                    }
                }
            }
            modalcreate.Style.Add("display","none");

            disp();
        }
        else
        {
            lb_error.Text = "Name already exist.";
        }
        
    }

    protected void Compose_click(object sender, EventArgs e)
    {
        txt_holname.Text = "";
        txt_holsdate.Text = "";
        txt_holedate.Text = "";
        lb_error.Text = "";
       
        modalcreate.Style.Add("display", "block");
    }

    protected void click_close(object sender, EventArgs e)
    {
        modal.Style.Add("display", "none");

        modalcreate.Style.Add("display", "none");
    }

    [WebMethod]
    public static string getUpdate(string term)
    {   
        string alert = "";
        if (term == null)
        {
            alert = "Failed to load.";
        }
        else
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                string datetime = DateTime.Now.ToString();
                string query = " Select year(dateupdated) [Yeardate] from MDayTypeHoliday order by dateupdated desc";
                
                using (SqlCommand clm = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = clm.ExecuteReader();
                    if (reader.Read())
                    {
                        alert = reader["Yeardate"].ToString();

                    }
                    con.Close();
                }
            }
        }

        alert=alert==""?(Convert.ToInt32(DateTime.Now.ToString("yyyy"))-1).ToString():alert;

        return alert;
    }

    [WebMethod]
    public static string SaveHoliday(List<holidays> term)
    {
        string alert = "";
        if (term == null)
        {
            alert = "Failed to load.";
        }
        else
        {
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                string datetime = DateTime.Now.ToString();
                string query = "";
                string apostrophe = "";
                string adh = "";
                DataRow[] dr=null;
                DataTable dt = new DataTable();
                //--------------------------------Get all previous assigned holidays except for ASNWD---------------------------------
                query = "Select * from MDayTypeHoliday where name!='Additional Special Non-Working Day' and [type] IS NOT NULL";
                using (SqlCommand clm = new SqlCommand(query, con))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    clm.CommandType = CommandType.Text;
                    con.Open();
                    sda.SelectCommand = clm;
                    sda.Fill(dt);
                    con.Close();
                }

                DataTable tb = dbhelper.getdata("Select * from MBranch order by id desc");
                int count=0;
                //--------------------------------Populate updated holidays-------------------------------
                foreach (holidays ss in term)
                {

                    apostrophe = ss.summary.Replace("'", "''").Replace("Holiday", "").Trim();

                    if (ss.summary != "Additional Special Non-Working Day")
                    {

                        if (dt.Rows.Count > 0)
                        {
                            dr = dt.Select("name='" + apostrophe + "' and startdate='"+ss.startdate+"'");
                        }

                            if (dr.Count() > 0)
                            {
                                //if holiday with type already exist then update
                                query += " Update MDayTypeHoliday set startdate='" + ss.startdate + "',enddate='" + ss.enddate + "',name='" + apostrophe + "',dateupdated='" + datetime + "' where id=" + dr[0]["id"].ToString() + "";

                                query += " Update MDayTypeDay set [Date]='" + ss.startdate + "',DateAfter='" + ss.startdate + "',DateBefore='" + ss.enddate + "' where HolidayId=" + dr[0]["id"].ToString() + "";
                            }
                        else
                            {
                                dr = dt.Select("name='" + apostrophe + "'");
                            //if new holiday is implemented then insert
                                if (dr.Count() > 0)
                                {
                                    query += " Insert into MDayTypeHoliday(startdate,enddate,name,dateupdated,type) Values('" + ss.startdate + "','" + ss.enddate + "','" + apostrophe + "','" + datetime + "'," + dr[0]["type"].ToString() + ")";
                                    if (tb.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in tb.Rows)
                                        {
                                            query += " Declare @max" + count + " int"
                                                    + " SELECT @max" + count + "=MAX(Id) FROM MDayTypeHoliday;"
                                                    + " insert into MDayTypeDay values (" + dr[0]["type"].ToString() + "," + row["Id"].ToString() + ",'" + ss.startdate + "','" + ss.startdate + "','" + ss.enddate + "',NULL,'" + apostrophe + "',NULL,@max" + count + ",NULL)";
                                            ++count;
                                        }
                                    }
                                }
                        }
                       

                    }
                }
                using (SqlCommand clm = new SqlCommand(query, con))
                {
                    con.Open();
                    clm.ExecuteNonQuery();
                    con.Close();
                }
                alert = "Mdaytype";
            }
        }
        return alert;
    }

    public class holidays
    {
        public string summary { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
    }

}