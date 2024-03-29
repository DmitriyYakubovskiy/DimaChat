using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.Server.DataAccess.Configurations;

public class ChatClientEntityConfiguration : IEntityTypeConfiguration<ChatClientEntity>
{
    public void Configure(EntityTypeBuilder<ChatClientEntity> builder)
    {
        builder.HasKey(x => x.Id).HasName("chat_client_id_pk");

        builder.ToTable("ChatClients");

        builder.HasOne(d => d.Client).WithMany(p => p.Chats)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("client_id_fk");

        builder.HasOne(d => d.Chat).WithMany(p => p.Clients)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("chat_id_fk");
    }
}
