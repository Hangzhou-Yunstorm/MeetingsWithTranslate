using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Yunstorm.Meeting.Data
{
    public class MeetingDbMigrationService : ITransientDependency
    {
        public ILogger<MeetingDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IMeetingDbSchemaMigrator _dbSchemaMigrator;

        public MeetingDbMigrationService(
            IDataSeeder dataSeeder,
            IMeetingDbSchemaMigrator dbSchemaMigrator)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrator = dbSchemaMigrator;

            Logger = NullLogger<MeetingDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            Logger.LogInformation("Migrating database schema...");
            await _dbSchemaMigrator.MigrateAsync();

            Logger.LogInformation("Executing database seed...");
            await _dataSeeder.SeedAsync();

            Logger.LogInformation("Successfully completed database migrations.");
        }
    }
}