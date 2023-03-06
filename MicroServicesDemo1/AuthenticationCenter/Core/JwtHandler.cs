using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCenter.Core
{
    public class JwtHandler
    {
        /// <summary>
        /// 颁发JWT
        /// </summary>
        /// <param name="tokenModel">当前颁发对象的用户信息</param>
        /// <returns>JWT字符串</returns>
        public static string IssueJwt(JwtUserInfo jwtUserInfo)
        {
            #region 【Step1-从配置文件中获取生成JWT所需要的数据】
            //string iss = Appsettings.GetVal(new string[] { "Audience", "Issuer" });//颁发者
            //string aud = Appsettings.GetVal(new string[] { "Audience", "Audience" });//使用者
            //string secret = Appsettings.GetVal(new string[] { "Audience", "Secret" }); //密钥
            string iss = "SixHorse";//颁发者
            string aud = "Everyone";//使用者
            string secret = "SixHorseJwtSecretKey"; //密钥
            #endregion

            #region 【Step2-通过Claim创建JWT中的Payload(载荷)信息】
            var claimsIdentity = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, jwtUserInfo.Uid.ToString()), //JWT ID
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//JWT的发布时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddDays(3)).ToUnixTimeSeconds()}"),//JWT到期时间
                new Claim(JwtRegisteredClaimNames.Iss, iss), //颁发者
                new Claim(JwtRegisteredClaimNames.Aud, aud)//使用者
            };
            //添加用户的角色信息（非必须，可添加多个）
            var claimRoleList = jwtUserInfo.Role.Split(',').Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claimsIdentity.AddRange(claimRoleList);
            #endregion

            #region 【Step3-签名对象】
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); //创建密钥对象
            var sigCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //创建密钥签名对象
            #endregion

            #region 【Step5-将JWT相关信息封装成对象】
            var jwt = new JwtSecurityToken(issuer: iss, claims: claimsIdentity, signingCredentials: sigCreds);
            #endregion

            #region 【Step6-将JWT信息对象生成字符串形式】
            var jwtHandler = new JwtSecurityTokenHandler();
            string token = jwtHandler.WriteToken(jwt);
            #endregion

            return token;
        }

        /// <summary>
        /// 将JWT加密的字符串进行解析
        /// </summary>
        /// <param name="jwtStr">JWT加密的字符</param>
        /// <returns>JWT中的用户信息</returns>
        public static JwtUserInfo SerializeJwtStr(string jwtStr)
        {
            JwtUserInfo jwtUserInfo = new JwtUserInfo();
            var jwtHandler = new JwtSecurityTokenHandler();

            if (!string.IsNullOrEmpty(jwtStr) && jwtHandler.CanReadToken(jwtStr))
            {
                //将JWT字符读取到JWT对象
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                //获取JWT中的用户信息
                jwtUserInfo.Uid = Convert.ToInt64(jwtToken.Id);
                object role;
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role); //获取角色信息
                jwtUserInfo.Role = role == null ? "" : role.ToString();
            }

            return jwtUserInfo;
        }

    }
}
