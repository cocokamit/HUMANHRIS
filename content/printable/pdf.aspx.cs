using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HiQPdf;
using System.IO;



public partial class content_printable_pdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {
        string filename = "";
        HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
        htmlToPdfConverter.BrowserWidth = 200;
        htmlToPdfConverter.BrowserHeight = 0;
        htmlToPdfConverter.HtmlLoadedTimeout = 120;
        htmlToPdfConverter.Document.PageSize = PdfPageSize.Legal;
        htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
        htmlToPdfConverter.Document.Margins = new PdfMargins(0);
        htmlToPdfConverter.WaitBeforeConvert = 1;
        byte[] pdfBuffer = null;

        string url = null;
        string urltobereplace = HttpContext.Current.Request.Url.AbsolutePath;

        //DataTable dt = dbhelper.getdata("select top 1 * from web_lnk order by id desc");

        url = HttpContext.Current.Request.Url.AbsoluteUri.Replace("pdf?", ""); //dt.Rows[0]["lnk"].ToString() + "/prntapp?key=" + function.Decrypt(Request.QueryString["key"].ToString(), true) + "";
        //string filename = url.Contains("twothree") ? "2316.pdf" : "Payslip.pdf";
        if (url.Contains("BIR1601C"))
        {
            filename = "1601-C.pdf";
        }
        else
            filename = url.Contains("twothree") ? "2316.pdf" : "Payslip.pdf";
        pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

        HttpContext.Current.Response.AddHeader("Content-Type", "application/PDF");
        HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("{0}; filename=" + filename + "; size={0}", "inline", pdfBuffer.Length.ToString()));
        HttpContext.Current.Response.BinaryWrite(pdfBuffer);
        HttpContext.Current.Response.End();
    }
 
}