﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Randomizer.Data.Configuration;
using Randomizer.Data.Options;
using Randomizer.Data.Services;
using Randomizer.Shared;
using Randomizer.SMZ3.ChatIntegration;
using Randomizer.SMZ3.Tracking.Services;
using static System.Int32;

namespace Randomizer.App.Windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window, INotifyPropertyChanged
    {
        private readonly IChatAuthenticationService _chatAuthenticationService;
        private readonly ILogger<OptionsWindow> _logger;
        private readonly ConfigProvider _trackerConfigProvider;
        private readonly IGitHubConfigDownloaderService _gitHubConfigDownloaderService;
        private readonly IGitHubSpriteDownloaderService _gitHubSpriteDownloaderService;
        private GeneralOptions _options = new();
        private bool _canLogIn = true;
        private ICollection<string> _availableProfiles;

        private Dictionary<string, string> _availableInputDevices = new() { { "Default", "Default" } };
        private SourceRomValidationService _sourceRomValidationService;

        public OptionsWindow(IChatAuthenticationService chatAuthenticationService,
            ConfigProvider configProvider,
            ILogger<OptionsWindow> logger,
            SourceRomValidationService sourceRomValidationService,
            IGitHubConfigDownloaderService gitHubConfigDownloaderService,
            IGitHubSpriteDownloaderService gitHubSpriteDownloaderService,
            IMicrophoneService microphoneService)
        {
            foreach (var device in microphoneService.GetDeviceDetails())
            {
                _availableInputDevices[device.Key] = device.Value;
            }

            InitializeComponent();
            _trackerConfigProvider = configProvider;
            _chatAuthenticationService = chatAuthenticationService;
            _logger = logger;
            _sourceRomValidationService = sourceRomValidationService;
            _gitHubConfigDownloaderService = gitHubConfigDownloaderService;
            _gitHubSpriteDownloaderService = gitHubSpriteDownloaderService;
            _availableProfiles = configProvider.GetAvailableProfiles();
            PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public GeneralOptions Options
        {
            get => _options;
            set
            {
                if (value != _options)
                {
                    _options = value;
                    DataContext = value;
                    PropertyChanged?.Invoke(this, new(nameof(Options)));
                    PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
                    PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
                }
            }
        }

        public bool IsLoggingIn
        {
            get => _canLogIn;
            set
            {
                if (_canLogIn != value)
                {
                    _canLogIn = value;
                    PropertyChanged?.Invoke(this, new(nameof(IsLoggingIn)));
                }
            }
        }

        public bool IsValidToken => !string.IsNullOrEmpty(Options.TwitchOAuthToken);

        public Dictionary<string, string> PushToTalkDevices
        {
            get => _availableInputDevices;
            set
            {
                _availableInputDevices = value;
                PropertyChanged?.Invoke(this, new(nameof(PushToTalkDevices)));
            }
        }

        public ICollection<string> AvailableProfiles
        {
            get => _availableProfiles;
            set
            {
                _availableProfiles = value;
                PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
            }
        }

        public ICollection<string> EnabledProfiles =>
            Options.SelectedProfiles.Where(x => !string.IsNullOrEmpty(x) && !"Default".Equals(x)).NonNull().ToList();

        public ICollection<string> DisabledProfiles =>
            AvailableProfiles?.Where(x => Options.SelectedProfiles.Contains(x) == false && !string.IsNullOrEmpty(x) && !"Default".Equals(x)).NonNull().ToList() ?? new List<string>();

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _ = DownloadUpdatesAndClose();
        }

        private async Task DownloadUpdatesAndClose()
        {
            if (Options.DownloadConfigsOnStartup && !Options.ConfigSources.Any())
            {
                await UpdateConfigsAsync();
            }

            if (Options.DownloadSpritesOnStartup && !Options.SpriteHashes.Any())
            {
                await UpdateSpritesAsync();
            }

            DialogResult = true;
        }

        private async void TwitchLoginButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoggingIn = false;
            Cursor = Cursors.AppStarting;
            try
            {
                await Dispatcher.Invoke(async () =>
                {
                    try
                    {
                        var token = await _chatAuthenticationService.GetTokenInteractivelyAsync(default);

                        if(token == null)
                        {
                            _logger.LogError("Token returned by chat authentication service null");
                            MessageBox.Show(this, "An unexpected error occurred while trying to log you in with Twitch. " +
                                "Please try again or report this issue with the log file.", "SMZ3 Cas’ Randomizer",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var userData = await _chatAuthenticationService.GetAuthenticatedUserDataAsync(token, default);

                        if (userData == null)
                        {
                            _logger.LogError("User Data returned by chat authentication service null");
                            MessageBox.Show(this, "An unexpected error occurred while trying to log you in with Twitch. " +
                                "Please try again or report this issue with the log file.", "SMZ3 Cas’ Randomizer",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Options.TwitchUserName = userData.Name;
                        Options.TwitchOAuthToken = token;
                        Options.TwitchChannel = string.IsNullOrEmpty(Options.TwitchChannel) ? userData.Name : Options.TwitchChannel;
                        Options.TwitchId = userData.Id;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An unknown error occurred while logging in with Twitch");
                        MessageBox.Show(this, "An unexpected error occurred while trying to log you in with Twitch. " +
                            "Please try again or report this issue with the log file.", "SMZ3 Cas’ Randomizer",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            finally
            {
                IsLoggingIn = true;
                Cursor = null;
            }

            await ValidateTwitchOAuthToken();
        }

        private void EnableProfile_Click(object sender, RoutedEventArgs e)
        {
            var newProfile = DisabledProfilesListBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(newProfile))
            {
                Options.SelectedProfiles.Add(newProfile);
                Options.SelectedProfiles.Remove(null);
                PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
                PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
            }
        }

        private void DisableProfile_Click(object sender, RoutedEventArgs e)
        {
            Options.SelectedProfiles.Remove(EnabledProfilesListBox.SelectedItem as string);
            PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
            PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
        }

        private void MoveProfileUp_Click(object sender, RoutedEventArgs e)
        {
            var index = EnabledProfilesListBox.SelectedIndex;
            if (index <= 0) return;
            var value = EnabledProfilesListBox.SelectedItem as string;
            var profiles = Options.SelectedProfiles.ToList();
            profiles.Remove(value);
            profiles.Insert(index - 1, value);
            Options.SelectedProfiles = profiles;
            PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
            PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
        }

        private void MoveProfileDown_Click(object sender, RoutedEventArgs e)
        {
            var index = EnabledProfilesListBox.SelectedIndex;
            if (index >= Options.SelectedProfiles.Count - 1) return;
            var value = EnabledProfilesListBox.SelectedItem as string;
            var profiles = Options.SelectedProfiles.ToList();
            profiles.Remove(value);
            profiles.Insert(index + 1, value);
            Options.SelectedProfiles = profiles;
            PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
            PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
        }

        private void OpenProfilesFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(_trackerConfigProvider.ConfigDirectory, "Sassy");
            Process.Start("explorer.exe", $"/select,\"{path}\"");
        }

        private void RefreshProfilesButton_Click(object sender, RoutedEventArgs e)
        {
            AvailableProfiles = _trackerConfigProvider.GetAvailableProfiles();
            PropertyChanged?.Invoke(this, new(nameof(EnabledProfiles)));
            PropertyChanged?.Invoke(this, new(nameof(DisabledProfiles)));
        }

        private async void Self_Loaded(object sender, RoutedEventArgs e)
        {
            await ValidateTwitchOAuthToken();
        }

        private async Task ValidateTwitchOAuthToken()
        {
            if (string.IsNullOrEmpty(Options.TwitchOAuthToken))
            {
                TwitchLoginFeedback.Text = "";
                TwitchLoginButton.Visibility = Visibility.Visible;
                TwitchLogoutButton.Visibility = Visibility.Collapsed;
                return;
            }

            var isValid = await _chatAuthenticationService.ValidateTokenAsync(Options.TwitchOAuthToken, default);
            if (!isValid)
            {
                Options.TwitchOAuthToken = "";
                TwitchLoginFeedback.Text = "Login expired.";
                TwitchLoginButton.Visibility = Visibility.Visible;
                TwitchLogoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                TwitchLoginFeedback.Text = "Logged in.";
                TwitchLoginButton.Visibility = Visibility.Collapsed;
                TwitchLogoutButton.Visibility = Visibility.Visible;
            }
        }

        private async void TwitchLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Options.TwitchOAuthToken))
                return;

            var revoked = await _chatAuthenticationService.RevokeTokenAsync(Options.TwitchOAuthToken, default);
            Options.TwitchOAuthToken = "";
            TwitchLoginFeedback.Text = revoked ? "Logged out." : "Something went wrong.";
            TwitchLoginButton.Visibility = Visibility.Visible;
            TwitchLogoutButton.Visibility = Visibility.Collapsed;
        }

        private void UndoExpirationTimeTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _ = TryParse(new string(UndoExpirationTimeTextBox.Text.Where(char.IsDigit).ToArray()), out var number);
            UndoExpirationTimeTextBox.Text = Math.Max(1, number).ToString();
        }

        private void FileSystemInput_OnOnPathUpdated(object? sender, EventArgs e)
        {
            var outputPath = Options.RomOutputPath;
            var msuPath = Options.MsuPath;

            if (string.IsNullOrEmpty(outputPath) || string.IsNullOrEmpty(msuPath))
            {
                return;
            }

            var outputDrive = new DriveInfo(outputPath);
            var msuDrive = new DriveInfo(msuPath);
            if (outputDrive.Name != msuDrive.Name)
            {
                MessageBox.Show(this,
                    "To preserve drive space, it is recommended that the Rom Output and MSU folders be on the same drive.",
                    "SMZ3 Cas' Randomizer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MetroidRomFileSystemInput_OnOnPathUpdated(object? sender, EventArgs e)
        {
            if (!_sourceRomValidationService.ValidateMetroidRom(MetroidRomFileSystemInput.Path))
            {
                MessageBox.Show(this,
                    "The rom selected does not appear to be a valid Super Metroid Japanese/US ROM. Generated SMZ3 ROMs may not work as expected.",
                    "SMZ3 Cas' Randomizer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ZeldaRomFileSystemInput_OnOnPathUpdated(object? sender, EventArgs e)
        {
            if (!_sourceRomValidationService.ValidateZeldaRom(ZeldaRomFileSystemInput.Path))
            {
                MessageBox.Show(this,
                    "The rom selected does not appear to be a valid ALttP Japanese v1.0 ROM. Generated SMZ3 ROMs may not work as expected.",
                    "SMZ3 Cas' Randomizer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateConfigsButton_OnClick(object sender, RoutedEventArgs e)
        {
            _ = UpdateConfigsAsync();
        }

        private void UpdateSpritesButton_OnClick(object sender, RoutedEventArgs e)
        {
            _ = UpdateSpritesAsync();
        }

        private async Task UpdateConfigsAsync()
        {
            var configSource = Options.ConfigSources.FirstOrDefault();
            if (configSource == null)
            {
                configSource = new ConfigSource() { Owner = "TheTrackerCouncil", Repo = "SMZ3CasConfigs" };
                Options.ConfigSources.Add(configSource);
            }
            await _gitHubConfigDownloaderService.DownloadFromSourceAsync(configSource);
        }

        private async Task UpdateSpritesAsync()
        {
            var sprites = await _gitHubSpriteDownloaderService.GetSpritesToDownloadAsync("TheTrackerCouncil", "SMZ3CasSprites");

            if (sprites?.Any() == true)
            {
                var spriteDownloaderWindow = new SpriteDownloaderWindow();
                spriteDownloaderWindow.Show();
                await _gitHubSpriteDownloaderService.DownloadSpritesAsync("TheTrackerCouncil", "SMZ3CasSprites");
                spriteDownloaderWindow.Close();
            }
        }
    }
}
