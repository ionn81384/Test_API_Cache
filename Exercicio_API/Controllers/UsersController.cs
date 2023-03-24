using Exercicio_API.DTO;
using Exercicio_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exercicio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private const string USERS_LISTS_CACHE_KEY = "USERS_LIST";
        private readonly IAPIRepoUsers _repo;
        private IMemoryCache _cache;
        private ILogger<UsersController> _logger;

        private MemoryCacheEntryOptions _cacheOptions;
        public UsersController(IAPIRepoUsers repo,
            IMemoryCache cache,
            ILogger<UsersController> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;

            // TODO setup in the app settings these vars
            _cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
        }

        // GET: api/<UserController>
        [EnableQuery(PageSize =3)]
        [HttpGet]
        public async Task<IQueryable<User>> Get()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of posts from cache.");
            if (_cache.TryGetValue(USERS_LISTS_CACHE_KEY, out IQueryable<User> users))
            {
                _logger.Log(LogLevel.Information, "Users list found in cache.");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(USERS_LISTS_CACHE_KEY, out users))
                    {
                        _logger.Log(LogLevel.Information, "Users list found in cache.");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "Users list not found in cache. Fetching from database.");
                        users = await _repo.GetAll();
                        _cache.Set(USERS_LISTS_CACHE_KEY, users, _cacheOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return users;
        }

        // GET api/<UserController>/5
        [EnableQuery]
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _repo.GetById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repo.Create(user);
            // reset existing cache
            _cache.Remove(USERS_LISTS_CACHE_KEY);

            return Created("User", user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != user.Id)
            {
                return BadRequest();
            }
            await _repo.Update(user);

            // reset existing cache
            _cache.Remove(USERS_LISTS_CACHE_KEY);

            return NoContent();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var company = await _repo.GetById(key);
            if (company is null)
            {
                return BadRequest();
            }
            await _repo.Delete(company);

            // reset existing cache
            _cache.Remove(USERS_LISTS_CACHE_KEY);

            return NoContent();
        }
    }
}
