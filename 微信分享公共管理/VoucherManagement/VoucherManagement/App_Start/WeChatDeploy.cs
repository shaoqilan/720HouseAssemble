using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin;
using Senparc.Weixin.MP.Containers;

namespace VoucherManagement
{
    public class WeChatDeploy
    {
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GetDeploy()
        {
            Dictionary<string, string> DKey = new Dictionary<string, string>();
            string[] Keys= System.Configuration.ConfigurationManager.AppSettings.AllKeys;
            foreach (var item in Keys)
            {
                if (item.IndexOf("WX_")==0)
                {
                    DKey.Add(item.Substring(3), System.Configuration.ConfigurationManager.AppSettings[item]);
                }
            }
            return DKey;
        }
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            var register = RegisterService.Start(SenparcSetting.BuildFromWebConfig(true)).UseSenparcGlobal();//CO2NET全局注册，必须！
            register.UseSenparcWeixin(SenparcWeixinSetting.BuildFromWebConfig(true), SenparcSetting.BuildFromWebConfig(true));////微信全局注册，必须！
            Dictionary<string, string> Deploy= GetDeploy();
            foreach (var item in Deploy)
            {
                AccessTokenContainer.Register(item.Key, item.Value, "WX_"+ item.Key);
            }
        }
        /// <summary>
        /// [注意只能是注册机才可以调用]获取全局AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken(string AppID)
        {
            return AccessTokenContainer.GetAccessTokenResult(AppID).access_token;
        }
        /// <summary>
        /// [注意只能是注册机才可以调用]获取jsapi的票据
        /// </summary>
        /// <returns></returns>
        public static string GetJSAPITicket(string AppID)
        {
            Dictionary<string, string> Deploy = GetDeploy();
            if (Deploy.Keys.Contains(AppID))
            {
                return Senparc.Weixin.MP.CommonAPIs.CommonApi.GetTicket(AppID, Deploy[AppID]).ticket;
            }
            else
            {
                return "";
            }
        }
    }
}