using Castle.Core.Logging;
using Exercicio_API.Controllers;
using Exercicio_API.DTO;
using Exercicio_API_Test.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Exercicio_API_Test
{
    public class UsersController_Test
    {
        [Fact]
        public void Get_WhenCalledWithID_ReturnsUser()
        {
            // ARRANGE
            var mockLoger = new Mock<ILogger<UsersController>>();
            var mockCache = new Mock<IMemoryCache>();
            var repo = new APIRepoUsersFake();
            var expectedUser = repo.GetById(1);
            var _controller = new UsersController(repo, mockCache.Object, mockLoger.Object);

            // Act
            var okResult = _controller.Get(1);

            // Assert
            Assert.IsType<User>(okResult.Result);
            Assert.Equal(expectedUser.Result, okResult.Result);
        }

        [Fact]
        public void Get_WhenCalledAndNotInCache_ReturnsOkResult()
        {
            // ARRANGE
            var mockLoger = new Mock<ILogger<UsersController>>();
            var mockCache = new Mock<IMemoryCache>();
            var repo = new APIRepoUsersFake();

            //TODO find a way to moq IMemoryCache
            //mockCache.Setup(p => p.TryGetValue(It.IsAny<string>(),out It.Ref<IQueryable<User>>.IsAny)).Returns(true);
            //mockCache.Setup(p => p.Set(It.IsAny<string>(),It.IsAny<object>, It.IsAny<MemoryCacheEntryOptions>()));

            //// Act
            //var _controller = new UsersController(repo, mockCache.Object, mockLoger.Object);
            //var okResult = _controller.Get();
            //// Assert
            //Assert.IsType<Task<IQueryable<User>>>(okResult);
        }
    }
}