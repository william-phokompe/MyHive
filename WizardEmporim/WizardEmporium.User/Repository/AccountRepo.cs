using System.Threading.Tasks;

public class AccountRepo : BaseRepo {
    public AccountRepo(string connectionString) : base(connectionString) {}

    public async Task InsertAccountAsync(string username, string passwordHash, int roleId) => 
        await GetConnectionAsync(connection => connection.ExecuteAsync(@"
            INSERT INTO Account(Username, PasswordHash, RoleId)
            VALUES(@username, @passwordHash, @roleId)", new { username, passwordHash, roleId }));

    public async Task<AccountDto> FindAccountAsync(string username, string passwordHash) => 
        await GetConnectionAsync(connection => connection.QueryFirstOrDefault<AccountDto>(@"
            SELECT AccountId, RoleId, Username, Suspended
            FROM Account
            WHERE Username = @username
            AND PasswordHash = @passwordHash", new { username, passwordHash }));
}