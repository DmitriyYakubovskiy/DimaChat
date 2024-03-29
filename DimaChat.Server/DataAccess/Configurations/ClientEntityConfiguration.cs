using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.Server.DataAccess.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.HasKey(x => x.Id).HasName("client_id_pk");

            builder.ToTable("Clients");

            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Password).HasMaxLength(100);
        }
    }
}
