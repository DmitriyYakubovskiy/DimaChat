namespace DimaChat.Client.Models
{
    public class ChatModel
    {
        public int Id { get; set; } 
        private List<ClientModel> clients;

        public void AddClient(ClientModel client)
        {
            clients.Add(client);
        }

        public void SendMessage(string message, ClientModel client)
        {
            //for(int i=0; i<clients.Count; i++)
            //{
            //    clients[i].ShowMessage(message);
            //}
        }
    }
}
