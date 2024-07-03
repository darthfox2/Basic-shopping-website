using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using DAL;

public partial class Backend_System_PermissionList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        //CheckUserPermission(strPageID, iPageType);
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.txbPerID.Enabled = true;
        this.txbPerID.Text = "";
        this.txbPerName.Text = "";
        this.txbPerAddr.Text = "";

        this.btnOK.Visible = true;
        this.btnEdit.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;

        //指標到標題欄位
        this.txbPerID.Focus();
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
            int returnID = clsDAL.AddPermission(txbPerID.Text, txbPerName.Text, txbPerAddr.Text);

            if (returnID == 1)
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

    //修改存檔
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            bool returnBool = clsDAL.UpdatePerm(hideID.Value, txbPerName.Text, txbPerAddr.Text);

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
                    //PerID
                    string PID = e.CommandArgument.ToString();
                    
                    DataTable dt = new DataTable();
                    dt = clsDAL.GetPermByID(PID);

                    this.txbPerID.Text = dt.Rows[0]["PerId"].ToString();
                    this.txbPerID.Enabled = false;//禁止修改帳號
                    this.hideID.Value = dt.Rows[0]["PerId"].ToString();//暫存原始ID
                    this.txbPerName.Text = dt.Rows[0]["PerName"].ToString();
                    this.txbPerAddr.Text = dt.Rows[0]["PerPage"].ToString();

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
                    //PerID
                    string PID = e.CommandArgument.ToString();

                    clsDAL.DelPerm(PID);
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
}