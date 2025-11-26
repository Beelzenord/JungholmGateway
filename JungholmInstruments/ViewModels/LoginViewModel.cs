using System;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JungholmInstruments.Models;
using JungholmInstruments.Services;
using Supabase.Gotrue;
using static Supabase.Postgrest.Constants;

namespace JungholmInstruments.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly SupabaseService _supabaseService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private UserProfile? _currentUser;

        public LoginViewModel()
        {
            _supabaseService = SupabaseService.Instance;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _supabaseService.Client.Auth.SignInWithPassword(Email, Password);

                if (response?.User != null)
                {
                    // Fetch user profile from profiles table
                    try
                    {
                        var userId = Guid.Parse(response.User.Id ?? throw new InvalidOperationException("User ID is null"));
                        var profileData = await _supabaseService.Client
                            .From<Profile>()
                            .Select("*")
                            .Filter("id", Operator.Equals, userId)
                            .Get();

                        var profile = profileData?.Models?.FirstOrDefault();
                        if (profile != null)
                        {
                            CurrentUser = new UserProfile
                            {
                                Id = profile.Id,
                                Email = profile.Email ?? response.User.Email,
                                FirstName = profile.FirstName,
                                LastName = profile.LastName,
                                Role = profile.Role
                            };
                        }
                        else
                        {
                            // Fallback to auth user data
                            CurrentUser = new UserProfile
                            {
                                Id = userId,
                                Email = response.User.Email
                            };
                        }
                    }
                    catch
                    {
                        // Fallback to auth user data if profile fetch fails
                        CurrentUser = new UserProfile
                        {
                            Id = Guid.Parse(response.User.Id ?? throw new InvalidOperationException("User ID is null")),
                            Email = response.User.Email
                        };
                    }

                    // Navigate to main window or dashboard
                    OnLoginSuccessful?.Invoke(CurrentUser);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event Action<UserProfile>? OnLoginSuccessful;
    }
}

