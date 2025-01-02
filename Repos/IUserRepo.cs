using PoetAPI.Models;

namespace PoetAPI.Repos
{
    public interface IUserRepo
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<CustomUser> GetUsers();
        public CustomUser GetSingleUser(string userId);
    }
}
