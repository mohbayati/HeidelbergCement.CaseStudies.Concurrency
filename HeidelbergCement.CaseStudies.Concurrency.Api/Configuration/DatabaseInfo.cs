namespace HeidelbergCement.CaseStudies.Concurrency.Configuration;

public class DatabaseInfo
{
    public DatabaseInfo(string host, string port, string username, string password, string database, bool disableSsl)
    {
        Username = username;
        Password = password;
        Host = host;
        Database = database;
        Port = port;
        DisableSsl = disableSsl;
    }

    private string Username { get; }
    private string Password { get; }
    private string Host { get; }
    private string Port { get; }
    private string Database { get; }
    private bool DisableSsl { get; }

    public string ConnectionString => $"User ID={Username};Password={Password};Host={Host};Port={Port};Database={Database};Pooling=true;{(DisableSsl ? "": "Ssl Mode = Require;")}";
}
