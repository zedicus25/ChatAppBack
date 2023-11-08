

namespace Domain.Interfaces
{
    public interface IUnitOfWorks
    {
        public IContactsRepository ContactsRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IConversationRepository ConversationRepository { get; }
        public IUsersRepository UsersRepository { get; }
        int Commit();
    }
}
