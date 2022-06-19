using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.DatabseContext;

namespace DAL.Repositories
{
    public class EmailMessageRepository : GenericRepository<EmailMessage, int>, IEmailMessageRepository
    {
        public EmailMessageRepository(DatabaseContext context) : base(context) { }
    }
}