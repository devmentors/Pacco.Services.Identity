using System;
using System.Threading.Tasks;
using Pacco.Services.Identity.Core.Domain;

namespace Pacco.Services.Identity.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}