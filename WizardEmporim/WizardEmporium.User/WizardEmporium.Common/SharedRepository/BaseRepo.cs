using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

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

}