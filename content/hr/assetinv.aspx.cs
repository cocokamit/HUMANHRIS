using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

public partial class content_hr_assetinv : System.Web.UI.Page
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

    [WebMethod]
    public static string[] GetAssetInv(string term)
    {
        List<string> retCategory = new List<string>();
        using (SqlConnection con = new SqlConnection(Config.connection()))
        {
            //string query = string.Format("select * from asset_inventory a left join asset_details b on a.id = b.inventid " +
            //    "left join asset_cat c on c.id = a.categoryid where b.status+' '+a.serial+' '+a.Description+' '+" +
            //    "c.Description+' '+c.um+' '+a.propertycode+' '+a.vendor+' '+a.brand+' '+a.price+' '+" +
            //    "(select convert(varchar,a.podate,101))+' '+(select convert(varchar,b.date,101)) like '%{0}%'", term);
            string query = string.Format("select * from asset_inventory a left join asset_cat b on a.categoryid = b.id where a.Description+' '+b.Description like '%{0}%'", term);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //retCategory.Add(string.Format("{0}-{1}", "0", "All"));
                while (reader.Read())
                {
                    retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["Description"]));
                    //retCategory.Add(string.Format("{0}-{1}", reader["id"], reader["Description"]));
                }
            }
            con.Close();
        }
        return retCategory.ToArray();
    }

    protected void click_close(object sender, EventArgs e)
    {
        popclear();
    }

    protected void popclear()
    {
        overlay.Visible = false;
        catPanel.Visible = false;
        invetPanel.Visible = false;
    }

    protected void click_inventory(object sender, EventArgs e)
    {
        invetPanel.Style.Add("top", "10%");
        overlay.Visible = true;
        invetPanel.Visible = true;
    }

    protected void showdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal_assetdetails.Style.Add("display", "block");
            string query = "select a.id,LEFT(convert(varchar,a.date,101),10)shelfdate,LEFT(convert(varchar,a.podate,101),10)podate,a.serial,a.Description Namejud,a.propertycode,a.vendor,a.brand,a.price,b.Description cat,b.um,LEFT(convert(varchar,d.date,101),10)transacdate,d.qty,d.status,e.FullName,(select Department from MDepartment where Id = e.DepartmentId)dept from asset_inventory a left join asset_cat b on a.categoryid = b.id left join asset_details c on c.inventid = a.id left join asset_assign d on d.invid = a.id left join MEmployee e on e.Id = d.empid where a.id = " + row.Cells[0].Text + "";
            DataTable dt = dbhelper.getdata(query);
            lbl_detail.Text = dt.Rows[0]["Namejud"].ToString();
            gridasset.DataSource = dt;
            gridasset.DataBind();
        }
    }
    protected void editdetails(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            modal_editasset.Style.Add("display", "block");
            seriesid.Value = row.Cells[0].Text;
            DataTable dt = dbhelper.getdata("select * from(select LEFT(convert(varchar, a.podate,101),10)podate,LEFT(convert(varchar, a.[date],101),10)[date],a.propertycode,a.price,a.brand,a.vendor,a.categoryid catid, b.inventid itemid,c.Description Category,a.serial serial,a.Description Namejud, convert(varchar,cast(round(b.qty,0)as numeric(16,0))) qty, c.um,b.status from asset_inventory a left join asset_details b on b.inventid = a.id left join asset_cat c on c.id = a.categoryid where c.serialized=1 and c.action is null and a.action is null UNION select LEFT(convert(varchar, b.podate,101),10)podate,LEFT(convert(varchar, b.[date],101),10)[date],b.propertycode,b.price,b.brand,b.vendor,a.id catid, b.id itemid, c.Description Category,  b.serial serial,b.Description Namejud, convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id)) qty,a.um, case when (select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id)>0 then 'On Stock' else 'Out of Stock' end status from asset_cat a left join asset_inventory b on b.categoryid = a.id left join asset_cat c on c.id = b.categoryid where a.serialized=0 and a.action is null) tt where itemid = " + seriesid.Value + "");
            if (dt.Rows[0]["serial"].ToString() == "-")
            {
                txt_s.Enabled = false;
            }
            lbldetails.Text = dt.Rows[0]["Namejud"].ToString();
            txt_s.Text = dt.Rows[0]["serial"].ToString();
            txt_pc.Text = dt.Rows[0]["propertycode"].ToString();
            txt_v.Text = dt.Rows[0]["vendor"].ToString();
            txt_vn.Text = dt.Rows[0]["brand"].ToString();
            txt_id.Text = dt.Rows[0]["Namejud"].ToString();
        }
    }
    protected void saveedit(object sender, EventArgs e)
    {
        dbhelper.getdata("update asset_inventory set serial = '" + txt_s.Text + "', propertycode = '" + txt_pc.Text + "', vendor = '" + txt_v.Text + "', brand = '" + txt_vn.Text + "', Description = '" + txt_id.Text + "' where id = '" + seriesid.Value + "'");
        Response.Redirect("~/manageasset");
    }
    protected void click_category(object sender, EventArgs e)
    {
        catPanel.Style.Add("top", "10%");
        overlay.Visible = true;
        catPanel.Visible = true;
    }
    protected void select_category(object sender, EventArgs e)
    {
        DataTable dt = dbhelper.getdata("select * FROM asset_cat where id=" + ddl_cat.SelectedValue + "");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["serialized"].ToString() == "False")
            {
                txt_serial.Text = "-";
                txt_serial.Enabled = false;
                //qty.Value = txt_nqty.Text.Replace(",","");
                txt_nqty.Enabled = true;
            }
            else
            {
                //qty.Value = "1";
                txt_nqty.Enabled = false;
                txt_serial.Text = "";
                txt_serial.Enabled = true;
            }
        }
    }
    protected void loadable()
    {
        DataTable dt = dbhelper.getdata("select * from asset_cat where action is null");
        ddl_cat.Items.Clear();
        ddl_cat.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_cat.Items.Add(new ListItem(dr["description"].ToString(), dr["Id"].ToString()));
        }
    }
    protected void disp()
    {
        loadable();
        DataTable dt = dbhelper.getdata("select * from asset_cat");
        grid_category.DataSource = dt;
        grid_category.DataBind();

        string query = "select * from(select LEFT(convert(varchar, a.podate,101),10)podate,LEFT(convert(varchar, a.[date],101),10)[date],a.propertycode,a.price,a.brand,a.vendor,a.categoryid catid, b.inventid itemid,c.Description Category,a.serial serial,a.Description Namejud, convert(varchar,cast(round(b.qty,0)as numeric(16,0))) qty, c.um,b.status from asset_inventory a left join asset_details b on b.inventid = a.id left join asset_cat c on c.id = a.categoryid where c.serialized=1 and c.action is null and a.action is null UNION select LEFT(convert(varchar, b.podate,101),10)podate,LEFT(convert(varchar, b.[date],101),10)[date],b.propertycode,b.price,b.brand,b.vendor,a.id catid, b.id itemid, c.Description Category,  b.serial serial,b.Description Namejud, convert(varchar,(select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id)) qty,a.um, case when (select cast(round(SUM(x.qty),0)as numeric(16,0)) - (select case when SUM(qty)is null then 0 else SUM(qty) end qty from asset_assign where categoryid=a.id and action is null and status is null)  from asset_inventory z left join asset_details x on z.id=x.inventid where z.categoryid=a.id)>0 then 'On Stock' else 'Out of Stock' end status from asset_cat a left join asset_inventory b on b.categoryid = a.id left join asset_cat c on c.id = b.categoryid where a.serialized=0 and a.action is null) tt where itemid is not null";
        DataTable dt1 = dbhelper.getdata(query);
        grid_inv.DataSource = dt1;
        grid_inv.DataBind();
    }
    protected void Clicksavecategory(object sender, EventArgs e)
    {
        DataTable dtchek = dbhelper.getdata("select * from asset_cat where description='" + txt_category.Text + "'");
        if (dtchek.Rows.Count == 0)
        {
            string ser = chk_srlized.Checked == true ? "True" : "False";
            DataTable dt = dbhelper.getdata("insert into asset_cat(description,serialized,um) values('" + txt_category.Text + "','" + ser + "','" + ddl_um.SelectedItem.Text + "')select scope_identity() idd");
            using (SqlConnection con = new SqlConnection(dbconnection.conn))
            {
                using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "InsAssetInv";
                    cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = txt_category.Text;
                    cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "Insert Category";
                    cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "ID: " + dt.Rows[0]["idd"].ToString();
                    cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                    cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                    cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            disp();
            Response.Redirect("manageasset", true);
        }
        else
            lbl_err.Text = "Data Exist!";
    }
    protected bool chk()
    {
        bool err = true;
        if (ddl_cat.SelectedValue == "0")
        {
            lbl_cat.Text = "*";
            err = false;
        }
        else
            lbl_cat.Text = "";

        //if (ddl_type.Text.Trim().Length == 0)
        //{
        //    lbl_type.Text = "*";
        //    err = false;
        //}
        //else
        //    lbl_type.Text = "";

        if (txt_serial.Text.Trim().Length == 0)
        {
            lbl_serial.Text = "*";
            err = false;
        }
        else
            lbl_serial.Text = "";

        if (txt_desc.Text.Trim().Length == 0)
        {
            lbl_desc.Text = "*";
            err = false;
        }
        else
            lbl_desc.Text = "";

        //if (txt_qty.Text.Trim().Length == 0)
        //{
        //    lbl_qty.Text = "*";
        //    err = false;
        //}
        //else
        //    lbl_qty.Text = "";

        //if (txt_location.Text.Trim().Length == 0)
        //{
        //    lbl_loc.Text = "*";
        //    err = false;
        //}
        //else
        //    lbl_loc.Text = "";

        return err;
    }
    protected void clicksaveinventory(object sender, EventArgs e)
    {
        DataTable dt;
        DataTable dtchek = dbhelper.getdata("select * from asset_inventory a left join asset_details b on a.id=b.inventid where a.serial='" + txt_serial.Text + "'");
        if (Button4.Text == "Save")
        {
            if (chk())
            {
                qty.Value = txt_nqty.Enabled == true ? txt_nqty.Text.Replace(",", "") : "1";
                if (dtchek.Rows.Count == 0 || txt_serial.Text == "-")
                {
                    DataTable dts = dbhelper.getdata("select count(*) from asset_inventory");
                    dt = dbhelper.getdata("insert into asset_inventory (propertycode, vendor, brand, price, podate, date,serial,Description,categoryid)values('" + txt_propertycode.Text + "','" + txt_vendor.Text + "','" + txt_brandname.Text + "','" + txt_price.Text + "','" + txt_podate.Text + "',getdate(),'" + txt_serial.Text + "','" + txt_desc.Text + "'," + ddl_cat.SelectedValue + ") select scope_identity() id ");
                    dt = dbhelper.getdata("insert into asset_details (inventid,date,userid,qty,location,status) values (" + dt.Rows[0]["id"].ToString() + ",getdate()," + Session["user_id"].ToString() + ",'" + qty.Value + "','" + txt_location.Text + "','On Stock')");
                    using (SqlConnection con = new SqlConnection(dbconnection.conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("audittrail_master", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "InsInventory";
                            cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = ddl_cat.SelectedItem.ToString() + qty.Value;
                            cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "Insert Inventory";
                            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = txt_desc.Text;
                            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                            cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                            cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    Response.Redirect("manageasset", true);
                }
                else
                    Response.Write("<script>alert('Exist!')</script>");
            }
        }
        if (Button4.Text == "UPDATE")
        {
            //string query="update asset_inventory set Description='" + txt_desc.Text + "',categoryid=" + ddl_cat.SelectedValue + ",type='" + ddl_type.Text + "' " ;
            //        if (ddl_type.SelectedItem.Text == "PERIPHERAL")
            //            query += ",serial='Peripheral - 00" + key.Value + "'";
            //         else
            //            query += ",serial='"+txt_serial.Text+"'";
            //            query+="where id=" + key.Value + " ";
            //            dt = dbhelper.getdata(query);

            //dt = dbhelper.getdata("update asset_details set location='" + txt_location.Text + "' where inventid=" + key.Value + " ");
            //Response.Redirect("manageasset", true);
        }
    }
    protected void cancelinv(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update asset_details set status='cancel' where inventid=" + row.Cells[0].Text + "");
                Response.Redirect("manageasset", true);
            }
            else
            {
            }
        }
    }

    protected void cancel(object sender, EventArgs e)
    {
        Response.Redirect("manageasset", true);
    }

    protected void rowboundasset(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //LinkButton lnk_stat = (LinkButton)e.Row.Cells[6].FindControl("lnk_stat");
            //LinkButton LinkButton1 = (LinkButton)e.Row.Cells[7].FindControl("LinkButton1");
            //LinkButton lnk_view = (LinkButton)e.Row.Cells[7].FindControl("lnk_view");
            //if (lnk_stat.Text != "On Stock")
            //{
            //    LinkButton1.Visible = false;
            //    lnk_view.Visible = false;
            //}
            //else
            //{
            //    DataTable dt = dbhelper.getdata("select * from asset_assign where assetid="+e.Row.Cells[0].Text+" and status<>'cancel'");
            //    if (dt.Rows.Count > 0)
            //    {
            //       // LinkButton1.Visible = false;
            //       // lnk_view.Visible = false;

            //    }
            //    else
            //    {
            //       // LinkButton1.Visible = true;
            //       // lnk_view.Visible = true;
            //    }

            //}
        }
    }
    protected void click_damage(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            LinkButton lnk_stat = (LinkButton)grid_inv.Rows[row.RowIndex].Cells[6].FindControl("lnk_stat");
            if (lnk_stat.Text == "Damage")
            {
                if (TextBox1.Text == "Yes")
                {
                    dbhelper.getdata("update asset_details set status='On Stock' where inventid=" + row.Cells[0].Text + "");
                    Response.Redirect("manageasset", true);
                }
                else { }
            }
        }
    }

    protected void gridview_paging(object sender, GridViewPageEventArgs e)
    {
        grid_inv.PageIndex = e.NewPageIndex;
        disp();
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("manageasset", false);
    }
}