using Microsoft.EntityFrameworkCore;
using Sugma.Api.Data;
using Sugma.Api.Models;

namespace Sugma.Api.GraphQL;

public class Mutation
{
    public async Task<Todo> AddTodo(string title, DateTime? dueAt, [Service] AppDbContext db)
    {
        var todo = new Todo { Title = title, DueAt = dueAt };
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return todo;
    }

    public async Task<Todo?> ToggleTodo(int id, [Service] AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null) return null;
        todo.IsComplete = !todo.IsComplete;
        await db.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> DeleteTodo(int id, [Service] AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null) return false;
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return true;
    }
}
