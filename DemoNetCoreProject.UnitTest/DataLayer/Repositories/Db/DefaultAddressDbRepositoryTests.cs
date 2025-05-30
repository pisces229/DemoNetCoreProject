using AutoFixture;
using AutoFixture.AutoMoq;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Repositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace DemoNetCoreProject.UnitTest.DataLayer.Repositories.Db;

/// <summary>
/// DefaultAddressDbRepository 的單元測試類別
/// 主要測試地址資料庫存取的相關功能
/// 使用 AutoFixture 和 Moq 框架進行測試資料準備和模擬
/// </summary>
[TestClass]
public class DefaultAddressDbRepositoryTests
{
    private IFixture _fixture = null!;

    private Mock<DefaultDbContext> _mockContext = null!;
    private Mock<DbSet<Address>> _mockDbSet = null!;
    private List<Address> _addresses = null!;

    private DefaultAddressDbRepository _repository = null!;

    /// <summary>
    /// 測試初始化方法
    /// 設置測試環境，包括：
    /// 1. 配置 AutoFixture 用於生成測試資料
    /// 2. 模擬 DbContext 和 DbSet
    /// 3. 建立測試用的地址資料集合
    /// 4. 初始化被測試的 Repository
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture()
            .Customize(new CommonCustomization())
            .Customize(new AutoMoqCustomization());

        var options = new DbContextOptionsBuilder<DefaultDbContext>()
            .Options;
        _mockContext = new Mock<DefaultDbContext>(options);

        _addresses = _fixture.CreateMany<Address>(10).ToList();
        _mockDbSet = _addresses.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Address>()).Returns(_mockDbSet.Object);

        _fixture.Register(() => _mockContext.Object);

        _repository = _fixture.Create<DefaultAddressDbRepository>();
    }

    /// <summary>
    /// 測試目的：驗證當地址存在時，GetByText 方法能正確返回對應的地址
    /// 測試重點：
    /// 1. 確保能夠根據地址文字準確查詢到對應的地址記錄
    /// 2. 驗證返回的地址資料與預期資料相符
    /// </summary>
    [TestMethod]
    public async Task GetByText_WhenAddressExists_ShouldReturnAddress()
    {
        // Arrange
        var expectedAddress = _fixture.Create<Address>();
        _addresses.Add(expectedAddress);

        // Act
        var result = await _repository.GetByText(expectedAddress.Text);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedAddress.Text, result.Text);
    }

    /// <summary>
    /// 測試目的：驗證當地址不存在時，GetByText 方法應該返回 null
    /// 測試重點：
    /// 1. 使用不存在的地址文字進行查詢
    /// 2. 確認方法正確處理查詢不到資料的情況
    /// 3. 驗證返回值為 null
    /// </summary>
    [TestMethod]
    public async Task GetByText_WhenAddressDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var nonExistentText = _fixture.Create<string>();

        // Act
        var result = await _repository.GetByText(nonExistentText);

        // Assert
        Assert.IsNull(result);
    }
}
