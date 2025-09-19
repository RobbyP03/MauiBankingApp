using CommunityToolkit.Maui;
using MauiBankingExercise.Configuration;
using MauiBankingExercise.Interface;
using MauiBankingExercise.Services;
using MauiBankingExercise.ViewModels;
using MauiBankingExercise.Views;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace MauiBankingExercise
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ApplicationSettings>(); 

            
            builder.Services.AddTransient<IBankingService, BankingDataApiService>();

           
            builder.Services.AddTransient<ListOfCustomersViewModel>();
            builder.Services.AddTransient<CustomerDashboardViewModel>();
            builder.Services.AddTransient<TransactionViewModel>();


            builder.Services.AddTransient<ListOfCustomersView>();
            builder.Services.AddTransient<CustomerDashboardView>();
            builder.Services.AddTransient<TransactionView>();

            return builder.Build();
        }
    }
}
