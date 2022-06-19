using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RegistrationCodeRepository : GenericRepository<RegistrationCode, int>, IRegistrationCodeRepository
    {
        public RegistrationCodeRepository(DatabaseContext context) : base(context) { }
    }
}
