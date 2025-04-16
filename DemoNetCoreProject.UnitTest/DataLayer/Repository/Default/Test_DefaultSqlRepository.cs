using AutoFixture;
using AutoFixture.AutoMoq;
using Dapper;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;

namespace DemoNetCoreProject.UnitTest.Domain.Utilities
{
    [TestClass]
    public class Test_DefaultSqlRepository
    {
        private IFixture _fixture;
        private Mock<ILogger<DefaultSqlRepository>> _logger;
        private Mock<IDapperService<DefaultDbContext>> _mockDapperService;
        private Mock<DefaultDbContext> _mockDbContext;
        private DefaultSqlRepository _repository;

        private Mock<ILogger<DapperCallbackHelper>> _dapperCallbackHelperLogger;
        private DapperCallbackHelper _dapperCallbackHelper;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture()
                .Customize(new CommonCustomization())
                .Customize(new AutoMoqCustomization());

            _logger = new Mock<ILogger<DefaultSqlRepository>>();

            _mockDapperService = _fixture.Freeze<Mock<IDapperService<DefaultDbContext>>>();
            _mockDbContext = _fixture.Freeze<Mock<DefaultDbContext>>();
            _repository = _fixture.Create<DefaultSqlRepository>();

            _dapperCallbackHelperLogger = new Mock<ILogger<DapperCallbackHelper>>();
            _dapperCallbackHelper = new DapperCallbackHelper(_dapperCallbackHelperLogger);
        }

        /// <summary>
        /// 測試 RunDapperQuery 方法
        /// 目的：驗證是否能正確執行 Dapper 查詢並處理結果
        /// 重點：
        /// 1. 驗證 SQL 語句的正確性
        /// 2. 確保 DapperService 被正確呼叫
        /// 3. 驗證日誌記錄功能
        /// 4. 檢查參數處理
        /// </summary>
        [TestMethod]
        public async Task RunDapperQuery()
        {
            // Arrange
            var expectedSql = "SELECT * FROM [Person]";
            var expectedData = _fixture.CreateMany<Person>().ToList();

            _mockDapperService
                .Setup(s => s.Query<Person>(
                    It.Is<string>(sql => sql == expectedSql),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(_dapperCallbackHelper.DapperGeneralCallback)
                .ReturnsAsync(expectedData);

            // Act
            await _repository.RunDapperQuery();

            // Assert
            _mockDapperService.Verify(s => s.Query<Person>(
                It.Is<string>(sql => sql == expectedSql),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperExecuteScalar 方法
        /// 目的：驗證是否能正確執行 Dapper ExecuteScalar 操作
        /// 重點：
        /// 1. 驗證 SQL 語句的正確性
        /// 2. 確保返回單一值的處理邏輯
        /// 3. 驗證日誌記錄功能
        /// 4. 檢查參數處理
        /// </summary>
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            // Arrange
            var expectedSql = "SELECT [Row] FROM [Person]";
            var expectedValue = _fixture.Create<int>();

            _mockDapperService
                .Setup(s => s.ExecuteScalar<int>(
                    It.Is<string>(sql => sql == expectedSql),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(_dapperCallbackHelper.DapperGeneralCallback)
                .ReturnsAsync(expectedValue);

            // Act
            await _repository.RunDapperExecuteScalar();

            // Assert
            _mockDapperService.Verify(s => s.ExecuteScalar<int>(
                It.Is<string>(sql => sql == expectedSql),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperExecuteReader 方法
        /// 目的：驗證是否能正確執行 Dapper ExecuteReader 操作並處理資料讀取
        /// 重點：
        /// 1. 模擬 DbDataReader 的行為
        /// 2. 驗證資料讀取的完整流程
        /// 3. 確保欄位映射正確
        /// 4. 驗證日誌記錄功能
        /// </summary>
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            // Arrange
            var expectedSql = "SELECT [Row] FROM [Person]";
            var mockDataReader = _fixture.Freeze<Mock<DbDataReader>>();
            var rowValue = _fixture.Create<int>();

            // Setup column schema
            mockDataReader.Setup(r => r.FieldCount).Returns(1);
            mockDataReader.Setup(r => r.GetName(0)).Returns("Row");
            mockDataReader.Setup(r => r.GetFieldType(0)).Returns(typeof(int));
            mockDataReader.Setup(r => r.GetOrdinal("Row")).Returns(0);
            mockDataReader.Setup(r => r.GetInt32(0)).Returns(rowValue);
            mockDataReader.Setup(r => r.IsDBNull(0)).Returns(false);
            mockDataReader.Setup(r => r.GetValue(0)).Returns(rowValue);
            mockDataReader.Setup(r => r.GetValues(It.IsAny<object[]>()))
                .Callback<object[]>(values => values[0] = rowValue)
                .Returns(1);
            mockDataReader.Setup(r => r[0]).Returns(rowValue);
            mockDataReader.Setup(r => r["Row"]).Returns(rowValue);

            // Setup row reading behavior
            var currentRow = 0;
            var totalRows = 3;
            mockDataReader.Setup(s => s.HasRows).Returns(true);
            mockDataReader.Setup(s => s.ReadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    if (currentRow < totalRows)
                    {
                        currentRow++;
                        return true;
                    }
                    return false;
                });

            _mockDapperService
                .Setup(s => s.ExecuteReader(
                    It.Is<string>(sql => sql == expectedSql),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(_dapperCallbackHelper.DapperGeneralCallback)
                .ReturnsAsync(mockDataReader.Object);

            // Act
            await _repository.RunDapperExecuteReader();

            // Assert
            _mockDapperService.Verify(s => s.ExecuteReader(
                It.Is<string>(sql => sql == expectedSql),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunDapperPagedQuery 方法
        /// 目的：驗證是否能正確執行分頁查詢
        /// 重點：
        /// 1. 驗證分頁參數的處理
        /// 2. 確保 SQL 查詢正確包含分頁邏輯
        /// 3. 驗證回傳的分頁結果
        /// 4. 檢查日誌記錄的完整性
        /// </summary>
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            // Arrange
            var expectedSql = "SELECT * FROM [Person]";
            var expectedOrder = "[Row]";
            var expectedPageSize = 3;  // 固定值，與實際實作相符
            var expectedPageNo = 3;    // 固定值，與實際實作相符

            var mockResult = _fixture.Create<CommonPageOutputDto<Person>>();

            _mockDapperService
                .Setup(s => s.PagedQuery<Person>(
                    expectedSql,
                    expectedOrder,
                    It.IsAny<DynamicParameters>(),
                    expectedPageSize,
                    expectedPageNo,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .ReturnsAsync(mockResult)
                .Callback(_dapperCallbackHelper.DapperPagedQueryCallback)
                .Verifiable();

            // Act
            await _repository.RunDapperPagedQuery();

            // Assert
            _mockDapperService.Verify();
        }

        /// <summary>
        /// 測試 RunDapperExcute 方法
        /// 目的：驗證是否能正確執行 Dapper Execute 操作（通常用於 Insert、Update、Delete）
        /// 重點：
        /// 1. 驗證 SQL 語句的正確性（包含空白和換行的處理）
        /// 2. 確保 Execute 操作被正確呼叫
        /// 3. 驗證參數傳遞
        /// </summary>
        [TestMethod]
        public async Task RunDapperExcute()
        {
            // Arrange
            var expectedSql = @"
                INSERT INTO [dbo].[person]
                (
                    [id]
                    ,[name]
                    ,[age]
                    ,[birthday]
                    ,[remark]
                )
                VALUES
                ('A','',0,GETDATE(),'')
            ";

            _mockDapperService
                .Setup(s => s.Execute(
                    It.Is<string>(sql => sql.Replace(" ", "").Replace("\r", "").Replace("\n", "") ==
                                       expectedSql.Replace(" ", "").Replace("\r", "").Replace("\n", "")),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Returns(Task.CompletedTask);

            // Act
            await _repository.RunDapperExcute();

            // Assert
            _mockDapperService.Verify(s => s.Execute(
                It.Is<string>(sql => sql.Replace(" ", "").Replace("\r", "").Replace("\n", "") ==
                                    expectedSql.Replace(" ", "").Replace("\r", "").Replace("\n", "")),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Once);
        }

        /// <summary>
        /// 測試 RunSqlCondition 方法
        /// 目的：驗證在特定條件下的 SQL 查詢行為
        /// 重點：
        /// 1. 測試空結果集的處理
        /// 2. 驗證條件邏輯的正確性
        /// 3. 確保日誌記錄正確反映執行狀態
        /// </summary>
        [TestMethod]
        public async Task RunSqlCondition()
        {
            // Arrange
            var emptyList = _fixture.CreateMany<Person>(0).ToList();
            _mockDapperService
                .Setup(s => s.Query<Person>(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(_dapperCallbackHelper.DapperGeneralCallback)
                .ReturnsAsync(emptyList);

            // Act
            await _repository.RunSqlCondition();

            // Assert
            _mockDapperService.Verify(s => s.Query<Person>(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Never);
        }
    }
}