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

public partial class Backend_MANAGE_NewsTypelist : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "A01";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();
    UserInfo User = new UserInfo();
    ////WebConfig參數
    //protected string SiteDomain = WebConfigurationManager.AppSettings["SiteDomain"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        CheckUserPermission(strPageID, iPageType);

        grid.Columns[0].ItemStyle.Width = 50;
        grid.Columns[0].ControlStyle.Width = 50;
        grid.Columns[2].ItemStyle.Width = 300;
        grid.Columns[2].ControlStyle.Width = 300;
        grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;

        if (!this.Page.IsPostBack)
        {
            //取得登入者
            User = (UserInfo)Session["UserInfo"];
            hideUserID.Value = User.UserID;

        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        txbNwtName.Text = "";
        txbNwtDesc.Text = "";

        this.btnOK.Visible = true;
        this.btnEdit.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string NwtID = clsDAL.GetKeyValue("NWT");
            string NwtName = txbNwtName.Text;
            string NwtDesc = txbNwtDesc.Text;
            string NwtEnable = "1";
            string NwtSort = Convert.ToString(grid.Rows.Count + 1);
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            int Result = clsDAL.InsertNewsType(NwtID, NwtName, NwtDesc, NwtEnable, NwtSort, LastUpdateUser, LastUpdateDate);

            if (Result == 1)
            {
                //ShowMessage("儲存成功");
                grid.DataBind();
                this.MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                ShowMessage("儲存失敗");
            }

        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string NwtID = hideID.Value;
            string NwtName = txbNwtName.Text;
            string NwtDesc = txbNwtDesc.Text;
            string NwtEnable = "1";
            string NwtSort = hideSort.Value;
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            bool Result = clsDAL.UpdateNewsType(NwtID, NwtName, NwtDesc, NwtEnable, NwtSort, LastUpdateUser, LastUpdateDate);

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

        }
    }


    #region GridView動作

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "modify":
                {
                    //ID
                    string NwtID = e.CommandArgument.ToString();
                    //暫存

                    hideID.Value = NwtID;

                    DataTable dt = new DataTable();

                    dt = clsDAL.GetNewsTypeByID(NwtID);

                    if (dt.Rows.Count != 0)
                    {
                        txbNwtName.Text = dt.Rows[0]["NwtName"].ToString();
                        txbNwtDesc.Text = dt.Rows[0]["NwtDesc"].ToString();
                        hideSort.Value = dt.Rows[0]["NwtSort"].ToString();
                    }

                    this.btnOK.Visible = false;
                    this.btnEdit.Visible = true;
                    this.MultiView1.ActiveViewIndex = 1;

                    break;
                }

            case "cancel":
                {
                    //ID
                    string NwtID = e.CommandArgument.ToString();

                    clsDAL.DeleteNewsType(NwtID);
                    this.grid.DataBind();

                    //顯示更新後的排序
                    DataTable dt = new DataTable();
                    dt = clsDAL.GetNewsType();

                    for (int i = 0; i < grid.Rows.Count; i++)
                    {
                        string NwtSort = dt.Rows[i]["NwtSort"].ToString();

                        clsDAL.NewsTypeSort(NwtSort, i + 1);
                    }

                    this.grid.DataBind();
                    break;
                }

            case "up":
                {
                    string NwtSort = e.CommandArgument.ToString();

                    bool Result = clsDAL.NewsTypeSortUp(NwtSort);
                    if (Result == false)
                    {
                        ShowMessage("此列為第一列");
                        break;
                    }
                    else
                    {
                        this.grid.DataBind();
                        break;
                    }

                }

            case "down":
                {
                    string NwtSort = e.CommandArgument.ToString();

                    bool Result = clsDAL.NewsTypeSortDown(NwtSort);
                    if (Result == false)
                    {
                        ShowMessage("此列為最後一列");
                        break;
                    }
                    else
                    {
                        this.grid.DataBind();
                        break;
                    }
                }

            default:
                {
                    return;
                }
        }
    }

    #endregion
}