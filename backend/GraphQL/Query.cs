using Microsoft.EntityFrameworkCore;
using Sugma.Api.Data;
using Sugma.Api.Models;

namespace Sugma.Api.GraphQL;

public class Query
{
    public async Task<List<Todo>> GetTodos([Service] AppDbContext db) =>
        await db.Todos.OrderBy(t => t.CreatedAt).ToListAsync();

    public async Task<Todo?> GetTodo(int id, [Service] AppDbContext db) =>
        await db.Todos.FindAsync(id);
}
