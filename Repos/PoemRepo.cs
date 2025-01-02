using Microsoft.EntityFrameworkCore;
using PoetAPI.Data;
using PoetAPI.DTOs;
using PoetAPI.Models;
using System;
using System.Security.Policy;
using System.Text.Json;

namespace PoetAPI.Repos
{
    public class PoemRepo : IPoemRepo
    {
        private readonly PoemDevelopmentDB _database;
        public PoemRepo() { 
            _database = new PoemDevelopmentDB();           
        }

        public async Task<List<PoemDTO>> apiCall(string urlExtension)
        {

            var baseUrl = "https://poetrydb.org";
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            var returnedPoems = new List<PoemDTO>();
            var response = await client.GetAsync(urlExtension);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                returnedPoems = JsonSerializer.Deserialize<List<PoemDTO>>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            if (returnedPoems == null)
            {
                throw new Exception("Failed to get poems");
            }

            return returnedPoems;
        }
        public void AddPoem(Poem poem)
        {
            if (poem != null)
            {
                _database.Add(poem);
            }
        }

        public IEnumerable<Poem> GetByAuthor(string authorName)
        {
            IEnumerable<Poem> poems = _database.Poems.Where(p => p.Author == authorName).ToList();
            return poems;
        }

        public Poem GetByID(int id)
        {
            Poem? searchedPoem = _database.Poems.Where(p => p.Id == id).FirstOrDefault<Poem>();
            if (searchedPoem != null)
            {
                return searchedPoem;
            }
            else
            {
                throw new Exception($"Poem with id {id} does not exist");
            }
        }

        public Poem GetByName(string name)
        {
            Poem? searchedPoem = _database.Poems.Where(p => p.Title == name).FirstOrDefault<Poem>();
            if (searchedPoem != null)
            {
                return searchedPoem;
            }

            throw new Exception($"Poem with name {name} does not exist");
        }


        public bool SaveChanges()
        {
            return _database.SaveChanges() > 0;
        }

        public void SavePoem(Poem poem, string userID)
        {
            CustomUser? user = _database.Users.Include(u => u.SavedPoems).FirstOrDefault(u => u.Id == userID);

            if(user == null)
            {
                throw new Exception("User does not exist");
            }
            
            Poem? existingPoem = _database.Poems
            .FirstOrDefault(p => p.Title == poem.Title && p.Author == poem.Author);

            if (existingPoem == null)
            {
                _database.Poems.Add(poem);
                existingPoem = poem;
                bool changesMade = SaveChanges();
                if(changesMade == false)
                {
                    throw new Exception("Unable to save new poem");
                }
            }

            if (!user.SavedPoems.Any(p => p.Id == existingPoem.Id))
            {
                user.SavedPoems.Add(existingPoem);
                bool changesMade = SaveChanges();
                if (changesMade == false)
                {
                    throw new Exception("Unable to save new poem to user");
                }
            }
        }

        public async Task<List<PoemDTO>> SearchByAuthor(string authorName)
        {
            var urlExtenion = $"/author/{authorName}";

            try
            {
                var poems = await apiCall(urlExtenion);
                return poems;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get random poems: " + e.ToString());
            }
        }
    

        public async Task<PoemDTO> SearchByPoemTitle(string poemTitle)
        {

            var baseUrl = "https://poetrydb.org";
            var client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            var urlExtenion = $"/title/{poemTitle}/author,title,linecount";
            var finalPoem = new PoemDTO();
            var response = await client.GetAsync(urlExtenion);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                finalPoem = JsonSerializer.Deserialize<PoemDTO>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            
            if (finalPoem == null)
            {
                throw new Exception("Poem not found");
            }
            return finalPoem;
        }

        public void UnsavePoem(int poemID, string userID)
        {
            CustomUser? user = _database.Users.Include(u => u.SavedPoems).FirstOrDefault(u => u.Id == userID);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            try
            {
                bool success = user.SavedPoems.Remove(user.SavedPoems.FirstOrDefault(p => p.Id == poemID));
                if (success == false)
                {
                    throw new Exception("Poem not found in user's saved poems");
                }
                return;
            }
            catch
            {
                throw new Exception("Unable to remove poem from user");
            }
     
        }

        public async Task<List<PoemDTO>> GetRandomPoems(int returnCount)
        {
            if (returnCount > 5 || returnCount < 1)
            {

               throw new Exception("Invalid return count. Must be between 1 and 5");
            }
            var urlExtenion = $"/random/{returnCount}";

            try
            {
                var poems = await apiCall(urlExtenion);
                return poems;
            } catch(Exception e)
            {
               throw new Exception("Failed to get random poems: " + e.ToString());
            }}
        }

        
}
