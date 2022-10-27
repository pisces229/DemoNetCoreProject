using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultRequestLogic : IDefaultRequestLogic
    {
        private readonly ILogger<DefaultRequestLogic> _logger;
        private readonly IDefaultDataProtector _defaultDataProtector;
        private readonly IMapper _mapper;
        private readonly JwtOption _jwtOption;
        private readonly IUserService _userService;
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;
        public DefaultRequestLogic(ILogger<DefaultRequestLogic> logger,
            IDefaultDataProtector defaultDataProtector,
            IMapper mapper,
            IOptions<JwtOption> JwtOptions,
            IUserService userService,
            ICache cache,
            IConfiguration configuration)
        {
            _logger = logger;
            _defaultDataProtector = defaultDataProtector;
            _mapper = mapper;
            _jwtOption = JwtOptions.Value;
            _userService = userService;
            _cache = cache;
            _configuration = configuration;
        }
        // _defaultDataProtector.Protect("Value")
        // _defaultDataProtector.UnProtect("Value")
        public async Task<CommonResponseDto<string>> SignIn(DefaultRequestLogicSignInInputDto model)
        {
            var result = new CommonResponseDto<string>();
            if (!string.IsNullOrEmpty(model.Account) && !string.IsNullOrEmpty(model.Password))
            {
                var commonTokenModel = new CommonTokenDto()
                {
                    Account = model.Account,
                    Expiration = _jwtOption.Expiration
                };
                var refreshTokenId = Guid.NewGuid().ToString();
                await _cache.Add(refreshTokenId, commonTokenModel, _jwtOption.IdleTime);
                result.Data = GenerateToken(model.Account, refreshTokenId);
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = "Login Fail";
            }
            return result;
        }
        public async Task<CommonResponseDto<string>> Validate()
        {
            var result = new CommonResponseDto<string>()
            {
                Success = true,
                Data = _userService.UserId
            };
            return await Task.FromResult(result);
        }
        public async Task<CommonResponseDto<string>> Refresh(string model)
        {
            var result = new CommonResponseDto<string>();
            try
            {
                var tokenValidationParameters = CreateTokenValidationParameters;
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(model, tokenValidationParameters, out _);
                //claimsPrincipal.Claims.ToList().ForEach(f =>
                //{
                //    _logger.LogInformation($"{f.Type}:{f.Value}");
                //});
                var refreshTokenId = claimsPrincipal.Claims
                    .Where(w => w.Type == JwtRegisteredClaimNames.Jti)
                    .Select(s => s.Value)
                    .First();
                if (await _cache.Exists(refreshTokenId))
                {
                    var commonTokenModel = await _cache.Get<CommonTokenDto>(refreshTokenId);
                    commonTokenModel.Expiration = _jwtOption.Expiration;
                    await _cache.Replace(refreshTokenId, commonTokenModel, _jwtOption.IdleTime);
                    result.Data = GenerateToken(commonTokenModel.Account, refreshTokenId);
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return result;
        }
        public async Task SignOut(string model)
        {
            try
            {
                var tokenValidationParameters = CreateTokenValidationParameters;
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(model, tokenValidationParameters, out _);
                //claimsPrincipal.Claims.ToList().ForEach(f =>
                //{
                //    Console.WriteLine($"{f.Type}:{f.Value}");
                //});
                var refreshTokenId = claimsPrincipal.Claims
                    .Where(w => w.Type == JwtRegisteredClaimNames.Jti)
                    .Select(s => s.Value)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(refreshTokenId))
                {
                    await _cache.Remove(refreshTokenId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
        public async Task<CommonResponseDto<DefaultRequestLogicJsonOutputDto>> JsonHttpGet(DefaultRequestLogicJsonHttpGetInputDto model)
        {
            var result = new CommonResponseDto<DefaultRequestLogicJsonOutputDto>()
            {
                Success = true,
                Data = _mapper.Map<DefaultRequestLogicJsonHttpGetInputDto, DefaultRequestLogicJsonOutputDto>(model)
            };
            return await Task.FromResult(result);
        }
        public async Task<CommonResponseDto<DefaultRequestLogicJsonOutputDto>> JsonHttpPost(DefaultRequestLogicJsonHttpPostInputDto model)
        {
            var result = new CommonResponseDto<DefaultRequestLogicJsonOutputDto>()
            {
                Success = true,
                Data = _mapper.Map<DefaultRequestLogicJsonHttpPostInputDto, DefaultRequestLogicJsonOutputDto>(model)
            };
            return await Task.FromResult(result);
        }
        public async Task<CommonPagedResultDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQuery(CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto> model)
        {
            var result = new CommonPagedResultDto<DefaultRequestLogicJsonOutputDto>()
            {
                Page = model.Page!,
                Data = new List<DefaultRequestLogicJsonOutputDto>()
                {
                    _mapper.Map<DefaultRequestLogicJsonHttpPostInputDto, DefaultRequestLogicJsonOutputDto>(model.Data!)
                }
            };
            return await Task.FromResult(result);
        }
        public async Task<CommonResponseDto<string>> Upload(DefaultRequestLogicUploadInputDto model)
        {
            var result = new CommonResponseDto<string>();
            if (model != null && model.File != null && model.File.Length > 0)
            {
                var name = !string.IsNullOrEmpty(model.Name) ? model.Name : model.File.FileName;
                var fileInfo = FileUtility.GetFile(
                    Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                    name);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                using (var stream = fileInfo.Create())
                {
                    await model.File.CopyToAsync(stream);
                }
                result.Success = true;
            }
            else
            {
                result.Message = "Please Select File";
            }
            return result;
        }
        public async Task<CommonResponseDto<CommonDownloadDto>> Download()
        {
            var result = new CommonResponseDto<CommonDownloadDto>();
            var fileInfo = FileUtility.GetFile(
                Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                "temp.zip");
            if (fileInfo.Exists)
            {
                result.Success = true;
                result.Data = new CommonDownloadDto()
                {
                    Filename = "Download.zip",
                    FileInfo = fileInfo
                };
            }
            else
            {
                result.Message = "File is not exist.";
            }
            return await Task.FromResult(result);
        }
        private string GenerateToken(string account, string refreshTokenId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOption.Issuer,
                Audience = _jwtOption.Audience,
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtOption.Issuer),
                    new Claim(JwtRegisteredClaimNames.Aud, _jwtOption.Audience),
                    new Claim(JwtRegisteredClaimNames.Sub, account),
                    new Claim(JwtRegisteredClaimNames.Exp, _jwtOption.Expiration.Ticks.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, refreshTokenId),
                }),
                Expires = _jwtOption.Expiration,
                SigningCredentials = _jwtOption.SigningCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);
            return serializeToken;
        }
        private TokenValidationParameters CreateTokenValidationParameters =>
            new()
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOption.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOption.Audience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtOption.SecurityKey
            };
    }
}
