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

public partial class Backend_System_RoleList : PageBase
{
    new protected int iPageType = 1;
    new protected string strPageID = "S03";
    Common webCommon = new Common();
    protected DALClass clsDAL = new DALClass();
    protected CommonClass clsCommon = new CommonClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();
        CheckUserPermission(strPageID, iPageType);
    }

    //新增按鈕的動作
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.txbRolID.Enabled = true;
        this.txbRolID.Text = "";
        this.txbRolName.Text = "";
        this.ckbIsEnable.Checked = true;

        //權限清單
        DataTable dtPerm = new DataTable();
        dtPerm = clsDAL.GetPermission();
        webCommon.CreateCkbList(ckblPermission, dtPerm, "PerName", "PerId");

        this.btnOK.Visible = true;
        this.btnEdit.Visible = false;
        this.MultiView1.ActiveViewIndex = 1;

        //指標到標題欄位
        this.txbRolID.Focus();
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
            int returnID = clsDAL.AddRoles(txbRolID.Text, txbRolName.Text, webCommon.GetChecked(ckbIsEnable));

            if (returnID == 1)
            {
                //更新帳號權限
                UpdatePermRole(ckblPermission, txbRolID.Text);

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
            bool returnBool = clsDAL.UpdateRoles(hideRolID.Value, txbRolName.Text, webCommon.GetChecked(ckbIsEnable));

            if (returnBool)
            {
                //更新帳號權限
                UpdatePermRole(ckblPermission, txbRolID.Text);

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
                    //RoleID
                    string RID = e.CommandArgument.ToString();

                    DataTable dt = new DataTable();
                    dt = clsDAL.GetRoleByID(RID);
                    this.txbRolID.Text = dt.Rows[0]["RolID"].ToString();
                    this.txbRolID.Enabled = false;//禁止修改角色編號
                    this.hideRolID.Value = dt.Rows[0]["RolID"].ToString();//暫存原始ID
                    this.txbRolName.Text = dt.Rows[0]["RolName"].ToString();
                    this.webCommon.SetChecked(ckbIsEnable, Convert.ToInt32(dt.Rows[0]["RolEnable"]));

                    //權限清單
                    DataTable dtPerm = new DataTable();
                    dtPerm = clsDAL.GetPermission();
                    webCommon.CreateCkbList(ckblPermission, dtPerm, "PerName", "PerId");

                    SetPermRole(ckblPermission, RID);

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
                    //RoleID
                    string RID = e.CommandArgument.ToString();

                    clsDAL.DelRoles(RID);
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

    #region 方法

    //更新帳號權限
    protected void UpdatePermRole(CheckBoxList ckbl, string RolID)
    {
        string PerId = "";

        for (int i = 0; i < ckbl.Items.Count; i++)
        {
            PerId = ckbl.Items[i].Value;

            if (ckbl.Items[i].Selected == true)
            {
                clsDAL.AddPermRole(RolID, PerId, 15);
            }
            else
            {
                clsDAL.DelPermRole(RolID, PerId);
            }
        }
    }

    //設定帳號權限
    protected void SetPermRole(CheckBoxList ckbl, string RolID)
    {
        DataTable dt = new DataTable();
        dt = clsDAL.GetPermRoleByID(RolID);

        if (dt.Rows.Count != 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ckbl.Items.FindByValue(dt.Rows[i]["PerID"].ToString()).Selected = true; ;
            }
        }
    }

    #endregion
}