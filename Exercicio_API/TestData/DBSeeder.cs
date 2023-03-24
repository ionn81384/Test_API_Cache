using Exercicio_API.DTO;
using Exercicio_API.Persistence;
using Newtonsoft.Json;
using System.Reflection;

namespace Exercicio_API.TestData
{
    public class DBSeeder
    {
        public static void AddCompaniesData(WebApplication app)
        {

            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<ApiContext>();
            db.Users.AddRange(GetUsersFromFile());
            db.Posts.AddRange(GetPostsFromFile());
            db.SaveChanges();
        }

        private static List<User> GetUsersFromFile()
        {
            string fileName = @"TestData\Users.json";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<User> users = (List<User>)serializer.Deserialize(file, typeof(List<User>));
                return users;
            }
        }

        private static List<Post> GetPostsFromFile()
        {
            string fileName = @"TestData\Posts.json";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Post> posts = (List<Post>)serializer.Deserialize(file, typeof(List<Post>));
                return posts;
            }
        }

    }
}
