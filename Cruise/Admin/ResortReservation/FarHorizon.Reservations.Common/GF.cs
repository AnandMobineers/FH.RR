using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using FarHorizon.Reservations.DataBaseManager;
using System.Text.RegularExpressions;

namespace FarHorizon.Reservations.Common
{
    //GenfFunction is renamed as GF, just to reduce the length of the name.
    public static class GF
    {
        const int _INDIAN_NATIONALITY_ID = 1;
        static DateTime ServerDateTime;        

        public static int GetIndianNatinalityId()
        {
            return _INDIAN_NATIONALITY_ID;
        }
        
        public static void LogError(string ModuleName, string ErrorString)
        {
            //string AppPath;
            //FileStream fs;
            //StreamWriter sw;
            ////AppPath = "C:/FARHorizon/ResortReservation/ResortManager/Log/Exception.txt";
            //AppPath = @"http:\\www.farhorizon.co.in\fhlog\errors.log";
            ////XMLFilePath = @Application.StartupPath;
            ////XMLFilePath += @"\XML\DatabaseConfig.xml";
            //try
            //{
            //    fs = new FileStream(AppPath, FileMode.Append, FileAccess.Write);
            //    sw = new StreamWriter(fs);
            //    sw.WriteLine("At " + GetDDMMMYYYY(GetDate(), true) + " Error: " + ErrorString + " in " + ModuleName);
            //    sw.Close();
            //    fs.Close();
            //}
            //catch (Exception exp)
            //{
            //    Console.WriteLine("Error in writing to log file" + exp.Message);
            //}
        }

        public static DateTime GetDate()
        {
            DatabaseManager oDB;
            string sQuery;
            if (ServerDateTime == DateTime.MinValue)
            {
                sQuery = " Select GetDate()";
                oDB = new DatabaseManager();
                oDB.DbCmd = oDB.GetSqlStringCommand(sQuery);
                //dsBookingData = oDB.FetchRecords("BookingData", sQuery);
                ServerDateTime = (DateTime)oDB.ExecuteScalar(oDB.DbCmd);
            }
            return ServerDateTime;
        }

        public static string GetMonthName(int MonthNo)
        {
            string MonthName = "";
            switch (MonthNo)
            {
                case 1:
                    MonthName = "Jan";
                    break;
                case 2:
                    MonthName = "Feb";
                    break;
                case 3:
                    MonthName = "Mar";
                    break;
                case 4:
                    MonthName = "Apr";
                    break;
                case 5:
                    MonthName = "May";
                    break;
                case 6:
                    MonthName = "Jun";
                    break;
                case 7:
                    MonthName = "Jul";
                    break;
                case 8:
                    MonthName = "Aug";
                    break;
                case 9:
                    MonthName = "Sep";
                    break;
                case 10:
                    MonthName = "Oct";
                    break;
                case 11:
                    MonthName = "Nov";
                    break;
                case 12:
                    MonthName = "Dec";
                    break;
            }
            return MonthName;
        }

        public static string GetDD_MM_YYYY(DateTime date)
        {
            return GetDD_MM_YYYY(date, false);
        }

        public static string GetDD_MM_YYYY(DateTime date, bool TimeReq)
        {
            string d = "";
            d = date.Day.ToString("00") + "-" + date.Month.ToString("00") + "-" + date.Year.ToString("0000");
            if (TimeReq == true)
            {
                d += " " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00");
            }
            return d;
        }

        public static string GetDD_MMM_YYYY(DateTime date, bool TimeReq)
        {
            string d;
            d = date.Day.ToString("00");
            d += "-" + GetMonthName(date.Month) + "-" + date.Year.ToString("0000");

            if (TimeReq == true)
            {
                d += " " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00");
            }
            return d;
        }

        public static string GetHHMM(DateTime dateTime)
        {
            string d;
            d = dateTime.Hour.ToString("00");
            d += "-" + dateTime.Minute.ToString("00");

            return d;
        }

        public static string GetYYYYMMDD(DateTime date)
        {
            return date.Year.ToString("0000") + date.Month.ToString("0#") + date.Day.ToString("0#");
        }

        public static DateTime GetDateFromYYYYMMDD(string date)
        {
            DateTime dt;
            string yyyy = date.Substring(0, 4);
            string mm = date.Substring(4, 2);
            string dd = date.Substring(6, 2);
            DateTime.TryParse(yyyy + "-" + mm + "-" + dd, out dt);
            return dt;
        }

        public static string HandleMaxMinDates(DateTime date, bool TimeReq)
        {
            string str;
            if (date == DateTime.MinValue || date == DateTime.MaxValue)
                str = "1900-01-01";
            else
            {
                if (TimeReq == true)
                    str = date.Year.ToString("0000") + "-" + date.Month.ToString("0#") + "-" + date.Day.ToString("0#") + " " + date.Hour.ToString("0#") + ":" + date.Minute.ToString("0#");
                else
                    str = date.Year.ToString("0000") + "-" + date.Month.ToString("0#") + "-" + date.Day.ToString("0#");
            }
            return str;
        }

        public static string Handle19000101(DateTime date, bool TimeReq)
        {
            /*If the date is "1900-01-01" then it will return "";
            else date will be return in DD-MM-YYYY";
            If TimeReq is True, it also return the time part of the date.*/
            if (date == Convert.ToDateTime("1900-01-01"))
                return "";
            else
                return GetDD_MMM_YYYY(date, TimeReq);
        }

        public static DateTime ParseDate(string date)
        {
            DateTime dt;
            if (DateTime.TryParse(date, out dt) == false)
                dt = DateTime.MinValue;
            return dt;
        }
        
        public static void HasRecords(string ID, string MasterName, out string Message)
        {
            string sProcName;
            DatabaseManager oDB;
            try
            {
                oDB = new DatabaseManager();
                Message = "";
                sProcName = "up_GetRecordCount";
                oDB.DbCmd = oDB.GetStoredProcCommand(sProcName);
                oDB.DbDatabase.AddInParameter(oDB.DbCmd, "@ID", DbType.String, ID);
                oDB.DbDatabase.AddInParameter(oDB.DbCmd, "@MasterName", DbType.String, MasterName);
                oDB.DbDatabase.AddOutParameter(oDB.DbCmd, "@Message", DbType.String, 500);
                oDB.ExecuteNonQuery(oDB.DbCmd);
                Message = oDB.DbCmd.Parameters[2].Value.ToString();
            }
            catch (Exception exp)
            {
                GF.LogError("clsAccomodationMaster.HasRecords", exp.Message.ToString());
                oDB = null;
                Message = "";
            }
            finally
            {
                oDB = null;
            }
        }

        public static DataSet GetDataFromDB(string Query)
        {
            DatabaseManager oDB = new DatabaseManager();
            DataSet ds = null;
            try
            {
                oDB.DbCmd = oDB.GetSqlStringCommand(Query);
                ds = oDB.ExecuteDataSet(oDB.DbCmd);
            }
            catch (Exception exp)
            {
                GF.LogError("Common.GetDataFromDB", exp.Message + " " + Query);
                oDB = null;
                ds = null;
            }
            return ds;
        }

        public static string ReplaceSpace(string inputStr)
        {
            return inputStr.Replace(" ", "~");
        }

        public static string RecoverSpace(string inputStr)
        {
            return inputStr.Replace("~", " ");
        }

        public static bool ValidateEmailId(string sEmail)
        {
            sEmail = sEmail.Trim().Replace(" ", "");

            string Pattern = @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
            //string Pattern = @"^[a-zA-Z][\w\-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";

            System.Text.RegularExpressions.Match match = Regex.Match(sEmail, Pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return true;
            else
                return false;
        }
    }    
}
