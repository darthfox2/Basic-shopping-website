using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;

public partial class Backend_MasterPage : System.Web.UI.MasterPage
{
    protected DALClass clsDAL = new DALClass();
    UserInfo User = new UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            this.GetUserInfo();
        }

        //[JavaScript]繁簡體轉換
        string myScript = "";
        myScript = "<script language='javascript'>TS_Switch2();</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", myScript);
    }

    //取得帳號及角色
    protected void GetUserInfo()
    {
        //載入帳號
        User = (UserInfo)Session["UserInfo"];
        string strUserID = User.UserID;
        string strRolName = "";
        string strUserName = "";
        DataTable dt = new DataTable();
        dt = clsDAL.GetUserByID(strUserID);
        if (dt.Rows.Count != 0)
        {
            strRolName = dt.Rows[0]["RolName"].ToString();
            strUserName = dt.Rows[0]["UseName"].ToString();
        }

        //lblUser.Text = string.Format("登入身分：［{0}］{1}", strRolName, strUserName);
        lblUser.Text = string.Format("登入身分：{0}［{1}］", strUserID, strUserName);
    }

}
