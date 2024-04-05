namespace DimaChat.DataAccess.Entities;

public class MessageEntity
{
    public int MessageId { get; set; } 
    public string MessageContent { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public int ChatId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;
    public virtual ChatEntity Chat { get; set; } = null!;
}
