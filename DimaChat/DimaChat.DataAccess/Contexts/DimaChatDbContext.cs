using DimaChat.DataAccess.Configurations;
using DimaChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DimaChat.Server.DataAccess.Contexts;

public class DimaChatDbContext : DbContext
{
    public DbSet<ChatEntity> Chats { get; set; }        
    public DbSet<ClientEntity> Clients { get; set; }        
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ChatClientEntity> ChatClients { get; set; }

    private readonly IConfiguration configuration;

    public DimaChatDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
        Database.EnsureCreated();
    }

    public DimaChatDbContext(DbContextOptions<DimaChatDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DimaChatConnectionString"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatClientEntityConfiguration()); 
    }
}
