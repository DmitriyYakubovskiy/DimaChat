namespace DimaChat.Server.DataAccess.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; } 
        public string Content { get; set; } = string.Empty;
        public virtual ClientEntity Client { get; set; } = null!;
        public virtual ChatEntity Chat { get; set; } = null!;
    }
}
