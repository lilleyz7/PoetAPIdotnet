using PoetAPI.DTOs;
using PoetAPI.Models;

namespace PoetAPI.Repos
{
    public interface IPoemRepo
    {
        public bool SaveChanges();
        public Poem GetByID(int id);
        public Poem GetByName(string name);

        public void AddPoem(Poem poem);
        public void SavePoem(Poem poem, string userID);
        public void UnsavePoem (int poemID, string userID);

        public Task<PoemDTO> SearchByPoemTitle(string name);
        public Task<List<PoemDTO>> SearchByAuthor(string authorName);

        public Task<List<PoemDTO>> GetRandomPoems(int returnCount);

    }
}
