using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VoucherManagement.Tools;

namespace VoucherManagement.Controllers
{
    /// <summary>
    /// 微信统一注册凭证
    /// </summary>
    [AllowCrossSiteJson]
    public class VoucherController : Controller
    {
        /// <summary>
        /// 获取初始化jssdk配置信息
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public JsonResult GetWeChatJsSdk(string AppId,string Url)
        {
            //生成微信sdk的签名参数
            if (!string.IsNullOrEmpty(AppId))
            {
                dynamic WXModel = new System.Dynamic.ExpandoObject();
                string timestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
                string nonceStr = Guid.NewGuid().ToString("N");
                string jsapi_ticket = WeChatDeploy.GetJSAPITicket(AppId);
                if (!string.IsNullOrEmpty(jsapi_ticket))
                {
                    string url = Url;
                    //签名
                    List<string> Tem = new List<string>();
                    Tem.Add("timestamp=" + timestamp);
                    Tem.Add("noncestr=" + nonceStr);
                    Tem.Add("jsapi_ticket=" + jsapi_ticket);
                    Tem.Add("url=" + url);
                    Tem.Sort();
                    byte[] StrRes = Encoding.Default.GetBytes(string.Join("&", Tem));
                    HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
                    StrRes = iSHA.ComputeHash(StrRes);
                    StringBuilder signature = new StringBuilder();
                    foreach (byte iByte in StrRes)
                    {
                        signature.AppendFormat("{0:x2}", iByte);
                    }
                    WXModel.appId = AppId;
                    WXModel.timestamp = timestamp;
                    WXModel.nonceStr = nonceStr;
                    WXModel.signature = signature.ToString().ToLower();
                    WXModel.jsApiList = new List<string>();
                    WXModel.jsApiList.Add("updateAppMessageShareData");
                    WXModel.jsApiList.Add("updateTimelineShareData");
                    WXModel.jsApiList.Add("onMenuShareWeibo");
                    WXModel.jsApiList.Add("hideMenuItems");
                    WXModel.jsApiList.Add("showMenuItems");
                    WXModel.jsApiList.Add("hideAllNonBaseMenuItem");
                    WXModel.jsApiList.Add("showAllNonBaseMenuItem");
                    WXModel.jsApiList.Add("getLocation");
                    WXModel.jsApiList.Add("openLocation");
                    return ResponseTool.Create(WXModel, 200, "ok");
                }
                else
                {
                    return ResponseTool.Create(201, "初始化失败");
                }
            }
            else
            {
                return ResponseTool.Create(201, "初始化失败,请传入AppId");
            }
        }
        /// <summary>
        /// 获取微信全局AccessToken
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public JsonResult GetWeChatAccessToken(string AppId)
        {
            return ResponseTool.Create(new { AccessToken = WeChatDeploy.GetAccessToken(AppId) }, 200, "ok");
        }
        /// <summary>
        /// 获取微信全局JsApiTicket
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public JsonResult GetWeChatJsApiTicket(string AppId)
        {
            return ResponseTool.Create(new { AccessToken = WeChatDeploy.GetJSAPITicket(AppId) }, 200, "ok");
        }
    }
}