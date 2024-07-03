using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace BLL
{
    public class SiteSettingClass
    {
        public static string AppRoot
        {
            get { return WebConfigurationManager.AppSettings["AppRoot"].ToString(); }
        }

        public static string PageTitle
        {
            get { return WebConfigurationManager.AppSettings["PageTitle"]; }
        }

        public static int CommandTimeout
        {
            get { return int.Parse(WebConfigurationManager.AppSettings["CommandTimeout"]); }
        }

        public static int AutoLogoutTimeout
        {
            get { return int.Parse(WebConfigurationManager.AppSettings["AutoLogoutTimeout"]); }
        }

        public static int PageSize
        {
            get { return int.Parse(WebConfigurationManager.AppSettings["PageSize"]); }
        }

        public static string DisableContextMenu
        {
            get { return (WebConfigurationManager.AppSettings["DisableContextMenu"].ToUpper() == "TRUE") ? "false" : "true"; }
        }

        public static bool IsQueryStringEncode
        {
            get { return bool.Parse(WebConfigurationManager.AppSettings["IsQueryStringEncode"]); }
        }

        public static string DateStr
        {
            get { return "yyyy/MM/dd HH:mm:ss"; }
        }

        public static string DayStr
        {
            get { return "yyyy/MM/dd"; }
        }
    }
}
