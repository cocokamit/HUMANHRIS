using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_hr_Otherdeduction : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //key.Value = function.Decrypt(Request.QueryString["user_id"].ToString(), true);
            disp();
        }
    }
    protected void disp()
    {
        DataTable dt = dbhelper.getdata("select (select Account from Maccount where id=a.AccountId) gl,* from MOtherDeduction a  where a.action is null");
        grid_view.DataSource = dt;
        grid_view.DataBind();
    }
    protected void add_deduction(object sender, EventArgs e)
    {
        DataTable dtcheck = dbhelper.getdata("select * from MOtherDeduction where OtherDeduction='" + txt_od.Text.Trim() + "' and action is null");
        if (dtcheck.Rows.Count == 0)
        {
           if(chk())
           {
                string isloan = chk_loan.Checked == true ? "True" : "False";
                string amt=txt_amount.Text.Length>0?txt_amount.Text.Replace(",", ""):"0";
                string inter = txt_interest.Text.Length > 0 ? (double.Parse(txt_interest.Text.Replace(",", "")) / 100).ToString() : "0";
                DataTable dt = dbhelper.getdata("insert into MOtherDeduction values('" + txt_od.Text.Trim() + "','" + isloan + "',4,'" + amt + "','" + inter + "','" + ddl_type.SelectedValue + "',NULL)");
                disp();
           }
        }
        else
        {
            lbl_errmsg.Text = "Other Deduction is already Exist!";
        }
    }
    protected void click_can(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update MotherDeduction set action='cancel-" + Session["emp_id"] + "-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + " ");
                Response.Redirect("Motherdeduction", false);
            }
            else
            {
            }
        }
    }
    protected bool chk()
    {
        bool err = true;
        if (ddl_type.SelectedValue == "0")
        {
            lbl_errmsg.Text = "Incorrect Input Type!";
            err = false;
        }
        else
            lbl_errmsg.Text = "";
        //if (txt_amount.Text.Length == 0)
        //{
        //    lbl_errmsg.Text = "Incorrect Input Amount!";
        //    err = false;
        //}
        //else
        //    lbl_errmsg.Text = "";
        //if (txt_interest.Text.Length == 0)
        //{
        //    lbl_errmsg.Text = "Incorrect Input Interest!";
        //    err = false;
        //}
        //else
        //    lbl_errmsg.Text = "";
        return err;
    }
    protected void allow(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hdn_deductionid.Value=row.Cells[0].Text;
            grid_list.DataBind();
            Div1.Visible = true;
            div_view_edit.Visible = true;
            div_view_edit.Style.Add("left", "450px");
            div_view_edit.Style.Add("width", "650px");
            div_view_edit.Style.Add("top", "10%");
           string query = "select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from other_deduction_saving a " +
              "left join memployee b on a.empid=b.id  " +
              "left join MCompany c on b.CompanyId=c.Id " +
              "left join MBranch d on b.BranchId=d.Id " +
              "left join MDepartment e on b.DepartmentId=e.Id " +
              "where a.action is null and a.other_deduction_id=" + hdn_deductionid.Value + " ";
            DataTable dtbind = dbhelper.getdata(query);
            grid_list.DataSource = dtbind;
            grid_list.DataBind();
        }
    }
    protected void allowemp(object sender, EventArgs e)
    {
        string query = "";
        DataTable dt = dbhelper.getdata("select * from other_deduction_saving where empid=" + lbl_bals.Value + " and other_deduction_id=" + hdn_deductionid.Value + " and action is null");
        if (dt.Rows.Count == 0)
        {
            query = "insert into other_deduction_saving (date,other_deduction_id,empid,userid,amount) values (getdate()," + hdn_deductionid.Value + "," + lbl_bals.Value.Replace(",", "") + "," + Session["user_id"] + ",'" + txt_deduct_amt.Text.Replace(",", "") + "')";
            dbhelper.getdata(query);
        }
        query="select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from other_deduction_saving a " +
                "left join memployee b on a.empid=b.id  " +
                "left join MCompany c on b.CompanyId=c.Id " +
                "left join MBranch d on b.BranchId=d.Id " +
                "left join MDepartment e on b.DepartmentId=e.Id " +
                "where a.action is null and a.other_deduction_id="+ hdn_deductionid.Value +" ";
        DataTable dtbind = dbhelper.getdata(query);
        grid_list.DataSource = dtbind;
        grid_list.DataBind();

        txt_searchemp.Text = "";

        
    }
    protected void click_cancel_det(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            if (TextBox1.Text == "Yes")
            {
                dbhelper.getdata("update other_deduction_saving set action='cancel-" + Session["user_id"].ToString() + "-" + DateTime.Now.ToShortDateString().ToString() + "' where Id=" + row.Cells[0].Text + " ");
               string query = "select a.id, b.LastName+', '+ b.FirstName+' '+b.MiddleName emp_name,c.Company,d.Branch,e.Department,case when a.amount is null then '0' else a.amount end Amount from other_deduction_saving a " +
              "left join memployee b on a.empid=b.id  " +
              "left join MCompany c on b.CompanyId=c.Id " +
              "left join MBranch d on b.BranchId=d.Id " +
              "left join MDepartment e on b.DepartmentId=e.Id " +
              "where a.action is null and a.other_deduction_id=" + hdn_deductionid.Value + " ";
                DataTable dtbind = dbhelper.getdata(query);
                grid_list.DataSource = dtbind;
                grid_list.DataBind();
            }
            else
            {
            }
        }
    }
    protected void ordb(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton LinkButton2 = (LinkButton)e.Row.FindControl("LinkButton2");
            if (e.Row.Cells[2].Text == "True" || e.Row.Cells[4].Text == "4")
            {
                LinkButton2.Enabled = false;
                LinkButton2.Visible = false;
            }
           

        }
    }

    protected void view(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            id.Value = row.Cells[0].Text;
            Div1.Visible = true;
            Div2.Visible = true;
            string query = "select * from MOtherDeduction where Id="+id.Value+"";
            DataTable dt = new DataTable();
            dt = dbhelper.getdata(query);

            txt_type.Text = dt.Rows[0]["OtherDeduction"].ToString();
            //txt_lamount.Text = dt.Rows[0]["Amount"].ToString();
            if (dt.Rows[0]["status"].ToString() == "2")
                txt_interestt.Text = dt.Rows[0]["interest"].ToString();
            else
            {
                lbl_interest.Visible = false;
                txt_interestt.Visible = false;
            }
        }
    }
    protected void dsp()
    {
        txt_interest.Enabled = false;
        txt_amount.Enabled = false;
        chk_loan.Enabled = false;
        chk_loan.Checked = false;
    }
    protected void change(object sender, EventArgs e)
    {
        dsp();
        if(ddl_type.SelectedValue == "static")
        {
            txt_interest.Enabled = false;
            txt_amount.Enabled = false;
            chk_loan.Checked = true;
        }
        if (ddl_type.SelectedValue == "2")
        {
            txt_interest.Enabled = true;
            chk_loan.Checked = true;
        }
        if (ddl_type.SelectedValue == "3")
        {
            txt_amount.Enabled = true;
            chk_loan.Enabled = false;
        }
        if (ddl_type.SelectedValue == "4")
        {
            txt_amount.Enabled = true;
            chk_loan.Enabled = false;
        }


    }
    protected void update(object sender, EventArgs e)
    {
       string query = "update MOtherDeduction set OtherDeduction='" + txt_type.Text + "',Amount='0',interest='" + txt_interestt.Text + "' where Id=" + id.Value + "  ";
        dbhelper.getdata(query);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Save Successfully'); window.location='Motherdeduction'", true);
    }

    protected void opop(object sender, EventArgs e)
    {
        Div1.Visible = true;
        Div2.Visible = true;
    }
    protected void cpop(object sender, EventArgs e)
    {
        Response.Redirect("Motherdeduction", false);
    }
    

}