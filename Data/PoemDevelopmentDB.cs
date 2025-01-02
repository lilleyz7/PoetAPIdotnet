using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoetAPI.Models;

namespace PoetAPI.Data
{
    public class PoemDevelopmentDB: IdentityDbContext<CustomUser>
    {
        public DbSet<Poem> Poems { get; set; }

        public string DbPath { get; }

        public PoemDevelopmentDB()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, "poemAPI.db");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=poemAPI.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomUser>()
                .HasMany(u => u.SavedPoems)
                .WithMany(p => p.SavedByUsers);
        }

    }
}
