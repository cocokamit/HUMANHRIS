using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class content_printable_alphalist : System.Web.UI.Page
{
    StringBuilder StrHtmlGenerate = new StringBuilder();
    StringBuilder StrExport = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out");
        if (!IsPostBack)
        {

           

            //Response.Redirect("ttos");
        }

    }
    protected void click_convert(object sender, EventArgs e)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=HTML.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        Response.Output.Write(Request.Form[hfGridHtml.UniqueID]);
        Response.Flush();
        Response.End();
       
    }
}