using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class MailClass
    {
        //建立CheckBoxList的方法
        public void SendMail(string MailTo, string MaiSubject, string MaiBody)
        {
            string strHost = System.Configuration.ConfigurationManager.AppSettings["Mail_Host"].ToString();
            string strPort = System.Configuration.ConfigurationManager.AppSettings["Mail_Port"].ToString();
            string MailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"].ToString();
            string MailFromDesc = System.Configuration.ConfigurationManager.AppSettings["MailFromDesc"].ToString();

            string[] strSplit = { ";" };
            string[] strToList = MailTo.Split(strSplit, StringSplitOptions.RemoveEmptyEntries);

            EmailAlert clsMail = new EmailAlert();
            clsMail.Mail_Host = strHost;
            clsMail.Mail_Port = int.Parse(strPort);

            clsMail.Subject = MaiSubject;
            clsMail.Body = MaiBody;

            clsMail.EmailSent(MailFrom, strToList, strToList.Length, "");
        }
    }
}
