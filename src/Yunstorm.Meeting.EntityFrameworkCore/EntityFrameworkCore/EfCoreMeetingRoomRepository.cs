using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Yunstorm.Meeting.Meeting;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    public class EfCoreMeetingRoomRepository : EfCoreRepository<MeetingDbContext, MeetingRoom, Guid>, IMeetingRoomRepository
    {
        public EfCoreMeetingRoomRepository(IDbContextProvider<MeetingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public bool Existed(string sessionCode)
        {
            return DbSet.FirstOrDefault(r => r.SessionCode == sessionCode) != null;
        }

        public Task<MeetingRoom> FindBySessionCodeAsync(string sessionCode)
        {
            return DbSet.FirstOrDefaultAsync(r => r.SessionCode == sessionCode);
        }
        public Task<MeetingRoom> FindBySessionCodeIncludingMessagesAsync(string sessionCode)
        {
            return DbSet.Include(r => r.Messages).FirstOrDefaultAsync(r => r.SessionCode == sessionCode);
        }
    }
}
