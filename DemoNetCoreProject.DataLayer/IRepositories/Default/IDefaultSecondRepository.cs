using DemoNetCoreProject.DataLayer.Dtos.Default;
using System;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultSecondRepository
    {
        Task<DefaultSecondRepositoryOutputDto> Run(DefaultSecondRepositoryInputDto model);
    }
}
