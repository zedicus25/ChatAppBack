using DataAccessEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class MessageRepo : GenericRepo<Message>, IMessageRepository
    {
        public MessageRepo(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
