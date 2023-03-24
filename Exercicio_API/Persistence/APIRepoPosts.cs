using Exercicio_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace Exercicio_API.Persistence
{
    public class APIRepoPosts : IAPIRepoPosts
    {
        private readonly ApiContext _context;
        public APIRepoPosts(ApiContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<Post>> GetAll()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts.AsQueryable();
        }
        public async Task<Post> GetById(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task Create(Post company)
        {
            _context.Posts.Add(company);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Post company)
        {
            _context.Posts.Update(company);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Post company)
        {
            _context.Posts.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
