namespace MobilitywaysCodeTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblyName = typeof(Startup).Assembly.FullName;

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });

            var host = hostBuilder.Build();

            host.Run();
        }
    }
}