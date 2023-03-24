using Exercicio_API.DTO;
using Exercicio_API.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercicio_API_Test.Repository
{
    //TODO moq EF instead of creating Repo Fake Class
    public class APIRepoUsersFake : IAPIRepoUsers
    {
        private readonly IEnumerable<User> _usersList;
        public APIRepoUsersFake()
        {
            _usersList = new List<User>()
            {
                new User(){ Id = 1, Name ="Leanne Graham",Username ="Bret", Email = "Sincere@april.biz", Phone = "1-770-736-8031 x56442", Website = "hildegard.org",
                Address = new Address (){Id = 1, City = "Gwenborough", Street = "Kulas Light",Suite = "Apt. 556", Zipcode = "92998-3874",
                Geo = new Geo(){Id = 1, Lat = "-37.3159", Lng = "81.1496"} },
                Company = new Company(){ Id = 1, Bs = "harness real-time e-markets", CatchPhrase = "Multi-layered client-server neural-net", Name = "Romaguera-Crona"} }
            };
        }

        public Task Create(User user)
        {
            _usersList.Append(user);
            return Task.CompletedTask;
        }

        public Task Delete(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<User>> GetAll()
        {
            return Task.FromResult(_usersList.AsQueryable());
        }

        public Task<User> GetById(int id)
        {
            return Task.FromResult(_usersList.FirstOrDefault(a => a.Id == id));
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
