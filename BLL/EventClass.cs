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
    public class EventClass
    {
        public string EvrType { get; set; }
        public string EvrMsg { get; set; }
        public string UseID { get; set; }

        // 加入一筆新的Event(BEAR)
        public void NewEventMsg()
        {
            DALClass clsDAL = new DALClass();
            clsDAL.AddEvent(EvrType, EvrMsg, UseID);
        }

        // 顯示所有的事件資訊
        public DataTable ShowEventList()
        {
            DALClass clsDAL = new DALClass();
            DataTable DT = new DataTable();
            DT = clsDAL.GetEventList();
            return DT;
        }

        // 根據發生日期查詢顯示錯誤訊息
        public DataTable ShowEventByDateRang(DateTime dtStartSet, DateTime dtEndSet, string EveType)
        {
            DALClass clsDAL = new DALClass();
            DataTable DT = new DataTable();
            DT = clsDAL.GetEventByDateRang(dtStartSet, dtEndSet, EveType);
            return DT;
        }
    }
}