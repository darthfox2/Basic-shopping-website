using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Text;
using DAL;
using BLL;

public partial class Backend_MANAGE_ProductList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "M01";
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

            //取得分頁控制項的PageSize
            if (!this.Page.IsPostBack)
            {
                this.grid.PageSize = this.PageControl1.PageSize;
            }
            this.ObjectDataSource1.Selected += new ObjectDataSourceStatusEventHandler(ObjectDataSource1_Selected);
            this.PageControl1.DataReBind += new WebControls_PageControl.DataBindDelegate(PageControl1_DataReBind);

            //語系類別
            DataTable dtType = new DataTable();
            dtType = clsDAL.GetLanguage();
            webCommon.CreateList(ddlType, dtType, "LgeName", "LgeID", false);

            //卡片類別
            DataTable dtProductType = new DataTable();
            dtProductType = clsDAL.GetProductType();
            webCommon.CreateList(ddlProductType, dtProductType, "PrtName", "PrtID", true);
        }
    }

    //綁定分頁控制項
    protected void PageControl1_DataReBind()
    {
        this.grid.PageIndex = this.PageControl1.PageIndex;
        this.grid.DataBind();
    }

    //分頁控制項資料來源
    protected void ObjectDataSource1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.ReturnValue == null)
            return;

        this.PageControl1.RowCount = (e.ReturnValue as DataTable).Rows.Count;
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string PrdID = clsDAL.GetKeyValue("PRD");
        hideID.Value = PrdID;
        txbPrdName.Text = "";
        txbPrdDesc.Text = "";
        CKEditorControl2.Text = "";
        txbPrdPrice.Text = "0";
        txbPrdSalePrice.Text = "0";
        lblPrice_Check.Text = "";
        lblMeg.Text = "";

        //類別
        DataTable dtType = new DataTable();
        dtType = clsDAL.GetProductType();
        webCommon.CreateList(ddlProductType2, dtType, "PrtName", "PrtID", false);
        if (ddlProductType.SelectedValue != "-1")
        {
            ddlProductType2.Items.FindByValue(ddlProductType.SelectedValue).Selected = true;
        }

        //圖片上傳
        gridPic.DataBind();

        //先新增一筆記錄
        string PrdName = txbPrdName.Text;
        string PrdDesc = txbPrdDesc.Text;
        string PrdPrice = txbPrdPrice.Text;
        string PrdSalePrice = txbPrdSalePrice.Text;

        //對外連結
        string PrdUrl = txbPrdUrl.Text;

        int returnBool = clsDAL.InsertProduct(PrdID, PrdName, PrdDesc, PrdPrice, PrdSalePrice, "", "1", "1", PrdUrl, "99999", hideUserID.Value, DateTime.Now, DateTime.Now, "0");

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
        if (Page.IsValid)
        {
            string PrdID = hideID.Value;
            string PrdName = txbPrdName.Text;
            string PrdDesc = txbPrdDesc.Text;
            string PrdContent = CKEditorControl2.Text;
            string PrdPrice = txbPrdPrice.Text;
            string PrdSalePrice = txbPrdSalePrice.Text;
            string PrtID = ddlProductType2.SelectedValue;

            string PrdUrl = txbPrdUrl.Text;

            string PrdEnable = webCommon.GetChecked(ckbIsEnable).ToString();

            //檢查價格是否填錯
            bool Check = true;
            double Count = 0;
            try
            {
                Count = Convert.ToDouble(txbPrdPrice.Text);
                Count = Convert.ToDouble(txbPrdSalePrice.Text);
            }
            catch (Exception ex)
            {
                Check = false;
            }

            if (Check)
            {
                lblPrice_Check.Text = "";
                lblMeg.Text = "";

                bool returnBool = clsDAL.UpdateProduct(PrdID, PrdName, PrdDesc, PrdPrice, PrdSalePrice, PrdContent, PrdEnable, "1", PrdUrl, PrtID, hideUserID.Value, DateTime.Now);

                this.MultiView1.ActiveViewIndex = 0;

                this.grid.DataBind();
            }
            else
            {
                lblMeg.Text = "請輸入正確的價格";
            }
        }
    }

    //修改存檔
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string PrdID = hideID.Value;
            string PrdName = txbPrdName.Text;
            string PrdDesc = txbPrdDesc.Text;
            string PrdContent = CKEditorControl2.Text;
            string PrdPrice = txbPrdPrice.Text;
            string PrdSalePrice = txbPrdSalePrice.Text;
            string PrtID = ddlProductType2.SelectedValue;
            string newFileName = hideFileName.Value;

            string PrdUrl = txbPrdUrl.Text;

            string PrdEnable = webCommon.GetChecked(ckbIsEnable).ToString();

            //檢查價格是否填錯
            bool Check = true;
            double Count = 0;
            try
            {
                Count = Convert.ToDouble(txbPrdPrice.Text);
                Count = Convert.ToDouble(txbPrdSalePrice.Text);
            }
            catch (Exception ex)
            {
                Check = false;
            }

            if (Check)
            {
                lblPrice_Check.Text = "";
                lblMeg.Text = "";

                bool returnBool = clsDAL.UpdateProduct(PrdID, PrdName, PrdDesc, PrdPrice, PrdSalePrice, PrdContent, PrdEnable, "1", PrdUrl, PrtID, hideUserID.Value, DateTime.Now);

                this.MultiView1.ActiveViewIndex = 0;

                this.grid.DataBind();
            }
            else
            {
                lblMeg.Text = "請輸入正確的價格";
            }

        }
    }

    #region GridView動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Modify":
                {
                    //ID
                    string PrdID = e.CommandArgument.ToString();

                    hideID.Value = PrdID;//暫存原始ID

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetProductByID(PrdID);

                    txbPrdName.Text = dt.Rows[0]["PrdName"].ToString();
                    txbPrdDesc.Text = dt.Rows[0]["PrdDesc"].ToString();
                    CKEditorControl2.Text = dt.Rows[0]["PrdContent"].ToString();
                    txbPrdPrice.Text = dt.Rows[0]["PrdPrice"].ToString();
                    txbPrdSalePrice.Text = dt.Rows[0]["PrdSalePrice"].ToString();

                    txbPrdUrl.Text = dt.Rows[0]["PrdUrl"].ToString();

                    this.webCommon.SetChecked(ckbIsEnable, Convert.ToInt32(dt.Rows[0]["PrdEnable"]));

                    lblPrice_Check.Text = "";
                    lblMeg.Text = "";

                    //類別
                    DataTable dtType = new DataTable();
                    dtType = clsDAL.GetProductType();
                    webCommon.CreateList(ddlProductType2, dtType, "PrtName", "PrtID", false);
                    this.ddlProductType2.Items.FindByValue(dt.Rows[0]["PrtID"].ToString()).Selected = true;

                    //圖片
                    DataTable dtPic = new DataTable();
                    dtPic = clsDAL.GetAlbumByPrdID(PrdID);
                    gridPic.DataSource = dtPic;
                    gridPic.DataBind();


                    this.btnOK.Visible = false;
                    this.btnEdit.Visible = true;
                    this.MultiView1.ActiveViewIndex = 1;
                    break;
                }

            case "Cancel":
                {
                    //ID
                    string PrdID = e.CommandArgument.ToString();
                    //同部刪除圖片
                    clsDAL.DeleteAlbumByPrdID(PrdID);

                    clsDAL.DeleteProduct(PrdID);
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
        //類別
        Label newlblPrtName = (Label)e.Row.FindControl("lblPrtName");
        if (newlblPrtName != null)
        {
            string PrtID = newlblPrtName.Text;

            DataTable dt = clsDAL.GetProductTypeByID(PrtID);
            if (dt.Rows.Count != 0)
            {
                newlblPrtName.Text = dt.Rows[0]["PrtName"].ToString();
            }
        }

        //說明
        Label newLabel = (Label)e.Row.FindControl("lblPrdDesc");
        if (newLabel != null)
        {
            newLabel.Text = clsCommon.GetShortText(newLabel.Text, 50); 
        }

        //縮圖
        Image imgPic = (Image)e.Row.FindControl("imgPic");
        if (imgPic != null)
        {
            string PrdID = imgPic.AlternateText;

            DataTable dtPic = new DataTable();
            dtPic = clsDAL.GetAlbumByPrdID(PrdID);

            if (dtPic.Rows.Count != 0)
            {
                imgPic.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                , dtPic.Rows[0]["PrdID"].ToString()
                                                , dtPic.Rows[0]["FILENAME"].ToString());
                imgPic.AlternateText = dtPic.Rows[0]["FILENAME"].ToString();
            }
        }

        //是否啟用
        Label newlblIsEnable = (Label)e.Row.FindControl("lblIsEnable");
        if (newlblIsEnable != null)
        {
            if (int.Parse(newlblIsEnable.Text) == 1)
            {
                newlblIsEnable.Text = clsCommon.ChangeIsEnable(1);
            }
            else if (int.Parse(newlblIsEnable.Text) == 0)
            {
                newlblIsEnable.Text = clsCommon.ChangeIsEnable(0);
            }
        }
    }

    #endregion

    #region 圖片管理

    //圖片上傳確認動作
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string PrdID = hideID.Value;

        DataTable dtPicCount = new DataTable();
        dtPicCount = clsDAL.GetAlbumByPrdID(PrdID);

        //圖片上限
        if (dtPicCount.Rows.Count < 5)
        {
            if (fulSetPath.HasFile)
            {
                //string FileName = fulSetPath.FileName;
                //string Name = Path.GetFileNameWithoutExtension(FileName);

                string FileName = clsDAL.GetKeyValue("PIC");
                string newFileName = Path.ChangeExtension(FileName, ".jpg");
                string FolderPath = string.Format("Uploads/{0}/", PrdID);
                string RealFolderPath = Server.MapPath(FolderPath);

                //上傳圖片
                int returnCode = clsCommon.UploadBigJpgPicWithNewName(fulSetPath, RealFolderPath, FileName);

                if (returnCode != 0)
                {
                    ShowMessage(clsCommon.GetErrorText(returnCode));
                }
                else
                {
                    //取得圖片排序
                    int SORT = 0;
                    if (dtPicCount.Rows.Count == 0)
                    {
                        SORT = 1;
                    }
                    else
                    {
                        SORT = Convert.ToInt32(dtPicCount.Rows[dtPicCount.Rows.Count - 1]["SORT"]) + 1;
                    }

                    //新增到資料庫
                    clsDAL.InsertAlbum(PrdID, FileName, "", newFileName, SORT, DateTime.Now, "FOX", DateTime.Now);

                    //重刷圖片管理
                    DataTable dtPic = new DataTable();
                    dtPic = clsDAL.GetAlbumByPrdID(PrdID);
                    gridPic.DataSource = dtPic;
                    gridPic.DataBind();
                }
            }
            else
            {
                ShowMessage(clsCommon.GetErrorText(3));
            }
        }
        else
        {
            ShowMessage(clsCommon.GetErrorText(4));
        }
    }

    //圖片列表的動作(刪除)
    protected void gridPic_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "PicCancel":
                {
                    string PrdID = hideID.Value;

                    string ID = e.CommandArgument.ToString();

                    //刪除實體圖檔
                    DataTable dtPicTemp = new DataTable();
                    dtPicTemp = clsDAL.GetAlbumByID(ID);
                    string FileName = "";
                    if (dtPicTemp.Rows.Count != 0)
                    {
                        FileName = dtPicTemp.Rows[0]["FILENAME"].ToString();
                    }
                    string FolderPath = string.Format("Uploads/{0}/", PrdID);
                    string RealFolderPath = Server.MapPath(FolderPath);
                    string RealFilePath = RealFolderPath + FileName;
                    if (File.Exists(RealFilePath))
                    {
                        File.Delete(RealFilePath);
                    }

                    //刪除資料庫
                    clsDAL.DeleteAlbum(ID);

                    //重刷圖片管理
                    DataTable dtPic = new DataTable();
                    dtPic = clsDAL.GetAlbumByPrdID(PrdID);
                    gridPic.DataSource = dtPic;
                    gridPic.DataBind();
                    break;
                }

            default:
                {
                    return;
                }
        }
    }

    //圖片列表資料綁定
    protected void gridPic_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //縮圖
        Image imgPic = (Image)e.Row.FindControl("imgPic");
        if (imgPic != null)
        {
            string ID = imgPic.AlternateText;

            DataTable dtPic = new DataTable();
            dtPic = clsDAL.GetAlbumByID(ID);
            if (dtPic.Rows.Count != 0)
            {
                imgPic.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                , dtPic.Rows[0]["PrdID"].ToString()
                                                , dtPic.Rows[0]["FILENAME"].ToString());
                imgPic.AlternateText = dtPic.Rows[0]["FILENAME"].ToString();
            }
        }
    }

    #endregion
   
}