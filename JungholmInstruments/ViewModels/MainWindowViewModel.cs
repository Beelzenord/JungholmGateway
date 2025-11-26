using CommunityToolkit.Mvvm.ComponentModel;
using JungholmInstruments.Models;

namespace JungholmInstruments.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private UserProfile? _currentUser;

        public string Greeting => CurrentUser != null 
            ? $"Welcome, {CurrentUser.FirstName ?? CurrentUser.Email}!" 
            : "Welcome to Jungholm Instruments!";
    }
}
