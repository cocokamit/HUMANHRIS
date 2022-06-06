using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_printable_travel_form : System.Web.UI.Page
{
    public static string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        //id = function.Decrypt(Request.QueryString["key"].ToString(), true);

        if (!IsPostBack)
            getdata();
    }
    protected void getdata()
    {

        id = function.Decrypt(Request.QueryString["key"].ToString(), true);

        string query =  "select a.id,a.expected_budget,a.actual_budget,a.travel_description,a.notes,a.purpose, " +
                        "b.place,b.travel_mode,b.arrange_type, " +
                        "c.lastname+', '+c.firstname+' '+ c.middlename+' '+c.extensionname as Fullname, " +
                        "d.meals,d.transportation,d.accommodation,d.otherexpense,d.totalcashapproved " +
                        "from Ttravel a " +
                        "left join Ttravelline b on a.id=b.travel_id " + 
                        "left join MEmployee c on a.emp_id=c.id " +
                        "left join Ttravel_budget d on a.id=d.travel_id " +
                        "where a.id='"+id+"'";

         DataTable dt = new DataTable();
        dt = dbhelper.getdata(query);

        txt_name.Text = dt.Rows[0]["Fullname"].ToString();
        txt_date.Text = DateTime.Now.ToShortDateString();
        txt_purpose.Text = dt.Rows[0]["purpose"].ToString();
        //txt_tplace.Text = dt.Rows[0][""].ToString();

        txt_meals.Text = dt.Rows[0]["meals"].ToString();
        txt_transpo.Text = dt.Rows[0]["transportation"].ToString();
        txt_accom.Text = dt.Rows[0]["accommodation"].ToString();
        txt_other.Text = dt.Rows[0]["otherexpense"].ToString();
        txt_total.Text = dt.Rows[0]["totalcashapproved"].ToString();

        query = "select * from Ttravelline where travel_id=" + id + " ";
        DataSet ds = new DataSet();
        ds = bol.display(query);

        grid_destinations.DataSource = ds;
        grid_destinations.DataBind();

    }
}