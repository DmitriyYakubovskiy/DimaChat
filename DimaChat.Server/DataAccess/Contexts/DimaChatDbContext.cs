using Microsoft.Extensions.Configuration;
using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using DimaChat.Server.DataAccess.Configurations;

namespace DimaChat.Server.DataAccess.Contexts
{
    public class DimaChatDbContext : DbContext
    {
        public DbSet<ChatEntity> Chats { get; set; }        
        public DbSet<ClientEntity> Clients { get; set; }        
        public DbSet<MessageEntity> Messages { get; set; }

        private readonly IConfiguration configuration;

        public DimaChatDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            Database.EnsureCreated();
        }

        public DimaChatDbContext(DbContextOptions<DimaChatDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("BooksConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        }
    }
}
