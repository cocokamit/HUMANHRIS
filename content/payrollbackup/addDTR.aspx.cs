using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Human;


public partial class content_payroll_DTR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {
           
  
            load2x();
            autoperiod();
            pay_range();
            //user_id.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
     
            if (grid_item.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");
            }
        }
    }
    protected void pay_range()
    {
        
        string query = "select a.id,left(convert(varchar,a.dfrom,101),10)ffrom,left(convert(varchar,a.dtoo,101),10) tto from payroll_range a " +
                        "where  " +
                        "(select COUNT(*) from tdtr where left(convert(varchar,DateStart,101),10)+'-'+left(convert(varchar,DateEnd,101),10)=left(convert(varchar,a.dfrom,101),10)+'-'+left(convert(varchar,a.dtoo,101),10)  and  status is null and payrollgroupid="+ddl_pg.SelectedValue+" )=0 and " +
                        "a.action is null  order by a.dfrom desc"; 
        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
        ddl_pay_range.Items.Add(new ListItem("Select Payroll Range", "0"));
        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }
    }
    protected void autoperiod()
    {
        //DataTable dtperiod = adjustdtrformat.payrollperiod("all",ddl_pg.SelectedValue);
        //txt_f.Text=dtperiod.Rows[0]["ffrom"].ToString();
        //txt_t.Text = dtperiod.Rows[0]["tto"].ToString();
    }
    protected void selectpg(object sender, EventArgs e)
    {
        pay_range();
    }
    protected void load2x()
    {
        string query = "select * from MPayrollGroup where status<>'0' order by id desc";
        DataTable dt = dbhelper.getdata(query);

        ddl_pg.Items.Clear();
        //ddl_pg.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddl_pg.Items.Add(new ListItem(dr["PayrollGroup"].ToString(), dr["id"].ToString()));
        }
    }
    private void gridviewrow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("col_1", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["col_1"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["Item_List"] = dt;
        grid_item.DataSource = dt;
        grid_item.DataBind();
    }
    protected void payroll_verify(object sender, EventArgs e)
    {
        switch (btn_go.Text)
        {
            case "GO":
                if (ddl_pay_range.SelectedValue != "0")
                {
                    string[] payrange = ddl_pay_range.SelectedValue.ToString().Split('-');
                    string txtf = payrange[0];
                    string txtt = payrange[1];
                    if (txtf.Trim().Length == 0 || txtt.Trim().Length == 0)
                        Label1.Text = "*";
                    else
                    {
                        DataSet ds = new DataSet();
                        stateclass sc = new stateclass();
                        sc.sa = txtf.Replace(" ", "");
                        sc.sb = txtt.Replace(" ", "");
                        sc.sc = ddl_pg.SelectedValue;
                        string x = bol.payroll_verify(sc);
                        if (x == "1")
                        {
                            DataTable master = Core.DTRF(txtf, txtt, ddl_pg.SelectedValue);

                            ViewState["master"] = master;
                            grid_item.DataSource = master;
                            grid_item.DataBind();
                            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");
                            if (grid_item.Rows.Count > 0)
                            {
                                procdtr.Visible = true;
                                tbl1.Visible = true;
                            }
                            else
                            {
                                procdtr.Visible = false;
                                tbl1.Visible = false;
                            }
                            ddl_pay_range.Enabled = false;
                            ddl_pg.Enabled = false;
                            btn_go.Text = "Change";
                        }
                        else
                        {
                            grid_item.DataBind();
                            Label1.Text = "Invalid range";
                            if (grid_item.Rows.Count > 0)
                            {
                                procdtr.Visible = true;
                                tbl1.Visible = true;
                            }
                            else
                            {
                                procdtr.Visible = false;
                                tbl1.Visible = false;
                            }
                        }
                    }
                }
                else
                    Response.Redirect("addDTR");
                break;
            case "Change":
                grid_item.DataSource = null;
                grid_item.DataBind();
                ddl_pg.Enabled = true;
                ddl_pay_range.Enabled = true;
                btn_go.Text = "GO";
                break;
        }
        
    }
 
    protected void load_dtrfile()
    {
        string query = "select id,LEFT(CONVERT(varchar,[datestart],101),10)[datestart],LEFT(CONVERT(varchar,[dateend],101),10)[dateend]  from tdtrperpayrol where status like'%Approved%' and dtr_id is null and payrollgroupid='" + ddl_pg.SelectedValue + "'";
        DataTable getfrombio = dbhelper.getdata(query);
        ddl_dtrfile.Items.Clear();
        ddl_dtrfile.Items.Add(new ListItem("None", "0"));
        foreach (DataRow dr in getfrombio.Rows)
        {
            ddl_dtrfile.Items.Add(new ListItem(dr["datestart"].ToString() + " - " + dr["dateend"].ToString(), dr["id"].ToString()));
        }
    }
    protected void loadable_inside_grid(int rcnt)
    {
        //get emplooyee
        string query = "select a.Id,a.lastname+', '+a.firstname+' '+ a.middlename+' '+a.extensionname as Fullname from MEmployee a " +
                        "left join MPosition b on a.PositionId=b.Id where a.PayrollGroupId=" + ddl_pg.SelectedValue + "";
        DataTable dt = dbhelper.getdata(query);
        DropDownList ddl_emp = (DropDownList)grid_item.Rows[rcnt].Cells[1].FindControl("ddl_emp");
        ddl_emp.Items.Add(" ");
        foreach (DataRow dr in dt.Rows)
        {
            ddl_emp.Items.Add(new ListItem(dr["Fullname"].ToString(), dr["id"].ToString()));
        }

        // get shiftcode
        query = "select ShiftCode,Id  from MShiftCode  where status is null ";
        DataTable dtshiftcode = dbhelper.getdata(query);
        DropDownList ddl_shiftcode = (DropDownList)grid_item.Rows[rcnt].Cells[2].FindControl("ddl_shiftcode");
        ddl_shiftcode.Items.Add(" ");
        foreach (DataRow dr in dtshiftcode.Rows)
        {
            ddl_shiftcode.Items.Add(new ListItem(dr["ShiftCode"].ToString(), dr["id"].ToString()));
        }

        // get daytype
        query = "select DayType,Id  from MDayType  where status is null ";
        DataTable dtdaytype = dbhelper.getdata(query);
        DropDownList ddl_daytype = (DropDownList)grid_item.Rows[rcnt].Cells[4].FindControl("ddl_type");
        ddl_daytype.Items.Add(" ");
        foreach (DataRow dr in dtdaytype.Rows)
        {
            ddl_daytype.Items.Add(new ListItem(dr["DayType"].ToString(), dr["id"].ToString()));
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        int x = addnewrow();
    }
    protected void setPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["Item_List"] != null)
        {
            DataTable dt = (DataTable)ViewState["Item_List"];

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txt_parts = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_parts");
                    TextBox txt_description = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_description");
                    TextBox txt_quantity = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_quantity");
                    TextBox txt_price = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_price");
                    TextBox txt_tprice = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_tprice");

                    Label lbl_parts = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_parts");
                    Label lbl_description = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_description");
                    Label lbl_quantity = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_quantity");
                    Label lbl_price = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_price");
                    Label lbl_tprice = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_tprice");

                    grid_item.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    txt_parts.Text = dt.Rows[i]["col_1"].ToString();
                    txt_description.Text = dt.Rows[i]["col_2"].ToString();
                    txt_quantity.Text = dt.Rows[i]["col_3"].ToString();
                    txt_price.Text = dt.Rows[i]["col_4"].ToString();
                    txt_tprice.Text = dt.Rows[i]["col_5"].ToString();

                    if (txt_parts.Text != "")
                    {
                        txt_parts.Attributes.Add("readonly", "");
                        txt_description.Attributes.Add("readonly", "");
                        txt_quantity.Attributes.Add("readonly", "");
                    }

                    rowIndex++;
                }
            }
        }
    }
    protected int addnewrow()
    {
        int rowIndex = 0;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List"];
        DataRow drCurrentRow = null;

        if (dtCurrentTable.Rows.Count > 0)
        {
            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
            {
                TextBox txt_parts = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_parts");
                TextBox txt_description = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_description");
                TextBox txt_quantity = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_quantity");
                TextBox txt_price = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_price");
                TextBox txt_tprice = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_tprice");

                Label lbl_parts = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_parts");
                Label lbl_description = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_description");
                Label lbl_quantity = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_quantity");
                Label lbl_price = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_price");
                Label lbl_tprice = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_tprice");

                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                dtCurrentTable.Rows[i - 1]["col_1"] = txt_parts.Text.Replace(" ", "");
                dtCurrentTable.Rows[i - 1]["col_2"] = txt_description.Text.Trim();
                dtCurrentTable.Rows[i - 1]["col_3"] = txt_quantity.Text.Replace(" ", "");
                dtCurrentTable.Rows[i - 1]["col_4"] = txt_price.Text.Replace(" ", "");
                dtCurrentTable.Rows[i - 1]["col_5"] = txt_tprice.Text.Replace(" ", "");

                if (i == dtCurrentTable.Rows.Count)
                {
                    lbl_description.Text = txt_description.Text.Trim().Length == 0 ? "empty" : "";
                    lbl_quantity.Text = txt_quantity.Text.Replace(" ", "").Length == 0 ? "empty" : "";
                    lbl_price.Text = txt_price.Text.Replace(" ", "").Length == 0 ? "empty" : "";
                    lbl_tprice.Text = txt_tprice.Text.Replace(" ", "").Length == 0 ? "empty" : "";
                }
                if ((lbl_tprice.Text + lbl_price.Text + lbl_description.Text + lbl_quantity.Text + lbl_parts.Text).Contains("empty") || lbl_parts.Text == "exists")
                    goto exit;
                rowIndex++;
            }
            dtCurrentTable.Rows.Add(drCurrentRow);
            ViewState["Item_List"] = dtCurrentTable;

            grid_item.DataSource = dtCurrentTable;
            grid_item.DataBind();
        }
        setPreviousData();
        return 1;
    exit:
        return rowIndex;
    }
    public static double ddf(DateTime d1, DateTime d2)
    {
        TimeSpan t = d1 - d2;
        double NrOfDays = t.TotalHours;
        return NrOfDays;
    }
    
    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[10].Text == "True")
            {
                e.Row.Cells[10].Text = "✓";
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[10].Text = "--";

            if (e.Row.Cells[11].Text == "True")
            {
                e.Row.Cells[11].Text = "✓";
                e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[11].Text = "--";

            if (e.Row.Cells[12].Text == "True")
            {
                e.Row.Cells[12].Text = "✓";
                e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[12].Text = "--";

            if (e.Row.Cells[13].Text == "True")
            {
                e.Row.Cells[13].Text = "✓";
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            }
            else
                e.Row.Cells[13].Text = "--";
        }
    }
    protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView gridView = sender as GridView;

            GridViewRow gridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell tableCell = new TableCell();
            tableCell.Text = "";
            tableCell.ColumnSpan = 4;
            tableCell.Attributes.Add("style", "background-color:#333;  ");
            gridViewRow.Cells.Add(tableCell);

            TableCell tableDAYS = new TableCell();
            tableDAYS.Text = "DAY";
            tableDAYS.ColumnSpan = 2;
            tableDAYS.Attributes.Add("style", "background-color:#333; color:white;  ");
            gridViewRow.Cells.Add(tableDAYS);

            TableCell test1 = new TableCell();
            test1.Text = "";
            test1.ColumnSpan = 3;
            test1.Attributes.Add("style", "background-color:#333; color:white;");
            gridViewRow.Cells.Add(test1);

            TableCell DTR_STATUS = new TableCell();
            DTR_STATUS.Text = "DTR STATUS";
            DTR_STATUS.ColumnSpan = 2;
            DTR_STATUS.Attributes.Add("style", "background-color:#333; color:white;");
            gridViewRow.Cells.Add(DTR_STATUS);

            TableCell tableworkhours = new TableCell();
            tableworkhours.Text = "WORK HOURS";
            tableworkhours.ColumnSpan = 5;
            tableworkhours.Attributes.Add("style", "background-color:#333; color:white;");
            gridViewRow.Cells.Add(tableworkhours);

            TableCell tabletardy = new TableCell();
            tabletardy.Text = "Tardy Hours";
            tabletardy.ColumnSpan = 2;
            tabletardy.Attributes.Add("style", "background-color:#333; color:white;  ");
            gridViewRow.Cells.Add(tabletardy);

            TableCell tablenethours = new TableCell();
            tablenethours.Text = "";
            tablenethours.ColumnSpan = 1;
            tablenethours.Attributes.Add("style", "background-color:#333; color:white;  ");
            gridViewRow.Cells.Add(tablenethours);

            TableCell tablerateperhour = new TableCell();
            tablerateperhour.Text = "Rate Per HOURS";
            tablerateperhour.ColumnSpan = 4;
            tablerateperhour.Attributes.Add("style", "background-color:#333; color:white;");
            gridViewRow.Cells.Add(tablerateperhour);

            TableCell tableamount = new TableCell();
            tableamount.Text = "Amount";
            tableamount.ColumnSpan = 4;
            tableamount.Attributes.Add("style", "background-color:#333; color:white;");
            gridViewRow.Cells.Add(tableamount);
            // add the new row to the gridview
            gridView.Controls[0].Controls.AddAt(0, gridViewRow);
        }
    }
    protected void click_save_DTR(object sender, EventArgs e)
    {
        DataTable dttt = (DataTable)ViewState["master"];
        DataTable dt = dbhelper.getdata("select * from mperiod where period=" + DateTime.Now.Year + "");
        //DataTable dtemp = dbhelper.getdata("select * from memployee");
        if (ddl_pay_range.SelectedValue != "0")
        {
            string[] payrange = ddl_pay_range.SelectedValue.ToString().Split('-');
            string txtf = payrange[0];
            string txtt = payrange[1];
            stateclass sc = new stateclass();
            //define insert tdtr table
            sc.sa = dt.Rows[0]["id"].ToString(); //period
            sc.sb = ddl_pg.SelectedValue;//payroll group
            sc.sc = txtf.Trim(); //date start
            sc.sd = txtt.Trim(); //date end
            sc.se = "0";//ddl_overtime.SelectedValue;//oevrtime
            sc.sf = "0";//ddl_leave.SelectedValue;//leave
            sc.sg = "0";//changeshift
            sc.sh = txt_remarks.Text;//remarks
            sc.si = Session["user_id"].ToString();// user_id.Value;//user_id
            string dtrmain = bol.dtrmain(sc);

            //define insert dtrline table
            int i = 0;
            foreach (DataRow dr in dttt.Rows)
            {
                string[] getdate = dr["date"].ToString().Split('-');// lbl_date.Text.Trim().Split('-');
                string[] dateformat = getdate[0].Split('/');
                string mm = dateformat[0].ToString().Length > 1 ? dateformat[0] : "0" + "" + dateformat[0].ToString();
                string dd = dateformat[1].ToString().Length > 1 ? dateformat[1] : "0" + "" + dateformat[1].ToString();
                //DataRow[] dremp = dtemp.Select("id=" + dr["empid"].ToString() + "");
                DataTable dtmealallowance = dbhelper.getdata("select * from mealallowance where empid=" + dr["empid"].ToString() + " and action is null order by id desc");
                string mealallowance = dtmealallowance.Rows.Count > 0 ? dtmealallowance.Rows[0]["amt"].ToString() : "0";
               
                sc.sa = dtrmain; //DTRId
                sc.sb = dr["empid"].ToString();//ddl_emp.SelectedValue; //EmployeeId
                sc.sc = dr["shiftcodeid"].ToString();//ddl_shiftcode.SelectedValue; //ShiftCodeId
                sc.sd = mm + "/" + dd + "/" + dateformat[2]; //getdate[0];//date
                sc.se = dr["daytypeid"].ToString();//ddl_type.SelectedValue; //DayTypeId
                sc.sf = dr["dm"].ToString();//lbl_dm.Text; //DayMultiplier
                sc.sg = dr["restday"].ToString();//chk_rd.Checked ? "True" : "False";//RestDay
                sc.sh = dr["timein1"].ToString();//lbl_timein1.Text; //TimeIn1
                sc.si = dr["timeout2"].ToString();//lbl_timeout2.Text;//TimeOut2
                sc.sj = dr["olw"].ToString();//chk_ol.Checked ? "True" : "False";//OnLeave
                sc.sjj = dr["olh"].ToString();//chk_olH.Checked == true ? "True" : "False";//onleave halfday
                sc.sk = dr["aw"].ToString();//chk_a.Checked ? "True" : "False";//Absent
                sc.sii = dr["ah"].ToString();//chk_aH.Checked == true ? "True" : "False";// absent halfday
                sc.sl = dr["reg_hr"].ToString().Replace(",", ""); ;//lbl_reg_workhours.Text;//RegularHours
                sc.sqq = dr["offsethrs"].ToString().Replace(",", ""); ;//@totaloffsethrs
                sc.sm = dr["night"].ToString().Replace(",", ""); //lbl_night.Text;//NightHours
                sc.sn = dr["ot"].ToString().Replace(",", ""); // lbl_ot.Text;//OvertimeHours
                sc.so = dr["otn"].ToString().Replace(",", "");// lbl_otn.Text;//OvertimeNightHours
                sc.sp = dr["totalhrs"].ToString().Replace(",", "");//lbl_total.Text;//GrossTotalHours
                sc.sq = dr["late"].ToString().Replace(",", "");//lbl_late.Text;//TardyLateHours
                sc.sr = dr["ut"].ToString().Replace(",", "");// lbl_ut.Text;//TardyUndertimeHours
                sc.ss = dr["nethours"].ToString().Replace(",", "");// lbl_nethours.Text;//NetTotalHour
                sc.st = dr["hrrate"].ToString().Replace(",", "");// lbl_regrateperhour.Text;//RatePerHour
                sc.su = dr["nightrate"].ToString().Replace(",", "");// lbl_nightrateperhour.Text;//RatePerNightHour
                sc.sv = dr["otrate"].ToString().Replace(",", "");// lbl_otrateperhour.Text;//RatePerOvertimeHour
                sc.sw = dr["otnrate"].ToString().Replace(",", "");// lbl_otnrateperhour.Text;//RatePerOvertimeNightHour
                sc.sx = dr["reg_amount"].ToString().Replace(",", "");//lbl_regamount.Text;//RegularAmount
                sc.sy = dr["night_amount"].ToString().Replace(",", "");//lbl_nightamount.Text;//NightAmount
                sc.sz = dr["ot_amount"].ToString().Replace(",", "");//lbl_otamount.Text;//OvertimeAmount
                sc.saa = dr["otn_amount"].ToString().Replace(",", "");//lbl_otnamount.Text;//OvertimeNightAmount
                sc.sbb = dr["totalamt"].ToString().Replace(",", "");//lbl_totalamt.Text;//TotalAmount
                sc.scc = dr["reghourlyrate"].ToString();//lbl_trph.Text;//RatePerHourTardy
                sc.sdd = dr["dailyrate"].ToString().Replace(",", "");//lbl_ARPD.Text;//RatePerAbsentDay
                sc.see = dr["tardyamt"].ToString().Replace(",", ""); // lbl_tardyamount.Text;//TardyAmount
                sc.sff = dr["absentamt"].ToString().Replace(",", "");// lbl_absentamount.Text;//AbsentAmount
                sc.sgg = dr["netamt"].ToString().Replace(",", "");// lbl_netamount.Text;//NetAmount
                sc.shh = dr["leaveamt"].ToString().Replace(",", "");// lbl_leaveamount.Text;//leaveAmount
                sc.skk = dr["restdayamt"].ToString().Replace(",", "");// lbl_rdamt.Text;//rdamt
                sc.sll = dr["holidayamt"].ToString().Replace(",", "");// lbl_hdamt.Text;//hdamt
                sc.smm = dr["lateamt"].ToString().Replace(",", "");// lbl_latea.Text; //lamount
                sc.snn = dr["lbl_undera"].ToString().Replace(",", "");// lbl_undera.Text;//uamount

                sc.soo = dr["timein2"].ToString(); //lbl_latea.Text; //lamount
                sc.spp = dr["timeout1"].ToString(); //lbl_undera.Text;//uamount
                sc.srr = decimal.Parse(dr["nethours"].ToString()) > 0 ? mealallowance : "0";
                sc.sss = dr["overbreak"].ToString().Replace(",", "");
                sc.stt = dr["surge_pay"].ToString().Replace(",", "");
                string dtrperline = bol.dtrperline(sc);
                i++;
            }
            //  dbhelper.getdata("update tdtrperpayrol set dtr_id='" + dtrmain + "' where id=" + ddl_dtrfile.SelectedValue + "  ");
            Response.Redirect("Mdtrlist", false);
        }
        else
            Response.Write("<script>alert(Invalid Range!)</script>");
    }
}