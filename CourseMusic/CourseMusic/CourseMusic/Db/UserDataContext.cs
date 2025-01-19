using Microsoft.EntityFrameworkCore;
using CourseMusic.Models;

namespace CourseMusic.Db;

public class UserDataContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source = {App.path}\\DataFile.db");
    }

    public DbSet<User> users { get; set; }
}