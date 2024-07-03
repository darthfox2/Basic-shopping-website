using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/// <summary>
/// Common 的摘要描述
/// </summary>
public class Common
{
    //建立CheckBoxList的方法
    public void CreateCkbList(CheckBoxList ckbl, DataTable dt, string Text, string Value)
    {
        ListItem item = null;
        ckbl.Items.Clear();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(dt.Rows[i][Text].ToString(), dt.Rows[i][Value].ToString());

            ckbl.Items.Add(item);
        }
    }

    //取得CheckBox的值(1:啟用0:停用)
    public int GetChecked(CheckBox ckb)
    {
        int checkValue = 0;
        if (ckb.Checked)
        {
            checkValue = 1;
        }
        else
        {
            checkValue = 0;
        }
        return checkValue;
    }

    //設定CheckBox的值(1:啟用0:停用)
    public void SetChecked(CheckBox ckb, int checkValue)
    {
        if (checkValue == 1)
        {
            ckb.Checked = true;
        }
        else if (checkValue == 0)
        {
            ckb.Checked = false;
        }
    }

    //建立下拉選單的方法
    public void CreateList(DropDownList ddl, DataTable dt, string Text, bool ShowAll)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem("全部", "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(dt.Rows[i][Text].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立下拉選單的方法(增加Value)
    public void CreateList(DropDownList ddl, DataTable dt, string Text, string Value, bool ShowAll)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem("全部", "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(dt.Rows[i][Text].ToString(), dt.Rows[i][Value].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立下拉選單的方法(自訂ShowAll)
    public void CreateList(DropDownList ddl, DataTable dt, string Text, string Value, bool ShowAll, string ShowText)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem(ShowText, "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(dt.Rows[i][Text].ToString(), dt.Rows[i][Value].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立下拉選單的方法(含兩個Text合併)
    public void CreateList(DropDownList ddl, DataTable dt, string Text, string Text2, string Value, bool ShowAll)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem("全部", "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(string.Format("{0}({1})", dt.Rows[i][Text].ToString(), dt.Rows[i][Text2].ToString()), dt.Rows[i][Value].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立下拉選單的方法(含兩個Text合併)(自訂前置文字)
    public void CreateListWithPreWord(DropDownList ddl, DataTable dt, string PreWord, string Text, string Value, bool ShowAll, string ShowText)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem(ShowText, "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(string.Format("{0}{1}", PreWord , dt.Rows[i][Text].ToString()), dt.Rows[i][Value].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立下拉選單的方法(含兩個Text合併)(自訂ShowAll)
    public void CreateList(DropDownList ddl, DataTable dt, string Text, string Text2, string Value, bool ShowAll, string ShowText)
    {
        ListItem item = null;
        ddl.Items.Clear();

        if (ShowAll)
        {
            item = new ListItem(ShowText, "-1");
            ddl.Items.Add(item);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            item = new ListItem(string.Format("{0}({1})", dt.Rows[i][Text].ToString(), dt.Rows[i][Text2].ToString()), dt.Rows[i][Value].ToString());

            ddl.Items.Add(item);
        }
    }

    //建立樹的方法
    public void CreateTree(TreeView tree, DataTable dt, string Text, string Value)
    {
        TreeNode newnode = null;
        tree.Nodes.Clear();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newnode = new TreeNode(dt.Rows[i][Text].ToString(), dt.Rows[i][Value].ToString());

            tree.Nodes.Add(newnode);
        }
    }

    //設定民國年下拉選單(從今年到之前哪一年)
    public void SetTYearList(DropDownList ddl, int LastTYear)
    {
        ListItem item = null;
        ddl.Items.Clear();

        int NowTY = GetTaiwanYear(DateTime.Now);

        for (int i = NowTY; i >= LastTYear; i--)
        {
            item = new ListItem(SetFixCode(i.ToString(), 3), SetFixCode(i.ToString(), 3));

            ddl.Items.Add(item);
        }
    }

    //設定西元年下拉選單(從今年到之前哪一年)
    public void SetWYearBeforeList(DropDownList ddl, int LastTYear)
    {
        ListItem item = null;
        ddl.Items.Clear();

        int NowWY = DateTime.Now.Year;

        for (int i = NowWY; i >= LastTYear; i--)
        {
            item = new ListItem(i.ToString(), i.ToString());

            ddl.Items.Add(item);
        }
    }

    //取得民國年
    public int GetTaiwanYear(DateTime dtDate)
    {
        int TYear = dtDate.Year - 1911;

        return TYear;
    }

    //取得西元年
    public int GetWestYear(int TYear)
    {
        int CYear = TYear + 1911;

        return CYear;
    }

    //數碼不足(FIX)自動補0
    public string SetFixCode(string orgCode, int Fix)
    {
        if (orgCode.Length < Fix)
        {
            orgCode = "0" + orgCode;
        }

        return orgCode;
    }

    #region 色碼處理

    //判斷色碼並回傳文字
    public string GetColorText(string ColorCode)
    {
        string ColorText = "";
        switch (ColorCode)
        {
            case "#BFBFBF":
                ColorText = "灰色";
                break;
            case "#81FF81":
                ColorText = "淺綠色";
                break;
            case "#00AF50":
                ColorText = "深綠色";
                break;
            case "#FE80FE":
                ColorText = "粉紅色";
                break;
            case "#FF0080":
                ColorText = "桃色";
                break;
            case "#FE0000":
                ColorText = "紅色";
                break;
            case "#FEFD0F":
                ColorText = "黃色";
                break;
            case "#7FFFFE":
                ColorText = "淺藍色";
                break;
            case "#007FFF":
                ColorText = "天藍色";
                break;
            case "#024678":
                ColorText = "深藍色";
                break;
            case "#9ACEC9":
                ColorText = "靛色";
                break;
            case "#7F80C1":
                ColorText = "淺紫色";
                break;
            case "#7030A0":
                ColorText = "深紫色";
                break;
        }
        return ColorText;
    }

    #endregion

}