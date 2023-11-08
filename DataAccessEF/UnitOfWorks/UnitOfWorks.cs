using DataAccessEF.Data;
using DataAccessEF.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks, IDisposable
    {
        public IContactsRepository ContactsRepository { get; }

        public IMessageRepository MessageRepository { get; }

        public IConversationRepository ConversationRepository { get; }

        public IUsersRepository UsersRepository { get; }

        private readonly ChatDbContext _dbContext;

        public UnitOfWorks(ChatDbContext context)
        {
            _dbContext = context;
            ContactsRepository = new ContactRepo(_dbContext);
            MessageRepository = new MessageRepo(_dbContext);
            ConversationRepository = new ConversationRepo(_dbContext);
            UsersRepository = new UserRepo(_dbContext);
        }

        public int Commit() => _dbContext.SaveChanges();

        public void Dispose() => _dbContext.Dispose();
    }
}
