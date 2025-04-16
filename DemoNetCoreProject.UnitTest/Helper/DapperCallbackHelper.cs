using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace DemoNetCoreProject.UnitTest.Helper
{
    public class DapperCallbackHelper
    {
        private readonly ILogger<DapperCallbackHelper> _logger;
        public DapperCallbackHelper(Mock<ILogger<DapperCallbackHelper>> logger)
        {
            logger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var message = invokeMethod.Invoke(formatter, new[] { state, exception });

                    LoggerFactory.Create(builder =>
                    {
                        builder
                            .AddConsole()
                            .SetMinimumLevel(LogLevel.Debug);
                    })
                    .CreateLogger("UnitTest")
                    .Log(logLevel, eventId, state, exception, (s, e) => message.ToString());
                }));
            _logger = logger.Object;
        }
        public void DapperGeneralCallback(string sql, DynamicParameters parameters, int? commandTimeout = null, CommandType? commandType = null)
        {
            _logger.LogInformation("SQL: {@sql}", sql);
            if (parameters != null)
            {
                _logger.LogInformation("Parameters: {@parameters}", parameters);
            }
        }

        public void DapperPagedQueryCallback(string sql, string countSql, DynamicParameters parameters, int pageSize, int pageNo, int? commandTimeout = null, CommandType? commandType = null)
        {
            _logger.LogInformation("SQL: {@sql}", sql);
            _logger.LogInformation("Count SQL: {@countSql}", countSql);
            if (parameters != null)
            {
                _logger.LogInformation("Parameters: {@parameters}", parameters);
            }
            _logger.LogInformation("Page Size: {pageSize}", pageSize);
            _logger.LogInformation("Page No: {pageNo}", pageNo);
        }
    }
}
