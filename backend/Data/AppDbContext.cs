using Microsoft.EntityFrameworkCore;
using Sugma.Api.Models;

namespace Sugma.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
