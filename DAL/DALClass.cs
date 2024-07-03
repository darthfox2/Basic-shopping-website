using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using BLL;

namespace DAL
{
    public class DALClass
    {
        ExceptionHandle clsEx = new ExceptionHandle();

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        SqlDataAdapter da;

        string ConnString = "";

        string errorMsg;

        public DALClass()
        {
            //使用應用程式組態檔連線方式,需加入參考[System.configuration.dll]
            ConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        #region "資料庫初始化"

        //資料庫初始化
        public void InitDB()
        {
            conn = null;
            conn = new SqlConnection(ConnString);
            conn.Open();

        }

        public void CloseDB()
        {
            conn.Close();
            conn.Dispose();
        }

        #endregion

        #region 取得KEY流水號

        /// <summary>
        /// 取得欄位唯一碼!!
        /// </summary>
        /// <param name="KeyID">取得Key之代碼，為3碼字串</param>
        /// <returns>回傳###000XXXXXXXXX，###為Key值，000為民國年，XXXXXXXXX為九碼流水號!!</returns>
        public string GetKeyValue(string KeyID)
        {
            try
            {
                string strKeyValue = "";

                //KyeID 加上三碼民國年碼
                string thisYear = (DateTime.Now.Year - 1911).ToString("000");
                KeyID = KeyID + thisYear;


                InitDB();

                string strSQL = "SELECT * from tbKeyGen Where keyID='" + KeyID + "'";
                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                da.Fill(DT);
                da.Dispose();

                int ivalue = 0;
                if (DT.Rows.Count > 0)
                {
                    ivalue = int.Parse(DT.Rows[0]["keyNum"].ToString()) + 1;
                    strSQL = "Update tbKeyGen Set keyNum =" + ivalue.ToString() + " Where keyID='" + KeyID + "'";
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    ivalue = 1;
                    strSQL = "Insert Into tbKeyGen (KeyID,keyNum) Values('" + KeyID + "'," + ivalue.ToString() + ")";
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                strKeyValue = KeyID + ivalue.ToString().PadLeft(9, '0');
                return strKeyValue;
            }
            catch
            {
                conn.Close();
                return "";

            }
        }

        //取得計畫編號
        public string GetKeyValueByType(int KeyInt)
        {
            try
            {
                string strKeyValue = "";
                string thisYear = (DateTime.Now.Year - 1911).ToString("000");
                string KeyID = thisYear + KeyInt.ToString("00");

                InitDB();

                string strSQL = "SELECT * from tbKeyGen Where keyID='" + KeyID + "'";
                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                da.Fill(DT);
                da.Dispose();

                int ivalue = 0;
                if (DT.Rows.Count > 0)
                {
                    ivalue = int.Parse(DT.Rows[0]["keyNum"].ToString()) + 1;
                    strSQL = "Update tbKeyGen Set keyNum =" + ivalue.ToString() + " Where keyID='" + KeyID + "'";
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    ivalue = 1;
                    strSQL = "Insert Into tbKeyGen (KeyID,keyNum) Values('" + KeyID + "'," + ivalue.ToString() + ")";
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                strKeyValue = KeyID + ivalue.ToString().PadLeft(7, '0');
                return strKeyValue;
            }
            catch
            {
                conn.Close();
                return "";

            }
        }

        #endregion

        #region 帳號權限處理

        #region 帳號

        //驗證帳密
        public UserInfo Authen(string strID, string strPwd)
        {
            UserInfo clsUser = new UserInfo();
            if (AuthenUser(strID, strPwd) == true)
            {
                clsUser = GetUserInfo(strID);
                clsUser.UserID = strID;
                clsUser.iControlPermission = 15;
                clsUser.iFormPermission = 15;
                clsUser.IsAuthen = true;
            }
            else
            {
                clsUser.UserID = strID;
                clsUser.IsAuthen = false;
            }

            return clsUser;
        }

        //驗證帳密
        public UserInfo Authen(string strID, string strPwd, string strIP, string strPageID)
        {
            UserInfo clsA = this.Authen(strID, strPwd);
            if (clsA.IsAuthen == true)
            {
                clsA.iFormPermission = GetPagePermission(strPageID, strID);
                //clsEvt.InsertData(6, strID, "登入成功", DateTime.Now, strIP, "");
            }
            else
            {
                //clsEvt.InsertData(6, strID, "登入失敗", DateTime.Now, strIP, "");
            }
            return clsA;
        }

        //驗證帳密
        public bool AuthenUser(string strID, string strPwd)
        {

            string selectCmd = "Select * from tbUsers where useid='" + strID + "'";
            string strDBPwd;
            bool bReturn = false;
            try
            {

                InitDB();

                da = new SqlDataAdapter(selectCmd, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    Security clsKey = new Security();
                    strDBPwd = DT.Rows[0]["UsePassword"].ToString().Trim();
                    strDBPwd = clsKey.Decrypt(strDBPwd);
                    if (strPwd == strDBPwd)
                        bReturn = true;
                    else
                        bReturn = false;
                }
                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return bReturn;
            }


            return bReturn;
        }

        //取得使用者資訊
        public UserInfo GetUserInfo(string strID)
        {
            UserInfo clsUser = new UserInfo();

            clsUser.dsData = GetAllPermission(strID);

            return clsUser;
        }

        //取得權限
        public DataSet GetAllPermission(string strUserID)
        {
            DataSet dsReturn = new DataSet();
            string selectCmd = "SELECT         dbo.tbRoles.RolID AS  SID, dbo.tbPermRole.RUPermission AS Permission,dbo.tbPermission.* ";
            selectCmd += "FROM             dbo.tbRoleUser INNER JOIN ";
            selectCmd += "dbo.tbRoles ON dbo.tbRoleUser.RolID = dbo.tbRoles.RolID INNER JOIN ";
            selectCmd += "dbo.tbUsers ON dbo.tbRoleUser.UseID = dbo.tbUsers.UseID INNER JOIN ";
            selectCmd += "dbo.tbPermRole ON dbo.tbRoles.RolID = dbo.tbPermRole.RolID INNER JOIN ";
            selectCmd += "dbo.tbPermission ON dbo.tbPermRole.PerId = dbo.tbPermission.PerId ";
            selectCmd += "WHERE (dbo.tbUsers.UseID ='" + strUserID + "' AND dbo.tbUsers.UseEnable=1 AND  dbo.tbRoles.RolEnable=1)";

            try
            {
                InitDB();

                da = new SqlDataAdapter(selectCmd, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);

                dsReturn.Tables.Add(DT);

                da.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dsReturn;
            }

            return dsReturn;
        }

        //取得所有使用者
        public DataTable GetUserList()
        {
            string strSQL = "SELECT dbo.tbUsers.UseID, dbo.tbUsers.UseName, dbo.tbRoles.RolName, dbo.tbUsers.UseEnable ";
            strSQL += "FROM dbo.tbRoles INNER JOIN dbo.tbRoleUser ON dbo.tbRoles.RolID = dbo.tbRoleUser.RolID INNER JOIN dbo.tbUsers ON dbo.tbRoleUser.UseID = dbo.tbUsers.UseID ";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(strSQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                return DT;
            }
        }

        //根據ID取得使用者資訊(FOX)
        public DataTable GetUserByID(string UseID)
        {
            string strSQL = "SELECT * ";
            strSQL += "FROM dbo.tbRoles INNER JOIN dbo.tbRoleUser ON dbo.tbRoles.RolID = dbo.tbRoleUser.RolID INNER JOIN dbo.tbUsers ON dbo.tbRoleUser.UseID = dbo.tbUsers.UseID ";
            strSQL += "WHERE dbo.tbUsers.UseID = '" + UseID + "' ";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(strSQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                return DT;
            }
        }

        //根據UseID及RolID取得使用者資訊(FOX)
        public DataTable GetUserByUseIDAndRolID(string UseID, string RolID)
        {
            string strSQL = "SELECT dbo.tbUsers.UseID, dbo.tbUsers.UseName, dbo.tbUsers.UsePassword, dbo.tbRoles.RolID, dbo.tbRoles.RolName, dbo.tbUsers.UseEnable ";
            strSQL += "FROM dbo.tbRoles INNER JOIN dbo.tbRoleUser ON dbo.tbRoles.RolID = dbo.tbRoleUser.RolID INNER JOIN dbo.tbUsers ON dbo.tbRoleUser.UseID = dbo.tbUsers.UseID ";
            strSQL += "WHERE dbo.tbRoles.RolID = '" + RolID + "' ";
            if (UseID != null)
            {
                strSQL += "AND dbo.tbUsers.UseID = '" + UseID + "' ";
            }

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(strSQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                return DT;
            }
        }

        //新增使用者(FOX)
        public int AddUser(string UseID, string UseName, string UsePassword, int UseEnable, string UseCountry, string UseZipCode, string UseAddr, string UseCell, string UseTel, string UseFax, string UseEmail, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                string strSQL = "SELECT * from tbUsers WHERE UseID='" + UseID + "'";

                InitDB();

                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);
                da.Dispose();

                if (DT.Rows.Count > 0)
                    return 2;

                Security clsKey = new Security();
                string Pass = clsKey.Encrypt(UsePassword);

                strSQL = "INSERT INTO tbUsers (UseID, UseName, UsePassword, UseEnable, UseCountry, UseZipCode, UseAddr, UseCell, UseTel, UseFax, UseEmail, LastUpdateUser, LastUpdateDate) VALUES(N'" + UseID + "',N'" + UseName + "',N'" + Pass + "',N'" + UseEnable + "',N'" + UseCountry + "',N'" + UseZipCode + "',N'" + UseAddr + "',N'" + UseCell + "',N'" + UseTel + "',N'" + UseFax + "',N'" + UseEmail + "',N'" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "')";
                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;

            }
        }

        //新增使用者帶角色(FOX)
        public int AddUser(string UseID, string UseName, string UsePassword, int UseEnable, string RolID, string UseCountry, string UseZipCode, string UseAddr, string UseCell, string UseTel, string UseFax, string UseEmail, string LastUpdateUser, DateTime LastUpdateDate)
        {

            int ret = AddUser(UseID, UseName, UsePassword, UseEnable, UseCountry, UseZipCode, UseAddr, UseCell, UseTel, UseFax, UseEmail, LastUpdateUser, LastUpdateDate);

            if (ret > 0)
            {
                try
                {
                    string strSQL = "SELECT * from tbRoleUser WHERE UseID='" + UseID + "' and RolID='" + RolID + "'";

                    InitDB();

                    da = new SqlDataAdapter(strSQL, conn);
                    DataTable DT = new DataTable();
                    DT.TableName = "newTable";
                    da.Fill(DT);
                    da.Dispose();

                    if (DT.Rows.Count > 0)
                        return 2;

                    strSQL = "INSERT INTO tbRoleUser (UseID,RolID) VALUES('" + UseID + "','" + RolID + "')";
                    cmd = new SqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }

            return 0;
        }

        //更新使用者(FOX)
        public bool UpdateUser(string UseID, string UseName, int UseEnable, string RolID, string UseCountry, string UseZipCode, string UseAddr, string UseCell, string UseTel, string UseFax, string UseEmail, string LastUpdateUser, DateTime LastUpdateDate)
        {
            if (UpadteRoleUsers(UseID, RolID))
            {
                Security clsKey = new Security();

                InitDB();

                string updateCmd;

                updateCmd = "UPDATE " + "tbUsers" + " SET UseName=N'" + UseName + "',UseEnable=N'" + UseEnable + "',UseCountry=N'" + UseCountry + "',UseZipCode=N'" + UseZipCode + "',UseAddr=N'" + UseAddr + "',UseCell=N'" + UseCell + "',UseTel=N'" + UseTel + "',UseFax=N'" + UseFax + "',UseEmail=N'" + UseEmail + "',LastUpdateUser=N'" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
                updateCmd = updateCmd + " WHERE UseID='" + UseID + "'";
                try
                {
                    cmd = new SqlCommand(updateCmd, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    conn.Close();
                    return false;

                }
            }
            return false;

        }

        //更新使用者密碼(FOX)
        public bool UpdatePassword(string UseID, string UsePassword)
        {
            Security clsKey = new Security();
            string Pass = clsKey.Encrypt(UsePassword);

            InitDB();

            string updateCmd;

            updateCmd = "UPDATE " + "tbUsers" + " SET UsePassword='" + Pass + "' ";
            updateCmd = updateCmd + " WHERE UseID='" + UseID + "'";
            try
            {
                cmd = new SqlCommand(updateCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        // 刪除使用者
        public bool DelUser(string UseID)
        {

            InitDB();

            string SQLCmd;

            SQLCmd = "DELETE FROM tbUsers  WHERE UseID='" + UseID + "'";

            try
            {
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                SQLCmd = "DELETE FROM tbRoleUser  WHERE UseID='" + UseID + "'";
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();


                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 權限

        //取得頁面權限
        public int GetPagePermission(string strPageID, string strUserID)
        {
            int iResult;
            iResult = GetFormPermissionCode(strUserID, strPageID);
            return iResult;
        }

        // 取得某個Form的權限
        public int GetFormPermissionCode(string strUserID, string strPermID)
        {
            string selectCmd = "SELECT dbo.tbRoles.RolID AS  SID, dbo.tbPermRole.RUPermission AS Permission ";
            selectCmd += "FROM dbo.tbRoleUser INNER JOIN ";
            selectCmd += "dbo.tbRoles ON dbo.tbRoleUser.RolID = dbo.tbRoles.RolID INNER JOIN ";
            selectCmd += "dbo.tbUsers ON dbo.tbRoleUser.UseID = dbo.tbUsers.UseID INNER JOIN ";
            selectCmd += "dbo.tbPermRole ON dbo.tbRoles.RolID = dbo.tbPermRole.RolID INNER JOIN ";
            selectCmd += "dbo.tbPermission ON dbo.tbPermRole.PerId = dbo.tbPermission.PerId ";
            selectCmd += "WHERE (dbo.tbUsers.UseID ='" + strUserID + "' and dbo.tbPermission.PerId='" + strPermID + "' AND dbo.tbUsers.UseEnable=1 AND  dbo.tbRoles.RolEnable=1)";


            int val = 0;
            int val1 = 0;
            string sid = "";

            try
            {
                InitDB();

                da = new SqlDataAdapter(selectCmd, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);

                if (DT.Rows.Count > 0)
                {


                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        sid = DT.Rows[i]["SID"].ToString();
                        val = int.Parse(DT.Rows[i]["Permission"].ToString());

                        val1 = val | val1;
                    }

                }
                da.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return val1;
            }

            return val1;
        }

        //取得所有權限(FOX)
        public DataTable GetPermission()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbPermission ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得權限(FOX)
        public DataTable GetPermByID(string PerID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbPermission ";
            sqlCmd += "WHERE PerID='" + PerID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據RolID取得Role所設定的權限
        public DataTable GetPermRoleByID(string RolID)
        {
            string strSQL = "SELECT * ";
            strSQL += "FROM dbo.tbPermRole ";
            strSQL += "WHERE RolID = '" + RolID + "' ";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(strSQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                return DT;
            }
        }

        //增加權限清單
        public int AddPermission(string PerId, string PerName, string PerPage)
        {
            try
            {
                string strSQL = "SELECT * from tbPermission WHERE PerId='" + PerId + "'";

                InitDB();

                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);
                da.Dispose();

                if (DT.Rows.Count > 0)
                    return 2;


                strSQL = "INSERT INTO tbPermission (PerId,PerName,PerPage) VALUES('" + PerId + "',N'" + PerName + "','" + PerPage + "')";
                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;

            }
        }

        //增加角色權限(FOX)
        public int AddPermRole(string RolID, string PerId, int RUPermission)
        {
            try
            {
                string strSQL = "SELECT * from tbPermRole WHERE RolID='" + RolID + "' and PerId='" + PerId + "'";

                InitDB();

                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);
                da.Dispose();

                if (DT.Rows.Count > 0)
                    return 2;


                strSQL = "INSERT INTO tbPermRole (RolID,PerId,RUPermission) VALUES('" + RolID + "','" + PerId + "'," + RUPermission.ToString() + ")";
                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;

            }
        }

        //更新權限(FOX)
        public bool UpdatePerm(string PerId, string PerName, string PerPage)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbPermission SET PerName=N'" + PerName + "',PerPage='" + PerPage + "'";
            sqlCmd += " WHERE PerId='" + PerId + "'";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除權限(FOX)
        public bool DelPerm(string PerId)
        {

            InitDB();

            string SQLCmd;

            SQLCmd = "DELETE FROM tbPermission  WHERE PerId='" + PerId + "'";

            try
            {
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                SQLCmd = "DELETE FROM tbPermRole  WHERE PerId='" + PerId + "'";
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        // 刪除角色權限(FOX)
        public bool DelPermRole(string RolID, string PerId)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbPermRole WHERE RolID='" + RolID + "' AND PerId='" + PerId + "'";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 角色

        //取得所有角色(FOX)
        public DataTable GetRoles()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbRoles ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據是否啟用取得所有角色(1=開啟;0=關閉)(FOX)
        public DataTable GetRolesByIsEnable(int IsEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbRoles ";
            sqlCmd += "WHERE RolEnable= '" + IsEnable + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得角色資訊(FOX)
        public DataTable GetRoleByID(string RolID)
        {
            string strSQL = "SELECT * ";
            strSQL += "FROM dbo.tbRoles ";
            strSQL += "WHERE RolID = '" + RolID + "' ";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(strSQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                return DT;
            }
        }

        //增加角色(FOX)
        public int AddRoles(string RolID, string RolName, int RolEnable)
        {
            try
            {
                string strSQL = "SELECT * from tbRoles WHERE RolID='" + RolID + "'";

                InitDB();

                da = new SqlDataAdapter(strSQL, conn);
                DataTable DT = new DataTable();
                DT.TableName = "newTable";
                da.Fill(DT);
                da.Dispose();

                if (DT.Rows.Count > 0)
                    return 2;


                strSQL = "INSERT INTO tbRoles (RolID,RolName,RolEnable) VALUES('" + RolID + "',N'" + RolName + "','" + RolEnable + "')";
                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;

            }


        }

        //更新帳號角色清單(FOX)
        public bool UpadteRoleUsers(string UseID, string RolID)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbRoleUser SET RolID='" + RolID + "'";
            sqlCmd += " WHERE UseID='" + UseID + "'";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新角色
        public bool UpdateRoles(string RolID, string RolName, int RolEnable)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbRoles SET RolName=N'" + RolName + "',RolEnable='" + RolEnable + "'";
            sqlCmd += " WHERE RolID='" + RolID + "'";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        // 刪除角色
        public bool DelRoles(string RolID)
        {

            InitDB();

            string SQLCmd;

            SQLCmd = "DELETE FROM tbRoles  WHERE RolID='" + RolID + "'";

            try
            {
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                SQLCmd = "DELETE FROM tbRoleUser  WHERE RolID='" + RolID + "'";
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                SQLCmd = "DELETE FROM tbPermRole  WHERE RolID='" + RolID + "'";
                cmd = new SqlCommand(SQLCmd, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #endregion

        #region 國家地區

        //取得所有國家地區
        public DataTable GetCountry()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbCountry ";
            sqlCmd += "ORDER BY CunSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 語系

        //取得所有語系
        public DataTable GetLanguage()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbLanguage ";
            sqlCmd += "ORDER BY LgeSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 留言管理

        //取得所有留言（日期排序）
        public DataTable GetMessage()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbMessage ";
            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據訂購日期取得所有留言（日期排序）
        public DataTable GetMessageByDateRang(DateTime dtStartSet, DateTime dtEndSet, string LgeID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbMessage ";
            sqlCmd += "WHERE CreateDate >= '" + TransferStartDateTime(dtStartSet) + "' AND CreateDate <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "' ";//範圍必須包含最後一筆
            if (LgeID != "-1")
            {
                sqlCmd += "AND LgeID='" + LgeID + "' ";
            }
            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得留言
        public DataTable GetMessageByID(string MsgID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbMessage ";
            sqlCmd += "WHERE MsgID='" + MsgID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增留言
        public int InsertMessage(string MsgID, string FacID, string LgeID, string MsgUserName, string MsgUserEmail, string MsgSubject, string MsgContent, string MsgDesc, string CreateUser, DateTime CreateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbMessage (MsgID, FacID, LgeID , MsgUserName, MsgUserEmail, MsgSubject, MsgContent, MsgDesc, CreateUser, CreateDate ) ";
                sqlCmd += "VALUES('" + MsgID + "','" + FacID + "','" + LgeID + "',N'" + MsgUserName + "',N'" + MsgUserEmail + "',N'" + MsgSubject + "',N'" + MsgContent + "',N'" + MsgDesc + "',N'" + CreateUser + "','" + TransferDateTime(CreateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //刪除留言
        public bool DeleteMessage(string MsgID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbMessage WHERE MsgID='" + MsgID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 商品類別

        //取得所有商品類別
        public DataTable GetProductType()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProductType ";
            sqlCmd += "ORDER BY PrtSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得商品類別
        public DataTable GetProductTypeByID(string PrtID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProductType ";
            sqlCmd += "WHERE PrtID='" + PrtID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 商品管理

        //根據ID取得商品價格
        public Double GetProductPriceByID(string PrdID, string LgeID)
        {
            Double Price = 0;

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProduct ";
            sqlCmd += "WHERE PrdID='" + PrdID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return Price;
            }

            if (LgeID == "CT")
            {
                Price = Convert.ToDouble(dt.Rows[0]["PrdPrice_CT"].ToString());
            }
            else if (LgeID == "CS")
            {
                Price = Convert.ToDouble(dt.Rows[0]["PrdPrice_CS"].ToString());
            }

            return Price;
        }

        //取得所有商品
        public DataTable GetProduct()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProduct ";
            //特殊過濾
            sqlCmd += "WHERE PrtID !='99999' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得商品
        public DataTable GetProductByID(string PrdID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProduct ";
            sqlCmd += "WHERE PrdID='" + PrdID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據商品類別取得商品
        public DataTable GetProductByPrtID(string PrtID, string RemovePrdID, string PrdEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbProduct ";
            sqlCmd += "WHERE PrtID !='99999' ";
            if (PrtID != "-1")
            {
                sqlCmd += "AND PrtID='" + PrtID + "' ";
            }
            if (RemovePrdID != "-1")
            {
                sqlCmd += "AND PrdID!='" + RemovePrdID + "' ";
            }
            if (PrdEnable != "-1")
            {
                sqlCmd += "AND PrdEnable='" + PrdEnable + "' ";
            }
            sqlCmd += "ORDER BY LastUpdateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增商品
        public int InsertProduct(string PrdID, string PrdName, string PrdDesc, string PrdPrice, string PrdSalePrice, string PrdContent, string PrdEnable, string PrdSort, string PrdUrl, string PrtID, string LastUpdateUser, DateTime LastUpdateDate, DateTime CreateDate, string PrdViews)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbProduct (PrdID, PrdName, PrdDesc, PrdPrice, PrdSalePrice, PrdContent, PrdEnable, PrdSort, PrdUrl, PrtID, LastUpdateUser, LastUpdateDate, CreateDate, PrdViews ) ";
                sqlCmd += "VALUES('" + PrdID + "',N'" + PrdName + "',N'" + PrdDesc + "','" + PrdPrice + "','" + PrdSalePrice + "',N'" + PrdContent + "','" + PrdEnable + "','" + PrdSort + "','" + PrdUrl + "','" + PrtID + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "','" + TransferDateTime(CreateDate) + "','" + PrdViews + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新商品
        public bool UpdateProduct(string PrdID, string PrdName, string PrdDesc, string PrdPrice, string PrdSalePrice, string PrdContent, string PrdEnable, string PrdSort, string PrdUrl, string PrtID, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbProduct SET PrdName=N'" + PrdName + "',PrdDesc=N'" + PrdDesc + "',PrdPrice='" + PrdPrice + "',PrdSalePrice='" + PrdSalePrice + "',PrdContent=N'" + PrdContent + "',PrdEnable='" + PrdEnable + "',PrdSort='" + PrdSort + "',PrdUrl='" + PrdUrl + "',PrtID='" + PrtID + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE PrdID='" + PrdID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                errorMsg = ex.Message;
                conn.Close();
                return false;
            }
        }

        //更新商品狀態
        public bool UpdateProductWithEnable(string PrdID, string PrdEnable, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbProduct SET PrdEnable='" + PrdEnable + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE PrdID='" + PrdID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新商品瀏覽數
        public bool UpdateProductWithViews(string PrdID, string PrdViews)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbProduct SET PrdViews='" + PrdViews + "' ";
            sqlCmd += " WHERE PrdID='" + PrdID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除商品
        public bool DeleteProduct(string PrdID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbProduct WHERE PrdID='" + PrdID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 訂單狀態

        //訂單狀態(OdrStatus)
        //"NO" = "訂單未完成"
        //"RD" = "訂單已完成"
        //"CP" = "完成付款"
        //"SI" = "出貨中"
        //"CS" = "完成出貨"
        //"OK" = "訂單終結"
        //"DL" = "訂單刪除"

        //取得所有訂單狀態
        public DataTable GetORStatus()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbORStatus ";
            sqlCmd += "ORDER BY OrsSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 訂單管理-主表

        //取得所有訂單（日期排序）(OdrStatus=OK)
        public DataTable GetOrderRecord()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            //sqlCmd += "WHERE OdrStatus='OK' ";
            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得訂單(Step1暫存階段)
        public DataTable GetS1OrderRecordByID(string OdrID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "WHERE OdrID='" + OdrID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得訂單
        public DataTable GetOrderRecordByID(string OdrID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "inner Join dbo.tbOrderUser ";//訂購人
            sqlCmd += "ON dbo.tbOrderRecord.OduID = dbo.tbOrderUser.OduID ";
            sqlCmd += "inner Join dbo.tbOrderFare ";//運費
            sqlCmd += "ON dbo.tbOrderRecord.OdfID = dbo.tbOrderFare.OdfID ";
            sqlCmd += "WHERE tbOrderRecord.OdrID='" + OdrID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得訂單
        public DataTable GetOrderRecordByIDWithS1(string OdrID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "WHERE tbOrderRecord.OdrID='" + OdrID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據UseID取得所有訂單（日期排序）
        public DataTable GetOrderRecordByUseID(string UseID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "inner Join dbo.tbUsers ";//使用者帳號
            sqlCmd += "ON dbo.tbOrderRecord.UseID = dbo.tbUsers.UseID ";
            sqlCmd += "WHERE tbOrderRecord.UseID='" + UseID + "' ";
            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據訂購日期取得所有訂單（日期排序）(OdrStatus=OK)
        public DataTable GetOrderRecordByDateRang(DateTime dtStartSet, DateTime dtEndSet, string OdrStatus)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "inner Join dbo.tbUsers ";//使用者帳號
            sqlCmd += "ON dbo.tbOrderRecord.UseID = dbo.tbUsers.UseID ";
            sqlCmd += "WHERE CreateDate >= '" + TransferStartDateTime(dtStartSet) + "' AND CreateDate <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "' ";//範圍必須包含最後一筆
            //sqlCmd += "AND OdrStatus='OK' ";
            if (OdrStatus != "-1")
            {
                sqlCmd += "AND OdrStatus = '" + OdrStatus + "' ";
            }

            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增訂單
        public int InsertOrderRecord(string OdrID, string UseID, string OdrTotalCount, string OdrTotalPrice, string OdrStatus, string OdrDesc, string OduID, string OdfID, DateTime CreateDate, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbOrderRecord (OdrID, UseID, OdrTotalCount, OdrTotalPrice, OdrStatus, OdrDesc, OduID, OdfID, CreateDate, LastUpdateUser, LastUpdateDate ) ";
                sqlCmd += "VALUES('" + OdrID + "','" + UseID + "','" + OdrTotalCount + "','" + OdrTotalPrice + "','" + OdrStatus + "',N'" + OdrDesc + "','" + OduID + "','" + OdfID + "','" + TransferDateTime(CreateDate) + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新訂單狀態
        public bool UpdateOrderRecordStatus(string OdrID, string OdrStatus, string OdrFeedBack, DateTime OdrFeedBackDate, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderRecord SET OdrStatus='" + OdrStatus + "',OdrFeedBack=N'" + OdrFeedBack + "',OdrFeedBackDate='" + TransferDateTime(OdrFeedBackDate) + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新訂單狀態(完成出貨)
        public bool UpdateOrderRecordStatusWithSendOK(string OdrID, string OdrStatus, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderRecord SET OdrStatus='" + OdrStatus + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新訂單價格(Step1暫存階段)
        public bool UpdateS1OrderRecordPrice(string OdrID, string OdrTotalPrice, string OdfID, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderRecord SET OdrTotalPrice='" + OdrTotalPrice + "',OdfID='" + OdfID + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新訂單備註(Step2暫存階段)
        public bool UpdateS2OrderRecordDesc(string OdrID, string OdrStatus, string OdrDesc, string OduID, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderRecord SET OdrDesc=N'" + OdrDesc + "',OdrStatus='" + OdrStatus + "',OduID='" + OduID + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新訂單
        public bool UpdateOrderRecord(string OdrID, string UseID, string OdrTotalCount, string OdrTotalPrice, string OdrStatus, string OdrDesc, string OduID, string OdfID, DateTime CreateDate, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderRecord SET UseID='" + UseID + "',OdrTotalCount='" + OdrTotalCount + "',OdrTotalPrice='" + OdrTotalPrice + "',OdrStatus='" + OdrStatus + "',OdrDesc=N'" + OdrDesc + "',OduID='" + OduID + "',OdfID='" + OdfID + "',CreateDate='" + TransferDateTime(CreateDate) + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除訂單
        public bool DeleteOrderRecord(string OdrID)
        {
            //先取得訂單資訊
            string OduID = "";

            DataTable dt = GetOrderRecordByID(OdrID);
            if (dt.Rows.Count != 0)
            {
                OduID = dt.Rows[0]["OduID"].ToString();
            }

            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbOrderRecord WHERE OdrID='" + OdrID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                //刪除商品資訊
                sqlCmd = "DELETE FROM dbo.tbOrderDetail WHERE OdrID='" + OdrID + "' ";
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                //刪除收件人
                sqlCmd = "DELETE FROM dbo.tbOrderUser WHERE OduID='" + OduID + "' ";
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 訂單管理-商品資訊

        //取得所有商品資訊（PrtID排序）
        public DataTable GetOrderDetail()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderDetail ";
            sqlCmd += "ORDER BY PrtID ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得商品資訊
        public DataTable GetOrderDetailByID(string OddID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderDetail ";
            sqlCmd += "inner Join dbo.tbProduct ";//商品內容
            sqlCmd += "ON dbo.tbOrderDetail.PrdID = dbo.tbProduct.PrdID ";
            sqlCmd += "WHERE OddID='" + OddID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據OdrID取得商品資訊
        public DataTable GetOrderDetailByOdrID(string OdrID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderDetail ";
            sqlCmd += "inner Join dbo.tbProduct ";//商品內容
            sqlCmd += "ON dbo.tbOrderDetail.PrdID = dbo.tbProduct.PrdID ";
            sqlCmd += "WHERE OdrID='" + OdrID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增商品資訊
        public int InsertOrderDetail(string OddID, string PrdID, string PrdPrice, string PrtID, string PrdTotalCount, string PrdTotalPrice, string OdrID)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbOrderDetail (OddID, PrdID, PrdPrice, PrtID, PrdTotalCount, PrdTotalPrice, OdrID ) ";
                sqlCmd += "VALUES('" + OddID + "','" + PrdID + "','" + PrdPrice + "','" + PrtID + "','" + PrdTotalCount + "','" + PrdTotalPrice + "','" + OdrID + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //刪除商品資訊
        public bool DeleteOrderDetail(string OddID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbOrderDetail WHERE OddID='" + OddID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 訂單管理-收件人

        //根據ID取得收件人
        public DataTable GetOrderUserByID(string OduID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderUser ";
            sqlCmd += "WHERE OduID='" + OduID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增收件人
        public int InsertOrderUser(string OduID, string UseName, string UseCountry, string UseZipCode, string UseAddr, string UseCell, string UseTel, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbOrderUser (OduID, UseName, UseCountry, UseZipCode, UseAddr, UseCell, UseTel, LastUpdateUser, LastUpdateDate ) ";
                sqlCmd += "VALUES('" + OduID + "',N'" + UseName + "',N'" + UseCountry + "',N'" + UseZipCode + "',N'" + UseAddr + "',N'" + UseCell + "',N'" + UseTel + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新收件人
        public bool UpdateOrderUser(string OduID, string UseName, string UseCountry, string UseZipCode, string UseAddr, string UseCell, string UseTel, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderUser SET UseName=N'" + UseName + "',UseCountry=N'" + UseCountry + "',UseZipCode=N'" + UseZipCode + "',UseAddr=N'" + UseAddr + "',UseCell=N'" + UseCell + "',UseTel=N'" + UseTel + "',LastUpdateUser=N'" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OduID='" + OduID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除收件人
        public bool DeleteOrderUser(string OduID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbOrderUser WHERE OduID='" + OduID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 訂單管理-運費

        //根據是否啟用取得運費
        public DataTable GetOrderFareByEnable(string OdfEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderFare ";
            if (OdfEnable != "-1")
            {
                sqlCmd += "WHERE OdfEnable = '" + OdfEnable + "' ";
            }
            sqlCmd += "ORDER BY OdfSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得運費
        public DataTable GetOrderFareByID(string OdfID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbOrderFare ";
            sqlCmd += "WHERE OdfID='" + OdfID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增運費
        public int InsertOrderFare(string OdfID, string OdfName, string OdfDesc, string OdfPrice_CT, string OdfPrice_CS, string OdfEnable, string OdfSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbOrderFare (OdfID, OdfName, OdfDesc, OdfPrice_CT, OdfPrice_CS, OdfEnable, OdfSort, LastUpdateUser, LastUpdateDate ) ";
                sqlCmd += "VALUES('" + OdfID + "',N'" + OdfName + "',N'" + OdfDesc + "','" + OdfPrice_CT + "','" + OdfPrice_CS + "','" + OdfEnable + "','" + OdfSort + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新運費
        public bool UpdateOrderFare(string OdfID, string OdfName, string OdfDesc, string OdfPrice_CT, string OdfPrice_CS, string OdfEnable, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbOrderFare SET OdfName=N'" + OdfName + "',OdfDesc=N'" + OdfDesc + "',OdfPrice_CT='" + OdfPrice_CT + "',OdfPrice_CS='" + OdfPrice_CS + "',OdfEnable='" + OdfEnable + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OdfID='" + OdfID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除運費
        public bool DeleteOrderFare(string OdfID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbOrderFare WHERE OdfID='" + OdfID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 訂單管理-匯率

        //取得匯率
        public DataTable GetExchangeRate()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbExchangeRate ";
            sqlCmd += "ORDER BY OerSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得匯率
        public DataTable GetExchangeRateByID(string OerID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbExchangeRate ";
            sqlCmd += "WHERE OerID='" + OerID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增匯率
        public int InsertExchangeRate(string OerID, string OerName, string OerCurrency, string OerRate, string OerSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbExchangeRate (OerID, OerName, OerCurrency, OerRate, OerSort, LastUpdateUser, LastUpdateDate ) ";
                sqlCmd += "VALUES('" + OerID + "',N'" + OerName + "',N'" + OerCurrency + "','" + OerRate + "','" + OerSort + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新匯率
        public bool UpdateExchangeRate(string OerID, string OerName, string OerCurrency, string OerRate, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbExchangeRate SET OerName=N'" + OerName + "',OerCurrency=N'" + OerCurrency + "',OerRate='" + OerRate + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE OerID='" + OerID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除匯率
        public bool DeleteExchangeRate(string OerID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbExchangeRate WHERE OerID='" + OerID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 月報表查詢

        //根據日期取得月報表
        public DataTable GetRecordByDateRange(DateTime dtStartSet, DateTime dtEndSet)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT SUM(OdrTotalCount) AS OdrTotalCount, SUM(OdrTotalPrice) AS OdrTotalPrice ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "WHERE OdrStatus != 'NO' ";
            sqlCmd += "AND CreateDate >= '" + TransferStartDateTime(dtStartSet) + "' AND CreateDate <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "' ";//範圍必須包含最後一筆

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據日期及訂單狀態取得月報表
        public DataTable GetRecordByDateRangeAndStatus(DateTime dtStartSet, DateTime dtEndSet, string OdrStatus)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT COUNT(*) AS COUNT ";
            sqlCmd += "FROM dbo.tbOrderRecord ";
            sqlCmd += "WHERE CreateDate >= '" + TransferStartDateTime(dtStartSet) + "' AND CreateDate <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "' ";//範圍必須包含最後一筆
            if (OdrStatus != "-1")
            {
                sqlCmd += "AND OdrStatus = '" + OdrStatus + "' ";
            }

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 購物車-主表

        //根據UseID取得購物車
        public DataTable GetShopCartRecordByUseID(string UseID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbShopCartRecord ";
            sqlCmd += "inner join dbo.tbShopCartDetail ";
            sqlCmd += "on dbo.tbShopCartRecord.SpcID = dbo.tbShopCartDetail.SpcID ";
            sqlCmd += "WHERE UseID='" + UseID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得購物車
        public DataTable GetShopCartRecordByID(string SpcID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbShopCartRecord ";
            sqlCmd += "WHERE SpcID='" + SpcID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增購物車
        public int InsertShopCartRecord(string SpcID, string UseID, string SpcTotalCount, string SpcTotalPrice, DateTime CreateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbShopCartRecord (SpcID, UseID, SpcTotalCount, SpcTotalPrice, CreateDate ) ";
                sqlCmd += "VALUES('" + SpcID + "','" + UseID + "','" + SpcTotalCount + "','" + SpcTotalPrice + "','" + TransferDateTime(CreateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新購物車
        public bool UpdateShopCartRecord(string SpcID, string SpcTotalCount, string SpcTotalPrice, DateTime CreateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbShopCartRecord SET SpcTotalCount='" + SpcTotalCount + "',SpcTotalPrice'" + SpcTotalPrice + "',CreateDate='" + TransferDateTime(CreateDate) + "' ";
            sqlCmd += " WHERE SpcID='" + SpcID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除購物車
        public bool DeleteShopCartRecord(string SpcID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbShopCartRecord WHERE SpcID='" + SpcID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                //刪除商品資訊
                sqlCmd = "DELETE FROM dbo.tbShopCartDetail WHERE SpcID='" + SpcID + "' ";
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 購物車-商品資訊

        //根據ID取得商品資訊
        public DataTable GeShopCartDetailByID(string SpdID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbShopCartDetail ";
            sqlCmd += "inner Join dbo.tbProduct ";//商品內容
            sqlCmd += "ON dbo.tbShopCartDetail.PrdID = dbo.tbProduct.PrdID ";
            sqlCmd += "WHERE SpdID='" + SpdID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據OdrID取得商品資訊
        public DataTable GetShopCartDetailBySpcID(string SpcID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbShopCartDetail ";
            sqlCmd += "inner Join dbo.tbProduct ";//商品內容
            sqlCmd += "ON dbo.tbShopCartDetail.PrdID = dbo.tbProduct.PrdID ";
            sqlCmd += "WHERE SpcID='" + SpcID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增商品資訊
        public int InsertShopCartDetail(string SpdID, string PrdID, string PrtID, string PrdTotalCount, string PrdTotalPrice, string SpcID)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbShopCartDetail (SpdID, PrdID, PrtID, PrdTotalCount, PrdTotalPrice, SpcID ) ";
                sqlCmd += "VALUES('" + SpdID + "','" + PrdID + "','" + PrtID + "','" + PrdTotalCount + "','" + PrdTotalPrice + "','" + SpcID + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //刪除商品資訊
        public bool DeleteShopCartDetail(string SpdID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbShopCartDetail WHERE SpdID='" + SpdID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 新文稿類別

        //取得所有新文稿類別
        public DataTable GetArticleType()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticleType ";
            sqlCmd += "ORDER BY AttSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得新文稿類別
        public DataTable GetArticleTypeByID(string AttID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticleType ";
            sqlCmd += "WHERE AttID='" + AttID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增類型
        public int InsertArticleType(string AttID, string AttName, string AttDesc, string AttEnable, string AttSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbArticleType (AttID, AttName, AttDesc, AttEnable, AttSort, LastUpdateUser, LastUpdateDate) ";
                sqlCmd += "VALUES('" + AttID + "',N'" + AttName + "',N'" + AttDesc + "','" + AttEnable + "','" + AttSort + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新類型
        public bool UpdateArticleType(string AttID, string AttName, string AttDesc, string AttEnable, string AttSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticleType SET AttName=N'" + AttName + "',AttDesc=N'" + AttDesc + "',AttEnable='" + AttEnable + "',AttSort='" + AttSort + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE AttID='" + AttID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除類型
        public bool DeleteArticleType(string AttID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbArticleType WHERE AttID='" + AttID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新排序
        public bool ArticleTypeSort(string AttSort, int i)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticleType SET AttSort='" + i + "' ";
            sqlCmd += " WHERE AttSort='" + AttSort + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //上移
        public bool ArticleTypeSortUp(string AttSort)
        {
            InitDB();

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sql = " SELECT Top 2 [AttID],[AttSort] ";
            sql += " FROM dbo.tbArticleType";
            sql += " Where AttSort <=" + AttSort;
            sql += " Order by AttSort DESC";

            da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            da.Dispose();

            if (dt.Rows.Count >= 2) //防呆
            {
                string firstID = dt.Rows[0]["AttID"].ToString();
                string firstSort = dt.Rows[0]["AttSort"].ToString();

                string secondID = dt.Rows[1]["AttID"].ToString();
                string secondSort = dt.Rows[1]["AttSort"].ToString();

                string sqlCmd = "Update dbo.tbArticleType Set AttSort='" + secondSort + "'";
                sqlCmd += " Where AttID = '" + firstID + "'";
                sqlCmd += " Update dbo.tbArticleType Set AttSort='" + firstSort + "'";
                sqlCmd += " Where AttID = '" + secondID + "'";

                try
                {
                    cmd = new SqlCommand(sqlCmd, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    conn.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        //下移
        public bool ArticleTypeSortDown(string AttSort)
        {
            InitDB();

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sql = " SELECT Top 2 [AttID],[AttSort] ";
            sql += " FROM dbo.tbArticleType";
            sql += " Where AttSort >=" + AttSort;
            sql += " Order by AttSort ASC";

            da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            da.Dispose();

            if (dt.Rows.Count >= 2) //防呆
            {
                string firstID = dt.Rows[0]["AttID"].ToString();
                string firstSort = dt.Rows[0]["AttSort"].ToString();

                string secondID = dt.Rows[1]["AttID"].ToString();
                string secondSort = dt.Rows[1]["AttSort"].ToString();

                string sqlCmd = "Update dbo.tbArticleType Set AttSort='" + secondSort + "'";
                sqlCmd += " Where AttID = '" + firstID + "'";
                sqlCmd += " Update dbo.tbArticleType Set AttSort='" + firstSort + "'";
                sqlCmd += " Where AttID = '" + secondID + "'";

                try
                {
                    cmd = new SqlCommand(sqlCmd, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    conn.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 新文稿管理

        //取得所有新文稿
        public DataTable GetArticle()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticle ";
            //特殊過濾
            sqlCmd += "WHERE AttID !='99999' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得新文稿
        public DataTable GetArticleByID(string AtcID, string AtcEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticle ";
            sqlCmd += "WHERE AtcID='" + AtcID + "' ";
            if (AtcEnable != "-1")
            {
                sqlCmd += "AND AtcEnable='" + AtcEnable + "' ";
            }

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據類別取得新文稿
        public DataTable GetArticleByAttID(string AttID, string AtcEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticle ";
            sqlCmd += "INNER JOIN dbo.tbArticleType ";//類別資料
            sqlCmd += "ON dbo.tbArticleType.AttID = dbo.tbArticle.AttID ";
            //特殊過濾
            sqlCmd += "WHERE dbo.tbArticle.AttID !='99999' ";
            if (AttID != "-1")
            {
                sqlCmd += "AND dbo.tbArticle.AttID='" + AttID + "' ";
            }
            if (AtcEnable != "-1")
            {
                sqlCmd += "AND AtcEnable='" + AtcEnable + "' ";
            }

            sqlCmd += "ORDER BY dbo.tbArticle.LastUpdateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增新文稿
        public int InsertArticle(string AtcID, string AttID, string AtcName, string AtcDesc, string AtcBackColor, string AtcPicPath, string AtcStatus, string AtcEnable, string AtcSort, string AtcSetOD, DateTime AtcOpenDate, string AtcFBPicPath, DateTime CreateDate, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbArticle (AtcID, AttID, AtcName, AtcDesc, AtcBackColor, AtcPicPath, AtcStatus, AtcEnable, AtcSort, AtcSetOD, AtcOpenDate, AtcFBPicPath, CreateDate, LastUpdateUser, LastUpdateDate) ";
                sqlCmd += "VALUES('" + AtcID + "','" + AttID + "',N'" + AtcName + "',N'" + AtcDesc + "',N'" + AtcBackColor + "',N'" + AtcPicPath + "','" + AtcStatus + "','" + AtcEnable + "','" + AtcSort + "','" + AtcSetOD + "','" + TransferDateTime(AtcOpenDate) + "',N'" + AtcFBPicPath + "','" + TransferDateTime(CreateDate) + "',N'" + LastUpdateUser + "',N'" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新新文稿
        public bool UpdateArticle(string AtcID, string AttID, string AtcName, string AtcDesc, string AtcBackColor, string AtcPicPath, string AtcStatus, string AtcEnable, string AtcSort, string AtcSetOD, DateTime AtcOpenDate, string AtcFBPicPath, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticle SET AttID='" + AttID + "',AtcName=N'" + AtcName + "',AtcDesc=N'" + AtcDesc + "',AtcBackColor='" + AtcBackColor + "',AtcPicPath=N'" + AtcPicPath + "',AtcStatus='" + AtcStatus + "',AtcEnable='" + AtcEnable + "',AtcSort='" + AtcSort + "',AtcSetOD='" + AtcSetOD + "',AtcOpenDate='" + TransferDateTime(AtcOpenDate) + "',AtcFBPicPath='" + AtcFBPicPath + "',LastUpdateUser=N'" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE AtcID='" + AtcID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新新文稿啟用
        public bool UpdateArticleWithEnable(string AtcID, string AtcEnable, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticle SET AtcEnable='" + AtcEnable + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE AtcID='" + AtcID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除新文稿
        public bool DeleteArticle(string AtcID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbArticle WHERE AtcID='" + AtcID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                //同步刪除所有內容
                sqlCmd = "DELETE FROM dbo.tbArticleContent WHERE AtcID='" + AtcID + "'";
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 新文稿內容管理

        //根據ID取得新文稿內容
        public DataTable GetArticleContentByID(string AtnID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticleContent ";
            sqlCmd += "WHERE AtnID='" + AtnID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據AtcID取得所有新文稿內容
        public DataTable GetArticleContentByAtcID(string AtcID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticleContent ";
            sqlCmd += "INNER JOIN dbo.tbArticleClass ";//tbArticleClass
            sqlCmd += "ON dbo.tbArticleContent.AtsID = dbo.tbArticleClass.AtsID ";
            sqlCmd += "WHERE AtcID='" + AtcID + "' ";
            sqlCmd += "ORDER BY AtnSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增新文稿內容
        public int InsertArticleContent(string AtnID, string AtsID, string AtnSubject, string AtnText, string AtnPicPath, string AtnVideoPath, string AtnMp4Path, string LastUpdateUser, DateTime LastUpdateDate, string AtnSort, string AtcID)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbArticleContent (AtnID, AtsID, AtnSubject, AtnText, AtnPicPath, AtnVideoPath, AtnMp4Path, LastUpdateUser, LastUpdateDate, AtnSort, AtcID) ";
                sqlCmd += "VALUES('" + AtnID + "','" + AtsID + "',N'" + AtnSubject + "',N'" + AtnText + "',N'" + AtnPicPath + "',N'" + AtnVideoPath + "',N'" + AtnMp4Path + "',N'" + LastUpdateUser + "',N'" + TransferDateTime(LastUpdateDate) + "','" + AtnSort + "','" + AtcID + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新新文稿內容
        public bool UpdateArticleContent(string AtnID, string AtsID, string AtnSubject, string AtnText, string AtnPicPath, string AtnVideoPath, string AtnMp4Path, string LastUpdateUser, DateTime LastUpdateDate, string AtnSort, string AtcID)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticleContent SET AtsID='" + AtsID + "',AtnSubject=N'" + AtnSubject + "',AtnText=N'" + AtnText + "',AtnPicPath='" + AtnPicPath + "',AtnVideoPath=N'" + AtnVideoPath + "',AtnMp4Path=N'" + AtnMp4Path + "',LastUpdateUser=N'" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "',AtnSort='" + AtnSort + "',AtcID='" + AtcID + "' ";
            sqlCmd += " WHERE AtnID='" + AtnID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新新文稿內容排序
        public bool UpdateArticleContentWithSort(string AtnID, string AtnSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbArticleContent SET AtnSort='" + AtnSort + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE AtnID='" + AtnID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除新文稿內容
        public bool DeleteArticleContent(string AtnID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbArticleContent WHERE AtnID='" + AtnID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        #region 新文稿選項

        //取得所有新文稿選項
        public DataTable GetArticleClass()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbArticleClass ";
            sqlCmd += "ORDER BY AtsSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        #endregion

        #region 系統錯誤查詢

        // 程式中發生錯誤時，將錯誤資訊記錄於DB
        public void AddException(string ErrType, string ErrMsg)
        {
            // ErrMsg字串需做過濾處理
            ErrMsg = ErrMsg.Replace("'", "@");

            string strSQL = "INSERT INTO dbo.tbExceptionRecord (CreateTime, ExrTypeID, ExrMsg) VALUES (GETDATE(), '";
            strSQL += ErrType + "', N'" + ErrMsg + "')";

            try
            {
                InitDB();

                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                CloseDB();
            }
        }

        // 取得所有的錯誤訊息列表（日期排序）
        public DataTable GetExceptionList()
        {
            string querySQL = "SELECT dbo.tbExceptionRecord.CreateTime, dbo.tbExceptionRecord.ExrTypeID, dbo.tbExceptionType.ExtType, dbo.tbExceptionType.ExtDesc, dbo.tbExceptionRecord.ExrMsg";
            querySQL += " FROM dbo.tbExceptionRecord INNER JOIN dbo.tbExceptionType ON dbo.tbExceptionRecord.ExrTypeID = dbo.tbExceptionType.ExtTypeID";
            querySQL += " ORDER BY dbo.tbExceptionRecord.CreateTime DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 根據類別取得錯誤訊息（日期排序）
        public DataTable GetExceptionByType(string ErrType)
        {
            string querySQL = "SELECT dbo.tbExceptionRecord.CreateTime, dbo.tbExceptionRecord.ExrTypeID, dbo.tbExceptionType.ExtType, dbo.tbExceptionType.ExtDesc,  dbo.tbExceptionRecord.ExrMsg";
            querySQL += " FROM dbo.tbExceptionRecord INNER JOIN dbo.tbExceptionType ON dbo.tbExceptionRecord.ExrTypeID = dbo.tbExceptionType.ExtTypeID";
            querySQL += " WHERE dbo.tbExceptionType.ExtType LIKE N'" + ErrType + "'";
            querySQL += " ORDER BY dbo.tbExceptionRecord.CreateTime DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 根據發生日期取得錯誤資訊（日期排序）
        public DataTable GetExceptionByDateRang(DateTime dtStartSet, DateTime dtEndSet, string ErrType)
        {
            string querySQL = "SELECT dbo.tbExceptionRecord.CreateTime, dbo.tbExceptionRecord.ExrTypeID, dbo.tbExceptionType.ExtType, dbo.tbExceptionType.ExtDesc,  dbo.tbExceptionRecord.ExrMsg";
            querySQL += " FROM dbo.tbExceptionRecord INNER JOIN dbo.tbExceptionType ON dbo.tbExceptionRecord.ExrTypeID = dbo.tbExceptionType.ExtTypeID";
            querySQL += " WHERE dbo.tbExceptionRecord.CreateTime >= '" + TransferStartDateTime(dtStartSet) + "' AND dbo.tbExceptionRecord.CreateTime <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "'";

            if (ErrType != "-1")
            {
                querySQL += " AND dbo.tbExceptionType.ExtType LIKE N'" + ErrType + "' ";
            }
            querySQL += " ORDER BY dbo.tbExceptionRecord.CreateTime DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 取得錯誤類別
        public DataTable GetErrType()
        {
            string querySQL = "SELECT DISTINCT ExtType";
            querySQL += " FROM dbo.tbExceptionType";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        #endregion

        #region 系統事件查詢

        // 程式中發生事件時，將事件資訊記錄於DB
        public void AddEvent(string EveType, string EveMsg, string UseID)
        {
            string strSQL = "INSERT INTO dbo.tbEventRecord (CreateTime, EvrTypeID, EvrMsg, UseID ) VALUES (GETDATE(), '";
            strSQL += EveType + "', N'" + EveMsg + "', N'" + UseID + "')";

            try
            {
                InitDB();

                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                CloseDB();
            }
        }

        // 取得所有的事件訊息列表（日期排序）
        public DataTable GetEventList()
        {
            string querySQL = "SELECT dbo.tbEventRecord.CreateTime, dbo.tbEventRecord.EvrTypeID, dbo.tbEventType.EvtType, dbo.tbEventType.EvtDesc, dbo.tbEventRecord.EvrMsg";
            querySQL += " FROM dbo.tbEventRecord INNER JOIN dbo.tbEventType ON dbo.tbEventRecord.EvrTypeID = dbo.tbEventType.EvtTypeID";
            querySQL += " ORDER BY dbo.tbEventRecord.CreateTime DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 根據發生日期取得錯誤資訊（日期排序）
        public DataTable GetEventByDateRang(DateTime dtStartSet, DateTime dtEndSet, string EveType)
        {
            string querySQL = "SELECT dbo.tbEventRecord.CreateTime, dbo.tbEventRecord.EvrTypeID, dbo.tbEventType.EvtType, dbo.tbEventType.EvtDesc,  dbo.tbEventRecord.EvrMsg";
            querySQL += " FROM dbo.tbEventRecord INNER JOIN dbo.tbEventType ON dbo.tbEventRecord.EvrTypeID = dbo.tbEventType.EvtTypeID";
            querySQL += " WHERE dbo.tbEventRecord.CreateTime >= '" + TransferStartDateTime(dtStartSet) + "' AND dbo.tbEventRecord.CreateTime <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "'";

            if (EveType != "-1")
            {
                querySQL += " AND dbo.tbEventType.EvtType LIKE N'" + EveType + "' ";
            }
            querySQL += " ORDER BY dbo.tbEventRecord.CreateTime DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 取得事件類別
        public DataTable GetEveType()
        {
            string querySQL = "SELECT DISTINCT EvtType";
            querySQL += " FROM dbo.tbEventType";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        //根據事件類別及UseID取得最近10筆事件紀錄
        public DataTable GetEventListByTypeAndUseID(string EvtTypeID, string UseID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT TOP (10) * ";
            sqlCmd += "FROM dbo.tbEventRecord ";
            sqlCmd += "WHERE EvrTypeID='" + EvtTypeID + "' ";
            if (UseID != "-1")
            {
                sqlCmd += " AND UseID='" + UseID + "' ";
            }
            sqlCmd += "ORDER BY CreateTime DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據事件類別及UseID取得總數量
        public string GetEventCountByTypeAndUseID(string EvtTypeID, string UseID)
        {
            string Count = "";

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT COUNT(*) AS [LOGINCOUNT] ";
            sqlCmd += "FROM dbo.tbEventRecord ";
            sqlCmd += "WHERE EvrTypeID='" + EvtTypeID + "' ";
            if (UseID != "-1")
            {
                sqlCmd += " AND UseID='" + UseID + "' ";
            }

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
            }

            if (dt.Rows.Count != 0)
            {
                Count = dt.Rows[0]["LOGINCOUNT"].ToString();
            }

            return Count;
        }

        #endregion

        #region 大圖輪播管理

        //取得所有大圖輪播
        public DataTable GetPicRotate()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbPicRotate ";
            sqlCmd += "ORDER BY ProSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據是否啟用取得所有大圖輪播
        public DataTable GetPicRotateByEnable(string ProEnable)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbPicRotate ";
            sqlCmd += "WHERE ProEnable ='" + ProEnable + "' ";
            sqlCmd += "ORDER BY ProSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得大圖輪播
        public DataTable GetPicRotateByID(string ProID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbPicRotate ";
            sqlCmd += "WHERE ProID='" + ProID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增大圖輪播
        public int InsertPicRotate(string ProID, string ProName, string ProDesc, string ProPicPath, string ProEnable, string ProType, string ProSort)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbPicRotate (ProID, ProName, ProDesc, ProPicPath, ProEnable, ProType, ProSort ) ";
                sqlCmd += "VALUES('" + ProID + "',N'" + ProName + "',N'" + ProDesc + "','" + ProPicPath + "','" + ProEnable + "',N'" + ProType + "','" + ProSort + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新大圖輪播
        public bool UpdatePicRotate(string ProID, string ProName, string ProDesc, string ProPicPath, string ProEnable, string ProType, string ProSort)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbPicRotate SET ProName=N'" + ProName + "',ProDesc=N'" + ProDesc + "',ProPicPath='" + ProPicPath + "',ProEnable='" + ProEnable + "',ProType='" + ProType + "',ProSort='" + ProSort + "' ";
            sqlCmd += " WHERE ProID='" + ProID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                conn.Close();
                return false;
            }
        }

        //刪除大圖輪播
        public bool DeletePicRotate(string ProID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbPicRotate WHERE ProID='" + ProID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                conn.Close();
                return false;
            }
        }

        #endregion

        #region 消息類別管理

        //取得所有公告類別
        public DataTable GetNewsType()
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbNewsType ";
            sqlCmd += "ORDER BY NwtSort ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據ID取得公告類別
        public DataTable GetNewsTypeByID(string NwtID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbNewsType ";
            sqlCmd += "WHERE NwtTypeID='" + NwtID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增類別
        public int InsertNewsType(string NwtID, string NwtName, string NwtDesc, string NwtEnable, string NwtSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbNewsType (NwtTypeID, NwtName, NwtDesc, NwtEnable, NwtSort, LastUpdateUser, LastUpdateDate) ";
                sqlCmd += "VALUES('" + NwtID + "',N'" + NwtName + "',N'" + NwtDesc + "','" + NwtEnable + "','" + NwtSort + "','" + LastUpdateUser + "','" + TransferDateTime(LastUpdateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //更新類別
        public bool UpdateNewsType(string NwtID, string NwtName, string NwtDesc, string NwtEnable, string NwtSort, string LastUpdateUser, DateTime LastUpdateDate)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbNewsType SET NwtName=N'" + NwtName + "',NwtDesc=N'" + NwtDesc + "',NwtEnable='" + NwtEnable + "',NwtSort='" + NwtSort + "',LastUpdateUser='" + LastUpdateUser + "',LastUpdateDate='" + TransferDateTime(LastUpdateDate) + "' ";
            sqlCmd += " WHERE NwtTypeID='" + NwtID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //刪除類型
        public bool DeleteNewsType(string NwtID)
        {
            InitDB();

            string sqlCmd;

            sqlCmd = "DELETE FROM dbo.tbNewsType WHERE NwtTypeID='" + NwtID + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //更新排序
        public bool NewsTypeSort(string NwtSort, int i)
        {
            InitDB();

            string sqlCmd = "UPDATE dbo.tbNewsType SET NwtSort='" + i + "' ";
            sqlCmd += " WHERE NwtSort='" + NwtSort + "' ";

            try
            {
                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //上移
        public bool NewsTypeSortUp(string NwtSort)
        {
            InitDB();

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sql = " SELECT Top 2 [NwtTypeID],[NwtSort] ";
            sql += " FROM dbo.tbNewsType";
            sql += " Where NwtSort <=" + NwtSort;
            sql += " Order by NwtSort DESC";

            da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            da.Dispose();

            if (dt.Rows.Count >= 2) //防呆
            {
                string firstID = dt.Rows[0]["NwtTypeID"].ToString();
                string firstSort = dt.Rows[0]["NwtSort"].ToString();

                string secondID = dt.Rows[1]["NwtTypeID"].ToString();
                string secondSort = dt.Rows[1]["NwtSort"].ToString();

                string sqlCmd = "Update dbo.tbNewsType Set NwtSort='" + secondSort + "'";
                sqlCmd += " Where NwtTypeID = '" + firstID + "'";
                sqlCmd += " Update dbo.tbNewsType Set NwtSort='" + firstSort + "'";
                sqlCmd += " Where NwtTypeID = '" + secondID + "'";

                try
                {
                    cmd = new SqlCommand(sqlCmd, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    conn.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        //下移
        public bool NewsTypeSortDown(string NwtSort)
        {
            InitDB();

            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sql = " SELECT Top 2 [NwtTypeID],[NwtSort] ";
            sql += " FROM dbo.tbNewsType";
            sql += " Where NwtSort >=" + NwtSort;
            sql += " Order by NwtSort ASC";

            da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            da.Dispose();

            if (dt.Rows.Count >= 2) //防呆
            {
                string firstID = dt.Rows[0]["NwtTypeID"].ToString();
                string firstSort = dt.Rows[0]["NwtSort"].ToString();

                string secondID = dt.Rows[1]["NwtTypeID"].ToString();
                string secondSort = dt.Rows[1]["NwtSort"].ToString();

                string sqlCmd = "Update dbo.tbNewsType Set NwtSort='" + secondSort + "'";
                sqlCmd += " Where NwtTypeID = '" + firstID + "'";
                sqlCmd += " Update dbo.tbNewsType Set NwtSort='" + firstSort + "'";
                sqlCmd += " Where NwtTypeID = '" + secondID + "'";

                try
                {
                    cmd = new SqlCommand(sqlCmd, conn);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    clsEx.ExrType = "EXT001";
                    clsEx.ExrMsg = ex.ToString();
                    clsEx.NewExceptionMsg();

                    conn.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 最新消息管理

        // 根據公告規則和發布日期取得公告訊息( 公告類別,發布日期排序 )(後台)(判斷公告有效日期)
        public DataTable GetNewsListDateRang(DateTime dtStartSet, DateTime dtEndSet, string NewsType)
        {
            string querySQL = "SELECT * ";
            querySQL += "FROM dbo.tbNewsList INNER JOIN dbo.tbNewsType ON dbo.tbNewsList.NwtTypeID = dbo.tbNewsType.NwtTypeID ";
            //querySQL += "WHERE dbo.tbNewsList.NwsStartDate <= '" + TransferDateTime(GetServerDate()) + "' AND dbo.tbNewsList.NwsEndDate >= '" + TransferDateTime(GetServerDate()) + "' ";
            querySQL += "WHERE dbo.tbNewsList.NwsCreaDate >= '" + TransferStartDateTime(dtStartSet) + "' AND dbo.tbNewsList.NwsCreaDate <= '" + TransferEndDateTime(dtEndSet.AddSeconds(1)) + "' ";

            if (NewsType != "-1")
            {
                querySQL += " AND dbo.tbNewsList.NwtTypeID = '" + NewsType + "' ";
            }

            querySQL += "ORDER BY dbo.tbNewsList.NwtTypeID, dbo.tbNewsList.NwsCreaDate DESC";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 根據NewsID取得詳細公告訊息
        public DataTable GetNewsDetailByID(string NewsID)
        {
            string querySQL = "SELECT * ";
            querySQL += "FROM dbo.tbNewsList INNER JOIN dbo.tbNewsType ON dbo.tbNewsList.NwtTypeID = dbo.tbNewsType.NwtTypeID ";
            querySQL += "WHERE dbo.tbNewsList.NwsIndex = '" + NewsID + "' ";

            DataTable DT = new DataTable();
            DT.TableName = "newTable";
            try
            {
                InitDB();

                da = new SqlDataAdapter(querySQL, conn);

                da.Fill(DT);
                da.Dispose();

                return DT;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return DT;
            }
            finally
            {
                CloseDB();
            }
        }

        // 新增公告訊息
        public bool AddNews(string NewsID, DateTime NwsCreaDate, DateTime NewsStartSet, DateTime NewsEndSet, int NewsEnable, string NewsType, string NewsTitle, string NewsMsg, string NewsUrl, string NwsPicPath, string LastUpdateUser, DateTime LastUpdateDate)
        {
            string strSQL = "INSERT INTO dbo.tbNewsList (NwsIndex, NwsCreaDate, NwsStartDate, NwsEndDate, NwsEnable, NwtTypeID, NwsTitle, ";
            strSQL += "NwsMsg, NwsUrl, NwsPicPath, LastUpdateUser, LastUpdateDate) ";
            strSQL += "VALUES ('" + NewsID + "', '" + TransferDateTime(NwsCreaDate) + "', '" + TransferDateTime(NewsStartSet) + "', '" + TransferDateTime(NewsEndSet) + "', " + NewsEnable + ", '";
            strSQL += NewsType + "', N'" + NewsTitle + "', N'" + NewsMsg + "', N'" + NewsUrl + "', N'" + NwsPicPath + "', N'" + LastUpdateUser + "', '" + TransferDateTime(LastUpdateDate) + "') ";

            try
            {
                InitDB();

                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseDB();
            }
        }

        // 更新公告訊息
        public bool UpdateNews(string NewsID, DateTime NwsCreaDate, DateTime NewsStartSet, DateTime NewsEndSet, int NewsEnable, string NewsType, string NewsTitle, string NewsMsg, string NewsUrl, string NwsPicPath, string LastUpdateUser, DateTime LastUpdateDate)
        {
            string strSQL = "UPDATE dbo.tbNewsList SET NwsCreaDate = '" + TransferDateTime(NwsCreaDate) + "', NwsStartDate = '" + TransferDateTime(NewsStartSet) + "', NwsEndDate = '" + TransferDateTime(NewsEndSet) + "', NwsEnable = ";
            strSQL += NewsEnable + ", NwtTypeID = '" + NewsType + "', NwsTitle = N'" + NewsTitle + "', NwsMsg = N'" + NewsMsg + "', NwsUrl = N'";
            strSQL += NewsUrl + "', NwsPicPath = N'" + NwsPicPath + "', LastUpdateUser = N'" + LastUpdateUser + "', LastUpdateDate = '" + TransferDateTime(LastUpdateDate) + "' ";
            strSQL += "WHERE NwsIndex = '" + NewsID + "' ";

            try
            {
                InitDB();

                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseDB();
            }
        }

        // 刪除公告訊息
        public bool DeleteNews(string NewsID)
        {
            string strSQL = "DELETE FROM dbo.tbNewsList WHERE NwsIndex = '" + NewsID + "' ";

            try
            {
                InitDB();

                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseDB();
            }
        }

        #endregion

        #region GGB開發

        #region 希望清單管理

        //根據ID取得希望商品
        public DataTable GetHopeProductByID(string HopID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbHope ";
            sqlCmd += "WHERE HopID = '" + HopID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //根據商品類別取得希望清單
        public DataTable GetHopeListByPrtID(string PrtID)
        {
            DataTable dt = new DataTable();
            dt.TableName = "newTable";

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbHope ";
            if (PrtID != "-1")
            {
                sqlCmd += "WHERE HopType = '" + PrtID + "' ";
            }
            sqlCmd += "ORDER BY CreateDate DESC ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        //新增希望清單
        public bool InsertHopeList(string HopID, string HopType, string HopPDName, string HopPDUrl, string HopUserName, string HopUserEmail,
                                                           string HopDesc, string CreateUser, DateTime CreateDate)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbHope (HopID, HopType, HopPDName, HopPDUrl, HopUserName, HopUserEmail, HopDesc, CreateUser, CreateDate) ";
                sqlCmd += "VALUES ('" + HopID + "', '" + HopType + "', N'" + HopPDName + "', N'" + HopPDUrl + "', N'" + HopUserName + "', '" + HopUserEmail + "', N'" + HopDesc + "', N'" + CreateUser + "', '" + TransferDateTime(CreateDate) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        #endregion

        #endregion

        //圖片管理

        #region 圖片

        // 根據ID取得圖片
        public DataTable GetAlbumByID(string ID)
        {
            DataTable dt = new DataTable();

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbAlbum ";
            sqlCmd += "WHERE ID = '" + ID + "' ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        // 根據計畫ID取得相關圖片
        public DataTable GetAlbumByPrdID(string PrdID)
        {
            DataTable dt = new DataTable();

            string sqlCmd = "SELECT * ";
            sqlCmd += "FROM dbo.tbAlbum ";
            sqlCmd += "WHERE PrdID = '" + PrdID + "' ";
            sqlCmd += "ORDER BY SORT ";

            try
            {
                InitDB();

                da = new SqlDataAdapter(sqlCmd, conn);
                da.Fill(dt);

                da.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                Console.WriteLine(ex.ToString());
                return dt;
            }

            return dt;
        }

        // 新增一筆圖片
        public int InsertAlbum(string PrdID, string NAME, string DESC, string FILENAME, int SORT, DateTime CREATE_DATE, string WRITER, DateTime WRITE_DATE)
        {
            try
            {
                InitDB();

                string sqlCmd = "INSERT INTO dbo.tbAlbum (PrdID, NAME, [DESC], FILENAME, SORT, CREATE_DATE, WRITER, WRITE_DATE) ";
                sqlCmd += "VALUES('" + PrdID + "',N'" + NAME + "',N'" + DESC + "','" + FILENAME + "','" + SORT + "','" + TransferDateTime(CREATE_DATE) + "','" + WRITER + "','" + TransferDateTime(WRITE_DATE) + "') ";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        //刪除一筆圖片
        public bool DeleteAlbum(string ID)
        {
            InitDB();

            string sqlCmd;

            try
            {
                sqlCmd = "DELETE FROM dbo.tbAlbum WHERE ID='" + ID + "'";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        //根據PrdID刪除圖片
        public bool DeleteAlbumByPrdID(string PrdID)
        {
            InitDB();

            string sqlCmd;

            try
            {
                sqlCmd = "DELETE FROM dbo.tbAlbum WHERE PrdID='" + PrdID + "'";

                cmd = new SqlCommand(sqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd = null;

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsEx.ExrType = "EXT001";
                clsEx.ExrMsg = ex.ToString();
                clsEx.NewExceptionMsg();

                conn.Close();
                return false;
            }
        }

        #endregion

        // 取得Server的時間
        public DateTime GetServerDate()
        {
            InitDB();

            DateTime dtime = new DateTime();
            string sqlCmd = "SELECT getdate() AS [NOW] ";

            cmd = new SqlCommand(sqlCmd, conn);

            dtime = (DateTime)cmd.ExecuteScalar();

            conn.Close();

            return dtime;
        }

        #region 方法

        //遇到Datetime時必須轉換
        public string TransferDateTime(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss");
        }

        //遇到Datetime時必須轉換(預設起時間)
        public string TransferStartDateTime(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd 00:00:00");
        }

        //遇到Datetime時必須轉換(預設迄時間)
        public string TransferEndDateTime(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd 23:59:59");
        }

        #endregion
    }
}
