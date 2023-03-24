using Exercicio_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace Exercicio_API.Persistence
{
    public class APIRepoUsers: IAPIRepoUsers
    {
        private readonly ApiContext _context;
        public APIRepoUsers(ApiContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<User>> GetAll()
        {
            var users = _context.Users
                .Include(x => x.Address)
                .Include(x => x.Company)
                .Include(x => x.Address.Geo).ToList();
            return users.AsQueryable();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(x => x.Address)
                .Include(x => x.Company)
                .Include(x => x.Address.Geo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
