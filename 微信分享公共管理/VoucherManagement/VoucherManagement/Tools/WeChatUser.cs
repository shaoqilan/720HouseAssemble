using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherManagement.Tools
{
    /// <summary>
    /// 微信用户的信息
    /// </summary>
    [Serializable]
    public class WeChatUser
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 呢称
        /// </summary>
        public string NickName { get; set; }
        public string OpenID { get; set; }
        public int Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string[] Privilege { get; set; }
        public string Province { get; set; }
        public string UnionID { get; set; }
    }
}
