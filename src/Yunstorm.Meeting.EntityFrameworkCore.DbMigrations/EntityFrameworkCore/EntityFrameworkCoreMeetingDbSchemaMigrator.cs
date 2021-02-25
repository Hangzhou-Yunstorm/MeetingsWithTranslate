using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yunstorm.Meeting.Data;
using Volo.Abp.DependencyInjection;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreMeetingDbSchemaMigrator 
        : IMeetingDbSchemaMigrator, ITransientDependency
    {
        private readonly MeetingMigrationsDbContext _dbContext;

        public EntityFrameworkCoreMeetingDbSchemaMigrator(MeetingMigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}