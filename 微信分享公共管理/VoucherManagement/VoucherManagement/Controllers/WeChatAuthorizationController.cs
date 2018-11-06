using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoucherManagement.Tools;

namespace VoucherManagement.Controllers
{
    /// <summary>
    /// 微信统一授权
    /// </summary>
    [AllowCrossSiteJson]
    public class WeChatAuthorizationController : Controller
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="AuthData"></param>
        /// <returns></returns>
        public ActionResult SponsorOAuth(string AuthData,string AppId)
        {
            //会自动解码，所以再次进行编码
            AuthData = HttpUtility.UrlEncode(AuthData, System.Text.Encoding.UTF8);
            bool AuthState = false;
            string ErrMsg = string.Empty;
            //缓存的键值
            string MarkKey = StringTool.GetMD5(AuthData).ToUpper();
            //获取授权的地址
            string RedirectUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/WeChatAuthorization/OAuthCallBack?MarkKey=" + MarkKey;
            //进行解密
            var AuthDataModel = new
            {
                SessionKey="",//授权完毕保存到session的键名
                AuthDataParam = "AuthData",//授权信息传递的参数名称
                AppId ="",
                IsBase=true,//是否是基础信息true表示只获取openid（隐士授权） false表示显示授权，可以获取用户的信息
                RedirectUrl = ""
            };
            try
            {
                AuthDataModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(Tools.DESEncryptTool.Decode(AuthData, Tools.StringTool.GetMD5(AppId).ToUpper()), AuthDataModel);
                if (AppId== AuthDataModel.AppId&& !string.IsNullOrEmpty(AuthDataModel.RedirectUrl)&& !string.IsNullOrEmpty(AuthDataModel.AppId))
                {
                    //将授权的信息保存在缓存中2分钟
                    HttpContext.Cache.Insert(MarkKey, AuthDataModel, null, DateTime.UtcNow.AddMinutes(2), TimeSpan.Zero);
                    string AuthUrl = string.Empty;
                    if (AuthDataModel.IsBase)
                    {
                        //只获取openid
                        AuthUrl = new UserAuthorization().GetOpenIDAuthorizeUrl(AuthDataModel.AppId, RedirectUrl, MarkKey);
                    }
                    else
                    {
                        AuthUrl = new UserAuthorization().GetUserInfoAuthorizeUrl(AuthDataModel.AppId, RedirectUrl, MarkKey);
                    }
                    AuthState = true;
                    return new RedirectResult(AuthUrl);
                }
                else
                {
                    //授权信息错误！
                    ErrMsg = "授权信息错误";
                }
            }
            catch (Exception ex)
            {
                //授权信息错误
                ErrMsg = "授权信息错误:"+ex.Message;
            }
            ViewBag.AuthState = AuthState;
            ViewBag.ErrMsg = ErrMsg;
            return View();
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuthCallBack(string code, string MarkKey)
        {
            bool AuthState = false;
            string ErrMsg = string.Empty;
            dynamic AuthDataModel = HttpContext.Cache.Get(MarkKey ?? "") as dynamic;
            if (AuthDataModel != null)
            {
                try
                {
                    string SessionKey = Convert.ToString(AuthDataModel.AppId);
                    string AuthDataParam = Convert.ToString(AuthDataModel.AuthDataParam);
                    string AppId = Convert.ToString(AuthDataModel.AppId);
                    bool IsBase =Convert.ToBoolean(AuthDataModel.IsBase);
                    Uri RedirectUrl = new Uri(AuthDataModel.RedirectUrl);
                    if (RedirectUrl != null)
                    {
                        //获取用户的信息
                        try
                        {
                            WeChatUser UModel = new WeChatUser();
                            if (IsBase)
                            {
                                UModel.OpenID = new UserAuthorization().GetAuthorizeUserOpenID(AppId, code);
                            }
                            else
                            {
                                UModel = new UserAuthorization().GetUserInfo(AppId, code);
                            }
                            string AuthData = Tools.DESEncryptTool.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(UModel),Tools.StringTool.GetMD5(AppId).ToUpper());
                            string LinkRedirectUrl = RedirectUrl.AbsoluteUri;
                            if (!string.IsNullOrEmpty(RedirectUrl.Query))
                            {
                                LinkRedirectUrl = LinkRedirectUrl + "&";
                            }
                            else
                            {
                                LinkRedirectUrl = LinkRedirectUrl + "?";
                            }
                            if (string.IsNullOrEmpty(AuthDataParam))
                            {
                                AuthDataParam = "AuthData";
                            }
                            LinkRedirectUrl = LinkRedirectUrl + AuthDataParam + "=" + AuthData;
                            //进行跳转
                            AuthState = true;
                            return new RedirectResult(LinkRedirectUrl);
                        }
                        catch (Exception ex)
                        {
                            //获取用户信息失败
                            ErrMsg = "获取用户信息失败";
                        }
                    }
                    else
                    {
                        //授权信息错误
                        ErrMsg = "授权信息错误";
                    }
                }
                catch (Exception ex)
                {
                    //授权信息错误！
                    ErrMsg = "授权信息错误";
                }
            }
            else
            {
                //授权信息错误！
                ErrMsg = "授权信息错误";
            }
            ViewBag.AuthState = AuthState;
            ViewBag.ErrMsg = ErrMsg;
            return View();
        }
    }
}