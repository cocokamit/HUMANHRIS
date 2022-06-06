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
using System.Web.Services;


public partial class content_payroll_DTR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Timeout = 68;
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
        DataTable dtyear = dbhelper.getdata("SELECT YEAR(GETDATE())todate");
        DataTable dtselect = dbhelper.getdata("select * from MPeriod where Period = '" + dtyear.Rows[0]["todate"] + "'");
        if (dtselect.Rows.Count == 0)
        {
            dbhelper.getdata("insert into mperiod values ('" + dtyear.Rows[0]["todate"] + "')");
        }

        string query = "select a.id,left(convert(varchar,a.dfrom,101),10)ffrom,left(convert(varchar,a.dtoo,101),10) tto from payroll_range a " +
                        "where  " +
                        "(select COUNT(*) from tdtr where left(convert(varchar,DateStart,101),10)+'-'+left(convert(varchar,DateEnd,101),10)=left(convert(varchar,a.dfrom,101),10)+'-'+left(convert(varchar,a.dtoo,101),10)  and  status is null and payrollgroupid=" + ddl_pg.SelectedValue + " )=0 and " +
                        "a.action is null  order by a.dfrom desc";
        DataTable dt_range = dbhelper.getdata(query);
        ddl_pay_range.Items.Clear();
        ddl_pay_range.Items.Add(new ListItem("Select Payroll Range", "0"));
        foreach (DataRow dr in dt_range.Rows)
        {
            ddl_pay_range.Items.Add(new ListItem(dr["ffrom"].ToString() + "-" + dr["tto"].ToString(), dr["ffrom"].ToString() + "-" + dr["tto"].ToString()));
        }

        btn_go.Style.Add("display", "none");
        procdtr.Style.Add("display", "none");
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
    //protected void payroll_verify(object sender, EventArgs e)
    //{
    //    switch (btn_go.Text)
    //    {
    //        case "GO":
    //            if (ddl_pay_range.SelectedValue != "0")
    //            {
    //                string[] payrange = ddl_pay_range.SelectedValue.ToString().Split('-');
    //                string txtf = payrange[0];
    //                string txtt = payrange[1];
    //                if (txtf.Trim().Length == 0 || txtt.Trim().Length == 0)
    //                    Label1.Text = "*";
    //                else
    //                {
    //                    DataSet ds = new DataSet();
    //                    stateclass sc = new stateclass();
    //                    sc.sa = txtf.Replace(" ", "");
    //                    sc.sb = txtt.Replace(" ", "");
    //                    sc.sc = ddl_pg.SelectedValue;
    //                    string x = bol.payroll_verify(sc);
    //                    if (x == "1")
    //                    {
    //                        DataTable master = Core.DTRF(txtf, txtt, ddl_pg.SelectedValue);

    //                        ViewState["master"] = master;
    //                        grid_item.DataSource = master;
    //                        grid_item.DataBind();
    //                        ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");
    //                        if (grid_item.Rows.Count > 0)
    //                        {
    //                            procdtr.Visible = true;
    //                            tbl1.Visible = true;
    //                        }
    //                        else
    //                        {
    //                            procdtr.Visible = false;
    //                            tbl1.Visible = false;
    //                        }
    //                        ddl_pay_range.Enabled = false;
    //                        ddl_pg.Enabled = false;
    //                        btn_go.Text = "Change";
    //                    }
    //                    else
    //                    {
    //                        grid_item.DataBind();
    //                        Label1.Text = "Invalid range";
    //                        if (grid_item.Rows.Count > 0)
    //                        {
    //                            procdtr.Visible = true;
    //                            tbl1.Visible = true;
    //                        }
    //                        else
    //                        {
    //                            procdtr.Visible = false;
    //                            tbl1.Visible = false;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //                Response.Redirect("addDTR");
    //            break;
    //        case "Change":
    //            grid_item.DataSource = null;
    //            grid_item.DataBind();
    //            ddl_pg.Enabled = true;
    //            ddl_pay_range.Enabled = true;
    //            btn_go.Text = "GO";
    //            break;
    //    }

    //}

    protected void cpop(object sender, EventArgs e)
    {
        modalrd.Style.Add("display", "none");
        modalsetup.Style.Add("display", "none");
        modalinfinate.Style.Add("display", "none");
    }
    protected void landingpage(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            Response.Redirect("addemployee?user_id=" + row.Cells[0].Text + "&app_id=" + row.Cells[0].Text + "&tp=ed");
        }
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

                        DataTable dtinfinate = Session["dtinfinate"] as DataTable;
                        DataTable dtchker = Session["dtchker"] as DataTable;
                        DataTable dtrd = Session["dtrd"] as DataTable;

                        if (dtchker.Rows.Count > 0)
                        {
                            gridsetup.DataSource = dtchker;
                            gridsetup.DataBind();
                            modalsetup.Style.Add("display", "block");
                        }
                        else if (dtinfinate.Rows.Count > 0)
                        {
                            gridinfinity.DataSource = dtinfinate;
                            gridinfinity.DataBind();
                            modalinfinate.Style.Add("display", "block");
                        }
                        else
                        {
                            if (dtrd.Rows.Count > 0)
                            {
                                gridchker.DataSource = dtrd;
                                gridchker.DataBind();
                                modalrd.Style.Add("display", "block");
                            }
                            else
                            {
                                if (x == "1")
                                {
                                    //DataTable master = Core.DTRF(txtf, txtt, ddl_pg.SelectedValue);

                                    DataTable master = masters;
                                    ViewState["master"] = master;
                                    Session["master"] = master;
                                    hfmastercount.Value = master.Rows.Count.ToString();
                                    grid_item.DataSource = master;
                                    grid_item.DataBind();
                                    ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'content_grid_item', 'HeaderDiv');</script>");
                                    if (grid_item.Rows.Count > 0)
                                    {
                                        procdtr.Visible = true;
                                        tbl1.Visible = true;
                                        btnpropro.Style.Add("display", "");
                                    }
                                    else
                                    {
                                        procdtr.Visible = false;
                                        tbl1.Visible = false;
                                        btnpropro.Style.Add("display", "none");
                                    }
                                    ddl_pay_range.Enabled = false;
                                    ddl_pg.Enabled = false;
                                    btn_go.Text = "Change";
                                    btngogo.Style.Add("display", "none");
                                    btn_go.Style.Add("display", "");
                                }
                                else
                                {
                                    grid_item.DataBind();
                                    Label1.Text = "Invalid range";
                                    if (grid_item.Rows.Count > 0)
                                    {
                                        procdtr.Visible = true;
                                        btnpropro.Style.Add("display", "");
                                        tbl1.Visible = true;
                                    }
                                    else
                                    {
                                        btnpropro.Style.Add("display", "none");
                                        procdtr.Visible = false;
                                        tbl1.Visible = false;
                                    }
                                }
                            }
                        }//end brace
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
                btn_go.Style.Add("display", "none");
                btngogo.Style.Add("display", "");
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
        string query = "";
        DataTable dttt = (DataTable)ViewState["master"];
        DataTable dt = dbhelper.getdata("select * from mperiod where period=" + DateTime.Now.Year + "");

        using (SqlConnection con = new SqlConnection(dbconnection.conn))
        {
            using (SqlCommand cmd = new SqlCommand("audittrail_master_payroll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Button", SqlDbType.VarChar, 50).Value = "createadjusted";
                cmd.Parameters.Add("@AlterF", SqlDbType.VarChar, 50).Value = "" + ddl_pg.SelectedItem.ToString() + "";
                cmd.Parameters.Add("@AlterT", SqlDbType.VarChar, 50).Value = "Create Adjusted Record";
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "" + ddl_pay_range.SelectedItem.ToString() + "";
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["user_id"];
                cmd.Parameters.Add("@out", SqlDbType.VarChar, 30);
                cmd.Parameters["@out"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

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
            int x = 0;
            string empid = "0";
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
                sc.otm = dr["otmeal"].ToString().Replace(",", "");// ot meal
                sc.soo = dr["timein2"].ToString(); //lbl_latea.Text; //lamount
                sc.spp = dr["timeout1"].ToString(); //lbl_undera.Text;//uamount
                sc.srr = decimal.Parse(dr["nethours"].ToString()) > 0 ? mealallowance : "0";
                sc.sss = dr["overbreak"].ToString().Replace(",", "");
                sc.stt = dr["surge_pay"].ToString().Replace(",", "");
                string dtrperline = bol.dtrperline(sc);
                i++;

                if (dr["rownumber"].ToString() == "0")
                {
                    empid = dr["empid"].ToString();
                }

                string temp = "";
                if (empid == dr["empid"].ToString())
                {
                    temp = empid;
                    x += Convert.ToInt32(sc.otm);
                }

                if (empid != temp || (dttt.Rows.Count == Convert.ToInt32(dr["rownumber"].ToString()) + 1))
                {
                    //query += "insert into allowed_otherincome(date,other_income_id,empid,userid,amount)values(GETDATE(),'5','" + empid + "','1','" + x + "') ";

                    empid = dr["empid"].ToString();
                    x = Convert.ToInt32(sc.otm);
                }
            }
            //DataTable dtotm = dbhelper.getdata(query);

            //  dbhelper.getdata("update tdtrperpayrol set dtr_id='" + dtrmain + "' where id=" + ddl_dtrfile.SelectedValue + "  ");
            Response.Redirect("Mdtrlist", false);
        }
        else
            Response.Write("<script>alert(Invalid Range!)</script>");
    }


    [WebMethod]
    public static string getdtrprogress(string ddl_pay_range, string ddl_pg, string txt_remarks)
    {
        string result = "0";
        DataTable dt = dbhelper.getdata("select * from mperiod where period=" + DateTime.Now.Year + "");
        stateclass sc = new stateclass();
        string[] payrange = ddl_pay_range.ToString().Split('-');
        string txtf = payrange[0];
        string txtt = payrange[1];
        //define insert tdtr table
        sc.sa = dt.Rows[0]["id"].ToString(); //period
        sc.sb = ddl_pg;//payroll group
        sc.sc = txtf.Trim(); //date start
        sc.sd = txtt.Trim(); //date end
        sc.se = "0";//ddl_overtime.SelectedValue;//oevrtime
        sc.sf = "0";//ddl_leave.SelectedValue;//leave
        sc.sg = "0";//changeshift
        sc.sh = txt_remarks;//remarks
        sc.si = HttpContext.Current.Session["user_id"].ToString();// user_id.Value;//user_id
        string dtrmain = bol.dtrmain(sc);

        if (dtrmain != "")
        {
            result = dtrmain;
        }
        count = "0";
        return result;
    }
    public static string count = "0";
    [WebMethod]
    public static string returnprograss(string dtrmain, string rnum)
    {
        string result = "0";
        int rn = Convert.ToInt32(rnum);
        string query = "";
        count = (Convert.ToInt32(count) + 1).ToString();

        stateclass sc = new stateclass();
        DataTable dttt = HttpContext.Current.Session["master"] as DataTable;
        DataRow[] dtrow = dttt.Select("rownumber='" + rn + "'");
        int i = 0;
        int x = 0;
        string empid = "0";

        if (dtrow.Count() > 0)
        {
            string[] getdate = dtrow[0]["date"].ToString().Split('-');// lbl_date.Text.Trim().Split('-');
            string[] dateformat = getdate[0].Split('/');
            string mm = dateformat[0].ToString().Length > 1 ? dateformat[0] : "0" + "" + dateformat[0].ToString();
            string dd = dateformat[1].ToString().Length > 1 ? dateformat[1] : "0" + "" + dateformat[1].ToString();
            //DataRow[] dremp = dtemp.Select("id=" + dr["empid"].ToString() + "");
            DataTable dtmealallowance = dbhelper.getdata("select * from mealallowance where empid=" + dtrow[0]["empid"].ToString() + " and action is null order by id desc");
            string mealallowance = dtmealallowance.Rows.Count > 0 ? dtmealallowance.Rows[0]["amt"].ToString() : "0";

            sc.sa = dtrmain; //DTRId
            sc.sb = dtrow[0]["empid"].ToString();//ddl_emp.SelectedValue; //EmployeeId
            sc.sc = dtrow[0]["shiftcodeid"].ToString();//ddl_shiftcode.SelectedValue; //ShiftCodeId
            sc.sd = mm + "/" + dd + "/" + dateformat[2]; //getdate[0];//date
            sc.se = dtrow[0]["daytypeid"].ToString();//ddl_type.SelectedValue; //DayTypeId
            sc.sf = dtrow[0]["dm"].ToString();//lbl_dm.Text; //DayMultiplier
            sc.sg = dtrow[0]["restday"].ToString();//chk_rd.Checked ? "True" : "False";//RestDay
            sc.sh = dtrow[0]["timein1"].ToString();//lbl_timein1.Text; //TimeIn1
            sc.si = dtrow[0]["timeout2"].ToString();//lbl_timeout2.Text;//TimeOut2
            sc.sj = dtrow[0]["olw"].ToString();//chk_ol.Checked ? "True" : "False";//OnLeave
            sc.sjj = dtrow[0]["olh"].ToString();//chk_olH.Checked == true ? "True" : "False";//onleave halfday
            sc.sk = dtrow[0]["aw"].ToString();//chk_a.Checked ? "True" : "False";//Absent
            sc.sii = dtrow[0]["ah"].ToString();//chk_aH.Checked == true ? "True" : "False";// absent halfday
            sc.sl = dtrow[0]["reg_hr"].ToString().Replace(",", ""); ;//lbl_reg_workhours.Text;//RegularHours
            sc.sqq = dtrow[0]["offsethrs"].ToString().Replace(",", ""); ;//@totaloffsethrs
            sc.sm = dtrow[0]["night"].ToString().Replace(",", ""); //lbl_night.Text;//NightHours
            sc.sn = dtrow[0]["ot"].ToString().Replace(",", ""); // lbl_ot.Text;//OvertimeHours
            sc.so = dtrow[0]["otn"].ToString().Replace(",", "");// lbl_otn.Text;//OvertimeNightHours
            sc.sp = dtrow[0]["totalhrs"].ToString().Replace(",", "");//lbl_total.Text;//GrossTotalHours
            sc.sq = dtrow[0]["late"].ToString().Replace(",", "");//lbl_late.Text;//TardyLateHours
            sc.sr = dtrow[0]["ut"].ToString().Replace(",", "");// lbl_ut.Text;//TardyUndertimeHours
            sc.ss = dtrow[0]["nethours"].ToString().Replace(",", "");// lbl_nethours.Text;//NetTotalHour
            sc.st = dtrow[0]["hrrate"].ToString().Replace(",", "");// lbl_regrateperhour.Text;//RatePerHour
            sc.su = dtrow[0]["nightrate"].ToString().Replace(",", "");// lbl_nightrateperhour.Text;//RatePerNightHour
            sc.sv = dtrow[0]["otrate"].ToString().Replace(",", "");// lbl_otrateperhour.Text;//RatePerOvertimeHour
            sc.sw = dtrow[0]["otnrate"].ToString().Replace(",", "");// lbl_otnrateperhour.Text;//RatePerOvertimeNightHour
            sc.sx = dtrow[0]["reg_amount"].ToString().Replace(",", "");//lbl_regamount.Text;//RegularAmount
            sc.sy = dtrow[0]["night_amount"].ToString().Replace(",", "");//lbl_nightamount.Text;//NightAmount
            sc.sz = dtrow[0]["ot_amount"].ToString().Replace(",", "");//lbl_otamount.Text;//OvertimeAmount
            sc.saa = dtrow[0]["otn_amount"].ToString().Replace(",", "");//lbl_otnamount.Text;//OvertimeNightAmount
            sc.sbb = dtrow[0]["totalamt"].ToString().Replace(",", "");//lbl_totalamt.Text;//TotalAmount
            sc.scc = dtrow[0]["reghourlyrate"].ToString();//lbl_trph.Text;//RatePerHourTardy
            sc.sdd = dtrow[0]["dailyrate"].ToString().Replace(",", "");//lbl_ARPD.Text;//RatePerAbsentDay
            sc.see = dtrow[0]["tardyamt"].ToString().Replace(",", ""); // lbl_tardyamount.Text;//TardyAmount
            sc.sff = dtrow[0]["absentamt"].ToString().Replace(",", "");// lbl_absentamount.Text;//AbsentAmount
            sc.sgg = dtrow[0]["netamt"].ToString().Replace(",", "");// lbl_netamount.Text;//NetAmount
            sc.shh = dtrow[0]["leaveamt"].ToString().Replace(",", "");// lbl_leaveamount.Text;//leaveAmount
            sc.skk = dtrow[0]["restdayamt"].ToString().Replace(",", "");// lbl_rdamt.Text;//rdamt
            sc.sll = dtrow[0]["holidayamt"].ToString().Replace(",", "");// lbl_hdamt.Text;//hdamt
            sc.smm = dtrow[0]["lateamt"].ToString().Replace(",", "");// lbl_latea.Text; //lamount
            sc.snn = dtrow[0]["lbl_undera"].ToString().Replace(",", "");// lbl_undera.Text;//uamount
            sc.otm = "0";// ot meal
            sc.soo = dtrow[0]["timein2"].ToString(); //lbl_latea.Text; //lamount
            sc.spp = dtrow[0]["timeout1"].ToString(); //lbl_undera.Text;//uamount
            sc.srr = decimal.Parse(dtrow[0]["nethours"].ToString()) > 0 ? mealallowance : "0";
            sc.sss = dtrow[0]["overbreak"].ToString().Replace(",", "");
            sc.stt = dtrow[0]["surge_pay"].ToString().Replace(",", "");
            string dtrperline = bol.dtrperline(sc);
            i++;

            if (dtrow[0]["rownumber"].ToString() == "0")
            {
                empid = dtrow[0]["empid"].ToString();
            }

            string temp = "";
            if (empid == dtrow[0]["empid"].ToString())
            {
                temp = empid;
                x += Convert.ToInt32(sc.otm);
            }

            if (empid != temp || (dttt.Rows.Count == Convert.ToInt32(dtrow[0]["rownumber"].ToString()) + 1))
            {
                query = "insert into allowed_otherincome(date,other_income_id,empid,userid,amount)values(GETDATE(),'5','" + empid + "','1','" + x + "') ";

                empid = dtrow[0]["empid"].ToString();
                x = Convert.ToInt32(sc.otm);
            }
            if (query != "")
            {
                DataTable dtotm = dbhelper.getdata(query);
            }
        }
        return count;
    }


    public static DataTable dttest { get; set; }
    public static DataTable dtmanual { get; set; }
    public static DataTable dtgtwthveri { get; set; }
    public static DataTable dtleave { get; set; }
    public static DataTable dtgetemp { get; set; }
    public static DataTable dtrdl { get; set; }
    public static DataTable dtchangeshift { get; set; }
    public static DataTable dtshiftcode { get; set; }
    public static DataTable dtrestday { get; set; }
    public static DataTable dtdaytype { get; set; }
    public static DataTable dtgtoffset { get; set; }
    public static DataTable dtchksc { get; set; }
    public static DataTable dtcompensation { get; set; }
    public static DataTable dtot { get; set; }
    public static DataTable dtgthdoffset { get; set; }
    public static DataTable dtchkrange { get; set; }
    public static DataTable dtotmeal { get; set; }
    public static DataTable dtdtOBT { get; set; }
    public static DataTable dtcurrdate { get; set; }
    public static DataTable dtloadable { get; set; }
    public static DataTable masters { get; set; }
    public static DataTable dttemp { get; set; }
    [WebMethod]
    public static string getdtrgo(string ff, string tt, string pg)
    {
        string result = "0";

        string txtf = ff.Trim();
        string txtt = tt.Trim();

        /**BTK 04102020**/
        string query = "select a.id empid, a.BranchId, a.HourlyRate,a.lastname+' '+a.firstname+' '+ a.middlename+' '+a.extensionname empname, " +
        "a.ShiftCodeId,c.ShiftCode,a.FixNumberOfHours,a.DailyRate,a.PayrollTypeId,left(convert(varchar,datehired,101),10)datehired,rownumber = ROW_NUMBER() Over (ORDER BY a.id), * " +
        "from MEmployee a left join MPosition b on a.PositionId=b.Id left join MShiftCode c on a.ShiftCodeId = c.Id where ";
        if (pg.Contains("KIOSK_"))
            query += " a.id=" + pg.Replace("KIOSK_", "");
        else if (pg.Contains("attlogs"))
        {
            string[] collection = pg.Split(',');
            string section = pg.Contains("section") ? " and a.sectionid=" + collection[1].Replace("section_", "") : "";
            string department = collection[0].Replace("attlogs_", "");
            query += " a.DepartmentId=" + department + section;
        }
        else
            query += " a.payrollgroupid=" + pg + " ";

        DataTable getemp = dbhelper.getdata(query);
        dtgetemp = getemp;
        DataTable compensation = dbhelper.getdata("select a.id,a.app_trn_id,c.PayrollType,a.fnod,a.fnoh,a.mr,a.pr,a.dr,a.hr,LEFT(CONVERT(varchar,a.effective_date,101),10)effective_date,b.empid from app_trn_salaryinc a left join app_trn b on a.app_trn_id=b.id left join MPayrollType c on a.paytypeid=c.Id where b.action is null order by a.id desc  ");
        dtcompensation = compensation;
        DataTable daytype = dbhelper.getdata("select *, a.id,a.DayType,a.workingdays,a.RestdayDays,case when b.BranchId is null then '0' else b.BranchId end BranchId ,case when left(convert(varchar,b.Date,101),10) is null then '0' else left(convert(varchar,b.Date,101),10) end datte from MDayType a left join MDayTypeDay b on a.Id=b.DayTypeId where b.status is null order by a.id asc ");
        dtdaytype = daytype;
        /**GET FINAL SCHEDULE**/
        string schedule = "select a.changeshiftid, b.ShiftCodeId, replace(replace(b.TimeIn1,'.',':'),' ',':00 ')punchin,replace(replace(b.TimeOut2,'.',':'),' ',':00 ')punchout,a.employeeid,left(convert(varchar,a.Date,101),10)dtrdate, b.NumberOfHours "
            + "from  TChangeShiftline a left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId left join MShiftCode c on a.ShiftCodeId=c.id "
            + "left join memployee d on a.EmployeeId=d.Id where a.status like '%Approved%' and c.status is null "
            + "and CONVERT(date,a.date) between '" + txtf + "' and '" + txtt + "' ";

        if (pg.Contains("KIOSK_"))
            schedule += "and a.EmployeeId=" + pg.Replace("KIOSK_", "");

        schedule += "order by a.[date]";
        DataTable changeshift = dbhelper.getdata(schedule);
        dtchangeshift = changeshift;
        DataTable restday = dbhelper.getdata("select * from MShiftCodeDay where RestDay='True'");
        dtrestday = restday;
        DataTable chksc = dbhelper.getdata("select * from MShiftCode where status is null ");
        dtchksc = chksc;
        DataTable manual = dbhelper.getdata("select left(convert(varchar,date,101),10)date,case when len(time_in)>8 then time_in else left(convert(varchar,date,101),10)+' '+time_in end time_in ,case when len(time_OUT)>8 then time_OUT else left(convert(varchar,date,101),10)+' '+time_OUT end time_OUT ,time_in1,time_OUT1,left(convert(varchar,date,101),10)dtrdate,employeeid from Tmanuallogline where status like '%Approved%' ");//and dtr_id is null
        dtmanual = manual;
        /**BTK LEAVE 
         * 04282020
         * OPTIMIZE FILTER VIA SPECIFIED PERIOD**/
        string leaveQuery = "select a.WithPay, a.dailyrate, a.amount_to_be_paid, a.inoutduringhalfdayleave, numberofhours noh, a.NumberOfHours, a.setupbasichrs, left(convert(varchar,a.date,101),10)dtrdate, a.employeeid , " +
        "b.PayrollGroupId,b.DepartmentId, b.sectionid from TLeaveApplicationLine a left join MEmployee b on a.EmployeeId=b.Id " +
        "where a.status like '%Approved%' and CONVERT(date,a.date) between '" + txtf + "' and '" + txtt + "' and ";

        if (pg.Contains("KIOSK_"))
            leaveQuery += "a.employeeid =" + pg.Replace("KIOSK_", "");
        else if (pg.Contains("attlogs"))
        {
            string[] collection = pg.Split(',');
            string section = pg.Contains("section") ? " and b.sectionid=" + collection[1].Replace("section_", "") : "";
            string department = collection[0].Replace("attlogs_", "");
            leaveQuery += "b.DepartmentId=" + department + section;
        }
        else
            leaveQuery += "b.payrollgroupid=" + pg + " ";
        DataTable leave = dbhelper.getdata(leaveQuery);
        dtleave = leave;
        DataTable rdl = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10) dtrdate from TRestdaylogs where status like '%Approved%' ");//and dtr_id is null
        dtrdl = rdl;
        DataTable shiftcode = dbhelper.getdata("select (Select Hrs from MShiftCodeDayUT where ShiftCodeDayId=a.Id) Hrs,case when a.nightbreakhours is null then '0' else a.nightbreakhours end nightbreakhours, a.LateFlexibility, a.nighshift, a.ShiftCodeId,replace(replace(a.TimeIn1,'.',':'),' ',':00 ')punchin,replace(replace(a.TimeOut1,'.',':'),' ',':00 ')punchout1,replace(replace(a.TimeIn2,'.',':'),' ',':00 ')punchin2,replace(replace(a.TimeOut2,'.',':'),' ',':00 ')punchout,replace(a.LateGraceMinute,'.00','')LateGraceMinute,a.NumberOfHours,a.NightHours,a.LateFlexibility,(select broke from MShiftCode b where id= a.ShiftCodeId  ) broke,(select shiftcode from MShiftCode b where id= a.ShiftCodeId  ) shiftcode,a.ShiftCodeId,a.day,a.breakwithpay,a.flexbreak,a.mandatorytopunch,a.openshift from MShiftCodeDay a where a.status is null ");//ddl_shiftcode.SelectedValue
        dtshiftcode = shiftcode;
        DataTable ot = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TOverTimeLine where  status like '%Approved%' ");//and dtr_id is null 
        dtot = ot;
        DataTable gtoffset = dbhelper.getdata("select *,left(convert(varchar,appliedfrom,101),10) appliedfrom from toffset  where  status like '%Approved%'");
        dtgtoffset = gtoffset;
        DataTable gthdoffset = dbhelper.getdata("select *,left(convert(varchar,appliedto,101),10) appliedfrom from toffset  where status like '%Approved%'");
        dtgthdoffset = gthdoffset;
        DataTable gtwthveri = dbhelper.getdata("select emp_id,date_filed,[time]ttime,[status]sstatus,case when dtunder IS null then LEFT(CONVERT(varchar,date_filed,101),10)+' '+[time] else dtunder  end  acttimeout from tundertime where status like '%Approved%'");
        dtgtwthveri = gtwthveri;
        /**BTK OBT**/
        //for update
        DataTable chkrange = dbhelper.getdata("select a.id,left(convert(varchar,b.DateStart,101),10)+' - '+left(convert(varchar,b.DateEnd,101),10)dtrrange,case when a.status_1 is null then 'Pending' else a.status_1 end status_1 from TPayroll a left join TDTR b on a.DTRId=b.Id "
            + "where a.status is null and b.DateStart between '" + txtf.Trim() + "' and '" + txtt.Trim() + "' order by a.PayrollDate desc");
        dtchkrange = chkrange;
        DataTable otmeal = dbhelper.getdata("select *,EmployeeId,left(convert(varchar, Date,101),10)dtrdate from TMealHours where status like '%Approved%'");
        dtotmeal = otmeal;
        DataTable dtOBT = dbhelper.getdata("select *,left(convert(varchar,obin,101),10)dtrdate from OBTLine where [status]='approved'");
        dtdtOBT = dtOBT;
        DataTable master = loadable();
        DataTable currdate = dbhelper.getdata("select convert(varchar,getdate(),101)currdate");
        dtcurrdate = currdate;
        dtloadable = loadable();
        masters = master;

        if (getemp.Rows.Count > 0)
        {
            result = getemp.Rows.Count.ToString();
        }

        count = "0";
        return result;
    }

    [WebMethod]
    public static string returndtrgoprogress(string ff, string tt, string ddl_pg)
    {
        string txtf = ff.Trim();
        string txtt = tt.Trim();

        string err;
        DataRow[] drgetemp = dtgetemp.Select("rownumber='" + count + "'");
        if (txtf.Trim().Length == 0 || txtt.Trim().Length == 0)
            err = "*";
        else
        {
            loadable();
            DateTime datef = Convert.ToDateTime(txtf);
            TimeSpan nod;
            nod = DateTime.Parse(txtf) - DateTime.Parse(txtt);
            string nodformat = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0}", nod.Days, nod.Hours, nod.Minutes, nod.Seconds).Replace("-", "");
            DataRow mdr;

            int rownumber = 0;
            foreach (DataRow dr in drgetemp)
            {
                int row_cnt = 0;
                decimal total_tardy_hrs = 0;
                decimal total_tempot_hrs = 0;
                DataTable dtcompanydet = dbhelper.getdata("select * from mcompany where id=" + dr["companyid"] + "");
                for (int i = 0; i <= int.Parse(nodformat); i++)
                {
                    decimal surge_pay = 0;
                    decimal t_total_tempot = 0;
                    decimal t_total_tempot_nightt = 0;
                    string status = "RWD";
                    decimal rdamount = 0;
                    string[] f_datef = datef.AddDays(i).ToString().Trim().Split(' ');
                    string[] getdate = f_datef[0].Trim().Split('/');
                    string month = getdate[0].Length > 1 ? getdate[0] : "0" + getdate[0];
                    string day = getdate[1].Length > 1 ? getdate[1] : "0" + getdate[1];
                    string date = f_datef[0] + "-" + DateTime.Parse(f_datef[0]).DayOfWeek;
                    string[] getspecdate = date.Trim().Split('-');
                    string[] dayofweek = date.Trim().Split('-');
                    string[] ggg = dayofweek[0].Split('/');
                    month = ggg[0].ToString().Length > 1 ? ggg[0].ToString() : "0" + ggg[0].ToString();
                    day = ggg[1].ToString().Length > 1 ? ggg[1].ToString() : "0" + ggg[1].ToString();
                    string dtrdate = month + "/" + day + "/" + ggg[2];
                    string curding = dtcurrdate.Rows[0]["currdate"].ToString();
                    /**START DEBUG**/

                    string aa = dr["id"].ToString(); //dr["id"].ToString().Length == 1 ? "0" + dr["id"].ToString() : dr["id"].ToString();

                    DataRow[] dtgetallDayType = dtdaytype.Select("datte='" + month + "/" + day + "/" + getdate[2] + "'");
                    if (dtgetallDayType.Count() > 0 && int.Parse(dtgetallDayType[0]["BranchId"].ToString()) > 0)
                        dtgetallDayType = dtdaytype.Select("datte='" + month + "/" + day + "/" + getdate[2] + "' and BranchId='" + dr["BranchId"].ToString() + "'");
                    else
                        dtgetallDayType = dtdaytype.Select("datte='" + month + "/" + day + "/" + getdate[2] + "' and BranchId='0'");
                    DataRow[] dtgetchangeshift = dtchangeshift.Select("dtrdate='" + dtrdate + "' and employeeid='" + dr["id"].ToString() + "'");
                    string shiftcodeid = dtgetchangeshift.Count() > 0 ? dtgetchangeshift[0]["shiftcodeid"].ToString() : dr["shiftcodeid"].ToString();

                    DataRow[] getscc = dtchksc.Select("id=" + shiftcodeid + "");

                    string ffix;
                    if (getscc[0]["fix"].ToString() == "1")
                        ffix = " and day='" + dayofweek[1] + "'";
                    else
                        ffix = "";

                    DataRow[] dtgetrestday = dtrestday.Select("ShiftCodeId=" + shiftcodeid + ffix + " ");
                    DataRow[] dtt = dttest.Select("emp_id='" + aa + "' and date='" + dtrdate + "'");
                    DataRow[] getmanual = dtmanual.Select("employeeid='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");
                    DataRow[] getleave = dtleave.Select("employeeid='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");
                    DataRow[] getrdl = dtrdl.Select("EmployeeId='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");

                    //BTK OBT
                    DataRow[] getobt = dtdtOBT.Select("empID='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");

                    //DataRow[] dtgetallDayType = DayType.Select("id='" + dtgetallDayType[0]["id"].ToString() + "'");
                    if (getrdl.Count() > 0)
                    {
                        shiftcodeid = getrdl.Count() > 0 ? getrdl[0]["shiftcodeid"].ToString() : shiftcodeid;
                        ffix = "";
                    }
                    //shiftcodeid = getrdl.Count() > 0 ? getrdl[0]["shiftcodeid"].ToString() : shiftcodeid;

                    DataRow[] dtgetshiftcode = dtshiftcode.Select("ShiftCodeId=" + shiftcodeid + ffix + "");

                    DataRow[] overttime = dtot.Select("EmployeeId='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");
                    // DataRow[] dremp = emplist.Select("id='" + dr["id"] + "'");

                    DataRow[] mealallowance = dtotmeal.Select("EmployeeId='" + dr["id"] + "' and dtrdate='" + dtrdate + "'");

                    //otoffset
                    DataRow[] droffset = dtgtoffset.Select("status like '%Approved%' and empid=" + dr["id"] + " and appliedfrom='" + dtrdate + "' and appliedto is null");
                    //offset from 
                    DataRow[] drhdoffsetfrom = dtgtoffset.Select("status like '%Approved%' and empid=" + dr["id"] + " and appliedfrom='" + dtrdate + "' and appliedto is not null");
                    //offset to 
                    DataRow[] drhdoffset = dtgthdoffset.Select("status like '%Approved%' and empid=" + dr["id"] + " and  appliedto='" + dtrdate + "' ");

                    DataRow[] drundertime = dtgtwthveri.Select("sstatus like '%Approved%' and emp_id=" + dr["id"] + " and date_filed='" + dtrdate + "'");

                    int mealtotal = mealallowance.Count() * 100;

                    DataRow[] drcompensation1 = dtcompensation.Select(" empid=" + dr["id"] + " and effective_date <='" + dtrdate + "'");
                    DataTable drcompensation = new DataTable();
                    if (drcompensation1.Count() > 0)
                    {
                        DataView hold = drcompensation1.CopyToDataTable().DefaultView;
                        hold.Sort = "id desc";
                        drcompensation = hold.ToTable();
                    }
                    string dailyrate = drcompensation.Rows.Count > 0 ? drcompensation.Rows[0]["dr"].ToString() : dr["dailyrate"].ToString();
                    string hourlyrate = drcompensation.Rows.Count > 0 ? drcompensation.Rows[0]["hr"].ToString() : dr["hourlyrate"].ToString();
                    string monthlyrate = drcompensation.Rows.Count > 0 ? drcompensation.Rows[0]["mr"].ToString() : dr["monthlyrate"].ToString();
                    string payrollrate = drcompensation.Rows.Count > 0 ? drcompensation.Rows[0]["pr"].ToString() : dr["payrollrate"].ToString();

                    string scs = shiftcodeid;
                    string scc = dtgetshiftcode.Count() > 0 ? dtgetshiftcode[0]["shiftcode"].ToString() : "No Shift Code";
                    decimal offsethrs = droffset.Count() > 0 ? decimal.Parse(droffset[0]["offsethrs"].ToString()) : 0;
                    decimal hdoffsethrs = drhdoffset.Count() > 0 ? decimal.Parse(drhdoffset[0]["offsethrs"].ToString()) : 0;
                    offsethrs = offsethrs + hdoffsethrs;
                    double getgracehours = double.Parse(dtgetshiftcode[0]["LateGraceMinute"].ToString()) / 60;
                    string getgracem = "-" + dtgetshiftcode[0]["LateGraceMinute"].ToString();
                    double flexhours = double.Parse(dtgetshiftcode[0]["LateFlexibility"].ToString()) / 60;
                    double total_flexIn = getgracehours + flexhours;

                    //break hours
                    decimal breakhrs = decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString());

                    mdr = masters.NewRow();
                    mdr["empid"] = dr["id"];
                    mdr["employee"] = dr["empname"];
                    mdr["shiftcodeid"] = scs;
                    mdr["ShiftCode"] = scc;
                    mdr["date"] = date;

                    //day multiplier
                    decimal daymultiplier = 0;
                    decimal nightmultiplier = 0;
                    decimal otdaymultiplier = 0;
                    decimal otnightmultiplier = 0;

                    if (drhdoffsetfrom.Count() > 0)
                    {

                        DataRow[] dtgetallDayTypeoffset = dtdaytype.Select(" id=1");
                        daymultiplier = dtgetallDayTypeoffset.Count() > 0 ? decimal.Parse(dtgetallDayTypeoffset[0]["workingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["workingdays"].ToString());
                        nightmultiplier = dtgetallDayTypeoffset.Count() > 0 ? decimal.Parse(dtgetallDayTypeoffset[0]["nightworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["nightworkingdays"].ToString());
                        otdaymultiplier = dtgetallDayTypeoffset.Count() > 0 ? decimal.Parse(dtgetallDayTypeoffset[0]["OTworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTworkingdays"].ToString());
                        otnightmultiplier = dtgetallDayTypeoffset.Count() > 0 ? decimal.Parse(dtgetallDayTypeoffset[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTNightWorkingDays"].ToString());
                    }
                    else
                    {
                        if (dtgetrestday.Count() > 0)
                        {
                            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["RestdayDays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["RestdayDays"].ToString());
                            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightrestdaydays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["nightrestdaydays"].ToString());
                            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTrestdaydays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTrestdaydays"].ToString());
                            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightRestdayDays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTNightRestdayDays"].ToString());
                        }
                        else
                        {
                            daymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["workingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["workingdays"].ToString());
                            nightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["nightworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["nightworkingdays"].ToString());
                            otdaymultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTworkingdays"].ToString());
                            otnightmultiplier = dtgetallDayType.Count() > 0 ? decimal.Parse(dtgetallDayType[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTNightWorkingDays"].ToString());
                        }
                    }
                    string tmin1 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[2].ToString().Length > 0 ? dtt[0].ItemArray[2].ToString() : "--" : "--" : getmanual[0]["time_in"].ToString().Length > 1 ? getmanual[0]["time_in"].ToString() : "--";
                    string tmout1 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[3].ToString().Length > 0 ? dtt[0].ItemArray[3].ToString() : "--" : "--" : getmanual[0]["time_out1"].ToString().Length > 1 ? getmanual[0]["time_out1"].ToString() : "--";
                    string tmin2 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[4].ToString().Length > 0 ? dtt[0].ItemArray[4].ToString() : "--" : "--" : getmanual[0]["time_in1"].ToString().Length > 1 ? getmanual[0]["time_in1"].ToString() : "--";
                    string tmout2 = getmanual.Count() == 0 ? dtt.Count() > 0 ? dtt[0].ItemArray[5].ToString().Length > 0 ? dtt[0].ItemArray[5].ToString() : "--" : "--" : getmanual[0]["time_out"].ToString().Length > 1 ? getmanual[0]["time_out"].ToString() : "--";
                    tmout2 = drundertime.Count() > 0 ? drundertime[0]["acttimeout"].ToString() : tmout2;

                    //check if Holiday
                    string rd = "False";
                    string hd = "False";
                    if (dtgetallDayType.Count() > 0)
                    {
                        if (int.Parse(dtgetallDayType[0]["id"].ToString()) > 1)
                        {
                            if (getrdl.Count() == 0)
                            {
                                //For CLI HRIS Level Officer 3 and Up Asumeaspresent
                                if (dr["DivisionId"].ToString() == "1" || dr["DivisionId"].ToString() == "4" || dr["DivisionId"].ToString() == "5" || dr["nonepunching"].ToString() == "True" || dr["DivisionId2"].ToString() == "7" || dr["DivisionId2"].ToString() == "8" || dr["DivisionId2"].ToString() == "9" || dr["DivisionId2"].ToString() == "10" || dr["DivisionId2"].ToString() == "11" || dr["DivisionId2"].ToString() == "12" || dr["DivisionId2"].ToString() == "13" || dr["DivisionId2"].ToString() == "14" || dr["DivisionId2"].ToString() == "15" || dr["DivisionId2"].ToString() == "16" || dr["DivisionId2"].ToString() == "19" || dr["DivisionId2"].ToString() == "21" || dr["DivisionId2"].ToString() == "22" || dr["DivisionId2"].ToString() == "23" || dr["DivisionId2"].ToString() == "24" || dr["DivisionId2"].ToString() == "25")
                                {
                                    tmin1 = "--";
                                    tmout1 = "--";
                                    tmin2 = "--";
                                    tmout2 = "--";
                                }
                            }
                            status = dtgetallDayType[0]["daytype"].ToString().Contains("Regular") ? "RH" : dtgetallDayType[0]["daytype"].ToString().Contains("Special") ? "SH" : dtgetallDayType[0]["daytype"].ToString().Contains("Company") ? "CH" : "DH";
                            hd = "True";
                        }
                    }

                    //check if RD
                    if (dtgetrestday.Count() > 0)
                    {
                        if (int.Parse(dtgetrestday[0]["id"].ToString()) > 1)
                        {
                            if (getrdl.Count() == 0)
                            {
                                tmin1 = "--";
                                tmout1 = "--";
                                tmin2 = "--";
                                tmout2 = "--";
                            }
                        }
                        status = "RD";
                        rd = "True";
                    }

                    //get actual punch
                    tmin1 = tmin1.Length <= 2 ? "--" : Convert.ToDateTime(tmin1).ToString("MM/dd/yyyy hh:mm tt");//DateTime.Parse(tmin1).ToString();
                    tmout1 = tmout1.Length <= 2 ? "--" : Convert.ToDateTime(tmout1).ToString("MM/dd/yyyy hh:mm tt");//DateTime.Parse(tmout1).ToString();
                    tmin2 = tmin2.Length <= 2 ? "--" : Convert.ToDateTime(tmin2).ToString("MM/dd/yyyy hh:mm tt");//DateTime.Parse(tmin2).ToString();
                    tmout2 = tmout2.Length <= 2 ? "--" : Convert.ToDateTime(tmout2).ToString("MM/dd/yyyy hh:mm tt");//DateTime.Parse(tmout2).ToString();

                    //biotime in and out actual
                    DateTime getin = Convert.ToDateTime(tmin1.ToString().Length > 2 ? tmin1.ToString() : "0:00:00");
                    DateTime getoutt1 = Convert.ToDateTime(tmout1.ToString().Length > 2 ? tmout1.ToString() : "0:00:00");
                    DateTime getinn1 = Convert.ToDateTime(tmin2.ToString().Length > 2 ? tmin2.ToString() : "0:00:00");
                    DateTime getout = Convert.ToDateTime(tmout2.ToString().Length > 2 ? tmout2.ToString() : "0:00:00");

                    //shift code set up
                    DateTime setupgetin1 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchin"].ToString()).AddMinutes(getgracehours); //total_flexIn
                    DateTime setupgetout1 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchout1"].ToString());
                    DateTime setupgetin2 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchin2"].ToString());
                    DateTime setupgetout2 = Convert.ToDateTime(dtrdate + " " + dtgetshiftcode[0]["punchout"].ToString());//.AddMinutes(flexhours)
                    string setupfinalout = setupgetout2.ToString();


                    //cross dates
                    string crossdate = null;
                    if (setupgetin1.ToString().Contains("PM"))
                    {
                        if (setupgetout1.ToString().Contains("AM"))
                        {
                            setupgetout1 = setupgetout1.AddDays(1);
                            crossdate = "1";
                        }
                        if (setupgetin2.ToString().Contains("AM"))
                        {
                            setupgetin2 = setupgetin2.AddDays(1);
                            crossdate = "1";
                        }
                        if (setupgetout2.ToString().Contains("AM"))
                        {
                            setupgetout2 = setupgetout2.AddDays(1);
                            crossdate = "1";
                        }
                    }
                    else if (setupgetout1.ToString().Contains("PM"))
                    {
                        if (setupgetin2.ToString().Contains("AM"))
                        {
                            setupgetin2 = setupgetin2.AddDays(1);
                            crossdate = "1";
                        }
                        if (setupgetout2.ToString().Contains("AM"))
                        {
                            setupgetout2 = setupgetout2.AddDays(1);
                            crossdate = "1";
                        }
                    }
                    else if (setupgetin2.ToString().Contains("PM"))
                    {
                        if (setupgetout2.ToString().Contains("AM"))
                        {
                            setupgetout2 = setupgetout2.AddDays(1);
                            crossdate = "1";
                        }
                    }
                    else if (setupgetin1.ToString().Contains("12:00:00 AM"))
                    {
                        /**BTK 02112020
                         * TO HANDLE MIDDAY SHIFT**/
                        setupgetin1 = setupgetin1.AddDays(1);
                        setupgetout2 = setupgetout2.AddDays(1);
                        crossdate = "1";
                    }

                    //detect leave  and absent
                    decimal t_leavehour = 0;
                    decimal leaveamount = 0;
                    decimal leavewoamount = 0;
                    string inoutduringleavehalfday = "0";
                    decimal nohleave = 0;
                    string leaveh = "False";
                    string leavew = "False";
                    string absenth = "False";
                    string absentw = "False";
                    string irregularity = "False";
                    if (getleave.Count() > 0)
                    {
                        if (decimal.Parse(getleave[0]["noh"].ToString()) >= 1)
                        {
                            leavew = "True";
                            nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                        }
                        else
                        {

                            leaveh = "True";
                            nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                            inoutduringleavehalfday = getleave[0]["inoutduringhalfdayleave"].ToString().Length > 0 ? getleave[0]["inoutduringhalfdayleave"].ToString() : "0";

                        }

                        t_leavehour = decimal.Parse(getleave[0]["NumberOfHours"].ToString()) * decimal.Parse(getleave[0]["setupbasichrs"].ToString().Length == 0 ? dr["FixNumberOfHours"].ToString() : getleave[0]["setupbasichrs"].ToString());

                        /**BTK LEAVE 
                        * 04282020
                        * CORRECT CONDITIONING [leavewoamount]**/
                        if (getleave[0]["WithPay"].ToString() == "True")
                        {
                            leaveamount = double.Parse(getleave[0]["amount_to_be_paid"].ToString()) > 0 ? decimal.Parse(getleave[0]["dailyrate"].ToString()) : decimal.Parse(getleave[0]["amount_to_be_paid"].ToString());
                            leavewoamount = 0;
                        }
                        else
                        {
                            leaveamount = 0;
                            leavewoamount = double.Parse(getleave[0]["amount_to_be_paid"].ToString()) > 0 ? t_leavehour * decimal.Parse(hourlyrate) : decimal.Parse(getleave[0]["amount_to_be_paid"].ToString());
                            //leavewoamount = decimal.Parse(getleave[0]["dailyrate"].ToString());
                        }
                    }

                    //cleaning punch
                    decimal finalut = 0;
                    decimal night = 0;
                    decimal reghours = 0;
                    TimeSpan stregh = new TimeSpan();
                    TimeSpan sthalf = new TimeSpan();
                    TimeSpan ndhalft = new TimeSpan();
                    TimeSpan ndut = new TimeSpan();
                    TimeSpan ndot = new TimeSpan();
                    decimal finalot = t_total_tempot;
                    decimal othrs = 0;
                    decimal othrsnight = 0;
                    TimeSpan brkhrs = new TimeSpan();
                    double breakdeduct = double.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString());
                    double nightbreakdeduct = 0;
                    decimal overbreak = 0;
                    decimal t_total = 0;
                    decimal t_total_ut = 0;
                    int yyyyy = 0;

                    //dre start
                    string hiredd = dr["datehired"].ToString();
                    //string hiredd = getemp.Rows[0]["datehired"].ToString();
                    if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                    {
                        if (tmin1.ToString().Length > 2 && tmout2.ToString().Length > 2)
                        {
                            //get late //basin dre dapit
                            TimeSpan templatehrs = new TimeSpan();
                            TimeSpan flexbrhrs = new TimeSpan();
                            DateTime fdateout1late = new DateTime();
                            decimal flexmidlate = 0;
                            if (leaveh == "True")
                            {
                                if (int.Parse(inoutduringleavehalfday) == 1)
                                    setupgetin1 = setupgetin1.AddHours(double.Parse(nohleave.ToString()) + breakdeduct + (0.5));
                                //fdateout1late = setupgetin1;
                                if (int.Parse(inoutduringleavehalfday) == 2)
                                {
                                    string ded = "-" + (double.Parse(nohleave.ToString()) + breakdeduct).ToString();
                                    setupgetout2 = setupgetout2.AddHours(double.Parse(ded));
                                    //fdateout1late = setupgetout2;
                                }
                                if (getin > setupgetin1)
                                    sthalf = getin - setupgetin1;
                                t_total = decimal.Parse(sthalf.TotalHours.ToString()) + decimal.Parse(ndhalft.TotalHours.ToString());
                            }
                            else
                            {
                                if (getin > setupgetin1)
                                {
                                    sthalf = getin - setupgetin1;
                                }
                                if (tmin2.ToString().Length > 2)
                                {
                                    if (dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True")
                                    {
                                        if (dtgetshiftcode[0]["flexbreak"].ToString() == "True")
                                        {
                                            if (dtgetshiftcode[0]["punchout1"].ToString() == "0:00:00" && dtgetshiftcode[0]["punchin2"].ToString() == "0:00:00")
                                            {
                                                flexbrhrs = (getinn1 - getoutt1);
                                                flexmidlate = decimal.Parse(flexbrhrs.TotalHours.ToString()) > decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) ? decimal.Parse(flexbrhrs.TotalHours.ToString()) - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) : 0;
                                            }
                                        }
                                        else
                                        {
                                            if (getinn1 > setupgetin2)
                                            {
                                                ndhalft = getinn1 - setupgetin2;
                                                flexmidlate = decimal.Parse(ndhalft.TotalHours.ToString()) > decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) ? decimal.Parse(ndhalft.TotalHours.ToString()) - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()) : 0;
                                            }
                                        }
                                    }
                                }

                                t_total = decimal.Parse(sthalf.TotalHours.ToString()) + flexmidlate;
                                t_total = t_total > 0 ? t_total : 0;
                                overbreak = flexmidlate;

                            }
                            // get undertime
                            DateTime fdateout1under = new DateTime();
                            if (leaveh == "True")
                            {
                                if (int.Parse(inoutduringleavehalfday) == 1)
                                    fdateout1under = setupgetout1;
                                if (int.Parse(inoutduringleavehalfday) == 2)
                                    fdateout1under = setupgetout2;
                            }
                            else
                                fdateout1under = setupgetout2;

                            double sp = Convert.ToDouble(dtgetshiftcode[0]["Hrs"].ToString() == "" ? "0" : dtgetshiftcode[0]["Hrs"].ToString());

                            DateTime tempgetout = getout.AddMinutes(sp);

                            DateTime tempout = tempgetout;
                            if (tempgetout > fdateout1under)
                            {
                                tempout = fdateout1under;
                            }
                            else
                            {
                                tempout = tempgetout;
                            }


                            if (tempout < fdateout1under)
                            {
                                ndut = fdateout1under - tempout;
                                t_total_ut = decimal.Parse(ndut.TotalHours.ToString());
                                finalut = t_total_ut - offsethrs;
                                finalut = finalut > 0 ? finalut : 0;

                            }
                            finalut = finalut > 0 ? finalut : 0;
                            //get temp ot
                            DateTime getoutforvertime = Convert.ToDateTime(tmout2);
                            if (getoutforvertime > setupgetout2)
                            {
                                ndot = getoutforvertime - setupgetout2;
                                t_total_tempot_nightt = decimal.Parse((getnight(setupgetout2.ToString(), getoutforvertime.ToString(), "dtr")).ToString());
                            }
                            t_total_tempot = decimal.Parse(ndot.TotalHours.ToString());

                            //get final ot
                            if (overttime.Count() > 0)
                            {
                                othrs = decimal.Parse(overttime[0]["overtimehoursapp"].ToString()) >= 1 ? decimal.Parse(overttime[0]["overtimehoursapp"].ToString()) : 0;
                                othrsnight = decimal.Parse(overttime[0]["overtimenighthoursapp"].ToString()) >= 1 ? decimal.Parse(overttime[0]["overtimenighthoursapp"].ToString()) : 0;
                            }
                            othrs = othrs > 0 ? othrs : 0;
                            othrsnight = othrsnight > 0 ? othrsnight : 0;

                            //get break hours
                            DateTime fdateout1 = new DateTime();
                            DateTime fdatein2 = new DateTime();
                            if (tmout1.ToString().Length > 0 && tmin2.ToString().Length > 2)
                            {
                                fdateout1 = getoutt1;
                                fdatein2 = getinn1;
                                if (dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True" && dtgetshiftcode[0]["flexbreak"].ToString() == "False")
                                {
                                    if (fdateout1 >= setupgetout1)
                                        fdateout1 = setupgetout1;
                                    if (fdatein2 <= setupgetin2)
                                        fdatein2 = setupgetin2;

                                    if (fdatein2 > fdateout1)
                                    {
                                        //need to break down night break in shift code
                                        brkhrs = fdatein2 - fdateout1;
                                        nightbreakdeduct = double.Parse((getnight(fdatein2.ToString(), fdateout1.ToString(), "dtr")).ToString());
                                        breakdeduct = double.Parse(brkhrs.TotalHours.ToString()) > breakdeduct ? double.Parse(brkhrs.TotalHours.ToString()) - nightbreakdeduct : breakdeduct;
                                    }
                                }
                            }

                            nightbreakdeduct = yyyyy == 1 ? 0 : nightbreakdeduct > 0 ? nightbreakdeduct : 0;
                            breakdeduct = yyyyy == 1 ? 0 : breakdeduct > 0 ? breakdeduct : 0;
                            overbreak = yyyyy == 1 ? 0 : overbreak > 0 ? overbreak : 0;

                            //get hours between punch
                            DateTime fdatein = new DateTime();
                            DateTime fdateout = new DateTime();

                            decimal excesshrs = 0;
                            if (getout > getin)
                            {
                                DateTime tmppp = new DateTime();
                                if (leaveh == "True")
                                {
                                    switch (int.Parse(inoutduringleavehalfday))
                                    {
                                        case 1:
                                            if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                                            {
                                                if (getout > getin)
                                                {
                                                    stregh = getout - getin;
                                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                                    excesshrs = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - nohleave : 0;
                                                    reghours = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - excesshrs : reghours;

                                                    night = decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                                                    reghours = reghours - night;
                                                }
                                            }
                                            else //not open shift
                                            {
                                                tmppp = setupgetout2;

                                                if (getin > setupgetin1)
                                                    fdatein = getin;
                                                else
                                                    fdatein = setupgetin1;

                                                if (getout > tmppp)
                                                    fdateout = tmppp;
                                                else
                                                    fdateout = getout;
                                                if (fdateout > fdatein)
                                                {
                                                    stregh = fdateout - fdatein;
                                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                                    night = decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString());
                                                    reghours = reghours - night;
                                                }
                                            }
                                            break;
                                        case 2:

                                            if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                                            {
                                                if (getout > getin)
                                                {
                                                    stregh = getout - getin;
                                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                                    excesshrs = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - nohleave : 0;
                                                    reghours = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - excesshrs : reghours;
                                                    night = decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString());
                                                    reghours = reghours - night;
                                                }
                                            }
                                            else //not open shift
                                            {
                                                tmppp = setupgetin1;
                                                if (getin > tmppp)
                                                    fdatein = getin;
                                                else
                                                    fdatein = tmppp;

                                                if (getout > setupgetout2)
                                                    fdateout = setupgetout2;
                                                else
                                                    fdateout = getout;
                                                if (getout > tmppp)
                                                {
                                                    stregh = fdateout - fdatein;
                                                    reghours = decimal.Parse(stregh.TotalHours.ToString());
                                                    night = decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString());
                                                    reghours = reghours - night;
                                                }
                                            }
                                            break;
                                    }
                                }
                                else
                                {

                                    if (dtgetshiftcode[0]["openshift"].ToString() == "True")
                                    {
                                        if (getout > getin)
                                        {
                                            stregh = getout - getin;
                                            reghours = decimal.Parse(stregh.TotalHours.ToString());
                                            night = (decimal.Parse((getnight(getin.ToString(), getout.ToString(), "dtr")).ToString())) - decimal.Parse(nightbreakdeduct.ToString());
                                            excesshrs = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - (decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5")) : 0;
                                            if (reghours >= decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") + decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString()))
                                                reghours = reghours - decimal.Parse(dtgetshiftcode[0]["nightbreakhours"].ToString());
                                            else
                                                reghours = reghours - excesshrs;

                                            reghours = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) ? decimal.Parse(dr["FixNumberOfHours"].ToString()) : reghours;
                                            reghours = reghours - night;

                                        }
                                    }
                                    else
                                    {
                                        if (getin > setupgetin1)
                                            fdatein = getin;
                                        else
                                            fdatein = setupgetin1;

                                        if (getout > setupgetout2)
                                            fdateout = setupgetout2;
                                        else
                                            fdateout = getout;

                                        if (fdateout > fdatein)
                                        {
                                            stregh = fdateout - fdatein;
                                            night = (decimal.Parse((getnight(fdatein.ToString(), fdateout.ToString(), "dtr")).ToString())) - decimal.Parse(nightbreakdeduct.ToString());
                                            reghours = decimal.Parse(stregh.TotalHours.ToString()) - night;
                                            reghours = reghours > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? reghours - decimal.Parse(breakdeduct.ToString()) : reghours;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (offsethrs > 0)
                                absentw = "False";
                            else
                                absentw = "True";
                            ////------------------------------------------------------------------
                            ////mao rani sa else --absentw = "True";
                            ////good for daily set up
                            ////posible for last pay no absent on no duty during payroll day
                            //if (Convert.ToDateTime(curding) < Convert.ToDateTime(dtrdate))
                            //    absentw = "False";
                            //else
                            //    absentw = "True";
                            ////------------------------------------------------------------------

                            if (rd == "True")
                            {
                                absentw = "False";
                                absenth = "False";
                            }

                            if (hd == "True")
                            {
                                absentw = "False";
                                absenth = "False";
                            }

                            if (leavew == "True" || leaveh == "True")
                                absentw = "False";

                        }
                    }
                    //dre end
                    //data finalizing
                    night = night > decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? night - decimal.Parse(breakdeduct.ToString()) : night;
                    night = night > 0 ? night : 0;
                    reghours = reghours > 0 ? reghours : 0;

                    /**OBT**/
                    /**BTK 01212020**/
                    if (dtrdate == "04/08/2020")
                    {
                    }

                    if (getobt.Count() > 0)
                    {
                        reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                        irregularity = "false";
                        absentw = "False";
                        absenth = "False";
                        offsethrs = 0;
                        night = 0;
                        t_total = 0;//late
                        othrs = 0;
                        othrsnight = 0;
                    }

                    //+overbreak
                    t_total = dtgetshiftcode[0]["openshift"].ToString() == "True" ? 0 : t_total;

                    if (t_total >= decimal.Parse(dr["FixNumberOfHours"].ToString()) / 2)
                    {
                        t_total = t_total - (decimal.Parse(dr["FixNumberOfHours"].ToString()) / 2);
                        absenth = "True";
                    }

                    reghours = dtcompanydet.Rows[0]["ob_roles"].ToString() == "2" ? reghours + overbreak : reghours;
                    t_total = dtcompanydet.Rows[0]["ob_roles"].ToString() == "2" ? t_total - overbreak : t_total;


                    if (finalut >= decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5"))
                    {
                        decimal halfhalf = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5");
                        if (finalut >= halfhalf + decimal.Parse(breakdeduct.ToString()))
                            finalut = finalut - decimal.Parse(breakdeduct.ToString());
                        finalut = finalut >= decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") ? finalut - (decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5")) : 0;
                        absenth = "True";
                    }

                    decimal absenthlfdayhrs = absenth == "True" ? decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") : 0;
                    decimal leavehlfdayhrs = leaveh == "True" ? decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5") : 0;
                    decimal total_hrs = decimal.Parse(string.Format("{0:n2}", reghours)) + decimal.Parse(string.Format("{0:n2}", night)) + decimal.Parse(string.Format("{0:n2}", t_total)) + decimal.Parse(string.Format("{0:n2}", finalut)) + decimal.Parse(string.Format("{0:n2}", offsethrs)) + absenthlfdayhrs + leavehlfdayhrs;


                    // string rd = dtgetrestday.Count() > 0 ? "True" : "False";  //dtgetrestday[0]["day"].ToString() == dayofweek[1].ToString() ? "True" : "False";
                    string daytyoeid = dtgetallDayType.Count() > 0 ? dtgetallDayType[0]["id"].ToString() : dtdaytype.Rows[0]["id"].ToString();
                    string daytypecode = dtgetallDayType.Count() > 0 ? dtgetallDayType[0]["DayType"].ToString() : dtdaytype.Rows[0]["DayType"].ToString();

                    //get late
                    decimal totallate = t_total;
                    decimal getlackinghrs = 0;
                    decimal totalhrs2 = totallate + finalut + reghours;

                    night = Math.Round(night, 5);
                    reghours = Math.Round(reghours, 5);

                    if (dtcompanydet.Rows[0]["premiumsettings"].ToString() == "EqualHalfDay")
                    {
                        if (reghours + night + offsethrs < decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse("0.5"))
                        {
                            if (rd == "True" || hd == "True")
                            {
                                DataRow[] dtgetallDayTypeifdakooreqaulhalfday = dtdaytype.Select("id=1");
                                daymultiplier = dtgetallDayTypeifdakooreqaulhalfday.Count() > 0 ? decimal.Parse(dtgetallDayTypeifdakooreqaulhalfday[0]["workingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["workingdays"].ToString());
                                nightmultiplier = dtgetallDayTypeifdakooreqaulhalfday.Count() > 0 ? decimal.Parse(dtgetallDayTypeifdakooreqaulhalfday[0]["nightworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["nightworkingdays"].ToString());
                                otdaymultiplier = dtgetallDayTypeifdakooreqaulhalfday.Count() > 0 ? decimal.Parse(dtgetallDayTypeifdakooreqaulhalfday[0]["OTworkingdays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTworkingdays"].ToString());
                                otnightmultiplier = dtgetallDayTypeifdakooreqaulhalfday.Count() > 0 ? decimal.Parse(dtgetallDayTypeifdakooreqaulhalfday[0]["OTNightWorkingDays"].ToString()) : decimal.Parse(dtdaytype.Rows[0]["OTNightWorkingDays"].ToString());
                            }
                        }
                    }

                    //CLI Level Setup for Officer 3 and Up
                    if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                    {
                        if (dr["DivisionId2"].ToString() == "7" || dr["DivisionId2"].ToString() == "8" || dr["DivisionId2"].ToString() == "9" || dr["DivisionId2"].ToString() == "10" || dr["DivisionId2"].ToString() == "11" || dr["DivisionId2"].ToString() == "12" || dr["DivisionId2"].ToString() == "13" || dr["DivisionId2"].ToString() == "14" || dr["DivisionId2"].ToString() == "15" || dr["DivisionId2"].ToString() == "16" || dr["DivisionId2"].ToString() == "19" || dr["DivisionId2"].ToString() == "21" || dr["DivisionId2"].ToString() == "22" || dr["DivisionId2"].ToString() == "23" || dr["DivisionId2"].ToString() == "24" || dr["DivisionId2"].ToString() == "25")
                        {
                            if (getleave.Count() > 0)
                            {
                                if (getleave[0]["WithPay"].ToString() == "True")
                                {
                                    reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                                    absentw = "False";
                                    absenth = "False";
                                }
                                else
                                {
                                    if (decimal.Parse(getleave[0]["noh"].ToString()) >= 1)
                                    {
                                        reghours = 0;
                                        absentw = "True";
                                        absenth = "False";
                                        leavew = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                    else
                                    {
                                        reghours = 4;
                                        absentw = "False";
                                        leaveh = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                        inoutduringleavehalfday = getleave[0]["inoutduringhalfdayleave"].ToString().Length > 0 ? getleave[0]["inoutduringhalfdayleave"].ToString() : "0";

                                        leavewoamount = decimal.Parse(getleave[0]["dailyrate"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                }
                                irregularity = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                            else
                            {
                                reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                                irregularity = "false";
                                absentw = "False";
                                absenth = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                        }
                        //for non-punching CLI Setup
                        if (dr["nonepunching"].ToString() == "True")
                        {
                            if (getleave.Count() > 0)
                            {
                                if (getleave[0]["WithPay"].ToString() == "True")
                                {
                                    reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                                    absentw = "False";
                                    absenth = "False";
                                }
                                else
                                {
                                    if (decimal.Parse(getleave[0]["noh"].ToString()) >= 1)
                                    {
                                        reghours = 0;
                                        absentw = "True";
                                        absenth = "False";
                                        leavew = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                    else
                                    {
                                        reghours = 4;
                                        absentw = "False";
                                        leaveh = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                        inoutduringleavehalfday = getleave[0]["inoutduringhalfdayleave"].ToString().Length > 0 ? getleave[0]["inoutduringhalfdayleave"].ToString() : "0";
                                        leavewoamount = decimal.Parse(getleave[0]["dailyrate"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                }
                                irregularity = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                            else
                            {
                                reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                                irregularity = "false";
                                absentw = "False";
                                absenth = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                        }

                        //derictor/executive/managers
                        if (dr["divisionid"].ToString() == "1" || dr["divisionid"].ToString() == "4" || dr["divisionid"].ToString() == "5")
                        {
                            if (getleave.Count() > 0)
                            {
                                if (getleave[0]["WithPay"].ToString() == "True")
                                {
                                    reghours = decimal.Parse(dr["FixNumberOfHours"].ToString());
                                    absentw = "False";
                                    absenth = "False";
                                }
                                else
                                {
                                    if (decimal.Parse(getleave[0]["noh"].ToString()) >= 1)
                                    {
                                        reghours = 0;
                                        absentw = "True";
                                        absenth = "False";
                                        leavew = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                    else
                                    {
                                        reghours = 4;
                                        absentw = "False";
                                        leaveh = "True";
                                        nohleave = decimal.Parse(dr["FixNumberOfHours"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                        inoutduringleavehalfday = getleave[0]["inoutduringhalfdayleave"].ToString().Length > 0 ? getleave[0]["inoutduringhalfdayleave"].ToString() : "0";
                                        leavewoamount = decimal.Parse(getleave[0]["dailyrate"].ToString()) * decimal.Parse(getleave[0]["noh"].ToString());
                                    }
                                }
                                irregularity = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                            else
                            {
                                string hireddd = dr["datehired"].ToString();
                                //string hireddd = getemp.Rows[0]["datehired"].ToString();
                                if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                                {
                                    reghours = decimal.Parse(dr["FixNumberOfHours"].ToString()); //instead of kani
                                }
                                else
                                {
                                    //absent ang nonpunching due to date hired
                                    reghours = 0;
                                    absentw = "True";
                                }
                                irregularity = "false";
                                //absentw = "False";
                                absenth = "False";
                                offsethrs = 0;
                                night = 0;
                                finalut = 0;//undertime
                                totallate = 0;//late
                                othrs = 0;
                                othrsnight = 0;
                            }
                        }
                    }
                    /**BTK 03262020
                    * LEAVE EXEMPTION**/

                    if (leavew == "True")
                    {
                        reghours = 0;
                        totallate = 0;
                        finalut = 0;
                        offsethrs = 0;
                        night = 0;
                        othrs = 0;
                        othrsnight = 0;
                    }
                    if (absentw == "True")
                    {
                        reghours = 0;
                        offsethrs = 0;
                        night = 0;
                        finalut = 0;//undertime
                        totallate = 0;//late
                        othrs = 0;
                        othrsnight = 0;
                    }

                    /**HOLIDAY**/

                    //dre sud for hd
                    if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                    {
                        if (hd == "True")
                        {
                            if (dr["nonepunching"].ToString() == "True" || dr["newhire"].ToString() == "True")
                            {
                                if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(dr["datehired"].ToString()))
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absentw = "False";
                                    reghours = 0;
                                    offsethrs = 0;
                                    night = 0;
                                    othrs = 0;
                                    othrsnight = 0;
                                    tmin1 = "0";
                                    tmout1 = "0";
                                    tmin2 = "0";
                                    tmout2 = "0";
                                    rd = "False";
                                    hd = "False";
                                }
                                else
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absenth = "False";
                                    absentw = "False";
                                    if (getrdl.Count() == 0)
                                    {
                                        //no holiday premuim
                                        if (dr["DivisionId"].ToString() == "1" || dr["DivisionId"].ToString() == "2" || dr["DivisionId"].ToString() == "4" || dr["DivisionId"].ToString() == "5" || dr["nonepunching"].ToString() == "True" || dr["DivisionId2"].ToString() == "7" || dr["DivisionId2"].ToString() == "8" || dr["DivisionId2"].ToString() == "9" || dr["DivisionId2"].ToString() == "10" || dr["DivisionId2"].ToString() == "11" || dr["DivisionId2"].ToString() == "12" || dr["DivisionId2"].ToString() == "13" || dr["DivisionId2"].ToString() == "14" || dr["DivisionId2"].ToString() == "15" || dr["DivisionId2"].ToString() == "16" || dr["DivisionId2"].ToString() == "19" || dr["DivisionId2"].ToString() == "21" || dr["DivisionId2"].ToString() == "22" || dr["DivisionId2"].ToString() == "23" || dr["DivisionId2"].ToString() == "24" || dr["DivisionId2"].ToString() == "25")
                                        {
                                            reghours = 0;
                                            offsethrs = 0;
                                            night = 0;
                                            othrs = 0;
                                            othrsnight = 0;
                                            tmin1 = "0";
                                            tmout1 = "0";
                                            tmin2 = "0";
                                            tmout2 = "0";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(dr["datehired"].ToString()))
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absentw = "True";
                                    reghours = 0;
                                    offsethrs = 0;
                                    night = 0;
                                    othrs = 0;
                                    othrsnight = 0;
                                    tmin1 = "0";
                                    tmout1 = "0";
                                    tmin2 = "0";
                                    tmout2 = "0";
                                    rd = "False";
                                    hd = "False";
                                }
                                else
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absenth = "False";
                                    absentw = "False";
                                    if (getrdl.Count() == 0)
                                    {
                                        //no holiday premuim
                                        if (dr["DivisionId"].ToString() == "1" || dr["DivisionId"].ToString() == "4" || dr["DivisionId"].ToString() == "5" || dr["nonepunching"].ToString() == "True" || dr["DivisionId2"].ToString() == "7" || dr["DivisionId2"].ToString() == "8" || dr["DivisionId2"].ToString() == "9" || dr["DivisionId2"].ToString() == "10" || dr["DivisionId2"].ToString() == "11" || dr["DivisionId2"].ToString() == "12" || dr["DivisionId2"].ToString() == "13" || dr["DivisionId2"].ToString() == "14" || dr["DivisionId2"].ToString() == "15" || dr["DivisionId2"].ToString() == "16" || dr["DivisionId2"].ToString() == "19" || dr["DivisionId2"].ToString() == "21" || dr["DivisionId2"].ToString() == "22" || dr["DivisionId2"].ToString() == "23" || dr["DivisionId2"].ToString() == "24" || dr["DivisionId2"].ToString() == "25")
                                        {
                                            reghours = 0;
                                            offsethrs = 0;
                                            night = 0;
                                            othrs = 0;
                                            othrsnight = 0;
                                            tmin1 = "0";
                                            tmout1 = "0";
                                            tmin2 = "0";
                                            tmout2 = "0";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                    {
                        if (rd == "True")
                        {
                            if (dr["nonepunching"].ToString() == "True" || dr["newhire"].ToString() == "True")
                            {
                                if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(dr["datehired"].ToString()))
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absentw = "False";
                                    reghours = 0;
                                    offsethrs = 0;
                                    night = 0;
                                    othrs = 0;
                                    othrsnight = 0;
                                    tmin1 = "0";
                                    tmout1 = "0";
                                    tmin2 = "0";
                                    tmout2 = "0";
                                    rd = "False";
                                    hd = "False";
                                }
                                else
                                {
                                    irregularity = "False";
                                    finalut = 0; //undertime
                                    totallate = 0; //late
                                    absenth = "False";
                                    absentw = "False";
                                    if (getrdl.Count() == 0)
                                    {
                                        reghours = 0;
                                        offsethrs = 0;
                                        night = 0;
                                        othrs = 0;
                                        othrsnight = 0;
                                        tmin1 = "0";
                                        tmout1 = "0";
                                        tmin2 = "0";
                                        tmout2 = "0";
                                    }
                                    else
                                    {
                                        /**BTK 01232020
                                         * FROM RD WORK VERFICATION NO LOGS**/
                                        if (tmin1.Length == 2 || tmout2.Length == 2)
                                        {
                                            absentw = "True";
                                            irregularity = "True";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //sud for RD
                                if (Convert.ToDateTime(hiredd) <= Convert.ToDateTime(dtrdate))
                                {
                                    if (Convert.ToDateTime(dtrdate) < Convert.ToDateTime(dr["datehired"].ToString()))
                                    {
                                        irregularity = "False";
                                        finalut = 0; //undertime
                                        totallate = 0; //late
                                        absentw = "True";
                                        reghours = 0;
                                        offsethrs = 0;
                                        night = 0;
                                        othrs = 0;
                                        othrsnight = 0;
                                        tmin1 = "0";
                                        tmout1 = "0";
                                        tmin2 = "0";
                                        tmout2 = "0";
                                        rd = "False";
                                        hd = "False";
                                    }
                                    else
                                    {
                                        irregularity = "False";
                                        finalut = 0; //undertime
                                        totallate = 0; //late
                                        absenth = "False";
                                        absentw = "False";
                                        if (getrdl.Count() == 0)
                                        {
                                            reghours = 0;
                                            offsethrs = 0;
                                            night = 0;
                                            othrs = 0;
                                            othrsnight = 0;
                                            tmin1 = "0";
                                            tmout1 = "0";
                                            tmin2 = "0";
                                            tmout2 = "0";
                                        }
                                        else
                                        {
                                            /**BTK 01232020
                                             * FROM RD WORK VERFICATION NO LOGS**/
                                            if (tmin1.Length == 2 || tmout2.Length == 2)
                                            {
                                                absentw = "True";
                                                irregularity = "True";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    decimal total = (Math.Round(reghours + night + othrs + othrsnight + offsethrs, 5));
                    string tardyamt = (Math.Round((decimal.Parse(Math.Round(reghours > 0 ? totallate : 0, 5).ToString()) + decimal.Parse(Math.Round(reghours > 0 ? finalut : 0, 5).ToString())) * decimal.Parse(hourlyrate), 5)).ToString();
                    decimal getnightamtaddon = 0; //(Math.Round(night * decimal.Parse(dr["hrrate"].ToString()) * nightmultiplier, 5)) - (Math.Round(night * decimal.Parse(dr["hrrate"].ToString()), 5));
                    decimal getregamtaddon = 0; //(Math.Round(reghours * decimal.Parse(dr["hrrate"].ToString()) * daymultiplier, 5)) - (Math.Round(reghours * decimal.Parse(dr["hrrate"].ToString()), 5));
                    decimal getnightamt = (Math.Round(night * decimal.Parse(hourlyrate) * nightmultiplier, 5));
                    decimal regamount = Math.Round((reghours + offsethrs) * decimal.Parse(hourlyrate) * daymultiplier, 5);
                    decimal otamount = (Math.Round(decimal.Parse(hourlyrate) * othrs * otdaymultiplier, 5));
                    decimal otnightamount = (Math.Round(decimal.Parse(hourlyrate) * othrsnight * otnightmultiplier, 5));
                    decimal offsetamt = (Math.Round(decimal.Parse(hourlyrate) * offsethrs, 5));
                    decimal totalamt = (Math.Round(regamount + getnightamt + otamount + otnightamount + offsetamt, 5));
                    decimal absentamount = 0;

                    if (absentw == "True")
                        absentamount = Math.Round(decimal.Parse(dailyrate), 2);
                    else if (absenth == "True")
                        absentamount = decimal.Parse(Math.Round(decimal.Parse("0.5") * decimal.Parse(dailyrate), 0).ToString());
                    else
                        absentamount = decimal.Parse("0.00");

                    decimal gghh = getnightamt + regamount + otamount + otnightamount;//(Math.Round(decimal.Parse(dtgetemp.Rows[0]["DailyRate"].ToString()) - decimal.Parse(tardyamt), 5));
                    decimal hdamount = 0;

                    //get rd
                    if (rd == "True")
                    {
                        if (getrdl.Count() > 0) //&& lbl_timein1.Text.Length > 2 && lbl_timeout2.Text.Length > 2)
                            rdamount = getregamtaddon + getnightamtaddon;
                        else
                            rdamount = 0;
                    }
                    else
                        rdamount = 0;

                    //holliday
                    if (int.Parse(daytyoeid) > 1)
                    {
                        if (reghours + night > 0)
                            hdamount = getregamtaddon + getnightamtaddon;
                        else
                        {
                            if (dr["PayrollTypeId"].ToString() == "1")
                                hdamount = (Math.Round(decimal.Parse(dailyrate)));
                            else
                                hdamount = 0;
                        }
                    }
                    else
                        hdamount = 0;

                    totallate = rd == "True" ? 0 : totallate;
                    finalut = rd == "True" ? 0 : finalut;

                    //dre dapita late
                    decimal lateamt = Math.Round(totallate, 2) * decimal.Parse(hourlyrate);
                    decimal underamt = Math.Round(finalut, 2) * decimal.Parse(hourlyrate);

                    // decimal offsetamt = offsethrs * decimal.Parse(dr["hourlyrate"].ToString());

                    decimal netamount = totalamt + leaveamount + rdamount + hdamount;
                    decimal thours = reghours + night + offsethrs;
                    //if (dr["surgepay_effective"].ToString().Length > 0 )
                    //{
                    //    DateTime surgepay_effective=Convert.ToDateTime(dr["surgepay_effective"].ToString());
                    //    DateTime curr_date = Convert.ToDateTime(dtrdate);
                    //    if (surgepay_effective <= curr_date)
                    //    {
                    //        if (getspecdate[1].ToString() == "Friday" || getspecdate[1].ToString() == "Saturday" || getspecdate[1].ToString() == "Sunday")
                    //            surge_pay = reghours + night > 0 ? decimal.Parse(dailyrate) * decimal.Parse("0.20") : 0;
                    //    }
                    //}


                    /**LEAVE**/
                    if (leavew == "True" || leaveh == "False")
                    {
                        if (netamount > leaveamount)
                            netamount = netamount - leaveamount;
                    }


                    /**DISREGARD LOGS IF ON LEAVE**/
                    if (leavew == "True")
                    {
                        absentw = "False";
                        absenth = "False";
                        absentamount = 0;
                        reghours = 0;
                        regamount = 0;
                        total = 0;
                        totalamt = 0;
                        netamount = 0;
                        totallate = 0;
                        lateamt = 0;
                        tardyamt = "0";
                        finalut = 0;
                        underamt = 0;
                        offsethrs = 0;
                        night = 0;
                        othrs = 0;
                        othrsnight = 0;
                    }

                    mdr["timein1"] = tmin1 == "0" ? "--" : tmin1;
                    mdr["timeout1"] = tmout1 == "0" ? "--" : dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True" ? tmout1 : "--";
                    mdr["timein2"] = tmin2 == "0" ? "--" : dtgetshiftcode[0]["mandatorytopunch"].ToString() == "True" ? tmin2 : "--";
                    mdr["timeout2"] = tmout2 == "0" ? "--" : tmout2;
                    mdr["olw"] = leavew;
                    mdr["aw"] = absentw;
                    mdr["olh"] = leaveh;
                    mdr["ah"] = absenth;
                    mdr["reg_hr"] = string.Format("{0:n2}", reghours);
                    mdr["night"] = string.Format("{0:n2}", night);
                    mdr["offsethrs"] = string.Format("{0:n2}", offsethrs);
                    mdr["ot"] = string.Format("{0:n2}", othrs);
                    mdr["otn"] = string.Format("{0:n2}", othrsnight);
                    mdr["totalhrs"] = string.Format("{0:n2}", total);
                    mdr["late"] = string.Format("{0:n2}", totallate);
                    mdr["ut"] = string.Format("{0:n2}", finalut);
                    mdr["nethours"] = string.Format("{0:n2}", total);
                    mdr["reg_amount"] = string.Format("{0:n2}", regamount + hdamount + leaveamount);
                    mdr["night_amount"] = string.Format("{0:n2}", getnightamt);
                    mdr["ot_amount"] = string.Format("{0:n2}", otamount);
                    mdr["otn_amount"] = string.Format("{0:n2}", otnightamount);
                    mdr["totalamt"] = string.Format("{0:n2}", totalamt);
                    mdr["tardyamt"] = string.Format("{0:n2}", lateamt + underamt);
                    mdr["absentamt"] = string.Format("{0:n2}", absentamount + leavewoamount);
                    mdr["restdayamt"] = string.Format("{0:n2}", rdamount);
                    mdr["holidayamt"] = string.Format("{0:n2}", hdamount);
                    mdr["leaveamt"] = string.Format("{0:n2}", leaveamount);
                    mdr["netamt"] = string.Format("{0:n2}", netamount);
                    mdr["lateamt"] = string.Format("{0:n2}", lateamt);
                    mdr["lbl_undera"] = string.Format("{0:n2}", underamt);
                    mdr["hrrate"] = decimal.Parse(hourlyrate) * daymultiplier;
                    mdr["nightrate"] = decimal.Parse(hourlyrate) * nightmultiplier;
                    mdr["otrate"] = decimal.Parse(hourlyrate) * otdaymultiplier;
                    mdr["otnrate"] = decimal.Parse(hourlyrate) * otnightmultiplier;
                    mdr["daytypeid"] = daytyoeid; //dtgetallDayType.Count() > 0 ? dtgetallDayType[0]["id"].ToString() : "1";
                    mdr["daytype"] = daytypecode; //dtgetallDayType.Count() > 0 ? dtgetallDayType[0]["DayType"].ToString(): "Working Day";
                    mdr["dm"] = string.Format("{0:n2}", daymultiplier); //dtgetallDayType.Count() > 0 ? dtgetallDayType[0]["workingdays"].ToString() : "1.00";
                    mdr["restday"] = rd;
                    mdr["FixNumberOfHours"] = dr["FixNumberOfHours"].ToString();
                    mdr["dailyrate"] = dailyrate;
                    mdr["paytype"] = dr["PayrollTypeId"].ToString();
                    mdr["tempot"] = string.Format("{0:n2}", t_total_tempot);
                    mdr["overbreak"] = string.Format("{0:n2}", overbreak);
                    mdr["status"] = status;
                    mdr["irregularity"] = irregularity;
                    mdr["total_tempot_reg"] = t_total_tempot > t_total_tempot_nightt ? t_total_tempot - t_total_tempot_nightt : 0;
                    mdr["total_tempot_night"] = t_total_tempot_nightt;
                    mdr["cnt"] = row_cnt;
                    mdr["rownumber"] = rownumber;
                    mdr["setupfinalout"] = setupfinalout;
                    mdr["surge_pay"] = surge_pay;
                    mdr["reghourlyrate"] = hourlyrate;
                    mdr["monthlyrate"] = monthlyrate;
                    mdr["payrollrate"] = payrollrate;
                    mdr["otmeal"] = mealtotal;
                    masters.Rows.Add(mdr);

                    total_tardy_hrs = total_tardy_hrs + totallate + finalut;
                    total_tempot_hrs = total_tempot_hrs + t_total_tempot + t_total_tempot_nightt;

                }
            }
        }

        count = (Convert.ToInt32(count) + 1).ToString();
        return count;
    }

    public static string getnight(string inn, string oout, string key)
    {
        //set up
        DateTime startnight = new DateTime();
        DateTime endnight = new DateTime();
        DateTime twlvehalftnight = new DateTime();
        DateTime startmorningnight = new DateTime();
        DateTime startDate = Convert.ToDateTime(inn);
        DateTime startDate1 = Convert.ToDateTime(inn);
        DateTime stopDate = Convert.ToDateTime(oout);
        TimeSpan nod = new TimeSpan();
        TimeSpan tnod = new TimeSpan();

        switch (key)
        {
            case "dtr":
                string[] getdate = inn.ToString().Split(' ');
                startnight = Convert.ToDateTime(getdate[0] + " 22:00:00.000");
                endnight = Convert.ToDateTime(getdate[0] + " 06:00:00.000").AddDays(1);
                twlvehalftnight = Convert.ToDateTime(getdate[0] + " 00:00:00.000").AddDays(1);
                startmorningnight = Convert.ToDateTime(getdate[0] + " 00:00:00.000").AddDays(1);

                while ((startDate) <= stopDate)
                {
                    if (startDate.ToString().Contains("PM"))
                    {
                        if (startDate.TimeOfDay >= startnight.TimeOfDay)
                        {
                            if (stopDate <= endnight)
                            {
                                if (startDate1 > startnight)
                                    nod = stopDate - startDate;
                                else
                                    nod = stopDate - startnight;
                                break;
                            }
                            else
                            {
                                nod = endnight - startDate;
                                break;
                            }
                        }
                    }
                    else if (startDate.ToString().Contains("AM"))
                    {
                        if (startDate.TimeOfDay <= endnight.TimeOfDay)
                        {
                            if (stopDate.TimeOfDay <= endnight.TimeOfDay)
                            {
                                nod = stopDate.TimeOfDay - startDate.TimeOfDay;
                                break;
                            }
                        }
                    }

                    startDate = startDate.AddHours(1);
                }
                tnod = nod;
                break;
            case "shiftcode":
                startnight = Convert.ToDateTime("22:00:00.000");
                endnight = Convert.ToDateTime("06:00:00.000").AddDays(1);
                twlvehalftnight = Convert.ToDateTime("00:00:00.000").AddDays(1);
                startmorningnight = Convert.ToDateTime("00:00:00.000").AddDays(1);
                while ((startDate) <= stopDate)
                {
                    if (stopDate > startnight)
                    {
                        if (stopDate > endnight)
                        {
                            if (startDate > startnight)
                                nod = endnight - startDate;
                            else
                                nod = endnight - startnight;
                            break;
                        }
                        else
                        {
                            if (startDate > startnight)
                                nod = stopDate - startDate;
                            else
                                nod = stopDate - startnight;
                            break;
                        }
                    }
                    startDate = startDate.AddHours(1);
                }
                tnod = nod;

                break;
        }
        return tnod.TotalHours.ToString();
    }



    [WebMethod]
    public static string checkdtrgo(string ddl_pg)
    {
        string result = "0";
        DataTable dtinfinate = dbhelper.getdata("select Id, LastName+', '+FirstName+' '+MiddleName as employee,(select ShiftCode from MShiftCode where Id=ShiftCodeId)shiftcode,MonthlyRate,DailyRate,HourlyRate,(select PayrollType from MPayrollType where Id=PayrollTypeId)PayrollTypeId,PayrollGroupId from MEmployee where PayrollGroupId = " + ddl_pg + " and HourlyRate = 'Infinity' ");
        DataTable dtchker = dbhelper.getdata("select Id, LastName+', '+FirstName+' '+MiddleName as employee,(select ShiftCode from MShiftCode where Id=ShiftCodeId)shiftcode,MonthlyRate,DailyRate,HourlyRate,(select PayrollType from MPayrollType where Id=PayrollTypeId)PayrollTypeId,PayrollGroupId from MEmployee where PayrollGroupId = " + ddl_pg + " and (HourlyRate = '0.00' or HourlyRate = '0')");
        DataTable dtrd = dbhelper.getdata("select Id,LastName+', '+FirstName+' '+MiddleName as employee,(select ShiftCode from MShiftCode where Id=ShiftCodeId)shiftcode,MonthlyRate,DailyRate,HourlyRate from MEmployee where PayrollGroupId = " + ddl_pg + " and ShiftCodeId = 1 ");

        if (dtinfinate.Rows.Count > 0)
        {
            result = "1";
        }

        if (dtchker.Rows.Count > 0)
        {
            result = "1";
        }

        if (dtrd.Rows.Count > 0)
        {
            result = "1";
        }

        HttpContext.Current.Session["dtinfinate"] = dtinfinate;
        HttpContext.Current.Session["dtchker"] = dtchker;
        HttpContext.Current.Session["dtrd"] = dtrd;


        return result;
    }




    protected static DataTable loadable()
    {
        //MASTER DATA
        DataTable master = new DataTable();
        master.Columns.Add(new DataColumn("rownumber", typeof(string)));
        master.Columns.Add(new DataColumn("cnt", typeof(string)));
        master.Columns.Add(new DataColumn("empid", typeof(string)));
        master.Columns.Add(new DataColumn("employee", typeof(string)));
        master.Columns.Add(new DataColumn("shiftcodeid", typeof(string)));
        master.Columns.Add(new DataColumn("ShiftCode", typeof(string)));
        master.Columns.Add(new DataColumn("date", typeof(string)));
        master.Columns.Add(new DataColumn("daytypeid", typeof(string)));
        master.Columns.Add(new DataColumn("daytype", typeof(string)));
        master.Columns.Add(new DataColumn("dm", typeof(string)));
        master.Columns.Add(new DataColumn("restday", typeof(string)));
        master.Columns.Add(new DataColumn("timein1", typeof(string)));
        master.Columns.Add(new DataColumn("timeout1", typeof(string)));
        master.Columns.Add(new DataColumn("timein2", typeof(string)));
        master.Columns.Add(new DataColumn("timeout2", typeof(string)));
        master.Columns.Add(new DataColumn("olw", typeof(string)));
        master.Columns.Add(new DataColumn("aw", typeof(string)));
        master.Columns.Add(new DataColumn("olh", typeof(string)));
        master.Columns.Add(new DataColumn("ah", typeof(string)));
        master.Columns.Add(new DataColumn("reg_hr", typeof(string)));
        master.Columns.Add(new DataColumn("night", typeof(string)));
        master.Columns.Add(new DataColumn("offsethrs", typeof(string)));
        master.Columns.Add(new DataColumn("ot", typeof(string)));
        master.Columns.Add(new DataColumn("otn", typeof(string)));
        master.Columns.Add(new DataColumn("totalhrs", typeof(string)));
        master.Columns.Add(new DataColumn("late", typeof(string)));
        master.Columns.Add(new DataColumn("ut", typeof(string)));
        master.Columns.Add(new DataColumn("nethours", typeof(string)));
        master.Columns.Add(new DataColumn("reg_amount", typeof(string)));
        master.Columns.Add(new DataColumn("night_amount", typeof(string)));
        master.Columns.Add(new DataColumn("ot_amount", typeof(string)));
        master.Columns.Add(new DataColumn("otn_amount", typeof(string)));
        master.Columns.Add(new DataColumn("totalamt", typeof(string)));
        master.Columns.Add(new DataColumn("tardyamt", typeof(string)));
        master.Columns.Add(new DataColumn("absentamt", typeof(string)));
        master.Columns.Add(new DataColumn("restdayamt", typeof(string)));
        master.Columns.Add(new DataColumn("holidayamt", typeof(string)));
        master.Columns.Add(new DataColumn("leaveamt", typeof(string)));
        master.Columns.Add(new DataColumn("netamt", typeof(string)));
        master.Columns.Add(new DataColumn("hrrate", typeof(string)));
        master.Columns.Add(new DataColumn("lateamt", typeof(string)));
        master.Columns.Add(new DataColumn("lbl_undera", typeof(string)));
        master.Columns.Add(new DataColumn("FixNumberOfHours", typeof(string)));
        master.Columns.Add(new DataColumn("dailyrate", typeof(string)));
        master.Columns.Add(new DataColumn("paytype", typeof(string)));
        master.Columns.Add(new DataColumn("nightrate", typeof(string)));
        master.Columns.Add(new DataColumn("otrate", typeof(string)));
        master.Columns.Add(new DataColumn("otnrate", typeof(string)));
        master.Columns.Add(new DataColumn("tempot", typeof(string)));
        master.Columns.Add(new DataColumn("overbreak", typeof(string)));
        master.Columns.Add(new DataColumn("status", typeof(string)));
        master.Columns.Add(new DataColumn("irregularity", typeof(string)));
        master.Columns.Add(new DataColumn("total_tempot_reg", typeof(string)));
        master.Columns.Add(new DataColumn("total_tempot_night", typeof(string)));
        master.Columns.Add(new DataColumn("setupfinalout", typeof(string)));
        master.Columns.Add(new DataColumn("surge_pay", typeof(string)));
        master.Columns.Add(new DataColumn("reghourlyrate", typeof(string)));
        master.Columns.Add(new DataColumn("monthlyrate", typeof(string)));
        master.Columns.Add(new DataColumn("payrollrate", typeof(string)));
        master.Columns.Add(new DataColumn("otmeal", typeof(string)));

        //master.Columns.Add(new DataColumn("total_temp_ot", typeof(string)));

        return master;
    }

    [WebMethod]
    public static string returndtrprogress1(string ff, string tt, string pg)
    {
        string result = "";
        string txtf = ff.Trim();
        string txtt = tt.Trim();

        string condition = "where emp_status<>4 and emp_status<>5 and emp_status<>6 ";
        if (pg.Contains("KIOSK_"))
            condition += " and id=" + pg.Replace("KIOSK_", "");
        else if (pg.Contains("attlogs"))
        {
            string[] collection = pg.Split(',');
            string section = pg.Contains("section") ? " and sectionid=" + collection[1].Replace("section_", "") : "";
            string department = collection[0].Replace("attlogs_", "");
            condition += " and DepartmentId=" + department + section;
        }
        else
            condition += " and payrollgroupid=" + pg + " ";
        DataTable dtemp = dbhelper.getdata("select *,rownumber = ROW_NUMBER() Over (ORDER BY Id) from memployee " + condition + "");
        dttemp = dtemp;
        if (dtemp.Rows.Count > 0)
        {
            result = dtemp.Rows.Count.ToString();
        }

        dttest = new DataTable();
        dttest.Columns.Add(new DataColumn("emp_id", typeof(string)));
        dttest.Columns.Add(new DataColumn("date", typeof(string)));
        dttest.Columns.Add(new DataColumn("Date_Time_In", typeof(string)));
        dttest.Columns.Add(new DataColumn("Date_Time_Out1", typeof(string)));
        dttest.Columns.Add(new DataColumn("Date_Time_In2", typeof(string)));
        dttest.Columns.Add(new DataColumn("Date_Time_Out", typeof(string)));
        dttest.Columns.Add(new DataColumn("RD", typeof(string)));

        count = "0";
        return result;
    }
    [WebMethod]
    public static void resetters(string id)
    {
        count = "0";
    }

    [WebMethod]
    public static string finaldtr(string frm, string to, string pg)
    {

        string nobel = "";
        string conn = "";
        DataTable final_disp = new DataTable();
        final_disp.Columns.Add(new DataColumn("emp_id", typeof(string)));
        final_disp.Columns.Add(new DataColumn("date", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_In", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_Out1", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_In2", typeof(string)));
        final_disp.Columns.Add(new DataColumn("Date_Time_Out", typeof(string)));
        final_disp.Columns.Add(new DataColumn("RD", typeof(string)));
        DataRow final_dr;

        DataTable dis_date = new DataTable();
        dis_date.Columns.Add(new DataColumn("date", typeof(string)));
        DataRow disdatedr;
        DateTime datef = Convert.ToDateTime(frm);
        DateTime datet = Convert.ToDateTime(to);
        /**BTK O4082020**/

        string[] fromm = datef.AddDays(-1).ToString().Replace(" 12:00:00 AM", "").Split('/');
        string[] too = datet.AddDays(1).ToString().Replace(" 12:00:00 AM", "").Split('/');


        string fmmonth = fromm[0].Length > 1 ? fromm[0] : "0" + fromm[0];
        string fdday = fromm[1].Length > 1 ? fromm[1] : "0" + fromm[1];
        string finaldatefrom = fmmonth + "/" + fdday + "/" + fromm[2];


        string tmmonth = too[0].Length > 1 ? too[0] : "0" + too[0];
        string tdday = too[1].Length > 1 ? too[1] : "0" + too[1];
        string finaldateto = tmmonth + "/" + tdday + "/" + too[2];

        /**BTK 01212020
         * OPTIMIZE SCHEDULE**/
        string checkcs = "select a.employeeid,left(convert(varchar,a.Date,101),10)date,a.status, b.ShiftCodeId, replace(replace(b.TimeIn1,'.',':'),' ',':00 ')punchin,replace(replace(b.TimeOut2,'.',':'),' ',':00 ')punchout " +
        "from TChangeShiftline a " +
        "left join MShiftCodeDay b on a.ShiftCodeId=b.ShiftCodeId " +
        "where a.date between convert(datetime,'" + frm + "') and convert(datetime,'" + to + "') + 1";

        if (pg.Contains("KIOSK_"))
            checkcs += "and a.EmployeeId=" + pg.Replace("KIOSK_", "") + " ";

        checkcs += "order by a.id desc";
        DataTable dtcheckcs = dbhelper.getdata(checkcs);

        DataRow[] dtemp = dttemp.Select("rownumber='" + count + "'");
        DataTable workverificationquery = dbhelper.getdata("select * from TRestdaylogs");
        //DataTable dtmodifiedlogs = dbhelper.Execute(dtrlogpercutoff(finaldatefrom, finaldateto, dtemp.Rows[0]["divisionid"].ToString()));
        DataTable MShiftCode = dbhelper.getdata("select * from MShiftCode");
        DataTable MShiftCodeday = dbhelper.getdata("select * from MShiftCodeDay");
        DataTable alllogs = dbhelper.getdata("select empid,idnumber,biotime from tdtrperpayrolperline where CONVERT(date,biotime)>='" + finaldatefrom + "' and CONVERT(date,biotime)<='" + finaldateto + "' order by biotime asc");
        foreach (DataRow dr in dtemp)
        {
            string hold = null;
            string dddatess = datef.AddDays(-1).ToString();
            DateTime datefff = Convert.ToDateTime(dddatess);
            TimeSpan nod = DateTime.Parse(dddatess) - DateTime.Parse(to);
            string nodformat = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0}", nod.Days, nod.Hours, nod.Minutes, nod.Seconds).Replace("-", "");
            for (int i = 0; i <= int.Parse(nodformat); i++)
            {
                string[] f_datef = datefff.AddDays(i).ToString().Trim().Split(' ');
                string[] getdate = f_datef[0].Trim().Split('/');
                string month = getdate[0].Length > 1 ? getdate[0] : "0" + getdate[0];
                string day = getdate[1].Length > 1 ? getdate[1] : "0" + getdate[1];
                string ddate = month + "/" + day + "/" + getdate[2];



                DataRow[] workverificationdr = workverificationquery.Select("employeeid=" + dr["id"] + " and date='" + ddate + "' and status like'%Approved%'");
                DataRow[] workverificationnxtdr = workverificationquery.Select("employeeid=" + dr["id"] + " and date='" + Convert.ToDateTime(ddate).AddDays(1) + "' and status like'%Approved%'");

                //current shift
                DataRow[] dtgetchangeshiftdr = dtcheckcs.Select("employeeid='" + dr["id"] + "' and Date='" + ddate + "'  and status like '%Approved%'");
                string sc = dtgetchangeshiftdr.Count() > 0 ? dtgetchangeshiftdr[0]["ShiftCodeId"].ToString() : dr["ShiftCodeId"].ToString();
                string shiftday = Convert.ToDateTime(ddate).DayOfWeek.ToString();
                if (workverificationdr.Count() > 0)
                {
                    if (decimal.Parse(workverificationdr[0]["shiftcodeId"].ToString()) > 0)
                        sc = workverificationdr[0]["shiftcodeId"].ToString();
                }

                DataRow[] MShiftCodedr = MShiftCode.Select(" id = " + sc + " ");
                if (MShiftCodedr[0]["fix"].ToString() == "True")
                    conn = " and day='" + shiftday + "'";
                DataRow[] MShiftCodedaydr = MShiftCodeday.Select(" ShiftCodeId = " + sc + conn + " ");

                //next shift
                //BUTYOK
                if (ddate == "08/26/2020")
                {
                }
                string[] hihu = Convert.ToDateTime(ddate).AddDays(1).ToShortDateString().Split('/');
                string monthnxt = hihu[0].Length > 1 ? hihu[0] : "0" + hihu[0];
                string daynxt = hihu[1].Length > 1 ? hihu[1] : "0" + hihu[1];
                string ddatenxt = monthnxt + "/" + daynxt + "/" + hihu[2];

                /**BTK 04062020
                 * PREVIOUS DAY SHIFT
                 * - INCREATE HRS IF PREVIOUS SHIFT RESTDAY **/
                string previousDate = Convert.ToDateTime(ddate).AddDays(-1).ToString("MM/dd/yyyy");
                DataRow[] drgetchangeshiftprevious = dtcheckcs.Select("employeeid='" + dr["id"] + "' and Date='" + previousDate + "' and status like '%Approved%'");
                string preshift = drgetchangeshiftprevious.Count() > 0 ? drgetchangeshiftprevious[0]["ShiftCodeId"].ToString() : dr["ShiftCodeId"].ToString();
                string shiftdayprevious = Convert.ToDateTime(previousDate).DayOfWeek.ToString();
                DataRow[] workverificationprevious = workverificationquery.Select("employeeid=" + dr["id"] + " and date='" + Convert.ToDateTime(previousDate) + "' and status like'%Approved%'");

                if (workverificationprevious.Count() > 0)
                {
                    if (decimal.Parse(workverificationprevious[0]["shiftcodeId"].ToString()) > 0)
                        preshift = workverificationprevious[0]["shiftcodeId"].ToString();
                }
                DataRow[] MShiftCodedrprevious = MShiftCode.Select("id = " + preshift + " ");
                if (MShiftCodedrprevious[0]["fix"].ToString() == "True")
                    conn = " and day='" + shiftdayprevious + "'";

                DataRow[] MShiftCodedayPrevious = MShiftCodeday.Select(" ShiftCodeId = " + preshift + conn + " ");


                /**NEXT DAY SHIFT**/
                DataRow[] dtgetchangeshiftnextdr = dtcheckcs.Select("employeeid='" + dr["id"] + "' and Date='" + ddatenxt + "' and status like '%Approved%'");
                string scnext = dtgetchangeshiftnextdr.Count() > 0 ? dtgetchangeshiftnextdr[0]["ShiftCodeId"].ToString() : dr["ShiftCodeId"].ToString();
                string shiftdaynext = Convert.ToDateTime(ddate).AddDays(1).DayOfWeek.ToString();
                if (workverificationnxtdr.Count() > 0)
                {
                    if (decimal.Parse(workverificationnxtdr[0]["shiftcodeId"].ToString()) > 0)
                        scnext = workverificationnxtdr[0]["shiftcodeId"].ToString();
                }
                DataRow[] MShiftCodedrnxt = MShiftCode.Select(" id = " + scnext + " ");
                if (MShiftCodedrnxt[0]["fix"].ToString() == "True")
                    conn = " and day='" + shiftdaynext + "'";

                DataRow[] MShiftCodedaydrnxt = MShiftCodeday.Select(" ShiftCodeId = " + scnext + conn + " ");

                //set prev in and nxt in
                DateTime get1in = Convert.ToDateTime(MShiftCodedaydr.Count() > 0 ? ddate + " " + MShiftCodedaydr[0]["TimeIn1"].ToString() : "0:00:00");
                string datenxt = Convert.ToDateTime(ddate).AddDays(1).ToString();
                string[] datearr = datenxt.Split(' ');
                DateTime get1innext = Convert.ToDateTime(MShiftCodedaydrnxt.Count() > 0 ? datearr[0] + " " + MShiftCodedaydrnxt[0]["TimeIn1"].ToString() : "0:00:00");

                string NEXTIN = MShiftCodedaydrnxt[0]["Restday"].ToString() == "True" ? get1innext.AddHours(20).ToString() : get1innext.AddHours(-2).ToString();

                /**BTK 02112020
                 * 12AM**/
                if (MShiftCodedaydrnxt[0]["Restday"].ToString() == "False")
                {
                    if (MShiftCodedaydrnxt[0]["TimeIn1"].ToString() == "12:00 AM")
                    {
                        NEXTIN = datearr[0] + " 11:59:00";
                    }
                }

                /**BUTYOK 11162020
                 * Public Holiday**/
                switch (MShiftCodedaydrnxt[0][1].ToString())
                {
                    case "65":
                        /**Public Holiday**/
                        NEXTIN = get1innext.AddHours(8).ToString();
                        break;
                }


                /**ACTIVE SHIFT**/

                /**BTK 04062020
                 * PREVIOUS DAY SHIFT
                 * - INCREATE HRS IF PREVIOUS SHIFT RESTDAY **/
                int LoginTrimHr = MShiftCodedayPrevious[0]["Restday"].ToString() == "True" ? -8 : -3;

                string ININ = MShiftCodedaydr[0]["TimeIn1"].ToString().Contains("01:00 AM") || MShiftCodedaydr[0]["TimeIn1"].ToString().Contains("02:00 AM") ? get1in.AddHours(-1).ToString() : get1in.AddHours(LoginTrimHr).ToString();

                if (MShiftCodedaydr[0]["TimeIn1"].ToString() == "12:00 AM")
                {
                    ININ = Convert.ToDateTime(ddate + " " + "11:59:00").AddHours(-2).ToString();
                }

                string rdd = null;
                string drquery = "where empid='" + dr["id"] + "' and CONVERT(datetime,Biotime)>='" + ININ + "' and CONVERT(datetime,Biotime)<='" + NEXTIN + "' )t ";
                DataTable tempdt = dbhelper.getdata(tempquery(drquery, dr["divisionid"].ToString(), nobel));


                final_dr = final_disp.NewRow();
                final_dr["emp_id"] = dr["id"];
                final_dr["date"] = ddate;
                final_dr["Date_Time_In"] = tempdt.Rows.Count > 0 ? tempdt.Rows[0]["time_in"].ToString() : "0";

                /**BTK 01212020
                 * ALLOW SPLIT SCHEDULE**/
                final_dr["Date_Time_Out1"] = tempdt.Rows.Count > 0 ? tempdt.Rows[0]["break_out"].ToString() : "0";
                final_dr["Date_Time_In2"] = tempdt.Rows.Count > 0 ? tempdt.Rows[0]["break_in"].ToString() : "0";

                final_dr["Date_Time_Out"] = tempdt.Rows.Count > 0 ? tempdt.Rows[0]["time_out"].ToString() : "0";
                final_dr["RD"] = rdd;
                final_disp.Rows.Add(final_dr);
                dttest.Rows.Add(final_disp);
            }

        }

        count = (Convert.ToInt32(count) + 1).ToString();
        return count;
    }

    public static string tempquery(string qq, string divid, string nobel)
    {
        string hold = null;
        if (nobel.Contains("where CONVERT(varchar,Biotime,22) <>"))
            nobel = nobel.ToString().Replace("where CONVERT(varchar,Biotime,22) <>", "");
        if (qq.Contains("and rn!=1"))
        {
            hold = "and rn!=1";
            qq = qq.Replace("and rn!=1", "");
        }
        string query = "declare @fcnt int   " +
                        "declare @cnt int  " +
                        "declare @deviceid int   " +
                        "declare @idnumber varchar(50)   " +
                        "declare @empid int  " +
                        "declare @biotime datetime " +
                        "declare @fbiotime datetime  " +
                        "declare @chektype varchar(50)  " +
                        "declare @Expr varchar(50)  " +
                        "declare @i int " +
                        "set @i=1 " +
                        "create table #dummy (id int identity(1,1),empid int,idnumber varchar(50),biotime datetime) " +
                        "create table #dummy1 (id int identity(1,1),empid int,idnumber varchar(50),biotime datetime) " +
                        "create table #dummy2 (id int identity(1,1),rn int,empid int,idnumber varchar(50),biotime datetime) " +
                        "insert into #dummy(empid,idnumber,biotime) " +
                        "select  * from (" +
                        "select  empid,idnumber,biotime from tdtrperpayrolperline " + qq + " group by empid,idnumber,biotime  order by biotime asc   " +
                        "select @cnt= COUNT(*) from #dummy " +
                        "while(@i<=@cnt)  " +
                        "begin  " +
                        "select  @empid=empid,@idnumber=idnumber,@biotime=biotime from #dummy where id=@i " +
                        "select top 1 @fbiotime=biotime from #dummy1 order by biotime desc " +
                        "select @fcnt= COUNT(*) from #dummy1 " +
                        "if(@fcnt>0) " +
                        "begin  " +
                        "if(DATEADD(MINUTE,case when " + divid + "=1 then 0 else 5 end,@fbiotime)<@biotime) " +
                        "begin " +
                        "insert into #dummy1(empid,idnumber,biotime)values(@empid,@idnumber,@biotime) " +
                        "end " +
                        "end " +
                        "else " +
                        "begin " +
                        "insert into #dummy1(empid,idnumber,biotime)values(@empid,@idnumber,@biotime) " +
                        "end " +
                        "set @i=@i+1; " +
                        "end " +
                        "declare @iin datetime " +
                        "declare @oout datetime " +
                        "declare @bout datetime " +
                        "declare @bin datetime " +
                        "declare @cntdummy1 int " +
                        "insert into #dummy2(rn,empid,idnumber,biotime) " +
                        "select * from(select ROW_NUMBER()OVER(ORDER BY biotime ASC)rn, empid,idnumber,biotime from #dummy1 )tt where CONVERT(varchar,Biotime,22)<>'" + nobel + "' order by biotime asc  " + //" + hold + "
                        "select @cntdummy1=count(*) from #dummy2  " +

                        "if(@cntdummy1>=4) " +
                        "begin " +
                        "set @iin=(select biotime from #dummy2 where id=1) " +
                        "set @bout=(select biotime from #dummy2 where id=2) " +
                        "set @bin=(select biotime from #dummy2 where id=3) " +
                        "set @oout=(select biotime from #dummy2 where id=4) " +
                        "end " +
                        "else " +
                        "begin " +
                        "if(@cntdummy1=1) " +
                        "set @iin=(select biotime from #dummy2 where id=1) " +
                        "if(@cntdummy1>1) " +
                        "begin " +
                        "set @iin=(select biotime from #dummy2 where id=1) " +
                        "set @oout=(select biotime from #dummy2 where id=@cntdummy1) " +
                        "end " +
                        "end " +
                        "select @iin time_in,@bout break_out,@bin break_in,@oout time_out, CONVERT(varchar,@oout,22)nobel " +
                        "drop table #dummy " +
                        "drop table #dummy1 " +
                        "drop table #dummy2 ";
        return query;
    }

}