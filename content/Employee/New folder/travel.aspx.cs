using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_travel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
            gridviewrow();
    }

    private void gridviewrow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("col_1", typeof(string)));
        dt.Columns.Add(new DataColumn("col_2", typeof(string)));
        dt.Columns.Add(new DataColumn("col_3", typeof(string)));
        dt.Columns.Add(new DataColumn("col_4", typeof(string)));




        dr = dt.NewRow();
        dr["RowNumber"] = 1;

        dr["col_1"] = string.Empty;
        dr["col_2"] = string.Empty;
        dr["col_3"] = string.Empty;
        dr["col_4"] = string.Empty;


        dt.Rows.Add(dr);

        ViewState["Item_List"] = dt;
        grid_item.DataSource = dt;
        grid_item.DataBind();


    }
    public bool setrow()
    {
        int rowIndex = 0;
        bool oi = true;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List"];
        dtCurrentTable.Clear();
        DataRow drCurrentRow = null;
        if (dtCurrentTable != null)
        {
            for (int i = 0; i < grid_item.Rows.Count; i++)
            {
                TextBox txt_date = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_date");

                DropDownList txt_time_in = (DropDownList)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                DropDownList txt_time_out = (DropDownList)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");

                Label lbl_date = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date");
                Label lbl_time_in = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in");
                Label lbl_time_out = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out");

                DropDownList txt_reason = (DropDownList)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_reason");

                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                drCurrentRow["col_1"] = txt_date.Text;
                drCurrentRow["col_2"] = txt_time_in.Text;
                drCurrentRow["col_3"] = txt_time_out.Text;
                drCurrentRow["col_4"] = txt_reason.Text;

                dtCurrentTable.Rows.Add(drCurrentRow);


                lbl_date.Text = txt_date.Text.Length == 0 ? "empty" : "";
                lbl_time_in.Text = txt_time_in.Text.Length == 0 ? "empty" : "";
                lbl_time_out.Text = txt_time_out.Text.Length == 0 ? "empty" : "";

                if ((lbl_date.Text + " " + lbl_time_in.Text + " " + lbl_time_out.Text).Contains("empty"))
                {
                    oi = false;
                    break;
                }

                rowIndex++;
            }

            ViewState["Item_List"] = dtCurrentTable;
        }
        return oi;
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        int x = addnewrow();
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

                TextBox txt_date = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_date");

                DropDownList txt_time_in = (DropDownList)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                DropDownList txt_time_out = (DropDownList)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");

                DropDownList txt_reason = (DropDownList)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_reason");


                Label lbl_date = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date");
                Label lbl_date_desp = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date_desp");

                Label lbl_time_in = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in");
                Label lbl_time_in_desp = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in_desp");

                Label lbl_time_out = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out");
                Label lbl_time_out_desp = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out_desp");

                Label lbl_reason = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_reason");
                Label lbl_reason_desp = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_reason_desp");

                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                dtCurrentTable.Rows[i - 1]["col_1"] = txt_date.Text.Length == 0 ? lbl_date_desp.Text : txt_date.Text;
                dtCurrentTable.Rows[i - 1]["col_2"] = txt_time_in.Text.Length == 0 ? lbl_time_in_desp.Text : txt_time_in.Text;
                dtCurrentTable.Rows[i - 1]["col_3"] = txt_time_out.Text.Length == 0 ? lbl_time_out_desp.Text : txt_time_out.Text;
                dtCurrentTable.Rows[i - 1]["col_4"] = txt_reason.Text.Length == 0 ? lbl_reason_desp.Text : txt_reason.Text;



                if (i == dtCurrentTable.Rows.Count)
                {
                    lbl_date.Text = txt_date.Text.Length == 0 ? "empty" : "";
                    lbl_time_in.Text = txt_time_in.Text.Length == 0 ? "empty" : "";
                    lbl_time_out.Text = txt_time_out.Text.Length == 0 ? "empty" : "";
                }

                if ((lbl_date.Text + " " + lbl_time_in.Text + " " + lbl_time_out.Text).Contains("empty"))
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
                    TextBox txt_date = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_date");

                    DropDownList txt_time_in = (DropDownList)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                    DropDownList txt_time_out = (DropDownList)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");

                    DropDownList txt_reason = (DropDownList)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_reason");


                    Label lbl_date = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date");
                    Label lbl_date_desp = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date_desp");

                    Label lbl_time_in = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in");
                    Label lbl_time_in_desp = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in_desp");

                    Label lbl_time_out = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out");
                    Label lbl_time_out_desp = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out_desp");


                    Label lbl_reason = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_reason");
                    Label lbl_reason_desp = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_reason_desp");
                    ImageButton can = (ImageButton)grid_item.Rows[rowIndex].Cells[5].FindControl("can");



                    grid_item.Rows[i].Cells[0].Text = Convert.ToString(i + 1);

                    txt_date.Text = dt.Rows[i]["col_1"].ToString();
                    txt_time_in.Text = dt.Rows[i]["col_2"].ToString();
                    txt_time_out.Text = dt.Rows[i]["col_3"].ToString();
                    txt_reason.Text = dt.Rows[i]["col_4"].ToString();

                    rowIndex++;
                }
            }
        }
    }
    protected void grid_item_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["Item_List"] != null)
        {
            setrow();
            DataTable dt = (DataTable)ViewState["Item_List"];
            DataRow drCurrentRow = null;
            int rowIndex = Convert.ToInt32(e.RowIndex);

            if (dt.Rows.Count > 1)
            {
                dt.Rows.Remove(dt.Rows[rowIndex]);
                drCurrentRow = dt.NewRow();
                ViewState["Item_List"] = dt;
                grid_item.DataSource = dt;
                grid_item.DataBind();

                for (int i = 0; i <= grid_item.Rows.Count - 1; i++)
                {
                    grid_item.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                }
                setPreviousData();
            }
            else
            {
                Response.Redirect("addClass", false);
            }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

        if (checker())
        {
            if (setrow())
            {

               // DataTable approver_id = dbhelper.getdata("select top 1 (under_id) from approver where emp_id=" + Session["emp_id"].ToString() + " ");
                DataTable approver_id = dbhelper.getdata("select top 1 (a.under_id),(select id from nobel_user where emp_id=a.under_id) under_userid from approver a where a.emp_id=" + Session["emp_id"].ToString() + " ");

                stateclass a = new stateclass();
                int rowcount = 0;
                DataTable table = ViewState["Item_List"] as DataTable;

                a.sa = Session["emp_id"].ToString();
                a.sb = txt_purpose.Text;
                a.sc = txt_sdate.Text;
                a.sd = txt_edate.Text;
                a.se = txt_expected.Text;
                a.sf = txt_actual.Text;
                a.sg = txt_description.Text.Replace("'", "");
                a.sh = txt_notes.Text.Replace("'", "");
                a.si = approver_id.Rows[0]["under_id"].ToString();

                string x = bol.travel(a);
                setrow();
                foreach (DataRow row in table.Rows)
                {
                    a.sa = x;
                    a.sb = row.ItemArray[1] as string;
                    a.sc = row.ItemArray[2] as string;
                    a.sd = row.ItemArray[3] as string;
                    bol.travel_line(a);
                }
                rowcount++;

                function.AddNotification("Travel Approval", "at", approver_id.Rows[0]["under_userid"].ToString(), x);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='KIOSK_travel'", true);
            }
        }
    }
    protected bool checker()
    {
        bool oi = true;


        if (txt_purpose.Text == "")
        {
            oi = false;
            lbl_purpose.Text = "*";
        }
        else
            lbl_purpose.Text = "";

        if (txt_sdate.Text.Length < 10)
        {
            oi = false;
            lbl_sdate.Text = "Invalid Date";
        }
        else
            lbl_sdate.Text = "";

        if (txt_edate.Text.Length < 10)
        {
            oi = false;
            lbl_edate.Text = "Invalid Date";
        }
        else
            lbl_edate.Text = "";


        if (txt_expected.Text == "")
        {
            oi = false;
            lbl_expected.Text = "*";
        }
        else
            lbl_expected.Text = "";


        if (txt_actual.Text == "")
        {
            oi = false;
            lbl_actual.Text = "*";
        }
        else
            lbl_actual.Text = "";

        //if (txt_description.Text == "")
        //{
        //    oi = false;
        //    lbl_description.Text = "*";
        //}
        //else
        //    lbl_description.Text = "";


        return oi;

    }
}