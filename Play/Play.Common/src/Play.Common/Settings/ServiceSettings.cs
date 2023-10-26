namespace Play.Common.Settings
{
    public class ServiceSettings
    {
        public string ServiceName { get; init; }

        public ServiceSettings(string serviceName)
        {
            this.ServiceName = serviceName;
        }
    }




}