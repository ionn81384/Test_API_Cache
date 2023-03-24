using Exercicio_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Exercicio_API.Persistence
{
    public interface IAPIRepoPosts
    {
        public Task<IQueryable<Post>> GetAll();
        public Task<Post> GetById(int id);
        public Task Create(Post company);
        public Task Update(Post company);
        public Task Delete(Post company);
    }
}