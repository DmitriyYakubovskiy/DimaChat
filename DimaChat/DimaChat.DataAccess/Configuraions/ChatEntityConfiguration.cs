using DimaChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.DataAccess.Configurations;

public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.HasKey(x => x.ChatId).HasName("chat_id_pk");

        builder.ToTable("Chats");

        builder.Property(e => e.ChatName).HasMaxLength(100);
        builder.HasIndex(e => e.ChatName).IsUnique();
    }
}
