using DimaChat.Server;

ServerModel server = new ServerModel("127.0.0.1", 8080);
server.Start();
Console.ReadLine();
server.Stop();