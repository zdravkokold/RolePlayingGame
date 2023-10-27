using Microsoft.EntityFrameworkCore;
using RPG.Characters;
using System.Numerics;
using System.Collections.Generic;

public class GameDbContext : DbContext
{
    public DbSet<Character> Characters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-TJIP50U\\SQLEXPRESS;Database=RPG;Trusted_Connection=True");
    }
}