using BLL.Services.Interfaces;
using Common.Enum;
using DAL;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class ApplicationService : Service, IApplicationService
    {
        public ApplicationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public ApplicationVersion GetByType(ApplicationTypeEnum type)
        {
            return UnitOfWork.ApplicationVersion.GetAll().Result.First(x => x.ApplicationType == type);
        }
    }
}
