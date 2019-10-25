using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSW.EliteDangerous.API;
using NSW.EliteDangerous.Monitor.Windows;
using NSW.Logging.Files;

namespace NSW.EliteDangerous.Monitor
{
    public partial class App : Application
    {
        public static ServiceProvider Services { get; }
        public static IConfiguration Configuration { get; }
        public static ILogger Log { get; }
        public static string Title { get; }

        static App()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("EliteMon.json", optional: false, reloadOnChange: true)
                .Build();

            Services = ConfigureServices(new ServiceCollection())
                .BuildServiceProvider();

            Log = Services
                .GetService<ILoggerFactory>()
                .CreateLogger<App>();

            var assembly = Assembly.GetExecutingAssembly();
            Title = $"{assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title} v.{assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddLogging(cfg => cfg
                    .AddFileLogger(o =>
                    {
                        o.Folder = "logs";
                        o.Template.AddColumn("Category", 50, e => e.Category);
                    }))
                .AddEliteDangerousAPI()
                .AddScoped<MainWindow>();
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetService<MainWindow>();
            mainWindow.Show();
            Log.LogInformation(Title);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.LogCritical(e.Exception, e.Exception.Message);
            MessageBox.Show("Unhandled exception: " + e.Exception.Message, "EliteMon Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
