using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using Serilog.Sinks.File.Archive;
using System.IO.Compression;

namespace SchoolStudentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();

            var logsDirectory = Path.Combine(Environment.CurrentDirectory, "Logs", "log-.txt");
            var archivedLogsPath = Path.Combine(Environment.CurrentDirectory, "Archived Logs");
            if (!Directory.Exists(archivedLogsPath))
            {
                Directory.CreateDirectory(archivedLogsPath);
            }

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .MinimumLevel.Information()
               .Enrich.WithProperty("SourceContext", "SchoolStudentAPI.Program")
               .Enrich.WithMachineName()
               .Enrich.FromLogContext()
               .WriteTo.File(
                             path: logsDirectory,
                             outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                             rollingInterval: RollingInterval.Hour,
                             restrictedToMinimumLevel: LogEventLevel.Information,
                             retainedFileCountLimit: 5,
                             fileSizeLimitBytes: 35 * 1024, // 35 KB
                             rollOnFileSizeLimit: true,
                             flushToDiskInterval: TimeSpan.FromSeconds(1),
                             hooks: new ArchiveHooks(CompressionLevel.Optimal , archivedLogsPath)
                             ).CreateLogger();
            try
            {
                Log.Information("Application is starting");
                var filesToArchive = Directory.GetFiles(Path.GetDirectoryName(logsDirectory))
                .Where(file => !file.EndsWith(".zip"))
                .OrderByDescending(file => File.GetLastWriteTime(file)) 
                .Skip(5);
                        foreach (var file in filesToArchive)
                        {
                            var archivedFilePath = Path.Combine(archivedLogsPath, Path.GetFileName(file));
                            File.Move(file, archivedFilePath);
                        }
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
                Log.CloseAndFlush();             // flush all pending log events before sending email
                Log.Logger.ForContext("EmailSubject", "Service Error Log")
                          .Error(ex, "Application failed to start");
                throw;  
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
