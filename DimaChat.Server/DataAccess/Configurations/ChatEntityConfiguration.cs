using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.Server.DataAccess.Configurations
{
    public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
    {
        public void Configure(EntityTypeBuilder<ChatEntity> builder)
        {
            builder.HasKey(x => x.Id).HasName("chat_id_pk");

            builder.ToTable("Chats");

            builder.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
