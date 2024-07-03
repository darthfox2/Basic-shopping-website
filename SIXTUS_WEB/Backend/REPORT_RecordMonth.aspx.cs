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

public partial class Backend_REPORT_RecordMonth : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "M04";
    Common webCommon = new Common();
    DALClass clsDAL = new DALClass();
    CommonClass clsCommon = new CommonClass();
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

            //設定日期

            //載入年度
            DateTime dtNow = DateTime.Now;
            webCommon.SetWYearBeforeList(ddlYear, 2013);
            ddlMonth.Items.FindByText(dtNow.Month.ToString()).Selected = true;

            //暫存資訊
            hideSDate.Value = string.Format("{0}/{1}/1 00:00:00", dtNow.Year.ToString(), dtNow.Month.ToString());
            hideEDate.Value = string.Format("{0}/{1}/{2} 23:59:59", dtNow.Year.ToString(), dtNow.Month.ToString(), Convert.ToDateTime(hideSDate.Value).AddMonths(1).AddDays(-1).Day.ToString());

            this.grid.DataBind();
        }
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

    #region 下拉選單

    //年度下拉選單的動作
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideSDate.Value = string.Format("{0}/{1}/1 00:00:00", ddlYear.Text, ddlMonth.Text);
        hideEDate.Value = string.Format("{0}/{1}/{2} 23:59:59", ddlYear.Text, ddlMonth.Text, Convert.ToDateTime(hideSDate.Value).AddMonths(1).AddDays(-1).Day.ToString());

        this.grid.DataBind();
    }

    //月份下拉選單的動作
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideSDate.Value = string.Format("{0}/{1}/1 00:00:00", ddlYear.Text, ddlMonth.Text);
        hideEDate.Value = string.Format("{0}/{1}/{2} 23:59:59", ddlYear.Text, ddlMonth.Text, Convert.ToDateTime(hideSDate.Value).AddMonths(1).AddDays(-1).Day.ToString());

        this.grid.DataBind();
    }

    #endregion

    #region grid動作

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //訂單已完成
        Label newlblRD = (Label)e.Row.FindControl("lblRD");
        if (newlblRD != null)
        {
            DataTable dtRD = clsDAL.GetRecordByDateRangeAndStatus(Convert.ToDateTime(hideSDate.Value), Convert.ToDateTime(hideEDate.Value), "RD");
            if (dtRD.Rows.Count != 0)
            {
                newlblRD.Text = dtRD.Rows[0]["COUNT"].ToString();
            }
        }

        //完成付款
        Label newlblCP = (Label)e.Row.FindControl("lblCP");
        if (newlblCP != null)
        {
            DataTable dtCP = clsDAL.GetRecordByDateRangeAndStatus(Convert.ToDateTime(hideSDate.Value), Convert.ToDateTime(hideEDate.Value), "CP");
            if (dtCP.Rows.Count != 0)
            {
                newlblCP.Text = dtCP.Rows[0]["COUNT"].ToString();
            }
        }

        //出貨中
        Label newlblSI = (Label)e.Row.FindControl("lblSI");
        if (newlblSI != null)
        {
            DataTable dtSI = clsDAL.GetRecordByDateRangeAndStatus(Convert.ToDateTime(hideSDate.Value), Convert.ToDateTime(hideEDate.Value), "SI");
            if (dtSI.Rows.Count != 0)
            {
                newlblSI.Text = dtSI.Rows[0]["COUNT"].ToString();
            }
        }

        //完成出貨
        Label newlblCS = (Label)e.Row.FindControl("lblCS");
        if (newlblCS != null)
        {
            DataTable dtCS = clsDAL.GetRecordByDateRangeAndStatus(Convert.ToDateTime(hideSDate.Value), Convert.ToDateTime(hideEDate.Value), "CS");
            if (dtCS.Rows.Count != 0)
            {
                newlblCS.Text = dtCS.Rows[0]["COUNT"].ToString();
            }
        }

        //訂單終結
        Label newlblOK = (Label)e.Row.FindControl("lblOK");
        if (newlblOK != null)
        {
            DataTable dtOK = clsDAL.GetRecordByDateRangeAndStatus(Convert.ToDateTime(hideSDate.Value), Convert.ToDateTime(hideEDate.Value), "OK");
            if (dtOK.Rows.Count != 0)
            {
                newlblOK.Text = dtOK.Rows[0]["COUNT"].ToString();
            }
        }
    }

    #endregion

    #region 匯出

    //匯出Excel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("訂單已完成(單)", typeof(string));
        dt.Columns.Add("完成付款(單)", typeof(string));
        dt.Columns.Add("出貨中(單)", typeof(string));
        dt.Columns.Add("完成出貨(單)", typeof(string));
        dt.Columns.Add("訂單終結(單)", typeof(string));
        dt.Columns.Add("商品總數量", typeof(string));
        dt.Columns.Add("商品總金額(NTD)", typeof(string));

        for (int i = 0; i < grid.Rows.Count; i++)
        {
            //訂單已完成
            Label newlblRD = new Label();
            newlblRD = (Label)grid.Rows[i].FindControl("lblRD");
            string lblRD = newlblRD.Text;

            //完成付款
            Label newlblCP = new Label();
            newlblCP = (Label)grid.Rows[i].FindControl("lblCP");
            string lblCP = newlblCP.Text;

            //出貨中
            Label newlblSI = new Label();
            newlblSI = (Label)grid.Rows[i].FindControl("lblSI");
            string lblSI = newlblSI.Text;

            //完成出貨
            Label newlblCS = new Label();
            newlblCS = (Label)grid.Rows[i].FindControl("lblCS");
            string lblCS = newlblCS.Text;

            //訂單終結
            Label newlblOK = new Label();
            newlblOK = (Label)grid.Rows[i].FindControl("lblOK");
            string lblOK = newlblOK.Text;

            dt.Rows.Add(newlblRD.Text, newlblCP.Text, newlblSI.Text, newlblCS.Text, newlblOK.Text ,grid.Rows[i].Cells[5].Text, grid.Rows[i].Cells[6].Text);
        }

        GridView gvExport = new GridView();
        gvExport.DataSource = dt;
        gvExport.DataBind();

        string strExportFilename = "ExportWorkRecord";

        Response.Clear();
        Response.AddHeader("content-disposition",
             "attachment;filename=" + strExportFilename + ".xls");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.xls";
        Response.Charset = "big5";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvExport.RenderControl(htmlWrite);

        //無資料就不匯出
        if (dt.Rows.Count != 0)
        {
            //重要標籤
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            Response.Write(stringWrite.ToString().Replace("<div>", "").Replace("</div>", ""));
        }
        Response.End(); 
    }

    #endregion


}