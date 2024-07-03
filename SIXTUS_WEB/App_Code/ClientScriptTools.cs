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

public static class ClientScriptTools
{
    public static void AddCalenderScript(TextBox control)
    {
        if (!control.Page.ClientScript.IsClientScriptIncludeRegistered("Calender"))
        {
            control.Page.ClientScript.RegisterClientScriptInclude("Calender", SiteSettings.AppRoot + "JavaScript/Calendar.js");
        }

        control.Attributes["onclick"] = "javascript:ShowCalendar(this);";
    }

    public static void AddIntNumericTextScript(TextBox control)
    {
        if (!control.Page.ClientScript.IsClientScriptIncludeRegistered("InputCheck"))
        {
            control.Page.ClientScript.RegisterClientScriptInclude("InputCheck", SiteSettings.AppRoot + "JavaScript/InputCheck.js");
        }

        control.Attributes["onkeydown"] = "NumericText(this, event, false, true);";
        //control.Attributes["onkeyup"] = "javascript:value = value.replace(/[^\\d]/g,'');";
        //control.Attributes["onkeydown"] = "javascript:value = value.replace(/[^\\d]/g,'');";
    }

    public static void AddIntNumericTextScript(TextBox control, bool canNegative)
    {
        if (!control.Page.ClientScript.IsClientScriptIncludeRegistered("InputCheck"))
        {
            control.Page.ClientScript.RegisterClientScriptInclude("InputCheck", SiteSettings.AppRoot + "JavaScript/InputCheck.js");
        }

        control.Attributes["onkeydown"] = "NumericText(this, event, " + canNegative.ToString().ToLower() + ", true);";
    }

    public static void AddFloatNumericTextScript(TextBox control)
    {
        if (!control.Page.ClientScript.IsClientScriptIncludeRegistered("InputCheck"))
        {
            control.Page.ClientScript.RegisterClientScriptInclude("InputCheck", SiteSettings.AppRoot + "JavaScript/InputCheck.js");
        }

        control.Attributes["onkeydown"] = "NumericText(this, event, false, false);";
        //control.Attributes["onkeyup"] = "javascript:value = value.replace(/[^\\d\\.]/g,'');";
        //control.Attributes["onkeydown"] = "javascript:value = value.replace(/[^\\d\\.]/g,'');";
    }

    public static void AddLogonNameTextScript(TextBox control)
    {
        if (!control.Page.ClientScript.IsClientScriptIncludeRegistered("InputCheck"))
        {
            control.Page.ClientScript.RegisterClientScriptInclude("InputCheck", "InputCheck.js");
        }

        control.Attributes["onkeypress"] = "javascript:return regInput(this, /^[0-9|a-z]*$/, String.fromCharCode(event.keyCode))";
        control.Attributes["onpaste"] = "javascript:return regInput(this, /^[0-9|a-z]*$/, window.clipboardData.getData('Text'))";
        control.Attributes["ondrop"] = "javascript:return regInput(this, /^[0-9|a-z]*$/, event.dataTransfer.getData('Text'))";
    }

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

    public static string GetMessageAndClosePageScript(string msg)
    {
        return "<Script Language=\"JavaScript\"> \n" +
               "alert('" + msg.Replace("'", "\\'") + "'); \n" +
               "self.close(); \n" +
               "</Script> \n";
    }

    public static string GetOpenWindowSciprt()
    {
        StringBuilder script = new StringBuilder();
        script.Append("<script type=\"text/javascript\"> \n");
        script.Append("<!--\n");
        script.Append("function OpenWin(sUrl,sWidth,sHeight)\n");
        script.Append("{\n");
        script.Append("    var obj = document.aspnetForm;\n");
        script.Append("    var x = parseInt(screen.width / 2.0) - (sWidth / 2.0);\n");
        script.Append("    var y = parseInt(screen.height / 2.0) - (sHeight / 2.0);\n");
        script.Append("    if (navigator.appName.indexOf(\"Explorer\") > -1)\n");
        script.Append("    {\n");
        script.Append("        //此為window.showModalDialog作法(只有IE適用),因為IE 利用submit() 方式會閃的很厲害\n");
        script.Append("        objWinObj = window.showModalDialog(sUrl,obj,\"dialogWidth:\"+sWidth+\"px;dialogHeight:\"+sHeight+\"px;status=no;\",\"center\");\n");
        script.Append("    }\n");
        script.Append("    else\n");
        script.Append("    {\n");
        script.Append("        objWinObj = window.open(sUrl,null,'top=' + y + ',left=' + x + ',dialog=yes,modal=yes,width=' + sWidth + ',height=' + sHeight + ',scrollbars=yes,resizable=no,status=no');\n");
        script.Append("        objWinObj.focus();\n");
        script.Append("    }\n");
        script.Append("}\n");
        script.Append("//-->\n");
        script.Append("</script>\n");
        return script.ToString();
    }

    public static string GetInlineOpenNormalWindowScript(string url, int width, int height)
    {
        return "JavaScript:var x = parseInt(screen.width / 2.0) - (" + width.ToString() + " / 2.0); var y = parseInt(screen.height / 2.0) - (" + height.ToString() + " / 2.0); window.open('" + url + "', '_blank', '');";
    }

    public static string GetInlineOpenWindowScript(string url, int width, int height)
    {
        //menubar=no,toolbar=no,channelmode=no,location=no
        string ieStyle = "'dialog=yes,modal=yes,status=yes,scrollbars=yes,resizable=no," +
                         "width=" + width.ToString() + ",height=" + height.ToString() + "," +
                         "top=' + y + ',left=' + x";
        return "JavaScript:var x = parseInt(screen.width / 2.0) - (" + width.ToString() + " / 2.0); var y = parseInt(screen.height / 2.0) - (" + height.ToString() + " / 2.0); window.open('" + url + "', '_blank', " + ieStyle + ");";
    }

    public static string GetDirectOpenWindowScript(string url, int width, int height)
    {
        return "<script language='javascript'>\n" +
               "window.open('" + url + "',null,'top=0,left=0,Width=" + width.ToString() + ",Height=" + height.ToString() + ",resizable=no,scrollbars=yes,toolbar=no,menubar=no,status=no');\n" +
               "</script>\n";
    }

    public static string GetCallOpenWinScript(string url, int width, int height)
    {
        return "<script language=\"javascript\">\n" +
               "<!--\n" +
               "OpenWin('" + url + "', " + width.ToString() + ", " + height.ToString() + ");\n" +
               "-->\n" +
               "</script>\n";
    }

    public static string GetClickButtonDisableScript()
    {
        System.Text.StringBuilder sbValid = new System.Text.StringBuilder();

        sbValid.Append("if (typeof(Page_ClientValidate) == 'function') ");

        sbValid.Append("{ if (Page_ClientValidate() == false) { return false; } } ");

        sbValid.Append("this.disabled=true; ");

        return sbValid.ToString();
    }

    public static string GetReplayObjectScript(string swfObj, string dataUrl, int width, int height)
    {
        return "<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\" width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" id=\"Replay\" align=\"middle\"> " +
               "    <param name=\"allowScriptAccess\" value=\"sameDomain\" /> " +
               "    <param name=\"movie\" value=\"" + swfObj + "\" /> " +
               "    <param name=\"quality\" value=\"high\" /> " +
               "    <param name=\"bgcolor\" value=\"#0099ff\" /> " +
               "    <param name=\"flashvars\" value=\"replayurl=" + dataUrl + "\" /> " +
               "    <embed src=\"" + swfObj + "\" quality=\"high\" bgcolor=\"#0099ff\" width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" name=\"Replay\" align=\"middle\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" /> " +
               "</object>";
    }

    public static void AddClickButtonDisableScript(Button btn)
    {
        if (btn.Attributes["onClick"].ToLower().IndexOf("this.disabled=true; ") != -1)          
            return;

        string script = GetClickButtonDisableScript();

        script += btn.Page.ClientScript.GetPostBackEventReference(btn, "") + ";";

        btn.Attributes["onClick"] = script;
    }

    public static void AddReadonlyAttrib(WebControl ctrl)
    {
        ctrl.Attributes["ContentEditable"] = "false";
    }

    public static void ClearReadonlyAttrib(WebControl ctrl)
    {
        ctrl.Attributes["ContentEditable"] = "";
    }
}
