using DimaChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.DataAccess.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.HasKey(x => x.ClientId).HasName("client_id_pk");

            builder.ToTable("Clients");

            builder.Property(e => e.ClientName).HasMaxLength(100);
            builder.HasIndex(e => e.ClientName).IsUnique();
            builder.Property(e => e.ClientPassword).HasMaxLength(100);
        }
    }
}
