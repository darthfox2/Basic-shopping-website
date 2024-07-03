using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using DAL;

/// <summary>
/// WebService 的摘要描述
/// </summary>
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    protected DALClass clsDAL = new DALClass();

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //方法不可宣告static
    public string[] GetCompletionList(string prefixText, int count)
    {
        List<string> suggestions = new List<string>();
        DataTable dtProductCode = new DataTable();
        //是否啟用 及 開盤狀態
        //dtProductCode = clsDAL.GetProductByCode(prefixText, count, "1", "1");

        try
        {
            for (int i = 0; i < dtProductCode.Rows.Count; i++)
            {
                suggestions.Add(dtProductCode.Rows[i]["PrdCode"].ToString());
            }

            return suggestions.ToArray();
        }
        catch (Exception ex)
        {
            suggestions.Add(ex.Message);
            return suggestions.ToArray();
        }
    }
}

