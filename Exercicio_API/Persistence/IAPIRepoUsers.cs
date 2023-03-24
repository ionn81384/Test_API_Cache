using Exercicio_API.DTO;

namespace Exercicio_API.Persistence
{
    public interface IAPIRepoUsers
    {
        public Task<IQueryable<User>> GetAll();
        public Task<User> GetById(int id);
        public Task Create(User user);
        public Task Update(User user);
        public Task Delete(User user);
    }
}