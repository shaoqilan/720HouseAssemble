using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherManagement
{
    /// <summary>
    /// 请求返回值管理
    /// </summary>
    public class ResponseTool
    {
        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static System.Web.Mvc.JsonResult Create(object Data, int ErrCode = 200, string ErrMsg = "")
        {
            CustomJsonResult JsonRes = new CustomJsonResult();
            JsonRes.ContentEncoding = System.Text.Encoding.UTF8;
            JsonRes.ContentType = "json";
            JsonRes.Data = new { ErrCode = (int)ErrCode, ErrMsg = ErrMsg,Data = Data};
            JsonRes.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            return JsonRes;
        }
        /// <summary>
        /// 创建消息返回
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static System.Web.Mvc.JsonResult Create(int ErrCode = 200, string ErrMsg = "")
        {
            CustomJsonResult JsonRes = new CustomJsonResult();
            JsonRes.ContentEncoding = System.Text.Encoding.UTF8;
            JsonRes.ContentType = "json";
            JsonRes.Data = new { ErrCode = (int)ErrCode, ErrMsg = ErrMsg};
            JsonRes.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            return JsonRes;
        }
        /// <summary>
        /// 创建消息返回
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static System.Web.Mvc.JsonResult CreateData(object Data)
        {
            CustomJsonResult JsonRes = new CustomJsonResult();
            JsonRes.ContentEncoding = System.Text.Encoding.UTF8;
            JsonRes.ContentType = "json";
            JsonRes.Data = Data;
            JsonRes.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            return JsonRes;
        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static string CreateString(object Data, int ErrCode = 200, string ErrMsg = "")
        {
            var TemData = new { ErrCode = (int)ErrCode, ErrMsg = ErrMsg, Data = Data };
            return Common.Help.JsonManager.JavaScriptSerializableByNewtonsoft(TemData);
        }
        /// <summary>
        /// 创建消息返回
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static string CreateString(int ErrCode = 200, string ErrMsg = "")
        {
            var Data = new { ErrCode = (int)ErrCode, ErrMsg = ErrMsg };
            return Common.Help.JsonManager.JavaScriptSerializableByNewtonsoft(Data);
        }

    }
}
