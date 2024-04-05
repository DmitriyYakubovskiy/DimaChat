namespace DimaChat.DataAccess.Entities;

public class ClientEntity
{
    public int ClientId { get; set; } 
    public string ClientName { get; set; } = string.Empty;
    public string ClientPassword { get; set; } = string.Empty;
    public virtual ICollection<ChatClientEntity> Chats { get; set; } = new List<ChatClientEntity>();
    public virtual ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}
