using DimaChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.DataAccess.Configurations;

public class ChatClientEntityConfiguration : IEntityTypeConfiguration<ChatClientEntity>
{
    public void Configure(EntityTypeBuilder<ChatClientEntity> builder)
    {
        builder.HasKey(x => x.ChatClientId).HasName("chat_client_id_pk");

        builder.ToTable("ChatClients");

        builder.Property(e => e.ClientId).ValueGeneratedOnAdd();
        builder.Property(e => e.ChatId).ValueGeneratedOnAdd();

        builder.HasOne(d => d.Client).WithMany(p => p.Chats)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("client_id_fk");

        builder.HasOne(d => d.Chat).WithMany(p => p.Clients)
            .HasForeignKey(d => d.ChatId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("chat_id_fk");
    }
}
