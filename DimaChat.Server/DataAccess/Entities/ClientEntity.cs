namespace DimaChat.Server.DataAccess.Entities
{
    public class ClientEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual ICollection<ChatClientEntity> Chats { get; set; } = new List<ChatClientEntity>();
        public virtual ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}
