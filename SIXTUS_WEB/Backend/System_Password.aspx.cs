using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using DAL;

public partial class Backend_System_Password : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "S02";
    UserInfo User = new UserInfo();
    protected DALClass clsDAL = new DALClass();
    protected Security clsSecurity = new Security();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        CheckUserPermission(strPageID, iPageType);

        //載入帳號
        User = (UserInfo)Session["UserInfo"];
        lblUseID1.Text = User.UserID;
    }

    //儲存修改
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            DataTable dt = new DataTable();
            dt = clsDAL.GetUserByID(lblUseID1.Text);
            string strOrgPW = clsSecurity.Decrypt(dt.Rows[0]["UsePassword"].ToString());
            if (txbOrgPW.Text != strOrgPW)
            {
                ShowMessage("舊密碼輸入不正確");
                return;
            }

            if (txbPW.Text != txbPW2.Text)
            {
                ShowMessage("兩組新密碼請輸入相同名稱");
                return;
            }

            bool returnBool = clsDAL.UpdatePassword(lblUseID1.Text, txbPW.Text);

            if (returnBool)
            {
                //ShowMessage("儲存成功");
            }
            else
            {
                ShowMessage("儲存失敗");
            }
        }
    }

}