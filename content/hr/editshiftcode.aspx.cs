using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;

public partial class content_hr_editshiftcode : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        }
    }
    protected void disp()
    {
        string query = "select * from MShiftCode a " +
                       "where a.Id=" + Request.QueryString["shift_code_id"].ToString() + "  ";
        DataTable dt = dbhelper.getdata(query);
        if (dt.Rows.Count > 0)
        {
            txt_shiftcode.Text = dt.Rows[0]["ShiftCode"].ToString();
            txt_remarks.Text = dt.Rows[0]["Remarks"].ToString();

            DataTable dts = dbhelper.getdata("select * from MShiftCodeDay where ShiftCodeId=" + Request.QueryString["shift_code_id"].ToString() + "");
            grid_view.DataSource = dts;
            grid_view.DataBind();
        }

    }
    protected void Update(object sender, EventArgs e)
    {
        dbhelper.getdata("update MShiftCode set ShiftCode='" + txt_shiftcode.Text + "' ,remarks='" + txt_remarks.Text + "' where id=" + Request.QueryString["shift_code_id"].ToString() + "");
        Response.Redirect("editshiftcode?shift_code_id=" + Request.QueryString["shift_code_id"].ToString() + "", false);


        //string rd = chk_rd.Checked ? "True" : "False";
        //DataTable dtcheck = dbhelper.getdata("select * from MShiftCode where ShiftCode='" + txt_shiftcode.Text.Trim() + "' and id<>" + Request.QueryString["shift_code_id"].ToString() + " and status is null");
        //if (dtcheck.Rows.Count == 0)
        //{
        //    if (txt_timein1.Text.Trim().Length == 0 && txt_timeout2.Text.Trim().Length == 0)
        //    {
        //        dbhelper.getdata("update MShiftCode set ShiftCode='" + txt_shiftcode.Text + "' ,remarks='" + txt_remarks.Text + "' where id=" + Request.QueryString["shift_code_id"].ToString() + "");
        //        Response.Redirect("editshiftcode?user_id=" + function.Encrypt(user_id, true) + "&shift_code_id=" + Request.QueryString["shift_code_id"].ToString() + "", false);
        //    }
        //    else
        //    {
        //        if (txt_timein1.Text.Trim().Length == 0 || txt_timeout2.Text.Trim().Length == 0)
        //        {
        //            Response.Write("<script>alert('Empty feild Time In 1 / Time Out 2  for adding new line!')</script>");
        //        }
        //        else
        //        {
        //                DataTable checkperline = dbhelper.getdata("select * from MShiftCodeDay where Day='" + ddl_day.Text + "' and ShiftCodeId=" + Request.QueryString["shift_code_id"].ToString() + " and status is null ");
        //                dbhelper.getdata("update MShiftCode set ShiftCode='" + txt_shiftcode.Text + "' ,remarks='" + txt_remarks.Text + "' where id=" + Request.QueryString["shift_code_id"].ToString() + "");
        //                string noh= txt_noh.Text.Trim().Length == 0 ? "0" : txt_noh.Text.Trim();
        //                string fh= txt_fh.Text.Trim().Length == 0 ? "0" : txt_fh.Text.Trim();
        //                string gh = txt_gm.Text.Trim().Length == 0 ? "0" : txt_gm.Text.Trim();
        //                string nh = txt_nh.Text.Trim().Length == 0 ? "0" : txt_nh.Text.Trim();
        //                string timeout1 = txt_timeout1.Text.Trim().Length == 0 ? "0" : txt_timeout1.Text.Trim();
        //                string timein2 = txt_timein2.Text.Trim().Length == 0 ? "0" : txt_timein2.Text.Trim();
        //                if (grid_view.Rows.Count < 7 && checkperline.Rows.Count==0)
        //                {
        //                    dbhelper.getdata("insert into MShiftCodeDay (ShiftCodeId,RestDay,Day,TimeIn1,TimeOut1,TimeIn2,TimeOut2,NumberOfHours,LateFlexibility,LateGraceMinute,NightHours) values (" + Request.QueryString["shift_code_id"].ToString() + ",'" + rd + "','" + ddl_day.SelectedItem + "','" + txt_timein1.Text + "','" + timeout1 + "','" + timein2 + "','" + txt_timeout2.Text.Trim() + "'," + noh + "," + fh + "," + gh + "," + nh + ")");
        //                }
        //                Response.Redirect("editshiftcode?user_id=" + function.Encrypt(user_id, true) + "&shift_code_id=" + Request.QueryString["shift_code_id"].ToString() + "", false);
        //        }
        //    }
        //}
        //else
        //{
        //    Response.Write("<script>alert('Day Type is already exist!')</script>");
        //}
    }
    protected void cancel_line(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MShiftCodeDay set status='cancel-" + Session["user_id"] + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("editshiftcode?shift_code_id=" + Request.QueryString["shift_code_id"].ToString() + "", false);
            }
            else
            {
            }
        }
    }
    protected bool check_save_data()
    {
        bool hh = true;
        if (txt_shiftcode.Text.Trim().Length == 0)
        {
            lbl_shiftcode.Text = "*";
            hh = false;
        }
        else if (txt_remarks.Text.Trim().Length == 0)
        {
            lbl_remarks.Text = "*";
            hh = false;
        }
        return hh;
    }
    protected bool check()
    {
        bool tt = true;
        Regex timeRegex = new Regex(@"^([0]?[1-9]|[1][0-2]):([0-5][0-9]|[1-9]) [AP]M$");
        if (!timeRegex.IsMatch(txt_timein1.Text))
        {
            tt = false;
        }
        return tt;
    }
}