using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using DbUchebPractikNET9.Data;

namespace DbUchebPractikNET9
{
    public partial class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(
                            context.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddSingleton<MainWindow>();
                })
                .Build();

            var app = new App();
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            app.Run(mainWindow);
        }
    }
}