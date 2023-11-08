using DataAccessEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class ConversationRepo : GenericRepo<Conversation>, IConversationRepository
    {
        public ConversationRepo(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
