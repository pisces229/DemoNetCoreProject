using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.Configuration;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    internal class DefaultRequestRepository : IDefaultRequestRepository
    {
        private readonly IConfiguration _configuration;
        public DefaultRequestRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> Upload(DefaultRequestRepositoryUploadInputDto model)
        {
            var file = FileUtility.GetFile(
                Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                Guid.NewGuid().ToString());
            using (model.File)
            using (var fileStream = file.Create())
            {
                model.File.Seek(0, SeekOrigin.Begin);
                await model.File.CopyToAsync(fileStream);
            }
            return true;
        }
        public CommonOutputDto<CommonDownloadDto> Download()
        {
            var result = new CommonOutputDto<CommonDownloadDto>();
            var fileInfo = FileUtility.GetFile(
                Directory.CreateDirectory(_configuration.GetValue<string>(ConfigurationConstant.PathTemp)),
                "temp.zip");
            if (fileInfo.Exists)
            {
                result.Success = true;
                result.Data = new CommonDownloadDto()
                {
                    FileName = "Download.zip",
                    FilePath = fileInfo.FullName,
                };
            }
            else
            {
                result.Message = "File No Exists";
            }
            return result;
        }
    }
}
