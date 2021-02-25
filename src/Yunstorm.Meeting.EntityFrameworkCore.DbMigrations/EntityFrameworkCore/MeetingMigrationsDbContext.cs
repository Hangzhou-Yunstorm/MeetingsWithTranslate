using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    /* This DbContext is only used for database migrations.
     * It is not used on runtime. See MeetingDbContext for the runtime DbContext.
     * It is a unified model that includes configuration for
     * all used modules and your application.
     */
    public class MeetingMigrationsDbContext : AbpDbContext<MeetingMigrationsDbContext>
    {
        public MeetingMigrationsDbContext(DbContextOptions<MeetingMigrationsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();

            /* Configure your own tables/entities inside the ConfigureMeeting method */

            builder.ConfigureMeeting();
        }
    }
}