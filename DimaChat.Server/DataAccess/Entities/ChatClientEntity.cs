namespace DimaChat.Server.DataAccess.Entities;

public class ChatClientEntity
{
    public int Id { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;
    public virtual ChatEntity Chat { get; set; } = null!;
}
