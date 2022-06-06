using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_Employee_ot : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        //user_id = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
        if (!IsPostBack)
        {
            //fload();
            gridviewrow();
        }
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
        dt.Columns.Add(new DataColumn("col_5", typeof(string)));
        dt.Columns.Add(new DataColumn("col_6", typeof(string)));




        dr = dt.NewRow();
        dr["RowNumber"] = 1;

        dr["col_1"] = string.Empty;
        dr["col_2"] = string.Empty;
        dr["col_3"] = string.Empty;
        dr["col_4"] = string.Empty;
        dr["col_5"] = string.Empty;
        dr["col_6"] = string.Empty;


        dt.Rows.Add(dr);

        ViewState["Item_List"] = dt;
        grid_item.DataSource = dt;
        grid_item.DataBind();


    }
    public void setrow()
    {
        int rowIndex = 0;
        DataTable dtCurrentTable = (DataTable)ViewState["Item_List"];
        dtCurrentTable.Clear();
        DataRow drCurrentRow = null;
        if (dtCurrentTable != null)
        {
            for (int i = 0; i < grid_item.Rows.Count; i++)
            {
                TextBox txt_date = (TextBox)grid_item.Rows[rowIndex].Cells[1].FindControl("txt_date");

                TextBox txt_time_in = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                TextBox txt_time_out = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");
                TextBox txt_hrs = (TextBox)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_hrs");
                TextBox txt_hrs_n = (TextBox)grid_item.Rows[rowIndex].Cells[5].FindControl("txt_hrs_n");
                TextBox txt_reason = (TextBox)grid_item.Rows[rowIndex].Cells[6].FindControl("txt_reason");

                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                drCurrentRow["col_1"] = txt_date.Text;
                drCurrentRow["col_2"] = txt_time_in.Text;
                drCurrentRow["col_3"] = txt_time_out.Text;
                drCurrentRow["col_4"] = txt_hrs.Text;
                drCurrentRow["col_5"] = txt_hrs_n.Text;
                drCurrentRow["col_6"] = txt_reason.Text;

                dtCurrentTable.Rows.Add(drCurrentRow);
                rowIndex++;
            }

            ViewState["Item_List"] = dtCurrentTable;
        }
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

                TextBox txt_time_in = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                TextBox txt_time_out = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");

                TextBox txt_hrs = (TextBox)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_hrs");
                TextBox txt_hrs_n = (TextBox)grid_item.Rows[rowIndex].Cells[5].FindControl("txt_hrs_n");
                TextBox txt_reason = (TextBox)grid_item.Rows[rowIndex].Cells[6].FindControl("txt_reason");


                Label lbl_date = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date");
                Label lbl_date_desp = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date_desp");

                Label lbl_time_in = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in");
                Label lbl_time_in_desp = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in_desp");

                Label lbl_time_out = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out");
                Label lbl_time_out_desp = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out_desp");

                Label lbl_hrs = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_hrs");
                Label lbl_hrs_desp = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_hrs_desp");

                Label lbl_hrs_n = (Label)grid_item.Rows[rowIndex].Cells[5].FindControl("lbl_hrs_n");
                Label lbl_hrs_n_desp = (Label)grid_item.Rows[rowIndex].Cells[5].FindControl("lbl_hrs_n_desp");

                Label lbl_reason = (Label)grid_item.Rows[rowIndex].Cells[6].FindControl("lbl_reason");
                Label lbl_reason_desp = (Label)grid_item.Rows[rowIndex].Cells[6].FindControl("lbl_reason_desp");

                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = i + 1;
                dtCurrentTable.Rows[i - 1]["col_1"] = txt_date.Text.Length == 0 ? lbl_date_desp.Text : txt_date.Text;
                dtCurrentTable.Rows[i - 1]["col_2"] = txt_time_in.Text.Length == 0 ? lbl_time_in_desp.Text : txt_time_in.Text;
                dtCurrentTable.Rows[i - 1]["col_3"] = txt_time_out.Text.Length == 0 ? lbl_time_out_desp.Text : txt_time_out.Text;
                dtCurrentTable.Rows[i - 1]["col_4"] = txt_hrs.Text.Length == 0 ? lbl_hrs_desp.Text : txt_hrs.Text;
                dtCurrentTable.Rows[i - 1]["col_5"] = txt_hrs_n.Text.Length == 0 ? lbl_hrs_n_desp.Text : txt_hrs_n.Text;
                dtCurrentTable.Rows[i - 1]["col_6"] = txt_reason.Text.Length == 0 ? lbl_reason_desp.Text : txt_reason.Text;

          
              
                if (i == dtCurrentTable.Rows.Count)
                {
                    lbl_date.Text = txt_date.Text.Length == 0 ? "empty" : "";
                    lbl_time_in.Text = txt_time_in.Text.Length == 0 ? "empty" : "";
                    lbl_time_out.Text = txt_time_out.Text.Length == 0 ? "empty" : "";
                    lbl_hrs.Text = txt_hrs.Text.Length == 0 ? "empty" : "";
                    lbl_hrs_n.Text = txt_hrs_n.Text.Length == 0 ? "empty" : "";
                    lbl_reason.Text = txt_reason.Text.Length == 0 ? "empty" : "";

                }

                if ((lbl_date.Text + " " + lbl_time_in.Text + " " + lbl_time_out.Text + " "+ lbl_hrs.Text +" "+ lbl_hrs_n.Text +" "+lbl_reason.Text).Contains("empty"))
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

                    TextBox txt_time_in = (TextBox)grid_item.Rows[rowIndex].Cells[2].FindControl("txt_time_in");
                    TextBox txt_time_out = (TextBox)grid_item.Rows[rowIndex].Cells[3].FindControl("txt_time_out");


                    TextBox txt_hrs = (TextBox)grid_item.Rows[rowIndex].Cells[4].FindControl("txt_hrs");

                    TextBox txt_hrs_n = (TextBox)grid_item.Rows[rowIndex].Cells[5].FindControl("txt_hrs_n");

                    TextBox txt_reason = (TextBox)grid_item.Rows[rowIndex].Cells[6].FindControl("txt_reason");

                    Label lbl_date = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date");
                    Label lbl_date_desp = (Label)grid_item.Rows[rowIndex].Cells[1].FindControl("lbl_date_desp");

                    Label lbl_time_in = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in");
                    Label lbl_time_in_desp = (Label)grid_item.Rows[rowIndex].Cells[2].FindControl("lbl_time_in_desp");

                    Label lbl_time_out = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out");
                    Label lbl_time_out_desp = (Label)grid_item.Rows[rowIndex].Cells[3].FindControl("lbl_time_out_desp");

                    Label lbl_hrs = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_hrs");
                    Label lbl_hrs_desp = (Label)grid_item.Rows[rowIndex].Cells[4].FindControl("lbl_hrs_desp");

                    Label lbl_hrs_n = (Label)grid_item.Rows[rowIndex].Cells[5].FindControl("lbl_hrs_n");
                    Label lbl_hrs_n_desp = (Label)grid_item.Rows[rowIndex].Cells[5].FindControl("lbl_hrs_n_desp");

                    Label lbl_reason = (Label)grid_item.Rows[rowIndex].Cells[6].FindControl("lbl_reason");
                    Label lbl_reason_desp = (Label)grid_item.Rows[rowIndex].Cells[6].FindControl("lbl_reason_desp");
                    ImageButton can = (ImageButton)grid_item.Rows[rowIndex].Cells[7].FindControl("can");

                    grid_item.Rows[i].Cells[0].Text = Convert.ToString(i + 1);

                    txt_date.Text = dt.Rows[i]["col_1"].ToString();
                    txt_time_in.Text = dt.Rows[i]["col_2"].ToString();
                    txt_time_out.Text = dt.Rows[i]["col_3"].ToString();
                    txt_hrs.Text = dt.Rows[i]["col_4"].ToString();
                    txt_hrs_n.Text = dt.Rows[i]["col_5"].ToString();
                    txt_reason.Text = dt.Rows[i]["col_6"].ToString();

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
        int rowcount = 0;
        DataTable table = ViewState["Item_List"] as DataTable;
            if (table != null)
            {
                setrow();
                foreach (DataRow row in table.Rows)
                {
                    DataTable dt = dbhelper.getdata("select * from TOverTimeLine where EmployeeId='" + Session["emp_id"].ToString() + "' and left(convert(varchar,Date,101),10)='" + row.ItemArray[1] as string + "'");
                    if (dt.Rows.Count == 0)
                    {
                        dbhelper.getdata("insert into TOverTimeLine values (NULL," + Session["emp_id"].ToString() + ",'" + row.ItemArray[1] as string + "','" + row.ItemArray[4] as string + "'," + row.ItemArray[5] as string + ",'0','" + row.ItemArray[6] as string + "','" + "For Approval-" + Session["user_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "',NULL,NULL,getdate(),'" + row.ItemArray[2] as string + "','" + row.ItemArray[3] as string + "' )");
                    }
                    else
                        Response.Write("<script>alert('already exist!')</script>");

                    rowcount++;
                }
            }
     
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='OT'", true);

    }
  }