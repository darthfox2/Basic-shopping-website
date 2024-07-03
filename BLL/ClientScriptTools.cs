using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BLL
{
    public static class ClientScriptTools
    {
       
        public static string GetMessageScript(string msg)
        {
            return "<Script Language=\"JavaScript\"> \n" +
                   "alert('" + msg.Replace("'", "\\'") + "'); \n" +
                   "</Script> \n";
        }

        public static string GetMessageAndBackScript(string msg)
        {
            return "<Script Language=\"JavaScript\"> \n" +
                   "alert('" + msg.Replace("'", "\\'") + "'); \n" +
                   "history.back(); \n" +
                   "</Script> \n";
        }
    }
}
