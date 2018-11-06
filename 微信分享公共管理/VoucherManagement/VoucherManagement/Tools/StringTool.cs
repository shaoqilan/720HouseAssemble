using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherManagement.Tools
{
    /// <summary>
    /// 字符串
    /// </summary>
    public static class StringTool
    {
        /// <summary>
        /// 将字符转成大写并且进行排序(考试答案比对使用)
        /// </summary>
        /// <param name="tem"></param>
        /// <returns></returns>
        public static string ToSort(this string tem)
        {
            List<char> ArryTem = tem.ToUpper().ToCharArray().ToList();
            ArryTem.Sort();
            return string.Join("", ArryTem.ToArray());
        }
        //转成隐私字符
        public static string ToPrivacy(string tem)
        {
            if (!string.IsNullOrEmpty(tem) && tem.Length > 4)
            {
                string behind = tem.Substring(tem.Length - 4);
                if ((tem.Length - behind.Length) > 4)
                {
                    string front = tem.Substring(0, 4);
                    tem = front + "****" + behind;
                }
                else
                {
                    tem = "****" + behind;
                }
            }
            return tem;
        }
        /// <summary>
        /// 从身份证中提取性别
        /// </summary>
        /// <param name="tem"></param>
        /// <returns></returns>
        public static string ExtractSex(this string tem)
        {
            int Sex = -1;
            if (tem.Length == 15)
            {
                //15位身份证
                int.TryParse(tem.Substring(12, 3), out Sex);
            }
            else if (tem.Length == 18)
            {
                //18位身份证
                int.TryParse(tem.Substring(14, 3), out Sex);
            }
            if (Sex != -1)
            {
                if (Sex % 2 == 0)
                {
                    return "女";
                }
                else
                {
                    return "男";
                }
            }
            else
            {
                return "身份证号码错误";
            }
        }
        /// <summary>
        /// 从身份证中提取出生年月
        /// </summary>
        /// <param name="tem"></param>
        /// <returns></returns>
        public static string ExtractBirthday(this string tem)
        {
            if (tem.Length == 18)
            {
                return tem.Substring(6, 4) + "-" + tem.Substring(10, 2) + "-" + tem.Substring(12, 2);
            }
            else if (tem.Length == 15)
            {
                return "19" + tem.Substring(6, 2) + "-" + tem.Substring(8, 2) + "-" + tem.Substring(10, 2);
            }
            else
            {
                return "身份证号码错误";
            }
        }
        /// <summary>
        /// 从身份证中提取年龄
        /// </summary>
        /// <param name="tem"></param>
        /// <returns></returns>
        public static string ExtractAge(this string tem)
        {
            if (tem.Length == 18)
            {
                DateTime birthdate = Convert.ToDateTime(tem.Substring(6, 4) + "-" + tem.Substring(10, 2) + "-" + tem.Substring(12, 2));
                DateTime now = DateTime.Now;
                int Age = now.Year - birthdate.Year;
                if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
                {
                    Age--;
                }
                return (Age < 0 ? 0 : Age).ToString();
            }
            else if (tem.Length == 15)
            {
                DateTime birthdate = Convert.ToDateTime("19" + tem.Substring(6, 2) + "-" + tem.Substring(8, 2) + "-" + tem.Substring(10, 2));
                DateTime now = DateTime.Now;
                int Age = now.Year - birthdate.Year;
                if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
                {
                    Age--;
                }
                return (Age < 0 ? 0 : Age).ToString();
            }
            else
            {
                return "身份证号码错误";
            }
        }
        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string CalculateAge(this string time)
        {
            DateTime birthdate = DateTime.Now;
            DateTime.TryParse(time, out birthdate);
            DateTime now = DateTime.Now;
            int Age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                Age--;
            }
            return (Age < 0 ? 0 : Age).ToString();
        }


        /// <summary>
        ///格式化时间 几天前 ...
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static string formatMsgTime(DateTime timespan)
        {
            DateTime dateTime = timespan;

            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;
            DateTime now = DateTime.Now;

            double milliseconds = 0;
            string timeSpanStr;

            milliseconds = new TimeSpan(now.Ticks).TotalSeconds - new TimeSpan(dateTime.Ticks).TotalSeconds;

            if (milliseconds <= 60 * 1)
            {
                timeSpanStr = "刚刚";
            }
            else if (60 * 1 < milliseconds && milliseconds <= 60 * 60)
            {
                timeSpanStr = Math.Floor((milliseconds / (60))) + "分钟前";
            }
            else if (60 * 60 * 1 < milliseconds && milliseconds <= 60 * 60 * 24)
            {
                timeSpanStr = Math.Floor(milliseconds / (60 * 60)) + "小时前";
            }
            else if (60 * 60 * 24 < milliseconds && milliseconds <= 60 * 60 * 24 * 15)
            {
                timeSpanStr = Math.Floor(milliseconds / (60 * 60 * 24)) + "天前";
            }
            else if (milliseconds > 60 * 60 * 24 * 15 && year == now.Year)
            {
                timeSpanStr = dateTime.ToString("MM-dd hh:mm");
            }
            else
            {
                timeSpanStr = dateTime.ToString("yyyy-MM-dd hh:mm");
            }
            return timeSpanStr;
        }

        public static string GetMD5(string Str)
        {
            return Common.Help.StrToMD5.StrToMD5ReturnStr(Str);
        }
    }
}
