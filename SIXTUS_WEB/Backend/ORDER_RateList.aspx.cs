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

public partial class Backend_ORDER_RateList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "M06";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();
    UserInfo User = new UserInfo();

    //起始載入
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        CheckUserPermission(strPageID, iPageType);

        if (!this.Page.IsPostBack)
        {
            //取得登入者
            User = (UserInfo)Session["UserInfo"];
            hideUserID.Value = User.UserID;
        }
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.txbOerName.Text = "";
        this.txbOerCurrency.Text = "";
        this.txbOerRate.Text = "0";
        this.lblLastUpdateDate.Text = "";

        this.btnOK.Visible = true;
        this.btnEdit.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;
    }

    //返回按鈕
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    //新增存檔
    protected void btnOK_Click(object sender, EventArgs e)
    {
        //if (Page.IsValid)
        //{
        //    int returnID = clsDAL.AddRoles(txbRolID.Text, txbRolName.Text, webCommon.GetChecked(ckbIsEnable));

        //    if (returnID == 1)
        //    {
        //        //更新帳號權限
        //        UpdatePermRole(ckblPermission, txbRolID.Text);

        //        //ShowMessage("儲存成功");
        //    }
        //    else
        //    {
        //        ShowMessage("儲存失敗");
        //    }
        //}

        //this.MultiView1.ActiveViewIndex = 0;

        //this.grid.DataBind();
    }

    //修改存檔
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            bool returnBool = clsDAL.UpdateExchangeRate(hideID.Value, txbOerName.Text, txbOerCurrency.Text, txbOerRate.Text, hideUserID.Value, DateTime.Now);

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
                    //OerID
                    string OerID = e.CommandArgument.ToString();

                    hideID.Value = OerID;

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetExchangeRateByID(OerID);
                    this.txbOerName.Text = dt.Rows[0]["OerName"].ToString();
                    this.txbOerCurrency.Text = dt.Rows[0]["OerCurrency"].ToString();
                    this.txbOerRate.Text = dt.Rows[0]["OerRate"].ToString();
                    this.lblLastUpdateDate.Text = dt.Rows[0]["LastUpdateDate"].ToString();

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
                    //OerID
                    string OerID = e.CommandArgument.ToString();

                    clsDAL.DeleteExchangeRate(OerID);
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
        ////是否啟用
        //Label newLabel = (Label)e.Row.FindControl("lblIsEnable");
        //if (newLabel != null)
        //{
        //    if (int.Parse(newLabel.Text) == 1)
        //    {
        //        newLabel.Text = clsCommon.ChangeIsEnable(1);
        //    }
        //    else if (int.Parse(newLabel.Text) == 0)
        //    {
        //        newLabel.Text = clsCommon.ChangeIsEnable(0);
        //    }
        //}
    }

    #endregion

}