using DemoNetCoreProject.DataLayer.Dtos.Default;
using System;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultFirstRepository
    {
        Task<DefaultFirstRepositoryOutputDto> Run(DefaultFirstRepositoryInputDto model);
    }
}
