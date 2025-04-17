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
    /// DbRepository 的單元測試類別
    /// 主要測試通用資料庫存取的基本功能
    /// 使用 AutoFixture 和 Moq 框架進行測試資料準備和模擬
    /// </summary>
    [TestClass]
    public class Test_DbRepository
    {
        private IFixture _fixture = null!;

        private Mock<DefaultDbContext> _mockContext = null!;
        private Mock<DbSet<Person>> _mockDbSet = null!;
        private List<Person> _people = null!;

        private DbRepository<DefaultDbContext, Person> _repository = null!;

        /// <summary>
        /// 測試初始化方法
        /// 設置測試環境，包括：
        /// 1. 配置 AutoFixture 用於生成測試資料
        /// 2. 模擬 DbContext 和 DbSet
        /// 3. 建立測試用的資料集合
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

            _repository = _fixture.Create<DbRepository<DefaultDbContext, Person>>();
        }

        /// <summary>
        /// 測試目的：驗證 Find 方法能正確返回指定 Row 的實體
        /// 測試重點：
        /// 1. 確保能夠根據 Row 準確查詢到對應的記錄
        /// 2. 驗證返回的資料與預期資料相符
        /// </summary>
        [TestMethod]
        public async Task Find_ShouldReturnCorrectEntity()
        {
            // Arrange
            var expectedPerson = _people.First();
            _mockDbSet.Setup(x => x.FindAsync(expectedPerson.Row))
                .ReturnsAsync(expectedPerson);

            // Act
            var result = await _repository.Find(expectedPerson.Row);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Row, result.Row);
        }

        /// <summary>
        /// 測試目的：驗證 Any 方法在有符合條件的資料時返回 true
        /// 測試重點：
        /// 1. 確保能夠正確判斷資料存在性
        /// 2. 驗證條件篩選功能正常運作
        /// </summary>
        [TestMethod]
        public async Task Any_WhenDataExists_ShouldReturnTrue()
        {
            // Arrange
            var targetAge = _people.First().Age;
            Func<IQueryable<Person>, IQueryable<Person>> where =
                query => query.Where(p => p.Age == targetAge);

            // Act
            var result = await _repository.Any(where);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 測試目的：驗證 Count 方法能正確計算符合條件的資料數量
        /// 測試重點：
        /// 1. 確保能夠正確計算資料數量
        /// 2. 驗證條件篩選功能正常運作
        /// </summary>
        [TestMethod]
        public async Task Count_ShouldReturnCorrectCount()
        {
            // Arrange
            var targetAge = _people.First().Age;
            Func<IQueryable<Person>, IQueryable<Person>> query =
                q => q.Where(p => p.Age == targetAge);

            // Act
            var result = await _repository.Count(query);

            // Assert
            var expectedCount = _people.Count(p => p.Age == targetAge);
            Assert.AreEqual(expectedCount, result);
        }

        /// <summary>
        /// 測試目的：驗證 Query 方法能正確返回符合條件的資料集合
        /// 測試重點：
        /// 1. 確保能夠正確篩選資料
        /// 2. 驗證返回的資料集合與預期相符
        /// </summary>
        [TestMethod]
        public async Task Query_ShouldReturnFilteredData()
        {
            // Arrange
            var targetAge = _people.First().Age;
            Func<IQueryable<Person>, IQueryable<Person>> query =
                q => q.Where(p => p.Age == targetAge);

            // Act
            var result = await _repository.Query(query);

            // Assert
            var expectedData = _people.Where(p => p.Age == targetAge);
            CollectionAssert.AreEqual(expectedData.ToList(), result.ToList());
        }

        /// <summary>
        /// 測試目的：驗證 PagedQuery 方法能正確返回分頁資料
        /// 測試重點：
        /// 1. 確保能夠正確進行分頁
        /// 2. 驗證返回的分頁資料與預期相符
        /// </summary>
        [TestMethod]
        public async Task PagedQuery_ShouldReturnCorrectPagedData()
        {
            // Arrange
            int pageSize = 5;
            int pageNo = 1;
            Func<IQueryable<Person>, IOrderedQueryable<Person>> order =
                q => q.OrderBy(p => p.Row);

            // Act
            var result = await _repository.PagedQuery(pageSize, pageNo, order: order);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_people.Count, result.TotalCount);
            Assert.AreEqual(pageSize, result.Data.Count());
            CollectionAssert.AreEqual(
                _people.OrderBy(p => p.Row).Take(pageSize).ToList(),
                result.Data.ToList());
        }

        /// <summary>
        /// 測試目的：驗證 Create 方法能正確新增資料
        /// 測試重點：
        /// 1. 確保能夠正確新增資料
        /// 2. 驗證 SaveChanges 被正確調用
        /// </summary>
        [TestMethod]
        public async Task Create_ShouldAddEntityAndSaveChanges()
        {
            // Arrange
            var newPerson = _fixture.Create<Person>();
            _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _repository.Create(newPerson);

            // Assert
            Assert.AreEqual(1, result);
            _mockDbSet.Verify(x => x.Add(newPerson), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// 測試目的：驗證 Modify 方法能正確更新資料
        /// 測試重點：
        /// 1. 確保能夠正確更新資料
        /// 2. 驗證 SaveChanges 被正確調用
        /// </summary>
        [TestMethod]
        public async Task Modify_ShouldUpdateEntityAndSaveChanges()
        {
            // Arrange
            var person = _people.First();
            _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _repository.Modify(person);

            // Assert
            Assert.AreEqual(1, result);
            _mockDbSet.Verify(x => x.Update(person), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// 測試目的：驗證 Remove 方法能正確刪除資料
        /// 測試重點：
        /// 1. 確保能夠正確刪除資料
        /// 2. 驗證 SaveChanges 被正確調用
        /// </summary>
        [TestMethod]
        public async Task Remove_ShouldDeleteEntityAndSaveChanges()
        {
            // Arrange
            var person = _people.First();
            _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _repository.Remove(person);

            // Assert
            Assert.AreEqual(1, result);
            _mockDbSet.Verify(x => x.Remove(person), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}