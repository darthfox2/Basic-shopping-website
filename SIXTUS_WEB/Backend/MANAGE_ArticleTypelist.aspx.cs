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

public partial class Backend_MANAGE_ArticleTypelist : PageBase
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
        txbAttName.Text = "";
        txbAttDesc.Text = "";

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
            


            string AttID = clsDAL.GetKeyValue("AT");
            string AttName = txbAttName.Text;
            string AttDesc = txbAttDesc.Text;
            string AttEnable = "1";
            string AttSort = Convert.ToString(grid.Rows.Count + 1);
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            int Result = clsDAL.InsertArticleType(AttID, AttName, AttDesc, AttEnable, AttSort, LastUpdateUser, LastUpdateDate);

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
            string AttID = hideID.Value;
            string AttName = txbAttName.Text;
            string AttDesc = txbAttDesc.Text;
            string AttEnable = "1";
            string AttSort = hideSort.Value;
            string LastUpdateUser = hideUserID.Value;
            DateTime LastUpdateDate = DateTime.Now;

            bool Result = clsDAL.UpdateArticleType(AttID, AttName, AttDesc, AttEnable, AttSort, LastUpdateUser, LastUpdateDate);

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
                    string AttID = e.CommandArgument.ToString();
                    //暫存

                    hideID.Value = AttID;
                    
                    DataTable dt = new DataTable();

                    dt = clsDAL.GetArticleTypeByID(AttID);

                    if (dt.Rows.Count != 0)
                    {
                        txbAttName.Text = dt.Rows[0]["AttName"].ToString();
                        txbAttDesc.Text = dt.Rows[0]["AttDesc"].ToString();
                        hideSort.Value = dt.Rows[0]["AttSort"].ToString();
                    }

                    this.btnOK.Visible = false;
                    this.btnEdit.Visible = true;
                    this.MultiView1.ActiveViewIndex = 1;

                    break;
                }

            case "cancel":
                {
                    //ID
                    string AttID = e.CommandArgument.ToString();

                    clsDAL.DeleteArticleType(AttID);
                    this.grid.DataBind();

                    //顯示更新後的排序
                    DataTable dt = new DataTable();
                    dt = clsDAL.GetArticleType();

                    for (int i = 0; i < grid.Rows.Count;i++ )
                    {
                        string AttSort = dt.Rows[i]["AttSort"].ToString();

                        clsDAL.ArticleTypeSort(AttSort, i + 1);
                    }


                        this.grid.DataBind();
                    break;
                }

            case "up":
                {
                    string AttSort = e.CommandArgument.ToString();

                    bool Result = clsDAL.ArticleTypeSortUp(AttSort);
                    if(Result==false)
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

            case "dowm":
                {
                    string AttSort = e.CommandArgument.ToString();

                    bool Result = clsDAL.ArticleTypeSortDown(AttSort);
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