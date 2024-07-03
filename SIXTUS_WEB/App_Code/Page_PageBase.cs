using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

/// <summary>
/// PageBase 的摘要描述
/// </summary>
public class Page_PageBase : System.Web.UI.Page
{
    protected DALClass clsDAL = new DALClass();
    protected UserInfo clsUserInfo = new UserInfo();
    protected string strUserID = "";
    protected string strUserIP = "";

    //網頁設定
    protected string strPageID = "";    //網頁編號
    protected int iPageType = 0;		//Page 型態
    protected string strPageName = "";  //網頁名稱	

    public Page_PageBase()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    private void Page_Load(object sender, EventArgs e)
    {
        
    }


    /// <summary>
    /// 檢查使用者
    /// </summary>
    public void CheckUser(string LgeID)
    {
        if (object.Equals(Session["UserInfo"], null))
        {
            strPageName = this.ToString().Substring(4, this.ToString().Length - 9);

            Server.Transfer("./Page_Login.aspx?LgeID=" + LgeID + "&ReturnPage=" + strPageName);

            //Server.Transfer("./LoginErr.aspx?ReturnPage=" + strPageName);

        }
        else
        {

            clsUserInfo = (UserInfo)Session["UserInfo"];

            strUserIP = Request.UserHostAddress;
            strUserID = clsUserInfo.UserID;
        }


    }

    /// <summary>
    /// 檢查使用者(產品頁面使用)
    /// </summary>
    public void CheckUserByPrdID(string LgeID, string PrdID)
    {
        if (object.Equals(Session["UserInfo"], null))
        {
            strPageName = this.ToString().Substring(4, this.ToString().Length - 9);

            Server.Transfer("./Page_Login.aspx?LgeID=" + LgeID + "&ReturnPage=" + strPageName + "&PrdID=" + PrdID);

            //Server.Transfer("./LoginErr.aspx?ReturnPage=" + strPageName);

        }
        else
        {

            clsUserInfo = (UserInfo)Session["UserInfo"];

            strUserIP = Request.UserHostAddress;
            strUserID = clsUserInfo.UserID;
        }


    }

    /// <summary>
    /// 檢查網頁存取權(主要呼叫)
    /// </summary>
    /// <param name="strPageID">Page ID</param>
    /// <param name="iPageType">網頁類型</param>
    protected void CheckUserPermission(string strPageID, int iPageType)
    {

        int iUserPermission = clsDAL.GetPagePermission(strPageID, clsUserInfo.UserID);
        //Response.Write("Start"+strPageID+"============="+clsUserInfo.UserID+"================"+ iUserPermission.ToString());
        //-----沒有權限
        if (iUserPermission == 0)
        {
            Response.Redirect("./PermissionErr.aspx");

        }

        //-----權限格式化及設定網頁權限控制
        else
        {
            //CheckPageTypePermission(iPageType, iUserPermission);

        }

    }

    //JavaScript提示視窗
    public void ShowMessage(string msg)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "MSG", ClientScriptTools.GetMessageScript(msg));
    }

}
