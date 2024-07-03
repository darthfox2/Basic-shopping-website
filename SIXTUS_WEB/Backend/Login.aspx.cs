using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DAL;
using BLL;
using System.Data;

public partial class Backend_Login : System.Web.UI.Page
{
    protected DALClass clsDAL = new DALClass();
    protected EventClass clsEVE = new EventClass();
    UserInfo User = new UserInfo();
    
    private string strReturnPage = "";

    //起始載入
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (object.Equals(Request.QueryString, null) == false)
            {
                if (object.Equals(Request.QueryString["ReturnPage"], null) == false)
                {
                    strReturnPage = Request.QueryString["ReturnPage"].ToString() + ".aspx";
                }
                else
                {
                    this.Session.Clear();
                    strReturnPage = "./MainIndex.aspx";
                }
            }
            else
            {
                this.Session.Clear();
                strReturnPage = "./MainIndex.aspx";
            }
            ViewState["ReturnPage"] = strReturnPage;
        }
        else
        {
            strReturnPage = (string)ViewState["ReturnPage"];
        }
    }

    //送出按鈕
    protected void Send_Click(object sender, EventArgs e)
    {
        lblLoginMsg.Text = "";

        //驗證角色(最高管理者,一般管理者)
        DataTable dtUser = clsDAL.GetUserByID(UserID.Text);
        if (dtUser.Rows.Count != 0)
        {
            string RolID = dtUser.Rows[0]["RolID"].ToString();

            if (RolID == "RW001" || RolID == "RW002")
            {
                //驗證帳號
                User = clsDAL.Authen(UserID.Text, Password.Text);

                if (User.IsAuthen == true)
                {
                    Session["UserInfo"] = User;
                    FormsAuthentication.RedirectFromLoginPage(User.UserID, false);

                    //EVENT LOG
                    User = (UserInfo)Session["UserInfo"];
                    string strDesc = string.Format("使用者［{0}］後台登入", User.UserID);
                    clsEVE.EvrType = "EVT001";
                    clsEVE.EvrMsg = strDesc;
                    clsEVE.UseID = User.UserID;
                    clsEVE.NewEventMsg();

                    //記住使用者的語系
                    //Session["Language"] = ddlLanguage.SelectedValue;

                    //轉換頁面
                    Response.Redirect(strReturnPage);
                }
                else
                {
                    lblLoginMsg.Text = "登入失敗";
                }

            }
            else
            {
                lblLoginMsg.Text = "您沒有權限登入系統";
            }
        }
        else
        {
            lblLoginMsg.Text = "帳號不存在";
        }
    }

    //清除按鈕
    protected void Cancel_Click(object sender, EventArgs e)
    {
        UserID.Text = "";
        Password.Text = "";
    }
   
}