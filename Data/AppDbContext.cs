using Microsoft.EntityFrameworkCore;
using PalindromeLab.Models;

namespace PalindromeLab.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CheckedWord> CheckedWords => Set<CheckedWord>();
}
