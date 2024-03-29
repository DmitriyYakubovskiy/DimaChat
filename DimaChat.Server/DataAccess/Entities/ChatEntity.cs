namespace DimaChat.Server.DataAccess.Entities
{
    public class ChatEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<ChatClientEntity> Clients { get; set; } = new List<ChatClientEntity>();
    }
}
