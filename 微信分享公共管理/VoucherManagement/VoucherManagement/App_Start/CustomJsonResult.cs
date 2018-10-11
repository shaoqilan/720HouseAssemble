using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VoucherManagement
{
    /// <summary>
    /// 重写JsonResult
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var Serializer = new Newtonsoft.Json.JsonSerializer();
                Serializer.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                StringBuilder JsonStr=new StringBuilder();
                Serializer.Serialize(new System.IO.StringWriter(JsonStr), this.Data);
                response.Write(JsonStr);
            }
        }
    }
}
