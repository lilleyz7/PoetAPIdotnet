using PoetAPI.Data;
using PoetAPI.Models;
using PoetAPI.Repos;

namespace DotnetAPI.Data
{
    public class UserRepo : IUserRepo
    {
        PoemDevelopmentDB _database;

        public UserRepo()
        {
            _database = new PoemDevelopmentDB();
        }

        public bool SaveChanges()
        {
            return _database.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _database.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _database.Remove(entityToAdd);
            }
        }

        public IEnumerable<CustomUser> GetUsers()
        {
            IEnumerable<CustomUser> users = _database.Users.ToList<CustomUser>();
            return users;
        }

        public CustomUser GetSingleUser(string userId)
        {
            CustomUser? user = _database.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault<CustomUser>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get CustomUser");
        }


    }
}