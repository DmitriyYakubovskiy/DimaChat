namespace DimaChat.DataAccess.Entities;

public class ChatClientEntity
{
    public int ChatClientId { get; set; }
    public int ClientId { get; set; }
    public int ChatId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;
    public virtual ChatEntity Chat { get; set; } = null!;
}
