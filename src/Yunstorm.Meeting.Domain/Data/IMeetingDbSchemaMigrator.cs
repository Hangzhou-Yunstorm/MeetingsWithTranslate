using System.Threading.Tasks;

namespace Yunstorm.Meeting.Data
{
    public interface IMeetingDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
