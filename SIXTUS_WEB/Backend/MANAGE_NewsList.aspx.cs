using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Web.Configuration;
using System.IO;
using System.Drawing;
using DAL;
using BLL;

public partial class Backend_MANAGE_NewsList : PageBase
{
    #region 宣告
    new protected int iPageType = 1;
    new protected string strPageID = "M02";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();
    protected ExceptionHandle clsEx = new ExceptionHandle();
    UserInfo User = new UserInfo();
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        CheckUserPermission(strPageID, iPageType);

        //取得分頁控制項的PageSize
        if (!this.Page.IsPostBack)
        {
            this.grid.PageSize = this.PageControl1.PageSize;
        }
        this.ObjectDataSource1.Selected += new ObjectDataSourceStatusEventHandler(ObjectDataSource1_Selected);
        this.PageControl1.DataReBind += new WebControls_PageControl.DataBindDelegate(PageControl1_DataReBind);

        if (!this.Page.IsPostBack)
        {
            //取得登入者和使用者權限群組
            User = (UserInfo)Session["UserInfo"];
            hideUserID.Value = User.UserID;

            //公告訊息類別
            DataTable dtType = new DataTable();
            dtType = clsDAL.GetNewsType();
            webCommon.CreateList(ddlType, dtType, "NwtName", "NwtTypeID", false);

            //設定日期
            DateTime dtNow = DateTime.Now;
            txbSDate.Text = string.Format("{0}/{1}/1", dtNow.Year.ToString(), dtNow.Month.ToString());
            txbEDate.Text = string.Format("{0}/{1}/{2}", dtNow.Year.ToString(), dtNow.Month.ToString(), DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString());

            //暫存資訊
            hideSDate.Value = txbSDate.Text;
            hideEDate.Value = txbEDate.Text;
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


    #region UI控制項事件

    protected void btnExport_Click(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (CheckDateRang(lblMeg, txbSDate, txbEDate))
        {
            //暫存資訊
            hideSDate.Value = txbSDate.Text;
            hideEDate.Value = txbEDate.Text;
            grid.DataBind();
        }
        else
            return;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        // TextBox文字設定
        txbNewsStartDate.Text = "";
        txbNewsEndDate.Text = "";
        txbNewsUrl.Text = "";
        txbNewsTitle.Text = "";
        CKEditorControl2.Text = "";

        // UI控制項屬性調整
        trNewsID.Visible = false;
        trLastUpdateUser.Visible = false;
        //trNewsCreaDate.Visible = false;
        btnOK.Visible = true;
        btnEdit.Visible = false;
        ckbIsPermanent.Checked = false;

        #region 圖片

        imgFiePicPath2.ImageUrl = "~/Backend/Images/default.jpg";

        #endregion

        //設定日期
        DateTime dtNow = DateTime.Now;
        txbNewsStartDate.Text = string.Format("{0}/{1}/1", dtNow.Year.ToString(), dtNow.Month.ToString());
        txbNewsEndDate.Text = string.Format("{0}/{1}/{2}", dtNow.Year.ToString(), dtNow.Month.ToString(), DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString());

        //建立日期
        txbNewsCreaDate.Text = string.Format("{0}/{1}/{2}", dtNow.Year.ToString(), dtNow.Month.ToString(), dtNow.Day.ToString());

        // 類別
        DataTable dtType = new DataTable();
        dtType = clsDAL.GetNewsType();
        webCommon.CreateList(ddlNewsType, dtType, "NwtName", "NwtTypeID", false);

        this.MultiView1.ActiveViewIndex = 1;
    }

    //修改儲存的動作
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string NewsID = txbNewsID.Text.ToString();
        DateTime NwsCreaDate  = Convert.ToDateTime(txbNewsCreaDate.Text);
        DateTime NewsStartSet = Convert.ToDateTime(txbNewsStartDate.Text.ToString());
        DateTime NewsEndSet = Convert.ToDateTime(txbNewsEndDate.Text.ToString());
        int NewsEnable = webCommon.GetChecked(ckbIsEnable);
        string NewsType = ddlNewsType.SelectedValue.ToString();
        string NewsTitle = txbNewsTitle.Text.ToString();
        string NewsMsg = CKEditorControl2.Text;
        string NewsUrl = txbNewsUrl.Text.ToString();
        string LastUpdateUser = hideUserID.Value.ToString();
        DateTime LastUpdateDate = DateTime.Now;
        string NwsPicPath = "";

        #region 圖片

        string newPicPathFileName = "";
        //上傳圖片
        bool PicCheck = true;
        if (uploadFiePicPath2.HasFile)
        {
            Boolean FileOK = false;

            string fileExtension = System.IO.Path.GetExtension(uploadFiePicPath2.FileName).ToLower();
            string[] allowExtensions = { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };
            for (int i = 0; i < allowExtensions.Length; i++)
            {
                if (fileExtension == allowExtensions[i])
                {
                    FileOK = true;
                }
            }

            if (FileOK)
            {
                string FileName = uploadFiePicPath2.FileName;
                string Name = Path.GetFileNameWithoutExtension(FileName);
                newPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                string FolderPath = string.Format("Uploads/{0}/", NewsID);
                string RealFolderPath = Server.MapPath(FolderPath);

                int returnCode = clsCommon.UploadJpgPic(uploadFiePicPath2, RealFolderPath);
                PicCheck = true;
            }
            else
            {
                PicCheck = false;
            }
        }

        #endregion

        if (PicCheck)
        {
            lblMeg3.Text = "";

            //圖片
            if (uploadFiePicPath2.FileName != "")
            {
                NwsPicPath = newPicPathFileName;
            }
            else
            {
                NwsPicPath = hideFiePicPath2.Value;
            }

            if (clsDAL.UpdateNews(NewsID, NwsCreaDate, NewsStartSet, NewsEndSet, NewsEnable, NewsType, NewsTitle, NewsMsg, NewsUrl, NwsPicPath, LastUpdateUser, LastUpdateDate))
            {
                this.MultiView1.ActiveViewIndex = 0;
                this.grid.DataBind();
            }
            else
            {
                lblMeg3.Text = "不允許此類型圖片上傳";
            }
        }
    }

    //新增儲存的動作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (CheckDateRang(lblMeg2, txbNewsStartDate, txbNewsEndDate))
        {
            string NewID = clsDAL.GetKeyValue("NEW");
            DateTime NwsCreaDate = Convert.ToDateTime(txbNewsCreaDate.Text);
            DateTime NewsStartSet = Convert.ToDateTime(txbNewsStartDate.Text.ToString());
            DateTime NewsEndSet = Convert.ToDateTime(txbNewsEndDate.Text.ToString());
            int NewsEnable = webCommon.GetChecked(ckbIsEnable);
            string NewsType = ddlNewsType.SelectedValue.ToString();
            string NewsTitle = txbNewsTitle.Text.ToString();
            string NewsMsg = CKEditorControl2.Text;
            string NewsUrl = txbNewsUrl.Text.ToString();
            string LastUpdateUser = hideUserID.Value.ToString();
            DateTime LastUpdateDate = DateTime.Now;
            string NwsPicPath = "";

            #region 圖片

            string newPicPathFileName = "";
            //上傳圖片
            bool PicCheck = true;
            if (uploadFiePicPath2.HasFile)
            {
                Boolean FileOK = false;

                string fileExtension = System.IO.Path.GetExtension(uploadFiePicPath2.FileName).ToLower();
                string[] allowExtensions = { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    if (fileExtension == allowExtensions[i])
                    {
                        FileOK = true;
                    }
                }

                if (FileOK)
                {
                    string FileName = uploadFiePicPath2.FileName;
                    string Name = Path.GetFileNameWithoutExtension(FileName);
                    newPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                    string FolderPath = string.Format("Uploads/{0}/", NewID);
                    string RealFolderPath = Server.MapPath(FolderPath);

                    int returnCode = clsCommon.UploadJpgPic(uploadFiePicPath2, RealFolderPath);
                    PicCheck = true;
                }
                else
                {
                    PicCheck = false;
                }
            }

            #endregion

            if (PicCheck)
            {
                lblMeg3.Text = "";

                //圖片
                NwsPicPath = newPicPathFileName;

                if (clsDAL.AddNews(NewID, NwsCreaDate, NewsStartSet, NewsEndSet, NewsEnable, NewsType, NewsTitle, NewsMsg, NewsUrl, NwsPicPath, LastUpdateUser, LastUpdateDate))
                {
                    this.MultiView1.ActiveViewIndex = 0;
                    this.grid.DataBind();
                }
            }
            else
            {
                lblMeg3.Text = "圖片錯誤，不允許此類型圖片上傳";
            }
        }
        else
            return;
    }

    //永久刊登的設定
    protected void ckbIsPermanent_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbIsPermanent.Checked == true)
        {
            hideNewsStartDate.Value = txbNewsStartDate.Text;
            hideNewsEndDate.Value = txbNewsEndDate.Text;

            txbNewsStartDate.Text = "1753/01/01";
            txbNewsEndDate.Text = "9999/12/31";
            txbNewsStartDate.Enabled = false;
            txbNewsEndDate.Enabled = false;
        }
        else
        {
            txbNewsStartDate.Text = hideNewsStartDate.Value;
            txbNewsEndDate.Text = hideNewsEndDate.Value;

            txbNewsStartDate.Enabled = true;
            txbNewsEndDate.Enabled = true;
        }
    }

    #endregion

    #region GridView動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            //瀏覽明細
            case "View":
                {
                    //ViewNewsDetail(e.CommandArgument.ToString());

                    //this.MultiView1.ActiveViewIndex = 1;
                    break;
                }
            //修改
            case "Modify":
                {
                    EditNewsDetail(e.CommandArgument.ToString());

                    this.MultiView1.ActiveViewIndex = 1;
                    break;
                }
            //刪除
            case "Cancel":
                {
                    if (clsDAL.DeleteNews(e.CommandArgument.ToString()))
                    {
                        this.grid.DataBind();
                    }

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
        //公告人
        Label newlblLastUpdateUser = (Label)e.Row.FindControl("lblLastUpdateUser");
        if (newlblLastUpdateUser != null)
        {
            string UserID = newlblLastUpdateUser.Text;
            DataTable dt = clsDAL.GetUserByID(UserID);
            if (dt.Rows.Count != 0)
            {
                newlblLastUpdateUser.Text = dt.Rows[0]["UseName"].ToString();
            }
        }

        //是否過期
        Label newlblNwsStartDate = (Label)e.Row.FindControl("lblNwsStartDate");
        Label newlblNwsEndDate = (Label)e.Row.FindControl("lblNwsEndDate");
        Label newlblIsOverdate = (Label)e.Row.FindControl("lblIsOverdate");
        if (newlblNwsStartDate != null && newlblNwsEndDate != null)
        {
            newlblIsOverdate.Text = clsCommon.ChangeOverdate(newlblNwsStartDate.Text, newlblNwsEndDate.Text);
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

    #region 子函式

    // 進入修改模式
    public void EditNewsDetail(string NewsID)
    {
        DataTable DT = new DataTable();
        DT = clsDAL.GetNewsDetailByID(NewsID);
        DataTable dtUserName = new DataTable(); // 將UserID替換成UserName
        dtUserName = clsDAL.GetUserByID(DT.Rows[0]["LastUpdateUser"].ToString());

        // 控制項屬性調整
        trNewsID.Visible = false;
        trLastUpdateUser.Visible = true;
        trNewsCreaDate.Visible = true;
        txbNewsID.Enabled = false;
        txbLastUpdateUser.Enabled = false;
        //txbNewsCreaDate.Enabled = false;
        btnOK.Visible = false;
        btnEdit.Visible = true;

        txbNewsID.Text = DT.Rows[0]["NwsIndex"].ToString();
        txbLastUpdateUser.Text = dtUserName.Rows[0]["UseName"].ToString();
        txbNewsStartDate.Text = Convert.ToDateTime(DT.Rows[0]["NwsStartDate"]).ToString("d");
        txbNewsEndDate.Text = Convert.ToDateTime(DT.Rows[0]["NwsEndDate"]).ToString("d");
        txbNewsCreaDate.Text = Convert.ToDateTime(DT.Rows[0]["NwsCreaDate"]).ToString("yyyy/MM/dd");
        txbNewsUrl.Text = DT.Rows[0]["NwsUrl"].ToString();
        txbNewsTitle.Text = DT.Rows[0]["NwsTitle"].ToString();
        CKEditorControl2.Text = DT.Rows[0]["NwsMsg"].ToString();

        //公告永久屬性調整
        if (txbNewsStartDate.Text == "1753/1/1" && txbNewsEndDate.Text == "9999/12/31")
        {
            ckbIsPermanent.Checked = true;
            txbNewsStartDate.Enabled = false;
            txbNewsEndDate.Enabled = false;
        }
        else
        {
            ckbIsPermanent.Checked = false;
            txbNewsStartDate.Enabled = true;
            txbNewsEndDate.Enabled = true;
        }

        // DropList類別
        DataTable dtType = new DataTable();
        dtType = clsDAL.GetNewsType();
        webCommon.CreateList(ddlNewsType, dtType, "NwtName", "NwtTypeID", false);
        this.ddlNewsType.Items.FindByValue(DT.Rows[0]["NwtTypeID"].ToString()).Selected = true;

        // CheckBox類別
        // 是否啟用狀態
        webCommon.SetChecked(ckbIsEnable, Convert.ToInt32(DT.Rows[0]["NwsEnable"].ToString()));

        #region 圖片

        //圖片
        if (DT.Rows[0]["NwsPicPath"].ToString() != "")
        {
            imgFiePicPath2.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                  , DT.Rows[0]["NwsIndex"].ToString()
                                                  , DT.Rows[0]["NwsPicPath"].ToString());
            imgFiePicPath2.AlternateText = DT.Rows[0]["NwsPicPath"].ToString();
        }
        else
        {
            imgFiePicPath2.ImageUrl = "~/Backend/Images/default.jpg";
        }
        hideFiePicPath2.Value = DT.Rows[0]["NwsPicPath"].ToString();

        #endregion
    }

    // 檢查日期格式區間
    protected bool CheckDateRang(Label lblStr, TextBox txbS, TextBox txbE)
    {
        //檢查日期是否填錯
        bool DTCheck = true;
        DateTime dtSet = DateTime.Now;
        try
        {
            dtSet = Convert.ToDateTime(txbS.Text);
        }
        catch (Exception ex)
        {
            DTCheck = false;
            clsEx.ExrMsg = ex.ToString();
            clsEx.NewExceptionMsg();
        }

        try
        {
            dtSet = Convert.ToDateTime(txbE.Text);
        }
        catch (Exception ex)
        {
            DTCheck = false;
            clsEx.ExrMsg = ex.ToString();
            clsEx.NewExceptionMsg();
        }

        if (DTCheck)
        {
            //檢查起迄時間
            if (Convert.ToDateTime(txbS.Text) <= Convert.ToDateTime(txbE.Text))
            {
                return true;
            }
            else
            {
                lblStr.Text = "起始時間不能大於結束時間";
                return false;
            }
        }
        else
        {
            lblStr.Text = "請輸入正確的日期";
            return false;
        }
    }

    #endregion
}