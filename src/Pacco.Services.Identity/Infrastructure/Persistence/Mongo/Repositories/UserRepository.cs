using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Identity.Core.Domain;
using Pacco.Services.Identity.Core.Repositories;

namespace Pacco.Services.Identity.Infrastructure.Persistence.Mongo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<User, Guid> _repository;

        public UserRepository(IMongoRepository<User, Guid> repository)
        {
            _repository = repository;
        }

        public Task<User> GetAsync(Guid id) => _repository.GetAsync(id);
        public Task<User> GetAsync(string email) => _repository.GetAsync(x => x.Email == email.ToLowerInvariant());
        public Task AddAsync(User user) => _repository.AddAsync(user);
        public Task UpdateAsync(User user) => _repository.UpdateAsync(user);
    }
}