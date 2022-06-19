using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Interfaces
{
    public interface IRegistrationCodeRepository : IGenericRepository<RegistrationCode, int>
    {
    }
}
