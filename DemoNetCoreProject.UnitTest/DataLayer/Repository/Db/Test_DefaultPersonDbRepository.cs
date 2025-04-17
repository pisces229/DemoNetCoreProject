using AutoFixture;
using AutoFixture.AutoMoq;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Repositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace DemoNetCoreProject.UnitTest.DataLayer.Repository.Db
{
    /// <summary>
    /// DefaultPersonDbRepository 的單元測試類別
    /// 主要測試人員資料庫存取的相關功能
    /// 使用 AutoFixture 和 Moq 框架進行測試資料準備和模擬
    /// </summary>
    [TestClass]
    public class Test_DefaultPersonDbRepository
    {
        private IFixture _fixture = null!;

        private Mock<DefaultDbContext> _mockContext = null!;
        private Mock<DbSet<Person>> _mockDbSet = null!;
        private List<Person> _people = null!;

        private DefaultPersonDbRepository _repository = null!;

        /// <summary>
        /// 測試初始化方法
        /// 設置測試環境，包括：
        /// 1. 配置 AutoFixture 用於生成測試資料
        /// 2. 模擬 DbContext 和 DbSet
        /// 3. 建立測試用的人員資料集合
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

            _people = _fixture.CreateMany<Person>(10).ToList();
            _mockDbSet = _people.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(x => x.Set<Person>()).Returns(_mockDbSet.Object);

            _fixture.Register(() => _mockContext.Object);

            _repository = _fixture.Create<DefaultPersonDbRepository>();
        }

        /// <summary>
        /// 測試目的：驗證當人員存在時，GetByName 方法能正確返回對應的人員
        /// 測試重點：
        /// 1. 確保能夠根據人員名稱準確查詢到對應的人員記錄
        /// 2. 驗證返回的人員資料與預期資料相符
        /// </summary>
        [TestMethod]
        public async Task GetByName_WhenPersonExists_ShouldReturnPerson()
        {
            // Arrange
            var expectedPerson = _fixture.Create<Person>();
            _people.Add(expectedPerson);

            // Act
            var result = await _repository.GetByName(expectedPerson.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Name, result.Name);
        }

        /// <summary>
        /// 測試目的：驗證當人員不存在時，GetByName 方法應該返回 null
        /// 測試重點：
        /// 1. 使用不存在的人員名稱進行查詢
        /// 2. 確認方法正確處理查詢不到資料的情況
        /// 3. 驗證返回值為 null
        /// </summary>
        [TestMethod]
        public async Task GetByName_WhenPersonDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var nonExistentName = _fixture.Create<string>();

            // Act
            var result = await _repository.GetByName(nonExistentName);

            // Assert
            Assert.IsNull(result);
        }
    }
} 