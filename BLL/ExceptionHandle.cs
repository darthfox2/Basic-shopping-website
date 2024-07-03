using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using DAL;


namespace BLL
{
    public class ExceptionHandle
    {
        public string ExrType { get; set; }
        public string ExrMsg { get; set; }


        // 加入一筆新的ExceptionRecord(BEAR)
        public void NewExceptionMsg()
        {
            DALClass clsDAL = new DALClass();
            clsDAL.AddException(ExrType, ExrMsg);      
        }

        // 顯示所有的錯誤資訊
        public DataTable ShowExceptionList()
        {
            DALClass clsDAL = new DALClass();
            DataTable DT = new DataTable();
            DT = clsDAL.GetExceptionList();
            return DT;
        }

        // 根據類別選單顯示錯誤資訊
        public DataTable ShowExceptionByType(string ErrType)
        {
            DALClass clsDAL = new DALClass();
            DataTable DT = new DataTable();
            DT = clsDAL.GetExceptionByType(ErrType);
            return DT;
        }

        // 根據發生日期查詢顯示錯誤訊息
        public DataTable ShowExceptionByDateRang(DateTime dtStartSet, DateTime dtEndSet, string ErrType)
        {
            DALClass clsDAL = new DALClass();
            DataTable DT = new DataTable();
            DT = clsDAL.GetExceptionByDateRang(dtStartSet, dtEndSet, ErrType);
            return DT;
        }
    }
}