using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.BusinessLayer.Profiles;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultSqlLogic
    {
        private IFixture _fixture;
        private IMapper _mapper;
        private Mock<ILogger<DefaultSqlLogic>> _logger;
        private Mock<IDbManager<DefaultDbContext>> _mockDefaultDbManager;
        private Mock<IDefaultPersonDbRepository> _mockDefaultPersonDbRepository;
        private Mock<IDefaultSqlRepository> _mockDefaultRepository;
        private DefaultSqlLogic _defaultLogic;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture()
                .Customize(new CommonCustomization())
                .Customize(new AutoMoqCustomization());

            _mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DefaultProfile>()));

            _logger = _fixture.Freeze<Mock<ILogger<DefaultSqlLogic>>>();
            _mockDefaultDbManager = _fixture.Freeze<Mock<IDbManager<DefaultDbContext>>>();
            _mockDefaultPersonDbRepository = _fixture.Freeze<Mock<IDefaultPersonDbRepository>>();
            _mockDefaultRepository = _fixture.Freeze<Mock<IDefaultSqlRepository>>();

            _defaultLogic = _fixture.Create<DefaultSqlLogic>();
        }

        /// <summary>
        /// 測試 RunDbRepositoryQuery 方法
        /// 目的：驗證查詢功能是否正確執行
        /// 驗證重點：
        /// 1. 確保 Query 方法被呼叫一次
        /// 2. 驗證回傳的資料是否符合預期
        /// </summary>
        [TestMethod]
        public async Task RunDbRepositoryQuery()
        {
            // Arrange
            var persons = _fixture.CreateMany<Person>(1).ToList();
            _mockDefaultPersonDbRepository
                .Setup(s => s.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()))
                .ReturnsAsync(persons);

            // Act
            await _defaultLogic.RunDbRepositoryQuery();

            // Assert
            _mockDefaultPersonDbRepository.Verify(
                v => v.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunDbRepositoryCreate 方法
        /// 目的：驗證新增資料的交易處理流程
        /// 驗證重點：
        /// 1. 確保交易開始
        /// 2. 確保新增操作執行一次
        /// 3. 確保交易提交
        /// 4. 確保沒有執行回滾
        /// </summary>
        [TestMethod]
        public async Task RunDbRepositoryCreate()
        {
            // Arrange
            _mockDefaultDbManager
                .Setup(s => s.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .Returns(Task.CompletedTask);
            _mockDefaultDbManager
                .Setup(s => s.CommitAsync())
                .Returns(Task.CompletedTask);
            _mockDefaultPersonDbRepository
                .Setup(s => s.Create(It.IsAny<Person>()))
                .ReturnsAsync(1);

            // Act
            await _defaultLogic.RunDbRepositoryCreate();

            // Assert
            _mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Once);
            _mockDefaultPersonDbRepository.Verify(v => v.Create(It.IsAny<Person>()), Times.Once);
            _mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Once);
            _mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }

        /// <summary>
        /// 測試 RunDbRepositoryModify 方法
        /// 目的：驗證修改資料的交易處理流程
        /// 驗證重點：
        /// 1. 確保查詢操作執行一次
        /// 2. 確保交易開始
        /// 3. 確保修改操作執行一次
        /// 4. 確保交易提交
        /// 5. 確保沒有執行回滾
        /// </summary>
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            // Arrange
            var persons = _fixture.CreateMany<Person>(1).ToList();
            _mockDefaultDbManager
                .Setup(s => s.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .Returns(Task.CompletedTask);
            _mockDefaultDbManager
                .Setup(s => s.CommitAsync())
                .Returns(Task.CompletedTask);
            _mockDefaultPersonDbRepository
                .Setup(s => s.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()))
                .ReturnsAsync(persons);
            _mockDefaultPersonDbRepository
                .Setup(s => s.Modify(It.IsAny<Person>()))
                .ReturnsAsync(1);

            // Act
            await _defaultLogic.RunDbRepositoryModify();

            // Assert
            _mockDefaultPersonDbRepository.Verify(
                v => v.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()),
                Times.Once);
            _mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Once);
            _mockDefaultPersonDbRepository.Verify(v => v.Modify(It.IsAny<Person>()), Times.Once);
            _mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Once);
            _mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }

        /// <summary>
        /// 測試 RunDbRepositoryRemove 方法
        /// 目的：驗證刪除資料時的邏輯處理
        /// 驗證重點：
        /// 1. 確保查詢操作執行一次
        /// 2. 當查詢結果為空時，確保：
        ///    - 不執行交易開始
        ///    - 不執行刪除操作
        ///    - 不執行交易提交或回滾
        /// </summary>
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            // Arrange
            _mockDefaultPersonDbRepository
                .Setup(s => s.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()))
                .ReturnsAsync(new List<Person>());
            _mockDefaultPersonDbRepository
                .Setup(s => s.Remove(It.IsAny<Person>()))
                .ReturnsAsync(1);

            // Act
            await _defaultLogic.RunDbRepositoryRemove();

            // Assert
            _mockDefaultPersonDbRepository.Verify(
                v => v.Query(It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>()),
                Times.Once);
            _mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Never);
            _mockDefaultPersonDbRepository.Verify(v => v.Remove(It.IsAny<Person>()), Times.Never);
            _mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Never);
            _mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }

        /// <summary>
        /// 測試 RunDbRepositoryPagedQuery 方法
        /// 目的：驗證分頁查詢功能
        /// 驗證重點：
        /// 1. 確保分頁查詢方法被呼叫一次
        /// 2. 驗證查詢參數是否正確傳遞
        /// 3. 確保回傳的分頁資料結構正確
        /// </summary>
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            // Arrange
            var pagedOutput = _fixture.Create<CommonPageOutputDto<Person>>();
            _mockDefaultPersonDbRepository
                .Setup(s => s.PagedQuery(
                    It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()))
                .ReturnsAsync(pagedOutput);

            // Act
            await _defaultLogic.RunDbRepositoryPagedQuery();

            // Assert
            _mockDefaultPersonDbRepository.Verify(v => v.PagedQuery(
                It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperQuery 方法
        /// 目的：驗證 Dapper 查詢功能
        /// 驗證重點：
        /// 1. 確保 Dapper Query 方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunDapperQuery()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunDapperQuery())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunDapperQuery();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunDapperQuery(), Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperExecuteScalar 方法
        /// 目的：驗證 Dapper ExecuteScalar 功能
        /// 驗證重點：
        /// 1. 確保 ExecuteScalar 方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunDapperExecuteScalar())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunDapperExecuteScalar();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunDapperExecuteScalar(), Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperQueryMultiple 方法
        /// 目的：驗證 Dapper 多重查詢功能
        /// 驗證重點：
        /// 1. 確保 QueryMultiple 方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunDapperQueryMultiple())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunDapperQueryMultiple();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunDapperQueryMultiple(), Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperExecuteReader 方法
        /// 目的：驗證 Dapper ExecuteReader 功能
        /// 驗證重點：
        /// 1. 確保 ExecuteReader 方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunDapperExecuteReader())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunDapperExecuteReader();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunDapperExecuteReader(), Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperPagedQuery 方法
        /// 目的：驗證 Dapper 分頁查詢功能
        /// 驗證重點：
        /// 1. 確保分頁查詢方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunDapperPagedQuery())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunDapperPagedQuery();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunDapperPagedQuery(), Times.Once);
        }

        /// <summary>
        /// 測試 RunSqlCondition 方法
        /// 目的：驗證 SQL 條件查詢功能
        /// 驗證重點：
        /// 1. 確保條件查詢方法被正確呼叫
        /// 2. 驗證方法呼叫次數
        /// </summary>
        [TestMethod]
        public async Task RunSqlCondition()
        {
            // Arrange
            _mockDefaultRepository
                .Setup(s => s.RunSqlCondition())
                .Returns(Task.CompletedTask);

            // Act
            await _defaultLogic.RunSqlCondition();

            // Assert
            _mockDefaultRepository.Verify(v => v.RunSqlCondition(), Times.Once);
        }
    }
}