using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

public partial class content_site : System.Web.UI.MasterPage, System.Web.UI.ICallbackEventHandler
{
    public string returnValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        String cbReference = Page.ClientScript.GetCallbackEventReference
                          (this, "arg", "ReceiveServerData", "context");
        String callbackScript = "function CallServer(arg, context)" +
                "{ " + cbReference + "} ;";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                "CallServer", callbackScript, true);
    }

    public void RaiseCallbackEvent(String eventArgument)
    {
        returnValue = DateTime.Now.ToString();
    }

    public string GetCallbackResult()
    {
        return returnValue;
    }
}
