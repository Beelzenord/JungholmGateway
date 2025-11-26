using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using JungholmInstruments.Models;
using JungholmInstruments.Services;
using JungholmInstruments.ViewModels;
using JungholmInstruments.Views;

namespace JungholmInstruments
{
    public partial class App : Application
    {
        private const string SupabaseUrl = "https://ufajajaofucrahuctmof.supabase.co";
        private const string SupabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVmYWphamFvZnVjcmFodWN0bW9mIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDg2MDYwOTMsImV4cCI6MjA2NDE4MjA5M30.SSCF_XwGXIXw7OrRb2WGA1ROMfV3VOjdWS8kOz2MSjw";

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                // Initialize Supabase
                SupabaseService.Instance.Initialize(SupabaseUrl, SupabaseAnonKey);

                // Show login window first
                var loginViewModel = new LoginViewModel();
                var loginWindow = new LoginView(loginViewModel);
                
                loginViewModel.OnLoginSuccessful += (user) =>
                {
                    // Create and show main window first
                    var mainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel { CurrentUser = user },
                    };
                    
                    // Set as new main window and show it
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                    
                    // Then close the login window (this won't close the app since MainWindow is now set)
                    loginWindow.Close();
                };

                desktop.MainWindow = loginWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}