using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace DemoNetCoreProject.Backend.Filters
{
    public class JwtAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<JwtAuthorizationFilter> _logger;
        private readonly ICache _cache;
        private readonly JwtOption _jwtOption;
        public JwtAuthorizationFilter(ILogger<JwtAuthorizationFilter> logger,
            ICache cache,
            IOptions<JwtOption> jwtOption)
        {
            _logger = logger;
            _cache = cache;
            _jwtOption = jwtOption.Value;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var headerKeys = context.HttpContext.Request.Headers.Keys;
            var httpStatusCode = HttpStatusCode.Forbidden;
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                try
                {
                    var headerValue = context.HttpContext.Request.Headers["Authorization"].ToString();
                    var values = headerValue.Split(' ');
                    if (values.Length == 2)
                    {
                        if ("Bearer".Equals(values.First()))
                        {
                            var token = values.Last();
                            var tokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = _jwtOption.Issuer,
                                ValidateAudience = true,
                                ValidAudience = _jwtOption.Audience,
                                ValidateLifetime = false,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = _jwtOption.SecurityKey
                            };
                            var securityToken = default(SecurityToken);
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                            if (securityToken!.ValidTo > DateTime.UtcNow)
                            {
                                var refreshTokenId = claimsPrincipal.Claims
                                    .Where(w => w.Type == JwtRegisteredClaimNames.Jti)
                                    .Select(s => s.Value)
                                    .First();
                                if (await _cache.Exists(refreshTokenId))
                                {
                                    httpStatusCode = HttpStatusCode.OK;
                                }
                            }
                            else
                            {
                                httpStatusCode = HttpStatusCode.Unauthorized;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            }
            if (httpStatusCode == HttpStatusCode.Unauthorized)
            {
                context.Result = new UnauthorizedResult();
            }
            else if (httpStatusCode == HttpStatusCode.Forbidden)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
