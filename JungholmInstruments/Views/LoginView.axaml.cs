using Avalonia.Controls;
using JungholmInstruments.ViewModels;

namespace JungholmInstruments.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public LoginView(LoginViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}

