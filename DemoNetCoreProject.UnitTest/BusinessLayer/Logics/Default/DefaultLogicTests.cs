using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.BusinessLayer.Profiles;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default;

[TestClass]
public class DefaultLogicTests
{
    private IFixture _fixture = null!;
    private IMapper _mapper = null!;
    private Mock<ILogger<DefaultLogic>> _mockLogger = null!;
    private Mock<IDefaultRepository> _mockDefaultRepository = null!;
    private Mock<IOptions<JwtOption>> _mockOptions = null!;
    private Mock<IUserService> _mockUserService = null!;
    private Mock<ICache> _mockCache = null!;
    private DefaultLogic _defaultLogic = null!;
    private JwtOption _jwtOption = null!;

    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture()
            .Customize(new CommonCustomization())
            .Customize(new AutoMoqCustomization());

        _mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DefaultProfile>()));

        _mockLogger = _fixture.Freeze<Mock<ILogger<DefaultLogic>>>();
        _mockDefaultRepository = _fixture.Freeze<Mock<IDefaultRepository>>();
        _mockOptions = _fixture.Freeze<Mock<IOptions<JwtOption>>>();
        _mockUserService = _fixture.Freeze<Mock<IUserService>>();
        _mockCache = _fixture.Freeze<Mock<ICache>>();

        var secretKey = "CC5B1B93EDA34B788DD2743CA39BAF89";
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        _jwtOption = new JwtOption
        {
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            Issuer = "https://localhost:44337/",
            Subject = "DemoNetCoreProject.Backend",
            Audience = "DemoNetCoreProject.Backend",
            ValidFor = TimeSpan.FromMinutes(1),
            IdleTime = TimeSpan.FromMinutes(5),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            SecurityKey = securityKey
        };
        _mockOptions.Setup(x => x.Value).Returns(_jwtOption);

        _defaultLogic = _fixture.Create<DefaultLogic>();
    }

    /// <summary>
    /// 測試基本執行功能
    /// 驗證重點：
    /// 1. 確保方法正常執行
    /// 2. 驗證日誌記錄是否正確
    /// </summary>
    [TestMethod]
    public async Task Run()
    {
        // Act
        await _defaultLogic.Run();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Run")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// 測試從請求主體接收數據的功能
    /// 驗證重點：
    /// 1. 確保方法成功處理輸入數據
    /// 2. 驗證返回結果的正確性
    /// 3. 確認所有必要的日誌記錄
    /// </summary>
    [TestMethod]
    public async Task FromBody()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicFromBodyInputDto>()
            .With(x => x.Value, "1")
            .With(x => x.Values, new[] { "1", "2" })
            .Create();

        // Act
        var result = await _defaultLogic.FromBody(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("FromBody")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Value:[{input.Value}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Values:[{input.Values.Count()}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// 測試表單數據上傳功能
    /// 驗證重點：
    /// 1. 確保文件上傳邏輯正確執行
    /// 2. 驗證倉儲層方法被正確調用
    /// 3. 確認日誌記錄完整性
    /// </summary>
    [TestMethod]
    public async Task FromForm()
    {
        // Arrange
        _mockDefaultRepository
            .Setup(s => s.Upload(It.IsAny<DefaultRepositoryUploadInputDto>()))
            .ReturnsAsync(true);

        var input = _fixture.Build<DefaultLogicFromFormInputDto>()
            .With(x => x.File, new MemoryStream())
            .With(x => x.Value, "1")
            .With(x => x.Values, ["1", "2"])
            .Create();

        // Act
        var result = await _defaultLogic.FromForm(input);

        // Assert
        _mockDefaultRepository.Verify(s => s.Upload(It.IsAny<DefaultRepositoryUploadInputDto>()), Times.Once);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("FromForm")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Value:[{input.Value}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Values:[{input.Values.Count()}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// 測試查詢參數處理功能
    /// 驗證重點：
    /// 1. 確保正確處理查詢參數
    /// 2. 驗證返回結果
    /// 3. 確認日誌記錄的準確性
    /// </summary>
    [TestMethod]
    public async Task FromQuery()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicFromQueryInputDto>()
            .With(x => x.Value, "1")
            .With(x => x.Values, new[] { "1", "2" })
            .Create();

        // Act
        var result = await _defaultLogic.FromQuery(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("FromQuery")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Value:[{input.Value}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Values:[{input.Values.Count()}]")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// 測試分頁查詢功能
    /// 驗證重點：
    /// 1. 確保分頁參數正確處理
    /// 2. 驗證返回結果的格式
    /// </summary>
    [TestMethod]
    public async Task PageQuery()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicPageQueryInputDto>()
            .With(x => x.Value, "1")
            .With(x => x.Values, new[] { "1", "2" })
            .Create();

        // Act
        var result = await _defaultLogic.PageQuery(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }

    /// <summary>
    /// 測試文件下載功能
    /// 驗證重點：
    /// 1. 確保下載邏輯正確執行
    /// 2. 驗證返回的文件信息準確性
    /// 3. 確認倉儲層方法調用
    /// </summary>
    [TestMethod]
    public async Task Download()
    {
        // Arrange
        var expectedOutput = new CommonOutputDto<CommonDownloadOutputDto>
        {
            Success = true,
            Data = new CommonDownloadOutputDto
            {
                FileName = "FileName",
                FilePath = "FilePath"
            }
        };

        _mockDefaultRepository
            .Setup(s => s.Download())
            .Returns(expectedOutput);

        // Act
        var result = await _defaultLogic.Download();

        // Assert
        _mockDefaultRepository.Verify(s => s.Download(), Times.Once);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(expectedOutput.Data.FileName, result.Data.FileName);
        Assert.AreEqual(expectedOutput.Data.FilePath, result.Data.FilePath);
    }

    /// <summary>
    /// 測試成功登入場景
    /// 驗證重點：
    /// 1. 確保正確生成 JWT Token
    /// 2. 驗證 Token 包含正確的聲明（Claims）
    /// 3. 確認緩存操作正確執行
    /// 4. 驗證 Token 的有效期設置
    /// </summary>
    [TestMethod]
    public async Task SignIn_Success()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicSignInInputDto>()
            .With(x => x.Account, "testAccount")
            .With(x => x.Password, "testPassword")
            .Create();

        _mockCache
            .Setup(s => s.Add(
                It.IsAny<string>(),
                It.IsAny<CommonTokenDto>(),
                It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _defaultLogic.SignIn(input);

        // Assert
        _mockCache.Verify(
            s => s.Add(
                It.IsAny<string>(),
                It.Is<CommonTokenDto>(x => x.Account == input.Account),
                It.Is<TimeSpan>(x => x == _jwtOption.IdleTime)),
            Times.Once);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);

        // 驗證生成的 Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(result.Data);
        Assert.AreEqual(_jwtOption.Issuer, token.Issuer);
        Assert.AreEqual(_jwtOption.Audience, token.Claims.First(x => x.Type == JwtRegisteredClaimNames.Aud).Value);
        Assert.AreEqual(input.Account, token.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
    }

    /// <summary>
    /// 測試帳號為空時的登入失敗場景
    /// 驗證重點：
    /// 1. 確保返回適當的錯誤信息
    /// 2. 驗證不會生成 Token
    /// 3. 確認不會進行緩存操作
    /// </summary>
    [TestMethod]
    public async Task SignIn_Fail_EmptyAccount()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicSignInInputDto>()
            .With(x => x.Account, string.Empty)
            .With(x => x.Password, "testPassword")
            .Create();

        // Act
        var result = await _defaultLogic.SignIn(input);

        // Assert
        _mockCache.Verify(
            s => s.Add(
                It.IsAny<string>(),
                It.IsAny<CommonTokenDto>(),
                It.IsAny<TimeSpan>()),
            Times.Never);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Login Fail", result.Message);
    }

    /// <summary>
    /// 測試密碼為空時的登入失敗場景
    /// 驗證重點：
    /// 1. 確保返回適當的錯誤信息
    /// 2. 驗證不會生成 Token
    /// 3. 確認不會進行緩存操作
    /// </summary>
    [TestMethod]
    public async Task SignIn_Fail_EmptyPassword()
    {
        // Arrange
        var input = _fixture.Build<DefaultLogicSignInInputDto>()
            .With(x => x.Account, "testAccount")
            .With(x => x.Password, string.Empty)
            .Create();

        // Act
        var result = await _defaultLogic.SignIn(input);

        // Assert
        _mockCache.Verify(
            s => s.Add(
                It.IsAny<string>(),
                It.IsAny<CommonTokenDto>(),
                It.IsAny<TimeSpan>()),
            Times.Never);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Login Fail", result.Message);
    }

    /// <summary>
    /// 測試用戶驗證功能
    /// 驗證重點：
    /// 1. 確保正確獲取用戶ID
    /// 2. 驗證返回結果的正確性
    /// </summary>
    [TestMethod]
    public async Task Validate()
    {
        // Arrange
        var userId = "testUser";
        _mockUserService.Setup(x => x.UserId).Returns(userId);

        // Act
        var result = await _defaultLogic.Validate();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(userId, result.Data);
    }

    /// <summary>
    /// 測試成功刷新 Token 的場景
    /// 驗證重點：
    /// 1. 確保正確驗證原有 Token
    /// 2. 驗證緩存中的 Token 信息
    /// 3. 確認新 Token 的生成
    /// 4. 驗證緩存更新操作
    /// </summary>
    [TestMethod]
    public async Task Refresh_Success()
    {
        // Arrange
        var account = "testAccount";
        var refreshTokenId = Guid.NewGuid().ToString();
        var token = GenerateToken(account, refreshTokenId);

        _mockCache.Setup(s => s.Exists(refreshTokenId)).ReturnsAsync(true);
        _mockCache.Setup(s => s.Get<CommonTokenDto>(refreshTokenId))
            .ReturnsAsync(new CommonTokenDto { Account = account });
        _mockCache.Setup(s => s.Replace(
            It.IsAny<string>(),
            It.IsAny<CommonTokenDto>(),
            It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _defaultLogic.Refresh(token);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);

        _mockCache.Verify(s => s.Exists(refreshTokenId), Times.Once);
        _mockCache.Verify(s => s.Get<CommonTokenDto>(refreshTokenId), Times.Once);
        _mockCache.Verify(
            s => s.Replace(
                It.Is<string>(x => x == refreshTokenId),
                It.Is<CommonTokenDto>(x => 
                    x.Account == account),
                It.Is<TimeSpan>(x => x == _jwtOption.IdleTime)),
            Times.Once);

        // 驗證新生成的 Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var newToken = tokenHandler.ReadJwtToken(result.Data);
        Assert.AreEqual(_jwtOption.Issuer, newToken.Issuer);
        Assert.AreEqual(_jwtOption.Audience, newToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Aud).Value);
        Assert.AreEqual(account, newToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
    }

    /// <summary>
    /// 測試 Token 不存在時的刷新失敗場景
    /// 驗證重點：
    /// 1. 確保正確處理不存在的 Token
    /// 2. 驗證返回適當的錯誤信息
    /// 3. 確認不會進行不必要的緩存操作
    /// </summary>
    [TestMethod]
    public async Task Refresh_Fail_TokenNotExist()
    {
        // Arrange
        var account = "testAccount";
        var refreshTokenId = Guid.NewGuid().ToString();
        var token = GenerateToken(account, refreshTokenId);

        _mockCache.Setup(s => s.Exists(refreshTokenId)).ReturnsAsync(false);

        // Act
        var result = await _defaultLogic.Refresh(token);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        _mockCache.Verify(s => s.Exists(refreshTokenId), Times.Once);
        _mockCache.Verify(s => s.Get<CommonTokenDto>(refreshTokenId), Times.Never);
        _mockCache.Verify(s => s.Replace(
            It.IsAny<string>(),
            It.IsAny<CommonTokenDto>(),
            It.IsAny<TimeSpan>()), 
            Times.Never);
    }

    /// <summary>
    /// 測試成功登出場景
    /// 驗證重點：
    /// 1. 確保正確清除 Token 緩存
    /// 2. 驗證返回結果的正確性
    /// </summary>
    [TestMethod]
    public async Task SignOut_Success()
    {
        // Arrange
        var account = "testAccount";
        var refreshTokenId = Guid.NewGuid().ToString();
        var token = GenerateToken(account, refreshTokenId);

        _mockCache.Setup(s => s.Remove(refreshTokenId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _defaultLogic.SignOut(token);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        _mockCache.Verify(s => s.Remove(refreshTokenId), Times.Once);
    }

    /// <summary>
    /// 測試無效 Token 的登出場景
    /// 驗證重點：
    /// 1. 確保優雅處理無效 Token
    /// 2. 驗證錯誤日誌記錄
    /// 3. 確認不會進行緩存操作
    /// 4. 返回失敗結果
    /// </summary>
    [TestMethod]
    public async Task SignOut_InvalidToken()
    {
        // Arrange
        var invalidToken = "invalidToken";

        // Act
        var result = await _defaultLogic.SignOut(invalidToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        _mockCache.Verify(s => s.Remove(It.IsAny<string>()), Times.Never);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private string GenerateToken(string account, string refreshTokenId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Iss, _jwtOption.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _jwtOption.Audience),
            new Claim(JwtRegisteredClaimNames.Sub, account),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, refreshTokenId)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: _jwtOption.SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}