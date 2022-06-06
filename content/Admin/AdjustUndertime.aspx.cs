using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class content_Admin_AdjustUndertime : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp_shiftcode();
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        }
    }
    protected void disp_shiftcode()
    {
        DataTable dt = dbhelper.getdata("select * from MShiftCode where status is null");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void click_add_shiftcode(object sender, EventArgs e)
    {
        Response.Redirect("addshiftcode", false);
    }
    protected void click_viewshiftcode(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("editshiftcode?shift_code_id=" + row.Cells[0].Text + "", false);
        }
    }
    protected void click_can(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MShiftCode set status='cancel-" + Session["user_id"] + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("Mshiftcode", false);
            }
            else
            {
            }
        }
    }

    [WebMethod]
    public static string SetUT(string Id)
    {
        string result = "";

        DataTable dt = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId="+Id+"");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                DataTable ds = dbhelper.getdata("Select * from MShiftCodeDayUT where ShifTCodeDayId="+row["Id"].ToString()+"");
                result += row["Id"] + ",";
                result += row["RestDay"] + ",";
                result += row["TimeIn1"] + ",";
                result += row["TimeOut1"] + ",";
                result += row["TimeIn2"] + ",";
                result += row["TimeOut2"] + ",";
                result += row["NumberOfHours"] + ",";
                if (ds.Rows.Count > 0)
                {
                    result += ds.Rows[0]["Hrs"].ToString() + ",";
                }
                else
                {
                    result += "0"+",";
                }
                result += "~";
            }
        }
        return result;
    }

    [WebMethod]
    public static void saveUThr(string tables)
    { 
        string[] arrays=tables.Split('`');
        string query="";
        foreach (string ar in arrays)
        {
            if (ar != "")
            {
                string[] splitter = ar.Split('~');
                query += "Delete from MShiftCodeDayUT where ShiftCodeDayId=" + splitter[1] + " " +
                    "Insert into MShiftCodeDayUT values(" + splitter[1] + "," + splitter[0] + ")";
            }
        }

        if (query != "")
        {
            dbhelper.getdata(query);
        }
    }
}