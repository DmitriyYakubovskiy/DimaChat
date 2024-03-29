using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.Server.DataAccess.Configurations
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(x => x.Id).HasName("message_id_pk");

            builder.ToTable("Messages");

            builder.Property(e => e.Content).HasMaxLength(1024);
        }
    }
}
