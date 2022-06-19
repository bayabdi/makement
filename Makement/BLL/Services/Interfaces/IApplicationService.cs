using Common.Enum;
using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IApplicationService : IService
    {
        ApplicationVersion GetByType(ApplicationTypeEnum type);
    }
}
