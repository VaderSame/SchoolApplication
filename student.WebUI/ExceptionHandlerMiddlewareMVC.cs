using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Sinks.Email;
using System.Net;

public class ExceptionHandlerMiddlewareMVC
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddlewareMVC(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred.");

            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build();

            Log.CloseAndFlush();
            Log.Logger = new LoggerConfiguration()
                    .Enrich.WithProperty("SourceContext", "SchoolStudentMVC.Program")
                    .Enrich.WithMachineName()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.Email(new EmailConnectionInfo
                    {
                        FromEmail = "semal.shastri@rockstechlabs.com",
                        ToEmail = "vaibhaw.sinha@rockstechlabs.com",
                        MailServer = "smtp-mail.outlook.com",
                        NetworkCredentials = new NetworkCredential
                        {
                            UserName = "semal.shastri@rockstechlabs.com",
                            Password = "VaderSam@2421"
                        },
                        EnableSsl = false,
                        Port = 587,
                        EmailSubject = "Service Error Log"
                    },
                   outputTemplate: "Timestamp: {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}\r\nLevel: {Level:u}\r\nMessage: {Message}\r\nException: {Exception}\r\nSourceContext: {SourceContext}\r\nMachineName: {MachineName}  ",
                   restrictedToMinimumLevel: LogEventLevel.Information, batchPostingLimit: 100)
                   .CreateLogger();

            Log.Error(ex, "Application failed to start");

            throw;
        }
    }
}
