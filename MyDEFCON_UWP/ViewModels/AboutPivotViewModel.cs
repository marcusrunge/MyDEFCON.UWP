﻿using MyDEFCON_UWP.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.System;

namespace MyDEFCON_UWP.ViewModels
{
    public class AboutPivotViewModel : Observable
    {
        private string _versionDescription;
        public string VersionDescription { get { return _versionDescription; } set { Set(ref _versionDescription, value); } }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private ICommand _emailCommand;
        public ICommand EmailCommand => _emailCommand ?? (_emailCommand = new RelayCommand<object>(async (param) => await Launcher.LaunchUriAsync(new Uri("mailto:mr.software@outlook.de?subject=MyDEFCON App"))));

        private ICommand _rateCommand;
        public ICommand RateCommand => _rateCommand ?? (_rateCommand = new RelayCommand<object>(async (param) => await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=d6facdb7-724c-47dc-91d5-489d5acb2eb2"))));

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}