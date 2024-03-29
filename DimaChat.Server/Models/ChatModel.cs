using DimaChat.Server.DataAccess.Entities;

namespace DimaChat.Server.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private List<ChatClientEntity> clients;
        public List<ChatClientEntity> Clients => clients;

        public ChatModel(List<ChatClientEntity> clients)
        {
            this.clients = clients; 
        }

        public ChatModel():this(new List<ChatClientEntity>()) { }

        public void AddClient(ChatClientEntity client)
        {
            clients.Add(client);
        }

        public void RemoveClient(ChatClientEntity client)
        {
            clients.Remove(client); 
        }
    }
}
