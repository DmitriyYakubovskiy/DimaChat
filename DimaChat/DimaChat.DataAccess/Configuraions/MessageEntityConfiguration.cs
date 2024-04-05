using DimaChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimaChat.DataAccess.Configurations
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(x => x.MessageId).HasName("message_id_pk");

            builder.ToTable("Messages");

            builder.Property(e => e.MessageContent).HasMaxLength(1024);
            builder.Property(e => e.ClientId).ValueGeneratedOnAdd();
            builder.Property(e => e.ChatId).ValueGeneratedOnAdd();

            builder.HasOne(d => d.Client).WithMany(p => p.Messages)
            .HasForeignKey(d => d.MessageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("chat_id_fk");

            builder.HasOne(d => d.Chat).WithMany(p => p.Messages)
            .HasForeignKey(d => d.MessageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("client_id_fk");
        }
    }
}
