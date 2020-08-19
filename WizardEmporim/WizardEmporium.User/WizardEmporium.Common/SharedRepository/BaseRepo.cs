using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System;

public abstract class BaseRepo {

    private readonly string connectionString;

    public BaseRepo(string connectionString) {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new NotSupportedException("Please specify connection string.");
        this.connectionString = connectionString;
    }

    private async Task<SqliteConnection> getConnectionAsync() {
        var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        return connection;
    }

    protected async Task<T> getConnectionAsync<T>(Func<SqliteConnection, Task<T>> action) {
        using var conn = await getConnectionAsync();
        return await action(conn);
    }

    protected async Task GetConnectionTransactionAsync(Func<SqliteConnection, SqliteTransaction, Task> action) {
        using var connection = await getConnectionAsync();
        using var transaction = connection.BeginTransaction();

        try {
            await action(connection, transaction);

            transaction.Commit();
        } catch (Exception) {
            transaction.Rollback();
            throw;
        }

    }

}