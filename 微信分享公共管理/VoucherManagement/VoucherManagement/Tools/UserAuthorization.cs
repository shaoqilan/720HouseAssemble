using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherManagement.Tools
{
    /// <summary>
    /// 用户授权
    /// </summary>
    public class UserAuthorization
    {
        /// <summary>
        /// 获取授权的地址
        /// </summary>
        /// <param name="RedirectUrl"></param>
        /// <param name="State">302跳转之后待的参数</param>
        /// <returns></returns>
        public string GetOpenIDAuthorizeUrl(string AppId, string RedirectUrl, string State)
        {
            return Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(AppId, RedirectUrl, State, OAuthScope.snsapi_base);
        }
        /// <summary>
        /// 获取用户信息的回掉地址
        /// </summary>
        /// <param name="RedirectUrl"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public string GetUserInfoAuthorizeUrl(string AppId, string RedirectUrl, string State)
        {
            return Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(AppId, RedirectUrl, State, OAuthScope.snsapi_userinfo);
        }
        /// <summary>
        /// 获取授权用户的openid
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string GetAuthorizeUserOpenID(string AppId,string Code)
        {
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult TokenResult = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(AppId,WeChatDeploy.GetDeploy()[AppId], Code);
            return TokenResult.openid;
        }
        /// <summary>
        /// 小程序获取用户登陆
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string GetWeChatLogin(string AppId, string Code)
        {
            string ApiUrl = "https://api.weixin.qq.com/sns/jscode2session?appid="+ AppId + "&secret="+ WeChatDeploy.GetDeploy()[AppId] + "&js_code="+ Code + "&grant_type=authorization_code";
            return new System.Net.WebClient().DownloadString(ApiUrl);
        }
        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public WeChatUser GetUserInfo(string AppId, string Code)
        {
            WeChatUser UModel = new WeChatUser();
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult TokenResult = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(AppId, WeChatDeploy.GetDeploy()[AppId], Code);
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthUserInfo Model = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(TokenResult.access_token, TokenResult.openid);
            UModel.HeadImgUrl = Model.headimgurl ?? "";
            UModel.NickName = Model.nickname ?? "";
            UModel.OpenID = Model.openid;
            UModel.Sex = Model.sex;
            UModel.City= Model.city??"";
            UModel.Country = Model.country??"";
            UModel.Privilege = Model.privilege??new string[0];
            UModel.Province = Model.province??"";
            UModel.UnionID = Model.unionid??"";
            return UModel;
        }
        /// <summary>
        /// 通过openid 获取用的基本信息
        /// </summary>
        /// <param name="Openid"></param>
        /// <returns></returns>
        public WeChatUser GetUserInfoOpenid(string AppId, string Openid)
        {
            WeChatUser UModel = new WeChatUser();
            Senparc.Weixin.MP.Entities.WeixinUserInfoResult Model = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetUserInfo(WeChatDeploy.GetAccessToken(AppId), Openid);
            UModel.HeadImgUrl = Model.headimgurl ?? "";
            UModel.NickName = Model.nickname ?? "";
            UModel.OpenID = Model.openid;
            UModel.Sex = Model.sex;
            UModel.City = Model.city ?? "";
            UModel.Country = Model.country ?? "";
            UModel.Privilege = new string[0];
            UModel.Province = Model.province ?? "";
            UModel.UnionID = Model.unionid ?? "";
            return UModel;
        }
        /// <summary>
        /// 判断是否关注
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Openid"></param>
        /// <returns></returns>
        public static bool VerifyAttention(string AppId, string Openid)
        {
            try
            {
                Senparc.Weixin.MP.Entities.WeixinUserInfoResult Model = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetUserInfo(WeChatDeploy.GetAccessToken(AppId), Openid);
                if (Model.subscribe == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
