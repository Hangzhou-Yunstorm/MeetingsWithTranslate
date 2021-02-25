using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Yunstorm.Meeting.Meeting;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See MeetingMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class MeetingDbContext : AbpDbContext<MeetingDbContext>
    {

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside MeetingDbContextModelCreatingExtensions.ConfigureMeeting
         */
        public DbSet<MeetingRoom> MeetingRooms { get; set; }
        //public DbSet<Participant> Participants { get; set; }
        public DbSet<Message> Messages { get; set; }

        public MeetingDbContext(DbContextOptions<MeetingDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside the ConfigureMeeting method */

            builder.ConfigureMeeting();
        }
    }
}
