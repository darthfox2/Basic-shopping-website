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

public partial class Backend_ORDER_FareList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "M05";
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
        this.txbOdfName.Text = "";
        this.txbOdfDesc.Text = "";
        this.txbOdfPrice.Text = "0";
        this.ckbIsEnable.Checked = true;

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
            bool returnBool = clsDAL.UpdateOrderFare(hideID.Value, txbOdfName.Text, txbOdfDesc.Text, txbOdfPrice.Text, txbOdfPrice.Text, webCommon.GetChecked(ckbIsEnable).ToString(), hideUserID.Value, DateTime.Now);

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
                    //OdfID
                    string OdfID = e.CommandArgument.ToString();

                    hideID.Value = OdfID;

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetOrderFareByID(OdfID);
                    this.txbOdfName.Text = dt.Rows[0]["OdfName"].ToString();
                    this.txbOdfDesc.Text = dt.Rows[0]["OdfDesc"].ToString();
                    this.txbOdfPrice.Text = dt.Rows[0]["OdfPrice"].ToString();

                    this.webCommon.SetChecked(ckbIsEnable, Convert.ToInt32(dt.Rows[0]["OdfEnable"]));

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
                    //OdfID
                    string OdfID = e.CommandArgument.ToString();

                    clsDAL.DeleteOrderFare(OdfID);
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
        //是否啟用
        Label newLabel = (Label)e.Row.FindControl("lblIsEnable");
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