using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using DAL;
using BLL;

public partial class Backend_System_UserList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected Security clsSecurity = new Security();
    protected CommonClass clsCommon = new CommonClass();
    UserInfo User = new UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        //CheckUserPermission(strPageID, iPageType);

        if (!this.Page.IsPostBack)
        {
            //取得登入者ID
            User = (UserInfo)Session["UserInfo"];
            hideLoginUseID.Value = User.UserID;

            //Role
            DataTable dt = new DataTable();
            dt = clsDAL.GetRolesByIsEnable(1);
            webCommon.CreateList(ddlSelRole, dt, "RolName", "RolID", false);
        }
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblPW.Visible = true;
        txbPW.Visible = true;
        rfvPW.Visible = true;

        this.txbUseID.Enabled = true;
        this.txbUseID.Text = "";
        this.txbName.Text = "";
        this.txbPW.Text = "";

        //Country
        DataTable dtC = new DataTable();
        dtC = clsDAL.GetCountry();
        webCommon.CreateList(ddlCountry, dtC, "CunName", "CunName", false);

        this.txbZipCode.Text = "";
        this.txbAddr.Text = "";
        this.txbCell.Text = "";
        this.txbEmail.Text = "";

        //Role
        DataTable dt = new DataTable();
        dt = clsDAL.GetRolesByIsEnable(1);
        webCommon.CreateList(ddlRole, dt, "RolName", "RolID", false);

        //是否啟用
        this.ckbIsEnable.Checked = true;

        this.btnOK.Visible = true;
        this.btnEdit.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;

        //指標到標題欄位
        this.txbUseID.Focus();
    }

    //查詢的動作
    protected void btnSearch_Click(object sender, EventArgs e)
    {
         grid.DataBind();
    }

    //返回按鈕
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    //新增存檔
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int returnID = clsDAL.AddUser(txbUseID.Text, txbName.Text, txbPW.Text, webCommon.GetChecked(ckbIsEnable), ddlRole.SelectedValue, ddlCountry.SelectedValue, txbZipCode.Text, txbAddr.Text, txbCell.Text, "", "", txbEmail.Text, hideLoginUseID.Value, DateTime.Now);

            if (returnID == 1)
            {
                //ShowMessage("儲存成功");
            }
            else
            {
                ShowMessage("儲存失敗");
            }
        }

        this.MultiView1.ActiveViewIndex = 0;

        this.grid.DataBind();
    }

    //修改存檔
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            bool returnBool = clsDAL.UpdateUser(hideUseID.Value, txbName.Text, webCommon.GetChecked(ckbIsEnable), ddlRole.SelectedValue, ddlCountry.SelectedValue, txbZipCode.Text, txbAddr.Text, txbCell.Text, "", "", txbEmail.Text, hideLoginUseID.Value, DateTime.Now);

            if (returnBool)
            {
                //ShowMessage("儲存成功");
            }
            else
            {
                ShowMessage("儲存失敗");
            }
        }

        this.MultiView1.ActiveViewIndex = 0;

        this.grid.DataBind();
    }

    #region GridView動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Modify":
                {
                    //指標到標題欄位
                    this.txbUseID.Focus();

                    //隱藏密碼
                    lblPW.Visible = false;
                    txbPW.Visible = false;
                    rfvPW.Visible = false;

                    //UserID
                    string UID = e.CommandArgument.ToString();

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetUserByID(UID);
                    this.txbUseID.Text = dt.Rows[0]["UseID"].ToString();
                    this.txbUseID.Enabled = false;//禁止修改帳號
                    this.hideUseID.Value = dt.Rows[0]["UseID"].ToString();//暫存原始ID
                    this.txbName.Text = dt.Rows[0]["UseName"].ToString();
                    this.txbPW.Text = clsSecurity.Decrypt(dt.Rows[0]["UsePassword"].ToString());

                    //Country
                    DataTable dtC = new DataTable();
                    dtC = clsDAL.GetCountry();
                    webCommon.CreateList(ddlCountry, dtC, "CunName", "CunName", false);
                    this.ddlCountry.Items.FindByValue(dt.Rows[0]["UseCountry"].ToString()).Selected = true;

                    this.txbZipCode.Text = dt.Rows[0]["UseZipCode"].ToString();
                    this.txbAddr.Text = dt.Rows[0]["UseAddr"].ToString();
                    this.txbCell.Text = dt.Rows[0]["UseCell"].ToString();
                    this.txbEmail.Text = dt.Rows[0]["UseEmail"].ToString();

                    this.webCommon.SetChecked(ckbIsEnable, Convert.ToInt32(dt.Rows[0]["UseEnable"]));

                    //Role
                    DataTable dt2 = new DataTable();
                    dt2 = clsDAL.GetRolesByIsEnable(1);
                    webCommon.CreateList(ddlRole, dt2, "RolName", "RolID", false);
                    this.ddlRole.Items.FindByValue(dt.Rows[0]["RolID"].ToString()).Selected = true;

                    this.btnOK.Visible = false;
                    this.btnEdit.Visible = true;
                    this.MultiView1.ActiveViewIndex = 1;
                    break;
                }

            case "Select":
                {
                    break;
                }

            case "Cancel":
                {
                    //UserID
                    string UID = e.CommandArgument.ToString();

                    clsDAL.DelUser(UID);
                    this.grid.DataBind();
                    break;
                }

            default:
                {
                    return;
                }
        }

    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label newLabel = (Label)e.Row.FindControl("lblUseEnable");
        if (newLabel != null)
        {
            if (int.Parse(newLabel.Text) == 1)
            {
                newLabel.Text = clsCommon.ChangeIsEnable(1);
            }
            else if (int.Parse(newLabel.Text) == 0)
            {
                newLabel.Text = clsCommon.ChangeIsEnable(0);
            }
        }
    }

    #endregion

}