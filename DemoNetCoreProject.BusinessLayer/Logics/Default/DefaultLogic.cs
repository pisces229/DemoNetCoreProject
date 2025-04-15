using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    public class DefaultLogic(ILogger<DefaultLogic> _logger,
        IDefaultRepository _defaultRepository,
        IMapper _mapper,
        IOptions<JwtOption> _jwtOptions,
        IUserService _userService,
        ICache _cache) : IDefaultLogic
    {
        private readonly JwtOption _jwtOption = _jwtOptions.Value;

        public async Task Run()
        {
            await Task.Run(() => _logger.LogInformation("Run"));
        }
        public async Task<CommonOutputDto<string>> FromBody(DefaultLogicFromBodyInputDto inputDto)
        {
            await Task.Run(() => _logger.LogInformation("FromBody"));
            var result = new CommonOutputDto<string>()
            {
                Success = true
            };
            _logger.LogInformation("Value:[{v}]", inputDto.Value);
            _logger.LogInformation("Values:[{v}]", inputDto.Values?.Count());
            return result;
        }
        public async Task<CommonOutputDto<string>> FromForm(DefaultLogicFromFormInputDto inputDto)
        {
            await Task.Run(() => _logger.LogInformation("FromForm"));
            var result = new CommonOutputDto<string>()
            {
                Success = true
            };
            _logger.LogInformation("Value:[{v}]", inputDto.Value);
            _logger.LogInformation("Values:[{v}]", inputDto.Values?.Count());
            using (inputDto.File)
            {
                var input = _mapper.Map<DefaultLogicFromFormInputDto, DefaultRepositoryUploadInputDto>(inputDto);
                result.Success = await _defaultRepository.Upload(input);
            }
            return result;
        }
        public async Task<CommonOutputDto<string>> FromQuery(DefaultLogicFromQueryInputDto inputDto)
        {
            await Task.Run(() => _logger.LogInformation("FromQuery"));
            var result = new CommonOutputDto<string>()
            {
                Success = true
            };
            _logger.LogInformation("Value:[{v}]", inputDto.Value);
            _logger.LogInformation("Values:[{v}]", inputDto.Values?.Count());
            return result;
        }
        public async Task<CommonOutputDto<CommonPageOutputDto<DefaultLogicPageQueryOutputDto>>> PageQuery(DefaultLogicPageQueryInputDto inputDto)
        {
            await Task.Run(() => _logger.LogInformation("PageQuery"));
            var result = new CommonOutputDto<CommonPageOutputDto<DefaultLogicPageQueryOutputDto>>()
            {
                Success = true,
                Data = new CommonPageOutputDto<DefaultLogicPageQueryOutputDto>()
                {
                    TotalCount = 10,
                    Data = new List<DefaultLogicPageQueryOutputDto>()
                    {
                        new DefaultLogicPageQueryOutputDto()
                        {
                            Value = "1",
                        },
                        new DefaultLogicPageQueryOutputDto()
                        {
                            Value = "2",
                        }
                    },
                },
            };
            _logger.LogInformation("PageSize:[{v}]", inputDto.PageSize);
            _logger.LogInformation("PageNo:[{v}]", inputDto.PageNo);
            _logger.LogInformation("Value:[{v}]", inputDto.Value);
            _logger.LogInformation("Values:[{v}]", inputDto.Values?.Count());
            return result;
        }
        public async Task<CommonOutputDto<CommonDownloadOutputDto>> Download()
        {
            var result = new CommonOutputDto<CommonDownloadOutputDto>();
            var downloadResult = _defaultRepository.Download();
            if (downloadResult.Success)
            {
                result.Success = true;
                result.Data = downloadResult.Data;
            }
            else
            {
                result.Message = downloadResult.Message;
            }
            return await Task.FromResult(result);
        }
        public async Task<CommonOutputDto<string>> SignIn(DefaultLogicSignInInputDto model)
        {
            var result = new CommonOutputDto<string>();
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
        public async Task<CommonOutputDto<string>> Validate()
        {
            var result = new CommonOutputDto<string>()
            {
                Success = true,
                Data = _userService.UserId
            };
            return await Task.FromResult(result);
        }
        public async Task<CommonOutputDto<string>> Refresh(string model)
        {
            var result = new CommonOutputDto<string>();
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
                _logger.LogError(0, e, "");
            }
            return result;
        }
        public async Task<CommonOutputDto<string>> SignOut(string model)
        {
            var result = new CommonOutputDto<string>();
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
                result.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "");
            }
            return result;
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
