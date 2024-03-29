﻿using Microsoft.IdentityModel.Tokens;

namespace DemoNetCoreProject.Common.Options
{
    /// <summary>
    /// JWT 設定項
    /// </summary>
    public class JwtOption
    {
        /// <summary>
        /// NameClaimType
        /// </summary>
        /// <remarks>透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name</remarks>
        public string NameClaimType { get; set; } = null!;
        /// <summary>
        /// RoleClaimType
        /// </summary>
        /// <remarks>透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色</remarks>
        public string RoleClaimType { get; set; } = null!;
        /// <summary>
        /// iss, Issuer, 簽發者
        /// </summary>
        /// <remarks>代表 JWT 的簽發主體。通常會放站台網址。</remarks>
        public string Issuer { get; set; } = null!;
        /// <summary>
        /// sub, Subject, 主體
        /// </summary>
        /// <remarks>代表 JWT 的主體，即它的所有人。通常會放站台名稱。</remarks>
        public string Subject { get; set; } = null!;
        /// <summary>
        /// aud, Audience, 對象
        /// </summary>
        /// <remarks>代表 JWT 的接收對象。通常會放對象名稱（String）或站台網址（URL）。</remarks>
        public string Audience { get; set; } = null!;
        /// <summary>
        /// nbf, Not Before, 非之前
        /// </summary>
        /// <remarks>定義在什麼時間之前，該 JWT 都是不可用的，即必須在指定時間後才生效。預設使用 UTC 時間。</remarks>
        public DateTime NotBefore => DateTime.UtcNow;
        /// <summary>
        /// iat, Issued At, 簽發時間
        /// </summary>
        /// <remarks>用來驗證 JWT 是否有效。預設使用 UTC 時間。</remarks>
        public DateTime IssuedAt => DateTime.UtcNow;
        /// <summary>
        /// exp, Expiration Time, 有效期間
        /// </summary>
        /// <remarks>用來驗證 JWT 是否有效，這個有效期間必須要大於簽發時間。這裡會回傳 IssuedAt + ValidFor 作為有效期間。</remarks>
        public DateTime Expiration => DateTime.UtcNow.Add(ValidFor);
        /// <summary>
        /// 設定 JWT 有效的時間間隔。預設為 5 分鐘。
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromSeconds(300);
        /// <summary>
        /// jti, JWT ID, 唯一身份標識
        /// </summary>
        /// <remarks>主要用來作為一次性 token，從而迴避重放攻擊。預設使用 GUID。</remarks>
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        /// <summary>
        /// 產生 JWT 時所使用的簽名密鑰
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; } = null!;
        public SecurityKey SecurityKey { get; set; } = null!;
        /// <summary>
        /// Token 時效
        /// </summary>
        public TimeSpan IdleTime { get; set; } = TimeSpan.FromSeconds(300);
    }
}
