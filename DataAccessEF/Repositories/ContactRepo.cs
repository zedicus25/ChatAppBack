using DataAccessEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class ContactRepo : GenericRepo<Contact>, IContactsRepository
    {
        public ContactRepo(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
