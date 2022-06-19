using AutoMapper;
using BLL.AutoMapper;
using BLL.Services.Interfaces;
using DAL;

namespace BLL.Services
{
    public abstract class Service : IService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IMapper mapper { get; private set; }
        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            mapper = MapBuilder.Build();
        }
    }
}
