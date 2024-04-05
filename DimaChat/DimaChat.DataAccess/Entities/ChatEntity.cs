namespace DimaChat.DataAccess.Entities;

public class ChatEntity
{
    public int ChatId { get; set; }
    public string ChatName { get; set; } = string.Empty;
    public virtual ICollection<ChatClientEntity> Clients { get; set; } = new List<ChatClientEntity>();
    public virtual ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}
