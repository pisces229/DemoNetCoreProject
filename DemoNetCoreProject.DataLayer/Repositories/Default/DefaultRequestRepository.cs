using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    internal class DefaultRequestRepository : IDefaultRequestRepository
    {
        private readonly ILogger<DefaultRequestRepository> _logger;
        private readonly IConfiguration _configuration;
        public DefaultRequestRepository(ILogger<DefaultRequestRepository> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task Upload(DefaultRequestRepositoryUploadInputDto model)
        {
            _logger.LogInformation(model.File.GetHashCode().ToString());
            var file = FileUtility.GetFile(
                Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                Guid.NewGuid().ToString());
            using (model.File)
            using (var fileStream = file.Create())
            {
                model.File.Seek(0, SeekOrigin.Begin);
                await model.File.CopyToAsync(fileStream);
            }
        }
        public CommonDownloadDto Download()
        {
            var result = new CommonDownloadDto();
            var fileInfo = FileUtility.GetFile(
                Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                "temp.zip");
            if (fileInfo.Exists)
            {
                result.FileName = "Download.zip";
                result.FilePath = fileInfo.FullName;
            }
            return result;
        }
    }
}
