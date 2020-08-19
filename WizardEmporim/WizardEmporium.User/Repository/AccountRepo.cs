using System.Threading.Tasks;

public class AccountRepo : BaseRepo {
    public AccountRepo(string connectionString) : base(connectionString) {}

    public async Task InsertAccountAsync(string username, string passwordHash, int roleId) => 
        await GetConnectionAsync(connection => connection.ExecuteAsync(@"
            INSERT INTO Account(Username, PasswordHash, RoleId)
            VALUES(@username, @passwordHash, @roleId)", new { username, passwordHash, roleId }));
}