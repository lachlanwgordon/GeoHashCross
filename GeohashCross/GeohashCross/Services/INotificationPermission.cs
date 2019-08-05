using System;
using System.Threading.Tasks;

namespace GeohashCross.Services
{
    public interface INotificationPermission
    {
        Task<bool> GetPermission();
    }
}
