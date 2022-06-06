using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class crop_image_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        string base64 = dataURLInto.InnerText;
        byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
        using (System.IO.FileStream stream = new System.IO.FileStream(Server.MapPath("~/crop image/test.png"), System.IO.FileMode.Create))
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
   
    }
}