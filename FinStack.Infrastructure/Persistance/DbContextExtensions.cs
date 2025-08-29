using FinStack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static async Task TruncateAllTablesAsync(this AppDbContext context)
    {
        var connection = context.Database.GetDbConnection();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "TRUNCATE TABLE \"AppUsers\", \"AuthUsers\" RESTART IDENTITY CASCADE;";
            await command.ExecuteNonQueryAsync();
        }
    }
}
