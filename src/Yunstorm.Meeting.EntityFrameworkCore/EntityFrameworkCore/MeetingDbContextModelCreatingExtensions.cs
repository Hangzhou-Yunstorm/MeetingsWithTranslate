using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp;
using Volo.Abp.Users;
using Yunstorm.Meeting.Meeting;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    public static class MeetingDbContextModelCreatingExtensions
    {
        public static void ConfigureMeeting(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            ConfigureMeetingRoom(builder.Entity<MeetingRoom>());
            ConfigureMessage(builder.Entity<Message>());
            //ConfigureParticipant(builder.Entity<Participant>());
        }

        private static void ConfigureMeetingRoom(EntityTypeBuilder<MeetingRoom> builder)
        {
            builder.ToTable("MeetingRooms");

            builder.Ignore(r => r.ExtraProperties);

            builder.Property(r => r.SessionCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(r => r.CreationTime)
                .IsRequired();

            builder.HasMany(r => r.Messages)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(r => r.Participants)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureMessage(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.Property(m => m.Content)
                .HasMaxLength(1024)
                .IsUnicode();

            builder.Property(m => m.SenderId)
                .HasMaxLength(100);

            builder.Property(m => m.IsVoice)
                .HasDefaultValue(false);
            builder.Property(m => m.VoiceFile)
                .HasMaxLength(512);

            builder.Property(m => m.Language)
                .IsRequired();

            builder.Property(m => m.SendingTime)
                .IsRequired();
        }

        //private static void ConfigureParticipant(EntityTypeBuilder<Participant> builder)
        //{
        //    builder.ToTable("Participants");

        //    builder.Property(p => p.Nickname)
        //        .IsRequired()
        //        .HasMaxLength(100);

        //    builder.Property(p => p.Language)
        //        .IsRequired()
        //        .HasMaxLength(50);
        //    builder.Property(p => p.JoinTime)
        //        .IsRequired();
        //    builder.Property(p => p.UniqueId)
        //        .IsRequired()
        //        .HasMaxLength(200);
        //    builder.Property(p => p.ConnectionId)
        //        .IsRequired()
        //        .HasMaxLength(200);
        //}
    }
}