namespace WebApp;

public static class ConfigurationExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            var logsPath = builder.Configuration["LogsPath"];
            if (string.IsNullOrEmpty(logsPath)) return;

            Console.WriteLine($"Logging to console at {logsPath}");
            Dictionary<string, LogLevel> overrides = new()
            {
                { "Microsoft", LogLevel.Warning },
            };

            builder.Logging.AddFile(logsPath + "/log-{Date}.txt" ?? "Logs/log-{Date}.txt", LogLevel.Information, overrides);
        }
    }
}
