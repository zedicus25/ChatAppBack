namespace ChatAppApi
{
    static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; set; }

        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
        }
    }
}
