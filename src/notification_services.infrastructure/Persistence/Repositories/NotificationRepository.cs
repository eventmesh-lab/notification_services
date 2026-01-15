using notification_services.domain.Entities;
using notification_services.infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notification_services.domain.Interfaces;

namespace notification_services.infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepositoryPostgres
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }
}
