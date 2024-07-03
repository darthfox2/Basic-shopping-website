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
using System.Web.Configuration;

public partial class Backend_MANAGE_ArticleList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "A01";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();
    UserInfo User = new UserInfo();
    ////WebConfig參數
    //protected string SiteDomain = WebConfigurationManager.AppSettings["SiteDomain"].ToString();

    //起始載入
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
            //取得登入者
            User = (UserInfo)Session["UserInfo"];
            hideUserID.Value = User.UserID;

            //類別
            DataTable dtType = new DataTable();
            dtType = clsDAL.GetArticleType();
            webCommon.CreateList(ddlType, dtType, "AttName", "AttID", false);
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

    //查詢的動作
    protected void btnSearch_Click(object sender, EventArgs e)
    {
    }

    //返回的動作
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //清空
        txbAtcName2.Text = "";
        txbAtcDesc2.Text = "";
        ckbAtcEnable2.Checked = true;
        lblMeg2.Text = "";

        //是否顯示控制
        ckbAtcSetOD2.Checked = false;
        txbAtcOpenDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");
        ddlDateH2.ClearSelection();
        ddlDateH2.Items[0].Selected = true;
        ddlDateM2.ClearSelection();
        ddlDateM2.Items[0].Selected = true;

        #region 圖片

        imgAtcPicPath2.ImageUrl = "Images/default.jpg";

        #endregion

        #region FB圖片

        imgAtcFBPicPath2.ImageUrl = "Images/default.jpg";

        #endregion

        //預先存一筆資料
        string AtcID = clsDAL.GetKeyValue("ATC");
        hideID.Value = AtcID;
        string AttID = "99999";
        string AtcName = "";
        string AtcDesc = "";
        string AtcBackColor = "";
        string AtcPicPath = "";
        string AtcStatus = "0";
        string AtcEnable = "0";
        string AtcSort = "0";
        string AtcSetOD = "0";
        DateTime AtcOpenDate = DateTime.Now;
        string AtcFBPicPath = "";
        DateTime CreateDate = DateTime.Now;
        string LastUpdateUser = hideUserID.Value;
        DateTime LastUpdateDate = DateTime.Now;
        int returnBool = clsDAL.InsertArticle(AtcID, AttID, AtcName, AtcDesc, AtcBackColor, AtcPicPath, AtcStatus, AtcEnable, AtcSort, AtcSetOD, AtcOpenDate, AtcFBPicPath, CreateDate, LastUpdateUser, LastUpdateDate);

        #region 文稿內容

        //內容類型
        DataTable dtClass = new DataTable();
        dtClass = clsDAL.GetArticleClass();
        webCommon.CreateList(ddlArticleClass2, dtClass, "AtsName", "AtsID", false);

        //清空列表
        gridContent2.DataBind();

        #endregion

        this.btnOK2.Visible = true;
        this.btnEdit2.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;
    }

    //新增儲存動作
    protected void btnOK2_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtcID = hideID.Value;
            string AttID = ddlType.SelectedValue;
            string AtcName = txbAtcName2.Text;
            string AtcDesc = txbAtcDesc2.Text;
            string AtcBackColor = "";
            string AtcPicPath = "";
            string AtcStatus = "0";
            string AtcEnable = webCommon.GetChecked(ckbAtcEnable2).ToString();
            string AtcSort = "0";
            string AtcSetOD = webCommon.GetChecked(ckbAtcSetOD2).ToString();
            DateTime AtcOpenDate = DateTime.Now;
            string AtcFBPicPath = "";
            DateTime CreateDate = DateTime.Now;
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            //檢查日期是否填錯
            bool DTCheck = true;
            try
            {
                AtcOpenDate = Convert.ToDateTime(string.Format("{0} {1}:{2}:00", txbAtcOpenDate2.Text, ddlDateH2.SelectedValue, ddlDateM2.SelectedValue));
            }
            catch (Exception ex)
            {
                DTCheck = false;
            }

            if (DTCheck)
            {
                //if (PicCheck)
                //{
                    //if (FBPicCheck)
                    //{
                        lblMeg2.Text = "";
                        //圖片
                        AtcPicPath = hideAtcPicPath2.Value;
                        //FaceBook分享圖片
                        AtcFBPicPath = hideAtcFBPicPath2.Value;

                        bool Result = clsDAL.UpdateArticle(AtcID, AttID, AtcName, AtcDesc, AtcBackColor, AtcPicPath, AtcStatus, AtcEnable, AtcSort, AtcSetOD, AtcOpenDate, AtcFBPicPath, LastUpdateUser, LastUpdateDate);

                        if (Result == true)
                        {
                            //ShowMessage("儲存成功");
                            grid.DataBind();
                            this.MultiView1.ActiveViewIndex = 0;
                        }
                        else
                        {
                            ShowMessage("儲存失敗");
                        }
                    //}
                    //else
                    //{
                    //    lblMeg2.Text = "FaceBook分享圖片錯誤，不允許此類型圖片上傳";
                    //}
                //}
                //else
                //{
                //    lblMeg2.Text = "圖片錯誤，不允許此類型圖片上傳";
                //}
            }
            else
            {
                lblMeg2.Text = "日期格式不正確";
            }
        }
    }

    //確定修改動作
    protected void btnEdit2_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtcID = hideID.Value;
            string AttID = ddlType.SelectedValue;
            string AtcName = txbAtcName2.Text;
            string AtcDesc = txbAtcDesc2.Text;
            string AtcBackColor = "";
            string AtcPicPath = "";
            string AtcStatus = "0";
            string AtcEnable = webCommon.GetChecked(ckbAtcEnable2).ToString();
            string AtcSort = "0";
            string AtcSetOD = webCommon.GetChecked(ckbAtcSetOD2).ToString();
            DateTime AtcOpenDate = DateTime.Now;
            string AtcFBPicPath = "";
            DateTime CreateDate = DateTime.Now;
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            //檢查日期是否填錯
            bool DTCheck = true;
            try
            {
                AtcOpenDate = Convert.ToDateTime(string.Format("{0} {1}:{2}:00", txbAtcOpenDate2.Text, ddlDateH2.SelectedValue, ddlDateM2.SelectedValue));
            }
            catch (Exception ex)
            {
                DTCheck = false;
            }

            if (DTCheck)
            {
                //if (PicCheck)
                //{
                //    if (FBPicCheck)
                //    {
                        lblMeg2.Text = "";

                        //圖片
                        //if (uploadAtcPicPath2.FileName != "")
                        //{
                        //    AtcPicPath = newPicPathFileName;
                        //}
                        //else
                        //{
                            AtcPicPath = hideAtcPicPath2.Value;
                        //}

                        //FB圖片
                        //if (uploadAtcFBPicPath2.FileName != "")
                        //{
                        //    AtcFBPicPath = newFBPicPathFileName;
                        //}
                        //else
                        //{
                            AtcFBPicPath = hideAtcFBPicPath2.Value;
                        //}

                        bool Result = clsDAL.UpdateArticle(AtcID, AttID, AtcName, AtcDesc, AtcBackColor, AtcPicPath, AtcStatus, AtcEnable, AtcSort, AtcSetOD, AtcOpenDate, AtcFBPicPath, LastUpdateUser, LastUpdateDate);

                        if (Result)
                        {
                            //ShowMessage("儲存成功");
                            grid.DataBind();
                            this.MultiView1.ActiveViewIndex = 0;
                        }
                        else
                        {
                            ShowMessage("儲存失敗");
                        }
                 
                    //}
                    //else
                    //{
                    //    lblMeg2.Text = "FaceBook分享圖片錯誤，不允許此類型圖片上傳";
                    //}
                //}
                //else
                //{
                //    lblMeg2.Text = "不允許此類型圖片上傳";
                //}
            }
            else
            {
                lblMeg2.Text = "日期格式不正確";
            }
        }
    }

    #region 圖片處理

    //圖片上傳
    protected void btnUploadPic2_Click(object sender, EventArgs e)
    {
        string AtcID = hideID.Value;

        string newPicPathFileName = "";
        //上傳圖片
        bool PicCheck = true;
        if (uploadAtcPicPath2.HasFile)
        {
            Boolean FileOK = false;

            string fileExtension = System.IO.Path.GetExtension(uploadAtcPicPath2.FileName).ToLower();
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
                //string FileName = uploadAtcPicPath2.FileName;
                //string Name = Path.GetFileNameWithoutExtension(FileName);

                string FileName = clsDAL.GetKeyValue("PIC");
                newPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                string FolderPath = string.Format("Uploads/{0}/", AtcID);
                string RealFolderPath = Server.MapPath(FolderPath);

                int returnCode = clsCommon.UploadBigJpgPicWithNewName(uploadAtcPicPath2, RealFolderPath, FileName);

                //檔案名稱暫存起來
                hideAtcPicPath2.Value = newPicPathFileName;

                #region 圖片顯示

                //圖片
                if (newPicPathFileName != "")
                {
                    imgAtcPicPath2.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                          , AtcID
                                                          , newPicPathFileName);
                    imgAtcPicPath2.AlternateText = newPicPathFileName;
                }
                else
                {
                    imgAtcPicPath2.ImageUrl = "Images/default.jpg";
                }

                #endregion


                PicCheck = true;
            }
            else
            {
                PicCheck = false;
            }
        }
    }

    //FB圖片上傳
    protected void btnUploadFBPic2_Click(object sender, EventArgs e)
    {
        string AtcID = hideID.Value;

        string newFBPicPathFileName = "";
        //上傳圖片
        bool FBPicCheck = true;
        if (uploadAtcFBPicPath2.HasFile)
        {
            Boolean FileOK = false;

            string fileExtension = System.IO.Path.GetExtension(uploadAtcFBPicPath2.FileName).ToLower();
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
                //string FileName = uploadAtcFBPicPath2.FileName;
                //string Name = Path.GetFileNameWithoutExtension(FileName);

                string FileName = clsDAL.GetKeyValue("PIC");
                newFBPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                string FolderPath = string.Format("Uploads/{0}/", AtcID);
                string RealFolderPath = Server.MapPath(FolderPath);

                int returnCode = clsCommon.UploadBigJpgPicWithNewName(uploadAtcFBPicPath2, RealFolderPath, FileName);

                //檔案名稱暫存起來
                hideAtcFBPicPath2.Value = newFBPicPathFileName;

                #region 圖片顯示

                //圖片
                if (newFBPicPathFileName != "")
                {
                    imgAtcFBPicPath2.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                          , AtcID
                                                          , newFBPicPathFileName);
                    imgAtcFBPicPath2.AlternateText = newFBPicPathFileName;
                }
                else
                {
                    imgAtcFBPicPath2.ImageUrl = "Images/default.jpg";
                }

                #endregion

                FBPicCheck = true;
            }
            else
            {
                FBPicCheck = false;
            }
        }
    }

    #endregion

    #region GridView動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {

            case "View":
                {
                    ////ID
                    //string AtcID = e.CommandArgument.ToString();

                    //string Url = string.Format("http://{0}/PreArticle/Page_PreArticle.aspx?AtcID={1}", SiteDomain, AtcID);

                    //Response.Write(string.Format("<script>window.open('{0}');</script>", Url));

                    break;
                }

            case "Modify":
                {
                    //ID
                    string AtcID = e.CommandArgument.ToString();
                    //暫存
                    hideID.Value = AtcID;

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetArticleByID(AtcID, "-1");
                    if (dt.Rows.Count != 0)
                    {
                        txbAtcName2.Text = dt.Rows[0]["AtcName"].ToString();
                        txbAtcDesc2.Text = dt.Rows[0]["AtcDesc"].ToString();
                        //上架日期
                        this.webCommon.SetChecked(ckbAtcSetOD2, Convert.ToInt32(dt.Rows[0]["AtcSetOD"]));
                        txbAtcOpenDate2.Text = Convert.ToDateTime(dt.Rows[0]["AtcOpenDate"]).ToString("yyyy/MM/dd");
                        ddlDateH2.ClearSelection();
                        ddlDateH2.Items.FindByValue(Convert.ToDateTime(dt.Rows[0]["AtcOpenDate"]).ToString("HH")).Selected = true;
                        ddlDateM2.ClearSelection();
                        ddlDateM2.Items.FindByValue(Convert.ToDateTime(dt.Rows[0]["AtcOpenDate"]).ToString("mm")).Selected = true;
                        //是否啟用
                        this.webCommon.SetChecked(ckbAtcEnable2, Convert.ToInt32(dt.Rows[0]["AtcEnable"]));

                        #region 圖片

                        //圖片
                        if (dt.Rows[0]["AtcPicPath"].ToString() != "")
                        {
                            imgAtcPicPath2.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                                  , dt.Rows[0]["AtcID"].ToString()
                                                                  , dt.Rows[0]["AtcPicPath"].ToString());
                            imgAtcPicPath2.AlternateText = dt.Rows[0]["AtcPicPath"].ToString();
                        }
                        else
                        {
                            imgAtcPicPath2.ImageUrl = "Images/default.jpg";
                        }
                        hideAtcPicPath2.Value = dt.Rows[0]["AtcPicPath"].ToString();

                        #endregion

                        #region FB圖片

                        //FB圖片
                        if (dt.Rows[0]["AtcFBPicPath"].ToString() != "")
                        {
                            imgAtcFBPicPath2.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                                  , dt.Rows[0]["AtcID"].ToString()
                                                                  , dt.Rows[0]["AtcFBPicPath"].ToString());
                            imgAtcFBPicPath2.AlternateText = dt.Rows[0]["AtcFBPicPath"].ToString();
                        }
                        else
                        {
                            imgAtcFBPicPath2.ImageUrl = "Images/default.jpg";
                        }
                        hideAtcFBPicPath2.Value = dt.Rows[0]["AtcFBPicPath"].ToString();

                        #endregion

                        #region 文稿內容

                        //內容類型
                        DataTable dtClass = new DataTable();
                        dtClass = clsDAL.GetArticleClass();
                        webCommon.CreateList(ddlArticleClass2, dtClass, "AtsName", "AtsID", false);

                        //更新列表
                        gridContent2.DataBind();

                        #endregion
                    }

                    lblMeg2.Text = "";

                    this.btnOK2.Visible = false;
                    this.btnEdit2.Visible = true;
                    this.MultiView1.ActiveViewIndex = 1;

                    break;
                }

            case "Cancel":
                {
                    //ID
                    string AtcID = e.CommandArgument.ToString();

                    clsDAL.DeleteArticle(AtcID);
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
        //類型
        Label newlblAttName = (Label)e.Row.FindControl("lblAttName");
        if (newlblAttName != null)
        {
            string AttID = newlblAttName.Text;
            DataTable dt = clsDAL.GetArticleTypeByID(AttID);
            if (dt.Rows.Count != 0)
            {
                newlblAttName.Text = dt.Rows[0]["AttName"].ToString();
            }
        }

        //網址
        //Label newlblLink = (Label)e.Row.FindControl("lblLink");
        //if (newlblLink != null)
        //{
        //    //AttID
        //    string AttID = newlblLink.Text.ToString().Split(',')[0].Trim();
        //    //AtcID
        //    string AtcID = newlblLink.Text.ToString().Split(',')[1].Trim();

        //    newlblLink.Text = string.Format("http://{0}/Article/Page_Article.aspx?AtcID={1}", SiteDomain, AtcID);
       
        //}

        //預計上架日期
        Label newlblOpenDate = (Label)e.Row.FindControl("lblOpenDate");
        if (newlblOpenDate != null)
        {
            //AtcID
            string AtcID = newlblOpenDate.Text;
            DataTable dt = clsDAL.GetArticleByID(AtcID, "-1");
            if (dt.Rows.Count != 0)
            {
                string AtcSetOD = dt.Rows[0]["AtcSetOD"].ToString();
                if (AtcSetOD == "1")
                {
                    newlblOpenDate.Text = Convert.ToDateTime(dt.Rows[0]["AtcOpenDate"]).ToString("yyyy/MM/dd HH:mm");
                }
                else
                {
                    newlblOpenDate.Text = "不限";
                }
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

        //縮圖
        Image imgPic = (Image)e.Row.FindControl("imgPic");
        if (imgPic != null)
        {
            string Temp = imgPic.AlternateText;

            //AtcID
            string AtcID = Temp.Split(',')[0].Trim();
            //AtcPicPath
            string AtcPicPath = Temp.Split(',')[1].Trim();

            imgPic.ImageUrl = string.Format("Uploads/{0}/{1}"
                                            , AtcID
                                            , AtcPicPath);
            imgPic.AlternateText = AtcPicPath;
        }
    }

    #endregion

    #region 內容管理

    //內容管理新增按鈕的動作
    protected void btnAdd2_Click(object sender, EventArgs e)
    {
        string AtsID = ddlArticleClass2.SelectedValue;

        switch (AtsID)
        {
            //副標區塊
            case "C01":
                //清空
                txbAtnSubject3.Text = "";
                CKEditorControl3.Text = "";

                this.btnOK3.Visible = true;
                this.btnEdit3.Visible = false;

                this.MultiView1.ActiveViewIndex = 2;
                break;
            //文字區塊
            case "C02":
                //清空
                txbAtnSubject4.Text = "";
                CKEditorControl4.Text = "";

                this.btnOK4.Visible = true;
                this.btnEdit4.Visible = false;

                this.MultiView1.ActiveViewIndex = 3;
                break;
            //圖片區塊
            case "C11":
                //清空
                txbAtnSubject5.Text = "";
                imgAtnPicPath5.ImageUrl = "Images/default.jpg";

                this.btnOK5.Visible = true;
                this.btnEdit5.Visible = false;

                this.MultiView1.ActiveViewIndex = 4;

                break;
            //串流影片區塊(YouTube)
            case "C31":
                //清空
                txbAtnSubject6.Text = "";
                txbAtnVideoPath6.Text = "";

                this.btnOK6.Visible = true;
                this.btnEdit6.Visible = false;

                this.MultiView1.ActiveViewIndex = 5;

                break;
            //串流影片區塊(Vimeo)
            case "C32":
                //清空
                txbAtnSubject7.Text = "";
                txbAtnVideoPath7.Text = "";

                this.btnOK7.Visible = true;
                this.btnEdit7.Visible = false;

                this.MultiView1.ActiveViewIndex = 6;

                break;
            //串流影片區塊(優酷)
            case "C33":
                //清空
                txbAtnSubject8.Text = "";
                txbAtnVideoPath8.Text = "";

                this.btnOK8.Visible = true;
                this.btnEdit8.Visible = false;

                this.MultiView1.ActiveViewIndex = 7;

                break;
            //串流影片區塊(土豆網)
            case "C34":
                //清空
                txbAtnSubject9.Text = "";
                txbAtnVideoPath9.Text = "";

                this.btnOK9.Visible = true;
                this.btnEdit9.Visible = false;

                this.MultiView1.ActiveViewIndex = 8;

                break;
            //MP4影片區塊
            case "C41":
                //清空
                txbAtnSubject10.Text = "";
                txbAtnVideoPath10.Text = "";

                this.btnOK10.Visible = true;
                this.btnEdit10.Visible = false;

                this.MultiView1.ActiveViewIndex = 9;

                break;

        }
    }

    //返回的動作
    protected void btnBack3_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 1;
    }

    #region 副標區塊

    //新增儲存副標區塊
    protected void btnOK3_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject3.Text;
            string AtnText = CKEditorControl3.Text;
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存副標區塊
    protected void btnEdit3_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID3.Value;
            string AtsID = hideAtsID3.Value;
            string AtnSubject = txbAtnSubject3.Text;
            string AtnText = CKEditorControl3.Text;
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort3.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 文字區塊

    //新增儲存文字區塊
    protected void btnOK4_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject4.Text;
            string AtnText = CKEditorControl4.Text;
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存文字區塊
    protected void btnEdit4_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID4.Value;
            string AtsID = hideAtsID4.Value;
            string AtnSubject = txbAtnSubject4.Text;
            string AtnText = CKEditorControl4.Text;
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort4.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 圖片區塊

    //新增儲存圖片區塊
    protected void btnOK5_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject5.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 圖片

            string newPicPathFileName = "";
            //上傳圖片
            bool PicCheck = true;
            if (uploadAtnPicPath5.HasFile)
            {
                Boolean FileOK = false;

                string fileExtension = System.IO.Path.GetExtension(uploadAtnPicPath5.FileName).ToLower();
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
                    //string FileName = uploadAtnPicPath5.FileName;
                    //string Name = Path.GetFileNameWithoutExtension(FileName);

                    string FileName = clsDAL.GetKeyValue("PIC");
                    newPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                    string FolderPath = string.Format("Uploads/{0}/", AtnID);
                    string RealFolderPath = Server.MapPath(FolderPath);

                    int returnCode = clsCommon.UploadBigJpgPicWithNewName(uploadAtnPicPath5, RealFolderPath, FileName);
                    PicCheck = true;
                }
                else
                {
                    PicCheck = false;
                }
            }

            #endregion

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            //圖片
            AtnPicPath = newPicPathFileName;

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存圖片區塊
    protected void btnEdit5_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID5.Value;
            string AtsID = hideAtsID5.Value;
            string AtnSubject = txbAtnSubject5.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = "";
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort5.Value;
            string AtcID = hideID.Value;

            #region 圖片

            string newPicPathFileName = "";
            //上傳圖片
            bool PicCheck = true;
            if (uploadAtnPicPath5.HasFile)
            {
                Boolean FileOK = false;

                string fileExtension = System.IO.Path.GetExtension(uploadAtnPicPath5.FileName).ToLower();
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
                    //string FileName = uploadAtnPicPath5.FileName;
                    //string Name = Path.GetFileNameWithoutExtension(FileName);

                    string FileName = clsDAL.GetKeyValue("PIC");
                    newPicPathFileName = Path.ChangeExtension(FileName, ".jpg");
                    string FolderPath = string.Format("Uploads/{0}/", AtnID);
                    string RealFolderPath = Server.MapPath(FolderPath);

                    int returnCode = clsCommon.UploadBigJpgPicWithNewName(uploadAtnPicPath5, RealFolderPath, FileName);
                    PicCheck = true;
                }
                else
                {
                    PicCheck = false;
                }
            }

            #endregion

            //圖片
            if (uploadAtnPicPath5.FileName != "")
            {
                AtnPicPath = newPicPathFileName;
            }
            else
            {
                AtnPicPath = hideAtnPicPath5.Value;
            }

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 串流影片區塊(YouTube)

    //新增儲存串流影片區塊(YouTube)
    protected void btnOK6_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject6.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath6.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存串流影片區塊(YouTube)
    protected void btnEdit6_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID6.Value;
            string AtsID = hideAtsID6.Value;
            string AtnSubject = txbAtnSubject6.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath6.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort6.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 串流影片區塊(Vimeo)

    //新增儲存串流影片區塊(Vimeo)
    protected void btnOK7_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject7.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath7.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存串流影片區塊(Vimeo)
    protected void btnEdit7_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID7.Value;
            string AtsID = hideAtsID7.Value;
            string AtnSubject = txbAtnSubject7.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath7.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort7.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 串流影片區塊(優酷)

    //新增儲存串流影片區塊(優酷)
    protected void btnOK8_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject8.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath8.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存串流影片區塊(優酷)
    protected void btnEdit8_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID8.Value;
            string AtsID = hideAtsID8.Value;
            string AtnSubject = txbAtnSubject8.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath8.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort8.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 串流影片區塊(土豆網)

    //新增儲存串流影片區塊(土豆網)
    protected void btnOK9_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject9.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath9.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改儲存串流影片區塊(土豆網)
    protected void btnEdit9_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID9.Value;
            string AtsID = hideAtsID9.Value;
            string AtnSubject = txbAtnSubject9.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath9.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort9.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region 串流MP4影片區塊

    //新增串流MP4影片區塊
    protected void btnOK10_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = clsDAL.GetKeyValue("ATN");
            string AtsID = ddlArticleClass2.SelectedValue;
            string AtnSubject = txbAtnSubject10.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath10.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = "0";
            string AtcID = hideID.Value;

            #region 取得排序碼

            DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
            if (dtAC.Rows.Count != 0)
            {
                int SortTemp = Convert.ToInt32(dtAC.Rows[dtAC.Rows.Count - 1]["AtnSort"]) + 1;

                AtnSort = SortTemp.ToString();
            }
            else
            {
                AtnSort = "1";
            }

            #endregion

            int returnBool = clsDAL.InsertArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    //修改串流MP4影片區塊
    protected void btnEdit10_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string AtnID = hideAtnID10.Value;
            string AtsID = hideAtsID10.Value;
            string AtnSubject = txbAtnSubject10.Text;
            string AtnText = "";
            string AtnPicPath = "";
            string AtnVideoPath = txbAtnVideoPath10.Text;
            string AtnMp4Path = "";
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;
            string AtnSort = hideAtnSort10.Value;
            string AtcID = hideID.Value;

            bool returnBool = clsDAL.UpdateArticleContent(AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID);

            //更新列表
            gridContent2.DataBind();

            this.MultiView1.ActiveViewIndex = 1;
        }
    }

    #endregion

    #region gridProduct動作

    protected void gridContent2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "View":
                {
                  
                    break;
                }

            case "SortUp":
                {
                    //AtcID
                    string AtcID = e.CommandArgument.ToString().Split(',')[0].Trim();
                    //AtnID
                    string AtnID = e.CommandArgument.ToString().Split(',')[1].Trim();
                    //AtnSort
                    string AtnSort = e.CommandArgument.ToString().Split(',')[2].Trim();

                    if (AtnSort != "1")
                    {
                        DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
                        if (dtAC.Rows.Count != 0)
                        {
                            string nowAtnID = AtnID;
                            string nowAtnSort = AtnSort;

                            #region 找出上一個ROWS

                            string upAtnID = "";
                            string upAtnSort = "";
                            string tempAtnSort = (Convert.ToInt32(AtnSort) - 1).ToString();

                            string filter = "AtnSort = '" + tempAtnSort + "'";
                            DataRow[] foundRows;
                            //篩選存在的CardCode
                            foundRows = dtAC.Select(filter);
                            if (foundRows.Length != 0)
                            {
                                object[] objRow1 = foundRows[0].ItemArray;
                                upAtnID = objRow1[0].ToString();
                                upAtnSort = objRow1[9].ToString();
                            }

                            #endregion

                            //更新排序狀態(互換)
                            clsDAL.UpdateArticleContentWithSort(nowAtnID, (Convert.ToInt32(nowAtnSort) - 1).ToString(), hideUserID.Value, DateTime.Now);
                            clsDAL.UpdateArticleContentWithSort(upAtnID, (Convert.ToInt32(upAtnSort) + 1).ToString(), hideUserID.Value, DateTime.Now);

                            gridContent2.DataBind();
                        }
                    }
                    else
                    {
                        ShowMessage("已經是第一筆紀錄");
                    }

                    break;
                }

            case "SortDown":
                {
                    //AtcID
                    string AtcID = e.CommandArgument.ToString().Split(',')[0].Trim();
                    //AtnID
                    string AtnID = e.CommandArgument.ToString().Split(',')[1].Trim();
                    //AtnSort
                    string AtnSort = e.CommandArgument.ToString().Split(',')[2].Trim();

                    DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
                    if (dtAC.Rows.Count != 0)
                    {
                        if (AtnSort != dtAC.Rows.Count.ToString())
                        {
                            string nowAtnID = AtnID;
                            string nowAtnSort = AtnSort;

                            #region 找出下一個ROWS

                            string downAtnID = "";
                            string downAtnSort = "";
                            string tempAtnSort = (Convert.ToInt32(AtnSort) + 1).ToString();

                            string filter = "AtnSort = '" + tempAtnSort + "'";
                            DataRow[] foundRows;
                            //篩選存在的CardCode
                            foundRows = dtAC.Select(filter);
                            if (foundRows.Length != 0)
                            {
                                object[] objRow1 = foundRows[0].ItemArray;
                                downAtnID = objRow1[0].ToString();
                                downAtnSort = objRow1[9].ToString();
                            }

                            #endregion

                            //更新排序狀態(互換)
                            clsDAL.UpdateArticleContentWithSort(nowAtnID, (Convert.ToInt32(nowAtnSort) + 1).ToString(), hideUserID.Value, DateTime.Now);
                            clsDAL.UpdateArticleContentWithSort(downAtnID, (Convert.ToInt32(downAtnSort) - 1).ToString(), hideUserID.Value, DateTime.Now);

                            gridContent2.DataBind();
                        }
                        else
                        {
                            ShowMessage("已經是最後一筆紀錄");
                        }
                    }

                    break;
                }

            case "Modify":
                {
                    DataTable dtAC = new DataTable();
                    //AtsID
                    string AtsID = e.CommandArgument.ToString().Split(',')[0].Trim();
                    //AtnID
                    string AtnID = e.CommandArgument.ToString().Split(',')[1].Trim();

                    switch (AtsID)
                    {
                        //副標區塊
                        case "C01":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject3.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                CKEditorControl3.Text = dtAC.Rows[0]["AtnText"].ToString();
                                //暫存
                                hideAtnID3.Value = AtnID;
                                hideAtsID3.Value = AtsID;
                                hideAtnSort3.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK3.Visible = false;
                            this.btnEdit3.Visible = true;

                            this.MultiView1.ActiveViewIndex = 2;
                            break;
                        //文字區塊
                        case "C02":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject4.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                CKEditorControl4.Text = dtAC.Rows[0]["AtnText"].ToString();
                                //暫存
                                hideAtnID4.Value = AtnID;
                                hideAtsID4.Value = AtsID;
                                hideAtnSort4.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK4.Visible = false;
                            this.btnEdit4.Visible = true;

                            this.MultiView1.ActiveViewIndex = 3;

                            break;
                        //圖片區塊
                        case "C11":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject5.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                //暫存
                                hideAtnID5.Value = AtnID;
                                hideAtsID5.Value = AtsID;
                                hideAtnSort5.Value = dtAC.Rows[0]["AtnSort"].ToString();

                                //圖片
                                if (dtAC.Rows[0]["AtnPicPath"].ToString() != "")
                                {
                                    imgAtnPicPath5.ImageUrl = string.Format("Uploads/{0}/{1}"
                                                                          , dtAC.Rows[0]["AtnID"].ToString()
                                                                          , dtAC.Rows[0]["AtnPicPath"].ToString());
                                    imgAtnPicPath5.AlternateText = dtAC.Rows[0]["AtnPicPath"].ToString();
                                }
                                else
                                {
                                    imgAtnPicPath5.ImageUrl = "Images/default.jpg";
                                }
                                hideAtnPicPath5.Value = dtAC.Rows[0]["AtnPicPath"].ToString();
                            }
                            this.btnOK5.Visible = false;
                            this.btnEdit5.Visible = true;

                            this.MultiView1.ActiveViewIndex = 4;

                            break;
                        //串流影片區塊(TouTube)
                        case "C31":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject6.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                txbAtnVideoPath6.Text = dtAC.Rows[0]["AtnVideoPath"].ToString();
                                //暫存
                                hideAtnID6.Value = AtnID;
                                hideAtsID6.Value = AtsID;
                                hideAtnSort6.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK6.Visible = false;
                            this.btnEdit6.Visible = true;

                            this.MultiView1.ActiveViewIndex = 5;

                            break;
                        //串流影片區塊(Vimeo)
                        case "C32":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject7.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                txbAtnVideoPath7.Text = dtAC.Rows[0]["AtnVideoPath"].ToString();
                                //暫存
                                hideAtnID7.Value = AtnID;
                                hideAtsID7.Value = AtsID;
                                hideAtnSort7.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK7.Visible = false;
                            this.btnEdit7.Visible = true;

                            this.MultiView1.ActiveViewIndex = 6;

                            break;
                        //串流影片區塊(優酷)
                        case "C33":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject8.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                txbAtnVideoPath8.Text = dtAC.Rows[0]["AtnVideoPath"].ToString();
                                //暫存
                                hideAtnID8.Value = AtnID;
                                hideAtsID8.Value = AtsID;
                                hideAtnSort8.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK8.Visible = false;
                            this.btnEdit8.Visible = true;

                            this.MultiView1.ActiveViewIndex = 7;

                            break;
                        //串流影片區塊(土豆網)
                        case "C34":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject9.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                txbAtnVideoPath9.Text = dtAC.Rows[0]["AtnVideoPath"].ToString();
                                //暫存
                                hideAtnID9.Value = AtnID;
                                hideAtsID9.Value = AtsID;
                                hideAtnSort9.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK9.Visible = false;
                            this.btnEdit9.Visible = true;

                            this.MultiView1.ActiveViewIndex = 8;

                            break;
                        //MP4影片區塊
                        case "C41":

                            dtAC = clsDAL.GetArticleContentByID(AtnID);
                            if (dtAC.Rows.Count != 0)
                            {
                                txbAtnSubject10.Text = dtAC.Rows[0]["AtnSubject"].ToString();
                                txbAtnVideoPath10.Text = dtAC.Rows[0]["AtnVideoPath"].ToString();
                                //暫存
                                hideAtnID10.Value = AtnID;
                                hideAtsID10.Value = AtsID;
                                hideAtnSort10.Value = dtAC.Rows[0]["AtnSort"].ToString();
                            }
                            this.btnOK10.Visible = false;
                            this.btnEdit10.Visible = true;

                            this.MultiView1.ActiveViewIndex = 9;

                            break;

                    }

                    break;
                }

            case "Cancel":
                {
                    //AtcID
                    string AtcID = e.CommandArgument.ToString().Split(',')[0].Trim();
                    //AtnID
                    string AtnID = e.CommandArgument.ToString().Split(',')[1].Trim();

                    //刪除
                    clsDAL.DeleteArticleContent(AtnID);

                    //刪除後重新排序
                    DataTable dtAC = clsDAL.GetArticleContentByAtcID(AtcID);
                    if (dtAC.Rows.Count != 0)
                    {
                        for (int i = 0; i < dtAC.Rows.Count; i++)
                        {
                            string tempAtnID = dtAC.Rows[i]["AtnID"].ToString();
                            string tempAttSort = (i + 1).ToString();

                            clsDAL.UpdateArticleContentWithSort(tempAtnID, tempAttSort, hideUserID.Value, DateTime.Now);
                        }
                    }

                    this.grid.DataBind();
                    break;
                }

            default:
                {
                    return;
                }
        }
    }

    #endregion


    #endregion



  
}