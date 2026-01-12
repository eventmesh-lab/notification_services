using notification_services.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_services.domain.Interfaces
{
    public interface INotificationRepositoryPostgres
    {
        Task AddAsync(Notification notification);
    }
}
