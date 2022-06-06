using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.Globalization;


public partial class content_hr_newshiftcode : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            disp();
        }
    }
    protected void click_add_shiftcode(object sender, EventArgs e)
    {
        if (check())
            disp();
        else
            Response.Write("<script>alert('test')</script>");
    }
    protected void disp()
    {
        DataTable dtp = new DataTable();
        DataRow drp = null;

        dtp.Columns.Add(new DataColumn("col_1", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_rd", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_2", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_3", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_4", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_5", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_6", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_7", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_8", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_9", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_10", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_11", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_12", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_13", typeof(string)));
        dtp.Columns.Add(new DataColumn("col_14", typeof(string)));
        dtp.Columns.Add(new DataColumn("flexbreak", typeof(string)));

        drp = dtp.NewRow();

        drp["col_1"] = string.Empty;
        drp["col_rd"] = string.Empty;
        drp["col_2"] = string.Empty;
        drp["col_3"] = string.Empty;
        drp["col_4"] = string.Empty;
        drp["col_5"] = string.Empty;
        drp["col_6"] = string.Empty;
        drp["col_7"] = string.Empty;
        drp["col_8"] = string.Empty;
        drp["col_9"] = string.Empty;
        drp["col_10"] = string.Empty;
        drp["col_11"] = string.Empty;
        drp["col_12"] = string.Empty;
        drp["col_13"] = string.Empty;
        drp["col_14"] = string.Empty;
        drp["flexbreak"] = string.Empty;

        dtp.Rows.Add(drp);
        ViewState["Item_List1_deb"] = dtp;

      
        grid_view.DataBind();
    
    }
    protected bool chkinput()
    {
        bool err = true;

        DateTime timein1 = Convert.ToDateTime(txt_timein1.Text);
        DateTime timein2 = Convert.ToDateTime(txt_timein2.Text);
        DateTime timeout1 = Convert.ToDateTime(txt_timeout1.Text);
        DateTime timeout2 = Convert.ToDateTime(txt_timeout2.Text);

        if (timein1.ToString().Contains("PM"))
        {
            if (timeout1.ToString().Contains("AM"))
                timeout1 = timeout1.AddDays(1);
            if (timein2.ToString().Contains("AM"))
                timein2 = timein2.AddDays(1);
            if (timeout2.ToString().Contains("AM"))
                timeout2 = timeout2.AddDays(1);
        }
        else if (timeout1.ToString().Contains("PM"))
        {
            if (timein2.ToString().Contains("AM"))
                timein2 = timein2.AddDays(1);
            if (timeout2.ToString().Contains("AM"))
                timeout2 = timeout2.AddDays(1);
        }
        else if (timein2.ToString().Contains("PM"))
        {
            if (timeout2.ToString().Contains("AM"))
                timeout2 = timeout2.AddDays(1);
        }
        if (timeout1 <= timein1 || timein2 <= timeout1 || timein2 <= timein1 || timeout2 <= timein2 || timeout2 <= timeout1 || timeout2 <= timein1)
        {
            lbl_err.Text = "Incorrect Time Set Up!";
            err = false;
        }

        
        //if()
        //if (txt_timein1.Text.Length == 0)
        //{
        //    lbl_err.Text = "Time In 1 must be required!";
        //    err = false;
        //}
        //else
        //{
        //    if (chk_broken.Checked == true)
        //    {
        //        if (txt_timeout1.Text.Length == 0)
        //        {
        //            lbl_err.Text = "Time Out 1 must be required!";
        //            err = false;
        //        }
        //        else if (txt_timeout1.Text.Length>0)
        //        { 

        //        }

        //        if (txt_timein2.Text.Length == 0)
        //        {
        //            lbl_err.Text = "Time In 2 must be required!";
        //            err = false;
        //        }

        //    }
        //}
        //if (txt_timeout2.Text.Length == 0)
        //{

        //    lbl_err.Text = "Time Out 2 must be required!";
        //    err = false;
        //}


        return err;
    }
    protected void add_into_datatable(object sender, EventArgs e)
    {
        DataTable dtCurrentTablep = (DataTable)ViewState["Item_List1_deb"];
        DataRow drp = null;
        int rowindex = 0;
        string naa = string.Empty;
        decimal noh = 0;
        foreach (DataRow dr in dtCurrentTablep.Rows)
        {
            if (dr["col_1"].ToString() == ddl_day.Text)
            {
                naa = "naaysulod";
            }
        }
        if (dtCurrentTablep.Rows.Count>0)
        {
            if (dtCurrentTablep.Rows[0][0].ToString() == string.Empty)
                dtCurrentTablep.Rows.Remove(dtCurrentTablep.Rows[0]);
        }
        if (btn_add.Text == "ADD")
        {
            if (check_broken())
            {
                if (dtCurrentTablep.Rows.Count <= 6)
                {
                    if (naa == string.Empty)
                    {
                        TimeSpan firsthalfthours = new TimeSpan();
                        TimeSpan secondhalfthours = new TimeSpan();
                        TimeSpan tnod = new TimeSpan();
                        DateTime get1in = Convert.ToDateTime(txt_timein1.Text);
                        DateTime get1out = Convert.ToDateTime(txt_timeout1.Text.Length > 0 ? txt_timeout1.Text : "0:00:00");
                        DateTime get2in = Convert.ToDateTime(txt_timein2.Text.Length > 0 ? txt_timein2.Text : "0:00:00");
                        DateTime get2out = Convert.ToDateTime(txt_timeout2.Text);
                        string tnhours = "0";
                        string sth = "0";
                        string ndh = "0";
                        string mid = "0";
                        string nightdeduct = "0";
                        DateTime startnightdiff = Convert.ToDateTime("22:00");
                        DateTime endnightdiff = Convert.ToDateTime("06:00");
                        if (txt_timeout1.Text.Length == 0 || txt_timein2.Text.Length == 0)
                        {
                            //if (get1in >= startnightdiff || get1in <= endnightdiff)
                            //    tnhours = "1";
                            if (get1in.ToString().Contains("PM"))
                            {
                                if (get2out.ToString().Contains("AM"))
                                {
                                    get2out=get2out.AddDays(1);
                                    tnhours = "1";
                                }
                            }
                        }
                        else
                        {
                            if (get1in.ToString().Contains("PM"))
                            {
                                if (get1out.ToString().Contains("AM"))
                                {
                                    get1out = get1out.AddDays(1);
                                    tnhours = "1";
                                }
                                if (get2in.ToString().Contains("AM"))
                                {
                                    get2in = get2in.AddDays(1);
                                    tnhours = "1";
                                }
                                if (get2out.ToString().Contains("AM"))
                                {
                                    get2out = get2out.AddDays(1);
                                    tnhours = "1";
                                }
                            }
                            else if (get1out.ToString().Contains("PM"))
                            {
                                if (get2in.ToString().Contains("AM"))
                                {
                                    get2in = get2in.AddDays(1);
                                    tnhours = "1";
                                }
                                if (get2out.ToString().Contains("AM"))
                                {

                                    get2out = get2out.AddDays(1);
                                    tnhours = "1";
                                }
                            }
                            else if (get2in.ToString().Contains("PM"))
                            {
                                if (get2out.ToString().Contains("AM"))
                                {
                                    get2out = get2out.AddDays(1);
                                    tnhours = "1";
                                }
                            }
                        }


                        if (txt_timeout1.Text.Length == 0 || txt_timein2.Text.Length == 0)
                        {
                            firsthalfthours = get2out - get1in;
                            noh = decimal.Parse(firsthalfthours.TotalHours.ToString());
                        }
                        else
                        {
                            firsthalfthours = get1out - get1in;
                            secondhalfthours = get2out - get2in;
                            noh=decimal.Parse(firsthalfthours.TotalHours.ToString())+decimal.Parse(secondhalfthours.TotalHours.ToString());
                        }
                        string nohb = txt_nohb.Text.Length>0?txt_nohb.Text:"0";
                        noh = noh - int.Parse(nohb) / 60;
                        tnod = get2in - get1out;
                        sth = getnightdif.getnight(get1in.ToString(), get1out.ToString(), "shiftcode");
                        ndh = getnightdif.getnight(get2in.ToString(), get2out.ToString(), "shiftcode");
                        mid = getnightdif.getnight(get1in.ToString(), get2out.ToString(), "shiftcode");
                        nightdeduct = getnightdif.getnight(get1out.ToString(), get2in.ToString(), "shiftcode");
                        //decimal noh = txt_timein1.Text.Length > 0 && txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0 && txt_timeout2.Text.Length > 0 ? decimal.Parse(firsthalfthours.TotalHours.ToString()) + decimal.Parse(secondhalfthours.TotalHours.ToString()) : decimal.Parse(tnod.TotalHours.ToString());
                      
                        if (chk_ibwp1.Checked == true)
                        {
                            noh = noh + decimal.Parse(tnod.TotalHours.ToString());
                            noh = Math.Round(noh, 1);
                        }
                        if (noh.ToString().Contains('-'))
                            Response.Write("<script>alert('Invalid No. of hours!')</script>");
                        else
                        {
                            if (noh > 0)
                            {
                                DataRow[] dtcurr = dtCurrentTablep.Select("col_1='Not Fix'");
                                if (dtcurr.Count() > 0)
                                {

                                }
                                else
                                {
                                    string[] in1 = get1in.ToString("MM/dd/yyyy hh:mm tt").Split(' ');
                                    string[] out1 = get1out.ToString("MM/dd/yyyy hh:mm tt").Split(' ');
                                    string[] in2 = get2in.ToString("MM/dd/yyyy hh:mm tt").Split(' ');
                                    string[] out2 = get2out.ToString("MM/dd/yyyy hh:mm tt").Split(' ');
                                    drp = dtCurrentTablep.NewRow();
                                    drp["col_1"] = chk_sched.Checked == true ? ddl_day.Text : "Not Fix"; //day
                                    drp["col_rd"] = chk_rd.Checked == true ? "True" : "False";//rd
                                    drp["col_2"] = in1[1] + " " + in1[2]; //time in
                                    drp["col_3"] = txt_timeout1.Text.Length > 0 ? out1[1] + " " + out1[2] : "0:00:00"; //time out 1
                                    drp["col_4"] = txt_timein2.Text.Length > 0 ? in2[1] + " " + in2[2] : "0:00:00"; //time in 2
                                    drp["col_5"] = out2[1] + " " + out2[2];//time out 2
                                    drp["col_6"] = noh;
                                    drp["col_7"] = txt_fh.Text.Trim().Length == 0 ? "0" : txt_fh.Text.Trim();
                                    drp["col_8"] = txt_gm.Text.Trim().Length == 0 ? "0" : txt_gm.Text.Trim();
                                    drp["col_9"] = "0"; //txt_timein1.Text.Length > 0 && txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0 && txt_timeout2.Text.Length > 0 ? decimal.Parse(sth) + decimal.Parse(ndh) : decimal.Parse(mid);
                                    drp["col_10"] = "0";//tnhours;//if night shift
                                    drp["col_11"] = txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0 ? tnod.TotalHours.ToString() : (int.Parse(nohb) / 60).ToString();//night breakhours
                                    drp["col_12"] = chk_ibwp1.Checked == true ? "True" : "False"; //paid break
                                    string mbp = "False";
                                    if ((txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0) || chk_flexbreak.Checked == true)
                                        mbp = "True";
                                    drp["col_13"] = mbp; //mandatory break punch
                                    drp["flexbreak"] = chk_flexbreak.Checked == true ? "True" : "False";
                                    dtCurrentTablep.Rows.Add(drp);
                                }
                            }
                            else
                                lbl_err.Text = "Total Hours must not be ZERO!";
                        }
                    }
                }
            }
        }
        ViewState["Item_List1_deb"] = dtCurrentTablep;
        grid_view.DataSource = dtCurrentTablep;
        grid_view.DataBind();
        if (grid_view.Rows.Count > 0)
            btn_save.Visible = true;
        else
            btn_save.Visible = false;
    }
    protected void click_break_setup(object sender, EventArgs e)
    {
        txt_timeout1.Text="";
        txt_timein2.Text = "";
        chk_ibwp1.Checked = false;
        //if (chk_break_setup.Checked == true)
        //{
        //    break_id.Style.Add("display", "block");
        //}
        //else
        //{
        //    break_id.Style.Add("display", "none");
        //}
    }
    protected void click_break_flex(object sender, EventArgs e)
    {
       // chk_break_setup.Checked = false;
        txt_timeout1.Text = "";
        txt_timein2.Text = "";
        txt_nohb.Text = "";
        if (chk_flexbreak.Checked == true)
        {
            break_id.Visible = false;
            nomb.Visible = true;
            //chk_break_setup.Checked = true;
            //chk_break_setup.Enabled = false;
        }
        else
        {
            break_id.Visible = true;
            nomb.Visible = false;
            //chk_break_setup.Checked = false;
            //chk_break_setup.Enabled = true;
          
        }
    }
    protected void click_pb(object sender, EventArgs e)
    {
        txt_nohb.Text = "";
        if (chk_ibwp1.Checked == true)
            nomb.Visible = false;
        else
            nomb.Visible = true;

        //if (txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0)
        //    nomb.Visible = false;
        //else
        //    nomb.Visible = true;
    }
    protected void click_bi(object sender, EventArgs e)
    {
       // txt_nohb.Text = "";
        if (txt_timeout1.Text.Length > 0 && txt_timein2.Text.Length > 0)
        {
            nomb.Visible = false;
            txt_nohb.Text = "";
        }
        else
            nomb.Visible = true;
    }
    protected void grid_viewrowbound(object sender, GridViewRowEventArgs e)
    {

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    LinkButton lnk_view = (LinkButton)e.Row.FindControl("LinkButton1");
        //    lnk_view.Visible = false;
        //}
    }
    protected void edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
          int rowindex = row.RowIndex;
            ddl_day.Text=row.Cells[0].Text;
            if (row.Cells[1].Text == "False")chk_rd.Checked = false; else chk_rd.Checked = true;
            txt_timein1.Text = row.Cells[2].Text;
            txt_timeout1.Text = row.Cells[3].Text;
            txt_timein2.Text = row.Cells[4].Text;
            txt_timeout2.Text = row.Cells[5].Text;
         //   txt_noh.Text = row.Cells[6].Text;
            txt_fh.Text=row.Cells[7].Text;
            txt_gm.Text = row.Cells[8].Text;
            txt_nh.Text = row.Cells[9].Text;

            btn_add.Text = "EDIT";

            DataTable dt = (DataTable)ViewState["Item_List1_deb"];
            grid_view.DataSource = dt;
            grid_view.DataBind();

            LinkButton lb = (LinkButton)grid_view.Rows[row.RowIndex].FindControl("LinkButton1");
            lb.Visible = true;
        
        }
    }
    protected void delete(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {

            DataTable dt = (DataTable)ViewState["Item_List1_deb"];
            if (grid_view.Rows.Count == 1)
            {
                dt.Rows.Remove(dt.Rows[row.RowIndex]);
                disp();
            }
            else 
            {
                dt.Rows.Remove(dt.Rows[row.RowIndex]);
            }
            grid_view.DataSource = dt;
            grid_view.DataBind();

           
        }
    }
    protected void cancel(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
             ddl_day.Text = string.Empty;
             chk_rd.Checked = false;
             txt_timein1.Text = string.Empty;
             txt_timeout1.Text = string.Empty;
             txt_timein2.Text = string.Empty;
             txt_timeout2.Text = string.Empty;
            // txt_noh.Text = string.Empty;
             txt_fh.Text = string.Empty;
             txt_gm.Text = string.Empty;
             txt_nh.Text = string.Empty;

            btn_add.Text = "ADD";

            DataTable dt = (DataTable)ViewState["Item_List1_deb"];
            grid_view.DataSource = dt;
            grid_view.DataBind();

            LinkButton lb = (LinkButton)grid_view.Rows[row.RowIndex].FindControl("LinkButton1");
            lb.Visible = false;
        }
    }

    protected void click_add_shiftcodes(object sender, EventArgs e)
    {
        if (check_save_data())
        {
            string ffix = chk_sched.Checked == true ? "1" : "0";
            DataTable dt_check = dbhelper.getdata("select * from MShiftCode where ShiftCode='" + txt_shiftcode.Text + "' and status is null ");
            DataTable dt = dbhelper.getdata("insert into MShiftCode values('" + txt_shiftcode.Text.Trim() + "','" + txt_remarks.Text.Trim() + "'," + Session["user_id"] + ",getdate(),NULL,NULL," + ffix + ") select scope_identity() id ");
            if (chk_sched.Checked == true)
            {
                if (dt_check.Rows.Count == 0)
                {
                    if (grid_view.Rows.Count == 7)
                    {
                        string kkl = "0";
                        //DataTable dtss = (DataTable)ViewState["Item_List1_deb"];
                        //foreach (GridViewRow dr in grid_view.Rows)
                        //{
                        //    string row3 = dr.Cells[3].Text == "0" ? "0:00:00" : dr.Cells[3].Text;
                        //    string row4 = dr.Cells[4].Text == "0" ? "0:00:00" : dr.Cells[4].Text;
                        //    dbhelper.getdata("insert into MShiftCodeDay (ShiftCodeId,RestDay,Day,TimeIn1,TimeOut1,TimeIn2,TimeOut2,NumberOfHours,LateFlexibility,LateGraceMinute,NightHours,nighshift,status,nightbreakhours,breakwithpay,mandatorytopunch,flexbreak)  " +
                        //                    " values " +
                        //                    "(" + dt.Rows[0]["id"].ToString() + ",'" + dr.Cells[1].Text + "','" + dr.Cells[0].Text + "','" + dr.Cells[2].Text + "','" + row3 + "','" + row4 + "','" + dr.Cells[5].Text + "','" + dr.Cells[6].Text + "','" + dr.Cells[7].Text + "','" + dr.Cells[8].Text + "','" + dr.Cells[9].Text + "','" + dr.Cells[10].Text + "',NULL,'" + dr.Cells[11].Text + "','" + dr.Cells[12].Text + "','" + dr.Cells[13].Text + "','" + dr.Cells[14].Text + "')");

                        //    Response.Redirect("Mshiftcode");
                        //}

                        DataTable dtss = (DataTable)ViewState["Item_List1_deb"];

                        foreach (DataRow dr in dtss.Rows)
                        {
                            dbhelper.getdata("insert into MShiftCodeDay(ShiftCodeId,RestDay,[Day],TimeIn1,TimeOut1,TimeIn2,TimeOut2,NumberOfHours,LateFlexibility,LateGraceMinute,NightHours,nighshift,nightbreakhours,breakwithpay,mandatorytopunch,flexbreak)"
                                                + "values(" + dt.Rows[0]["id"].ToString() + ",'" + dr["col_rd"].ToString() + "','" + dr["col_1"].ToString() + "','" + dr["col_2"].ToString() + "','" + dr["col_3"].ToString() + "','" + dr["col_4"].ToString() + "',"
                                                + "'" + dr["col_5"].ToString() + "','" + dr["col_6"].ToString() + "','" + dr["col_7"].ToString() + "','" + dr["col_8"].ToString() + "','" + dr["col_9"].ToString() + "','" + dr["col_10"].ToString() + "',"
                                                + "'" + dr["col_11"].ToString() + "','" + dr["col_13"].ToString() + "','" + dr["col_13"].ToString() + "','" + dr["flexbreak"].ToString() + "')");
                        }
                        Response.Redirect("Mshiftcode");
                    }
                    else
                    {
                        Response.Write("<script>alert('Pls. Complete the 7 days set up.')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Shift Code is already exist!')</script>");
                }
            }
            else
            {
                if (dt_check.Rows.Count == 0)
                {
                    if (grid_view.Rows.Count > 0)
                    {
                        string kkl = "0";
                        foreach (GridViewRow dr in grid_view.Rows)
                        {
                            string row3 = dr.Cells[3].Text == "0" ? "0:00:00" : dr.Cells[3].Text;
                            string row4 = dr.Cells[4].Text == "0" ? "0:00:00" : dr.Cells[4].Text;
                            dbhelper.getdata("insert into MShiftCodeDay (ShiftCodeId,RestDay,Day,TimeIn1,TimeOut1,TimeIn2,TimeOut2,NumberOfHours,LateFlexibility,LateGraceMinute,NightHours,nighshift,status,nightbreakhours,breakwithpay,mandatorytopunch,flexbreak)  " +
                                            " values " +
                                            "(" + dt.Rows[0]["id"].ToString() + ",'" + dr.Cells[1].Text + "','" + dr.Cells[0].Text + "','" + dr.Cells[2].Text + "','" + row3 + "','" + row4 + "','" + dr.Cells[5].Text + "','" + dr.Cells[6].Text + "','" + dr.Cells[7].Text + "','" + dr.Cells[8].Text + "','" + dr.Cells[9].Text + "','" + dr.Cells[10].Text + "',NULL,'" + dr.Cells[11].Text + "','" + dr.Cells[12].Text + "','" + dr.Cells[13].Text + "','" + dr.Cells[14].Text + "')");

                            Response.Redirect("Mshiftcode");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('No Data to save.')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Shift Code is already exist!')</script>");
                }
            }


        }
    }
    protected bool check_broken()
    {
            bool err = true;
            if (txt_timein1.Text.Length == 0)
            {
                lbl_err.Text = "Time In 1 Invalid Input!";
                err = false;
            }
            else if (txt_timeout2.Text.Length == 0)
            {
                lbl_err.Text = "Time Out 2 Invalid Input!"; 
                err = false;
            }
            //else if (txt_timein2.Text.Length == 0)
            //{
            //    lbl_err.Text = "In From Break Invalid Input!";
            //    err = false;
            //}
            //else if (txt_timeout1.Text.Length == 0)
            //{
            //    lbl_err.Text = "Out For Break Invalid Input!";
            //    err = false;
            //}
          
            return err;
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
    protected void change(object sender, EventArgs e)
    {
        if (chk_sched.Checked == true)
        {
            fix.Style.Add("display", "block");
            disp();
        }
        else
        {
            fix.Style.Add("display", "none");
            disp();
        }
    }
}