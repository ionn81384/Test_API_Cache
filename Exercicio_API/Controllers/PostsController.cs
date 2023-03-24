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
    public class PostsController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private const string postsListCacheKey = "POSTS_LIST";
        private readonly IAPIRepoPosts _repo;
        private IMemoryCache _cache;
        private ILogger<PostsController> _logger;

        private MemoryCacheEntryOptions _cacheOptions;
        public PostsController(IAPIRepoPosts repo,
            IMemoryCache cache,
            ILogger<PostsController> logger)
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

        // GET: api/<PostController>
        [EnableQuery(PageSize = 3)]
        [HttpGet]
        public async Task<IQueryable<Post>> Get()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of posts from cache.");
            if (_cache.TryGetValue(postsListCacheKey, out IQueryable<Post> posts))
            {
                _logger.Log(LogLevel.Information, "Posts list found in cache.");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(postsListCacheKey, out posts))
                    {
                        _logger.Log(LogLevel.Information, "Posts list found in cache.");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "Posts list not found in cache. Fetching from database.");
                        posts = await _repo.GetAll();
                        _cache.Set(postsListCacheKey, posts, _cacheOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return posts;
        }

        // GET api/<PostController>/5
        [EnableQuery]
        [HttpGet("{id}")]
        public async Task<Post> Get(int id)
        {
            return await _repo.GetById(id);
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repo.Create(post);

            // reset existing cache
            _cache.Remove(postsListCacheKey);

            return Created("Post", post);
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != post.Id)
            {
                return BadRequest();
            }
            await _repo.Update(post);
            // reset existing cache
            _cache.Remove(postsListCacheKey);
            return NoContent();
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromODataUri] int key)
        {
            var company = await _repo.GetById(key);
            if (company is null)
            {
                return BadRequest();
            }
            await _repo.Delete(company);

            // reset existing cache
            _cache.Remove(postsListCacheKey);
            return NoContent();
        }
    }
}
