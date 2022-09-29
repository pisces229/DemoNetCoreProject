using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DemoNetCoreProject.Backend.Services
{
    public class UserBackendService : IUserService
    {
        private readonly CommonUserDto _commonUser;
        public UserBackendService(IHttpContextAccessor httpContextAccessor,
            ILogger<UserBackendService> logger,
            ICache cache,
            IOptions<JwtOption> jwtOptions)
        {
            _commonUser = new CommonUserDto()
            {
                ProgId = "",
                UserId = "_"
            };
            try
            {
                var headerKeys = httpContextAccessor.HttpContext!.Request.Headers.Keys;
                if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var headerValue = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                    var values = headerValue.Split(' ');
                    if (values.Length == 2)
                    {
                        if ("Bearer".Equals(values.First()))
                        {
                            var jwtOption = jwtOptions.Value;
                            var token = values.Last();
                            var tokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = jwtOption.Issuer,
                                ValidateAudience = true,
                                ValidAudience = jwtOption.Audience,
                                ValidateLifetime = false,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = jwtOption.SecurityKey
                            };
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                            var refreshTokenId = claimsPrincipal.Claims
                                .Where(w => w.Type == JwtRegisteredClaimNames.Jti)
                                .Select(s => s.Value)
                                .First();
                            if (!string.IsNullOrEmpty(refreshTokenId))
                            {
                                Task.Run(async () =>
                                {
                                    if (await cache.Exists(refreshTokenId))
                                    {
                                        var cmmonToken = await cache.Get<CommonTokenDto>(refreshTokenId);
                                        _commonUser.UserId = cmmonToken.Account;
                                    }
                                }).Wait();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }
        public string ProgId => _commonUser.ProgId;
        public string UserId => _commonUser.UserId;
    }
}
