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

public partial class System_ExceptionList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "L01";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();
    protected ExceptionHandle clsEx = new ExceptionHandle();
    UserInfo User = new UserInfo();


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
            //定義本頁的錯誤類別代碼
            clsEx.ExrType = "EXT021";

            //取得登入者
            User = (UserInfo)Session["UserInfo"];
            hideUserID.Value = User.UserID;

            // 錯誤類別
            DataTable dtType = new DataTable();
            dtType = clsDAL.GetErrType();
            webCommon.CreateList(ddlType, dtType, "ExtType", "ExtType", true);

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
        //檢查日期是否填錯
        bool DTCheck = true;
        DateTime dtSet = DateTime.Now;
        try
        {
            dtSet = Convert.ToDateTime(txbSDate.Text);
        }
        catch (Exception ex)
        {
            DTCheck = false;
            clsEx.ExrMsg = ex.ToString();
            clsEx.NewExceptionMsg();
        }
    
        try
        {
            dtSet = Convert.ToDateTime(txbEDate.Text);
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
            if (Convert.ToDateTime(txbSDate.Text) <= Convert.ToDateTime(txbEDate.Text))
            {
                //暫存資訊
                hideSDate.Value = txbSDate.Text;
                hideEDate.Value = txbEDate.Text;
                grid.DataBind();
            }
            else
            {
                lblMeg.Text = "起始時間不能大於結束時間";
            }
        }
        else
        {
            lblMeg.Text = "請輸入正確的日期";
        }
    }

    #endregion


    #region Gridview動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            //瀏覽明細
            case "View":
                {
                    this.MultiView1.ActiveViewIndex = 3;
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

    }

    #endregion
}