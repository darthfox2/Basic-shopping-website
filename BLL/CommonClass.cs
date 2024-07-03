using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using DAL;
using System.Text;

namespace BLL
{
    public class CommonClass
    {
        DALClass newDAL = new DALClass();

        #region 通用

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

        #endregion

        #region 字串處理

        //裁減字數限制
        public string GetShortText(string FullText, int MaxWords)
        {
            string ShortText = "";

            if (FullText.Length >= MaxWords)
            {
                ShortText = FullText.Substring(0, MaxWords) + "...";
            }
            else
            {
                ShortText = FullText;
            }

            return ShortText;
        }

        //裁減字數限制,加上超連結
        public string GetShortText(string FullText, int MaxWords, string LinkPage)
        {
            string ShortText = "";

            if (FullText.Length >= MaxWords)
            {
                ShortText = FullText.Substring(0, MaxWords) + "<a href='" + LinkPage + "'>...more</a>";
            }
            else
            {
                ShortText = FullText;
            }

            return ShortText;
        }

        //復原字串斷行(HTML)
        public string RestoreHTMLText(string OrgText)
        {
            string FixText = "";

            FixText = OrgText.Replace("\r\n", "<br>");

            return FixText;
        }

        //判斷為匯出可能有問題的工號做轉換
        public string ExchangeEmpID(string EmpID)
        {
            string fixEmpID = "";

            char[] array = EmpID.ToCharArray();
            if (array[2] == 'E')
            {
                fixEmpID = string.Format("{0}.", EmpID);
            }
            else
            {
                fixEmpID = EmpID;
            }

            return fixEmpID;
        }

        #endregion

        #region 數碼處理

        //數碼不足自動補0
        public string SetFixCode(string orgCode, int CodeLength)
        {
            if (orgCode.Length < CodeLength)
            {
                int FixLength = CodeLength - orgCode.Length;
                string FixCode = "";
                for (int i = 0; i < FixLength; i++)
                {
                    FixCode = FixCode + "0";
                }

                orgCode = FixCode + orgCode;
            }

            return orgCode;
        }

        //數碼自動去除0
        public string SetClearCode(string orgCode)
        {
            if (orgCode.Length >= 3)
            {
                string temp = orgCode.Substring(0, 1);

                if (temp == "0")
                {
                    orgCode = orgCode.Remove(0, 1);
                }
            }

            return orgCode;
        }

        //判定是否為偶數
        public bool CheckIsEven(int Num)
        {
            bool Check = false;

            if (Num % 2 == 0)
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

            return Check;
        }

        //判定是否為奇數
        public bool CheckIsOdd(int Num)
        {
            bool Check = false;

            if (Num % 2 == 0)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }

            return Check;
        }

        //取出指定數碼
        public string GetCuteCode(string orgCode, int CodeLength , int StartIndex, int CutLength)
        {
            string newCode = "";

            if (orgCode.Length == CodeLength)
            {
                newCode = orgCode.Substring(StartIndex, CutLength);
            }

            return newCode;
        }

        //轉換成美元格式
        public string GetDollar(string Price)
        {
            string Dollar = "";

            Dollar = "$" + Price;

            return Dollar;
        }

        //判斷是否為3的倍數
        public bool CheckNumber(int Number)
        {
            bool Check = false;

            if (Number % 3 == 0)
            {
                Check = true;
            }

            return Check;
        }

        #endregion

        #region 狀態處理

        //轉換啟停狀態
        public string ChangeIsEnable(int IsEnable)
        {
            string IsEnableText = "";

            if (IsEnable == 1)
            {
                IsEnableText = "啟用";
            }
            else if (IsEnable == 0)
            {
                IsEnableText = "停用";
            }

            return IsEnableText;
        }

        //轉換卡片狀態
        public string ChangeApplyStatus(string ApyStatus)
        {
            string StatusText = "";

            switch (ApyStatus)
            {
                case "0":
                    StatusText = "停用";
                    break;
                case "1":
                    StatusText = "正常";
                    break;
                case "2":
                    StatusText = "逾期";
                    break;
            }

            return StatusText;
        }

        //轉換核銷狀態
        public string ChangeBalanceStatus(string BalStatus)
        {
            string StatusText = "";

            switch (BalStatus)
            {
                case "0":
                    StatusText = "未核銷";
                    break;
                case "1":
                    StatusText = "已核銷";
                    break;
            }

            return StatusText;
        }

        //轉換進出類型
        public string ChangePayType(string PayType)
        {
            string PayTypeText = "";

            switch (PayType)
            {
                case "1":
                    PayTypeText = "卡片";
                    break;
                case "2":
                    PayTypeText = "臨停票卷";
                    break;
            }

            return PayTypeText;
        }

        //轉換交易類型
        public string ChangeSellKing(string SlgKing)
        {
            string SlgKingText = "";

            switch (SlgKing)
            {
                case "1":
                    SlgKingText = "新增";
                    break;
                case "2":
                    SlgKingText = "儲值";
                    break;
                case "3":
                    SlgKingText = "補卡";
                    break;
            }

            return SlgKingText;
        }

        //轉換訂單狀態
        public string ChangeORStatus(string OdrStatus)
        {
            string StatusText = "";

            switch (OdrStatus)
            {
                case "NO":
                    StatusText = "訂單未完成";
                    break;
                case "RD":
                    StatusText = "訂單已完成";
                    break;
                case "CP":
                    StatusText = "完成付款";
                    break;
                case "SI":
                    StatusText = "出貨中";
                    break;
                case "CS":
                    StatusText = "完成出貨";
                    break;
                case "OK":
                    StatusText = "訂單終結";
                    break;
                case "DL":
                    StatusText = "訂單刪除";
                    break;
            }

            return StatusText;
        }

        //轉換BLOG狀態
        public string ChangeBlogStatus(string BlgEnable)
        {
            string StatusText = "";

            switch (BlgEnable)
            {
                case "0":
                    StatusText = "下架中";
                    break;
                case "1":
                    StatusText = "上架中";
                    break;
            }

            return StatusText;
        }

        //轉換交易狀態
        public string ChangeTradeType(string Type)
        {
            string TypeText = "";

            switch (Type)
            {
                case "1":
                    TypeText = "買入";
                    break;
                case "2":
                    TypeText = "賣出";
                    break;
            }

            return TypeText;
        }

        #endregion

        #region 年度日期處理

        //根據日期取得今天星期幾
        public int GetDayOfWeekByDate(DateTime dtDate)
        {
            int intDOW = 0;
            string strDOW = dtDate.DayOfWeek.ToString();

            if (strDOW == "Sunday")
            {
                intDOW = 7;//星期日 
            }
            else if (strDOW == "Monday")
            {
                intDOW = 1;//星期一 
            }
            else if (strDOW == "Tuesday")
            {
                intDOW = 2;//星期二 
            }
            else if (strDOW == "Wednesday")
            {
                intDOW = 3;//星期三 
            }
            else if (strDOW == "Thursday")
            {
                intDOW = 4;//星期四 
            }
            else if (strDOW == "Friday")
            {
                intDOW = 5;//星期五 
            }
            else if (strDOW == "Saturday")
            {
                intDOW = 6;//星期六 
            }

            return intDOW;
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

        //取得該週起始日期
        public DateTime GetWeekStart(DateTime Stime)
        {
            int Week = Stime.DayOfWeek.GetHashCode();
            DateTime _time = Stime.Date.AddDays(0 - Week);
            return _time;
        }

        //取得該週結束日期
        public DateTime GetWeekEnd(DateTime Stime)
        {
            int Week = Stime.DayOfWeek.GetHashCode();
            DateTime _time = Stime.Date.AddDays(0 - Week).AddDays(6);
            return _time;
        }

        #endregion

        #region 圖片管理

        //上傳圖片（自動轉為JPG）(不縮圖)
        public int UploadOrgJpgPic(FileUpload ful, string UploadFolderPath)
        {
            int status = 0;//正常

            if (ful.HasFile)
            {
                if (IsValidImage(ful.FileName))
                {
                    string RealFolderPath = UploadFolderPath;
                    //檢查資料夾是否存在
                    if (!Directory.Exists(RealFolderPath))
                    {
                        Directory.CreateDirectory(RealFolderPath);
                    }

                    string RealFilePath = RealFolderPath + Path.ChangeExtension(ful.FileName, ".jpg");
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(ful.PostedFile.InputStream))
                    {

                        int thumbWidth = 0;
                        int thumbHeight = 0;

                        thumbWidth = image.Width;
                        thumbHeight = image.Height;

                        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image, new System.Drawing.Size(thumbWidth, thumbHeight)))
                        {
                            if (Directory.Exists(RealFolderPath))
                            {
                                if (!File.Exists(RealFilePath))
                                {
                                    bitmap.Save(RealFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    status = 0;
                                }
                                else
                                {
                                    status = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    status = 2;
                }
            }

            return status;
        }

        //上傳圖片（自動轉為JPG）
        public int UploadJpgPic(FileUpload ful, string UploadFolderPath)
        {
            int status = 0;//正常

            if (ful.HasFile)
            {
                if (IsValidImage(ful.FileName))
                {
                    string RealFolderPath = UploadFolderPath;
                    //檢查資料夾是否存在
                    if (!Directory.Exists(RealFolderPath))
                    {
                        Directory.CreateDirectory(RealFolderPath);
                    }

                    string RealFilePath = RealFolderPath + Path.ChangeExtension(ful.FileName, ".jpg");
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(ful.PostedFile.InputStream))
                    {
                        //圖片尺寸若大於600px才進行縮小
                        int thumbWidth = 0;
                        int thumbHeight = 0;
                        if (image.Height > 600 || image.Width > 600)
                        {
                            decimal sizeRatio = ((decimal)image.Height / image.Width);
                            thumbWidth = 600;
                            thumbHeight = decimal.ToInt32(sizeRatio * thumbWidth);
                        }
                        else
                        {
                            thumbWidth = image.Width;
                            thumbHeight = image.Height;
                        }

                        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image, new System.Drawing.Size(thumbWidth, thumbHeight)))
                        {
                            if (Directory.Exists(RealFolderPath))
                            {
                                if (!File.Exists(RealFilePath))
                                {
                                    bitmap.Save(RealFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    status = 0;
                                }
                                else
                                {
                                    status = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    status = 2;
                }
            }

            return status;
        }

        //上傳圖片(檔名為序號)（自動轉為寬為1080的JPG）
        public int UploadBigJpgPicWithNewName(FileUpload ful, string UploadFolderPath, string newFileName)
        {
            int status = 0;//正常

            if (ful.HasFile)
            {
                if (IsValidImage(ful.FileName))
                {
                    string RealFolderPath = UploadFolderPath;
                    //檢查資料夾是否存在
                    if (!Directory.Exists(RealFolderPath))
                    {
                        Directory.CreateDirectory(RealFolderPath);
                    }

                    string RealFilePath = RealFolderPath + Path.ChangeExtension(newFileName, ".jpg");
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(ful.PostedFile.InputStream))
                    {
                        //圖片尺寸若大於1080才進行縮小
                        int thumbWidth = 0;
                        int thumbHeight = 0;
                        if (image.Height > 1980 || image.Width > 1080)
                        {
                            decimal sizeRatio = ((decimal)image.Height / image.Width);
                            thumbWidth = 1080;
                            thumbHeight = decimal.ToInt32(sizeRatio * thumbWidth);
                        }
                        else
                        {
                            thumbWidth = image.Width;
                            thumbHeight = image.Height;
                        }

                        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image, new System.Drawing.Size(thumbWidth, thumbHeight)))
                        {
                            if (Directory.Exists(RealFolderPath))
                            {
                                if (!File.Exists(RealFilePath))
                                {
                                    bitmap.Save(RealFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    status = 0;
                                }
                                else
                                {
                                    status = 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    status = 2;
                }
            }

            return status;
        }

        //檢查檔案格式是否正確
        public bool IsValidImage(string path)
        {
            return Regex.IsMatch(path, @"(.*?)\.(bmp|gif|jpg|jpeg|png)$", RegexOptions.IgnoreCase);
        }

        //根據錯誤代碼取得說明字串
        public string GetErrorText(int ErrorCode)
        {
            string ErrorText = "";

            switch (ErrorCode)
            {
                case 0:
                    ErrorText = "正常";
                    break;
                case 1:
                    ErrorText = "上傳失敗，檔案已存在";
                    break;
                case 2:
                    ErrorText = "檔案格式錯誤，僅支援（bmp,jpg,jpeg,png,gif）等格式";
                    break;
                case 3:
                    ErrorText = "請先選擇檔案路徑";
                    break;
                case 4:
                    ErrorText = "上傳失敗，超過五張圖片";
                    break;
            }

            return ErrorText;
        }

        #endregion

        #region 時間處理

        //根據起迄時間，取得停留時間
        public string GetStayTime(DateTime dtOutDate, DateTime LastInDate)
        {
            string StayTime = "";

            double StayMinutes = new TimeSpan(dtOutDate.Ticks - LastInDate.Ticks).TotalMinutes;

            //天
            double Day = StayMinutes / 1440;

            //時
            double Hour = (StayMinutes % 1440)/60;

            //分
            double Min = (StayMinutes % 1440) % 60;

            StayTime = string.Format("{0}天 {1}時 {2}分", Convert.ToInt32(Day), Convert.ToInt32(Hour), Convert.ToInt32(Min));

            return StayTime;
        }


        //根據起迄時間，取得停留日
        public string GetStayDay(DateTime dtEndDate, DateTime LastStartDate)
        {
            string StayTime = "";

            double StayMinutes = new TimeSpan(dtEndDate.Ticks - LastStartDate.Ticks).TotalMinutes;

            //天
            double Day = StayMinutes / 1440;

            StayTime = Convert.ToInt32(Day).ToString();

            return StayTime;
        }

        #endregion

        #region 購物車

        //加入一件商品到購物車
        public bool AddShopCart(string UseID, string PrdID, int Count)
        {
            bool Result = false;

            //先取得產品資訊-----------------------------------------------------------------
            string PrtID = "";
            int PrdTotalCount = 0;
            string PrdTotalPrice = "";
            DataTable dtPD = newDAL.GetProductByID(PrdID);
            if (dtPD.Rows.Count != 0)
            {
                PrtID = dtPD.Rows[0]["PrtID"].ToString();
                PrdTotalCount = Count;
                PrdTotalPrice += (Convert.ToDouble(dtPD.Rows[0]["PrdPrice"]) * Count).ToString();
            }

            //建立購物車主表-----------------------------------------------------------------
            DataTable dtSR = newDAL.GetShopCartRecordByUseID(UseID);
            //若沒有資料
            if (dtSR.Rows.Count == 0)
            {
                //取得ID
                string SpcID = newDAL.GetKeyValue("SPC");
                string SpcTotalCount = PrdTotalCount.ToString();
                string SpcTotalPrice = PrdTotalPrice;

                newDAL.InsertShopCartRecord(SpcID, UseID, SpcTotalCount, SpcTotalPrice, DateTime.Now);

                //建立購物車商品表==========================
                string SpdID = newDAL.GetKeyValue("SPD");
                newDAL.InsertShopCartDetail(SpdID, PrdID, PrtID, SpcTotalCount, PrdTotalPrice, SpcID);

                Result = true;
            }
            //若已經有資料
            else
            {
                //SpcID
                string SpcID = dtSR.Rows[0]["SpcID"].ToString();

                //建立購物車商品表==========================
                string SpdID = newDAL.GetKeyValue("SPD");
                newDAL.InsertShopCartDetail(SpdID, PrdID, PrtID, PrdTotalCount.ToString(), PrdTotalPrice, SpcID);

                //更新購物出主表資訊
                double SpcTotalCount = 0;
                double SpcTotalPrice = 0;
                DataTable dt = newDAL.GetShopCartDetailBySpcID(SpcID);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SpcTotalCount += Convert.ToDouble(dt.Rows[i]["PrdTotalCount"]);
                    SpcTotalPrice += Convert.ToDouble(dt.Rows[i]["PrdTotalPrice"]);
                }

                //更新主表
                newDAL.UpdateShopCartRecord(SpcID, SpcTotalCount.ToString(), SpcTotalPrice.ToString(), DateTime.Now);

                Result = true;
            }

            return Result;
        }

        #endregion

        #region 方法

        //根據帳號集合取得部門名稱
        public string GetDepID(DataTable dtUser, string EmpID)
        {
            string DepID = "";

            DataRow[] dtUserPart = dtUser.Select("EmpID = '" + EmpID + "' ");
            if (dtUserPart.Length != 0)
            {
                DepID = dtUserPart[0]["DepID"].ToString();
            }

            return DepID;
        }

        //根據帳號集合取得部門名稱
        public string GetDepName(DataTable dtUser, string EmpID)
        {
            string DepName = "";

            DataRow[] dtUserPart = dtUser.Select("EmpID = '" + EmpID + "' ");
            if (dtUserPart.Length != 0)
            {
                DepName = dtUserPart[0]["DepName"].ToString();
            }

            return DepName;
        }

        //根據帳號集合取得使用者名稱
        public string GetEmpCName(DataTable dtUser, string EmpID)
        {
            string EmpCName = "";

            DataRow[] dtUserPart = dtUser.Select("EmpID = '" + EmpID + "' ");
            if (dtUserPart.Length != 0)
            {
                EmpCName = dtUserPart[0]["EmpCName"].ToString();
            }

            return EmpCName;
        }

        #endregion

        #region GGB開發

        //轉換日期失效狀態
        public string ChangeOverdate(string StartDate, string EndDate)
        {
            string OverdateStatus = "";
            DateTime now = DateTime.Now;
            DateTime startDate = Convert.ToDateTime(StartDate);
            DateTime endDate = Convert.ToDateTime(EndDate);

            if (DateTime.Compare(startDate, now) < 0 && DateTime.Compare(endDate, now) > 0)
            {
                OverdateStatus = "日期有效";
            }
            else
            {
                OverdateStatus = "日期失效";
            }

            return OverdateStatus;
        }

        #endregion

    }
}
