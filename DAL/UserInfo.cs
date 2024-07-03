using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace DAL
{
    /// <summary>
    /// UserInfo 的摘要描述
    /// </summary>
    public class UserInfo
    {
        #region 宣告

        public bool CanAdd = false, CanDelete = false, CanUpdate = false, CanSelect = false, CanPrint = false;
        public bool CanView = false, CanSet = false, CanForce = false;
        public DataSet dsData;
        public int iControlPermission, iFormPermission;
        #endregion

        #region 屬性
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 使用者權限等級 0:無 1:新增 2:刪除 4:修改 8:查詢 16:列印
        /// </summary>
        public int FormPermission { get; set; }

        /// <summary>
        /// 控制權限等級 0:無 1:查詢 2:控制
        /// </summary>
        public int ControlPermission { get; set; }

        /// <summary>
        /// 使用者權限等
        /// </summary>
        public bool IsAuthen { get; set; }
        #endregion

        #region 方法

        public UserInfo()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        public void ParseFormPermission(int iPerm)
        {

            int iTemp;

            iTemp = iPerm;
            if ((iTemp &= 0x0001) == 0x0001)
                this.CanAdd = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0002) == 0x0002)
                this.CanDelete = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0004) == 0x0004)
                this.CanUpdate = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0008) == 0x0008)
                this.CanSelect = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0010) == 0x0010)
                this.CanPrint = true;
        }

        public void ParseControlPermission(int iPerm)
        {
            int iTemp;

            iTemp = iPerm;
            if ((iTemp &= 0x0001) == 0x0001)
                this.CanAdd = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0002) == 0x0002)
                this.CanDelete = true;
            iTemp = iPerm;
            if ((iTemp &= 0x0004) == 0x0004)
                this.CanForce = true;
        }

        #endregion
    }
}
