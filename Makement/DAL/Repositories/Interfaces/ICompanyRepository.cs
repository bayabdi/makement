using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        Task<IEnumerable<Company>> GetAllWithUsers();
    }
}
