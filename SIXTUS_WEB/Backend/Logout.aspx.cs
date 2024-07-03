using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

public partial class Backend_Logout : System.Web.UI.Page
{
    protected DALClass clsDAL = new DALClass();
    protected EventClass clsEVE = new EventClass();
    UserInfo User = new UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        //寫入登入LOG
        User = (UserInfo)Session["UserInfo"];

        //EVENT LOG
        string strDesc = string.Format("使用者［{0}］後台登出", User.UserID);
        clsEVE.EvrType = "EVT002";
        clsEVE.EvrMsg = strDesc;
        clsEVE.UseID = User.UserID;
        clsEVE.NewEventMsg();

        Session["UserInfo"] = null;
        Server.Transfer("Login.aspx"); 
    }
}
