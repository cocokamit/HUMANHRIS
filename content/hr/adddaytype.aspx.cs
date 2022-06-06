using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class content_hr_adddaytype : System.Web.UI.Page
{
    //public static int rowindex;
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
        DataTable dtp = new DataTable();
        DataRow drp = null;

        dtp.Columns.Add(new DataColumn("col_1", typeof(string)));
      
        dtp.Columns.Add(new DataColumn("col_2", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_3", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_4", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_5", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_6", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_7", typeof(string)));


        drp = dtp.NewRow();

        drp["col_1"] = string.Empty;
    
        drp["col_2"] = string.Empty;
        drp["col_3"] = string.Empty;
        drp["col_4"] = string.Empty;
        drp["col_5"] = string.Empty;
        drp["col_6"] = string.Empty;
        drp["col_7"] = string.Empty;


        dtp.Rows.Add(drp);
        ViewState["Item_List1_deb"] = dtp;

       


    }

    
    protected void click_save_daytype(object sender, EventArgs e)
    {
        if (checkperinsert())
        {
               DataTable dtcheck = dbhelper.getdata("select * from MDayType where DayType='"+txt_daytype.Text.Trim()+"' and status is null");
                if (dtcheck.Rows.Count == 0)
                {
                    DataTable dt = dbhelper.getdata("insert into MDayType values('" + txt_daytype.Text + "' ," + txt_mow.Text + "," + txt_mor.Text + "," + txt_owm.Text + "," + txt_orm.Text + "," + txt_nwm.Text + "," + txt_nrm.Text + ",'" + Session["emp_id"].ToString() + "',getdate(),'" + Session["emp_id"].ToString() + "',getdate(), NULL) select scope_identity() id");
                }
                else
                {
                    Response.Write("<script>alert('DayType is already exist!')</script>");
                }
        }
    }
    protected bool checkperinsert()
    {
        bool perinsert = true;
        if (txt_daytype.Text.Trim().Length == 0)
        {
            lbl_dt.Text = "*";
            perinsert = false;
        }
        else if (txt_mow.Text.Trim().Length == 0)
        {
            lbl_mow.Text = "*";
            perinsert = false;
        }
        else if (txt_mor.Text.Trim().Length == 0)
        {
            lbl_mor.Text = "*";
            perinsert = false;
        }
       


        return perinsert;
    }
    
}