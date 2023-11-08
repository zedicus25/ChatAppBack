using DataAccessEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class UserRepo : GenericRepo<User>, IUsersRepository
    {
        public UserRepo(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
