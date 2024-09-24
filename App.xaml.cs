using System.Diagnostics;
using AsignmentWinUI.Activation;
using AsignmentWinUI.Contracts.Services;
using AsignmentWinUI.Core;
using AsignmentWinUI.Core.Contracts.Services;
using AsignmentWinUI.Core.Infrastructure.SpLite.DataContext;
using AsignmentWinUI.Core.Infrastructure.SpLite.Repositories;
using AsignmentWinUI.Core.Services;
using AsignmentWinUI.Core.UseCases._Repositories;
using AsignmentWinUI.Core.UseCases.GetMessageUseCase;
using AsignmentWinUI.Core.UseCases.GetOnlineGroupMemberUseCase;
using AsignmentWinUI.Core.UseCases.Repositories;
using AsignmentWinUI.Core.UseCases.SendMessageUseCase;
using AsignmentWinUI.Core.UseCases.Services;
using AsignmentWinUI.Helpers;
using AsignmentWinUI.Services;
using AsignmentWinUI.ViewModels;
using AsignmentWinUI.Views;
using CleanArchitectureSignalR.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace AsignmentWinUI;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();
            services.AddSingleton<ChatHub>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));
            services.AddTransient<ISendMessageUseCase, SendMessageUseCase>();
            services.AddTransient<IGetMessageUseCase, GetMessageUseCase>();
            services.AddTransient<IGetOnlineGroupMemberUseCase, GetOnlineGroupMemberUseCase>();

            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IGroupMembersRepository, GroupMemberRepository>();

            services.AddTransient<IMessageService>(services => new MessageService(
                services.GetRequiredService<ISendMessageUseCase>(),
                services.GetRequiredService<IGetMessageUseCase>()));
            // Views and ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            // Configuration
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        //try
        //{
        //    await Host.StartAsync();
        //    Debug.WriteLine("Host started successfully.");
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine($"Error starting host: {ex.Message}");
        //}
        var dbContext = GetRequiredService<AppDbContext>();
        bool databaseCreated = await dbContext.Database.EnsureCreatedAsync();
            if (databaseCreated)
            {
                Debug.WriteLine("Database was created.");
            }
            else
            {
                Debug.WriteLine("Database already exists.");
            }
        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    public static T GetRequiredService<T>() where T : class
    {
        return (App.Current as App)!.Host.Services.GetRequiredService<T>();
    }
}
