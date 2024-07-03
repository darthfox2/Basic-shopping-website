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

public partial class Backend_MANAGE_MessageList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "999";
    Common webCommon = new Common();
    DALClass clsDAL = new DALClass();
    CommonClass clsCommon = new CommonClass();
    UserInfo User = new UserInfo();

    //起始載入
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        //CheckUserPermission(strPageID, iPageType);

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

            //語系類別
            DataTable dtType = new DataTable();
            dtType = clsDAL.GetLanguage();
            webCommon.CreateList(ddlType, dtType, "LgeName", "LgeID", true);

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

    //查詢的動作
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
        }

        try
        {
            dtSet = Convert.ToDateTime(txbEDate.Text);
        }
        catch (Exception ex)
        {
            DTCheck = false;
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

    //返回按鈕
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    #region grid動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            //瀏覽明細
            case "View":
                {
                    ////ID
                    //string BalID = e.CommandArgument.ToString();
                    //string BalStartDate = "";
                    //string BalEndDate = "";
                    //string BalMoney = "";
                    //string BalShouldMoney = "";

                    //DataTable dt = new DataTable();
                    //dt = clsDAL.GetBalanceByID(BalID);
                    //if (dt.Rows.Count != 0)
                    //{
                    //    BalStartDate = Convert.ToDateTime(dt.Rows[0]["BalStartDate"]).ToString("yyyy/MM/dd HH:mm:ss");
                    //    BalEndDate = Convert.ToDateTime(dt.Rows[0]["BalEndDate"]).ToString("yyyy/MM/dd HH:mm:ss");

                    //    BalMoney = dt.Rows[0]["BalMoney"].ToString();
                    //    BalShouldMoney = dt.Rows[0]["BalShouldMoney"].ToString();
                    //}

                    ////結帳起迄日期
                    //lblBalStartDate4.Text = BalStartDate;
                    //lblBalEndDate4.Text = BalEndDate;

                    //lblBalShouldMoney4.Text = BalShouldMoney;
                    //lblBalMoney4.Text = BalMoney;

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
        ////狀態
        //Label newlblStatus = (Label)e.Row.FindControl("lblStatus");
        //if (newlblStatus != null)
        //{
        //    string BalStatus = newlblStatus.Text;
        //    newlblStatus.Text = clsCommon.ChangeBalanceStatus(BalStatus);
        //}
    }

    #endregion

    #region 匯出

    //匯出Excel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("出勤日期", typeof(string));
        //dt.Columns.Add("部門", typeof(string));
        //dt.Columns.Add("工號", typeof(string));
        //dt.Columns.Add("中文姓名", typeof(string));
        //dt.Columns.Add("上班時間", typeof(string));
        //dt.Columns.Add("下班時間", typeof(string));
        //dt.Columns.Add("出勤狀態", typeof(string));
        //dt.Columns.Add("狀態描述", typeof(string));
        //dt.Columns.Add("KPI", typeof(string));
        //dt.Columns.Add("假單狀態", typeof(string));

        //for (int i = 0; i < grid.Rows.Count; i++)
        //{
        //    ////部門
        //    //Label newlblDep = new Label();
        //    //newlblDep = (Label)grid.Rows[i].FindControl("lblDep");
        //    //string lblDep = newlblDep.Text;

        //    ////中文姓名
        //    //Label newlblCName = new Label();
        //    //newlblCName = (Label)grid.Rows[i].FindControl("lblCName");
        //    //string lblCName = newlblCName.Text;

        //    //狀態描述
        //    Label newlblStatusDesc = new Label();
        //    newlblStatusDesc = (Label)grid.Rows[i].FindControl("lblStatusDesc");
        //    string lblStatusDesc = newlblStatusDesc.Text;

        //    //出勤狀態
        //    Label newlblStatus = new Label();
        //    newlblStatus = (Label)grid.Rows[i].FindControl("lblStatus");
        //    string lblStatus = newlblStatus.Text;

        //    //KPI
        //    Label newlblKPI = new Label();
        //    newlblKPI = (Label)grid.Rows[i].FindControl("lblKPI");
        //    string lblKPI = newlblKPI.Text;

        //    //假單狀態
        //    Label newlblUltimasStatus = new Label();
        //    newlblUltimasStatus = (Label)grid.Rows[i].FindControl("lblUltimasStatus");
        //    string lblUltimasStatus = newlblUltimasStatus.Text;

        //    dt.Rows.Add(grid.Rows[i].Cells[0].Text, grid.Rows[i].Cells[1].Text, grid.Rows[i].Cells[2].Text, grid.Rows[i].Cells[3].Text, grid.Rows[i].Cells[4].Text.Replace("&nbsp;", ""), grid.Rows[i].Cells[5].Text.Replace("&nbsp;", ""), lblStatus, lblStatusDesc, lblKPI, grid.Rows[i].Cells[10].Text);
        //}

        //GridView gvExport = new GridView();
        //gvExport.DataSource = dt;
        //gvExport.DataBind();

        //string strExportFilename = "ExportWorkRecord";

        //Response.Clear();
        //Response.AddHeader("content-disposition",
        //     "attachment;filename=" + strExportFilename + ".xls");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = "application/vnd.xls";
        //Response.Charset = "big5";

        //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        //gvExport.RenderControl(htmlWrite);

        ////無資料就不匯出
        //if (dt.Rows.Count != 0)
        //{
        //    //重要標籤
        //    Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
        //    Response.Write(stringWrite.ToString().Replace("<div>", "").Replace("</div>", ""));
        //}
        //Response.End(); 
    }

    #endregion

}