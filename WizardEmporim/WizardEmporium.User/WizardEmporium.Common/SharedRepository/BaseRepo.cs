public abstract class BaseRepo {

    private readonly string connectionString;

    public BaseRepo(string connectionString) {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new NotSupportedException("Please specify connection string.");
        this.connectionString = connectionString;
    }

}