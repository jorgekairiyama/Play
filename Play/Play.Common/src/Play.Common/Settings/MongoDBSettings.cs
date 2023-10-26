namespace Play.Common.Settings
{
    public class MongoDBSettings
    {
        public string Host{get; init;}

        public int Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";

        public MongoDBSettings(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }
    }
}