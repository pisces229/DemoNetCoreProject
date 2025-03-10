﻿using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Configuration;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    internal class DefaultRepository : IDefaultRepository
    {
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _configuration;
        public DefaultRepository(IFileManager fileManager,
            IConfiguration configuration)
        {
            _fileManager = fileManager;
            _configuration = configuration;
        }
        public async Task<bool> Upload(DefaultRepositoryUploadInputDto model)
        {
            var filepath = _fileManager.CombineFilePath(
                _configuration.GetValue<string>(ConfigurationConstant.PathTemp),
                Guid.NewGuid().ToString());
            using (model.File)
            using (var fileStream = File.Create(filepath))
            {
                model.File.Seek(0, SeekOrigin.Begin);
                await model.File.CopyToAsync(fileStream);
            }
            return true;
        }
        public CommonOutputDto<CommonDownloadOutputDto> Download()
        {
            var result = new CommonOutputDto<CommonDownloadOutputDto>();
            var filepath = _fileManager.CombineFilePath(
                _configuration.GetValue<string>(ConfigurationConstant.PathTemp),
                "Ubuntu.pdf");
            if (File.Exists(filepath))
            {
                result.Success = true;
                result.Data = new CommonDownloadOutputDto()
                {
                    FileName = "Download.pdf",
                    FilePath = filepath,
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
